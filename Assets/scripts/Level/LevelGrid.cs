using UnityEngine;

namespace Snake2D
{
    public class LevelGrid
    {
        private Vector2Int foodGridPosition;
        private GameObject foodGameObject;

        private int width;
        private int height;


        public LevelGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
        }


        public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
        {
            if (gridPosition.x < 0)
            {
                gridPosition.x = width;
            }
            if (gridPosition.x > width)
            {
                gridPosition.x = 0;
            }
            if (gridPosition.y < 0)
            {
                gridPosition.y = height;
            }
            if (gridPosition.y > height)
            {
                gridPosition.y = 0;
            }
            return gridPosition;
        }
    }

}