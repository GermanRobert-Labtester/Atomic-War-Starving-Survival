// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
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
        // Optional additional, explicit producer contexts. The first entry is
        // the primary producer and remains represented by producer_id for
        // backwards compatibility with the existing manifest contract.
        public string[] producer_ids = Array.Empty<string>();
        public int min_day = 1;
        public int weight = 1;
        public bool one_time = true;
        public string truth_class = string.Empty;
        public string provenance_label = string.Empty;
        public string identity_status = string.Empty;
        public string numeric_claim_label = string.Empty;
        public string[] related_discovery_ids = Array.Empty<string>();

        public string DiscoveryId => discovery_id;
        public string SourceCatalog => source_catalog;
        public string SourceRecordId => source_record_id;
        public string Channel => channel;
        public string ProducerId => producer_id;
        public IReadOnlyList<string> ProducerIds => producer_ids;
        public int MinDay => min_day;
        public int Weight => weight;
        public bool OneTime => one_time;
        public string TruthClass => truth_class;
        public string ProvenanceLabel => provenance_label;
        public string IdentityStatus => identity_status;
        public string NumericClaimLabel => numeric_claim_label;
        public IReadOnlyList<string> RelatedDiscoveryIds => related_discovery_ids;
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
        public string[] ProducerIds { get; set; } = Array.Empty<string>();
        public int MinDay { get; set; } = 1;

        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string BodyText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public string TruthClass { get; set; } = string.Empty;
        public string ProvenanceLabel { get; set; } = string.Empty;
        public string IdentityStatus { get; set; } = string.Empty;
        public string NumericClaimLabel { get; set; } = string.Empty;
        public string RecordFamily { get; set; } = string.Empty;
        public string FacilityOrStationLabel { get; set; } = string.Empty;
        public string TechnicalSummary { get; set; } = string.Empty;
        public string[] RelatedDiscoveryIds { get; set; } = Array.Empty<string>();
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

        public static int GetIntProp(JsonElement elem, string propName, int fallback = 0)
        {
            if (elem.ValueKind == JsonValueKind.Object && elem.TryGetProperty(propName, out var p))
            {
                if (p.ValueKind == JsonValueKind.Number && p.TryGetInt32(out int val))
                {
                    return val;
                }
            }
            return fallback;
        }

        public static float GetFloatProp(JsonElement elem, string propName, float fallback = 0f)
        {
            if (elem.ValueKind == JsonValueKind.Object && elem.TryGetProperty(propName, out var p))
            {
                if (p.ValueKind == JsonValueKind.Number && p.TryGetSingle(out float val))
                    return val;
            }
            return fallback;
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

    /// <summary>
    /// Exact source allowlist for Plan 150. Personal and unsent letters are
    /// journal-only artifacts; this contract never routes into inventory,
    /// faction, or quest authority.
    /// </summary>
    public static class PersonalLetterRuntimeContract
    {
        public const string LettersExpansionCatalog = "narrative/letters_expansion.json";
        public const string UnsentLettersBatch2Catalog = "narrative/unsent_letters_batch_2.json";

        public static readonly string[] SourceCatalogs =
        {
            LettersExpansionCatalog, UnsentLettersBatch2Catalog
        };

        public static bool IsSourceCatalog(string sourceCatalog)
        {
            string normalized = (sourceCatalog ?? string.Empty).Replace('\\', '/');
            return normalized.EndsWith("letters_expansion.json", StringComparison.OrdinalIgnoreCase)
                || normalized.EndsWith("unsent_letters_batch_2.json", StringComparison.OrdinalIgnoreCase);
        }
    }

    public sealed class PersonalLetterSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) =>
            PersonalLetterRuntimeContract.IsSourceCatalog(sourceCatalog);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string sender = NarrativeJsonHelpers.GetStringProp(sourceRecord, "sender");
            string recipient = NarrativeJsonHelpers.GetStringProp(sourceRecord, "recipient");
            string location = NarrativeJsonHelpers.GetStringProp(sourceRecord, "location");
            string content = NarrativeJsonHelpers.GetStringProp(sourceRecord, "content");
            int day = NarrativeJsonHelpers.GetIntProp(sourceRecord, "day");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            string title = $"Letter: To {recipient}";
            string subtitle = $"From: {sender} | Day {day} | {location}";

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.discovery_id,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.discovery_id),
                SourceCatalog = entry.source_catalog,
                SourceRecordId = entry.source_record_id,
                Channel = entry.channel,
                ProducerId = entry.producer_id,
                MinDay = entry.min_day,
                Title = title,
                Subtitle = subtitle,
                BodyText = content,
                Category = "Personal Letters & Unsent Correspondence",
                Tags = tags
            };
        }
    }

    /// <summary>
    /// Exact source allowlist for Plan 151. Abyssal science logs are archival
    /// observations only; historical measurements never mutate live systems.
    /// </summary>
    public static class AbyssalAnomaliesRuntimeContract
    {
        public const string HydrophoneCatalog = "narrative/hydrophone_acoustic_logs.json";
        public const string GeothermalCatalog = "narrative/geothermal_borehole_logs.json";
        public const string CryopodCatalog = "narrative/cryopod_failure_logs.json";
        public const string SaltMineCatalog = "narrative/salt_mine_inscriptions.json";

        public static readonly string[] SourceCatalogs =
        {
            HydrophoneCatalog, GeothermalCatalog, CryopodCatalog, SaltMineCatalog
        };

        public static bool IsSourceCatalog(string sourceCatalog)
        {
            string normalized = (sourceCatalog ?? string.Empty).Replace('\\', '/');
            return normalized.EndsWith("hydrophone_acoustic_logs.json", StringComparison.OrdinalIgnoreCase)
                || normalized.EndsWith("geothermal_borehole_logs.json", StringComparison.OrdinalIgnoreCase)
                || normalized.EndsWith("cryopod_failure_logs.json", StringComparison.OrdinalIgnoreCase)
                || normalized.EndsWith("salt_mine_inscriptions.json", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Activated records only — deferred Phase-2 rows stay locked even when
        /// their producer is inspected.
        /// </summary>
        public static bool IsActivatedSourceRecord(string sourceRecordId)
            => AbyssalAnomaliesProjection.IsActivated(sourceRecordId);
    }

    public sealed class AbyssalAnomaliesSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) =>
            AbyssalAnomaliesRuntimeContract.IsSourceCatalog(sourceCatalog);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string prose = NarrativeJsonHelpers.GetStringProp(sourceRecord, "prose");
            string timestamp = NarrativeJsonHelpers.GetStringProp(sourceRecord, "timestamp_relative");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");

            string title;
            string subtitle;
            string category;

            if (entry.source_catalog.EndsWith("hydrophone_acoustic_logs.json", StringComparison.OrdinalIgnoreCase))
            {
                string callsign = NarrativeJsonHelpers.GetStringProp(sourceRecord, "buoy_callsign");
                string classification = NarrativeJsonHelpers.GetStringProp(sourceRecord, "signal_classification");
                string freq = NarrativeJsonHelpers.GetStringProp(sourceRecord, "acoustic_frequency_hz");
                string depth = NarrativeJsonHelpers.GetStringProp(sourceRecord, "depth_meters");
                string amp = NarrativeJsonHelpers.GetStringProp(sourceRecord, "signal_amplitude_db");

                title = $"Hydrophone Log: {callsign} ({classification})";
                subtitle = $"Depth: {depth}m | Freq: {freq} Hz | Amp: {amp} dB | Recorded: {timestamp}";
                category = "Abyssal Anomalies — Hydrophone Acoustic Logs";
            }
            else if (entry.source_catalog.EndsWith("geothermal_borehole_logs.json", StringComparison.OrdinalIgnoreCase))
            {
                string boreholeId = NarrativeJsonHelpers.GetStringProp(sourceRecord, "borehole_id");
                string formation = NarrativeJsonHelpers.GetStringProp(sourceRecord, "geological_formation");
                string depth = NarrativeJsonHelpers.GetStringProp(sourceRecord, "depth_meters");
                string temp = NarrativeJsonHelpers.GetStringProp(sourceRecord, "temperature_celsius");
                string pressure = NarrativeJsonHelpers.GetStringProp(sourceRecord, "casing_pressure_bar");

                title = $"Borehole Log: {boreholeId} ({formation})";
                subtitle = $"Depth: {depth}m | Temp at recording: {temp}°C | Pressure: {pressure} bar | Recorded: {timestamp}";
                category = "Abyssal Anomalies — Geothermal Borehole Logs";
            }
            else if (entry.source_catalog.EndsWith("cryopod_failure_logs.json", StringComparison.OrdinalIgnoreCase))
            {
                string podId = NarrativeJsonHelpers.GetStringProp(sourceRecord, "pod_id");
                string subject = NarrativeJsonHelpers.GetStringProp(sourceRecord, "subject_designation");
                string alert = NarrativeJsonHelpers.GetStringProp(sourceRecord, "system_alert");
                string temp = NarrativeJsonHelpers.GetStringProp(sourceRecord, "core_temperature_kelvin");
                string pressure = NarrativeJsonHelpers.GetStringProp(sourceRecord, "chamber_pressure_kpa");

                title = $"Cryopod Incident: {podId} ({subject})";
                subtitle = $"Alert: {alert} | Core Temp at incident: {temp} K | Chamber Pressure: {pressure} kPa | Recorded: {timestamp}";
                category = "Abyssal Anomalies — Cryopod Failure Logs";
            }
            else // salt_mine_inscriptions.json
            {
                string gallery = NarrativeJsonHelpers.GetStringProp(sourceRecord, "mine_gallery");
                string medium = NarrativeJsonHelpers.GetStringProp(sourceRecord, "rock_medium");
                string tool = NarrativeJsonHelpers.GetStringProp(sourceRecord, "inscription_tool");
                string recorder = NarrativeJsonHelpers.GetStringProp(sourceRecord, "recorder_identity");

                title = $"Salt-Mine Inscription: {gallery} ({recorder})";
                subtitle = $"Medium: {medium} | Tool: {tool} | Recorded: {timestamp}";
                category = "Abyssal Anomalies — Salt-Mine Inscriptions";
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
                Title = title,
                Subtitle = subtitle,
                BodyText = prose,
                Category = category,
                Tags = tags
            };
        }
    }

    /// <summary>
    /// Stable source names and presentation rules for Plan 153. These records
    /// are cultural artifacts. This class deliberately contains no adapters to
    /// RadiationSystem, Foundry, audio, factions, or mortality systems.
    /// </summary>
    public static class FringeCultRuntimeContract
    {
        public const string CobaltCatalog = "narrative/cobalt_liturgies.json";
        public const string IronCatalog = "narrative/iron_synod_canons.json";
        public const string HymnalCatalog = "narrative/geophone_hymnals.json";
        public const string EpitaphCatalog = "narrative/wasteland_grave_epitaphs.json";

        public static readonly string[] SourceCatalogs =
        {
            CobaltCatalog, IronCatalog, HymnalCatalog, EpitaphCatalog
        };

        public static bool IsSourceCatalog(string sourceCatalog)
        {
            if (string.IsNullOrEmpty(sourceCatalog)) return false;
            for (int i = 0; i < SourceCatalogs.Length; i++)
            {
                if (string.Equals(SourceCatalogs[i], sourceCatalog, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public static string DefaultTruthClass(string sourceCatalog)
        {
            if (sourceCatalog.EndsWith("cobalt_liturgies.json", StringComparison.OrdinalIgnoreCase))
                return "Doctrine / Belief";
            if (sourceCatalog.EndsWith("iron_synod_canons.json", StringComparison.OrdinalIgnoreCase))
                return "Institutional Rule";
            if (sourceCatalog.EndsWith("geophone_hymnals.json", StringComparison.OrdinalIgnoreCase))
                return "Historical Observation";
            return "Memorial Testimony";
        }

        public static bool IsValidTruthClass(string truthClass)
        {
            return string.Equals(truthClass, "Doctrine / Belief", StringComparison.Ordinal)
                || string.Equals(truthClass, "Ritual Procedure", StringComparison.Ordinal)
                || string.Equals(truthClass, "Institutional Rule", StringComparison.Ordinal)
                || string.Equals(truthClass, "Memorial Testimony", StringComparison.Ordinal)
                || string.Equals(truthClass, "Historical Observation", StringComparison.Ordinal)
                || string.Equals(truthClass, "Mixed / Requires Reconciliation", StringComparison.Ordinal);
        }
    }

    /// <summary>Projects the four fringe-cult source schemas without executing
    /// any doctrine as a simulation command.</summary>
    public sealed class FringeCultSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) => FringeCultRuntimeContract.IsSourceCatalog(sourceCatalog);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string timestamp = NarrativeJsonHelpers.GetStringProp(sourceRecord, "timestamp_relative");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");
            string title;
            string subtitle;
            string category;
            string claimLabel;
            string defaultProvenance;
            string defaultIdentity;

            if (entry.SourceCatalog.EndsWith("cobalt_liturgies.json", StringComparison.OrdinalIgnoreCase))
            {
                string group = NarrativeJsonHelpers.GetStringProp(sourceRecord, "cult_faction");
                string liturgy = NarrativeJsonHelpers.GetStringProp(sourceRecord, "liturgy_type");
                string sacrament = NarrativeJsonHelpers.GetStringProp(sourceRecord, "ritual_sacrament");
                int threshold = NarrativeJsonHelpers.GetIntProp(sourceRecord, "sacred_rad_threshold_cpm");
                title = $"Cobalt Liturgy: {liturgy}";
                subtitle = $"Group: {group} · Sacrament: {sacrament} · Recorded: {timestamp}";
                claimLabel = $"Sacred count named in the liturgy: {threshold.ToString(CultureInfo.InvariantCulture)} cpm";
                category = "Fringe Cults · Cobalt Liturgies";
                defaultProvenance = "Authored liturgy record";
                defaultIdentity = "Unresolved sect identity; display only";
            }
            else if (entry.SourceCatalog.EndsWith("iron_synod_canons.json", StringComparison.OrdinalIgnoreCase))
            {
                string chapter = NarrativeJsonHelpers.GetStringProp(sourceRecord, "synod_chapter");
                string number = NarrativeJsonHelpers.GetStringProp(sourceRecord, "canon_number");
                string rule = NarrativeJsonHelpers.GetStringProp(sourceRecord, "metallurgical_rule");
                int temperature = NarrativeJsonHelpers.GetIntProp(sourceRecord, "sacred_temperature_celsius");
                title = $"Iron Synod Canon {number}: {rule}";
                subtitle = $"Chapter: {chapter} · Recorded: {timestamp}";
                claimLabel = $"Canon-prescribed furnace temperature: {temperature.ToString(CultureInfo.InvariantCulture)} °C";
                category = "Fringe Cults · Iron Synod Canons";
                defaultProvenance = "Authored canon record";
                defaultIdentity = "Unresolved institutional chapter; display only";
            }
            else if (entry.SourceCatalog.EndsWith("geophone_hymnals.json", StringComparison.OrdinalIgnoreCase))
            {
                string circle = NarrativeJsonHelpers.GetStringProp(sourceRecord, "monastery_circle");
                string number = NarrativeJsonHelpers.GetStringProp(sourceRecord, "hymn_number");
                string mode = NarrativeJsonHelpers.GetStringProp(sourceRecord, "liturgical_acoustic_mode");
                float frequency = NarrativeJsonHelpers.GetFloatProp(sourceRecord, "resonant_frequency_hz");
                title = $"Geophone Hymnal {number}: {mode}";
                subtitle = $"Circle: {circle} · Recorded: {timestamp}";
                claimLabel = $"Hymnal frequency notation: {frequency.ToString("0.###", CultureInfo.InvariantCulture)} Hz";
                category = "Fringe Cults · Geophone Hymnals";
                defaultProvenance = "Authored hymnal record";
                defaultIdentity = "Unresolved monastery circle; display only";
            }
            else
            {
                string site = NarrativeJsonHelpers.GetStringProp(sourceRecord, "grave_site");
                string material = NarrativeJsonHelpers.GetStringProp(sourceRecord, "marker_material");
                string deceased = NarrativeJsonHelpers.GetStringProp(sourceRecord, "deceased_identity");
                string cause = NarrativeJsonHelpers.GetStringProp(sourceRecord, "cause_of_death");
                title = $"Wasteland Epitaph: {deceased}";
                subtitle = $"Grave site: {site} · Marker: {material} · Recorded: {timestamp}";
                claimLabel = $"Cause of death recorded on marker: {cause}";
                category = "Fringe Cults · Wasteland Epitaphs";
                defaultProvenance = "Recovered epitaph record";
                defaultIdentity = deceased.IndexOf("UNKNOWN", StringComparison.OrdinalIgnoreCase) >= 0
                    ? "Anonymous/local memorial"
                    : "Unresolved memorial identity; display only";
            }

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.DiscoveryId,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.DiscoveryId),
                SourceCatalog = entry.SourceCatalog,
                SourceRecordId = entry.SourceRecordId,
                Channel = entry.Channel,
                ProducerId = entry.ProducerId,
                MinDay = entry.MinDay,
                Title = title,
                Subtitle = subtitle,
                BodyText = NarrativeJsonHelpers.GetStringProp(sourceRecord, "prose"),
                Category = category,
                Tags = tags,
                TruthClass = string.IsNullOrEmpty(entry.TruthClass)
                    ? FringeCultRuntimeContract.DefaultTruthClass(entry.SourceCatalog)
                    : entry.TruthClass,
                ProvenanceLabel = string.IsNullOrEmpty(entry.ProvenanceLabel) ? defaultProvenance : entry.ProvenanceLabel,
                IdentityStatus = string.IsNullOrEmpty(entry.IdentityStatus) ? defaultIdentity : entry.IdentityStatus,
                NumericClaimLabel = string.IsNullOrEmpty(entry.NumericClaimLabel) ? claimLabel : entry.NumericClaimLabel,
                RelatedDiscoveryIds = entry.related_discovery_ids ?? Array.Empty<string>()
            };
        }
    }

    /// <summary>
    /// Exact source allowlist for Plan 156. These catalogs contain authored
    /// industrial observations; their measurements never become production
    /// parameters or item effects through this adapter.
    /// </summary>
    public static class PaperPrintRuntimeContract
    {
        public const string HollanderCatalog = "narrative/hollander_beater_pulping_logs.json";
        public const string DeckleCatalog = "narrative/deckle_mould_watermark_audits.json";
        public const string PressCatalog = "narrative/screw_press_felt_reports.json";
        public const string SizingCatalog = "narrative/tub_sizing_gelatin_assays.json";
        public const string RagPulpCatalog = "narrative/rag_pulp_beater_records.json";
        public const string InkCatalog = "narrative/iron_gall_ink_acidity_reports.json";
        public const string TypeCatalog = "narrative/typographic_lead_wear_logs.json";
        public const string StencilCatalog = "narrative/stencil_propaganda_smear_logs.json";

        public static readonly string[] SourceCatalogs =
        {
            HollanderCatalog, DeckleCatalog, PressCatalog, SizingCatalog,
            RagPulpCatalog, InkCatalog, TypeCatalog, StencilCatalog
        };

        public static bool IsSourceCatalog(string sourceCatalog)
        {
            string normalized = (sourceCatalog ?? string.Empty).Replace('\\', '/');
            for (int i = 0; i < SourceCatalogs.Length; i++)
            {
                if (string.Equals(normalized, SourceCatalogs[i], StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public static bool IsPaperMakingCatalog(string sourceCatalog) =>
            string.Equals((sourceCatalog ?? string.Empty).Replace('\\', '/'), HollanderCatalog, StringComparison.OrdinalIgnoreCase)
            || string.Equals((sourceCatalog ?? string.Empty).Replace('\\', '/'), DeckleCatalog, StringComparison.OrdinalIgnoreCase)
            || string.Equals((sourceCatalog ?? string.Empty).Replace('\\', '/'), PressCatalog, StringComparison.OrdinalIgnoreCase)
            || string.Equals((sourceCatalog ?? string.Empty).Replace('\\', '/'), SizingCatalog, StringComparison.OrdinalIgnoreCase);

        public static string GetFamily(string sourceCatalog)
        {
            string normalized = (sourceCatalog ?? string.Empty).Replace('\\', '/');
            if (string.Equals(normalized, HollanderCatalog, StringComparison.OrdinalIgnoreCase)) return "Paper Making · Hollander Beater Pulping";
            if (string.Equals(normalized, DeckleCatalog, StringComparison.OrdinalIgnoreCase)) return "Paper Making · Deckle Mould & Watermark";
            if (string.Equals(normalized, PressCatalog, StringComparison.OrdinalIgnoreCase)) return "Paper Making · Screw Press & Felt";
            if (string.Equals(normalized, SizingCatalog, StringComparison.OrdinalIgnoreCase)) return "Paper Making · Tub Sizing";
            if (string.Equals(normalized, RagPulpCatalog, StringComparison.OrdinalIgnoreCase)) return "Printing · Rag Pulp Beater";
            if (string.Equals(normalized, InkCatalog, StringComparison.OrdinalIgnoreCase)) return "Printing · Iron-Gall Ink Assay";
            if (string.Equals(normalized, TypeCatalog, StringComparison.OrdinalIgnoreCase)) return "Printing · Typographic Lead Wear";
            if (string.Equals(normalized, StencilCatalog, StringComparison.OrdinalIgnoreCase)) return "Printing · Stencil Propaganda Smear";
            return string.Empty;
        }
    }

    /// <summary>
    /// Projects the eight Plan 156 source schemas into the shared codex model.
    /// It deliberately reads numeric observations only to render provenance
    /// labels; it contains no effect or production dispatch path.
    /// </summary>
    public sealed class PaperPrintSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) => PaperPrintRuntimeContract.IsSourceCatalog(sourceCatalog);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string source = entry.SourceCatalog.Replace('\\', '/');
            string timestamp = NarrativeJsonHelpers.GetStringProp(sourceRecord, "timestamp_relative");
            string[] tags = NarrativeJsonHelpers.GetStringArrayProp(sourceRecord, "tags");
            string family = PaperPrintRuntimeContract.GetFamily(source);
            string facility;
            string title;
            string technicalSummary;

            if (string.Equals(source, PaperPrintRuntimeContract.HollanderCatalog, StringComparison.OrdinalIgnoreCase))
            {
                string tub = NarrativeJsonHelpers.GetStringProp(sourceRecord, "beater_tub_id");
                string feedstock = NarrativeJsonHelpers.GetStringProp(sourceRecord, "rag_feedstock_type");
                facility = "Beater tub: " + tub;
                title = "Hollander Beater Log: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored measurements — beating duration: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "beating_duration_hours"))} h; Schopper-Riegler freeness: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "schopper_riegler_freeness_sr"))} SR; feedstock: {feedstock}.";
            }
            else if (string.Equals(source, PaperPrintRuntimeContract.DeckleCatalog, StringComparison.OrdinalIgnoreCase))
            {
                string mould = NarrativeJsonHelpers.GetStringProp(sourceRecord, "mould_frame_id");
                float width = NarrativeJsonHelpers.GetFloatProp(sourceRecord, "sheet_width_mm");
                float length = NarrativeJsonHelpers.GetFloatProp(sourceRecord, "sheet_length_mm");
                facility = "Mould frame: " + mould;
                title = "Deckle Mould Audit: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored measurements — wire mesh: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "wire_mesh_count_per_inch"))}/in; sheet format: {FormatNumber(width)} × {FormatNumber(length)} mm.";
            }
            else if (string.Equals(source, PaperPrintRuntimeContract.PressCatalog, StringComparison.OrdinalIgnoreCase))
            {
                string station = NarrativeJsonHelpers.GetStringProp(sourceRecord, "press_station_id");
                facility = "Press station: " + station;
                title = "Screw Press Report: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored measurements — post: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "post_sheet_count"))} sheets; pressing force: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "pressing_force_kilonewtons"))} kN; moisture removed: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "moisture_removed_pct"))}%.";
            }
            else if (string.Equals(source, PaperPrintRuntimeContract.SizingCatalog, StringComparison.OrdinalIgnoreCase))
            {
                string vat = NarrativeJsonHelpers.GetStringProp(sourceRecord, "sizing_vat_id");
                facility = "Sizing vat: " + vat;
                title = "Tub Sizing Assay: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored measurements — gelatin solution: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "gelatin_solution_temp_celsius"))} °C; alum addition: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "alum_additive_pct"))}%; Cobb absorption: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "cobb_water_absorption_g_per_m2"))} g/m².";
            }
            else if (string.Equals(source, PaperPrintRuntimeContract.RagPulpCatalog, StringComparison.OrdinalIgnoreCase))
            {
                string station = NarrativeJsonHelpers.GetStringProp(sourceRecord, "beater_station_id");
                string fiber = NarrativeJsonHelpers.GetStringProp(sourceRecord, "raw_fiber_source");
                facility = "Beater station: " + station;
                title = "Rag-Pulp Beater Record: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored measurements — Canadian freeness: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "freeness_canadian_ml"))} mL; hydration: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "pulp_hydration_hours"))} h; fiber source: {fiber}.";
            }
            else if (string.Equals(source, PaperPrintRuntimeContract.InkCatalog, StringComparison.OrdinalIgnoreCase))
            {
                string formulation = NarrativeJsonHelpers.GetStringProp(sourceRecord, "ink_formulation_code");
                string tannin = NarrativeJsonHelpers.GetStringProp(sourceRecord, "tannin_source");
                string pigment = NarrativeJsonHelpers.GetStringProp(sourceRecord, "pigment_complex");
                facility = "Ink formulation: " + formulation;
                title = "Iron-Gall Ink Assay: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored assay — measured pH: {FormatNumber(NarrativeJsonHelpers.GetFloatProp(sourceRecord, "measured_ph_level"))}; tannin source: {tannin}; pigment complex: {pigment}.";
            }
            else if (string.Equals(source, PaperPrintRuntimeContract.TypeCatalog, StringComparison.OrdinalIgnoreCase))
            {
                string fontCase = NarrativeJsonHelpers.GetStringProp(sourceRecord, "font_case_identifier");
                string metal = NarrativeJsonHelpers.GetStringProp(sourceRecord, "type_metal_composition");
                string wear = NarrativeJsonHelpers.GetStringProp(sourceRecord, "wear_phenomenon");
                facility = "Font case: " + fontCase;
                title = "Typographic Lead Wear Log: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored wear record — impressions: {NarrativeJsonHelpers.GetIntProp(sourceRecord, "impression_count_cycles")}; metal: {metal}; phenomenon: {wear}.";
            }
            else
            {
                string print = NarrativeJsonHelpers.GetStringProp(sourceRecord, "stencil_print_id");
                string matrix = NarrativeJsonHelpers.GetStringProp(sourceRecord, "matrix_material_type");
                string pigment = NarrativeJsonHelpers.GetStringProp(sourceRecord, "ink_pigment_base");
                string smear = NarrativeJsonHelpers.GetStringProp(sourceRecord, "smear_artifact_description");
                facility = "Print artifact: " + print;
                title = "Stencil Print Artifact: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored print artifact — matrix: {matrix}; pigment: {pigment}; smear: {smear}.";
            }

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.DiscoveryId,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.DiscoveryId),
                SourceCatalog = source,
                SourceRecordId = entry.SourceRecordId,
                Channel = entry.Channel,
                ProducerId = entry.ProducerId,
                ProducerIds = entry.ProducerIds.Count > 0 ? ToArray(entry.ProducerIds) : new[] { entry.ProducerId },
                MinDay = entry.MinDay,
                Title = title,
                Subtitle = $"{facility} · Recorded: {timestamp}",
                BodyText = NarrativeJsonHelpers.GetStringProp(sourceRecord, "prose"),
                Category = "Paper & Print Material Culture",
                Tags = tags,
                TruthClass = string.IsNullOrEmpty(entry.TruthClass) ? "Historical Observation" : entry.TruthClass,
                ProvenanceLabel = string.IsNullOrEmpty(entry.ProvenanceLabel) ? "Authored industrial process record" : entry.ProvenanceLabel,
                IdentityStatus = string.IsNullOrEmpty(entry.IdentityStatus) ? "Static facility/station label; display only" : entry.IdentityStatus,
                NumericClaimLabel = string.IsNullOrEmpty(entry.NumericClaimLabel) ? "Authored measurement — not a live production value" : entry.NumericClaimLabel,
                RecordFamily = family,
                FacilityOrStationLabel = facility,
                TechnicalSummary = technicalSummary,
                RelatedDiscoveryIds = entry.related_discovery_ids ?? Array.Empty<string>()
            };
        }

        private static string[] ToArray(IReadOnlyList<string> values)
        {
            var result = new string[values.Count];
            for (int i = 0; i < values.Count; i++) result[i] = values[i];
            return result;
        }

        private static string FormatNumber(float value) => value.ToString("0.###", CultureInfo.InvariantCulture);

        private static string FormatTitle(string recordId)
        {
            var parts = recordId.Split('_');
            var words = new List<string>();
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                    words.Add(char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1));
            }
            return string.Join(" ", words);
        }
    }

    /// <summary>
    /// Exact source allowlist for Plan 160. Bone, horn and antler records are
    /// authored process observations; their material, animal and geometry
    /// labels never become inventory, wildlife, crafting, durability, combat
    /// or trade authority through this contract.
    /// </summary>
    public static class BoneHornRuntimeContract
    {
        public const string DegreasingCatalog = "narrative/bone_degreasing_prep_logs.json";
        public const string SawingCatalog = "narrative/antler_horn_sawing_records.json";
        public const string PolishingCatalog = "narrative/scraping_polishing_reports.json";
        public const string ToolAssayCatalog = "narrative/needle_awl_hook_assays.json";

        public static readonly string[] SourceCatalogs =
        {
            DegreasingCatalog, SawingCatalog, PolishingCatalog, ToolAssayCatalog
        };

        public static bool IsSourceCatalog(string sourceCatalog)
        {
            string normalized = (sourceCatalog ?? string.Empty).Replace('\\', '/');
            for (int i = 0; i < SourceCatalogs.Length; i++)
            {
                if (string.Equals(normalized, SourceCatalogs[i], StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public static string GetFamily(string sourceCatalog)
        {
            string normalized = (sourceCatalog ?? string.Empty).Replace('\\', '/');
            if (string.Equals(normalized, DegreasingCatalog, StringComparison.OrdinalIgnoreCase))
                return "Bone & Horn · Bone Degreasing & Preparation";
            if (string.Equals(normalized, SawingCatalog, StringComparison.OrdinalIgnoreCase))
                return "Bone & Horn · Antler/Horn Sawing";
            if (string.Equals(normalized, PolishingCatalog, StringComparison.OrdinalIgnoreCase))
                return "Bone & Horn · Scraping & Polishing";
            if (string.Equals(normalized, ToolAssayCatalog, StringComparison.OrdinalIgnoreCase))
                return "Bone & Horn · Needle/Awl/Hook Assays";
            return string.Empty;
        }
    }

    /// <summary>
    /// Projects the four Plan 160 source schemas into the shared codex model.
    /// It exposes measurements as labelled authored observations and has no
    /// path to wildlife, inventory, crafting, item condition, combat, fishing,
    /// medical, trade or companion state.
    /// </summary>
    public sealed class BoneHornSourceAdapter : INarrativeSourceAdapter
    {
        public bool CanAdapt(string sourceCatalog) => BoneHornRuntimeContract.IsSourceCatalog(sourceCatalog);

        public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord)
        {
            string source = entry.SourceCatalog.Replace('\\', '/');
            string family = BoneHornRuntimeContract.GetFamily(source);
            string facility;
            string title;
            string technicalSummary;

            if (string.Equals(source, BoneHornRuntimeContract.DegreasingCatalog, StringComparison.OrdinalIgnoreCase))
            {
                string animal = NarrativeJsonHelpers.GetStringProp(sourceRecord, "bone_source_animal");
                string method = NarrativeJsonHelpers.GetStringProp(sourceRecord, "degreasing_method");
                int days = NarrativeJsonHelpers.GetIntProp(sourceRecord, "prep_duration_days");
                facility = "Historical animal label: " + animal;
                title = "Bone Preparation Log: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored process observation — source label: {animal}; degreasing method: {method}; preparation duration: {days} day(s). This is not a current carcass, companion or crafting timer.";
            }
            else if (string.Equals(source, BoneHornRuntimeContract.SawingCatalog, StringComparison.OrdinalIgnoreCase))
            {
                string material = NarrativeJsonHelpers.GetStringProp(sourceRecord, "material_type");
                string saw = NarrativeJsonHelpers.GetStringProp(sourceRecord, "saw_tool_id");
                string shape = NarrativeJsonHelpers.GetStringProp(sourceRecord, "blank_shape_cut");
                facility = "Historical saw label: " + saw;
                title = "Antler/Horn Sawing Record: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored material observation — material label: {material}; saw label: {saw}; blank shape: {shape}. Labels are provenance only and do not resolve to inventory.";
            }
            else if (string.Equals(source, BoneHornRuntimeContract.PolishingCatalog, StringComparison.OrdinalIgnoreCase))
            {
                string material = NarrativeJsonHelpers.GetStringProp(sourceRecord, "blank_material");
                string abrasive = NarrativeJsonHelpers.GetStringProp(sourceRecord, "abrasive_used");
                string finish = NarrativeJsonHelpers.GetStringProp(sourceRecord, "surface_finish");
                facility = "Historical abrasive label: " + abrasive;
                title = "Scraping & Polishing Report: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored finish observation — blank label: {material}; abrasive: {abrasive}; surface finish: {finish}. No abrasive or quality modifier is created.";
            }
            else
            {
                string tool = NarrativeJsonHelpers.GetStringProp(sourceRecord, "tool_type");
                string blank = NarrativeJsonHelpers.GetStringProp(sourceRecord, "bone_blank_id");
                float angle = NarrativeJsonHelpers.GetFloatProp(sourceRecord, "point_angle_degrees");
                facility = "Historical blank label: " + blank;
                title = "Needle/Awl/Hook Assay: " + FormatTitle(entry.SourceRecordId);
                technicalSummary = $"Authored geometry observation — tool label: {tool}; blank label: {blank}; point angle: {FormatNumber(angle)}°. This is not a damage, quality, fishing or repair effectiveness stat.";
            }

            return new NarrativeDiscoveredRecord
            {
                DiscoveryId = entry.DiscoveryId,
                KnowledgeKey = KnowledgeKeys.NarrativeDiscovered(entry.DiscoveryId),
                SourceCatalog = source,
                SourceRecordId = entry.SourceRecordId,
                Channel = entry.Channel,
                ProducerId = entry.ProducerId,
                ProducerIds = entry.ProducerIds.Count > 0 ? ToArray(entry.ProducerIds) : new[] { entry.ProducerId },
                MinDay = entry.MinDay,
                Title = title,
                Subtitle = $"{facility} · Authored chronology: source record has no timestamp field",
                BodyText = NarrativeJsonHelpers.GetStringProp(sourceRecord, "log_text"),
                Category = "Bone, Horn & Antler Material Culture",
                Tags = Array.Empty<string>(),
                TruthClass = string.IsNullOrEmpty(entry.TruthClass) ? "Historical Observation" : entry.TruthClass,
                ProvenanceLabel = string.IsNullOrEmpty(entry.ProvenanceLabel)
                    ? "Authored Vector-Block Tsadi craft record"
                    : entry.ProvenanceLabel,
                IdentityStatus = string.IsNullOrEmpty(entry.IdentityStatus)
                    ? "Animal/material/tool labels are historical; no live entity inferred"
                    : entry.IdentityStatus,
                NumericClaimLabel = string.IsNullOrEmpty(entry.NumericClaimLabel)
                    ? "Authored process observation — not a live item, recipe, quality, damage or wildlife value"
                    : entry.NumericClaimLabel,
                RecordFamily = family,
                FacilityOrStationLabel = facility,
                TechnicalSummary = technicalSummary,
                RelatedDiscoveryIds = entry.related_discovery_ids ?? Array.Empty<string>()
            };
        }

        private static string[] ToArray(IReadOnlyList<string> values)
        {
            var result = new string[values.Count];
            for (int i = 0; i < values.Count; i++) result[i] = values[i];
            return result;
        }

        private static string FormatNumber(float value) => value.ToString("0.###", CultureInfo.InvariantCulture);

        private static string FormatTitle(string recordId)
        {
            var parts = recordId.Split('_');
            var words = new List<string>();
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                    words.Add(char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1));
            }
            return string.Join(" ", words);
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
            RegisterAdapter(new ProcessLogSourceAdapter());
            RegisterAdapter(new BunkerGlitchSourceAdapter());
            RegisterAdapter(new BunkerBlueprintSourceAdapter());
            RegisterAdapter(new BunkerCourtSourceAdapter());
            RegisterAdapter(new WireConfessionSourceAdapter());
            RegisterAdapter(new TradeLedgerSourceAdapter());
            RegisterAdapter(new RegionalTreatySourceAdapter());
            RegisterAdapter(new SurgeonsCasebookSourceAdapter());
            RegisterAdapter(new DeadHandDirectiveSourceAdapter());
            RegisterAdapter(new CourierDispatchSourceAdapter());
            RegisterAdapter(new PersonalLetterSourceAdapter());
            RegisterAdapter(new AbyssalAnomaliesSourceAdapter());
            RegisterAdapter(new FringeCultSourceAdapter());
            RegisterAdapter(new PaperPrintSourceAdapter());
            RegisterAdapter(new BoneHornSourceAdapter());
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
            if (doc.RootElement.TryGetProperty("schema_version", out var schemaProp)
                && schemaProp.ValueKind == JsonValueKind.Number
                && schemaProp.TryGetInt32(out int schemaVersion)
                && schemaVersion != 1)
                return;
            if (!doc.RootElement.TryGetProperty("entries", out var entriesProp) || entriesProp.ValueKind != JsonValueKind.Array)
                return;

            var rawSourceCache = new Dictionary<string, JsonDocument>(StringComparer.Ordinal);

            try
            {
                var seenDiscoveryIds = new HashSet<string>(StringComparer.Ordinal);
                foreach (var entryElem in entriesProp.EnumerateArray())
                {
                    string discId = NarrativeJsonHelpers.GetStringProp(entryElem, "discovery_id");
                    string sourceCatalog = NarrativeJsonHelpers.GetStringProp(entryElem, "source_catalog");
                    string sourceRecordId = NarrativeJsonHelpers.GetStringProp(entryElem, "source_record_id");
                    string channel = NarrativeJsonHelpers.GetStringProp(entryElem, "channel");
                    string producerId = NarrativeJsonHelpers.GetStringProp(entryElem, "producer_id");
                    string truthClass = NarrativeJsonHelpers.GetStringProp(entryElem, "truth_class");
                    int minDay = 1;
                    if (entryElem.TryGetProperty("min_day", out var mdProp) && mdProp.TryGetInt32(out int mdVal))
                        minDay = mdVal;
                    if (string.IsNullOrEmpty(discId) || !seenDiscoveryIds.Add(discId) || minDay < 1)
                        continue;
                    if (FringeCultRuntimeContract.IsSourceCatalog(sourceCatalog)
                        && !string.IsNullOrEmpty(truthClass)
                        && !FringeCultRuntimeContract.IsValidTruthClass(truthClass))
                        continue;

                    var entry = new NarrativeDiscoveryManifestEntry
                    {
                        discovery_id = discId,
                        source_catalog = sourceCatalog,
                        source_record_id = sourceRecordId,
                        channel = channel,
                        producer_id = producerId,
                        min_day = minDay,
                        weight = NarrativeJsonHelpers.GetIntProp(entryElem, "weight", 1),
                        one_time = !entryElem.TryGetProperty("one_time", out var otProp) || otProp.ValueKind != JsonValueKind.False,
                        truth_class = truthClass,
                        provenance_label = NarrativeJsonHelpers.GetStringProp(entryElem, "provenance_label"),
                        identity_status = NarrativeJsonHelpers.GetStringProp(entryElem, "identity_status"),
                        numeric_claim_label = NarrativeJsonHelpers.GetStringProp(entryElem, "numeric_claim_label"),
                        related_discovery_ids = NarrativeJsonHelpers.GetStringArrayProp(entryElem, "related_discovery_ids")
                    };
                    string[] configuredProducers = NarrativeJsonHelpers.GetStringArrayProp(entryElem, "producer_ids");
                    var producers = new List<string>();
                    if (!string.IsNullOrEmpty(entry.producer_id))
                        producers.Add(entry.producer_id);
                    for (int producerIndex = 0; producerIndex < configuredProducers.Length; producerIndex++)
                    {
                        string configuredProducer = configuredProducers[producerIndex];
                        if (!string.IsNullOrEmpty(configuredProducer)
                            && !producers.Exists(p => string.Equals(p, configuredProducer, StringComparison.Ordinal)))
                            producers.Add(configuredProducer);
                    }
                    entry.producer_ids = producers.ToArray();

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

            ReindexDeterministically();
        }

        private void ReindexDeterministically()
        {
            _records.Sort((left, right) =>
            {
                int result = string.Compare(left.SourceCatalog, right.SourceCatalog, StringComparison.Ordinal);
                if (result != 0) return result;
                result = string.Compare(left.SourceRecordId, right.SourceRecordId, StringComparison.Ordinal);
                if (result != 0) return result;
                return string.Compare(left.DiscoveryId, right.DiscoveryId, StringComparison.Ordinal);
            });

            _byDiscoveryId.Clear();
            _byProducerId.Clear();
            _byChannel.Clear();
            for (int i = 0; i < _records.Count; i++)
            {
                var record = _records[i];
                if (_byDiscoveryId.ContainsKey(record.DiscoveryId)) continue;
                _byDiscoveryId.Add(record.DiscoveryId, record);
                if (record.ProducerIds != null && record.ProducerIds.Length > 0)
                {
                    for (int producerIndex = 0; producerIndex < record.ProducerIds.Length; producerIndex++)
                        AddToIndex(_byProducerId, record.ProducerIds[producerIndex], record);
                }
                else
                {
                    AddToIndex(_byProducerId, record.ProducerId, record);
                }
                AddToIndex(_byChannel, record.Channel, record);
            }
        }

        private static void AddToIndex(
            Dictionary<string, List<NarrativeDiscoveredRecord>> index,
            string key,
            NarrativeDiscoveredRecord record)
        {
            if (!index.TryGetValue(key, out var list))
            {
                list = new List<NarrativeDiscoveredRecord>();
                index.Add(key, list);
            }
            list.Add(record);
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
