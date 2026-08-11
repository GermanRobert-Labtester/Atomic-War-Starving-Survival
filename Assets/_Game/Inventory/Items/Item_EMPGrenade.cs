using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public sealed class EMPGrenadeState
    {
        public string itemId = "item_emp_grenade";
        public bool disablesRobotics = true;
        public float bunkerPowerOutageHours = 24f;
    }

    public sealed class Item_EMPGrenade
    {
        public event Action<string, string> OnRoboticsDisabled;    // (throwerId, robotType)
        public event Action<string, float> OnPowerNetworkWiped;   // (throwerId, hours)

        private EMPGrenadeState _state = new EMPGrenadeState();

        // Throw the EMP at a group of robotic enemies.
        // Instantly disables all targeted robotics (RobotDogs, Turrets, etc.).
        public void ThrowAtEnemies(string throwerId, List<string> roboticTargetIds)
        {
            if (string.IsNullOrEmpty(throwerId))
                throw new ArgumentNullException(nameof(throwerId));
            if (roboticTargetIds == null)
                throw new ArgumentNullException(nameof(roboticTargetIds));

            for (int i = 0; i < roboticTargetIds.Count; i++)
            {
                string targetId = roboticTargetIds[i];
                if (string.IsNullOrEmpty(targetId)) continue;

                // Determine robot type from the id prefix for the event payload.
                string robotType = InferRobotType(targetId);
                OnRoboticsDisabled?.Invoke(throwerId, robotType);
            }
        }

        // Detonate the EMP inside the bunker.
        // Wipes the PowerNetwork for the configured duration (default 24 h).
        public void DetonateInBunker(string throwerId)
        {
            if (string.IsNullOrEmpty(throwerId))
                throw new ArgumentNullException(nameof(throwerId));

            OnPowerNetworkWiped?.Invoke(throwerId, _state.bunkerPowerOutageHours);
        }

        public float GetOutageHours() => _state.bunkerPowerOutageHours;

        // --- helpers ----------------------------------------------------------
        private static string InferRobotType(string targetId)
        {
            if (string.IsNullOrEmpty(targetId)) return "unknown";
            if (targetId.IndexOf("robot_dog", StringComparison.OrdinalIgnoreCase) >= 0)
                return "robot_dog";
            if (targetId.IndexOf("turret", StringComparison.OrdinalIgnoreCase) >= 0)
                return "turret";
            if (targetId.IndexOf("drone", StringComparison.OrdinalIgnoreCase) >= 0)
                return "drone";
            return "robotic";
        }

        // --- Save / Load -----------------------------------------------------
        public EMPGrenadeState CaptureState() => new EMPGrenadeState
        {
            itemId = _state.itemId,
            disablesRobotics = _state.disablesRobotics,
            bunkerPowerOutageHours = _state.bunkerPowerOutageHours
        };

        public void RestoreState(EMPGrenadeState saved)
        {
            _state.itemId = saved.itemId;
            _state.disablesRobotics = saved.disablesRobotics;
            _state.bunkerPowerOutageHours = saved.bunkerPowerOutageHours;
        }
    }
}
