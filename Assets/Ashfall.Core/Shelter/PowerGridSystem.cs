// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// ASHFALL Power Grid — Core system (item 13).
    ///
    /// Single authority for shelter electrical state. The Core owns the
    /// deterministic math (generation, draw, battery reserve, brownout
    /// effects); the host presents a panel and applies the consequences
    /// to air filtration, water, clinic, greenhouse, foundry, and lighting
    /// via adapters.
    ///
    /// State is captured/restored through <see cref="PowerGridState"/> and
    /// the envelope in <see cref="PowerGridSave"/>. Every mutation emits a
    /// typed <see cref="PowerGridEvent"/> that the host listens to.
    /// </summary>
    public sealed class PowerGridSystem
    {
        private readonly PowerGridState _state;
        private readonly List<PowerGridRoom> _rooms;
        private readonly ISeededRng _rng;
        // Runtime generation sources are projections from their owning systems.
        // They are deliberately not persisted here: the owning save section
        // restores first, then the host republishes the contribution.
        private readonly Dictionary<string, float> _generationContributions =
            new Dictionary<string, float>(StringComparer.Ordinal);

        // Phase 2 (B5–B8): runtime-only brownout edge bookkeeping. Never saved;
        // RestoreState re-seeds it from the restored state so a reload never
        // replays a brownout-began/ended transition (flagship §14.3).
        private bool _prevTickBrownout;

        // B5–B8 expansion: runtime-only source-degradation edge latches
        // (flagship §27 — source maintenance/failure events). Each fires once
        // per degradation cycle; RestoreState re-seeds them from the restored
        // state so a reload never replays a warning.
        private bool _generatorWornWarned;
        private bool _fuelStarvedWarned;

        /// <summary>Raised whenever a room's powered state changes.</summary>
        public event Action<PowerGridEvent>? OnPowerChanged;

        /// <summary>Raised at end of every tick with the day summary.</summary>
        public event Action<PowerGridTickSummary>? OnTickSummary;

        public PowerGridSystem(PowerGridState state, IEnumerable<PowerGridRoom> rooms, ISeededRng rng)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (rooms == null) throw new ArgumentNullException(nameof(rooms));
            _rooms = new List<PowerGridRoom>();
            var roomIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var room in rooms)
            {
                if (room == null || string.IsNullOrWhiteSpace(room.RoomId)) continue;
                string roomId = room.RoomId.Trim();
                if (!roomIds.Add(roomId)) continue;
                _rooms.Add(CloneRoom(room, roomId));
            }
            if (_rooms.Count == 0)
                throw new InvalidOperationException("PowerGridSystem: at least one room required.");
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _state = new PowerGridState();
            _state.RestoreInto(state, _rooms);
            RepublishEbPvdInstalledContribution();
        }

        public PowerGridState State => _state;
        public IReadOnlyList<PowerGridRoom> Rooms => _rooms;

        /// <summary>
        /// Base generator output plus current runtime contributions (kW is
        /// represented as watts at this authority). External contributions are
        /// fuel-free; the base generator remains the only fuel-burning source.
        /// </summary>
        public float GenerationWatts
        {
            get
            {
                // B5–B8 Phase 5: the base generator's rated output degrades
                // with condition (factor is 1 at/above the threshold — legacy
                // parity for a healthy generator). External contributions are
                // NOT scaled here; their owning systems own their condition.
                float total = _state.GenerationWatts * GeneratorOutputFactor;
                foreach (var contribution in _generationContributions.Values)
                    total += Math.Max(0f, contribution);
                return total;
            }
        }

        public float BaseGenerationWatts => _state.GenerationWatts;
        public IReadOnlyDictionary<string, float> GenerationContributions => _generationContributions;
        public float FuelUnits => _state.FuelUnits;
        public float BatteryReserveWh => _state.BatteryReserveWh;
        public float BatteryCapacityWh => _state.BatteryCapacityWh;
        public float TotalDrawWatts => ComputeTotalDraw();
        public float NetWatts => GenerationWatts - TotalDrawWatts;
        public bool IsBrownout => TotalDrawWatts > GenerationWatts && BatteryReserveWh <= 0;

        // ---- C2[6] 23A: canonical demand/supply/deficit read model ----------
        //
        // Panels, briefings and the cascade layer must read these instead of
        // re-deriving arithmetic. <see cref="AvailableSupplyWatts"/> is exactly the
        // number the deterministic allocator uses (generation plus the sustainable
        // battery discharge the tick can hold for a day), so a UI forecast can
        // never disagree with the simulation.

        /// <summary>Battery watts the current reserve can sustain across a 24 h day.</summary>
        public float SustainableBatteryDischargeWatts => Math.Max(0f, _state.BatteryReserveWh / 24f);

        /// <summary>Generation plus sustainable battery discharge — the served-power ceiling.</summary>
        public float AvailableSupplyWatts => GenerationWatts + SustainableBatteryDischargeWatts;

        /// <summary>Intent draw above the served-power ceiling (0 when supply covers demand).</summary>
        public float DeficitWatts => Math.Max(0f, TotalDrawWatts - AvailableSupplyWatts);

        /// <summary>
        /// Estimated hours the current battery reserve lasts against the current
        /// intent draw, using the same generation/draw numbers the daily tick
        /// exchanges. <see cref="float.PositiveInfinity"/> when generation covers
        /// demand. Never negative; never NaN.
        /// </summary>
        public float EstimatedRuntimeHours
        {
            get
            {
                float drainWatts = TotalDrawWatts - GenerationWatts;
                if (drainWatts <= 0f) return float.PositiveInfinity;
                return _state.BatteryReserveWh / drainWatts;
            }
        }

        /// <summary>
        /// Estimated days of fuel left at the base generator's current rated
        /// (condition-scaled) burn, matching the tick's fuel formula exactly.
        /// External fuel-free contributions are excluded by construction.
        /// </summary>
        public float EstimatedFuelRunwayDays
        {
            get
            {
                float baseWatts = _state.GenerationWatts * GeneratorOutputFactor;
                float burnPerDay = Math.Max(0.0001f, baseWatts * 24f * 0.001f);
                return _state.FuelUnits / burnPerDay;
            }
        }

        /// <summary>
        /// Publish one external generation source. The source ID is stable and
        /// replacing a value is idempotent, so a host can republish after every
        /// campaign tick without accumulating duplicate output.
        /// </summary>
        public bool SetGenerationContribution(string sourceId, float watts)
        {
            if (string.IsNullOrWhiteSpace(sourceId)) return false;
            if (float.IsNaN(watts) || float.IsInfinity(watts)) return false;
            float normalized = Math.Max(0f, watts);
            if (normalized <= 0f)
                _generationContributions.Remove(sourceId);
            else
                _generationContributions[sourceId] = normalized;

            OnPowerChanged?.Invoke(new PowerGridEvent(
                PowerGridEventKind.GenerationChanged,
                sourceId,
                _state.SimDay,
                normalized > 0f ? "generation_contribution_set" : "generation_contribution_removed",
                normalized));
            return true;
        }

        public bool RemoveGenerationContribution(string sourceId)
        {
            if (string.IsNullOrWhiteSpace(sourceId)) return false;
            return SetGenerationContribution(sourceId, 0f);
        }

        /// <summary>
        /// Plans 146–149 MED: stable runtime contribution id for installed
        /// EB-PVD coated generator parts. Host republishes after restore.
        /// </summary>
        public const string EbPvdInstalledSourceId = "ebpvd_installed";

        /// <summary>Hard cap on concurrent installed coated parts (blade / combustor / injector).</summary>
        public const int MaxInstalledCoatedParts = 3;

        /// <summary>Hard cap on total coated contribution watts.</summary>
        public const float MaxEbPvdInstalledWatts = 80f;

        // ---- B5–B8 Phase 2 (Plan 65): battery bank build chain -----------------

        /// <summary>Canonical install item for one battery bank. Research gates
        /// the item through the reconditioning recipe chain; research alone
        /// never grants capacity.</summary>
        public const string BatteryBankItemId = "item_battery_reconditioned";

        /// <summary>Capacity added per installed bank (Wh). Old saves with zero
        /// banks keep their stored capacity unchanged.</summary>
        public const float BatteryBankCapacityWh = 1000f;

        /// <summary>Hard cap on installed banks (bounded build chain).</summary>
        public const int MaxInstalledBatteryBanks = 4;

        // ---- B5–B8 Phase 5 (Plan 65): generator condition/maintenance --------

        /// <summary>Canonical maintenance consumable for the base generator
        /// (same item the subgrid repair and battery service use; produced by
        /// the Fischer-Tropsch lubricant chain). The host consumes it — the
        /// grid never touches inventory.</summary>
        public const string GeneratorMaintenanceItemId = "machine_oil";

        /// <summary>Condition wear per day while the generator actually burns
        /// fuel. 100 → 0 over 400 burning days; an idle generator does not
        /// wear.</summary>
        public const float GeneratorWearPerDay = 0.25f;

        /// <summary>Below this condition the generator's rated output begins
        /// to degrade (worn bearings, fouled injectors).</summary>
        public const float GeneratorDegradationThreshold = 50f;

        /// <summary>Output factor at zero condition (a half-dead engine still
        /// runs at half rating — bounded, never zero while fueled).</summary>
        public const float GeneratorMinOutputFactor = 0.5f;

        /// <summary>
        /// Current generator condition 0..100. Bounded component state
        /// (flagship §8.8): wear producer (fuel-burning days), maintenance
        /// action with a real item cost, bounded effect, save persistence.
        /// </summary>
        public float GeneratorCondition => _state.GeneratorCondition;

        /// <summary>Output multiplier the condition applies to the base
        /// generator's rated watts (external contributions are unaffected —
        /// their owning systems own their own condition).</summary>
        public float GeneratorOutputFactor => _state.GeneratorCondition >= GeneratorDegradationThreshold
            ? 1f
            : GeneratorMinOutputFactor
              + (1f - GeneratorMinOutputFactor) * (_state.GeneratorCondition / GeneratorDegradationThreshold);

        /// <summary>
        /// B5–B8 Phase 5: service the generator back to full condition.
        /// Caller consumes the canonical
        /// <see cref="GeneratorMaintenanceItemId"/> first (coated-part
        /// discipline). A service on an already-healthy generator is blocked —
        /// wasted effort is a blocked action, not a silent success.
        /// </summary>
        public bool PerformGeneratorMaintenance(out string reason)
        {
            reason = string.Empty;
            if (_state.GeneratorCondition >= 100f)
            {
                reason = "condition_full";
                return false;
            }

            _state.GeneratorCondition = 100f;
            _generatorWornWarned = false;
            OnPowerChanged?.Invoke(new PowerGridEvent(
                PowerGridEventKind.GeneratorMaintained,
                GeneratorMaintenanceItemId,
                _state.SimDay,
                "generator_serviced",
                GeneratorCondition));
            return true;
        }

        /// <summary>
        /// Install one battery bank (caller consumes the canonical
        /// <see cref="BatteryBankItemId"/> item first — same discipline as
        /// coated parts). Adds bounded capacity; never touches the current
        /// reserve, so an old save's stored energy is unchanged.
        /// </summary>
        public bool TryInstallBatteryBank(out string reason)
        {
            reason = string.Empty;
            if (_state.InstalledBatteryBankCount >= MaxInstalledBatteryBanks)
            {
                reason = "battery_bank_slots_full";
                return false;
            }

            _state.InstalledBatteryBankCount++;
            _state.BatteryCapacityWh += BatteryBankCapacityWh;
            OnPowerChanged?.Invoke(new PowerGridEvent(
                PowerGridEventKind.BatteryBankInstalled,
                BatteryBankItemId,
                _state.SimDay,
                "battery_bank_installed",
                BatteryBankCapacityWh));
            return true;
        }

        public int InstalledBatteryBankCount => _state.InstalledBatteryBankCount;

        public IReadOnlyList<string> InstalledCoatedPartItemIds => _state.InstalledCoatedPartItemIds;

        /// <summary>
        /// Install one coated generator part by inventory item id. Requires an
        /// explicit install action — minting a coating never auto-buffs the grid.
        /// One slot per family (blade / combustor / injector).
        /// </summary>
        public bool TryInstallCoatedPart(string itemId, out string reason)
        {
            reason = string.Empty;
            if (string.IsNullOrWhiteSpace(itemId))
            {
                reason = "missing_item";
                return false;
            }

            string family = ResolveCoatedPartFamily(itemId);
            if (string.IsNullOrEmpty(family))
            {
                reason = "unsupported_coated_part";
                return false;
            }

            var installed = _state.InstalledCoatedPartItemIds;
            for (int i = 0; i < installed.Count; i++)
            {
                if (string.Equals(installed[i], itemId, StringComparison.Ordinal))
                {
                    reason = "already_installed";
                    return false;
                }
                if (string.Equals(ResolveCoatedPartFamily(installed[i]), family, StringComparison.Ordinal))
                {
                    reason = "family_slot_occupied";
                    return false;
                }
            }

            if (installed.Count >= MaxInstalledCoatedParts)
            {
                reason = "install_slots_full";
                return false;
            }

            installed.Add(itemId);
            RepublishEbPvdInstalledContribution();
            OnPowerChanged?.Invoke(new PowerGridEvent(
                PowerGridEventKind.GenerationChanged,
                itemId,
                _state.SimDay,
                "coated_part_installed",
                ResolveCoatedPartWatts(itemId)));
            return true;
        }

        public bool TryUninstallCoatedPart(string itemId, out string reason)
        {
            reason = string.Empty;
            if (string.IsNullOrWhiteSpace(itemId))
            {
                reason = "missing_item";
                return false;
            }

            if (!_state.InstalledCoatedPartItemIds.Remove(itemId))
            {
                reason = "not_installed";
                return false;
            }

            RepublishEbPvdInstalledContribution();
            OnPowerChanged?.Invoke(new PowerGridEvent(
                PowerGridEventKind.GenerationChanged,
                itemId,
                _state.SimDay,
                "coated_part_uninstalled",
                0f));
            return true;
        }

        /// <summary>
        /// Idempotent republish of installed coated-part watts into the runtime
        /// contribution map. Call after restore and after install/uninstall.
        /// </summary>
        public void RepublishEbPvdInstalledContribution()
        {
            float watts = 0f;
            var installed = _state.InstalledCoatedPartItemIds;
            for (int i = 0; i < installed.Count; i++)
                watts += ResolveCoatedPartWatts(installed[i]);
            watts = Math.Clamp(watts, 0f, MaxEbPvdInstalledWatts);
            SetGenerationContribution(EbPvdInstalledSourceId, watts);
        }

        public static string ResolveCoatedPartFamily(string itemId)
        {
            if (string.Equals(itemId, "item_coated_turbine_blade", StringComparison.Ordinal))
                return "blade";
            if (string.Equals(itemId, "item_coated_combustor_tile", StringComparison.Ordinal))
                return "combustor";
            if (string.Equals(itemId, "item_coated_diesel_injector", StringComparison.Ordinal))
                return "injector";
            return string.Empty;
        }

        public static float ResolveCoatedPartWatts(string itemId)
        {
            // Bounded historical-engineering bonuses; never overwrite base GenerationWatts.
            if (string.Equals(itemId, "item_coated_turbine_blade", StringComparison.Ordinal))
                return 40f;
            if (string.Equals(itemId, "item_coated_combustor_tile", StringComparison.Ordinal))
                return 35f;
            if (string.Equals(itemId, "item_coated_diesel_injector", StringComparison.Ordinal))
                return 25f;
            return 0f;
        }

        public PowerGridSnapshot Snapshot()
        {
            var snapshot = new PowerGridSnapshot
            {
                Day = _state.SimDay,
                GenerationWatts = GenerationWatts,
                FuelUnits = FuelUnits,
                BatteryReserveWh = BatteryReserveWh,
                BatteryCapacityWh = BatteryCapacityWh,
                TotalDrawWatts = TotalDrawWatts,
                NetWatts = NetWatts,
                IsBrownout = IsBrownout,
                RoomIds = new List<string>(RoomPoweredStates())
            };
            foreach (var contribution in _generationContributions)
                snapshot.GenerationContributions[contribution.Key] = contribution.Value;
            return snapshot;
        }

        public bool IsRoomPowered(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return false;
            var r = FindRoom(roomId);
            if (r == null) return false;
            return _state.IsBreakerClosed(roomId) && !_state.IsRoomTripped(roomId) &&
                   !IsBrownout;
        }

        public PowerGridRoomPriority EffectivePriority(string roomId)
        {
            var r = FindRoom(roomId);
            if (r == null) return PowerGridRoomPriority.Disabled;
            // Phase 2 (B5–B8): player override wins; catalog DefaultPriority
            // otherwise. Previously an override-less room always read Standard,
            // silently discarding the catalog's critical/low classification.
            for (int i = 0; i < _state.Priorities.Count; i++)
            {
                if (_state.Priorities[i].RoomId == roomId)
                    return _state.Priorities[i].Priority;
            }
            return r.DefaultPriority;
        }

        /// <summary>
        /// Toggle a breaker. Returns false if the room id is unknown.
        /// Idempotent: re-closing an already-closed breaker is a no-op.
        /// </summary>
        public bool ToggleBreaker(string roomId)
        {
            var r = FindRoom(roomId);
            if (r == null) return false;
            bool wasClosed = _state.IsBreakerClosed(roomId);
            _state.SetBreaker(roomId, !wasClosed);
            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.BreakerToggled,
                roomId, _state.SimDay, wasClosed ? "closed_to_open" : "open_to_closed"));
            return true;
        }

        public bool SetBreaker(string roomId, bool closed)
        {
            var r = FindRoom(roomId);
            if (r == null) return false;
            bool wasClosed = _state.IsBreakerClosed(roomId);
            if (wasClosed == closed) return true;
            _state.SetBreaker(roomId, closed);
            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.BreakerToggled,
                roomId, _state.SimDay, closed ? "open_to_closed" : "closed_to_open"));
            return true;
        }

        public void MarkTripped(string roomId, int day)
        {
            _state.MarkTripped(roomId, day);
            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.Tripped,
                roomId, day, "manual_trip"));
        }

        public void ClearTripped(string roomId) => _state.ClearTripped(roomId);

        public bool IsRoomTripped(string roomId) => _state.IsRoomTripped(roomId);

        public bool SetPriority(string roomId, PowerGridRoomPriority priority)
        {
            var r = FindRoom(roomId);
            if (r == null || !Enum.IsDefined(typeof(PowerGridRoomPriority), priority))
                return false;
            _state.SetRoomPriority(roomId, priority);
            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.PriorityChanged,
                roomId, _state.SimDay, priority.ToString()));
            return true;
        }

        /// <summary>
        /// B5–B8 Phase 3 (Plan 66): register a dynamic load room — the
        /// power-load subscription contract (§6.3). Consumer systems (sump
        /// pump nodes now, perimeter emplacements later) expose a stable load
        /// id and nominal draw; the grid owns allocation/shedding from that
        /// point on. The consumer reads its served state via
        /// <see cref="IsRoomServed"/> and never mutates generation or
        /// brownout state.
        ///
        /// Idempotent: a RoomId already present (e.g. from the
        /// power_grid.json catalog) is left untouched — catalog rooms take
        /// precedence. Registered loads participate in draw and priority
        /// allocation immediately.
        ///
        /// Dynamic loads are re-registered by their owning system after that
        /// system's save restores (the grid's room list itself is not saved).
        /// </summary>
        public bool RegisterLoadRoom(PowerGridRoom room)
        {
            if (room == null || string.IsNullOrWhiteSpace(room.RoomId)
                || float.IsNaN(room.DrawWatts)
                || float.IsInfinity(room.DrawWatts)
                || room.DrawWatts < 0f
                || !Enum.IsDefined(typeof(PowerGridRoomPriority), room.DefaultPriority))
                return false;

            string roomId = room.RoomId.Trim();
            if (FindRoom(roomId) != null) return false;
            var cloned = CloneRoom(room, roomId);
            _rooms.Add(cloned);
            OnPowerChanged?.Invoke(new PowerGridEvent(
                PowerGridEventKind.LoadRoomRegistered,
                roomId, _state.SimDay, "load_room_registered", cloned.DrawWatts));
            return true;
        }

        /// <summary>
        /// B5–B8 expansion (§27): emergency load-shed preset — the
        /// brownout-management shortcut. Demotes every non-Critical load one
        /// tier (Standard → Low, Low stays Low); Critical life-support loads
        /// are never touched. Deterministic, typed (one PriorityChanged event
        /// per changed room), fully reversible by re-applying catalog defaults
        /// via <see cref="ApplyCatalogDefaultPriorities"/> or manual overrides.
        /// Returns the changed room ids (journal/briefing surface).
        /// </summary>
        public IReadOnlyList<string> ApplyBrownoutShedPreset()
        {
            var changed = new List<string>();
            for (int i = 0; i < _rooms.Count; i++)
            {
                var r = _rooms[i];
                var current = EffectivePriority(r.RoomId);
                var target = current switch
                {
                    PowerGridRoomPriority.Standard => PowerGridRoomPriority.Low,
                    _ => current
                };
                if (target != current)
                {
                    SetPriority(r.RoomId, target);
                    changed.Add(r.RoomId);
                }
            }
            OnPowerChanged?.Invoke(new PowerGridEvent(
                PowerGridEventKind.PriorityChanged, "__preset__", _state.SimDay,
                "brownout_shed_preset", changed.Count));
            return changed;
        }

        /// <summary>B5–B8 expansion: restore the catalog defaults — clears all
        /// player priority overrides so every room returns to its
        /// power_grid.json classification. Returns the affected room count.</summary>
        public int ApplyCatalogDefaultPriorities()
        {
            int count = _state.Priorities.Count;
            _state.Priorities.Clear();
            if (count > 0)
            {
                OnPowerChanged?.Invoke(new PowerGridEvent(
                    PowerGridEventKind.PriorityChanged, "__preset__", _state.SimDay,
                    "catalog_defaults_restored", count));
            }
            return count;
        }

        public void AddFuel(float units)
        {
            if (units <= 0f || float.IsNaN(units) || float.IsInfinity(units)) return;
            double next = (double)_state.FuelUnits + units;
            _state.FuelUnits = (float)Math.Min(next, float.MaxValue);
            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.FuelAdded, null!,
                _state.SimDay, "fuel_added", units));
        }

        /// <summary>EMP storm severity applied on weather onset — catalog-driven
        /// via power_grid.json `emp_storm_severity` (SHELTER_HARDENING); falls
        /// back to this default when the catalog omits the field.</summary>
        public const float DefaultEmpStormSurgeSeverity = 0.6f;

        /// <summary>Default fraction of battery capacity drained at full surge severity.</summary>
        public const float DefaultSurgeBatteryDrainFraction = 0.15f;

        private float _empStormSurgeSeverity = DefaultEmpStormSurgeSeverity;
        private float _surgeBatteryDrainFraction = DefaultSurgeBatteryDrainFraction;

        /// <summary>Catalog-driven surge tuning (0..1 each). Applies from the next surge.</summary>
        public void ConfigureSurge(float empStormSeverity, float batteryDrainFraction)
        {
            _empStormSurgeSeverity = float.IsNaN(empStormSeverity) || float.IsInfinity(empStormSeverity)
                ? 0f
                : Math.Clamp(empStormSeverity, 0f, 1f);
            _surgeBatteryDrainFraction = float.IsNaN(batteryDrainFraction) || float.IsInfinity(batteryDrainFraction)
                ? 0f
                : Math.Clamp(batteryDrainFraction, 0f, 1f);
        }

        public float EmpStormSeverity => _empStormSurgeSeverity;
        public float SurgeBatteryDrain => _surgeBatteryDrainFraction;

        /// <summary>
        /// Apply an external electrical surge (EMP storm onset, orbital impact).
        /// Deterministic: trips the lowest-priority non-tripped circuits first
        /// (priority tier ascending, then RoomId ordinal), draining the battery
        /// proportionally to severity. Critical rooms are exempt below 0.9
        /// severity so a surge can wound the grid without destroying it.
        /// Deduped per day: a second surge event on the same day is a no-op.
        /// The surge day persists via <see cref="PowerGridState.LastSurgeDay"/>.
        /// </summary>
        public IReadOnlyList<string> ApplySurgeDay(int day, float severity01)
        {
            if (float.IsNaN(severity01) || float.IsInfinity(severity01))
                return Array.Empty<string>();
            float severity = Math.Clamp(severity01, 0f, 1f);
            if (severity <= 0f) return Array.Empty<string>();
            if (day <= _state.LastSurgeDay) return Array.Empty<string>();

            bool criticalEligible = severity >= 0.9f;
            var candidates = new List<(string RoomId, int Tier)>();
            foreach (var r in _rooms)
            {
                if (_state.IsRoomTripped(r.RoomId)) continue;
                // Effective tier: player override wins; catalog default otherwise.
                bool hasOverride = false;
                for (int p = 0; p < _state.Priorities.Count; p++)
                {
                    if (_state.Priorities[p].RoomId == r.RoomId) { hasOverride = true; break; }
                }
                var tier = (int)(hasOverride
                    ? _state.GetRoomPriority(r.RoomId)
                    : r.DefaultPriority);
                if (tier == (int)PowerGridRoomPriority.Disabled) continue;
                if (tier == (int)PowerGridRoomPriority.Critical && !criticalEligible) continue;
                candidates.Add((r.RoomId, tier));
            }
            candidates.Sort(static (a, b) =>
            {
                int byTier = a.Tier.CompareTo(b.Tier);
                return byTier != 0 ? byTier : string.CompareOrdinal(a.RoomId, b.RoomId);
            });

            int tripCount = (int)(severity * candidates.Count);
            var tripped = new List<string>(tripCount);
            for (int i = 0; i < tripCount; i++)
            {
                _state.MarkTripped(candidates[i].RoomId, day);
                tripped.Add(candidates[i].RoomId);
            }

            float drain = (float)Math.Floor(_state.BatteryCapacityWh * _surgeBatteryDrainFraction * severity);
            _state.BatteryReserveWh = Math.Max(0f, _state.BatteryReserveWh - drain);
            _state.LastSurgeDay = day;

            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.SurgeApplied,
                tripped.Count > 0 ? string.Join(",", tripped) : "none",
                day, "surge_applied", severity));
            return tripped;
        }

        /// <summary>
        /// Tick one full day. Deterministic: given the same fuel/battery state
        /// and RNG, the result is identical across hosts and runs.
        /// </summary>
        public PowerGridTickSummary TickDay(int day, ISeededRng tickRng)
        {
            var rng = tickRng ?? _rng;
            _state.SimDay = day;
            float draw = ComputeTotalDraw();
            float gen = GenerationWatts;
            float net = gen - draw; // Wh per hour assumed; full day = 24 units
            float fuelConsumed = 0f;
            float brownoutHours = 0f;

            // Burn fuel proportional to generation.
            float externalGeneration = 0f;
            foreach (var contribution in _generationContributions.Values)
                externalGeneration += Math.Max(0f, contribution);
            float fuelNeed = Math.Max(0f, gen - externalGeneration) * 24f * 0.001f;
            if (_state.FuelUnits >= fuelNeed)
            {
                _state.FuelUnits -= fuelNeed;
                fuelConsumed = fuelNeed;
            }
            else
            {
                fuelConsumed = _state.FuelUnits;
                _state.FuelUnits = 0;
                gen *= 0.5f; // partial generation when fuel-starved.
            }

            // B5–B8 expansion: fuel-starvation is a failure edge — the first
            // dry day warns once (runtime latch; service/refuel resets it).
            if (fuelConsumed < fuelNeed && !_fuelStarvedWarned)
            {
                _fuelStarvedWarned = true;
                OnPowerChanged?.Invoke(new PowerGridEvent(
                    PowerGridEventKind.FuelStarved, null!, _state.SimDay,
                    "generator_fuel_starved", fuelConsumed));
            }
            else if (fuelConsumed >= fuelNeed)
            {
                _fuelStarvedWarned = false;
            }

            // B5–B8 expansion: the generator wears only on burning days. An
            // idle engine does not degrade.
            if (fuelConsumed > 0f)
            {
                _state.GeneratorCondition = Math.Max(0f,
                    _state.GeneratorCondition - GeneratorWearPerDay);

                // Crossing below the degradation threshold is the warn edge —
                // output is now derated; fires once per wear cycle.
                if (!_generatorWornWarned &&
                    _state.GeneratorCondition < GeneratorDegradationThreshold)
                {
                    _generatorWornWarned = true;
                    OnPowerChanged?.Invoke(new PowerGridEvent(
                        PowerGridEventKind.GeneratorWorn, GeneratorMaintenanceItemId,
                        _state.SimDay, "generator_worn", _state.GeneratorCondition));
                }
            }

            // Phase 2 (B5–B8): deterministic priority allocation projection.
            // Computed after the fuel-starvation adjustment (that is the real
            // generation this tick) and before the battery exchange (so the
            // discharge capacity reflects the start-of-tick reserve). This is
            // a projection only — the aggregate battery/fuel/brownout-hours
            // math below is untouched and stays byte-parity with legacy ticks.
            float generationUsed = gen;
            float reserveBeforeExchange = _state.BatteryReserveWh;
            var allocation = ComputeAllocation(generationUsed, draw, reserveBeforeExchange);

            if (net >= 0)
            {
                float spareWh = net * 24f;
                _state.BatteryReserveWh = Math.Min(_state.BatteryCapacityWh,
                    _state.BatteryReserveWh + spareWh);
            }
            else
            {
                float demandWh = -net * 24f;
                if (_state.BatteryReserveWh >= demandWh)
                {
                    _state.BatteryReserveWh -= demandWh;
                }
                else
                {
                    float unmet = demandWh - _state.BatteryReserveWh;
                    _state.BatteryReserveWh = 0;
                    brownoutHours = Math.Min(24f, unmet / Math.Max(1f, draw));
                }
            }

            // Random load spike (deterministic via injected rng).
            if (rng.NextDouble() < 0.05 && _state.BatteryReserveWh > 0)
            {
                float spike = (float)(rng.NextDouble() * 30.0);
                _state.BatteryReserveWh = Math.Max(0, _state.BatteryReserveWh - spike);
                brownoutHours += 0.5f;
            }

            // Trip breakers that overload for more than 4 hours of brownout.
            if (brownoutHours >= 4f)
            {
                foreach (var r in _rooms)
                {
                    if (_state.GetRoomPriority(r.RoomId) == PowerGridRoomPriority.Disabled)
                        continue;
                    if (!_state.IsBreakerClosed(r.RoomId)) continue;
                    if (rng.NextDouble() < 0.10)
                    {
                        _state.MarkTripped(r.RoomId, day);
                        OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.Tripped,
                            r.RoomId, day, "brownout_overload"));
                    }
                }
            }

            var summary = new PowerGridTickSummary
            {
                Day = day,
                FuelConsumed = fuelConsumed,
                BatteryEndWh = _state.BatteryReserveWh,
                BrownoutHours = brownoutHours,
                IsBrownout = IsBrownout,
                // Phase 2 (B5–B8) allocation projection + edge transitions.
                GenerationWatts = generationUsed,
                RequestedDrawWatts = draw,
                ServedWatts = allocation.ServedWatts,
                UnservedWatts = Math.Max(0f, draw - allocation.ServedWatts),
                HasCriticalDeficit = allocation.HasCriticalDeficit,
                ServedRoomIds = allocation.ServedRoomIds,
                ShedRoomIds = allocation.ShedRoomIds
            };
            bool brownoutNow = summary.IsBrownout;
            summary.BrownoutBegan = brownoutNow && !_prevTickBrownout;
            summary.BrownoutEnded = !brownoutNow && _prevTickBrownout;
            _prevTickBrownout = brownoutNow;
            OnTickSummary?.Invoke(summary);
            return summary;
        }

        /// <summary>Tolerance for serving a room within available capacity.</summary>
        internal const float AllocationEpsilon = 0.01f;

        /// <summary>
        /// Phase 2 (B5–B8): deterministic priority allocation projection.
        ///
        /// Eligible rooms (closed breaker, not tripped, not Disabled) are
        /// ordered by effective priority tier descending, then RoomId ordinal
        /// ascending. Available power is generation (post fuel adjustment)
        /// plus the battery discharge this tick can sustain
        /// (<c>reserve / 24h</c>, only while demand exceeds generation — the
        /// same condition under which the legacy aggregate math drains the
        /// battery). Rooms are served in order while cumulative draw fits;
        /// every remaining room is shed. Consequences:
        ///
        /// - a higher-priority room is never shed while a lower-priority room
        ///   is served (single ordered pass, shed set is always a suffix);
        /// - unserved Critical-tier rooms raise <see cref="HasCriticalDeficit"/>
        ///   — the explicit life-support emergency flag (flagship §8.6);
        /// - no RNG participates in load order.
        ///
        /// The projection does not mutate state; served/shed classification is
        /// derived from exactly the same generation/reserve numbers the legacy
        /// battery math consumes, so both stay consistent by construction.
        ///
        /// Serving is a strict priority prefix: the first room whose load does
        /// not fit is shed together with every room after it, even if a later
        /// smaller room would fit. Real grids shed whole feeders in order — a
        /// predictable suffix beats best-fit scavenging, and it keeps the
        /// flagship invariant exact: no lower-priority load is ever served
        /// while a higher-priority load is unserved. Spare capacity below the
        /// next whole room stays unused (the legacy aggregate battery drain is
        /// untouched parity behavior).
        /// </summary>
        private PowerGridAllocation ComputeAllocation(float generationWatts, float requestedDrawWatts,
            float batteryReserveWh)
        {
            var ordered = new List<(PowerGridRoom Room, int Tier)>(_rooms.Count);
            for (int i = 0; i < _rooms.Count; i++)
            {
                var r = _rooms[i];
                if (!_state.IsBreakerClosed(r.RoomId)) continue;
                if (_state.IsRoomTripped(r.RoomId)) continue;
                int tier = (int)EffectivePriority(r.RoomId);
                if (tier == (int)PowerGridRoomPriority.Disabled) continue;
                ordered.Add((r, tier));
            }
            ordered.Sort(static (a, b) =>
            {
                int byTier = b.Tier.CompareTo(a.Tier); // higher tier first
                return byTier != 0 ? byTier : string.CompareOrdinal(a.Room.RoomId, b.Room.RoomId);
            });

            float deficit = Math.Max(0f, requestedDrawWatts - generationWatts);
            float batteryDischargeWatts = deficit > 0f
                ? Math.Min(batteryReserveWh / 24f, deficit)
                : 0f;
            float availablePower = generationWatts + batteryDischargeWatts;

            var result = new PowerGridAllocation();
            float cumulative = 0f;
            bool shedding = false;
            for (int i = 0; i < ordered.Count; i++)
            {
                var room = ordered[i].Room;
                if (!shedding &&
                    cumulative + room.DrawWatts <= availablePower + AllocationEpsilon)
                {
                    result.ServedRoomIds.Add(room.RoomId);
                    cumulative += room.DrawWatts;
                }
                else
                {
                    // Strict suffix: once one room doesn't fit, everything
                    // after it sheds regardless of size (deterministic, and
                    // never serves a lower-priority room first).
                    shedding = true;
                    result.ShedRoomIds.Add(room.RoomId);
                    if (ordered[i].Tier == (int)PowerGridRoomPriority.Critical)
                        result.HasCriticalDeficit = true;
                }
            }
            result.ServedWatts = cumulative;
            return result;
        }

        /// <summary>
        /// Phase 2 (B5–B8): allocation-aware typed powered query for consumer
        /// systems (greenhouse controlled-environment, sump pump, perimeter
        /// sentries). Unlike <see cref="IsRoomPowered"/> — which treats a
        /// brownout as a global outage — this reports whether the room's load
        /// is actually served under deterministic priority allocation, so
        /// critical loads can remain powered while optional loads shed.
        /// Consumers subscribe to <see cref="OnTickSummary"/> for the tick
        /// projection; this query is for point-in-time reads.
        /// </summary>
        public bool IsRoomServed(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return false;
            var allocation = ComputeAllocation(GenerationWatts, TotalDrawWatts, _state.BatteryReserveWh);
            return allocation.ServedRoomIds.Contains(roomId);
        }

        private sealed class PowerGridAllocation
        {
            public float ServedWatts;
            public bool HasCriticalDeficit;
            public List<string> ServedRoomIds = new List<string>();
            public List<string> ShedRoomIds = new List<string>();
        }

        public PowerGridState CaptureState() => _state.Capture();

        public void RestoreState(PowerGridState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state.RestoreInto(state, _rooms);
            // Runtime contribution map is not serialized; republish installed
            // coated-part watts so GenerationWatts matches InstalledCoatedPartItemIds.
            RepublishEbPvdInstalledContribution();
            // Phase 2 (B5–B8): re-seed brownout edge bookkeeping from the
            // restored state so a reload never replays a begin/end transition.
            _prevTickBrownout = IsBrownout;
            // B5–B8 expansion: re-seed source-degradation latches from the
            // restored state (an already-worn restored generator must not
            // replay its warning).
            _generatorWornWarned = _state.GeneratorCondition < GeneratorDegradationThreshold;
            _fuelStarvedWarned = _state.FuelUnits <= 0f;
        }

        private PowerGridRoom? FindRoom(string roomId)
        {
            for (int i = 0; i < _rooms.Count; i++)
                if (_rooms[i].RoomId == roomId) return _rooms[i];
            return null;
        }

        private static PowerGridRoom CloneRoom(PowerGridRoom source, string canonicalRoomId)
        {
            return new PowerGridRoom
            {
                RoomId = canonicalRoomId,
                DisplayName = source.DisplayName ?? string.Empty,
                DrawWatts = PowerGridState.SanitizeNonNegativeFinite(source.DrawWatts),
                DefaultPriority = Enum.IsDefined(typeof(PowerGridRoomPriority), source.DefaultPriority)
                    ? source.DefaultPriority
                    : PowerGridRoomPriority.Standard,
                FailureEffectId = source.FailureEffectId ?? string.Empty
            };
        }

        /// <summary>
        /// Current draw a room presents to the distribution network: its catalog
        /// draw when powered, 0 when tripped/open/disabled (SHELTER_HARDENING —
        /// feeds the subgrid's per-node thermal/fuse model via ApplyRoomLoad).
        /// </summary>
        public float GetRoomDrawWatts(string roomId)
        {
            if (!IsRoomPowered(roomId)) return 0f;
            var r = FindRoom(roomId);
            return r?.DrawWatts ?? 0f;
        }

        private float ComputeTotalDraw()
        {
            // Phase 6: this now returns the *intent* draw — the sum of all rooms'
            // logical DrawWatts that are eligible (closed breaker, not tripped,
            // not disabled). The brownout suppression number lives on the new
            // EffectiveTotalDrawWatts property. Splitting intent from effect
            // stops IsBrownout from flipping false the same tick brownout fires:
            // a 0-returning TotalDrawWatts made NetWatts positive, which made
            // the battery charge UP during brownout (opposite of intent).
            float draw = 0f;
            for (int i = 0; i < _rooms.Count; i++)
            {
                var r = _rooms[i];
                if (!_state.IsBreakerClosed(r.RoomId)) continue;
                if (_state.IsRoomTripped(r.RoomId)) continue;
                var pri = _state.GetRoomPriority(r.RoomId);
                if (pri == PowerGridRoomPriority.Disabled) continue;
                draw += r.DrawWatts;
            }
            return draw;
        }

        /// <summary>
        /// Number that actually drains the battery during a brownout. When the
        /// grid is in brownout state, every room drops to 0 effective draw,
        /// which means the unmet-deficit (and thus the brownout-hours
        /// calculation) collapses to whatever the *previous-tick* steady-state
        /// looked like. Returns 0 during brownout to preserve the legacy
        /// TickDay brownout-hours math that downstream code relies on.
        /// </summary>
        public float EffectiveTotalDrawWatts =>
            IsBrownout ? 0f : TotalDrawWatts;

        private List<string> RoomPoweredStates()
        {
            var list = new List<string>(_rooms.Count);
            for (int i = 0; i < _rooms.Count; i++)
            {
                var r = _rooms[i];
                bool powered = IsRoomPowered(r.RoomId);
                list.Add(r.RoomId + "=" + (powered ? "on" : "off"));
            }
            return list;
        }
    }

    /// <summary>Priority used by the grid when total draw exceeds generation.</summary>
    public enum PowerGridRoomPriority
    {
        Disabled = 0,
        Low = 1,
        Standard = 2,
        Critical = 3
    }

    [Serializable]
    public sealed class PowerGridRoom
    {
        public string RoomId;
        public string DisplayName;
        public float DrawWatts;
        public PowerGridRoomPriority DefaultPriority;
        public string FailureEffectId; // semantic id the host looks up.

        public PowerGridRoom() { }

        public PowerGridRoom(string roomId, string displayName, float drawWatts,
            PowerGridRoomPriority defaultPriority = PowerGridRoomPriority.Standard,
string? failureEffectId = null)
        {
            RoomId = roomId;
            DisplayName = displayName;
            DrawWatts = drawWatts;
            DefaultPriority = defaultPriority;
            FailureEffectId = failureEffectId;
        }
    }

    [Serializable]
    public sealed class PowerGridState
    {
        public int SimDay;
        public float GenerationWatts;
        public float FuelUnits;
        public float BatteryReserveWh;
        public float BatteryCapacityWh;
        public List<string> ClosedBreakers = new List<string>();
        public List<string> TrippedRooms = new List<string>();
        public List<RoomPriorityRecord> Priorities = new List<RoomPriorityRecord>();

        /// <summary>
        /// Day of the last applied surge (EMP/orbital). Optional field: saves
        /// written before the field existed restore as 0 ("no surge yet") per
        /// the repository's optional-field migration tolerance.
        /// </summary>
        public int LastSurgeDay;

        /// <summary>
        /// Plans 146–149 MED: inventory item ids of coated parts installed into
        /// the generator. Optional — old saves restore empty (no install).
        /// Runtime watts are republished via <see cref="PowerGridSystem.RepublishEbPvdInstalledContribution"/>.
        /// </summary>
        public List<string> InstalledCoatedPartItemIds = new List<string>();

        /// <summary>
        /// B5–B8 Phase 2: installed battery-bank count (additive — old saves
        /// restore 0 and keep their stored capacity exactly).
        /// </summary>
        public int InstalledBatteryBankCount;

        /// <summary>
        /// B5–B8 Phase 5: base generator condition 0..100. Field initializer
        /// 100 is the migration contract: legacy saves lacking the field
        /// restore a healthy generator (no retroactive degradation); new saves
        /// persist the value explicitly.
        /// </summary>
        public float GeneratorCondition = 100f;

        public bool IsBreakerClosed(string roomId) => !ClosedBreakers.Contains(roomId);
        public bool IsRoomTripped(string roomId) => TrippedRooms.Contains(roomId);

        public void SetBreaker(string roomId, bool closed)
        {
            if (closed) ClosedBreakers.Remove(roomId);
            else if (!ClosedBreakers.Contains(roomId)) ClosedBreakers.Add(roomId);
        }

        public void MarkTripped(string roomId, int day)
        {
            if (!TrippedRooms.Contains(roomId)) TrippedRooms.Add(roomId);
        }

        public void ClearTripped(string roomId) => TrippedRooms.Remove(roomId);

        public PowerGridRoomPriority GetRoomPriority(string roomId)
        {
            for (int i = 0; i < Priorities.Count; i++)
                if (Priorities[i].RoomId == roomId) return Priorities[i].Priority;
            return PowerGridRoomPriority.Standard;
        }

        public void SetRoomPriority(string roomId, PowerGridRoomPriority priority)
        {
            for (int i = 0; i < Priorities.Count; i++)
            {
                if (Priorities[i].RoomId == roomId)
                {
                    Priorities[i].Priority = priority;
                    return;
                }
            }
            Priorities.Add(new RoomPriorityRecord { RoomId = roomId, Priority = priority });
        }

        public void NormalizeAndValidate(IReadOnlyList<PowerGridRoom> rooms)
        {
            ClosedBreakers ??= new List<string>();
            TrippedRooms ??= new List<string>();
            Priorities ??= new List<RoomPriorityRecord>();
            InstalledCoatedPartItemIds ??= new List<string>();

            GenerationWatts = SanitizeNonNegativeFinite(GenerationWatts);
            FuelUnits = SanitizeNonNegativeFinite(FuelUnits);
            BatteryCapacityWh = SanitizeNonNegativeFinite(BatteryCapacityWh);
            BatteryReserveWh = SanitizeNonNegativeFinite(BatteryReserveWh);
            if (BatteryReserveWh > BatteryCapacityWh) BatteryReserveWh = BatteryCapacityWh;
            InstalledBatteryBankCount = Math.Clamp(
                InstalledBatteryBankCount,
                0,
                PowerGridSystem.MaxInstalledBatteryBanks);
            GeneratorCondition = IsFinite(GeneratorCondition)
                ? Math.Clamp(GeneratorCondition, 0f, 100f)
                : 0f;
            if (SimDay < 0) SimDay = 0;
            if (LastSurgeDay < 0) LastSurgeDay = 0;

            var validIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < rooms.Count; i++)
                validIds.Add(rooms[i].RoomId);

            NormalizeIdList(ClosedBreakers, validIds);
            NormalizeIdList(TrippedRooms, validIds);
            NormalizePriorities(validIds);
            NormalizeInstalledCoatedParts();
        }

        public PowerGridState Capture()
        {
            var copy = new PowerGridState
            {
                SimDay = Math.Max(0, SimDay),
                GenerationWatts = SanitizeNonNegativeFinite(GenerationWatts),
                FuelUnits = SanitizeNonNegativeFinite(FuelUnits),
                BatteryReserveWh = SanitizeNonNegativeFinite(BatteryReserveWh),
                BatteryCapacityWh = SanitizeNonNegativeFinite(BatteryCapacityWh),
                ClosedBreakers = new List<string>(),
                TrippedRooms = new List<string>(),
                Priorities = new List<RoomPriorityRecord>(),
                LastSurgeDay = Math.Max(0, LastSurgeDay),
                InstalledBatteryBankCount = Math.Clamp(
                    InstalledBatteryBankCount,
                    0,
                    PowerGridSystem.MaxInstalledBatteryBanks),
                GeneratorCondition = IsFinite(GeneratorCondition)
                    ? Math.Clamp(GeneratorCondition, 0f, 100f)
                    : 0f,
                InstalledCoatedPartItemIds = new List<string>()
            };
            if (copy.BatteryReserveWh > copy.BatteryCapacityWh)
                copy.BatteryReserveWh = copy.BatteryCapacityWh;

            if (ClosedBreakers != null)
            {
                foreach (var roomId in ClosedBreakers)
                    if (!string.IsNullOrWhiteSpace(roomId)) copy.ClosedBreakers.Add(roomId);
            }
            if (TrippedRooms != null)
            {
                foreach (var roomId in TrippedRooms)
                    if (!string.IsNullOrWhiteSpace(roomId)) copy.TrippedRooms.Add(roomId);
            }
            if (Priorities != null)
            {
                foreach (var priority in Priorities)
                {
                    if (priority == null || string.IsNullOrWhiteSpace(priority.RoomId)) continue;
                    copy.Priorities.Add(new RoomPriorityRecord
                    {
                        RoomId = priority.RoomId,
                        Priority = priority.Priority
                    });
                }
            }
            if (InstalledCoatedPartItemIds != null)
            {
                foreach (var itemId in InstalledCoatedPartItemIds)
                    if (!string.IsNullOrWhiteSpace(itemId)) copy.InstalledCoatedPartItemIds.Add(itemId);
            }
            return copy;
        }

        public void RestoreInto(PowerGridState state, IReadOnlyList<PowerGridRoom> rooms)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            var restored = state.Capture();
            SimDay = restored.SimDay;
            GenerationWatts = restored.GenerationWatts;
            FuelUnits = restored.FuelUnits;
            BatteryReserveWh = restored.BatteryReserveWh;
            BatteryCapacityWh = restored.BatteryCapacityWh;
            LastSurgeDay = restored.LastSurgeDay;
            InstalledBatteryBankCount = restored.InstalledBatteryBankCount;
            GeneratorCondition = restored.GeneratorCondition;
            ClosedBreakers = restored.ClosedBreakers;
            TrippedRooms = restored.TrippedRooms;
            Priorities = restored.Priorities;
            InstalledCoatedPartItemIds = restored.InstalledCoatedPartItemIds;
            NormalizeAndValidate(rooms);
        }

        internal static float SanitizeNonNegativeFinite(float value) =>
            IsFinite(value) && value > 0f ? value : 0f;

        private static bool IsFinite(float value) =>
            !float.IsNaN(value) && !float.IsInfinity(value);

        private static void NormalizeIdList(List<string> values, HashSet<string> validIds)
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = values.Count - 1; i >= 0; i--)
            {
                string roomId = values[i];
                if (string.IsNullOrWhiteSpace(roomId)
                    || !validIds.Contains(roomId)
                    || !seen.Add(roomId))
                {
                    values.RemoveAt(i);
                }
            }
        }

        private void NormalizePriorities(HashSet<string> validIds)
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = Priorities.Count - 1; i >= 0; i--)
            {
                var priority = Priorities[i];
                if (priority == null
                    || string.IsNullOrWhiteSpace(priority.RoomId)
                    || !validIds.Contains(priority.RoomId)
                    || !seen.Add(priority.RoomId))
                {
                    Priorities.RemoveAt(i);
                    continue;
                }
                if (!Enum.IsDefined(typeof(PowerGridRoomPriority), priority.Priority))
                    priority.Priority = PowerGridRoomPriority.Standard;
            }
        }

        private void NormalizeInstalledCoatedParts()
        {
            var normalized = new List<string>();
            var seenItems = new HashSet<string>(StringComparer.Ordinal);
            var seenFamilies = new HashSet<string>(StringComparer.Ordinal);
            foreach (var itemId in InstalledCoatedPartItemIds)
            {
                if (string.IsNullOrWhiteSpace(itemId) || !seenItems.Add(itemId)) continue;
                string family = PowerGridSystem.ResolveCoatedPartFamily(itemId);
                if (string.IsNullOrEmpty(family) || !seenFamilies.Add(family)) continue;
                normalized.Add(itemId);
            }
            InstalledCoatedPartItemIds = normalized;
        }
    }

    [Serializable]
    public sealed class RoomPriorityRecord
    {
        public string RoomId;
        public PowerGridRoomPriority Priority;
    }

    [Serializable]
    public sealed class PowerGridEvent
    {
        public PowerGridEventKind Kind;
        public string RoomId;
        public int Day;
        public string Detail;
        public float Numeric;

        public PowerGridEvent() { }

        public PowerGridEvent(PowerGridEventKind kind, string roomId, int day,
string? detail = null, float numeric = 0f)
        {
            Kind = kind;
            RoomId = roomId ?? string.Empty;
            Day = day;
            Detail = detail ?? string.Empty;
            Numeric = numeric;
        }
    }

    public enum PowerGridEventKind
    {
        BreakerToggled,
        PriorityChanged,
        FuelAdded,
        Tripped,
        SurgeApplied,
        GenerationChanged,
        BatteryBankInstalled,
        LoadRoomRegistered,
        GeneratorMaintained,
        GeneratorWorn,
        FuelStarved,
        TickSummary
    }

    [Serializable]
    public sealed class PowerGridTickSummary
    {
        public int Day;
        public float FuelConsumed;
        public float BatteryEndWh;
        public float BrownoutHours;
        public bool IsBrownout;

        // ---- Phase 2 (B5–B8) additive fields: allocation projection + edges.
        // Not persisted; consumers read them via OnTickSummary only.

        /// <summary>Generation actually available this tick (after any fuel
        /// starvation adjustment), including external contributions.</summary>
        public float GenerationWatts;

        /// <summary>Intent draw of all eligible rooms (same number the legacy
        /// aggregate math uses).</summary>
        public float RequestedDrawWatts;

        /// <summary>Watts of room load served under deterministic priority
        /// allocation (generation + sustainable battery discharge).</summary>
        public float ServedWatts;

        /// <summary>RequestedDrawWatts − ServedWatts; the shed load.</summary>
        public float UnservedWatts;

        /// <summary>True when at least one Critical-tier room is unserved —
        /// the explicit life-support emergency state, distinct from an ordinary
        /// brownout where only lower-priority loads shed.</summary>
        public bool HasCriticalDeficit;

        /// <summary>Edge: brownout began this tick (false on a restored
        /// campaign that was already in brownout — transitions never replay).</summary>
        public bool BrownoutBegan;

        /// <summary>Edge: brownout ended this tick.</summary>
        public bool BrownoutEnded;

        /// <summary>Room IDs served this tick, in deterministic allocation order
        /// (priority tier descending, RoomId ordinal ascending).</summary>
        public List<string> ServedRoomIds = new List<string>();

        /// <summary>Room IDs shed this tick, in the same deterministic order.</summary>
        public List<string> ShedRoomIds = new List<string>();
    }

    [Serializable]
    public sealed class PowerGridSnapshot
    {
        public int Day;
        public float GenerationWatts;
        public float FuelUnits;
        public float BatteryReserveWh;
        public float BatteryCapacityWh;
        public float TotalDrawWatts;
        public float NetWatts;
        public bool IsBrownout;
        public List<string> RoomIds = new List<string>();
        public Dictionary<string, float> GenerationContributions =
            new Dictionary<string, float>(StringComparer.Ordinal);
    }
}
