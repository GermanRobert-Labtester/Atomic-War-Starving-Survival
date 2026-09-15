// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Foundry
{
    // ── Plan 213 domain types ────────────────────────────────────

    /// <summary>Material purity ladder (derived — never a real chemical percentage).</summary>
    public enum FoundryPurityTier
    {
        Poor = 0,
        Standard = 1,
        High = 2,
        Exceptional = 3
    }

    public static class FoundryPurityNames
    {
        public const string Poor = "Poor";
        public const string Standard = "Standard";
        public const string High = "High";
        public const string Exceptional = "Exceptional";

        public static string Name(FoundryPurityTier tier) => tier switch
        {
            FoundryPurityTier.High => High,
            FoundryPurityTier.Exceptional => Exceptional,
            FoundryPurityTier.Poor => Poor,
            _ => Standard
        };
    }

    /// <summary>Abstract forging commands (game decisions — never real procedures).</summary>
    public enum FoundryForgingCommand
    {
        Heat = 0,
        Shape = 1,
        Finish = 2,
        Inspect = 3
    }

    /// <summary>Serialized forging session (additive on the foundry state).</summary>
    [Serializable]
    public sealed class FoundryForgingSessionState
    {
        public string productOutputItemId = string.Empty;   // the batch output being worked
        public string materialProfileId = string.Empty;
        public string purity = FoundryPurityNames.Standard;
        public List<string> submitted = new List<string>();  // authored command names
        public int startedDay = 0;
        public bool completed = false;
        public int finalQualityPermille = 0;
    }

    /// <summary>Deterministic forging outcome (§173.7 — Core owns the result).</summary>
    public readonly struct FoundryForgingResult
    {
        public readonly bool Accepted;
        public readonly string ProductOutputItemId;
        public readonly int FinalQualityPermille;
        public readonly FoundryPurityTier Purity;
        public readonly int MatchedCommands;
        public readonly int ExpectedCommands;
        public readonly string Reason;

        public FoundryForgingResult(bool accepted, string outputItemId, int finalQualityPermille,
            FoundryPurityTier purity, int matched, int expected, string reason)
        {
            Accepted = accepted;
            ProductOutputItemId = outputItemId;
            FinalQualityPermille = finalQualityPermille;
            Purity = purity;
            MatchedCommands = matched;
            ExpectedCommands = expected;
            Reason = reason ?? string.Empty;
        }
    }

    /// <summary>Material quality handoff for consumers (equipment/vehicle seam — D6).</summary>
    public readonly struct FoundryMaterialQuality
    {
        public readonly string MaterialProfileId;
        public readonly FoundryPurityTier Purity;
        public readonly int DurabilityModifierBp;
        public readonly int ArmorModifierBp;
        public readonly int CorrosionModifierBp;

        public FoundryMaterialQuality(string profileId, FoundryPurityTier purity,
            int durabilityBp, int armorBp, int corrosionBp)
        {
            MaterialProfileId = profileId;
            Purity = purity;
            DurabilityModifierBp = durabilityBp;
            ArmorModifierBp = armorBp;
            CorrosionModifierBp = corrosionBp;
        }
    }

    /// <summary>
    /// Plan 213 — ADVANCED METALLURGY RECONCILIATION extension partial of the
    /// Silent Foundry authority. Adds purity tiers, the deterministic forging
    /// abstraction, and material provenance to the EXISTING batch machine.
    ///
    /// Reconciliation contract (authority map §2.4 — no duplicates):
    /// - no second heat simulation, queue, alloy catalog, or save store;
    /// - purity derives from the standard completion path (quality +
    ///   contamination + slag), it is not a parallel meter;
    /// - forging consumes a completed batch's provenance; the command
    ///   sequence is deterministic (no RNG), headless-testable;
    /// - provenance rides <see cref="FoundryProductionRecord"/> additively
    ///   (old saves read Standard/unknown — never recalculated retroactively);
    /// - vehicle armor is a handoff QUERY only (D6 — no vehicle mutation, no
    ///   vehicle authority exists).
    /// </summary>
    public sealed partial class SilentFoundrySystem
    {
        private MaterialProfileCatalog? _materialProfiles;

        // ── Binding ──────────────────────────────────────────────────

        /// <summary>
        /// Bind the material profile catalog and map profiles onto foundry
        /// output items (composition-time; recipes stay in metallurgy_recipes).
        /// </summary>
        public void BindMaterialProfiles(MaterialProfileCatalog catalog, IReadOnlyDictionary<string, string> outputItemToMaterialId)
        {
            if (catalog == null) return;
            _materialProfiles = catalog;
            if (outputItemToMaterialId != null)
            {
                foreach (var kvp in outputItemToMaterialId)
                    catalog.BindOutputItem(kvp.Key, kvp.Value);
            }
        }

        private MaterialProfileCatalog? MaterialProfiles => _materialProfiles;

        // ── Purity derivation (bounded; pure function) ──────────────

        /// <summary>
        /// Derive the purity tier of a completed cast: batch quality is the
        /// primary signal, with contamination and crucible slag as bounded
        /// downgrades. Poor ≤ quality 40; Standard ≥ 55; High ≥ 75;
        /// Exceptional ≥ 90 with low contamination/slag.
        /// </summary>
        public static FoundryPurityTier DerivePurityTier(float quality, float contamination, float slag)
        {
            float q = Math.Clamp(quality, 0f, 100f);
            // Bounded penalties: contamination up to −12, slag up to −8.
            float contaminationPenalty = Math.Clamp(contamination, 0f, 100f) * 0.12f;
            float slagPenalty = Math.Clamp(slag, 0f, 100f) * 0.10f;
            float effective = q - contaminationPenalty - slagPenalty;

            if (effective >= 90f) return FoundryPurityTier.Exceptional;
            if (effective >= 75f) return FoundryPurityTier.High;
            if (effective >= 55f) return FoundryPurityTier.Standard;
            return FoundryPurityTier.Poor;
        }

        /// <summary>Plan 213 provenance stamp applied by the completion path.</summary>
        private void ApplyPlan213Provenance(FoundryProductEntry product, FoundryProductionRecord record, float quality)
        {
            if (product == null || record == null) return;
            record.purity = FoundryPurityNames.Name(DerivePurityTier(
                quality, _state.contamination, _state.metallurgySlag));
            var profile = _materialProfiles?.FindByOutputItem(product.result_item_id);
            record.materialProfileId = profile?.material_id ?? string.Empty;
        }

        /// <summary>
        /// Material-quality handoff query for consumers (equipment/vehicle
        /// seam — D6). Returns the LATEST completed batch's provenance for a
        /// product; false when the foundry never produced it or old saves
        /// carry no provenance yet.
        /// </summary>
        /// <summary>Panel helper: the latest provenance-bearing batch regardless of product.</summary>
        public bool TryGetLatestMaterialQualityAny(out FoundryMaterialQuality quality)
        {
            quality = default;
            if (_materialProfiles == null) return false;
            for (int i = _state.completed.Count - 1; i >= 0; i--)
            {
                var record = _state.completed[i];
                if (record == null || record.materialProfileId.Length == 0) continue;
                var profile = _materialProfiles.Find(record.materialProfileId);
                if (profile == null) continue;
                quality = new FoundryMaterialQuality(
                    profile.material_id, PurityFromName(record.purity),
                    profile.durability_modifier_bp, profile.armor_modifier_bp, profile.corrosion_modifier_bp);
                return true;
            }
            return false;
        }

        public bool TryGetLatestMaterialQuality(string outputItemId, out FoundryMaterialQuality quality)
        {
            quality = default;
            if (_materialProfiles == null) return false;
            for (int i = _state.completed.Count - 1; i >= 0; i--)
            {
                var record = _state.completed[i];
                if (record == null || record.materialProfileId.Length == 0) continue;
                if (!string.Equals(record.productId, outputItemId, StringComparison.Ordinal)) continue;
                var profile = _materialProfiles.Find(record.materialProfileId);
                if (profile == null) continue;
                var purity = PurityFromName(record.purity);
                quality = new FoundryMaterialQuality(
                    profile.material_id, purity,
                    profile.durability_modifier_bp, profile.armor_modifier_bp, profile.corrosion_modifier_bp);
                return true;
            }
            return false;
        }

        private static FoundryPurityTier PurityFromName(string? name) => name switch
        {
            FoundryPurityNames.High => FoundryPurityTier.High,
            FoundryPurityNames.Exceptional => FoundryPurityTier.Exceptional,
            FoundryPurityNames.Poor => FoundryPurityTier.Poor,
            _ => FoundryPurityTier.Standard
        };

        // ── Deterministic forging abstraction (§173.7) ──────────────

        public FoundryForgingSessionState? ActiveForging =>
            _state.activeForging != null && !_state.activeForging.completed ? _state.activeForging : null;

        /// <summary>
        /// Begin a forging pass on the LATEST completed batch of an output
        /// item. Requires the foundry unlocked, no heat in progress, and an
        /// existing provenance-bearing batch. Atomic preflight.
        /// </summary>
        public string BeginForging(string outputItemId, int day)
        {
            if (!_state.unlocked) return "The Silent Foundry is not unlocked.";
            if (IsHeatActive) return "A heat is in progress; the anvil waits.";
            if (ActiveForging != null) return "A forging pass is already in progress.";
            if (_materialProfiles == null) return "No material profiles bound.";

            if (!TryGetLatestMaterialQuality(outputItemId, out var quality))
                return "No provenance-bearing batch available for " + outputItemId + ".";

            var profile = _materialProfiles.Find(quality.MaterialProfileId);
            if (profile == null) return "Unknown material profile: " + quality.MaterialProfileId;

            _state.activeForging = new FoundryForgingSessionState
            {
                productOutputItemId = outputItemId,
                materialProfileId = quality.MaterialProfileId,
                purity = FoundryPurityNames.Name(quality.Purity),
                startedDay = day,
                submitted = new List<string>()
            };
            Raise("silent_foundry_forging_started",
                profile.material_id + " pass begun (" + quality.Purity + " purity)");
            RaiseStateChanged();
            return "Forging started on the latest " + outputItemId + " batch.";
        }

        /// <summary>Submit one abstract forging command (deterministic order capture).</summary>
        public string SubmitForgingCommand(FoundryForgingCommand command, int day)
        {
            var session = ActiveForging;
            if (session == null) return "No forging pass in progress.";
            if (session.submitted.Count >= MaterialProfileCatalogLoader.MaxSequenceLength)
                return "The forging sequence is complete; finish the pass.";
            session.submitted.Add(command.ToString().ToLowerInvariant());
            RaiseStateChanged();
            return command + " recorded.";
        }

        /// <summary>
        /// Complete the pass: deterministic scoring of the submitted command
        /// sequence against the profile's authored ideal order, scaled by
        /// batch purity. No RNG anywhere.
        /// </summary>
        public FoundryForgingResult CompleteForging(int day)
        {
            var session = _state.activeForging;
            if (session == null || session.completed)
                return new FoundryForgingResult(false, string.Empty, 0, FoundryPurityTier.Standard, 0, 0, "no_forging_in_progress");
            var profile = _materialProfiles?.Find(session.materialProfileId);
            if (profile == null || profile.forging_sequence.Count == 0)
                return new FoundryForgingResult(false, string.Empty, 0, FoundryPurityTier.Standard, 0, 0, "no_material_profile");

            int expected = profile.forging_sequence.Count;
            int matched = 0;
            for (int i = 0; i < Math.Min(expected, session.submitted.Count); i++)
                if (string.Equals(session.submitted[i], profile.forging_sequence[i], StringComparison.Ordinal))
                    matched++;

            var purity = PurityFromName(session.purity);
            // Bounded deterministic score: sequence fidelity × purity ladder.
            int sequencePermille = 1000 * matched / expected;
            int purityFactor = purity switch
            {
                FoundryPurityTier.Exceptional => 1200,
                FoundryPurityTier.High => 1100,
                FoundryPurityTier.Standard => 1000,
                _ => 850
            };
            int final = Math.Clamp(sequencePermille * purityFactor / 1000, 100, 1300);

            session.completed = true;
            session.finalQualityPermille = final;

            // Stamp the craft result onto the latest provenance-bearing record.
            for (int i = _state.completed.Count - 1; i >= 0; i--)
            {
                var record = _state.completed[i];
                if (record != null && record.materialProfileId == session.materialProfileId)
                {
                    record.craftQualityPermille = final;
                    break;
                }
            }

            var result = new FoundryForgingResult(true, session.productOutputItemId,
                final, purity, matched, expected, string.Empty);
            Raise("silent_foundry_forging_completed",
                session.productOutputItemId + " forged (quality " + final + "/1000, " + purity + ")");
            RaiseStateChanged();
            OnForgingCompleted?.Invoke(result);
            return result;
        }

        /// <summary>Plan 213 typed completion event (host: sparks/steam/audio only).</summary>
        public event Action<FoundryForgingResult>? OnForgingCompleted;
    }
}
