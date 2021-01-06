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

        [SerializeField] private Transform smallSizeMuzzle;
        [SerializeField] private Transform bigSizeMuzzle;

        private BaseAbility[] abilities;
        [CanBeNull] private BaseAbility active;
        private Mario mario;

        public AudioClip ShootSound => shootSound;
        public bool CanShoot => active != null;
        public Mario Mario => mario;
        
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

        public Transform GetMuzzle()
        {
            return mario.Size == MarioSize.SMALL ? smallSizeMuzzle : bigSizeMuzzle;
        }

        private void Start()
        {
            abilities = GetComponents<BaseAbility>();
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
