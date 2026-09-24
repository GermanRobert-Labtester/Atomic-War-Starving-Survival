// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class SkillCertificationSaveStore
    {
        public const string SectionName = "skill_certifications";
        public const string FileName = "skill_certifications_save.json";

        private static readonly SaveStore<SkillCertificationState> s_store =
            SaveStoreHub.Checksummed<SkillCertificationState>(FileName, nameof(SkillCertificationSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string? TryCapturePersisted(SkillCertificationState state) => s_store.CaptureBare(state);
        public static SkillCertificationState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(SkillCertificationState state) => s_store.TrySave(state);
        public static SkillCertificationState? TryLoad() => s_store.TryLoad();
    }

    public sealed class SkillCertificationHostSession : HostSessionBase
    {
        public SkillCertificationSystem System { get; }

        public SkillCertificationHostSession(SkillCertificationSystem? system = null)
        {
            System = system ?? new SkillCertificationSystem();
        }

        public static SkillCertificationHostSession Create(string dataDir, SkillCertificationState? restoredState = null)
        {
            var system = new SkillCertificationSystem(restoredState);
            string catalogPath = Path.Combine(dataDir, "skill_certifications.json");
            if (File.Exists(catalogPath))
            {
                system.LoadCatalog(File.ReadAllText(catalogPath));
            }
            return new SkillCertificationHostSession(system);
        }

        public bool CanAttemptExam(string survivorId, string certId, float candidateSkill, int currentDay, out string reason)
        {
            return System.CanAttemptExam(survivorId, certId, candidateSkill, currentDay, out reason);
        }

        public (bool passed, float score, string message) ConductExam(
            string candidateId,
            string certId,
            float candidateSkill,
            string examinerId,
            float examinerSkill,
            int day,
            ISeededRng rng)
        {
            var result = System.ConductExam(candidateId, certId, candidateSkill, examinerId, examinerSkill, day, rng);
            if (result.passed) RaiseStateChanged();
            return result;
        }

        public bool HasCertification(string survivorId, string certId) => System.HasCertification(survivorId, certId);

        public bool HasSpecialization(string survivorId, string specId) => System.HasSpecialization(survivorId, specId);

        public IReadOnlyList<string> GetUnlockedBenefits(string survivorId) => System.GetUnlockedBenefits(survivorId);

        public SkillTier GetTierForLevel(float skillLevel) => SkillCertificationSystem.GetTierForLevel(skillLevel);

        public SurvivorCertificationProfile? GetProfile(string survivorId) => System.GetProfile(survivorId);

        public SkillCertificationCensus GetCensus() => System.GetCensus();

        public SkillCertificationState CaptureState() => System.CaptureState();

        public void RestoreState(SkillCertificationState state)
        {
            System.RestoreState(state);
            RaiseStateChanged();
        }
    }
}
