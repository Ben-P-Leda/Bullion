using Assets.Scripts.Gameplay.Player.Support;

namespace Assets.Scripts.Gameplay.Player.Interfaces
{
    public interface IModifiable
    {
        CharacterConfigurationModifier ConfigurationModifier { set; }
    }
}
