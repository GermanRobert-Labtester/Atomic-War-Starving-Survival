// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Survivors
{
    public enum SkillTier
    {
        Novice = 0,
        Competent = 1,
        Proficient = 2,
        Expert = 3,
        Master = 4
    }

    [Serializable]
    public sealed class SkillTierDef
    {
        public SkillTier Tier { get; set; }
        public string TierName { get; set; } = string.Empty;
        public float LevelThreshold { get; set; }
        public float BonusModifier { get; set; } = 1.0f;
        public string Description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SkillCertificationDef
    {
        public string cert_id { get; set; } = string.Empty;
        public string cert_name { get; set; } = string.Empty;
        public string discipline { get; set; } = string.Empty;
        public string required_tier { get; set; } = string.Empty;
        public float required_level { get; set; }
        public float exam_difficulty { get; set; } = 50.0f;
        public List<string> required_experience { get; set; } = new List<string>();
        public List<string> benefits { get; set; } = new List<string>();
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SkillSpecializationDef
    {
        public string spec_id { get; set; } = string.Empty;
        public string spec_name { get; set; } = string.Empty;
        public List<string> required_cert_ids { get; set; } = new List<string>();
        public List<string> unique_abilities { get; set; } = new List<string>();
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SkillCertificationCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<SkillCertificationDef> certifications { get; set; } = new List<SkillCertificationDef>();
        public List<SkillSpecializationDef> specializations { get; set; } = new List<SkillSpecializationDef>();
    }

    [Serializable]
    public sealed class SurvivorCertificationRecord
    {
        public string CertId { get; set; } = string.Empty;
        public int EarnedDay { get; set; }
        public string CertifyingSurvivorId { get; set; } = string.Empty;
        public float ExamScore { get; set; }
    }

    [Serializable]
    public sealed class SurvivorSpecializationRecord
    {
        public string SpecId { get; set; } = string.Empty;
        public int EarnedDay { get; set; }
    }

    [Serializable]
    public sealed class SurvivorCertificationProfile
    {
        public string SurvivorId { get; set; } = string.Empty;
        public List<SurvivorCertificationRecord> Certifications { get; set; } = new List<SurvivorCertificationRecord>();
        public List<SurvivorSpecializationRecord> Specializations { get; set; } = new List<SurvivorSpecializationRecord>();
        public int LastExamDay { get; set; }
    }

    [Serializable]
    public sealed class SkillCertificationState
    {
        public int SchemaVersion { get; set; } = 1;
        public int TotalCertificationsAwarded { get; set; }
        public int TotalExamsConduct { get; set; }
        public List<SurvivorCertificationProfile> Profiles { get; set; } = new List<SurvivorCertificationProfile>();
    }

    public struct SkillCertificationCensus
    {
        public readonly int CertifiedSurvivorsCount;
        public readonly int TotalCertificationsAwarded;
        public readonly int TotalSpecializationsAwarded;
        public readonly int TotalExamsConducted;

        public SkillCertificationCensus(int certifiedSurvivors, int totalCerts, int totalSpecs, int totalExams)
        {
            CertifiedSurvivorsCount = certifiedSurvivors;
            TotalCertificationsAwarded = totalCerts;
            TotalSpecializationsAwarded = totalSpecs;
            TotalExamsConducted = totalExams;
        }
    }

    /// <summary>
    /// Plan 180 — Skill Certification & Tier System.
    /// Provides formal qualifications, tiered progression thresholds, and specializations
    /// over survivor disciplines, unlocking concrete capabilities and social roles.
    /// Pure Core domain authority.
    /// </summary>
    public sealed class SkillCertificationSystem
    {
        private static readonly SkillTierDef[] TierDefinitions = new[]
        {
            new SkillTierDef { Tier = SkillTier.Novice, TierName = "Novice", LevelThreshold = 0f, BonusModifier = 1.0f, Description = "Basic apprentice level." },
            new SkillTierDef { Tier = SkillTier.Competent, TierName = "Competent", LevelThreshold = 20f, BonusModifier = 1.10f, Description = "Reliable foundation and independent execution." },
            new SkillTierDef { Tier = SkillTier.Proficient, TierName = "Proficient", LevelThreshold = 40f, BonusModifier = 1.25f, Description = "Advanced execution and consistent quality." },
            new SkillTierDef { Tier = SkillTier.Expert, TierName = "Expert", LevelThreshold = 60f, BonusModifier = 1.50f, Description = "High-grade mastery capable of complex operations." },
            new SkillTierDef { Tier = SkillTier.Master, TierName = "Master", LevelThreshold = 80f, BonusModifier = 2.0f, Description = "Pinnacle authority capable of pioneering and training." }
        };

        private readonly SkillCertificationState _state;
        private readonly List<SkillCertificationDef> _certifications = new List<SkillCertificationDef>();
        private readonly List<SkillSpecializationDef> _specializations = new List<SkillSpecializationDef>();

        public event Action<string, string>? OnCertificationEarned;     // survivorId, certId
        public event Action<string, string>? OnSpecializationEarned;    // survivorId, specId
        public event Action<string, string, string>? OnExamFailed;       // survivorId, certId, reason

        public IReadOnlyList<SkillCertificationDef> Certifications => _certifications;
        public IReadOnlyList<SkillSpecializationDef> Specializations => _specializations;
        public IReadOnlyList<SurvivorCertificationProfile> Profiles => _state.Profiles;

        public IReadOnlyList<SkillCertificationDef> GetAllCertifications() => _certifications;
        public IReadOnlyList<SkillSpecializationDef> GetAllSpecializations() => _specializations;

        public SkillCertificationSystem(SkillCertificationState? state = null)
        {
            _state = state ?? new SkillCertificationState();
        }

        public static SkillTier GetTierForLevel(float skillLevel)
        {
            if (skillLevel >= 80f) return SkillTier.Master;
            if (skillLevel >= 60f) return SkillTier.Expert;
            if (skillLevel >= 40f) return SkillTier.Proficient;
            if (skillLevel >= 20f) return SkillTier.Competent;
            return SkillTier.Novice;
        }

        public static SkillTierDef GetTierDef(SkillTier tier)
        {
            int idx = (int)tier;
            if (idx >= 0 && idx < TierDefinitions.Length) return TierDefinitions[idx];
            return TierDefinitions[0];
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var cat = JsonSerializer.Deserialize<SkillCertificationCatalog>(json, options);
                if (cat == null) return;

                if (cat.certifications != null)
                {
                    foreach (var c in cat.certifications)
                    {
                        if (!string.IsNullOrWhiteSpace(c.cert_id))
                        {
                            int idx = _certifications.FindIndex(x => string.Equals(x.cert_id, c.cert_id, StringComparison.Ordinal));
                            if (idx >= 0) _certifications[idx] = c;
                            else _certifications.Add(c);
                        }
                    }
                }

                if (cat.specializations != null)
                {
                    foreach (var s in cat.specializations)
                    {
                        if (!string.IsNullOrWhiteSpace(s.spec_id))
                        {
                            int idx = _specializations.FindIndex(x => string.Equals(x.spec_id, s.spec_id, StringComparison.Ordinal));
                            if (idx >= 0) _specializations[idx] = s;
                            else _specializations.Add(s);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Fallback handled by data integrity checks
            }
        }

        public SurvivorCertificationProfile GetOrCreateProfile(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentNullException(nameof(survivorId));

            var p = _state.Profiles.FirstOrDefault(x => string.Equals(x.SurvivorId, survivorId, StringComparison.Ordinal));
            if (p == null)
            {
                p = new SurvivorCertificationProfile { SurvivorId = survivorId };
                _state.Profiles.Add(p);
            }
            return p;
        }

        public SurvivorCertificationProfile? GetProfile(string survivorId) =>
            _state.Profiles.FirstOrDefault(x => string.Equals(x.SurvivorId, survivorId, StringComparison.Ordinal));

        public bool HasCertification(string survivorId, string certId)
        {
            var p = GetProfile(survivorId);
            return p != null && p.Certifications.Any(c => string.Equals(c.CertId, certId, StringComparison.Ordinal));
        }

        public bool HasSpecialization(string survivorId, string specId)
        {
            var p = GetProfile(survivorId);
            return p != null && p.Specializations.Any(s => string.Equals(s.SpecId, specId, StringComparison.Ordinal));
        }

        public bool CanAttemptExam(string survivorId, string certId, float candidateSkillLevel, int currentDay, out string reason)
        {
            if (string.IsNullOrWhiteSpace(survivorId))
            {
                reason = "invalid_survivor";
                return false;
            }

            var cert = _certifications.FirstOrDefault(c => string.Equals(c.cert_id, certId, StringComparison.Ordinal));
            if (cert == null)
            {
                reason = "unknown_certification";
                return false;
            }

            if (HasCertification(survivorId, certId))
            {
                reason = "already_certified";
                return false;
            }

            if (candidateSkillLevel < cert.required_level)
            {
                reason = $"insufficient_skill_level (requires {cert.required_level:F0})";
                return false;
            }

            var p = GetProfile(survivorId);
            if (p != null && (currentDay - p.LastExamDay) < 2 && p.LastExamDay > 0)
            {
                reason = "exam_cooldown";
                return false;
            }

            reason = "eligible";
            return true;
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
            if (!CanAttemptExam(candidateId, certId, candidateSkill, day, out string eligibilityReason))
            {
                OnExamFailed?.Invoke(candidateId, certId, eligibilityReason);
                return (false, 0f, eligibilityReason);
            }

            var cert = _certifications.First(c => string.Equals(c.cert_id, certId, StringComparison.Ordinal));
            var profile = GetOrCreateProfile(candidateId);
            profile.LastExamDay = day;
            _state.TotalExamsConduct++;

            // Candidate baseline roll
            double roll = rng.NextDouble() * 30.0; // 0-30
            float baseScore = (candidateSkill * 0.70f) + (float)roll;

            // Examiner contribution
            if (!string.IsNullOrWhiteSpace(examinerId) && !string.Equals(examinerId, candidateId, StringComparison.Ordinal))
            {
                bool examinerCertified = HasCertification(examinerId, certId);
                float examinerBonus = examinerCertified ? 15.0f : (examinerSkill * 0.10f);
                baseScore += examinerBonus;
            }

            float finalScore = Math.Clamp(baseScore, 0f, 100f);
            if (finalScore >= cert.exam_difficulty)
            {
                var record = new SurvivorCertificationRecord
                {
                    CertId = certId,
                    EarnedDay = day,
                    CertifyingSurvivorId = examinerId ?? string.Empty,
                    ExamScore = finalScore
                };
                profile.Certifications.Add(record);
                _state.TotalCertificationsAwarded++;
                OnCertificationEarned?.Invoke(candidateId, certId);

                // Check specialization unlocks
                EvaluateSpecializations(profile, day);

                return (true, finalScore, "Certification awarded successfully.");
            }
            else
            {
                OnExamFailed?.Invoke(candidateId, certId, $"Score {finalScore:F1} did not meet difficulty {cert.exam_difficulty:F1}");
                return (false, finalScore, $"Failed exam: scored {finalScore:F1}/{cert.exam_difficulty:F1}.");
            }
        }

        private void EvaluateSpecializations(SurvivorCertificationProfile profile, int day)
        {
            var certifiedIds = new HashSet<string>(profile.Certifications.Select(c => c.CertId), StringComparer.Ordinal);

            foreach (var spec in _specializations)
            {
                if (profile.Specializations.Any(s => string.Equals(s.SpecId, spec.spec_id, StringComparison.Ordinal)))
                    continue;

                bool allMet = spec.required_cert_ids.All(req => certifiedIds.Contains(req));
                if (allMet)
                {
                    profile.Specializations.Add(new SurvivorSpecializationRecord
                    {
                        SpecId = spec.spec_id,
                        EarnedDay = day
                    });
                    OnSpecializationEarned?.Invoke(profile.SurvivorId, spec.spec_id);
                }
            }
        }

        public IReadOnlyList<string> GetUnlockedBenefits(string survivorId)
        {
            var p = GetProfile(survivorId);
            if (p == null) return Array.Empty<string>();

            var benefits = new HashSet<string>(StringComparer.Ordinal);
            foreach (var certRec in p.Certifications)
            {
                var def = _certifications.FirstOrDefault(c => string.Equals(c.cert_id, certRec.CertId, StringComparison.Ordinal));
                if (def?.benefits != null)
                {
                    foreach (var b in def.benefits) benefits.Add(b);
                }
            }

            foreach (var specRec in p.Specializations)
            {
                var def = _specializations.FirstOrDefault(s => string.Equals(s.spec_id, specRec.SpecId, StringComparison.Ordinal));
                if (def?.unique_abilities != null)
                {
                    foreach (var a in def.unique_abilities) benefits.Add(a);
                }
            }

            return benefits.ToList();
        }

        public SkillCertificationCensus GetCensus()
        {
            int totalCerts = 0;
            int totalSpecs = 0;
            int certifiedSurvivors = 0;

            foreach (var p in _state.Profiles)
            {
                if (p.Certifications.Count > 0)
                {
                    certifiedSurvivors++;
                    totalCerts += p.Certifications.Count;
                }
                totalSpecs += p.Specializations.Count;
            }

            return new SkillCertificationCensus(
                certifiedSurvivors,
                totalCerts,
                totalSpecs,
                _state.TotalExamsConduct);
        }

        public SkillCertificationState CaptureState()
        {
            var captured = new SkillCertificationState
            {
                SchemaVersion = _state.SchemaVersion,
                TotalCertificationsAwarded = _state.TotalCertificationsAwarded,
                TotalExamsConduct = _state.TotalExamsConduct,
                Profiles = new List<SurvivorCertificationProfile>(_state.Profiles.Count)
            };

            foreach (var p in _state.Profiles)
            {
                var pCopy = new SurvivorCertificationProfile
                {
                    SurvivorId = p.SurvivorId,
                    LastExamDay = p.LastExamDay,
                    Certifications = new List<SurvivorCertificationRecord>(p.Certifications.Count),
                    Specializations = new List<SurvivorSpecializationRecord>(p.Specializations.Count)
                };

                foreach (var c in p.Certifications)
                {
                    pCopy.Certifications.Add(new SurvivorCertificationRecord
                    {
                        CertId = c.CertId,
                        EarnedDay = c.EarnedDay,
                        CertifyingSurvivorId = c.CertifyingSurvivorId,
                        ExamScore = c.ExamScore
                    });
                }

                foreach (var s in p.Specializations)
                {
                    pCopy.Specializations.Add(new SurvivorSpecializationRecord
                    {
                        SpecId = s.SpecId,
                        EarnedDay = s.EarnedDay
                    });
                }

                captured.Profiles.Add(pCopy);
            }

            return captured;
        }

        public void RestoreState(SkillCertificationState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.TotalCertificationsAwarded = state.TotalCertificationsAwarded;
            _state.TotalExamsConduct = state.TotalExamsConduct;
            _state.Profiles.Clear();

            if (state.Profiles != null)
            {
                foreach (var p in state.Profiles)
                {
                    var pCopy = new SurvivorCertificationProfile
                    {
                        SurvivorId = p.SurvivorId,
                        LastExamDay = p.LastExamDay,
                        Certifications = new List<SurvivorCertificationRecord>(),
                        Specializations = new List<SurvivorSpecializationRecord>()
                    };

                    if (p.Certifications != null)
                    {
                        foreach (var c in p.Certifications)
                        {
                            pCopy.Certifications.Add(new SurvivorCertificationRecord
                            {
                                CertId = c.CertId,
                                EarnedDay = c.EarnedDay,
                                CertifyingSurvivorId = c.CertifyingSurvivorId ?? string.Empty,
                                ExamScore = c.ExamScore
                            });
                        }
                    }

                    if (p.Specializations != null)
                    {
                        foreach (var s in p.Specializations)
                        {
                            pCopy.Specializations.Add(new SurvivorSpecializationRecord
                            {
                                SpecId = s.SpecId,
                                EarnedDay = s.EarnedDay
                            });
                        }
                    }

                    _state.Profiles.Add(pCopy);
                }
            }
        }
    }
}
