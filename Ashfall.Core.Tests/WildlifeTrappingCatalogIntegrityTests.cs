using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Tests
{
    public class WildlifeTrappingCatalogIntegrityTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            return string.Empty;
        }

        private static CatalogIntegrityReport ValidateScratch(Action<string> seed)
        {
            string scratch = Path.Combine(Path.GetTempPath(), "trapping_integrity_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                seed(scratch);
                return CatalogIntegrityValidator.Validate(scratch, new FileSystemIO());
            }
            finally
            {
                if (Directory.Exists(scratch))
                    Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void ShippedWildlifeTrappingCatalog_PassesIntegrityCheck()
        {
            string dir = DataDir();
            Assert.False(string.IsNullOrEmpty(dir), "StreamingAssets/Data must be found");

            var report = CatalogIntegrityValidator.Validate(dir, new FileSystemIO());
            Assert.True(report.Clean,
                "shipped catalogs must be valid:\n" + string.Join("\n", report.Errors));
        }

        [Theory]
        [InlineData(-0.1, false)]
        [InlineData(1.5, false)]
        [InlineData(0.0, true)]
        [InlineData(1.0, true)]
        [InlineData(0.25, true)]
        public void BycatchChance_NumericRangeValidation(double chance, bool shouldPass)
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "traps.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_test\",\"bycatchChance\":" +
                    chance.ToString(System.Globalization.CultureInfo.InvariantCulture) + "}]}");
            });

            if (shouldPass)
            {
                Assert.DoesNotContain(report.Errors, e => e.Contains("bycatchChance"));
            }
            else
            {
                Assert.Contains(report.Errors, e => e.Contains("bycatchChance") && e.Contains("trap 'trap_test'"));
            }
        }

        [Fact]
        public void BycatchChance_NonNumericString_Rejected()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "traps.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_test\",\"bycatchChance\":\"NaN\"}]}");
            });

            Assert.Contains(report.Errors, e => e.Contains("bycatchChance") && e.Contains("trap 'trap_test'"));
        }

        [Theory]
        [InlineData(-5.0, false)]
        [InlineData(0.0, true)]
        [InlineData(4.0, true)]
        [InlineData(20.0, true)]
        public void ContaminationDose_NonNegativeValidation(double dose, bool shouldPass)
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"prey_test\",\"contaminationDose\":" +
                    dose.ToString(System.Globalization.CultureInfo.InvariantCulture) + "}]}");
            });

            if (shouldPass)
            {
                Assert.DoesNotContain(report.Errors, e => e.Contains("contaminationDose"));
            }
            else
            {
                Assert.Contains(report.Errors, e => e.Contains("contaminationDose") && e.Contains("prey 'prey_test'"));
            }
        }

        [Fact]
        public void ContaminationDose_NonNumericString_Rejected()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"prey_test\",\"contaminationDose\":\"invalid\"}]}");
            });

            Assert.Contains(report.Errors, e => e.Contains("contaminationDose") && e.Contains("prey 'prey_test'"));
        }

        [Theory]
        [InlineData("PositiveInfinity")]
        [InlineData("NegativeInfinity")]
        [InlineData("Infinity")]
        public void ContaminationDose_Infinity_Rejected(string doseStr)
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"prey_test\",\"contaminationDose\":\"" +
                    doseStr + "\"}]}");
            });

            Assert.Contains(report.Errors, e => e.Contains("contaminationDose") && e.Contains("prey 'prey_test'"));
        }

        [Fact]
        public void ContaminationDose_ExactAcuteThreshold_PassesWithoutWarning()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"prey_test\",\"contaminationDose\":80.0}]}");
            });

            Assert.True(report.Clean, "Dose at exactly AcuteThreshold must pass: " + string.Join("\n", report.Errors));
            Assert.DoesNotContain(report.Warnings, w => w.Contains("contaminationDose"));
        }

        [Fact]
        public void ContaminationDose_ExceedsAcuteThreshold_PassesWithWarning()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"prey_test\",\"contaminationDose\":100.0}]}");
            });

            Assert.True(report.Clean, "Dose > AcuteThreshold must still pass as error-free: " + string.Join("\n", report.Errors));
            Assert.Contains(report.Warnings, w => w.Contains("contaminationDose") && w.Contains("exceeds acute threshold") && w.Contains("prey 'prey_test'"));
        }

        [Fact]
        public void DiseaseId_ValidReference_ResolvesCleanly()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "diseases.json"),
                    "{\"schema_version\":1,\"diseases\":[{\"id\":\"disease_typhoid_waterborne\"}]}");
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"rat\",\"diseaseId\":\"disease_typhoid_waterborne\"}]}");
            });

            Assert.True(report.Clean, "Valid disease reference must resolve: " + string.Join("\n", report.Errors));
        }

        [Fact]
        public void DiseaseId_EmptyString_IsIgnoredAndPasses()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"rabbit\",\"diseaseId\":\"\"}]}");
            });

            Assert.True(report.Clean, "Empty diseaseId must pass (runtime fallback): " + string.Join("\n", report.Errors));
        }

        [Fact]
        public void DiseaseId_NonexistentReference_FailsWithErrorContainingPreyAndDiseaseId()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"rat\",\"diseaseId\":\"disease_nonexistent\"}]}");
            });

            Assert.False(report.Clean);
            Assert.Contains(report.Errors, e =>
                e.Contains("disease_nonexistent") && e.Contains("rat"));
        }

        [Fact]
        public void BycatchSpecies_NonexistentSpecies_FailsWithErrorContainingTrapAndBadSpecies()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"rat\"},{\"speciesId\":\"hedgehog\"}]}");
                File.WriteAllText(Path.Combine(scratch, "traps.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_cage\",\"bycatchSpecies\":[" +
                    "{\"speciesId\":\"rat\",\"weight\":2.0}," +
                    "{\"speciesId\":\"species_fake\",\"weight\":1.0}]}]}");
            });

            Assert.False(report.Clean);
            Assert.Contains(report.Errors, e =>
                e.Contains("species_fake") && e.Contains("trap_cage"));
        }

        [Fact]
        public void BycatchSpecies_ValidSpeciesAndEmptyArray_Passes()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"rat\"}]}");
                File.WriteAllText(Path.Combine(scratch, "traps.json"),
                    "{\"schema_version\":1,\"traps\":[" +
                    "{\"trap_id\":\"trap_1\",\"bycatchSpecies\":[{\"speciesId\":\"rat\",\"weight\":1.0}]}," +
                    "{\"trap_id\":\"trap_2\",\"bycatchSpecies\":[]}]}");
            });

            Assert.True(report.Clean, "Valid species and empty bycatch list must pass: " + string.Join("\n", report.Errors));
        }

        [Fact]
        public void PreyDefinition_Validate_ValidatesCorrectly()
        {
            var valid = new PreyDefinition
            {
                speciesId = "rat",
                contaminationDose = 4.0f,
                diseaseRisk = 0.35f,
                contaminationRisk = 0.20f
            };
            Assert.True(valid.Validate(out string err), err);

            var negativeDose = new PreyDefinition
            {
                speciesId = "rat",
                contaminationDose = -1.0f
            };
            Assert.False(negativeDose.Validate(out _));

            var nanDose = new PreyDefinition
            {
                speciesId = "rat",
                contaminationDose = float.NaN
            };
            Assert.False(nanDose.Validate(out _));

            var invalidRisk = new PreyDefinition
            {
                speciesId = "rat",
                diseaseRisk = 1.5f
            };
            Assert.False(invalidRisk.Validate(out _));

            var emptySpecies = new PreyDefinition
            {
                speciesId = "",
                contaminationDose = 1.0f
            };
            Assert.False(emptySpecies.Validate(out _));
        }

        [Fact]
        public void TrapDefinition_Validate_ValidatesBycatchCandidates()
        {
            var trap = new TrapDefinition
            {
                trap_id = "trap_test",
                networkPenaltyPerTrap = 0.05f,
                bycatchChance = 0.25f,
                bycatchSpecies = new List<BycatchCandidate>
                {
                    new BycatchCandidate { speciesId = "rat", weight = 1.0f }
                }
            };
            Assert.True(trap.Validate(out string err), err);

            // Empty speciesId in bycatch candidate
            trap.bycatchSpecies.Add(new BycatchCandidate { speciesId = "", weight = 1.0f });
            Assert.False(trap.Validate(out _));

            // Invalid weight in bycatch candidate
            trap.bycatchSpecies[1].speciesId = "bad_species";
            trap.bycatchSpecies[1].weight = -1.0f;
            Assert.False(trap.Validate(out _));

            // NaN bycatch chance
            trap.bycatchSpecies.Clear();
            trap.bycatchChance = float.NaN;
            Assert.False(trap.Validate(out _));
        }

        [Fact]
        public void DiseaseId_OmittedFromPrey_PassesCleanly()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"rabbit\",\"displayName\":\"Ash Rabbit\"}]}");
            });

            Assert.True(report.Clean, "Omitted diseaseId must pass: " + string.Join("\n", report.Errors));
        }

        [Fact]
        public void DiseaseId_NullValue_PassesCleanly()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"rabbit\",\"diseaseId\":null}]}");
            });

            Assert.True(report.Clean, "Null diseaseId must pass: " + string.Join("\n", report.Errors));
        }

        [Fact]
        public void ContaminationDose_NegativeInfinity_NumericString_Rejected()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[{\"speciesId\":\"prey_test\",\"contaminationDose\":\"-Infinity\"}]}");
            });

            Assert.Contains(report.Errors, e => e.Contains("contaminationDose") && e.Contains("prey 'prey_test'"));
        }

        [Fact]
        public void ContaminationDose_MultipleInvalidEntries_StableDeterministicDiagnostics()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "prey.json"),
                    "{\"schema_version\":1,\"prey\":[" +
                    "{\"speciesId\":\"bad_prey_1\",\"contaminationDose\":-10.0}," +
                    "{\"speciesId\":\"bad_prey_2\",\"contaminationDose\":\"NaN\"}]}");
            });

            Assert.Equal(2, report.Errors.Count(e => e.Contains("contaminationDose")));
            Assert.Contains(report.Errors, e => e.Contains("negative contaminationDose") && e.Contains("prey 'bad_prey_1'"));
            Assert.Contains(report.Errors, e => e.Contains("non-finite contaminationDose") && e.Contains("prey 'bad_prey_2'"));
        }

        [Fact]
        public void TrapDefinition_CalculateRepairBill_ImprovisedWireSnare_CeilRounding()
        {
            var trap = new TrapDefinition
            {
                trap_id = "trap_improvised_wire",
                setupCosts = new List<TrapSetupCost>
                {
                    new TrapSetupCost { itemId = "copper_wire_10m_of_10m", amount = 1 }
                }
            };

            var bill = trap.CalculateRepairBill();
            Assert.Single(bill.Costs);
            var cost = bill.Costs[0];
            Assert.Equal("copper_wire_10m_of_10m", cost.ItemId);
            Assert.Equal(1, cost.Amount); // ceil(1 * 0.5) = 1
        }

        [Fact]
        public void TrapDefinition_CalculateRepairBill_BoxTrap_PreservesOrderAndAggregates()
        {
            var trap = new TrapDefinition
            {
                trap_id = "trap_box",
                setupCosts = new List<TrapSetupCost>
                {
                    new TrapSetupCost { itemId = "scrap_wood", amount = 2 },
                    new TrapSetupCost { itemId = "scrap_metal", amount = 1 },
                    new TrapSetupCost { itemId = "box_of_nails_10", amount = 1 }
                }
            };

            var bill = trap.CalculateRepairBill();
            Assert.Equal(3, bill.Costs.Count);
            Assert.Equal("scrap_wood", bill.Costs[0].ItemId);
            Assert.Equal(1, bill.Costs[0].Amount); // ceil(2 * 0.5) = 1
            Assert.Equal("scrap_metal", bill.Costs[1].ItemId);
            Assert.Equal(1, bill.Costs[1].Amount); // ceil(1 * 0.5) = 1
            Assert.Equal("box_of_nails_10", bill.Costs[2].ItemId);
            Assert.Equal(1, bill.Costs[2].Amount); // ceil(1 * 0.5) = 1
        }
    }
}
