using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class PowerUpLifeCycle : ChestItemLifeCycle
    {
        public PowerUpEffect Effect;

        public override void InitializeComponents()
        {
            base.InitializeComponents();

            LaunchTriggerEvent = EventMessage.Collect_Treasure;
            EventValue = (float)Effect;
        }
    }
}