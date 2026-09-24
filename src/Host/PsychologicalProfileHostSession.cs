// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Psychology;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class PsychologicalProfileSaveStore
    {
        public const string SectionName = "psychological_profiles";
        public const string FileName = "psychological_profiles_save.json";

        private static readonly SaveStore<PsychologyState> s_store =
            SaveStoreHub.Checksummed<PsychologyState>(FileName, nameof(PsychologicalProfileSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string? TryCapturePersisted(PsychologyState state) => s_store.CaptureBare(state);
        public static PsychologyState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(PsychologyState state) => s_store.TrySave(state);
        public static PsychologyState? TryLoad() => s_store.TryLoad();
    }

    public sealed class PsychologicalProfileHostSession : HostSessionBase
    {
        public PsychologicalProfileSystem System { get; }

        public PsychologicalProfileHostSession(PsychologicalProfileSystem? system = null)
        {
            System = system ?? new PsychologicalProfileSystem();
        }

        public static PsychologicalProfileHostSession Create(string dataDir, PsychologyState? restoredState = null)
        {
            var system = new PsychologicalProfileSystem(restoredState);
            string catalogPath = Path.Combine(dataDir, "psychology_profiles.json");
            if (File.Exists(catalogPath))
            {
                system.LoadCatalog(File.ReadAllText(catalogPath));
            }
            return new PsychologicalProfileHostSession(system);
        }

        public bool RecordTraumaEvent(string survivorId, string eventType, float severity, int day, ISeededRng rng)
        {
            bool phobiaDeveloped = System.RecordTraumaEvent(survivorId, eventType, severity, day, rng);
            RaiseStateChanged();
            return phobiaDeveloped;
        }

        public PhobiaTriggerResult EvaluatePhobiaExposure(string survivorId, string triggerCondition)
        {
            return System.EvaluatePhobiaExposure(survivorId, triggerCondition);
        }

        public bool TeachCopingMechanism(string survivorId, string mechanismId, string source = "therapy", int day = 1)
        {
            bool ok = System.TeachCopingMechanism(survivorId, mechanismId, source, day);
            if (ok) RaiseStateChanged();
            return ok;
        }

        public bool ConductTherapySession(string survivorId, string phobiaId, float therapistSkill)
        {
            bool ok = System.ConductTherapySession(survivorId, phobiaId, therapistSkill);
            if (ok) RaiseStateChanged();
            return ok;
        }

        public float GetProfileResilienceScore(string survivorId) => System.GetProfileResilienceScore(survivorId);

        public SurvivorPsychologicalProfile? GetProfile(string survivorId) => System.GetProfile(survivorId);

        public PsychologicalProfileCensus GetCensus() => System.GetCensus();

        public PsychologyState CaptureState() => System.CaptureState();

        public void RestoreState(PsychologyState state)
        {
            System.RestoreState(state);
            RaiseStateChanged();
        }
    }
}
