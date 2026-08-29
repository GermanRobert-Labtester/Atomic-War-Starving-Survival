// SPDX-License-Identifier: MIT
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// Run the runtime selftest manifest: emit JSON describing every test
        /// registered in <see cref="HostCliRegistry"/>. Used by
        /// <c>scripts/ci/generate-selftest-manifest.py</c> via
        /// <c>--selftest-manifest</c>.
        /// </summary>
        public static int RunSelfTestManifest(string dataDirectory)
        {
            string json = HostCliRegistry.GenerateJsonManifest();
            GD.Print("[HOST_SELFTEST] === ASHFALL Self-Test Manifest ===");
            GD.Print(json);
            return EmitSummary("selftest_manifest", true, 0,
                details: "PASS: self-test manifest emitted");
        }

        /// <summary>
        /// Print all registered selftests and run their signature live so a
        /// developer can audit the runtime/CLI parity without rebuilding.
        /// </summary>
        public static int RunListSelfTests(string dataDirectory)
        {
            HostCliRegistry.PrintSelfTests(line => GD.Print(line));
            return EmitSummary("list_selftests", true, 0,
                details: "PASS: self-test list emitted");
        }
    }
}
