using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Hostage Situations & Ransoms (Prompt #73). When an encounter goes
    /// catastrophically wrong, the survivor is captured, not killed. The
    /// EventRunner triggers a Radio broadcast demanding 50 CleanWater and
    /// 10 AntiRad for their return. A massive economic shock that forces
    /// the player to drain reserves or abandon a friend.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class HostageSystem
    {
        /// <summary>Default ransom: clean water units demanded.</summary>
        public const float DefaultRansomWater = 50f;

        /// <summary>Default ransom: anti-rad units demanded.</summary>
        public const float DefaultRansomAntiRad = 10f;

        /// <summary>Event id for the radio ransom broadcast.</summary>
        public const string RansomEventId = "hostage_ransom_demand";

        /// <summary>Hours until ransom expires (survivor executed).</summary>
        public const float RansomExpireHours = 72f;

        /// <summary>Morale penalty for abandoning a hostage.</summary>
        public const float AbandonMoralePenalty = 30f;

        /// <summary>Morale recovery when a hostage is rescued.</summary>
        public const float RescueMoraleBoost = 15f;

        /// <summary>Trust penalty with the captor faction on abandon.</summary>
        public const float AbandonTrustPenalty = -25f;

        /// <summary>Active hostage situations.</summary>
        public class HostageSituation
        {
            public string ExpeditionId;
            public string SurvivorId;
            public string SurvivorName;
            public string CaptorFactionId;
            public float HoursUntilExpire;
            public float RansomWater;
            public float RansomAntiRad;
            public bool RansomPaid;
            public bool Expired;
        }

        private readonly List<HostageSituation> _activeHostages = new List<HostageSituation>();
        private int _seq;

        // -- Events --
        public event Action<HostageSituation> OnHostageTaken;
        public event Action<HostageSituation> OnRansomPaid;
        public event Action<HostageSituation> OnHostageExecuted;
        public event Action<HostageSituation> OnHostageAbandoned;

        public IReadOnlyList<HostageSituation> ActiveHostages => _activeHostages;

        public HostageSystem() { }

        /// <summary>
        /// Capture an expedition survivor. Called when an encounter goes
        /// catastrophically wrong.
        /// </summary>
        public HostageSituation CaptureSurvivor(
            ExpeditionState exp, string captorFactionId,
            float ransomWater = DefaultRansomWater,
            float ransomAntiRad = DefaultRansomAntiRad)
        {
            if (exp == null || string.IsNullOrEmpty(captorFactionId)) return null;
            if (exp.Phase == ExpeditionPhase.Captured) return null;

            exp.Phase = ExpeditionPhase.Captured;
            exp.CaptorFactionId = captorFactionId;
            exp.RansomCleanWater = ransomWater;
            exp.RansomAntiRad = ransomAntiRad;

            var situation = new HostageSituation
            {
                ExpeditionId = exp.ExpeditionId,
                SurvivorId = exp.SurvivorId,
                SurvivorName = exp.Survivor?.DisplayName ?? "Unknown",
                CaptorFactionId = captorFactionId,
                HoursUntilExpire = RansomExpireHours,
                RansomWater = ransomWater,
                RansomAntiRad = ransomAntiRad,
                RansomPaid = false,
                Expired = false
            };
            _activeHostages.Add(situation);
            OnHostageTaken?.Invoke(situation);
            return situation;
        }

        /// <summary>
        /// Pay the ransom. Drains water and anti-rad from storage.
        /// Returns true if the hostage is freed.
        /// </summary>
        public bool PayRansom(string expeditionId,
            Func<float, float> consumeCleanWater,
            Func<float, float> consumeAntiRad)
        {
            var sit = FindByExpedition(expeditionId);
            if (sit == null || sit.RansomPaid || sit.Expired) return false;

            // Check if we can afford it.
            if (consumeCleanWater == null || consumeAntiRad == null) return false;

            float waterTaken = consumeCleanWater(sit.RansomWater);
            float antiRadTaken = consumeAntiRad(sit.RansomAntiRad);

            if (waterTaken < sit.RansomWater || antiRadTaken < sit.RansomAntiRad)
            {
                // Insufficient — refund what was taken and fail.
                // (Refund handled by caller if needed.)
                return false;
            }

            sit.RansomPaid = true;
            OnRansomPaid?.Invoke(sit);
            return true;
        }

        /// <summary>
        /// Tick hostage timers. Expired hostages are executed.
        /// </summary>
        public void Tick(float gameHours, Action<HostageSituation> onExecute = null)
        {
            if (gameHours <= 0f) return;

            for (int i = _activeHostages.Count - 1; i >= 0; i--)
            {
                var sit = _activeHostages[i];
                if (sit.RansomPaid) { _activeHostages.RemoveAt(i); continue; }

                sit.HoursUntilExpire -= gameHours;
                if (sit.HoursUntilExpire <= 0f && !sit.Expired)
                {
                    sit.Expired = true;
                    OnHostageExecuted?.Invoke(sit);
                    onExecute?.Invoke(sit);
                }
            }
        }

        /// <summary>
        /// Player chooses to abandon the hostage (can't/won't pay).
        /// </summary>
        public void AbandonHostage(string expeditionId,
            IReadOnlyList<Survivors.Survivor> survivors)
        {
            var sit = FindByExpedition(expeditionId);
            if (sit == null) return;
            sit.Expired = true;

            // Morale penalty to all survivors.
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || !sv.IsAlive) continue;
                    if (sv.Id == sit.SurvivorId) continue;
                    sv.Needs.Morale = Mathf.Clamp(
                        sv.Needs.Morale - AbandonMoralePenalty, 0f, 100f);
                }
            }

            OnHostageAbandoned?.Invoke(sit);
        }

        private HostageSituation FindByExpedition(string expeditionId)
        {
            if (string.IsNullOrEmpty(expeditionId)) return null;
            for (int i = 0; i < _activeHostages.Count; i++)
                if (_activeHostages[i].ExpeditionId == expeditionId)
                    return _activeHostages[i];
            return null;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public HostageSave CaptureState()
        {
            var entries = new HostageEntrySave[_activeHostages.Count];
            for (int i = 0; i < _activeHostages.Count; i++)
            {
                var h = _activeHostages[i];
                entries[i] = new HostageEntrySave
                {
                    ExpeditionId = h.ExpeditionId,
                    SurvivorId = h.SurvivorId,
                    CaptorFactionId = h.CaptorFactionId,
                    HoursUntilExpire = h.HoursUntilExpire,
                    RansomWater = h.RansomWater,
                    RansomAntiRad = h.RansomAntiRad,
                    RansomPaid = h.RansomPaid,
                    Expired = h.Expired
                };
            }
            return new HostageSave { Entries = entries };
        }

        public void RestoreState(HostageSave save)
        {
            _activeHostages.Clear();
            if (save?.Entries == null) return;
            for (int i = 0; i < save.Entries.Length; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.ExpeditionId)) continue;
                _activeHostages.Add(new HostageSituation
                {
                    ExpeditionId = e.ExpeditionId,
                    SurvivorId = e.SurvivorId,
                    CaptorFactionId = e.CaptorFactionId,
                    HoursUntilExpire = e.HoursUntilExpire,
                    RansomWater = e.RansomWater,
                    RansomAntiRad = e.RansomAntiRad,
                    RansomPaid = e.RansomPaid,
                    Expired = e.Expired
                });
            }
        }
    }

    [Serializable]
    public class HostageSave
    {
        public HostageEntrySave[] Entries;
    }

    [Serializable]
    public class HostageEntrySave
    {
        public string ExpeditionId;
        public string SurvivorId;
        public string CaptorFactionId;
        public float HoursUntilExpire;
        public float RansomWater;
        public float RansomAntiRad;
        public bool RansomPaid;
        public bool Expired;
    }
}
