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

    public void SpawnFood() {
        foodGridPosition = new Vector2Int(Random.Range(5, width), Random.Range(5, height));

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
        foodGameObject.layer = 6;
        //foodGameObject.tag = "Food";
    }


    public void SnakeMoved(Vector2Int snakeGridPosition)
    {
        if(snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObject);
            SpawnFood();
        }
    }

    public void SnakeSetup(Snake snake)
    {
        this.snake = snake;
    }
}
