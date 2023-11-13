using System.Collections.Generic;
using UnityEngine;
using Snake2D.UI;

namespace Snake2D
{
    public class SnakeHandler : MonoBehaviour
    {
        [SerializeField] private LevelGrid levelGrid;
        [SerializeField] private GameObject snakeHead;
        [SerializeField] private Snake snake;
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private PlayerType playerType;
        [SerializeField] private Color playerColor;
        [SerializeField] private GameObject snakePrefab;
        [SerializeField] private GameObject snakeBodyPrefab;
        private void Start()
        {
            InitLevelGrid();
            InitSnake();
           
        }

        private void InitLevelGrid()
        {
            levelGrid = new LevelGrid(GlobalConstant.MAX_WIDTH, GlobalConstant.MAX_HEIGHT);

        }
        private void InitSnake()
        {
            snakeHead = Instantiate(snakePrefab, new Vector3(0, 0, 0), Quaternion.identity);

            SpriteRenderer snakeSpriteRenderer = snakeHead.GetComponent<SpriteRenderer>();
            snakeSpriteRenderer.color = playerColor;

            snake = snakeHead.GetComponent<Snake>();
            snake.LevelGridSetup(levelGrid);
            snake.scoreController = scoreController;
            snake.SetSnakeType(playerType);
            snake.snakeHead = snakeHead;
            snake.snakeBodyPrefab = snakeBodyPrefab;

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
}