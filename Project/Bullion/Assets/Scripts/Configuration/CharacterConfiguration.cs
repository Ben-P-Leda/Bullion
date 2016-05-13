using UnityEngine;

namespace Assets.Scripts.Configuration
{
    public class CharacterConfiguration
    {
        public string Name { get; set; }
        public float MovementSpeed { get; set; }
        public int ComboStepCount { get; set; }
        public float[] ComboStepDamage { get; set; }
        public float MaximumHealth { get; set; }
    }
}