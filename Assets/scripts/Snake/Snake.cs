using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Snake : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    private enum State
    {
        Alive,
        Dead
    }

    private State state;
    private Vector2Int gridPosition;
    private Direction gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimeMax;
    [SerializeField] private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeBodyPart> snakeBodyList;
    private List<SnakeMovePosition> snakeMovePositionList;
    public ScoreController scoreController;
    public GameObject gameOverController;
    public Snake() {
        snakeBodySize = 3;
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodyList = new List<SnakeBodyPart>();
    }
    public void LevelGridSetup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }
    private void Awake()
    {
        
        InitPosition();
        gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.snakeHeadSprite;
        state = State.Alive;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case State.Dead:
                break;
        }
        
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

            //Food taken & increase size
            if (levelGrid.IsSnakeTookFood(gridPosition))
            {
                snakeBodySize++;
                CreateSnakeBodyPart();
                scoreController.AddScore(10);
            }
          
            gridMoveTimer -= gridMoveTimeMax;

            if (snakeMovePositionList.Count >= snakeBodySize + 1 )
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }
           

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

            UpdateSnakeBodyParts();

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    Debug.Log("Game Over");
                    state = State.Dead;
                    gameOverController.SetActive(true);
                }
            }
        }

        


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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Direction.Down != gridMoveDirection)
            {
                gridMoveDirection = Direction.Up;
            }
            //gridPosition.y += 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Direction.Up != gridMoveDirection)
            {
                gridMoveDirection = Direction.Down;
            }
            //gridPosition.y -= 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Direction.Right != gridMoveDirection)
            {
                gridMoveDirection = Direction.Left;
            }
            //gridPosition.x -= 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
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
        gridMoveTimeMax = 0.4f;
        gridMoveTimer = gridMoveTimeMax;
        gridMoveDirection = Direction.Right;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }
    private void CreateSnakeBodyPart()
    {
        snakeBodyList.Add(new SnakeBodyPart(snakeBodyList.Count));
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

        public SnakeBodyPart(int bodyIndex)
        {
            var snakeBody = new GameObject("SnakeBody", typeof(SpriteRenderer));

            snakeBody.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.bodySprite;
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
    }
}



