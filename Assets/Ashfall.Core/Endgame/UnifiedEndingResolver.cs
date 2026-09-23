// SPDX-License-Identifier: MIT
// Plan 145: Unified Ending Resolution & Epilogue Personalization
// Merges Holdfast endings, Muster epilogues, and Epilogue Matrix into a single coherent endgame resolution.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.Endgame
{
    public enum EndingCategory
    {
        Political = 0,
        Social = 1,
        Personal = 2,
        Moral = 3,
        Judicial = 4
    }

    public enum SurvivorFateStatus
    {
        Alive = 0,
        Deceased = 1,
        Retired = 2,
        Unknown = 3
    }

    [Serializable]
    public sealed class SurvivorEpilogueFate
    {
        public string survivorId { get; set; } = string.Empty;
        public string survivorName { get; set; } = string.Empty;
        public SurvivorFateStatus status { get; set; } = SurvivorFateStatus.Alive;
        public string notableTrait { get; set; } = string.Empty;
        public string epilogueText { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class UnifiedEndingContext
    {
        public int totalDaysSurvived { get; set; }
        public int livingDwellerCount { get; set; }
        public int totalDeathsRecorded { get; set; }
        public bool grandTreatySigned { get; set; }
        public bool tempestDecommissioned { get; set; }
        public bool debtLedgersBurned { get; set; }
        public bool childrenSurvived { get; set; }
        public bool velSecretExposed { get; set; }

        public string factionBranchId { get; set; } = "None"; // Military, Rebel, Independent, PRPF, None
        public string musterApproachId { get; set; } = string.Empty; // e.g. the_open_muster, the_amnesty
        public string moralChoiceBand { get; set; } = "Neutral"; // VeryPositive, Positive, Neutral, Negative, VeryEvil
        public string holdfastEndingId { get; set; } = string.Empty; // ending_holdfast_schedule, etc.
        public string verdictEndingId { get; set; } = string.Empty; // Recount, Held, Lease, None

        public List<SurvivorEpilogueFate> keySurvivorFates { get; set; } = new();
        public List<string> majorQuestCompletions { get; set; } = new();
        public List<string> expeditionDiscoveries { get; set; } = new();
        public List<string> shelterUpgrades { get; set; } = new();
        public Dictionary<string, int> factionStandings { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }

    [Serializable]
    public sealed class UnifiedEndingResult
    {
        public string resolutionId { get; set; } = string.Empty;
        public string overallTitle { get; set; } = string.Empty;
        public string campaignDurationLabel { get; set; } = string.Empty;
        public string durationProse { get; set; } = string.Empty;

        public string politicalOutcome { get; set; } = string.Empty;
        public string politicalProse { get; set; } = string.Empty;

        public string socialOutcome { get; set; } = string.Empty;
        public string socialProse { get; set; } = string.Empty;

        public string moralOutcome { get; set; } = string.Empty;
        public string moralProse { get; set; } = string.Empty;

        public string personalSummary { get; set; } = string.Empty;
        public List<SurvivorEpilogueFate> survivorEpilogues { get; set; } = new();

        public string expeditionProse { get; set; } = string.Empty;
        public string shelterProse { get; set; } = string.Empty;

        public List<string> legacyTraitsAwarded { get; set; } = new();
        public string fullPersonalizedChronicle { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class UnifiedEndingSaveState
    {
        public int schema_version { get; set; } = 1;
        public bool isResolved { get; set; }
        public UnifiedEndingResult? lastResult { get; set; }
    }

    public interface IPersonalizedEpilogueSink
    {
        void ReceivePersonalizedEpilogue(UnifiedEndingResult result);
    }

    /// <summary>
    /// Pure domain engine unifying Holdfast endings, Muster epilogues, and Epilogue Matrix
    /// into a personalized, literary-grade endgame resolution.
    /// Pure function of campaign state, deterministic, zero engine dependencies.
    /// </summary>
    public sealed class UnifiedEndingResolver
    {
        private readonly Dictionary<string, string> _durationTemplates = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _politicalTemplates = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _socialTemplates = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _moralTemplates = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _survivorFateTemplates = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _discoveryTemplates = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _shelterUpgradeTemplates = new(StringComparer.OrdinalIgnoreCase);

        private UnifiedEndingResult? _lastResult;
        private bool _isResolved;

        // Seams
        public Action<UnifiedEndingResult>? OnUnifiedEndingResolvedSeam;
        public Action<SurvivorEpilogueFate>? OnSurvivorEpilogueGeneratedSeam;

        public IPersonalizedEpilogueSink? EpilogueSink { get; set; }

        public bool IsResolved => _isResolved;
        public UnifiedEndingResult? LastResult => _lastResult;

        public UnifiedEndingResolver()
        {
            LoadDefaultTemplates();
        }

        public void LoadCatalog(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent)) return;

            ParseMap(jsonContent, "duration_variants", _durationTemplates);
            ParseMap(jsonContent, "political_templates", _politicalTemplates);
            ParseMap(jsonContent, "social_templates", _socialTemplates);
            ParseMap(jsonContent, "moral_templates", _moralTemplates);
            ParseMap(jsonContent, "survivor_fate_templates", _survivorFateTemplates);
            ParseMap(jsonContent, "discovery_templates", _discoveryTemplates);
            ParseMap(jsonContent, "shelter_upgrade_templates", _shelterUpgradeTemplates);
        }

        public void LoadCatalog(IFileIO fileIO, string path)
        {
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (!fileIO.FileExists(path))
                throw new System.IO.FileNotFoundException($"Epilogue personalization catalog not found at {path}");

            LoadCatalog(fileIO.ReadAllText(path));
        }

        public UnifiedEndingResult ResolveEnding(UnifiedEndingContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var result = new UnifiedEndingResult();
            result.resolutionId = $"res_{context.totalDaysSurvived}_{ComputeContextFingerprint(context)}";

            // 1. Duration & Overall Title
            if (context.totalDaysSurvived < 180)
            {
                result.campaignDurationLabel = "Short";
                result.durationProse = FormatTemplate(GetTemplate(_durationTemplates, "short", "The brief experiment lasted {days} days."), context.totalDaysSurvived);
            }
            else if (context.totalDaysSurvived <= 365)
            {
                result.campaignDurationLabel = "Medium";
                result.durationProse = FormatTemplate(GetTemplate(_durationTemplates, "medium", "Across a grueling year of {days} days, the shelter endured."), context.totalDaysSurvived);
            }
            else
            {
                result.campaignDurationLabel = "Long";
                result.durationProse = FormatTemplate(GetTemplate(_durationTemplates, "long", "Years stretched into legend over {days} days of enduring survival."), context.totalDaysSurvived);
            }

            // 2. Political Category: Holdfast ending priority or faction branch fallback
            string branch = context.factionBranchId?.ToLowerInvariant() ?? "none";
            string holdfastId = context.holdfastEndingId ?? string.Empty;

            if (!string.IsNullOrEmpty(holdfastId) && HoldfastEndings.IsKnown(holdfastId))
            {
                result.politicalOutcome = HoldfastEndings.DisplayName(holdfastId);
                if (holdfastId == HoldfastEndings.Schedule && branch == "military")
                    result.politicalProse = GetTemplate(_politicalTemplates, "military_schedule", HoldfastEndings.FlavorText(holdfastId));
                else if (holdfastId == HoldfastEndings.DarkRoad && branch == "rebel")
                    result.politicalProse = GetTemplate(_politicalTemplates, "rebel_dark_road", HoldfastEndings.FlavorText(holdfastId));
                else if (holdfastId == HoldfastEndings.Tender && branch == "independent")
                    result.politicalProse = GetTemplate(_politicalTemplates, "independent_tender", HoldfastEndings.FlavorText(holdfastId));
                else if (holdfastId == HoldfastEndings.Reserve && branch == "prpf")
                    result.politicalProse = GetTemplate(_politicalTemplates, "prpf_reserve", HoldfastEndings.FlavorText(holdfastId));
                else
                    result.politicalProse = HoldfastEndings.FlavorText(holdfastId);
            }
            else
            {
                if (branch == "military")
                {
                    result.politicalOutcome = "Garrison Protectorate";
                    result.politicalProse = GetTemplate(_politicalTemplates, "military_default", "The Garrison incorporated the shelter into its forward defense network.");
                }
                else if (branch == "rebel")
                {
                    result.politicalOutcome = "Free Commonwealth";
                    result.politicalProse = GetTemplate(_politicalTemplates, "rebel_default", "The free survivors formed an independent commonwealth.");
                }
                else if (branch == "independent")
                {
                    result.politicalOutcome = "Autonomous Redoubt";
                    result.politicalProse = GetTemplate(_politicalTemplates, "independent_default", "The shelter remained fiercely independent of external masters.");
                }
                else if (branch == "prpf")
                {
                    result.politicalOutcome = "Shadow Network Enclave";
                    result.politicalProse = GetTemplate(_politicalTemplates, "prpf_default", "The shelter became a hub for clandestine operations.");
                }
                else
                {
                    result.politicalOutcome = "Isolated Stronghold";
                    result.politicalProse = GetTemplate(_politicalTemplates, "none_fractured", "Without outside alliances, the bunker stood alone against the wastes.");
                }
            }

            // 3. Social Category: Muster approach or community cohesion
            string musterKey = context.musterApproachId?.ToLowerInvariant() ?? string.Empty;
            if (!string.IsNullOrEmpty(musterKey) && _socialTemplates.ContainsKey(musterKey))
            {
                result.socialOutcome = FormatTitle(musterKey);
                result.socialProse = _socialTemplates[musterKey];
            }
            else if (context.livingDwellerCount <= 0)
            {
                result.socialOutcome = "Ghost Shelter";
                result.socialProse = "Silence claimed the corridors. No living voice remains to remember what was built.";
            }
            else
            {
                result.socialOutcome = "Shelter Fraternity";
                result.socialProse = GetTemplate(_socialTemplates, "default_social", "The social fabric of the shelter remained tightly woven through shared hardship.");
            }

            // 4. Moral Category: Moral Choice Band
            string moralBand = context.moralChoiceBand?.ToLowerInvariant() ?? "neutral";
            result.moralOutcome = context.moralChoiceBand ?? "Neutral";
            if (moralBand.Contains("verypositive") || moralBand.Contains("very_positive") || moralBand == "saint")
            {
                result.moralProse = GetTemplate(_moralTemplates, "very_positive", "Your boundless compassion became a beacon in the dark.");
            }
            else if (moralBand.Contains("positive") || moralBand == "compassionate")
            {
                result.moralProse = GetTemplate(_moralTemplates, "positive", "Through measured mercy, you preserved dignity in an undignified age.");
            }
            else if (moralBand.Contains("veryevil") || moralBand.Contains("very_evil") || moralBand == "tyrant")
            {
                result.moralProse = GetTemplate(_moralTemplates, "very_evil", "Your cold ruthlessness became a terrifying legend in the wastes.");
            }
            else if (moralBand.Contains("negative") || moralBand == "ruthless")
            {
                result.moralProse = GetTemplate(_moralTemplates, "negative", "Harsh calculations left bitter memories among the survivors.");
            }
            else
            {
                result.moralProse = GetTemplate(_moralTemplates, "neutral", "Pragmatism guided every choice; decisions were recognized as necessary.");
            }

            // 5. Personal Survivor Epilogues
            result.survivorEpilogues.Clear();
            if (context.keySurvivorFates != null && context.keySurvivorFates.Count > 0)
            {
                foreach (var fate in context.keySurvivorFates)
                {
                    string text;
                    string name = string.IsNullOrEmpty(fate.survivorName) ? fate.survivorId : fate.survivorName;
                    string trait = fate.notableTrait?.ToLowerInvariant() ?? string.Empty;

                    if (fate.status == SurvivorFateStatus.Deceased)
                    {
                        text = GetTemplate(_survivorFateTemplates, "deceased_default", "{name}'s sacrifice was etched into the shelter memorial wall.")
                            .Replace("{name}", name);
                    }
                    else if (fate.status == SurvivorFateStatus.Retired)
                    {
                        text = GetTemplate(_survivorFateTemplates, "retired_default", "{name} passed working tools to the next generation and settled into an honored eldership.")
                            .Replace("{name}", name);
                    }
                    else
                    {
                        string templateKey = $"alive_{trait}";
                        if (_survivorFateTemplates.TryGetValue(templateKey, out var aliveTemplate))
                        {
                            text = aliveTemplate.Replace("{name}", name);
                        }
                        else
                        {
                            text = GetTemplate(_survivorFateTemplates, "alive_default", "{name} lived to see peace break through the ash.")
                                .Replace("{name}", name);
                        }
                    }

                    var epilogueFate = new SurvivorEpilogueFate
                    {
                        survivorId = fate.survivorId,
                        survivorName = name,
                        status = fate.status,
                        notableTrait = fate.notableTrait,
                        epilogueText = text
                    };

                    result.survivorEpilogues.Add(epilogueFate);
                    OnSurvivorEpilogueGeneratedSeam?.Invoke(epilogueFate);
                }

                result.personalSummary = $"Recorded {result.survivorEpilogues.Count} distinct personal epilogue chronicles.";
            }
            else
            {
                result.personalSummary = "No specific survivor chronicles were registered.";
            }

            // 6. Expeditions & Discoveries
            if (context.expeditionDiscoveries != null && context.expeditionDiscoveries.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var disc in context.expeditionDiscoveries)
                {
                    string key = disc.ToLowerInvariant();
                    if (_discoveryTemplates.TryGetValue(key, out var dt))
                    {
                        sb.Append(dt).Append(" ");
                    }
                    else
                    {
                        sb.Append($"The expedition to {disc} uncovered crucial salvaged relics. ");
                    }
                }
                result.expeditionProse = sb.ToString().Trim();
            }
            else
            {
                result.expeditionProse = "The wastes beyond the perimeter remained shrouded in mystery.";
            }

            // 7. Shelter Upgrades
            if (context.shelterUpgrades != null && context.shelterUpgrades.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var upg in context.shelterUpgrades)
                {
                    string key = upg.ToLowerInvariant();
                    if (_shelterUpgradeTemplates.TryGetValue(key, out var ut))
                    {
                        sb.Append(ut).Append(" ");
                    }
                    else
                    {
                        sb.Append($"The construction of {upg} fortified the shelter. ");
                    }
                }
                result.shelterProse = sb.ToString().Trim();
            }
            else
            {
                result.shelterProse = "The shelter retained its original austere concrete infrastructure.";
            }

            // 8. Legacy Traits Awarded
            result.legacyTraitsAwarded.Clear();
            if (context.grandTreatySigned) result.legacyTraitsAwarded.Add("legacy_trait_diplomat");
            if (context.livingDwellerCount >= 10) result.legacyTraitsAwarded.Add("legacy_trait_prosperous_haven");
            if (context.tempestDecommissioned) result.legacyTraitsAwarded.Add("legacy_trait_storm_breaker");
            if (moralBand.Contains("positive")) result.legacyTraitsAwarded.Add("legacy_trait_humanitarian");

            // Overall Title
            result.overallTitle = $"ASHFALL RESOLUTION: {result.politicalOutcome} & {result.socialOutcome}";

            // 9. Full Personalized Chronicle Synthesis
            var chronicle = new StringBuilder();
            chronicle.AppendLine($"=== {result.overallTitle} ===");
            chronicle.AppendLine();
            chronicle.AppendLine(result.durationProse);
            chronicle.AppendLine();
            chronicle.AppendLine($"[POLITICAL RESOLUTION: {result.politicalOutcome}]");
            chronicle.AppendLine(result.politicalProse);
            chronicle.AppendLine();
            chronicle.AppendLine($"[COMMUNITY RESOLUTION: {result.socialOutcome}]");
            chronicle.AppendLine(result.socialProse);
            chronicle.AppendLine();
            chronicle.AppendLine($"[ETHICAL LEGACY: {result.moralOutcome}]");
            chronicle.AppendLine(result.moralProse);
            chronicle.AppendLine();

            if (!string.IsNullOrEmpty(result.expeditionProse))
            {
                chronicle.AppendLine("[EXPEDITION DISCOVERIES]");
                chronicle.AppendLine(result.expeditionProse);
                chronicle.AppendLine();
            }

            if (!string.IsNullOrEmpty(result.shelterProse))
            {
                chronicle.AppendLine("[SHELTER EXPANSION]");
                chronicle.AppendLine(result.shelterProse);
                chronicle.AppendLine();
            }

            if (result.survivorEpilogues.Count > 0)
            {
                chronicle.AppendLine("[FATES OF THE SURVIVORS]");
                foreach (var s in result.survivorEpilogues)
                {
                    chronicle.AppendLine($"* {s.epilogueText}");
                }
                chronicle.AppendLine();
            }

            result.fullPersonalizedChronicle = chronicle.ToString().Trim();

            _lastResult = result;
            _isResolved = true;

            OnUnifiedEndingResolvedSeam?.Invoke(result);
            EpilogueSink?.ReceivePersonalizedEpilogue(result);

            return result;
        }

        public UnifiedEndingSaveState CaptureState()
        {
            return new UnifiedEndingSaveState
            {
                schema_version = 1,
                isResolved = _isResolved,
                lastResult = _lastResult
            };
        }

        public void RestoreState(UnifiedEndingSaveState? state)
        {
            if (state == null)
            {
                _isResolved = false;
                _lastResult = null;
                return;
            }

            _isResolved = state.isResolved;
            _lastResult = state.lastResult;
        }

        private static string FormatTemplate(string template, int days)
        {
            return template.Replace("{days}", days.ToString());
        }

        /// <summary>
        /// Deterministic short fingerprint of the ending-relevant campaign facts.
        /// Wall-clock and Guid identifiers are forbidden in deterministic Core state:
        /// the same ending context must always resolve to the same resolution id.
        /// </summary>
        internal static string ComputeContextFingerprint(UnifiedEndingContext context)
        {
            var sb = new StringBuilder();
            sb.Append(context.totalDaysSurvived).Append('|')
              .Append(context.livingDwellerCount).Append('|')
              .Append(context.totalDeathsRecorded).Append('|')
              .Append(context.grandTreatySigned ? '1' : '0').Append('|')
              .Append(context.tempestDecommissioned ? '1' : '0').Append('|')
              .Append(context.debtLedgersBurned ? '1' : '0').Append('|')
              .Append(context.childrenSurvived ? '1' : '0').Append('|')
              .Append(context.velSecretExposed ? '1' : '0').Append('|')
              .Append(context.factionBranchId).Append('|')
              .Append(context.musterApproachId).Append('|')
              .Append(context.moralChoiceBand).Append('|')
              .Append(context.holdfastEndingId).Append('|')
              .Append(context.verdictEndingId);

            using var sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            var hex = new StringBuilder(8);
            for (int i = 0; i < 4; i++)
            {
                hex.Append(hash[i].ToString("x2"));
            }
            return hex.ToString();
        }

        private static string GetTemplate(Dictionary<string, string> dict, string key, string fallback)
        {
            if (dict.TryGetValue(key, out var val)) return val;
            return fallback;
        }

        private static string FormatTitle(string snake)
        {
            if (string.IsNullOrEmpty(snake)) return string.Empty;
            string[] words = snake.Split('_');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpperInvariant(words[i][0]) + words[i].Substring(1);
                }
            }
            return string.Join(" ", words);
        }

        private void LoadDefaultTemplates()
        {
            _durationTemplates["short"] = "The brief experiment at the shelter lasted {days} days before the final reckoning arrived.";
            _durationTemplates["medium"] = "Across a grueling year of survival spanning {days} days, the shelter etched its enduring mark upon the wastes.";
            _durationTemplates["long"] = "Years stretched into legend over {days} days as the shelter outlasted storms, starvation, and the collapse of empires.";

            _politicalTemplates["military_schedule"] = "The Garrison's iron discipline became the bunker's law under the unrelenting schedule, establishing an unbending martial order.";
            _politicalTemplates["military_default"] = "The Garrison incorporated the shelter into its forward defense perimeter, enforcing stability through military presence.";
            _politicalTemplates["rebel_dark_road"] = "The resistance's fire spread from the wastes into the shelter along the dark road, tearing down old hierarchies.";
            _politicalTemplates["rebel_default"] = "The free survivors formed a decentralized commonwealth, fiercely defending their autonomy from regional despots.";
            _politicalTemplates["independent_tender"] = "The Fleet's arrival transformed the bunker from an isolated refuge into a thriving crossroads port for trade caravans.";
            _politicalTemplates["independent_default"] = "Standing resolute without external masters, the shelter prospered through pragmatic alliances and fierce self-reliance.";
            _politicalTemplates["prpf_reserve"] = "The hidden power emerged from the shadows to claim what was owed from the reserve, establishing covert governance.";
            _politicalTemplates["prpf_default"] = "The clandestine PRPF network integrated the shelter's stockpiles into their shadow economy.";
            _politicalTemplates["none_fractured"] = "Without outside alliances, the bunker stood alone against the wastes as fractured warlords squabbled in the distance.";

            _socialTemplates["the_open_muster"] = "The substation rally point grew into an open commonwealth where demobilized conscripts and wanderers built anew.";
            _socialTemplates["the_amnesty"] = "The signed amnesty petition held firm, ending fugitive status and closing decades of historical retribution.";
            _socialTemplates["the_corridor"] = "Quiet corridors and dispersed routes let people slip past conflict and survive peacefully in small clusters.";
            _socialTemplates["the_blood_price"] = "The heavy blood price was paid, leaving quiet graves and chilling confessions in the bunker's archives.";
            _socialTemplates["the_rate_card_revised"] = "A publicly audited water rate card brought transparency and ended unilateral extortion across the basin.";
            _socialTemplates["the_administrator"] = "A new administrator took command of the water plant, shifting power without softening the daily hardship.";
            _socialTemplates["the_measured_truth"] = "The uncompromised broadcast resonated across the valley, setting an enduring benchmark for scientific truth.";
            _socialTemplates["the_measured_truth_contested"] = "The contested findings sparked unending debate, keeping the valley's future precariously balanced.";
            _socialTemplates["default_social"] = "The social fabric of the shelter remained tightly woven through shared hardship and mutual trust.";

            _moralTemplates["very_positive"] = "Your boundless compassion became a beacon in the dark, proving that humanity could outlive the ruins of civilization.";
            _moralTemplates["positive"] = "Through measured mercy and consistent principle, you preserved human dignity in an undignified age.";
            _moralTemplates["neutral"] = "Pragmatism guided every choice; decisions were neither celebrated nor cursed, merely recognized as necessary for survival.";
            _moralTemplates["negative"] = "Harsh calculations left bitter memories among those who bore the weight of survival under your rule.";
            _moralTemplates["very_evil"] = "Your cold ruthlessness became a terrifying legend whispered around wasteland campfires by those who feared your wrath.";

            _survivorFateTemplates["alive_social"] = "{name} became the heart of the emerging settlement, fostering community bonds and welcoming weary travelers.";
            _survivorFateTemplates["alive_independent"] = "{name} took to the perimeter trails, scouting safe passages and mapping uncharted wastes for future generations.";
            _survivorFateTemplates["alive_ambitious"] = "{name} rose to civic prominence, drafting the community's foundational trade treaties and civil charters.";
            _survivorFateTemplates["alive_stubborn"] = "{name} guarded the storehouse vaults, ensuring no ration was ever misappropriated or wasted.";
            _survivorFateTemplates["alive_default"] = "{name} lived to see green shoots break through the ash, enjoying hard-won peace in the shelter.";
            _survivorFateTemplates["deceased_default"] = "{name}'s sacrifice was etched into the shelter's memorial wall, an eternal reminder of the price of survival.";
            _survivorFateTemplates["retired_default"] = "{name} passed the working tools to the next generation and settled into an honored role as shelter elder.";

            _discoveryTemplates["water_aquifer"] = "The discovery of the deep clean aquifer secured fresh drinking water for generations.";
            _discoveryTemplates["copper_vein"] = "The rich copper vein provided the raw conductors needed for continuous electrification.";
            _discoveryTemplates["medical_cache"] = "The recovered antibiotics cache saved dozens from the dreaded winter fever.";
            _discoveryTemplates["default_discovery"] = "The wasteland expeditions returned with crucial salvaged relics of old-world science.";

            _shelterUpgradeTemplates["greenhouse_dome"] = "The verdant greenhouse dome fed generations long after the old world's preserved tins ran out.";
            _shelterUpgradeTemplates["perimeter_turrets"] = "The automated defensive perimeter repelled every bandit raid without a single inner breach.";
            _shelterUpgradeTemplates["water_filtration_array"] = "The high-yield filtration array produced crystal clear water daily, quenching the valley's thirst.";
            _shelterUpgradeTemplates["default_upgrade"] = "The structural fortifications transformed the fragile redoubt into a permanent home for humanity.";
        }

        private static void ParseMap(string json, string sectionName, Dictionary<string, string> target)
        {
            string sectionToken = $"\"{sectionName}\"";
            int idx = json.IndexOf(sectionToken, StringComparison.Ordinal);
            if (idx < 0) return;

            int objStart = json.IndexOf('{', idx + sectionToken.Length);
            if (objStart < 0) return;

            int objEnd = FindMatchingBrace(json, objStart);
            if (objEnd <= objStart) return;

            string body = json.Substring(objStart + 1, objEnd - objStart - 1);
            int pos = 0;
            while (pos < body.Length)
            {
                int kQuoteStart = body.IndexOf('"', pos);
                if (kQuoteStart < 0) break;
                int kQuoteEnd = body.IndexOf('"', kQuoteStart + 1);
                if (kQuoteEnd < 0) break;

                string key = body.Substring(kQuoteStart + 1, kQuoteEnd - kQuoteStart - 1);
                int colon = body.IndexOf(':', kQuoteEnd + 1);
                if (colon < 0) break;

                int vQuoteStart = body.IndexOf('"', colon + 1);
                if (vQuoteStart < 0) break;
                int vQuoteEnd = body.IndexOf('"', vQuoteStart + 1);
                while (vQuoteEnd < body.Length && body[vQuoteEnd - 1] == '\\')
                {
                    vQuoteEnd = body.IndexOf('"', vQuoteEnd + 1);
                }
                if (vQuoteEnd < 0) break;

                string val = body.Substring(vQuoteStart + 1, vQuoteEnd - vQuoteStart - 1);
                target[key] = val;

                pos = vQuoteEnd + 1;
            }
        }

        private static int FindMatchingBrace(string text, int openIndex)
        {
            int depth = 0;
            bool inString = false;
            for (int i = openIndex; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '"' && (i == 0 || text[i - 1] != '\\'))
                {
                    inString = !inString;
                }
                else if (!inString)
                {
                    if (c == '{') depth++;
                    else if (c == '}')
                    {
                        depth--;
                        if (depth == 0) return i;
                    }
                }
            }
            return -1;
        }

        public UnifiedEndingCensus GetCensus()
        {
            return new UnifiedEndingCensus(
                _isResolved,
                _durationTemplates.Count,
                _politicalTemplates.Count,
                _socialTemplates.Count,
                _moralTemplates.Count,
                _survivorFateTemplates.Count,
                _discoveryTemplates.Count,
                _shelterUpgradeTemplates.Count,
                _lastResult?.survivorEpilogues?.Count ?? 0);
        }
    }

    public readonly struct UnifiedEndingCensus
    {
        public readonly bool IsResolved;
        public readonly int DurationTemplatesCount;
        public readonly int PoliticalTemplatesCount;
        public readonly int SocialTemplatesCount;
        public readonly int MoralTemplatesCount;
        public readonly int SurvivorFateTemplatesCount;
        public readonly int DiscoveryTemplatesCount;
        public readonly int ShelterUpgradeTemplatesCount;
        public readonly int EpiloguesGeneratedCount;

        public UnifiedEndingCensus(bool isResolved, int dur, int pol, int soc, int mor, int surv, int disc, int upg, int epilogues)
        {
            IsResolved = isResolved;
            DurationTemplatesCount = dur;
            PoliticalTemplatesCount = pol;
            SocialTemplatesCount = soc;
            MoralTemplatesCount = mor;
            SurvivorFateTemplatesCount = surv;
            DiscoveryTemplatesCount = disc;
            ShelterUpgradeTemplatesCount = upg;
            EpiloguesGeneratedCount = epilogues;
        }
    }
}

