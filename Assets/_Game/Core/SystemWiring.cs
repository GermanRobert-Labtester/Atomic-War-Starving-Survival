using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Simulation; // CompostSystem, ChelationSystem, etc. (audit C-3 split)
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Per-tick wiring for the systems added in Prompts #119-#178. GameBootstrap
    /// calls <see cref="WireDaily"/> once per game-day (idempotent) from
    /// TickSystems. This file is the audit C-1 remediation: previously 22 of
    /// 26 newly added systems were constructed and save-wired but never ticked,
    /// so their state never advanced. Now each system is either:
    ///   1. Ticked here (state actually progresses), or
    ///   2. Marked as event-driven and tested via the SystemWiringTests suite.
    ///
    /// CompostSystem: daily waste intake is here; hour conversion tick is in
    /// GameBootstrap.TickCompost (per-substep registry entry "compost").
    ///
    /// The systems deliberately NOT ticked here (event-driven, AI/player only):
    ///   - ScrapWeaponSystem   (pure function)
    ///   - SterilizationSystem (event-driven: BoilTools)
    ///   - WindTurbineSystem   (event-driven: Build)
    ///   - AntibioticResistanceSystem (event-driven: TryUseExpired)
    ///   - InternalHaulingSystem (event-driven: HaulFromAirlock)
    ///   - WeaponMaintenanceSystem (event-driven: Fire, OilWeapon)
    ///   - TriageBoardSystem   (storage only: SetPermission)
    ///   - ResilienceSystem    (event-driven: OnTraumaSurvived)
    ///   - ExcavationSystem    (event-driven: ClearRubble)
    ///   - HiddenStorageSystem (storage: HideItem, RetrieveItem)
    ///   - TunnelingSystem     (event-driven: Tunnel)
    ///   - MaterialShieldingSystem (event-driven: UpgradeCeiling)
    ///   - AirlockSystem       (event-driven: BuildAirlock, ScavengerEnterAirlock)
    ///   - EscapeHatchSystem   (event-driven: Excavate, TriggerEvacuation)
    ///   - ClothingDegradationSystem (hour tick via GameBootstrap.TickClothing)
    ///
    /// Every system above has at least one EditMode test in
    /// Assets/Tests/EditMode/SystemWiringTests.cs.
    /// </summary>
    public class SystemWiring
    {
        private int _lastDailyRunDay = -1;

        /// <summary>Last day the daily housekeeping pass ran (for tests).</summary>
        public int LastDailyRunDay => _lastDailyRunDay;

        /// <summary>Number of daily passes run since construction (for tests).</summary>
        public int DailyRunCount { get; private set; }

        /// <summary>True after the first call to WireDaily (idempotency guard).</summary>
        public bool HasRunOnce => _lastDailyRunDay >= 0;

        /// <summary>Daily-tick dependencies for <see cref="WireDaily"/>.</summary>
        public sealed class DailyContext
        {
            public int CurrentDay;
            public CompostSystem Compost;
            public ChelationSystem Chelation;
            public RoomAestheticsSystem Aesthetics;
            public HamRadioSystem HamRadio;
            public PolypharmacySystem Polypharmacy;
            public CeilingCollapseSystem CeilingCollapse;
            public LocationQuestSystem LocationQuest;
            public Shelter.Shelter Shelter;
            public Inventory.Inventory Inventory;
            public List<Survivor> Survivors;
            public List<ShelterRoom> Rooms;
            public System.Random Rng;
            /// <summary>Prompt #200 — Thermodynamics warm-day streak.</summary>
            public ShelterPerkSystem ShelterPerks;
            public float IndoorTemperatureC = 15f;
        }

        /// <summary>
        /// Run all daily housekeeping. Idempotent for the same day.
        /// </summary>
        public void WireDaily(DailyContext ctx)
        {
            if (ctx == null) return;
            if (ctx.CurrentDay == _lastDailyRunDay) return;
            _lastDailyRunDay = ctx.CurrentDay;
            DailyRunCount++;

            TickCompost(ctx);
            TickChelation(ctx);
            TickAesthetics(ctx);
            TickHamRadio(ctx);
            TickPolypharmacy(ctx);
            TickCeilingCollapse(ctx);
            TickThermodynamicsWarmDay(ctx);
            ctx.LocationQuest?.TickDaily(ctx.CurrentDay);
        }

        private static void TickThermodynamicsWarmDay(DailyContext ctx)
        {
            if (ctx.ShelterPerks == null) return;
            ctx.ShelterPerks.RecordWarmDay(
                ctx.IndoorTemperatureC, ctx.Survivors, ctx.CurrentDay);
        }

        private static void TickCompost(DailyContext ctx)
        {
            if (ctx.Compost == null) return;
            int count = ctx.Survivors != null ? ctx.Survivors.Count : 0;
            ctx.Compost.DailyWasteFromSurvivors(count);
        }

        private static void TickChelation(DailyContext ctx)
        {
            if (ctx.Chelation == null || ctx.Survivors == null) return;
            for (int i = 0; i < ctx.Survivors.Count; i++)
            {
                var sv = ctx.Survivors[i];
                if (sv == null) continue;
                ctx.Chelation.AdvanceDay(sv.Id);
            }
        }

        private void TickAesthetics(DailyContext ctx)
        {
            if (ctx.Aesthetics == null || ctx.Rooms == null) return;
            for (int i = 0; i < ctx.Rooms.Count; i++)
            {
                var room = ctx.Rooms[i];
                if (room == null) continue;
                // Derive room quality from existing ShelterRoom fields.
                float lighting = room.OxygenFraction > 0.19f ? 1f : 0f;
                float temp = 20f;
                float hygiene = 1f - room.MoldLevel;
                float score = ctx.Aesthetics.CalculateScore(lighting, temp, hygiene, room.RoomId);
                float aura = ctx.Aesthetics.GetMoraleAura(score);
                ApplyAuraToOccupants(ctx.Survivors, room.RoomId, aura, ctx.CurrentDay);
            }
        }

        private static void TickHamRadio(DailyContext ctx)
        {
            if (ctx.HamRadio == null) return;
            bool radioTowerActive = false;
            if (ctx.Shelter != null)
            {
                var tower = ctx.Shelter.GetModule("radio_tower");
                radioTowerActive = tower != null && tower.IsOperational;
            }
            ctx.HamRadio.TickBroadcast(24f, radioTowerActive);
        }

        private static void TickPolypharmacy(DailyContext ctx)
        {
            if (ctx.Polypharmacy == null || ctx.Survivors == null) return;
            float nowGameHours = ctx.CurrentDay * 24f;
            for (int i = 0; i < ctx.Survivors.Count; i++)
            {
                var sv = ctx.Survivors[i];
                if (sv == null) continue;
                ctx.Polypharmacy.PruneStaleDoses(sv.Id, nowGameHours);
            }
        }

        private static void TickCeilingCollapse(DailyContext ctx)
        {
            if (ctx.CeilingCollapse == null || ctx.Rng == null) return;
            ctx.CeilingCollapse.DailyCollapseCheck(ctx.Shelter, ctx.Rng);
        }

        private static void ApplyAuraToOccupants(List<Survivor> survivors, string roomId, float aura, int currentDay)
        {
            if (survivors == null || string.IsNullOrEmpty(roomId) || aura == 0f) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (!string.Equals(sv.CurrentRoomId, roomId, StringComparison.Ordinal)) continue;
                // Aesthetics aura is small; apply directly to morale.
                sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale + aura, 0f, 100f);
            }
        }
    }
}
