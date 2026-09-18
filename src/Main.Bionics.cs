// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 177 — Bionics host wire
// System       : BionicsSystem (implant components over the limb authority)
// Authority    : Core owns eligibility/surgery/maintenance/power/typed
//                disruption; the host feeds the real power-grid charger state,
//                forks the day-keyed malfunction roll, journals typed events,
//                routes the Plan 176 electrostatic anomaly contract, and
//                persists state. The limb truth stays in AmputationSystem.
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private BionicsSystem? _bionics;

        // ── Plan 177: Bionics & Cybernetic Prosthetics ──────────────────

        public BionicsSystem EnsureBionics()
        {
            if (_bionics != null) return _bionics;

            // The limb/socket authority must exist first (single body model).
            EnsureAmputation();

            var defs = new List<ImplantDefinition>();
            string catalogPath = CatalogPath.ResolveCatalog("bionics.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string _bionicsCatalogJson = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var root = System.Text.Json.JsonSerializer.Deserialize<BionicsCatalogRoot>(_bionicsCatalogJson);
                        if (root?.implants != null)
                        {
                            var load = new BionicsCatalogLoadResult();
                            foreach (var def in root.implants)
                                if (def != null) load.Implants.Add(def);
                            defs.AddRange(BionicsCatalogLoader.ToDefinitions(load));
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Bionics] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            _bionics = new BionicsSystem(_amputation!, defs);

            // Canonical inventory port (single item-quantity authority).
            var inv = _inventory?.Inventory;
            if (inv != null)
            {
                _bionics.BindInventory(
                    id => inv.CountById(id),
                    (id, amount) => inv.RemoveById(id, amount));
            }

            // §6.10 — charger truth from the real power grid: the quarantine
            // ward is the implant-charging station; no powered ward, no charge.
            // C2[6] 23A: the previous gate read `room_ward_clinical`, which is
            // not a row in power_grid.json and is never registered as a dynamic
            // load, so IsRoomPowered returned false permanently and the charger
            // was dead. Use the canonical quarantine ward (same room the disease
            // isolation check uses).
            _bionics.ChargerAvailable = () =>
                _powerGrid?.System != null
                && !_powerGrid.System.IsBrownout
                && _powerGrid.System.IsRoomPowered("room_ward_quarantine");

            var saved = BionicsSaveStore.TryLoad();
            if (saved != null)
            {
                _bionics.RestoreState(saved);
            }

            _bionics.OnImplantInstalled += (instance, def) =>
            {
                _journal?.TryAddRawEntry($"implant_installed_{instance.instance_id}",
                    $"Surgery complete: {def.display_name} installed on {instance.survivor_id}. Integration begins.",
                    null!, _simDay);
            };
            _bionics.OnImplantComplication += (instance, complication) =>
            {
                _journal?.TryAddRawEntry($"implant_complication_{instance.instance_id}",
                    $"Post-surgical complication ({complication.ToString().ToLowerInvariant()}) on {instance.survivor_id}'s implant.",
                    null!, _simDay);
            };
            _bionics.OnIntegrationCompleted += instance =>
            {
                _journal?.TryAddRawEntry($"implant_integrated_{instance.instance_id}",
                    $"{instance.survivor_id}'s implant has fully integrated.",
                    null!, _simDay);
            };
            _bionics.OnImplantConditionChanged += instance =>
            {
                if (instance.condition <= 40f)
                    _journal?.TryAddRawEntry($"implant_worn_{instance.instance_id}",
                        $"{instance.survivor_id}'s implant is worn ({instance.condition:F0}%) — maintenance due.",
                        null!, _simDay);
            };
            _bionics.OnImplantMalfunctioned += (instance, malfunction) =>
            {
                _journal?.TryAddRawEntry($"implant_malfunction_{instance.instance_id}",
                    $"{instance.survivor_id}'s implant malfunctioned: {malfunction.ToString().ToLowerInvariant()}.",
                    null!, _simDay);
            };
            _bionics.OnImplantRemoved += instance =>
            {
                _journal?.TryAddRawEntry($"implant_removed_{instance.instance_id}",
                    $"{instance.survivor_id}'s implant was surgically removed.",
                    null!, _simDay);
            };
            _bionics.OnImplantDestroyed += instance =>
            {
                _journal?.TryAddRawEntry($"implant_destroyed_{instance.instance_id}",
                    $"{instance.survivor_id}'s implant has failed beyond repair. The socket is bare again.",
                    null!, _simDay);
            };

            return _bionics;
        }

        private void SetupBionics()
        {
            EnsureBionics();
        }

        private void SaveBionics()
        {
            if (_bionics != null)
            {
                CaptureSection("bionics", BionicsSaveStore.TryCapturePersisted(_bionics.CaptureState()));
            }
        }

        /// <summary>
        /// Daily bionics tick (Plan 177). Deterministic: day-keyed malfunction
        /// fork, Core TickDay, then the Plan 176 → 177 cross-contract: an
        /// electrostatic anomaly field over the shelter applies TYPED component
        /// disruption (never health damage) once per day, per survivor.
        /// </summary>
        public void TickBionicsDay(int day)
        {
            if (_bionics == null) return;

            // Day-keyed malfunction fork (Core stores no RNG state).
            ISeededRng malfunctionRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Bionics, day, 0)
                : new SeededRng(unchecked(177 * 397 + day));
            _bionics.MalfunctionRoll = () => malfunctionRng.NextDouble();

            _bionics.TickDay(day);

            // ── Plan 176 → 177 contract (§8.2) ──────────────────────────
            // An electrostatic anomaly field over the shelter disrupts powered
            // implants (typed: battery drain, stun, lock, condition damage).
            // The field truth stays with the anomaly authority; this layer only
            // CONSUMES its effect tags. Biological health is never touched.
            if (_anomalyHazard != null && !_bionicsDisruptionAlreadyRan(day))
            {
                var tags = _anomalyHazard.GetEnvironmentalEffectTags(0f, 0f); // shelter zone
                if (tags.Contains("electrostatic_discharge"))
                {
                    var needs = _survivors?.Needs;
                    if (needs != null)
                    {
                        foreach (var s in needs.Registered)
                        {
                            if (s == null || !s.IsAliveState) continue;
                            if (_bionics.ImplantsFor(s.Id).Count == 0) continue;
                            var outcomes = _bionics.ApplyElectricalDisruption(s.Id, severity: 0.5f);
                            if (outcomes.Count > 0)
                            {
                                _journal?.TryAddRawEntry($"implant_disruption_{s.Id}_{day}",
                                    $"An electrostatic field washes over the shelter — {s.Id}'s implant discharges and stutters.",
                                    null!, _simDay);
                            }
                        }
                    }
                }
            }
        }

        private int _lastBionicsDisruptionDay = -1;
        /// <summary>Once-per-day guard for the electrostatic disruption sweep
        /// (idempotent across the tick path, never suppresses Core decay).</summary>
        private bool _bionicsDisruptionAlreadyRan(int day)
        {
            if (_lastBionicsDisruptionDay == day) return true;
            _lastBionicsDisruptionDay = day;
            return false;
        }

        /// <summary>Host command: combat damage handoff for the combat seam
        /// (typed; destroys the implant and reverts the limb when fatal).</summary>
        public ImplantDisruptionOutcome BionicsCombatDamage(string survivorId, Ashfall.Core.Medical.LimbId limb, float severity)
        {
            if (_bionics == null) throw new InvalidOperationException("bionics system unbound");
            return _bionics.ApplyCombatDamage(survivorId, limb, severity);
        }

        /// <summary>Host command: surgical removal.</summary>
        public bool BionicsRemove(string survivorId, Ashfall.Core.Medical.LimbId limb)
        {
            return _bionics?.RemoveImplant(survivorId, limb) ?? false;
        }
    }
}
