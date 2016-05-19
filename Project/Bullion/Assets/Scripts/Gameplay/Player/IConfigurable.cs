using Assets.Scripts.Configuration;

namespace Assets.Scripts.Gameplay.Player
{
    public interface IConfigurable
    {
        CharacterConfiguration Configuration { set; }
    }
}
