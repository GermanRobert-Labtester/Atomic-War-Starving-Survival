// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class HostCliCloudSeeding
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Cloud Seeding Self-Test (PLAN-WEATHER-ATMOSPHERE-28 cloud-seeding package) ===");
            int passed = 0; const int total = 7;
            try
            {
                var weather = new WeatherSystem();
                var session = new CloudSeedingHostSession();
                session.Bind(weather, null, new SeededRng(28));

                if (session.IsBound) { Console.WriteLine("[PASS] Check 1: Instrument bound to canonical weather owner."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: bind failed.");

                var pre0 = session.Preflight(1, WeatherKind.FalloutStorm);
                if (!pre0.CanDeploy && pre0.Reason == "cloud_seeding.not_installed")
                { Console.WriteLine("[PASS] Check 2: Deploy refused before install."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: preflight allowed uninstalled deploy.");

                var install = session.Install(1);
                if (session.IsInstalled) { Console.WriteLine($"[PASS] Check 3: Instrument installed ({install.MessageKey})."); passed++; }
                else Console.WriteLine("[FAIL] Check 3: install failed.");

                var pre1 = session.Preflight(2, WeatherKind.FalloutStorm, 3);
                if (pre1.CanDeploy && pre1.SuccessChance > 0f) { Console.WriteLine($"[PASS] Check 4: Preflight computes success chance ({pre1.SuccessChance:0.00})."); passed++; }
                else Console.WriteLine($"[FAIL] Check 4: preflight refused ({pre1.Reason}).");

                var deploy = session.Deploy(2, WeatherKind.FalloutStorm, 3);
                if (deploy != null && session.CooldownRemaining > 0) { Console.WriteLine($"[PASS] Check 5: Deploy applied cooldown ({session.CooldownRemaining}d)."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: deploy did not apply cooldown.");

                var blocked = session.Preflight(3, WeatherKind.FalloutStorm, 4);
                if (!blocked.CanDeploy && blocked.Reason == "cloud_seeding.cooldown_active") { Console.WriteLine("[PASS] Check 6: Cooldown blocks a second deploy."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: cooldown not enforced.");

                var state = session.CaptureState();
                var restored = new CloudSeedingHostSession();
                restored.Bind(weather, null, new SeededRng(1));
                restored.RestoreState(state);
                if (restored.IsInstalled == session.IsInstalled && restored.CooldownRemaining == session.CooldownRemaining)
                { Console.WriteLine("[PASS] Check 7: Save/restore round-trips instrument state."); passed++; }
                else Console.WriteLine("[FAIL] Check 7: restore mismatch.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Cloud Seeding Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
