using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Journal;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class NarrativeDiscoveryManifestEntry
    {
        public string discovery_id = string.Empty;
        public string source_catalog = string.Empty;
        public string source_record_id = string.Empty;
        public string channel = string.Empty;
        public string producer_id = string.Empty;
        public int min_day = 1;
        public int weight = 1;
        public bool one_time = true;

        public string DiscoveryId => discovery_id;
        public string SourceCatalog => source_catalog;
        public string SourceRecordId => source_record_id;
        public string Channel => channel;
        public string ProducerId => producer_id;
        public int MinDay => min_day;
        public int Weight => weight;
        public bool OneTime => one_time;
    }

    [Serializable]
    public sealed class NarrativeDiscoveryManifestFile
    {
        public int schema_version = 1;
        public List<NarrativeDiscoveryManifestEntry> entries = new List<NarrativeDiscoveryManifestEntry>();
    }

    /// <summary>
    /// Canonical projected model of an activated narrative codex record.
    /// Heterogeneous source catalog records are adapted deterministically
    /// into this projection for Journal, Codex, and UI inspection surfaces.
    /// </summary>
    public sealed class NarrativeDiscoveredRecord
    {
        public string DiscoveryId { get; set; } = string.Empty;
        public string KnowledgeKey { get; set; } = string.Empty;
        public string SourceCatalog { get; set; } = string.Empty;
        public string SourceRecordId { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string ProducerId { get; set; } = string.Empty;
        public int MinDay { get; set; } = 1;

        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string BodyText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Pluggable adapter interface to project heterogeneous source schemas
    /// into the canonical NarrativeDiscoveredRecord without schema collapse.
    /// </summary>
    public interface INarrativeSourceAdapter
    {
        bool CanAdapt(string sourceCatalog);
        NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord);
    }

    public static class NarrativeJsonHelpers
    {
        public static string GetStringProp(JsonElement elem, string propName, string fallback = "")
        {
            if (elem.ValueKind == JsonValueKind.Object && elem.TryGetProperty(propName, out var p))
            {
                if (p.ValueKind == JsonValueKind.String)
                {
                    return p.GetString() ?? fallback;
                }
                return p.ToString();
            }
            return fallback;
        }

        public static string[] GetStringArrayProp(JsonElement elem, string propName)
        {
            if (elem.ValueKind == JsonValueKind.Object && elem.TryGetProperty(propName, out var p) && p.ValueKind == JsonValueKind.Array)
            {
                var list = new List<string>();
                foreach (var item in p.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.String)
                    {
                        string? s = item.GetString();
                        if (!string.IsNullOrEmpty(s)) list.Add(s);
                    }
                }
                return list.ToArray();
            }
            return Array.Empty<string>();
        }
    }

    #region Concrete Adapters

    /// <summary>
    /// Adapter for catalogs following the items array with a prose text body.
    /// </summary>
    public sealed class ProcessLogSourceAdapter : INarrativeSourceAdapter
    {
        private static readonly HashSet<string> HandledCatalogs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "narrative/boiler_feedwater_deaerator_audits.json",
            "narrative/ragdoll_germination_assays.json",
            "narrative/artesian_well_contamination_logs.json",
            "narrative/slow_sand_schmutzdecke_logs.json",
            "narrative/scavenger_expedition_route_notes.json",
            "narrative/surface_radiation_topo_sheets.json",
            "narrative/water_clock_orifice_silt_records.json",
            "narrative/pot_furnace_glass_melts.json",
            "narrative/bunker_children_folklore.json"
        };

        public bool CanAdapt(string sourceCatalog) => HandledCatalogs.Contains(sourceCatalog);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string prose = NarrativeJsonHelpers.GetStringProp(sourceRecord, "prose");
            string timeRel = NarrativeJsonHelpers.GetStringProp(sourceRecord, "timestamp_relative");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            // Extract domain-specific metadata
            string subtitle = "";
            string category = "Process Log";

            if (entry.SourceCatalog.Contains("boiler_feedwater"))
            {
                category = "Equipment Failure Report";
                string plant = NarrativeJsonHelpers.GetStringProp(sourceRecord, "boiler_plant_id");
                string oxygen = NarrativeJsonHelpers.GetStringProp(sourceRecord, "dissolved_oxygen_ppb");
                subtitle = $"Plant: {plant} | Dissolved O2: {oxygen} ppb | {timeRel}";
            }
            else if (entry.SourceCatalog.Contains("ragdoll_germination"))
            {
                category = "Greenhouse Seed Assay";
                string cultivar = NarrativeJsonHelpers.GetStringProp(sourceRecord, "crop_cultivar_name");
                string viability = NarrativeJsonHelpers.GetStringProp(sourceRecord, "germination_viability_pct");
                subtitle = $"Cultivar: {cultivar} | Viability: {viability}% | {timeRel}";
            }
            else if (entry.SourceCatalog.Contains("artesian_well"))
            {
                category = "Water Well Contamination Log";
                string well = NarrativeJsonHelpers.GetStringProp(sourceRecord, "well_identifier");
                string agent = NarrativeJsonHelpers.GetStringProp(sourceRecord, "contaminant_agent");
                subtitle = $"Well: {well} | Contaminant: {agent} | {timeRel}";
            }
            else if (entry.SourceCatalog.Contains("slow_sand"))
            {
                category = "Water Treatment Schmutzdecke Log";
                string basin = NarrativeJsonHelpers.GetStringProp(sourceRecord, "filter_basin_id");
                string turb = NarrativeJsonHelpers.GetStringProp(sourceRecord, "effluent_turbidity_ntu");
                subtitle = $"Basin: {basin} | Effluent Turbidity: {turb} NTU | {timeRel}";
            }
            else if (entry.SourceCatalog.Contains("scavenger_expedition"))
            {
                category = "Scavenger Route Notes";
                string scout = NarrativeJsonHelpers.GetStringProp(sourceRecord, "lead_scout_name");
                string route = NarrativeJsonHelpers.GetStringProp(sourceRecord, "route_identifier");
                subtitle = $"Scout: {scout} | Route: {route} | {timeRel}";
            }
            else if (entry.SourceCatalog.Contains("surface_radiation"))
            {
                category = "Radiation Topo Sheet";
                string quad = NarrativeJsonHelpers.GetStringProp(sourceRecord, "quadrangle_name");
                string peak = NarrativeJsonHelpers.GetStringProp(sourceRecord, "peak_gamma_field_r_hr");
                subtitle = $"Quadrangle: {quad} | Peak Gamma: {peak} R/hr | {timeRel}";
            }
            else if (entry.SourceCatalog.Contains("water_clock"))
            {
                category = "Clepsydra Telemetry Log";
                string station = NarrativeJsonHelpers.GetStringProp(sourceRecord, "clepsydra_station_id");
                string flow = NarrativeJsonHelpers.GetStringProp(sourceRecord, "nominal_flow_ml_min");
                subtitle = $"Station: {station} | Flow: {flow} mL/min | {timeRel}";
            }
            else if (entry.SourceCatalog.Contains("pot_furnace"))
            {
                category = "Foundry Glass Melt Log";
                string pot = NarrativeJsonHelpers.GetStringProp(sourceRecord, "furnace_pot_identifier");
                string temp = NarrativeJsonHelpers.GetStringProp(sourceRecord, "melt_temperature_celsius");
                subtitle = $"Pot: {pot} | Temp: {temp}°C | {timeRel}";
            }
            else if (entry.SourceCatalog.Contains("bunker_children"))
            {
                category = "Children Folklore & Verse";
                string tradition = NarrativeJsonHelpers.GetStringProp(sourceRecord, "tradition_type");
                string sector = NarrativeJsonHelpers.GetStringProp(sourceRecord, "origin_sector");
                subtitle = $"Tradition: {tradition} | Origin: {sector} | {timeRel}";
            }

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.discovery_id,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.discovery_id),
                SourceCatalog = entry.source_catalog,
                SourceRecordId = entry.source_record_id,
                Channel = entry.channel,
                ProducerId = entry.producer_id,
                MinDay = entry.min_day,
                Title = FormatTitle(entry.source_record_id),
                Subtitle = subtitle,
                BodyText = prose,
                Category = category,
                Tags = tags
            };
        }

        private static string FormatTitle(string recordId)
        {
            var parts = recordId.Split('_');
            var capitalized = new List<string>();
            foreach (var part in parts)
            {
                if (part.Length > 0)
                    capitalized.Add(char.ToUpperInvariant(part[0]) + part.Substring(1));
            }
            return string.Join(" ", capitalized);
        }
    }

    public sealed class BunkerGlitchSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) =>
            sourceCatalog.EndsWith("bunker_maintenance_glitches.json", StringComparison.OrdinalIgnoreCase);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string subsystem = NarrativeJsonHelpers.GetStringProp(sourceRecord, "affected_subsystem");
            string anomaly = NarrativeJsonHelpers.GetStringProp(sourceRecord, "anomaly_description");
            string protocol = NarrativeJsonHelpers.GetStringProp(sourceRecord, "emergency_protocol");
            string shiftNote = NarrativeJsonHelpers.GetStringProp(sourceRecord, "dmitri_shift_note");
            string telemetry = NarrativeJsonHelpers.GetStringProp(sourceRecord, "diagnostic_telemetry");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            string body = anomaly;
            if (!string.IsNullOrEmpty(protocol)) body += "\n\nEmergency Protocol:\n" + protocol;
            if (!string.IsNullOrEmpty(shiftNote)) body += "\n\nDmitri Shift Note:\n\"" + shiftNote + "\"";

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.discovery_id,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.discovery_id),
                SourceCatalog = entry.source_catalog,
                SourceRecordId = entry.source_record_id,
                Channel = entry.channel,
                ProducerId = entry.producer_id,
                MinDay = entry.min_day,
                Title = "Bunker Maintenance Glitch: " + subsystem,
                Subtitle = $"Telemetry: {telemetry}",
                BodyText = body,
                Category = "Bunker Maintenance Logs",
                Tags = tags
            };
        }
    }

    public sealed class BunkerBlueprintSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) =>
            sourceCatalog.EndsWith("bunker_blueprints_codex.json", StringComparison.OrdinalIgnoreCase);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string roomName = NarrativeJsonHelpers.GetStringProp(sourceRecord, "room_name");
            string headerSpec = NarrativeJsonHelpers.GetStringProp(sourceRecord, "structural_header_spec");
            string failureMode = NarrativeJsonHelpers.GetStringProp(sourceRecord, "catastrophic_failure_mode");
            string engineerNote = NarrativeJsonHelpers.GetStringProp(sourceRecord, "chief_engineer_note");
            string depth = NarrativeJsonHelpers.GetStringProp(sourceRecord, "optimal_depth_meters");
            string power = NarrativeJsonHelpers.GetStringProp(sourceRecord, "base_power_draw_kw");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            string body = $"Structural Spec: {headerSpec}\n\nFailure Hazard:\n{failureMode}";
            if (!string.IsNullOrEmpty(engineerNote))
                body += $"\n\nChief Engineer Note:\n\"{engineerNote}\"";

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.discovery_id,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.discovery_id),
                SourceCatalog = entry.source_catalog,
                SourceRecordId = entry.source_record_id,
                Channel = entry.channel,
                ProducerId = entry.producer_id,
                MinDay = entry.min_day,
                Title = "Architectural Blueprint: " + roomName,
                Subtitle = $"Depth: {depth}m | Power Draw: {power} kW",
                BodyText = body,
                Category = "Engineering Blueprint Notes",
                Tags = tags
            };
        }
    }

    public sealed class BunkerCourtSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) =>
            sourceCatalog.EndsWith("bunker_court_verdicts_codex.json", StringComparison.OrdinalIgnoreCase);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string defendant = NarrativeJsonHelpers.GetStringProp(sourceRecord, "defendant_name");
            string charge = NarrativeJsonHelpers.GetStringProp(sourceRecord, "charge_summary");
            string outcome = NarrativeJsonHelpers.GetStringProp(sourceRecord, "verdict_outcome");
            string penalty = NarrativeJsonHelpers.GetStringProp(sourceRecord, "disciplinary_penalty");
            string magistrate = NarrativeJsonHelpers.GetStringProp(sourceRecord, "presiding_magistrate");
            string docket = NarrativeJsonHelpers.GetStringProp(sourceRecord, "docket_number");
            string marginNotes = NarrativeJsonHelpers.GetStringProp(sourceRecord, "clerk_margin_notes");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            string body = $"Charge: {charge}\n\nVerdict: {outcome}\nPenalty: {penalty}";
            if (!string.IsNullOrEmpty(marginNotes))
                body += $"\n\nClerk Margin Notes:\n\"{marginNotes}\"";

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.discovery_id,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.discovery_id),
                SourceCatalog = entry.source_catalog,
                SourceRecordId = entry.source_record_id,
                Channel = entry.channel,
                ProducerId = entry.producer_id,
                MinDay = entry.min_day,
                Title = $"Tribunal Case {docket}: {defendant}",
                Subtitle = $"Presiding Magistrate: {magistrate}",
                BodyText = body,
                Category = "Council Meeting Minutes & Verdicts",
                Tags = tags
            };
        }
    }

    public sealed class WireConfessionSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) =>
            sourceCatalog.EndsWith("wire_confessions.json", StringComparison.OrdinalIgnoreCase);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string author = NarrativeJsonHelpers.GetStringProp(sourceRecord, "author_name");
            string title = NarrativeJsonHelpers.GetStringProp(sourceRecord, "title");
            string transcript = NarrativeJsonHelpers.GetStringProp(sourceRecord, "transcript");
            string day = NarrativeJsonHelpers.GetStringProp(sourceRecord, "recorded_day");
            string device = NarrativeJsonHelpers.GetStringProp(sourceRecord, "device_type");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.discovery_id,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.discovery_id),
                SourceCatalog = entry.source_catalog,
                SourceRecordId = entry.source_record_id,
                Channel = entry.channel,
                ProducerId = entry.producer_id,
                MinDay = entry.min_day,
                Title = $"Wire Confession: {title}",
                Subtitle = $"Speaker: {author} | Recorded Day: {day} | Medium: {device}",
                BodyText = transcript,
                Category = "Conflict Mediation & Wiretap Logs",
                Tags = tags
            };
        }
    }

    public sealed class TradeLedgerSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) =>
            sourceCatalog.EndsWith("bunker_trade_ledger_batch_2.json", StringComparison.OrdinalIgnoreCase);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string day = NarrativeJsonHelpers.GetStringProp(sourceRecord, "day");
            string giver = NarrativeJsonHelpers.GetStringProp(sourceRecord, "giver");
            string receiver = NarrativeJsonHelpers.GetStringProp(sourceRecord, "receiver");
            string given = NarrativeJsonHelpers.GetStringProp(sourceRecord, "given");
            string received = NarrativeJsonHelpers.GetStringProp(sourceRecord, "received");
            string notes = NarrativeJsonHelpers.GetStringProp(sourceRecord, "notes");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            string body = $"Barter Exchange:\n- Provided: {given}\n- Received: {received}\n\nLedger Notes:\n{notes}";

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.discovery_id,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.discovery_id),
                SourceCatalog = entry.source_catalog,
                SourceRecordId = entry.source_record_id,
                Channel = entry.channel,
                ProducerId = entry.producer_id,
                MinDay = entry.min_day,
                Title = $"Trade Ledger Entry: {giver} to {receiver}",
                Subtitle = $"Recorded Day {day} | Counterparty: {receiver}",
                BodyText = body,
                Category = "Bunker Trade Ledgers",
                Tags = tags
            };
        }
    }

    public sealed class RegionalTreatySourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) =>
            sourceCatalog.EndsWith("regional_treaty_protocols.json", StringComparison.OrdinalIgnoreCase);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string title = NarrativeJsonHelpers.GetStringProp(sourceRecord, "treaty_title");
            string articles = NarrativeJsonHelpers.GetStringProp(sourceRecord, "treaty_articles");
            string day = NarrativeJsonHelpers.GetStringProp(sourceRecord, "ratified_day");
            string territory = NarrativeJsonHelpers.GetStringProp(sourceRecord, "demarcated_territory");
            string[] factions = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "signatory_factions");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            string factionsStr = string.Join(", ", factions);
            string body = $"Demarcated Territory: {territory}\n\nArticles of Agreement:\n{articles}";

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.discovery_id,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.discovery_id),
                SourceCatalog = entry.source_catalog,
                SourceRecordId = entry.source_record_id,
                Channel = entry.channel,
                ProducerId = entry.producer_id,
                MinDay = entry.min_day,
                Title = $"Regional Treaty: {title}",
                Subtitle = $"Ratified Day {day} | Signatories: {factionsStr}",
                BodyText = body,
                Category = "Diplomatic Contact Records",
                Tags = tags
            };
        }
    }

    public sealed class SurgeonsCasebookSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) =>
            sourceCatalog.EndsWith("surgeons_casebook_batch_2.json", StringComparison.OrdinalIgnoreCase);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string patient = NarrativeJsonHelpers.GetStringProp(sourceRecord, "patient");
            string symptoms = NarrativeJsonHelpers.GetStringProp(sourceRecord, "presenting_symptoms");
            string diagnosis = NarrativeJsonHelpers.GetStringProp(sourceRecord, "diagnosis");
            string treatment = NarrativeJsonHelpers.GetStringProp(sourceRecord, "treatment");
            string outcome = NarrativeJsonHelpers.GetStringProp(sourceRecord, "outcome");
            string attending = NarrativeJsonHelpers.GetStringProp(sourceRecord, "attending");
            string admittedDay = NarrativeJsonHelpers.GetStringProp(sourceRecord, "admitted_day");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            string body = $"Presenting Symptoms: {symptoms}\n\nDiagnosis: {diagnosis}\nTreatment: {treatment}\nClinical Outcome: {outcome}";

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.discovery_id,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.discovery_id),
                SourceCatalog = entry.source_catalog,
                SourceRecordId = entry.source_record_id,
                Channel = entry.channel,
                ProducerId = entry.producer_id,
                MinDay = entry.min_day,
                Title = $"Surgeon's Casebook: {patient}",
                Subtitle = $"Attending: {attending} | Admitted Day: {admittedDay}",
                BodyText = body,
                Category = "Medical Casebook & Triage Records",
                Tags = tags
            };
        }
    }

    public sealed class DeadHandDirectiveSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) =>
            sourceCatalog.EndsWith("dead_hand_directives.json", StringComparison.OrdinalIgnoreCase);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string title = NarrativeJsonHelpers.GetStringProp(sourceRecord, "directive_title");
            string authority = NarrativeJsonHelpers.GetStringProp(sourceRecord, "issuing_authority");
            string transcript = NarrativeJsonHelpers.GetStringProp(sourceRecord, "transcript");
            string timeUtc = NarrativeJsonHelpers.GetStringProp(sourceRecord, "timestamp_utc");
            string clearance = NarrativeJsonHelpers.GetStringProp(sourceRecord, "clearance_level");
            string notes = NarrativeJsonHelpers.GetStringProp(sourceRecord, "archaeological_notes");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            string body = transcript;
            if (!string.IsNullOrEmpty(notes))
                body += $"\n\nArchaeological Notes:\n{notes}";

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.discovery_id,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.discovery_id),
                SourceCatalog = entry.source_catalog,
                SourceRecordId = entry.source_record_id,
                Channel = entry.channel,
                ProducerId = entry.producer_id,
                MinDay = entry.min_day,
                Title = $"Directive: {title}",
                Subtitle = $"Authority: {authority} | Clearance: {clearance} | UTC: {timeUtc}",
                BodyText = body,
                Category = "Faction Directives & Notices",
                Tags = tags
            };
        }
    }

    public sealed class CourierDispatchSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) =>
            sourceCatalog.EndsWith("courier_dispatches_master.json", StringComparison.OrdinalIgnoreCase);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string sender = NarrativeJsonHelpers.GetStringProp(sourceRecord, "sender");
            string recipient = NarrativeJsonHelpers.GetStringProp(sourceRecord, "recipient");
            string route = NarrativeJsonHelpers.GetStringProp(sourceRecord, "route");
            string transcript = NarrativeJsonHelpers.GetStringProp(sourceRecord, "transcript");
            string day = NarrativeJsonHelpers.GetStringProp(sourceRecord, "recorded_day");
            string status = NarrativeJsonHelpers.GetStringProp(sourceRecord, "delivery_status");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.discovery_id,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.discovery_id),
                SourceCatalog = entry.source_catalog,
                SourceRecordId = entry.source_record_id,
                Channel = entry.channel,
                ProducerId = entry.producer_id,
                MinDay = entry.min_day,
                Title = $"Courier Dispatch: {sender} to {recipient}",
                Subtitle = $"Route: {route} | Day: {day} | Status: {status}",
                BodyText = transcript,
                Category = "Courier Dispatch & Mission Records",
                Tags = tags
            };
        }
    }

    #endregion

    /// <summary>
    /// Engine-agnostic catalog manager for Plan 135 Narrative Discovery Activation.
    /// Loads the manifest, reads authoritative source catalogs on demand, applies
    /// deterministic source adapters, and connects discovery state to JournalSystem.
    /// </summary>
    public sealed class NarrativeDiscoveryCatalog
    {
        private readonly List<NarrativeDiscoveredRecord> _records = new List<NarrativeDiscoveredRecord>();
        private readonly Dictionary<string, NarrativeDiscoveredRecord> _byDiscoveryId =
            new Dictionary<string, NarrativeDiscoveredRecord>(StringComparer.Ordinal);
        private readonly Dictionary<string, List<NarrativeDiscoveredRecord>> _byProducerId =
            new Dictionary<string, List<NarrativeDiscoveredRecord>>(StringComparer.Ordinal);
        private readonly Dictionary<string, List<NarrativeDiscoveredRecord>> _byChannel =
            new Dictionary<string, List<NarrativeDiscoveredRecord>>(StringComparer.Ordinal);

        private readonly List<INarrativeSourceAdapter> _adapters = new List<INarrativeSourceAdapter>();

        public IReadOnlyList<NarrativeDiscoveredRecord> AllRecords => _records;
        public int Count => _records.Count;

        public NarrativeDiscoveryCatalog()
        {
            RegisterDefaultAdapters();
        }

        public void RegisterAdapter(INarrativeSourceAdapter adapter)
        {
            if (adapter != null)
                _adapters.Add(adapter);
        }

        private void RegisterDefaultAdapters()
        {
            _adapters.Add(new ProcessLogSourceAdapter());
            _adapters.Add(new BunkerGlitchSourceAdapter());
            _adapters.Add(new BunkerBlueprintSourceAdapter());
            _adapters.Add(new BunkerCourtSourceAdapter());
            _adapters.Add(new WireConfessionSourceAdapter());
            _adapters.Add(new TradeLedgerSourceAdapter());
            _adapters.Add(new RegionalTreatySourceAdapter());
            _adapters.Add(new SurgeonsCasebookSourceAdapter());
            _adapters.Add(new DeadHandDirectiveSourceAdapter());
            _adapters.Add(new CourierDispatchSourceAdapter());
        }

        public void Clear()
        {
            _records.Clear();
            _byDiscoveryId.Clear();
            _byProducerId.Clear();
            _byChannel.Clear();
        }

        public void LoadFromFiles(string dataDirectory, IFileIO files)
        {
            string manifestPath = Path.Combine(dataDirectory, "narrative_discovery_manifest.json");
            if (!files.FileExists(manifestPath)) return;
            string json = files.ReadAllText(manifestPath);
            Load(json, dataDirectory, files);
        }

        public void Load(string manifestJson, string dataDirectory, IFileIO files)
        {
            Clear();
            if (string.IsNullOrWhiteSpace(manifestJson)) return;

            using var doc = JsonDocument.Parse(manifestJson);
            if (!doc.RootElement.TryGetProperty("entries", out var entriesProp) || entriesProp.ValueKind != JsonValueKind.Array)
                return;

            var rawSourceCache = new Dictionary<string, JsonDocument>(StringComparer.Ordinal);

            try
            {
                foreach (var entryElem in entriesProp.EnumerateArray())
                {
                    string discId = NarrativeJsonHelpers.GetStringProp(entryElem, "discovery_id");
                    string sourceCatalog = NarrativeJsonHelpers.GetStringProp(entryElem, "source_catalog");
                    string sourceRecordId = NarrativeJsonHelpers.GetStringProp(entryElem, "source_record_id");
                    string channel = NarrativeJsonHelpers.GetStringProp(entryElem, "channel");
                    string producerId = NarrativeJsonHelpers.GetStringProp(entryElem, "producer_id");
                    int minDay = 1;
                    if (entryElem.TryGetProperty("min_day", out var mdProp) && mdProp.TryGetInt32(out int mdVal))
                        minDay = mdVal;

                    var entry = new NarrativeDiscoveryManifestEntry
                    {
                        discovery_id = discId,
                        source_catalog = sourceCatalog,
                        source_record_id = sourceRecordId,
                        channel = channel,
                        producer_id = producerId,
                        min_day = minDay
                    };

                    // Find adapter
                    INarrativeSourceAdapter? matchedAdapter = null;
                    for (int i = 0; i < _adapters.Count; i++)
                    {
                        if (_adapters[i].CanAdapt(sourceCatalog))
                        {
                            matchedAdapter = _adapters[i];
                            break;
                        }
                    }

                    if (matchedAdapter == null) continue;

                    // Load source document
                    string fullPath = Path.Combine(dataDirectory, sourceCatalog);
                    if (!files.FileExists(fullPath)) continue;

                    if (!rawSourceCache.TryGetValue(sourceCatalog, out var sourceDoc))
                    {
                        string sourceText = files.ReadAllText(fullPath);
                        sourceDoc = JsonDocument.Parse(sourceText);
                        rawSourceCache[sourceCatalog] = sourceDoc;
                    }

                    // Locate record in source document
                    if (TryFindSourceRecord(sourceDoc.RootElement, sourceRecordId, out var sourceRecord))
                    {
                        var projected = matchedAdapter.Adapt(entry, sourceRecord);
                        if (projected != null && !string.IsNullOrEmpty(projected.DiscoveryId))
                        {
                            _records.Add(projected);
                            _byDiscoveryId[projected.DiscoveryId] = projected;

                            if (!_byProducerId.TryGetValue(projected.ProducerId, out var prodList))
                            {
                                prodList = new List<NarrativeDiscoveredRecord>();
                                _byProducerId[projected.ProducerId] = prodList;
                            }
                            prodList.Add(projected);

                            if (!_byChannel.TryGetValue(projected.Channel, out var chanList))
                            {
                                chanList = new List<NarrativeDiscoveredRecord>();
                                _byChannel[projected.Channel] = chanList;
                            }
                            chanList.Add(projected);
                        }
                    }
                }
            }
            finally
            {
                foreach (var kvp in rawSourceCache)
                {
                    kvp.Value.Dispose();
                }
            }
        }

        private static bool TryFindSourceRecord(JsonElement root, string recordId, out JsonElement found)
        {
            found = default;
            if (root.ValueKind != JsonValueKind.Object) return false;

            foreach (var prop in root.EnumerateObject())
            {
                if (prop.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in prop.Value.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.Object)
                        {
                            foreach (var field in item.EnumerateObject())
                            {
                                if (field.Value.ValueKind == JsonValueKind.String &&
                                    string.Equals(field.Value.GetString(), recordId, StringComparison.Ordinal))
                                {
                                    found = item;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool TryGetRecord(string discoveryId, out NarrativeDiscoveredRecord? record)
        {
            if (string.IsNullOrEmpty(discoveryId))
            {
                record = null;
                return false;
            }
            return _byDiscoveryId.TryGetValue(discoveryId, out record);
        }

        public IReadOnlyList<NarrativeDiscoveredRecord> GetByProducer(string producerId)
        {
            if (!string.IsNullOrEmpty(producerId) && _byProducerId.TryGetValue(producerId, out var list))
                return list;
            return Array.Empty<NarrativeDiscoveredRecord>();
        }

        public IReadOnlyList<NarrativeDiscoveredRecord> GetByChannel(string channel)
        {
            if (!string.IsNullOrEmpty(channel) && _byChannel.TryGetValue(channel, out var list))
                return list;
            return Array.Empty<NarrativeDiscoveredRecord>();
        }

        /// <summary>
        /// Attempts to discover the given narrative record for the player via JournalSystem.
        /// Idempotent: returns true only on first discovery; subsequent calls return false.
        /// </summary>
        public bool TryDiscover(string discoveryId, JournalSystem journal, out NarrativeDiscoveredRecord? record)
        {
            record = null;
            if (journal == null || string.IsNullOrEmpty(discoveryId)) return false;

            if (!TryGetRecord(discoveryId, out record) || record == null)
                return false;

            return journal.UnlockNarrativeDiscovered(discoveryId);
        }
    }
}
