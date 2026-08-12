using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class Phase11HudWiringTests
    {
        [Test]
        public void GameBootstrap_HasWirePhase11ExpansionHudMethod()
        {
            var method = typeof(GameBootstrap).GetMethod(
                "WirePhase11ExpansionHud",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method, "GameBootstrap must define WirePhase11ExpansionHud");
        }

        [Test]
        public void HUD_ExposesAllPhase11Widgets()
        {
            var go = new GameObject("HUD");
            var hud = go.AddComponent<HUD>();

            Assert.NotNull(hud.RadiationPhaseIndicator);
            Assert.NotNull(hud.PhantomMemoryVignette);
            Assert.NotNull(hud.HypervigilanceIndicator);
            Assert.NotNull(hud.MoralBranchDisplay);
            Assert.NotNull(hud.KeepsakeSlotUi);
            Assert.NotNull(hud.MemorialWallUi);
            Assert.NotNull(hud.TerminalPrognosisBanner);
            Assert.NotNull(hud.AddictionDetoxIndicator);

            Object.DestroyImmediate(go);
        }
    }
}
