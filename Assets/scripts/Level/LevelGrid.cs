using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;
    private int width;
    private int height;
    [SerializeField] private Snake snake;

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
        Debug.Log("level started");
    }

    public void SpawnFood()
    {

        do
        {
            foodGridPosition = new Vector2Int(Random.Range(5, width), Random.Range(5, height));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
        foodGameObject.layer = 6;
        //foodGameObject.tag = "Food";
    }


    public bool IsSnakeTookFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObject);
            SpawnFood();
            return true;
        }
        return false;
    }

    public void SnakeSetup(Snake snake)
    {
        this.snake = snake;
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0)
        {
            gridPosition.x = width ;
        }
        if (gridPosition.x > width)
        {
            gridPosition.x =  0;
        }
        if (gridPosition.y < 0)
        {
            gridPosition.y = height ;
        }
        if (gridPosition.y > height)
        {
            gridPosition.y = 0;
        }
        return gridPosition;
    }
}
