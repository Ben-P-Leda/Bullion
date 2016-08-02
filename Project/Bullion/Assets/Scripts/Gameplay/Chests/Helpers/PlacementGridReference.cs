namespace Assets.Scripts.Gameplay.Chests.Helpers
{
    public class PlacementGridReference
    {
        public int x { get; set; }
        public int z { get; set; }

        public PlacementGridReference()
        {
            x = 0;
            z = 0;
        }

        public PlacementGridReference(int x, int z)
        {
            this.x = x;
            this.z = z;
        }
    }
}
