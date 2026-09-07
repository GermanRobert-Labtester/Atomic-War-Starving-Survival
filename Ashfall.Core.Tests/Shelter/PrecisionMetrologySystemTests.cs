// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Shelter
{
    public class PrecisionMetrologySystemTests
    {
        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 10 && dir != null; i++)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "metrology_standards_catalog.json");
                if (File.Exists(probe))
                    return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Directory.GetParent(dir)?.FullName;
            }

            string cwd = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (File.Exists(Path.Combine(cwd, "metrology_standards_catalog.json")))
                return cwd;

            throw new DirectoryNotFoundException(
                "Assets/StreamingAssets/Data/metrology_standards_catalog.json not found from " + AppContext.BaseDirectory);
        }

        private static MetrologyStandardsCatalog LoadCatalog()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return PrecisionMetrologyCatalogLoader.Load(FindDataDir(), files, json);
        }

        private static PrecisionMetrologySystem Create(
            int seed = 89,
            InventoryContainer? inventory = null,
            MetrologyStandardsCatalog? catalog = null)
        {
            var sys = new PrecisionMetrologySystem(new SeededRng(seed), inventory);
            sys.LoadCatalog(catalog ?? LoadCatalog());
            return sys;
        }

        private static InventoryContainer MakeInventory(params (string id, int qty)[] items)
        {
            var inv = new InventoryContainer { Capacity = 64, MaxWeight = 500f };
            foreach (var (id, qty) in items)
                Assert.True(inv.TryProduce(id, qty), $"failed stocking {id}");
            return inv;
        }

        [Fact]
        public void Catalog_Loads_WithSchemaAndNoDuplicateIds()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.schema_version >= 1);
            Assert.NotEmpty(catalog.grades);
            Assert.NotEmpty(catalog.standards);
            Assert.NotEmpty(catalog.consumers);
            PrecisionMetrologyCatalogLoader.Validate(catalog);
        }

        [Fact]
        public void Catalog_MissingFile_ReturnsEmptyIdleCatalog()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string tmp = Path.Combine(Path.GetTempPath(), "ashfall_metrology_missing_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tmp);
            try
            {
                var catalog = PrecisionMetrologyCatalogLoader.Load(tmp, files, json);
                Assert.Empty(catalog.grades);
                Assert.Empty(catalog.standards);
            }
            finally
            {
                Directory.Delete(tmp, recursive: true);
            }
        }

        [Fact]
        public void Catalog_DuplicateGradeId_Throws()
        {
            var catalog = new MetrologyStandardsCatalog
            {
                schema_version = 1,
                grades =
                {
                    new MetrologyGradeDef { grade_id = "field", tooling_calibration = 0.35f },
                    new MetrologyGradeDef { grade_id = "field", tooling_calibration = 0.40f }
                }
            };
            Assert.Throws<InvalidOperationException>(() => PrecisionMetrologyCatalogLoader.Validate(catalog));
        }

        [Fact]
        public void UnregisteredConsumer_ReceivesZeroBenefit()
        {
            var sys = Create();
            Assert.Equal(0f, sys.QueryToolingCalibration("unknown_machine"));
            Assert.Equal(PrecisionCalibrationGrade.Uncalibrated, sys.QueryGrade("unknown_machine"));
            Assert.False(sys.QueryCapability("unknown_machine").IsRegisteredConsumer);

            var result = sys.CalibrateInstrument("unknown_machine", "std_gauge_block_set", day: 10);
            Assert.NotEqual(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal("unregistered_consumer", result.FailureCode);
        }

        [Fact]
        public void Calibrate_RegisteredConsumer_RaisesGrade()
        {
            var inv = MakeInventory(("item_gauge_block_set", 1), ("machinist_caliper", 1));
            var sys = Create(inventory: inv);

            var before = sys.QueryToolingCalibration("workshop_precision");
            Assert.Equal(0f, before);

            var result = sys.CalibrateInstrument(
                "workshop_precision",
                "std_gauge_block_set",
                day: 12,
                roomId: "room_workshop_precision");

            Assert.True(result.IsSuccess, result.MessageKey);
            Assert.True(sys.QueryToolingCalibration("workshop_precision") > 0.5f);
            Assert.True(sys.QueryGrade("workshop_precision") >= PrecisionCalibrationGrade.Shop);
        }

        [Fact]
        public void Calibrate_WrongRoom_IsBlocked()
        {
            var inv = MakeInventory(("item_gauge_block_set", 1));
            var sys = Create(inventory: inv);
            var result = sys.CalibrateInstrument(
                "workshop_precision",
                "std_gauge_block_set",
                day: 3,
                roomId: "room_kitchen");
            Assert.False(result.IsSuccess);
            Assert.Equal("wrong_room", result.FailureCode);
        }

        [Fact]
        public void ProjectToWorkshop_UpdatesRegisteredRoomsOnly()
        {
            var inv = MakeInventory(("item_gauge_block_set", 1));
            var sys = Create(inventory: inv);
            Assert.True(sys.CalibrateInstrument(
                "workshop_precision",
                "std_gauge_block_set",
                day: 5,
                roomId: "room_workshop_precision").IsSuccess);

            var workshop = new ShelterWorkshopSystem(
                new InventoryContainer { Capacity = 32 },
                new SeededRng(1));

            // Unrelated room starts at default 1.0 from GetOrCreate — leave a sentinel.
            var unrelated = workshop.GetOrCreateMachineState("room_filtration");
            unrelated.Calibration = 0.42f;

            int projected = sys.ProjectToWorkshop(workshop);
            Assert.True(projected >= 1);

            var precision = workshop.GetOrCreateMachineState("room_workshop_precision");
            Assert.True(precision.Calibration > 0.5f);
            Assert.Equal(0.42f, workshop.GetOrCreateMachineState("room_filtration").Calibration);
        }

        [Fact]
        public void Certify_UnregisteredConsumer_Rejected()
        {
            var sys = Create();
            var result = sys.CertifyComponentLot(
                "not_a_consumer",
                "lot_a",
                PrecisionCalibrationGrade.Certified,
                day: 8);
            Assert.False(result.IsSuccess);
            Assert.Equal("unregistered_consumer", result.FailureCode);
            Assert.Empty(sys.State.certificates);
        }

        [Fact]
        public void Certify_WithoutLiveInstrumentGrade_IsBlocked()
        {
            var sys = Create();
            var result = sys.CertifyComponentLot(
                "workshop_precision",
                "lot_uncalibrated",
                PrecisionCalibrationGrade.Certified,
                day: 1);
            Assert.False(result.IsSuccess);
            Assert.Equal("instrument_grade_too_low", result.FailureCode);
            Assert.Empty(sys.State.certificates);
        }

        [Fact]
        public void Disturbance_DriftsCalibration_Deterministically()
        {
            var inv = MakeInventory(("item_gauge_block_set", 1));
            var a = Create(seed: 101, inventory: inv);
            var b = Create(seed: 101, inventory: MakeInventory(("item_gauge_block_set", 1)));

            Assert.True(a.CalibrateInstrument("ballistics_workbench", "std_gauge_block_set", 20,
                roomId: "room_workshop_precision").IsSuccess);
            Assert.True(b.CalibrateInstrument("ballistics_workbench", "std_gauge_block_set", 20,
                roomId: "room_workshop_precision").IsSuccess);

            float before = a.QueryToolingCalibration("ballistics_workbench");
            Assert.True(a.ApplyDisturbance(5.0f, day: 21, disturbanceKey: "quake_21").IsSuccess);
            Assert.True(b.ApplyDisturbance(5.0f, day: 21, disturbanceKey: "quake_21").IsSuccess);

            float afterA = a.QueryToolingCalibration("ballistics_workbench");
            float afterB = b.QueryToolingCalibration("ballistics_workbench");
            Assert.True(afterA < before);
            Assert.Equal(afterA, afterB);
        }

        [Fact]
        public void Disturbance_IdempotentWithinWindow()
        {
            var inv = MakeInventory(("item_gauge_block_set", 1));
            var sys = Create(inventory: inv);
            Assert.True(sys.CalibrateInstrument("workshop_precision", "std_gauge_block_set", 1,
                roomId: "room_workshop_precision").IsSuccess);

            Assert.True(sys.ApplyDisturbance(4.5f, 2, "quake_2").IsSuccess);
            float mid = sys.QueryToolingCalibration("workshop_precision");
            Assert.True(sys.ApplyDisturbance(4.5f, 2, "quake_2").IsSuccess);
            Assert.Equal(mid, sys.QueryToolingCalibration("workshop_precision"));
        }

        [Fact]
        public void CaptureRestore_RoundTripPreservesInstruments()
        {
            var inv = MakeInventory(("item_gauge_block_set", 1), ("item_optical_flat", 1));
            var sys = Create(inventory: inv);
            Assert.True(sys.CalibrateInstrument("workshop_precision", "std_gauge_block_set", 9,
                roomId: "room_workshop_precision").IsSuccess);
            Assert.True(sys.CertifyComponentLot(
                "workshop_precision", "lot_bearing_01", PrecisionCalibrationGrade.Shop, 9).IsSuccess);

            var captured = sys.CaptureState();
            var restored = Create(seed: 999);
            restored.RestoreState(captured);

            Assert.Equal(
                sys.QueryToolingCalibration("workshop_precision"),
                restored.QueryToolingCalibration("workshop_precision"));
            Assert.Equal(sys.QueryGrade("workshop_precision"), restored.QueryGrade("workshop_precision"));
            Assert.Single(restored.State.certificates);
            Assert.Equal("lot_bearing_01", restored.State.certificates[0].componentLotId);
        }

        [Fact]
        public void OldSave_MissingInstruments_DefaultsSafely()
        {
            var sys = Create();
            sys.RestoreState(new PrecisionMetrologyState { schemaVersion = 1, currentDay = 4 });
            Assert.Equal(0f, sys.QueryToolingCalibration("workshop_precision"));
            Assert.Equal(PrecisionCalibrationGrade.Uncalibrated, sys.QueryGrade("ballistics_workbench"));
        }

        [Fact]
        public void BallisticsConsumer_ToolingFloat_IsLiveNotHardcoded()
        {
            var inv = MakeInventory(("machinist_caliper", 1), ("item_gauge_block_set", 1));
            var sys = Create(inventory: inv);

            // Field/shop via caliper (max shop); consumer room defaults to room_workshop.
            Assert.True(sys.CalibrateInstrument("ballistics_workbench", "std_machinist_caliper", 1).IsSuccess);
            float fieldish = sys.QueryToolingCalibration("ballistics_workbench");
            Assert.InRange(fieldish, 0.30f, 0.60f);
            Assert.NotEqual(0.75f, fieldish);

            Assert.True(sys.CalibrateInstrument("ballistics_workbench", "std_gauge_block_set", 2,
                roomId: "room_workshop_precision").IsSuccess);
            float certifiedish = sys.QueryToolingCalibration("ballistics_workbench");
            Assert.True(certifiedish > fieldish);
            Assert.InRange(certifiedish, 0.70f, 1.0f);
        }
    }
}
