using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake2D.Food
{
    public class FoodSpawner : MonoBehaviour
    {
        public static FoodSpawner Instance { get; private set; }
        private Vector2Int foodGridPosition;
        private GameObject foodGameObject;
        [SerializeField] private GameObject foodPrefab;
        [SerializeField] private SnakeHandler[] snakeHandlers;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        private void Start()
        {
            StartCoroutine(SpawnFoodAfterDelay(0));
        }
        public void SpawnFood()
        {
            if (snakeHandlers != null)
            {
                List<Vector2Int> allSnakeFullBody = new List<Vector2Int>();
                foreach (var s in snakeHandlers)
                {
                    if (s)
                    {
                        var positions = s.GetFullSnakeGridPositionList();
                        if (positions != null)
                            allSnakeFullBody.AddRange(positions);
                    }
                }
                do
                {
                    foodGridPosition = new Vector2Int(Random.Range(GlobalConstant.MIN_WIDTH, GlobalConstant.MAX_WIDTH), Random.Range(GlobalConstant.MIN_HEIGHT, GlobalConstant.MAX_HEIGHT));
                } while (allSnakeFullBody.IndexOf(foodGridPosition) != -1);
            }

            foodGameObject = Instantiate(foodPrefab, new Vector3(foodGridPosition.x, foodGridPosition.y), Quaternion.identity);


        }

        public void DestroyFood()
        {
            Object.Destroy(foodGameObject);
            StartCoroutine(SpawnFoodAfterDelay(0));
        }


        IEnumerator SpawnFoodAfterDelay(int secs)
        {
            yield return new WaitForSeconds(secs);
            SpawnFood();
        }
    }
}