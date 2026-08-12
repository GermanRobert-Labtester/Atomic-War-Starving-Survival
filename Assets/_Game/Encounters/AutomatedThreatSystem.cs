using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Encounters
{
    /// <summary>
    /// Expansion III — Automated Threat System (The Dead Hand).
    /// Manages "Ghost Sentries" — pneumatic, rusted machine-gun emplacements
    /// executing their final targeting loops on degrading batteries.
    ///
    /// Ghost Sentries:
    ///   Each sentry has AmmoBeltDurability (0..100). The sentry fires at
    ///   any AcousticSignature above its threshold. Players can deploy
    ///   item_acoustic_decoy to force the sentry to fire until its belt
    ///   snaps or its barrel melts (Hazard_CookOff), rendering it safe
    ///   to scavenge for item_tactical_scrap.
    ///
    /// Loitering Munitions:
    ///   Slow, buzzing suicide drones attracted by high acoustic signatures.
    ///   They crash through shelter surface intake valves.
    ///
    /// Save/load safe. Plain C#. No MonoBehaviour.
    /// </summary>
    [Serializable]
    public class AutomatedThreatSystemSave
    {
        public string systemId = "automated_threats";
        public List<GhostSentryState> sentries = new List<GhostSentryState>();
        public int totalSentriesBurnedOut;
        public int totalMunitionsAttracted;
        public int totalDecoysDeployed;
    }

    [Serializable]
    public class GhostSentryState
    {
        public string sentryId;
        public string locationNodeId;
        public float ammoBeltDurability = 100f;
        public float barrelHeat;
        public bool isActive = true;
        public bool burnedOut;
        public float acousticThreshold = 30f;
        public float roundsFired;
        public float hoursSinceLastShot;
    }

    public struct SentryFireEvent
    {
        public string SentryId;
        public string TargetNodeId;
        public float RoundsFired;
        public float BarrelHeat;
        public bool BeltSnapped;
    }

    public struct SentryBurnoutEvent
    {
        public string SentryId;
        public string LocationNodeId;
        public float TotalRoundsFired;
        public bool BarrelMelted;
    }

    public struct LoiteringMunitionStrikeEvent
    {
        public string SurvivorId;
        public string RoomId;
        public float AcousticSignature;
    }

    public class AutomatedThreatSystem
    {
        /// <summary>Rounds per burst when sentry detects acoustic anomaly.</summary>
        public const float RoundsPerBurst = 15f;

        /// <summary>Ammo belt degradation per round fired (%).</summary>
        public const float BeltDegradationPerRound = 0.5f;

        /// <summary>Barrel heat added per round fired (°C).</summary>
        public const float HeatPerRound = 2.5f;

        /// <summary>Barrel heat threshold for cook-off (°C).</summary>
        public const float BarrelCookOffThreshold = 400f;

        /// <summary>Natural barrel cooling per hour (°C).</summary>
        public const float BarrelCoolingPerHour = 15f;

        /// <summary>Hours of pneumatic pressure decay before sentry fails.</summary>
        public const float PneumaticFailureHours = 48f;

        /// <summary>Health damage from sentry burst fire on a survivor.</summary>
        public const float SentryBurstDamage = 25f;

        /// <summary>Health damage from loitering munition strike.</summary>
        public const float MunitionStrikeDamage = 40f;

        /// <summary>Chance per hour for loitering munition to hit shelter intake.</summary>
        public const float MunitionIntakeHitChance = 0.08f;

        /// <summary>Tactical scrap yield from a burned-out sentry.</summary>
        public const int ScrapYieldPerSentry = 3;

        /// <summary>Acoustic signature threshold for loitering munition attraction. Mirrors UXOFieldSystem constant.</summary>
        public const float LoiteringMunitionThreshold = 60f;

        /// <summary>Chance per hour per 10 points above threshold to attract a munition. Mirrors UXOFieldSystem constant.</summary>
        public const float MunitionAttractionChancePer10 = 0.12f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<SentryFireEvent> OnSentryFired;
        public event Action<SentryBurnoutEvent> OnSentryBurnedOut;
        public event Action<LoiteringMunitionStrikeEvent> OnMunitionStrike;
        public event Action OnThreatStateChanged;

        // ── State ─────────────────────────────────────────────────────
        private readonly Dictionary<string, GhostSentryState> _sentries = new Dictionary<string, GhostSentryState>();
        private int _totalSentriesBurnedOut;
        private int _totalMunitionsAttracted;
        private int _totalDecoysDeployed;

        // Host callbacks
        public Action<string, float> ApplyHealthDamage; // survivorId, damage
        public Action<string, string> InflictAffliction; // survivorId, afflictionId
        public Action<string, int> GrantItem; // itemId, count

        public IReadOnlyDictionary<string, GhostSentryState> Sentries => _sentries;
        public int TotalSentriesBurnedOut => _totalSentriesBurnedOut;
        public int ActiveSentryCount
        {
            get
            {
                int count = 0;
                foreach (var kv in _sentries)
                    if (kv.Value.isActive) count++;
                return count;
            }
        }

        // ── Sentry Management ─────────────────────────────────────────

        /// <summary>Register a ghost sentry at a location node.</summary>
        public void RegisterSentry(string sentryId, string locationNodeId, float acousticThreshold = 30f)
        {
            if (string.IsNullOrEmpty(sentryId)) return;
            if (_sentries.ContainsKey(sentryId)) return;

            _sentries[sentryId] = new GhostSentryState
            {
                sentryId = sentryId,
                locationNodeId = locationNodeId,
                ammoBeltDurability = 100f,
                barrelHeat = 0f,
                isActive = true,
                burnedOut = false,
                acousticThreshold = acousticThreshold
            };
        }

        /// <summary>Check if a location has an active sentry.</summary>
        public bool HasActiveSentry(string locationNodeId)
        {
            foreach (var kv in _sentries)
            {
                if (kv.Value.locationNodeId == locationNodeId && kv.Value.isActive)
                    return true;
            }
            return false;
        }

        /// <summary>Get the active sentry at a location (null if none).</summary>
        public GhostSentryState GetSentryAt(string locationNodeId)
        {
            foreach (var kv in _sentries)
            {
                if (kv.Value.locationNodeId == locationNodeId && kv.Value.isActive)
                    return kv.Value;
            }
            return null;
        }

        // ── Tick ──────────────────────────────────────────────────────
        /// <summary>
        /// Called every game-hour. Cools barrels, decays pneumatic pressure,
        /// and evaluates sentry fire triggers against acoustic signatures.
        /// </summary>
        public void Tick(float gameHours, float acousticSignature, System.Random rng = null)
        {
            if (gameHours <= 0f) return;

            foreach (var kv in _sentries)
            {
                var sentry = kv.Value;
                if (!sentry.isActive) continue;

                // Phase 1: Barrel cooling
                sentry.barrelHeat = Mathf.Max(0f,
                    sentry.barrelHeat - BarrelCoolingPerHour * gameHours);

                sentry.hoursSinceLastShot += gameHours;

                // Phase 2: Pneumatic pressure decay
                if (sentry.hoursSinceLastShot >= PneumaticFailureHours)
                {
                    sentry.isActive = false;
                    sentry.burnedOut = true;
                    _totalSentriesBurnedOut++;

                    OnSentryBurnedOut?.Invoke(new SentryBurnoutEvent
                    {
                        SentryId = sentry.sentryId,
                        LocationNodeId = sentry.locationNodeId,
                        TotalRoundsFired = sentry.roundsFired,
                        BarrelMelted = false
                    });
                    continue;
                }

                // Phase 3: Sentry fires at acoustic signature above threshold
                if (acousticSignature >= sentry.acousticThreshold && rng != null)
                {
                    float chance = 0.3f * gameHours;
                    if ((float)rng.NextDouble() < chance)
                    {
                        FireSentry(sentry);
                    }
                }

                // Phase 4: Barrel cook-off check
                if (sentry.barrelHeat >= BarrelCookOffThreshold)
                {
                    sentry.isActive = false;
                    sentry.burnedOut = true;
                    _totalSentriesBurnedOut++;

                    OnSentryBurnedOut?.Invoke(new SentryBurnoutEvent
                    {
                        SentryId = sentry.sentryId,
                        LocationNodeId = sentry.locationNodeId,
                        TotalRoundsFired = sentry.roundsFired,
                        BarrelMelted = true
                    });
                }
            }

            OnThreatStateChanged?.Invoke();
        }

        /// <summary>
        /// Daily tick — loitering munition attraction and shelter intake strikes.
        /// </summary>
        public void TickDaily(float acousticSignature, string roomId = null, System.Random rng = null)
        {
            if (rng == null || acousticSignature < LoiteringMunitionThreshold) return;

            float excess = acousticSignature - LoiteringMunitionThreshold;
            float chance = MunitionAttractionChancePer10 * (excess / 10f);
            if ((float)rng.NextDouble() < chance)
            {
                _totalMunitionsAttracted++;

                if (roomId != null && (float)rng.NextDouble() < MunitionIntakeHitChance)
                {
                    OnMunitionStrike?.Invoke(new LoiteringMunitionStrikeEvent
                    {
                        RoomId = roomId,
                        AcousticSignature = acousticSignature
                    });
                }
            }
        }

        private void FireSentry(GhostSentryState sentry)
        {
            float rounds = RoundsPerBurst;
            sentry.roundsFired += rounds;
            sentry.barrelHeat += HeatPerRound * rounds;
            sentry.ammoBeltDurability = Mathf.Max(0f,
                sentry.ammoBeltDurability - BeltDegradationPerRound * rounds);
            sentry.hoursSinceLastShot = 0f;

            bool beltSnapped = sentry.ammoBeltDurability <= 0f;
            if (beltSnapped)
            {
                sentry.isActive = false;
                sentry.burnedOut = true;
                _totalSentriesBurnedOut++;
            }

            OnSentryFired?.Invoke(new SentryFireEvent
            {
                SentryId = sentry.sentryId,
                TargetNodeId = sentry.locationNodeId,
                RoundsFired = rounds,
                BarrelHeat = sentry.barrelHeat,
                BeltSnapped = beltSnapped
            });

            if (beltSnapped)
            {
                OnSentryBurnedOut?.Invoke(new SentryBurnoutEvent
                {
                    SentryId = sentry.sentryId,
                    LocationNodeId = sentry.locationNodeId,
                    TotalRoundsFired = sentry.roundsFired,
                    BarrelMelted = false
                });
            }
        }

        // ── Actions ───────────────────────────────────────────────────

        /// <summary>
        /// Deploy an acoustic decoy at a sentry's location. Forces the sentry
        /// to fire its full burst, rapidly degrading its ammo belt.
        /// </summary>
        public bool DeployDecoyAtSentry(string sentryId)
        {
            if (!_sentries.TryGetValue(sentryId, out var sentry)) return false;
            if (!sentry.isActive) return false;

            _totalDecoysDeployed++;

            // Decoy forces 5 bursts
            for (int i = 0; i < 5; i++)
            {
                if (!sentry.isActive) break;
                FireSentry(sentry);
            }

            return true;
        }

        /// <summary>
        /// Scavenge a burned-out sentry for tactical scrap.
        /// </summary>
        public bool ScavengeSentry(string sentryId)
        {
            if (!_sentries.TryGetValue(sentryId, out var sentry)) return false;
            if (sentry.isActive || !sentry.burnedOut) return false;

            GrantItem?.Invoke("item_tactical_scrap", ScrapYieldPerSentry);
            _sentries.Remove(sentryId);
            OnThreatStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Apply sentry burst damage to a survivor at a location.
        /// Called by the expedition encounter resolver.
        /// </summary>
        public void ApplySentryDamageToSurvivor(string survivorId, string sentryId)
        {
            if (!_sentries.TryGetValue(sentryId, out var sentry)) return;
            if (!sentry.isActive) return;

            ApplyHealthDamage?.Invoke(survivorId, SentryBurstDamage);
            InflictAffliction?.Invoke(survivorId, "bleeding");
        }

        // ── Save / Load ────────────────────────────────────────────────
        public AutomatedThreatSystemSave CaptureState()
        {
            var save = new AutomatedThreatSystemSave
            {
                totalSentriesBurnedOut = _totalSentriesBurnedOut,
                totalMunitionsAttracted = _totalMunitionsAttracted,
                totalDecoysDeployed = _totalDecoysDeployed
            };

            foreach (var kv in _sentries)
            {
                var s = kv.Value;
                save.sentries.Add(new GhostSentryState
                {
                    sentryId = s.sentryId,
                    locationNodeId = s.locationNodeId,
                    ammoBeltDurability = s.ammoBeltDurability,
                    barrelHeat = s.barrelHeat,
                    isActive = s.isActive,
                    burnedOut = s.burnedOut,
                    acousticThreshold = s.acousticThreshold,
                    roundsFired = s.roundsFired,
                    hoursSinceLastShot = s.hoursSinceLastShot
                });
            }

            return save;
        }

        public void RestoreState(AutomatedThreatSystemSave save)
        {
            _sentries.Clear();
            _totalSentriesBurnedOut = 0;
            _totalMunitionsAttracted = 0;
            _totalDecoysDeployed = 0;

            if (save == null) return;

            _totalSentriesBurnedOut = save.totalSentriesBurnedOut;
            _totalMunitionsAttracted = save.totalMunitionsAttracted;
            _totalDecoysDeployed = save.totalDecoysDeployed;

            for (int i = 0; i < save.sentries.Count; i++)
            {
                if (save.sentries[i] != null)
                    _sentries[save.sentries[i].sentryId] = save.sentries[i];
            }
        }
    }
}
