using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance { get; private set; }
    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;
    
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
                allSnakeFullBody.AddRange(s.GetFullSnakeGridPositionList());
            }
            do
            {
                foodGridPosition = new Vector2Int(Random.Range(GlobalConstant.MIN_WIDTH, GlobalConstant.MAX_WIDTH), Random.Range(GlobalConstant.MIN_HEIGHT, GlobalConstant.MAX_HEIGHT));
            } while (allSnakeFullBody.IndexOf(foodGridPosition) != -1);
        }

        foodGameObject = new GameObject(GlobalConstant.FoodTag, typeof(SpriteRenderer));
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.foodSprite;
        foodGameObject.layer = GlobalConstant.OVER_LAYER;
        var collider2D = foodGameObject.AddComponent<CircleCollider2D>();
        collider2D.radius = 0.3f;
        collider2D.isTrigger = true;
        foodGameObject.AddComponent<Food>();
        var rigidbody2D = foodGameObject.AddComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        rigidbody2D.simulated = true;
        foodGameObject.tag = GlobalConstant.FoodTag;
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
