using System;
using System.Globalization;

namespace Ashfall.Core.UI
{
    /// <summary>
    /// Presentation model for collectible cards and loot display.
    /// Implements Task 5 accessibility and metadata requirements: all critical semantics
    /// (category, rarity, discovery state, effect hint, locked reason) are
    /// expressed in visible/accessible text rather than through color or icons alone.
    /// </summary>
    public sealed class CollectiblePresentationModel
    {
        public string ItemId { get; }
        public string DisplayName { get; }
        public string Category { get; }
        public string Rarity { get; }
        public CollectibleDiscoveryStatus DiscoveryStatus { get; }
        public string DiscoveryStateText { get; }
        public bool IsNewDiscovery { get; }
        public string EffectIntentText { get; }
        public bool HasEffectBenefit { get; }
        public string Description { get; }
        public bool IsLocked { get; }
        public string LockedReason { get; }
        public string AccessibleLabel { get; }
        public string TooltipText => AccessibleLabel;

        public CollectiblePresentationModel(
            CollectibleDefinition def,
            bool isDiscovered,
            string? displayName = null,
            string? description = null,
            bool isLocked = false,
            string? lockedReason = null)
            : this(
                def,
                isDiscovered ? CollectibleDiscoveryStatus.DiscoveredAcknowledged : CollectibleDiscoveryStatus.NewUnacknowledged,
                displayName,
                description,
                isLocked,
                lockedReason)
        {
        }

        public CollectiblePresentationModel(
            CollectibleDefinition def,
            CollectibleDiscoveryStatus status,
            string? displayName = null,
            string? description = null,
            bool isLocked = false,
            string? lockedReason = null)
        {
            if (def == null) throw new ArgumentNullException(nameof(def));

            ItemId = def.item_id ?? string.Empty;
            DisplayName = !string.IsNullOrWhiteSpace(displayName)
                ? displayName
                : FormatDisplayNameFromId(ItemId);

            Category = FormatCategory(def.category);
            Rarity = FormatRarity(def.rarity);

            DiscoveryStatus = status;
            IsNewDiscovery = status == CollectibleDiscoveryStatus.NewUnacknowledged;
            DiscoveryStateText = IsNewDiscovery ? "NEW" : "DISCOVERED";

            EffectIntentText = FormatEffectIntent(def.effect_type, def.effect_target, def.effect_value);
            HasEffectBenefit = !string.Equals(def.effect_type, "none", StringComparison.OrdinalIgnoreCase);

            Description = description ?? string.Empty;

            IsLocked = isLocked;
            LockedReason = lockedReason ?? string.Empty;

            // Canonical semantic composition (Sections 5.12, 30):
            // {DisplayName}. {Category}. {Rarity}. {DiscoveryState}. {EffectHint}. {Description}.
            var sb = new System.Text.StringBuilder();
            sb.Append(DisplayName).Append(". ");
            sb.Append(Category).Append(". ");
            sb.Append(Rarity).Append(". ");
            sb.Append(DiscoveryStateText).Append(". ");
            sb.Append(EffectIntentText).Append(". ");
            if (!string.IsNullOrWhiteSpace(Description))
            {
                sb.Append(Description).Append(". ");
            }
            if (IsLocked && !string.IsNullOrWhiteSpace(LockedReason))
            {
                sb.Append("Locked: ").Append(LockedReason).Append(". ");
            }

            AccessibleLabel = sb.ToString().TrimEnd();
        }

        private static string FormatDisplayNameFromId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return "Unknown Collectible";
            string stripped = itemId.StartsWith("item_collectible_", StringComparison.Ordinal)
                ? itemId.Substring("item_collectible_".Length)
                : itemId;
            string[] parts = stripped.Split('_', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                {
                    parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1);
                }
            }
            return string.Join(" ", parts);
        }

        public static string FormatCategory(string? rawCategory)
        {
            if (string.IsNullOrWhiteSpace(rawCategory)) return "Artifact";
            return rawCategory switch
            {
                "vinyl" => "Vinyl Record",
                "photograph" => "Photograph",
                "poster" => "Poster",
                "book" => "Book",
                "magazine" => "Magazine",
                "technical_manual" => "Technical Manual",
                "military_document" => "Military Document",
                "personal_letter" => "Personal Letter",
                "badge" => "Badge",
                "patch" => "Insignia Patch",
                "toy" => "Pre-War Toy",
                "religious_object" => "Religious Object",
                "sports_memorabilia" => "Sports Memorabilia",
                "cultural_artifact" => "Cultural Artifact",
                "newspaper" => "Newspaper",
                "map" => "Cartographic Map",
                _ => char.ToUpperInvariant(rawCategory[0]) + rawCategory.Substring(1).Replace('_', ' ')
            };
        }

        public static string FormatRarity(string? rawRarity)
        {
            if (string.IsNullOrWhiteSpace(rawRarity)) return "Common";
            return rawRarity.ToLowerInvariant() switch
            {
                "common" => "Common",
                "uncommon" => "Uncommon",
                "rare" => "Rare",
                "unique" => "Unique",
                _ => char.ToUpperInvariant(rawRarity[0]) + rawRarity.Substring(1)
            };
        }

        public static string FormatEffectIntent(string? effectType, string? effectTarget, float effectValue)
        {
            return (effectType ?? "none").ToLowerInvariant() switch
            {
                "morale" => $"Grants morale (+{effectValue.ToString("0.#", CultureInfo.InvariantCulture)})",
                "knowledge" => string.IsNullOrEmpty(effectTarget)
                    ? "Reveals research knowledge"
                    : $"Reveals research knowledge ({effectTarget})",
                "journal_unlock" => string.IsNullOrEmpty(effectTarget)
                    ? "Unlocks journal entry"
                    : $"Unlocks journal entry ({effectTarget})",
                "faction_info" => string.IsNullOrEmpty(effectTarget)
                    ? "Reveals faction intelligence"
                    : $"Reveals faction intelligence ({effectTarget})",
                "location_clue" => string.IsNullOrEmpty(effectTarget)
                    ? "Reveals map location"
                    : $"Reveals map location ({effectTarget})",
                "recipe" => string.IsNullOrEmpty(effectTarget)
                    ? "Unlocks crafting recipe"
                    : $"Unlocks crafting recipe ({effectTarget})",
                "none" => "Archive record (no immediate effect)",
                _ => $"Special effect ({effectType})"
            };
        }
    }
}
