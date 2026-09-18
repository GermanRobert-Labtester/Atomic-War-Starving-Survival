// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 198-201 Host Wire & Orchestration
// Subsystems   : CBRN Hazard Warfare, Comms Array, Wasteland Ceremonies, Robotics
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ChemWarfareSystem? _chemWarfare;
        private CommsArraySystem? _commsArray;
        private CeremonySystem? _ceremonySystem;
        private RoboticsSystem? _robotics;

        private bool _chemWarfareDirty;
        private bool _commsArrayDirty;
        private bool _ceremonyDirty;
        private bool _roboticsDirty;

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

        // ── Plan 199: Communications Arrays & Distant Contact ───────────

        public CommsArraySystem EnsureCommsArray()
        {
            if (_commsArray != null) return _commsArray;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("comms_array") : new SeededRng(199);
            _commsArray = new CommsArraySystem(rng, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("comms_targets.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _commsArray.LoadCatalog(json);
                }
            }

            var saved = CommsArraySaveStore.TryLoad();
            if (saved != null)
            {
                _commsArray.RestoreState(saved);
            }

            _commsArray.OnContactEstablished += (target, lockState) =>
            {
                _journal?.TryAddRawEntry("comms_contact_locked", $"Long-range carrier lock established: {target.DisplayName} ({target.FrequencyKhz} kHz)", null!, _simDay);
            };

            _commsArray.OnStrategicStrikeRequested += (targetId, code) =>
            {
                _journal?.TryAddRawEntry("strategic_strike_uplink", $"CRITICAL: Strategic orbital uplink transmission authorized! Target: {targetId} [AUTH: {code}]", null!, _simDay);
            };

            _commsArray.OnStateChanged += () => _commsArrayDirty = true;
            return _commsArray;
        }

        private void SetupCommsArray()
        {
            EnsureCommsArray();
        }

        private void SaveCommsArray()
        {
            if (_commsArray != null)
            {
                CaptureSection("comms_array", CommsArraySaveStore.TryCapturePersisted(_commsArray.CaptureState()));
                _commsArrayDirty = false;
            }
        }

        // ── Plan 200: Wasteland Festivals & Ceremonies ───────────────────

        public CeremonySystem EnsureCeremonySystem()
        {
            if (_ceremonySystem != null) return _ceremonySystem;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("ceremony_system") : new SeededRng(200);
            _ceremonySystem = new CeremonySystem(rng, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("ceremonies.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _ceremonySystem.LoadCatalog(json);
                }
            }

            var saved = CeremonySaveStore.TryLoad();
            if (saved != null)
            {
                _ceremonySystem.RestoreState(saved);
            }

            _ceremonySystem.OnTruceRequested += (factionId, days) =>
            {
                _journal?.TryAddRawEntry("ceremony_truce_declared", $"Festival truce negotiated with {factionId} for {days} days.", null!, _simDay);
            };

            _ceremonySystem.OnCeremonyDisaster += (ceremonyId, disasterId) =>
            {
                _journal?.TryAddRawEntry("ceremony_disaster_event", $"Incident during festival celebration: {disasterId}!", null!, _simDay);
            };

            _ceremonySystem.OnStateChanged += () => _ceremonyDirty = true;
            return _ceremonySystem;
        }

        private void SetupCeremony()
        {
            EnsureCeremonySystem();
        }

        private void SaveCeremony()
        {
            if (_ceremonySystem != null)
            {
                CaptureSection("ceremony", CeremonySaveStore.TryCapturePersisted(_ceremonySystem.CaptureState()));
                _ceremonyDirty = false;
            }
        }

        // ── Plan 201: Advanced Robotics & Pre-War AI ─────────────────────

        public RoboticsSystem EnsureRobotics()
        {
            if (_robotics != null) return _robotics;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("robotics") : new SeededRng(201);
            _robotics = new RoboticsSystem(rng, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("robotics.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _robotics.LoadCatalog(json);
                }
            }

            var saved = RoboticsSaveStore.TryLoad();
            if (saved != null)
            {
                _robotics.RestoreState(saved);
            }

            _robotics.OnRogueEventTriggered += (unit) =>
            {
                _journal?.TryAddRawEntry("robot_rogue_event", $"WARNING: Automaton unit {unit.UnitId} logic core corrupted! Unit is unresponsive and rogue.", null!, _simDay);
            };

            _robotics.OnStateChanged += () => _roboticsDirty = true;
            return _robotics;
        }

        private void SetupRobotics()
        {
            EnsureRobotics();
        }

        private void SaveRobotics()
        {
            if (_robotics != null)
            {
                CaptureSection("robotics", RoboticsSaveStore.TryCapturePersisted(_robotics.CaptureState()));
                _roboticsDirty = false;
            }
        }

        // ── Daily Tick Orchestration for Plans 198-201 ──────────────────

        private void TickPlans198_201(int day, List<DayStateChangeEvent> events)
        {
            float gridWatts = _powerGrid?.System != null ? _powerGrid.System.GenerationWatts : 1000f;
            // C2[6] 23A: the comms array is the radio-room load; use the
            // allocation-aware served state so a brownout that still serves
            // room_radio_tuner keeps the array online.
            bool gridPowered = _powerGrid?.System == null
                || _powerGrid.System.IsRoomServed("room_radio_tuner");

            if (_chemWarfare != null)
            {
                var weather = _world != null ? _world.Weather.Current : WeatherKind.Clear;
                _chemWarfare.TickCombat(weather, 0, 1);
                _chemWarfareDirty = true;
            }

            if (_commsArray != null)
            {
                _commsArray.SetPowerState(gridPowered, gridWatts);
                _commsArray.TickScan(day, 12, 0.5f);
                _commsArrayDirty = true;
            }

            if (_ceremonySystem != null)
            {
                _ceremonySystem.TickDay(day, out _);
                _ceremonyDirty = true;
            }

            if (_robotics != null)
            {
                _robotics.TickLabor(24, gridPowered, gridWatts);
                _roboticsDirty = true;
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

        // ── Plan 199: comms array console commands ──────────────────────────

        private void HandleCommsArrayAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseCommsArrayTransceiverPanel(); return; }
            if (_commsArrayTransceiverPanel == null || _commsArray == null) return;

            switch (action)
            {
                case "tune":
                {
                    if (_commsArray.TargetCatalog.TryGetValue(param, out var target))
                    {
                        _commsArray.TuneFrequency(target.FrequencyKhz, target.Band);
                        _commsArrayTransceiverPanel.ShowFeedback(
                            $"Carrier moved to {target.FrequencyKhz} kHz ({target.Band}). Scanning.", false);
                    }
                    break;
                }
                case "upgrade_tier":
                {
                    // Strategic investment: each tier costs rare electronics,
                    // paid atomically from the canonical inventory authority.
                    var bill = new InventoryBill();
                    bill.AddCost("scrap_electronic", 5);
                    if (TryPayBill(bill))
                    {
                        _commsArray.SetArrayTier(_commsArray.State.ArrayTier + 1);
                        _commsArrayTransceiverPanel.ShowFeedback(
                            $"Array raised to tier {_commsArray.State.ArrayTier}. The antenna hears farther now.", false);
                    }
                    else
                    {
                        _commsArrayTransceiverPanel.ShowFeedback(
                            "Upgrade needs 5 salvaged electronics.", true);
                    }
                    break;
                }
                case "request_strike":
                {
                    // Endgame fictional capability: the intercepted code is
                    // authoritative and single-use; the Core request path owns
                    // every gate (tier, power, strategic target, code).
                    var lockState = _commsArray.GetOrCreateLock(param);
                    if (_commsArray.RequestStrategicStrike(param, lockState.InterceptedData, out string error))
                    {
                        _commsArrayTransceiverPanel.ShowFeedback(
                            "Uplink accepted. The request has left our hands.", false);
                    }
                    else
                    {
                        _commsArrayTransceiverPanel.ShowFeedback(error, true);
                    }
                    break;
                }
            }
            _commsArrayTransceiverPanel.RefreshView();
        }

        // ── Plan 200: ceremony console commands ───────────────────────────

        private void HandleCeremonyAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseCeremonyFestivalPanel(); return; }
            if (_ceremonyFestivalPanel == null || _ceremonySystem == null) return;

            switch (action)
            {
                case "schedule":
                {
                    int population = _survivors?.RosterState.Count ?? 0;
                    if (_ceremonySystem.ScheduleCeremony(param, _simDay, population, out string error))
                    {
                        _ceremonyFestivalPanel.ShowFeedback("Preparation begins. The feast stores must be filled before the day comes.", false);
                    }
                    else
                    {
                        _ceremonyFestivalPanel.ShowFeedback(error, true);
                    }
                    break;
                }
                case "contribute":
                {
                    // param: itemId:quantity — atomic pay-then-commit.
                    var parts = param.Split(':');
                    if (parts.Length != 2 || !int.TryParse(parts[1], out int qty) || qty <= 0)
                        break;
                    var bill = new InventoryBill();
                    bill.AddCost(parts[0], qty);
                    if (TryPayBill(bill))
                    {
                        if (!_ceremonySystem.ContributeResource(parts[0], qty))
                        {
                            RefundBill(bill);
                            _ceremonyFestivalPanel.ShowFeedback("The feast stores would not take that.", true);
                        }
                        else
                        {
                            _ceremonyFestivalPanel.ShowFeedback("Stores committed to the ceremony.", false);
                        }
                    }
                    else
                    {
                        _ceremonyFestivalPanel.ShowFeedback("Not enough in storage for that commitment.", true);
                    }
                    break;
                }
                case "invite":
                {
                    float trust = EnsureSharedFactionStance().GetTrust(param);
                    if (_ceremonySystem.InviteFaction(param, (int)trust))
                    {
                        bool accepted = trust >= -10f;
                        _ceremonyFestivalPanel.ShowFeedback(
                            accepted ? $"{UI.FactionDisplay.Name(param)} has accepted the invitation."
                                     : $"An envoy was sent to {UI.FactionDisplay.Name(param)}.",
                            !accepted);
                    }
                    else
                    {
                        _ceremonyFestivalPanel.ShowFeedback("That faction cannot be invited again.", true);
                    }
                    break;
                }
            }
            _ceremonyFestivalPanel.RefreshView();
        }

        // ── Plan 201: robotics workshop commands ─────────────────────────

        private void HandleRoboticsAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseRoboticsWorkshopPanel(); return; }
            if (_roboticsWorkshopPanel == null || _robotics == null) return;

            switch (action)
            {
                case "reactivate":
                {
                    // Atomic material payment before the core raises the unit.
                    if (!_robotics.RobotCatalog.TryGetValue(param, out var def))
                        break;
                    var bill = new InventoryBill();
                    foreach (var m in def.ReactivationMaterials)
                        bill.AddCost(m.ItemId, m.Quantity);
                    if (TryPayBill(bill))
                    {
                        var unit = _robotics.ReactivateRobot(param, 0.5f, out string error);
                        if (unit != null)
                        {
                            _roboticsWorkshopPanel.ShowFeedback($"{def.DisplayName} powers on. Servos remember their work.", false);
                        }
                        else
                        {
                            RefundBill(bill);
                            _roboticsWorkshopPanel.ShowFeedback(error, true);
                        }
                    }
                    else
                    {
                        _roboticsWorkshopPanel.ShowFeedback("Reactivation parts missing from storage.", true);
                    }
                    break;
                }
                case "program":
                {
                    // param: unitId:directiveId
                    var parts = param.Split(':');
                    if (parts.Length != 2) break;
                    if (_robotics.ProgramDirective(parts[0], parts[1], 0.5f, out string error))
                    {
                        _roboticsWorkshopPanel.ShowFeedback("Directive accepted.", false);
                    }
                    else
                    {
                        _roboticsWorkshopPanel.ShowFeedback(error, true);
                    }
                    break;
                }
                case "repair":
                {
                    var bill = new InventoryBill();
                    bill.AddCost("scrap_metal", 2);
                    if (TryPayBill(bill))
                    {
                        if (!_robotics.RepairRobot(param, 250))
                        {
                            RefundBill(bill);
                            _roboticsWorkshopPanel.ShowFeedback("That unit cannot take repair now.", true);
                        }
                        else
                        {
                            _roboticsWorkshopPanel.ShowFeedback("Chassis patched — 250 integrity restored.", false);
                        }
                    }
                    else
                    {
                        _roboticsWorkshopPanel.ShowFeedback("Repair needs 2 scrap metal.", true);
                    }
                    break;
                }
            }
            _roboticsWorkshopPanel.RefreshView();
        }

        /// <summary>Atomically consumes one bill through the canonical
        /// inventory authority. Returns false (nothing consumed) if unaffordable.</summary>
        private bool TryPayBill(InventoryBill bill)
        {
            if (_inventory?.Inventory == null) return false;
            using var tx = _inventory.Inventory.BeginTransaction(bill);
            if (!tx.Validation.IsValid) return false;
            return tx.TryCommit();
        }

        /// <summary>Refund path for a committed bill whose Core command was
        /// rejected afterwards — restores the same items.</summary>
        private void RefundBill(InventoryBill bill)
        {
            if (_inventory?.Inventory == null) return;
            foreach (var cost in bill.Costs)
                _inventory.Inventory.TryProduce(cost.ItemId, cost.Amount);
        }
    }
}
