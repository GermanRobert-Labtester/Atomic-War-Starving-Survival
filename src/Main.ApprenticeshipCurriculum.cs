// SPDX-License-Identifier: MIT
// ============================================================================
// Apprenticeship curriculum host composition. The Core
// ApprenticeshipCurriculumEngine is the sole authority over literacy
// progression, comprehension gain, fatigue onset, and vocational certification
// readiness. The host owns only the learner roster and supplies the
// teacher-skill, manual-availability, and deterministic session-seed facts.
// ============================================================================

using Ashfall.Core.Education;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ApprenticeshipCurriculumHostSession? _curriculum;
        private bool _curriculumDirty;

        public ApprenticeshipCurriculumHostSession? ApprenticeshipCurriculumSession => _curriculum;

        public void SetupApprenticeshipCurriculum()
        {
            if (_curriculum != null) return;
            var saved = ApprenticeshipCurriculumSaveStore.TryLoad();
            _curriculum = ApprenticeshipCurriculumHostSession.Create(saved);
            _curriculum.StateChanged += () => _curriculumDirty = true;
        }

        public LearnerRecord EnrollCurriculumLearner(
            string survivorId,
            LiteracyLevel literacyLevel = LiteracyLevel.Illiterate,
            int comprehensionPermille = 0)
        {
            SetupApprenticeshipCurriculum();
            var learner = _curriculum!.EnrollLearner(survivorId, literacyLevel, comprehensionPermille);
            _curriculumDirty = true;
            return learner;
        }

        /// <summary>
        /// Runs one teaching session. Teacher skill and manual availability come from
        /// the existing mentorship and library owners; the session seed is derived
        /// from the deterministic campaign RNG stream so replays stay stable.
        /// </summary>
        public CurriculumSessionResult RunCurriculumSession(
            string survivorId, int teacherSkillPermille, int manualAvailabilityPermille, int day)
        {
            SetupApprenticeshipCurriculum();
            uint sessionSeed = unchecked((uint)((day * 31) ^ survivorId?.GetHashCode() ?? 0));
            var result = _curriculum!.RunSession(
                survivorId, teacherSkillPermille, manualAvailabilityPermille, (int)sessionSeed, day);
            _curriculumDirty = true;
            return result;
        }

        public bool IsCurriculumCertificationReady(string survivorId, string tradeId)
        {
            SetupApprenticeshipCurriculum();
            return _curriculum!.IsCertificationReady(survivorId, tradeId);
        }

        public (int Learners, int CertifiedTrades) GetApprenticeshipCurriculumReadout()
        {
            SetupApprenticeshipCurriculum();
            int certified = 0;
            foreach (var learner in _curriculum!.Learners)
                certified += _curriculum.CertifiedTradeCount(learner.SurvivorId);
            return (_curriculum.LearnerCount, certified);
        }

        public void SaveApprenticeshipCurriculum()
        {
            if (_curriculum == null) return;
            var state = _curriculum.CaptureState();
            if (CaptureSection(ApprenticeshipCurriculumSaveStore.SectionName, ApprenticeshipCurriculumSaveStore.TryCapturePersisted(state)))
                _curriculumDirty = false;
        }

        public void ResetApprenticeshipCurriculum()
        {
            _curriculum = null;
            _curriculumDirty = false;
        }
    }
}
