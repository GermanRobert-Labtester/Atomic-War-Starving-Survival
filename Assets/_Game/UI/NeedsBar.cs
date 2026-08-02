using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Renders a survivor's need values (hunger, thirst, fatigue, warmth, morale,
    /// health) as bars. Refreshed by the HUD when need events fire. Thin MonoBehaviour.
    /// </summary>
    public class NeedsBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _barRoot;

        /// <summary>Update all bars from a needs snapshot.</summary>
        public void SetNeeds(Needs needs) => throw new System.NotImplementedException();
    }
}
