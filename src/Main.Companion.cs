// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 174 — Companion Animals host wire
// System       : CompanionAnimalSystem (persistent companion authority)
// Authority    : Core owns care/bond/training/role eligibility/sickness;
//                the host feeds canonical food ports, forks the day-keyed
//                sickness RNG, routes bounded role modifiers into the owning
//                systems (DefenseSystem, ExpeditionSystem, NeedsSystem), and
//                persists state. No presentation logic here.
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Ecology;
using Ashfall.Core.IO;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CompanionAnimalSystem? _companions;
        private CompanionAnimalHostSession? _companionSession;

        /// <summary>Preferred food tag → concrete stocked item id (host mapping;
        /// the item quantities stay in the canonical inventory).</summary>
        private static readonly IReadOnlyDictionary<string, string> CompanionPreferredFoodMap =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "meat", "raw_meat" },
                { "raw_meat", "raw_meat" },
                { "forage", "crop_leafy_green" },
                { "grain", "crop_ash_grain" },
                { "greens", "crop_leafy_green" }
            };

        // ── Plan 174: Companion Animals & Working Beasts ────────────────

        public CompanionAnimalSystem EnsureCompanions()
        {
            if (_companions != null) return _companions;

            var profiles = new List<CompanionSpeciesProfile>();
            string catalogPath = CatalogPath.ResolveCatalog("companion_animals.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var root = System.Text.Json.JsonSerializer.Deserialize<CompanionCatalogRoot>(json);
                        if (root?.companions != null)
                        {
                            var load = new CompanionCatalogLoadResult();
                            foreach (var def in root.companions)
                                if (def != null) load.Companions.Add(def);
                            profiles.AddRange(CompanionAnimalCatalogLoader.ToProfiles(load));
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Companion] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            _companions = new CompanionAnimalSystem(profiles);

            // Canonical food port (single item-quantity authority).
            var inv = _inventory?.Inventory;
            if (inv != null)
            {
                _companions.BindFoodPort(
                    id => inv.CountById(id),
                    (id, amount) => inv.RemoveById(id, amount));
            }

            // Species knowledge delegated to the wildlife species authority (§5.1).
            _companions.KnownSpeciesCheck = speciesId =>
                _wildlifeEcosystem?.System?.Species(speciesId) != null;

            // Pack cargo route (§5.11): the expedition authority applies the
            // bonus only after ITS OWN successful start — ordering-safe because
            // the provider resolves lazily through this captured instance.
            BindCompanionSeams();

            var saved = CompanionSaveStore.TryLoad();
            if (saved != null)
            {
                _companions.RestoreState(saved);
            }
            else
            {
                // Old-save baseline: adopt any already-tamed wildlife animals
                // (never fabricate animals — only real taming records map).
                AdoptExistingTamedAnimals();
            }

            _companions.OnCompanionRegistered += c =>
            {
                _journal?.TryAddRawEntry($"companion_registered_{c.companion_id}",
                    $"A {c.species_id.Replace("species_", "").Replace('_', ' ')} joined the pens as a companion.",
                    null!, _simDay);
            };
            _companions.OnRoleChanged += (c, role) =>
            {
                _journal?.TryAddRawEntry($"companion_role_{c.companion_id}",
                    $"{c.name} is now on {role.ToString().ToLowerInvariant()} duty.",
                    null!, _simDay);
            };
            _companions.OnSicknessChanged += (c, s) =>
            {
                _journal?.TryAddRawEntry($"companion_sick_{c.companion_id}",
                    $"{c.name} has fallen ill — {SicknessLabel(s)}. Treatment will be needed.",
                    null!, _simDay);
            };
            _companions.OnCompanionRecovered += c =>
            {
                _journal?.TryAddRawEntry($"companion_recovered_{c.companion_id}",
                    $"{c.name} is back on its feet.", null!, _simDay);
            };
            _companions.OnHungerChanged += (c, hunger) =>
            {
                if (hunger >= CompanionAnimalSystem.HungerCritical)
                    _journal?.TryAddRawEntry($"companion_starving_{c.companion_id}",
                        $"{c.name} is starving — the stores have no {c.species_id.Replace("species_", "").Replace('_', ' ')} feed.",
                        null!, _simDay);
            };
            _companions.OnCompanionDied += c => HandleCompanionDeath(c);

            return _companions;
        }

        private void SetupCompanionAnimals()
        {
            EnsureCompanions();
        }

        /// <summary>Ordering-safe seam binding: safe to call from both the
        /// companion setup and the expedition setup (idempotent delegate sets).</summary>
        private void BindCompanionSeams()
        {
            if (_expeditions != null)
            {
                _expeditions.PackCapacityProvider = survivorId =>
                    _companions?.GetPackCapacityBonusForSurvivor(survivorId) ?? 0f;
            }
        }

        private void SaveCompanionAnimals()
        {
            if (_companions != null)
            {
                CaptureSection("companion_animals", CompanionSaveStore.TryCapturePersisted(_companions.CaptureState()));
            }
        }

        /// <summary>Old-save migration: register companions ONLY from existing
        /// wildlife taming records (never fabricate animals — §10).</summary>
        private void AdoptExistingTamedAnimals()
        {
            var wildlife = _wildlifeEcosystem?.System;
            if (wildlife == null || _companions == null) return;
            foreach (var d in wildlife.DomesticAnimals)
            {
                if (d == null || string.IsNullOrEmpty(d.animal_id)) continue;
                if (_companions.Companion(d.animal_id) != null) continue;
                _companions.RegisterCompanion(d.animal_id, d.species_id, d.tamed_day, null);
            }
        }

        /// <summary>Bounded grief (§5.15): the companion system computes the
        /// bond-scaled delta; the canonical NeedsSystem morale channel applies
        /// it exactly once. The system never writes morale directly.</summary>
        private void HandleCompanionDeath(CompanionState c)
        {
            _journal?.TryAddRawEntry($"companion_died_{c.companion_id}",
                $"{c.name} is gone. The kennel is quieter than it was.",
                null!, _simDay);

            int deltaBp = _companions?.GetGriefMoraleDeltaBp(c.companion_id) ?? 0;
            if (deltaBp == 0 || string.IsNullOrEmpty(c.assigned_survivor_id)) return;

            var needs = _survivors?.Needs;
            var survivorState = needs?.Get(c.assigned_survivor_id);
            if (survivorState != null && survivorState.IsAliveState)
            {
                // bp → morale points (100 bp = 1 point). Negative delta = morale
                // damage through the canonical Modify convention.
                float moraleDelta = deltaBp / 100f;
                needs!.Modify(survivorState, Ashfall.Core.Survivors.NeedKind.Morale, moraleDelta);
            }
        }

        /// <summary>
        /// Daily companion tick (Plan 174). Deterministic: day-keyed sickness
        /// fork, real inventory feeding, expedition-presence sync, and the
        /// bounded morale-support route through the canonical NeedsSystem.
        /// </summary>
        public void TickCompanionDay(int day)
        {
            if (_companions == null) return;

            // Day-keyed sickness fork (Core stores no RNG state).
            ISeededRng sicknessRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.CompanionAnimal, day, 0)
                : new SeededRng(unchecked(174 * 397 + day));
            _companions.SicknessRoll = () => sicknessRng.NextDouble();

            _companions.TickDay(day);

            // Expedition presence sync (host knows the roster).
            var activeExpeditions = _expeditions?.Engine?.Active;
            foreach (var c in _companions.State.companions)
            {
                if (c == null) continue;
                bool handlerAway = !string.IsNullOrEmpty(c.assigned_survivor_id)
                    && activeExpeditions != null
                    && activeExpeditions.ContainsKey(c.assigned_survivor_id);
                _companions.SetOnExpedition(c.companion_id, handlerAway);
            }

            // Bounded daily morale support through the canonical authority (§5.12).
            var needs = _survivors?.Needs;
            if (needs == null) return;
            var handled = new HashSet<string>(StringComparer.Ordinal);
            foreach (var c in _companions.State.companions)
            {
                if (c == null || !c.alive) continue;
                if ((CompanionRole)c.role != CompanionRole.Morale) continue;
                if (string.IsNullOrEmpty(c.assigned_survivor_id) || !handled.Add(c.assigned_survivor_id)) continue;
                int supportBp = _companions.GetMoraleSupportBp(c.companion_id);
                if (supportBp <= 0) continue;
                var state = needs.Get(c.assigned_survivor_id);
                if (state == null || !state.IsAliveState) continue;
                // Positive delta = morale support through the canonical Modify
                // convention; bounded by the authored bp ceiling.
                needs.Modify(state, Ashfall.Core.Survivors.NeedKind.Morale, supportBp / 100f);
            }
        }

        /// <summary>Host command: veterinary treatment via canonical item.</summary>
        public CompanionAssignResult CompanionTreat(string companionId, string itemId)
        {
            if (_companions == null) return new CompanionAssignResult { Success = false, ReasonCode = "companion_system_unbound" };
            return _companions.TreatSickness(companionId, itemId);
        }

        public CompanionAnimalHostSession EnsureCompanionSession()
        {
            EnsureCompanions();
            if (_companionSession == null && _companions != null)
            {
                _companionSession = new CompanionAnimalHostSession(_companions);
            }
            return _companionSession!;
        }

        public CompanionAssignResult AssignCompanion(string companionId, string survivorId, CompanionRole role)
        {
            EnsureCompanions();
            if (_companions == null) return CompanionAssignResult.Fail("companion_system_unbound");
            return _companions.Assign(companionId, survivorId, role, id => _survivors?.Find(id)?.IsAlive ?? true);
        }

        public CompanionFeedResult FeedCompanion(string companionId, int day)
        {
            EnsureCompanions();
            if (_companions == null) return new CompanionFeedResult { ReasonCode = "companion_system_unbound" };
            var c = _companions.Companion(companionId);
            if (c == null) return new CompanionFeedResult { ReasonCode = "unknown_companion" };
            var profile = _companions.Profile(c.species_id);
            if (profile == null) return new CompanionFeedResult { ReasonCode = "unknown_species" };
            return _companions.Feed(c, profile, day, CompanionPreferredFoodMap);
        }

        public CompanionAssignResult RegisterCompanion(string companionId, string speciesId, int tamedDay, string? name = null)
        {
            EnsureCompanions();
            if (_companions == null) return CompanionAssignResult.Fail("companion_system_unbound");
            return _companions.RegisterCompanion(companionId, speciesId, tamedDay, name);
        }

        public IReadOnlyList<CompanionState> GetAllCompanions()
        {
            EnsureCompanions();
            return _companions?.State.companions ?? (IReadOnlyList<CompanionState>)Array.Empty<CompanionState>();
        }

        public float GetGuardModifierTotal()
        {
            EnsureCompanions();
            return _companions?.GetGuardModifierTotal() ?? 0f;
        }

        public float GetPackCapacityBonusForSurvivor(string survivorId)
        {
            EnsureCompanions();
            return _companions?.GetPackCapacityBonusForSurvivor(survivorId) ?? 0f;
        }

        private static string SicknessLabel(CompanionSicknessState s) => s switch
        {
            CompanionSicknessState.Infection => "infection",
            CompanionSicknessState.Injured => "injury",
            CompanionSicknessState.Malnutrition => "malnutrition",
            CompanionSicknessState.RadiationSickness => "radiation sickness",
            _ => "healthy"
        };
    }
}
