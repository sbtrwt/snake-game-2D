using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridPosition;
    private Vector2Int gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimeMax;
    [SerializeField] private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeBody> snakeBodyList;

    public Snake() {
        snakeBodySize = 3;
        snakeBodyList = new List<SnakeBody>();
    }
    public void LevelGridSetup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }
    private void Awake()
    {
        
        InitPosition();
        gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.snakeHeadSprite;
    }

    private void Update()
    {
        HandleInput();

        HandleGridMovement();
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer > gridMoveTimeMax)
        {
            gridPosition += gridMoveDirection;

            if (levelGrid.IsSnakeTookFood(gridPosition))
            {
                snakeBodySize++;
            }
            snakeBodyList.Insert(0,new SnakeBody( gridPosition , gridMoveDirection));
            gridMoveTimer -= gridMoveTimeMax;

            if (snakeBodyList.Count >= snakeBodySize + 1)
            {
                snakeBodyList.RemoveAt(snakeBodyList.Count - 1);
            }
            for (int i = 1; i < snakeBodyList.Count; i++)
            {
                Vector2Int snakeMovePosition = snakeBodyList[i].Position;
                GameObject body = CreateBody(snakeMovePosition);
                body.transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(snakeBodyList[i].Direction) - 90);
                StartCoroutine(DestroyBody(body));
            }
        }

        transform.position = new Vector3(gridPosition.x, gridPosition.y);
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);


    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection.y != -1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = 1;
            }
            //gridPosition.y += 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection.y != 1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;

            }
            //gridPosition.y -= 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection.x != 1)
            {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;

            }
            //gridPosition.x -= 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection.x != 1)
            {
                gridMoveDirection.x = 1;
                gridMoveDirection.y = 0;

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
        gridMoveDirection = new Vector2Int(1, 0);
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    private GameObject CreateBody(Vector2Int position)
    {
        var snakeBody = new GameObject();

        SpriteRenderer snakeSpriteRenderer = snakeBody.AddComponent<SpriteRenderer>();
        snakeSpriteRenderer.sprite = GameAssets.Instance.bodySprite;
        snakeBody.transform.position = new Vector3(position.x, position.y);
        //snakeBody.transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(position) - 90);
        snakeBody.layer = 1;
        return snakeBody;
    }

    private IEnumerator DestroyBody(GameObject body)
    {
        yield return new WaitForSeconds(gridMoveTimeMax);
        Destroy(body);
    }

    public List<Vector2Int> GetFullSnakeGridPositionList(){
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        //gridPositionList.AddRange(snakeBodyList);
        return gridPositionList;
    }
}

public class SnakeBody
{
    public Vector2Int Position { get; set; }
    public Vector2Int Direction { get; set; }
    public SnakeBody() { }
    public SnakeBody(Vector2Int position, Vector2Int direction) 
    {
        this.Position = position;
        this.Direction = direction;
    }

}