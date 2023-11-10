using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Snake : MonoBehaviour
{

    private enum State
    {
        Alive,
        Dead,
        Pause
    }

    private const float MAX_SPEED = 0.4f;
    private const float MAX_DELAY = 10f;
    private State state;
    private PowerType powerUp;
    private Vector2Int gridPosition;
    private Direction gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimeMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeBodyPart> snakeBodyList;
    private List<SnakeMovePosition> snakeMovePositionList;
    public ScoreController scoreController;

    private PlayerType playerType;
    public GameObject snakeHead;

    private void Awake()
    {
        snakeBodySize = 3;
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodyList = new List<SnakeBodyPart>();
        InitPosition();
        gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.snakeHeadSprite;
        state = State.Alive;
        powerUp = PowerType.None;
        playerType = PlayerType.One;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Alive:
                HandleCoreAction();
                break;
            case State.Dead:
                break;
            case State.Pause:
                break;
        }

    }
    public void LevelGridSetup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }
    private void HandleCoreAction()
    {
        HandleInput();
        HandleGridMovement();
    }
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer > gridMoveTimeMax)
        {
            Vector2Int gridMoveDirectionVector;

            //Insert snake position
            snakeMovePositionList.Insert(0, new SnakeMovePosition(gridPosition, gridMoveDirection));

            //Position increment
            gridMoveDirectionVector = GetMoveDirectionVector();
            gridPosition += gridMoveDirectionVector;
            gridPosition = levelGrid.ValidateGridPosition(gridPosition);

            gridMoveTimer -= gridMoveTimeMax;

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }


            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

            UpdateSnakeBodyParts();

            if (powerUp != PowerType.Shield)
            {
                foreach (SnakeBodyPart snakeBodyPart in snakeBodyList)
                {
                    Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                    if (gridPosition == snakeBodyPartGridPosition)
                    {
                        //Debug.Log("Game Over");
                        SoundManager.Instance.Play(SoundType.SnakeDeath);
                        state = State.Dead;
                        UIController.gameOverController.SetActive(true);
                    }
                }
            }
        }
    }

    private void CatchPower(PowerType type)
    {
        powerUp = type;
        switch (powerUp)
        {
            case PowerType.Shield:
                SetPowerShield();
                break;
            case PowerType.ScoreBoost:
                SetPowerScoreBoost();
                break;

            case PowerType.SpeedUp:
                SetPowerSpeedUp();
                break;
        }

    }

    private void IncreaseSnakeSize()
    {
        snakeBodySize++;
        CreateSnakeBodyPart();
        scoreController.AddScore(powerUp == PowerType.ScoreBoost ? 50 : 10);
    }

    private Vector2Int GetMoveDirectionVector()
    {
        Vector2Int gridMoveDirectionVector;
        switch (gridMoveDirection)
        {
            default:
            case Direction.Right: gridMoveDirectionVector = new Vector2Int(1, 0); break;
            case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
            case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, 1); break;
            case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
        }

        return gridMoveDirectionVector;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(playerType == PlayerType.One ? KeyCode.UpArrow : KeyCode.W))
        {
            if (Direction.Down != gridMoveDirection)
            {
                gridMoveDirection = Direction.Up;
            }
          
        }
        if (Input.GetKeyDown(playerType == PlayerType.One ? KeyCode.DownArrow : KeyCode.S))
        {
            if (Direction.Up != gridMoveDirection)
            {
                gridMoveDirection = Direction.Down;
            }
           
        }
        if (Input.GetKeyDown(playerType == PlayerType.One ? KeyCode.LeftArrow : KeyCode.A))
        {
            if (Direction.Right != gridMoveDirection)
            {
                gridMoveDirection = Direction.Left;
            }
           
        }
        if (Input.GetKeyDown(playerType == PlayerType.One ? KeyCode.RightArrow : KeyCode.D))
        {
            if (Direction.Left != gridMoveDirection)
            {
                gridMoveDirection = Direction.Right;
            }
          
        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    private void InitPosition()
    {
        gridPosition = new Vector2Int(Random.Range(GlobalConstant.MIN_WIDTH, GlobalConstant.MAX_WIDTH), Random.Range(GlobalConstant.MIN_HEIGHT, GlobalConstant.MAX_HEIGHT));
        gridMoveTimeMax = MAX_SPEED;
        gridMoveTimer = gridMoveTimeMax;
        gridMoveDirection = Direction.Right;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }
    private void CreateSnakeBodyPart()
    {
        var body = new SnakeBodyPart(snakeBodyList.Count, this);
        body.SetGlowBodyPart(powerUp);
        snakeBodyList.Add(body);

    }
    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyList.Count; i++)
        {
            snakeBodyList[i].SetGridPosition(snakeMovePositionList[i]);
        }
    }


    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach (var position in snakeMovePositionList)
        {
            gridPositionList.Add(position.GridPosition);
        }
      
        return gridPositionList;
    }

    public void SetPause()
    {
        state = State.Pause;
    }
    public void SetResume()
    {
        state = State.Alive;
    }

    public void SetPowerShield(float delay = MAX_DELAY)
    {
        powerUp = PowerType.Shield;

        foreach (var part in snakeBodyList)
        {
            part.SetGlowBodyPart(powerUp);
        }
        StartCoroutine(ResetPowerAfterDelay(delay));
    }
    public void SetPowerScoreBoost(float delay = MAX_DELAY)
    {
        powerUp = PowerType.ScoreBoost;
        foreach (var part in snakeBodyList)
        {
            part.SetGlowBodyPart(powerUp);
        }
        StartCoroutine(ResetPowerAfterDelay(delay));
    }
    public void SetPowerSpeedUp(float delay = MAX_DELAY)
    {
        powerUp = PowerType.SpeedUp;
        gridMoveTimeMax = 0.2f;
        foreach (var part in snakeBodyList)
        {
            part.SetGlowBodyPart(powerUp);
        }
        StartCoroutine(ResetPowerAfterDelay(delay));
    }
    public void ResetPower()
    {
        powerUp = PowerType.None;
        gridMoveTimeMax = MAX_SPEED;
        foreach (var part in snakeBodyList)
        {
            part.SetGlowBodyPart(powerUp);
        }

    }

    private IEnumerator ResetPowerAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        ResetPower();
    }


    public void SetSnakeType(PlayerType type)
    {
        playerType = type;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.CompareTag(GlobalConstant.FoodTag))
        {
            SoundManager.Instance.Play(SoundType.FoodCollect);
            IncreaseSnakeSize();
        }
        if (other.CompareTag(GlobalConstant.PowerTag))
        {
            SoundManager.Instance.Play(SoundType.PowerCollect);
            CatchPower(other.GetComponent<Power>().PowerType);
        }
        if (other.CompareTag(GlobalConstant.SnakeTag))
        {
            Debug.Log("Inside snake : " + other.tag);
            var snakeBody = other.gameObject.GetComponent<SnakeBody>();
            if (snakeBody != null && snakeBody.snake != this)
            {
                SoundManager.Instance.Play(SoundType.SnakeDeath);
                snakeBody.snake.SetSnakeDead();
                StartCoroutine(snakeBody.snake.DestroySnakeBody());
            }
        }
    }

    public void SetSnakeDead()
    {
        state = State.Dead;
    }
    public IEnumerator DestroySnakeBody()
    {

        foreach (var body in snakeBodyList)
        {
            body.DestroySnakeBody();
            yield return null;
        }
        Destroy(snakeHead);

    }
    private class SnakeMovePosition
    {
        private Vector2Int gridPosition;
        private Direction direction;
        public SnakeMovePosition(Vector2Int gridPosition, Direction direction)
        {
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GridPosition { get { return gridPosition; } set { gridPosition = value; } }
        public Direction Direction { get { return direction; } set { direction = value; } }
    }

    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        private SpriteRenderer spriteRenderer;
        private GameObject snakeBody;

        public SnakeBodyPart(int bodyIndex, Snake parentSnake)
        {
            snakeBody = new GameObject("SnakeBody", typeof(SpriteRenderer));
            spriteRenderer = snakeBody.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = GameAssets.Instance.bodySprite;
          
            snakeBody.layer = 1;
            transform = snakeBody.transform;
            var collider2D = snakeBody.AddComponent<CircleCollider2D>();
            collider2D.radius = 0.5f;
            collider2D.isTrigger = true;
            snakeBody.tag = GlobalConstant.SnakeTag;

            var snakebody = snakeBody.AddComponent<SnakeBody>();
            snakebody.snakeHead = parentSnake.snakeHead;
            snakebody.snake = parentSnake;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GridPosition.x, snakeMovePosition.GridPosition.y);
        }

        public void SetGridPosition(SnakeMovePosition gridPosition)
        {
            this.snakeMovePosition = gridPosition;
            transform.position = new Vector3(gridPosition.GridPosition.x, gridPosition.GridPosition.y);
        }

        public Vector2Int GetGridPosition()
        {
            if (snakeMovePosition != null)
                return snakeMovePosition.GridPosition;
            return new Vector2Int();
        }

        public void SetGlowBodyPart(PowerType powerType)
        {
            if (spriteRenderer != null)
                switch (powerType)
                {
                    case PowerType.ScoreBoost:
                        spriteRenderer.color = new Color(0, 0, 255);
                        break;
                    case PowerType.Shield:
                        spriteRenderer.color = new Color(0, 255, 0);
                        break;
                    case PowerType.SpeedUp:
                        spriteRenderer.color = new Color(255, 0, 0);
                        break;
                    case PowerType.None:
                        spriteRenderer.color = new Color(255, 255, 255);
                        break;
                }
        }

        public void DestroySnakeBody()
        {
            Destroy(snakeBody);
        }
    }
}



