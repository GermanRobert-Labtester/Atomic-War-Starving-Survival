using UnityEngine;
using UnityEngine.UI;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Minimal starting-screen controller. Shows the UI style-reference image
    /// fullscreen and lets the player leave the app immediately with ESC.
    ///
    /// This is intentionally a thin placeholder entry point: the actual main
    /// menu (buttons, Continue/New Game, Options, etc.) is authored separately
    /// and wired onto this scene. Keep this file dependency-light so the deploy
    /// stays small and RAM-friendly.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class StartScreenController : MonoBehaviour
    {
        [Header("Background")]
        [Tooltip("Fullscreen style-reference image shown on launch.")]
        [SerializeField] private RawImage _background;

        [Header("Behaviour")]
        [Tooltip("Exit the application immediately when ESC is pressed.")]
        [SerializeField] private bool _quitOnEscape = true;

        private void Awake()
        {
            // Keep pointer visible and unlocked so the (future) menu is usable.
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void Update()
        {
            if (!_quitOnEscape) return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Quit();
            }
        }

        /// <summary>Immediate, no-cond interstitial exit.</summary>
        public void Quit()
        {
            Application.Quit();
#if UNITY_EDITOR
            // Allow the flow to be exercised inside the Editor without a full app exit.
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        /// <summary>Assign the background texture safely (used by the scene builder).</summary>
        public void SetBackground(RawImage target) => _background = target;
    }
}