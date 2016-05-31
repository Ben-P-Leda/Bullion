using UnityEngine;

namespace Assets.Scripts.Gameplay.Treasure.Helpers
{
    public class PlacementGridCell
    {
        public Vector3 Center { get; set; }
        public bool PermanentlyUnavailable { get; set; }
        public bool TemporarilyUnavailable { get; set; }
        public bool Available {  get { return !(PermanentlyUnavailable || TemporarilyUnavailable); } }
        public bool CannotBeClusterCenter { get; set; }
    }
}