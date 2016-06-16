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
        public const string Respawn_Blast = "RespawnBlast";
        public const string End_Launch_Effect = "EndLaunchEffect";
        public const string Update_Rush_Charge = "UpdateRushCharge";
        public const string Begin_Rush_Sequence = "BeginRushSequence";
        public const string Begin_Rush_Movement = "BeginRushMovement";
        public const string End_Rush_Movement = "EndRushMovement";
        public const string Rush_Stun_Impact = "RushStunImpact";
        public const string Rush_Knockback = "RushKnockback";
        public const string Rush_Head_On_Collision = "RushHeadOnCollision";

        public const string Entered_Deep_Water = "EnteredDeepWater";
        public const string Left_Deep_Water = "LeftDeepWater";

        public const string Chest_Destroyed = "ChestDestroyed";
        public const string Spawn_Treasure = "SpawnTreasure";
        public const string Collect_Treasure = "CollectTreasure";
        public const string Update_Treasure = "UpdateTreasure";
        public const string Spawn_Power_Up = "SpawnPowerUp";

        public const string Set_Round_Timer = "SetRoundTimer";
        public const string Start_Round = "StartRound";
        public const string End_Round = "EndRound";

        public const string Apply_Power_Up_Prefix = "ApplyPowerUp-";
        public const string Start_Power_Up_Timer_Prefix = "StartPowerUpTimer-";
        public const string Total_Treasure_Prefix = "TotalTreasure-";

        public const string Avatar_Selector_Confirm = "AvatarSelectorConfirm";
        public const string Avatar_Selection_Step = "AvatarSelectionStep";
        public const string Avatar_Selected = "AvatarSelected";
    }
}
