namespace Ashfall.Core.Economy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Engine-agnostic faction stance provider. Owns trust state and faction
    /// threshold data; exposes stance queries through the interface below.
    /// All external data (day, radiation, survivors, quests) flows in through
    /// Func providers so the core never touches UnityEngine types.
    /// </summary>
    public sealed class FactionStanceEngine : IFactionStanceProvider
    {
        // ── State ─────────────────────────────────────────────────────
        private readonly Dictionary<string, float> _trust = new();
        private readonly Dictionary<string, FactionThresholds> _factions = new();
        private readonly Dictionary<string, float> _aggressionOverrides = new();

        // ── Host-wired providers (null-safe defaults) ────────────────
        public Func<int> DayProvider { get; set; } = () => 0;
        public Func<float> PartyRadiationProvider { get; set; } = () => -1f;
        public Func<bool> PartyHasArsProvider { get; set; } = () => false;
        public Func<bool> PartyIntactHazmatProvider { get; set; } = () => false;
        public Func<bool> HasHatedMilitarySurvivor { get; set; } = () => false;
        public Func<float, float> ClampTrustProvider { get; set; } = v => v;
        public Func<string, bool> IsMilitaryFaction { get; set; } = id => false;

        // ── IFactionStanceProvider ───────────────────────────────────
        public float GetTrust(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return 0f;
            if (_trust.TryGetValue(factionId, out float stored)) return stored;
            return 0f;
        }

        public float GetEffectiveTrust(string factionId)
        {
            float stored = GetTrust(factionId);
            if (IsMilitaryFaction != null && IsMilitaryFaction(factionId) && HasHatedMilitarySurvivor != null && HasHatedMilitarySurvivor())
                return FactionStanceConstants.MinTrust;

            if (!TryGetThresholds(factionId, out var fac) || !fac.TrustInversion)
                return stored;

            // ARS reverence outranks dose and hazmat.
            if (PartyHasArsProvider != null && PartyHasArsProvider()) return FactionStanceConstants.MaxTrust;

            float rad = PartyRadiationProvider != null ? PartyRadiationProvider() : -1f;
            bool intactHazmat = PartyIntactHazmatProvider != null && PartyIntactHazmatProvider();

            // Hazmat / zero-rad contempt: sealed clean blood is heresy.
            if (intactHazmat)
            {
                if (rad < 0f) return FactionStanceConstants.MinTrust;
                float hazCeiling = Math.Max(0f, Math.Min(100f, fac.HealthyRadiationCeiling));
                if (rad <= hazCeiling) return FactionStanceConstants.MinTrust;
            }

            if (rad < 0f) return stored;

            float ceiling = Math.Max(0f, Math.Min(100f, fac.HealthyRadiationCeiling));
            float floor = Math.Max(0f, Math.Min(100f, fac.HighRadiationFloor));
            if (floor <= ceiling) floor = Math.Min(100f, ceiling + 1f);

            if (rad <= ceiling) return FactionStanceConstants.MinTrust;
            if (rad >= floor) return FactionStanceConstants.MaxTrust;
            // Linear interpolation between floor and ceiling.
            return -100f + 200f * (rad - ceiling) / (floor - ceiling);
        }

        public float ModifyTrust(string factionId, float delta)
        {
            if (string.IsNullOrEmpty(factionId)) return GetTrust(factionId);
            float next = GetTrust(factionId) + delta;
            next = ApplyTrustClamp(next, factionId);
            SetTrust(factionId, next);
            return next;
        }

        public void SetTrust(string factionId, float value)
        {
            if (string.IsNullOrEmpty(factionId)) return;
            _trust[factionId] = value;
        }

        public TradeStance GetStance(string factionId)
        {
            if (!IsFactionActive(factionId))
                return TradeStance.Refuse;

            float trust = GetEffectiveTrust(factionId);
            if (!TryGetThresholds(factionId, out var fac))
            {
                fac = new FactionThresholds(
                    factionId,
                    raidThreshold: -50f,
                    robThreshold: -20f,
                    minTrustToTrade: -40f,
                    intelShareThreshold: 40f);
            }

            if (trust <= fac.RaidThreshold) return TradeStance.HostileRaid;
            if (trust <= fac.RobThreshold) return TradeStance.Rob;
            if (trust < fac.MinTrustToTrade) return TradeStance.Refuse;
            if (trust >= fac.IntelShareThreshold) return TradeStance.ShareIntel;
            return TradeStance.Trade;
        }

        public bool WillTrade(string factionId)
        {
            var s = GetStance(factionId);
            return s == TradeStance.Trade || s == TradeStance.ShareIntel;
        }

        public bool WillShareIntel(string factionId) => GetStance(factionId) == TradeStance.ShareIntel;

        public float GetRaidAggression(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return 0.5f;
            if (_aggressionOverrides.TryGetValue(factionId, out float ovr))
                return Math.Max(0f, Math.Min(1f, ovr));
            if (!TryGetThresholds(factionId, out var fac)) return 0.5f;
            return fac.RaidAggression;
        }

        public void SetRaidAggression(string factionId, float value)
        {
            if (string.IsNullOrEmpty(factionId)) return;
            _aggressionOverrides[factionId] = Math.Max(0f, Math.Min(1f, value));
        }

        public bool IsFactionActive(string factionId)
        {
            if (!TryGetThresholds(factionId, out var fac)) return false;
            if (!fac.TrustInversion) return true;
            if (DayProvider == null) return true;
            return DayProvider() >= FactionStanceConstants.CultActivationDay;
        }

        // ── Configuration ────────────────────────────────────────────
        public void RegisterFaction(FactionThresholds thresholds)
        {
            if (string.IsNullOrEmpty(thresholds.FactionId)) return;
            _factions[thresholds.FactionId] = thresholds;
        }

        public void RegisterFactions(IEnumerable<FactionThresholds> thresholds)
        {
            if (thresholds == null) return;
            foreach (var t in thresholds) RegisterFaction(t);
        }

        public IReadOnlyDictionary<string, float> SnapshotTrust() => _trust;

        // ── Helpers ──────────────────────────────────────────────────
        private bool TryGetThresholds(string factionId, out FactionThresholds thresholds)
        {
            if (!string.IsNullOrEmpty(factionId) && _factions.TryGetValue(factionId, out thresholds))
                return true;
            thresholds = default;
            return false;
        }

        private float ApplyTrustClamp(float raw, string factionId)
        {
            var hostClamped = ClampTrustProvider != null ? ClampTrustProvider(raw) : raw;
            if (Math.Abs(hostClamped - raw) > 0.001f) return hostClamped;

            // Built-in clamp: [-100, 100].
            return Math.Max(-100f, Math.Min(100f, raw));
        }
    }
}
