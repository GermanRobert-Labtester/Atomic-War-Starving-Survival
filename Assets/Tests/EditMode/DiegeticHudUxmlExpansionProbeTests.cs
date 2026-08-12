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

        [Test]
        public void EnsureBuilt_KeepsExpansionRoots()
        {
            var go = new UnityEngine.GameObject("DiegeticHudExpansionProbe");
            var diegetic = go.AddComponent<AtomicWar._Game.UI.DiegeticHudController>();
            try
            {
                Assert.IsTrue(diegetic.EnsureDocumentMounted());
                diegetic.EnsureBuilt();
                var docRoot = diegetic.Document != null ? diegetic.Document.rootVisualElement : null;
                bool hasLoc = docRoot != null && docRoot.Q("location-detail-panel") != null;
                bool hasSiege = docRoot != null && docRoot.Q("siege-status") != null;
                bool hasRad = docRoot != null && docRoot.Q("radiation-phase-root") != null;
                AgentDebugLog.Write("H5", "DiegeticHudUxmlExpansionProbeTests.EnsureBuilt", "controller probe",
                    "{\"docNull\":" + (diegetic.Document == null ? "true" : "false")
                    + ",\"isBuilt\":" + (diegetic.IsBuilt ? "true" : "false")
                    + ",\"hasLocationPanel\":" + (hasLoc ? "true" : "false")
                    + ",\"hasSiege\":" + (hasSiege ? "true" : "false")
                    + ",\"hasRadPhase\":" + (hasRad ? "true" : "false")
                    + ",\"viewRootName\":\"" + (diegetic.View != null && diegetic.View.Root != null ? diegetic.View.Root.name : "null") + "\"}");
                Assert.IsTrue(diegetic.IsBuilt);
                Assert.IsTrue(hasLoc, "EnsureBuilt must keep location-detail-panel from authored UXML");
                Assert.IsTrue(hasRad, "EnsureBuilt must keep radiation-phase-root from authored UXML");
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(go);
            }
        }
    }
}
