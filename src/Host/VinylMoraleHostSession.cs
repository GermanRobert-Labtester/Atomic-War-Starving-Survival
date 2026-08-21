using System;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for VinylMoraleSystem.
    /// Manages pre-war vinyl records, common room playback, morale recovery, and flashback suppression.
    /// </summary>
    public sealed class VinylMoraleHostSession
    {
        public VinylMoraleSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public VinylMoraleHostSession(VinylMoraleSystem system)
        {
            System = system ?? new VinylMoraleSystem(new GodotLog());

            System.OnPlaybackChanged += () =>
            {
                LastEvent = System.IsPlaying
                    ? $"[Vinyl] Turntable started spinning: {System.State.currentPlayingId}"
                    : "[Vinyl] Turntable stopped.";
                StateChanged?.Invoke();
            };

            System.OnMoraleApplied += amount =>
            {
                LastEvent = $"[Vinyl] Daily record broadcast applied +{amount:F0} Morale to all shelter dwellers.";
                StateChanged?.Invoke();
            };
        }

        public void AcquireRecord(string recordId)
        {
            System.AcquireRecord(recordId);
            LastEvent = $"Acquired pre-war vinyl album: {recordId}";
            StateChanged?.Invoke();
        }

        public ActionResult PlayRecord(string recordId)
        {
            var res = System.Play(recordId);
            if (res.IsSuccess)
            {
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult StopPlayback()
        {
            var res = System.Stop();
            if (res.IsSuccess)
            {
                StateChanged?.Invoke();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.ApplyDailyEffect(day);
            StateChanged?.Invoke();
        }
    }
}
