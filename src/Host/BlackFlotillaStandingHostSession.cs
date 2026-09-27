// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Black Flotilla Standing — host adapter over the ALREADY-LIVE
// FactionStanceEngine held by DeepCoastHostSession.
//
// FactionStanceEngine remains the only trust store. This adapter does one thing:
// register the flotilla's AUTHORED thresholds and Plan-23 tier verdicts so the
// faction stops falling back to the engine's generic synthesised defaults.
// ============================================================================

using System;
using Ashfall.Core.Economy;
using Ashfall.Core.Maritime;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Registers <see cref="BlackFlotillaStanding.Thresholds"/> on the live
    /// <see cref="FactionStanceEngine"/> and exposes the authored tier verdicts.
    /// Holds no trust of its own.
    /// </summary>
    public sealed class BlackFlotillaStandingHostSession : HostSessionBase
    {
        private readonly Func<FactionStanceEngine?> _stanceProvider;
        private bool _registered;

        public string LastEvent { get; private set; } = string.Empty;
        public bool IsRegistered => _registered;
        public static string FactionId => BlackFlotillaStanding.FactionId;

        public BlackFlotillaStandingHostSession(Func<FactionStanceEngine?> stanceProvider)
        {
            _stanceProvider = stanceProvider ?? throw new ArgumentNullException(nameof(stanceProvider));
        }

        /// <summary>
        /// Registers the authored thresholds once on the live engine. Idempotent:
        /// a second call re-affirms the same authored row and never a copy.
        /// </summary>
        public bool Register()
        {
            var engine = _stanceProvider();
            if (engine == null)
            {
                LastEvent = "Flotilla standing unavailable: no live stance engine.";
                return false;
            }
            BlackFlotillaStanding.Register(engine);
            _registered = true;
            LastEvent = $"Registered authored Black Flotilla thresholds on faction '{FactionId}'.";
            RaiseStateChanged();
            return true;
        }

        /// <summary>Current trust the live engine holds for the flotilla.</summary>
        public float Trust()
        {
            var engine = _stanceProvider();
            return engine == null ? 0f : engine.GetEffectiveTrust(FactionId);
        }

        /// <summary>Authored tier for the flotilla's live trust.</summary>
        public BlackFlotillaTier Tier() => BlackFlotillaStanding.TierFor(Trust());

        // ── Authored gates, evaluated on live trust ────────────────────────
        public bool CanTrade() => BlackFlotillaStanding.CanTrade(Trust());
        public bool CanShareIntel() => BlackFlotillaStanding.CanShareIntel(Trust());
        public bool IsSalvageTrusted() => BlackFlotillaStanding.IsSalvageTrusted(Trust());
        public bool CanCooperateOnDeepDives() => BlackFlotillaStanding.CanCooperateOnDeepDives(Trust());

        /// <summary>The exact reason a gate refused, or empty when it allows.</summary>
        public string RefusalReason(string gate)
        {
            float trust = Trust();
            switch (gate?.ToLowerInvariant())
            {
                case "trade":
                    return trust >= BlackFlotillaStanding.MinTrustToTrade
                        ? string.Empty : $"trade needs trust ≥ {BlackFlotillaStanding.MinTrustToTrade:0} (have {trust:0})";
                case "intel":
                    return trust >= BlackFlotillaStanding.IntelShareThreshold
                        ? string.Empty : $"intel sharing needs trust ≥ {BlackFlotillaStanding.IntelShareThreshold:0} (have {trust:0})";
                case "salvage":
                    return trust >= BlackFlotillaStanding.SalvageTrustedTrust
                        ? string.Empty : $"salvage trust needs tier ≥ SalvageTrusted (have {trust:0})";
                case "deepdive":
                    return trust >= BlackFlotillaStanding.DeepCooperationTrust
                        ? string.Empty : $"deep-cooperation needs trust ≥ {BlackFlotillaStanding.DeepCooperationTrust:0} (have {trust:0})";
                case "raid":
                    return trust <= BlackFlotillaStanding.RaidThreshold
                        ? string.Empty : $"no raid at trust {trust:0} (threshold {BlackFlotillaStanding.RaidThreshold:0})";
                default:
                    return "unknown_gate";
            }
        }

        /// <summary>
        /// True when the live engine has not been given the authored flotilla row.
        /// Uses only the engine's public surface, so this stays a read-only probe.
        /// </summary>
        public bool IsRunningOnGenericDefaults()
        {
            var engine = _stanceProvider();
            if (engine == null) return true;
            if (!engine.IsFactionActive(FactionId)) return true;
            return Math.Abs(engine.GetRaidAggression(FactionId) - BlackFlotillaStanding.RaidAggression) > 0.0001f;
        }

        /// <summary>Truthful projection for a status readout.</summary>
        public string StatusLine()
        {
            if (!_registered) return "authored thresholds not registered";
            return $"flotilla trust {Trust():0} · tier {Tier()}"
                 + $" · trade {(CanTrade() ? "open" : "closed")}"
                 + $" · intel {(CanShareIntel() ? "open" : "closed")}";
        }
    }
}
