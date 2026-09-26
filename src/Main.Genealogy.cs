// SPDX-License-Identifier: MIT
// ASHFALL Plan 217 — Survivor Genealogy & Family Tree host wiring.
//
// Authority boundaries (plan revision 2026-09-24): the roster and
// ChildDevelopment own birth identity; RomanceFamilySystem owns family units
// and bonds; SurvivorFate owns death. This adapter only records committed
// kinship FACTS into the genealogy authority when canonical owners emit them:
//   family unit established (two committed partners)  -> one union fact
//   child welcomed (birth/adoption)                   -> parent→child lineage
//   survivor fate (death)                             -> one family event
// It never infers kinship from names, never creates a second family-unit
// ledger, and never generates a name as proof of kinship.

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Legacy;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private GenealogyHostSession? _genealogy;
        private bool _genealogyDirty;

        public GenealogyHostSession? Genealogy => _genealogy;

        public void SetupGenealogy()
        {
            if (_genealogy != null) return;

            // Compose over the hosted succession engine — never a second one.
            var engine = _expansions?.Generational ?? new GenerationalSuccessionEngine();
            _genealogy = new GenealogyHostSession(engine);

            var saved = GenealogySaveStore.TryLoad();
            if (saved != null)
            {
                _genealogy.RestoreState(saved);
            }

            _genealogy.StateChanged += () => _genealogyDirty = true;

            // Canonical producer 1+2: the romance/family owner's committed
            // facts. Parent resolution goes through the canonical family unit
            // (FamilyResolver), never through name inference.
            if (_romanceFamily != null)
            {
                _genealogy.FamilyResolver = familyId =>
                {
                    foreach (var unit in _romanceFamily.System.FamilyUnits)
                    {
                        if (unit != null && string.Equals(unit.FamilyId, familyId, StringComparison.OrdinalIgnoreCase))
                        {
                            return unit;
                        }
                    }
                    return null;
                };
                _romanceFamily.System.OnFamilyUnitEstablishedSeam += (familyId, parentIds) =>
                {
                    if (_genealogy == null || parentIds == null || parentIds.Count < 2) return;
                    _genealogy.RecordUnion(parentIds[0], parentIds[1], _campaignDay.LastAdvancedDay);
                };
                _romanceFamily.System.OnChildWelcomedToFamilySeam += (familyId, childId, isAdopted) =>
                {
                    _genealogy?.RecordChild(familyId, childId, isAdopted, _campaignDay.LastAdvancedDay);
                };
            }

            // Canonical producer 3: the fate owner's committed death fact
            // (OnSurvivorFate is the canonical "survivor_perished" producer —
            // the same event the Plan 42 voice trigger and Plan 46 metrics use).
            _survivorFate.OnSurvivorFate += fate =>
            {
                if (fate == null || string.IsNullOrWhiteSpace(fate.survivorId)) return;
                _genealogy?.RecordDeath(fate.survivorId, fate.day > 0 ? fate.day : _campaignDay.LastAdvancedDay);
            };

            if (_survivorDetailPanel != null)
            {
                _survivorDetailPanel.KinshipProvider = id =>
                {
                    if (_genealogy == null) return null;
                    var (parents, children, spouse, familyName) = _genealogy.GetKinship(id);
                    if (parents.Count == 0 && children.Count == 0
                        && string.IsNullOrEmpty(spouse) && string.IsNullOrEmpty(familyName))
                    {
                        return null;
                    }
                    var parts = new List<string>();
                    if (!string.IsNullOrEmpty(familyName)) parts.Add($"family {familyName}");
                    if (!string.IsNullOrEmpty(spouse)) parts.Add($"partner {spouse}");
                    if (parents.Count > 0) parts.Add($"parents {string.Join(", ", parents)}");
                    if (children.Count > 0) parts.Add($"children {string.Join(", ", children)}");
                    return string.Join(" · ", parts);
                };
            }
        }

        public void SaveGenealogy()
        {
            if (_genealogy == null) return;
            var state = _genealogy.CaptureState();
            GenealogySaveStore.TrySave(state);
            if (CaptureSection("genealogy", GenealogySaveStore.TryCapturePersisted(state)))
            {
                _genealogyDirty = false;
            }
        }

        public void FlushGenealogyIfDirty()
        {
            if (_genealogyDirty) SaveGenealogy();
        }

        public void ResetGenealogy()
        {
            _genealogy = null;
            _genealogyDirty = false;
        }
    }
}
