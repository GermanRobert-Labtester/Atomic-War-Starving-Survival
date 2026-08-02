using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Guided first-3-days tutorial overlay. Steps through key mechanics
    /// (eat, drink, shelter, crafting, events) with tooltip panels.
    /// Attach to the HUD GameObject; activate on first run.
    /// </summary>
    public class TutorialOverlay : MonoBehaviour
    {
        // Day provider injected by the composition root; avoids a UI -> Core assembly
        // cycle (Core already references UI). Null until SetDayProvider is called.
        private System.Func<int> _getCurrentDay;

        /// <summary>Inject a provider for the current game day (e.g. () => TimeSystem.CurrentDay).</summary>
        public void SetDayProvider(System.Func<int> getCurrentDay)
        {
            _getCurrentDay = getCurrentDay;
        }

        public enum TutorialStep
        {
            None,
            Welcome,
            NeedsExplained,
            EatAndDrink,
            ShelterExplained,
            CraftingIntro,
            EventsIntro,
            RadExplained,
            Done
        }

        public TutorialStep CurrentStep { get; private set; } = TutorialStep.None;
        public bool IsActive { get; private set; }
        public string CurrentMessage { get; private set; }

        private int _lastDay = -1;

        private static readonly string[] Messages =
        {
            "",
            "Welcome to the bunker. The bombs have fallen. You must survive.\n\nPress SPACE to continue.",
            "Your survivors have needs: hunger, thirst, warmth, fatigue, morale, and health.\nIf any reach critical levels, health will drain.\n\nPress SPACE to continue.",
            "Press F1 to eat, F2 to drink. Rations are limited — scavenge to find more.\n\nPress SPACE to continue.",
            "The shelter provides radiation shielding, air filtration, and warmth.\nKeep the filter maintained and the heater fueled.\n\nPress SPACE to continue.",
            "Press C to open crafting. Recipes require a workbench and materials.\nCraft bandages, filters, and anti-rad medication.\n\nPress SPACE to continue.",
            "Events will occur — strangers at the door, supply drops, emergencies.\nChoose wisely. Press 1, 2, or 3 to select a choice.\n\nPress SPACE to continue.",
            "Radiation accumulates from the environment. Iodine pills grant temporary resistance.\nAnti-rad reduces current dose. Watch the dosimeter.\n\nPress SPACE to dismiss tutorial.",
            ""
        };

        public void StartTutorial()
        {
            CurrentStep = TutorialStep.Welcome;
            IsActive = true;
            CurrentMessage = Messages[(int)CurrentStep];
        }

        public void Advance()
        {
            if (!IsActive) return;

            int next = (int)CurrentStep + 1;
            if (next >= (int)TutorialStep.Done)
            {
                EndTutorial();
                return;
            }

            CurrentStep = (TutorialStep)next;
            CurrentMessage = Messages[(int)CurrentStep];
        }

        public void EndTutorial()
        {
            CurrentStep = TutorialStep.None;
            IsActive = false;
            CurrentMessage = "";
        }

        private void Update()
        {
            if (!IsActive) return;

            // Auto-advance on day change for later steps
            if (_getCurrentDay != null)
            {
                int day = _getCurrentDay();
                if (day != _lastDay)
                {
                    _lastDay = day;
                    if (day >= 3 && CurrentStep < TutorialStep.Done)
                    {
                        EndTutorial();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Advance();
            }
        }
    }
}
