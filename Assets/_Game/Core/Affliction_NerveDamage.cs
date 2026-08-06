using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class Affliction_NerveDamageState
    {
        public string survivorId = string.Empty;
        public bool isDamaged = false;
        public float firearmAccuracyPenalty = 0.75f;
        public bool surgeryDisabled = false;
        public bool craftingDisabled = false;
        public string triggerCause = string.Empty;
    }

    public class Affliction_NerveDamage
    {
        public Affliction_NerveDamageState State { get; private set; }

        public event Action<string, string> OnNerveDamageAfflicted;
        public event Action<string> OnNerveDamageCleared;

        public Affliction_NerveDamage()
        {
            State = new Affliction_NerveDamageState();
        }

        public Affliction_NerveDamage(Affliction_NerveDamageState state)
        {
            State = state ?? new Affliction_NerveDamageState();
        }

        public void Afflict(string survivorId, string cause)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                return;
            }

            State.survivorId = survivorId;
            State.isDamaged = true;
            State.surgeryDisabled = true;
            State.craftingDisabled = true;
            State.triggerCause = cause ?? "unknown";

            OnNerveDamageAfflicted?.Invoke(survivorId, State.triggerCause);
        }

        public bool CanUseFirearm()
        {
            return !State.isDamaged;
        }

        public bool CanPerformSurgery()
        {
            return !State.surgeryDisabled;
        }

        public bool CanPerformPrecisionCrafting()
        {
            return !State.craftingDisabled;
        }

        public float GetFirearmAccuracyModifier()
        {
            return State.isDamaged ? (1f - State.firearmAccuracyPenalty) : 1f;
        }

        public void Clear()
        {
            if (!State.isDamaged)
            {
                return;
            }

            string id = State.survivorId;
            State.isDamaged = false;
            State.surgeryDisabled = false;
            State.craftingDisabled = false;
            State.triggerCause = string.Empty;

            OnNerveDamageCleared?.Invoke(id);
        }
    }
}
