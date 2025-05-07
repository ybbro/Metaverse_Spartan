using UnityEngine;
namespace MiniGame2
{
    public class StatHandler : MonoBehaviour
    {
        [Range(1, 100), SerializeField] int health = 10;

        public int Health
        {
            get => health;
            set => health = Mathf.Clamp(value, 0, 100);
        }

        [Range(1f, 20f), SerializeField] float speed = 3;
        public float Speed
        {
            get => speed;
            set => speed = Mathf.Clamp(value, 0, 20);
        }
    }
}
