// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Typed calibration grade. Ordinal order matters: higher grades subsume lower ones.
    /// Unregistered consumers receive <see cref="Uncalibrated"/> (zero benefit).
    /// </summary>
    public enum PrecisionCalibrationGrade
    {
        Uncalibrated = 0,
        Field = 1,
        Shop = 2,
        Reference = 3,
        Certified = 4
    }

    [Serializable]
    public sealed class MetrologyGradeDef
    {
        public string grade_id = string.Empty;
        public string display_name = string.Empty;
        public int ordinal;
        public float tooling_calibration;
        public float drift_per_day;
    }

    [Serializable]
    public sealed class MetrologyStandardDef
    {
        public string standard_id = string.Empty;
        public string display_name = string.Empty;
        public string item_id = string.Empty;
        public string max_grade = "shop";
        public List<string> required_room_ids = new List<string>();
        public int calibration_labor_ticks = 30;
        public bool bootstrap;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class MetrologyConsumerDef
    {
        public string consumer_id = string.Empty;
        public string room_id = string.Empty;
        public string min_grade = "field";
        public bool benefits_from_metrology = true;
    }

    [Serializable]
    public sealed class MetrologyDisturbanceDef
    {
        public float quake_drift_per_magnitude = 0.04f;
        public float min_magnitude = 3.0f;
        public int idempotent_key_window_days = 1;
    }

    [Serializable]
    public sealed class MetrologyStandardsCatalog
    {
        public int schema_version = 1;
        public List<MetrologyGradeDef> grades = new List<MetrologyGradeDef>();
        public List<MetrologyStandardDef> standards = new List<MetrologyStandardDef>();
        public List<MetrologyConsumerDef> consumers = new List<MetrologyConsumerDef>();
        public MetrologyDisturbanceDef disturbance = new MetrologyDisturbanceDef();
    }

    [Serializable]
    public sealed class MetrologyInstrumentState
    {
        public string consumerId = string.Empty;
        public string roomId = string.Empty;
        public PrecisionCalibrationGrade grade = PrecisionCalibrationGrade.Uncalibrated;
        public float toolingCalibration;
        public string lastStandardId = string.Empty;
        public int lastCalibratedDay = -1;
        public int lastDisturbanceDay = -1;
        public string lastDisturbanceKey = string.Empty;
    }

    [Serializable]
    public sealed class MetrologyCertificateState
    {
        public string certificateId = string.Empty;
        public string componentLotId = string.Empty;
        public string consumerId = string.Empty;
        public PrecisionCalibrationGrade grade = PrecisionCalibrationGrade.Uncalibrated;
        public int issuedDay = -1;
        public bool revoked;
    }

    [Serializable]
    public sealed class PrecisionMetrologyState
    {
        public string systemId = PrecisionMetrologySystem.SystemId;
        public int schemaVersion = 1;
        public int currentDay;
        public List<MetrologyInstrumentState> instruments = new List<MetrologyInstrumentState>();
        public List<MetrologyCertificateState> certificates = new List<MetrologyCertificateState>();
    }

    /// <summary>
    /// Pure projection of a calibration grade into bounded consumer bonuses.
    /// Unregistered systems must not invent bunker-wide multipliers from this type.
    /// </summary>
    public sealed class PrecisionCalibrationCapability
    {
        public PrecisionCalibrationGrade Grade { get; set; } = PrecisionCalibrationGrade.Uncalibrated;
        public float ToolingCalibration { get; set; }
        public bool IsRegisteredConsumer { get; set; }

        public static PrecisionCalibrationCapability None => new PrecisionCalibrationCapability();

        public static PrecisionCalibrationCapability FromGrade(
            PrecisionCalibrationGrade grade,
            float toolingCalibration,
            bool registered)
        {
            return new PrecisionCalibrationCapability
            {
                Grade = grade,
                ToolingCalibration = Math.Clamp(toolingCalibration, 0f, 1f),
                IsRegisteredConsumer = registered
            };
        }
    }

    /// <summary>
    /// Plan B89 — precision metrology authority.
    /// Owns grades, certificates, and disturbance drift. Projects into workshop
    /// Calibration and registered consumers only. No global breakdown magic.
    /// </summary>
    public sealed class PrecisionMetrologySystem
    {
        public const string SystemId = "precision_metrology";

        private readonly InventoryContainer? _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private MetrologyStandardsCatalog _catalog = new MetrologyStandardsCatalog();
        private PrecisionMetrologyState _state = new PrecisionMetrologyState();
        private readonly Dictionary<string, MetrologyGradeDef> _gradesById =
            new Dictionary<string, MetrologyGradeDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, MetrologyStandardDef> _standardsById =
            new Dictionary<string, MetrologyStandardDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, MetrologyConsumerDef> _consumersById =
            new Dictionary<string, MetrologyConsumerDef>(StringComparer.Ordinal);

        public event Action? OnStateChanged;
        public event Action<MetrologyInstrumentState>? OnInstrumentCalibrated;
        public event Action<MetrologyInstrumentState, float>? OnInstrumentDisturbed;
        public event Action<MetrologyCertificateState>? OnCertificateIssued;

        public PrecisionMetrologyState State => _state;
        public MetrologyStandardsCatalog Catalog => _catalog;
        public IReadOnlyDictionary<string, MetrologyConsumerDef> Consumers => _consumersById;

        public PrecisionMetrologySystem(
            ISeededRng rng,
            InventoryContainer? inventory = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory;
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(MetrologyStandardsCatalog catalog)
        {
            _catalog = catalog ?? new MetrologyStandardsCatalog();
            RebuildIndexes();
            EnsureRegisteredInstruments();
        }

        public void SetDay(int day) => _state.currentDay = Math.Max(0, day);

        /// <summary>
        /// Query the live tooling calibration float for a registered consumer.
        /// Unregistered consumers always return 0 (zero benefit).
        /// </summary>
        public float QueryToolingCalibration(string consumerId)
        {
            if (!TryGetRegisteredInstrument(consumerId, out var instrument))
                return 0f;
            return Math.Clamp(instrument.toolingCalibration, 0f, 1f);
        }

        public PrecisionCalibrationGrade QueryGrade(string consumerId)
        {
            if (!TryGetRegisteredInstrument(consumerId, out var instrument))
                return PrecisionCalibrationGrade.Uncalibrated;
            return instrument.grade;
        }

        public PrecisionCalibrationCapability QueryCapability(string consumerId)
        {
            if (!TryGetRegisteredInstrument(consumerId, out var instrument))
                return PrecisionCalibrationCapability.None;
            return PrecisionCalibrationCapability.FromGrade(
                instrument.grade, instrument.toolingCalibration, registered: true);
        }

        public bool IsRegisteredConsumer(string consumerId)
            => !string.IsNullOrEmpty(consumerId) && _consumersById.ContainsKey(consumerId);

        /// <summary>
        /// Calibrate a registered consumer using an authored standard item.
        /// Bootstrap standards may raise grade without inventory when inventory is null (tests).
        /// </summary>
        public ActionResult CalibrateInstrument(
            string consumerId,
            string standardId,
            int day,
            string? roomId = null)
        {
            if (string.IsNullOrWhiteSpace(consumerId))
                return ActionResult.Failed("missing_consumer", "metrology.missing_consumer");
            if (!_consumersById.TryGetValue(consumerId, out var consumer) || !consumer.benefits_from_metrology)
                return ActionResult.Failed("unregistered_consumer", "metrology.unregistered_consumer");
            if (!_standardsById.TryGetValue(standardId, out var standard))
                return ActionResult.Failed("unknown_standard", "metrology.unknown_standard");

            // Fail closed on required rooms: omit roomId → fall back to consumer.room_id.
            // Callers can no longer skip the gate by omitting roomId when the consumer
            // (or an explicit room) can be resolved.
            if (standard.required_room_ids != null && standard.required_room_ids.Count > 0)
            {
                string effectiveRoom = !string.IsNullOrEmpty(roomId) ? roomId : (consumer.room_id ?? string.Empty);
                if (string.IsNullOrEmpty(effectiveRoom))
                    return ActionResult.Blocked("missing_room", "metrology.missing_room");

                bool roomOk = false;
                for (int i = 0; i < standard.required_room_ids.Count; i++)
                {
                    if (string.Equals(standard.required_room_ids[i], effectiveRoom, StringComparison.Ordinal))
                    {
                        roomOk = true;
                        break;
                    }
                }
                if (!roomOk)
                    return ActionResult.Blocked("wrong_room", "metrology.wrong_room");
            }

            if (_inventory != null && !string.IsNullOrEmpty(standard.item_id))
            {
                if (_inventory.CountById(standard.item_id) < 1)
                    return ActionResult.Blocked("missing_standard", "metrology.missing_standard");
            }
            else if (_inventory != null && string.IsNullOrEmpty(standard.item_id))
            {
                return ActionResult.Failed("invalid_standard", "metrology.invalid_standard");
            }

            PrecisionCalibrationGrade targetGrade = ParseGrade(standard.max_grade);
            PrecisionCalibrationGrade minGrade = ParseGrade(consumer.min_grade);
            if (targetGrade < minGrade)
            {
                // Non-bootstrap standards that cannot meet the consumer minimum are blocked.
                // Bootstrap standards may leave a weaker field grade without inventing a higher one.
                if (!standard.bootstrap)
                    return ActionResult.Blocked("standard_too_weak", "metrology.standard_too_weak");
            }

            // Measurement noise nudges tooling slightly below catalog nominal when Certified.
            float nominal = ToolingForGrade(targetGrade);
            float noise = 0f;
            if (targetGrade >= PrecisionCalibrationGrade.Reference)
            {
                // Deterministic bounded noise from dedicated stream fork position.
                noise = (_rng.NextFloat() - 0.5f) * 0.02f;
            }

            var instrument = GetOrCreateInstrument(consumerId, consumer.room_id);
            instrument.grade = targetGrade;
            instrument.toolingCalibration = Math.Clamp(nominal + noise, 0f, 1f);
            instrument.lastStandardId = standardId;
            instrument.lastCalibratedDay = day;
            instrument.roomId = string.IsNullOrEmpty(roomId) ? consumer.room_id : roomId;
            _state.currentDay = day;

            OnInstrumentCalibrated?.Invoke(instrument);
            OnStateChanged?.Invoke();
            return ActionResult.Success("metrology.calibrated");
        }

        /// <summary>
        /// Issue a lot certificate for an explicitly registered consumer.
        /// Unregistered consumers are rejected (zero benefit path).
        /// </summary>
        public ActionResult CertifyComponentLot(
            string consumerId,
            string componentLotId,
            PrecisionCalibrationGrade grade,
            int day)
        {
            if (!_consumersById.TryGetValue(consumerId, out var consumer) || !consumer.benefits_from_metrology)
                return ActionResult.Failed("unregistered_consumer", "metrology.unregistered_consumer");
            if (string.IsNullOrWhiteSpace(componentLotId))
                return ActionResult.Failed("missing_lot", "metrology.missing_lot");

            PrecisionCalibrationGrade minGrade = ParseGrade(consumer.min_grade);
            if (grade < minGrade)
                return ActionResult.Blocked("grade_too_low", "metrology.grade_too_low");

            // Certificates require a live instrument grade at least as high as the requested cert.
            PrecisionCalibrationGrade liveGrade = QueryGrade(consumerId);
            if (liveGrade < grade)
                return ActionResult.Blocked("instrument_grade_too_low", "metrology.instrument_grade_too_low");

            string certId = $"cert_{consumerId}_{componentLotId}_{day}";
            for (int i = 0; i < _state.certificates.Count; i++)
            {
                if (string.Equals(_state.certificates[i].certificateId, certId, StringComparison.Ordinal))
                    return ActionResult.Blocked("already_certified", "metrology.already_certified");
            }

            var cert = new MetrologyCertificateState
            {
                certificateId = certId,
                componentLotId = componentLotId,
                consumerId = consumerId,
                grade = grade,
                issuedDay = day,
                revoked = false
            };
            _state.certificates.Add(cert);
            _state.currentDay = day;
            OnCertificateIssued?.Invoke(cert);
            OnStateChanged?.Invoke();
            return ActionResult.Success("metrology.certified");
        }

        /// <summary>
        /// Apply seismic / vibration disturbance. Idempotent per day+key window.
        /// </summary>
        public ActionResult ApplyDisturbance(float magnitude, int day, string disturbanceKey)
        {
            var rules = _catalog.disturbance ?? new MetrologyDisturbanceDef();
            if (magnitude < rules.min_magnitude)
                return ActionResult.Blocked("below_threshold", "metrology.disturbance_below_threshold");
            if (string.IsNullOrWhiteSpace(disturbanceKey))
                disturbanceKey = $"quake_{day}";

            float totalDrift = 0f;
            for (int i = 0; i < _state.instruments.Count; i++)
            {
                var instrument = _state.instruments[i];
                if (instrument.grade == PrecisionCalibrationGrade.Uncalibrated)
                    continue;

                if (instrument.lastDisturbanceDay >= 0 &&
                    day - instrument.lastDisturbanceDay < Math.Max(1, rules.idempotent_key_window_days) &&
                    string.Equals(instrument.lastDisturbanceKey, disturbanceKey, StringComparison.Ordinal))
                {
                    continue;
                }

                float drift = magnitude * rules.quake_drift_per_magnitude;
                // Extra deterministic jitter from calibration-drift stream.
                drift += _rng.NextFloat() * 0.01f;
                drift = Math.Clamp(drift, 0f, 0.5f);

                instrument.toolingCalibration = Math.Max(0f, instrument.toolingCalibration - drift);
                instrument.grade = GradeFromTooling(instrument.toolingCalibration);
                instrument.lastDisturbanceDay = day;
                instrument.lastDisturbanceKey = disturbanceKey;
                totalDrift += drift;
                OnInstrumentDisturbed?.Invoke(instrument, drift);
            }

            _state.currentDay = day;
            OnStateChanged?.Invoke();
            return ActionResult.Success("metrology.disturbed");
        }

        /// <summary>
        /// Passive per-day drift for calibrated instruments (maintenance cadence).
        /// </summary>
        public void TickDay(int day)
        {
            _state.currentDay = day;
            for (int i = 0; i < _state.instruments.Count; i++)
            {
                var instrument = _state.instruments[i];
                if (instrument.grade == PrecisionCalibrationGrade.Uncalibrated)
                    continue;

                float daily = DriftPerDay(instrument.grade);
                if (daily <= 0f) continue;
                instrument.toolingCalibration = Math.Max(0f, instrument.toolingCalibration - daily);
                instrument.grade = GradeFromTooling(instrument.toolingCalibration);
            }
            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Project registered room-bound consumers into workshop machine Calibration.
        /// Unregistered rooms are left untouched.
        /// </summary>
        public int ProjectToWorkshop(ShelterWorkshopSystem workshop)
        {
            if (workshop == null) return 0;
            int projected = 0;
            foreach (var kv in _consumersById)
            {
                var consumer = kv.Value;
                if (!consumer.benefits_from_metrology) continue;
                if (string.IsNullOrEmpty(consumer.room_id)) continue;

                var instrument = GetOrCreateInstrument(consumer.consumer_id, consumer.room_id);
                var machine = workshop.GetOrCreateMachineState(consumer.room_id);
                machine.Calibration = Math.Clamp(instrument.toolingCalibration, 0f, 1f);
                projected++;
            }
            return projected;
        }

        public PrecisionMetrologyState CaptureState() => CloneState(_state);

        public void RestoreState(PrecisionMetrologyState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            EnsureRegisteredInstruments();
        }

        private void RebuildIndexes()
        {
            _gradesById.Clear();
            _standardsById.Clear();
            _consumersById.Clear();

            if (_catalog.grades != null)
            {
                for (int i = 0; i < _catalog.grades.Count; i++)
                {
                    var g = _catalog.grades[i];
                    if (string.IsNullOrEmpty(g.grade_id)) continue;
                    _gradesById[g.grade_id] = g;
                }
            }

            if (_catalog.standards != null)
            {
                for (int i = 0; i < _catalog.standards.Count; i++)
                {
                    var s = _catalog.standards[i];
                    if (string.IsNullOrEmpty(s.standard_id)) continue;
                    _standardsById[s.standard_id] = s;
                }
            }

            if (_catalog.consumers != null)
            {
                for (int i = 0; i < _catalog.consumers.Count; i++)
                {
                    var c = _catalog.consumers[i];
                    if (string.IsNullOrEmpty(c.consumer_id)) continue;
                    _consumersById[c.consumer_id] = c;
                }
            }
        }

        private void EnsureRegisteredInstruments()
        {
            foreach (var kv in _consumersById)
                GetOrCreateInstrument(kv.Key, kv.Value.room_id);
        }

        private bool TryGetRegisteredInstrument(string consumerId, out MetrologyInstrumentState instrument)
        {
            instrument = null!;
            if (string.IsNullOrEmpty(consumerId) || !_consumersById.ContainsKey(consumerId))
                return false;
            instrument = GetOrCreateInstrument(consumerId, _consumersById[consumerId].room_id);
            return true;
        }

        private MetrologyInstrumentState GetOrCreateInstrument(string consumerId, string roomId)
        {
            for (int i = 0; i < _state.instruments.Count; i++)
            {
                if (string.Equals(_state.instruments[i].consumerId, consumerId, StringComparison.Ordinal))
                    return _state.instruments[i];
            }

            var created = new MetrologyInstrumentState
            {
                consumerId = consumerId,
                roomId = roomId ?? string.Empty,
                grade = PrecisionCalibrationGrade.Uncalibrated,
                toolingCalibration = 0f
            };
            _state.instruments.Add(created);
            return created;
        }

        private float ToolingForGrade(PrecisionCalibrationGrade grade)
        {
            string id = GradeToId(grade);
            if (_gradesById.TryGetValue(id, out var def))
                return Math.Clamp(def.tooling_calibration, 0f, 1f);
            return grade switch
            {
                PrecisionCalibrationGrade.Field => 0.35f,
                PrecisionCalibrationGrade.Shop => 0.55f,
                PrecisionCalibrationGrade.Reference => 0.75f,
                PrecisionCalibrationGrade.Certified => 0.95f,
                _ => 0f
            };
        }

        private float DriftPerDay(PrecisionCalibrationGrade grade)
        {
            string id = GradeToId(grade);
            if (_gradesById.TryGetValue(id, out var def))
                return Math.Max(0f, def.drift_per_day);
            return 0f;
        }

        private PrecisionCalibrationGrade GradeFromTooling(float tooling)
        {
            // Walk grades high→low against catalog tooling thresholds.
            PrecisionCalibrationGrade best = PrecisionCalibrationGrade.Uncalibrated;
            float bestTooling = -1f;
            foreach (var kv in _gradesById)
            {
                var def = kv.Value;
                if (def.tooling_calibration <= tooling + 0.0001f &&
                    def.tooling_calibration >= bestTooling)
                {
                    bestTooling = def.tooling_calibration;
                    best = ParseGrade(def.grade_id);
                }
            }
            if (bestTooling < 0f)
            {
                if (tooling >= 0.95f) return PrecisionCalibrationGrade.Certified;
                if (tooling >= 0.75f) return PrecisionCalibrationGrade.Reference;
                if (tooling >= 0.55f) return PrecisionCalibrationGrade.Shop;
                if (tooling >= 0.35f) return PrecisionCalibrationGrade.Field;
                return PrecisionCalibrationGrade.Uncalibrated;
            }
            return best;
        }

        public static PrecisionCalibrationGrade ParseGrade(string? gradeId)
        {
            if (string.IsNullOrWhiteSpace(gradeId))
                return PrecisionCalibrationGrade.Uncalibrated;
            if (string.Equals(gradeId, "field", StringComparison.OrdinalIgnoreCase))
                return PrecisionCalibrationGrade.Field;
            if (string.Equals(gradeId, "shop", StringComparison.OrdinalIgnoreCase))
                return PrecisionCalibrationGrade.Shop;
            if (string.Equals(gradeId, "reference", StringComparison.OrdinalIgnoreCase))
                return PrecisionCalibrationGrade.Reference;
            if (string.Equals(gradeId, "certified", StringComparison.OrdinalIgnoreCase))
                return PrecisionCalibrationGrade.Certified;
            if (Enum.TryParse(gradeId, ignoreCase: true, out PrecisionCalibrationGrade parsed))
                return parsed;
            return PrecisionCalibrationGrade.Uncalibrated;
        }

        public static string GradeToId(PrecisionCalibrationGrade grade) => grade switch
        {
            PrecisionCalibrationGrade.Field => "field",
            PrecisionCalibrationGrade.Shop => "shop",
            PrecisionCalibrationGrade.Reference => "reference",
            PrecisionCalibrationGrade.Certified => "certified",
            _ => "uncalibrated"
        };

        private static PrecisionMetrologyState CloneState(PrecisionMetrologyState src)
        {
            if (src == null) return new PrecisionMetrologyState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<PrecisionMetrologyState>(json) ?? new PrecisionMetrologyState();
        }
    }

    /// <summary>Loads <c>metrology_standards_catalog.json</c>.</summary>
    public static class PrecisionMetrologyCatalogLoader
    {
        public const string DefaultFileName = "metrology_standards_catalog.json";

        public static MetrologyStandardsCatalog Load(string dataDir, IFileIO files, IJsonSerializer json, ILog? log = null)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) throw new ArgumentException("dataDir required", nameof(dataDir));
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));

            string path = files.Combine(dataDir, DefaultFileName);
            if (!files.FileExists(path))
            {
                log?.Warn($"[PrecisionMetrology] catalog not found at {path}");
                return new MetrologyStandardsCatalog();
            }

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new MetrologyStandardsCatalog();

            var catalog = json.Deserialize<MetrologyStandardsCatalog>(raw)
                          ?? throw new InvalidOperationException("Failed to deserialize metrology_standards_catalog.json");

            Validate(catalog);
            return catalog;
        }

        public static void Validate(MetrologyStandardsCatalog catalog)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));
            if (catalog.schema_version < 1)
                throw new InvalidOperationException("metrology catalog: schema_version must be >= 1");

            var seenGrades = new HashSet<string>(StringComparer.Ordinal);
            if (catalog.grades != null)
            {
                for (int i = 0; i < catalog.grades.Count; i++)
                {
                    var g = catalog.grades[i];
                    if (string.IsNullOrEmpty(g.grade_id))
                        throw new InvalidOperationException("metrology catalog: grade_id required");
                    if (!seenGrades.Add(g.grade_id))
                        throw new InvalidOperationException($"metrology catalog: duplicate grade_id '{g.grade_id}'");
                    if (g.tooling_calibration < 0f || g.tooling_calibration > 1f)
                        throw new InvalidOperationException($"metrology catalog: tooling_calibration out of range for '{g.grade_id}'");
                    if (g.drift_per_day < 0f)
                        throw new InvalidOperationException($"metrology catalog: negative drift_per_day for '{g.grade_id}'");
                }
            }

            var seenStandards = new HashSet<string>(StringComparer.Ordinal);
            if (catalog.standards != null)
            {
                for (int i = 0; i < catalog.standards.Count; i++)
                {
                    var s = catalog.standards[i];
                    if (string.IsNullOrEmpty(s.standard_id))
                        throw new InvalidOperationException("metrology catalog: standard_id required");
                    if (!seenStandards.Add(s.standard_id))
                        throw new InvalidOperationException($"metrology catalog: duplicate standard_id '{s.standard_id}'");
                    if (string.IsNullOrEmpty(s.item_id))
                        throw new InvalidOperationException($"metrology catalog: item_id required for '{s.standard_id}'");
                }
            }

            var seenConsumers = new HashSet<string>(StringComparer.Ordinal);
            if (catalog.consumers != null)
            {
                for (int i = 0; i < catalog.consumers.Count; i++)
                {
                    var c = catalog.consumers[i];
                    if (string.IsNullOrEmpty(c.consumer_id))
                        throw new InvalidOperationException("metrology catalog: consumer_id required");
                    if (!seenConsumers.Add(c.consumer_id))
                        throw new InvalidOperationException($"metrology catalog: duplicate consumer_id '{c.consumer_id}'");
                }
            }
        }
    }
}
