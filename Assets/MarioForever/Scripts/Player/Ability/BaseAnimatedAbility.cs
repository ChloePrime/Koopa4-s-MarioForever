namespace SweetMoleHouse.MarioForever.Player.Ability
{
    public abstract class BaseAnimatedAbility : BaseAbility
    {
        public override void OnShoot()
        {
            Manager.Mario.Anims.StartShooting();
            Global.PlaySound(Manager.ShootSound);
        }
    }
}
