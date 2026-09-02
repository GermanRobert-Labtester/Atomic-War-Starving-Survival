// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 180: Radioactive Mutation Trees
// Pure deterministic simulation: cumulative radiation and genetic instability,
// branching tree with parent and exclusivity requirements, RadAway detox toxicity,
// clinical gene therapy, and generic capability tag projection.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Random;

namespace Ashfall.Core.Medical
{
    [Serializable]
    public sealed class MutationNode
    {
        public string mutation_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string branch_id { get; set; } = "sensory";
        public int tier { get; set; } = 1;
        public List<string> parent_mutation_ids { get; set; } = new List<string>();
        public List<string> exclusive_mutation_ids { get; set; } = new List<string>();
        public Dictionary<string, float> stat_modifiers { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
        public List<string> capability_tags { get; set; } = new List<string>();
        public List<string> visual_tags { get; set; } = new List<string>();
        public float instability_cost { get; set; } = 20.0f;
        public float required_exposure { get; set; } = 50.0f;
    }

    [Serializable]
    public sealed class MutationCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<MutationNode> mutations { get; set; } = new List<MutationNode>();
    }

    [Serializable]
    public sealed class SurvivorMutationProfile
    {
        public string survivorId { get; set; } = string.Empty;
        public float geneticInstability { get; set; } = 0.0f;
        public float lifetimePeakInstability { get; set; } = 0.0f;
        public float cumulativeRadDose { get; set; } = 0.0f;
        public List<string> activeMutationIds { get; set; } = new List<string>();
        public int lastMutationDay { get; set; } = 0;
        public int radAwayDosesAdministered { get; set; } = 0;
        public int geneTherapiesReceived { get; set; } = 0;
    }

    [Serializable]
    public sealed class MutationState
    {
        public int schema_version { get; set; } = 1;
        public List<SurvivorMutationProfile> profiles { get; set; } = new List<SurvivorMutationProfile>();
        public int totalMutationsAcquired { get; set; } = 0;
        public int totalGeneTherapies { get; set; } = 0;
    }

    public sealed class GeneTherapyResult
    {
        public bool Success { get; set; }
        public string FailureCode { get; set; } = string.Empty;
        public string RemovedMutationId { get; set; } = string.Empty;

        public static GeneTherapyResult Fail(string code) =>
            new GeneTherapyResult { Success = false, FailureCode = code };
    }

    public sealed class MutationSystem
    {
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly ILog _log;

        private readonly Dictionary<string, MutationNode> _mutations =
            new Dictionary<string, MutationNode>(StringComparer.Ordinal);

        private MutationState _state = new MutationState();

        public event Action<string, string, List<string>>? OnMutationAcquired;
        public event Action<string, string>? OnMutationRemoved;
        public event Action<string, float>? OnInstabilitySpike;

        public MutationState State => _state;

        public MutationSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _log = log ?? new NullLog();
        }

        public void RegisterMutation(MutationNode node)
        {
            if (node == null || string.IsNullOrWhiteSpace(node.mutation_id)) return;
            _mutations[node.mutation_id] = node;
        }

        public SurvivorMutationProfile EnsureProfile(string survivorId)
        {
            var existing = _state.profiles.FirstOrDefault(p => p.survivorId == survivorId);
            if (existing != null) return existing;

            var prof = new SurvivorMutationProfile
            {
                survivorId = survivorId,
                geneticInstability = 0.0f,
                lifetimePeakInstability = 0.0f,
                cumulativeRadDose = 0.0f
            };
            _state.profiles.Add(prof);
            return prof;
        }

        public SurvivorMutationProfile? GetProfile(string survivorId)
        {
            return _state.profiles.FirstOrDefault(p => p.survivorId == survivorId);
        }

        public void AddRadiationExposure(string survivorId, float dose, int currentDay)
        {
            var prof = EnsureProfile(survivorId);
            prof.cumulativeRadDose += Math.Max(0.0f, dose);

            // Dose above 50 mSv triggers genetic instability spike
            if (dose > 20.0f)
            {
                float spike = (dose - 20.0f) * 0.25f;
                prof.geneticInstability = Math.Min(100.0f, prof.geneticInstability + spike);
                prof.lifetimePeakInstability = Math.Max(prof.lifetimePeakInstability, prof.geneticInstability);
                OnInstabilitySpike?.Invoke(survivorId, prof.geneticInstability);
            }
        }

        public float CalculateMutationChance(string survivorId)
        {
            var prof = GetProfile(survivorId);
            if (prof == null) return 0.0f;

            float baseChance = (prof.cumulativeRadDose * 0.0015f) + (prof.geneticInstability * 0.004f);
            return Math.Clamp(baseChance, 0.0f, 0.90f);
        }

        public bool TryMutateSurvivor(string survivorId, int currentDay)
        {
            var prof = EnsureProfile(survivorId);
            if (currentDay == prof.lastMutationDay) return false; // Cooldown: max 1 mutation per day

            float chance = CalculateMutationChance(survivorId);
            float roll = (float)_rng.NextDouble();
            if (roll > chance) return false;

            // Filter eligible mutations:
            // 1. Not already owned
            // 2. Parents satisfied
            // 3. Exclusivity satisfied
            // 4. Required cumulative exposure satisfied
            var eligible = _mutations.Values
                .Where(m => !prof.activeMutationIds.Contains(m.mutation_id))
                .Where(m => m.required_exposure <= prof.cumulativeRadDose)
                .Where(m => m.parent_mutation_ids.Count == 0 || m.parent_mutation_ids.All(p => prof.activeMutationIds.Contains(p)))
                .Where(m => !m.exclusive_mutation_ids.Any(ex => prof.activeMutationIds.Contains(ex)))
                .OrderBy(m => m.mutation_id, StringComparer.Ordinal)
                .ToList();

            if (eligible.Count == 0) return false;

            // Deterministic pick from eligible
            int index = _rng.Next(0, eligible.Count);
            var chosen = eligible[index];

            prof.activeMutationIds.Add(chosen.mutation_id);
            prof.geneticInstability = Math.Min(100.0f, prof.geneticInstability + chosen.instability_cost);
            prof.lifetimePeakInstability = Math.Max(prof.lifetimePeakInstability, prof.geneticInstability);
            prof.lastMutationDay = currentDay;
            _state.totalMutationsAcquired++;

            OnMutationAcquired?.Invoke(survivorId, chosen.mutation_id, chosen.capability_tags);
            return true;
        }

        public void AdministerRadAway(string survivorId, float detoxAmount, int currentDay)
        {
            var prof = EnsureProfile(survivorId);
            prof.cumulativeRadDose = Math.Max(0.0f, prof.cumulativeRadDose - detoxAmount);
            prof.radAwayDosesAdministered++;

            // Heavy RadAway detox induces chemical stress and elevates instability
            if (prof.radAwayDosesAdministered >= 3)
            {
                prof.geneticInstability = Math.Min(100.0f, prof.geneticInstability + 8.0f);
                prof.lifetimePeakInstability = Math.Max(prof.lifetimePeakInstability, prof.geneticInstability);
                OnInstabilitySpike?.Invoke(survivorId, prof.geneticInstability);
            }
        }

        public GeneTherapyResult PerformGeneTherapy(string survivorId, string targetMutationId, int currentDay)
        {
            var prof = GetProfile(survivorId);
            if (prof == null || !prof.activeMutationIds.Contains(targetMutationId))
                return GeneTherapyResult.Fail("mutation_not_active");

            // Check consumables
            if (_inventory.CountById("gene_therapy_retroviral_vial") < 1)
                return GeneTherapyResult.Fail("missing_retroviral_vial");

            _inventory.RemoveById("gene_therapy_retroviral_vial", 1);

            prof.activeMutationIds.Remove(targetMutationId);
            prof.geneticInstability = Math.Max(0.0f, prof.geneticInstability - 15.0f);
            prof.geneTherapiesReceived++;
            _state.totalGeneTherapies++;

            OnMutationRemoved?.Invoke(survivorId, targetMutationId);
            return new GeneTherapyResult
            {
                Success = true,
                RemovedMutationId = targetMutationId
            };
        }

        public List<string> GetCapabilityTags(string survivorId)
        {
            var prof = GetProfile(survivorId);
            if (prof == null) return new List<string>();

            var tags = new HashSet<string>(StringComparer.Ordinal);
            foreach (var mutId in prof.activeMutationIds)
            {
                if (_mutations.TryGetValue(mutId, out var node))
                {
                    foreach (var tag in node.capability_tags)
                        tags.Add(tag);
                }
            }
            return tags.OrderBy(t => t, StringComparer.Ordinal).ToList();
        }

        public MutationState CaptureState() => _state;

        public void RestoreState(MutationState state)
        {
            if (state == null) return;
            _state = state;
        }
    }
}
