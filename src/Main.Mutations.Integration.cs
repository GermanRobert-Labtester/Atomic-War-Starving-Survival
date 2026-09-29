// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Combat;
using Ashfall.Core.Factions;
using Ashfall.Core.Medical;
using Ashfall.Core.Survivors;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private MutationSystem? _mutations;

        // ── Plan 180: Radioactive Mutation Trees ──────────────────────────

        public MutationSystem EnsureMutations()
        {
            if (_mutations != null) return _mutations;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("mutations") : new SeededRng(180);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _mutations = new MutationSystem(rng, inv, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("mutations.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<MutationCatalog>(json);
                        if (catalog?.mutations != null)
                        {
                            foreach (var m in catalog.mutations)
                                _mutations.RegisterMutation(m);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Mutations] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = MutationSaveStore.TryLoad();
            if (saved != null)
            {
                _mutations.RestoreState(saved);
            }

            _mutations.OnMutationAcquired += (survivorId, mutationId, capabilities) =>
            {
                string caps = string.Join(", ", capabilities);
                _journal?.TryAddRawEntry("mutation_manifested", $"Biological mutation manifest: {survivorId} developed {mutationId} (Capabilities: {caps}).", null!, _simDay);
            };

            return _mutations;
        }

        private RadiationMutationHostSession? _mutationSession;

        public RadiationMutationHostSession EnsureMutationSession()
        {
            if (_mutationSession != null) return _mutationSession;
            _mutationSession = new RadiationMutationHostSession(EnsureMutations());
            return _mutationSession;
        }

        public bool ApplyRadiationExposure(string survivorId, float dose, int day)
        {
            if (string.IsNullOrWhiteSpace(survivorId) || dose <= 0f) return false;
            EnsureMutations().AddRadiationExposure(survivorId, dose, day);
            return true;
        }

        public void TickMutations(int day, List<DayStateChangeEvent>? events = null)
        {
            SetupMutations();
            if (_mutations == null) return;

            if (_survivors != null)
            {
                var survivors = _survivors.RosterState
                    .Where(s => s.IsAlive)
                    .OrderBy(s => s.Id, StringComparer.Ordinal);

                foreach (var s in survivors)
                {
                    if (_mutations.TryMutateSurvivor(s.Id, day))
                    {
                        events?.Add(new DayStateChangeEvent("mutation_developed", "mutation_tree", s.Id, null, day));
                    }
                }
            }
        }

        public GeneTherapyResult PerformGeneTherapy(string survivorId, string mutationId)
        {
            SetupMutations();
            return EnsureMutations().PerformGeneTherapy(survivorId, mutationId, _simDay);
        }

        public void AdministerRadAway(string survivorId, float detoxAmount)
        {
            SetupMutations();
            EnsureMutations().AdministerRadAway(survivorId, detoxAmount, _simDay);
        }

        public SurvivorMutationProfile? GetSurvivorMutationProfile(string survivorId)
        {
            SetupMutations();
            return EnsureMutations().GetProfile(survivorId);
        }

        public List<string> GetSurvivorCapabilities(string survivorId)
        {
            SetupMutations();
            return EnsureMutations().GetCapabilityTags(survivorId);
        }

        private void SetupMutations()
        {
            EnsureMutations();
        }

        private void SaveMutations()
        {
            if (_mutations != null)
            {
                CaptureSection("mutation_tree", MutationSaveStore.TryCapturePersisted(_mutations.CaptureState()));
            }
        }

    }
}
