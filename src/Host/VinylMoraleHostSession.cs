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
    : HostSessionBase{
        public VinylMoraleSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public Func<int> DayProvider { get; set; } = () => -1;

        public VinylMoraleHostSession(VinylMoraleSystem system)
        {
            System = system ?? new VinylMoraleSystem(new GodotLog());

            System.OnPlaybackChanged += () =>
            {
                LastEvent = System.IsPlaying
                    ? $"[Vinyl] Turntable started spinning: {System.State.currentPlayingId}"
                    : "[Vinyl] Turntable stopped.";
                RaiseStateChanged();
            };

            System.OnMoraleApplied += amount =>
            {
                LastEvent = $"[Vinyl] Daily record broadcast applied +{amount:F0} Morale to all shelter dwellers.";
                RaiseStateChanged();
            };
        }

        public void AcquireRecord(string recordId)
        {
            System.AcquireRecord(recordId);
            LastEvent = $"Acquired pre-war vinyl album: {recordId}";
            RaiseStateChanged();
        }

        public ActionResult PlayRecord(string recordId, int day = -1)
        {
            int effectiveDay = day >= 0 ? day : DayProvider != null ? DayProvider.Invoke() : -1;
            var res = System.Play(recordId, effectiveDay);
            if (res.IsSuccess)
            {
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult StopPlayback()
        {
            var res = System.Stop();
            if (res.IsSuccess)
            {
                RaiseStateChanged();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.ApplyDailyEffect(day);
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            VinylMoraleSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
