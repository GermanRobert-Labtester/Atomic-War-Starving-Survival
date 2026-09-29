// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Farming;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {

        // ==================================================================
        // Plan 164 — PsychologicalArcSystem (breakdown arcs)
        // ==================================================================

        private PsychologyArcHostSession _psychologyArcs = null!;
        private bool _psychologyArcsDirty;
        private UI.PsychologyArcPanel _psychologyArcPanel = null!;
        private bool _psychologyArcPanelBound;

        private void SetupPsychologyArcs()
        {
            if (_psychologyArcs != null) return;
            SetupSurvivors();
            SetupInventory();

            var catalog = Ashfall.Core.Survivors.MentalArcCatalogLoader.Load(_dataDir);
            var system = new Ashfall.Core.Survivors.PsychologicalArcSystem(catalog.arcs);
            var saved = PsychologyArcSaveStore.TryLoad();
            if (saved != null)
                system.RestoreState(saved);

            _psychologyArcs = new PsychologyArcHostSession(system);
            _psychologyArcs.StateChanged += () => _psychologyArcsDirty = true;

            // Hoarding: host picks the item deterministically (first low-value
            // staple in slot order) and moves it out of the shared inventory.
            // Refusal returns (string, int)? null — nothing ever vanishes.
            system.TryTransferToStash = (survivorId, _, day) =>
            {
                var inv = _inventory.Inventory;
                if (inv.CountById("canned_food") > 2 && inv.TryConsume("canned_food", 1))
                    return ("canned_food", 1);
                return null;
            };

            system.OnBreakdownArcStarted += (survivorId, arcId) =>
            {
                // A first canonical breakdown is the existing trauma authority's
                // producer for the scarred-state narrative gate. The flag is
                // monotonic and provenance-backed; EchoSystem only reads it.
                _consequenceLedger.Set(
                    "scarred_state",
                    Ashfall.Core.Survivors.PsychologicalArcSystem.SystemId,
                    $"breakdown:{survivorId}:{arcId}",
                    _simDay);
                _journal?.TryAddRawEntry($"arc_started_{survivorId}",
                    $"{survivorId} is not holding together — {arcId.Replace("arc_", "").Replace('_', ' ')} taking hold.",
                    null!, _simDay);
            };
            system.OnBreakdownEscalated += (survivorId, arcId, from, to) =>
                _journal?.TryAddRawEntry($"arc_stage_{survivorId}_{to}",
                    $"{survivorId}'s crisis deepened ({from} → {to}).",
                    null!, _simDay);
            system.OnUnsafeFireIncidentRequested += (survivorId, day) =>
            {
                // The fire authority owns ignition and damage (plan §7.11).
                var fire = ShelterFireSession?.System;
                if (fire == null) return;
                var incidentId = $"arc_fire_{survivorId}_{day}";
                fire.Ignite(incidentId, "room_bunker_corridor", day, new List<Ashfall.Core.Shelter.FireZoneState>());
                _journal?.TryAddRawEntry(incidentId,
                    $"A small fire started near {survivorId}'s bunk. It was put out. Nobody said much after.",
                    null!, _simDay);
            };
            system.OnBreakdownBehaviorOccurred += (survivorId, behavior) =>
            {
                if (behavior == Ashfall.Core.Survivors.ArcBehavior.RefuseAssignment)
                {
                    // Canonical needs + relations authorities carry the effect.
                    _survivors?.Needs.Modify(survivorId, NeedKind.Morale, 3f);
                    string other = FirstOtherSurvivor(survivorId);
                    if (other != null && _survivorRelationsCore != null)
                        _survivorRelationsCore.ModifyAffinity(survivorId, other, -5f);
                }
                else if (behavior == Ashfall.Core.Survivors.ArcBehavior.WithdrawSelfCare)
                {
                    // Plan 24B (A1): the self-care lapse routes through the
                    // shared attributed seam — same +8 hygiene (higher =
                    // worse), now named in the contributor display.
                    _survivors?.Needs.ApplyAttributedDelta(
                        survivorId, NeedKind.Hygiene, 8f,
                        "hygiene.self_care_withdrawn");
                }
            };
            system.OnStashDiscovered += survivorId =>
                _journal?.TryAddRawEntry($"stash_found_{survivorId}",
                    $"A hidden stash was found in {survivorId}'s things.",
                    null!, _simDay);
        }

        private string FirstOtherSurvivor(string survivorId)
        {
            if (_survivors?.Needs == null) return null!;
            foreach (var s in _survivors.Needs.Registered)
                if (s != null && !string.Equals(s.Id, survivorId, StringComparison.Ordinal)) return s.Id;
            return null!;
        }

        private void SavePsychologyArcs()
        {
            if (_psychologyArcs == null) return;
            var payload = PsychologyArcSaveStore.TryCapturePersisted(_psychologyArcs.System.CaptureState());
            if (!string.IsNullOrEmpty(payload))
            {
                CaptureSection(PsychologyArcSaveStore.SectionName, payload);
                _psychologyArcsDirty = false;
            }
        }

        /// <summary>Phase-4 day tick — after needs (phase 3) are finalized.</summary>
        private void TickPsychologyArcsDay(int day)
        {
            SetupPsychologyArcs();
            if (_survivors?.Needs == null) return;
            var ids = new List<string>();
            foreach (var s in _survivors.Needs.Registered)
                if (s != null) ids.Add(s.Id);

            float ReadStress(string id)
            {
                var s = _survivors.Needs.Get(id);
                return s?.Morale ?? 0f; // canonical stress proxy (higher = worse)
            }

            var triggerRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.PsychologyArcTrigger, day, 0)
                : new SeededRng(1640 + day);
            var behaviorRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.PsychologyArcBehavior, day, 0)
                : new SeededRng(1641 + day);
            var recoveryRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.PsychologyRecovery, day, 0)
                : new SeededRng(1642 + day);
            _psychologyArcs.System.TickDay(day, ids, ReadStress, triggerRng, behaviorRng, recoveryRng);
            _psychologyArcPanel?.RefreshView();
        }

        private void HandlePsychologyAction(string action, string param)
        {
            if (action == "OPEN")
            {
                SetupPsychologyArcs();
                if (!_psychologyArcPanelBound && _psychologyArcs != null)
                {
                    _psychologyArcPanel.Bind(_psychologyArcs, () => _survivors?.Needs);
                    _psychologyArcPanelBound = true;
                }
                _psychologyArcPanel.Open();
                return;
            }
            if (action == "CLOSE")
            {
                _psychologyArcPanel.Close();
                return;
            }

            if (_psychologyArcs == null) return;
            SetupInventory();
            var sys = _psychologyArcs.System;

            switch (action)
            {
                case "SEARCH_STASH":
                {
                    if (string.IsNullOrEmpty(param)) break;
                    var ledger = sys.DiscoverStash(param);
                    if (ledger.Count == 0)
                    {
                        _psychologyArcs.MarkDirty($"No stash found for {param}.");
                        break;
                    }
                    _psychologyArcs.MarkDirty($"{param}'s stash discovered: {ledger.Count} entr(ies).");
                    break;
                }
                case "RETURN_STASH":
                {
                    if (string.IsNullOrEmpty(param)) break;
                    var ledger = sys.StashOf(param);
                    if (ledger.Count == 0)
                    {
                        _psychologyArcs.MarkDirty($"Nothing to return for {param}.");
                        break;
                    }
                    // Atomic return: produce every ledgered item, then clear.
                    bool all = true;
                    foreach (var e in ledger)
                        if (!_inventory.Inventory.AddById(e.item_id, e.count)) all = false;
                    if (all)
                    {
                        sys.ConfirmStashReturned(param);
                        _psychologyArcs.MarkDirty($"{param}'s stash returned to stores.");
                    }
                    else _psychologyArcs.MarkDirty("Return blocked — stores cannot take the items.");
                    break;
                }
            }
            _psychologyArcPanel?.RefreshView();
        }

    }
}
