using JetBrains.Annotations;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace SweetMoleHouse.MarioForever.Scripts.Player.Ability
{
    public class AbilityManager : MonoBehaviour
    {
        [SerializeField, RenameInInspector("射击音效")]
        private AudioClip shootSound;

        [SerializeField] private Transform smallSizeMuzzle;
        [SerializeField] private Transform bigSizeMuzzle;

        private BaseAbility[] abilities;
        [CanBeNull] private BaseAbility active;
        private Mario mario;

        public AudioClip ShootSound => shootSound;
        public bool CanShoot => active != null;
        public Mario Mario => mario;

        private BaseAbility[] Abilities => abilities ??= GetComponents<BaseAbility>();

        public void SetPowerup(MarioPowerup powerup)
        {
            // 防止重复 set 浪费性能
            if (active != null && active.CorrespondingPowerup == powerup) return;

            active = null;
            foreach (var ability in Abilities)
            {
                var isActive = ability.CorrespondingPowerup == powerup;
                ability.enabled = isActive;
                if (isActive)
                {
                    active = ability;
                }
            }
        }

        public Transform GetMuzzle()
        {
            return mario.Size == MarioSize.SMALL ? smallSizeMuzzle : bigSizeMuzzle;
        }

        private void Start()
        {
            mario = GetComponentInParent<Mario>();
            
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

    }
}
