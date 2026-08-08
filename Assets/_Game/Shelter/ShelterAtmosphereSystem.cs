using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Room atmosphere: humidity, oxygen, local CO, fire ignition / fight / seal.
    /// Internal Horror — Fire in the Hole + humidity foundation for pantry rust.
    /// </summary>
    public class ShelterAtmosphereSystem
    {
        /// <summary>Durability at/below which diesel/heater may spark a fire.</summary>
        public const float IgnitionDurabilityThreshold = 25f;

        /// <summary>Base chance per game-hour that a low-durability source ignites its room.</summary>
        public const float IgnitionChancePerHour = 0.15f;

        /// <summary>Oxygen fraction consumed by fire per intensity-hour.</summary>
        public const float FireO2ConsumptionPerHour = 0.06f;

        /// <summary>Local CO ppm added by fire per intensity-hour.</summary>
        public const float FireCoPpmPerHour = 140f;

        /// <summary>Bunker-wide CO ppm mirrored onto PowerNetwork per intensity-hour.</summary>
        public const float FireNetworkCoPpmPerHour = 40f;

        /// <summary>O2 fraction at which a sealed fire starves out.</summary>
        public const float FireExtinguishO2Threshold = 0.08f;

        /// <summary>O2 below which survivors take hypoxia pressure (hook for needs).</summary>
        public const float HypoxiaO2Threshold = 0.16f;

        /// <summary>Burn health damage when fighting a fire (per attempt).</summary>
        public const float FightFireBurnDamage = 12f;

        /// <summary>Fire intensity removed by one FightFire action (bare hands).</summary>
        public const float FightFireIntensityReduction = 0.45f;

        /// <summary>Fire intensity removed by one ExtinguishFire action (with water/sand).</summary>
        public const float ExtinguishFireIntensityReduction = 0.7f;

        /// <summary>Water units consumed per ExtinguishFire action.</summary>
        public const float ExtinguishFireWaterCost = 2f;

        /// <summary>Fire intensity above which spread to adjacent rooms is possible.</summary>
        public const float FireSpreadIntensityThreshold = 0.5f;

        /// <summary>Chance per hour that fire spreads to an adjacent room at max intensity.</summary>
        public const float FireSpreadChancePerHour = 0.25f;

        /// <summary>Humidity rise per hour when mold is present.</summary>
        public const float HumidityFromMoldPerHour = 0.015f;

        /// <summary>Natural humidity bleed toward ambient baseline when dry.</summary>
        public const float HumidityBaseline = 0.35f;

        /// <summary>Base CO2 clear rate (ppm/hour) when air filtration is operational.</summary>
        public const float BaseCo2ClearPerHour = 20f;

        /// <summary>Base mold clear rate (0..1 per hour) when air filtration is operational.</summary>
        public const float BaseMoldClearPerHour = 0.02f;

        private readonly List<ShelterRoom> _rooms = new List<ShelterRoom>();
        private readonly System.Random _rng;
        private float _ventilationClearMultiplier = 1f;
        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;

        public event Action<ShelterRoom> OnFireStarted;
        public event Action<ShelterRoom> OnFireExtinguished;
        public event Action<ShelterRoom> OnBulkheadSealed;
        public event Action OnAtmosphereChanged;

        public IReadOnlyList<ShelterRoom> Rooms => _rooms;

        /// <summary>Prompt #226 — Grid Walker prevents generator fires.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        /// <summary>
        /// Prompt #197 — HVAC Tech: 1.3 when tech is in bunker (clears CO2/mold 30% faster).
        /// </summary>
        public float VentilationClearMultiplier
        {
            get => _ventilationClearMultiplier;
            set => _ventilationClearMultiplier = Mathf.Max(0.01f, value);
        }

        public ShelterAtmosphereSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(17);
        }

        public void RegisterRoom(ShelterRoom room)
        {
            if (room == null || string.IsNullOrEmpty(room.RoomId)) return;
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i] != null && _rooms[i].RoomId == room.RoomId)
                {
                    _rooms[i] = room;
                    return;
                }
            }
            _rooms.Add(room);
        }

        public ShelterRoom GetRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return null;
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i] != null && _rooms[i].RoomId == roomId)
                    return _rooms[i];
            }
            return null;
        }

        /// <summary>
        /// Prompt #20 / #197 — clear room CO2 and mold via ventilation.
        /// Returns true when any value changed.
        /// </summary>
        public bool ApplyVentilationClear(ShelterRoom room, float gameHours, float speedMultiplier = 1f)
        {
            if (room == null || gameHours <= 0f) return false;
            float mult = Mathf.Max(0.01f, speedMultiplier);
            bool changed = false;

            if (room.Co2Ppm > 0f)
            {
                float cleared = BaseCo2ClearPerHour * mult * gameHours;
                float next = Mathf.Max(0f, room.Co2Ppm - cleared);
                if (!Mathf.Approximately(next, room.Co2Ppm))
                {
                    room.Co2Ppm = next;
                    changed = true;
                }
            }

            if (room.MoldLevel > 0f || room.HasMold)
            {
                float cleared = BaseMoldClearPerHour * mult * gameHours;
                float next = Mathf.Max(0f, room.MoldLevel - cleared);
                if (!Mathf.Approximately(next, room.MoldLevel))
                {
                    room.MoldLevel = next;
                    changed = true;
                }
                if (room.MoldLevel <= 0.001f)
                {
                    room.MoldLevel = 0f;
                    room.HasMold = false;
                    changed = true;
                }
            }

            return changed;
        }

        public static bool IsAirFilterOperational(Shelter shelter)
        {
            var air = shelter?.GetModule("air_filtration");
            return air != null && air.IsOperational && air.FilterHealth > 0f;
        }

        /// <summary>
        /// Advance humidity, fire O2/CO consumption, and low-durability ignition rolls.
        /// </summary>
        public void Tick(
            float gameHours,
            PowerNetwork power = null,
            Shelter shelter = null)
        {
            if (gameHours <= 0f) return;

            TryIgniteFromLowDurability(gameHours, power, shelter);

            bool filterOk = IsAirFilterOperational(shelter);
            bool changed = false;
            for (int i = 0; i < _rooms.Count; i++)
            {
                var room = _rooms[i];
                if (room == null) continue;

                // Prompt #20 / #197 — operational filtration clears CO2 and mold.
                if (filterOk && ApplyVentilationClear(room, gameHours, _ventilationClearMultiplier))
                    changed = true;

                // Humidity: mold drives it up; otherwise drift slowly toward baseline.
                if (room.HasMold || room.MoldLevel > 0.05f)
                {
                    float mold = Mathf.Max(room.MoldLevel, room.HasMold ? 0.2f : 0f);
                    room.Humidity = Mathf.Clamp01(
                        room.Humidity + HumidityFromMoldPerHour * mold * gameHours);
                    changed = true;
                }
                else if (room.Humidity > HumidityBaseline)
                {
                    room.Humidity = Mathf.Max(
                        HumidityBaseline,
                        room.Humidity - 0.005f * gameHours);
                    changed = true;
                }

                if (!room.IsOnFire) continue;

                float intensity = Mathf.Clamp01(room.FireIntensity);
                if (intensity <= 0f)
                {
                    ClearFire(room, raiseEvent: true);
                    changed = true;
                    continue;
                }

                // Consume oxygen; sealed rooms cannot exchange gas.
                float o2Loss = FireO2ConsumptionPerHour * intensity * gameHours;
                room.OxygenFraction = Mathf.Clamp(
                    room.OxygenFraction - o2Loss, 0f, ShelterRoom.DefaultOxygenFraction);

                float localCo = FireCoPpmPerHour * intensity * gameHours;
                room.LocalCoPpm = Mathf.Max(0f, room.LocalCoPpm + localCo);

                if (power != null)
                {
                    power.AddCarbonMonoxide(FireNetworkCoPpmPerHour * intensity * gameHours);
                }

                // Sealed bulkhead + low O2 starves the fire.
                if (room.BulkheadSealed && room.OxygenFraction <= FireExtinguishO2Threshold)
                {
                    ClearFire(room, raiseEvent: true);
                    changed = true;
                    continue;
                }

                // Open room: fire slowly grows unless fought.
                if (!room.BulkheadSealed)
                {
                    room.FireIntensity = Mathf.Clamp01(room.FireIntensity + 0.05f * gameHours);
                }
                else
                {
                    // Sealed but not yet starved: intensity bleeds as O2 drops.
                    room.FireIntensity = Mathf.Clamp01(
                        room.FireIntensity - 0.12f * gameHours);
                }

                changed = true;
            }

            // Fire spread: cellular automaton to adjacent rooms (Prompt #54).
            changed |= SpreadFireToAdjacentRooms(gameHours, shelter);

            if (changed) OnAtmosphereChanged?.Invoke();
        }

        /// <summary>
        /// Overworked generators / heaters with low durability may ignite their room.
        /// </summary>
        public void TryIgniteFromLowDurability(
            float gameHours,
            PowerNetwork power,
            Shelter shelter = null)
        {
            // Prompt #226 — Grid Walker: generators never cause fires.
            if (_personalQuests != null
                && _personalQuests.GeneratorsImmuneToBreakdown(_getSurvivors?.Invoke()))
                return;

            if (gameHours <= 0f || power == null) return;

            var sources = power.Sources;
            if (sources == null) return;

            for (int i = 0; i < sources.Count; i++)
            {
                var src = sources[i];
                if (src == null || !src.IsEnabled) continue;
                if (src.Durability > IgnitionDurabilityThreshold) continue;
                if (src.Definition == null) continue;

                // Diesel gens and any source running in a room with fuel heat risk.
                bool isRiskKind = src.Definition.Kind == PowerSourceKind.Diesel
                    || src.Definition.Kind == PowerSourceKind.Bicycle;
                if (!isRiskKind && string.IsNullOrEmpty(src.RoomId)) continue;

                float chance = IgnitionChancePerHour * gameHours
                    * (1f - src.Durability / IgnitionDurabilityThreshold);
                if (_rng.NextDouble() > chance) continue;

                string roomId = !string.IsNullOrEmpty(src.RoomId) ? src.RoomId : "plant";
                var room = GetRoom(roomId);
                if (room == null)
                {
                    room = new ShelterRoom(roomId, null);
                    RegisterRoom(room);
                }
                if (room.IsOnFire) continue;

                StartFire(room, intensity: 0.55f);
            }

            // Heater module wear mirrors generator durability risk.
            if (shelter != null)
            {
                var heater = shelter.GetModule("heater");
                if (heater != null && heater.IsOperational && heater.FilterHealth <= IgnitionDurabilityThreshold)
                {
                    float chance = IgnitionChancePerHour * gameHours
                        * (1f - heater.FilterHealth / IgnitionDurabilityThreshold);
                    if (_rng.NextDouble() <= chance)
                    {
                        string roomId = !string.IsNullOrEmpty(heater.RoomId) ? heater.RoomId : "quarters";
                        var room = GetRoom(roomId);
                        if (room == null)
                        {
                            room = new ShelterRoom(roomId, null);
                            RegisterRoom(room);
                        }
                        if (!room.IsOnFire)
                            StartFire(room, intensity: 0.4f);
                    }
                }
            }
        }

        public void StartFire(ShelterRoom room, float intensity = 0.5f)
        {
            if (room == null) return;
            room.IsOnFire = true;
            room.FireIntensity = Mathf.Clamp01(intensity);
            OnFireStarted?.Invoke(room);
            OnAtmosphereChanged?.Invoke();
        }

        /// <summary>
        /// Fight the fire in person: take burn damage, reduce intensity.
        /// Returns true if the fire was fully extinguished.
        /// </summary>
        public bool FightFire(string roomId, Survivor fighter, NeedsSystem needs)
        {
            var room = GetRoom(roomId);
            if (room == null || !room.IsOnFire) return false;
            if (fighter == null || !fighter.IsAlive) return false;

            if (needs != null)
                needs.Modify(fighter, NeedKind.Health, -FightFireBurnDamage);

            room.FireIntensity = Mathf.Clamp01(
                room.FireIntensity - FightFireIntensityReduction);

            if (room.FireIntensity <= 0.05f)
            {
                ClearFire(room, raiseEvent: true);
                return true;
            }

            OnAtmosphereChanged?.Invoke();
            return false;
        }

        /// <summary>
        /// Seal the bulkhead: sacrifice modules in the room to starve the fire of O2.
        /// </summary>
        public bool SealBulkhead(string roomId, Shelter shelter)
        {
            var room = GetRoom(roomId);
            if (room == null) return false;

            room.BulkheadSealed = true;
            room.ModulesSacrificed = true;

            if (shelter != null)
            {
                var mods = shelter.Modules;
                for (int i = 0; i < mods.Count; i++)
                {
                    var m = mods[i];
                    if (m == null) continue;
                    if (!string.Equals(m.RoomId, roomId, StringComparison.OrdinalIgnoreCase))
                        continue;
                    m.IsEnabled = false;
                }
            }

            OnBulkheadSealed?.Invoke(room);
            OnAtmosphereChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Extinguish fire with water or sand. More effective than bare-handed
        /// fighting but consumes clean water. Returns true if fire is out.
        /// </summary>
        public bool ExtinguishFire(string roomId, Survivor fighter, NeedsSystem needs,
            WaterStorage water = null)
        {
            var room = GetRoom(roomId);
            if (room == null || !room.IsOnFire) return false;
            if (fighter == null || !fighter.IsAlive) return false;

            // Consume water if available; fall back to sand (less effective).
            float reduction = ExtinguishFireIntensityReduction;
            if (water != null && water.CleanWater >= ExtinguishFireWaterCost)
            {
                water.ConsumeClean(ExtinguishFireWaterCost);
            }
            else if (water != null && water.DirtyWater >= ExtinguishFireWaterCost)
            {
                water.ConsumeDirty(ExtinguishFireWaterCost);
            }
            else
            {
                // Sand / blanket: less effective, more fatigue.
                reduction *= 0.6f;
                if (needs != null)
                    needs.Modify(fighter, NeedKind.Fatigue, 5f);
            }

            if (needs != null
                && (_personalQuests == null || !_personalQuests.IsImmuneToFireAndTemperature(fighter)))
                needs.Modify(fighter, NeedKind.Health, -FightFireBurnDamage * 0.5f);

            room.FireIntensity = Mathf.Clamp01(room.FireIntensity - reduction);

            if (room.FireIntensity <= 0.05f)
            {
                ClearFire(room, raiseEvent: true);
                // #270 Trial by Fire: count extinguish toward Fire-Breather.
                _personalQuests?.RecordBunkerFireExtinguished(fighter);
                return true;
            }

            OnAtmosphereChanged?.Invoke();
            return false;
        }

        /// <summary>
        /// #284 Apply O2/CO2 atmospheric health penalty (Deep Delver immune).
        /// </summary>
        public float ApplyGasPenaltyToSurvivor(
            Survivor sv, float rawHealthDamage, bool isO2OrCo2Penalty = true)
        {
            if (_personalQuests == null || sv == null) return 0f;
            return _personalQuests.ApplyAtmosphereGasPenalty(sv, rawHealthDamage, isO2OrCo2Penalty);
        }

        /// <summary>
        /// #284 CO leak shelter event: resolve for all coal miners (Canary quest).
        /// </summary>
        public void ResolveCoLeakEvent(
            float healthDamageTakenByMiner,
            bool minerSavedAnother,
            int currentDay = 0)
        {
            if (_personalQuests == null || _getSurvivors == null) return;
            var list = _getSurvivors();
            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var sv = list[i];
                if (sv == null || !sv.IsAlive) continue;
                if (!string.Equals(sv.ArchetypeId, PersonalQuestSystem.CoalMinerId,
                        StringComparison.Ordinal)
                    && !_personalQuests.HasBlackLung(sv)
                    && !_personalQuests.HasDeepDelver(sv))
                    continue;
                _personalQuests.ResolveCoLeakShelterEvent(
                    sv, healthDamageTakenByMiner, minerSavedAnother, currentDay);
            }
        }

        /// <summary>
        /// #270 Pyromaniac AI quirk: 5% daily chance to start a fire when morale &lt; 30.
        /// Call once per day from host.
        /// </summary>
        public bool TryPyromaniacDeliberateFire(System.Random rng = null)
        {
            if (_personalQuests == null || _getSurvivors == null || _rooms == null || _rooms.Count == 0)
                return false;
            var list = _getSurvivors();
            if (list == null) return false;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("shelteratmospheresystem");
            for (int i = 0; i < list.Count; i++)
            {
                var sv = list[i];
                if (sv == null || !sv.IsAlive) continue;
                if (!_personalQuests.ShouldDeliberatelyStartFire(sv, rng)) continue;
                // Pick first non-burning room.
                for (int r = 0; r < _rooms.Count; r++)
                {
                    var room = _rooms[r];
                    if (room == null || room.IsOnFire) continue;
                    StartFire(room, intensity: 0.45f);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Fire spread: each room on fire may ignite adjacent rooms (Prompt #54).
        /// Uses shelter room adjacency. Spread chance scales with fire intensity.
        /// </summary>
        private bool SpreadFireToAdjacentRooms(float gameHours, Shelter shelter)
        {
            if (shelter == null || gameHours <= 0f) return false;
            bool spread = false;

            // Snapshot burning rooms so spread doesn't cascade infinitely in one tick.
            var burning = new List<ShelterRoom>();
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i] != null && _rooms[i].IsOnFire
                    && _rooms[i].FireIntensity >= FireSpreadIntensityThreshold)
                    burning.Add(_rooms[i]);
            }

            for (int i = 0; i < burning.Count; i++)
            {
                var fireRoom = burning[i];
                float spreadChance = FireSpreadChancePerHour * fireRoom.FireIntensity * gameHours;
                if (_rng.NextDouble() >= spreadChance) continue;

                // Find adjacent rooms.
                var allRooms = shelter.Rooms;
                if (allRooms == null) continue;
                var candidates = new List<ShelterRoom>();
                for (int j = 0; j < allRooms.Count; j++)
                {
                    var other = allRooms[j];
                    if (other == null || other == fireRoom) continue;
                    if (other.IsOnFire) continue;
                    if (shelter.AreRoomsAdjacent(fireRoom.RoomId, other.RoomId))
                        candidates.Add(other);
                }

                if (candidates.Count == 0) continue;
                var target = candidates[_rng.Next(candidates.Count)];
                StartFire(target, intensity: 0.35f);
                spread = true;
            }

            return spread;
        }

        public void ClearFire(ShelterRoom room, bool raiseEvent)
        {
            if (room == null) return;
            bool wasOnFire = room.IsOnFire;
            room.IsOnFire = false;
            room.FireIntensity = 0f;
            if (raiseEvent && wasOnFire)
                OnFireExtinguished?.Invoke(room);
            OnAtmosphereChanged?.Invoke();
        }

        public ShelterAtmosphereSave CaptureState()
        {
            var save = new ShelterAtmosphereSave
            {
                Rooms = new ShelterRoomAtmosphereSave[_rooms.Count]
            };
            for (int i = 0; i < _rooms.Count; i++)
            {
                var r = _rooms[i];
                if (r == null) continue;
                save.Rooms[i] = new ShelterRoomAtmosphereSave
                {
                    RoomId = r.RoomId,
                    Humidity = r.Humidity,
                    OxygenFraction = r.OxygenFraction,
                    LocalCoPpm = r.LocalCoPpm,
                    IsOnFire = r.IsOnFire,
                    FireIntensity = r.FireIntensity,
                    BulkheadSealed = r.BulkheadSealed,
                    ModulesSacrificed = r.ModulesSacrificed,
                    HasMold = r.HasMold,
                    MoldLevel = r.MoldLevel,
                    Co2Ppm = r.Co2Ppm,
                    AmbientContamination = r.AmbientContamination,
                    AmbientRadiation = r.AmbientRadiation
                };
            }
            return save;
        }

        public void RestoreState(ShelterAtmosphereSave save)
        {
            if (save?.Rooms == null) return;
            for (int i = 0; i < save.Rooms.Length; i++)
            {
                var row = save.Rooms[i];
                if (row == null || string.IsNullOrEmpty(row.RoomId)) continue;
                var room = GetRoom(row.RoomId);
                if (room == null)
                {
                    room = new ShelterRoom(row.RoomId, null);
                    RegisterRoom(room);
                }
                room.Humidity = row.Humidity;
                room.OxygenFraction = row.OxygenFraction > 0f
                    ? row.OxygenFraction
                    : ShelterRoom.DefaultOxygenFraction;
                room.LocalCoPpm = row.LocalCoPpm;
                room.IsOnFire = row.IsOnFire;
                room.FireIntensity = row.FireIntensity;
                room.BulkheadSealed = row.BulkheadSealed;
                room.ModulesSacrificed = row.ModulesSacrificed;
                room.HasMold = row.HasMold;
                room.MoldLevel = row.MoldLevel;
                room.Co2Ppm = row.Co2Ppm;
                room.AmbientContamination = row.AmbientContamination;
                room.AmbientRadiation = row.AmbientRadiation;
            }
            OnAtmosphereChanged?.Invoke();
        }
    }

    [Serializable]
    public class ShelterAtmosphereSave
    {
        public ShelterRoomAtmosphereSave[] Rooms;
    }

    [Serializable]
    public class ShelterRoomAtmosphereSave
    {
        public string RoomId;
        public float Humidity;
        public float OxygenFraction;
        public float LocalCoPpm;
        public bool IsOnFire;
        public float FireIntensity;
        public bool BulkheadSealed;
        public bool ModulesSacrificed;
        public bool HasMold;
        public float MoldLevel;
        public float Co2Ppm;
        public float AmbientContamination;
        public float AmbientRadiation;
    }
}
