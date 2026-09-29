// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        public static int RunWorldSelfTest()
        {
            var report = WorldHeadlessDemo.Run(new GodotLog());

            // ── WorldHostSession Sky Layer Armor save restoration probe ──
            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition)
                {
                    report.PassedCount++;
                    GD.Print("[PASS] " + name);
                }
                else
                {
                    report.FailedCount++;
                    GD.Print("[FAIL] " + name);
                }
            }

            try
            {
                var session1 = new WorldHostSession();
                Check(session1.SkyArmorStatusLine() == "Sky armor: no cells plated", "world sky armor initially unplated");

                session1.SetSkyArmorDemo(0, "concrete", 2.0f);
                session1.SetSkyArmorDemo(1, "lead", 1.5f);
                Check(session1.SkyArmor.GetCell(0) != null, "session1 set cell 0 (concrete)");
                Check(session1.SkyArmor.GetCell(1) != null, "session1 set cell 1 (lead)");

                var save = session1.CaptureSkyArmorSave();
                Check(save != null && save.cells.Count == 2, "capture sky armor save captures 2 cells");

                var session2 = new WorldHostSession();
                Check(session2.SkyArmor.GetCell(0) == null, "session2 initially empty");

                session2.RestoreSkyArmorSave(save!);
                var c0 = session2.SkyArmor.GetCell(0);
                var c1 = session2.SkyArmor.GetCell(1);
                Check(c0 != null && c0.material == CeilingMaterialTier.ReinforcedConcrete && Math.Abs(c0.thicknessMeters - 2.0f) < 0.001f, "session2 restored cell 0 concrete");
                Check(c1 != null && c1.material == CeilingMaterialTier.LeadSheeting && Math.Abs(c1.thicknessMeters - 1.5f) < 0.001f, "session2 restored cell 1 lead");
                Check(session2.SkyArmorStatusLine().StartsWith("Sky armor: 2 cells"), "session2 status line reflects restored cells");

                string absorbed = session2.ImpactDemo(0, 10f);
                Check(absorbed.Contains("absorbed"), "session2 impact absorbed by concrete");

                string breach = session2.ImpactDemo(0, 100f);
                Check(breach.Contains("BREACH"), "session2 impact breaches concrete");

                // Overwrite test: new state replaces prior cells
                var smallSave = new SkyArmorSaveState
                {
                    cells = new System.Collections.Generic.List<CeilingCellArmor>
                    {
                        new CeilingCellArmor { gridX = 5, material = CeilingMaterialTier.TungstenComposite, thicknessMeters = 3.0f, currentDurability = 100f }
                    }
                };
                session2.RestoreSkyArmorSave(smallSave);
                Check(session2.SkyArmor.GetCell(0) == null && session2.SkyArmor.GetCell(5) != null, "restore overwrites previous cells");

                // Null safety test: safely clears existing state
                session2.RestoreSkyArmorSave(null!);
                Check(session2.SkyArmorStatusLine() == "Sky armor: no cells plated", "restore null clears cells safely");
            }
            catch (Exception ex)
            {
                Check(false, "WorldHostSession sky armor probe exception: " + ex.Message);
            }

            report.Passed = report.FailedCount == 0;
            report.Summary = $"[WorldHeadlessDemo] {(report.Passed ? "PASS" : "FAIL")} {report.PassedCount}/{report.PassedCount + report.FailedCount}";

            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("world_selftest", report);
        }

    }
}
