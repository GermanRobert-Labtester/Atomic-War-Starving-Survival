using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Radio
{
    public enum PsyOpsCampaignStatus
    {
        Active = 0,
        Stalled = 1,     // transmitter down or off-power: day passes, no pressure
        Expired = 2
    }

    /// <summary>One running broadcast campaign (instantiated from the catalog).</summary>
    public sealed class PsyOpsCampaignState
    {
        public string campaignId = string.Empty;     // psyops_campaign_*
        public string targetFactionId = string.Empty;
        public string messageTheme = string.Empty;   // theme label from the catalog
        public float baseReach;                      // 0..100 (catalog)
        public float powerDemandWatts;
        public float receptiveness;                  // 0..1 (catalog)
        public float loyaltyPressurePerDay;
        public int startedDay;
        public int durationDays;
        public int daysElapsed;
        public int status;                           // (int) PsyOpsCampaignStatus
    }

    /// <summary>Abstract rival-jamming the player runs against a faction's airtime.</summary>
    public sealed class PsyOpsJammingState
    {
        public string targetFactionId = string.Empty;
        public float strength;        // 0..1 abstract
        public int daysRemaining;
    }

    /// <summary>Counter-propaganda suppression of one hostile/active campaign.</summary>
    public sealed class PsyOpsCounterState
    {
        public string campaignId = string.Empty;
        public int daysRemaining;
    }

    /// <summary>Serialized psyops state (checksummed via PsyOpsSaveCodec).</summary>
    public sealed class PsyOpsState
    {
        public int schemaVersion = 1;
        public List<PsyOpsCampaignState> campaigns = new List<PsyOpsCampaignState>();
        public List<PsyOpsJammingState> jamming = new List<PsyOpsJammingState>();
        public List<PsyOpsCounterState> counters = new List<PsyOpsCounterState>();
        /// <summary>Faction id → accumulated ideological pressure, −100..100, decays daily.</summary>
        public List<string> pressureFactionIds = new List<string>();
        public List<float> pressureValues = new List<float>();
        public List<string> fatigueFactionIds = new List<string>();
        public List<int> fatigueDays = new List<int>();
    }

    /// <summary>
    /// Flagship XI — Plan 157: fictional strategic influence layer. Owns campaign
    /// instances, abstract jamming/counter-propaganda, campaign fatigue, and an
    /// ideological-pressure ledger. It does NOT own faction trust/standing: real
    /// loyalty shifts happen only through the host-routed
    /// <see cref="LoyaltyShiftRequested"/> delegate, and only for the targeted
    /// faction. All values are abstract game numbers — no real-world persuasion
    /// or targeting guidance is encoded. Deterministic: ordinal iteration,
    /// intercept rolls seeded per (day, campaignId).
    /// </summary>
    public sealed class PsyOpsSystem
    {
        public const string SystemId = "psyops";
        public const float StallReachFactor = 0.3f;        // brownout broadcast
        public const float CounterPressureFactor = 0.25f;  // suppressed campaign residual
        public const float JammingReachLoss = 0.6f;        // full-strength jamming cut
        public const float FatigueAfterDays = 7;
        public const float FatigueFactor = 0.6f;
        public const float PressureDecayPerDay = 0.05f;
        public const float InterceptBaseChance = 0.15f;
        public const float InterceptJammingBonus = 0.2f;
        public const float MaxJammingStrength = 1f;

        private readonly PsyOpsCatalogContainer _catalog;
        private readonly PsyOpsState _state = new PsyOpsState();

        /// <summary>Catalog display name for a campaign id (UI labels; never persisted).</summary>
        public string CatalogDisplay(string campaignId)
        {
            var def = FindDef(campaignId);
            return def != null && !string.IsNullOrWhiteSpace(def.display_name) ? def.display_name : campaignId;
        }

        /// <summary>Transmitter gate (host: comms array tier + grid power).</summary>
        public Func<bool> TransmitterReady { get; set; } = () => false;
        /// <summary>
        /// Host-routed loyalty shift for the TARGETED faction only (host maps to
        /// the canonical authority that owns that faction's aggregate). Null-safe.
        /// </summary>
        public Action<string, float>? LoyaltyShiftRequested { get; set; }
        /// <summary>Optional external hostile-jamming fixture per faction, 0..1.</summary>
        public Func<string, float>? ExternalJamming { get; set; }

        public event Action<string, int>? OnCampaignStarted;       // campaignId, day
        public event Action<string, int>? OnCampaignExpired;       // campaignId, day
        public event Action<string, string, float, int>? OnLoyaltyShiftApplied; // factionId, campaignId, delta, day
        public event Action<string, string, float, int>? OnBroadcastIntercepted; // campaignId, factionId, confidence, day
        public event Action<string, float, int>? OnJammingStarted; // factionId, strength, day
        public event Action<string, int>? OnCounterPropagandaStarted; // campaignId, day

        public PsyOpsSystem(PsyOpsCatalogContainer catalog)
        {
            _catalog = catalog ?? new PsyOpsCatalogContainer();
        }

        public PsyOpsState State => _state;
        public IReadOnlyList<PsyOpsCampaignState> Campaigns => _state.campaigns;

        // ----------------------------------------------------------- campaigns

        private PsyOpsCampaignDef? FindDef(string campaignId)
        {
            foreach (var def in _catalog.propaganda_campaigns)
                if (def != null && string.Equals(def.id, campaignId, StringComparison.Ordinal))
                    return def;
            return null;
        }

        /// <summary>Starts a campaign (materials/power contracts are the host's). One active campaign per target.</summary>
        public bool StartCampaign(string campaignId, int day)
        {
            var def = FindDef(campaignId);
            if (def == null) return false;
            if (_state.campaigns.Any(c => c != null && string.Equals(c.campaignId, campaignId, StringComparison.Ordinal)
                                         && c.status == (int)PsyOpsCampaignStatus.Active))
                return false; // already running
            if (_state.campaigns.Any(c => c != null && c.status == (int)PsyOpsCampaignStatus.Active &&
                                          string.Equals(c.targetFactionId, def.target_faction_id, StringComparison.Ordinal)))
                return false; // one channel per target faction

            _state.campaigns.Add(new PsyOpsCampaignState
            {
                campaignId = def.id,
                targetFactionId = def.target_faction_id,
                messageTheme = def.message_theme,
                baseReach = Math.Clamp(def.base_reach, 0f, 100f),
                powerDemandWatts = Math.Max(0f, def.power_demand_watts),
                receptiveness = Math.Clamp(def.receptiveness, 0f, 1f),
                loyaltyPressurePerDay = Math.Max(0f, def.loyalty_pressure_per_day),
                startedDay = day,
                durationDays = Math.Max(1, def.duration_days)
            });
            OnCampaignStarted?.Invoke(def.id, day);
            return true;
        }

        /// <summary>Theme direction: fear pushes away, everything else pulls closer.</summary>
        public static float ThemeSign(string theme) =>
            string.Equals(theme?.Trim(), "Fear", StringComparison.OrdinalIgnoreCase) ? -1f : 1f;

        /// <summary>Pure: effective reach for a campaign under today's conditions.</summary>
        public float EffectiveReach(PsyOpsCampaignState campaign)
        {
            float reach = campaign.baseReach * (TransmitterReady() ? 1f : StallReachFactor);
            reach *= 1f - JammingFor(campaign.targetFactionId) * JammingReachLoss;
            if (_state.counters.Any(c => c != null && string.Equals(c.campaignId, campaign.campaignId, StringComparison.Ordinal)))
                reach *= CounterPressureFactor;
            if (FatigueDays(campaign.targetFactionId) >= FatigueAfterDays)
                reach *= FatigueFactor;
            return Math.Clamp(reach, 0f, 100f);
        }

        /// <summary>
        /// One deterministic broadcast day (ordinal by campaignId): reach under
        /// today's power/jamming/counter/fatigue conditions, signed ideological
        /// pressure to the targeted faction only, seeded intercept rolls.
        /// </summary>
        public void TickCampaigns(int day)
        {
            // Counters and jamming age first.
            for (int i = _state.counters.Count - 1; i >= 0; i--)
                if (--_state.counters[i].daysRemaining <= 0) _state.counters.RemoveAt(i);
            for (int i = _state.jamming.Count - 1; i >= 0; i--)
                if (--_state.jamming[i].daysRemaining <= 0) _state.jamming.RemoveAt(i);

            foreach (var campaign in _state.campaigns
                .Where(c => c != null && c.status == (int)PsyOpsCampaignStatus.Active)
                .OrderBy(c => c.campaignId, StringComparer.Ordinal)
                .ToList())
            {
                campaign.daysElapsed++;
                if (campaign.daysElapsed >= campaign.durationDays)
                {
                    campaign.status = (int)PsyOpsCampaignStatus.Expired;
                    OnCampaignExpired?.Invoke(campaign.campaignId, day);
                    continue;
                }

                float reach = EffectiveReach(campaign);
                campaign.status = TransmitterReady()
                    ? (int)PsyOpsCampaignStatus.Active
                    : (int)PsyOpsCampaignStatus.Stalled;
                AddFatigue(campaign.targetFactionId);

                float delta = ThemeSign(campaign.messageTheme)
                              * campaign.loyaltyPressurePerDay
                              * (reach / 100f)
                              * (0.5f + 0.5f * campaign.receptiveness);
                if (MathF.Abs(delta) > 0.001f)
                {
                    AddPressure(campaign.targetFactionId, delta);
                    LoyaltyShiftRequested?.Invoke(campaign.targetFactionId, delta);
                    OnLoyaltyShiftApplied?.Invoke(campaign.targetFactionId, campaign.campaignId, delta, day);
                }

                float interceptChance = InterceptBaseChance
                                        + InterceptJammingBonus * JammingFor(campaign.targetFactionId);
                var rng = new SeededRng(InterceptSeed(day, campaign.campaignId));
                if (rng.NextDouble() < interceptChance)
                    OnBroadcastIntercepted?.Invoke(campaign.campaignId, campaign.targetFactionId, reach / 100f, day);
            }

            // Pressure ledger decays toward neutral every day.
            for (int i = 0; i < _state.pressureValues.Count; i++)
                _state.pressureValues[i] *= 1f - PressureDecayPerDay;
        }

        // ------------------------------------------------------------ jamming

        /// <summary>Player-run rival jamming against a faction's airtime (abstract; costs are the host's).</summary>
        public bool StartJamming(string targetFactionId, float strength, int days, int day)
        {
            if (string.IsNullOrEmpty(targetFactionId) || days < 1) return false;
            strength = Math.Clamp(strength, 0f, MaxJammingStrength);
            if (strength <= 0f) return false;
            var existing = _state.jamming.FirstOrDefault(j => j != null &&
                string.Equals(j.targetFactionId, targetFactionId, StringComparison.Ordinal));
            if (existing != null)
            {
                existing.strength = Math.Max(existing.strength, strength);
                existing.daysRemaining = Math.Max(existing.daysRemaining, days);
            }
            else
            {
                _state.jamming.Add(new PsyOpsJammingState
                {
                    targetFactionId = targetFactionId,
                    strength = strength,
                    daysRemaining = days
                });
            }
            OnJammingStarted?.Invoke(targetFactionId, strength, day);
            return true;
        }

        public bool StartCounterPropaganda(string campaignId, int days, int day)
        {
            if (string.IsNullOrEmpty(campaignId) || days < 1) return false;
            if (!_state.campaigns.Any(c => c != null &&
                string.Equals(c.campaignId, campaignId, StringComparison.Ordinal) &&
                c.status == (int)PsyOpsCampaignStatus.Active))
                return false;
            if (_state.counters.Any(c => c != null && string.Equals(c.campaignId, campaignId, StringComparison.Ordinal)))
                return false;

            _state.counters.Add(new PsyOpsCounterState { campaignId = campaignId, daysRemaining = days });
            OnCounterPropagandaStarted?.Invoke(campaignId, day);
            return true;
        }

        private float JammingFor(string factionId)
        {
            float own = _state.jamming
                .Where(j => j != null && string.Equals(j.targetFactionId, factionId, StringComparison.Ordinal))
                .Select(j => j.strength)
                .DefaultIfEmpty(0f)
                .Max();
            float external = ExternalJamming?.Invoke(factionId) ?? 0f;
            return Math.Clamp(Math.Max(own, Math.Clamp(external, 0f, 1f)), 0f, 1f);
        }

        private int FatigueDays(string factionId)
        {
            for (int i = 0; i < _state.fatigueFactionIds.Count; i++)
                if (string.Equals(_state.fatigueFactionIds[i], factionId, StringComparison.Ordinal))
                    return _state.fatigueDays[i];
            return 0;
        }

        private void AddFatigue(string factionId)
        {
            for (int i = 0; i < _state.fatigueFactionIds.Count; i++)
            {
                if (string.Equals(_state.fatigueFactionIds[i], factionId, StringComparison.Ordinal))
                {
                    _state.fatigueDays[i]++;
                    return;
                }
            }
            _state.fatigueFactionIds.Add(factionId);
            _state.fatigueDays.Add(1);
        }

        private void AddPressure(string factionId, float delta)
        {
            for (int i = 0; i < _state.pressureFactionIds.Count; i++)
            {
                if (string.Equals(_state.pressureFactionIds[i], factionId, StringComparison.Ordinal))
                {
                    _state.pressureValues[i] = Math.Clamp(_state.pressureValues[i] + delta, -100f, 100f);
                    return;
                }
            }
            _state.pressureFactionIds.Add(factionId);
            _state.pressureValues.Add(Math.Clamp(delta, -100f, 100f));
        }

        /// <summary>Current accumulated ideological pressure on a faction, −100..100.</summary>
        public float PressureOn(string factionId)
        {
            for (int i = 0; i < _state.pressureFactionIds.Count; i++)
                if (string.Equals(_state.pressureFactionIds[i], factionId, StringComparison.Ordinal))
                    return _state.pressureValues[i];
            return 0f;
        }

        /// <summary>Stable per-(day, id) roll seed — FNV-1a over the parts.</summary>
        public static int InterceptSeed(int day, string id)
        {
            unchecked
            {
                uint hash = 2166136261u;
                foreach (char c in id ?? string.Empty) { hash ^= c; hash *= 16777619u; }
                hash ^= 0x2B; hash *= 16777619u;
                for (int i = 0; i < 4; i++)
                {
                    hash ^= (byte)((day >> (i * 8)) & 0xFF);
                    hash *= 16777619u;
                }
                return (int)hash;
            }
        }

        // -------------------------------------------------------- persistence

        public PsyOpsState CaptureState()
        {
            var copy = new PsyOpsState();
            foreach (var c in _state.campaigns)
                copy.campaigns.Add(new PsyOpsCampaignState
                {
                    campaignId = c.campaignId, targetFactionId = c.targetFactionId,
                    messageTheme = c.messageTheme, baseReach = c.baseReach,
                    powerDemandWatts = c.powerDemandWatts, receptiveness = c.receptiveness,
                    loyaltyPressurePerDay = c.loyaltyPressurePerDay, startedDay = c.startedDay,
                    durationDays = c.durationDays, daysElapsed = c.daysElapsed, status = c.status
                });
            foreach (var j in _state.jamming)
                copy.jamming.Add(new PsyOpsJammingState
                { targetFactionId = j.targetFactionId, strength = j.strength, daysRemaining = j.daysRemaining });
            foreach (var c in _state.counters)
                copy.counters.Add(new PsyOpsCounterState { campaignId = c.campaignId, daysRemaining = c.daysRemaining });
            copy.pressureFactionIds = new List<string>(_state.pressureFactionIds);
            copy.pressureValues = new List<float>(_state.pressureValues);
            copy.fatigueFactionIds = new List<string>(_state.fatigueFactionIds);
            copy.fatigueDays = new List<int>(_state.fatigueDays);
            return copy;
        }

        /// <summary>NON-OPERATIVE restore: reconstructs state; applies no pressure.</summary>
        public void RestoreState(PsyOpsState state)
        {
            _state.campaigns.Clear();
            _state.jamming.Clear();
            _state.counters.Clear();
            _state.pressureFactionIds.Clear();
            _state.pressureValues.Clear();
            _state.fatigueFactionIds.Clear();
            _state.fatigueDays.Clear();
            if (state == null) return;

            foreach (var c in state.campaigns ?? new List<PsyOpsCampaignState>())
                if (c != null && !string.IsNullOrEmpty(c.campaignId))
                    _state.campaigns.Add(new PsyOpsCampaignState
                    {
                        campaignId = c.campaignId, targetFactionId = c.targetFactionId ?? string.Empty,
                        messageTheme = c.messageTheme ?? string.Empty,
                        baseReach = Math.Clamp(c.baseReach, 0f, 100f),
                        powerDemandWatts = Math.Max(0f, c.powerDemandWatts),
                        receptiveness = Math.Clamp(c.receptiveness, 0f, 1f),
                        loyaltyPressurePerDay = Math.Max(0f, c.loyaltyPressurePerDay),
                        startedDay = c.startedDay, durationDays = Math.Max(1, c.durationDays),
                        daysElapsed = Math.Max(0, c.daysElapsed),
                        status = Math.Clamp(c.status, 0, (int)PsyOpsCampaignStatus.Expired)
                    });
            foreach (var j in state.jamming ?? new List<PsyOpsJammingState>())
                if (j != null && !string.IsNullOrEmpty(j.targetFactionId))
                    _state.jamming.Add(new PsyOpsJammingState
                    {
                        targetFactionId = j.targetFactionId,
                        strength = Math.Clamp(j.strength, 0f, MaxJammingStrength),
                        daysRemaining = Math.Max(0, j.daysRemaining)
                    });
            foreach (var c in state.counters ?? new List<PsyOpsCounterState>())
                if (c != null && !string.IsNullOrEmpty(c.campaignId))
                    _state.counters.Add(new PsyOpsCounterState
                    { campaignId = c.campaignId, daysRemaining = Math.Max(0, c.daysRemaining) });
            foreach (var id in state.pressureFactionIds ?? new List<string>())
                if (!string.IsNullOrEmpty(id)) _state.pressureFactionIds.Add(id);
            foreach (var v in state.pressureValues ?? new List<float>())
                _state.pressureValues.Add(Math.Clamp(v, -100f, 100f));
            foreach (var id in state.fatigueFactionIds ?? new List<string>())
                if (!string.IsNullOrEmpty(id)) _state.fatigueFactionIds.Add(id);
            foreach (var d in state.fatigueDays ?? new List<int>())
                _state.fatigueDays.Add(Math.Max(0, d));
        }
    }
}
