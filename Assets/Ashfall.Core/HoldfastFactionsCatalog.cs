using System;
using System.Collections;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>Holdfast faction (trade surface). Matches the terminal/catalog contract.</summary>
    public sealed class HoldfastFactionEntry
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string alignment { get; set; } = string.Empty;
        public string home_region { get; set; } = string.Empty;
        public bool is_active { get; set; } = true;
        public float trust { get; set; } = 0f;
        public string[] wants { get; set; } = Array.Empty<string>();
        public string[] offers { get; set; } = Array.Empty<string>();
        public string signature_quote { get; set; } = string.Empty;
        public string access_rule { get; set; } = string.Empty;
        public string badge_asset_id { get; set; } = string.Empty;

        public string Id => id;
        public string DisplayName => display_name;
        public string Alignment => alignment;
        public string HomeRegion => home_region;
        public bool IsActive => is_active;
        public float Trust => trust;
        public string[] Wants => wants;
        public string[] Offers => offers;
        public string SignatureQuote => signature_quote;
        public string AccessRule => access_rule;
        public string BadgeAssetId => badge_asset_id;

        public HoldfastFactionEntry() { }

        public HoldfastFactionEntry(string id, string displayName, string alignment, string homeRegion = "", bool isActive = true, float trust = 0f, string[] wants = null!, string[] offers = null!, string signatureQuote = "", string accessRule = "", string badgeAssetId = "")
        {
            this.id = id ?? string.Empty;
            this.display_name = displayName ?? string.Empty;
            this.alignment = alignment ?? string.Empty;
            this.home_region = homeRegion ?? string.Empty;
            this.is_active = isActive;
            this.trust = trust;
            this.wants = wants ?? Array.Empty<string>();
            this.offers = offers ?? Array.Empty<string>();
            this.signature_quote = signatureQuote ?? string.Empty;
            this.access_rule = accessRule ?? string.Empty;
            this.badge_asset_id = badgeAssetId ?? string.Empty;
        }

        public string FactionDescription()
        {
            return alignment switch
            {
                "order" => "A disciplined collective that values structure above all else. Their trade is conducted with military precision. They see the unlisted as either assets to be scheduled or threats to be contained. Their ledgers are immaculate. Their patience is not.",
                "chaos" => "A loose network of scavengers and opportunists who deal in secrets as much as supplies. They have no patience for bureaucracy, only for leverage. The unlisted are either fresh meat or fresh recruits—either way, they'll be put to work before the ink dries on the ledger.",
                "neutral" => "Pragmatic survivors who trade with anyone willing to meet their price. Ideology takes a backseat to survival. They'll deal with the unlisted, but they won't vouch for them. Trust is currency, and they're running low.",
                _ => "A mysterious faction whose true nature remains obscured by the wastes. They may be allies, they may be enemies—either way, they're watching."
            };
        }

        public string[] NotableMembers()
        {
            return id switch
            {
                "faction_holdfast_schedule" => new string[] { "Registrar-General Cael Ormund", "Clerk Edor Vale", "Auditor Veyra Dain", "Archivist Halvard Renn (deceased)", "Quartermaster Lina Kovač" },
                "faction_holdfast_reserve" => new string[] { "Shift Lead Leva Quist", "Engineer Rurik Voss", "Plant Foreman Tomas Harkin", "Chemist Mira Solis", "Outfall Worker Jace Morrow" },
                "faction_holdfast_dark_road" => new string[] { "Cutter Yara Holm", "Ice Pilot Ivy Corrigan", "Waystation Keeper Dain Marrow", "Lamplighter Elias Voss", "Scout Kael Tann" },
                "faction_holdfast_tender" => new string[] { "Sparks Halden Mire", "Radio Operator Nomi Fisk", "Engineer Bram Kettle", "Navigator Sela Renn", "Deckhand Rook" },
                "faction_holdfast_white" => new string[] { "The Pale One", "The Witness", "The Keeper of the White", "Silent Librarian", "The Archivist's Ghost" },
                _ => Array.Empty<string>()
            };
        }

        public string[] HostileActions()
        {
            return id switch
            {
                "faction_holdfast_schedule" => new string[] { "File you as a labor reserve", "Send auditors to your bunker", "Confiscate unlisted survivors", "Freeze your Ice Road access", "Issue levy orders with impossible terms" },
                "faction_holdfast_reserve" => new string[] { "Poison your water supply", "Sabotage your steam connections", "Overcharge for critical repairs", "Refuse to share medical supplies", "Blame you for plant failures" },
                "faction_holdfast_dark_road" => new string[] { "Mark your ice as unsafe", "Sabotage your waystation", "Steal your beacon oil", "Leave you stranded in a blizzard", "Charge exorbitant passage fees" },
                "faction_holdfast_tender" => new string[] { "Demand authentication before boarding", "Refuse to share radio frequencies", "Charge for safe passage", "Blame you for Fleet delays", "Take your survivors as crew" },
                "faction_holdfast_white" => new string[] { "Whisper in the dark", "Leave cryptic notes", "Disappear survivors", "Corrupt your records", "Make you question reality" },
                _ => Array.Empty<string>()
            };
        }

        public string[] TrustBuildingRequirements()
        {
            return id switch
            {
                "faction_holdfast_schedule" => new string[] { "Complete census forms accurately", "Honor levy orders", "Share survivor occupations", "Provide accurate location data", "File paperwork on time" },
                "faction_holdfast_reserve" => new string[] { "Deliver brass fittings", "Repair membrane systems", "Share medical supplies", "Work outfall shifts", "Provide iodine crystals" },
                "faction_holdfast_dark_road" => new string[] { "Relight dark beacons", "Provide lamp oil", "Work ice road maintenance", "Share navigation charts", "Honor Cutter rules" },
                "faction_holdfast_tender" => new string[] { "Provide clean water", "Share radio frequencies", "Work on the tender", "Honor Fleet protocols", "Provide engine parts" },
                "faction_holdfast_white" => new string[] { "Leave offerings at the White", "Share cryptic knowledge", "Work in silence", "Honor the Witness", "Provide blank paper" },
                _ => Array.Empty<string>()
            };
        }
    }

    /// <summary>Immutable-after-load Holdfast faction catalog.</summary>
    public sealed class HoldfastFactionsCatalog : IEnumerable<HoldfastFactionEntry>
    {
        private readonly Dictionary<string, HoldfastFactionEntry> _byId =
            new Dictionary<string, HoldfastFactionEntry>(StringComparer.Ordinal);
        private readonly List<HoldfastFactionEntry> _order = new List<HoldfastFactionEntry>();

        public int Count => _order.Count;
        public HoldfastFactionEntry this[int index] => _order[index];

        public static HoldfastFactionsCatalog Empty() => new HoldfastFactionsCatalog();

        public void Register(HoldfastFactionEntry entry)
        {
            if (entry == null || string.IsNullOrEmpty(entry.Id) || _byId.ContainsKey(entry.Id)) return;
            _byId[entry.Id] = entry;
            _order.Add(entry);
        }

        public HoldfastFactionEntry? GetById(string id)
            => string.IsNullOrEmpty(id) ? null : (_byId.TryGetValue(id, out var e) ? e : null);

        public bool Contains(string id) => GetById(id) != null;

        public IEnumerator<HoldfastFactionEntry> GetEnumerator() => _order.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _order.GetEnumerator();
    }
}
