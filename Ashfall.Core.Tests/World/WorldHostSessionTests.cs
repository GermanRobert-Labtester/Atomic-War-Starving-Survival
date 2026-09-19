// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Tests for the WorldHostSession Sky Layer Armor contract:
    /// validating that RestoreSkyArmorSave delegates to SkyLayerArmorSystem.RestoreState,
    /// clears previous cell configurations, restores cells across all material tiers,
    /// preserves attenuation calculations and kinetic impact resistance, safely handles
    /// null or empty payloads, updates status lines, and is idempotent.
    /// </summary>
    public class WorldHostSessionTests
    {
        /// <summary>
        /// Test double mirroring WorldHostSession's Sky Layer Armor delegation contract.
        /// </summary>
        private sealed class TestWorldHostSession : StatefulSessionBase
        {
            public SkyLayerArmorSystem SkyArmor { get; }

            public TestWorldHostSession(SkyLayerArmorSystem? skyArmor = null)
            {
                SkyArmor = skyArmor ?? new SkyLayerArmorSystem();
            }

            public string SetSkyArmorDemo(int gridX, string material, float thickness)
            {
                var tier = material switch
                {
                    "dirt" => CeilingMaterialTier.Dirt,
                    "wood" => CeilingMaterialTier.Wood,
                    "concrete" => CeilingMaterialTier.ReinforcedConcrete,
                    "lead" => CeilingMaterialTier.LeadSheeting,
                    "tungsten" => CeilingMaterialTier.TungstenComposite,
                    _ => CeilingMaterialTier.Dirt
                };
                SkyArmor.SetCellArmor(gridX, tier, thickness);
                return $"Sky armor set at grid {gridX}: {tier} ({thickness}m). Attenuation: {SkyArmor.GetAttenuationFactor(gridX):F3}.";
            }

            public string ImpactDemo(int gridX, float energyMJ)
            {
                bool breached = SkyArmor.EvaluateKineticImpact(gridX, energyMJ, out float damage);
                return breached ? $"BREACH at grid {gridX}! {damage:F1} MJ through." : $"Impact absorbed at grid {gridX}.";
            }

            public string SkyArmorStatusLine()
            {
                var save = SkyArmor.CaptureState();
                if (save.cells.Count == 0) return "Sky armor: no cells plated";
                return $"Sky armor: {save.cells.Count} cells · avg attenuation {AvgAttenuation():F3}";
            }

            private float AvgAttenuation()
            {
                var save = SkyArmor.CaptureState();
                if (save.cells.Count == 0) return 1f;
                float sum = 0f;
                foreach (var c in save.cells) sum += SkyArmor.GetAttenuationFactor(c.gridX);
                return sum / save.cells.Count;
            }

            public SkyArmorSaveState CaptureSkyArmorSave() => SkyArmor.CaptureState();
            public void RestoreSkyArmorSave(SkyArmorSaveState state) => SkyArmor.RestoreState(state);
        }

        [Fact]
        public void RestoreSkyArmorSave_NullState_ClearsExistingCellsGracefully()
        {
            var session = new TestWorldHostSession();
            session.SetSkyArmorDemo(0, "concrete", 2.0f);
            session.SetSkyArmorDemo(1, "lead", 1.5f);

            Assert.NotNull(session.SkyArmor.GetCell(0));
            Assert.NotNull(session.SkyArmor.GetCell(1));

            session.RestoreSkyArmorSave(null!);

            Assert.Null(session.SkyArmor.GetCell(0));
            Assert.Null(session.SkyArmor.GetCell(1));
            Assert.Equal("Sky armor: no cells plated", session.SkyArmorStatusLine());
            Assert.Empty(session.CaptureSkyArmorSave().cells);
        }

        [Fact]
        public void RestoreSkyArmorSave_EmptyState_ClearsExistingCellsGracefully()
        {
            var session = new TestWorldHostSession();
            session.SetSkyArmorDemo(5, "tungsten", 3.0f);

            Assert.NotNull(session.SkyArmor.GetCell(5));

            session.RestoreSkyArmorSave(new SkyArmorSaveState());

            Assert.Null(session.SkyArmor.GetCell(5));
            Assert.Equal("Sky armor: no cells plated", session.SkyArmorStatusLine());
            Assert.Empty(session.CaptureSkyArmorSave().cells);
        }

        [Fact]
        public void RestoreSkyArmorSave_NullCellsProperty_ClearsExistingCells()
        {
            var session = new TestWorldHostSession();
            session.SetSkyArmorDemo(2, "wood", 1.0f);

            var corruptSave = new SkyArmorSaveState { cells = null! };
            session.RestoreSkyArmorSave(corruptSave);

            Assert.Null(session.SkyArmor.GetCell(2));
            Assert.Equal("Sky armor: no cells plated", session.SkyArmorStatusLine());
        }

        [Fact]
        public void RestoreSkyArmorSave_PopulatedState_RestoresAllMaterialTiers()
        {
            var session = new TestWorldHostSession();
            var state = new SkyArmorSaveState
            {
                cells = new List<CeilingCellArmor>
                {
                    new CeilingCellArmor { gridX = 0, material = CeilingMaterialTier.Dirt, thicknessMeters = 0.5f, currentDurability = 60f },
                    new CeilingCellArmor { gridX = 1, material = CeilingMaterialTier.Wood, thicknessMeters = 1.0f, currentDurability = 70f },
                    new CeilingCellArmor { gridX = 2, material = CeilingMaterialTier.ReinforcedConcrete, thicknessMeters = 1.5f, currentDurability = 80f },
                    new CeilingCellArmor { gridX = 3, material = CeilingMaterialTier.LeadSheeting, thicknessMeters = 2.0f, currentDurability = 90f },
                    new CeilingCellArmor { gridX = 4, material = CeilingMaterialTier.TungstenComposite, thicknessMeters = 2.5f, currentDurability = 100f }
                }
            };

            session.RestoreSkyArmorSave(state);

            Assert.Equal(5, session.CaptureSkyArmorSave().cells.Count);

            var c0 = session.SkyArmor.GetCell(0);
            Assert.NotNull(c0);
            Assert.Equal(CeilingMaterialTier.Dirt, c0.material);
            Assert.Equal(0.5f, c0.thicknessMeters);
            Assert.Equal(60f, c0.currentDurability);

            var c1 = session.SkyArmor.GetCell(1);
            Assert.NotNull(c1);
            Assert.Equal(CeilingMaterialTier.Wood, c1.material);
            Assert.Equal(1.0f, c1.thicknessMeters);
            Assert.Equal(70f, c1.currentDurability);

            var c2 = session.SkyArmor.GetCell(2);
            Assert.NotNull(c2);
            Assert.Equal(CeilingMaterialTier.ReinforcedConcrete, c2.material);
            Assert.Equal(1.5f, c2.thicknessMeters);
            Assert.Equal(80f, c2.currentDurability);

            var c3 = session.SkyArmor.GetCell(3);
            Assert.NotNull(c3);
            Assert.Equal(CeilingMaterialTier.LeadSheeting, c3.material);
            Assert.Equal(2.0f, c3.thicknessMeters);
            Assert.Equal(90f, c3.currentDurability);

            var c4 = session.SkyArmor.GetCell(4);
            Assert.NotNull(c4);
            Assert.Equal(CeilingMaterialTier.TungstenComposite, c4.material);
            Assert.Equal(2.5f, c4.thicknessMeters);
            Assert.Equal(100f, c4.currentDurability);
        }

        [Fact]
        public void RestoreSkyArmorSave_OverwritesExistingCells_RemovesUnrepresentedCells()
        {
            var session = new TestWorldHostSession();
            session.SetSkyArmorDemo(10, "dirt", 1.0f);
            session.SetSkyArmorDemo(11, "wood", 1.0f);
            session.SetSkyArmorDemo(12, "concrete", 1.0f);

            Assert.NotNull(session.SkyArmor.GetCell(10));
            Assert.NotNull(session.SkyArmor.GetCell(11));
            Assert.NotNull(session.SkyArmor.GetCell(12));

            var replacement = new SkyArmorSaveState
            {
                cells = new List<CeilingCellArmor>
                {
                    new CeilingCellArmor { gridX = 42, material = CeilingMaterialTier.TungstenComposite, thicknessMeters = 3.0f, currentDurability = 100f }
                }
            };

            session.RestoreSkyArmorSave(replacement);

            Assert.Null(session.SkyArmor.GetCell(10));
            Assert.Null(session.SkyArmor.GetCell(11));
            Assert.Null(session.SkyArmor.GetCell(12));

            var c42 = session.SkyArmor.GetCell(42);
            Assert.NotNull(c42);
            Assert.Equal(CeilingMaterialTier.TungstenComposite, c42.material);
            Assert.Equal(1, session.CaptureSkyArmorSave().cells.Count);
        }

        [Fact]
        public void RestoreSkyArmorSave_PreservesAttenuationCalculations()
        {
            var source = new TestWorldHostSession();
            source.SetSkyArmorDemo(0, "dirt", 1.0f);
            source.SetSkyArmorDemo(1, "concrete", 2.0f);
            source.SetSkyArmorDemo(2, "tungsten", 2.5f);

            float dirtAtt = source.SkyArmor.GetAttenuationFactor(0);
            float concreteAtt = source.SkyArmor.GetAttenuationFactor(1);
            float tungstenAtt = source.SkyArmor.GetAttenuationFactor(2);

            var save = source.CaptureSkyArmorSave();

            var restored = new TestWorldHostSession();
            restored.RestoreSkyArmorSave(save);

            Assert.Equal(dirtAtt, restored.SkyArmor.GetAttenuationFactor(0), precision: 4);
            Assert.Equal(concreteAtt, restored.SkyArmor.GetAttenuationFactor(1), precision: 4);
            Assert.Equal(tungstenAtt, restored.SkyArmor.GetAttenuationFactor(2), precision: 4);

            // Hierarchy check: tungsten shields far better than concrete, concrete better than dirt
            Assert.True(tungstenAtt < concreteAtt);
            Assert.True(concreteAtt < dirtAtt);
        }

        [Fact]
        public void RestoreSkyArmorSave_UnprotectedGridCellsReturnDefaultBleed()
        {
            var session = new TestWorldHostSession();
            var state = new SkyArmorSaveState
            {
                cells = new List<CeilingCellArmor>
                {
                    new CeilingCellArmor { gridX = 2, material = CeilingMaterialTier.ReinforcedConcrete, thicknessMeters = 1.0f, currentDurability = 100f },
                    new CeilingCellArmor { gridX = 4, material = CeilingMaterialTier.LeadSheeting, thicknessMeters = 1.0f, currentDurability = 100f }
                }
            };

            session.RestoreSkyArmorSave(state);

            // Grid 3 was never configured -> unplated bleed factor is 1.0f
            Assert.Equal(1.0f, session.SkyArmor.GetAttenuationFactor(3));
            Assert.Equal(1.0f, session.SkyArmor.GetAttenuationFactor(99));
        }

        [Fact]
        public void RestoreSkyArmorSave_RestoresKineticImpactAbsorptionBehavior()
        {
            var session = new TestWorldHostSession();
            var state = new SkyArmorSaveState
            {
                cells = new List<CeilingCellArmor>
                {
                    // ReinforcedConcrete base threshold = 25 MJ * 2.0m = 50 MJ
                    new CeilingCellArmor { gridX = 1, material = CeilingMaterialTier.ReinforcedConcrete, thicknessMeters = 2.0f, currentDurability = 100f }
                }
            };

            session.RestoreSkyArmorSave(state);

            // 20 MJ strike <= 50 MJ threshold: absorbed completely
            bool breached = session.SkyArmor.EvaluateKineticImpact(1, 20f, out float roofDamage);
            Assert.False(breached);
            Assert.Equal(0f, roofDamage);

            var cell = session.SkyArmor.GetCell(1);
            Assert.NotNull(cell);
            // Durability drops by (20 / 50) * 20f = 8f -> 92f
            Assert.Equal(92f, cell.currentDurability, precision: 1);
        }

        [Fact]
        public void RestoreSkyArmorSave_RestoresKineticImpactBreachBehavior()
        {
            var session = new TestWorldHostSession();
            var state = new SkyArmorSaveState
            {
                cells = new List<CeilingCellArmor>
                {
                    // Wood base threshold = 2 MJ * 1.0m = 2 MJ
                    new CeilingCellArmor { gridX = 3, material = CeilingMaterialTier.Wood, thicknessMeters = 1.0f, currentDurability = 100f }
                }
            };

            session.RestoreSkyArmorSave(state);

            // 15 MJ strike > 2 MJ threshold: breach!
            bool breached = session.SkyArmor.EvaluateKineticImpact(3, 15f, out float roofDamage);
            Assert.True(breached);
            Assert.Equal(13f, roofDamage, precision: 1);

            var cell = session.SkyArmor.GetCell(3);
            Assert.NotNull(cell);
            // On breach, durability drops by 50f -> 50f
            Assert.Equal(50f, cell.currentDurability);
        }

        [Fact]
        public void RestoreSkyArmorSave_ImpactDemo_ReportsAbsorptionAndBreachAccurately()
        {
            var session = new TestWorldHostSession();
            var state = new SkyArmorSaveState
            {
                cells = new List<CeilingCellArmor>
                {
                    new CeilingCellArmor { gridX = 0, material = CeilingMaterialTier.ReinforcedConcrete, thicknessMeters = 2.0f, currentDurability = 100f }
                }
            };

            session.RestoreSkyArmorSave(state);

            // Impact below threshold -> absorbed
            string absorbedMsg = session.ImpactDemo(0, 15f);
            Assert.Equal("Impact absorbed at grid 0.", absorbedMsg);

            // Impact above threshold -> breach
            string breachMsg = session.ImpactDemo(0, 75f);
            Assert.StartsWith("BREACH at grid 0!", breachMsg);
            Assert.Contains("MJ through", breachMsg);

            // Impact on unplated grid -> complete penetration
            string unplatedMsg = session.ImpactDemo(88, 10f);
            Assert.StartsWith("BREACH at grid 88!", unplatedMsg);
            Assert.Contains("100.0 MJ through", unplatedMsg);
        }

        [Fact]
        public void RestoreSkyArmorSave_AffectsSkyArmorStatusLine_PlatedVsEmpty()
        {
            var session = new TestWorldHostSession();
            Assert.Equal("Sky armor: no cells plated", session.SkyArmorStatusLine());

            var state = new SkyArmorSaveState
            {
                cells = new List<CeilingCellArmor>
                {
                    new CeilingCellArmor { gridX = 0, material = CeilingMaterialTier.Dirt, thicknessMeters = 1.0f, currentDurability = 100f },
                    new CeilingCellArmor { gridX = 1, material = CeilingMaterialTier.LeadSheeting, thicknessMeters = 1.0f, currentDurability = 100f }
                }
            };

            session.RestoreSkyArmorSave(state);

            string status = session.SkyArmorStatusLine();
            Assert.StartsWith("Sky armor: 2 cells", status);
            Assert.Contains("avg attenuation", status);

            // Restoring empty state returns to unplated
            session.RestoreSkyArmorSave(new SkyArmorSaveState());
            Assert.Equal("Sky armor: no cells plated", session.SkyArmorStatusLine());
        }

        [Fact]
        public void RestoreSkyArmorSave_AffectsSkyArmorStatusLine_AvgAttenuationCalculation()
        {
            var session = new TestWorldHostSession();
            // Dirt 1.0m durability 100 -> attenuation = 0.60
            // Lead 1.0m durability 100 -> attenuation = 0.05
            // Average = (0.60 + 0.05) / 2 = 0.325
            var state = new SkyArmorSaveState
            {
                cells = new List<CeilingCellArmor>
                {
                    new CeilingCellArmor { gridX = 0, material = CeilingMaterialTier.Dirt, thicknessMeters = 1.0f, currentDurability = 100f },
                    new CeilingCellArmor { gridX = 1, material = CeilingMaterialTier.LeadSheeting, thicknessMeters = 1.0f, currentDurability = 100f }
                }
            };

            session.RestoreSkyArmorSave(state);

            string status = session.SkyArmorStatusLine();
            Assert.Contains("0.325", status);
        }

        [Fact]
        public void RestoreSkyArmorSave_RoundTripFidelity_CaptureRestoreCaptureAreIdentical()
        {
            var session1 = new TestWorldHostSession();
            session1.SetSkyArmorDemo(0, "wood", 1.2f);
            session1.SetSkyArmorDemo(1, "concrete", 2.4f);
            session1.SetSkyArmorDemo(2, "tungsten", 3.6f);

            var save1 = session1.CaptureSkyArmorSave();

            var session2 = new TestWorldHostSession();
            session2.RestoreSkyArmorSave(save1);
            var save2 = session2.CaptureSkyArmorSave();

            Assert.Equal(save1.cells.Count, save2.cells.Count);
            for (int i = 0; i < save1.cells.Count; i++)
            {
                Assert.Equal(save1.cells[i].gridX, save2.cells[i].gridX);
                Assert.Equal(save1.cells[i].material, save2.cells[i].material);
                Assert.Equal(save1.cells[i].thicknessMeters, save2.cells[i].thicknessMeters);
                Assert.Equal(save1.cells[i].currentDurability, save2.cells[i].currentDurability);
            }
        }

        [Fact]
        public void RestoreSkyArmorSave_IsIdempotent_SuccessiveRestoresProduceIdenticalState()
        {
            var session = new TestWorldHostSession();
            var state = new SkyArmorSaveState
            {
                cells = new List<CeilingCellArmor>
                {
                    new CeilingCellArmor { gridX = 7, material = CeilingMaterialTier.ReinforcedConcrete, thicknessMeters = 2.0f, currentDurability = 85f }
                }
            };

            session.RestoreSkyArmorSave(state);
            var pass1 = session.CaptureSkyArmorSave();

            session.RestoreSkyArmorSave(state);
            var pass2 = session.CaptureSkyArmorSave();

            Assert.Single(pass1.cells);
            Assert.Single(pass2.cells);
            Assert.Equal(pass1.cells[0].gridX, pass2.cells[0].gridX);
            Assert.Equal(pass1.cells[0].material, pass2.cells[0].material);
            Assert.Equal(pass1.cells[0].thicknessMeters, pass2.cells[0].thicknessMeters);
            Assert.Equal(pass1.cells[0].currentDurability, pass2.cells[0].currentDurability);
        }

        [Fact]
        public void RestoreSkyArmorSave_PostRestoreDurabilityDegradationAndRepair()
        {
            var session = new TestWorldHostSession();
            var state = new SkyArmorSaveState
            {
                cells = new List<CeilingCellArmor>
                {
                    new CeilingCellArmor { gridX = 4, material = CeilingMaterialTier.ReinforcedConcrete, thicknessMeters = 1.0f, currentDurability = 40f }
                }
            };

            session.RestoreSkyArmorSave(state);

            float initialAtt = session.SkyArmor.GetAttenuationFactor(4);

            // Repair by 30
            session.SkyArmor.RepairCell(4, 30f);
            var cell = session.SkyArmor.GetCell(4);
            Assert.NotNull(cell);
            Assert.Equal(70f, cell.currentDurability);

            float repairedAtt = session.SkyArmor.GetAttenuationFactor(4);
            // Higher durability improves attenuation factor (lower bleed)
            Assert.True(repairedAtt < initialAtt);

            // Over-repair caps at 100
            session.SkyArmor.RepairCell(4, 100f);
            Assert.Equal(100f, cell.currentDurability);
        }

        [Fact]
        public void RestoreSkyArmorSave_DoesNotMutateOriginalSaveStateObject()
        {
            var originalCell = new CeilingCellArmor
            {
                gridX = 1,
                material = CeilingMaterialTier.LeadSheeting,
                thicknessMeters = 1.0f,
                currentDurability = 100f
            };
            var state = new SkyArmorSaveState
            {
                cells = new List<CeilingCellArmor> { originalCell }
            };

            var session = new TestWorldHostSession();
            session.RestoreSkyArmorSave(state);

            // Mutate cell in session via impact
            session.ImpactDemo(1, 20f);

            // Verify originalCell durability remains intact (deep copied during restore)
            Assert.Equal(100f, originalCell.currentDurability);
        }
    }
}
