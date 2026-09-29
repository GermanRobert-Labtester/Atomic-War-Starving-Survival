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
        // Plan 163 — DefenseSystem (traps + pre-combat raid resolution)
        // ==================================================================

        private DefenseHostSession _defense = null!;
        private bool _defenseDirty;
        private UI.DefenseGridPanel _defenseGridPanel = null!;
        private bool _defenseGridPanelBound;

        private void SetupDefense()
        {
            if (_defense != null) return;
            SetupInventory();

            var traps = Ashfall.Core.Defense.TrapCatalogLoader.Load(_dataDir);
            var system = new Ashfall.Core.Defense.DefenseSystem(traps);
            var saved = DefenseSaveStore.TryLoad();
            if (saved != null)
                system.RestoreState(saved);

            _defense = new DefenseHostSession(system);
            // Plan 203: attach the perimeter authority when it already exists
            // (defense setup can run before or after perimeter setup).
            if (_perimeterDefense != null) _defense.AttachPerimeter(_perimeterDefense);
            _defense.StateChanged += () => _defenseDirty = true;

            // Capture handoff: DefenseSystem only reports the fact; the
            // PrisonerSystem is the single captive authority (plan §6.13-6.14).
            _defense.OnCaptivesToHandOff += (day, count) =>
            {
                var prisoners = EnsurePrisoners();
                int taken = 0;
                for (int i = 0; i < count; i++)
                {
                    string captiveId = $"captive_raid_{day}_{i + 1}";
                    if (prisoners.TakePrisoner(captiveId, "faction_iron_raiders", day))
                        taken++;
                }
                _journal?.TryAddRawEntry($"raid_captures_{day}",
                    taken > 0
                        ? $"{taken} raider(s) taken alive from the pit and moved to the cells."
                        : "The pit held raiders, but the cells are full — they were turned loose at the wire.",
                    null!, _simDay);
            };

            _defense.System.OnTrapSprung += (installationId, trapId) =>
                _journal?.TryAddRawEntry($"trap_sprung_{installationId}",
                    $"A {trapId.Replace("trap_", "").Replace('_', ' ')} sprang at the wire.",
                    null!, _simDay);
        }

        private void SaveDefense()
        {
            if (_defense == null) return;
            var payload = DefenseSaveStore.TryCapturePersisted(_defense.System.CaptureState());
            if (!string.IsNullOrEmpty(payload))
            {
                CaptureSection(DefenseSaveStore.SectionName, payload);
                _defenseDirty = false;
            }
        }

        /// <summary>
        /// Plan 163 pre-combat phase: static defenses (traps, then perimeter
        /// emplacements) resolve BEFORE direct survivor combat. Returns the
        /// engagement; the caller escalates only what breached.
        /// </summary>
        private Ashfall.Core.Defense.DefenseEngagementResult ResolveRaidDefenses(
            int day, int raiderStrength, bool isNight)
        {
            SetupDefense();
            var targetingRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.DefenseTargeting, day, 0)
                : new SeededRng(1630 + day);
            var captureRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.DefenseCapture, day, 0)
                : new SeededRng(1631 + day);
            // Turrets draw from the grid: emplacements are exterior hardware.
            // Plan 71: automated emplacements additionally draw through the
            // room_armory_munitions circuit — while it is shed or tripped the
            // turrets freeze even if the grid as a whole is healthy.
            // B5–B8 Phase 7: allocation-aware served state (was the global-
            // outage IsRoomPowered read) — during a brownout the armory
            // circuit stays live while generation covers it, and sheds by
            // priority otherwise. Same migration the sump pump received.
            // CORE-MECH W5: the raid resolver is the single authority (DP-CM-2). We
            // publish its result to the defense surface and the journal so the
            // player can see what their fortifications actually did — the mechanic
            // was already live; this makes it legible.
            _defense.EmplacementPoweredProvider =
                id => _powerGrid?.System?.IsRoomServed("room_armory_munitions") ?? false;

            var engagement = _defense.System.ResolvePreCombatRaid(
                day, raiderStrength, isNight,
                _perimeterDefense,
                id => (_powerGrid?.System?.IsRoomServed("room_armory_munitions") ?? false),
                targetingRng, captureRng,
                // Plan 174 — guard animals improve night detection (§5.10):
                // the authored guard rating normalized to a 0..1 fraction.
                _companions != null ? Math.Clamp(_companions.GetGuardModifierTotal() / 100f, 0f, 1f) : 0f);

            _defense.RecordEngagement(engagement);
            SetupJournal();
            _journal?.TryAddRawEntry(
                $"raid_defenses_{day}_{raiderStrength}",
                engagement.Repelled
                    ? $"Raiders ({raiderStrength}) turned back at the perimeter — {engagement.RaidersNeutralizedByTraps} down, {engagement.RaidersCaptured} captured."
                    : $"Perimeter breached — {engagement.RemainingRaiders} raiders reached the shelter ({engagement.RaidersNeutralizedByTraps} stopped, {engagement.RaidersCaptured} captured).",
                null!, day);
            return engagement;
        }

        private void HandleDefenseAction(string action, string param)
        {
            if (action == "OPEN")
            {
                SetupDefense();
                if (!_defenseGridPanelBound && _defense != null)
                {
                    _defenseGridPanel.Bind(_defense);
                    _defenseGridPanelBound = true;
                }
                _defenseGridPanel.Open();
                return;
            }
            if (action == "CLOSE")
            {
                _defenseGridPanel.Close();
                return;
            }

            if (_defense == null) return;
            SetupInventory();
            var inv = _inventory.Inventory;
            var sys = _defense.System;
            bool ok = false;
            string? feedback = null;

            // param shapes: INSTALL:<trapId>|<placement> or <installationId>
            int colon = param?.IndexOf(':') ?? -1;
            string arg = colon > 0 ? param!.Substring(colon + 1) : param ?? "";

            switch (action)
            {
                case "INSTALL": // param "trapId|placement"
                {
                    int bar = arg.IndexOf('|');
                    if (bar > 0)
                    {
                        string trapId = arg.Substring(0, bar);
                        string placement = arg.Substring(bar + 1);
                        var def = sys.FindTrapDefinition(trapId);
                        if (def == null) { feedback = "Unknown trap."; break; }
                        bool costsOk = true;
                        foreach (var cost in def.build_costs)
                            if (!inv.HasSufficient(cost.Key, cost.Value)) { costsOk = false; feedback = $"Needs {cost.Value}× {cost.Key}."; break; }
                        if (!costsOk) break;
                        ok = sys.InstallTrap(trapId, placement, (item, n) => inv.TryConsume(item, n));
                    }
                    break;
                }
                case "RESET":
                {
                    if (!sys.CanResetTrap(arg)) { feedback = "Trap must be sprung and intact."; break; }
                    var inst = sys.FindInstallation(arg);
                    var def = sys.FindTrapDefinition(inst!.trap_id);
                    bool costsOk = true;
                    foreach (var cost in def!.reset_costs)
                        if (!inv.HasSufficient(cost.Key, cost.Value)) { costsOk = false; feedback = $"Needs {cost.Value}× {cost.Key}."; break; }
                    if (!costsOk) break;
                    ok = sys.TryResetTrap(arg, (item, n) => inv.TryConsume(item, n));
                    break;
                }
                case "REPAIR":
                {
                    if (!sys.CanRepairTrap(arg)) { feedback = "Nothing to repair."; break; }
                    var inst = sys.FindInstallation(arg);
                    var def = sys.FindTrapDefinition(inst!.trap_id);
                    bool costsOk = true;
                    foreach (var cost in def!.repair_costs)
                        if (!inv.HasSufficient(cost.Key, cost.Value)) { costsOk = false; feedback = $"Needs {cost.Value}× {cost.Key}."; break; }
                    if (!costsOk) break;
                    ok = sys.TryRepairTrap(arg, (item, n) => inv.TryConsume(item, n));
                    break;
                }
                case "SIMULATE": // manual drill: resolve against a dummy squad
                {
                    var result = ResolveRaidDefenses(_simDay, int.TryParse(arg, out var s) && s > 0 ? Math.Min(s, 12) : 4, isNight: false);
                    ok = true;
                    _journal?.TryAddRawEntry($"defense_drill_{_simDay}",
                        $"Defense drill against {result.InitialRaiderStrength} simulated raiders: {(result.Repelled ? "repelled." : $"breached with {result.RemainingRaiders} through.")}",
                        null!, _simDay);
                    break;
                }
                case "BUILD": // B5–B8 Phase 7 (Plan 67 §10.5): emplacement construction
                {
                    var def = _perimeterDefense!.FindDefinition(arg);
                    if (def == null) { feedback = "Unknown emplacement."; break; }
                    // Live capability query — research is permission, never a
                    // built emplacement (§15.3). Basic fieldworks pass freely.
                    bool capability = string.IsNullOrEmpty(def.required_knowledge)
                        || (_sharedResearch?.HasCapability(def.required_knowledge) ?? false);
                    var construct = _perimeterDefense!.ConstructEmplacement(def.defense_id, capability);
                    ok = construct.IsSuccess;
                    if (!ok) feedback = construct.MessageKey;
                    break;
                }
            }

            if (!ok && feedback != null)
                _defense.MarkDirty(feedback);
            _defenseGridPanel?.RefreshView();
        }

    }
}
