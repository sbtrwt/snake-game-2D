using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridPosition;
    private Vector2Int gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimeMax;
    
    private void Awake()
    {
        gridPosition = new Vector2Int(0, 0);
        gridMoveTimeMax = 1f;
        gridMoveTimer = gridMoveTimeMax;
        gridMoveDirection = new Vector2Int(1, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gridPosition.y += 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gridPosition.y -= 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gridPosition.x -= 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gridPosition.x += 1;
        }

        gridMoveTimer += Time.deltaTime;
        if(gridMoveTimer > gridMoveTimeMax)
        {
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimeMax;
        }
            
        transform.position = new Vector3(gridPosition.x, gridPosition.y);
    }
}
