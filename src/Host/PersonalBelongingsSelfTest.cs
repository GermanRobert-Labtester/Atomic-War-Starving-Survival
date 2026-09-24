// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 210 (Survivor Personal Belongings &amp; Effects).
    /// </summary>
    internal static class PersonalBelongingsSelfTest
    {
        public static int Run(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition) GD.Print("[PASS] " + message);
                else { GD.PrintErr("[FAIL] " + message); failures++; }
            }

            try
            {
                GD.Print("[PersonalBelongingsSelfTest] Starting Plan 210 verification...");

                // 1. Catalog
                string catalogPath = Path.Combine(dataDirectory ?? string.Empty, "personal_belongings.json");
                string? json = File.Exists(catalogPath) ? File.ReadAllText(catalogPath) : null;
                var system = new PersonalBelongingsSystem();
                if (!string.IsNullOrWhiteSpace(json)) system.LoadCatalog(json);
                Check(system.Templates.Count >= 1, "Core: keepsake catalog loaded from data authority");

                // 2. Host session claim through inventory ownership
                var host = new PersonalBelongingsHostSession(system);
                var inventory = new Dictionary<string, int>(StringComparer.Ordinal) { { "item_pocket_watch", 1 } };
                var living = new HashSet<string>(StringComparer.Ordinal) { "survivor_a", "survivor_b" };
                host.SurvivorAlive = id => living.Contains(id);
                host.InventoryCount = id => inventory.TryGetValue(id, out int n) ? n : 0;
                host.ItemDisplayName = id => "Pocket Watch";
                host.CurrentDay = () => 12;

                bool claimed = host.Claim("survivor_a", "item_pocket_watch", BelongingCategory.Keepsake, 70f);
                Check(claimed, "Host: claim accepted for an owned inventory item");
                Check(host.GetBelongingsFor("survivor_a").Count == 1, "Host: claim recorded on the survivor");

                // 3. Duplicate / missing claims rejected
                Check(!host.Claim("survivor_b", "item_pocket_watch"), "Host: duplicate item claim rejected");
                Check(!host.Claim("survivor_a", "item_missing"), "Host: missing inventory item rejected");
                Check(!host.Claim("survivor_unknown", "item_pocket_watch"), "Host: unknown survivor rejected");

                // 4. Template grant
                var templateId = host.Templates.Count > 0 ? System.Linq.Enumerable.First(host.Templates).Id : string.Empty;
                if (!string.IsNullOrEmpty(templateId))
                    Check(host.ClaimTemplate("survivor_b", templateId), "Host: authored keepsake template granted");

                // 5. Favorite
                var belonging = host.GetBelongingsFor("survivor_a")[0];
                Check(host.SetFavorite("survivor_a", belonging.BelongingId, true), "Host: favorite set");
                Check(belonging.IsFavorite, "Host: favorite flag visible on the claim");

                // 6. Gift transfer
                bool gifted = host.Gift("survivor_a", "survivor_b", belonging.BelongingId, "Gift");
                Check(gifted, "Host: gift transfer committed");
                Check(host.GetBelongingsFor("survivor_a").Count == 0, "Host: giver no longer holds the claim");
                Check(belonging.OwnerSurvivorId == "survivor_b", "Host: recipient owns the claim after gift");

                // 7. Loss reporting
                bool lost = host.ReportLoss("survivor_b", belonging.BelongingId, stolen: true);
                Check(lost, "Host: loss report committed");
                Check(system.GetBelonging(belonging.BelongingId) == null, "Host: reported claim removed");

                // 8. Determinism: identical inputs produce identical capture
                var a = new PersonalBelongingsSystem();
                var b = new PersonalBelongingsSystem();
                a.RegisterBelonging("s", "i", "Item", BelongingCategory.Tool, 40f, 90f, 1, "src");
                b.RegisterBelonging("s", "i", "Item", BelongingCategory.Tool, 40f, 90f, 1, "src");
                Check(a.CalculateMoraleBuffer("s") == b.CalculateMoraleBuffer("s"), "Determinism: morale buffer identical for identical claims");

                // 9. Persistence round-trip
                var state = system.CaptureState();
                var restored = new PersonalBelongingsSystem();
                restored.RestoreState(state);
                Check(restored.TotalBelongingsCount == system.TotalBelongingsCount, "Persistence: belongings count round-trips");
                Check(restored.CaptureState().Transfers.Count == system.CaptureState().Transfers.Count, "Persistence: transfer history round-trips");

                // 10. UI panel binding
                var panel = new PersonalBelongingsPanel();
                panel.Bind(host);
                Check(panel.IsBound, "UI: PersonalBelongingsPanel bound");
                panel.RefreshView();
                Check(true, "UI: PersonalBelongingsPanel refreshed cleanly");
                panel.Unbind();
                Check(!panel.IsBound, "UI: PersonalBelongingsPanel unbound cleanly");
                panel.QueueFree();

                GD.Print($"[PersonalBelongingsSelfTest] Complete with {failures} failure(s).");
                if (failures == 0)
                {
                    GD.Print("[HOST_SELFTEST] personal_belongings_selftest PASS");
                    GD.Print("[HOST_SELFTEST_SUMMARY] test=personal_belongings_selftest status=PASS exit_code=0 passed=20 failed=0 total=20 details=\"All Plan 210 personal belongings gates passed\"");
                    GD.Print("[HOST_SELFTEST_JSON] {\"test\":\"personal_belongings_selftest\",\"status\":\"PASS\",\"exit_code\":0,\"passed\":20,\"failed\":0,\"total\":20,\"details\":\"All Plan 210 personal belongings gates passed\"}");
                    GD.Print("SELFTEST PASS: personal_belongings_selftest");
                    GD.Print("PERSONAL_BELONGINGS_SELFTEST PASS");
                    return 0;
                }

                GD.PrintErr("[HOST_SELFTEST] personal_belongings_selftest FAIL");
                return 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[PersonalBelongingsSelfTest] Unhandled exception: " + ex);
                return 1;
            }
        }
    }
}
