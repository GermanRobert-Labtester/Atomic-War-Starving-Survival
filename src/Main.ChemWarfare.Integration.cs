// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ChemWarfareSystem? _chemWarfare;

        private bool _chemWarfareDirty;

        // ── Plan 198: Biological Weapons & Chemical Warfare ─────────────

        public ChemWarfareSystem EnsureChemWarfare()
        {
            if (_chemWarfare != null) return _chemWarfare;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("chem_warfare") : new SeededRng(198);
            _chemWarfare = new ChemWarfareSystem(rng, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("chemical_weapons.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _chemWarfare.LoadCatalog(json);
                }
            }

            var saved = ChemWarfareSaveStore.TryLoad();
            if (saved != null)
            {
                _chemWarfare.RestoreState(saved);
            }

            _chemWarfare.OnShelterResidueCreated += (sector, severity) =>
            {
                _journal?.TryAddRawEntry("chem_hazard_breach", $"Toxic chemical residue detected in {sector} (Severity: {severity})!", null!, _simDay);
            };

            // Toxic exposure → combat/survivor health (not radiation / DoseLedger).
            // EvaluateActorExposure is invoked from CombatHostSession.ActionEndTurn.
            _chemWarfare.OnToxicExposureResolved += (actorId, severity, lane) =>
            {
                _journal?.TryAddRawEntry(
                    "chem_toxic_exposure",
                    $"Toxic exposure on {actorId} in lane {lane + 1} (Severity: {severity}).",
                    null!,
                    _simDay);

                float hp = 8f * Math.Max(0, severity);
                if (hp <= 0f) return;

                if (_combat?.Engine?.State != null && !_combat.Engine.State.Resolved)
                {
                    var c = _combat.Engine.State.Combatants
                        .Find(x => x != null && (x.Id == actorId || x.SurvivorId == actorId));
                    if (c != null && !c.IsDowned)
                        c.Health = Math.Max(0f, c.Health - hp);
                    string survivorId = c != null && !string.IsNullOrEmpty(c.SurvivorId)
                        ? c.SurvivorId
                        : actorId;
                    _combat.Engine.Ports?.DamageSurvivor?.Invoke(survivorId, hp);
                }
                else if (_survivors?.Needs != null)
                {
                    _survivors.Needs.Modify(actorId, NeedKind.Health, -hp);
                }

                _chemWarfareDirty = true;
            };

            _chemWarfare.OnStateChanged += () => _chemWarfareDirty = true;
            return _chemWarfare;
        }

        private void SetupChemWarfare()
        {
            EnsureChemWarfare();
        }

        private void SaveChemWarfare()
        {
            if (_chemWarfare != null)
            {
                CaptureSection("chem_warfare", ChemWarfareSaveStore.TryCapturePersisted(_chemWarfare.CaptureState()));
                _chemWarfareDirty = false;
            }
        }

        // ── Plan 198: chem warfare console commands ────────────────────────

        private void HandleChemWarfareAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseChemWarfareDefensePanel(); return; }
            if (_chemWarfareDefensePanel == null || _chemWarfare == null) return;

            switch (action)
            {
                case "clear_hazard":
                {
                    // Decon dispatch: clears the tactical hazard through the
                    // canonical owner. Residue incidents keep routing through
                    // OnShelterResidueCreated → journal (wired in EnsureChemWarfare).
                    bool cleared = _chemWarfare.ClearHazard(param);
                    _chemWarfareDefensePanel.ShowFeedback(
                        cleared ? "Decon team reports the hazard dispersed."
                                : "That hazard is no longer on the board.",
                        !cleared);
                    break;
                }
            }
            _chemWarfareDefensePanel.RefreshView();
        }

    }
}
