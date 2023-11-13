using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Snake2D.PowerUp
{
    public class PowerSpawner : MonoBehaviour
    {
        public static PowerSpawner Instance { get; private set; }
        private Vector2Int powerGridPosition;
        private GameObject powerGameObject;
        [SerializeField] private GameObject powerShieldPrefab;
        [SerializeField] private GameObject powerBoostPrefab;
        [SerializeField] private GameObject powerSpeedPrefab;

        [SerializeField] private SnakeHandler[] snakeHandlers;
        private void Awake()
        {
            if (Instance == null) Instance = this;
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
                    if (s)
                    {
                        var positions = s.GetFullSnakeGridPositionList();
                        if(positions != null)
                            allSnakeFullBody.AddRange(positions);
                    }
                }
                do
                {
                    powerGridPosition = new Vector2Int(Random.Range(GlobalConstant.MIN_WIDTH, GlobalConstant.MAX_WIDTH), Random.Range(GlobalConstant.MIN_HEIGHT, GlobalConstant.MAX_HEIGHT));
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

        public void CreatePower(Vector2Int gridPosition, PowerType powerType)
        {
            switch (powerType)
            {
                case PowerType.Shield:
                    powerGameObject = Instantiate(powerShieldPrefab, new Vector3(gridPosition.x, gridPosition.y), Quaternion.identity);
                    break;

                case PowerType.ScoreBoost:
                    powerGameObject = Instantiate(powerBoostPrefab, new Vector3(gridPosition.x, gridPosition.y), Quaternion.identity);
                    break;

                case PowerType.SpeedUp:
                    powerGameObject = Instantiate(powerSpeedPrefab, new Vector3(gridPosition.x, gridPosition.y), Quaternion.identity);
                    break;
            }

            Power power = powerGameObject.GetComponent<Power>();
            power.PowerType = powerType;

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
}