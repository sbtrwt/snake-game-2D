using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance { get; private set; }
    public Sprite snakeHeadSprite;
    public Sprite foodSprite;
    public Sprite bodySprite;
    public Sprite cornerBodySprite;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

   


}
