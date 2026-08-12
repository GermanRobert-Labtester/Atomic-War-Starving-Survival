using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;
using AtomicWar._Game.Utilities;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>Debug-session probe: does DiegeticHud.uxml clone named expansion panels?</summary>
    [TestFixture]
    public class DiegeticHudUxmlExpansionProbeTests
    {
        const string UxmlPath = "Assets/_Game/UI/DiegeticHud.uxml";

        [Test]
        public void CloneTree_ExposesExpansionAndPhase11Roots()
        {
            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
            Assert.IsNotNull(uxml, "DiegeticHud.uxml must import");

            var root = uxml.CloneTree();
            Assert.IsNotNull(root);

            string[] names =
            {
                "diegetic-root",
                "vitals-panel",
                "radiation-phase-root",
                "keepsake-slot-root",
                "location-detail-panel",
                "item-condition-badge",
                "questline-tracker",
                "siege-status",
                "faction-intelligence-panel",
                "vehicle-status-panel",
                "tactical-command-bar",
                "questline-stage-tracker",
                "lore-codex-panel",
                "faction-relationship-map",
                "character-arc-panel"
            };

            var sb = new System.Text.StringBuilder();
            sb.Append('{');
            sb.Append("\"childCount\":").Append(root.childCount);
            for (int i = 0; i < names.Length; i++)
            {
                bool found = root.Q(names[i]) != null;
                sb.Append(",\"").Append(names[i]).Append("\":").Append(found ? "true" : "false");
            }
            sb.Append('}');
            AgentDebugLog.Write("H1", "DiegeticHudUxmlExpansionProbeTests.CloneTree", "uxml clone probe", sb.ToString());

            Assert.IsNotNull(root.Q("diegetic-root"));
            Assert.IsNotNull(root.Q("vitals-panel"));
        }
    }
}
