using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// PlayMode test stubs (run in a player/scene context). Replace with real
    /// integration tests (bootstrap wiring, time/needs tick over frames, save/load
    /// round-trip) as systems land.
    /// </summary>
    [TestFixture]
    public class GameBootstrapStubTests
    {
        [UnityTest]
        public IEnumerator Bootstrap_Scene_IsStubbed()
        {
            yield return null;
            Assert.Pass("Stub: bootstrap integration not yet implemented.");
        }
    }
}
