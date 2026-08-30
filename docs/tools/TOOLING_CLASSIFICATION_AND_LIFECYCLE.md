# ASHFALL — Non-Runtime Tooling Architecture, Classification, & Lifecycle

**Date:** 2026-08-27<br>
**Scope:** Governance, lifecycle policy, and complete registry of all non-runtime developer tools, CLI utilities, and build harnesses across ASHFALL.

---

## 1. Architectural Role of Non-Runtime Tools

In ASHFALL, non-runtime tools support four vital functions without polluting the runtime simulation:
1. **Headless Frame Rendering & UI Validation**: Standalone rendering harnesses that validate UI layout and assets without requiring a Godot render server.
2. **AI Agent & Skill Governance**: Validating, testing, and syncing agent capabilities across clients and models.
3. **Asset & Audio Pipelines**: Triage, conversion, loudness normalization, and quality assurance for 2D sprites, UI textures, and synthesized audio.
4. **Data & Narrative Forensics**: Graph reachability, schema validation, and catalog consistency checks.

---

## 2. In-Depth: Headless UI Preview Harness (`tools/ui-preview.csproj`)

- **Location:** [`tools/ui-preview.csproj`](../../tools/ui-preview.csproj) / [`tools/ui-preview.cs`](../../tools/ui-preview.cs)
- **Assembly:** `Ashfall.UiPreview`
- **SDK Target:** `net9.0`
- **Classification:** **`ACTIVE`**

### Key Capabilities
- **Engine-Agnostic Texture Loader**: Reads Godot PNG/RGBA assets directly without loading Godot assemblies.
- **Manifest Integrity**: Validates all textures specified in [`Assets/Ashfall.Core/UI/UiAssetManifest.cs`](../../Assets/Ashfall.Core/UI/UiAssetManifest.cs).
- **Deterministic Preview Generation**: Synthesizes fixed 1920×1080 frames demonstrating menu screens, HUD elements, status rails, and data grids.
- **Diffing & Golden Verification**: Outputs PNG screenshots that can be diffed against golden reference images in headless environments.

### Command Line Options
| Option | Default | Purpose |
|---|---|---|
| `--root <dir>` | Current directory | Root directory of the ASHFALL project repository. |
| `--out <dir>` | `tools/bin/ui-preview` | Output directory where rendered PNG frames and metadata are written. |

### Build & Run
```bash
dotnet build tools/ui-preview.csproj
dotnet run --project tools/ui-preview.csproj -- --root . --out snapshot-capture/ui_preview
```

---

## 3. Tooling Lifecycle Classifications

All tools and maintenance utilities are organized into three strict lifecycle tiers:

```
┌────────────────────────────────────────────────────────────────────────┐
│ ACTIVE                                                                 │
│ - Routinely executed in CI, test suites, or core developer loops.      │
│ - Required to compile with 0 warnings under Directory.Build.props.    │
│ - Examples: tools/ui-preview.csproj, agent-skill-manager, export tools │
└────────────────────────────────────────────────────────────────────────┘
                                    │
┌────────────────────────────────────────────────────────────────────────┐
│ MAINTENANCE-ONLY                                                       │
│ - Kept operational for occasional diagnostics, audits, or asset runs.  │
│ - Maintained to avoid bitrot, but not executed in standard CI gates.   │
│ - Examples: visual_asset_audit.py, audit_narrative_continuity.py       │
└────────────────────────────────────────────────────────────────────────┘
                                    │
┌────────────────────────────────────────────────────────────────────────┐
│ RETIRED                                                                │
│ - Historical one-off migration utilities and obsolete generators.      │
│ - Preserved exclusively for forensic audits and provenance tracking.   │
│ - Prohibited from being invoked in live workflows.                     │
│ - Examples: generate_2k_faction_emblems.py, generate_holdfast_json.py  │
└────────────────────────────────────────────────────────────────────────┘
```

---

## 4. Master Tool Registry

| Tool / Utility | Path | Status | Domain | Description |
|---|---|---|---|---|
| **Headless UI Preview** | [`tools/ui-preview.csproj`](../../tools/ui-preview.csproj) | `ACTIVE` | UI / Graphics | Standalone C# .NET 9 UI renderer and texture validator. |
| **Agent Skill Manager** | [`tools/agent-skill-manager/`](../../tools/agent-skill-manager/) | `ACTIVE` | Agent Systems | Rust CLI tool for validating and synchronizing AI agent skills. |
| **Stitch Asset Exporter** | [`tools/export_all_stitch_assets.py`](../../tools/export_all_stitch_assets.py) | `ACTIVE` | UI / Stitch MCP | Batch downloads and stages screen designs from Google Stitch MCP. |
| **Stitch Inventory Exporter** | [`tools/export_stitch_inventory.py`](../../tools/export_stitch_inventory.py) | `ACTIVE` | UI / Stitch MCP | Queries and lists current screen inventory from Stitch MCP. |
| **Narrative Continuity Auditor** | [`tools/audit_narrative_continuity.py`](../../tools/audit_narrative_continuity.py) | `MAINTENANCE-ONLY` | Narrative | Validates reachability across 199 narrative JSON graph files. |
| **Script Loop Auditor** | [`tools/audit_loops.sh`](../../tools/audit_loops.sh) | `MAINTENANCE-ONLY` | Shell CI | Static analyzer for shell script loop constructs. |
| **Transcript Exporter** | [`tools/export_transcript_to_desktop.py`](../../tools/export_transcript_to_desktop.py) | `MAINTENANCE-ONLY` | Operations | Extracts and formats agent JSONL conversation logs. |
| **Audio Synthesis Tool** | [`tools/generate_audio.py`](../../tools/generate_audio.py) | `MAINTENANCE-ONLY` | Audio | Generates diegetic audio streams from text prompts. |
| **ElevenLabs SFX Tool** | [`tools/generate_elevenlabs_sfx.py`](../../tools/generate_elevenlabs_sfx.py) | `MAINTENANCE-ONLY` | Audio | Selective SFX planning plus explicit reviewed-asset acceptance; direct API generation stays opt-in. |
| **Visual Asset Auditor** | [`tools/visual_asset_audit.py`](../../tools/visual_asset_audit.py) | `MAINTENANCE-ONLY` | Art Assets | Audits 2000+ textures for missing bindings and orphan assets. |
| **Visual Wiring Tracer** | [`tools/visual_wiring_trace.py`](../../tools/visual_wiring_trace.py) | `MAINTENANCE-ONLY` | UI / Assets | Traces asset references across Godot scenes and UI code. |
| **Visual Baseline Updater** | [`tools/visual_wiring_baseline.py`](../../tools/visual_wiring_baseline.py) | `MAINTENANCE-ONLY` | Art Assets | Computes asset usage baselines for regression detection. |
| **Pixel Stats Analyzer** | [`tools/visual_pixel_stats.py`](../../tools/visual_pixel_stats.py) | `MAINTENANCE-ONLY` | Art Assets | Evaluates texture resolutions, color bit-depth, and compression. |
| **Production Manifest Tool** | [`tools/production_manifest.py`](../../tools/production_manifest.py) | `MAINTENANCE-ONLY` | Art Assets | Compiles production art manifests for build staging. |
| **Production QA Tool** | [`tools/production_qa.py`](../../tools/production_qa.py) | `MAINTENANCE-ONLY` | Art Assets | Runs automated QA rules on generated textures. |
| **Production Promotion Tool** | [`tools/production_promote.py`](../../tools/production_promote.py) | `MAINTENANCE-ONLY` | Art Assets | Moves verified art assets into active game trees. |
| **Production Quarantine Tool** | [`tools/production_quarantine_plan.py`](../../tools/production_quarantine_plan.py) | `MAINTENANCE-ONLY` | Art Assets | Manages quarantine policies for deprecated art files. |
| **Production Ledger Tool** | [`tools/production_ledger.py`](../../tools/production_ledger.py) | `MAINTENANCE-ONLY` | Art Assets | Maintains immutable hash logs of all art transformations. |
| **Production HTML Gallery** | [`tools/production_gallery.py`](../../tools/production_gallery.py) | `MAINTENANCE-ONLY` | Art Assets | Generates responsive web visual review galleries. |
| **Prompt Composer Tool** | [`tools/production_prompt_composer.py`](../../tools/production_prompt_composer.py) | `MAINTENANCE-ONLY` | Content AI | Authors structured image prompts for external generative tools. |
| **Initial 2K Faction Emblems** | [`tools/generate_2k_faction_emblems.py`](../../tools/generate_2k_faction_emblems.py) | `RETIRED` | Historical Gen | One-off generator for initial faction emblems. |
| **50 Expansion Emblems** | [`tools/generate_50_more_2k_faction_emblems.py`](../../tools/generate_50_more_2k_faction_emblems.py) | `RETIRED` | Historical Gen | One-off generator for expansion faction emblems. |
| **Supplementary Emblems** | [`tools/generate_more_2k_faction_emblems.py`](../../tools/generate_more_2k_faction_emblems.py) | `RETIRED` | Historical Gen | One-off supplementary emblem generator. |
| **Deep Lore Emblems** | [`tools/generate_deep_lore_2k_emblems.py`](../../tools/generate_deep_lore_2k_emblems.py) | `RETIRED` | Historical Gen | One-off lore faction emblem generator. |
| **36 Expanded Emblems** | [`tools/generate_expanded_36_faction_emblems.py`](../../tools/generate_expanded_36_faction_emblems.py) | `RETIRED` | Historical Gen | One-off 36-faction emblem generator. |
| **Holdfast Data Generator** | [`tools/generate_holdfast_json.py`](../../tools/generate_holdfast_json.py) | `RETIRED` | Historical Gen | One-off generator for Holdfast expansion data. |
| **Item Text Merger** | [`tools/merge_item_text.py`](../../tools/merge_item_text.py) | `RETIRED` | Historical Migration | One-off migration script for merging item text batches. |
| **Replacement Queue Writer** | [`tools/write_replacement_queue.py`](../../tools/write_replacement_queue.py) | `RETIRED` | Historical Audit | Legacy script dumping asset replacement markdown. |
| **Legacy Visual Audit Writer** | [`tools/write_visual_audit.py`](../../tools/write_visual_audit.py) | `RETIRED` | Historical Audit | Legacy visual audit dumper (superseded by `visual_asset_audit.py`). |
