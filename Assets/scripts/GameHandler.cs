using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }
    [SerializeField]private LevelGrid levelGrid;
    [SerializeField] private GameObject snakeHead;
    [SerializeField] private Snake snake;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        snake = new Snake();
        InitLevelGrid();
        InitSnake();
        levelGrid.SpawnFood();
        
    }
    private void InitLevelGrid() 
    {
        levelGrid = new LevelGrid(26, 16);
        levelGrid.SnakeSetup(snake);
    }
    private void InitSnake() {
        snakeHead = new GameObject();

        SpriteRenderer snakeSpriteRenderer = snakeHead.AddComponent<SpriteRenderer>();
        snakeSpriteRenderer.sprite = GameAssets.Instance.snakeHeadSprite;
        snake =  snakeHead.AddComponent<Snake>() ;
        snake.LevelGridSetup(levelGrid);
    }
}
