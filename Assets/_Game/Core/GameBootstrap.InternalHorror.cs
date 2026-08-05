using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        // ── Reusable per-frame HUD snapshot buffers ──────────────────────
        // RefreshInternalHorrorHud() is driven from Update(), so it runs on
        // every rendered frame, but the state it reports (corpses, fires,
        // coma patients) only changes on simulation ticks. Building the
        // snapshot fresh allocated an InternalHorrorSnapshot, two Lists and
        // two arrays per frame -- roughly 480 allocations/second at 60fps,
        // all of it garbage identical to the previous frame.
        //
        // Reusing one snapshot instance is safe because
        // InternalHorrorHUD.ApplySnapshot copies the values it needs out
        // into its own view state; it never retains the instance.
        private readonly AtomicWar._Game.UI.InternalHorrorSnapshot _horrorSnapshot =
            new AtomicWar._Game.UI.InternalHorrorSnapshot();

        // Element pools grow to the high-water mark and are then reused, so a
        // steady fire/coma count allocates nothing.
        private readonly List<AtomicWar._Game.UI.FireRoomSnapshot> _firePool =
            new List<AtomicWar._Game.UI.FireRoomSnapshot>();
        private readonly List<AtomicWar._Game.UI.ComaPatientSnapshot> _comaPool =
            new List<AtomicWar._Game.UI.ComaPatientSnapshot>();

        // Exact-length views handed to the HUD, re-cut only when the count changes.
        private AtomicWar._Game.UI.FireRoomSnapshot[] _fireBuffer =
            Array.Empty<AtomicWar._Game.UI.FireRoomSnapshot>();
        private AtomicWar._Game.UI.ComaPatientSnapshot[] _comaBuffer =
            Array.Empty<AtomicWar._Game.UI.ComaPatientSnapshot>();

        private AtomicWar._Game.UI.InternalHorrorSnapshot BuildInternalHorrorSnapshot()
        {
            var snap = _horrorSnapshot;
            snap.CareIntervalHours = AtomicWar._Game.Medical.MedicalSystem.ComaCareIntervalHours;

            FillCorpseSnapshot(snap);
            snap.ContaminatedFoodCount = Inventory != null
                ? Inventory.CountByType(ItemType.ContaminatedFood)
                : 0;
            snap.Fires = BuildFireRoomSnapshots();

            // FillComaSnapshot early-returns when the medical system or roster
            // is absent. With a fresh snapshot that left Comas empty; with a
            // reused one it would leak the previous frame's patients, so the
            // coma fields are reset before the fill runs.
            snap.Comas = Array.Empty<AtomicWar._Game.UI.ComaPatientSnapshot>();
            snap.ComaCareUrgent = false;
            FillComaSnapshot(snap);
            return snap;
        }

        private void FillCorpseSnapshot(AtomicWar._Game.UI.InternalHorrorSnapshot snap)
        {
            int corpses = CorpseSystem != null
                ? CorpseSystem.CorpseCount
                : (Inventory != null ? Inventory.CountByType(ItemType.Corpse) : 0);
            snap.CorpseCount = corpses;
            float daylight = PhotoperiodSystem != null
                ? PhotoperiodSystem.EffectiveDaylightHours
                : 8f;
            snap.DaylightHoursAvailable = daylight;
            snap.CanBury = corpses > 0
                && daylight >= CorpseManagementSystem.BuryHours
                && FindFirstLivingSurvivor() != null;
        }

        private AtomicWar._Game.UI.FireRoomSnapshot[] BuildFireRoomSnapshots()
        {
            if (AtmosphereSystem?.Rooms == null)
                return Array.Empty<AtomicWar._Game.UI.FireRoomSnapshot>();

            var rooms = AtmosphereSystem.Rooms;
            int count = 0;
            for (int i = 0; i < rooms.Count; i++)
            {
                var r = rooms[i];
                if (r == null || !r.IsOnFire) continue;

                // Only ever allocate on a new high-water mark of burning rooms.
                if (count == _firePool.Count)
                    _firePool.Add(new AtomicWar._Game.UI.FireRoomSnapshot());

                var slot = _firePool[count];
                slot.RoomId = r.RoomId;
                slot.IsOnFire = true;
                slot.Intensity = r.FireIntensity;
                slot.OxygenFraction = r.OxygenFraction;
                slot.LocalCoPpm = r.LocalCoPpm;
                slot.BulkheadSealed = r.BulkheadSealed;
                count++;
            }

            if (count == 0) return Array.Empty<AtomicWar._Game.UI.FireRoomSnapshot>();
            if (_fireBuffer.Length != count)
                _fireBuffer = new AtomicWar._Game.UI.FireRoomSnapshot[count];
            for (int i = 0; i < count; i++) _fireBuffer[i] = _firePool[i];
            return _fireBuffer;
        }

        private void FillComaSnapshot(AtomicWar._Game.UI.InternalHorrorSnapshot snap)
        {
            if (MedicalSystem == null || Survivors == null) return;

            bool anyUrgent = false;
            int count = 0;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (!MedicalSystem.IsComatose(sv)) continue;

                float sinceCare = ResolveComaHoursSinceCare(sv);
                bool needs = MedicalSystem.NeedsCare(sv);
                if (needs) anyUrgent = true;

                if (count == _comaPool.Count)
                    _comaPool.Add(new AtomicWar._Game.UI.ComaPatientSnapshot());

                var slot = _comaPool[count];
                slot.SurvivorId = sv.Id;
                slot.DisplayName = sv.DisplayName;
                slot.HoursSinceLastCare = sinceCare;
                slot.NeedsCare = needs;
                count++;
            }

            if (count > 0)
            {
                if (_comaBuffer.Length != count)
                    _comaBuffer = new AtomicWar._Game.UI.ComaPatientSnapshot[count];
                for (int i = 0; i < count; i++) _comaBuffer[i] = _comaPool[i];
                snap.Comas = _comaBuffer;
            }
            snap.ComaCareUrgent = anyUrgent;
        }

        private float ResolveComaHoursSinceCare(Survivor sv)
        {
            var active = MedicalSystem.GetActive(sv);
            for (int a = 0; a < active.Count; a++)
            {
                if (active[a].AfflictionId == AtomicWar._Game.Medical.AfflictionSO.Ids.Coma)
                    return active[a].HoursSinceLastCare;
            }
            return 0f;
        }
    }
}
