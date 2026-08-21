using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// ASHFALL: THE DOSE (Expansion 07) — Vigil State Machine.
    /// Manages the 4-minute quiet bedside vigil for terminally ill dwellers.
    /// Engine-agnostic: plain C#, raises events, save/load safe via Capture/Restore.
    /// </summary>
    [Serializable]
    public sealed class VigilSaveState
    {
        public bool isActive;
        public float elapsedSeconds;
        public float durationSeconds;
        public string dwellerId;
        public int recitedCount;
        public bool phantomKnockFired;
        public bool wasSkipped;
        public bool isCompleted;
        public List<string> namesToRecite = new List<string>();
    }

    public sealed class VigilStateMachine
    {
        public const float DefaultDuration = 240f; // 4 minutes

        public bool IsActive { get; private set; }
        public float ElapsedSeconds { get; private set; }
        public float DurationSeconds { get; private set; } = DefaultDuration;
        public string DwellerId { get; private set; } = string.Empty;
        public int RecitedCount { get; private set; }
        public bool PhantomKnockFired { get; private set; }
        public bool WasSkipped { get; private set; }
        public bool IsCompleted { get; private set; }

        private readonly List<string> _names = new List<string>();
        public IReadOnlyList<string> Names => _names;

        public event Action<string> OnVigilStarted;
        public event Action<string, int> OnNameRecited;
        public event Action OnPhantomKnock;
        public event Action<bool> OnVigilCompleted;

        public void StartVigil(string dwellerId, IEnumerable<string> names, float duration = DefaultDuration)
        {
            DwellerId = dwellerId ?? string.Empty;
            _names.Clear();
            if (names != null) _names.AddRange(names);
            DurationSeconds = duration > 0 ? duration : DefaultDuration;
            ElapsedSeconds = 0f;
            RecitedCount = 0;
            PhantomKnockFired = false;
            WasSkipped = false;
            IsCompleted = false;
            IsActive = true;

            OnVigilStarted?.Invoke(DwellerId);
        }

        public void Tick(float deltaSeconds)
        {
            if (!IsActive || IsCompleted) return;

            ElapsedSeconds += deltaSeconds;

            // Recite names spaced evenly across the duration
            if (_names.Count > 0 && RecitedCount < _names.Count)
            {
                float timePerName = (DurationSeconds * 0.85f) / _names.Count;
                int targetIndex = (int)(ElapsedSeconds / timePerName);
                while (RecitedCount < targetIndex && RecitedCount < _names.Count)
                {
                    string name = _names[RecitedCount];
                    RecitedCount++;
                    OnNameRecited?.Invoke(name, RecitedCount);
                }
            }

            // Phantom knock at 95% completion
            if (!PhantomKnockFired && ElapsedSeconds >= DurationSeconds * 0.95f)
            {
                PhantomKnockFired = true;
                OnPhantomKnock?.Invoke();
            }

            if (ElapsedSeconds >= DurationSeconds)
            {
                Complete(skipped: false);
            }
        }

        public void Skip()
        {
            if (!IsActive || IsCompleted) return;
            Complete(skipped: true);
        }

        private void Complete(bool skipped)
        {
            WasSkipped = skipped;
            IsActive = false;
            IsCompleted = true;
            OnVigilCompleted?.Invoke(WasSkipped);
        }

        public VigilSaveState CaptureState()
        {
            return new VigilSaveState
            {
                isActive = IsActive,
                elapsedSeconds = ElapsedSeconds,
                durationSeconds = DurationSeconds,
                dwellerId = DwellerId,
                recitedCount = RecitedCount,
                phantomKnockFired = PhantomKnockFired,
                wasSkipped = WasSkipped,
                isCompleted = IsCompleted,
                namesToRecite = new List<string>(_names)
            };
        }

        public void RestoreState(VigilSaveState state)
        {
            if (state == null) return;
            IsActive = state.isActive;
            ElapsedSeconds = state.elapsedSeconds;
            DurationSeconds = state.durationSeconds > 0 ? state.durationSeconds : DefaultDuration;
            DwellerId = state.dwellerId ?? string.Empty;
            RecitedCount = state.recitedCount;
            PhantomKnockFired = state.phantomKnockFired;
            WasSkipped = state.wasSkipped;
            IsCompleted = state.isCompleted;
            _names.Clear();
            if (state.namesToRecite != null) _names.AddRange(state.namesToRecite);
        }
    }
}
