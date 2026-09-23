# PLAN-HOST-EVENT-ARCHIVE-91 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, host-inclusive scan).
**Scaffolding authority:** [`PLAN-TELEMETRY-PRIVACY-58`](../EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-TELEMETRY-PRIVACY-58.md) — this plan adopts the authority's patterns (below) rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `TP-58A` | field catalogue: generated from the recorder/aggregator DTOs; every field classified. |
| `TP-58B` | consent + opt-out: first-run notice; toggle; off means no file created. |
| `TP-58C` | retention/deletion: bounded rotation (e.g. last N sessions), explicit delete; verified on disk. |

## 2. Source inventory (20 files, Core + host)

| File | Lines |
|---|---:|
| `Expeditions/ReconTelemetryCatalog.cs` | 83 |
| `Expeditions/ReconTelemetryCatalogLoader.cs` | 34 |
| `Expeditions/ReconTelemetryHeadlessDemo.cs` | 94 |
| `Expeditions/ReconTelemetryState.cs` | 57 |
| `Expeditions/ReconTelemetrySystem.cs` | 331 |
| `Journal/JournalCorpus.cs` | 348 |
| `Journal/JournalEntry.cs` | 34 |
| `Journal/JournalSystem.cs` | 484 |
| `Journal/JournalVoice.cs` | 59 |
| `Journal/JournalVoiceProseCatalog.cs` | 150 |
| `Journal/KnowledgeBase.cs` | 152 |
| `Journal/ProceduralEulogyEngine.cs` | 105 |
| `Journal/RiskBiasTrait.cs` | 43 |
| `OrbitalHarrowTelemetrySystem.cs` | 332 |
| `Telemetry/PlaySessionRecorder.cs` | 402 |
| `Telemetry/PlayableMetricsAggregationEngine.cs` | 186 |
| `host:Host/ReconTelemetryHostSession.cs` | 74 |
| `host:Host/ReconTelemetrySaveStore.cs` | 36 |
| `host:UI/AshfallMetricCard.cs` | 141 |
| `host:UI/ReconTelemetryPanel.cs` | 160 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `recon_telemetry_probes.json` | object(2 keys) |
| `orbital_kinetic_telemetry.json` | 8 |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupTelemetryHEScaffold` / `SaveTelemetryHEScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method; no new timer.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Telemetry/HE91ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class HE91ScaffoldTests
{
    [Fact] public void HEA91A_TODO() { /* taxonomy: enumerated fact ids with day/hour/subject fields; unknown facts are dr */ }
    [Fact] public void HEA91B_TODO() { /* writer: ring file per slot, rotation, deterministic ordering, flush on save. */ }
    [Fact] public void HEA91C_TODO() { /* independence guard: focused test toggling archive on/off and comparing checksums */ }
    [Fact] public void HEA91D_TODO() { /* dump verb + bundle integration under consent. */ }
    [Fact] public void HEA91E_TODO() { /* retention doc + header counters. */ }
    [Fact] public void HE91_AuthorityConformance_TODO() { /* pattern parity with PLAN-TELEMETRY-PRIVACY-58 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Telemetry/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win over new ones; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
