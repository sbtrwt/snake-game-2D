using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerType
{
    None,
    Shield,
    ScoreBoost,
    SpeedUp
}
public enum Direction
{
    Left,
    Right,
    Up,
    Down
}
public enum PlayerType
{
    One,
    Two
}
public class GlobalConstant 
{
    public static string SnakeTag = "Snake";
    public static string FoodTag = "Food";
    public static string PowerTag = "Power";
    public const int  MAX_WIDTH = 26;
    public const int MAX_HEIGHT= 13;
    public const int MIN_WIDTH = 1;
    public const int MIN_HEIGHT = 1;
    public const int OVER_LAYER = 6;
}