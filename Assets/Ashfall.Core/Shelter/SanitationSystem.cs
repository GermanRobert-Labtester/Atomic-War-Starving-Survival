// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Shelter
{
    // ── Public waste taxonomy (keep it simple: three types) ──────

    public enum WasteType
    {
        Organic = 0,
        Chemical = 1,
        Radioactive = 2
    }

    public static class WasteTypeNames
    {
        public const string Organic = "organic";
        public const string Chemical = "chemical";
        public const string Radioactive = "radioactive";

        public static WasteType FromName(string name) => name switch
        {
            Organic => WasteType.Organic,
            Chemical => WasteType.Chemical,
            Radioactive => WasteType.Radioactive,
            _ => WasteType.Organic
        };
    }

    /// <summary>Cleaning priority — multiplies effective cleaning effort.</summary>
    public enum CleaningPriority
    {
        Normal = 0,
        High = 1,
        Critical = 2
    }

    /// <summary>Derived hygiene band (hygiene itself is never stored).</summary>
    public enum HygieneBand
    {
        Excellent = 0,
        Acceptable = 1,
        Poor = 2,
        Squalid = 3,
        Hazardous = 4
    }

    // ── Serialized state ─────────────────────────────────────────

    /// <summary>Per-room waste accumulation (Plan 210 §170.6 — room-level state).</summary>
    [Serializable]
    public sealed class RoomWasteState
    {
        public string roomId = string.Empty;
        public RoomWasteRole role = RoomWasteRole.Other;
        public float organic = 0f;
        public float chemical = 0f;
        public float radioactive = 0f;
    }

    /// <summary>What a room produces. Host maps real rooms to roles.</summary>
    public enum RoomWasteRole
    {
        Residential = 0,
        FoodPrep = 1,
        Industrial = 2,
        Medical = 3,
        Other = 4
    }

    [Serializable]
    public sealed class InstalledFacilityState
    {
        public string facilityId = string.Empty;
        public string roomId = string.Empty;
        public float condition = 100f;   // 0..100
        public float fill = 0f;          // storage facilities: contained units
        public bool powered = true;      // host feeds grid availability
        public bool staffed = true;      // host feeds duty coverage
    }

    [Serializable]
    public sealed class CompostBatchState
    {
        public string batchId = string.Empty;
        public string roomId = string.Empty;
        public float inputUnits = 0f;
        public int startedDay = 0;
        public int readyDay = 0;
        public string outputItemId = string.Empty;
        public float outputUnits = 0f;
    }

    /// <summary>One active toxic spill (deterministic trigger only).</summary>
    [Serializable]
    public sealed class ActiveSpillState
    {
        public string roomId = string.Empty;
        public WasteType type = WasteType.Organic;
        public float severity = 0f;      // 0..1
        public int startDay = 0;
        public string reason = string.Empty;
    }

    /// <summary>Versioned sanitation save state.</summary>
    [Serializable]
    public sealed class SanitationState
    {
        public const int CurrentVersion = 1;

        public int schemaVersion = CurrentVersion;
        public List<RoomWasteState> rooms = new List<RoomWasteState>();
        public List<InstalledFacilityState> facilities = new List<InstalledFacilityState>();
        public List<CompostBatchState> compostQueue = new List<CompostBatchState>();
        public ActiveSpillState? activeSpill = null;   // null = none (legacy default)
        public int lastComplaintDay = -1;              // narrative complaint cooldown
        public int lastTickDay = -1;                   // tick-exactly-once guard
    }

    /// <summary>Cleaning outcome (domain result, no exceptions).</summary>
    public readonly struct CleaningResult
    {
        public readonly bool Accepted;
        public readonly float OrganicRemoved;
        public readonly float ChemicalRemoved;
        public readonly float RadioactiveRemoved;
        public readonly string Reason;

        public CleaningResult(bool accepted, float organic, float chemical, float radioactive, string reason)
        {
            Accepted = accepted;
            OrganicRemoved = organic;
            ChemicalRemoved = chemical;
            RadioactiveRemoved = radioactive;
            Reason = reason ?? string.Empty;
        }
    }

    /// <summary>
    /// Plan 210 — WASTE MANAGEMENT &amp; SANITATION authority.
    ///
    /// Owns: shelter room waste state, facility processing, compost queue,
    /// spill state, derived hygiene. Delegates: infection (DiseaseSystem via
    /// the exposure modifier query), morale (MoraleMarkSystem via bands),
    /// room damage (hazard owners), agriculture (compost output consumed by
    /// the farming flow), UI (projection only).
    ///
    /// Determinism: no wall clock, no unseeded randomness. Spills trigger
    /// deterministically on overcapacity/mishandling only — no arbitrary
    /// catastrophe while the shelter is within safe capacity. Same seed +
    /// same inputs give identical outcomes; no roll is recreated after
    /// restore.
    /// </summary>
    public sealed class SanitationSystem
    {
        public const string SystemId = "sanitation";

        // Authored tuning (bounded; no unbounded exponentials anywhere).
        /// <summary>Organic waste produced per survivor per day.</summary>
        public const float OrganicPerSurvivorPerDay = 1.0f;
        /// <summary>Food-prep rooms contribute a flat organic stream per day.</summary>
        public const float OrganicPerFoodPrepRoomPerDay = 2.0f;
        /// <summary>Waste allowance per room before burden reaches 1.0.</summary>
        public const float RoomWasteAllowance = 60f;
        /// <summary>Effective type weights for burden and hygiene.</summary>
        public const float ChemicalBurdenWeight = 1.5f;
        public const float RadioactiveBurdenWeight = 2.0f;
        /// <summary>Spill trigger: room waste (weighted) beyond this multiple of the allowance.</summary>
        public const float SpillOvercapacityFactor = 2.0f;
        /// <summary>Spill severity: weighted waste at (factor × allowance) → severity 1.0 slope.</summary>
        public const float SpillSeveritySlope = 1.0f;
        /// <summary>Active spill caps room hygiene at this permille.</summary>
        public const float SpillHygieneCapPermille = 200f;
        /// <summary>Cleaning output per assigned worker per day at skill 1.</summary>
        public const float CleaningPerWorkerPerDay = 5.0f;
        /// <summary>Cleaning priority multipliers.</summary>
        public const float CleaningPriorityNormal = 1.0f;
        public const float CleaningPriorityHigh = 1.5f;
        public const float CleaningPriorityCritical = 2.0f;
        /// <summary>Compost conversion: units of organic → units of fertilizer (loss factor included).</summary>
        public const float CompostOutputFraction = 0.6f;
        public const int CompostProcessDays = 6;
        public const string CompostOutputItemId = "item_compost_humus";
        /// <summary>Narrative complaint cooldown (days).</summary>
        public const int ComplaintCooldownDays = 4;
        /// <summary>Pathogen exposure modifier envelope (bounded, never unbounded).</summary>
        public const float PathogenModifierMin = 1.0f;
        public const float PathogenModifierMax = 2.0f;

        private readonly SanitationState _state;
        private SanitationFacilityCatalog _catalog = new SanitationFacilityCatalog();

        public event Action<ActiveSpillState>? OnSpillStarted;
        public event Action<ActiveSpillState>? OnSpillResolved;
        public event Action<CompostBatchState>? OnCompostReady;
        public event Action<string, HygieneBand>? OnSanitationComplaint;   // topic, current band
        public event Action<SanitationState>? OnStateChanged;

        /// <summary>
        /// Plan 210 follow-up — live grid feed: the host supplies the canonical
        /// <see cref="Shelter.PowerGridSystem.IsRoomPowered"/> query here.
        /// Unset (tests, legacy paths) → stored facility.powered wins, byte-identical.
        /// </summary>
        public Func<string, bool>? RoomPowerProvider { get; set; }

        public SanitationSystem(SanitationState? state = null)
        {
            _state = state ?? new SanitationState();
            NormalizeState();
        }

        private void NormalizeState()
        {
            if (_state.schemaVersion < 1 || _state.schemaVersion > SanitationState.CurrentVersion)
                _state.schemaVersion = SanitationState.CurrentVersion;
            _state.rooms ??= new List<RoomWasteState>();
            _state.facilities ??= new List<InstalledFacilityState>();
            _state.compostQueue ??= new List<CompostBatchState>();
            // Legacy saves: activeSpill null → clean baseline (never a
            // hazardous shelter materializing from a pre-210 save).
        }

        public SanitationState State => _state;

        public void BindFacilityCatalog(SanitationFacilityCatalog catalog)
        {
            if (catalog != null) _catalog = catalog;
        }

        public SanitationFacilityCatalog FacilityCatalog => _catalog;

        // ── Registration ─────────────────────────────────────────────

        /// <summary>Register (or fetch) a room for waste tracking. Idempotent.</summary>
        public RoomWasteState EnsureRoom(string roomId, RoomWasteRole role)
        {
            if (string.IsNullOrEmpty(roomId)) throw new ArgumentException("roomId required");
            var room = FindRoom(roomId);
            if (room == null)
            {
                room = new RoomWasteState { roomId = roomId, role = role };
                _state.rooms.Add(room);
            }
            else
            {
                room.role = role;
            }
            return room;
        }

        public RoomWasteState? FindRoom(string roomId)
        {
            foreach (var r in _state.rooms)
                if (r != null && r.roomId == roomId) return r;
            return null;
        }

        /// <summary>Install a facility into a room. Validates against the catalog.</summary>
        public InstalledFacilityState? InstallFacility(string facilityId, string roomId)
        {
            var def = _catalog.Find(facilityId);
            if (def == null) return null;
            if (string.IsNullOrEmpty(roomId)) return null;
            var installed = new InstalledFacilityState { facilityId = facilityId, roomId = roomId };
            _state.facilities.Add(installed);
            return installed;
        }

        // ── Producers ────────────────────────────────────────────────

        /// <summary>
        /// Industrial/medical producers push non-organic waste here. Chemical
        /// and radioactive waste never auto-accumulates — only real emitters.
        /// Rejected (returns false) for non-positive amounts or unknown rooms.
        /// </summary>
        public bool EmitWaste(string roomId, WasteType type, float amount)
        {
            if (string.IsNullOrEmpty(roomId) || amount <= 0f) return false;
            var room = FindRoom(roomId);
            if (room == null) return false;
            switch (type)
            {
                case WasteType.Organic: room.organic += amount; break;
                case WasteType.Chemical: room.chemical += amount; break;
                case WasteType.Radioactive: room.radioactive += amount; break;
            }
            return true;
        }

        // ── Daily tick ───────────────────────────────────────────────

        /// <summary>
        /// Advance one day: accumulate → facility processing → spill check →
        /// complaint check. Deterministic; called exactly once per day by the
        /// `hygiene` day owner (phase 3, before the disease tick).
        /// </summary>
        public void TickDaily(int day, int population)
        {
            if (_state.lastTickDay == day) return;   // tick-exactly-once guard
            _state.lastTickDay = day;

            // 1. Accumulation.
            var residential = _state.rooms.Where(r => r.role == RoomWasteRole.Residential).ToList();
            var foodPrep = _state.rooms.Where(r => r.role == RoomWasteRole.FoodPrep).ToList();
            if (residential.Count > 0 && population > 0)
            {
                float perRoom = population * OrganicPerSurvivorPerDay / residential.Count;
                foreach (var room in residential) room.organic += perRoom;
            }
            foreach (var room in foodPrep)
                room.organic += OrganicPerFoodPrepRoomPerDay;

            // 2. Facility processing.
            ProcessFacilities(day);

            // 3. Spill check (deterministic overcapacity only).
            CheckSpills(day);

            // 4. Complaint threshold + cooldown.
            CheckComplaints(day);

            OnStateChanged?.Invoke(_state);
        }

        private void ProcessFacilities(int day)
        {
            foreach (var facility in _state.facilities)
            {
                if (facility == null || string.IsNullOrEmpty(facility.facilityId)) continue;

                // Plan 210 follow-up — the host feeds live grid availability;
                // provider unset (legacy saves/tests) keeps the stored value.
                if (RoomPowerProvider != null && !string.IsNullOrEmpty(facility.roomId))
                    facility.powered = RoomPowerProvider(facility.roomId);

                var def = _catalog.Find(facility.facilityId);
                if (def == null) continue;
                var room = FindRoom(facility.roomId);
                if (room == null) continue;
                if (!facility.powered && def.power_draw > 0) continue;   // unpowered machine stalls
                if (!facility.staffed && def.labor_required > 0) continue;

                float conditionFactor = Math.Clamp(facility.condition, 0f, 100f) / 100f;
                float rate = def.processing_rate * conditionFactor;
                if (rate <= 0f) continue;

                bool isStorage = string.Equals(def.facility_type, "storage", StringComparison.Ordinal);
                foreach (var wasteTypeName in def.waste_types)
                {
                    var type = WasteTypeNames.FromName(wasteTypeName);
                    float available = GetWaste(room, type);
                    if (available <= 0f) continue;

                    if (isStorage)
                    {
                        // Storage CONTAINS waste into its own capacity; when
                        // full, nothing more is processed (backup → spill risk).
                        float freeCapacity = def.capacity - facility.fill;
                        if (freeCapacity <= 0f) continue;
                        float contained = Math.Min(Math.Min(available, rate), freeCapacity);
                        facility.fill += contained;
                        AddWaste(room, type, -contained);
                    }
                    else if (string.Equals(def.facility_type, "compost", StringComparison.Ordinal))
                    {
                        // Compost converts organic into a fertilizer batch.
                        // Chemical/radioactive waste can NEVER enter the
                        // compost path (cross-contamination rejection).
                        if (type != WasteType.Organic) continue;
                        float converted = Math.Min(available, rate);
                        AddWaste(room, WasteType.Organic, -converted);
                        _state.compostQueue.Add(new CompostBatchState
                        {
                            batchId = $"compost_{facility.roomId}_{day}_{_state.compostQueue.Count + 1}",
                            roomId = facility.roomId,
                            inputUnits = converted,
                            startedDay = day,
                            readyDay = day + CompostProcessDays,
                            outputItemId = CompostOutputItemId,
                            outputUnits = converted * CompostOutputFraction
                        });
                    }
                    else
                    {
                        // Destroying/processing facility: waste simply reduced.
                        float processed = Math.Min(available, rate);
                        AddWaste(room, type, -processed);
                    }
                }
            }

            // Mature compost batches → typed event (host delivers the item).
            for (int i = _state.compostQueue.Count - 1; i >= 0; i--)
            {
                var batch = _state.compostQueue[i];
                if (batch == null) { _state.compostQueue.RemoveAt(i); continue; }
                if (batch.readyDay <= day)
                {
                    _state.compostQueue.RemoveAt(i);
                    OnCompostReady?.Invoke(batch);
                }
            }
        }

        private void CheckSpills(int day)
        {
            RoomWasteState? worst = null;
            float worstSeverity = 0f;
            WasteType worstType = WasteType.Organic;

            foreach (var room in _state.rooms)
            {
                if (room == null) continue;
                foreach (var (type, amount, weight) in new[]
                {
                    (WasteType.Organic, room.organic, 1.0f),
                    (WasteType.Chemical, room.chemical, ChemicalBurdenWeight),
                    (WasteType.Radioactive, room.radioactive, RadioactiveBurdenWeight)
                })
                {
                    float weighted = amount * weight;
                    float threshold = RoomWasteAllowance * SpillOvercapacityFactor * weight;
                    if (weighted > threshold)
                    {
                        float severity = Math.Min(1f, (weighted - threshold) / (threshold * SpillSeveritySlope) + 0.5f);
                        if (severity > worstSeverity)
                        {
                            worstSeverity = severity;
                            worst = room;
                            worstType = type;
                        }
                    }
                }
            }

            if (worst == null)
            {
                // Within safe capacity: resolve any stale spill.
                if (_state.activeSpill != null)
                {
                    var resolved = _state.activeSpill;
                    _state.activeSpill = null;
                    OnSpillResolved?.Invoke(resolved);
                }
                return;
            }

            if (_state.activeSpill == null)
            {
                var spill = new ActiveSpillState
                {
                    roomId = worst.roomId,
                    type = worstType,
                    severity = Math.Clamp(worstSeverity, 0f, 1f),
                    startDay = day,
                    reason = $"overcapacity_{WasteTypeName(worstType)}"
                };
                _state.activeSpill = spill;
                OnSpillStarted?.Invoke(spill);
            }
            else if (_state.activeSpill.roomId == worst.roomId)
            {
                // Same room: severity follows current burden (never resets randomly).
                _state.activeSpill.severity = Math.Clamp(
                    Math.Max(_state.activeSpill.severity, worstSeverity), 0f, 1f);
            }
            // A different room's overflow queues behind the active spill
            // (one emergency at a time — cleaning resolves the first).
        }

        private void CheckComplaints(int day)
        {
            var band = GetShelterHygieneBand();
            bool neverComplained = _state.lastComplaintDay < 0;
            bool cooldownElapsed = day - _state.lastComplaintDay >= ComplaintCooldownDays;
            if (band >= HygieneBand.Poor && (neverComplained || cooldownElapsed))
            {
                _state.lastComplaintDay = day;
                string topic = _state.activeSpill != null
                    ? "contaminated_corridor"
                    : band >= HygieneBand.Squalid ? "blocked_latrine" : "odor_flies";
                OnSanitationComplaint?.Invoke(topic, band);
            }
        }

        // ── Cleaning ─────────────────────────────────────────────────

        /// <summary>
        /// Assign cleaning effort to a room (deterministic). Priority scales
        /// the effective worker-days. Workers are a labor trade-off — the
        /// duty roster owns their assignment; this API only consumes effort.
        /// </summary>
        public CleaningResult ApplyCleaning(string roomId, int workers, float skill01, CleaningPriority priority, int day)
        {
            var room = FindRoom(roomId);
            if (room == null) return new CleaningResult(false, 0, 0, 0, "unknown_room");
            if (workers <= 0) return new CleaningResult(false, 0, 0, 0, "no_workers");
            float skill = Math.Clamp(skill01, 0f, 1f);
            float priorityMult = priority switch
            {
                CleaningPriority.High => CleaningPriorityHigh,
                CleaningPriority.Critical => CleaningPriorityCritical,
                _ => CleaningPriorityNormal
            };
            float effort = workers * skill * priorityMult;
            if (effort <= 0f) return new CleaningResult(false, 0, 0, 0, "no_effort");

            float organicRemoved = Math.Min(room.organic, effort * CleaningPerWorkerPerDay);
            float chemicalRemoved = Math.Min(room.chemical, effort * CleaningPerWorkerPerDay * 0.5f);   // harder to clean
            float radioactiveRemoved = 0f;   // sealed waste needs the shielded store, never bare hands

            room.organic -= organicRemoved;
            room.chemical -= chemicalRemoved;

            // Critical emergency cleaning can resolve an active spill in the room.
            if (priority == CleaningPriority.Critical
                && _state.activeSpill != null
                && _state.activeSpill.roomId == roomId
                && organicRemoved + chemicalRemoved > 0f)
            {
                var resolved = _state.activeSpill;
                _state.activeSpill = null;
                OnSpillResolved?.Invoke(resolved);
            }

            OnStateChanged?.Invoke(_state);
            return new CleaningResult(true, organicRemoved, chemicalRemoved, radioactiveRemoved, day.ToString());
        }

        // ── Derived state (never stored) ─────────────────────────────

        private static float GetWaste(RoomWasteState room, WasteType type) => type switch
        {
            WasteType.Organic => room.organic,
            WasteType.Chemical => room.chemical,
            WasteType.Radioactive => room.radioactive,
            _ => 0f
        };

        private static void AddWaste(RoomWasteState room, WasteType type, float delta)
        {
            switch (type)
            {
                case WasteType.Organic: room.organic = Math.Max(0f, room.organic + delta); break;
                case WasteType.Chemical: room.chemical = Math.Max(0f, room.chemical + delta); break;
                case WasteType.Radioactive: room.radioactive = Math.Max(0f, room.radioactive + delta); break;
            }
        }

        private static string WasteTypeName(WasteType type) => type switch
        {
            WasteType.Chemical => WasteTypeNames.Chemical,
            WasteType.Radioactive => WasteTypeNames.Radioactive,
            _ => WasteTypeNames.Organic
        };

        /// <summary>Weighted room burden normalized by the room allowance (0..1+).</summary>
        public float GetRoomBurden(string roomId)
        {
            var room = FindRoom(roomId);
            if (room == null) return 0f;
            float weighted = room.organic + room.chemical * ChemicalBurdenWeight + room.radioactive * RadioactiveBurdenWeight;
            return weighted / RoomWasteAllowance;
        }

        /// <summary>Derived per-room hygiene (permille 0..1000). Spill caps it.</summary>
        public int GetRoomHygienePermille(string roomId)
        {
            float burden = GetRoomBurden(roomId);
            int hygiene = (int)MathF.Round(1000f * Math.Clamp(1f - burden, 0f, 1f));
            if (_state.activeSpill != null && _state.activeSpill.roomId == roomId)
                hygiene = Math.Min(hygiene, (int)SpillHygieneCapPermille);
            return Math.Clamp(hygiene, 0, 1000);
        }

        /// <summary>Shelter aggregate hygiene: population-weighted room mean (plain mean here).</summary>
        public int GetShelterHygienePermille()
        {
            if (_state.rooms.Count == 0) return 1000;
            float sum = 0f;
            foreach (var room in _state.rooms)
                if (room != null) sum += GetRoomHygienePermille(room.roomId);
            return (int)MathF.Round(sum / _state.rooms.Count);
        }

        public HygieneBand BandForPermille(int permille) => permille switch
        {
            >= 850 => HygieneBand.Excellent,
            >= 600 => HygieneBand.Acceptable,
            >= 350 => HygieneBand.Poor,
            >= 150 => HygieneBand.Squalid,
            _ => HygieneBand.Hazardous
        };

        public HygieneBand GetShelterHygieneBand() => BandForPermille(GetShelterHygienePermille());

        /// <summary>
        /// Bounded pathogen exposure modifier for DiseaseSystem.TryExpose
        /// (host wires it as ProbabilityModifier). 1.0 in a clean shelter,
        /// at most 2.0 in the foulest — infection outcomes stay owned by the
        /// disease authority; sanitation only supplies risk.
        /// </summary>
        public float GetPathogenExposureModifier(string roomId)
        {
            var room = FindRoom(roomId);
            if (room == null) return PathogenModifierMin;
            float organicFactor = Math.Min(1f, room.organic / 80f);
            float burden = Math.Min(1f, GetRoomBurden(roomId));
            float squalor = burden * 0.6f;
            float modifier = PathogenModifierMin + 0.8f * Math.Min(1f, organicFactor * 0.7f + squalor * 0.6f);
            return Math.Clamp(modifier, PathogenModifierMin, PathogenModifierMax);
        }

        public ActiveSpillState? ActiveSpill => _state.activeSpill;
        public IReadOnlyList<CompostBatchState> CompostQueue => _state.compostQueue;

        // ── Save / restore ───────────────────────────────────────────

        public SanitationState CaptureState()
        {
            var copy = new SanitationState
            {
                schemaVersion = SanitationState.CurrentVersion,
                lastComplaintDay = _state.lastComplaintDay,
                lastTickDay = _state.lastTickDay,
                activeSpill = _state.activeSpill == null ? null : new ActiveSpillState
                {
                    roomId = _state.activeSpill.roomId,
                    type = _state.activeSpill.type,
                    severity = Math.Clamp(_state.activeSpill.severity, 0f, 1f),
                    startDay = _state.activeSpill.startDay,
                    reason = _state.activeSpill.reason ?? string.Empty
                }
            };
            foreach (var room in _state.rooms.OrderBy(r => r.roomId, StringComparer.Ordinal))
            {
                if (room == null) continue;
                copy.rooms.Add(new RoomWasteState
                {
                    roomId = room.roomId,
                    role = room.role,
                    organic = Math.Max(0f, room.organic),
                    chemical = Math.Max(0f, room.chemical),
                    radioactive = Math.Max(0f, room.radioactive)
                });
            }
            foreach (var f in _state.facilities.OrderBy(f => f.roomId, StringComparer.Ordinal))
            {
                if (f == null) continue;
                copy.facilities.Add(new InstalledFacilityState
                {
                    facilityId = f.facilityId,
                    roomId = f.roomId,
                    condition = Math.Clamp(f.condition, 0f, 100f),
                    fill = Math.Max(0f, f.fill),
                    powered = f.powered,
                    staffed = f.staffed
                });
            }
            foreach (var b in _state.compostQueue.OrderBy(b => b.batchId, StringComparer.Ordinal))
            {
                if (b == null) continue;
                copy.compostQueue.Add(new CompostBatchState
                {
                    batchId = b.batchId,
                    roomId = b.roomId,
                    inputUnits = Math.Max(0f, b.inputUnits),
                    startedDay = Math.Max(0, b.startedDay),
                    readyDay = Math.Max(0, b.readyDay),
                    outputItemId = b.outputItemId ?? string.Empty,
                    outputUnits = Math.Max(0f, b.outputUnits)
                });
            }
            return copy;
        }

        public void RestoreState(SanitationState? saved)
        {
            if (saved == null) return;
            if (saved.schemaVersion > SanitationState.CurrentVersion)
                throw new InvalidOperationException(
                    $"sanitation save version {saved.schemaVersion} is newer than supported ({SanitationState.CurrentVersion})");

            _state.schemaVersion = SanitationState.CurrentVersion;
            _state.rooms.Clear();
            if (saved.rooms != null)
            {
                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var r in saved.rooms)
                {
                    if (r == null || string.IsNullOrEmpty(r.roomId) || !seen.Add(r.roomId)) continue;
                    _state.rooms.Add(new RoomWasteState
                    {
                        roomId = r.roomId,
                        role = r.role,
                        organic = Math.Max(0f, r.organic),
                        chemical = Math.Max(0f, r.chemical),
                        radioactive = Math.Max(0f, r.radioactive)
                    });
                }
            }
            _state.facilities.Clear();
            if (saved.facilities != null)
            {
                foreach (var f in saved.facilities)
                {
                    if (f == null || string.IsNullOrEmpty(f.facilityId)) continue;
                    _state.facilities.Add(new InstalledFacilityState
                    {
                        facilityId = f.facilityId,
                        roomId = f.roomId ?? string.Empty,
                        condition = Math.Clamp(f.condition, 0f, 100f),
                        fill = Math.Max(0f, f.fill),
                        powered = f.powered,
                        staffed = f.staffed
                    });
                }
            }
            _state.compostQueue.Clear();
            if (saved.compostQueue != null)
            {
                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var b in saved.compostQueue)
                {
                    if (b == null || string.IsNullOrEmpty(b.batchId) || !seen.Add(b.batchId)) continue;
                    _state.compostQueue.Add(new CompostBatchState
                    {
                        batchId = b.batchId,
                        roomId = b.roomId ?? string.Empty,
                        inputUnits = Math.Max(0f, b.inputUnits),
                        startedDay = Math.Max(0, b.startedDay),
                        readyDay = Math.Max(0, b.readyDay),
                        outputItemId = b.outputItemId ?? string.Empty,
                        outputUnits = Math.Max(0f, b.outputUnits)
                    });
                }
            }
            _state.activeSpill = saved.activeSpill == null ? null : new ActiveSpillState
            {
                roomId = saved.activeSpill.roomId ?? string.Empty,
                type = saved.activeSpill.type,
                severity = Math.Clamp(saved.activeSpill.severity, 0f, 1f),
                startDay = Math.Max(0, saved.activeSpill.startDay),
                reason = saved.activeSpill.reason ?? string.Empty
            };
            _state.lastComplaintDay = saved.lastComplaintDay;
            _state.lastTickDay = saved.lastTickDay;
            OnStateChanged?.Invoke(_state);
        }
    }
}
