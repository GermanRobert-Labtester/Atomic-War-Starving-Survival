// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Narrative
{
    public static class PatrolEncounterValidator
    {
        private static readonly HashSet<string> AllowedTerritoryStates = new(StringComparer.OrdinalIgnoreCase)
        {
            "controlled", "contested", "border"
        };

        private static readonly HashSet<string> AllowedArchetypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "checkpoint", "caravan_escort", "raid_party", "press_gang",
            "refugee_eviction", "supply_run", "reconnaissance", "border_patrol"
        };

        public static List<string> Validate(
            IEnumerable<TravelEncounterDefinition> encounters,
            ISet<string>? validFactionIds = null,
            ISet<string>? validItemIds = null)
        {
            var errors = new List<string>();
            if (encounters == null) return errors;

            var encounterList = encounters.ToList();
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Group variants by cooldown_group
            var cooldownGroups = new Dictionary<string, List<TravelEncounterDefinition>>(StringComparer.OrdinalIgnoreCase);

            foreach (var enc in encounterList)
            {
                if (string.IsNullOrWhiteSpace(enc.Id))
                {
                    errors.Add("Encounter has null or empty id.");
                    continue;
                }

                if (!seenIds.Add(enc.Id))
                {
                    errors.Add($"Duplicate encounter id '{enc.Id}'.");
                }

                bool isPatrol = enc.Id.StartsWith("enc_patrol_", StringComparison.OrdinalIgnoreCase);
                if (!isPatrol)
                {
                    // Validator is specialized for patrol encounters (Plan 45 / F15)
                    continue;
                }

                // 1. Category must be "Human"
                if (!string.Equals(enc.Category, "Human", StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add($"Patrol '{enc.Id}' must have category 'Human', but was '{enc.Category}'.");
                }

                // 2. FactionId must be present and valid
                if (string.IsNullOrWhiteSpace(enc.FactionId))
                {
                    errors.Add($"Patrol '{enc.Id}' has missing or empty faction_id.");
                }
                else if (validFactionIds != null && !validFactionIds.Contains(enc.FactionId))
                {
                    errors.Add($"Patrol '{enc.Id}' references unknown faction '{enc.FactionId}'.");
                }

                // 3. territory_state must be controlled, contested, or border
                if (string.IsNullOrWhiteSpace(enc.TerritoryState) || !AllowedTerritoryStates.Contains(enc.TerritoryState))
                {
                    errors.Add($"Patrol '{enc.Id}' has invalid territory_state '{enc.TerritoryState}'. Allowed: controlled, contested, border.");
                }

                if (enc.CooldownDays < 0)
                {
                    errors.Add($"Patrol '{enc.Id}' has invalid cooldown_days {enc.CooldownDays}. Must be >= 0.");
                }

                if (!string.IsNullOrWhiteSpace(enc.PatrolArchetype) && !AllowedArchetypes.Contains(enc.PatrolArchetype))
                {
                    errors.Add($"Patrol '{enc.Id}' has invalid patrol_archetype '{enc.PatrolArchetype}'.");
                }

                if (enc.ChainStage < 0 || enc.PrereqChainStage < -1)
                {
                    errors.Add($"Patrol '{enc.Id}' has invalid chain stage values ({enc.ChainStage}, {enc.PrereqChainStage}).");
                }

                // 4. Choices count in [2, 6]. Six permits a compact staged
                // recognition chain without duplicating whole encounter rows.
                if (enc.Choices == null || enc.Choices.Count < 2 || enc.Choices.Count > 6)
                {
                    int count = enc.Choices?.Count ?? 0;
                    errors.Add($"Patrol '{enc.Id}' must have between 2 and 6 choices, but has {count}.");
                }
                else
                {
                    var seenChoiceIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    var seenChoiceTexts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    foreach (var c in enc.Choices)
                    {
                        if (string.IsNullOrWhiteSpace(c.ChoiceId))
                        {
                            errors.Add($"Patrol '{enc.Id}' has choice with empty choice_id.");
                        }
                        else if (!seenChoiceIds.Add(c.ChoiceId))
                        {
                            errors.Add($"Patrol '{enc.Id}' has duplicate choice_id '{c.ChoiceId}'.");
                        }

                        if (string.IsNullOrWhiteSpace(c.Text))
                        {
                            errors.Add($"Patrol '{enc.Id}' choice '{c.ChoiceId}' has empty text.");
                        }
                        else if (!seenChoiceTexts.Add(c.Text.Trim()))
                        {
                            errors.Add($"Patrol '{enc.Id}' has duplicate choice text '{c.Text}'.");
                        }

                        // Standing delta range [-25, 10]
                        if (c.FactionStandingDelta < -25 || c.FactionStandingDelta > 10)
                        {
                            errors.Add($"Patrol '{enc.Id}' choice '{c.ChoiceId}' standing delta {c.FactionStandingDelta} outside allowed range [-25, 10].");
                        }

                        // Choice-specific faction reference
                        if (!string.IsNullOrWhiteSpace(c.FactionId) && validFactionIds != null && !validFactionIds.Contains(c.FactionId))
                        {
                            errors.Add($"Patrol '{enc.Id}' choice '{c.ChoiceId}' references unknown faction '{c.FactionId}'.");
                        }

                        // Cost items validation
                        var costs = c.GetNormalizedCosts();
                        var costItemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        foreach (var cost in costs)
                        {
                            costItemIds.Add(cost.ItemId);
                            if (validItemIds != null && !validItemIds.Contains(cost.ItemId))
                            {
                                errors.Add($"Patrol '{enc.Id}' choice '{c.ChoiceId}' references unknown cost item '{cost.ItemId}'.");
                            }
                        }

                        // Required item gating
                        if (!string.IsNullOrWhiteSpace(c.RequiredItemId))
                        {
                            if (validItemIds != null && !validItemIds.Contains(c.RequiredItemId))
                            {
                                errors.Add($"Patrol '{enc.Id}' choice '{c.ChoiceId}' references unknown required item '{c.RequiredItemId}'.");
                            }

                            if (c.RequiredItemQuantity <= 0)
                            {
                                errors.Add($"Patrol '{enc.Id}' choice '{c.ChoiceId}' has invalid required_item_quantity {c.RequiredItemQuantity}. Must be > 0.");
                            }

                            if (costItemIds.Contains(c.RequiredItemId))
                            {
                                errors.Add($"Patrol '{enc.Id}' choice '{c.ChoiceId}' required item '{c.RequiredItemId}' cannot also be consumed in costs.");
                            }
                        }

                        if (c.RequiredChainStage < -1 || c.MaxChainStage < -1)
                        {
                            errors.Add($"Patrol '{enc.Id}' choice '{c.ChoiceId}' has invalid recognition stage bounds.");
                        }

                        if (c.RequiredChainStage >= 0 && c.MaxChainStage >= 0 && c.RequiredChainStage > c.MaxChainStage)
                        {
                            errors.Add($"Patrol '{enc.Id}' choice '{c.ChoiceId}' requires chain stage {c.RequiredChainStage} after its maximum {c.MaxChainStage}.");
                        }
                    }
                }

                // 5. Weight bounds [0.1, 5.0]
                if (enc.BaseWeight < 0.1f || enc.BaseWeight > 5.0f)
                {
                    errors.Add($"Patrol '{enc.Id}' base_weight {enc.BaseWeight} outside allowed range [0.1, 5.0].");
                }

                // 6. Danger bounds
                if (enc.MinDangerLevel < 0 || enc.MaxDangerLevel < enc.MinDangerLevel)
                {
                    errors.Add($"Patrol '{enc.Id}' has invalid danger levels (min: {enc.MinDangerLevel}, max: {enc.MaxDangerLevel}).");
                }

                // 7. Region and Season tags
                if (enc.RegionTags == null || enc.RegionTags.Count == 0)
                {
                    errors.Add($"Patrol '{enc.Id}' has empty region_tags.");
                }

                if (enc.SeasonTags == null || enc.SeasonTags.Count == 0)
                {
                    errors.Add($"Patrol '{enc.Id}' has empty season_tags.");
                }

                // 8. Cooldown group formatting
                if (enc.CooldownGroup != null && enc.CooldownGroup.Length > 0 && string.IsNullOrWhiteSpace(enc.CooldownGroup))
                {
                    errors.Add($"Patrol '{enc.Id}' has whitespace-only cooldown_group.");
                }

                // 9. Flagship VII: War-state and territory owner validation
                if (!TravelEncounterDefinition.IsValidWarState(enc.WarStateString))
                {
                    errors.Add($"Patrol '{enc.Id}' has invalid war_state '{enc.WarStateString}'. Allowed: any, peacetime, wartime.");
                }

                if (enc.WarWeightMultiplier <= 0f)
                {
                    errors.Add($"Patrol '{enc.Id}' war_weight_multiplier {enc.WarWeightMultiplier} must be positive.");
                }

                if (!string.IsNullOrWhiteSpace(enc.RequiredTerritoryOwner) && validFactionIds != null && !validFactionIds.Contains(enc.RequiredTerritoryOwner))
                {
                    errors.Add($"Patrol '{enc.Id}' references unknown required_territory_owner '{enc.RequiredTerritoryOwner}'.");
                }

                // Collect for variant family comparison
                if (!string.IsNullOrWhiteSpace(enc.CooldownGroup))
                {
                    string groupKey = enc.CooldownGroup.Trim();
                    if (!cooldownGroups.TryGetValue(groupKey, out var groupList))
                    {
                        groupList = new List<TravelEncounterDefinition>();
                        cooldownGroups[groupKey] = groupList;
                    }
                    groupList.Add(enc);
                }
            }

            // 9. Variant family consistency checks
            foreach (var kvp in cooldownGroups)
            {
                var groupList = kvp.Value;
                if (groupList.Count <= 1) continue;

                var lead = groupList[0];
                for (int i = 1; i < groupList.Count; i++)
                {
                    var other = groupList[i];

                    if (!string.Equals(lead.Category, other.Category, StringComparison.OrdinalIgnoreCase))
                    {
                        errors.Add($"Variant family '{kvp.Key}' category mismatch between '{lead.Id}' ({lead.Category}) and '{other.Id}' ({other.Category}).");
                    }

                    if (!string.Equals(lead.FactionId, other.FactionId, StringComparison.OrdinalIgnoreCase))
                    {
                        errors.Add($"Variant family '{kvp.Key}' faction_id mismatch between '{lead.Id}' ({lead.FactionId}) and '{other.Id}' ({other.FactionId}).");
                    }

                    if (!string.Equals(lead.TerritoryState, other.TerritoryState, StringComparison.OrdinalIgnoreCase))
                    {
                        errors.Add($"Variant family '{kvp.Key}' territory_state mismatch between '{lead.Id}' ({lead.TerritoryState}) and '{other.Id}' ({other.TerritoryState}).");
                    }

                    if (lead.ChainId != other.ChainId || lead.ChainStage != other.ChainStage)
                    {
                        errors.Add($"Variant family '{kvp.Key}' chain mismatch between '{lead.Id}' and '{other.Id}'.");
                    }

                    if (lead.CooldownDays != other.CooldownDays ||
                        !string.Equals(lead.PatrolArchetype, other.PatrolArchetype, StringComparison.OrdinalIgnoreCase))
                    {
                        errors.Add($"Variant family '{kvp.Key}' recurrence metadata mismatch between '{lead.Id}' and '{other.Id}'.");
                    }

                    // Choices must be mechanically identical
                    if (lead.Choices.Count != other.Choices.Count)
                    {
                        errors.Add($"Variant family '{kvp.Key}' choices count mismatch between '{lead.Id}' ({lead.Choices.Count}) and '{other.Id}' ({other.Choices.Count}).");
                        continue;
                    }

                    for (int cIdx = 0; cIdx < lead.Choices.Count; cIdx++)
                    {
                        var c1 = lead.Choices[cIdx];
                        var c2 = other.Choices[cIdx];

                        if (c1.ChoiceId != c2.ChoiceId ||
                            c1.IsNonviolent != c2.IsNonviolent ||
                            c1.IsAvoidance != c2.IsAvoidance ||
                            c1.MoraleDelta != c2.MoraleDelta ||
                            c1.GuiltDelta != c2.GuiltDelta ||
                            c1.FactionStandingDelta != c2.FactionStandingDelta ||
                            c1.RequiredItemId != c2.RequiredItemId ||
                            c1.RequiredItemQuantity != c2.RequiredItemQuantity ||
                            c1.RequiredChainStage != c2.RequiredChainStage ||
                            c1.MaxChainStage != c2.MaxChainStage ||
                            c1.MinFactionStanding != c2.MinFactionStanding ||
                            c1.MaxFactionStanding != c2.MaxFactionStanding)
                        {
                            errors.Add($"Variant family '{kvp.Key}' choice mechanics mismatch at index {cIdx} between '{lead.Id}' and '{other.Id}'.");
                        }

                        var costs1 = c1.GetNormalizedCosts().OrderBy(x => x.ItemId).ThenBy(x => x.Quantity).ToList();
                        var costs2 = c2.GetNormalizedCosts().OrderBy(x => x.ItemId).ThenBy(x => x.Quantity).ToList();
                        if (costs1.Count != costs2.Count)
                        {
                            errors.Add($"Variant family '{kvp.Key}' choice costs count mismatch at index {cIdx} between '{lead.Id}' and '{other.Id}'.");
                        }
                        else
                        {
                            for (int k = 0; k < costs1.Count; k++)
                            {
                                if (costs1[k].ItemId != costs2[k].ItemId || costs1[k].Quantity != costs2[k].Quantity)
                                {
                                    errors.Add($"Variant family '{kvp.Key}' choice cost mismatch at index {cIdx} between '{lead.Id}' and '{other.Id}'.");
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            errors.Sort(StringComparer.Ordinal);
            return errors;
        }

        public static List<string> ValidateJson(
            string jsonText,
            ISet<string>? validFactionIds = null,
            ISet<string>? validItemIds = null)
        {
            var catalog = TravelEncounterCatalog.LoadFromJson(jsonText);
            return Validate(catalog.Encounters, validFactionIds, validItemIds);
        }
    }
}
