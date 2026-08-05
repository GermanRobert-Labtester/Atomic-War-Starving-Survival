using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Economy
{
    /// <summary>
    /// Phase-driven item values + per-faction TrustLevel matrix. Values combine
    /// TradeEconomy phase multipliers with global supply/demand pressure. Trust
    /// (-100..100) gates trade / rob / intel / hatch raids. EventRunner choices
    /// with FactionId + TrustDelta (or RelationshipDelta) mutate trust.
    /// </summary>
    public class DynamicEconomySystem
    {
        private PersonalQuestSystem _personalQuests;
        private System.Func<System.Collections.Generic.IReadOnlyList<Survivor>> _getSurvivors;

        /// <summary>Prompt #236 — Demagogue: FactionTrust floor 0 + tribute drops.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            System.Func<System.Collections.Generic.IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }


        public const float MinTrust = -100f;
        public const float MaxTrust = 100f;
        public const float DefaultRaidThreshold = -50f;
        /// <summary>Consecutive hatch repels before a faction auto-surrenders.</summary>
        public const int RepelsForAutoSurrender = 2;
        /// <summary>Trust lift above raid threshold after surrender.</summary>
        public const float SurrenderTrustBuffer = 18f;
        /// <summary>Aggression multiplier applied on surrender (0..1 scale factor).</summary>
        public const float SurrenderAggressionScale = 0.45f;
        /// <summary>
        /// After a parley / surrender, player sell prices improve by this fraction
        /// (on top of trust). Softens the table without stacking forever — only
        /// while <see cref="HasSurrendered"/> is true.
        /// </summary>
        public const float ParleyBarterSellBonus = 0.12f;
        /// <summary>After parley / surrender, player buy prices drop by this fraction.</summary>
        public const float ParleyBarterBuyDiscount = 0.10f;

        /// <summary>
        /// Cult of the Glow (#16 polish): when any party member has Acute Radiation
        /// Syndrome, player buy prices on high-value gear drop by this fraction
        /// (they revere the dying and open the cache).
        /// </summary>
        public const float ArsReverenceBuyDiscount = 0.35f;

        /// <summary>
        /// Trust-inversion factions (Cult of the Glow) only open after the exchange.
        /// Pre-Day-30 they refuse trade / intel; day provider null keeps them active
        /// for pure unit tests that never set a day clock.
        /// </summary>
        public const int CultActivationDay = 30;

        /// <summary>
        /// Fallback barter multiplier for <see cref="ItemType.IrradiatedWater"/> when a
        /// trust-inversion faction has a non-positive SO multiplier.
        /// </summary>
        public const float DefaultTrustInversionIrradiatedWaterMult = 12f;

        /// <summary>Supply pressure clamp: demand mult stays in [min, max].</summary>
        public const float MinDemandMult = 0.25f;
        public const float MaxDemandMult = 4f;

        private readonly Dictionary<string, FactionSO> _factions = new Dictionary<string, FactionSO>();
        private readonly Dictionary<string, float> _trust = new Dictionary<string, float>();
        /// <summary>Per item-id demand pressure. 1 = neutral; &gt;1 scarce; &lt;1 surplus.</summary>
        private readonly Dictionary<string, float> _demand = new Dictionary<string, float>();
        /// <summary>Runtime aggression override (0..1). Absent = use FactionSO.raidAggression.</summary>
        private readonly Dictionary<string, float> _aggressionOverride = new Dictionary<string, float>();
        private readonly Dictionary<string, int> _successionGeneration = new Dictionary<string, int>();
        private readonly Dictionary<string, string> _leaderName = new Dictionary<string, string>();
        private readonly Dictionary<string, int> _consecutiveRepels = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _totalRaidsLaunched = new Dictionary<string, int>();
        private readonly Dictionary<string, bool> _hasSurrendered = new Dictionary<string, bool>();

        /// <summary>
        /// Barter-only mode: traders will only accept offers whose items are
        /// in <see cref="_barterOnlyAcceptedItemIds"/>. Set by the Day-30
        /// Flashpoint Choreographer (days 25-29 buildup) so the trader panic
        /// reads as a diegetic refusal, not a UI tooltip. Cleared by restore
        /// if needed; not auto-expired (choreographer decides).
        /// </summary>
        private readonly HashSet<string> _barterOnlyAcceptedItemIds = new HashSet<string>();
        private bool _barterOnlyMode;

        private Func<WorldPhase> _getPhase;
        private Shelter.Shelter _shelter;
        private System.Random _rng;
        private HatchDefenseSystem _hatchDefense;
        private Func<int> _getDay;
        /// <summary>
        /// Living-party average RadiationDose (0..100). Used by trust-inversion
        /// factions (Cult of the Glow). Null = inversion falls back to stored trust.
        /// </summary>
        private Func<float> _getPartyAverageRadiation;
        /// <summary>
        /// True when any living party member has Acute Radiation Syndrome.
        /// Cult reverence path (#16 polish). Null = false.
        /// </summary>
        private Func<bool> _getPartyHasArs;
        /// <summary>
        /// True when the party wears intact hazmat / full suit. Cult contempt path
        /// for healthy sealed blood (#16 polish). Null = false.
        /// </summary>
        private Func<bool> _getPartyIntactHazmat;
        /// <summary>
        /// Last sampled party avg radiation for trust-inversion raid cascade.
        /// NaN = never sampled (first <see cref="NotifyPartyRadiationChanged"/> only seeds).
        /// </summary>
        private float _lastObservedPartyRadiation = float.NaN;

        public event Action<string, float, float> OnTrustChanged; // factionId, old, new
        public event Action<WorldPhase> OnEconomyPhaseChanged;
        public event Action<FactionRaidResult> OnRaidResolved;
        public event Action<FactionSuccessionResult> OnFactionSuccession;
        public event Action<FactionSurrenderResult> OnFactionSurrender;
        public event Action OnEconomyChanged;
        /// <summary>Fired when barter-only mode flips (parameter: new value).</summary>
        public event Action<bool> OnBarterOnlyModeChanged;

        /// <summary>Systemic hatch defense (Prompt #33). Null-safe for pure trade tests.</summary>
        public HatchDefenseSystem HatchDefense => _hatchDefense;

        public WorldPhase CurrentPhase => _getPhase != null ? _getPhase() : WorldPhase.CivilWar;
        public bool BarterOnlyMode => _barterOnlyMode;
        public IReadOnlyCollection<string> BarterOnlyAcceptedItemIds => _barterOnlyAcceptedItemIds;

        public DynamicEconomySystem(
            Func<WorldPhase> getPhase = null,
            Shelter.Shelter shelter = null,
            System.Random rng = null)
        {
            _getPhase = getPhase ?? (() => WorldPhase.CivilWar);
            _shelter = shelter;
            _rng = rng ?? new System.Random(7);
        }

        public void SetPhaseProvider(Func<WorldPhase> getPhase)
        {
            _getPhase = getPhase ?? (() => WorldPhase.CivilWar);
        }

        public void SetShelter(Shelter.Shelter shelter) => _shelter = shelter;

        public void SetHatchDefense(HatchDefenseSystem hatchDefense) => _hatchDefense = hatchDefense;

        public void SetDayProvider(Func<int> getDay) => _getDay = getDay;

        /// <summary>
        /// Wire living-party average radiation (0..100) for trust-inversion factions.
        /// When unset, inversion factions use stored trust like everyone else.
        /// Resets the radiation-sample seed so the next
        /// <see cref="NotifyPartyRadiationChanged"/> only records a baseline.
        /// </summary>
        public void SetPartyRadiationProvider(Func<float> getPartyAverageRadiation)
        {
            _getPartyAverageRadiation = getPartyAverageRadiation;
            _lastObservedPartyRadiation = float.NaN;
        }

        /// <summary>
        /// Wire "any living party member has Acute Radiation Syndrome" for Cult
        /// reverence (#16 polish). Null / unset = false.
        /// </summary>
        public void SetPartyHasArsProvider(Func<bool> getPartyHasArs)
        {
            _getPartyHasArs = getPartyHasArs;
        }

        /// <summary>
        /// Wire "party wears intact hazmat / full suit" for Cult contempt
        /// (#16 polish). Null / unset = false.
        /// </summary>
        public void SetPartyIntactHazmatProvider(Func<bool> getPartyIntactHazmat)
        {
            _getPartyIntactHazmat = getPartyIntactHazmat;
        }

        public void NotifyPhaseChanged(WorldPhase phase)
        {
            OnEconomyPhaseChanged?.Invoke(phase);
            OnEconomyChanged?.Invoke();
        }

        /// <summary>
        /// Sample party average radiation and mirror <see cref="ModifyTrust"/>'s
        /// raid cascade for trust-inversion factions: when dose crosses a faction's
        /// <see cref="FactionSO.healthyRadiationCeiling"/> downward (was above,
        /// now at/below), call <see cref="TryLaunchRaid"/> once. First call only
        /// seeds the baseline — no raid. No-op without a radiation provider.
        /// Wire after dose ticks / anti-rad use (see GameBootstrap).
        /// </summary>
        public void NotifyPartyRadiationChanged()
        {
            float rad = GetPartyAverageRadiation();
            if (rad < 0f) return;

            float prev = _lastObservedPartyRadiation;
            _lastObservedPartyRadiation = rad;

            // First sample: establish baseline only (no false raid on load/boot).
            if (float.IsNaN(prev)) return;

            foreach (var kv in _factions)
            {
                var fac = kv.Value;
                if (fac == null || !fac.trustInversion || string.IsNullOrEmpty(fac.id))
                    continue;

                float ceiling = Mathf.Clamp(fac.healthyRadiationCeiling, 0f, 100f);
                // Downward cross through the healthy ceiling → fully hostile.
                if (prev > ceiling && rad <= ceiling)
                    TryLaunchRaid(fac.id);
            }
        }

        // -----------------------------------------------------------------
        // Factions / trust
        // -----------------------------------------------------------------

        public void RegisterFaction(FactionSO faction)
        {
            if (faction == null || string.IsNullOrEmpty(faction.id)) return;
            _factions[faction.id] = faction;
            if (!_trust.ContainsKey(faction.id))
                _trust[faction.id] = Mathf.Clamp(faction.startingTrust, MinTrust, MaxTrust);
            if (!_successionGeneration.ContainsKey(faction.id))
                _successionGeneration[faction.id] = 0;
            if (!_leaderName.ContainsKey(faction.id) || string.IsNullOrEmpty(_leaderName[faction.id]))
                _leaderName[faction.id] = faction.displayName ?? faction.id;
            if (!_consecutiveRepels.ContainsKey(faction.id))
                _consecutiveRepels[faction.id] = 0;
            if (!_totalRaidsLaunched.ContainsKey(faction.id))
                _totalRaidsLaunched[faction.id] = 0;
            if (!_hasSurrendered.ContainsKey(faction.id))
                _hasSurrendered[faction.id] = false;
        }

        public FactionSO GetFaction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return null;
            return _factions.TryGetValue(factionId, out var f) ? f : null;
        }

        public IReadOnlyDictionary<string, FactionSO> Factions => _factions;

        public float GetTrust(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return 0f;
            float t = _trust.TryGetValue(factionId, out float stored) ? stored : 0f;
            // #257 Hated: stored military trust floors at −100 while a Hated survivor lives.
            if (_personalQuests != null && _getSurvivors != null && IsMilitaryFaction(factionId))
            {
                var list = _getSurvivors();
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] != null && list[i].IsAlive
                            && _personalQuests.IsShotOnSightByMilitary(list[i]))
                            return Mathf.Min(t, PersonalQuestSystem.HatedMilitaryTrust);
                    }
                }
            }
            return t;
        }

        /// <summary>Military remnant factions that Hated Generals are shot on sight by.</summary>
        public static bool IsMilitaryFaction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return false;
            return string.Equals(factionId, FactionSO.Ids.MilitaryRemnants, StringComparison.Ordinal)
                   || factionId.IndexOf("military", StringComparison.OrdinalIgnoreCase) >= 0
                   || factionId.IndexOf("army", StringComparison.OrdinalIgnoreCase) >= 0
                   || factionId.IndexOf("militia", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Living-party average RadiationDose (0..100), or −1 when no provider is wired.
        /// </summary>
        public float GetPartyAverageRadiation()
        {
            if (_getPartyAverageRadiation == null) return -1f;
            return Mathf.Clamp(_getPartyAverageRadiation(), 0f, 100f);
        }

        /// <summary>
        /// True when the ARS provider reports any living party member with
        /// Acute Radiation Syndrome. False when unset.
        /// </summary>
        public bool PartyHasAcuteRadiationSyndrome()
        {
            return _getPartyHasArs != null && _getPartyHasArs();
        }

        /// <summary>
        /// True when the hazmat provider reports intact full-suit protection.
        /// False when unset.
        /// </summary>
        public bool PartyWearsIntactHazmat()
        {
            return _getPartyIntactHazmat != null && _getPartyIntactHazmat();
        }

        /// <summary>
        /// Whether a faction is available for trade / intel / raid pressure.
        /// Trust-inversion factions (Cult of the Glow) activate on
        /// <see cref="CultActivationDay"/> when a day provider is wired.
        /// Without a day provider they stay active (unit-test default).
        /// </summary>
        public bool IsFactionActive(string factionId)
        {
            var fac = GetFaction(factionId);
            if (fac == null) return false;
            if (!fac.trustInversion) return true;
            if (_getDay == null) return true;
            return _getDay() >= CultActivationDay;
        }

        /// <summary>
        /// Trust used for stance and barter. For <see cref="FactionSO.trustInversion"/>
        /// factions with a radiation provider, maps party dose to disposition:
        /// healthy (≤ ceiling) → hostile, highly irradiated (≥ floor) → friendly.
        /// #16 polish: ARS → MaxTrust (reverence); healthy + intact hazmat → MinTrust
        /// (contempt, including when stored trust is high / no rad provider).
        /// Stored trust is unchanged and still used for events / save.
        /// </summary>
        public float GetEffectiveTrust(string factionId)
        {
            float stored = GetTrust(factionId);
            // #257 Hated: all military factions are shot-on-sight (−100 trust).
            if (_personalQuests != null && _getSurvivors != null
                && IsMilitaryFaction(factionId))
            {
                var list = _getSurvivors();
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var sv = list[i];
                        if (sv == null || !sv.IsAlive) continue;
                        if (_personalQuests.IsShotOnSightByMilitary(sv))
                            return PersonalQuestSystem.HatedMilitaryTrust;
                    }
                }
            }

            var fac = GetFaction(factionId);
            if (fac == null || !fac.trustInversion) return stored;

            // ARS reverence outranks dose and hazmat: the glow is already in the blood.
            if (PartyHasAcuteRadiationSyndrome())
                return MaxTrust;

            float rad = GetPartyAverageRadiation();
            bool intactHazmat = PartyWearsIntactHazmat();

            // Hazmat / zero-rad contempt: sealed clean blood is heresy.
            // - With rad provider: healthy (≤ ceiling) + intact hazmat → MinTrust
            // - Without rad provider: intact hazmat alone floors trust (even if stored high)
            if (intactHazmat)
            {
                if (rad < 0f)
                    return MinTrust;
                float hazCeiling = Mathf.Clamp(fac.healthyRadiationCeiling, 0f, 100f);
                if (rad <= hazCeiling)
                    return MinTrust;
            }

            if (rad < 0f) return stored; // no provider → fall back

            float ceiling = Mathf.Clamp(fac.healthyRadiationCeiling, 0f, 100f);
            float floor = Mathf.Clamp(fac.highRadiationFloor, 0f, 100f);
            if (floor <= ceiling) floor = Mathf.Min(100f, ceiling + 1f);

            if (rad <= ceiling) return MinTrust;
            if (rad >= floor) return MaxTrust;

            float t = (rad - ceiling) / (floor - ceiling);
            return Mathf.Lerp(MinTrust, MaxTrust, t);
        }

        public float ModifyTrust(string factionId, float delta)
        {
            if (string.IsNullOrEmpty(factionId) || Mathf.Approximately(delta, 0f)) return GetTrust(factionId);

            float old = GetTrust(factionId);
            float next = Mathf.Clamp(old + delta, MinTrust, MaxTrust);
            if (_personalQuests != null && _getSurvivors != null)
                next = _personalQuests.ClampFactionTrust(next, _getSurvivors());
            _trust[factionId] = next;
            OnTrustChanged?.Invoke(factionId, old, next);
            OnEconomyChanged?.Invoke();

            // Crossing into raid territory may trigger an immediate hatch check.
            // Trust-inversion factions with a radiation provider gate disposition on
            // dose, not stored trust — skip the trust-delta raid cascade for them.
            var fac = GetFaction(factionId);
            bool radDriven = fac != null && fac.trustInversion && GetPartyAverageRadiation() >= 0f;
            if (!radDriven)
            {
                float threshold = fac != null ? fac.raidThreshold : DefaultRaidThreshold;
                if (old > threshold && next <= threshold)
                    TryLaunchRaid(factionId);
            }

            return next;
        }

        public void SetTrust(string factionId, float value)
        {
            if (string.IsNullOrEmpty(factionId)) return;
            float old = GetTrust(factionId);
            float next = Mathf.Clamp(value, MinTrust, MaxTrust);
            if (_personalQuests != null && _getSurvivors != null)
                next = _personalQuests.ClampFactionTrust(next, _getSurvivors());
            _trust[factionId] = next;
            if (!Mathf.Approximately(old, next))
            {
                OnTrustChanged?.Invoke(factionId, old, next);
                OnEconomyChanged?.Invoke();
            }
        }

        public TradeStance GetStance(string factionId)
        {
            // Pre-activation (Cult Day gate): refuse without raid pressure.
            if (!IsFactionActive(factionId))
                return TradeStance.Refuse;

            float trust = GetEffectiveTrust(factionId);
            var fac = GetFaction(factionId);
            float raidAt = fac != null ? fac.raidThreshold : DefaultRaidThreshold;
            float robAt = fac != null ? fac.robThreshold : -20f;
            float tradeAt = fac != null ? fac.minTrustToTrade : -40f;
            float intelAt = fac != null ? fac.intelShareThreshold : 40f;

            if (trust <= raidAt) return TradeStance.HostileRaid;
            if (trust <= robAt) return TradeStance.Rob;
            if (trust < tradeAt) return TradeStance.Refuse;
            if (trust >= intelAt) return TradeStance.ShareIntel;
            return TradeStance.Trade;
        }

        public bool WillTrade(string factionId)
        {
            var s = GetStance(factionId);
            return s == TradeStance.Trade || s == TradeStance.ShareIntel;
        }

        public bool WillShareIntel(string factionId) => GetStance(factionId) == TradeStance.ShareIntel;

        // -----------------------------------------------------------------
        // Aggression / succession / surrender
        // -----------------------------------------------------------------

        /// <summary>Effective raid aggression 0..1 (runtime override or SO default).</summary>
        public float GetRaidAggression(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return 0.5f;
            if (_aggressionOverride.TryGetValue(factionId, out float ovr))
                return Mathf.Clamp01(ovr);
            var fac = GetFaction(factionId);
            return fac != null ? Mathf.Clamp01(fac.raidAggression) : 0.5f;
        }

        public void SetRaidAggression(string factionId, float aggression01)
        {
            if (string.IsNullOrEmpty(factionId)) return;
            _aggressionOverride[factionId] = Mathf.Clamp01(aggression01);
            OnEconomyChanged?.Invoke();
        }

        public int GetSuccessionGeneration(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return 0;
            return _successionGeneration.TryGetValue(factionId, out int g) ? g : 0;
        }

        public string GetLeaderName(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return string.Empty;
            if (_leaderName.TryGetValue(factionId, out string name) && !string.IsNullOrEmpty(name))
                return name;
            var fac = GetFaction(factionId);
            return fac != null ? fac.displayName : factionId;
        }

        public int GetConsecutiveRepels(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return 0;
            return _consecutiveRepels.TryGetValue(factionId, out int n) ? n : 0;
        }

        public bool HasSurrendered(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return false;
            return _hasSurrendered.TryGetValue(factionId, out bool s) && s;
        }

        /// <summary>
        /// Leadership change inside a faction. Softens trust toward startingTrust,
        /// bumps succession generation, optional aggression reset.
        /// </summary>
        public FactionSuccessionResult ApplySuccession(
            string factionId,
            string newLeaderName = null,
            float trustBlendTowardStarting = 0.55f,
            float? newAggression = null)
        {
            var result = new FactionSuccessionResult { FactionId = factionId };
            if (string.IsNullOrEmpty(factionId) || !_factions.ContainsKey(factionId))
            {
                result.Message = "Unknown faction.";
                return result;
            }

            var fac = GetFaction(factionId);
            float oldTrust = GetTrust(factionId);
            float blend = Mathf.Clamp01(trustBlendTowardStarting);
            float target = fac != null ? fac.startingTrust : 0f;
            float nextTrust = Mathf.Lerp(oldTrust, target, blend);
            SetTrust(factionId, nextTrust);

            int gen = GetSuccessionGeneration(factionId) + 1;
            _successionGeneration[factionId] = gen;
            string previousLeader = GetLeaderName(factionId);
            string leader = string.IsNullOrEmpty(newLeaderName)
                ? $"{(fac != null ? fac.displayName : factionId)} cell {gen}"
                : newLeaderName;
            _leaderName[factionId] = leader;

            float oldAgg = GetRaidAggression(factionId);
            if (newAggression.HasValue)
                SetRaidAggression(factionId, newAggression.Value);
            // New leadership often reassesses the grudge
            _consecutiveRepels[factionId] = 0;
            _hasSurrendered[factionId] = false;

            result.Applied = true;
            result.PreviousLeader = previousLeader;
            result.NewLeader = leader;
            result.Generation = gen;
            result.OldTrust = oldTrust;
            result.NewTrust = GetTrust(factionId);
            result.OldAggression = oldAgg;
            result.NewAggression = GetRaidAggression(factionId);
            result.Message = $"{previousLeader} is gone. {leader} holds the banner now.";
            OnFactionSuccession?.Invoke(result);
            OnEconomyChanged?.Invoke();
            return result;
        }

        /// <summary>Last faction the player successfully repelled at the hatch (for UI).</summary>
        public string LastRepelledFactionId { get; private set; } = string.Empty;

        /// <summary>
        /// True when the player can demand parley / surrender: at least one
        /// consecutive hatch repel and the faction has not already stood down.
        /// </summary>
        public bool CanDemandParley(string factionId)
        {
            if (string.IsNullOrEmpty(factionId) || !_factions.ContainsKey(factionId)) return false;
            if (HasSurrendered(factionId)) return false;
            return GetConsecutiveRepels(factionId) >= 1;
        }

        /// <summary>
        /// Player demands parley after holding the hatch (requires a repel streak).
        /// Lifts trust and cuts aggression via <see cref="ForceSurrender"/>.
        /// </summary>
        public FactionSurrenderResult DemandParley(string factionId)
        {
            if (!CanDemandParley(factionId))
            {
                return new FactionSurrenderResult
                {
                    FactionId = factionId,
                    Applied = false,
                    Message = HasSurrendered(factionId)
                        ? "Already stood down."
                        : "They will not parley until you hold the hatch at least once."
                };
            }
            var result = ApplySurrender(factionId, auto: false);
            if (result.Applied)
                result.Message = $"{GetLeaderName(factionId)} accepts the parley. The raid is called off.";
            return result;
        }

        /// <summary>
        /// Force a hostile faction to stand down after a successful defense.
        /// Lifts trust above raid threshold and cuts aggression.
        /// Does not require a repel (scripted / test path); player UI should use
        /// <see cref="DemandParley"/> after a repel.
        /// </summary>
        public FactionSurrenderResult ForceSurrender(string factionId)
        {
            return ApplySurrender(factionId, auto: false);
        }

        /// <summary>
        /// Auto-surrender when consecutive hatch repels reach
        /// <see cref="RepelsForAutoSurrender"/>.
        /// </summary>
        public FactionSurrenderResult TryAutoSurrender(string factionId)
        {
            if (GetConsecutiveRepels(factionId) < RepelsForAutoSurrender)
            {
                return new FactionSurrenderResult
                {
                    FactionId = factionId,
                    Applied = false,
                    Message = "Not enough consecutive repels for auto-surrender."
                };
            }
            return ApplySurrender(factionId, auto: true);
        }

        private FactionSurrenderResult ApplySurrender(string factionId, bool auto)
        {
            var result = new FactionSurrenderResult { FactionId = factionId, Auto = auto };
            if (string.IsNullOrEmpty(factionId) || !_factions.ContainsKey(factionId))
            {
                result.Message = "Unknown faction.";
                return result;
            }
            if (HasSurrendered(factionId))
            {
                result.Applied = false;
                result.Message = "Already stood down.";
                return result;
            }

            var fac = GetFaction(factionId);
            float raidAt = fac != null ? fac.raidThreshold : DefaultRaidThreshold;
            float oldTrust = GetTrust(factionId);
            float oldAgg = GetRaidAggression(factionId);

            float nextTrust = Mathf.Max(oldTrust, raidAt + SurrenderTrustBuffer);
            SetTrust(factionId, nextTrust);
            SetRaidAggression(factionId, oldAgg * SurrenderAggressionScale);
            _hasSurrendered[factionId] = true;
            _consecutiveRepels[factionId] = 0;

            result.Applied = true;
            result.OldTrust = oldTrust;
            result.NewTrust = GetTrust(factionId);
            result.OldAggression = oldAgg;
            result.NewAggression = GetRaidAggression(factionId);
            result.NewStance = GetStance(factionId);
            result.Message = auto
                ? $"{GetLeaderName(factionId)} stops hammering the hatch. They've had enough."
                : $"{GetLeaderName(factionId)} signals for parley. The raid is called off.";
            OnFactionSurrender?.Invoke(result);
            OnEconomyChanged?.Invoke();
            return result;
        }

        /// <summary>Record raid outcome for surrender streak tracking.</summary>
        public void NoteRaidOutcome(string factionId, bool launched, bool repelled)
        {
            if (string.IsNullOrEmpty(factionId) || !launched) return;
            if (!_totalRaidsLaunched.ContainsKey(factionId))
                _totalRaidsLaunched[factionId] = 0;
            _totalRaidsLaunched[factionId]++;

            if (repelled)
            {
                LastRepelledFactionId = factionId;
                int n = GetConsecutiveRepels(factionId) + 1;
                _consecutiveRepels[factionId] = n;
                if (n >= RepelsForAutoSurrender)
                    TryAutoSurrender(factionId);
            }
            else
            {
                _consecutiveRepels[factionId] = 0;
            }
        }

        // -----------------------------------------------------------------
        // Pricing
        // -----------------------------------------------------------------

        public float GetDemandMultiplier(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return 1f;
            return _demand.TryGetValue(itemId, out float d) ? Mathf.Clamp(d, MinDemandMult, MaxDemandMult) : 1f;
        }

        /// <summary>Nudge global demand (scarcity). Positive = more scarce / valuable.</summary>
        public void AdjustDemand(string itemId, float delta)
        {
            if (string.IsNullOrEmpty(itemId) || Mathf.Approximately(delta, 0f)) return;
            float cur = GetDemandMultiplier(itemId);
            _demand[itemId] = Mathf.Clamp(cur + delta, MinDemandMult, MaxDemandMult);
            OnEconomyChanged?.Invoke();
        }

        /// <summary>
        /// Effective trade value for an item under current WorldPhase + supply/demand.
        /// Does not apply faction trust (use <see cref="GetBarterUnitValue"/> for that).
        /// </summary>
        public float GetTradeValue(ItemDefinition item)
        {
            if (item == null) return 0f;
            float phaseVal = TradeEconomy.GetEffectiveValue(item, CurrentPhase);
            if (phaseVal <= 0f) return 0f;
            float v = phaseVal * GetDemandMultiplier(item.id);
            // #281 Beacon of Truth: global trade prices −30%.
            if (_personalQuests != null && _getSurvivors != null)
                v *= _personalQuests.GetBeaconTradePriceMultiplier(_getSurvivors());
            return v;
        }

        /// <summary>
        /// #255 Master Manipulator: Junk offered by the Liar trades at high-tier
        /// Medicine prices when the trader has the latent trait.
        /// </summary>
        public float GetTradeValue(ItemDefinition item, Survivor trader)
        {
            float baseVal = GetTradeValue(item);
            if (item == null || trader == null || _personalQuests == null) return baseVal;
            if (!_personalQuests.TradesJunkAsMedicine(trader)) return baseVal;
            bool isJunk = item.type == ItemType.Material
                || (!string.IsNullOrEmpty(item.id)
                    && item.id.IndexOf("junk", System.StringComparison.OrdinalIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(item.displayName)
                    && item.displayName.IndexOf("junk", System.StringComparison.OrdinalIgnoreCase) >= 0);
            // Prefer explicit junk ids / low-value scrap materials.
            if (!isJunk && item.tradeValue > 8f) return baseVal;
            if (!isJunk && item.type != ItemType.Material) return baseVal;
            // High-tier medicine reference (~antibiotics baseline under demand).
            float medicineTier = 40f * GetDemandMultiplier("antibiotics");
            if (medicineTier <= 0f) medicineTier = 40f;
            return _personalQuests.GetJunkTradeValueAsMedicine(trader, baseVal, medicineTier);
        }

        /// <summary>
        /// Unit value in a barter with a specific faction. High trust improves the
        /// player's selling price and softens buy prices; low trust does the reverse.
        /// After a hatch parley / surrender, prices soften further in the player's favor
        /// (see <see cref="ParleyBarterSellBonus"/> / <see cref="ParleyBarterBuyDiscount"/>).
        /// Trust-inversion factions (Cult of the Glow) use effective (radiation-driven)
        /// trust and multiply <see cref="ItemType.IrradiatedWater"/> heavily — the glow
        /// is sacrament, not waste. With party ARS, high-value gear buy prices drop
        /// further (<see cref="ArsReverenceBuyDiscount"/>).
        /// </summary>
        /// <param name="playerSelling">True when the player is offering the item to the faction.</param>
        public float GetBarterUnitValue(ItemDefinition item, string factionId, bool playerSelling)
        {
            float baseVal = GetTradeValue(item);
            if (baseVal <= 0f) return 0f;

            var fac = GetFaction(factionId);
            // Cult of the Glow / trust-inversion: irradiated water is prized currency.
            if (fac != null && fac.trustInversion && item.type == ItemType.IrradiatedWater)
            {
                float mult = fac.irradiatedWaterValueMultiplier > 0f
                    ? fac.irradiatedWaterValueMultiplier
                    : DefaultTrustInversionIrradiatedWaterMult;
                baseVal *= mult;
            }

            float trust = GetEffectiveTrust(factionId);
            // trust -100..100 → factor ~0.7..1.3 for seller favor when player sells
            float trustNorm = Mathf.Clamp(trust, MinTrust, MaxTrust) / MaxTrust; // -1..1
            float factor = playerSelling
                ? 1f + 0.3f * trustNorm   // trusted: get more for goods you sell
                : 1f - 0.25f * trustNorm; // trusted: pay less for goods you buy

            // Post-parley softener: they flinched at the hatch; the table tilts.
            if (HasSurrendered(factionId))
            {
                factor *= playerSelling
                    ? 1f + ParleyBarterSellBonus
                    : 1f - ParleyBarterBuyDiscount;
            }

            // ARS reverence: steep discount when the player buys high-value gear
            // from a trust-inversion faction (they gift the dying).
            if (!playerSelling
                && fac != null
                && fac.trustInversion
                && PartyHasAcuteRadiationSyndrome()
                && IsArsReverenceHighValueItem(item))
            {
                factor *= 1f - ArsReverenceBuyDiscount;
            }

            return Mathf.Max(0f, baseVal * factor);
        }

        /// <summary>
        /// High-value gear types that receive the ARS reverence buy discount at
        /// trust-inversion factions (Protective / Weapon / Medical / AntiRad / Device / Tool).
        /// </summary>
        public static bool IsArsReverenceHighValueItem(ItemDefinition item)
        {
            if (item == null) return false;
            switch (item.type)
            {
                case ItemType.Protective:
                case ItemType.Weapon:
                case ItemType.Medical:
                case ItemType.AntiRad:
                case ItemType.Device:
                case ItemType.Tool:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Sum of barter value for a list of (item, amount) offers.
        /// </summary>
        public float EvaluateOffer(
            IReadOnlyList<BarterLine> lines,
            string factionId,
            bool playerSelling)
        {
            if (lines == null || lines.Count == 0) return 0f;
            float total = 0f;
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.Item == null || line.Amount <= 0) continue;
                total += GetBarterUnitValue(line.Item, factionId, playerSelling) * line.Amount;
            }
            return total;
        }

        /// <summary>
        /// Whether the player's offer covers the faction ask under current barter math.
        /// </summary>
        public bool IsFairTrade(
            IReadOnlyList<BarterLine> playerOffers,
            IReadOnlyList<BarterLine> factionAsks,
            string factionId,
            out float playerValue,
            out float factionValue)
        {
            playerValue = EvaluateOffer(playerOffers, factionId, playerSelling: true);
            factionValue = EvaluateOffer(factionAsks, factionId, playerSelling: false);
            if (!WillTrade(factionId)) return false;
            // Small epsilon for float fairness
            return playerValue + 0.01f >= factionValue;
        }

        /// <summary>
        /// Execute a fair trade: move items between player inventory and a virtual
        /// faction stock bag (also an Inventory). Returns false if unfair / refused.
        /// </summary>
        public bool TryExecuteTrade(
            Inventory.Inventory playerInv,
            Inventory.Inventory factionStock,
            IReadOnlyList<BarterLine> playerOffers,
            IReadOnlyList<BarterLine> factionAsks,
            string factionId)
        {
            if (playerInv == null || factionStock == null) return false;
            if (!WillTrade(factionId)) return false;
            if (!IsOfferBarterOnlyAcceptable(playerOffers)) return false;
            if (!IsFairTrade(playerOffers, factionAsks, factionId, out _, out _)) return false;

            // Validate stock
            if (!HasLines(playerInv, playerOffers) || !HasLines(factionStock, factionAsks))
                return false;

            // Transfer
            if (!TransferLines(playerInv, factionStock, playerOffers)) return false;
            if (!TransferLines(factionStock, playerInv, factionAsks))
            {
                // Best-effort rollback of player→faction transfer
                TransferLines(factionStock, playerInv, playerOffers);
                return false;
            }

            // Supply/demand: player sold goods become more common; bought goods scarcer
            NudgeDemandFromTrade(playerOffers, soldByPlayer: true);
            NudgeDemandFromTrade(factionAsks, soldByPlayer: false);

            // Tiny trust bump for completing a deal
            ModifyTrust(factionId, +1f);
            return true;
        }

        private void NudgeDemandFromTrade(IReadOnlyList<BarterLine> lines, bool soldByPlayer)
        {
            if (lines == null) return;
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.Item == null) continue;
                // Player selling floods market → demand down; player buying drains → demand up
                float delta = soldByPlayer ? -0.05f * line.Amount : 0.05f * line.Amount;
                AdjustDemand(line.Item.id, delta);
            }
        }

        private static bool HasLines(Inventory.Inventory inv, IReadOnlyList<BarterLine> lines)
        {
            if (lines == null) return true;
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.Item == null || line.Amount <= 0) continue;
                if (inv.Count(line.Item) < line.Amount) return false;
            }
            return true;
        }

        private static bool TransferLines(
            Inventory.Inventory from,
            Inventory.Inventory to,
            IReadOnlyList<BarterLine> lines)
        {
            if (lines == null) return true;
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.Item == null || line.Amount <= 0) continue;
                if (!from.Remove(line.Item, line.Amount)) return false;
                if (!to.Add(line.Item, line.Amount))
                {
                    from.Add(line.Item, line.Amount);
                    return false;
                }
            }
            return true;
        }

        // -----------------------------------------------------------------
        // Raids
        // -----------------------------------------------------------------

        /// <summary>
        /// Attempt a hatch raid when trust is at/below the faction's raid threshold.
        /// Delegates to HatchDefenseSystem (security + weapons vs raid strength).
        /// Post-Day 30 only unless hatch defense is forced for tests.
        /// Trust-inversion factions use radiation-driven effective trust.
        /// </summary>
        public FactionRaidResult TryLaunchRaid(string factionId, bool ignoreDayGate = false)
        {
            var result = new FactionRaidResult { FactionId = factionId };
            var fac = GetFaction(factionId);
            float trust = GetEffectiveTrust(factionId);
            float threshold = fac != null ? fac.raidThreshold : DefaultRaidThreshold;

            // Surrender outranks the trust gate (surrender lifts trust on purpose).
            if (HasSurrendered(factionId))
            {
                result.Launched = false;
                result.Message = "Faction already stood down after the last push.";
                return result;
            }

            // Cult pre-activation: no hatch pressure until Day ≥ CultActivationDay.
            if (!IsFactionActive(factionId))
            {
                result.Launched = false;
                result.Message = "Faction not active yet.";
                return result;
            }

            if (trust > threshold)
            {
                result.Launched = false;
                result.Message = "Trust still above raid threshold.";
                return result;
            }

            float aggression = GetRaidAggression(factionId);
            // Map aggression 0..1 → raid strength ~30..70
            float strength = 30f + aggression * 40f;
            result.Aggression = aggression;

            int day = _getDay != null ? _getDay() : HatchDefenseSystem.RaidUnlockDay;
            int shieldLevel = 0;
            var shieldMod = _shelter?.GetModule("radiation_shielding");
            if (shieldMod != null) shieldLevel = shieldMod.Level;
            result.ShieldingLevel = shieldLevel;

            if (_hatchDefense != null)
            {
                // Post-Day 30 pressure; tests may pass ignoreDayGate: true.
                if (!ignoreDayGate && !_hatchDefense.IsRaidUnlocked(day))
                {
                    result.Launched = false;
                    result.Message = "Pre-Day 30: hatch raids not yet active.";
                    return result;
                }

                var resolution = _hatchDefense.ResolveFactionRaid(
                    factionId, strength, day, ignoreDayGate: true);

                result.Launched = resolution.Launched;
                result.Repelled = resolution.Repelled;
                result.Breached = resolution.Breached;
                result.HatchDamage = resolution.HatchDamage;
                result.RaidStrength = resolution.RaidStrength;
                result.DefenseScore = resolution.DefenseScore;
                result.ShelterSecurity = resolution.ShelterSecurity;
                result.StolenItemCount = resolution.StolenItems != null ? resolution.StolenItems.Count : 0;
                result.Message = resolution.Message;
            }
            else
            {
                // Legacy fallback when hatch defense not wired
                result.Launched = true;
                float integrity = Mathf.Clamp01(shieldLevel * 0.25f);
                result.Repelled = integrity >= 0.5f && _rng.NextDouble() < integrity;
                if (result.Repelled)
                {
                    result.HatchDamage = 5f + 10f * aggression * (1f - integrity);
                    result.Message = "Hatch held. Scuffs on the plate, nothing more.";
                }
                else
                {
                    result.HatchDamage = 20f + 40f * aggression * (1f - integrity);
                    result.Message = "Hatch forced. Interior took the hit.";
                    ApplyHatchDamageLegacy(result.HatchDamage);
                    result.Breached = true;
                }
            }

            NoteRaidOutcome(factionId, result.Launched, result.Repelled);
            if (HasSurrendered(factionId))
                result.SurrenderedAfter = true;

            OnRaidResolved?.Invoke(result);
            OnEconomyChanged?.Invoke();
            return result;
        }

        private void ApplyHatchDamageLegacy(float damage)
        {
            if (_shelter == null || damage <= 0f) return;

            var air = _shelter.GetModule("air_filtration");
            if (air != null)
            {
                air.FilterHealth = Mathf.Max(0f, air.FilterHealth - damage);
            }

            if (damage >= 40f)
            {
                var shield = _shelter.GetModule("radiation_shielding");
                if (shield != null && shield.Level > 0)
                    shield.Level = Mathf.Max(0, shield.Level - 1);
            }
        }

        // -----------------------------------------------------------------
        // Barter-only mode (flashpoint trader panic)
        // -----------------------------------------------------------------

        /// <summary>
        /// Toggle the Day-30 trader panic. When enabled, player offers are
        /// refused unless every offered item is in <paramref name="acceptedItemIds"/>
        /// (defaults to a no-op set if null is passed and enable is false).
        /// Pass null for <paramref name="acceptedItemIds"/> to keep the current
        /// set when toggling, or a new list to replace it.
        /// </summary>
        public void SetBarterOnlyMode(bool enabled, IReadOnlyList<string> acceptedItemIds = null)
        {
            if (acceptedItemIds != null)
            {
                _barterOnlyAcceptedItemIds.Clear();
                for (int i = 0; i < acceptedItemIds.Count; i++)
                {
                    var id = acceptedItemIds[i];
                    if (!string.IsNullOrEmpty(id)) _barterOnlyAcceptedItemIds.Add(id);
                }
            }

            if (_barterOnlyMode == enabled) return;
            _barterOnlyMode = enabled;
            OnBarterOnlyModeChanged?.Invoke(enabled);
            OnEconomyChanged?.Invoke();
        }

        /// <summary>
        /// Returns true if the player's offer is acceptable under the current
        /// barter-only rules. When barter-only is off, every offer is
        /// acceptable. When on, every line must be an item whose id is in
        /// the accepted set; an empty accepted set rejects everything.
        /// </summary>
        public bool IsOfferBarterOnlyAcceptable(IReadOnlyList<BarterLine> playerOffers)
        {
            if (!_barterOnlyMode) return true;
            if (playerOffers == null || playerOffers.Count == 0) return false;
            if (_barterOnlyAcceptedItemIds.Count == 0) return false;
            for (int i = 0; i < playerOffers.Count; i++)
            {
                var line = playerOffers[i];
                if (line.Item == null || string.IsNullOrEmpty(line.Item.id)) return false;
                if (!_barterOnlyAcceptedItemIds.Contains(line.Item.id)) return false;
            }
            return true;
        }

        // -----------------------------------------------------------------
        // EventRunner binding
        // -----------------------------------------------------------------

        /// <summary>
        /// Subscribe to EventRunner so choice FactionId + TrustDelta/RelationshipDelta
        /// alter the trust matrix. Also applies EventEffect trust fields when present.
        /// </summary>
        public void BindEventRunner(EventRunner runner)
        {
            if (runner == null) return;
            runner.OnChoiceApplied += HandleChoiceApplied;
        }

        public void UnbindEventRunner(EventRunner runner)
        {
            if (runner == null) return;
            runner.OnChoiceApplied -= HandleChoiceApplied;
        }

        private void HandleChoiceApplied(GameEvent gameEvent, EventChoice choice, EventContext context)
        {
            if (choice == null) return;

            string factionId = choice.FactionId;
            float delta = choice.TrustDelta;
            // RelationshipDelta is the older field used by narrative content; treat as trust
            // when FactionId is set and TrustDelta was left at 0.
            if (Mathf.Approximately(delta, 0f) && !Mathf.Approximately(choice.RelationshipDelta, 0f))
                delta = choice.RelationshipDelta;

            if (!string.IsNullOrEmpty(factionId) && !Mathf.Approximately(delta, 0f))
                ModifyTrust(factionId, delta);

            // Effects may also carry faction trust deltas
            if (choice.Effects != null)
            {
                for (int i = 0; i < choice.Effects.Count; i++)
                {
                    var fx = choice.Effects[i];
                    if (fx == null || string.IsNullOrEmpty(fx.FactionId)) continue;
                    if (!Mathf.Approximately(fx.TrustDelta, 0f))
                        ModifyTrust(fx.FactionId, fx.TrustDelta);
                }
            }
        }

        // -----------------------------------------------------------------
        // Factories
        // -----------------------------------------------------------------

        public static List<FactionSO> CreateDefaultFactions()
        {
            return new List<FactionSO>
            {
                MakeFaction(FactionSO.Ids.MilitaryRemnants, "Military Remnants",
                    "Scattered uniformed holdouts. Trade ammo and order for food.",
                    startingTrust: 10f, raidAggression: 0.7f, raidThreshold: -50f,
                    minTrade: -35f, rob: -15f, intel: 45f),
                MakeFaction(FactionSO.Ids.ScavengerCamp, "Scavenger Camp",
                    "Loose camp on the ring road. Everything has a price.",
                    startingTrust: 0f, raidAggression: 0.55f, raidThreshold: -50f,
                    minTrade: -40f, rob: -20f, intel: 35f),
                MakeFaction(FactionSO.Ids.DoomsdayPreppers, "Doomsday Preppers",
                    "Bunker neighbors who stocked early. Paranoid, but solvent.",
                    startingTrust: 5f, raidAggression: 0.35f, raidThreshold: -55f,
                    minTrade: -30f, rob: -25f, intel: 50f),
                // Concept 16 — The Cult of the Glow: clean blood is heresy.
                MakeFaction(FactionSO.Ids.CultOfTheGlow, "Cult of the Glow",
                    "They drink the fallout and call it grace. Clean blood is heresy; the glow is kin.",
                    startingTrust: -10f, raidAggression: 0.6f, raidThreshold: -50f,
                    minTrade: -40f, rob: -20f, intel: 40f,
                    trustInversion: true,
                    healthyRadiationCeiling: 20f,
                    highRadiationFloor: 60f,
                    irradiatedWaterValueMultiplier: 12f),
            };
        }

        private static FactionSO MakeFaction(
            string id, string name, string desc,
            float startingTrust, float raidAggression, float raidThreshold,
            float minTrade, float rob, float intel,
            bool trustInversion = false,
            float healthyRadiationCeiling = 20f,
            float highRadiationFloor = 60f,
            float irradiatedWaterValueMultiplier = 12f)
        {
            var f = ScriptableObject.CreateInstance<FactionSO>();
            f.id = id;
            f.displayName = name;
            f.description = desc;
            f.startingTrust = startingTrust;
            f.raidAggression = raidAggression;
            f.raidThreshold = raidThreshold;
            f.minTrustToTrade = minTrade;
            f.robThreshold = rob;
            f.intelShareThreshold = intel;
            f.trustInversion = trustInversion;
            f.healthyRadiationCeiling = healthyRadiationCeiling;
            f.highRadiationFloor = highRadiationFloor;
            f.irradiatedWaterValueMultiplier = irradiatedWaterValueMultiplier;
            return f;
        }

        /// <summary>
        /// Narrative event: faction scouts ask to enter the bunker.
        /// Refuse → trust drop (may cascade into raid at -50).
        /// </summary>
        public static GameEvent CreateFactionScoutEvent(FactionSO faction)
        {
            if (faction == null) throw new ArgumentNullException(nameof(faction));
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "evt_faction_scout_" + faction.id;
            ev.title = "Knock at the Hatch";
            ev.bodyText =
                $"{faction.displayName} scouts want a look inside. " +
                "Letting them in risks a map of your stores. Refusing risks a grudge.";
            ev.weight = 1f;
            ev.conditions = new EventConditions { MinDay = 1 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "allow_scout",
                    Text = "Crack the hatch. Let them see we're still human.",
                    MoraleDelta = -2f,
                    FactionId = faction.id,
                    TrustDelta = 12f,
                    RelationshipDelta = 12f
                },
                new EventChoice
                {
                    ChoiceId = "refuse_scout",
                    Text = "Keep it sealed. Nobody maps our stores.",
                    MoraleDelta = 1f,
                    FactionId = faction.id,
                    TrustDelta = -30f,
                    RelationshipDelta = -30f
                }
            };
            return ev;
        }

        /// <summary>
        /// Post-repel modal: demand parley, open trade, or dismiss.
        /// Choice ids: parley_now | open_trade | dismiss.
        /// Copy and channel-tag VO flavor are faction-specific.
        /// </summary>
        public GameEvent CreateParleyOfferEvent(string factionId)
        {
            var fac = GetFaction(factionId);
            string name = fac != null && !string.IsNullOrEmpty(fac.displayName)
                ? fac.displayName
                : (factionId ?? "unknown");
            string leader = GetLeaderName(factionId);
            if (string.IsNullOrEmpty(leader)) leader = "Their lead";
            string channelTag = GetParleyChannelTag(factionId);

            GetParleyOfferFlavor(factionId, name, leader, channelTag,
                out string title, out string body,
                out string parleyChoice, out string tradeChoice, out string dismissChoice);

            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "evt_parley_offer_" + (factionId ?? "unknown");
            ev.title = title;
            ev.bodyText = body;
            ev.weight = 1f;
            ev.conditions = new EventConditions { MinDay = 1 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "parley_now",
                    Text = parleyChoice,
                    MoraleDelta = 2f,
                    FactionId = factionId
                },
                new EventChoice
                {
                    ChoiceId = "open_trade",
                    Text = tradeChoice,
                    MoraleDelta = 0f,
                    FactionId = factionId
                },
                new EventChoice
                {
                    ChoiceId = "dismiss",
                    Text = dismissChoice,
                    MoraleDelta = -1f,
                    FactionId = factionId
                }
            };
            return ev;
        }

        /// <summary>Short diegetic VO/channel label for radio UI + parley body.</summary>
        public static string GetParleyChannelTag(string factionId)
        {
            switch (factionId)
            {
                case FactionSO.Ids.MilitaryRemnants: return "CH-7 MILBAND";
                case FactionSO.Ids.ScavengerCamp: return "CH-3 ASH ROAD";
                case FactionSO.Ids.DoomsdayPreppers: return "CH-11 STOCKPILE";
                case FactionSO.Ids.CultOfTheGlow: return "CH-13 GLOWBAND";
                default: return "CH-OPEN";
            }
        }

        /// <summary>
        /// Faction-flavored title/body/choices for the post-repel parley modal.
        /// Tone stays cold and human — no glory, no slogans.
        /// </summary>
        public static void GetParleyOfferFlavor(
            string factionId,
            string factionDisplayName,
            string leaderName,
            string channelTag,
            out string title,
            out string body,
            out string parleyChoice,
            out string tradeChoice,
            out string dismissChoice)
        {
            string name = string.IsNullOrEmpty(factionDisplayName) ? "They" : factionDisplayName;
            string leader = string.IsNullOrEmpty(leaderName) ? "Their lead" : leaderName;
            string tag = string.IsNullOrEmpty(channelTag) ? "CH-OPEN" : channelTag;

            switch (factionId)
            {
                case FactionSO.Ids.MilitaryRemnants:
                    title = "They Flinched at the Hatch";
                    body =
                        $"[{tag}] Static. Then a clipped voice — {leader}, {name}. " +
                        "Orders break mid-sentence. Boots scrape back from the plate. " +
                        "You can demand a formal stand-down now, or keep the hatch sealed " +
                        "and force the issue across a trade table.";
                    parleyChoice = "Open channel. Demand formal stand-down. [parley]";
                    tradeChoice = "Hold the seal. Call them to the table first. [trade]";
                    dismissChoice = "Let the band go cold. Not yet.";
                    break;

                case FactionSO.Ids.ScavengerCamp:
                    title = "They Flinched at the Hatch";
                    body =
                        $"[{tag}] Market chatter dies. {leader} of {name} is on the air — " +
                        "short curses, someone laughing once, then dead air. Hatch held. " +
                        "You can make them name the stand-down, or open trade while the " +
                        "road still remembers who bounced.";
                    parleyChoice = "Cut in. Make them stand down out loud. [parley]";
                    tradeChoice = "Open the bag first. Talk prices, not raids. [trade]";
                    dismissChoice = "Not now. Let the road stew.";
                    break;

                case FactionSO.Ids.DoomsdayPreppers:
                    title = "They Flinched at the Hatch";
                    body =
                        $"[{tag}] A hymn cuts out mid-line. {leader} of {name} whispers " +
                        "inventory codes into the dark — then nothing. Their test of the " +
                        "hatch failed. Demand they fold the raid, or bargain over sealed " +
                        "stores before the next sermon.";
                    parleyChoice = "Answer the hymn. Demand the raid is over. [parley]";
                    tradeChoice = "Offer a sealed trade. Keep the hatch blind. [trade]";
                    dismissChoice = "Silence. Let them recount their stores alone.";
                    break;

                case FactionSO.Ids.CultOfTheGlow:
                    title = "They Flinched at the Hatch";
                    body =
                        $"[{tag}] A wet cough on the band — {leader} of {name}. " +
                        "They sound disappointed, not angry. The plate held; the glow " +
                        "did not get in. You can make them name a stand-down, or open " +
                        "trade and offer what they actually want: the dirty water.";
                    parleyChoice = "Answer the glowband. Demand the raid is over. [parley]";
                    tradeChoice = "Offer irradiated water first. Keep the hatch sealed. [trade]";
                    dismissChoice = "Cut the band. Let them pray to the ash alone.";
                    break;

                default:
                    title = "They Flinched at the Hatch";
                    body =
                        $"[{tag}] {name} bounced off the plate. {leader} is still on the band — " +
                        "short curses, then dead air. You can demand they stand down now, " +
                        "or open trade and press the issue at the table.";
                    parleyChoice = "Open the channel. Demand they stand down. [parley]";
                    tradeChoice = "Crack trade first. Keep the hatch sealed. [trade]";
                    dismissChoice = "Not now. Let them stew.";
                    break;
            }
        }

        // -----------------------------------------------------------------
        // Save / load
        // -----------------------------------------------------------------

        public DynamicEconomySave CaptureState()
        {
            var save = new DynamicEconomySave();
            var trustRows = new List<FactionTrustSave>();
            foreach (var kv in _trust)
            {
                string id = kv.Key;
                trustRows.Add(new FactionTrustSave
                {
                    FactionId = id,
                    Trust = kv.Value,
                    AggressionOverride = _aggressionOverride.TryGetValue(id, out float a) ? a : -1f,
                    SuccessionGeneration = GetSuccessionGeneration(id),
                    LeaderName = GetLeaderName(id),
                    ConsecutiveRepels = GetConsecutiveRepels(id),
                    HasSurrendered = HasSurrendered(id)
                });
            }
            save.Trust = trustRows.ToArray();

            var demandRows = new List<DemandSave>();
            foreach (var kv in _demand)
            {
                demandRows.Add(new DemandSave { ItemId = kv.Key, Multiplier = kv.Value });
            }
            save.Demand = demandRows.ToArray();

            save.BarterOnlyMode = _barterOnlyMode;
            save.BarterOnlyAccepted = _barterOnlyAcceptedItemIds.ToArray();
            save.LastRepelledFactionId = LastRepelledFactionId ?? string.Empty;
            return save;
        }

        public void RestoreState(DynamicEconomySave save)
        {
            if (save == null) return;
            _trust.Clear();
            _aggressionOverride.Clear();
            _successionGeneration.Clear();
            _leaderName.Clear();
            _consecutiveRepels.Clear();
            _hasSurrendered.Clear();
            LastRepelledFactionId = string.Empty;
            if (save.Trust != null)
            {
                for (int i = 0; i < save.Trust.Length; i++)
                {
                    var row = save.Trust[i];
                    if (row == null || string.IsNullOrEmpty(row.FactionId)) continue;
                    _trust[row.FactionId] = Mathf.Clamp(row.Trust, MinTrust, MaxTrust);
                    if (row.AggressionOverride >= 0f)
                        _aggressionOverride[row.FactionId] = Mathf.Clamp01(row.AggressionOverride);
                    _successionGeneration[row.FactionId] = Mathf.Max(0, row.SuccessionGeneration);
                    if (!string.IsNullOrEmpty(row.LeaderName))
                        _leaderName[row.FactionId] = row.LeaderName;
                    _consecutiveRepels[row.FactionId] = Mathf.Max(0, row.ConsecutiveRepels);
                    _hasSurrendered[row.FactionId] = row.HasSurrendered;
                }
            }
            _demand.Clear();
            if (save.Demand != null)
            {
                for (int i = 0; i < save.Demand.Length; i++)
                {
                    var row = save.Demand[i];
                    if (row == null || string.IsNullOrEmpty(row.ItemId)) continue;
                    _demand[row.ItemId] = Mathf.Clamp(row.Multiplier, MinDemandMult, MaxDemandMult);
                }
            }
            _barterOnlyMode = save.BarterOnlyMode;
            _barterOnlyAcceptedItemIds.Clear();
            if (save.BarterOnlyAccepted != null)
            {
                for (int i = 0; i < save.BarterOnlyAccepted.Length; i++)
                {
                    var id = save.BarterOnlyAccepted[i];
                    if (!string.IsNullOrEmpty(id)) _barterOnlyAcceptedItemIds.Add(id);
                }
            }
            LastRepelledFactionId = save.LastRepelledFactionId ?? string.Empty;
            OnEconomyChanged?.Invoke();
        }
    }

    [Serializable]
    public struct BarterLine
    {
        public ItemDefinition Item;
        public int Amount;

        public BarterLine(ItemDefinition item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }

    [Serializable]
    public class FactionRaidResult
    {
        public string FactionId;
        public bool Launched;
        public bool Repelled;
        public bool Breached;
        public float HatchDamage;
        public int ShieldingLevel;
        public float RaidStrength;
        public float DefenseScore;
        public float ShelterSecurity;
        public int StolenItemCount;
        public float Aggression;
        public bool SurrenderedAfter;
        public string Message;
    }

    [Serializable]
    public class FactionSuccessionResult
    {
        public string FactionId;
        public bool Applied;
        public string PreviousLeader;
        public string NewLeader;
        public int Generation;
        public float OldTrust;
        public float NewTrust;
        public float OldAggression;
        public float NewAggression;
        public string Message;
    }

    [Serializable]
    public class FactionSurrenderResult
    {
        public string FactionId;
        public bool Applied;
        public bool Auto;
        public float OldTrust;
        public float NewTrust;
        public float OldAggression;
        public float NewAggression;
        public TradeStance NewStance;
        public string Message;
    }

    [Serializable]
    public class FactionTrustSave
    {
        public string FactionId;
        public float Trust;
        /// <summary>Runtime aggression override; −1 means use FactionSO default.</summary>
        public float AggressionOverride = -1f;
        public int SuccessionGeneration;
        public string LeaderName;
        public int ConsecutiveRepels;
        public bool HasSurrendered;
    }

    [Serializable]
    public class DemandSave
    {
        public string ItemId;
        public float Multiplier;
    }

    [Serializable]
    public class DynamicEconomySave
    {
        public FactionTrustSave[] Trust;
        public DemandSave[] Demand;
        /// <summary>Whether barter-only mode is on (Day-30 trader panic).</summary>
        public bool BarterOnlyMode;
        /// <summary>Item ids the player may offer while barter-only is on.</summary>
        public string[] BarterOnlyAccepted;
        /// <summary>Last faction successfully repelled at the hatch (trade strip / parley).</summary>
        public string LastRepelledFactionId;
    }
}
