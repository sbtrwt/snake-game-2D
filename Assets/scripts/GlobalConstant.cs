using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake2D
{
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
        public const string SnakeTag = "Snake";
        public const string FoodTag = "Food";
        public const string PowerTag = "Power";
        public const int MAX_WIDTH = 26;
        public const int MAX_HEIGHT = 13;
        public const int MIN_WIDTH = 1;
        public const int MIN_HEIGHT = 1;
        public const int OVER_LAYER = 6;
        public const string SnakeHead = "SnakeHead";
    }
}