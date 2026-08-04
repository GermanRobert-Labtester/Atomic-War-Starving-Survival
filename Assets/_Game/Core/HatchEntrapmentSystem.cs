using System;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Save DTO for weather-driven hatch entrapment (Prompt #48).
    /// </summary>
    [Serializable]
    public class HatchEntrapmentSave
    {
        public HatchState State;
        public float ContinuousHazardHours;
        public WeatherKind LastHazardWeather;
        public bool BuriedEventFired;
        public bool SuffocationActive;
        public float SuffocationHoursRemaining;
        public bool FactionRescueScheduled;
        public string FactionRescueFactionId;
    }

    /// <summary>
    /// WeatherSystem dictates narrative pacing by sealing the hatch.
    /// Continuous Blizzard (or FalloutStorm) past 72 hours buries/freezes the hatch,
    /// hard-locks expeditions, and forces DigOut exertion that spikes entry-room CO2.
    /// Broken air filter while sealed starts a suffocation countdown.
    /// High faction trust can schedule an outside dig-out — at massive debt.
    /// </summary>
    public class HatchEntrapmentSystem
    {
        /// <summary>3 days of continuous extreme weather seals the hatch.</summary>
        public const float HazardHoursToSeal = 72f;

        /// <summary>DigOut exertion CO2 spike in the entry room (ppm).</summary>
        public const float DigOutCo2SpikePpm = 120f;

        /// <summary>Hours until fatal air if filter is dead while sealed.</summary>
        public const float SuffocationDurationHours = 18f;

        /// <summary>Strict greater-than: trust must exceed this for outside rescue.</summary>
        public const float FactionRescueTrustThreshold = 80f;

        /// <summary>Trust penalty when a faction digs you out (massive debt).</summary>
        public const float FactionRescueDebtTrust = -45f;

        public const string EntryRoomId = "entry";

        // World / event flags
        public const string FlagHatchBuried = "hatch_buried";
        public const string FlagHatchFrozen = "hatch_frozen";
        public const string FlagFactionDigOutDebt = "faction_dig_out_debt";
        public const string FlagBuriedAliveOffered = "is_buried_alive_offered";

        private HatchState _state = HatchState.Clear;
        private float _continuousHazardHours;
        private WeatherKind _lastHazardWeather = WeatherKind.Clear;
        private bool _buriedEventFired;
        private bool _suffocationActive;
        private float _suffocationHoursRemaining;
        private bool _factionRescueScheduled;
        private string _factionRescueFactionId;

        public HatchState State => _state;

        /// <summary>True when the hatch is Buried or Frozen — no one leaves.</summary>
        public bool AreExpeditionsLocked => _state != HatchState.Clear;

        /// <summary>Expedition UI should disable when the hatch is sealed.</summary>
        public bool IsExpeditionUiEnabled => !AreExpeditionsLocked;

        public float ContinuousHazardHours => _continuousHazardHours;
        public bool IsSuffocationActive => _suffocationActive;
        public float SuffocationHoursRemaining => _suffocationHoursRemaining;
        public bool BuriedEventFired => _buriedEventFired;
        public bool FactionRescueScheduled => _factionRescueScheduled;
        public string FactionRescueFactionId => _factionRescueFactionId;

        public event Action<HatchState, HatchState> OnHatchStateChanged;
        public event Action OnBuriedAliveTriggered;
        public event Action OnSuffocationStarted;
        public event Action OnSuffocationCleared;
        public event Action<string> OnFactionRescueScheduled;
        /// <summary>
        /// Fired when a faction dig-out is accepted (Prompt #18 schedules the
        /// delayed debt collector from this). Parameter: rescuer faction id.
        /// </summary>
        public event Action<string> OnFactionRescueApplied;

        /// <summary>
        /// Advance continuous hazard tracking, seal the hatch after 72h of
        /// Blizzard/FalloutStorm, drive suffocation, and schedule faction rescue.
        /// </summary>
        public void Tick(
            float gameHours,
            WeatherKind weather,
            Shelter.Shelter shelter = null,
            Func<string, float> getFactionTrust = null,
            Action<string, int, string> scheduleEvent = null,
            int currentDay = 1,
            EventContext eventFlags = null)
        {
            if (gameHours <= 0f) return;

            // Continuous extreme weather accumulates; clear weather resets the
            // clock only while the hatch is still open. Once sealed, only DigOut
            // (or faction rescue) reopens — snow does not melt on a schedule.
            if (IsExtremeWeather(weather))
            {
                _continuousHazardHours += gameHours;
                _lastHazardWeather = weather;
            }
            else if (_state == HatchState.Clear)
            {
                _continuousHazardHours = 0f;
            }

            if (_state == HatchState.Clear && _continuousHazardHours >= HazardHoursToSeal)
            {
                HatchState next = (_lastHazardWeather == WeatherKind.FalloutStorm
                        || _lastHazardWeather == WeatherKind.BlackRain)
                    ? HatchState.Frozen
                    : HatchState.Buried;
                SetState(next, eventFlags);
                if (!_buriedEventFired)
                {
                    _buriedEventFired = true;
                    eventFlags?.SetEventFlag(FlagBuriedAliveOffered, true);
                    OnBuriedAliveTriggered?.Invoke();
                }
            }

            // Suffocation: sealed + broken air filter.
            if (AreExpeditionsLocked && IsAirFilterBroken(shelter))
            {
                if (!_suffocationActive)
                {
                    _suffocationActive = true;
                    _suffocationHoursRemaining = SuffocationDurationHours;
                    OnSuffocationStarted?.Invoke();
                }
                else
                {
                    _suffocationHoursRemaining = Mathf.Max(0f, _suffocationHoursRemaining - gameHours);
                }
            }
            else if (_suffocationActive)
            {
                _suffocationActive = false;
                _suffocationHoursRemaining = 0f;
                OnSuffocationCleared?.Invoke();
            }

            // Story variance: any faction trust > 80 can dig from outside.
            if (AreExpeditionsLocked && !_factionRescueScheduled && getFactionTrust != null)
            {
                string rescuer = FindHighTrustFaction(getFactionTrust);
                if (!string.IsNullOrEmpty(rescuer))
                {
                    _factionRescueScheduled = true;
                    _factionRescueFactionId = rescuer;
                    int fireDay = Mathf.Max(1, currentDay + 1);
                    scheduleEvent?.Invoke(EventRunner.FactionDigOutEventId, fireDay, FlagFactionDigOutDebt);
                    OnFactionRescueScheduled?.Invoke(rescuer);
                }
            }
        }

        /// <summary>
        /// DigOut from inside: heavy exertion in a sealed space spikes entry-room CO2,
        /// then clears the hatch. Returns false if the hatch is already Clear.
        /// </summary>
        public bool DigOut(ShelterRoom entryRoom, EventContext eventFlags = null)
        {
            if (_state == HatchState.Clear) return false;

            if (entryRoom != null)
            {
                entryRoom.Co2Ppm = Mathf.Max(0f, entryRoom.Co2Ppm) + DigOutCo2SpikePpm;
            }

            SetState(HatchState.Clear, eventFlags);
            _continuousHazardHours = 0f;
            _buriedEventFired = false;
            if (_suffocationActive)
            {
                _suffocationActive = false;
                _suffocationHoursRemaining = 0f;
                OnSuffocationCleared?.Invoke();
            }
            return true;
        }

        /// <summary>
        /// Faction digs the hatch open from outside. Clears the seal and records debt.
        /// Caller applies trust penalty via economy. Fires
        /// <see cref="OnFactionRescueApplied"/> with the rescuer faction id so
        /// Prompt #18 can schedule the delayed collector (day + 20).
        /// </summary>
        public bool ApplyFactionRescue(EventContext eventFlags = null)
        {
            if (_state == HatchState.Clear) return false;
            SetState(HatchState.Clear, eventFlags);
            _continuousHazardHours = 0f;
            _buriedEventFired = false;
            eventFlags?.SetEventFlag(FlagFactionDigOutDebt, true);
            if (_suffocationActive)
            {
                _suffocationActive = false;
                _suffocationHoursRemaining = 0f;
                OnSuffocationCleared?.Invoke();
            }

            string rescuer = _factionRescueFactionId;
            _factionRescueScheduled = false;
            OnFactionRescueApplied?.Invoke(rescuer);
            return true;
        }

        /// <summary>Force a hatch state (tests / scripted beats).</summary>
        public void ForceState(HatchState state, EventContext eventFlags = null)
        {
            SetState(state, eventFlags);
            if (state != HatchState.Clear)
            {
                _continuousHazardHours = HazardHoursToSeal;
                _buriedEventFired = true;
            }
            else
            {
                _continuousHazardHours = 0f;
                _buriedEventFired = false;
            }
        }

        public static bool IsExtremeWeather(WeatherKind weather)
        {
            return weather == WeatherKind.Blizzard
                || weather == WeatherKind.FalloutStorm
                || weather == WeatherKind.BlackRain;
        }

        public static bool IsAirFilterBroken(Shelter.Shelter shelter)
        {
            if (shelter == null) return true;
            var air = shelter.GetModule("air_filtration");
            if (air == null) return true;
            if (!air.IsOperational) return true;
            return air.FilterHealth <= 0f;
        }

        /// <summary>
        /// First faction id whose trust is strictly greater than
        /// <see cref="FactionRescueTrustThreshold"/>, or null.
        /// </summary>
        public static string FindHighTrustFaction(Func<string, float> getTrust, params string[] factionIds)
        {
            if (getTrust == null) return null;
            if (factionIds == null || factionIds.Length == 0)
            {
                // Order is priority: first faction over the threshold digs. Cult of the
                // Glow is eligible when the trust callback reports effective trust
                // (radiation-driven for trustInversion factions) above threshold.
                factionIds = new[]
                {
                    AtomicWar._Game.Economy.FactionSO.Ids.ScavengerCamp,
                    AtomicWar._Game.Economy.FactionSO.Ids.DoomsdayPreppers,
                    AtomicWar._Game.Economy.FactionSO.Ids.MilitaryRemnants,
                    AtomicWar._Game.Economy.FactionSO.Ids.CultOfTheGlow
                };
            }
            for (int i = 0; i < factionIds.Length; i++)
            {
                string id = factionIds[i];
                if (string.IsNullOrEmpty(id)) continue;
                if (getTrust(id) > FactionRescueTrustThreshold)
                    return id;
            }
            return null;
        }

        public HatchEntrapmentSave CaptureState()
        {
            return new HatchEntrapmentSave
            {
                State = _state,
                ContinuousHazardHours = _continuousHazardHours,
                LastHazardWeather = _lastHazardWeather,
                BuriedEventFired = _buriedEventFired,
                SuffocationActive = _suffocationActive,
                SuffocationHoursRemaining = _suffocationHoursRemaining,
                FactionRescueScheduled = _factionRescueScheduled,
                FactionRescueFactionId = _factionRescueFactionId
            };
        }

        public void RestoreState(HatchEntrapmentSave save)
        {
            if (save == null)
            {
                _state = HatchState.Clear;
                _continuousHazardHours = 0f;
                _lastHazardWeather = WeatherKind.Clear;
                _buriedEventFired = false;
                _suffocationActive = false;
                _suffocationHoursRemaining = 0f;
                _factionRescueScheduled = false;
                _factionRescueFactionId = null;
                return;
            }

            _state = save.State;
            _continuousHazardHours = save.ContinuousHazardHours;
            _lastHazardWeather = save.LastHazardWeather;
            _buriedEventFired = save.BuriedEventFired;
            _suffocationActive = save.SuffocationActive;
            _suffocationHoursRemaining = save.SuffocationHoursRemaining;
            _factionRescueScheduled = save.FactionRescueScheduled;
            _factionRescueFactionId = save.FactionRescueFactionId;
        }

        private void SetState(HatchState next, EventContext eventFlags)
        {
            if (next == _state) return;
            var prev = _state;
            _state = next;

            if (eventFlags != null)
            {
                eventFlags.SetEventFlag(FlagHatchBuried, next == HatchState.Buried);
                eventFlags.SetEventFlag(FlagHatchFrozen, next == HatchState.Frozen);
                if (next == HatchState.Clear)
                {
                    eventFlags.SetEventFlag(FlagBuriedAliveOffered, false);
                }
            }

            OnHatchStateChanged?.Invoke(prev, next);
        }
    }
}
