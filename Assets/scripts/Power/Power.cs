using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public PowerType PowerType { get; set; }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Inside food : " + other.tag);
        if (other.CompareTag(GlobalConstant.SnakeTag))
        {
            PowerSpawner.Instance.DestroyPower();
        }
    }
}
