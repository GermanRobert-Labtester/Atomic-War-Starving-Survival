using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — 10 new
    /// scavenging locations authored as a data-driven catalog builder.
    /// Mirrors the <see cref="Inventory.AshGetsDeeperItemsCatalog"/>
    /// pattern: pure C# Spec rows that materialise through
    /// <c>MaterialiseAll</c> with a host-supplied lookup, ready to be
    /// JSON-imported later. The four columns in the brief
    /// (Danger, Travel, BaseRads, Description) map directly to
    /// <see cref="LocationDefinitionSO"/>'s existing fields.
    /// </summary>
    public static class AshGetsDeeperLocationsCatalog
    {
        public static class Ids
        {
            public const string CollapsedSchool        = "collapsed_school";
            public const string IrradiatedGreenhouse   = "irradiated_greenhouse";
            public const string FloodedSubwayStation   = "flooded_subway_station";
            public const string PharmacyChainRuin     = "pharmacy_chain_ruin";
            public const string MilitaryCheckpointWreck = "military_checkpoint_wreck";
            public const string WaterTreatmentPlant    = "water_treatment_plant";
            public const string HighwayPileup          = "highway_pileup";
            public const string ChurchOfStMaren        = "church_of_st_maren";
            public const string RadioTowerBase         = "radio_tower_base";
            public const string PetCemetery            = "pet_cemetery";
        }

        public class Spec
        {
            public string Id;
            public string DisplayName;
            public int DangerLevel;          // 1..10
            public float TravelHours;
            public float BaseRadsPerHour;
            [TextArea(2, 4)] public string Description;
        }

        public static List<Spec> BuildAll()
        {
            var list = new List<Spec>
            {
                new Spec {
                    Id = Ids.CollapsedSchool, DisplayName = "The School on Vellum Street",
                    DangerLevel = 4, TravelHours = 1.5f, BaseRadsPerHour = 18f,
                    Description = "The roof caved in on the east wing. The west classrooms are intact. Desks still in rows. Names still on the coat hooks. The cafeteria has canned stock — if you can get past the smell in the gymnasium."
                },
                new Spec {
                    Id = Ids.IrradiatedGreenhouse, DisplayName = "Suki's Dead Greenhouse",
                    DangerLevel = 5, TravelHours = 2.0f, BaseRadsPerHour = 28f,
                    Description = "The glass panels held during the blast. Inside, the plants are wrong — pale, overgrown, faintly warm to the touch. The soil reads hot. But the seed stock in the back office is sealed in lead-lined boxes."
                },
                new Spec {
                    Id = Ids.FloodedSubwayStation, DisplayName = "Tessarat Metro — Platform 3",
                    DangerLevel = 6, TravelHours = 2.5f, BaseRadsPerHour = 22f,
                    Description = "The tunnels flooded when the mains burst. Waist-deep black water. Something floats in the maintenance bay. The ticket office is dry and locked. The lock is the easy part."
                },
                new Spec {
                    Id = Ids.PharmacyChainRuin, DisplayName = "PharmaPlus (Collapsed)",
                    DangerLevel = 4, TravelHours = 1.0f, BaseRadsPerHour = 12f,
                    Description = "Half the building is rubble. The pharmacy counter is behind a security gate. The gate is intact. The shelves behind it might still have antibiotics, iodine, morphine. The back room is flooded."
                },
                new Spec {
                    Id = Ids.MilitaryCheckpointWreck, DisplayName = "Checkpoint Kilo (Overrun)",
                    DangerLevel = 7, TravelHours = 3.0f, BaseRadsPerHour = 40f,
                    Description = "Sandbag ring, burnt-out APC, body bags still in a row. Whoever overran this position didn't stay. The ammo crate is still bolted to the truck bed. The truck bed is still on fire, low and slow, three weeks later."
                },
                new Spec {
                    Id = Ids.WaterTreatmentPlant, DisplayName = "Tessarat Water Works",
                    DangerLevel = 6, TravelHours = 3.5f, BaseRadsPerHour = 30f,
                    Description = "The filtration tanks are intact but contaminated. The chemical storage room has chlorine, alum, and activated carbon. The control room has the pump schematics. The basement is where the night shift was when it happened."
                },
                new Spec {
                    Id = Ids.HighwayPileup, DisplayName = "Highway 9 Pileup",
                    DangerLevel = 5, TravelHours = 2.0f, BaseRadsPerHour = 20f,
                    Description = "Forty cars, three trucks, a fuel tanker. Frozen in the ash like insects in amber. Siphon fuel from the tanks. Strip batteries. One of the trucks has a sealed cargo container. The lock is military-grade."
                },
                new Spec {
                    Id = Ids.ChurchOfStMaren, DisplayName = "Church of St. Maren",
                    DangerLevel = 3, TravelHours = 1.5f, BaseRadsPerHour = 8f,
                    Description = "The steeple survived. The nave didn't. Pews are splintered. The altar cloth is ash-grey but intact. The crypt below is sealed and cold. Someone has been leaving candles at the door. Recently."
                },
                new Spec {
                    Id = Ids.RadioTowerBase, DisplayName = "Broadcast Tower 7 (Base Station)",
                    DangerLevel = 7, TravelHours = 4.0f, BaseRadsPerHour = 45f,
                    Description = "The tower itself is down, snapped at the third guy-wire. But the base station building is shielded. Inside: broadcast equipment, a backup generator, and a sealed frequency log. The military used this site. The codes might still work."
                },
                new Spec {
                    Id = Ids.PetCemetery, DisplayName = "Hillside Pet Cemetery",
                    DangerLevel = 2, TravelHours = 1.0f, BaseRadsPerHour = 6f,
                    Description = "Small plots, hand-painted markers. The soil is surprisingly clean — the hillside faced away from the blast. Someone buried more than pets here. The fresh mounds are from after the exchange. A tin box is half-exposed near the fence."
                },
            };
            return list;
        }

        public static LocationDefinitionSO Materialise(Spec spec)
        {
            if (spec == null) return null;
            var so = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            so.id = spec.Id;
            so.displayName = spec.DisplayName;
            so.description = spec.Description ?? string.Empty;
            so.dangerLevel = spec.DangerLevel;
            so.travelHours = spec.TravelHours;
            so.baseRadsPerHour = spec.BaseRadsPerHour;
            return so;
        }

        public static List<LocationDefinitionSO> MaterialiseAll()
        {
            var specs = BuildAll();
            var outList = new List<LocationDefinitionSO>(specs.Count);
            for (int i = 0; i < specs.Count; i++) outList.Add(Materialise(specs[i]));
            return outList;
        }
    }
}
