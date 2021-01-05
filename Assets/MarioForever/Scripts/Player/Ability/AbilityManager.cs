using JetBrains.Annotations;
using SweetMoleHouse.MarioForever.Constants;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace SweetMoleHouse.MarioForever.Player.Ability
{
    public class AbilityManager : MonoBehaviour
    {
        [SerializeField, RenameInInspector("射击音效")]
        private AudioClip shootSound;

        private BaseAbility[] abilities;
        [CanBeNull] private BaseAbility active;

        public AudioClip ShootSound => shootSound;
        public bool CanShoot => active != null;

        private void Start()
        {
            abilities = GetComponents<BaseAbility>();
            InitInput();
        }

        private void InitInput()
        {
            var input = Global.Inputs.Mario;
            input.FireOrRun.performed += OnFire;
        }

        private void OnDisable()
        {
            var input = Global.Inputs.Mario;
            input.FireOrRun.performed -= OnFire;
        }

        private void OnFire(CallbackContext ctx)
        {
            if (active != null)
            {
                active.OnShoot();
            }
        }

        public void SetPowerup(MarioPowerup powerup)
        {
            // 防止重复 set 浪费性能
            if (active != null && active.CorrespondingPowerup == powerup) return;

            active = null;
            foreach (var ability in abilities)
            {
                var isActive = ability.CorrespondingPowerup == powerup;
                ability.enabled = isActive;
                if (isActive)
                {
                    active = ability;
                }
            }
        }
    }
}
