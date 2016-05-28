namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestPlacementGridCell
    {
        public bool PermanentlyUnavailable { get; set; }
        public bool TemporarilyUnavailable { get; set; }
        public bool Available {  get { return !(PermanentlyUnavailable || TemporarilyUnavailable); } }
        public bool CannotBeClusterCenter { get; set; }
    }
}