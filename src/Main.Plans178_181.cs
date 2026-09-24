// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 178-181 Host Wire & Orchestration
// Subsystems   : Childhood & Generational Rearing, Prisoner Management &
//                Interrogation, Radioactive Mutation Trees, Stealth & Camouflage
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Factions;
using Ashfall.Core.Medical;
using Ashfall.Core.Combat;
using Ashfall.Core.Campaign;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private GenerationalSystem? _generational;
        private PrisonerSystem? _prisoners;
        private MutationSystem? _mutations;
        private StealthSystem? _stealth;

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

        // ── Plan 179: Prisoner Management & Interrogation ─────────────────

        public PrisonerSystem EnsurePrisoners()
        {
            if (_prisoners != null) return _prisoners;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("prisoners") : new SeededRng(179);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _prisoners = new PrisonerSystem(rng, inv, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("interrogation_tactics.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<InterrogationTacticsCatalog>(json);
                        if (catalog?.tactics != null)
                        {
                            foreach (var t in catalog.tactics)
                                _prisoners.RegisterTactic(t);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Prisoners] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = PrisonerSaveStore.TryLoad();
            if (saved != null)
            {
                _prisoners.RestoreState(saved);
            }
            else
            {
                MigrateLegacyShelterPrisoners();
            }

            _prisoners.OnIntelExtracted += (captiveId, intelId, isTrue) =>
            {
                string veracity = isTrue ? "Verified" : "Unconfirmed/Suspect";
                _journal?.TryAddRawEntry("prisoner_intel", $"Interrogation intel recovered from {captiveId} (Report: {intelId}, Status: {veracity}).", null!, _simDay);
            };

            _prisoners.OnPrisonerEscaped += (captiveId) =>
            {
                _journal?.TryAddRawEntry("prison_break", $"Security alert: Captive {captiveId} has breached confinement and escaped into the wasteland!", null!, _simDay);
            };

            _prisoners.OnPrisonerRecruited += (captiveId) =>
            {
                _journal?.TryAddRawEntry("captive_recruited", $"Rehabilitation success: Former captive {captiveId} has formally sworn allegiance to the holdfast.", null!, _simDay);
            };

            return _prisoners;
        }

        /// <summary>
        /// ORPHAN-SEAL-W1 (2026-09-23): one-time import of Plan 63's retired
        /// shelter_prisoners section into the single captive ledger. Runs only
        /// when no canonical save exists yet; terminal records (recruited /
        /// paroled) are not resurrected. The old section is left on disk
        /// untouched and is no longer written.
        /// </summary>
        private void MigrateLegacyShelterPrisoners()
        {
            if (_prisoners == null) return;
            var legacy = ShelterPrisonerSaveStore.TryLoad();
            if (legacy?.Prisoners == null || legacy.Prisoners.Count == 0) return;
            int imported = 0;
            for (int i = 0; i < legacy.Prisoners.Count; i++)
            {
                var record = legacy.Prisoners[i];
                if (record == null || string.IsNullOrEmpty(record.PrisonerId)) continue;
                if (record.Status == Ashfall.Core.Shelter.PrisonerStatus.Paroled
                    || record.Status == Ashfall.Core.Shelter.PrisonerStatus.Escaped
                    || record.Status == Ashfall.Core.Shelter.PrisonerStatus.Deceased) continue;
                string sourceFaction = string.IsNullOrEmpty(record.FactionOrigin)
                    ? "faction_unknown"
                    : record.FactionOrigin;
                if (_prisoners.TakePrisoner(record.PrisonerId, sourceFaction, Math.Max(1, _simDay)))
                    imported++;
            }
            if (imported > 0)
            {
                _journal?.TryAddRawEntry("prisoner_legacy_migration",
                    $"{imported} captive record(s) from the retired Plan 63 cells were moved into the single prisoner ledger.",
                    null!, Math.Max(1, _simDay));
            }
        }

        private void SetupPrisoners()
        {
            EnsurePrisoners();
        }

        private void SavePrisoners()
        {
            if (_prisoners != null)
            {
                CaptureSection("prisoner_management", PrisonerSaveStore.TryCapturePersisted(_prisoners.CaptureState()));
            }
        }

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

        // ── Plan 181: Stealth & Camouflage Mechanics ──────────────────────

        public StealthSystem EnsureStealth()
        {
            if (_stealth != null) return _stealth;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("stealth") : new SeededRng(181);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _stealth = new StealthSystem(rng, inv, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("camouflage_gear.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<CamouflageGearCatalog>(json);
                        if (catalog?.gear != null)
                        {
                            foreach (var g in catalog.gear)
                                _stealth.RegisterCamouflageGear(g);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Stealth] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            _stealth.RegisterWeaponNoise(new WeaponNoiseProfile
            {
                weapon_id = "weapon_assault_rifle",
                handling_noise = 0.12f,
                melee_noise = 0.18f,
                fired_noise = 0.85f,
                is_suppressed = false
            });
            _stealth.RegisterWeaponNoise(new WeaponNoiseProfile
            {
                weapon_id = "weapon_pipe_rifle",
                handling_noise = 0.16f,
                melee_noise = 0.22f,
                fired_noise = 0.92f,
                is_suppressed = false
            });
            _stealth.RegisterWeaponNoise(new WeaponNoiseProfile
            {
                weapon_id = "weapon_suppressed_rifle",
                handling_noise = 0.10f,
                melee_noise = 0.18f,
                fired_noise = 0.28f,
                is_suppressed = true
            });

            var saved = StealthSaveStore.TryLoad();
            if (saved != null)
            {
                _stealth.RestoreState(saved);
            }

            _stealth.OnStealthBroken += (expeditionId, reason) =>
            {
                _journal?.TryAddRawEntry("stealth_broken", $"Expedition {expeditionId} had its concealment broken! Trigger: {reason}.", null!, _simDay);
            };

            return _stealth;
        }

        private void SetupStealth()
        {
            EnsureStealth();
        }

        private void SaveStealth()
        {
            if (_stealth != null)
            {
                CaptureSection("expedition_stealth", StealthSaveStore.TryCapturePersisted(_stealth.CaptureState()));
            }
        }

        // ── Daily Tick Advance ──────────────────────────────────────────

        public void TickPlans178_181(int currentDay)
        {
            EnsureGenerational().GrowthTick(currentDay);
            EnsurePrisoners().TickUpkeepAndEscape(currentDay);
        }

        private void HandlePrisonerAction(string action, string param)
        {
            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                SetupPrisoners();
                _prisonerPanel.Bind(EnsurePrisoners());
                _prisonerPanel.Open();
                return;
            }
            if (string.Equals(action, "CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                _prisonerPanel.Close();
                return;
            }

            SetupPrisoners();
            var system = EnsurePrisoners();
            if (string.IsNullOrWhiteSpace(param))
            {
                _prisonerPanel.ShowFeedback("No detained captive selected.", isFailure: true);
                _prisonerPanel.RefreshView();
                return;
            }

            switch (action)
            {
                case "interrogate":
                {
                    string? tacticId = system.FirstTacticId;
                    if (string.IsNullOrEmpty(tacticId))
                    {
                        _prisonerPanel.ShowFeedback("No interrogation tactics registered.", isFailure: true);
                        break;
                    }
                    var result = system.Interrogate(param, tacticId, _simDay);
                    string successMsg = result.IntelDiscovered
                        ? (result.IsFalseIntel
                            ? $"Interrogation yielded a lead ({result.ExtractedIntelId}) — authenticity uncertain."
                            : $"Interrogation extracted intel {result.ExtractedIntelId}.")
                        : "Interrogation complete — no new intel this round.";
                    _prisonerPanel.ShowFeedback(
                        result.Success ? successMsg : $"Interrogation failed: {result.FailureCode}",
                        isFailure: !result.Success);
                    break;
                }
                case "recruit":
                {
                    bool ok = system.RecruitPrisoner(param, _simDay);
                    _prisonerPanel.ShowFeedback(
                        ok ? $"Captive {param} recruited into the holdfast." : "Recruitment refused — trust, time, or abuse history blocks it.",
                        isFailure: !ok);
                    break;
                }
                case "release":
                {
                    bool ok = system.ReleasePrisoner(param);
                    _prisonerPanel.ShowFeedback(
                        ok ? $"Captive {param} released." : "Release failed — captive not detained.",
                        isFailure: !ok);
                    break;
                }
                default:
                    _prisonerPanel.ShowFeedback($"Unknown detention action: {action}", isFailure: true);
                    break;
            }
            _prisonerPanel.RefreshView();
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
