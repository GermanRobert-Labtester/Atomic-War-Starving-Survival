// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ApprenticeshipCurriculumSaveStore
// Core Engine: Ashfall.Core.Education.ApprenticeshipCurriculumEngine
// Host Caller: Main.ApprenticeshipCurriculum
// Purpose    : Apprenticeship curriculum. The Core engine is the sole authority
//              over literacy progression, comprehension gain, fatigue onset, and
//              vocational certification readiness; the host owns only the learner
//              roster and its persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Education;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    [Serializable]
    public sealed class ApprenticeshipCurriculumSaveState
    {
        public int schema_version = 1;
        public int currentDay;
        public List<LearnerRecord> learners = new List<LearnerRecord>();
    }

    public static class ApprenticeshipCurriculumSaveStore
    {
        public const string FileName = "apprenticeship_curriculum_save.json";
        public const string SectionName = "apprenticeship_curriculum";

        private static readonly SaveStore<ApprenticeshipCurriculumSaveState> s_store =
            SaveStoreHub.Checksummed<ApprenticeshipCurriculumSaveState>(FileName, nameof(ApprenticeshipCurriculumSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(ApprenticeshipCurriculumSaveState state) => s_store.CaptureBare(state);
        public static ApprenticeshipCurriculumSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ApprenticeshipCurriculumSaveState state) => s_store.TrySave(state);
        public static ApprenticeshipCurriculumSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the pure Core curriculum engine.</summary>
    public sealed class ApprenticeshipCurriculumHostSession : HostSessionBase
    {
        private readonly ApprenticeshipCurriculumSaveState _state;

        public ApprenticeshipCurriculumHostSession(ApprenticeshipCurriculumSaveState? state = null)
        {
            _state = state ?? new ApprenticeshipCurriculumSaveState();
            _state.learners ??= new List<LearnerRecord>();
        }

        public static ApprenticeshipCurriculumHostSession Create(ApprenticeshipCurriculumSaveState? state = null) =>
            new ApprenticeshipCurriculumHostSession(state);

        public string LastEvent { get; private set; } = string.Empty;
        public int CurrentDay => _state.currentDay;
        public IReadOnlyList<LearnerRecord> Learners => _state.learners;
        public int LearnerCount => _state.learners.Count;

        public LearnerRecord? FindLearner(string survivorId) => _state.learners.FirstOrDefault(l =>
            string.Equals(l.SurvivorId, survivorId, StringComparison.Ordinal));

        public LearnerRecord EnrollLearner(
            string survivorId,
            LiteracyLevel literacyLevel = LiteracyLevel.Illiterate,
            int comprehensionPermille = 0)
        {
            var existing = FindLearner(survivorId);
            if (existing != null) return existing;

            var learner = new LearnerRecord
            {
                SurvivorId = survivorId ?? string.Empty,
                LiteracyLevel = literacyLevel,
                ComprehensionPermille = Math.Max(0, Math.Min(1000, comprehensionPermille))
            };
            _state.learners.Add(learner);
            LastEvent = $"Enrolled {survivorId} in the apprenticeship curriculum.";
            RaiseStateChanged();
            return learner;
        }

        public bool RemoveLearner(string survivorId)
        {
            int removed = _state.learners.RemoveAll(l => string.Equals(l.SurvivorId, survivorId, StringComparison.Ordinal));
            if (removed == 0) return false;
            LastEvent = $"Withdrew {survivorId} from the curriculum.";
            RaiseStateChanged();
            return true;
        }

        /// <summary>
        /// Runs one teaching session. The Core engine decides comprehension gain,
        /// tier advancement, and fatigue; the host only applies the returned
        /// deltas and records the session.
        /// </summary>
        public CurriculumSessionResult RunSession(
            string survivorId,
            int teacherSkillPermille,
            int manualAvailabilityPermille,
            int sessionSeed,
            int day)
        {
            var learner = FindLearner(survivorId);
            if (learner == null) throw new InvalidOperationException($"Unknown learner '{survivorId}'.");

            var result = ApprenticeshipCurriculumEngine.AdvanceLiteracySession(
                learner, teacherSkillPermille, manualAvailabilityPermille, sessionSeed);

            learner.ComprehensionPermille = Math.Max(0, Math.Min(1000,
                learner.ComprehensionPermille + result.ComprehensionGained));
            if (result.AdvancedLiteracyTier) learner.LiteracyLevel = result.NewLiteracyLevel;
            if (result.FatiguePenaltyPermille > 0) learner.FatigueSessionCount++;

            _state.currentDay = day;
            LastEvent = $"Session for {survivorId}: +{result.ComprehensionGained} comprehension, tier {learner.LiteracyLevel}.";
            RaiseStateChanged();
            return result;
        }

        public bool IsCertificationReady(string survivorId, string tradeId)
        {
            var learner = FindLearner(survivorId);
            if (learner == null) return false;
            if (!ApprenticeshipCurriculumEngine.EvaluateVocationalCertification(learner, tradeId)) return false;
            if (!learner.TradeSkillPermille.TryGetValue(tradeId, out int skill)) return false;
            return skill >= ApprenticeshipCurriculumEngine.CertificationReadyThreshold;
        }

        public LiteracyLevel GetLiteracyLevel(string survivorId) =>
            FindLearner(survivorId)?.LiteracyLevel ?? LiteracyLevel.Illiterate;

        public int CertifiedTradeCount(string survivorId)
        {
            var learner = FindLearner(survivorId);
            if (learner == null) return 0;
            int n = 0;
            foreach (var kvp in learner.TradeSkillPermille)
            {
                if (ApprenticeshipCurriculumEngine.EvaluateVocationalCertification(learner, kvp.Key)
                    && kvp.Value >= ApprenticeshipCurriculumEngine.CertificationReadyThreshold)
                    n++;
            }
            return n;
        }

        public ApprenticeshipCurriculumSaveState CaptureState()
        {
            var copy = new ApprenticeshipCurriculumSaveState
            {
                schema_version = _state.schema_version,
                currentDay = _state.currentDay
            };
            foreach (var l in _state.learners) copy.learners.Add(l.Clone());
            return copy;
        }

        public void RestoreState(ApprenticeshipCurriculumSaveState state)
        {
            _state.currentDay = state?.currentDay ?? 0;
            _state.learners.Clear();
            if (state == null) return;
            _state.schema_version = state.schema_version;
            if (state.learners != null)
                foreach (var l in state.learners)
                    if (l != null) _state.learners.Add(l.Clone());
            LastEvent = "Restored apprenticeship curriculum state.";
            RaiseStateChanged();
        }

        public bool TrySave() => ApprenticeshipCurriculumSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = ApprenticeshipCurriculumSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
