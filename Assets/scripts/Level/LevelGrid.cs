using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;
    private PowerGridPosition powerGridPosition;
    
    private int width;
    private int height;
    [SerializeField] private Snake snake;

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
        //Debug.Log("level started");
       
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

    public void SpawnPower()
    {
        Vector2Int position;
        do
        {
            position = new Vector2Int(Random.Range(5, width), Random.Range(5, height));

        } while (snake.GetFullSnakeGridPositionList().IndexOf(position) != -1);

        int power = Random.Range(1, 4);
        PowerUp powerUp = PowerUp.None;
        switch (power)
        {
            case 1:
                powerUp = PowerUp.Shield;
                break;
            case 2:
                powerUp = PowerUp.ScoreBoost;
                break;
            case 3:
                powerUp = PowerUp.SpeedUp;
                break;
        }
        powerGridPosition = new PowerGridPosition(position, powerUp);
        powerGridPosition.CreatePower();
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

    public PowerUp IsSnakeTookPower(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == powerGridPosition.GetGridPosition())
        {
            powerGridPosition.DestroyPower();
            return powerGridPosition.GetPowerType();
        }
        return PowerUp.None;
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

    private class PowerGridPosition
    {
        private Vector2Int gridPosition;
        private PowerUp power;
        private GameObject powerGameObject;
        public PowerGridPosition(Vector2Int position, PowerUp power)
        {
            this.gridPosition = position;
            this.power = power;
        }

        public void CreatePower() 
        {
            powerGameObject = new GameObject("Power", typeof(SpriteRenderer));
            var spriteRenderer = powerGameObject.GetComponent<SpriteRenderer>();
          
            switch (power)
            {
                case PowerUp.Shield:
                    spriteRenderer.sprite = GameAssets.Instance.shieldPowerSprite;
                    break;
                case PowerUp.ScoreBoost:
                    spriteRenderer.sprite = GameAssets.Instance.scoreBoostPowerSprite;
                    break;
                case PowerUp.SpeedUp:
                    spriteRenderer.sprite = GameAssets.Instance.speedUpPowerSprite;
                    break;
            }
          
            powerGameObject.transform.position = new Vector3(gridPosition.x, gridPosition.y);
            powerGameObject.layer = 6;
        }

        public void DestroyPower() 
        {
            Object.Destroy(powerGameObject);
        }

        public PowerUp GetPowerType()
        {
            return power;
        }
        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }
    }
}
