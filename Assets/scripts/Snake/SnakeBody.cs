using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    public GameObject snakeHead;
    public Snake snake;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GlobalConstant.SnakeTag))
        {
            //Debug.Log("Inside snake body : " + other.tag);
            //var otherSnake = snakeHead.GetComponent<Snake>();
            //if (otherSnake != null && otherSnake != snake)
            //{
            //    otherSnake.SetSnakeDead();
            //    otherSnake.DestroySnakeBody();
            //}
        }
    }
}
