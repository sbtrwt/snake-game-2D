using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeHandler : MonoBehaviour
{
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
    private void Start()
    {
        InitLevelGrid();
        InitSnake();
        levelGrid.SnakeSetup(snake);
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
        levelGrid = new LevelGrid(GlobalConstant.MAX_WIDTH, GlobalConstant.MAX_HEIGHT);
       
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
        snake.snakeHead = snakeHead;
        var collider2D = snakeHead.AddComponent<CircleCollider2D>();
        collider2D.radius = 0.5f;
        snakeHead.tag = GlobalConstant.SnakeTag;
        var rigidbody2D =  snakeHead.AddComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        rigidbody2D.simulated = true;
    }

    public List<Vector2Int> GetFullSnakeGridPositionList() 
    {
        if (snake == null) return null;
        return snake.GetFullSnakeGridPositionList();
    }

    public void DestroySnake()
    {
        Destroy(snakeHead);
    }
}