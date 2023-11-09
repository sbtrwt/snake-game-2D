using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSpawner : MonoBehaviour
{
    public static PowerSpawner Instance { get; private set; }
    private Vector2Int powerGridPosition;
    private GameObject powerGameObject;
    private int maxWidth = GlobalConstant.MAX_WIDTH;
    private int maxHeight = GlobalConstant.MAX_HEIGHT;
    private int minWidth = GlobalConstant.MIN_WIDTH;
    private int minHeight = GlobalConstant.MIN_HEIGHT;
    [SerializeField] private SnakeHandler[] snakeHandlers;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnFoodAfterDelay(3));
    }
    public void SpawnPower()
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
                powerGridPosition = new Vector2Int(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
            } while (allSnakeFullBody.IndexOf(powerGridPosition) != -1);
        }
     

        int power = Random.Range(1, 4);
        PowerType powerType = PowerType.None;
        switch (power)
        {
            case 1:
                powerType = PowerType.Shield;
                break;
            case 2:
                powerType = PowerType.ScoreBoost;
                break;
            case 3:
                powerType = PowerType.SpeedUp;
                break;
        }
        CreatePower(powerGridPosition, powerType);

    }

    public void CreatePower(Vector2Int gridPosition,  PowerType powerType)
    {
        powerGameObject = new GameObject(GlobalConstant.PowerTag, typeof(SpriteRenderer));
        powerGameObject.transform.position = new Vector3(gridPosition.x, gridPosition.y);
        var spriteRenderer = powerGameObject.GetComponent<SpriteRenderer>();

        switch (powerType)
        {
            case PowerType.Shield:
                spriteRenderer.sprite = GameAssets.Instance.shieldPowerSprite;
                break;
            case PowerType.ScoreBoost:
                spriteRenderer.sprite = GameAssets.Instance.scoreBoostPowerSprite;
                break;
            case PowerType.SpeedUp:
                spriteRenderer.sprite = GameAssets.Instance.speedUpPowerSprite;
                break;
        }

      
        powerGameObject.layer = GlobalConstant.OVER_LAYER;
        var collider2D = powerGameObject.AddComponent<CircleCollider2D>();
        collider2D.radius = 0.3f;
        collider2D.isTrigger = true;
        Power power = powerGameObject.AddComponent<Power>();
        power.PowerType = powerType;
        var rigidbody2D = powerGameObject.AddComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        rigidbody2D.simulated = true;
        powerGameObject.tag = GlobalConstant.PowerTag;
    }

    public void DestroyPower()
    {
        Object.Destroy(powerGameObject);
        StartCoroutine(SpawnFoodAfterDelay(3));
    }


    IEnumerator SpawnFoodAfterDelay(int secs)
    {
        yield return new WaitForSeconds(secs);
        SpawnPower();
    }
}
