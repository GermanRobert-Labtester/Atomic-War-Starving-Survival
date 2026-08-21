using System;

namespace Ashfall.Core.Muster
{
    /// <summary>Serialized state of the Iron Raiders (Section V.6) — the Toll's den
    /// at loc_iron_raiders_den. No offers; wants what the player has. Raid chance
    /// is read, never authored, from the wartime tension the sector already tracks.</summary>
    public class IronRaidersState
    {
        public string systemId = IronRaidersSystem.SystemId;
        public bool isActive;
        public float aggressionLevel;   // read from sector wartime tension (0..1)
        public int raidsThisSeason;
        public float shelterVisibility = 1f; // lowered by fortifying approach routes
    }

    /// <summary>
    /// Engine-agnostic state machine for faction_iron_raiders (Section V.6). The only
    /// faction in the roster the player may strike first with no narrative gate, and
    /// deliberately the only one with no diplomatic Approach. EvaluateRaidChance reads
    /// aggression and visibility; ExecuteRaid is a combat/loss event with no dialogue.
    /// </summary>
    public class IronRaidersSystem
    {
        public const string SystemId = "iron_raiders_system";

        private readonly IronRaidersState _state;

        public event Action<IronRaidersState> OnStateChanged;
        public event Action OnRaidExecuted;
        public event Action OnFortified;

        public IronRaidersSystem(IronRaidersState state = null!)
        {
            _state = state ?? new IronRaidersState();
            if (_state.systemId != SystemId) _state.systemId = SystemId;
        }

        public IronRaidersState State => _state;
        public float AggressionLevel => _state.aggressionLevel;
        public int RaidsThisSeason => _state.raidsThisSeason;

        /// <summary>Feed the sector's wartime tension value each day (0..1).</summary>
        public void SetAggressionLevel(float level)
        {
            float clamped = Math.Max(0f, Math.Min(1f, level));
            if (Math.Abs(clamped - _state.aggressionLevel) < 0.001f) { _state.aggressionLevel = clamped; return; }
            _state.aggressionLevel = clamped;
            RaiseChanged();
        }

        /// <summary>Infrastructure fortify on the den's approach routes lowers visibility,
        /// shrinking the raid window without touching aggression.</summary>
        public void FortifyApproachRoutes(float reductionPercent)
        {
            float reduction = Math.Max(0f, Math.Min(100f, reductionPercent)) / 100f;
            if (reduction <= 0f) return;
            _state.shelterVisibility = Math.Max(0.1f, _state.shelterVisibility * (1f - reduction));
            OnFortified?.Invoke();
            RaiseChanged();
        }

        public void ProvokeRaid() { ExecuteRaid(); }

        /// <summary>A combat/loss event. No dialogue tree by design.</summary>
        public void ExecuteRaid()
        {
            _state.raidsThisSeason++;
            OnRaidExecuted?.Invoke();
            RaiseChanged();
        }

        /// <summary>Chance (0..1) an active raid window opens today, driven jointly by
        /// wartime aggression and how visible the shelter keeps itself.</summary>
        public float EvaluateRaidChance()
        {
            float baseChance = _state.aggressionLevel * 0.6f;           // siege weeks bleed into raider weeks
            float visibilityPenalty = _state.shelterVisibility * 0.25f; // fortify to shave it
            return Math.Max(0f, Math.Min(1f, baseChance + visibilityPenalty));
        }

        public void Activate() { _state.isActive = true; RaiseChanged(); }

        // ── Save / Load ────────────────────────────────────────────────

        public IronRaidersState CaptureState() => new IronRaidersState
        {
            systemId = _state.systemId,
            isActive = _state.isActive,
            aggressionLevel = _state.aggressionLevel,
            raidsThisSeason = _state.raidsThisSeason,
            shelterVisibility = _state.shelterVisibility
        };

        public void RestoreState(IronRaidersState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.isActive = saved.isActive;
            _state.aggressionLevel = Math.Max(0f, Math.Min(1f, saved.aggressionLevel));
            _state.raidsThisSeason = Math.Max(0, saved.raidsThisSeason);
            _state.shelterVisibility = Math.Max(0.1f, saved.shelterVisibility);
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
