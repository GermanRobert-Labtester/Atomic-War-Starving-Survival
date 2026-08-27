using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    /// <summary>
    /// Mirror of a per-store envelope DTO (e.g. WeatherHostSave) used to pin
    /// byte-identity between the generic SaveStore<T> envelope and the
    /// hand-rolled per-store pattern it replaces.
    /// </summary>
    public class StoreServiceMirrorEnvelope
    {
        public StoreServiceState State;
        public string Checksum = string.Empty;
    }

    public class StoreServiceState
    {
        public int Day;
        public string Name = string.Empty;
        public float Dose;
        public List<string>? Notes;
    }

    public class SaveStoreServiceTests : IDisposable
    {
        private readonly string _tempDir;
        private readonly CapturingLog _log = new CapturingLog();

        public SaveStoreServiceTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "AshfallSaveStoreServiceTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                try { Directory.Delete(_tempDir, true); } catch { }
            }
        }

        private SaveStore<StoreServiceState> NewStore(string fileName, bool createBackup = false)
        {
            string baseDir = _tempDir;
            return new SaveStore<StoreServiceState>(
                fileName, new FileSystemIO(), new SystemTextJsonSerializer(), _log,
                () => baseDir, "TestStore", createBackup);
        }

        private static StoreServiceState Sample(int day = 12) => new StoreServiceState
        {
            Day = day,
            Name = "Bunker Gamma",
            Dose = 0.25f,
            Notes = new List<string> { "filter worn", "door sealed" }
        };

        [Fact]
        public void RoundTrip_SaveThenLoad_ReturnsEqualState()
        {
            var store = NewStore("round_trip.json");
            Assert.True(store.TrySave(Sample()));
            Assert.True(store.Exists());

            StoreServiceState? loaded = store.TryLoad();
            Assert.NotNull(loaded);
            Assert.Equal(12, loaded!.Day);
            Assert.Equal("Bunker Gamma", loaded.Name);
            Assert.Equal(0.25f, loaded.Dose);
            Assert.NotNull(loaded.Notes);
            Assert.Equal(2, loaded.Notes!.Count);
        }

        [Fact]
        public void Envelope_IsByteIdenticalToPerStorePattern()
        {
            var store = NewStore("byte_identity.json");
            var state = Sample(day: 33);
            Assert.True(store.TrySave(state));

            string disk = File.ReadAllText(store.SavePath);

            // Hand-build the envelope exactly the way the per-store
            // boilerplate did: DTO with State + Checksum, checksum computed
            // over the envelope, serialized with the shared serializer.
            var mirror = new StoreServiceMirrorEnvelope { State = state };
            mirror.Checksum = SaveChecksum.Compute(mirror);
            string expected = new SystemTextJsonSerializer().Serialize(mirror);

            Assert.Equal(expected, disk);
            Assert.StartsWith("{\"State\":", disk, StringComparison.Ordinal);
            Assert.Contains("\"Checksum\":\"", disk, StringComparison.Ordinal);
        }

        [Fact]
        public void TamperedState_IsRejectedWithNullResult()
        {
            var store = NewStore("tampered.json");
            Assert.True(store.TrySave(Sample()));

            string raw = File.ReadAllText(store.SavePath);
            File.WriteAllText(store.SavePath, raw.Replace("Bunker Gamma", "Bunker EVIL"));

            Assert.Null(store.TryLoad());
            Assert.Contains(_log.Messages, m => m.Contains("load failed", StringComparison.Ordinal));
        }

        [Fact]
        public void NewFormatEnvelopeWithEmptyChecksum_IsRejected()
        {
            var store = NewStore("empty_checksum.json");
            File.WriteAllText(
                store.SavePath,
                "{\"State\":{\"Day\":5,\"Name\":\"X\",\"Dose\":0},\"Checksum\":\"\"}");
            Assert.Null(store.TryLoad());
        }

        [Fact]
        public void LegacyBareState_StillLoads()
        {
            var store = NewStore("legacy.json");
            File.WriteAllText(
                store.SavePath,
                "{\"Day\":7,\"Name\":\"Legacy Bunker\",\"Dose\":0.5}");
            StoreServiceState? loaded = store.TryLoad();
            Assert.NotNull(loaded);
            Assert.Equal(7, loaded!.Day);
            Assert.Equal("Legacy Bunker", loaded.Name);
        }

        [Fact]
        public void MissingOrEmptyFile_ReturnsNullSilently()
        {
            var store = NewStore("missing.json");
            _log.Messages.Clear();
            Assert.Null(store.TryLoad());
            Assert.Empty(_log.Messages);

            File.WriteAllText(store.SavePath, "   ");
            Assert.Null(store.TryLoad());
            Assert.Empty(_log.Messages);
        }

        [Fact]
        public void NullState_SaveReturnsFalse()
        {
            var store = NewStore("null_state.json");
            Assert.False(store.TrySave(null!));
            Assert.False(store.Exists());
        }

        [Fact]
        public void AtomicWrite_LeavesNoTempFileAndBacksUpOnlyWhenEnabled()
        {
            var plain = NewStore("plain.json");
            Assert.True(plain.TrySave(Sample(day: 1)));
            Assert.True(plain.TrySave(Sample(day: 2)));
            Assert.False(File.Exists(plain.SavePath + ".tmp"));
            Assert.False(File.Exists(plain.SavePath + ".bak"));
            Assert.Equal(2, plain.TryLoad()!.Day);

            var backed = NewStore("backed.json", createBackup: true);
            Assert.True(backed.TrySave(Sample(day: 1)));
            Assert.True(backed.TrySave(Sample(day: 2)));
            Assert.False(File.Exists(backed.SavePath + ".tmp"));
            Assert.True(File.Exists(backed.BackupPath));

            // The .bak holds the previous (v1) envelope.
            var (bakOk, bakState, _) = SaveEnvelopeHelper.RestoreEnvelope<StoreServiceState>(
                File.ReadAllText(backed.BackupPath), new SystemTextJsonSerializer());
            Assert.True(bakOk);
            Assert.Equal(1, bakState!.Day);
            Assert.Equal(2, backed.TryLoad()!.Day);
        }

        [Fact]
        public void PathOverride_IsHonouredForSaveAndLoad()
        {
            var store = NewStore("default.json");
            string overridePath = Path.Combine(_tempDir, "override", "explicit.json");

            Assert.True(store.TrySave(Sample(day: 77), overridePath));
            Assert.True(File.Exists(overridePath));
            Assert.False(store.Exists(), "default path must stay untouched by an overridden save");

            StoreServiceState? loaded = store.TryLoad(overridePath);
            Assert.NotNull(loaded);
            Assert.Equal(77, loaded!.Day);
        }

        [Fact]
        public void BaseDirProvider_IsReEvaluatedPerCall()
        {
            string baseDir = Path.Combine(_tempDir, "slotA");
            var store = new SaveStore<StoreServiceState>(
                "slotswitch.json", new FileSystemIO(), new SystemTextJsonSerializer(), _log,
                () => baseDir, "TestStore");
            Directory.CreateDirectory(baseDir);

            Assert.True(store.TrySave(Sample(day: 5)));
            Assert.Equal(Path.Combine(baseDir, "slotswitch.json"), store.SavePath);

            baseDir = Path.Combine(_tempDir, "slotB");
            Directory.CreateDirectory(baseDir);
            Assert.Equal(Path.Combine(baseDir, "slotswitch.json"), store.SavePath);
            Assert.False(store.Exists());
            Assert.Null(store.TryLoad());

            Assert.True(store.TrySave(Sample(day: 6)));
            Assert.True(File.Exists(Path.Combine(_tempDir, "slotB", "slotswitch.json")));
        }

        [Fact]
        public void FromCodec_DelegatesEncodingAndDecoding()
        {
            var json = new SystemTextJsonSerializer();
            var store = SaveStore<StoreServiceState>.FromCodec(
                "codec.json", new FileSystemIO(), json, _log, () => _tempDir, "CodecStore",
                (state, serializer) => serializer.Serialize(state),
                (raw, serializer) => serializer.Deserialize<StoreServiceState>(raw));

            Assert.True(store.TrySave(Sample(day: 21)));

            // Codec flavor writes exactly what the codec produced — no
            // envelope wrapping on top of the codec's own format.
            string disk = File.ReadAllText(store.SavePath);
            Assert.Equal(json.Serialize(Sample(day: 21)), disk);

            StoreServiceState? loaded = store.TryLoad();
            Assert.NotNull(loaded);
            Assert.Equal(21, loaded!.Day);
        }

        [Fact]
        public void FromCodec_DecodeFailure_ReturnsNullWithoutThrowing()
        {
            var store = SaveStore<StoreServiceState>.FromCodec(
                "codec_fail.json", new FileSystemIO(), new SystemTextJsonSerializer(), _log, () => _tempDir, "CodecStore",
                (state, serializer) => serializer.Serialize(state),
                (_, _) => throw new InvalidOperationException("codec rejected the save"));

            Assert.True(store.TrySave(Sample()));
            Assert.Null(store.TryLoad());
            Assert.Contains(_log.Messages, m => m.Contains("load failed", StringComparison.Ordinal));
        }

        [Fact]
        public void CaptureBareAndRestoreBare_RoundTripWithoutEnvelope()
        {
            var store = NewStore("bare.json");
            string captured = store.CaptureBare(Sample(day: 44));
            Assert.DoesNotContain("Checksum", captured, StringComparison.Ordinal);

            StoreServiceState? restored = store.RestoreBare(captured);
            Assert.NotNull(restored);
            Assert.Equal(44, restored!.Day);
        }

        [Fact]
        public void CaptureEnvelopeAndRestoreEnvelope_RoundTripWithIntegrity()
        {
            var store = NewStore("envcapture.json");
            string captured = store.CaptureEnvelope(Sample(day: 55));
            Assert.Contains("Checksum", captured, StringComparison.Ordinal);

            StoreServiceState? restored = store.RestoreEnvelope(captured);
            Assert.NotNull(restored);
            Assert.Equal(55, restored!.Day);

            Assert.Null(store.RestoreEnvelope(captured.Replace("Bunker Gamma", "Forged")));
        }

        [Fact]
        public void LegacyBareStateFallback_CanBeDisabled()
        {
            string baseDir = _tempDir;
            var store = new SaveStore<StoreServiceState>(
                "strict.json", new FileSystemIO(), new SystemTextJsonSerializer(), _log,
                () => baseDir, "StrictStore", createBackup: false, allowLegacyBareState: false);

            File.WriteAllText(store.SavePath, "{\"Day\":9,\"Name\":\"PreEnvelope\",\"Dose\":1}");
            Assert.True(store.TryLoad() == null, "sections that dropped their pre-envelope format must not adopt bare-state files");

            // Envelope saves still load normally with the flag off.
            Assert.True(store.TrySave(Sample(day: 10)));
            Assert.Equal(10, store.TryLoad()!.Day);
        }

        [Fact]
        public void SchemaVersionedEnvelope_IsByteAndBehaviorCompatibleWithLegacyStores()
        {
            var json = new SystemTextJsonSerializer();

            string encoded = SchemaVersionedEnvelope<StoreServiceState>.Encode(Sample(day: 61), json);
            Assert.StartsWith("{\"SchemaVersion\":\"1.0\",\"State\":", encoded, StringComparison.Ordinal);
            Assert.Contains("\"Checksum\":\"", encoded, StringComparison.Ordinal);

            // The legacy stores stamped SaveChecksum over a property-only
            // envelope, which walks zero public fields — a constant. The
            // adapter must keep producing exactly that value.
            var mirror = new LegacyPropertyEnvelope { State = Sample(day: 61) };
            Assert.Equal(SaveChecksum.Compute(mirror), json.Deserialize<LegacyChecksumProbe>(encoded)!.Checksum);

            // Historical load semantics: checksum presence is checked but not
            // verified — a tampered payload still loads.
            string tampered = encoded.Replace("Bunker Gamma", "Forged");
            StoreServiceState? loaded = SchemaVersionedEnvelope<StoreServiceState>.Decode(tampered, json);
            Assert.NotNull(loaded);
            Assert.Equal("Forged", loaded!.Name);

            // Empty checksum in the legacy shape is still rejected...
            Assert.Throws<InvalidOperationException>(() =>
                SchemaVersionedEnvelope<StoreServiceState>.Decode(
                    "{\"SchemaVersion\":\"1.0\",\"State\":{\"Day\":1},\"Checksum\":\"\"}", json));

            // ...and bare-state files still parse through the fallback.
            StoreServiceState? bare = SchemaVersionedEnvelope<StoreServiceState>.Decode(
                "{\"Day\":7,\"Name\":\"Legacy\",\"Dose\":0}", json);
            Assert.NotNull(bare);
            Assert.Equal(7, bare!.Day);
        }

        private sealed class LegacyChecksumProbe
        {
            public string SchemaVersion { get; set; } = "1.0";
            public StoreServiceState State { get; set; } = null!;
            public string Checksum { get; set; } = string.Empty;
        }

        private sealed class LegacyPropertyEnvelope
        {
            public string SchemaVersion { get; set; } = "1.0";
            public StoreServiceState State { get; set; } = null!;
            public string Checksum { get; set; } = string.Empty;
        }

        private sealed class CapturingLog : ILog
        {
            public readonly List<string> Messages = new List<string>();

            public void Info(string message) => Messages.Add(message);
            public void Warn(string message) => Messages.Add(message);
            public void Error(string message) => Messages.Add(message);
        }
    }
}
