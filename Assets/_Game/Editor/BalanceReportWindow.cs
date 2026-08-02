using UnityEditor;
using UnityEngine;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Editor window that displays balance-report data from the most recent
    /// SurvivalSmokeTest run. Open via Tools/ASHFALL/Balance Report.
    /// Feed data via BalanceReportWindow.SetReport(formattedText).
    /// </summary>
    public class BalanceReportWindow : EditorWindow
    {
        private Vector2 _scroll;
        private string _reportText = "No report data yet. Run the SurvivalSmokeTest to generate.";

        [MenuItem("Tools/ASHFALL/Balance Report")]
        public static void ShowWindow()
        {
            GetWindow<BalanceReportWindow>("ASHFALL Balance Report");
        }

        /// <summary>Set the report text from external code (e.g. a test).</summary>
        public static void SetReport(string text)
        {
            var window = GetWindow<BalanceReportWindow>(false);
            if (window != null)
            {
                window._reportText = text ?? "(null)";
                window.Repaint();
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("ASHFALL Balance Report", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Run SurvivalSmokeTest (EditMode) to populate this window.\n" +
                "The report shows per-survivor stats: days survived, cause of death,\n" +
                "average radiation, peak radiation, hours starving/dehydrated/freezing,\n" +
                "and whether chronic illness or ARS developed.",
                MessageType.Info);

            EditorGUILayout.Space(8);

            if (GUILayout.Button("Copy Report to Clipboard"))
            {
                EditorGUIUtility.systemCopyBuffer = _reportText;
                Debug.Log("[BalanceReport] Copied to clipboard.");
            }

            EditorGUILayout.Space(4);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            EditorGUILayout.TextArea(_reportText, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }
    }
}
