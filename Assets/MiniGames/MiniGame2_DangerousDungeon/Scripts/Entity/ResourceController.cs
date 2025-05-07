using UnityEngine;
using System;
namespace MiniGame2
{
    public class ResourceController : MonoBehaviour
    {
        [SerializeField] float healthChangeDelay = .5f; // 피격 후 무적 시간
        BaseController baseController;
        StatHandler statHandler;
        AnimationHandler animationHandler;

        float timeSinceLastChange = float.MaxValue;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => statHandler.Health;

        public AudioClip damageClip;

        Action<float, float> OnHealthChange;

        private void Awake()
        {
            TryGetComponent(out baseController);
            TryGetComponent(out statHandler);
            TryGetComponent(out animationHandler);
        }

        private void Start()
        {
            CurrentHealth = statHandler.Health;
        }

        private void Update()
        {
            if (timeSinceLastChange < healthChangeDelay)
            {
                timeSinceLastChange += Time.deltaTime;
                if (timeSinceLastChange >= healthChangeDelay)
                {
                    animationHandler.InvincibilityEnd();
                }
            }
        }

        public bool ChangeHealth(float change)
        {
            // 변화량이 음수가 아니거나(대미지가 아님), 무적 상태라면
            if (change >= 0 || timeSinceLastChange < healthChangeDelay)
                return false;

            timeSinceLastChange = 0;
            CurrentHealth += change;
            CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
            CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

            // 체력 변경 UI 표시 이벤트 호출
            OnHealthChange?.Invoke(CurrentHealth, MaxHealth);

            if (change < 0)
            {
                animationHandler.Damage();

                if (damageClip)
                    GameManager.Instance.soundManager.PlaySfx(damageClip);
            }

            if (CurrentHealth <= 0)
                Death();

            return true;
        }

        void Death()
        {
            baseController.Death();
        }

        public void Add_HPChange_Event(Action<float, float> action) => OnHealthChange += action;
        public void Remove_HPChange_Event(Action<float, float> action) => OnHealthChange -= action;
    }
}