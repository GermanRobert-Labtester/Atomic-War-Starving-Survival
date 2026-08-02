using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Root HUD controller: subscribes to system events on the EventBus and
    /// routes state to child widgets (NeedsBar, DosimeterHUD). Thin MonoBehaviour;
    /// holds no game logic.
    /// </summary>
    public class HUD : MonoBehaviour
    {
        [SerializeField] private NeedsBar _needsBar;
        [SerializeField] private DosimeterHUD _dosimeterHud;

        /// <summary>Bind all widgets to a specific survivor's state.</summary>
        public void Bind(Survivor survivor) => throw new System.NotImplementedException();

        /// <summary>Refresh every widget from current system state.</summary>
        public void Refresh() => throw new System.NotImplementedException();
    }
}
