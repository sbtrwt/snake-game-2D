using UnityEngine;

namespace Snake2D.PowerUp
{
    public class Power : MonoBehaviour
    {
        public PowerType PowerType { get; set; }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GlobalConstant.SnakeTag))
            {
                PowerSpawner.Instance.DestroyPower();
            }
        }
    }
}