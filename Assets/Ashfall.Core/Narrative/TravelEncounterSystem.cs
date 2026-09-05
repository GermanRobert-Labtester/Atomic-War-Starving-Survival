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
        MissingRequiredFlag = 3
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
    }

    public sealed class TravelEncounterSystem
    {
        private readonly TravelEncounterCatalog _catalog;
        private readonly Dictionary<string, int> _chainStages = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> _encounterAvailableDay = new(StringComparer.OrdinalIgnoreCase);
        private Inventory.Inventory? _inventory;
        private FactionWarSystem? _factionWar;

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

        public event Action<string, string>? OnChoiceResolved;
        public event Action<string, int>? OnChainStageAdvanced;

        public TravelEncounterSystem(TravelEncounterCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public TravelEncounterSystem(TravelEncounterCatalog catalog, Inventory.Inventory? inventory, FactionWarSystem? factionWar = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _inventory = inventory;
            _factionWar = factionWar;
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

        public bool IsEncounterEligible(TravelEncounterDefinition encounter, string region, float dangerLevel, string currentSeason, int currentDay)
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
                int currentStage = GetChainStage(encounter.ChainId);
                if (encounter.PrereqChainStage != currentStage)
                {
                    return false;
                }
            }

            return true;
        }

        public float GetEffectiveWeight(TravelEncounterDefinition encounter, string stance)
        {
            if (encounter == null) return 0f;
            float weight = encounter.BaseWeight;

            if (!string.IsNullOrEmpty(stance) && encounter.StanceWeights != null && encounter.StanceWeights.TryGetValue(stance, out float multiplier))
            {
                weight *= multiplier;
            }

            return Math.Max(0.01f, weight);
        }

        public TravelEncounterDefinition? SelectEncounter(string region, float dangerLevel, string stance, string currentSeason, int currentDay, ISeededRng rng)
        {
            var eligible = new List<TravelEncounterDefinition>();
            var weights = new List<float>();
            float totalWeight = 0f;

            foreach (var encounter in _catalog.Encounters)
            {
                if (IsEncounterEligible(encounter, region, dangerLevel, currentSeason, currentDay))
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

        public List<(TravelEncounterDefinition encounter, float weight)> GetEligiblePatrolCandidates(
            string region, float dangerLevel, string stance, string currentSeason, int currentDay)
        {
            var list = new List<(TravelEncounterDefinition, float)>();
            foreach (var enc in _catalog.Encounters)
            {
                if (!enc.Id.StartsWith("enc_patrol_", StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(enc.FactionId))
                    continue;

                if (IsEncounterEligible(enc, region, dangerLevel, currentSeason, currentDay))
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

            var avail = EvaluateChoiceAvailability(selectedChoice, inventory ?? _inventory, flagEvaluator);

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

            // Check if encounter group or individual is on cooldown
            string cdKey = GetCooldownKey(encounter);
            if (_encounterAvailableDay.TryGetValue(cdKey, out int nextDay) && currentDay < nextDay)
            {
                return false;
            }

            // Evaluate non-consuming required item gate
            if (!string.IsNullOrWhiteSpace(selectedChoice.RequiredItemId) && selectedChoice.RequiredItemQuantity > 0)
            {
                int avail = _inventory?.CountById(selectedChoice.RequiredItemId) ?? 0;
                if (avail < selectedChoice.RequiredItemQuantity)
                {
                    return false;
                }
            }

            // Deduct costs atomically via InventoryBill transaction
            var costs = selectedChoice.GetNormalizedCosts();
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

            // Set cooldown for this group/encounter
            _encounterAvailableDay[cdKey] = currentDay + 5;

            // Advance chain if applicable
            if (!string.IsNullOrEmpty(encounter.ChainId) && selectedChoice.AdvancesChainStage > 0)
            {
                SetChainStage(encounter.ChainId, selectedChoice.AdvancesChainStage);
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
                DeductedCosts = costs,
                CooldownKey = cdKey
            };

            OnChoiceResolved?.Invoke(encounterId, choiceId);
            return true;
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
            return new TravelEncounterState
            {
                ChainStages = new Dictionary<string, int>(_chainStages, StringComparer.OrdinalIgnoreCase),
                EncounterAvailableDay = new Dictionary<string, int>(_encounterAvailableDay, StringComparer.OrdinalIgnoreCase)
            };
        }

        public void RestoreState(TravelEncounterState? state)
        {
            _chainStages.Clear();
            _encounterAvailableDay.Clear();
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
        }
    }
}
