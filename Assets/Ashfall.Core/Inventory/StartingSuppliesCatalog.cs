// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// One fresh-campaign inventory profile. This describes only the material
    /// inserted into a new inventory; it is not a class, trait, or campaign state.
    /// </summary>
    [Serializable]
    public sealed class StartingSuppliesProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public List<(string itemId, int amount)> supplies { get; } =
            new List<(string itemId, int amount)>();

        public StartingSuppliesProfile Clone()
        {
            var clone = new StartingSuppliesProfile
            {
                id = id,
                display_name = display_name,
                description = description
            };
            for (int i = 0; i < supplies.Count; i++)
                clone.supplies.Add(supplies[i]);
            return clone;
        }
    }

    /// <summary>
    /// Validated starting-supplies profiles. The inventory runtime consumes
    /// only the selected profile's stack list; no profile metadata is persisted.
    /// </summary>
    public sealed class StartingSuppliesCatalog
    {
        public const string StandardProfileId = "origin_standard_holdfast";

        private readonly List<StartingSuppliesProfile> _profiles;
        private readonly Dictionary<string, StartingSuppliesProfile> _byId;

        public StartingSuppliesCatalog(
            IEnumerable<StartingSuppliesProfile> profiles,
            string defaultProfileId)
        {
            _profiles = new List<StartingSuppliesProfile>();
            _byId = new Dictionary<string, StartingSuppliesProfile>(StringComparer.Ordinal);

            if (profiles != null)
            {
                foreach (var profile in profiles)
                {
                    if (profile == null || string.IsNullOrEmpty(profile.id) ||
                        _byId.ContainsKey(profile.id))
                        continue;

                    var copy = profile.Clone();
                    _profiles.Add(copy);
                    _byId.Add(copy.id, copy);
                }
            }

            DefaultProfileId = !string.IsNullOrEmpty(defaultProfileId) &&
                               _byId.ContainsKey(defaultProfileId)
                ? defaultProfileId
                : StandardProfileId;
        }

        public IReadOnlyList<StartingSuppliesProfile> Profiles => _profiles;
        public string DefaultProfileId { get; }

        public StartingSuppliesProfile DefaultProfile =>
            TryGet(DefaultProfileId, out var profile)
                ? profile
                : throw new InvalidOperationException(
                    "Starting supplies catalog has no default profile.");

        public bool TryGet(string profileId, out StartingSuppliesProfile profile)
        {
            if (!string.IsNullOrEmpty(profileId) &&
                _byId.TryGetValue(profileId, out profile!))
                return true;

            profile = null!;
            return false;
        }

        public StartingSuppliesProfile ResolveOrDefault(string? profileId)
        {
            return TryGet(profileId ?? string.Empty, out var profile)
                ? profile
                : DefaultProfile;
        }

        /// <summary>
        /// The compatibility fallback for missing, empty, or malformed
        /// starting_supplies.json. Keep this list byte-for-byte equivalent in
        /// IDs and quantities to the legacy production opening.
        /// </summary>
        public static StartingSuppliesProfile CreateLegacyFallbackProfile()
        {
            var profile = new StartingSuppliesProfile
            {
                id = StandardProfileId,
                display_name = "Standard Holdfast",
                description = "The unchanged legacy opening."
            };
            Add(profile, "clean_water", 12);
            Add(profile, "canned_food", 16);
            Add(profile, "irradiated_water", 4);
            Add(profile, "item_air_filter_hepa", 2);
            Add(profile, "item_desal_membrane", 1);
            Add(profile, "iodine_pills", 4);
            Add(profile, "bandage", 2);
            Add(profile, "rad_away", 1);
            Add(profile, "item_dosimeter_pen", 1);
            Add(profile, "item_geiger_m3", 1);
            Add(profile, "gas_mask", 1);
            Add(profile, "hazmat_suit", 1);
            Add(profile, "battery", 4);
            Add(profile, "scrap_mechanical", 6);
            Add(profile, "scrap_electronic", 3);
            return profile;
        }

        private static void Add(StartingSuppliesProfile profile, string itemId, int amount)
        {
            profile.supplies.Add((itemId, amount));
        }
    }
}
