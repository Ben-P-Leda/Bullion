using Assets.Scripts.Configuration;

namespace Assets.Scripts.Gameplay.Player.Interfaces
{
    public interface IConfigurable
    {
        CharacterConfiguration Configuration { set; }
    }
}
