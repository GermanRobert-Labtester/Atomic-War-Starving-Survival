// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// Describes the in-world acquisition path for one pre-war vinyl record.
    /// </summary>
    public sealed class VinylAcquisitionRoute
    {
        public string RecordId { get; set; } = string.Empty;
        public string AssociatedItemId { get; set; } = string.Empty;
        public string AcquisitionChannel { get; set; } = string.Empty; // scavenging, expedition, barter, cultural_vault
        public string LocationType { get; set; } = string.Empty;
        public string RarityTier { get; set; } = "rare"; // common, uncommon, rare, very_rare
        public int DiscoveryWeight { get; set; } = 10;
    }

    /// <summary>
    /// Master acquisition map connecting the 30 Pre-War Radio Music Archive records
    /// to physical inventory items, scavenging tables, expedition events, and barter sources.
    /// Engine-agnostic and deterministic.
    /// </summary>
    public static class VinylRecordAcquisitionMap
    {
        public const string DefaultVinylCrateItemId = "item_vinyl_collection";
        public const string ChamberRecordCollectibleItemId = "item_collectible_vinyl_chamber_record";
        public const string CivilBroadcastCollectibleItemId = "item_collectible_vinyl_civil_broadcast";
        public const string FolkCompilationCollectibleItemId = "item_collectible_vinyl_folk_compilation";

        private static readonly Dictionary<string, VinylAcquisitionRoute> RoutesByRecordId =
            new Dictionary<string, VinylAcquisitionRoute>(StringComparer.OrdinalIgnoreCase);

        private static readonly List<VinylAcquisitionRoute> AllRoutesList = new List<VinylAcquisitionRoute>();

        static VinylRecordAcquisitionMap()
        {
            RegisterAllRoutes();
        }

        private static void Register(string recordId, string itemId, string channel, string locationType, string rarity, int weight)
        {
            var route = new VinylAcquisitionRoute
            {
                RecordId = recordId,
                AssociatedItemId = itemId,
                AcquisitionChannel = channel,
                LocationType = locationType,
                RarityTier = rarity,
                DiscoveryWeight = weight
            };
            RoutesByRecordId[recordId] = route;
            AllRoutesList.Add(route);
        }

        private static void RegisterAllRoutes()
        {
            // All 30 records from Assets/StreamingAssets/Data/narrative/vinyl_record_archive.json
            Register("record_01_valse_triste_sibelius_78rpm", DefaultVinylCrateItemId, "scavenging", "apartment_block", "rare", 10);
            Register("record_02_autumn_leaves_dark_eyes_gypsy_trio", FolkCompilationCollectibleItemId, "barter", "apartment_block", "uncommon", 15);
            Register("record_03_rachmaninoff_piano_concerto_2_adagio", ChamberRecordCollectibleItemId, "scavenging", "concert_hall", "very_rare", 5);
            Register("record_04_midnight_in_moscow_jazz_octet", CivilBroadcastCollectibleItemId, "expedition", "radio_station", "rare", 10);
            Register("record_05_song_of_the_volga_boatmen_choral", FolkCompilationCollectibleItemId, "scavenging", "church", "common", 20);
            Register("record_06_claire_de_lune_debussy_harp_flute", ChamberRecordCollectibleItemId, "scavenging", "concert_hall", "rare", 10);
            Register("record_07_the_crane_flock_ballad_mark_bernes", DefaultVinylCrateItemId, "barter", "military_outpost", "uncommon", 15);
            Register("record_08_solitude_tango_accordion_orchestra", DefaultVinylCrateItemId, "scavenging", "apartment_block", "uncommon", 15);
            Register("record_09_tchaikovsky_swan_lake_scene_oboe", ChamberRecordCollectibleItemId, "scavenging", "concert_hall", "rare", 10);
            Register("record_10_blue_shawl_wartime_waltz_klavdiya", DefaultVinylCrateItemId, "scavenging", "military_outpost", "common", 20);
            Register("record_11_bach_cello_suite_1_prelude_rostopovich", ChamberRecordCollectibleItemId, "scavenging", "library", "very_rare", 5);
            Register("record_12_smuglianka_moldavian_folk_duet", FolkCompilationCollectibleItemId, "barter", "apartment_block", "uncommon", 15);
            Register("record_13_beethoven_moonlight_sonata_adagio", ChamberRecordCollectibleItemId, "scavenging", "concert_hall", "rare", 10);
            Register("record_14_georgian_polyphonic_winter_hymn", FolkCompilationCollectibleItemId, "scavenging", "church", "rare", 10);
            Register("record_15_shostakovich_waltz_no_2_jazz_suite", DefaultVinylCrateItemId, "scavenging", "concert_hall", "rare", 10);
            Register("record_16_katyusha_acoustic_guitar_lullaby", FolkCompilationCollectibleItemId, "scavenging", "military_outpost", "common", 20);
            Register("record_17_chopin_nocturne_op9_no2_eb_major", ChamberRecordCollectibleItemId, "scavenging", "concert_hall", "rare", 10);
            Register("record_18_old_maple_tree_village_duet", FolkCompilationCollectibleItemId, "barter", "apartment_block", "common", 20);
            Register("record_19_dvorak_new_world_symphony_largo", ChamberRecordCollectibleItemId, "expedition", "radio_station", "rare", 10);
            Register("record_20_st-petersburg_white_nights_waltz", DefaultVinylCrateItemId, "scavenging", "apartment_block", "uncommon", 15);
            Register("record_21_evening_bells_church_chimes_choir", FolkCompilationCollectibleItemId, "scavenging", "church", "common", 20);
            Register("record_22_hope_anna_german_soviet_ballad", CivilBroadcastCollectibleItemId, "expedition", "radio_station", "rare", 10);
            Register("record_23_tchaikovsky_seasons_june_barcarolle", ChamberRecordCollectibleItemId, "scavenging", "library", "rare", 10);
            Register("record_24_tenderness_maya_kristalinskaya_1965", CivilBroadcastCollectibleItemId, "expedition", "radio_station", "uncommon", 15);
            Register("record_25_dark_is_the_night_mark_bernes_1943", DefaultVinylCrateItemId, "scavenging", "military_outpost", "common", 20);
            Register("record_26_moss_roses_and_lilacs_string_quartet", ChamberRecordCollectibleItemId, "scavenging", "library", "rare", 10);
            Register("record_27_sailors_dance_apple_yablochko", FolkCompilationCollectibleItemId, "barter", "military_outpost", "common", 20);
            Register("record_28_morning_mood_grieg_peer_gynt", ChamberRecordCollectibleItemId, "scavenging", "concert_hall", "uncommon", 15);
            Register("record_29_wide_is_my_motherland_brass_march", CivilBroadcastCollectibleItemId, "expedition", "radio_station", "common", 20);
            Register("record_30_century_seed_spring_waltz_broadcast", CivilBroadcastCollectibleItemId, "cultural_vault", "radio_station", "very_rare", 5);
        }

        public static IReadOnlyList<VinylAcquisitionRoute> GetAllRoutes() => AllRoutesList;

        public static VinylAcquisitionRoute? GetRoute(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return null;
            RoutesByRecordId.TryGetValue(recordId, out var route);
            return route;
        }

        public static bool IsVinylAcquisitionItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return string.Equals(itemId, DefaultVinylCrateItemId, StringComparison.OrdinalIgnoreCase)
                || string.Equals(itemId, ChamberRecordCollectibleItemId, StringComparison.OrdinalIgnoreCase)
                || string.Equals(itemId, CivilBroadcastCollectibleItemId, StringComparison.OrdinalIgnoreCase)
                || string.Equals(itemId, FolkCompilationCollectibleItemId, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// When a vinyl crate or vinyl collectible item is acquired, resolves an unowned record
        /// corresponding to that item type and registers it in the VinylMoraleSystem.
        /// Returns the acquired record ID, or null if all matching records are already owned.
        /// </summary>
        public static string? TryAcquireFromItem(
            string itemId,
            VinylMoraleSystem vinylSystem,
            ISeededRng? rng = null)
        {
            if (vinylSystem == null || !IsVinylAcquisitionItem(itemId)) return null;

            var owned = vinylSystem.State.ownedRecordIds ?? new List<string>();
            var candidates = AllRoutesList
                .Where(r => string.Equals(r.AssociatedItemId, itemId, StringComparison.OrdinalIgnoreCase) && !owned.Contains(r.RecordId))
                .ToList();

            // If no matching unowned records for the specific item type, fallback to any unowned record
            if (candidates.Count == 0)
            {
                candidates = AllRoutesList
                    .Where(r => !owned.Contains(r.RecordId))
                    .ToList();
            }

            if (candidates.Count == 0) return null; // All 30 records already owned

            VinylAcquisitionRoute selected;
            if (rng != null && candidates.Count > 1)
            {
                int idx = rng.Next(0, candidates.Count);
                selected = candidates[idx];
            }
            else
            {
                selected = candidates[0];
            }

            vinylSystem.AcquireRecord(selected.RecordId);
            return selected.RecordId;
        }

        /// <summary>
        /// Deterministically selects an unowned record for discovery during scavenging or expedition
        /// based on location type.
        /// </summary>
        public static string? ResolveRecordForDiscovery(
            string locationType,
            IReadOnlyCollection<string> alreadyOwned,
            ISeededRng rng)
        {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            var ownedSet = new HashSet<string>(alreadyOwned ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);

            var candidates = AllRoutesList
                .Where(r => !ownedSet.Contains(r.RecordId))
                .ToList();

            if (candidates.Count == 0) return null;

            var matchingLocation = candidates
                .Where(r => string.Equals(r.LocationType, locationType, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var pool = matchingLocation.Count > 0 ? matchingLocation : candidates;

            int totalWeight = 0;
            for (int i = 0; i < pool.Count; i++)
            {
                totalWeight += Math.Max(1, pool[i].DiscoveryWeight);
            }

            int roll = rng.Next(0, totalWeight);
            int running = 0;
            for (int i = 0; i < pool.Count; i++)
            {
                running += Math.Max(1, pool[i].DiscoveryWeight);
                if (roll < running)
                {
                    return pool[i].RecordId;
                }
            }

            return pool[pool.Count - 1].RecordId;
        }
    }
}
