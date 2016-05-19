using UnityEngine;

namespace Assets.Scripts.Configuration
{
    public class CharacterConfiguration
    {
        public string Name { get; set; }
        public float AliveMovementSpeed { get; set; }
        public float DeadMovementSpeed { get; set; }
        public int ComboStepCount { get; set; }
        public float[] ComboStepDamage { get; set; }
        public float MaximumHealth { get; set; }
        public float RushRechargeSpeed { get; set; }
        public Color RespawnPointColour { get; set; }
    }
}