// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Difficulty;
using Xunit;

namespace Ashfall.Core.Tests.Difficulty
{
    public sealed class DifficultyPresetCatalogTests
    {
        [Fact]
        public void ProductionCatalog_LoadsWithExpectedDefaultAndPresetCount()
        {
            DifficultyPresetCatalog catalog = DifficultyPresetCatalogLoader.Load(DataDirectory(), new FileSystemIO());

            Assert.Equal("difficulty_standard", catalog.default_preset_id);
            Assert.Equal(4, catalog.presets.Count);
            Assert.True(catalog.TryGet("difficulty_dirge", out DifficultyPreset dirge));
            Assert.Equal(1.75f, dirge.scalars.hunger_rate_mult);
        }

        [Fact]
        public void Validate_RejectsDuplicateIdsAndOutOfRangeScalars()
        {
            DifficultyPresetCatalog duplicate = ValidCatalog();
            duplicate.presets.Add(Clone(duplicate.presets[0]));
            Assert.False(duplicate.Validate(out string duplicateError));
            Assert.Contains("duplicate preset id", duplicateError);

            DifficultyPresetCatalog outOfRange = ValidCatalog();
            outOfRange.presets[0].scalars.radiation_gain_mult = 2.6f;
            Assert.False(outOfRange.Validate(out string scalarError));
            Assert.Contains("radiation_gain_mult", scalarError);
        }

        [Fact]
        public void Loader_FailsClosedForMalformedSchemaAndUnknownDefault()
        {
            Assert.Throws<InvalidOperationException>(() => DifficultyPresetCatalogLoader.LoadFromJson(
                "{\"schema_version\":2,\"presets\":[],\"default_preset_id\":\"difficulty_standard\"}"));

            DifficultyPresetCatalog catalog = ValidCatalog();
            catalog.default_preset_id = "difficulty_missing";
            Assert.False(catalog.Validate(out string error));
            Assert.Contains("default_preset_id", error);
        }

        [Fact]
        public void IntegrityValidation_ReportsMalformedDifficultyCatalog()
        {
            string directory = Path.Combine(Path.GetTempPath(), "ashfall_difficulty_catalog_test_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(directory);
                File.WriteAllText(Path.Combine(directory, DifficultyPresetCatalogLoader.FileName),
                    "{\"schema_version\":1,\"presets\":[],\"default_preset_id\":\"difficulty_missing\"}");
                var report = new CatalogIntegrityReport();

                CatalogIntegrityValidator.ValidateDifficultyPresetCatalog(directory, new FileSystemIO(), report);

                Assert.Single(report.Errors);
                Assert.Contains("at least one preset", report.Errors[0]);
            }
            finally
            {
                if (Directory.Exists(directory)) Directory.Delete(directory, recursive: true);
            }
        }

        private static DifficultyPresetCatalog ValidCatalog()
        {
            var catalog = new DifficultyPresetCatalog
            {
                default_preset_id = "difficulty_standard",
                presets = new List<DifficultyPreset>
                {
                    new DifficultyPreset
                    {
                        id = "difficulty_standard",
                        display_name = "STANDARD",
                        description = "Baseline.",
                        scalars = DifficultyScalars.Legacy()
                    }
                }
            };
            catalog.Index();
            return catalog;
        }

        private static DifficultyPreset Clone(DifficultyPreset source)
        {
            return new DifficultyPreset
            {
                id = source.id,
                display_name = source.display_name,
                description_key = source.description_key,
                description = source.description,
                scalars = source.scalars.Clone(),
                starting_bonus_item_ids = new List<string>(source.starting_bonus_item_ids)
            };
        }

        private static string DataDirectory()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new InvalidOperationException("StreamingAssets/Data directory was not found.");
        }
    }
}
