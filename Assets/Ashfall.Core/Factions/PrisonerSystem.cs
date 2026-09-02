// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 179: Prisoner Management & Interrogation
// Pure deterministic simulation: captive detention, upkeep consumption,
// interrogation tactics with false-intel risks, escape pressure, and multi-factor
// recruitment.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Random;

namespace Ashfall.Core.Factions
{
    public enum CaptiveStatus
    {
        Detained = 0,
        Injured = 1,
        Cooperative = 2,
        Released = 3,
        Recruited = 4,
        Escaped = 5,
        Deceased = 6
    }

    [Serializable]
    public sealed class ItemRequirement
    {
        public string item_id { get; set; } = string.Empty;
        public int amount { get; set; } = 1;
    }

    [Serializable]
    public sealed class InterrogationTacticDef
    {
        public string tactic_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public List<string> required_facility_tags { get; set; } = new List<string>();
        public List<ItemRequirement> required_item_costs { get; set; } = new List<ItemRequirement>();
        public int duration_hours { get; set; } = 1;
        public float base_compliance_delta { get; set; } = 10.0f;
        public float trust_delta { get; set; } = 0.0f;
        public float fear_delta { get; set; } = 0.0f;
        public float resentment_delta { get; set; } = 0.0f;
        public float health_damage { get; set; } = 0.0f;
        public float morale_penalty { get; set; } = 0.0f;
        public float intel_chance { get; set; } = 0.5f;
        public float false_intel_chance { get; set; } = 0.1f;
        public int cooldown_days { get; set; } = 1;
        public string severity { get; set; } = "Standard";
    }

    [Serializable]
    public sealed class InterrogationTacticsCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<InterrogationTacticDef> tactics { get; set; } = new List<InterrogationTacticDef>();
    }

    [Serializable]
    public sealed class CaptiveState
    {
        public string captiveId { get; set; } = string.Empty;
        public string sourceFactionId { get; set; } = string.Empty;
        public int captureDay { get; set; } = 1;
        public float health { get; set; } = 100.0f;
        public float hydration { get; set; } = 100.0f;
        public float nutrition { get; set; } = 100.0f;
        public float compliance { get; set; } = 10.0f;
        public float fear { get; set; } = 20.0f;
        public float trust { get; set; } = 10.0f;
        public float resentment { get; set; } = 20.0f;
        public List<string> extractedIntelIds { get; set; } = new List<string>();
        public int cooldownUntilDay { get; set; } = 0;
        public float escapeProgress { get; set; } = 0.0f;
        public CaptiveStatus status { get; set; } = CaptiveStatus.Detained;
        public string? assignedGuardId { get; set; } = null;
        public int abuseWitnessCount { get; set; } = 0;
    }

    [Serializable]
    public sealed class ExtractedIntel
    {
        public string intelId { get; set; } = string.Empty;
        public string sourceCaptiveId { get; set; } = string.Empty;
        public string intelType { get; set; } = "stash_coordinates";
        public string targetLocationOrFactionId { get; set; } = string.Empty;
        public int dayExtracted { get; set; } = 1;
        public bool isTrueIntel { get; set; } = true;
    }

    [Serializable]
    public sealed class PrisonerState
    {
        public int schema_version { get; set; } = 1;
        public List<CaptiveState> captives { get; set; } = new List<CaptiveState>();
        public List<ExtractedIntel> extractedIntelRecords { get; set; } = new List<ExtractedIntel>();
        public int maxCellCapacity { get; set; } = 4;
        public int totalRecruits { get; set; } = 0;
        public int totalEscapes { get; set; } = 0;
        public int totalReleases { get; set; } = 0;
        public float cumulativeMoraleShock { get; set; } = 0.0f;
    }

    public sealed class InterrogationResult
    {
        public bool Success { get; set; }
        public string FailureCode { get; set; } = string.Empty;
        public float ComplianceDelta { get; set; }
        public float HealthDamage { get; set; }
        public bool IntelDiscovered { get; set; }
        public bool IsFalseIntel { get; set; }
        public string ExtractedIntelId { get; set; } = string.Empty;
        public float MoraleShock { get; set; }

        public static InterrogationResult Fail(string code) =>
            new InterrogationResult { Success = false, FailureCode = code };
    }

    public sealed class PrisonerSystem
    {
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly ILog _log;

        private readonly Dictionary<string, InterrogationTacticDef> _tactics =
            new Dictionary<string, InterrogationTacticDef>(StringComparer.Ordinal);

        private PrisonerState _state = new PrisonerState();

        public event Action<string, string, bool>? OnIntelExtracted;
        public event Action<string>? OnPrisonerEscaped;
        public event Action<string>? OnPrisonerRecruited;
        public event Action<string>? OnPrisonerReleased;
        public event Action<float>? OnSettlementMoraleShock;

        public PrisonerState State => _state;

        public PrisonerSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _log = log ?? new NullLog();
        }

        public void RegisterTactic(InterrogationTacticDef tactic)
        {
            if (tactic == null || string.IsNullOrWhiteSpace(tactic.tactic_id)) return;
            _tactics[tactic.tactic_id] = tactic;
        }

        public bool TakePrisoner(string captiveId, string sourceFactionId, int currentDay)
        {
            int activeCaptives = _state.captives.Count(c => c.status == CaptiveStatus.Detained || c.status == CaptiveStatus.Injured);
            if (activeCaptives >= _state.maxCellCapacity)
            {
                return false;
            }

            var existing = _state.captives.FirstOrDefault(c => c.captiveId == captiveId);
            if (existing != null) return false;

            var captive = new CaptiveState
            {
                captiveId = captiveId,
                sourceFactionId = sourceFactionId,
                captureDay = currentDay,
                status = CaptiveStatus.Detained,
                health = 100.0f,
                hydration = 100.0f,
                nutrition = 100.0f,
                compliance = 15.0f,
                trust = 10.0f,
                fear = 25.0f,
                resentment = 30.0f
            };
            _state.captives.Add(captive);
            return true;
        }

        public CaptiveState? GetCaptive(string captiveId)
        {
            return _state.captives.FirstOrDefault(c => c.captiveId == captiveId);
        }

        public InterrogationResult Interrogate(string captiveId, string tacticId, int currentDay)
        {
            var captive = GetCaptive(captiveId);
            if (captive == null || captive.status != CaptiveStatus.Detained)
                return InterrogationResult.Fail("captive_not_eligible");

            if (currentDay < captive.cooldownUntilDay)
                return InterrogationResult.Fail("interrogation_cooldown");

            if (!_tactics.TryGetValue(tacticId, out var tactic))
                return InterrogationResult.Fail("invalid_tactic");

            // Verify item requirements
            foreach (var cost in tactic.required_item_costs)
            {
                if (_inventory.CountById(cost.item_id) < cost.amount)
                    return InterrogationResult.Fail($"missing_item_{cost.item_id}");
            }

            // Consume costs
            foreach (var cost in tactic.required_item_costs)
            {
                _inventory.RemoveById(cost.item_id, cost.amount);
            }

            // Apply compliance, trust, fear, resentment
            captive.compliance = Math.Clamp(captive.compliance + tactic.base_compliance_delta, 0.0f, 100.0f);
            captive.trust = Math.Clamp(captive.trust + tactic.trust_delta, 0.0f, 100.0f);
            captive.fear = Math.Clamp(captive.fear + tactic.fear_delta, 0.0f, 100.0f);
            captive.resentment = Math.Clamp(captive.resentment + tactic.resentment_delta, 0.0f, 100.0f);
            captive.health = Math.Clamp(captive.health - tactic.health_damage, 0.0f, 100.0f);

            if (tactic.severity == "Severe" || tactic.severity == "Brutal")
            {
                captive.abuseWitnessCount++;
            }

            captive.cooldownUntilDay = currentDay + tactic.cooldown_days;

            if (captive.health <= 0.0f)
            {
                captive.status = CaptiveStatus.Deceased;
            }

            // Morale shock
            if (tactic.morale_penalty < 0.0f)
            {
                _state.cumulativeMoraleShock += Math.Abs(tactic.morale_penalty);
                OnSettlementMoraleShock?.Invoke(tactic.morale_penalty);
            }

            // Intel extraction roll
            bool gotIntel = false;
            bool isFalse = false;
            string intelId = string.Empty;

            float intelRoll = (float)_rng.NextDouble();
            if (intelRoll <= tactic.intel_chance)
            {
                gotIntel = true;
                float falseRoll = (float)_rng.NextDouble();
                isFalse = falseRoll <= tactic.false_intel_chance;

                intelId = $"intel_{captiveId}_{currentDay}_{_state.extractedIntelRecords.Count + 1}";
                var record = new ExtractedIntel
                {
                    intelId = intelId,
                    sourceCaptiveId = captiveId,
                    intelType = isFalse ? "false_lead" : "outpost_cache",
                    targetLocationOrFactionId = captive.sourceFactionId,
                    dayExtracted = currentDay,
                    isTrueIntel = !isFalse
                };
                _state.extractedIntelRecords.Add(record);
                captive.extractedIntelIds.Add(intelId);
                OnIntelExtracted?.Invoke(captiveId, intelId, !isFalse);
            }

            return new InterrogationResult
            {
                Success = true,
                ComplianceDelta = tactic.base_compliance_delta,
                HealthDamage = tactic.health_damage,
                IntelDiscovered = gotIntel,
                IsFalseIntel = isFalse,
                ExtractedIntelId = intelId,
                MoraleShock = tactic.morale_penalty
            };
        }

        public void TickUpkeepAndEscape(int currentDay)
        {
            foreach (var captive in _state.captives)
            {
                if (captive.status != CaptiveStatus.Detained) continue;

                // 1. Food and Water upkeep
                bool fed = _inventory.RemoveById("ration_hardtack", 1) || _inventory.RemoveById("canned_food", 1);
                bool watered = _inventory.RemoveById("clean_water", 1) || _inventory.RemoveById("dirty_water", 1);

                if (!fed)
                {
                    captive.nutrition = Math.Max(0.0f, captive.nutrition - 25.0f);
                    if (captive.nutrition <= 0.0f) captive.health = Math.Max(0.0f, captive.health - 15.0f);
                }
                else
                {
                    captive.nutrition = Math.Min(100.0f, captive.nutrition + 15.0f);
                }

                if (!watered)
                {
                    captive.hydration = Math.Max(0.0f, captive.hydration - 35.0f);
                    if (captive.hydration <= 0.0f) captive.health = Math.Max(0.0f, captive.health - 25.0f);
                }
                else
                {
                    captive.hydration = Math.Min(100.0f, captive.hydration + 20.0f);
                }

                if (captive.health <= 0.0f)
                {
                    captive.status = CaptiveStatus.Deceased;
                    continue;
                }

                // 2. Escape Pressure
                float escapeDelta = 10.0f;
                if (string.IsNullOrEmpty(captive.assignedGuardId))
                {
                    escapeDelta += 15.0f; // Unguarded cells accumulate rapid escape pressure
                }
                else
                {
                    escapeDelta -= 12.0f; // Guard actively watches cell
                }

                // Resentment and fear drive escape attempts
                if (captive.resentment > 50.0f) escapeDelta += 5.0f;
                if (captive.fear > 70.0f) escapeDelta += 5.0f;

                captive.escapeProgress = Math.Clamp(captive.escapeProgress + escapeDelta, 0.0f, 100.0f);

                if (captive.escapeProgress >= 100.0f)
                {
                    captive.status = CaptiveStatus.Escaped;
                    _state.totalEscapes++;
                    OnPrisonerEscaped?.Invoke(captive.captiveId);
                }
            }
        }

        public bool RecruitPrisoner(string captiveId, int currentDay)
        {
            var captive = GetCaptive(captiveId);
            if (captive == null || captive.status != CaptiveStatus.Detained) return false;

            int daysDetained = currentDay - captive.captureDay;
            // Multi-factor recruitment: requires minimum stay, sufficient trust, low resentment, no active severe abuse
            if (daysDetained < 2) return false;
            if (captive.trust < 40.0f) return false;
            if (captive.resentment > 30.0f) return false;
            if (captive.compliance < 50.0f) return false;
            if (captive.abuseWitnessCount > 1) return false;

            captive.status = CaptiveStatus.Recruited;
            _state.totalRecruits++;
            OnPrisonerRecruited?.Invoke(captiveId);
            return true;
        }

        public bool ReleasePrisoner(string captiveId)
        {
            var captive = GetCaptive(captiveId);
            if (captive == null || captive.status != CaptiveStatus.Detained) return false;

            captive.status = CaptiveStatus.Released;
            _state.totalReleases++;
            OnPrisonerReleased?.Invoke(captiveId);
            return true;
        }

        public PrisonerState CaptureState() => _state;

        public void RestoreState(PrisonerState state)
        {
            if (state == null) return;
            _state = state;
        }
    }
}
