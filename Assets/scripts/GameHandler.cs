using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }
    [SerializeField] private LevelGrid levelGrid;
    [SerializeField] private GameObject snakeHead;
    [SerializeField] private Snake snake;
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private GameObject gameOverController;
    [SerializeField] private GameObject resumeController;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private PlayerType playerType;
    [SerializeField] private Color playerColor;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }
    private void Start()
    {
        InitLevelGrid();
        InitSnake();
        levelGrid.SnakeSetup(snake);
        levelGrid.SpawnFood();
        levelGrid.SpawnPower();
        resumeButton.onClick.AddListener(OnClickResume);
        pauseButton.onClick.AddListener(OnClickPause);
    }
    private void OnClickResume()
    {
        resumeController.SetActive(false);
        snake.SetResume();
    }
    private void OnClickPause()
    {
        resumeController.SetActive(true);
        snake.SetPause();
    }
    private void InitLevelGrid() 
    {
        levelGrid = new LevelGrid(26, 16);
       
    }
    private void InitSnake() {
        snakeHead = new GameObject("SnakeHead" + playerType);

        SpriteRenderer snakeSpriteRenderer = snakeHead.AddComponent<SpriteRenderer>();
        snakeSpriteRenderer.sprite = GameAssets.Instance.snakeHeadSprite;
        snakeSpriteRenderer.color = playerColor;
         snake =  snakeHead.AddComponent<Snake>() ;
        snake.LevelGridSetup(levelGrid);
        snake.scoreController = scoreController;
        snake.gameOverController = gameOverController;
        snake.SetSnakeType(playerType);
    }
}
