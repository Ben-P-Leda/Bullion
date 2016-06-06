using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Chests.Contents.PowerUps
{
    public class PowerUpLifeCycle : ChestItemLifeCycle
    {
        public PowerUpEffect Effect;

        public override void InitializeComponents()
        {
            base.InitializeComponents();

            LaunchTriggerEvent = EventMessage.Collect_Power_Up;
            EventValue = (float)Effect;
        }
    }
}