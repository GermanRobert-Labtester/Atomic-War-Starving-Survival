using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Expansion IV — Chapter 45 UI Additions.
    /// StructuralStressWireframe: Diegetic X-ray overlay activated when selecting The_Concrete_Boss or The_Architect.
    /// Rebar grids are rendered over concrete walls; rusting rebar glows pulsing orange; spalling concrete shows as black voids.
    /// </summary>
    public class StructuralStressWireframe : MonoBehaviour
    {
        [SerializeField] private Color healthyRebarColor = new Color(0.2f, 0.8f, 0.2f, 0.6f);
        [SerializeField] private Color rustingRebarColor = new Color(1.0f, 0.35f, 0.0f, 0.9f);
        [SerializeField] private Color spallingVoidColor = new Color(0.05f, 0.05f, 0.05f, 1.0f);

        private bool _isOverlayActive;
        private StructuralEntropySystem _entropySystem;

        public bool IsOverlayActive => _isOverlayActive;

        public void BindEntropySystem(StructuralEntropySystem entropySystem)
        {
            _entropySystem = entropySystem;
        }

        public void OnSurvivorSelected(Survivor selectedSurvivor)
        {
            if (selectedSurvivor == null || !selectedSurvivor.IsAlive)
            {
                SetOverlayActive(false);
                return;
            }

            bool isInspectorRole = string.Equals(selectedSurvivor.ArchetypeId, "survivor_concrete_boss", StringComparison.OrdinalIgnoreCase) ||
                                  selectedSurvivor.HasTrait("trait_architect") ||
                                  selectedSurvivor.HasTrait("trait_concrete_boss");

            SetOverlayActive(isInspectorRole);
        }

        public void SetOverlayActive(bool active)
        {
            _isOverlayActive = active;
        }

        public Color GetRoomRebarColor(ShelterRoom room)
        {
            if (room == null) return healthyRebarColor;
            if (room.IsSpalling) return spallingVoidColor;

            float pulse = Mathf.Sin(Time.time * 5.0f) * 0.15f + 0.85f;
            Color interpolated = Color.Lerp(healthyRebarColor, rustingRebarColor, room.RebarCorrosion);
            interpolated.a *= pulse;
            return interpolated;
        }
    }
}
