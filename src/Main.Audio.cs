using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Disease;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using AtomicWar.GodotApp.Audio;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Presentation-only exposure of the live Core audio event sources.
    /// AudioManager discovers this through its parent and safely rebinds when
    /// campaign host sessions are created, replaced, or cleared.
    /// </summary>
    public partial class Main : IAudioDomainProvider, IExpansionAudioProvider
    {
        private Ashfall.Core.AudioConditionSystem _audioConditions = new Ashfall.Core.AudioConditionSystem();
        RadiationSystem? IAudioDomainProvider.AudioRadiation => _survivors?.Radiation;
        WeatherSystem? IAudioDomainProvider.AudioWeather => _world?.Weather;
        TacticalCombatSystem? IAudioDomainProvider.AudioCombat => _combat?.Engine;
        CraftingSystem? IAudioDomainProvider.AudioCrafting => _crafting?.Engine;
        ExpeditionSystem? IAudioDomainProvider.AudioExpeditions => _expeditions?.Engine;
        DiseaseSystem? IAudioDomainProvider.AudioDisease => _disease?.Engine;
        SurvivorFateSystem? IAudioDomainProvider.AudioSurvivorFate => _survivorFate;
        PowerGridSystem? IAudioDomainProvider.AudioPowerGrid => _powerGrid?.System;
        StartingLevelSystem? IAudioDomainProvider.AudioStartingLevel => _startingLevel?.System;
        Ashfall.Core.AudioConditionSystem? IAudioDomainProvider.AudioConditions => _audioConditions;

        Ashfall.Core.Survivors.DesperationSystem? IExpansionAudioProvider.AudioDesperation => EnsureDesperation();
        Ashfall.Core.Medical.MutationSystem? IExpansionAudioProvider.AudioMutation => EnsureMutations();
        Ashfall.Core.Combat.ChemWarfareSystem? IExpansionAudioProvider.AudioChemWarfare => EnsureChemWarfare();
        Ashfall.Core.Expeditions.RailwaySystem? IExpansionAudioProvider.AudioRailway => EnsureRailway();
    }
}
