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
        private AtomicWar._Game.UI.InternalHorrorSnapshot BuildInternalHorrorSnapshot()
        {
            var snap = new AtomicWar._Game.UI.InternalHorrorSnapshot
            {
                CareIntervalHours = AtomicWar._Game.Medical.MedicalSystem.ComaCareIntervalHours
            };

            FillCorpseSnapshot(snap);
            snap.ContaminatedFoodCount = Inventory != null
                ? Inventory.CountByType(ItemType.ContaminatedFood)
                : 0;
            snap.Fires = BuildFireRoomSnapshots();
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

            var fireList = new List<AtomicWar._Game.UI.FireRoomSnapshot>();
            var rooms = AtmosphereSystem.Rooms;
            for (int i = 0; i < rooms.Count; i++)
            {
                var r = rooms[i];
                if (r == null || !r.IsOnFire) continue;
                fireList.Add(new AtomicWar._Game.UI.FireRoomSnapshot
                {
                    RoomId = r.RoomId,
                    IsOnFire = true,
                    Intensity = r.FireIntensity,
                    OxygenFraction = r.OxygenFraction,
                    LocalCoPpm = r.LocalCoPpm,
                    BulkheadSealed = r.BulkheadSealed
                });
            }
            return fireList.ToArray();
        }

        private void FillComaSnapshot(AtomicWar._Game.UI.InternalHorrorSnapshot snap)
        {
            if (MedicalSystem == null || Survivors == null) return;

            var comaList = new List<AtomicWar._Game.UI.ComaPatientSnapshot>();
            bool anyUrgent = false;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (!MedicalSystem.IsComatose(sv)) continue;

                float sinceCare = ResolveComaHoursSinceCare(sv);
                bool needs = MedicalSystem.NeedsCare(sv);
                if (needs) anyUrgent = true;
                comaList.Add(new AtomicWar._Game.UI.ComaPatientSnapshot
                {
                    SurvivorId = sv.Id,
                    DisplayName = sv.DisplayName,
                    HoursSinceLastCare = sinceCare,
                    NeedsCare = needs
                });
            }
            snap.Comas = comaList.ToArray();
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
