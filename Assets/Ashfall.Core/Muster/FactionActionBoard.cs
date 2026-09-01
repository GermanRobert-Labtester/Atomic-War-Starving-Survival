using System;
using System.Collections.Generic;
using Ashfall.Core.Flags;
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>Host-applied item transfer for a resolved action's choice. Core never
    /// reaches into an inventory; the host decides whether the shelter can pay.</summary>
    public interface IFactionActionItemSink
    {
        /// <summary>Apply the item gain/loss. Returns false when it cannot be applied
        /// (the resolution record still stands; the host surfaces the refusal).</summary>
        bool Deliver(string itemId, int amount);
    }

    /// <summary>An action the player may act on right now, with its standing-band
    /// variant already resolved.</summary>
    public class FactionActionOffer
    {
        public FactionActionDefinition Definition;
        public string Band = FactionActionBands.Neutral;
        public string VariantText = string.Empty;
    }

    /// <summary>One persisted resolution — the board's political memory and its
    /// idempotence guard (reload can never re-apply a once-only action).</summary>
    public class FactionActionResolutionRecord
    {
        public string actionId = string.Empty;
        public string choiceId = string.Empty;
        public string band = string.Empty;
        public int day;
    }

    /// <summary>Serialized board state (save/load safe; additive fields tolerate
    /// old saves that predate the board).</summary>
    public class FactionActionBoardState
    {
        public string systemId = FactionActionBoard.SystemId;
        public List<FactionActionResolutionRecord> resolved = new List<FactionActionResolutionRecord>();
        public List<string> producedFlags = new List<string>();
    }

    /// <summary>
    /// Plan 25 peacetime faction-action runtime. Deterministically selects authored
    /// actions from muster_faction_actions.json by day window, flag gates,
    /// once/cooldown history, and each faction system's OWN standing scalar
    /// (guild/hydro trust, raider aggression/visibility, camp formed/members/lockout).
    /// No new standing store, no RNG: availability order is ordinal by action id,
    /// variant selection is first authored band match with a neutral fallback.
    /// Engine-agnostic; hosts only present it and apply item effects via IFactionActionItemSink.
    /// </summary>
    public class FactionActionBoard
    {
        public const string SystemId = "faction_action_board";

        public const string FactionScavengerGuild = "faction_scavenger_guild";
        public const string FactionHydroBarons = "faction_hydro_barons";
        public const string FactionIronRaiders = "faction_iron_raiders";
        public const string FactionDeserterCoalition = "faction_deserter_coalition";

        private readonly FactionActionBoardState _state;
        private readonly ScavengerGuildSystem _guild;
        private readonly HydroBaronsSystem _hydro;
        private readonly IronRaidersSystem _raiders;
        private readonly CoalitionCampSystem _camp;
        private readonly IFlagLedger _ledger;
        private readonly List<FactionActionDefinition> _catalog = new List<FactionActionDefinition>();

        public event Action<FactionActionResolutionRecord> OnActionResolved;
        public event Action<FactionActionBoardState> OnStateChanged;

        public FactionActionBoard(
            ScavengerGuildSystem guild = null,
            HydroBaronsSystem hydro = null,
            IronRaidersSystem raiders = null,
            CoalitionCampSystem camp = null,
            FactionActionBoardState state = null,
            IFlagLedger ledger = null)
        {
            _guild = guild;
            _hydro = hydro;
            _raiders = raiders;
            _camp = camp;
            _ledger = ledger;
            _state = state ?? new FactionActionBoardState();
            if (_state.systemId != SystemId) _state.systemId = SystemId;
            if (_state.resolved == null) _state.resolved = new List<FactionActionResolutionRecord>();
            if (_state.producedFlags == null) _state.producedFlags = new List<string>();
        }

        public FactionActionBoardState State => _state;
        public IReadOnlyList<FactionActionDefinition> Catalog => _catalog;

        // ── Catalog ────────────────────────────────────────────────────

        /// <summary>Replace the authored action set (dedup by id, ordinal order).</summary>
        public void SetCatalog(IEnumerable<FactionActionDefinition> definitions)
        {
            _catalog.Clear();
            if (definitions == null) { RaiseChanged(); return; }
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var def in definitions)
            {
                if (def == null || string.IsNullOrEmpty(def.id) || !seen.Add(def.id)) continue;
                _catalog.Add(def);
            }
            _catalog.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
            RaiseChanged();
        }

        public FactionActionDefinition FindDefinition(string actionId)
        {
            for (int i = 0; i < _catalog.Count; i++)
                if (_catalog[i].id == actionId) return _catalog[i];
            return null;
        }

        // ── Standing bands (each faction's own scalar; thresholds per
        //    docs/factions/MUSTER_FACTION_RUNTIME_CONTRACT.md §1) ──────

        public static string BandForTrust(float trust)
        {
            if (trust <= 0f) return FactionActionBands.Hostile;
            if (trust < 4f) return FactionActionBands.Poor;
            if (trust < 9f) return FactionActionBands.Neutral;
            if (trust < 15f) return FactionActionBands.Good;
            return FactionActionBands.Allied;
        }

        public string ComputeBand(string factionId)
        {
            switch (factionId)
            {
                case FactionScavengerGuild:
                    return BandForTrust(_guild != null ? _guild.Trust : 0f);
                case FactionHydroBarons:
                    return BandForTrust(_hydro != null ? _hydro.State.trust : 0f);
                case FactionIronRaiders:
                {
                    float a = _raiders != null ? _raiders.AggressionLevel : 0f;
                    float v = _raiders != null ? _raiders.State.shelterVisibility : 1f;
                    if (a >= 0.75f) return FactionActionBands.Hostile;
                    if (a >= 0.5f) return FactionActionBands.Poor;
                    if (a >= 0.25f) return FactionActionBands.Neutral;
                    if (a < 0.1f && v <= 0.3f) return FactionActionBands.Allied;
                    if (a < 0.25f && v < 0.3f) return FactionActionBands.Good;
                    return FactionActionBands.Neutral;
                }
                case FactionDeserterCoalition:
                {
                    bool formed = _camp != null && _camp.Formed;
                    int lockout = _camp != null ? _camp.GarrisonLockoutRisk : 0;
                    int members = _camp != null ? _camp.MembersRallied : 0;
                    string strategy = _camp != null ? _camp.ChosenStrategy : string.Empty;
                    if (!formed) return FactionActionBands.Hostile;
                    if (strategy == "D" || lockout >= 60) return FactionActionBands.Hostile;
                    if (lockout >= 30) return FactionActionBands.Poor;
                    if (members >= 15) return FactionActionBands.Allied;
                    if (members >= 12) return FactionActionBands.Good;
                    return FactionActionBands.Neutral;
                }
                default:
                    return FactionActionBands.Neutral;
            }
        }

        // ── Availability ───────────────────────────────────────────────

        /// <summary>Actions the player may act on today, ordinal by id. Coalition
        /// actions additionally require a formed camp.</summary>
        public List<FactionActionOffer> AvailableActions(int day)
        {
            var offers = new List<FactionActionOffer>();
            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                if (def.factionId == FactionDeserterCoalition && !(_camp != null && _camp.Formed))
                    continue;
                if (day < def.minDay) continue;
                if (def.maxDay > 0 && day > def.maxDay) continue;
                if (def.once && LastResolvedDay(def.id) >= 0) continue;
                if (!CooldownElapsed(def, day)) continue;
                if (!FlagsGateOpen(def)) continue;

                string band = ComputeBand(def.factionId);
                FactionActionVariant variant = SelectVariant(def, band);
                if (variant == null) continue;
                offers.Add(new FactionActionOffer
                {
                    Definition = def,
                    Band = band,
                    VariantText = variant.text
                });
            }
            return offers;
        }

        public FactionActionOffer FindOffer(string actionId, int day)
        {
            var offers = AvailableActions(day);
            for (int i = 0; i < offers.Count; i++)
                if (offers[i].Definition.id == actionId) return offers[i];
            return null;
        }

        /// <summary>First authored variant whose band matches; falls back to the
        /// neutral variant, then to a single-variant definition; null if malformed.</summary>
        public static FactionActionVariant SelectVariant(FactionActionDefinition def, string band)
        {
            if (def == null || def.variants.Count == 0) return null;
            for (int i = 0; i < def.variants.Count; i++)
                if (def.variants[i].band == band) return def.variants[i];
            for (int i = 0; i < def.variants.Count; i++)
                if (def.variants[i].band == FactionActionBands.Neutral) return def.variants[i];
            return def.variants.Count == 1 ? def.variants[0] : null;
        }

        // ── Resolution ─────────────────────────────────────────────────

        /// <summary>Resolve a choice on an available action. Re-validates availability,
        /// applies effects through each faction system's own seams, records the
        /// resolution once (idempotence guard for once/cooldown and reload replay).</summary>
        public bool Resolve(string actionId, string choiceId, int day, IFactionActionItemSink itemSink = null)
        {
            var def = FindDefinition(actionId);
            if (def == null) return false;
            if (def.factionId == FactionDeserterCoalition && !(_camp != null && _camp.Formed))
                return false;
            if (day < def.minDay) return false;
            if (def.maxDay > 0 && day > def.maxDay) return false;
            if (def.once && LastResolvedDay(def.id) >= 0) return false;
            if (!CooldownElapsed(def, day)) return false;
            if (!FlagsGateOpen(def)) return false;

            string band = ComputeBand(def.factionId);
            var variant = SelectVariant(def, band);
            if (variant == null) return false;
            FactionActionChoice choice = null;
            for (int i = 0; i < variant.choices.Count; i++)
                if (variant.choices[i].choiceId == choiceId) { choice = variant.choices[i]; break; }
            if (choice == null) return false;

            var fx = choice.effects;
            if (fx != null)
            {
                switch (def.factionId)
                {
                    case FactionScavengerGuild:
                        _guild?.AdjustTrust(fx.trustDelta);
                        break;
                    case FactionHydroBarons:
                        _hydro?.AdjustTrust(fx.trustDelta);
                        break;
                    case FactionIronRaiders:
                        if (_raiders != null && Math.Abs(fx.aggressionDelta) > 0.0001f)
                            _raiders.SetAggressionLevel(_raiders.AggressionLevel + fx.aggressionDelta);
                        break;
                    case FactionDeserterCoalition:
                        if (_camp != null)
                        {
                            if (fx.membersDelta != 0) _camp.AdjustMembers(fx.membersDelta);
                            if (fx.lockoutDelta != 0) _camp.AdjustLockoutRisk(fx.lockoutDelta);
                        }
                        break;
                }
                if (!string.IsNullOrEmpty(fx.itemId) && fx.itemAmount != 0)
                    itemSink?.Deliver(fx.itemId, fx.itemAmount);
                if (fx.flags != null)
                    for (int i = 0; i < fx.flags.Count; i++)
                        ProduceFlag(fx.flags[i], day);
            }

            var record = new FactionActionResolutionRecord
            {
                actionId = def.id,
                choiceId = choiceId,
                band = band,
                day = day
            };
            _state.resolved.Add(record);
            OnActionResolved?.Invoke(record);
            RaiseChanged();
            return true;
        }

        // ── History / flags ────────────────────────────────────────────

        /// <summary>Last day the action was resolved, or −1 when never resolved.</summary>
        public int LastResolvedDay(string actionId)
        {
            int last = -1;
            for (int i = 0; i < _state.resolved.Count; i++)
                if (_state.resolved[i].actionId == actionId && _state.resolved[i].day > last)
                    last = _state.resolved[i].day;
            return last;
        }

        public bool HasResolved(string actionId, string choiceId = null)
        {
            for (int i = 0; i < _state.resolved.Count; i++)
            {
                var r = _state.resolved[i];
                if (r.actionId != actionId) continue;
                if (choiceId == null || r.choiceId == choiceId) return true;
            }
            return false;
        }

        public bool IsFlagSet(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return false;
            if (_state.producedFlags.Contains(flagId)) return true;
            return _ledger != null && _ledger.IsSet(flagId);
        }

        private void ProduceFlag(string flagId, int day)
        {
            if (string.IsNullOrEmpty(flagId)) return;
            if (!_state.producedFlags.Contains(flagId))
                _state.producedFlags.Add(flagId);
            _ledger?.Set(flagId, originSystem: SystemId, day: day);
        }

        private bool FlagsGateOpen(FactionActionDefinition def)
        {
            for (int i = 0; i < def.requiresFlags.Count; i++)
                if (!IsFlagSet(def.requiresFlags[i])) return false;
            for (int i = 0; i < def.forbidsFlags.Count; i++)
                if (IsFlagSet(def.forbidsFlags[i])) return false;
            return true;
        }

        private bool CooldownElapsed(FactionActionDefinition def, int day)
        {
            if (def.cooldownDays <= 0) return true;
            int last = LastResolvedDay(def.id);
            if (last < 0) return true;
            return day >= last + def.cooldownDays;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public FactionActionBoardState CaptureState()
        {
            var copy = new FactionActionBoardState { systemId = SystemId };
            var ordered = new List<FactionActionResolutionRecord>(_state.resolved);
            ordered.Sort((a, b) =>
            {
                int byDay = a.day.CompareTo(b.day);
                if (byDay != 0) return byDay;
                int byAction = string.CompareOrdinal(a.actionId, b.actionId);
                if (byAction != 0) return byAction;
                return string.CompareOrdinal(a.choiceId, b.choiceId);
            });
            for (int i = 0; i < ordered.Count; i++)
            {
                copy.resolved.Add(new FactionActionResolutionRecord
                {
                    actionId = ordered[i].actionId,
                    choiceId = ordered[i].choiceId,
                    band = ordered[i].band,
                    day = ordered[i].day
                });
            }
            var flags = new List<string>(_state.producedFlags);
            flags.Sort(StringComparer.Ordinal);
            copy.producedFlags = flags;
            return copy;
        }

        public void RestoreState(FactionActionBoardState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.resolved.Clear();
            if (saved.resolved != null)
            {
                for (int i = 0; i < saved.resolved.Count; i++)
                {
                    var r = saved.resolved[i];
                    if (r == null || string.IsNullOrEmpty(r.actionId)) continue;
                    _state.resolved.Add(new FactionActionResolutionRecord
                    {
                        actionId = r.actionId,
                        choiceId = r.choiceId ?? string.Empty,
                        band = r.band ?? string.Empty,
                        day = r.day
                    });
                }
            }
            _state.producedFlags.Clear();
            if (saved.producedFlags != null)
                for (int i = 0; i < saved.producedFlags.Count; i++)
                    if (!string.IsNullOrEmpty(saved.producedFlags[i]))
                        _state.producedFlags.Add(saved.producedFlags[i]);
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
