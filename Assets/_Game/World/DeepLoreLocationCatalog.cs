using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion IX/X — Deep-Lore Locations with Variable Loot Tables.
    /// Each location defines its own procedural loot using VariableLootNode arrays.
    /// Environmental storytelling, hazards, and encounters integrated.
    /// </summary>
    public static class DeepLoreLocationCatalog
    {
        // ── Location 1: Municipal Library (The Pyre of History) ───────
        public const string Loc_Library = "location_municipal_library";
        public const float Lib_Rads = 12f;
        public const int Lib_Danger = 4;
        public const int Lib_Travel = 2;

        public static List<VariableLootNode> LibraryLoot => new List<VariableLootNode>
        {
            new VariableLootNode { ItemId = "book", MinQty = 0, MaxQty = 12, SpawnChance = 0.65f, DegradationChance = 0.40f, DegradedItemId = "paper_scrap" },
            new VariableLootNode { ItemId = "wood_block", MinQty = 2, MaxQty = 18, SpawnChance = 0.80f },
            new VariableLootNode { ItemId = "scrap_wood", MinQty = 10, MaxQty = 45, SpawnChance = 0.95f },
            new VariableLootNode { ItemId = "cloth", MinQty = 4, MaxQty = 15, SpawnChance = 0.70f },
            new VariableLootNode { ItemId = "cigarette_lighter", MinQty = 0, MaxQty = 3, SpawnChance = 0.25f },
            new VariableLootNode { ItemId = "family_photograph", MinQty = 0, MaxQty = 6, SpawnChance = 0.40f },
            new VariableLootNode { ItemId = "paper_scrap", MinQty = 20, MaxQty = 100, SpawnChance = 0.90f },
            new VariableLootNode { ItemId = "sealed_government_document", MinQty = 0, MaxQty = 1, SpawnChance = 0.05f }
        };

        // ── Location 2: Sunshine Daycare (The Nap Time) ───────────────
        public const string Loc_Daycare = "location_sunshine_daycare";
        public const float Daycare_Rads = 8f;
        public const int Daycare_Danger = 3;
        public const int Daycare_Travel = 2; // 1.5h

        public static List<VariableLootNode> DaycareLoot => new List<VariableLootNode>
        {
            new VariableLootNode { ItemId = "water_bottle_0_5l_of_1l", MinQty = 2, MaxQty = 14, SpawnChance = 0.85f },
            new VariableLootNode { ItemId = "canned_food", MinQty = 1, MaxQty = 8, SpawnChance = 0.75f },
            new VariableLootNode { ItemId = "bandage", MinQty = 2, MaxQty = 10, SpawnChance = 0.80f },
            new VariableLootNode { ItemId = "item_teddy_bear", MinQty = 0, MaxQty = 4, SpawnChance = 0.60f },
            new VariableLootNode { ItemId = "wool_blanket", MinQty = 1, MaxQty = 6, SpawnChance = 0.90f },
            new VariableLootNode { ItemId = "crayon", MinQty = 5, MaxQty = 30, SpawnChance = 0.85f },
            new VariableLootNode { ItemId = "pistol_cz75_9x19", MinQty = 0, MaxQty = 1, SpawnChance = 0.15f },
            new VariableLootNode { ItemId = "ammo_9x19", MinQty = 0, MaxQty = 8, SpawnChance = 0.15f }
        };

        // ── Location 3: Regional Blood Bank (The Broken Chain) ────────
        public const string Loc_BloodBank = "location_regional_blood_bank";
        public const float BloodBank_Rads = 18f;
        public const int BloodBank_Danger = 6;
        public const int BloodBank_Travel = 3;

        public static List<VariableLootNode> BloodBankLoot => new List<VariableLootNode>
        {
            new VariableLootNode { ItemId = "antiseptic_1l_of_1l", MinQty = 0, MaxQty = 6, SpawnChance = 0.55f },
            new VariableLootNode { ItemId = "alcohol_wipes_box_10_of_10", MinQty = 2, MaxQty = 15, SpawnChance = 0.85f },
            new VariableLootNode { ItemId = "battery", MinQty = 4, MaxQty = 25, SpawnChance = 0.90f },
            new VariableLootNode { ItemId = "mechanical_parts", MinQty = 5, MaxQty = 20, SpawnChance = 0.80f },
            new VariableLootNode { ItemId = "rubber_hose", MinQty = 3, MaxQty = 12, SpawnChance = 0.75f },
            new VariableLootNode { ItemId = "blood_bag", MinQty = 0, MaxQty = 3, SpawnChance = 0.10f, DegradationChance = 0.90f, DegradedItemId = "spoiled_blood_bag" },
            new VariableLootNode { ItemId = "clean_water", MinQty = 0, MaxQty = 8, SpawnChance = 0.40f }
        };

        // ── Location 4: Grand Cinema (The Final Feature) ──────────────
        public const string Loc_Cinema = "location_grand_cinema";
        public const float Cinema_Rads = 14f;
        public const int Cinema_Danger = 5;
        public const int Cinema_Travel = 3; // 2.5h

        public static List<VariableLootNode> CinemaLoot => new List<VariableLootNode>
        {
            new VariableLootNode { ItemId = "fuel_1l", MinQty = 0, MaxQty = 12, SpawnChance = 0.60f },
            new VariableLootNode { ItemId = "sugar", MinQty = 5, MaxQty = 40, SpawnChance = 0.95f },
            new VariableLootNode { ItemId = "electronic_scrap", MinQty = 4, MaxQty = 22, SpawnChance = 0.85f },
            new VariableLootNode { ItemId = "cloth", MinQty = 15, MaxQty = 50, SpawnChance = 0.90f },
            new VariableLootNode { ItemId = "vacuum_tube", MinQty = 1, MaxQty = 8, SpawnChance = 0.45f },
            new VariableLootNode { ItemId = "water_bottle_1l_full", MinQty = 3, MaxQty = 20, SpawnChance = 0.75f },
            new VariableLootNode { ItemId = "duct_tape", MinQty = 2, MaxQty = 10, SpawnChance = 0.80f }
        };

        // ── Location 5: Logging Camp (The Sawdust Fortress) ───────────
        public const string Loc_LoggingCamp = "location_upland_logging_camp";
        public const float LoggingCamp_Rads = 20f;
        public const int LoggingCamp_Danger = 7;
        public const int LoggingCamp_Travel = 5; // 4.5h

        public static List<VariableLootNode> LoggingCampLoot => new List<VariableLootNode>
        {
            new VariableLootNode { ItemId = "wood_block", MinQty = 20, MaxQty = 80, SpawnChance = 1.0f },
            new VariableLootNode { ItemId = "scrap_wood", MinQty = 30, MaxQty = 100, SpawnChance = 1.0f },
            new VariableLootNode { ItemId = "sawdust_block", MinQty = 10, MaxQty = 50, SpawnChance = 0.90f },
            new VariableLootNode { ItemId = "box_of_nails_10", MinQty = 2, MaxQty = 15, SpawnChance = 0.85f },
            new VariableLootNode { ItemId = "fuel_1l", MinQty = 2, MaxQty = 12, SpawnChance = 0.70f },
            new VariableLootNode { ItemId = "mechanical_parts", MinQty = 5, MaxQty = 20, SpawnChance = 0.80f },
            new VariableLootNode { ItemId = "military_grade_hatchet", MinQty = 0, MaxQty = 3, SpawnChance = 0.25f },
            new VariableLootNode { ItemId = "item_ash_ghillie", MinQty = 0, MaxQty = 1, SpawnChance = 0.10f }
        };

        // ── Location 6: Stadium Evacuation Center ─────────────────────
        public const string Loc_Stadium = "location_stadium_evacuation_center";
        public const float Stadium_Rads = 18f;
        public const int Stadium_Danger = 6;
        public const int Stadium_Travel = 4; // 3.5h

        public static List<VariableLootNode> StadiumLoot => new List<VariableLootNode>
        {
            new VariableLootNode { ItemId = "cloth", MinQty = 20, MaxQty = 120, SpawnChance = 0.95f },
            new VariableLootNode { ItemId = "family_photograph", MinQty = 5, MaxQty = 40, SpawnChance = 0.80f },
            new VariableLootNode { ItemId = "currency", MinQty = 100, MaxQty = 500, SpawnChance = 0.90f },
            new VariableLootNode { ItemId = "canned_food", MinQty = 0, MaxQty = 15, SpawnChance = 0.45f, DegradationChance = 0.60f, DegradedItemId = "spoiled_canned_food" },
            new VariableLootNode { ItemId = "jewelry", MinQty = 0, MaxQty = 8, SpawnChance = 0.20f },
            new VariableLootNode { ItemId = "water_bottle_1l_full", MinQty = 2, MaxQty = 20, SpawnChance = 0.60f },
            new VariableLootNode { ItemId = "item_suitcase_locked", MinQty = 0, MaxQty = 5, SpawnChance = 0.30f }
        };

        // ── Location 7: Automated Abattoir ────────────────────────────
        public const string Loc_Abattoir = "location_automated_abattoir";
        public const float Abattoir_Rads = 12f;
        public const int Abattoir_Danger = 7;
        public const int Abattoir_Travel = 4;

        public static List<VariableLootNode> AbattoirLoot => new List<VariableLootNode>
        {
            new VariableLootNode { ItemId = "raw_meat", MinQty = 30, MaxQty = 150, SpawnChance = 1.0f, DegradationChance = 0.50f, DegradedItemId = "spoiled_meat" },
            new VariableLootNode { ItemId = "fat_rendered", MinQty = 10, MaxQty = 60, SpawnChance = 0.85f },
            new VariableLootNode { ItemId = "industrial_bleach", MinQty = 2, MaxQty = 12, SpawnChance = 0.70f },
            new VariableLootNode { ItemId = "bone_saw", MinQty = 0, MaxQty = 4, SpawnChance = 0.40f },
            new VariableLootNode { ItemId = "mechanical_parts", MinQty = 10, MaxQty = 40, SpawnChance = 0.90f },
            new VariableLootNode { ItemId = "rubber_hose", MinQty = 5, MaxQty = 25, SpawnChance = 0.80f },
            new VariableLootNode { ItemId = "ammonia_tank", MinQty = 0, MaxQty = 3, SpawnChance = 0.25f }
        };

        // ── Location 8: Central Postal Hub ────────────────────────────
        public const string Loc_PostalHub = "location_central_postal_hub";
        public const float PostalHub_Rads = 14f;
        public const int PostalHub_Danger = 4;
        public const int PostalHub_Travel = 3; // 2.5h

        public static List<VariableLootNode> PostalHubLoot => new List<VariableLootNode>
        {
            new VariableLootNode { ItemId = "paper_scrap", MinQty = 30, MaxQty = 150, SpawnChance = 1.0f },
            new VariableLootNode { ItemId = "cardboard_box", MinQty = 5, MaxQty = 25, SpawnChance = 0.85f, DegradationChance = 0.60f, DegradedItemId = "paper_scrap" },
            new VariableLootNode { ItemId = "duct_tape", MinQty = 2, MaxQty = 12, SpawnChance = 0.75f },
            new VariableLootNode { ItemId = "family_photograph", MinQty = 0, MaxQty = 15, SpawnChance = 0.60f },
            new VariableLootNode { ItemId = "book", MinQty = 0, MaxQty = 8, SpawnChance = 0.40f },
            new VariableLootNode { ItemId = "sealed_government_document", MinQty = 0, MaxQty = 2, SpawnChance = 0.05f },
            new VariableLootNode { ItemId = "cigarette_pack_sealed", MinQty = 0, MaxQty = 4, SpawnChance = 0.15f },
            new VariableLootNode { ItemId = "brass_fittings", MinQty = 1, MaxQty = 6, SpawnChance = 0.30f }
        };

        // ── Location 9: Municipal Water Reservoir ─────────────────────
        public const string Loc_Reservoir = "location_municipal_water_reservoir";
        public const float Reservoir_Rads = 25f;
        public const int Reservoir_Danger = 8;
        public const int Reservoir_Travel = 5; // 4.5h

        public static List<VariableLootNode> ReservoirLoot => new List<VariableLootNode>
        {
            new VariableLootNode { ItemId = "chemicals", MinQty = 10, MaxQty = 60, SpawnChance = 0.90f },
            new VariableLootNode { ItemId = "water_purification_tablets_40_of_40", MinQty = 0, MaxQty = 4, SpawnChance = 0.25f },
            new VariableLootNode { ItemId = "pipe_wrench", MinQty = 0, MaxQty = 2, SpawnChance = 0.40f },
            new VariableLootNode { ItemId = "mechanical_parts", MinQty = 5, MaxQty = 25, SpawnChance = 0.80f },
            new VariableLootNode { ItemId = "scrap_metal", MinQty = 15, MaxQty = 50, SpawnChance = 0.95f },
            new VariableLootNode { ItemId = "ammo_762x54r_jhp_ap", MinQty = 0, MaxQty = 10, SpawnChance = 0.30f }
        };

        // ── Location 10: TV Studio (Channel 4) ────────────────────────
        public const string Loc_TVStudio = "location_television_studio";
        public const float TVStudio_Rads = 15f;
        public const int TVStudio_Danger = 4;
        public const int TVStudio_Travel = 2;

        public static List<VariableLootNode> TVStudioLoot => new List<VariableLootNode>
        {
            new VariableLootNode { ItemId = "electronic_scrap", MinQty = 15, MaxQty = 80, SpawnChance = 0.95f },
            new VariableLootNode { ItemId = "vacuum_tube", MinQty = 2, MaxQty = 15, SpawnChance = 0.60f },
            new VariableLootNode { ItemId = "acoustic_foam_panel", MinQty = 5, MaxQty = 30, SpawnChance = 0.85f },
            new VariableLootNode { ItemId = "cassette_tape", MinQty = 0, MaxQty = 12, SpawnChance = 0.50f },
            new VariableLootNode { ItemId = "battery", MinQty = 5, MaxQty = 25, SpawnChance = 0.75f },
            new VariableLootNode { ItemId = "item_anchor_notes", MinQty = 1, MaxQty = 1, SpawnChance = 1.0f },
            new VariableLootNode { ItemId = "duct_tape", MinQty = 2, MaxQty = 10, SpawnChance = 0.80f }
        };
    }
}
