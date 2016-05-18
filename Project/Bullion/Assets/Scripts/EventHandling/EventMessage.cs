namespace Assets.Scripts.EventHandling
{
    public class EventMessage
    {
        public const string Block_Movement_Attack = "AttackBlockMovement";
        public const string Change_Strike_State = "ChangeStrikeState";
        public const string Inflict_Damage = "InflictDamage";
        public const string Has_Died = "BeginDeathSequence";
        public const string Enter_Dead_Mode = "EndDeathSequence";
        public const string Block_Attack_Swimming = "IsSwimming";
        public const string Hit_Trigger_Collider = "HitTriggerCollider";
        public const string Respawn = "Respawn";
        public const string Update_Health = "UpdateHealth";
    }
}
