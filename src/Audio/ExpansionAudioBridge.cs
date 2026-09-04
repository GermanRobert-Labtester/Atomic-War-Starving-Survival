using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Medical;
using Ashfall.Core.Combat;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp.Audio
{
    public interface IExpansionAudioProvider
    {
        DesperationSystem? AudioDesperation { get; }
        MutationSystem? AudioMutation { get; }
        ChemWarfareSystem? AudioChemWarfare { get; }
        RailwaySystem? AudioRailway { get; }
    }

    public sealed class ExpansionAudioBridge : IDisposable
    {
        private readonly Action<string> _playCue;
        private DesperationSystem? _desperation;
        private MutationSystem? _mutation;
        private ChemWarfareSystem? _chemWarfare;
        private RailwaySystem? _railway;
        private bool _disposed;

        public ExpansionAudioBridge(AudioManager audio)
            : this(audio != null ? audio.PlayCue : throw new ArgumentNullException(nameof(audio)))
        {
        }

        internal ExpansionAudioBridge(Action<string> playCue)
        {
            _playCue = playCue ?? throw new ArgumentNullException(nameof(playCue));
        }

        public void SubscribeAll(IExpansionAudioProvider? provider)
        {
            if (_disposed) return;
            if (provider == null)
            {
                UnbindAll();
                return;
            }

            BindDesperation(provider.AudioDesperation);
            BindMutation(provider.AudioMutation);
            BindChemWarfare(provider.AudioChemWarfare);
            BindRailway(provider.AudioRailway);
        }

        public void BindDesperation(DesperationSystem? next)
        {
            if (_disposed) return;
            if (ReferenceEquals(_desperation, next)) return;

            if (_desperation != null)
            {
                _desperation.OnTabooBroken -= HandleTabooBroken;
            }
            _desperation = next;
            if (_desperation != null)
            {
                _desperation.OnTabooBroken += HandleTabooBroken;
            }
        }

        public void BindMutation(MutationSystem? next)
        {
            if (_disposed) return;
            if (ReferenceEquals(_mutation, next)) return;

            if (_mutation != null)
            {
                _mutation.OnMutationAcquired -= HandleMutationAcquired;
            }
            _mutation = next;
            if (_mutation != null)
            {
                _mutation.OnMutationAcquired += HandleMutationAcquired;
            }
        }

        public void BindChemWarfare(ChemWarfareSystem? next)
        {
            if (_disposed) return;
            if (ReferenceEquals(_chemWarfare, next)) return;

            if (_chemWarfare != null)
            {
                _chemWarfare.OnHazardDeployed -= HandleHazardDeployed;
            }
            _chemWarfare = next;
            if (_chemWarfare != null)
            {
                _chemWarfare.OnHazardDeployed += HandleHazardDeployed;
            }
        }

        public void BindRailway(RailwaySystem? next)
        {
            if (_disposed) return;
            if (ReferenceEquals(_railway, next)) return;

            if (_railway != null)
            {
                _railway.OnDerailment -= HandleDerailment;
            }
            _railway = next;
            if (_railway != null)
            {
                _railway.OnDerailment += HandleDerailment;
            }
        }

        private void HandleTabooBroken(DesperationActRecord record)
        {
            _playCue(AudioCueCatalog.InterrogationSlam);
        }

        private void HandleMutationAcquired(string survivorId, string mutationId, List<string> tags)
        {
            _playCue(AudioCueCatalog.BioMutationPulse);
        }

        private void HandleHazardDeployed(ToxicHazardZoneState hazard)
        {
            _playCue(AudioCueCatalog.HazardToxicSizzle);
        }

        private void HandleDerailment(string trainId, string segmentId)
        {
            _playCue(AudioCueCatalog.TrainScreechCrash);
        }

        public void UnbindAll()
        {
            if (_desperation != null)
            {
                _desperation.OnTabooBroken -= HandleTabooBroken;
                _desperation = null;
            }
            if (_mutation != null)
            {
                _mutation.OnMutationAcquired -= HandleMutationAcquired;
                _mutation = null;
            }
            if (_chemWarfare != null)
            {
                _chemWarfare.OnHazardDeployed -= HandleHazardDeployed;
                _chemWarfare = null;
            }
            if (_railway != null)
            {
                _railway.OnDerailment -= HandleDerailment;
                _railway = null;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            UnbindAll();
        }
    }
}
