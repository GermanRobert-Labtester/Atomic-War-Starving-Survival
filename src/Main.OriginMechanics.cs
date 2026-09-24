// SPDX-License-Identifier: MIT
// ============================================================================
// C3-174 — Mechanical Origin Effects Seam host wiring.
// Applies the enriched survivor origin (SurvivorEnrichmentService.GetOriginModifier)
// to gameplay through the canonical owners: the keepsake item through Inventory
// and the primary skill through SkillProgressionSystem. No parallel skill or
// item store is created; the origin is a bounded, once-per-campaign grant.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private bool _originMechanicsApplied;

        /// <summary>
        /// C3-174 — applies each enriched survivor's mechanical origin once per
        /// campaign: the personal keepsake through the canonical inventory owner
        /// and the primary skill through the canonical skill-progression owner.
        /// Unenriched survivors receive nothing.
        /// </summary>
        public void ApplySurvivorOriginModifiers()
        {
            if (_originMechanicsApplied || _survivors == null) return;
            SetupEnrichment();
            _originMechanicsApplied = true;

            for (int i = 0; i < _survivors.RosterState.Count; i++)
            {
                var survivor = _survivors.RosterState[i];
                if (survivor == null || string.IsNullOrEmpty(survivor.Id)) continue;

                var def = _survivors.Roster?.FindDefinition(survivor.Id);
                var modifier = _enrichmentService.GetOriginModifier(survivor.Id, def);
                if (!modifier.HasMechanicalOrigin) continue;

                ApplyOriginModifier(modifier);
            }
        }

        /// <summary>
        /// Applies one survivor's mechanical origin on demand and reports the
        /// resolved modifier. Used by the survivor-inspection surface and the probe.
        /// </summary>
        public bool ApplyOriginModifierFor(string survivorId, out SurvivorOriginModifier modifier)
        {
            modifier = SurvivorOriginModifier.Empty(survivorId);
            if (string.IsNullOrWhiteSpace(survivorId)) return false;

            SetupEnrichment();
            var def = _survivors?.Roster?.FindDefinition(survivorId);
            modifier = _enrichmentService.GetOriginModifier(survivorId, def);
            if (!modifier.HasMechanicalOrigin) return false;

            ApplyOriginModifier(modifier);
            return true;
        }

        private void ApplyOriginModifier(SurvivorOriginModifier modifier)
        {
            // Keepsake item — granted once through the canonical inventory owner.
            // The authored enrichment catalog contains keepsake ids that are not
            // present in any item catalog (a recorded data gap), so the grant is
            // gated on the item actually resolving: an unknown id must not spam
            // the inventory log or invent an item.
            if (!string.IsNullOrEmpty(modifier.GrantedKeepsakeItemId) && _inventory?.Inventory != null)
            {
                bool resolvable = _inventory.Catalog?.Get(modifier.GrantedKeepsakeItemId) != null;
                if (resolvable && _inventory.Inventory.CountById(modifier.GrantedKeepsakeItemId) == 0)
                    _inventory.Inventory.AddById(modifier.GrantedKeepsakeItemId, 1);
            }

            // Primary skill — granted once through the canonical skill owner.
            if (!string.IsNullOrEmpty(modifier.PrimarySkillId) && _sharedSkillProgression != null)
            {
                if (!_sharedSkillProgression.HasActiveSkill(modifier.SurvivorId, modifier.PrimarySkillId))
                {
                    var actor = new SimpleSkillActor(modifier.SurvivorId);
                    _sharedSkillProgression.TryGrantSkill(actor, modifier.PrimarySkillId, _simDay);
                }
            }
        }

        public void ResetOriginMechanics() => _originMechanicsApplied = false;
    }
}
