using System;
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
    public GameObject gameOverController;
    private PlayerType playerType;
  

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

            //Power took
            //CatchPower();

            //Food taken & increase size
            //if (levelGrid.IsSnakeTookFood(gridPosition))
            //{
            //    IncreaseSnakeSize();
            //}

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
                //Debug.Log(powerUp);
                foreach (SnakeBodyPart snakeBodyPart in snakeBodyList)
                {
                    Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                    if (gridPosition == snakeBodyPartGridPosition)
                    {
                        //Debug.Log("Game Over");
                        state = State.Dead;
                        gameOverController.SetActive(true);
                    }
                }
            }
        }




    }

    private void CatchPower(PowerType type )
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
            //gridPosition.y += 1;
        }
        if (Input.GetKeyDown(playerType == PlayerType.One ? KeyCode.DownArrow : KeyCode.S ))
        {
            if (Direction.Up != gridMoveDirection)
            {
                gridMoveDirection = Direction.Down;
            }
            //gridPosition.y -= 1;
        }
        if (Input.GetKeyDown(playerType == PlayerType.One ? KeyCode.LeftArrow : KeyCode.A ))
        {
            if (Direction.Right != gridMoveDirection)
            {
                gridMoveDirection = Direction.Left;
            }
            //gridPosition.x -= 1;
        }
        if (Input.GetKeyDown(playerType == PlayerType.One ? KeyCode.RightArrow : KeyCode.D ))
        {
            if (Direction.Left != gridMoveDirection)
            {
                gridMoveDirection = Direction.Right;
            }
            //gridPosition.x += 1;
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
        gridPosition = new Vector2Int(16, 9);
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
        var body = new SnakeBodyPart(snakeBodyList.Count);
        body.SetGlowBodyPart(powerUp);
        snakeBodyList.Add(body);

    }
    private void UpdateSnakeBodyParts()
    {
        for(int i =0; i< snakeBodyList.Count; i++)
        {
            snakeBodyList[i].SetGridPosition(snakeMovePositionList[i]);
        }
    }
   

    public List<Vector2Int> GetFullSnakeGridPositionList(){
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach(var position in snakeMovePositionList)
        {
            gridPositionList.Add(position.GridPosition);
        }
        //gridPositionList.AddRange(snakeBodyList);
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
        //Debug.Log("Inside snake : "+ other.tag);
        if (other.CompareTag(GlobalConstant.FoodTag))
        {
            IncreaseSnakeSize();
        }
        if (other.CompareTag(GlobalConstant.PowerTag))
        {
            CatchPower(other.GetComponent<Power>().PowerType);
        }
            
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
        public SnakeBodyPart(int bodyIndex)
        {
            var snakeBody = new GameObject("SnakeBody", typeof(SpriteRenderer));
            spriteRenderer = snakeBody.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = GameAssets.Instance.bodySprite;
            //snakeBody.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            snakeBody.layer = 1;
            transform = snakeBody.transform;
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
            if(snakeMovePosition != null)
                return snakeMovePosition.GridPosition;
            return new Vector2Int();
        }

        public void SetGlowBodyPart(PowerType powerType) 
        {
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

        
    }
}



