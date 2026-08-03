using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Drives scavenging and survey missions to LocationDefinitionSO sites.
    /// Exposure always uses TrueRad (sim truth). Player planning uses
    /// RadiationKnowledgeMap views — the dread of uncertain safety.
    /// </summary>
    public class LocationScavengingSystem
    {
        /// <summary>Hours spent taking readings on a survey mission (on top of travel).</summary>
        public const float SurveyHours = 1f;

        private readonly RadiationSystem _radSystem;
        private readonly Inventory.Inventory _inventory;
        private readonly ItemCatalogSO _itemCatalog;
        private readonly RadiationKnowledgeMap _knowledge;
        private readonly Func<int> _getCurrentDay;
        private readonly System.Random _rng;
        private readonly List<ActiveMission> _active = new List<ActiveMission>();

        public event Action<ActiveMission> OnMissionStarted;
        public event Action<ActiveMission, List<ItemDefinition>> OnMissionCompleted;
        public event Action<ActiveMission, bool> OnSurveyCompleted;

        public LocationScavengingSystem(
            RadiationSystem radSystem,
            Inventory.Inventory inventory,
            ItemCatalogSO itemCatalog,
            int seed = 42,
            RadiationKnowledgeMap knowledge = null,
            Func<int> getCurrentDay = null)
        {
            _radSystem = radSystem;
            _inventory = inventory;
            _itemCatalog = itemCatalog;
            _knowledge = knowledge;
            _getCurrentDay = getCurrentDay ?? (() => 0);
            _rng = new System.Random(seed);
        }

        public IReadOnlyList<ActiveMission> ActiveMissions => _active;
        public RadiationKnowledgeMap Knowledge => _knowledge;

        /// <summary>Start a scavenging mission to a location. Returns false if survivor is dead or already on mission.</summary>
        public bool StartMission(Survivor survivor, LocationDefinitionSO location)
        {
            if (survivor == null || !survivor.IsAlive || location == null) return false;
            if (IsOnMission(survivor.Id)) return false;

            float trueRad = ResolveTrueRad(location);

            var mission = new ActiveMission
            {
                SurvivorId = survivor.Id,
                LocationId = location.id,
                LocationName = location.displayName,
                HoursRemaining = location.travelHours,
                TotalHours = location.travelHours,
                RadPerHour = trueRad,
                DangerLevel = location.dangerLevel,
                Kind = MissionKind.Scavenge,
                Survivor = survivor
            };

            _active.Add(mission);
            OnMissionStarted?.Invoke(mission);
            return true;
        }

        /// <summary>
        /// Start a survey mission: travel + SurveyHours of readings with a working geiger.
        /// Fails if no working geiger is in inventory. Exposure uses TrueRad; the recorded
        /// measurement is biased by the device's calibration.
        /// </summary>
        public bool StartSurvey(Survivor survivor, LocationDefinitionSO location)
        {
            if (survivor == null || !survivor.IsAlive || location == null) return false;
            if (IsOnMission(survivor.Id)) return false;
            if (_inventory == null || !_inventory.HasWorkingGeiger()) return false;

            float trueRad = ResolveTrueRad(location);
            float hours = location.travelHours + SurveyHours;

            var mission = new ActiveMission
            {
                SurvivorId = survivor.Id,
                LocationId = location.id,
                LocationName = location.displayName,
                HoursRemaining = hours,
                TotalHours = hours,
                RadPerHour = trueRad,
                DangerLevel = location.dangerLevel,
                Kind = MissionKind.Survey,
                Survivor = survivor
            };

            _active.Add(mission);
            OnMissionStarted?.Invoke(mission);
            return true;
        }

        /// <summary>
        /// Immediate survey resolution for tests / same-tile bunker checks: no travel,
        /// just attempt a reading with the best working geiger.
        /// </summary>
        public bool TryImmediateSurvey(string locationId, out float measuredRad)
        {
            measuredRad = 0f;
            if (_knowledge == null || string.IsNullOrEmpty(locationId)) return false;
            if (_inventory == null) return false;

            var slot = _inventory.FindBestWorkingDevice("geiger_counter");
            if (slot?.Device == null) return false;

            float trueRad = _knowledge.GetTrueRad(locationId);
            if (!InstrumentDevice.TryRead(slot.Device, trueRad, out measuredRad)) return false;

            int day = _getCurrentDay();
            InstrumentDevice.DrainBattery(slot.Device, InstrumentDevice.BatteryDrainPerSurvey);
            _knowledge.RecordSurvey(locationId, measuredRad, slot.Device.Calibration, day);
            return true;
        }

        /// <summary>Advance active missions over elapsed game hours.</summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var mission = _active[i];
                // Cap exposure to time actually spent on the mission (no overshoot dose)
                float elapsed = Mathf.Min(gameHours, Mathf.Max(0f, mission.HoursRemaining));
                mission.HoursRemaining -= gameHours;

                // Accumulate radiation during travel/survey — always TrueRad
                if (_radSystem != null && mission.Survivor != null && mission.Survivor.IsAlive
                    && elapsed > 0f)
                {
                    _radSystem.Expose(mission.Survivor, mission.RadPerHour, elapsed);
                }

                if (mission.HoursRemaining <= 0f)
                {
                    if (mission.Kind == MissionKind.Survey)
                    {
                        CompleteSurvey(mission);
                    }
                    else
                    {
                        CompleteMission(mission);
                    }
                    _active.RemoveAt(i);
                }
            }
        }

        private void CompleteMission(ActiveMission mission)
        {
            var loot = RollLoot(mission.DangerLevel);
            if (_inventory != null && loot.Count > 0)
            {
                foreach (var item in loot)
                {
                    _inventory.Add(item, 1);
                }
            }
            OnMissionCompleted?.Invoke(mission, loot);
        }

        private void CompleteSurvey(ActiveMission mission)
        {
            bool success = false;
            if (_inventory != null && _knowledge != null)
            {
                var slot = _inventory.FindBestWorkingDevice("geiger_counter");
                if (slot?.Device != null
                    && InstrumentDevice.TryRead(slot.Device, mission.RadPerHour, out float measured))
                {
                    int day = _getCurrentDay();
                    InstrumentDevice.DrainBattery(slot.Device, InstrumentDevice.BatteryDrainPerSurvey);
                    success = _knowledge.RecordSurvey(
                        mission.LocationId, measured, slot.Device.Calibration, day);
                }
            }

            // Survey still returns empty loot list for API symmetry
            OnMissionCompleted?.Invoke(mission, new List<ItemDefinition>());
            OnSurveyCompleted?.Invoke(mission, success);
        }

        private float ResolveTrueRad(LocationDefinitionSO location)
        {
            if (_knowledge != null)
            {
                var tile = _knowledge.GetTile(location.id);
                if (tile != null) return tile.TrueRad;
            }
            return location.baseRadsPerHour;
        }

        private bool IsOnMission(string survivorId)
        {
            for (int i = 0; i < _active.Count; i++)
            {
                if (_active[i].SurvivorId == survivorId) return true;
            }
            return false;
        }

        private List<ItemDefinition> RollLoot(float dangerLevel)
        {
            var loot = new List<ItemDefinition>();
            if (_itemCatalog == null || _itemCatalog.items.Count == 0) return loot;

            int itemCount = 1 + (int)(dangerLevel / 3f);
            itemCount = Mathf.Clamp(itemCount, 1, 4);

            for (int i = 0; i < itemCount; i++)
            {
                float chance = 0.6f + dangerLevel * 0.03f;
                if (_rng.NextDouble() < chance)
                {
                    var item = _itemCatalog.items[_rng.Next(_itemCatalog.items.Count)];
                    if (item != null)
                    {
                        loot.Add(item);
                    }
                }
            }

            return loot;
        }
    }

    public enum MissionKind
    {
        Scavenge,
        Survey
    }

    [Serializable]
    public class ActiveMission
    {
        public string SurvivorId;
        public string LocationId;
        public string LocationName;
        public float HoursRemaining;
        public float TotalHours;
        public float RadPerHour;
        public float DangerLevel;
        public MissionKind Kind;
        [NonSerialized] public Survivor Survivor;
    }
}
