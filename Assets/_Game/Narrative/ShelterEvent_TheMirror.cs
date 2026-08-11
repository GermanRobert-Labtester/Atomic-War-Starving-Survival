using System;

namespace AtomicWar._Game.Narrative
{
    /// <summary>
    /// Shelter Event — The Mirror (Prompt #593). A depressed survivor locks
    /// themselves in the bathroom and talks to their reflection. The outcome
    /// is binary and uncontrollable: 50 % chance the survivor is cured of
    /// Depression, 50 % chance they commit suicide. The player cannot intervene.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class ShelterEvent_TheMirror
    {
        public const string EventId = "shelter_event_the_mirror";
        public const float CureChance = 0.50f;
        public const float SuicideChance = 0.50f;

        // -- Runtime state --
        public bool IsActive { get; private set; }
        public string LockedSurvivorId { get; private set; }
        public bool IsDepressed { get; private set; }
        public bool ResolutionPending { get; private set; }

        // -- Events --
        public event Action<string> OnMirrorEventStarted;   // survivorId
        public event Action<string> OnSurvivorCured;        // survivorId
        public event Action<string> OnSurvivorSuicide;      // survivorId

        public ShelterEvent_TheMirror() { }

        /// <summary>
        /// Trigger the mirror event for a depressed survivor. The survivor
        /// locks in the bathroom; resolution happens on the next call to
        /// <see cref="Resolve"/>.
        /// </summary>
        public void Trigger(string depressedSurvivorId)
        {
            if (IsActive) return;
            if (string.IsNullOrEmpty(depressedSurvivorId)) return;

            IsActive = true;
            LockedSurvivorId = depressedSurvivorId;
            IsDepressed = true;
            ResolutionPending = true;

            OnMirrorEventStarted?.Invoke(depressedSurvivorId);
        }

        /// <summary>
        /// Resolve the event. Cannot be interrupted. Returns the outcome
        /// ("cured" or "suicide") and the affected survivor id.
        /// </summary>
        public (string outcome, string survivorId) Resolve(Random rng)
        {
            if (!ResolutionPending) return (null, null);

            ResolutionPending = false;
            IsActive = false;
            IsDepressed = false;

            float roll = (float)(rng != null ? rng.NextDouble() : 0.5);
            string id = LockedSurvivorId;

            if (roll < CureChance)
            {
                LockedSurvivorId = null;
                OnSurvivorCured?.Invoke(id);
                return ("cured", id);
            }
            else
            {
                LockedSurvivorId = null;
                OnSurvivorSuicide?.Invoke(id);
                return ("suicide", id);
            }
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public MirrorEventSave CaptureState()
        {
            return new MirrorEventSave
            {
                IsActive = IsActive,
                LockedSurvivorId = LockedSurvivorId,
                IsDepressed = IsDepressed,
                ResolutionPending = ResolutionPending
            };
        }

        public void RestoreState(MirrorEventSave save)
        {
            if (save == null) return;
            IsActive = save.IsActive;
            LockedSurvivorId = save.LockedSurvivorId;
            IsDepressed = save.IsDepressed;
            ResolutionPending = save.ResolutionPending;
        }
    }

    [Serializable]
    public class MirrorEventSave
    {
        public bool IsActive;
        public string LockedSurvivorId;
        public bool IsDepressed;
        public bool ResolutionPending;
    }
}
