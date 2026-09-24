// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : RadiationMutationHostSession
// Core Source  : Ashfall.Core.Medical.MutationSystem (Plan 172)
// Purpose      : Thin Godot adapter for radiation exposure, instability,
//                mutation trees, and gene therapy.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public sealed class MutationCensus
    {
        public int AuthoredMutationsCount { get; init; }
        public int TrackedProfilesCount { get; init; }
        public int TotalMutationsAcquired { get; init; }
        public int TotalGeneTherapiesReceived { get; init; }
    }

    public sealed class RadiationMutationHostSession : HostSessionBase
    {
        public MutationSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public MutationCensus Census => new MutationCensus
        {
            AuthoredMutationsCount = System.GetAllMutations().Count,
            TrackedProfilesCount = System.State.profiles.Count,
            TotalMutationsAcquired = System.State.totalMutationsAcquired,
            TotalGeneTherapiesReceived = System.State.totalGeneTherapies
        };

        public RadiationMutationHostSession(MutationSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));

            System.OnMutationAcquired += (survivorId, mutationId, capabilities) =>
            {
                LastEvent = $"Mutation manifested: {survivorId} developed {mutationId}.";
                RaiseStateChanged();
            };

            System.OnMutationRemoved += (survivorId, mutationId) =>
            {
                LastEvent = $"Gene therapy completed: {survivorId} excised {mutationId}.";
                RaiseStateChanged();
            };

            System.OnInstabilitySpike += (survivorId, instability) =>
            {
                LastEvent = $"Instability spike: {survivorId} reached {instability:F1}%.";
                RaiseStateChanged();
            };
        }

        public static RadiationMutationHostSession Create(
            string? dataDir = null,
            ISeededRng? rng = null,
            Ashfall.Core.Inventory.Inventory? inventory = null)
        {
            var seedRng = rng ?? new SeededRng(172);
            var inv = inventory ?? new Ashfall.Core.Inventory.Inventory();
            var system = new MutationSystem(seedRng, inv, new GodotLog());

            string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
            string catalogPath = Path.Combine(dataRoot, "mutations.json");

            if (File.Exists(catalogPath))
            {
                try
                {
                    system.LoadCatalog(File.ReadAllText(catalogPath));
                }
                catch
                {
                    // Degrade safely on parse error
                }
            }

            return new RadiationMutationHostSession(system);
        }

        public bool AddExposure(string survivorId, float dose, int day)
        {
            if (string.IsNullOrWhiteSpace(survivorId) || dose <= 0f) return false;
            System.AddRadiationExposure(survivorId, dose, day);
            LastEvent = $"Exposed {survivorId} to {dose:F1} mSv on day {day}.";
            RaiseStateChanged();
            return true;
        }

        public bool TryMutateSurvivor(string survivorId, int day)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return false;
            bool mutated = System.TryMutateSurvivor(survivorId, day);
            if (mutated)
            {
                RaiseStateChanged();
            }
            return mutated;
        }

        public void AdministerRadAway(string survivorId, float detoxAmount, int day)
        {
            if (string.IsNullOrWhiteSpace(survivorId) || detoxAmount <= 0f) return;
            System.AdministerRadAway(survivorId, detoxAmount, day);
            LastEvent = $"Administered RadAway detox to {survivorId} (-{detoxAmount:F1} mSv).";
            RaiseStateChanged();
        }

        public GeneTherapyResult PerformGeneTherapy(string survivorId, string mutationId, int day)
        {
            if (string.IsNullOrWhiteSpace(survivorId) || string.IsNullOrWhiteSpace(mutationId))
                return GeneTherapyResult.Fail("invalid_args");

            var result = System.PerformGeneTherapy(survivorId, mutationId, day);
            if (result.Success)
            {
                LastEvent = $"Gene therapy succeeded for {survivorId}: removed {mutationId}.";
                RaiseStateChanged();
            }
            else
            {
                LastEvent = $"Gene therapy failed for {survivorId}: {result.FailureCode}.";
            }
            return result;
        }

        public SurvivorMutationProfile? GetProfile(string survivorId) => System.GetProfile(survivorId);

        public Dictionary<string, float> GetStatModifiers(string survivorId) => System.GetStatModifiers(survivorId);

        public List<string> GetCapabilityTags(string survivorId) => System.GetCapabilityTags(survivorId);

        public List<string> GetVisibleTags(string survivorId) => System.GetVisibleTags(survivorId);

        public float CalculateSocialStigmaPenalty(string survivorId) => System.CalculateSocialStigmaPenalty(survivorId);

        public void LoadCatalog(string json) => System.LoadCatalog(json);

        public MutationState CaptureSave() => System.CaptureState();

        public void RestoreSave(MutationState? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "Mutation system state restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            MutationSaveStore.TrySave(CaptureSave());
        }
    }
}
