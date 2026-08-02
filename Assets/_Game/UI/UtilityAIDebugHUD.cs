using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Debug HUD overlay displaying all available SurvivorActions and their live score
    /// for selected survivors. Toggleable with F1.
    /// </summary>
    public class UtilityAIDebugHUD : MonoBehaviour
    {
        public KeyCode ToggleKey = KeyCode.F1;
        public bool ShowHUD = false;

        private Survivor _activeSurvivor;
        private AIContext _activeContext;
        private List<SurvivorAction> _candidates = new List<SurvivorAction>();
        private ActionScorer _scorer = new ActionScorer();

        public void SetTarget(Survivor survivor, AIContext context, IEnumerable<SurvivorAction> candidates)
        {
            _activeSurvivor = survivor;
            _activeContext = context;
            _candidates.Clear();
            if (candidates != null)
            {
                _candidates.AddRange(candidates);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(ToggleKey))
            {
                ShowHUD = !ShowHUD;
            }
        }

        private void OnGUI()
        {
            if (!ShowHUD) return;

            GUILayout.BeginArea(new Rect(20, 20, 420, 600), "Utility AI Live Debug HUD (F1)", GUI.skin.window);

            if (_activeSurvivor != null)
            {
                GUILayout.Label($"Survivor: {_activeSurvivor.DisplayName} (ID: {_activeSurvivor.Id})");
                GUILayout.Label($"Hunger: {_activeSurvivor.Needs.Hunger:F1} | Thirst: {_activeSurvivor.Needs.Thirst:F1} | Fatigue: {_activeSurvivor.Needs.Fatigue:F1}");
                GUILayout.Label($"Warmth: {_activeSurvivor.Needs.Warmth:F1} | Morale: {_activeSurvivor.Needs.Morale:F1} | RadDose: {_activeSurvivor.RadiationDose:F1}");
                GUILayout.Space(10);

                GUILayout.Label("Candidate Actions & Live Scores:");
                for (int i = 0; i < _candidates.Count; i++)
                {
                    var action = _candidates[i];
                    if (action != null)
                    {
                        float score = _scorer.Score(action, _activeContext ?? new AIContext(_activeSurvivor));
                        GUILayout.BeginHorizontal();
                        GUILayout.Label($"{action.displayName} ({action.id})", GUILayout.Width(200));
                        GUILayout.HorizontalSlider(score, 0f, 1f, GUILayout.Width(100));
                        GUILayout.Label($"{score:F3}");
                        GUILayout.EndHorizontal();
                    }
                }
            }
            else
            {
                GUILayout.Label("No active survivor registered for Utility AI debug.");
            }

            GUILayout.EndArea();
        }
    }
}
