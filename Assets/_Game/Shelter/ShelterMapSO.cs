using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// ShelterMapSO defines a shelter layout: room count, traits, starting
    /// modules, debris type, and narrative flavor. Used by HouseToBunkerSystem
    /// to initialize the shelter at game start (Prompts #79-#84).
    /// </summary>
    [CreateAssetMenu(fileName = "ShelterLayout", menuName = "ASHFALL/Shelter/Shelter Layout")]
    public class ShelterMapSO : ScriptableObject
    {
        public string layoutId;
        public string layoutName;
        [TextArea(2, 4)] public string description;

        [Header("Rooms")]
        public int roomCount = 3;
        public string[] roomIds;
        public string[] roomNames;
        public float[] roomSizes; // 1=small, 2=medium, 3=large

        [Header("Starting Modules")]
        public ShelterLayoutModule[] startingModules;

        [Header("Traits")]
        public ShelterLayoutTrait[] traits;

        [Header("Debris")]
        public DebrisType debrisType = DebrisType.WoodRubble;
        public float startingDebrisHours = 12f;

        [Header("Shielding")]
        public float inherentShielding; // rad attenuation fraction 0..1
        public float debrisShieldingBonus = 0.2f; // added when house collapses

        [Header("Water")]
        public float startingCleanWater;
        public bool hasWaterHeater;

        [Header("Structural")]
        public float startingHouseDurability = 100f;
        public float startingIntegrity = 100f;
        public float integrityDegradeMultiplier = 1f;

        [Header("Security")]
        public float startingHatchSecurity;
        public float startingHatchDamage;

        [Header("Anomalies")]
        public string[] anomalies; // "shared_pipe", "flooded", "exposed_hatch", etc.
    }

    public enum DebrisType
    {
        WoodRubble,
        ConcreteRubble,
        BrickRubble,
        MetalDebris,
        Dirt
    }

    public enum ShelterLayoutTrait
    {
        RootCellar,        // High food, high mold
        SharedWalls,       // Adjacent buildings
        DeepUnderground,   // Max rad shielding
        ExposedHatch,      // No rubble, vulnerable
        WaterHeater,       // Dismantle for 20 clean water
        DirtFloor,         // Structural degrades faster
        PrepperCache,      // Pre-installed broken modules
        Flooded,           // Must pump water first
        SharedPipe,        // Water drains/tainted flows in
        FallenBeam         // Blocked stairs, need Saw
    }

    [Serializable]
    public class ShelterLayoutModule
    {
        public string moduleId;
        public int level = 1;
        public bool isEnabled = true;
        public float fuel;
        public float filterHealth = 100f;
        public string roomId;
    }
}
