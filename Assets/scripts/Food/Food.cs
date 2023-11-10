using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GlobalConstant.SnakeTag))
        {
            FoodSpawner.Instance.DestroyFood();
        }
    }
}
