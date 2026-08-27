using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Combat
{
    /// <summary>Stance numeric modifiers applied to combat values.</summary>
    public struct StanceMods
    {
        public float Accuracy;      // player fire accuracy
        public float Damage;        // player fire damage
        public float Defense;       // reduces enemy accuracy (0..1)
        public float AmmoUse;       // rounds per burst multiplier
        public float Degrade;       // weapon degradation multiplier
        public float JamRisk;       // jam chance multiplier
        public float Noise;         // encounter exposure / noise
        public float Mobility;      // chance to successfully flee
        public float MoraleDelta;   // stance morale impact
        public bool CanFlee;        // may this stance retreat?
        public bool DeathIsInstant; // last stand: 0 HP = instant death + mutual kill
    }

    /// <summary>
    /// Engine-agnostic tactical combat authority. Wires weapon condition/jam,
    /// ballistic resolution, tactical stances and lanes, suppression, flanking,
    /// bleed-out, last-stand, retreat, injury/morale/loot/journal hooks, and
    /// save/load. Every roll consumes the injected ISeededRng — the host owns
    /// seeding, so replaying identical commands from an identical state with a
    /// fresh rng of the same seed reproduces identical outcomes.
    /// </summary>
    public partial class TacticalCombatSystem
    {
        public const string SystemId = "combat_system";

        public const int DefaultBleedTurns = 3;
        public const int DefaultSuppressDuration = 1;
        public const int MaxRicochetBounces = BallisticsSystem.MaxRicochetCount;

        private CombatState _state = new CombatState();
        private readonly WeaponConditionSystem _condition = new WeaponConditionSystem();
        private readonly Dictionary<string, CombatPerks> _perksBySurvivor = new Dictionary<string, CombatPerks>(StringComparer.Ordinal);
        private CombatHostPorts _ports;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<CombatState> OnStateChanged;
        public event Action<CombatState, CombatEvent> OnCombatEvent;
        public event Action<CombatState> OnEncounterEnded;

        public CombatState State => _state;
        public CombatHostPorts Ports { get => _ports; set => _ports = value; }

        public TacticalCombatSystem(CombatState? state = null, CombatHostPorts? ports = null)
        {
            if (state != null) _state = state;
            _ports = ports ?? new CombatHostPorts();
            CombatCatalog.SeedDefaults();
        }

        /// <summary>The perks tracker for a survivor (lazily created, save-safe).</summary>
        public CombatPerks? PerksFor(string survivorId, int seed)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            if (!_perksBySurvivor.TryGetValue(survivorId, out var p))
            {
                p = new CombatPerks(seed);
                _perksBySurvivor[survivorId] = p;
            }
            return p;
        }

        // ══ Stance table ══════════════════════════════════════════════════

        private static readonly TacticalStance[] s_allStances = (TacticalStance[])Enum.GetValues(typeof(TacticalStance));

        public static string StanceId(TacticalStance s) => "combat_stance_" + s.ToString().ToLowerInvariant();

        public static bool TryParseStance(string id, out TacticalStance stance)
        {
            stance = TacticalStance.HoldPosition;
            if (string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < s_allStances.Length; i++)
            {
                TacticalStance s = s_allStances[i];
                if (string.Equals(id, StanceId(s), StringComparison.OrdinalIgnoreCase))
                {
                    stance = s;
                    return true;
                }
            }
            return false;
        }

        public static StanceMods GetStanceMods(TacticalStance s)
        {
            switch (s)
            {
                case TacticalStance.HoldPosition:
                    return new StanceMods { Accuracy = 1.0f, Damage = 1.0f, Defense = 0.15f, AmmoUse = 1.0f, Degrade = 1.0f, JamRisk = 1.0f, Noise = 1.0f, Mobility = 0.75f, MoraleDelta = 0f, CanFlee = true };
                case TacticalStance.Advance:
                    return new StanceMods { Accuracy = 1.15f, Damage = 1.10f, Defense = 0f, AmmoUse = 1.3f, Degrade = 1.15f, JamRisk = 1.1f, Noise = 1.3f, Mobility = 0.6f, MoraleDelta = 2f, CanFlee = true };
                case TacticalStance.SuppressiveFire:
                    return new StanceMods { Accuracy = 0.6f, Damage = 0.6f, Defense = 0.05f, AmmoUse = 2.0f, Degrade = 1.25f, JamRisk = 1.2f, Noise = 1.5f, Mobility = 0.5f, MoraleDelta = 1f, CanFlee = true };
                case TacticalStance.Retreat:
                    return new StanceMods { Accuracy = 0.6f, Damage = 0.8f, Defense = 0f, AmmoUse = 1.0f, Degrade = 1.0f, JamRisk = 1.0f, Noise = 1.0f, Mobility = 0.9f, MoraleDelta = -2f, CanFlee = true };
                case TacticalStance.LastStand:
                    return new StanceMods { Accuracy = 2.0f, Damage = 2.0f, Defense = 0f, AmmoUse = 1.0f, Degrade = 1.3f, JamRisk = 1.0f, Noise = 1.2f, Mobility = 0f, MoraleDelta = 4f, CanFlee = false, DeathIsInstant = true };
                default:
                    return new StanceMods { Accuracy = 1f, Damage = 1f, Defense = 0f, AmmoUse = 1f, Degrade = 1f, JamRisk = 1f, Noise = 1f, Mobility = 0.75f, MoraleDelta = 0f, CanFlee = true };
            }
        }

        // ══ Encounter lifecycle ══════════════════════════════════════════

        /// <summary>
        /// Begin a combat encounter. Player survivors carry their weapons;
        /// enemies are generated from a count/health template. Returns structured
        /// failure (false) instead of throwing on invalid inputs.
        /// </summary>
        public bool BeginEncounter(
            string encounterId,
            string expeditionId,
            string locationId,
            string locationName,
            int day,
            int seed,
            IReadOnlyList<CombatantState> players,
            IReadOnlyList<WeaponInstanceState> playerWeapons,
            int enemyCount,
            float enemyHealth,
            ILog? log = null)
        {
            if (string.IsNullOrEmpty(encounterId)
                || players == null || players.Count == 0
                || enemyCount < 1)
                return false;

            _state = new CombatState
            {
                SaveVersion = CombatState.CurrentSaveVersion,
                EncounterId = encounterId,
                ExpeditionId = expeditionId ?? string.Empty,
                LocationId = locationId ?? string.Empty,
                LocationName = locationName ?? string.Empty,
                Day = day,
                Seed = seed,
                Turn = 1,
                Phase = (int)CombatPhase.PlayerTurn,
                PlayerStance = StanceId(TacticalStance.HoldPosition),
                RoundNumber = 1,
                Resolved = false
            };

            // Deep-copy players into state with stable ids.
            for (int i = 0; i < players.Count; i++)
                _state.Combatants.Add(CloneCombatant(players[i]));

            // Register player weapons.
            if (playerWeapons != null)
            {
                for (int i = 0; i < playerWeapons.Count; i++)
                    _state.Weapons.Add(CloneWeapon(playerWeapons[i]));
            }

            // Link each player combatant to its weapon (first unassigned).
            AssignPlayerWeapons();

            // Generate enemies deterministically.
            for (int i = 0; i < enemyCount; i++)
            {
                _state.Combatants.Add(new CombatantState
                {
                    Id = "enemy_" + encounterId + "_" + i,
                    Name = "Raider",
                    IsPlayer = false,
                    FactionId = "faction_raiders",
                    Lane = (int)(i % 3),
                    Health = enemyHealth,
                    MaxHealth = enemyHealth,
                    ArmorRating = 0f,
                    CoverRating = 0.3f // raiders use rubble cover
                });
            }

            // Seed ammo for player weapons that lack a live host catalog.
            SeedWeaponAmmo();

            AddEvent("encounter_start", encounterId, "Combat begins at " + (string.IsNullOrEmpty(locationName) ? locationId : locationName));
            OnStateChanged?.Invoke(_state);
            return true;
        }

        private void AssignPlayerWeapons()
        {
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (!c.IsPlayer || c.IsDowned || !string.IsNullOrEmpty(c.WeaponInstanceId))
                    continue;
                // find next unassigned weapon owned by this survivor, else any free
                for (int w = 0; w < _state.Weapons.Count; w++)
                {
                    var wp = _state.Weapons[w];
                    if (wp.OwnerSurvivorId != c.SurvivorId || !string.IsNullOrEmpty(wp.OwnerCombatantId))
                        continue;
                    c.WeaponInstanceId = wp.InstanceId;
                    wp.OwnerCombatantId = c.Id;
                    break;
                }
            }
        }

        private void SeedWeaponAmmo()
        {
            for (int i = 0; i < _state.Weapons.Count; i++)
            {
                var w = _state.Weapons[i];
                var def = CombatCatalog.GetWeapon(w.WeaponId);
                if (def != null && string.IsNullOrEmpty(w.AmmoId)) w.AmmoId = def.caliber;
                if (w.AmmoRemaining <= 0 && def != null) w.AmmoRemaining = def.burst * 10;
            }
        }

        private TacticalStance CurrentStance()
        {
            return TryParseStance(_state.PlayerStance, out var s) ? s : TacticalStance.HoldPosition;
        }

        private void AddEvent(string kind, string targetId, string detail, float value = 0f)
        {
            var e = new CombatEvent
            {
                Kind = kind,
                Day = _state.Day,
                Turn = _state.Turn,
                SubjectId = _state.EncounterId,
                TargetId = targetId ?? string.Empty,
                Detail = detail,
                Value = value
            };
            _state.Events.Add(e);
            OnCombatEvent?.Invoke(_state, e);
        }

        private void Notify() => OnStateChanged?.Invoke(_state);
    }
}
