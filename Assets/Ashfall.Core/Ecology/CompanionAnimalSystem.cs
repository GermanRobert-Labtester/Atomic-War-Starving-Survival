// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 174 — Companion Animals & Working Beasts (Core authority).
// Authority split (one authority per concern):
//   • WildlifeEcosystemSystem — species catalog + taming (OnWildlifeTamed
//     creates the DomesticAnimalState origin record). This system NEVER
//     re-defines species and never thins wild populations.
//   • Inventory (via IPlayerInventoryPort) — food item quantities.
//   • NeedsSystem / MoraleContagion / MoraleMark — survivor morale. This
//     system only REPORTS bounded support modifiers (§5.12); the morale
//     authorities apply them.
//   • DefenseSystem / PerimeterDefenseSystem — raid detection. Guard benefit
//     is a bounded modifier the host routes; combat stays with its owners.
//   • ExpeditionSystem — cargo capacity. Pack benefit is a bounded kg bonus
//     query; the expedition authority applies it with its own rules.
//   • MedicalPipelineCoordinator / DiseaseSystem — afflictions. Companion
//     sickness states are veterinary extensions the medical host treats.
//   • RadiationSystem — dose (Plan 176 handoff feeds it, never bypassed).
// Determinism: bond/training/upkeep are pure functions of state + catalog +
// day. Only sickness rolls consume RNG (host-forked, day-keyed).
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.IO;

namespace Ashfall.Core.Ecology
{
    /// <summary>Companion working roles (§5.10-5.12). Untrained animals serve
    /// none; one role at a time at full strength (§5.5).</summary>
    public enum CompanionRole
    {
        Unassigned = 0,
        Guard = 1,
        Pack = 2,
        Morale = 3
    }

    public enum CompanionTrainingLevel
    {
        Untrained = 0,
        Familiar = 1,
        Trained = 2,
        Reliable = 3,
        Expert = 4
    }

    public enum CompanionSicknessState
    {
        Healthy = 0,
        Infection = 1,
        Injured = 2,
        Malnutrition = 3,
        RadiationSickness = 4
    }

    /// <summary>One authored companion profile per tameable wildlife species.</summary>
    [Serializable]
    public sealed class CompanionSpeciesProfile
    {
        public string species_id { get; set; } = string.Empty;     // must exist in wildlife_ecosystem.json
        public string display_name { get; set; } = string.Empty;
        public List<string> role_tags { get; set; } = new List<string>();    // guard | pack | morale
        public int base_food_per_day { get; set; }                 // units of canonical food/day
        public List<string> preferred_food_tags { get; set; } = new List<string>();
        public List<string> fallback_food_item_ids { get; set; } = new List<string>();
        public int max_health { get; set; } = 50;
        public int trainability { get; set; } = 5;                 // 1..10
        public int bond_rate { get; set; } = 5;                    // 1..10
        public int guard_rating { get; set; } = 0;                 // 0..100 warning value
        public int pack_capacity_kg { get; set; } = 0;             // 0..60
        public int morale_support_bp { get; set; } = 0;            // 0..500 bounded
        public int disease_resistance { get; set; } = 0;           // 0..10
        public List<string> terrain_tags { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class CompanionCatalogRoot
    {
        public int schema_version { get; set; } = 1;
        public List<CompanionSpeciesProfile> companions { get; set; } = new List<CompanionSpeciesProfile>();
    }

    /// <summary>Load outcome: rows plus validation errors (domain result, no exceptions).</summary>
    public sealed class CompanionCatalogLoadResult
    {
        public List<CompanionSpeciesProfile> Companions { get; } = new List<CompanionSpeciesProfile>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>Persistent companion instance state (§5.3). Derived bonuses are
    /// never persisted — they are computed from these fields on demand.</summary>
    [Serializable]
    public sealed class CompanionState
    {
        public string companion_id { get; set; } = string.Empty;   // = wildlife DomesticAnimalState.animal_id
        public string species_id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;           // presentation/state data, NOT identity
        public string assigned_survivor_id { get; set; } = string.Empty;
        public int role { get; set; } = (int)CompanionRole.Unassigned;
        public int training_progress { get; set; }                 // 0..TrainingProgressPerLevel-1
        public int training_level { get; set; }                    // (int) CompanionTrainingLevel
        public int bond { get; set; }                              // 0..MaxBond
        public int health { get; set; } = 50;
        public int hunger { get; set; }                            // 0..100, HIGHER = WORSE (NeedsSystem parity)
        public int sickness { get; set; } = 0;                     // (int) CompanionSicknessState
        public int last_fed_day { get; set; } = -1;
        public int tamed_day { get; set; }
        public bool on_expedition { get; set; }
        public bool alive { get; set; } = true;
    }

    [Serializable]
    public sealed class CompanionSystemState
    {
        public string system_id { get; set; } = "companion_animals";
        public int schema_version { get; set; } = 1;
        public int last_tick_day { get; set; }
        public List<CompanionState> companions { get; set; } = new List<CompanionState>();
    }

    /// <summary>Assignment outcome (domain result, no exceptions).</summary>
    public sealed class CompanionAssignResult
    {
        public bool Success;
        public string ReasonCode = string.Empty;                   // unknown_companion | companion_dead |
                                                                   // unknown_survivor | role_incompatible |
                                                                   // already_assigned_to_other | handler_dead
        public static CompanionAssignResult Fail(string reason) => new CompanionAssignResult { Success = false, ReasonCode = reason };
    }

    /// <summary>Feeding outcome for one companion day.</summary>
    public sealed class CompanionFeedResult
    {
        public bool Fed;
        public string FoodItemId = string.Empty;                   // canonical item consumed ("" = none available)
        public bool UsedEmergencyFallback;                          // fallback item: reduced benefit
        public string ReasonCode = string.Empty;                   // no_food_available | already_fed | companion_dead
    }

    /// <summary>
    /// Companion/working-beast authority: persistent instances over the
    /// wildlife species+taming authorities, with deterministic care, bond,
    /// training, bounded role benefits, sickness, and death.
    /// </summary>
    public sealed class CompanionAnimalSystem
    {
        public const string SystemId = "companion_animals";

        public const int MaxBond = 100;
        public const int TrainingProgressPerLevel = 10;
        public const int HungerDailyGain = 25;             // unfed day
        public const int HungerCritical = 80;              // health risk begins (§5.8: never one missed meal → death)
        public const int HealthLossPerHungryDay = 6;
        public const int HealthLossPerSickDay = 4;
        public const int BondDailyCareGain = 2;            // fed + tended day (§5.6)
        public const int BondHungerLossPerDay = 3;
        public const int BondSickLossPerDay = 2;
        public const int BondOnDeathOfHandler = 10;
        public const int GriefMoraleShockMaxBp = -1500;    // bounded grief ceiling (§5.15 — morale authority applies)
        public const int GriefMoraleFloorBp = -300;        // low-bond companions still register, smaller shock
        public const float GuardBenefitHealthFloor = 0.4f; // injured/hungry animals fade (§5.10)
        public const float PackBenefitHealthFloor = 0.4f;

        private readonly Dictionary<string, CompanionSpeciesProfile> _profiles =
            new Dictionary<string, CompanionSpeciesProfile>(StringComparer.Ordinal);
        private CompanionSystemState _state = new CompanionSystemState();

        /// <summary>Host-provided food port (canonical inventory authority).</summary>
        private Func<string, int>? _foodCount;
        private Action<string, int>? _foodConsume;
        /// <summary>Host-forked sickness RNG (day-keyed); Core stores no RNG state.</summary>
        public Func<double>? SicknessRoll;
        /// <summary>Wildlife species resolver: only species the wildlife authority
        /// knows can be profiled (§5.1 — no duplicate species definitions).</summary>
        public Func<string, bool>? KnownSpeciesCheck;

        public event Action<CompanionState>? OnCompanionRegistered;
        public event Action<CompanionState, CompanionRole>? OnRoleChanged;
        public event Action<CompanionState, int>? OnHungerChanged;
        public event Action<CompanionState, CompanionSicknessState>? OnSicknessChanged;
        public event Action<CompanionState>? OnCompanionRecovered;
        public event Action<CompanionState>? OnCompanionDied;

        public CompanionSystemState State => _state;
        public IReadOnlyCollection<CompanionSpeciesProfile> Profiles => _profiles.Values;

        public CompanionAnimalSystem(IEnumerable<CompanionSpeciesProfile>? profiles = null)
        {
            if (profiles != null)
                foreach (var p in profiles)
                    if (p != null && !string.IsNullOrEmpty(p.species_id)) _profiles[p.species_id] = p;
        }

        public void BindFoodPort(Func<string, int> count, Action<string, int> consume)
        {
            _foodCount = count;
            _foodConsume = consume;
        }

        public CompanionSpeciesProfile? Profile(string speciesId) =>
            !string.IsNullOrEmpty(speciesId) && _profiles.TryGetValue(speciesId, out var p) ? p : null;

        // ── Registration (§5.4: stable identity from the wildlife authority) ──

        /// <summary>Adopt a tamed animal as a persistent companion. Requires the
        /// species profile and (when bound) wildlife species knowledge. The
        /// companion_id MUST be the wildlife DomesticAnimalState.animal_id.</summary>
        public CompanionAssignResult RegisterCompanion(string companionId, string speciesId, int tamedDay, string? name = null)
        {
            if (string.IsNullOrEmpty(companionId) || _state.companions.Any(c => c != null && string.Equals(c.companion_id, companionId, StringComparison.Ordinal)))
                return CompanionAssignResult.Fail("unknown_companion");
            var profile = Profile(speciesId);
            if (profile == null) return CompanionAssignResult.Fail("unknown_species");
            if (KnownSpeciesCheck != null && !KnownSpeciesCheck(speciesId))
                return CompanionAssignResult.Fail("unknown_species");

            var state = new CompanionState
            {
                companion_id = companionId,
                species_id = speciesId,
                name = string.IsNullOrEmpty(name) ? profile.display_name : name,
                health = profile.max_health,
                tamed_day = tamedDay
            };
            _state.companions.Add(state);
            OnCompanionRegistered?.Invoke(state);
            return new CompanionAssignResult { Success = true, ReasonCode = "registered" };
        }

        public CompanionState? Companion(string companionId) =>
            _state.companions.FirstOrDefault(c => c != null && string.Equals(c.companion_id, companionId, StringComparison.Ordinal));

        /// <summary>Best guard contribution across all companions assigned to the
        /// Guard role (for BaseDefense raid detection routing, §5.10).</summary>
        public float GetGuardModifierTotal()
        {
            float best = 0f;
            foreach (var c in _state.companions)
            {
                if (c == null || !c.alive) continue;
                if ((CompanionRole)c.role != CompanionRole.Guard) continue;
                best = Math.Max(best, GetGuardModifier(c.companion_id));
            }
            return best;
        }

        /// <summary>Best pack bonus across companions assigned to the Pack role
        /// for one handler (§5.11). The expedition authority applies its own
        /// terrain/capacity rules on top.</summary>
        public float GetPackCapacityBonusForSurvivor(string survivorId)
        {
            float best = 0f;
            foreach (var c in _state.companions)
            {
                if (c == null || !c.alive) continue;
                if (!string.Equals(c.assigned_survivor_id, survivorId, StringComparison.Ordinal)) continue;
                if ((CompanionRole)c.role != CompanionRole.Pack) continue;
                best = Math.Max(best, GetPackCapacityBonus(c.companion_id));
            }
            return best;
        }

        /// <summary>Mark expedition presence (host knows the roster; Core stores it).</summary>
        public void SetOnExpedition(string companionId, bool onExpedition)
        {
            var c = Companion(companionId);
            if (c == null) return;
            c.on_expedition = onExpedition;
        }

        /// <summary>Veterinary treatment handoff (§5.13): consumes a canonical
        /// medical item through the bound port and clears a treatable state.
        /// Malnutrition resolves only through feeding, not a kit.</summary>
        public CompanionAssignResult TreatSickness(string companionId, string itemId)
        {
            var c = Companion(companionId);
            if (c == null) return CompanionAssignResult.Fail("unknown_companion");
            if (!c.alive) return CompanionAssignResult.Fail("companion_dead");
            var sickness = (CompanionSicknessState)c.sickness;
            if (sickness == CompanionSicknessState.Healthy)
                return CompanionAssignResult.Fail("not_sick");
            if (sickness == CompanionSicknessState.Malnutrition)
                return CompanionAssignResult.Fail("needs_feeding_not_treatment");
            if (_foodCount == null || _foodConsume == null)
                return CompanionAssignResult.Fail("food_port_unbound");
            if (_foodCount(itemId) <= 0)
                return CompanionAssignResult.Fail("missing_treatment_item");

            _foodConsume(itemId, 1);
            c.sickness = (int)CompanionSicknessState.Healthy;
            OnCompanionRecovered?.Invoke(c);
            return new CompanionAssignResult { Success = true, ReasonCode = "treated" };
        }

        // ── Assignment (§5.5) ──────────────────────────────────────────

        public CompanionAssignResult Assign(string companionId, string survivorId, CompanionRole role, Func<string, bool>? survivorAlive = null)
        {
            var c = Companion(companionId);
            if (c == null) return CompanionAssignResult.Fail("unknown_companion");
            if (!c.alive) return CompanionAssignResult.Fail("companion_dead");
            if (string.IsNullOrEmpty(survivorId)) return CompanionAssignResult.Fail("unknown_survivor");
            if (survivorAlive != null && !survivorAlive(survivorId)) return CompanionAssignResult.Fail("handler_dead");

            var profile = Profile(c.species_id);
            if (profile == null) return CompanionAssignResult.Fail("unknown_species");
            string roleTag = role switch
            {
                CompanionRole.Guard => "guard",
                CompanionRole.Pack => "pack",
                CompanionRole.Morale => "morale",
                _ => "unassigned"
            };
            if (role != CompanionRole.Unassigned && !profile.role_tags.Contains(roleTag))
                return CompanionAssignResult.Fail("role_incompatible");

            // Bond ownership: an animal keeps at most one bonded handler. A new
            // assignment requires either no handler or the same handler.
            if (!string.IsNullOrEmpty(c.assigned_survivor_id)
                && !string.Equals(c.assigned_survivor_id, survivorId, StringComparison.Ordinal)
                && role != CompanionRole.Unassigned)
                return CompanionAssignResult.Fail("already_assigned_to_other");

            bool roleChanged = c.role != (int)role;
            c.assigned_survivor_id = survivorId;
            c.role = (int)role;
            if (roleChanged) OnRoleChanged?.Invoke(c, role);
            return new CompanionAssignResult { Success = true, ReasonCode = "assigned" };
        }

        // ── Daily tick (§5.6-5.8, §5.13, §5.15) ────────────────────────

        /// <summary>
        /// One companion day: feed hierarchy → hunger/health → bond/training →
        /// sickness rolls → death. Deterministic except the host-forked
        /// sickness/death rolls. Feeding consumes REAL inventory units through
        /// the bound port — never a parallel food store (§5.8).
        /// </summary>
        public void TickDay(int day)
        {
            if (_state.last_tick_day == day) return;
            _state.last_tick_day = day;

            var ordered = _state.companions.Where(c => c != null).OrderBy(c => c.companion_id, StringComparer.Ordinal).ToList();
            foreach (var c in ordered)
            {
                if (!c.alive) continue;
                var profile = Profile(c.species_id);
                if (profile == null) continue;

                // 1. Feeding (preferred → fallback → none).
                var feed = Feed(c, profile, day);
                bool fedToday = feed.Fed;

                // 2. Hunger & health drift.
                if (!fedToday)
                {
                    c.hunger = Math.Min(100, c.hunger + HungerDailyGain);
                    OnHungerChanged?.Invoke(c, c.hunger);
                }
                int healthLoss = 0;
                if (c.hunger >= HungerCritical) healthLoss += HealthLossPerHungryDay;
                if (c.sickness != (int)CompanionSicknessState.Healthy) healthLoss += HealthLossPerSickDay;
                if (healthLoss > 0) c.health = Math.Max(0, c.health - healthLoss);

                // 3. Bond progression (deterministic care math).
                if (fedToday) c.bond = Math.Min(MaxBond, c.bond + BondDailyCareGain + Math.Max(0, profile.bond_rate) / 3);
                if (c.hunger >= HungerCritical) c.bond = Math.Max(0, c.bond - BondHungerLossPerDay);
                if (c.sickness != (int)CompanionSicknessState.Healthy) c.bond = Math.Max(0, c.bond - BondSickLossPerDay);

                // 4. Training progression (needs handler, food, health).
                if (!string.IsNullOrEmpty(c.assigned_survivor_id) && fedToday
                    && c.health > profile.max_health / 2
                    && (CompanionRole)c.role != CompanionRole.Unassigned)
                {
                    int gain = Math.Max(1, (profile.trainability + c.bond / 25) / 3);
                    c.training_progress += gain;
                    if (c.training_progress >= TrainingProgressPerLevel && c.training_level < (int)CompanionTrainingLevel.Expert)
                    {
                        c.training_progress = 0;
                        c.training_level++;
                    }
                }

                // 5. Sickness roll (host-forked; healthy + hungry animals at risk).
                if (SicknessRoll != null && c.sickness == (int)CompanionSicknessState.Healthy
                    && c.hunger >= 50
                    && SicknessRoll() < 0.15 + (c.hunger / 1000.0))
                {
                    c.sickness = (int)CompanionSicknessState.Infection;
                    OnSicknessChanged?.Invoke(c, CompanionSicknessState.Infection);
                }
                else if (SicknessRoll != null && c.sickness != (int)CompanionSicknessState.Healthy
                    && c.hunger < 50
                    && SicknessRoll() < 0.3 + profile.disease_resistance * 0.03)
                {
                    c.sickness = (int)CompanionSicknessState.Healthy;
                    OnCompanionRecovered?.Invoke(c);
                }

                // 6. Death — only sustained neglect/sickness reaches zero (§5.8).
                if (c.health <= 0)
                {
                    c.alive = false;
                    OnCompanionDied?.Invoke(c);
                }
            }
        }

        /// <summary>Feed one companion from canonical inventory: preferred tag
        /// items first, then authored fallback items, then nothing. The caller
        /// maps preferred tags to concrete item ids (host-side mapping table).</summary>
        public CompanionFeedResult Feed(CompanionState c, CompanionSpeciesProfile profile, int day,
            IReadOnlyDictionary<string, string>? preferredTagItemMap = null)
        {
            if (!c.alive) return new CompanionFeedResult { ReasonCode = "companion_dead" };
            if (c.last_fed_day == day) return new CompanionFeedResult { ReasonCode = "already_fed", FoodItemId = string.Empty };

            string itemId = ResolveFood(profile, preferredTagItemMap);
            if (string.IsNullOrEmpty(itemId))
                return new CompanionFeedResult { ReasonCode = "no_food_available" };

            int needed = Math.Max(1, profile.base_food_per_day);
            if (_foodCount == null || _foodConsume == null)
                return new CompanionFeedResult { ReasonCode = "food_port_unbound" };
            if (_foodCount(itemId) < needed)
            {
                // Short stock: feed what exists (partial), hunger eases but stays.
                int available = _foodCount(itemId);
                if (available <= 0) return new CompanionFeedResult { ReasonCode = "no_food_available" };
                _foodConsume(itemId, available);
                c.last_fed_day = day;
                c.hunger = Math.Max(0, c.hunger - 10 * available);
                OnHungerChanged?.Invoke(c, c.hunger);
                return new CompanionFeedResult { Fed = true, FoodItemId = itemId, UsedEmergencyFallback = true, ReasonCode = "partial_feed" };
            }

            _foodConsume(itemId, needed);
            c.last_fed_day = day;
            c.hunger = 0;
            OnHungerChanged?.Invoke(c, 0);
            bool fallback = !string.IsNullOrEmpty(itemId) && profile.fallback_food_item_ids.Contains(itemId)
                && (preferredTagItemMap == null || !preferredTagItemMap.Values.Contains(itemId));
            return new CompanionFeedResult { Fed = true, FoodItemId = itemId, UsedEmergencyFallback = fallback, ReasonCode = "fed" };
        }

        private string ResolveFood(CompanionSpeciesProfile profile, IReadOnlyDictionary<string, string>? preferredTagItemMap)
        {
            // Preferred tags first (host maps tag → concrete item id it stocks).
            if (preferredTagItemMap != null && _foodCount != null)
            {
                foreach (var tag in profile.preferred_food_tags)
                {
                    if (!preferredTagItemMap.TryGetValue(tag, out var itemId)) continue;
                    if (_foodCount(itemId) >= Math.Max(1, profile.base_food_per_day)) return itemId;
                }
            }
            // Authored fallback items.
            if (_foodCount != null)
            {
                foreach (var itemId in profile.fallback_food_item_ids)
                    if (_foodCount(itemId) >= Math.Max(1, profile.base_food_per_day)) return itemId;
            }
            return string.Empty;
        }

        // ── Bounded role benefit queries (host routes to the owning systems) ──

        /// <summary>Guard warning modifier (0..guard_rating) for BaseDefense:
        /// scales with training, health, hunger, and presence (§5.10).</summary>
        public float GetGuardModifier(string companionId)
        {
            var c = Companion(companionId);
            if (c == null || !c.alive) return 0f;
            var profile = Profile(c.species_id);
            if (profile == null || profile.guard_rating <= 0) return 0f;
            if ((CompanionRole)c.role != CompanionRole.Guard) return 0f;

            float trainingFactor = 0.35f + 0.1625f * c.training_level;   // Untrained 0.35 → Expert 1.0
            float condition = Math.Min(1f, (float)c.health / Math.Max(1, profile.max_health));
            if (condition < GuardBenefitHealthFloor) condition = 0f;
            float hungerFactor = 1f - (c.hunger / 200f);                 // starving animals dull
            if (c.sickness != (int)CompanionSicknessState.Healthy) hungerFactor *= 0.5f;
            float bondFactor = 0.6f + 0.4f * ((float)c.bond / MaxBond);

            return Math.Max(0f, profile.guard_rating * trainingFactor * condition * Math.Clamp(hungerFactor, 0f, 1f) * bondFactor);
        }

        /// <summary>Pack cargo bonus (kg) for ExpeditionSystem: scales with
        /// training, health, and absence of sickness (§5.11). The expedition
        /// authority applies terrain penalties and its own capacity rules.</summary>
        public float GetPackCapacityBonus(string companionId)
        {
            var c = Companion(companionId);
            if (c == null || !c.alive) return 0f;
            var profile = Profile(c.species_id);
            if (profile == null || profile.pack_capacity_kg <= 0) return 0f;
            if ((CompanionRole)c.role != CompanionRole.Pack) return 0f;

            float trainingFactor = 0.5f + 0.125f * c.training_level;    // Untrained 0.5 → Expert 1.0
            float condition = Math.Min(1f, (float)c.health / Math.Max(1, profile.max_health));
            if (condition < PackBenefitHealthFloor) condition = 0f;
            if (c.sickness != (int)CompanionSicknessState.Healthy) condition *= 0.5f;

            return Math.Max(0f, profile.pack_capacity_kg * trainingFactor * condition);
        }

        /// <summary>Bounded morale support (bp) for the morale authorities
        /// (§5.12): scales with bond; grieving/dead companions return 0.</summary>
        public int GetMoraleSupportBp(string companionId)
        {
            var c = Companion(companionId);
            if (c == null || !c.alive) return 0;
            var profile = Profile(c.species_id);
            if (profile == null || (CompanionRole)c.role != CompanionRole.Morale) return 0;
            float bondFactor = 0.4f + 0.6f * ((float)c.bond / MaxBond);
            float condition = Math.Min(1f, (float)c.health / Math.Max(1, profile.max_health));
            if (c.sickness != (int)CompanionSicknessState.Healthy) condition *= 0.6f;
            return (int)Math.Round(profile.morale_support_bp * bondFactor * condition);
        }

        /// <summary>Bounded grief-shock payload for a companion death (§5.15):
        /// the MORALE AUTHORITY applies this; this system never writes morale.</summary>
        public int GetGriefMoraleDeltaBp(string companionId)
        {
            var c = Companion(companionId);
            if (c == null) return 0;
            float bondFactor = (float)c.bond / MaxBond;
            return (int)Math.Round(GriefMoraleFloorBp + (GriefMoraleShockMaxBp - GriefMoraleFloorBp) * bondFactor);
        }

        // ── Save (§5.18) ───────────────────────────────────────────────

        public CompanionSystemState CaptureState()
        {
            var copy = new CompanionSystemState
            {
                last_tick_day = _state.last_tick_day,
                companions = new List<CompanionState>(_state.companions.Count)
            };
            foreach (var c in _state.companions)
            {
                if (c == null) continue;
                copy.companions.Add(new CompanionState
                {
                    companion_id = c.companion_id, species_id = c.species_id, name = c.name,
                    assigned_survivor_id = c.assigned_survivor_id, role = c.role,
                    training_progress = c.training_progress, training_level = c.training_level,
                    bond = c.bond, health = c.health, hunger = c.hunger, sickness = c.sickness,
                    last_fed_day = c.last_fed_day, tamed_day = c.tamed_day,
                    on_expedition = c.on_expedition, alive = c.alive
                });
            }
            return copy;
        }

        public void RestoreState(CompanionSystemState? state)
        {
            if (state == null) return;
            _state = new CompanionSystemState
            {
                last_tick_day = state.last_tick_day,
                companions = new List<CompanionState>(state.companions?.Count ?? 0)
            };
            if (state.companions != null)
                foreach (var c in state.companions)
                    if (c != null)
                        _state.companions.Add(new CompanionState
                        {
                            companion_id = c.companion_id, species_id = c.species_id, name = c.name,
                            assigned_survivor_id = c.assigned_survivor_id, role = c.role,
                            training_progress = c.training_progress, training_level = c.training_level,
                            bond = c.bond, health = c.health, hunger = c.hunger, sickness = c.sickness,
                            last_fed_day = c.last_fed_day, tamed_day = c.tamed_day,
                            on_expedition = c.on_expedition, alive = c.alive
                        });
        }
    }

    /// <summary>
    /// Engine-agnostic loader for companion_animals.json with load-time
    /// validation: duplicate species ids, snake_case ids, bounded ranges,
    /// role-tag vocabulary, fallback item id shape. Errors collected, never
    /// thrown. Rows must reference wildlife species ids — the runtime
    /// additionally checks KnownSpeciesCheck at registration (§5.1).
    /// </summary>
    public static class CompanionAnimalCatalogLoader
    {
        public const string FileName = "companion_animals.json";
        public const int CurrentSchemaVersion = 1;

        public static readonly IReadOnlyList<string> AcceptedRoleTags = new[] { "guard", "pack", "morale" };

        public const int MaxFoodPerDay = 10;
        public const int MaxHealth = 200;
        public const int MaxTrainability = 10;
        public const int MaxBondRate = 10;
        public const int MaxGuardRating = 100;
        public const int MaxPackCapacityKg = 60;
        public const int MaxMoraleSupportBp = 500;
        public const int MaxDiseaseResistance = 10;

        private static readonly Regex SnakeCase = new Regex("^[a-z0-9_]+$", RegexOptions.Compiled);

        public static CompanionCatalogLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new CompanionCatalogLoadResult();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("loader requires dataDir, IFileIO and IJsonSerializer");
                return result;
            }

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add("catalog file missing: " + FileName);
                return result;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.Errors.Add("catalog file empty: " + FileName);
                return result;
            }

            CompanionCatalogRoot root;
            try
            {
                root = json.Deserialize<CompanionCatalogRoot>(raw);
            }
            catch (Exception e)
            {
                result.Errors.Add("catalog malformed JSON: " + e.Message);
                return result;
            }
            if (root == null)
            {
                result.Errors.Add("catalog parsed to null");
                return result;
            }
            if (root.schema_version > CurrentSchemaVersion)
            {
                result.Errors.Add($"schema_version {root.schema_version} newer than supported {CurrentSchemaVersion}");
                return result;
            }
            if (root.schema_version < 1)
            {
                result.Errors.Add($"schema_version {root.schema_version} invalid");
                return result;
            }
            if (root.companions == null || root.companions.Count == 0)
            {
                result.Errors.Add("catalog has no companion rows");
                return result;
            }

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < root.companions.Count; i++)
            {
                var def = root.companions[i];
                string at = $"companions[{i}]";
                if (def == null)
                {
                    result.Errors.Add(at + ": null row");
                    continue;
                }

                if (string.IsNullOrEmpty(def.species_id) || !SnakeCase.IsMatch(def.species_id)
                    || !def.species_id.StartsWith("species_", StringComparison.Ordinal))
                {
                    result.Errors.Add(at + ": species_id must be snake_case with species_ prefix");
                    continue;
                }
                if (!seenIds.Add(def.species_id))
                {
                    result.Errors.Add(at + ": duplicate species_id " + def.species_id);
                    continue;
                }
                if (string.IsNullOrWhiteSpace(def.display_name))
                    result.Errors.Add(at + " (" + def.species_id + "): display_name required");
                if (def.role_tags == null || def.role_tags.Count == 0)
                    result.Errors.Add(at + " (" + def.species_id + "): role_tags required");
                else
                    foreach (var tag in def.role_tags)
                        if (!AcceptedRoleTags.Contains(tag))
                            result.Errors.Add(at + " (" + def.species_id + "): unknown role_tag " + tag);

                if (def.base_food_per_day < 0 || def.base_food_per_day > MaxFoodPerDay)
                    result.Errors.Add(at + " (" + def.species_id + "): base_food_per_day out of range [0," + MaxFoodPerDay + "]");
                if (def.max_health <= 0 || def.max_health > MaxHealth)
                    result.Errors.Add(at + " (" + def.species_id + "): max_health out of range (0," + MaxHealth + "]");
                if (def.trainability < 0 || def.trainability > MaxTrainability)
                    result.Errors.Add(at + " (" + def.species_id + "): trainability out of range [0," + MaxTrainability + "]");
                if (def.bond_rate < 0 || def.bond_rate > MaxBondRate)
                    result.Errors.Add(at + " (" + def.species_id + "): bond_rate out of range [0," + MaxBondRate + "]");
                if (def.guard_rating < 0 || def.guard_rating > MaxGuardRating)
                    result.Errors.Add(at + " (" + def.species_id + "): guard_rating out of range [0," + MaxGuardRating + "]");
                if (def.pack_capacity_kg < 0 || def.pack_capacity_kg > MaxPackCapacityKg)
                    result.Errors.Add(at + " (" + def.species_id + "): pack_capacity_kg out of range [0," + MaxPackCapacityKg + "]");
                if (def.morale_support_bp < 0 || def.morale_support_bp > MaxMoraleSupportBp)
                    result.Errors.Add(at + " (" + def.species_id + "): morale_support_bp out of range [0," + MaxMoraleSupportBp + "]");
                if (def.disease_resistance < 0 || def.disease_resistance > MaxDiseaseResistance)
                    result.Errors.Add(at + " (" + def.species_id + "): disease_resistance out of range [0," + MaxDiseaseResistance + "]");

                if (def.fallback_food_item_ids == null || def.fallback_food_item_ids.Count == 0)
                    result.Errors.Add(at + " (" + def.species_id + "): fallback_food_item_ids required");

                result.Companions.Add(def);
            }

            return result;
        }

        /// <summary>Index a successful load result into the runtime catalog.</summary>
        public static List<CompanionSpeciesProfile> ToProfiles(CompanionCatalogLoadResult result)
        {
            var list = new List<CompanionSpeciesProfile>();
            if (result == null) return list;
            foreach (var def in result.Companions)
                if (def != null && !string.IsNullOrEmpty(def.species_id)) list.Add(def);
            return list;
        }
    }
}
