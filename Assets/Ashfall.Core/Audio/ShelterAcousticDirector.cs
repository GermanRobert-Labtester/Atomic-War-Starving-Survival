using System;
using System.Collections.Generic;

namespace Ashfall.Core.Audio
{
    [Serializable]
    public sealed class AcousticSimulationFacts
    {
        public int generatorWattage;
        public int generatorMaxWattage = 1000;
        public int ventilationLoadPermille; // 0..1000
        public int waterPumpLoadPermille;  // 0..1000
        public float ambientRadiationMillisieverts;
        public int structuralStressPermille; // 0..1000
        public bool hasUnreadRadioBroadcast;
        public string activeZoneOrRoom = "inner_vault";
    }

    [Serializable]
    public sealed class AcousticSnapshot
    {
        public string activeMixProfileId = "acue_prof_inner_vault";
        public Dictionary<string, int> continuousLayerIntensities = new Dictionary<string, int>(StringComparer.Ordinal);
        public List<string> pendingOneShotCues = new List<string>();
    }

    public sealed class ShelterAcousticDirector
    {
        private readonly Dictionary<string, ShelterAudioCueDefinition> _cues =
            new Dictionary<string, ShelterAudioCueDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, AcousticMixProfileDefinition> _profiles =
            new Dictionary<string, AcousticMixProfileDefinition>(StringComparer.Ordinal);

        private AcousticSimulationFacts _facts = new AcousticSimulationFacts();
        private readonly List<string> _pendingOneShots = new List<string>();
        private readonly ISeededRng _rng;

        public ShelterAcousticDirector(ShelterAudioCueCatalog? catalog = null, ISeededRng? rng = null)
        {
            _rng = rng ?? new SeededRng(5353);
            if (catalog != null)
            {
                LoadCatalog(catalog);
            }
        }

        public void LoadCatalog(ShelterAudioCueCatalog catalog)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));
            _cues.Clear();
            foreach (var cue in catalog.cues)
            {
                if (!string.IsNullOrEmpty(cue.id))
                    _cues[cue.id] = cue;
            }

            _profiles.Clear();
            foreach (var prof in catalog.mix_profiles)
            {
                if (!string.IsNullOrEmpty(prof.id))
                    _profiles[prof.id] = prof;
            }
        }

        public IReadOnlyDictionary<string, ShelterAudioCueDefinition> Cues => _cues;
        public IReadOnlyDictionary<string, AcousticMixProfileDefinition> Profiles => _profiles;

        public void UpdateSimulationFacts(AcousticSimulationFacts facts)
        {
            _facts = facts ?? new AcousticSimulationFacts();
        }

        public void TriggerSemanticCue(string cueId)
        {
            if (string.IsNullOrEmpty(cueId)) return;
            if (_cues.ContainsKey(cueId))
            {
                _pendingOneShots.Add(cueId);
            }
        }

        public AcousticSnapshot EvaluateSnapshot()
        {
            var snapshot = new AcousticSnapshot();

            // 1. Determine active profile based on room / zone
            string targetProfile = "acue_prof_inner_vault";
            string room = _facts.activeZoneOrRoom.ToLowerInvariant();
            if (room.Contains("surface") || room.Contains("outpost") || room.Contains("gate"))
            {
                targetProfile = "acue_prof_surface";
            }
            else if (room.Contains("airlock") || room.Contains("decon"))
            {
                targetProfile = "acue_prof_airlock";
            }
            else if (room.Contains("excavation") || room.Contains("mine") || room.Contains("stope"))
            {
                targetProfile = "acue_prof_deep_excavation";
            }

            snapshot.activeMixProfileId = _profiles.ContainsKey(targetProfile)
                ? targetProfile
                : "acue_prof_inner_vault";

            // 2. Continuous layer intensities (0..1000)
            // Generator intensity
            int genIntensity = _facts.generatorMaxWattage > 0
                ? (int)Math.Clamp((_facts.generatorWattage * 1000L) / _facts.generatorMaxWattage, 0, 1000)
                : 0;
            snapshot.continuousLayerIntensities["generator"] = genIntensity;

            // Ventilation intensity
            snapshot.continuousLayerIntensities["ventilation"] = Math.Clamp(_facts.ventilationLoadPermille, 0, 1000);

            // Water pump / machinery
            snapshot.continuousLayerIntensities["machinery"] = Math.Clamp(_facts.waterPumpLoadPermille, 0, 1000);

            // Radiation geiger click density
            int radIntensity = (int)Math.Clamp(_facts.ambientRadiationMillisieverts * 100f, 0, 1000);
            snapshot.continuousLayerIntensities["radiation"] = radIntensity;

            // Structural creak / hazard
            snapshot.continuousLayerIntensities["structural"] = Math.Clamp(_facts.structuralStressPermille, 0, 1000);

            // Radio activity
            snapshot.continuousLayerIntensities["radio"] = _facts.hasUnreadRadioBroadcast ? 600 : 50;

            // 3. One-shots
            snapshot.pendingOneShotCues.AddRange(_pendingOneShots);
            _pendingOneShots.Clear();

            // Spontaneous structural creak or falling dust if high structural stress
            if (_facts.structuralStressPermille > 600 && _rng.Next(0, 100) < 30)
            {
                snapshot.pendingOneShotCues.Add("acue_structural_creak");
            }
            if (_facts.structuralStressPermille > 800 && _rng.Next(0, 100) < 40)
            {
                snapshot.pendingOneShotCues.Add("acue_falling_dust");
            }

            return snapshot;
        }
    }
}
