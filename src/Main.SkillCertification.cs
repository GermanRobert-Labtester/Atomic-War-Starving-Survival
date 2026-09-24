// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SkillCertificationHostSession? _skillCertifications;
        private bool _skillCertificationsDirty;

        public SkillCertificationHostSession EnsureSkillCertifications()
        {
            if (_skillCertifications != null) return _skillCertifications;
            SetupSkillCertifications();
            return _skillCertifications!;
        }

        private void SetupSkillCertifications()
        {
            if (_skillCertifications != null) return;

            string dataDir = CatalogPath.ResolveDataDir();
            var saved = SkillCertificationSaveStore.TryLoad();
            _skillCertifications = SkillCertificationHostSession.Create(dataDir, saved);

            _skillCertifications.StateChanged += () =>
            {
                _skillCertificationsDirty = true;
            };

            _skillCertifications.System.OnCertificationEarned += (survivorId, certId) =>
            {
                _journal?.TryAddRawEntry(
                    "certification_earned",
                    $"Survivor {survivorId} earned formal certification: {certId}.",
                    null!,
                    _simDay);
            };

            _skillCertifications.System.OnSpecializationEarned += (survivorId, specId) =>
            {
                _journal?.TryAddRawEntry(
                    "specialization_earned",
                    $"Survivor {survivorId} unlocked elite specialization: {specId}.",
                    null!,
                    _simDay);
            };
        }

        private void SaveSkillCertifications()
        {
            if (_skillCertifications == null) return;

            var state = _skillCertifications.CaptureState();
            SkillCertificationSaveStore.TrySave(state);
            string? payload = SkillCertificationSaveStore.TryCapturePersisted(state);
            if (!string.IsNullOrEmpty(payload))
            {
                CaptureSection(SkillCertificationSaveStore.SectionName, payload);
            }
            _skillCertificationsDirty = false;
        }

        public void FlushSkillCertificationsSave()
        {
            if (_skillCertificationsDirty)
            {
                SaveSkillCertifications();
            }
        }

        public void ResetSkillCertifications()
        {
            _skillCertifications = null;
            _skillCertificationsDirty = false;
        }

        public bool CanAttemptCertificationExam(string survivorId, string certId, float candidateSkill, out string reason)
        {
            return EnsureSkillCertifications().CanAttemptExam(survivorId, certId, candidateSkill, _simDay, out reason);
        }

        public (bool passed, float score, string message) ConductCertificationExam(
            string candidateId,
            string certId,
            float candidateSkill,
            string examinerId = "",
            float examinerSkill = 0f)
        {
            var session = EnsureSkillCertifications();
            var rng = _campaignDay?.Rng.Fork("skill_certification") ?? new SeededRng(180);
            return session.ConductExam(candidateId, certId, candidateSkill, examinerId, examinerSkill, _simDay, rng);
        }

        public bool HasSurvivorCertification(string survivorId, string certId)
        {
            return EnsureSkillCertifications().HasCertification(survivorId, certId);
        }

        public bool HasSurvivorSpecialization(string survivorId, string specId)
        {
            return EnsureSkillCertifications().HasSpecialization(survivorId, specId);
        }

        public IReadOnlyList<string> GetSurvivorCertificationBenefits(string survivorId)
        {
            return EnsureSkillCertifications().GetUnlockedBenefits(survivorId);
        }

        public SkillCertificationCensus GetSkillCertificationCensus()
        {
            return EnsureSkillCertifications().GetCensus();
        }
    }
}
