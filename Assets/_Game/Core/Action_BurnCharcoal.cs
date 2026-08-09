using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BurnCharcoalState
    {
        public string actionId = "action_burn_charcoal";
        public float coConcentration = 1.0f;
        public bool requiresSealedBarrel = true;
    }

    /// <summary>DEMOTE-Action-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_BurnCharcoal
    {
        public event Action<string> OnCharcoalProduced;
        public event Action<string, float> OnCOGenerated;
        public event Action<string> OnUnsafeLocation;

        private static readonly string[] SafeRoomTypes = { "airlock", "empty_room" };

        private BurnCharcoalState _state;

        public Action_BurnCharcoal()
        {
            _state = new BurnCharcoalState();
        }

        public Action_BurnCharcoal(BurnCharcoalState state)
        {
            _state = state ?? new BurnCharcoalState();
        }

        public BurnCharcoalState CaptureState() => _state;

        public void RestoreState(BurnCharcoalState state)
        {
            _state = state ?? new BurnCharcoalState();
        }

        public bool BurnCharcoal(string survivorId, string roomId, string roomType)
        {
            bool isSafe = false;
            for (int i = 0; i < SafeRoomTypes.Length; i++)
            {
                if (SafeRoomTypes[i] == roomType)
                {
                    isSafe = true;
                    break;
                }
            }

            if (!isSafe)
            {
                OnUnsafeLocation?.Invoke(roomId);
                return false;
            }

            OnCharcoalProduced?.Invoke(survivorId);
            OnCOGenerated?.Invoke(roomId, _state.coConcentration);

            return true;
        }
    }
}
