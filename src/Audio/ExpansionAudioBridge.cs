using System;
using Ashfall.Core;

namespace AtomicWar.GodotApp.Audio
{
    public interface IExpansionAudioProvider
    {
        Ashfall.Core.Survivors.DesperationSystem? AudioDesperation { get; }
        Ashfall.Core.Medical.MutationSystem? AudioMutation { get; }
        Ashfall.Core.Combat.ChemWarfareSystem? AudioChemWarfare { get; }
        Ashfall.Core.Expeditions.RailwaySystem? AudioRailway { get; }
    }

    public sealed class ExpansionAudioBridge : IDisposable
    {
        private readonly Action<string> _playCue;
        private bool _disposed;

        public ExpansionAudioBridge(AudioManager audio)
            : this(audio != null ? audio.PlayCue : throw new ArgumentNullException(nameof(audio)))
        {
        }

        internal ExpansionAudioBridge(Action<string> playCue)
        {
            _playCue = playCue;
        }

        public void SubscribeAll(IExpansionAudioProvider provider)
        {
            if (provider.AudioDesperation != null)
                provider.AudioDesperation.OnTabooBroken += (r) => _playCue(AudioCueCatalog.InterrogationSlam);
            if (provider.AudioMutation != null)
                provider.AudioMutation.OnMutationAcquired += (a,b,c) => _playCue(AudioCueCatalog.BioMutationPulse);
            if (provider.AudioChemWarfare != null)
                provider.AudioChemWarfare.OnHazardDeployed += (h) => _playCue(AudioCueCatalog.HazardToxicSizzle);
            if (provider.AudioRailway != null)
                provider.AudioRailway.OnDerailment += (a,b) => _playCue(AudioCueCatalog.TrainScreechCrash);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
        }
    }
}
