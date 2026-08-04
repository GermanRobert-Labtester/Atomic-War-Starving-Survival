using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// One scavenge / expedition site on the proc-gen wasteland graph.
    /// Stores distance, rad profile id, loot table id, and encounter deck ids.
    /// ScriptableObject assets are bound later by Core when catalogs are available.
    /// </summary>
    [Serializable]
    public class MapNode
    {
        public string NodeId;
        public string DisplayName;
        public DangerRing Ring = DangerRing.Suburbs;

        /// <summary>Base travel hours from shelter along the shortest path (pre-weather).</summary>
        public float DistanceFromShelter;

        /// <summary>Layout angle in radians (deterministic polar placement).</summary>
        public float AngleRadians;

        /// <summary>Layout radius (ring index + jitter) for UI placement 0..1-ish.</summary>
        public float LayoutRadius;

        /// <summary>Authoritative ambient rads/hour at this node.</summary>
        public float TrueRad;

        /// <summary>What survivors have heard — shown for unsurveyed silhouettes.</summary>
        public float RumoredRad;

        /// <summary>snake_case id of a RadZoneProfile asset (optional bind).</summary>
        public string RadZoneProfileId;

        /// <summary>snake_case id of a LootTableSO asset.</summary>
        public string LootTableId;

        /// <summary>Encounter deck: ordered encounter ids for this node.</summary>
        public List<string> EncounterDeckIds = new List<string>();

        /// <summary>True once the player has visited or radio intel fully revealed the site.</summary>
        public bool IsRevealed;

        /// <summary>True once a survivor has physically reached this node.</summary>
        public bool IsVisited;

        /// <summary>Danger score used by expedition encounter weighting (1..5).</summary>
        public float DangerLevel = 1f;

        /// <summary>
        /// Civil-war unexploded ordnance remnant (Prompt #12). Hidden from the
        /// player; Reckless loot or Flee may detonate.
        /// </summary>
        public bool HasUxo;

        /// <summary>
        /// Prompt #14 — lethal rad pocket (shifting hotspot). Windstorms can
        /// move the pocket two path-hops away.
        /// </summary>
        public bool IsDeathZone;

        /// <summary>
        /// Prompt #15 — narrative Deserter's Stand site (empty checkpoint,
        /// mutual kill over food). Forced discovery beat on first arrival.
        /// </summary>
        public bool HasDeserterStand;

        public bool IsShelter => Ring == DangerRing.Shelter
            || string.Equals(NodeId, GeneratedMap.ShelterNodeId, StringComparison.Ordinal);

        /// <summary>
        /// Player-facing label: real name only when revealed; otherwise a silhouette tag.
        /// </summary>
        public string GetDisplayLabel()
        {
            if (IsShelter) return DisplayName;
            if (IsRevealed || IsVisited) return DisplayName;
            return RingSilhouetteName(Ring);
        }

        public static string RingSilhouetteName(DangerRing ring)
        {
            switch (ring)
            {
                case DangerRing.Suburbs: return "Silhouette — Suburbs";
                case DangerRing.CityOutskirts: return "Silhouette — Outskirts";
                case DangerRing.GroundZero: return "Silhouette — Far Zone";
                default: return "Silhouette";
            }
        }

        public MapNode Clone()
        {
            var copy = new MapNode
            {
                NodeId = NodeId,
                DisplayName = DisplayName,
                Ring = Ring,
                DistanceFromShelter = DistanceFromShelter,
                AngleRadians = AngleRadians,
                LayoutRadius = LayoutRadius,
                TrueRad = TrueRad,
                RumoredRad = RumoredRad,
                RadZoneProfileId = RadZoneProfileId,
                LootTableId = LootTableId,
                IsRevealed = IsRevealed,
                IsVisited = IsVisited,
                DangerLevel = DangerLevel,
                HasUxo = HasUxo,
                IsDeathZone = IsDeathZone,
                HasDeserterStand = HasDeserterStand,
                EncounterDeckIds = new List<string>()
            };
            if (EncounterDeckIds != null)
            {
                for (int i = 0; i < EncounterDeckIds.Count; i++)
                    copy.EncounterDeckIds.Add(EncounterDeckIds[i]);
            }
            return copy;
        }
    }

    /// <summary>Undirected edge between two map nodes with base travel hours.</summary>
    [Serializable]
    public class MapPath
    {
        public string FromNodeId;
        public string ToNodeId;
        /// <summary>Base path travel time in game-hours (before weather).</summary>
        public float BaseTravelHours;

        public MapPath() { }

        public MapPath(string from, string to, float hours)
        {
            FromNodeId = from;
            ToNodeId = to;
            BaseTravelHours = Mathf.Max(0.1f, hours);
        }

        public bool Connects(string a, string b)
        {
            return (FromNodeId == a && ToNodeId == b) || (FromNodeId == b && ToNodeId == a);
        }
    }

    /// <summary>UI / fog-of-war snapshot for one map node.</summary>
    [Serializable]
    public struct MapNodePlayerView
    {
        public string NodeId;
        public string Label;
        public DangerRing Ring;
        public float DistanceFromShelter;
        public float DisplayedRad;
        public float RumoredRad;
        public bool IsSilhouette;
        public bool IsRevealed;
        public bool IsVisited;
        public bool IsShelter;
        public float LayoutX;
        public float LayoutY;
        public string LootTableId;
        public float DangerLevel;
    }
}
