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
        private GenerationalSystem? _generational;

        // ── Plan 178: Childhood & Generational Rearing ────────────────────

        public GenerationalSystem EnsureGenerational()
        {
            if (_generational != null) return _generational;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("generational") : new SeededRng(178);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
            var needs = _survivors?.Needs;

            _generational = new GenerationalSystem(rng, inv, needs, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("development_traits.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<DevelopmentTraitsCatalog>(json);
                        if (catalog?.traits != null)
                        {
                            foreach (var t in catalog.traits)
                                _generational.RegisterTrait(t);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Generational] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = GenerationalSaveStore.TryLoad();
            if (saved != null)
            {
                _generational.RestoreState(saved);
            }

            _generational.OnAdulthoodReached += (childId, phase, traits) =>
            {
                string traitList = string.Join(", ", traits);
                _journal?.TryAddRawEntry("adulthood_reached", $"Milestone: {childId} has transitioned to adulthood! Acquired traits: {traitList}", null!, _simDay);
            };

            _generational.OnCanonicalStageAdvanced += (childId, stage) =>
            {
                // Plan 183 is a projection over the canonical Generational
                // record. Journal presentation is the observable host route;
                // no stage copy or second save section is created here.
                _journal?.TryAddRawEntry(
                    "child_development_stage",
                    $"{childId} reached developmental stage {stage}.",
                    null!,
                    _simDay);
            };

            return _generational;
        }

        /// <summary>
        /// Returns the detached Plan 183 read model for a canonical child.
        /// GenerationalSystem remains the sole owner of birth, care, growth,
        /// and adult-transition state.
        /// </summary>
        public ChildProfile? GetCanonicalChildDevelopment(string childId, int currentDay = -1)
        {
            SetupGenerational();
            return EnsureGenerational().GetCanonicalChildProfile(
                childId,
                currentDay > 0 ? currentDay : _simDay);
        }

        private void SetupGenerational()
        {
            EnsureGenerational();
        }

        private void SaveGenerational()
        {
            if (_generational != null)
            {
                CaptureSection("child_development", GenerationalSaveStore.TryCapturePersisted(_generational.CaptureState()));
            }
        }

        private void HandleNurseryAction(string action, string param)
        {
            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                SetupGenerational();
                _nurseryPanel.Bind(EnsureGenerational());
                _nurseryPanel.Open();
                return;
            }
            if (string.Equals(action, "CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                _nurseryPanel.Close();
                return;
            }

            SetupGenerational();
            var system = EnsureGenerational();
            if (string.IsNullOrWhiteSpace(param))
            {
                _nurseryPanel.ShowFeedback("No child under care selected.", isFailure: true);
                _nurseryPanel.RefreshView();
                return;
            }

            string? adultId = FirstAvailableAdultId(system);
            if (string.IsNullOrEmpty(adultId))
            {
                _nurseryPanel.ShowFeedback("No available adult on the roster.", isFailure: true);
                _nurseryPanel.RefreshView();
                return;
            }

            switch (action)
            {
                case "assign_guardian":
                {
                    bool ok = system.AssignGuardian(param, adultId);
                    _nurseryPanel.ShowFeedback(
                        ok ? $"Guardian {adultId} assigned to {param}." : "Guardian assignment failed.",
                        isFailure: !ok);
                    break;
                }
                case "assign_teacher":
                {
                    var child = system.GetChild(param);
                    string focus = child?.educationFocusId ?? "practical_survival";
                    bool ok = system.AssignTeacher(param, adultId, focus);
                    _nurseryPanel.ShowFeedback(
                        ok ? $"Teacher {adultId} assigned to {param} ({focus})." : "Teacher assignment failed.",
                        isFailure: !ok);
                    break;
                }
                default:
                    _nurseryPanel.ShowFeedback($"Unknown nursery action: {action}", isFailure: true);
                    break;
            }
            _nurseryPanel.RefreshView();
        }

        private string? FirstAvailableAdultId(GenerationalSystem generational)
        {
            if (_survivors == null) return null;
            for (int i = 0; i < _survivors.RosterState.Count; i++)
            {
                var s = _survivors.RosterState[i];
                if (s == null || string.IsNullOrWhiteSpace(s.Id)) continue;
                if (generational.GetChild(s.Id) != null) continue;
                return s.Id;
            }
            return null;
        }

    }
}
