using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Radiation;
using Ashfall.Core.World;
using AtomicWar.GodotApp.Audio;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Presentation-only exposure of the live Core audio event sources.
    /// AudioManager discovers this through its parent and safely rebinds when
    /// campaign host sessions are created, replaced, or cleared.
    /// </summary>
    public partial class Main : IAudioDomainProvider
    {
        RadiationSystem? IAudioDomainProvider.AudioRadiation => _survivors?.Radiation;
        WeatherSystem? IAudioDomainProvider.AudioWeather => _world?.Weather;
        TacticalCombatSystem? IAudioDomainProvider.AudioCombat => _combat?.Engine;
        CraftingSystem? IAudioDomainProvider.AudioCrafting => _crafting?.Engine;
    }
}
