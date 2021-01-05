namespace SweetMoleHouse.MarioForever.Player.Ability
{
    public abstract class BaseAnimatedAbility : BaseAbility
    {
        private Mario mario;
        protected override void Start()
        {
            base.Start();
            mario = GetComponentInParent<Mario>();
        }

        public override void OnShoot()
        {
            mario.Anims.StartShooting();
            Global.PlaySound(Manager.ShootSound);
        }
    }
}
