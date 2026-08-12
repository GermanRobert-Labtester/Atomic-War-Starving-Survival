using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Authored DiegeticHud.uxml must expose expansion / Phase 11 roots, and
    /// EnsureBuilt must bind that tree instead of wiping it.
    /// </summary>
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
            Assert.IsNotNull(root.Q("diegetic-root"));
            Assert.IsNotNull(root.Q("vitals-panel"));
            Assert.AreEqual(19, DiegeticHudController.ExpansionWidgetRootNames.Length);
            for (int i = 0; i < DiegeticHudController.ExpansionWidgetRootNames.Length; i++)
            {
                string name = DiegeticHudController.ExpansionWidgetRootNames[i];
                Assert.IsNotNull(root.Q(name), "DiegeticHud.uxml must expose " + name);
            }
        }

        [Test]
        public void EnsureBuilt_KeepsExpansionRoots()
        {
            var go = new UnityEngine.GameObject("DiegeticHudExpansionProbe");
            var diegetic = go.AddComponent<DiegeticHudController>();
            try
            {
                Assert.IsTrue(diegetic.EnsureDocumentMounted());
                diegetic.EnsureBuilt();
                var docRoot = diegetic.Document != null ? diegetic.Document.rootVisualElement : null;
                Assert.IsNotNull(docRoot);
                Assert.IsTrue(diegetic.IsBuilt);
                Assert.IsNotNull(docRoot.Q("location-detail-panel"),
                    "EnsureBuilt must keep location-detail-panel from authored UXML");
                Assert.IsNotNull(docRoot.Q("radiation-phase-root"),
                    "EnsureBuilt must keep radiation-phase-root from authored UXML");
                Assert.IsNotNull(docRoot.Q("siege-status"),
                    "EnsureBuilt must keep siege-status from authored UXML");
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(go);
            }
        }
    }
}
