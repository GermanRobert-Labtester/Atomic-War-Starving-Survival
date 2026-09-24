// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Psychology;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PsychologicalProfileHostSession? _psychologicalProfiles;
        private bool _psychologicalProfilesDirty;

        public PsychologicalProfileHostSession EnsurePsychologicalProfiles()
        {
            if (_psychologicalProfiles != null) return _psychologicalProfiles;
            SetupPsychologicalProfiles();
            return _psychologicalProfiles!;
        }

        private void SetupPsychologicalProfiles()
        {
            if (_psychologicalProfiles != null) return;

            string dataDir = CatalogPath.ResolveDataDir();
            var saved = PsychologicalProfileSaveStore.TryLoad();
            _psychologicalProfiles = PsychologicalProfileHostSession.Create(dataDir, saved);

            _psychologicalProfiles.StateChanged += () =>
            {
                _psychologicalProfilesDirty = true;
            };

            _psychologicalProfiles.System.OnPhobiaDeveloped += (survivorId, phobiaId, severity) =>
            {
                _journal?.TryAddRawEntry(
                    "phobia_developed",
                    $"Survivor {survivorId} developed {phobiaId} (severity: {severity:F0}).",
                    null!,
                    _simDay);
            };

            _psychologicalProfiles.System.OnPhobiaTriggered += (survivorId, phobiaId) =>
            {
                _journal?.TryAddRawEntry(
                    "phobia_triggered",
                    $"Survivor {survivorId}'s {phobiaId} was triggered by environmental conditions.",
                    null!,
                    _simDay);
            };
        }

        private void SavePsychologicalProfiles()
        {
            if (_psychologicalProfiles == null) return;

            var state = _psychologicalProfiles.CaptureState();
            PsychologicalProfileSaveStore.TrySave(state);
            string? payload = PsychologicalProfileSaveStore.TryCapturePersisted(state);
            if (!string.IsNullOrEmpty(payload))
            {
                CaptureSection(PsychologicalProfileSaveStore.SectionName, payload);
            }
            _psychologicalProfilesDirty = false;
        }

        public void FlushPsychologicalProfilesSave()
        {
            if (_psychologicalProfilesDirty)
            {
                SavePsychologicalProfiles();
            }
        }

        public void ResetPsychologicalProfiles()
        {
            _psychologicalProfiles = null;
            _psychologicalProfilesDirty = false;
        }

        public bool RecordSurvivorTrauma(string survivorId, string eventType, float severity)
        {
            var session = EnsurePsychologicalProfiles();
            var rng = _campaignDay?.Rng.Fork("psychology_trauma") ?? new SeededRng(179);
            return session.RecordTraumaEvent(survivorId, eventType, severity, _simDay, rng);
        }

        public PhobiaTriggerResult EvaluateSurvivorPhobiaTriggers(string survivorId, string triggerCondition)
        {
            return EnsurePsychologicalProfiles().EvaluatePhobiaExposure(survivorId, triggerCondition);
        }

        public bool TeachSurvivorCopingMechanism(string survivorId, string mechanismId)
        {
            return EnsurePsychologicalProfiles().TeachCopingMechanism(survivorId, mechanismId, "training", _simDay);
        }

        public bool ConductSurvivorTherapy(string survivorId, string phobiaId, float therapistSkill)
        {
            return EnsurePsychologicalProfiles().ConductTherapySession(survivorId, phobiaId, therapistSkill);
        }

        public float GetSurvivorPsychologicalResilience(string survivorId)
        {
            return EnsurePsychologicalProfiles().GetProfileResilienceScore(survivorId);
        }

        public PsychologicalProfileCensus GetPsychologicalProfileCensus()
        {
            return EnsurePsychologicalProfiles().GetCensus();
        }
    }
}
