// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Medical
{
    public enum GraftBiocompatibilityTier
    {
        Autograft = 0,
        AllograftMatched = 1,
        AllograftUnmatched = 2,
        XenograftSynthetic = 3
    }

    public enum GraftStatus
    {
        Integrating = 0,
        FullyIntegrated = 1,
        Rejected = 2,
        Infected = 3
    }

    [Serializable]
    public sealed class SurgicalGraftRecordSaveState
    {
        public string GraftId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string LimbKey { get; set; } = string.Empty;
        public string DonorSourceId { get; set; } = string.Empty;
        public GraftBiocompatibilityTier Tier { get; set; }
        public int GraftDay { get; set; }
        public GraftStatus Status { get; set; }
        public int IntegrationProgressPermille { get; set; }
        public int RejectionRiskPermille { get; set; }
        public int ImmunosuppressantLevelPermille { get; set; }
        public int DaysWithoutImmunosuppressant { get; set; }
        public int LastUpdateDay { get; set; }
    }

    [Serializable]
    public sealed class SurgicalGraftRejectionEngineSaveState
    {
        public int schema_version { get; set; } = 1;
        public List<SurgicalGraftRecordSaveState> Grafts { get; set; } = new();
    }

    public sealed class SurgicalGraftRecord
    {
        public string GraftId { get; }
        public string SurvivorId { get; }
        public string LimbKey { get; }
        public string DonorSourceId { get; }
        public GraftBiocompatibilityTier Tier { get; }
        public int GraftDay { get; }
        public GraftStatus Status { get; set; }
        public int IntegrationProgressPermille { get; set; }
        public int RejectionRiskPermille { get; set; }
        public int ImmunosuppressantLevelPermille { get; set; }
        public int DaysWithoutImmunosuppressant { get; set; }
        public int LastUpdateDay { get; set; }

        public SurgicalGraftRecord(
            string graftId,
            string survivorId,
            string limbKey,
            string donorSourceId,
            GraftBiocompatibilityTier tier,
            int graftDay)
        {
            GraftId = graftId ?? throw new ArgumentNullException(nameof(graftId));
            SurvivorId = survivorId ?? throw new ArgumentNullException(nameof(survivorId));
            LimbKey = limbKey ?? throw new ArgumentNullException(nameof(limbKey));
            DonorSourceId = donorSourceId ?? string.Empty;
            Tier = tier;
            GraftDay = graftDay;
            Status = GraftStatus.Integrating;
            IntegrationProgressPermille = 0;
            RejectionRiskPermille = 1000 - SurgicalGraftRejectionEngine.GetBaseBiocompatibilityPermille(tier);
            ImmunosuppressantLevelPermille = 500; // Standard initial dose at surgery
            DaysWithoutImmunosuppressant = 0;
            LastUpdateDay = graftDay;
        }

        internal SurgicalGraftRecord(SurgicalGraftRecordSaveState state)
        {
            GraftId = state.GraftId;
            SurvivorId = state.SurvivorId;
            LimbKey = state.LimbKey;
            DonorSourceId = state.DonorSourceId;
            Tier = state.Tier;
            GraftDay = state.GraftDay;
            Status = state.Status;
            IntegrationProgressPermille = state.IntegrationProgressPermille;
            RejectionRiskPermille = state.RejectionRiskPermille;
            ImmunosuppressantLevelPermille = state.ImmunosuppressantLevelPermille;
            DaysWithoutImmunosuppressant = state.DaysWithoutImmunosuppressant;
            LastUpdateDay = state.LastUpdateDay;
        }

        public SurgicalGraftRecordSaveState CaptureState()
        {
            return new SurgicalGraftRecordSaveState
            {
                GraftId = GraftId,
                SurvivorId = SurvivorId,
                LimbKey = LimbKey,
                DonorSourceId = DonorSourceId,
                Tier = Tier,
                GraftDay = GraftDay,
                Status = Status,
                IntegrationProgressPermille = IntegrationProgressPermille,
                RejectionRiskPermille = RejectionRiskPermille,
                ImmunosuppressantLevelPermille = ImmunosuppressantLevelPermille,
                DaysWithoutImmunosuppressant = DaysWithoutImmunosuppressant,
                LastUpdateDay = LastUpdateDay
            };
        }
    }

    /// <summary>
    /// Expansion 16 / UNBLOCK-01 §5.9 / UNBLOCK-05 §3.1:
    /// The Rebuilt Body — Surgical Grafting & Rejection Engine.
    /// Manages biological tissue graft compatibility tiers, daily tissue integration,
    /// immunosuppressant tracking, and immune rejection risks over survivor limb state.
    /// </summary>
    public sealed class SurgicalGraftRejectionEngine
    {
        public const int DailyIntegrationRatePermille = 50; // 5% integration progress per day
        public const int DailyImmunosuppressantDecayPermille = 200; // 20% decay per day
        public const int ImmunosuppressantThresholdPermille = 250; // Below this, rejection risk escalates

        private readonly List<SurgicalGraftRecord> _grafts = new();

        public IReadOnlyList<SurgicalGraftRecord> Grafts => _grafts;

        public event Action<SurgicalGraftRecord>? OnGraftIntegrated;
        public event Action<SurgicalGraftRecord>? OnGraftRejected;
        public event Action<SurgicalGraftRecord, int>? OnRejectionRiskIncreased;

        public static int GetBaseBiocompatibilityPermille(GraftBiocompatibilityTier tier)
        {
            return tier switch
            {
                GraftBiocompatibilityTier.Autograft => 950,
                GraftBiocompatibilityTier.AllograftMatched => 750,
                GraftBiocompatibilityTier.AllograftUnmatched => 500,
                GraftBiocompatibilityTier.XenograftSynthetic => 350,
                _ => 500
            };
        }

        public SurgicalGraftRecord PerformGraft(
            string survivorId,
            string limbKey,
            string donorSourceId,
            GraftBiocompatibilityTier tier,
            int currentDay)
        {
            if (string.IsNullOrWhiteSpace(survivorId))
                throw new ArgumentException("SurvivorId cannot be empty.", nameof(survivorId));
            if (string.IsNullOrWhiteSpace(limbKey))
                throw new ArgumentException("LimbKey cannot be empty.", nameof(limbKey));

            string graftId = $"graft_{survivorId}_{limbKey}_{currentDay}_{_grafts.Count + 1}";
            var record = new SurgicalGraftRecord(graftId, survivorId, limbKey, donorSourceId, tier, currentDay);
            _grafts.Add(record);
            return record;
        }

        public SurgicalGraftRecord? GetGraft(string graftId)
        {
            if (string.IsNullOrWhiteSpace(graftId)) return null;
            return _grafts.FirstOrDefault(g => string.Equals(g.GraftId, graftId, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<SurgicalGraftRecord> GetGraftsForSurvivor(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return Array.Empty<SurgicalGraftRecord>();
            return _grafts.Where(g => string.Equals(g.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public bool AdministerImmunosuppressant(string graftId, int dosePermille, int currentDay)
        {
            var graft = GetGraft(graftId);
            if (graft == null || graft.Status != GraftStatus.Integrating) return false;

            graft.ImmunosuppressantLevelPermille = Math.Clamp(graft.ImmunosuppressantLevelPermille + dosePermille, 0, 1000);
            graft.DaysWithoutImmunosuppressant = 0;
            return true;
        }

        public void ProcessDailyTick(int currentDay, Func<string, int, int>? seededRngRoll = null)
        {
            for (int i = 0; i < _grafts.Count; i++)
            {
                var graft = _grafts[i];
                if (graft.Status != GraftStatus.Integrating) continue;
                if (currentDay <= graft.LastUpdateDay) continue;

                int elapsedDays = currentDay - graft.LastUpdateDay;

                for (int d = 0; d < elapsedDays; d++)
                {
                    if (graft.Status != GraftStatus.Integrating) break;

                    // Decay immunosuppressant
                    graft.ImmunosuppressantLevelPermille = Math.Max(0, graft.ImmunosuppressantLevelPermille - DailyImmunosuppressantDecayPermille);

                    if (graft.ImmunosuppressantLevelPermille < ImmunosuppressantThresholdPermille)
                    {
                        graft.DaysWithoutImmunosuppressant++;
                        int baseComp = GetBaseBiocompatibilityPermille(graft.Tier);
                        int riskIncr = ((1000 - baseComp) / 10) + (graft.DaysWithoutImmunosuppressant * 20);
                        graft.RejectionRiskPermille = Math.Min(1000, graft.RejectionRiskPermille + riskIncr);
                        OnRejectionRiskIncreased?.Invoke(graft, riskIncr);

                        // Rejection check using deterministic seeded roll
                        if (seededRngRoll != null)
                        {
                            int roll = seededRngRoll(graft.GraftId, currentDay);
                            if (roll >= 0 && roll < graft.RejectionRiskPermille)
                            {
                                graft.Status = GraftStatus.Rejected;
                                OnGraftRejected?.Invoke(graft);
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Adequate immunosuppression: recover risk and advance integration
                        graft.DaysWithoutImmunosuppressant = 0;
                        graft.RejectionRiskPermille = Math.Max(0, graft.RejectionRiskPermille - 30);

                        graft.IntegrationProgressPermille += DailyIntegrationRatePermille;
                        if (graft.IntegrationProgressPermille >= 1000)
                        {
                            graft.IntegrationProgressPermille = 1000;
                            graft.Status = GraftStatus.FullyIntegrated;
                            graft.RejectionRiskPermille = 0;
                            OnGraftIntegrated?.Invoke(graft);
                            break;
                        }
                    }
                }

                graft.LastUpdateDay = currentDay;
            }
        }

        public SurgicalGraftRejectionEngineSaveState CaptureState()
        {
            var save = new SurgicalGraftRejectionEngineSaveState
            {
                schema_version = 1,
                Grafts = new List<SurgicalGraftRecordSaveState>(_grafts.Count)
            };

            for (int i = 0; i < _grafts.Count; i++)
            {
                save.Grafts.Add(_grafts[i].CaptureState());
            }

            return save;
        }

        public void RestoreState(SurgicalGraftRejectionEngineSaveState? state)
        {
            _grafts.Clear();
            if (state?.Grafts == null) return;

            for (int i = 0; i < state.Grafts.Count; i++)
            {
                var g = state.Grafts[i];
                if (g != null)
                {
                    _grafts.Add(new SurgicalGraftRecord(g));
                }
            }
        }
    }
}
