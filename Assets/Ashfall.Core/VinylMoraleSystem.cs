using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class VinylMoraleState
    {
        public string systemId = VinylMoraleSystem.SystemId;
        public List<string> ownedRecordIds = new List<string>();
        public string currentPlayingId = string.Empty;
        public string lastPlayedId = string.Empty;
        public int lastPlayedDay = -1;
        public int totalPlays;
        public float totalMoraleApplied;
        public bool isTurntableActive;
    }

    [Serializable]
    public sealed class VinylRecordDefinition
    {
        public string record_id = string.Empty;
        public string display_name = string.Empty;
        public string genre = string.Empty;
        public float morale_daily_bonus = 3f;
        public float flashback_suppression; // 0-1, reduces flashback probability
        public string audio_cue_id = string.Empty;
        public string description = string.Empty;
    }

    public sealed class VinylMoraleSystem
    {
        public const string SystemId = "vinyl_morale";
        private VinylMoraleState _state = new VinylMoraleState();
        private readonly Dictionary<string, VinylRecordDefinition> _catalog = new Dictionary<string, VinylRecordDefinition>(StringComparer.Ordinal);
        private readonly ILog _log;

        public VinylMoraleState State => _state;
        public bool IsPlaying => _state.isTurntableActive && !string.IsNullOrEmpty(_state.currentPlayingId);

        public event Action<float> OnMoraleApplied;      // morale amount
        public event Action<float> OnFlashbackSuppressed; // suppression amount
        public event Action OnPlaybackChanged;

        public VinylMoraleSystem(ILog log = null)
        {
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(List<VinylRecordDefinition> records)
        {
            if (records == null) return;
            _catalog.Clear();
            foreach (var r in records)
                if (!string.IsNullOrEmpty(r.record_id))
                    _catalog[r.record_id] = r;
        }

        public void AcquireRecord(string recordId)
        {
            if (!_state.ownedRecordIds.Contains(recordId))
            {
                _state.ownedRecordIds.Add(recordId);
                _log.Info($"[Vinyl] acquired record '{recordId}'");
                OnPlaybackChanged?.Invoke();
            }
        }

        public ActionResult Play(string recordId)
        {
            if (!_state.ownedRecordIds.Contains(recordId))
                return ActionResult.Blocked("not_owned", "vinyl.not_owned");
            if (!_catalog.TryGetValue(recordId, out var record))
                return ActionResult.Failed("unknown_record", "vinyl.unknown");

            _state.lastPlayedId = _state.currentPlayingId;
            _state.currentPlayingId = recordId;
            _state.isTurntableActive = true;
            _log.Info($"[Vinyl] playing '{record.display_name}'");
            OnPlaybackChanged?.Invoke();
            return ActionResult.Success("vinyl.playing",
                new Dictionary<string, double>
                {
                    { "morale_bonus", record.morale_daily_bonus },
                    { "flashback_suppression", record.flashback_suppression }
                });
        }

        public ActionResult Stop()
        {
            if (!_state.isTurntableActive)
                return ActionResult.Blocked("not_playing", "vinyl.not_playing");
            _state.isTurntableActive = false;
            OnPlaybackChanged?.Invoke();
            return ActionResult.Success("vinyl.stopped");
        }

        /// <summary>Called once per day to apply the daily morale effect.</summary>
        public void ApplyDailyEffect(int day)
        {
            if (!IsPlaying) return;
            if (_state.lastPlayedDay == day) return; // one-time per day

            if (!_catalog.TryGetValue(_state.currentPlayingId, out var record)) return;

            _state.totalPlays++;
            _state.totalMoraleApplied += record.morale_daily_bonus;
            _state.lastPlayedDay = day;

            OnMoraleApplied?.Invoke(record.morale_daily_bonus);
            if (record.flashback_suppression > 0)
                OnFlashbackSuppressed?.Invoke(record.flashback_suppression);

            _log.Info($"[Vinyl] daily effect: +{record.morale_daily_bonus} morale, -{record.flashback_suppression:P0} flashback");
        }

        public VinylRecordDefinition GetRecord(string id)
        {
            _catalog.TryGetValue(id, out var r);
            return r;
        }

        public VinylMoraleState CaptureState() => _state;
        public void RestoreState(VinylMoraleState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnPlaybackChanged?.Invoke();
        }
    }
}
