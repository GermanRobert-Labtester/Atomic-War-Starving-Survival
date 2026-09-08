using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Ashfall.Core.IO;

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// One trader voice profile from trade_texts.json. This is presentation
    /// data only: it contains no price, stock, faction, debt, or scenario
    /// authority.
    /// </summary>
    [Serializable]
    public sealed class TradeTextTraderDefinition
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string profile = string.Empty;
        public Dictionary<string, string> greetings = new(StringComparer.Ordinal);
        public Dictionary<string, string> item_examinations = new(StringComparer.Ordinal);
        public Dictionary<string, string> offers = new(StringComparer.Ordinal);
        public Dictionary<string, string> counter_offers = new(StringComparer.Ordinal);
        public Dictionary<string, string> acceptance = new(StringComparer.Ordinal);
        public Dictionary<string, string> rejection = new(StringComparer.Ordinal);
        public Dictionary<string, string> regret = new(StringComparer.Ordinal);
        public Dictionary<string, string> insult = new(StringComparer.Ordinal);
        public Dictionary<string, string> flattery = new(StringComparer.Ordinal);
        public Dictionary<string, string> threat = new(StringComparer.Ordinal);
    }

    [Serializable]
    public sealed class TradeTextScenarioDefinition
    {
        public string description = string.Empty;
        public string trader_text = string.Empty;
        public string player_text = string.Empty;
    }

    [Serializable]
    public sealed class TradeTextCatalogFile
    {
        public int schema_version;
        public string collection_id = string.Empty;
        public List<TradeTextTraderDefinition> traders = new();
        public Dictionary<string, TradeTextScenarioDefinition> trade_scenarios = new(StringComparer.Ordinal);
    }

    /// <summary>
    /// Immutable-at-use catalog view. Definitions are loaded once and then
    /// read by the presentation resolver; no save state is carried here.
    /// </summary>
    public sealed class TradeTextCatalog
    {
        private readonly Dictionary<string, TradeTextTraderDefinition> _traders;
        private readonly Dictionary<string, TradeTextScenarioDefinition> _scenarios;

        public int SchemaVersion { get; }
        public string CollectionId { get; }
        public bool IsFallback { get; }
        public int TraderCount => _traders.Count;
        public int ScenarioCount => _scenarios.Count;
        public IReadOnlyDictionary<string, TradeTextTraderDefinition> Traders => _traders;
        public IReadOnlyDictionary<string, TradeTextScenarioDefinition> Scenarios => _scenarios;

        internal TradeTextCatalog(
            int schemaVersion,
            string collectionId,
            IEnumerable<TradeTextTraderDefinition> traders,
            IDictionary<string, TradeTextScenarioDefinition> scenarios,
            bool isFallback)
        {
            SchemaVersion = schemaVersion;
            CollectionId = collectionId ?? string.Empty;
            IsFallback = isFallback;
            _traders = new Dictionary<string, TradeTextTraderDefinition>(StringComparer.Ordinal);
            _scenarios = new Dictionary<string, TradeTextScenarioDefinition>(StringComparer.Ordinal);

            foreach (var trader in traders ?? Array.Empty<TradeTextTraderDefinition>())
            {
                if (trader == null || string.IsNullOrWhiteSpace(trader.id)) continue;
                _traders[trader.id] = trader;
            }

            foreach (var pair in scenarios ?? new Dictionary<string, TradeTextScenarioDefinition>())
            {
                if (string.IsNullOrWhiteSpace(pair.Key) || pair.Value == null) continue;
                _scenarios[pair.Key] = pair.Value;
            }
        }

        public bool TryGetTrader(string id, out TradeTextTraderDefinition trader) =>
            _traders.TryGetValue(id ?? string.Empty, out trader!);

        public bool TryGetScenario(string id, out TradeTextScenarioDefinition scenario) =>
            _scenarios.TryGetValue(id ?? string.Empty, out scenario!);
    }

    public sealed class TradeTextCatalogLoadResult
    {
        public TradeTextCatalog Catalog { get; }
        public IReadOnlyList<string> Errors { get; }
        public bool LoadedFromFile { get; }
        public bool UsedFallback => Catalog.IsFallback;
        public bool IsValid => Errors.Count == 0 && !Catalog.IsFallback;

        internal TradeTextCatalogLoadResult(
            TradeTextCatalog catalog,
            IReadOnlyList<string> errors,
            bool loadedFromFile)
        {
            Catalog = catalog;
            Errors = errors;
            LoadedFromFile = loadedFromFile;
        }
    }

    /// <summary>
    /// Optional loader for the trade voice catalog. A missing or malformed
    /// optional catalog never blocks trade; malformed present data emits the
    /// standard catalog diagnostic before falling back.
    /// </summary>
    public static class TradeTextCatalogLoader
    {
        public const string FileName = "trade_texts.json";
        public const int SupportedSchemaVersion = 1;
        public const string CollectionId = "trade_texts";

        private static readonly string[] s_requiredFamilies =
        {
            "greetings",
            "item_examinations",
            "offers",
            "counter_offers",
            "acceptance",
            "rejection",
            "regret",
            "insult",
            "flattery",
            "threat"
        };

        private static readonly Dictionary<string, string[]> s_requiredKeys =
            new(StringComparer.Ordinal)
            {
                ["greetings"] = new[] { "hostile", "wary", "neutral", "warm" },
                ["item_examinations"] = new[] { "valuable", "worthless", "interesting", "dangerous" },
                ["offers"] = new[] { "fair", "generous", "stingy", "desperate" },
                ["counter_offers"] = new[] { "accept", "reject", "negotiate", "insulted" },
                ["acceptance"] = new[] { "pleased", "relieved", "indifferent", "suspicious" },
                ["rejection"] = new[] { "polite", "annoyed", "angry", "sad" },
                ["regret"] = new[] { "buyer", "seller", "mutual" },
                ["insult"] = new[] { "mild", "moderate", "severe" },
                ["flattery"] = new[] { "subtle", "obvious", "excessive" },
                ["threat"] = new[] { "veiled", "direct", "desperate" }
            };

        private static readonly Regex s_allowedPlaceholder =
            new(@"\[item\]", RegexOptions.CultureInvariant);
        private static readonly Regex s_anyBracketToken =
            new(@"\[[^\]]+\]|\{[^}]+\}", RegexOptions.CultureInvariant);

        public static TradeTextCatalogLoadResult Load(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return Fallback(Array.Empty<string>(), loadedFromFile: false);

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return Fallback(Array.Empty<string>(), loadedFromFile: false);

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return Reject("trade_texts.json is present but empty.");

            try
            {
                var file = json.Deserialize<TradeTextCatalogFile>(raw);
                return Validate(file);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "trade_texts root object", ex);
                return Fallback(new[] { "trade_texts.json could not be parsed: " + ex.Message }, true);
            }
        }

        public static TradeTextCatalogLoadResult LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return Reject("trade_texts JSON is empty.");

            try
            {
                var file = new SystemTextJsonSerializer().Deserialize<TradeTextCatalogFile>(json);
                return Validate(file);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "trade_texts root object", ex);
                return Fallback(new[] { "trade_texts JSON could not be parsed: " + ex.Message }, true);
            }
        }

        public static TradeTextCatalog CreateFallbackCatalog()
        {
            var trader = new TradeTextTraderDefinition
            {
                id = TradeVoiceResolver.DefaultProfileId,
                display_name = "The Merchant",
                profile = "A plain voice for a trade that has no reliable local profile."
            };

            Add(trader.greetings, "hostile", "Keep the terms clear. We are still trading.");
            Add(trader.greetings, "wary", "Show what you have. Keep your hands where I can see them.");
            Add(trader.greetings, "neutral", "Goods for goods. State the terms.");
            Add(trader.greetings, "warm", "Good to see you. Let us keep the exchange clean.");
            Add(trader.item_examinations, "valuable", "That is useful. Name your price.");
            Add(trader.item_examinations, "worthless", "I cannot carry that for nothing.");
            Add(trader.item_examinations, "interesting", "I can use this. What do you want for it?");
            Add(trader.item_examinations, "dangerous", "Keep that covered. We can still discuss a trade.");
            Add(trader.offers, "fair", "[item] for [item]. The terms are even.");
            Add(trader.offers, "generous", "[item] for [item]. Take it before the terms change.");
            Add(trader.offers, "stingy", "[item] for [item]. That is what I can spare.");
            Add(trader.offers, "desperate", "I need [item]. I will give you [item].");
            Add(trader.counter_offers, "accept", "Agreed. Put it on the table.");
            Add(trader.counter_offers, "reject", "No. That exchange does not hold.");
            Add(trader.counter_offers, "negotiate", "How about [item] instead?");
            Add(trader.counter_offers, "insulted", "Do not mistake a hard bargain for stupidity.");
            Add(trader.acceptance, "pleased", "Done. Both sides leave with something useful.");
            Add(trader.acceptance, "relieved", "Thank you. That gets us through another day.");
            Add(trader.acceptance, "indifferent", "Done. Take your goods.");
            Add(trader.acceptance, "suspicious", "Done. No returns.");
            Add(trader.rejection, "polite", "Not today. Travel carefully.");
            Add(trader.rejection, "annoyed", "You have spent enough of my time.");
            Add(trader.rejection, "angry", "Leave before this becomes something else.");
            Add(trader.rejection, "sad", "I needed the exchange. It still will not hold.");
            Add(trader.regret, "buyer", "I should have asked for more.");
            Add(trader.regret, "seller", "I sold that too cheaply.");
            Add(trader.regret, "mutual", "Both sides carried away what they needed.");
            Add(trader.insult, "mild", "Try again without the insult.");
            Add(trader.insult, "moderate", "Need is not the same as weakness.");
            Add(trader.insult, "severe", "Leave now.");
            Add(trader.flattery, "subtle", "You know how to look at a table.");
            Add(trader.flattery, "obvious", "You are generous. I will remember the terms.");
            Add(trader.flattery, "excessive", "Praise does not change the count.");
            Add(trader.threat, "veiled", "Careful. The road keeps records.");
            Add(trader.threat, "direct", "Do not force the exchange.");
            Add(trader.threat, "desperate", "I need [item]. Do not make me ask twice.");

            return new TradeTextCatalog(
                SupportedSchemaVersion,
                CollectionId,
                new[] { trader },
                new Dictionary<string, TradeTextScenarioDefinition>(StringComparer.Ordinal),
                isFallback: true);
        }

        private static TradeTextCatalogLoadResult Validate(TradeTextCatalogFile? file)
        {
            var errors = new List<string>();
            if (file == null)
            {
                errors.Add("trade_texts root object is missing.");
                return Reject(errors);
            }

            if (file.schema_version != SupportedSchemaVersion)
                errors.Add($"Unsupported trade_texts schema_version {file.schema_version}; expected {SupportedSchemaVersion}.");
            if (!string.Equals(file.collection_id, CollectionId, StringComparison.Ordinal))
                errors.Add($"Expected collection_id '{CollectionId}', got '{file.collection_id}'.");
            if (file.traders == null || file.traders.Count == 0)
                errors.Add("trade_texts.traders must contain at least one profile.");

            var seen = new HashSet<string>(StringComparer.Ordinal);
            if (file.traders != null)
            {
                foreach (var trader in file.traders)
                {
                    if (trader == null)
                    {
                        errors.Add("trade_texts.traders contains a null profile.");
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(trader.id))
                    {
                        errors.Add("Every trade voice profile needs a non-empty id.");
                        continue;
                    }
                    if (!seen.Add(trader.id))
                        errors.Add($"Duplicate trade voice profile id '{trader.id}'.");

                    ValidateProfile(trader, errors);
                }
            }

            if (errors.Count > 0)
            {
                var ex = new InvalidDataException(string.Join(" ", errors));
                CatalogDiagnostics.Warn(FileName, "validated trade_texts catalog", ex);
                return Fallback(errors, loadedFromFile: true);
            }

            return new TradeTextCatalogLoadResult(
                new TradeTextCatalog(
                    file.schema_version,
                    file.collection_id,
                    file.traders!,
                    file.trade_scenarios ?? new Dictionary<string, TradeTextScenarioDefinition>(StringComparer.Ordinal),
                    isFallback: false),
                Array.Empty<string>(),
                loadedFromFile: true);
        }

        private static void ValidateProfile(TradeTextTraderDefinition trader, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(trader.display_name))
                errors.Add($"Profile '{trader.id}' has no display_name.");

            foreach (var family in s_requiredFamilies)
            {
                var lines = Family(trader, family);
                if (lines == null || lines.Count == 0)
                {
                    errors.Add($"Profile '{trader.id}' is missing line family '{family}'.");
                    continue;
                }

                if (s_requiredKeys.TryGetValue(family, out var required))
                {
                    foreach (var key in required)
                    {
                        if (!lines.TryGetValue(key, out var text) || string.IsNullOrWhiteSpace(text))
                            errors.Add($"Profile '{trader.id}' is missing '{family}.{key}'.");
                    }
                }

                foreach (var pair in lines)
                {
                    if (string.IsNullOrWhiteSpace(pair.Value)) continue;
                    var unknown = s_anyBracketToken.Matches(pair.Value);
                    foreach (Match token in unknown)
                    {
                        if (!s_allowedPlaceholder.IsMatch(token.Value))
                            errors.Add($"Profile '{trader.id}' contains unknown placeholder '{token.Value}' in '{family}.{pair.Key}'.");
                    }

                    int count = s_allowedPlaceholder.Matches(pair.Value).Count;
                    int max = PlaceholderLimit(family, pair.Key);
                    if (count > max)
                        errors.Add($"Profile '{trader.id}' has {count} [item] placeholders in '{family}.{pair.Key}', maximum is {max}.");
                }
            }
        }

        private static Dictionary<string, string>? Family(TradeTextTraderDefinition trader, string family) =>
            family switch
            {
                "greetings" => trader.greetings,
                "item_examinations" => trader.item_examinations,
                "offers" => trader.offers,
                "counter_offers" => trader.counter_offers,
                "acceptance" => trader.acceptance,
                "rejection" => trader.rejection,
                "regret" => trader.regret,
                "insult" => trader.insult,
                "flattery" => trader.flattery,
                "threat" => trader.threat,
                _ => null
            };

        private static int PlaceholderLimit(string family, string key)
        {
            if (family == "offers") return 2;
            if (family == "counter_offers" && key == "negotiate") return 1;
            if (family == "threat" && key == "desperate") return 1;
            return 0;
        }

        private static TradeTextCatalogLoadResult Reject(string error)
        {
            return Reject(new[] { error });
        }

        private static TradeTextCatalogLoadResult Reject(IReadOnlyList<string> errors)
        {
            var ex = new InvalidDataException(string.Join(" ", errors));
            CatalogDiagnostics.Warn(FileName, "trade_texts catalog", ex);
            return Fallback(errors, loadedFromFile: true);
        }

        private static TradeTextCatalogLoadResult Fallback(IReadOnlyList<string> errors, bool loadedFromFile)
        {
            return new TradeTextCatalogLoadResult(
                CreateFallbackCatalog(),
                errors,
                loadedFromFile);
        }

        private static void Add(Dictionary<string, string> target, string key, string value) =>
            target[key] = value;
    }

    public enum TradeVoiceLineFamily
    {
        Greeting,
        ItemExamination,
        Offer,
        CounterOffer,
        Acceptance,
        Rejection,
        Regret,
        Insult,
        Flattery,
        Threat
    }

    public enum TradeVoiceItemReaction
    {
        Valuable,
        Worthless,
        Interesting,
        Dangerous
    }

    public sealed class TradeVoiceContext
    {
        public string TraderProfileId { get; set; } = string.Empty;
        public string ScenarioId { get; set; } = string.Empty;
        public string FactionId { get; set; } = string.Empty;
        public string CaravanId { get; set; } = string.Empty;
        public string CaravanOriginRegion { get; set; } = string.Empty;
        public string SpecialtyId { get; set; } = string.Empty;
        public TradeStance Stance { get; set; } = TradeStance.Trade;
        public float Trust { get; set; }
        public string StableContextKey { get; set; } = string.Empty;
    }

    public sealed class TradeVoiceResult
    {
        public string ProfileId { get; }
        public string Band { get; }
        public string Text { get; }
        public bool UsedFallback { get; }

        public TradeVoiceResult(string profileId, string band, string text, bool usedFallback)
        {
            ProfileId = profileId ?? string.Empty;
            Band = band ?? string.Empty;
            Text = text ?? string.Empty;
            UsedFallback = usedFallback;
        }
    }

    /// <summary>
    /// Read-only voice adapter. Context resolution is explicit and stable;
    /// line selection uses keyed lookups rather than economy/world RNG.
    /// </summary>
    public sealed class TradeVoiceResolver
    {
        public const string DefaultProfileId = "trader_merchant";

        private static readonly Dictionary<string, string> s_scenarioProfiles =
            new(StringComparer.Ordinal)
            {
                ["depot_window"] = "trader_quartermaster",
                ["emergency_requisition"] = "trader_quartermaster",
                ["crate_lot"] = "trader_bulk_dealer",
                ["last_vials"] = "trader_medical_supplier",
                ["back_room_exchange"] = "trader_flotilla_salvager",
                ["salvage_caravan"] = "trader_foundry_broker",
                ["long_road_caravan"] = "trader_wanderer",
                ["road_knowledge"] = "trader_wanderer"
            };

        private static readonly Dictionary<string, string> s_factionProfiles =
            new(StringComparer.Ordinal)
            {
                ["faction_silent_foundry"] = "trader_foundry_broker",
                ["faction_the_fleet"] = "trader_flotilla_salvager",
                ["faction_black_cross"] = "trader_medical_supplier",
                ["faction_wandering_menders"] = "trader_medical_supplier",
                ["faction_rebuilders"] = "trader_bulk_dealer",
                ["faction_salvagers"] = "trader_scavenger",
                ["faction_permafrost_nomads"] = "trader_bulk_dealer",
                ["faction_the_scale"] = "trader_merchant"
            };

        private static readonly Dictionary<string, string> s_caravanProfiles =
            new(StringComparer.Ordinal)
            {
                ["caravan_foundry_coal_iron"] = "trader_foundry_broker",
                ["caravan_flotilla_salt_run"] = "trader_flotilla_salvager",
                ["caravan_medic_syndicate"] = "trader_medical_supplier",
                ["caravan_permafrost_traders"] = "trader_bulk_dealer",
                ["caravan_scrap_salvagers"] = "trader_scavenger",
                ["caravan_verge_grain_convoy"] = "trader_bulk_dealer",
                ["caravan_free_trader_circuit"] = "trader_merchant"
            };

        private readonly TradeTextCatalog _catalog;

        public TradeTextCatalog Catalog => _catalog;

        public TradeVoiceResolver(TradeTextCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public string ResolveProfileId(TradeVoiceContext? context)
        {
            context ??= new TradeVoiceContext();
            if (HasProfile(context.TraderProfileId)) return context.TraderProfileId;
            if (TryMapped(s_scenarioProfiles, context.ScenarioId, out var profile) && HasProfile(profile)) return profile;
            if (TryMapped(s_caravanProfiles, context.CaravanId, out profile) && HasProfile(profile)) return profile;
            if (TryMapped(s_factionProfiles, context.FactionId, out profile) && HasProfile(profile)) return profile;

            string region = context.CaravanOriginRegion ?? string.Empty;
            if (region.Equals("industrial_belt", StringComparison.Ordinal) && HasProfile("trader_foundry_broker"))
                return "trader_foundry_broker";
            if (region.Equals("deep_coast", StringComparison.Ordinal) && HasProfile("trader_flotilla_salvager"))
                return "trader_flotilla_salvager";
            if (region.Equals("ash_flats", StringComparison.Ordinal) && HasProfile("trader_bulk_dealer"))
                return "trader_bulk_dealer";

            string specialty = context.SpecialtyId ?? string.Empty;
            if (specialty.IndexOf("medical", StringComparison.OrdinalIgnoreCase) >= 0 && HasProfile("trader_medical_supplier"))
                return "trader_medical_supplier";
            if (specialty.IndexOf("bulk", StringComparison.OrdinalIgnoreCase) >= 0 && HasProfile("trader_bulk_dealer"))
                return "trader_bulk_dealer";
            if (specialty.IndexOf("foundry", StringComparison.OrdinalIgnoreCase) >= 0 && HasProfile("trader_foundry_broker"))
                return "trader_foundry_broker";

            return HasProfile(DefaultProfileId) ? DefaultProfileId : FirstProfileId();
        }

        public TradeVoiceResult ResolveGreeting(TradeVoiceContext? context)
        {
            context ??= new TradeVoiceContext();
            string profileId = ResolveProfileId(context);
            string band = BandForTrust(context.Trust);
            return ResolveFromFamily(profileId, TradeVoiceLineFamily.Greeting, band, band, Array.Empty<string>());
        }

        public TradeVoiceResult ResolveItemExamination(
            TradeVoiceContext? context,
            TradeVoiceItemReaction reaction,
            string itemDisplayName)
        {
            string profileId = ResolveProfileId(context);
            string key = reaction.ToString().ToLowerInvariant();
            return ResolveFromFamily(profileId, TradeVoiceLineFamily.ItemExamination, key, string.Empty, Array.Empty<string>());
        }

        public TradeVoiceResult ResolveLine(
            TradeVoiceContext? context,
            TradeVoiceLineFamily family,
            string variant,
            params string[] itemDisplayNames)
        {
            string profileId = ResolveProfileId(context);
            return ResolveFromFamily(
                profileId,
                family,
                variant ?? string.Empty,
                string.Empty,
                itemDisplayNames ?? Array.Empty<string>());
        }

        public TradeVoiceResult ResolveScenarioTraderText(TradeVoiceContext? context, string scenarioId)
        {
            string profileId = ResolveProfileId(context);
            if (_catalog.TryGetScenario(scenarioId, out var scenario) &&
                !string.IsNullOrWhiteSpace(scenario.trader_text))
            {
                return new TradeVoiceResult(profileId, BandForTrust(context?.Trust ?? 0f), scenario.trader_text, false);
            }
            return new TradeVoiceResult(profileId, BandForTrust(context?.Trust ?? 0f), string.Empty, true);
        }

        public static string BandForTrust(float trust)
        {
            if (trust <= -40f) return "hostile";
            if (trust <= 0f) return "wary";
            if (trust <= 40f) return "neutral";
            return "warm";
        }

        private TradeVoiceResult ResolveFromFamily(
            string profileId,
            TradeVoiceLineFamily family,
            string key,
            string band,
            IReadOnlyList<string> itemDisplayNames)
        {
            if (!_catalog.TryGetTrader(profileId, out var trader))
                return FallbackResult(profileId, band, family, key);

            var lines = Family(trader, family);
            if (lines == null || !lines.TryGetValue(key, out var template) || string.IsNullOrWhiteSpace(template))
                return FallbackResult(profileId, band, family, key);

            if (!TryFormat(template, itemDisplayNames, out var text))
                return FallbackResult(profileId, band, family, key);

            return new TradeVoiceResult(profileId, band, text, _catalog.IsFallback);
        }

        private TradeVoiceResult FallbackResult(
            string profileId,
            string band,
            TradeVoiceLineFamily family,
            string key)
        {
            string text = family switch
            {
                TradeVoiceLineFamily.Greeting => "Goods for goods. State the terms.",
                TradeVoiceLineFamily.ItemExamination => "I can use this. Name your price.",
                TradeVoiceLineFamily.Offer => "The terms are clear.",
                TradeVoiceLineFamily.CounterOffer => "No. That exchange does not hold.",
                TradeVoiceLineFamily.Acceptance => "Done. Both sides leave with something useful.",
                TradeVoiceLineFamily.Rejection => "Not today. Travel carefully.",
                TradeVoiceLineFamily.Regret => "The terms could have been better.",
                TradeVoiceLineFamily.Insult => "Try again without the insult.",
                TradeVoiceLineFamily.Flattery => "Praise does not change the count.",
                TradeVoiceLineFamily.Threat => "Do not force the exchange.",
                _ => "The table stays quiet."
            };
            return new TradeVoiceResult(profileId, band, text, true);
        }

        private static Dictionary<string, string>? Family(TradeTextTraderDefinition trader, TradeVoiceLineFamily family) =>
            family switch
            {
                TradeVoiceLineFamily.Greeting => trader.greetings,
                TradeVoiceLineFamily.ItemExamination => trader.item_examinations,
                TradeVoiceLineFamily.Offer => trader.offers,
                TradeVoiceLineFamily.CounterOffer => trader.counter_offers,
                TradeVoiceLineFamily.Acceptance => trader.acceptance,
                TradeVoiceLineFamily.Rejection => trader.rejection,
                TradeVoiceLineFamily.Regret => trader.regret,
                TradeVoiceLineFamily.Insult => trader.insult,
                TradeVoiceLineFamily.Flattery => trader.flattery,
                TradeVoiceLineFamily.Threat => trader.threat,
                _ => null
            };

        private static bool TryFormat(
            string template,
            IReadOnlyList<string> itemDisplayNames,
            out string text)
        {
            int placeholderCount = 0;
            foreach (Match _ in Regex.Matches(template, @"\[item\]", RegexOptions.CultureInvariant))
                placeholderCount++;
            if (placeholderCount > itemDisplayNames.Count)
            {
                text = string.Empty;
                return false;
            }

            text = template;
            for (int i = 0; i < itemDisplayNames.Count; i++)
            {
                int placeholderIndex = text.IndexOf("[item]", StringComparison.Ordinal);
                if (placeholderIndex < 0) break;
                string safeName = SanitizeDisplayName(itemDisplayNames[i]);
                text = text.Substring(0, placeholderIndex)
                    + safeName
                    + text.Substring(placeholderIndex + "[item]".Length);
            }
            return !text.Contains("[item]", StringComparison.Ordinal) &&
                   !text.Contains('{') &&
                   !text.Contains('}');
        }

        public static string SanitizeDisplayName(string? displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName)) return "the item";
            var chars = displayName.Trim().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '[' || chars[i] == ']' || chars[i] == '{' || chars[i] == '}')
                    chars[i] = ' ';
                else if (char.IsControl(chars[i]))
                    chars[i] = ' ';
            }
            return new string(chars).Replace('\n', ' ').Replace('\r', ' ').Trim();
        }

        private bool HasProfile(string id) =>
            !string.IsNullOrWhiteSpace(id) && _catalog.TryGetTrader(id, out _);

        private static bool TryMapped(
            IReadOnlyDictionary<string, string> map,
            string? key,
            out string profile)
        {
            if (!string.IsNullOrWhiteSpace(key) && map.TryGetValue(key, out profile!))
                return true;
            profile = string.Empty;
            return false;
        }

        private string FirstProfileId()
        {
            foreach (var pair in _catalog.Traders)
                return pair.Key;
            return DefaultProfileId;
        }
    }
}
