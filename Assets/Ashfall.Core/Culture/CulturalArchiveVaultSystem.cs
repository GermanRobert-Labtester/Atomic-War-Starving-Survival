using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Catalogs;
using Ashfall.Core.Culture;
using Ashfall.Core.Institutions;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Culture
{
    // ---------------------------------------------------------------------
    // Persisted state
    // ---------------------------------------------------------------------

    /// <summary>Runtime state of one archival document (physical + knowledge).</summary>
    [Serializable]
    public sealed class ArchiveDocumentState
    {
        public string document_id = string.Empty;
        public int physical_degradation_permille;        // 0..1000
        public bool is_chemically_stabilized;
        public int transcription_permille;               // 0..1000
        public string active_scholar_id = string.Empty;
        public int microfiche_copy_count;
        public bool knowledge_preserved;                 // the permanent unlock (microfiche)
        public string status = "archived";               // archived | transcribing | transcribed | lost
    }

    [Serializable]
    public sealed class ArchiveProjectState
    {
        public string document_id = string.Empty;
        public string kind = string.Empty;               // restoration | transcription
        public string survivor_id = string.Empty;
        public int started_day = -1;
        public int last_progress_day = -1;
    }

    [Serializable]
    public sealed class ArchiveRecordingState
    {
        public string recording_id = string.Empty;
        public string category = string.Empty;           // music_performance | oral_history | survivor_testimony | radio_archive | commemorative
        public string operator_id = string.Empty;
        public int recorded_day = -1;
    }

    [Serializable]
    public sealed class ArchiveSalonState
    {
        public bool active;
        public string modifier_key = "salon_stress_resistance";
        public int start_day = -1;
        public int duration_days;
        public int cooldown_until_day = -1;
    }

    [Serializable]
    public sealed class ArchiveChronicleEntry
    {
        public string chronicle_id = string.Empty;
        public int campaign_day;
        public string event_type = string.Empty;
        public string summary_key = string.Empty;
        public List<string> participants = new();
        public string author_id = string.Empty;
        public string volume_id = string.Empty;
    }

    /// <summary>Aggregate save state for the cultural archives section.</summary>
    [Serializable]
    public sealed class CulturalArchiveVaultSave
    {
        public int schema_version = 1;
        public List<ArchiveDocumentState> documents = new();
        public List<ArchiveProjectState> active_projects = new();
        public List<ArchiveRecordingState> recordings = new();
        public List<ArchiveChronicleEntry> chronicle_entries = new();
        public ArchiveSalonState salon = new();
        public int next_chronicle_ordinal;
        public float degradation_remainder;              // deterministic fractional permille carry
    }

    // ---------------------------------------------------------------------
    // System
    // ---------------------------------------------------------------------

    /// <summary>
    /// Deep-vault cultural archives & legacy sound engraving (flagship Task 5).
    ///
    /// Owns: archive document state, transcription, restoration, microfiche
    /// knowledge preservation, acetate disc cutting, philosophical salons,
    /// chronicle records. Does NOT own: global inventory (atomic transactions
    /// only), playback morale (VinylMoraleSystem owns it — this system only
    /// creates the media definition and acquires it), codex knowledge
    /// (JournalSystem consumes the events), survivor identity.
    ///
    /// Determinism: no stochastic behavior — degradation is an authored
    /// formula over the injected humidity provider, so no RNG stream is
    /// consumed (plan §5.9).
    /// </summary>
    public sealed class CulturalArchiveVaultSystem
    {
        public const string SystemId = "cultural_archives";
        public const string InstitutionId = "institution_cultural_archive";

        // Authored balance constants (documented; not catalog-tunable in v1).
        public const int RestorationReliefPermille = 350;
        public const int LegibilityLimitPermille = 900;   // above this, pages cannot be worked
        public const int LostThresholdPermille = 1000;
        public const float BaseDailyDegradationPermille = 2f;
        public const int SalonDefaultDurationDays = 5;
        public const int SalonCooldownDays = 10;
        public const float SalonMoralePerDay = 2f;
        public const string CutDiscCostItemId = "acetate_blank_disc";

        private readonly Inventory.Inventory _inventory;
        private readonly ILog _log;
        private readonly Func<float>? _humidityPercentProvider;   // 0..100, authoritative climate
        private readonly IInstitutionAvailability? _availability;
        private readonly VinylMoraleSystem? _vinyl;

        private readonly Dictionary<string, CulturalArchiveTomeDefinition> _tomes = new(StringComparer.Ordinal);
        private CulturalArchiveVaultSave _state = new();

        public CulturalArchiveVaultSystem(
            Inventory.Inventory inventory,
            ILog? log = null,
            Func<float>? humidityPercentProvider = null,
            IInstitutionAvailability? availability = null,
            VinylMoraleSystem? vinyl = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _log = log ?? new ConsoleLog();
            _humidityPercentProvider = humidityPercentProvider;
            _availability = availability;
            _vinyl = vinyl;
        }

        // -----------------------------------------------------------------
        // Events (single-consumer wiring done by the host)
        // -----------------------------------------------------------------

        public event Action<string>? OnDocumentRestored;               // documentId
        public event Action<string>? OnMicroficheCreated;              // documentId
        public event Action<string>? OnTomeTranscribed;                // documentId
        public event Action<string>? OnDocumentLost;                   // documentId
        public event Action<string, VinylRecordDefinition>? OnArchiveRecordingCreated;
        public event Action<int>? OnSalonStarted;                      // day
        public event Action<int>? OnSalonEnded;                        // day
        public event Action<float>? OnSalonMoraleTick;                 // morale delta, once per day while active
        public event Action<ArchiveChronicleEntry>? OnChronicleEntryAdded;

        // -----------------------------------------------------------------
        // Catalog
        // -----------------------------------------------------------------

        public void LoadTomeCatalog(List<CulturalArchiveTomeDefinition> tomes)
        {
            if (tomes == null) return;
            _tomes.Clear();
            foreach (var t in tomes)
                if (!string.IsNullOrEmpty(t.tome_id))
                    _tomes[t.tome_id] = t;

            // First load seeds authoritative document state for each tome.
            foreach (var t in tomes)
            {
                if (string.IsNullOrEmpty(t.tome_id) || _state.documents.Any(d => d.document_id == t.tome_id))
                    continue;
                _state.documents.Add(new ArchiveDocumentState
                {
                    document_id = t.tome_id,
                    physical_degradation_permille = t.initial_degradation_permille,
                });
            }
        }

        public IReadOnlyList<ArchiveDocumentState> Documents => _state.documents.AsReadOnly();
        public IReadOnlyList<ArchiveRecordingState> Recordings => _state.recordings.AsReadOnly();
        public IReadOnlyList<ArchiveChronicleEntry> Chronicle => _state.chronicle_entries.AsReadOnly();
        public ArchiveSalonState Salon => _state.salon;
        public ArchiveDocumentState? GetDocument(string documentId) =>
            _state.documents.FirstOrDefault(d => d.document_id == documentId);

        // -----------------------------------------------------------------
        // Restoration
        // -----------------------------------------------------------------

        /// <summary>Atomically consumes restoration materials and improves one document's condition.</summary>
        public ActionResult TryRestoreDocument(string documentId)
        {
            var doc = GetDocument(documentId);
            if (doc == null)
                return ActionResult.Blocked("unknown_document", "culture.unknown_document");
            if (doc.physical_degradation_permille >= LegibilityLimitPermille)
                return ActionResult.Blocked("too_degraded", "culture.too_degraded");
            if (doc.physical_degradation_permille <= 0 && doc.is_chemically_stabilized)
                return ActionResult.Blocked("already_restored", "culture.already_restored");
            if (!_tomes.TryGetValue(documentId, out var tome))
                return ActionResult.Blocked("unknown_tome", "culture.unknown_tome");

            var bill = BuildBill(tome.restoration_costs);
            if (!_inventory.TryExecuteTransaction(bill))
                return ActionResult.Blocked("missing_inputs", "culture.missing_restoration_inputs");

            doc.physical_degradation_permille = Math.Max(0, doc.physical_degradation_permille - RestorationReliefPermille);
            doc.is_chemically_stabilized = true;
            _log.Info($"[Culture] restored '{documentId}' to {doc.physical_degradation_permille}\u2030 (stabilized)");
            OnDocumentRestored?.Invoke(documentId);
            return ActionResult.Success("culture.document_restored",
                new Dictionary<string, double> { { "degradation_permille", doc.physical_degradation_permille } });
        }

        // -----------------------------------------------------------------
        // Transcription
        // -----------------------------------------------------------------

        public ActionResult TryStartTranscription(string documentId, string scholarId)
        {
            var doc = GetDocument(documentId);
            if (doc == null)
                return ActionResult.Blocked("unknown_document", "culture.unknown_document");
            if (doc.physical_degradation_permille >= LegibilityLimitPermille)
                return ActionResult.Blocked("too_degraded", "culture.too_degraded");
            if (doc.status is "transcribing" or "transcribed")
                return ActionResult.Blocked("already_in_progress", "culture.transcription_in_progress");
            if (string.IsNullOrEmpty(scholarId))
                return ActionResult.Blocked("no_scholar", "culture.no_scholar");
            if (_availability != null && !_availability.TryClaim(scholarId, InstitutionId, "scholar"))
                return ActionResult.Blocked("scholar_unavailable", "culture.scholar_unavailable");

            doc.active_scholar_id = scholarId;
            doc.status = "transcribing";
            _state.active_projects.Add(new ArchiveProjectState
            {
                document_id = documentId,
                kind = "transcription",
                survivor_id = scholarId,
                started_day = _currentDay,
                last_progress_day = _currentDay,
            });
            _log.Info($"[Culture] transcription started: '{documentId}' by {scholarId}");
            return ActionResult.Success("culture.transcription_started");
        }

        // -----------------------------------------------------------------
        // Microfiche preservation
        // -----------------------------------------------------------------

        public ActionResult TryCreateMicroficheCopy(string documentId, string operatorId)
        {
            var doc = GetDocument(documentId);
            if (doc == null)
                return ActionResult.Blocked("unknown_document", "culture.unknown_document");
            if (doc.physical_degradation_permille >= LegibilityLimitPermille)
                return ActionResult.Blocked("too_degraded", "culture.too_degraded");
            if (doc.knowledge_preserved)
                return ActionResult.Blocked("already_preserved", "culture.already_preserved");
            if (!_tomes.TryGetValue(documentId, out var tome))
                return ActionResult.Blocked("unknown_tome", "culture.unknown_tome");

            var bill = BuildBill(tome.microfiche_costs);
            if (!_inventory.TryExecuteTransaction(bill))
                return ActionResult.Blocked("missing_inputs", "culture.missing_microfiche_inputs");

            doc.knowledge_preserved = true;
            doc.microfiche_copy_count++;
            _log.Info($"[Culture] microfiche created for '{documentId}' (knowledge preserved)");
            OnMicroficheCreated?.Invoke(documentId);
            return ActionResult.Success("culture.microfiche_created",
                new Dictionary<string, double> { { "copies", doc.microfiche_copy_count } });
        }

        // -----------------------------------------------------------------
        // Acetate disc cutting
        // -----------------------------------------------------------------

        public static readonly string[] LegalRecordingCategories =
        {
            "music_performance", "oral_history", "survivor_testimony", "radio_archive", "commemorative",
        };

        public ActionResult TryCutArchiveDisc(string recordingId, string category, string operatorId, int day)
        {
            if (string.IsNullOrEmpty(recordingId))
                return ActionResult.Blocked("invalid_id", "culture.invalid_recording_id");
            if (!LegalRecordingCategories.Contains(category))
                return ActionResult.Blocked("invalid_category", "culture.invalid_recording_category");
            if (_state.recordings.Any(r => r.recording_id == recordingId))
                return ActionResult.Blocked("duplicate_recording", "culture.duplicate_recording");

            var bill = new InventoryBill();
            bill.AddCost(CutDiscCostItemId, 1);
            if (!_inventory.TryExecuteTransaction(bill))
                return ActionResult.Blocked("missing_inputs", "culture.missing_disc_blanks");

            var recording = new ArchiveRecordingState
            {
                recording_id = recordingId,
                category = category,
                operator_id = operatorId ?? string.Empty,
                recorded_day = day,
            };
            _state.recordings.Add(recording);

            var recordDef = BuildRecordDefinition(recording);
            _vinyl?.AcquireRecord(recordingId);
            _log.Info($"[Culture] cut archive disc '{recordingId}' ({category})");
            OnArchiveRecordingCreated?.Invoke(recordingId, recordDef);
            return ActionResult.Success("culture.disc_cut",
                new Dictionary<string, double> { { "recordings", _state.recordings.Count } });
        }

        /// <summary>
        /// Authored media definition for a cut disc. The HOST merges this into
        /// the vinyl catalog (VinylMoraleSystem.LoadCatalog replaces the whole
        /// catalog, so this system never calls it directly).
        /// </summary>
        public static VinylRecordDefinition BuildRecordDefinition(ArchiveRecordingState recording)
        {
            float morale = recording.category switch
            {
                "music_performance" => 3.5f,
                "commemorative" => 2.5f,
                "oral_history" => 2f,
                "survivor_testimony" => 2f,
                _ => 1.5f,
            };
            float suppression = recording.category is "survivor_testimony" or "commemorative" ? 0.2f : 0f;
            return new VinylRecordDefinition
            {
                record_id = recording.recording_id,
                display_name = $"Archive Disc: {recording.category}",
                genre = recording.category,
                morale_daily_bonus = morale,
                flashback_suppression = suppression,
                description = "Cut in the shelter archive on a lacquer blank — one pass, no second takes.",
            };
        }

        // -----------------------------------------------------------------
        // Philosophical salons
        // -----------------------------------------------------------------

        public ActionResult TryStartSalon(int day)
        {
            if (_state.salon.active)
                return ActionResult.Blocked("salon_active", "culture.salon_active");
            if (day <= _state.salon.cooldown_until_day)
                return ActionResult.Blocked("salon_cooldown", "culture.salon_cooldown");

            _state.salon.active = true;
            _state.salon.start_day = day;
            _state.salon.duration_days = SalonDefaultDurationDays;
            _log.Info($"[Culture] philosophical salon convened on day {day}");
            OnSalonStarted?.Invoke(day);
            return ActionResult.Success("culture.salon_started");
        }

        // -----------------------------------------------------------------
        // Chronicles
        // -----------------------------------------------------------------

        /// <summary>
        /// Records one structured campaign milestone into the shelter
        /// chronicle. Consumes structured facts — never UI logs. Duplicate
        /// (event_type, campaign_day, summary_key) triples are rejected so a
        /// milestone emitted twice (e.g. across a save/load boundary) records
        /// exactly once.
        /// </summary>
        public ActionResult TryRecordChronicleEntry(
            string eventType, int campaignDay, string summaryKey,
            IReadOnlyList<string>? participants, string authorId = "", string? volumeId = null)
        {
            if (string.IsNullOrEmpty(eventType) || string.IsNullOrEmpty(summaryKey))
                return ActionResult.Blocked("invalid_milestone", "culture.invalid_milestone");

            bool duplicate = _state.chronicle_entries.Any(e =>
                e.event_type == eventType && e.campaign_day == campaignDay && e.summary_key == summaryKey);
            if (duplicate)
                return ActionResult.Blocked("duplicate_chronicle", "culture.duplicate_chronicle");

            int ordinal = _state.next_chronicle_ordinal++;
            string volume = string.IsNullOrEmpty(volumeId)
                ? $"volume_{ordinal / 12 + 1}"
                : volumeId;
            var entry = new ArchiveChronicleEntry
            {
                chronicle_id = $"chronicle_{campaignDay}_{ordinal}",
                campaign_day = campaignDay,
                event_type = eventType,
                summary_key = summaryKey,
                participants = participants?.ToList() ?? new List<string>(),
                author_id = authorId ?? string.Empty,
                volume_id = volume,
            };
            _state.chronicle_entries.Add(entry);
            OnChronicleEntryAdded?.Invoke(entry);
            return ActionResult.Success("culture.chronicle_recorded");
        }

        // -----------------------------------------------------------------
        // Daily tick
        // -----------------------------------------------------------------

        private int _currentDay;

        public void TickDay(int day)
        {
            _currentDay = day;
            float humidity = Math.Clamp(_humidityPercentProvider?.Invoke() ?? 0f, 0f, 100f);
            float humidityScale = 0.5f + humidity / 100f; // 0.5 dry .. 1.5 soaked

            // Transcription projects progress before degradation is applied.
            for (int i = _state.active_projects.Count - 1; i >= 0; i--)
            {
                var project = _state.active_projects[i];
                var doc = GetDocument(project.document_id);
                if (doc == null)
                {
                    _state.active_projects.RemoveAt(i);
                    continue;
                }
                if (!_tomes.TryGetValue(project.document_id, out var tome))
                    continue;

                int step = Math.Max(1, 1000 / tome.transcription_days);
                doc.transcription_permille = Math.Min(1000, doc.transcription_permille + step);
                project.last_progress_day = day;

                if (doc.transcription_permille >= 1000)
                {
                    doc.status = "transcribed";
                    doc.active_scholar_id = string.Empty;
                    _state.active_projects.RemoveAt(i);
                    _availability?.Release(project.survivor_id, InstitutionId, "scholar");
                    _log.Info($"[Culture] transcription complete: '{doc.document_id}'");
                    OnTomeTranscribed?.Invoke(doc.document_id);
                }
            }

            // Physical degradation (paper only — never erases knowledge state).
            float remainder = _state.degradation_remainder;
            foreach (var doc in _state.documents)
            {
                if (doc.physical_degradation_permille >= LostThresholdPermille)
                    continue;
                if (!_tomes.TryGetValue(doc.document_id, out var tome))
                    continue;

                float tierMult = tome.paper_brittleness_tier switch
                {
                    1 => 1.0f,
                    2 => 1.5f,
                    _ => 2.5f,
                };
                float storageScale = doc.is_chemically_stabilized ? 0.25f : 1.0f;
                float daily = BaseDailyDegradationPermille * tierMult * humidityScale * storageScale;

                remainder += daily;
                int whole = (int)remainder;
                if (whole > 0)
                {
                    remainder -= whole;
                    int before = doc.physical_degradation_permille;
                    doc.physical_degradation_permille = Math.Min(LostThresholdPermille, before + whole);
                    if (before < LostThresholdPermille && doc.physical_degradation_permille >= LostThresholdPermille)
                    {
                        doc.status = "lost";
                        if (doc.active_scholar_id.Length > 0)
                        {
                            _availability?.Release(doc.active_scholar_id, InstitutionId, "scholar");
                            doc.active_scholar_id = string.Empty;
                        }
                        _state.active_projects.RemoveAll(p => p.document_id == doc.document_id);
                        _log.Info($"[Culture] document lost to decay: '{doc.document_id}'");
                        OnDocumentLost?.Invoke(doc.document_id);
                    }
                }
            }
            _state.degradation_remainder = remainder;

            // Salon lifecycle: one shelter-wide modifier, explicit end + cooldown.
            if (_state.salon.active)
            {
                OnSalonMoraleTick?.Invoke(SalonMoralePerDay);
                if (day >= _state.salon.start_day + _state.salon.duration_days - 1)
                {
                    _state.salon.active = false;
                    _state.salon.cooldown_until_day = day + SalonCooldownDays;
                    _log.Info($"[Culture] salon ended on day {day}");
                    OnSalonEnded?.Invoke(day);
                }
            }
        }

        // -----------------------------------------------------------------
        // Save / restore
        // -----------------------------------------------------------------

        public CulturalArchiveVaultSave CaptureState() => Clone(_state);

        public void RestoreState(CulturalArchiveVaultSave? saved)
        {
            if (saved == null) return;
            _state = Clone(saved);
        }

        private static CulturalArchiveVaultSave Clone(CulturalArchiveVaultSave src)
        {
            var json = new SystemTextJsonSerializer();
            return json.Deserialize<CulturalArchiveVaultSave>(json.Serialize(src)) ?? new CulturalArchiveVaultSave();
        }

        private static InventoryBill BuildBill(List<InstitutionCatalogParse.CatalogCostEntry>? costs)
        {
            var bill = new InventoryBill();
            if (costs == null) return bill;
            foreach (var c in costs)
                bill.AddCost(c.item_id, c.amount);
            return bill;
        }
    }
}
