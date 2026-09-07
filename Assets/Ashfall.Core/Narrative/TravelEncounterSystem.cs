using System;
using System.Collections.Generic;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Narrative
{
    public enum ChoiceRequirementFailureType
    {
        None = 0,
        MissingRequiredItem = 1,
        InsufficientCostItems = 2,
        MissingRequiredFlag = 3,
        RecognitionCondition = 4,
        StandingCondition = 5
    }

    public sealed class ChoiceRequirementFailure
    {
        public ChoiceRequirementFailureType FailureType { get; set; }
        public string ItemId { get; set; } = string.Empty;
        public int RequiredQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public sealed class ChoiceAvailabilityResult
    {
        public bool IsAvailable => Failures.Count == 0;
        public List<ChoiceRequirementFailure> Failures { get; } = new List<ChoiceRequirementFailure>();
    }

    public sealed class TravelEncounterResolutionPlan
    {
        public string EncounterId { get; init; } = string.Empty;
        public string ChoiceId { get; init; } = string.Empty;
        public int CurrentDay { get; init; }
        public string CooldownKey { get; init; } = string.Empty;
        public int CooldownExpiryDay { get; init; }
        public bool IsOnCooldown { get; init; }
        public int MoraleDelta { get; init; }
        public int GuiltDelta { get; init; }
        public string RawFactionId { get; init; } = string.Empty;
        public string CanonicalFactionId { get; init; } = string.Empty;
        public int FactionStandingDelta { get; init; }
        public string UnlocksFieldGuideId { get; init; } = string.Empty;
        public int AdvancesChainStage { get; init; }
        public string? ChainId { get; init; }
        public IReadOnlyList<NormalizedItemCost> Costs { get; init; } = Array.Empty<NormalizedItemCost>();
        public string RequiredItemId { get; init; } = string.Empty;
        public int RequiredItemQuantity { get; init; }
        public string RequiredFlag { get; init; } = string.Empty;
        public ChoiceAvailabilityResult Availability { get; init; } = new ChoiceAvailabilityResult();
        public bool CanExecute => !IsOnCooldown && Availability.IsAvailable;
    }

    public sealed class TravelEncounterResolutionResult
    {
        public string EncounterId { get; set; } = string.Empty;
        public string ChoiceId { get; set; } = string.Empty;
        public int Day { get; set; }
        public int MoraleDelta { get; set; }
        public int GuiltDelta { get; set; }
        public string FactionId { get; set; } = string.Empty;
        public string CanonicalFactionId { get; set; } = string.Empty;
        public int FactionStandingDelta { get; set; }
        public string UnlocksFieldGuideId { get; set; } = string.Empty;
        public int ChainStageAdvanced { get; set; }
        public List<NormalizedItemCost> DeductedCosts { get; set; } = new List<NormalizedItemCost>();
        public string CooldownKey { get; set; } = string.Empty;
        public FactionBountyRecord? BountyRecord { get; set; }
    }

    /// <summary>
    /// Stable, read-only projection of the player's prior contact with one
    /// patrol faction. The projection is deliberately smaller than the save
    /// records so selection and presentation never need to inspect raw history.
    /// </summary>
    public sealed class PatrolRecognitionContext
    {
        public string FactionId { get; init; } = string.Empty;
        public string EncounterId { get; init; } = string.Empty;
        public IReadOnlyList<string> PriorChoiceIds { get; init; } = Array.Empty<string>();
        public int EncountersWithFaction { get; init; }
        public int PaidTollCount { get; init; }
        public int FoughtPatrolCount { get; init; }
        public int CurrentChainStage { get; init; }
        public int CurrentStanding { get; init; }
        public IReadOnlyList<string> RecognitionTags { get; init; } = Array.Empty<string>();

        public bool HasChoice(string choiceId)
        {
            if (string.IsNullOrWhiteSpace(choiceId)) return false;
            for (int i = 0; i < PriorChoiceIds.Count; i++)
            {
                if (string.Equals(PriorChoiceIds[i], choiceId, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public bool HasTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return false;
            for (int i = 0; i < RecognitionTags.Count; i++)
            {
                if (string.Equals(RecognitionTags[i], tag, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Presentation-only projection for a patrol choice. It carries the
    /// authoritative availability result and the authored mechanics needed by
    /// a host view; it does not own or mutate encounter state.
    /// </summary>
    public sealed class PatrolChoicePresentation
    {
        public string ChoiceId { get; init; } = string.Empty;
        public string Text { get; init; } = string.Empty;
        public bool IsAvailable { get; init; }
        public string DisabledReasonCode { get; init; } = string.Empty;
        public string RequiredItemId { get; init; } = string.Empty;
        public int RequiredItemQuantity { get; init; }
        public IReadOnlyList<NormalizedItemCost> Costs { get; init; } = Array.Empty<NormalizedItemCost>();
        public int MoraleDelta { get; init; }
        public int GuiltDelta { get; init; }
        public string FactionId { get; init; } = string.Empty;
        public int FactionStandingDelta { get; init; }
        public IReadOnlyList<ChoiceRequirementFailure> Failures { get; init; } = Array.Empty<ChoiceRequirementFailure>();
    }

    /// <summary>Stable host-facing patrol header and choice projection.</summary>
    public sealed class PatrolEncounterPresentation
    {
        public string EncounterId { get; init; } = string.Empty;
        /// <summary>Canonical systems ID for standing, history, and other Core integrations.</summary>
        public string FactionId { get; init; } = string.Empty;
        /// <summary>Authored lore ID used by hosts to resolve faction names and emblems.</summary>
        public string DisplayFactionId { get; init; } = string.Empty;
        public string TerritoryState { get; init; } = string.Empty;
        public string PatrolArchetype { get; init; } = string.Empty;
        public int CurrentChainStage { get; init; }
        public string RecognitionLabel { get; init; } = string.Empty;
        public IReadOnlyList<PatrolChoicePresentation> Choices { get; init; } = Array.Empty<PatrolChoicePresentation>();
    }

    public sealed class TravelEncounterSystem
    {
        private readonly TravelEncounterCatalog _catalog;
        private readonly Dictionary<string, int> _chainStages = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> _patrolChainStages = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> _encounterAvailableDay = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, PatrolHistoryRecord> _patrolHistory = new(StringComparer.OrdinalIgnoreCase);
        private Inventory.Inventory? _inventory;
        private FactionWarSystem? _factionWar;
        private World.ITerritoryAuthority? _territoryAuthority;
        private FactionBountySystem? _bountySystem;

        public TravelEncounterCatalog Catalog => _catalog;

        public Inventory.Inventory? Inventory
        {
            get => _inventory;
            set => _inventory = value;
        }

        public FactionWarSystem? FactionWar
        {
            get => _factionWar;
            set => _factionWar = value;
        }

        public World.ITerritoryAuthority? TerritoryAuthority
        {
            get => _territoryAuthority;
            set => _territoryAuthority = value;
        }

        public FactionBountySystem? BountySystem
        {
            get => _bountySystem;
            set => _bountySystem = value;
        }

        public event Action<string, string>? OnChoiceResolved;
        public event Action<string, int>? OnChainStageAdvanced;
        public event Action<string, string>? OnPatrolHistoryRecorded;

        public TravelEncounterSystem(TravelEncounterCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public TravelEncounterSystem(
            TravelEncounterCatalog catalog,
            Inventory.Inventory? inventory,
            FactionWarSystem? factionWar = null,
            World.ITerritoryAuthority? territoryAuthority = null,
            FactionBountySystem? bountySystem = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _inventory = inventory;
            _factionWar = factionWar;
            _territoryAuthority = territoryAuthority;
            _bountySystem = bountySystem;
        }

        public static string GetCooldownKey(TravelEncounterDefinition encounter)
        {
            if (encounter == null) return string.Empty;
            return !string.IsNullOrWhiteSpace(encounter.CooldownGroup)
                ? encounter.CooldownGroup.Trim()
                : encounter.Id;
        }

        public int GetCooldownExpiry(string cooldownKey)
        {
            if (string.IsNullOrWhiteSpace(cooldownKey)) return 0;
            return _encounterAvailableDay.TryGetValue(cooldownKey.Trim(), out int exp) ? exp : 0;
        }

        public int GetCooldownExpiry(TravelEncounterDefinition encounter)
        {
            if (encounter == null) return 0;
            return GetCooldownExpiry(GetCooldownKey(encounter));
        }

        public int GetChainStage(string chainId)
        {
            if (string.IsNullOrEmpty(chainId)) return 0;
            return _chainStages.TryGetValue(chainId, out int stage) ? stage : 0;
        }

        public void SetChainStage(string chainId, int stage)
        {
            if (string.IsNullOrEmpty(chainId)) return;
            _chainStages[chainId] = stage;
            OnChainStageAdvanced?.Invoke(chainId, stage);
        }

        private static string GetPatrolChainKey(string factionId, string chainId)
        {
            string faction = FactionStandingIdResolver.ToSystemsId(factionId);
            return string.IsNullOrWhiteSpace(chainId)
                ? string.Empty
                : faction + "::" + chainId.Trim();
        }

        private static string GetPatrolHistoryKey(string factionId, string encounterId, string choiceId)
        {
            return FactionStandingIdResolver.ToSystemsId(factionId) + "::" +
                   (encounterId ?? string.Empty).Trim() + "::" + (choiceId ?? string.Empty).Trim();
        }

        public int GetPatrolChainStage(string factionId, string chainId)
        {
            string key = GetPatrolChainKey(factionId, chainId);
            return string.IsNullOrEmpty(key) || !_patrolChainStages.TryGetValue(key, out int stage) ? 0 : stage;
        }

        public int GetPatrolChainStage(TravelEncounterDefinition encounter)
        {
            if (encounter == null) return 0;
            return GetPatrolChainStage(encounter.FactionId, encounter.ChainId);
        }

        public PatrolRecognitionContext GetRecognitionContext(string factionId, string encounterId = "", string chainId = "")
        {
            string canonicalFaction = FactionStandingIdResolver.ToSystemsId(factionId);
            var choices = new List<string>();
            var tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            int factionCount = 0;
            int paidTolls = 0;
            int fights = 0;

            foreach (var record in _patrolHistory.Values)
            {
                if (!string.Equals(record.FactionId, canonicalFaction, StringComparison.OrdinalIgnoreCase)) continue;
                factionCount += Math.Max(1, record.TimesSelected);
                if (string.IsNullOrWhiteSpace(encounterId) || string.Equals(record.EncounterId, encounterId, StringComparison.OrdinalIgnoreCase))
                    choices.Add(record.ChoiceId);

                if (record.Tags != null)
                {
                    foreach (var tag in record.Tags)
                    {
                        if (string.IsNullOrWhiteSpace(tag)) continue;
                        tags.Add(tag);
                        if (string.Equals(tag, "paid_toll", StringComparison.OrdinalIgnoreCase)) paidTolls += Math.Max(1, record.TimesSelected);
                        if (string.Equals(tag, "hostile", StringComparison.OrdinalIgnoreCase)) fights += Math.Max(1, record.TimesSelected);
                    }
                }
            }

            int standing = _factionWar?.GetStanding(canonicalFaction) ?? 0;
            return new PatrolRecognitionContext
            {
                FactionId = canonicalFaction,
                EncounterId = encounterId ?? string.Empty,
                PriorChoiceIds = choices,
                EncountersWithFaction = factionCount,
                PaidTollCount = paidTolls,
                FoughtPatrolCount = fights,
                CurrentChainStage = GetPatrolChainStage(canonicalFaction, chainId),
                CurrentStanding = standing,
                RecognitionTags = new List<string>(tags)
            };
        }

        public PatrolRecognitionContext GetRecognitionContext(TravelEncounterDefinition encounter)
        {
            if (encounter == null) return new PatrolRecognitionContext();
            return GetRecognitionContext(encounter.FactionId, encounter.Id, encounter.ChainId);
        }

        public bool IsEncounterEligible(
            TravelEncounterDefinition encounter,
            string region,
            float dangerLevel,
            string currentSeason,
            int currentDay,
            string locationId = "")
        {
            if (encounter == null) return false;

            // Cooldown check using canonical cooldown key
            string cdKey = GetCooldownKey(encounter);
            if (_encounterAvailableDay.TryGetValue(cdKey, out int nextDay) && currentDay < nextDay)
            {
                return false;
            }

            // Danger level range
            if (dangerLevel < encounter.MinDangerLevel || dangerLevel > encounter.MaxDangerLevel)
            {
                return false;
            }

            // Region filter
            if (encounter.RegionTags != null && encounter.RegionTags.Count > 0 && !string.IsNullOrEmpty(region))
            {
                if (!encounter.RegionTags.Exists(r => string.Equals(r, region, StringComparison.OrdinalIgnoreCase) || string.Equals(r, "all", StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
            }

            // Season filter
            if (encounter.SeasonTags != null && encounter.SeasonTags.Count > 0 && !string.IsNullOrEmpty(currentSeason))
            {
                if (!encounter.SeasonTags.Exists(s => string.Equals(s, currentSeason, StringComparison.OrdinalIgnoreCase) || string.Equals(s, "all", StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
            }

            // Chain progression filter
            if (!string.IsNullOrEmpty(encounter.ChainId))
            {
                int currentStage = IsPatrol(encounter)
                    ? GetPatrolChainStage(encounter)
                    : GetChainStage(encounter.ChainId);
                // -1 is the authored "any stage" value used by recurring
                // patrol variants. Legacy chains retain exact-stage matching.
                if (encounter.PrereqChainStage >= 0 && encounter.PrereqChainStage != currentStage)
                {
                    return false;
                }
            }

            // Flagship VII (F11) — War-state filter
            bool isWartime = _factionWar?.IsAtWar == true;
            if (encounter.WarState == TravelEncounterWarState.Peacetime && isWartime)
            {
                return false;
            }
            if (encounter.WarState == TravelEncounterWarState.Wartime && !isWartime)
            {
                return false;
            }

            // Flagship VII (F12) — Dynamic territory filter
            if (!string.IsNullOrWhiteSpace(encounter.RequiredTerritoryOwner) && _territoryAuthority != null)
            {
                string resolvedLocation = World.PatrolTerritoryResolver.ResolveLocation(locationId, region);
                if (string.IsNullOrWhiteSpace(resolvedLocation) ||
                    !_territoryAuthority.IsClaimedBy(resolvedLocation, encounter.RequiredTerritoryOwner))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsEncounterEligible(TravelEncounterDefinition encounter, TravelEncounterSelectionContext context)
        {
            if (encounter == null || context == null) return false;
            return IsEncounterEligible(
                encounter,
                context.Region,
                context.DangerLevel,
                context.CurrentSeason,
                context.CurrentDay,
                context.LocationId);
        }

        public float GetEffectiveWeight(TravelEncounterDefinition encounter, string stance)
        {
            if (encounter == null) return 0f;
            float weight = encounter.BaseWeight;

            if (!string.IsNullOrEmpty(stance) && encounter.StanceWeights != null && encounter.StanceWeights.TryGetValue(stance, out float multiplier))
            {
                weight *= multiplier;
            }

            // Flagship VII (F11) — Wartime weight multiplier
            if (_factionWar?.IsAtWar == true && encounter.WarWeightMultiplier > 0f)
            {
                weight *= encounter.WarWeightMultiplier;
            }

            if (IsPatrol(encounter))
            {
                var recognition = GetRecognitionContext(encounter);
                // Hostile Warlord contact makes future raid parties more
                // likely, while remaining a bounded authored runtime modifier.
                if (recognition.HasTag("hostile") &&
                    string.Equals(FactionStandingIdResolver.ToSystemsId(encounter.FactionId), "faction_scavenger_warlords", StringComparison.OrdinalIgnoreCase))
                {
                    weight *= 1.25f;
                }
            }

            return Math.Max(0.01f, weight);
        }

        private static bool IsPatrol(TravelEncounterDefinition encounter) =>
            encounter != null && encounter.Id.StartsWith("enc_patrol_", StringComparison.OrdinalIgnoreCase);

        public TravelEncounterDefinition? SelectEncounter(
            string region,
            float dangerLevel,
            string stance,
            string currentSeason,
            int currentDay,
            ISeededRng rng,
            string locationId = "")
        {
            var eligible = new List<TravelEncounterDefinition>();
            var weights = new List<float>();
            float totalWeight = 0f;

            foreach (var encounter in _catalog.Encounters)
            {
                if (IsEncounterEligible(encounter, region, dangerLevel, currentSeason, currentDay, locationId))
                {
                    float w = GetEffectiveWeight(encounter, stance);
                    eligible.Add(encounter);
                    weights.Add(w);
                    totalWeight += w;
                }
            }

            if (eligible.Count == 0 || totalWeight <= 0f)
            {
                return null;
            }

            float roll = (float)(rng.NextDouble() * totalWeight);
            float accum = 0f;

            for (int i = 0; i < eligible.Count; i++)
            {
                accum += weights[i];
                if (roll <= accum || i == eligible.Count - 1)
                {
                    return eligible[i];
                }
            }

            return eligible[0];
        }

        public TravelEncounterDefinition? SelectEncounter(TravelEncounterSelectionContext context)
        {
            if (context == null) return null;
            var rng = context.Rng ?? new SeededRng(context.CurrentDay * 397 + 17);
            return SelectEncounter(
                context.Region,
                context.DangerLevel,
                context.Stance,
                context.CurrentSeason,
                context.CurrentDay,
                rng,
                context.LocationId);
        }

        public List<(TravelEncounterDefinition encounter, float weight)> GetEligiblePatrolCandidates(
            string region, float dangerLevel, string stance, string currentSeason, int currentDay, string locationId = "")
        {
            var list = new List<(TravelEncounterDefinition, float)>();
            foreach (var enc in _catalog.Encounters)
            {
                if (!enc.Id.StartsWith("enc_patrol_", StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(enc.FactionId))
                    continue;

                if (IsEncounterEligible(enc, region, dangerLevel, currentSeason, currentDay, locationId))
                {
                    float w = GetEffectiveWeight(enc, stance);
                    if (w > 0f)
                    {
                        list.Add((enc, w));
                    }
                }
            }
            return list;
        }

        public ChoiceAvailabilityResult EvaluateChoiceAvailability(
            TravelEncounterChoice choice,
            Inventory.Inventory? inventory = null,
            Func<string, bool>? flagEvaluator = null)
        {
            var result = new ChoiceAvailabilityResult();
            if (choice == null) return result;
            var inv = inventory ?? _inventory;

            // Check required item gate
            if (!string.IsNullOrWhiteSpace(choice.RequiredItemId) && choice.RequiredItemQuantity > 0)
            {
                int avail = inv?.CountById(choice.RequiredItemId) ?? 0;
                if (avail < choice.RequiredItemQuantity)
                {
                    result.Failures.Add(new ChoiceRequirementFailure
                    {
                        FailureType = ChoiceRequirementFailureType.MissingRequiredItem,
                        ItemId = choice.RequiredItemId,
                        RequiredQuantity = choice.RequiredItemQuantity,
                        AvailableQuantity = avail,
                        Reason = $"Missing required item '{choice.RequiredItemId}' (need {choice.RequiredItemQuantity}, have {avail})"
                    });
                }
            }

            // Check required flag gate
            if (!string.IsNullOrWhiteSpace(choice.RequiredFlag))
            {
                if (flagEvaluator != null && !flagEvaluator(choice.RequiredFlag))
                {
                    result.Failures.Add(new ChoiceRequirementFailure
                    {
                        FailureType = ChoiceRequirementFailureType.MissingRequiredFlag,
                        Reason = $"Missing required flag '{choice.RequiredFlag}'"
                    });
                }
            }

            // Check cost items
            var costs = choice.GetNormalizedCosts();
            foreach (var cost in costs)
            {
                int avail = inv?.CountById(cost.ItemId) ?? 0;
                if (avail < cost.Quantity)
                {
                    result.Failures.Add(new ChoiceRequirementFailure
                    {
                        FailureType = ChoiceRequirementFailureType.InsufficientCostItems,
                        ItemId = cost.ItemId,
                        RequiredQuantity = cost.Quantity,
                        AvailableQuantity = avail,
                        Reason = $"Insufficient item '{cost.ItemId}' (x{cost.Quantity}, have {avail})"
                    });
                }
            }

            return result;
        }

        public ChoiceAvailabilityResult EvaluateChoiceAvailability(
            TravelEncounterDefinition encounter,
            TravelEncounterChoice choice,
            Inventory.Inventory? inventory = null,
            Func<string, bool>? flagEvaluator = null)
        {
            var result = EvaluateChoiceAvailability(choice, inventory, flagEvaluator);
            if (encounter == null || choice == null || !IsPatrol(encounter)) return result;

            var recognition = GetRecognitionContext(encounter);
            if (choice.RequiredChainStage >= 0 && recognition.CurrentChainStage != choice.RequiredChainStage)
            {
                result.Failures.Add(new ChoiceRequirementFailure
                {
                    FailureType = ChoiceRequirementFailureType.RecognitionCondition,
                    Reason = $"Requires patrol chain stage {choice.RequiredChainStage}; current stage is {recognition.CurrentChainStage}."
                });
            }
            if (choice.MaxChainStage >= 0 && recognition.CurrentChainStage > choice.MaxChainStage)
            {
                result.Failures.Add(new ChoiceRequirementFailure
                {
                    FailureType = ChoiceRequirementFailureType.RecognitionCondition,
                    Reason = $"Choice is no longer available after patrol chain stage {choice.MaxChainStage}."
                });
            }
            if (choice.RequiredHistoryTags != null)
            {
                foreach (var tag in choice.RequiredHistoryTags)
                {
                    if (!recognition.HasTag(tag))
                    {
                        result.Failures.Add(new ChoiceRequirementFailure
                        {
                            FailureType = ChoiceRequirementFailureType.RecognitionCondition,
                            Reason = $"Requires prior patrol history '{tag}'."
                        });
                    }
                }
            }
            if (choice.ForbiddenHistoryTags != null)
            {
                foreach (var tag in choice.ForbiddenHistoryTags)
                {
                    if (recognition.HasTag(tag))
                    {
                        result.Failures.Add(new ChoiceRequirementFailure
                        {
                            FailureType = ChoiceRequirementFailureType.RecognitionCondition,
                            Reason = $"Unavailable after prior patrol history '{tag}'."
                        });
                    }
                }
            }

            string faction = !string.IsNullOrWhiteSpace(choice.FactionId) ? choice.FactionId : encounter.FactionId;
            int standing = _factionWar?.GetStanding(FactionStandingIdResolver.ToSystemsId(faction)) ?? 0;
            if (choice.MinFactionStanding.HasValue && standing < choice.MinFactionStanding.Value)
            {
                result.Failures.Add(new ChoiceRequirementFailure
                {
                    FailureType = ChoiceRequirementFailureType.StandingCondition,
                    Reason = $"Requires standing at least {choice.MinFactionStanding.Value}; current standing is {standing}."
                });
            }
            if (choice.MaxFactionStanding.HasValue && standing > choice.MaxFactionStanding.Value)
            {
                result.Failures.Add(new ChoiceRequirementFailure
                {
                    FailureType = ChoiceRequirementFailureType.StandingCondition,
                    Reason = $"Requires standing at most {choice.MaxFactionStanding.Value}; current standing is {standing}."
                });
            }

            return result;
        }

        /// <summary>
        /// Builds the authoritative, read-only view projection for one patrol.
        /// Eligibility and affordability are evaluated by TravelEncounterSystem;
        /// hosts only format this result for presentation.
        /// </summary>
        public PatrolEncounterPresentation? BuildPatrolPresentation(
            string encounterId,
            Inventory.Inventory? inventory = null,
            Func<string, bool>? flagEvaluator = null)
        {
            if (!_catalog.TryGetEncounter(encounterId, out var encounter) || !IsPatrol(encounter))
                return null;

            var recognition = GetRecognitionContext(encounter);
            var choices = new List<PatrolChoicePresentation>();
            foreach (var choice in encounter.Choices ?? new List<TravelEncounterChoice>())
            {
                var availability = EvaluateChoiceAvailability(encounter, choice, inventory ?? _inventory, flagEvaluator);
                choices.Add(new PatrolChoicePresentation
                {
                    ChoiceId = choice.ChoiceId,
                    Text = choice.Text,
                    IsAvailable = availability.IsAvailable,
                    DisabledReasonCode = GetAvailabilityReasonCode(availability),
                    RequiredItemId = choice.RequiredItemId,
                    RequiredItemQuantity = choice.RequiredItemQuantity,
                    Costs = choice.GetNormalizedCosts(),
                    MoraleDelta = choice.MoraleDelta,
                    GuiltDelta = choice.GuiltDelta,
                    FactionId = !string.IsNullOrWhiteSpace(choice.FactionId) ? choice.FactionId : encounter.FactionId,
                    FactionStandingDelta = choice.FactionStandingDelta,
                    Failures = new List<ChoiceRequirementFailure>(availability.Failures)
                });
            }

            string recognitionLabel = recognition.HasTag("hostile")
                ? "Hostile history"
                : recognition.CurrentChainStage >= 2
                    ? "Trusted regular"
                    : recognition.CurrentChainStage == 1
                        ? "Recognized regular"
                        : recognition.EncountersWithFaction > 0
                            ? "Prior contact"
                            : "No prior contact";

            return new PatrolEncounterPresentation
            {
                EncounterId = encounter.Id,
                FactionId = FactionStandingIdResolver.ToSystemsId(encounter.FactionId),
                DisplayFactionId = encounter.FactionId,
                TerritoryState = encounter.TerritoryState ?? string.Empty,
                PatrolArchetype = encounter.PatrolArchetype ?? string.Empty,
                CurrentChainStage = recognition.CurrentChainStage,
                RecognitionLabel = recognitionLabel,
                Choices = choices
            };
        }

        private static string GetAvailabilityReasonCode(ChoiceAvailabilityResult availability)
        {
            if (availability == null || availability.IsAvailable) return string.Empty;
            foreach (var failure in availability.Failures)
            {
                if (failure == null) continue;
                return failure.FailureType switch
                {
                    ChoiceRequirementFailureType.InsufficientCostItems => "cost_unavailable",
                    ChoiceRequirementFailureType.MissingRequiredItem => "required_item_missing",
                    ChoiceRequirementFailureType.MissingRequiredFlag => "required_condition_missing",
                    ChoiceRequirementFailureType.RecognitionCondition => "recognition_requirement_unmet",
                    ChoiceRequirementFailureType.StandingCondition => "standing_requirement_unmet",
                    _ => "unavailable"
                };
            }
            return "unavailable";
        }

        public bool TryBuildResolutionPlan(
            string encounterId,
            string choiceId,
            int currentDay,
            out TravelEncounterResolutionPlan? plan,
            Inventory.Inventory? inventory = null,
            Func<string, bool>? flagEvaluator = null)
        {
            plan = null;
            if (!_catalog.TryGetEncounter(encounterId, out var encounter))
            {
                return false;
            }

            TravelEncounterChoice? selectedChoice = null;
            foreach (var c in encounter.Choices)
            {
                if (string.Equals(c.ChoiceId, choiceId, StringComparison.OrdinalIgnoreCase))
                {
                    selectedChoice = c;
                    break;
                }
            }

            if (selectedChoice == null)
            {
                return false;
            }

            string cdKey = GetCooldownKey(encounter);
            int expiry = GetCooldownExpiry(cdKey);
            bool onCooldown = _encounterAvailableDay.TryGetValue(cdKey, out int nextDay) && currentDay < nextDay;

            var avail = EvaluateChoiceAvailability(encounter, selectedChoice, inventory ?? _inventory, flagEvaluator);

            string targetFaction = !string.IsNullOrWhiteSpace(selectedChoice.FactionId)
                ? selectedChoice.FactionId
                : (selectedChoice.FactionStandingDelta != 0 ? encounter.FactionId : string.Empty);
            string canonicalFaction = FactionStandingIdResolver.ToSystemsId(targetFaction);

            plan = new TravelEncounterResolutionPlan
            {
                EncounterId = encounterId,
                ChoiceId = choiceId,
                CurrentDay = currentDay,
                CooldownKey = cdKey,
                CooldownExpiryDay = expiry,
                IsOnCooldown = onCooldown,
                MoraleDelta = selectedChoice.MoraleDelta,
                GuiltDelta = selectedChoice.GuiltDelta,
                RawFactionId = targetFaction,
                CanonicalFactionId = canonicalFaction,
                FactionStandingDelta = selectedChoice.FactionStandingDelta,
                UnlocksFieldGuideId = selectedChoice.UnlocksFieldGuideId,
                AdvancesChainStage = selectedChoice.AdvancesChainStage,
                ChainId = encounter.ChainId,
                Costs = selectedChoice.GetNormalizedCosts(),
                RequiredItemId = selectedChoice.RequiredItemId,
                RequiredItemQuantity = selectedChoice.RequiredItemQuantity,
                RequiredFlag = selectedChoice.RequiredFlag,
                Availability = avail
            };

            return true;
        }

        public bool ResolveChoice(string encounterId, string choiceId, int currentDay, out TravelEncounterResolutionResult? result)
        {
            result = null;
            if (!_catalog.TryGetEncounter(encounterId, out var encounter))
            {
                return false;
            }

            TravelEncounterChoice? selectedChoice = null;
            foreach (var c in encounter.Choices)
            {
                if (string.Equals(c.ChoiceId, choiceId, StringComparison.OrdinalIgnoreCase))
                {
                    selectedChoice = c;
                    break;
                }
            }

            if (selectedChoice == null)
            {
                return false;
            }

            // Preflight every non-consuming requirement before opening the
            // inventory transaction. This keeps failed recognition/cost paths
            // fully atomic: no item, standing, history, chain, or cooldown is
            // changed when the choice cannot be committed.
            if (!TryBuildResolutionPlan(encounterId, choiceId, currentDay, out var plan) || plan == null || !plan.CanExecute)
            {
                return false;
            }

            string cdKey = plan.CooldownKey;

            // Deduct costs atomically via InventoryBill transaction
            var costs = plan.Costs;
            if (costs.Count > 0)
            {
                if (_inventory == null)
                {
                    return false;
                }

                var bill = new InventoryBill();
                foreach (var cost in costs)
                {
                    bill.AddCost(cost.ItemId, cost.Quantity);
                }

                using var tx = _inventory.BeginTransaction(bill);
                if (!tx.Validation.IsValid || !tx.TryCommit())
                {
                    return false;
                }
            }

            // Apply faction standing via canonical ID
            string targetFaction = !string.IsNullOrWhiteSpace(selectedChoice.FactionId)
                ? selectedChoice.FactionId
                : (selectedChoice.FactionStandingDelta != 0 ? encounter.FactionId : string.Empty);
            string canonicalFaction = FactionStandingIdResolver.ToSystemsId(targetFaction);

            if (_factionWar != null && !string.IsNullOrWhiteSpace(canonicalFaction) && selectedChoice.FactionStandingDelta != 0)
            {
                _factionWar.ModifyStanding(canonicalFaction, selectedChoice.FactionStandingDelta);
            }

            // Set cooldown for this group/encounter using the definition's
            // authored recurrence rather than a uniform runtime constant.
            _encounterAvailableDay[cdKey] = currentDay + encounter.GetCooldownDays();

            // Advance chain if applicable
            if (!string.IsNullOrEmpty(encounter.ChainId) && selectedChoice.AdvancesChainStage > 0)
            {
                if (IsPatrol(encounter))
                {
                    string chainKey = GetPatrolChainKey(encounter.FactionId, encounter.ChainId);
                    int existing = GetPatrolChainStage(encounter);
                    int next = Math.Max(existing, selectedChoice.AdvancesChainStage);
                    _patrolChainStages[chainKey] = next;
                    OnChainStageAdvanced?.Invoke(chainKey, next);
                }
                else
                {
                    SetChainStage(encounter.ChainId, selectedChoice.AdvancesChainStage);
                }
            }

            if (IsPatrol(encounter))
            {
                RecordPatrolHistory(encounter, selectedChoice, currentDay);
            }

            // Flagship VII (F10) — Severe patrol violations hand off to bounty authority
            FactionBountyRecord? bountyRecord = null;
            if (_bountySystem != null &&
                !string.IsNullOrWhiteSpace(canonicalFaction) &&
                selectedChoice.FactionStandingDelta <= FactionBountySystem.PatrolBountyStandingThreshold)
            {
                bountyRecord = _bountySystem.IssuePatrolBounty(
                    canonicalFaction,
                    encounterId,
                    choiceId,
                    selectedChoice.FactionStandingDelta,
                    currentDay);
            }

            result = new TravelEncounterResolutionResult
            {
                EncounterId = encounterId,
                ChoiceId = choiceId,
                Day = currentDay,
                MoraleDelta = selectedChoice.MoraleDelta,
                GuiltDelta = selectedChoice.GuiltDelta,
                FactionId = targetFaction,
                CanonicalFactionId = canonicalFaction,
                FactionStandingDelta = selectedChoice.FactionStandingDelta,
                UnlocksFieldGuideId = selectedChoice.UnlocksFieldGuideId,
                ChainStageAdvanced = selectedChoice.AdvancesChainStage,
                DeductedCosts = new List<NormalizedItemCost>(costs),
                CooldownKey = cdKey,
                BountyRecord = bountyRecord
            };

            OnChoiceResolved?.Invoke(encounterId, choiceId);
            return true;
        }

        private void RecordPatrolHistory(TravelEncounterDefinition encounter, TravelEncounterChoice choice, int day)
        {
            string faction = FactionStandingIdResolver.ToSystemsId(encounter.FactionId);
            string key = GetPatrolHistoryKey(faction, encounter.Id, choice.ChoiceId);
            if (!_patrolHistory.TryGetValue(key, out var record))
            {
                record = new PatrolHistoryRecord
                {
                    FactionId = faction,
                    EncounterId = encounter.Id,
                    ChoiceId = choice.ChoiceId,
                    ResolutionDay = day,
                    LastResolutionDay = day,
                    TimesSelected = 0,
                    Tags = new List<string>()
                };
                _patrolHistory[key] = record;
            }

            record.TimesSelected = Math.Max(0, record.TimesSelected) + 1;
            record.LastResolutionDay = day;
            if (record.ResolutionDay <= 0) record.ResolutionDay = day;

            var tags = new HashSet<string>(record.Tags ?? new List<string>(), StringComparer.OrdinalIgnoreCase);
            if (choice.HistoryTags != null)
            {
                foreach (var tag in choice.HistoryTags)
                {
                    if (!string.IsNullOrWhiteSpace(tag)) tags.Add(tag.Trim());
                }
            }
            if (choice.ChoiceId.IndexOf("toll", StringComparison.OrdinalIgnoreCase) >= 0)
                tags.Add("paid_toll");
            if (!choice.IsNonviolent || choice.FactionStandingDelta <= -10)
                tags.Add("hostile");
            if (choice.FactionStandingDelta > 0)
                tags.Add("cooperative");
            record.Tags = new List<string>(tags);
            OnPatrolHistoryRecorded?.Invoke(encounter.Id, choice.ChoiceId);
        }

        public bool ResolveChoice(string encounterId, string choiceId, int currentDay, out int moraleDelta, out int guiltDelta, out string unlockedFieldGuideId)
        {
            if (ResolveChoice(encounterId, choiceId, currentDay, out var res) && res != null)
            {
                moraleDelta = res.MoraleDelta;
                guiltDelta = res.GuiltDelta;
                unlockedFieldGuideId = res.UnlocksFieldGuideId;
                return true;
            }
            moraleDelta = 0;
            guiltDelta = 0;
            unlockedFieldGuideId = string.Empty;
            return false;
        }

        public TravelEncounterState CaptureState()
        {
            var history = new List<PatrolHistoryRecord>();
            foreach (var record in _patrolHistory.Values)
            {
                history.Add(new PatrolHistoryRecord
                {
                    FactionId = record.FactionId,
                    EncounterId = record.EncounterId,
                    ChoiceId = record.ChoiceId,
                    ResolutionDay = record.ResolutionDay,
                    TimesSelected = record.TimesSelected,
                    LastResolutionDay = record.LastResolutionDay,
                    Tags = new List<string>(record.Tags ?? new List<string>())
                });
            }
            history.Sort((a, b) => string.Compare(
                a.FactionId + "::" + a.EncounterId + "::" + a.ChoiceId,
                b.FactionId + "::" + b.EncounterId + "::" + b.ChoiceId,
                StringComparison.Ordinal));

            return new TravelEncounterState
            {
                ChainStages = new Dictionary<string, int>(_chainStages, StringComparer.OrdinalIgnoreCase),
                EncounterAvailableDay = new Dictionary<string, int>(_encounterAvailableDay, StringComparer.OrdinalIgnoreCase),
                PatrolChainStages = new Dictionary<string, int>(_patrolChainStages, StringComparer.OrdinalIgnoreCase),
                PatrolHistory = history
            };
        }

        public void RestoreState(TravelEncounterState? state)
        {
            _chainStages.Clear();
            _patrolChainStages.Clear();
            _encounterAvailableDay.Clear();
            _patrolHistory.Clear();
            if (state == null) return;

            if (state.ChainStages != null)
            {
                foreach (var kvp in state.ChainStages)
                {
                    _chainStages[kvp.Key] = kvp.Value;
                }
            }

            if (state.EncounterAvailableDay != null)
            {
                foreach (var kvp in state.EncounterAvailableDay)
                {
                    _encounterAvailableDay[kvp.Key] = kvp.Value;
                }

                // Backward-compatible migration:
                // If legacy saves recorded cooldowns under member encounter IDs instead of cooldown_group,
                // fold active member cooldowns into their group key using the maximum expiry value.
                if (_catalog != null)
                {
                    foreach (var enc in _catalog.Encounters)
                    {
                        if (string.IsNullOrWhiteSpace(enc.CooldownGroup)) continue;
                        string groupKey = enc.CooldownGroup.Trim();

                        if (_encounterAvailableDay.TryGetValue(enc.Id, out int memberExpiry))
                        {
                            int currentGroupExpiry = _encounterAvailableDay.TryGetValue(groupKey, out int gExp) ? gExp : 0;
                            _encounterAvailableDay[groupKey] = Math.Max(currentGroupExpiry, memberExpiry);
                            _encounterAvailableDay.Remove(enc.Id);
                        }
                    }
                }
            }

            if (state.PatrolChainStages != null)
            {
                foreach (var kvp in state.PatrolChainStages)
                {
                    _patrolChainStages[kvp.Key] = kvp.Value;
                }
            }

            if (state.PatrolHistory != null)
            {
                foreach (var source in state.PatrolHistory)
                {
                    if (source == null || string.IsNullOrWhiteSpace(source.ChoiceId)) continue;
                    string faction = FactionStandingIdResolver.ToSystemsId(source.FactionId);
                    var copy = new PatrolHistoryRecord
                    {
                        FactionId = faction,
                        EncounterId = source.EncounterId ?? string.Empty,
                        ChoiceId = source.ChoiceId,
                        ResolutionDay = source.ResolutionDay,
                        TimesSelected = Math.Max(1, source.TimesSelected),
                        LastResolutionDay = source.LastResolutionDay,
                        Tags = new List<string>(source.Tags ?? new List<string>())
                    };
                    _patrolHistory[GetPatrolHistoryKey(faction, copy.EncounterId, copy.ChoiceId)] = copy;
                }
            }
        }
    }
}
