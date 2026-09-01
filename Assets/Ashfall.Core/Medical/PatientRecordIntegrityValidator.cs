// SPDX-License-Identifier: MIT
// Task #133 — Pipeline referential integrity validator (restoration + tests).
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Validates medical pipeline state against canonical survivor identity and
    /// the treatment catalog. Run at restoration and in tests — never per frame.
    ///
    /// <para>Findings classify as repairable (deterministic documented rule) or
    /// fatal (load must reject with the precise code). Never let load order
    /// decide implicitly (Phase 54).</para>
    /// </summary>
    public static class PatientRecordIntegrityValidator
    {
        /// <summary>Delegates the host supplies: known survivor ids and their lifecycle availability.</summary>
        public sealed class Context
        {
            /// <summary>True when the id is a survivor this campaign knows.</summary>
            public Func<string, bool> IsKnownSurvivor = _ => true;
            /// <summary>True when treatment/procedures are legal for this survivor right now.</summary>
            public Func<string, bool> IsTreatmentEligible = _ => true;
            /// <summary>True when the inventory knows this item id (catalog membership).</summary>
            public Func<string, bool> IsKnownItem = _ => true;
        }

        public static List<MedicalPipelineIntegrityFinding> Validate(
            MedicalPipelineSaveState state, Context context)
        {
            var findings = new List<MedicalPipelineIntegrityFinding>();
            if (state == null) return findings;

            // ── Diagnosis knowledge ──────────────────────────────────
            if (state.diagnosis != null)
            {
                foreach (var record in state.diagnosis.records)
                {
                    if (record == null || string.IsNullOrEmpty(record.episodeId))
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "diagnosis_missing_episode",
                            detail = "empty episode id dropped on restore",
                            fatal = false
                        });
                        continue;
                    }
                    if (!AfflictionEpisodeId.IsValid(record.episodeId))
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "diagnosis_invalid_episode_id",
                            detail = record.episodeId,
                            fatal = true
                        });
                        continue;
                    }
                    var episode = new AfflictionEpisodeId(record.episodeId);
                    if (!context.IsKnownSurvivor(episode.Survivor.Value))
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "diagnosis_unknown_survivor",
                            detail = record.episodeId,
                            fatal = true
                        });
                    }
                }
            }

            // ── Reservations ─────────────────────────────────────────
            if (state.reservations != null)
            {
                var seen = new HashSet<int>();
                foreach (var r in state.reservations.reservations)
                {
                    if (r == null || r.reservationId <= 0)
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "reservation_invalid_id",
                            detail = "row dropped on restore",
                            fatal = false
                        });
                        continue;
                    }
                    if (!seen.Add(r.reservationId))
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "reservation_duplicate_id",
                            detail = $"reservation {r.reservationId} appears twice",
                            fatal = true
                        });
                        continue;
                    }
                    if (!context.IsKnownSurvivor(r.survivorId))
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "reservation_unknown_survivor",
                            detail = $"reservation {r.reservationId} for '{r.survivorId}'",
                            fatal = true
                        });
                    }
                    if (r.kind == "medicine" && !context.IsKnownItem(r.targetId))
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "reservation_unknown_item",
                            detail = $"reservation {r.reservationId} targets '{r.targetId}'",
                            fatal = true
                        });
                    }
                    if (r.quantity <= 0)
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "reservation_invalid_quantity",
                            detail = $"reservation {r.reservationId} quantity {r.quantity}",
                            fatal = false
                        });
                    }
                }
            }

            // ── Procedures ───────────────────────────────────────────
            if (state.procedures != null)
            {
                var reservationOwner = new Dictionary<int, int>();
                foreach (var p in state.procedures.procedures)
                {
                    if (p == null || p.procedureId <= 0)
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "procedure_invalid_id",
                            detail = "row dropped on restore",
                            fatal = false
                        });
                        continue;
                    }
                    if (!context.IsKnownSurvivor(p.survivorId))
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "procedure_unknown_survivor",
                            detail = $"procedure {p.procedureId} for '{p.survivorId}'",
                            fatal = true
                        });
                    }
                    if (MedicalTreatmentCatalog.Get(p.treatmentId) == null)
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "procedure_unknown_treatment",
                            detail = $"procedure {p.procedureId} treatment '{p.treatmentId}'",
                            fatal = true
                        });
                    }
                    if (string.Equals(p.status, "active", StringComparison.Ordinal) && p.remainingHours < 0f)
                    {
                        findings.Add(new MedicalPipelineIntegrityFinding
                        {
                            code = "procedure_negative_remaining",
                            detail = $"procedure {p.procedureId} remaining {p.remainingHours}",
                            fatal = false
                        });
                    }
                    foreach (var resId in p.reservationIds)
                    {
                        if (reservationOwner.TryGetValue(resId, out var other) && other != p.procedureId)
                        {
                            findings.Add(new MedicalPipelineIntegrityFinding
                            {
                                code = "reservation_double_claim",
                                detail = $"reservation {resId} claimed by procedures {other} and {p.procedureId}",
                                fatal = true
                            });
                        }
                        else
                        {
                            reservationOwner[resId] = p.procedureId;
                        }
                    }
                }
            }

            return findings;
        }
    }
}
