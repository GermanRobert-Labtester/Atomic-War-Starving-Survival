# ASHFALL — Non-Runtime Tools & Utilities Catalog

**Date:** 2026-08-27<br>
**Authority:** Non-runtime tooling registry and lifecycle governance.

This directory contains standalone developer utilities, build tools, visual auditing scripts, and content generation pipelines for ASHFALL.

---

## 1. Highlight: Headless UI Preview Harness (`tools/ui-preview.csproj`)

- **Path:** [`tools/ui-preview.csproj`](ui-preview.csproj) / [`tools/ui-preview.cs`](ui-preview.cs)
- **Assembly:** `Ashfall.UiPreview`
- **Target Framework:** `net9.0` (with shared `Directory.Build.props` / `Directory.Packages.props`)
- **Classification:** **`ACTIVE`**

### Purpose & Architecture
`HeadlessUiPreview` is a standalone C# CLI harness that executes completely independent of the Godot engine runtime and render servers. It:
1. Loads raw textures directly from project assets (`assets/art/`, `assets/ui/`, `assets/sprites/`).
2. Validates UI asset manifests defined in [`Assets/Ashfall.Core/UI/UiAssetManifest.cs`](../Assets/Ashfall.Core/UI/UiAssetManifest.cs).
3. Synthesizes deterministic 1920×1080 RGBA UI preview frames (e.g. main menu variants, status rails, data grids).
4. Emits standalone PNG screenshots and visual metadata for CI verification and UI design validation without launching a display server.

### Build & Execution
```bash
# Build standalone preview tool
dotnet build tools/ui-preview.csproj

# Run UI preview frame generator
dotnet run --project tools/ui-preview.csproj -- --root . --out snapshots/headless_preview
```

---

## 2. Complete Tooling Classification & Lifecycle Matrix

Every tool in `tools/` and related maintenance directories is classified under one of three lifecycle states:
- **`ACTIVE`**: Actively supported, routinely executed in CI or local developer workflows.
- **`MAINTENANCE-ONLY`**: Kept operational for occasional diagnostics, audits, or asset re-generation.
- **`RETIRED`**: Historical one-off migration utilities or superseded legacy scripts preserved for provenance.

| Tool / Script | Status | Language / Tech | Primary Domain | Purpose & Operating Notes |
|---|---|---|---|---|
| [`ui-preview.csproj`](ui-preview.csproj) | **`ACTIVE`** | C# / .NET 9 | UI / Rendering | Standalone headless UI frame renderer and asset validator. |
| [`agent-skill-manager/`](agent-skill-manager/) | **`ACTIVE`** | Rust / Cargo | Agent System | CLI tool for validating, auditing, and syncing agent skill definitions. |
| [`export_all_stitch_assets.py`](export_all_stitch_assets.py) | **`ACTIVE`** | Python 3 | Stitch UI / UX | Batch exports screen designs and variants from Google Stitch MCP. |
| [`export_stitch_inventory.py`](export_stitch_inventory.py) | **`ACTIVE`** | Python 3 | Stitch UI / UX | Exports screen inventory from Google Stitch MCP workspace. |
| [`audit_narrative_continuity.py`](audit_narrative_continuity.py) | **`MAINTENANCE-ONLY`** | Python 3 | Narrative QA | Cross-file graph auditor for quests, encounters, echoes, and flag triggers. |
| [`audit_loops.sh`](audit_loops.sh) | **`MAINTENANCE-ONLY`** | Bash | Shell Hygiene | Scans bash scripts for infinite execution loops or unbound recursion. |
| [`export_transcript_to_desktop.py`](export_transcript_to_desktop.py) | **`MAINTENANCE-ONLY`** | Python 3 | Dev Operations | Exports structured JSONL agent conversation transcript logs. |
| [`generate_audio.py`](generate_audio.py) | **`MAINTENANCE-ONLY`** | Python 3 | Audio Pipeline | Generates game audio assets via text-to-speech / audio synthesis. |
| [`generate_elevenlabs_sfx.py`](generate_elevenlabs_sfx.py) | **`MAINTENANCE-ONLY`** | Python 3 | Audio Pipeline | Batch synthesizes sound effects using ElevenLabs API. |
| [`visual_asset_audit.py`](visual_asset_audit.py) | **`MAINTENANCE-ONLY`** | Python 3 | Visual Assets | Forensic auditor for visual assets, missing textures, and sprite bindings. |
| [`visual_wiring_trace.py`](visual_wiring_trace.py) | **`MAINTENANCE-ONLY`** | Python 3 | Visual Assets | Traces asset references across Godot `.tscn`, `.tres`, and C# UI scripts. |
| [`visual_wiring_baseline.py`](visual_wiring_baseline.py) | **`MAINTENANCE-ONLY`** | Python 3 | Visual Assets | Computes and updates baseline asset usage counts. |
| [`visual_pixel_stats.py`](visual_pixel_stats.py) | **`MAINTENANCE-ONLY`** | Python 3 | Visual Assets | Analyzes pixel dimensions, color space, and compression for UI textures. |
| [`production_manifest.py`](production_manifest.py) | **`MAINTENANCE-ONLY`** | Python 3 | Asset Pipeline | Generates and manages the production art manifest. |
| [`production_qa.py`](production_qa.py) | **`MAINTENANCE-ONLY`** | Python 3 | Asset Pipeline | Automated quality assurance rules for production art assets. |
| [`production_promote.py`](production_promote.py) | **`MAINTENANCE-ONLY`** | Python 3 | Asset Pipeline | Promotes approved art assets from staging to live asset paths. |
| [`production_quarantine_plan.py`](production_quarantine_plan.py) | **`MAINTENANCE-ONLY`** | Python 3 | Asset Pipeline | Formulates asset quarantine and retirement migration plans. |
| [`production_ledger.py`](production_ledger.py) | **`MAINTENANCE-ONLY`** | Python 3 | Asset Pipeline | Tracks production asset version history and ledger records. |
| [`production_gallery.py`](production_gallery.py) | **`MAINTENANCE-ONLY`** | Python 3 | Asset Pipeline | Builds interactive HTML galleries for inspecting generated textures. |
| [`production_prompt_composer.py`](production_prompt_composer.py) | **`MAINTENANCE-ONLY`** | Python 3 | Content AI | Generates structured text prompts for image/emblem synthesis models. |
| [`generate_2k_faction_emblems.py`](generate_2k_faction_emblems.py) | **`RETIRED`** | Python 3 | Historical Asset Gen | Batch generator for initial 2K faction emblem set (run once). |
| [`generate_50_more_2k_faction_emblems.py`](generate_50_more_2k_faction_emblems.py) | **`RETIRED`** | Python 3 | Historical Asset Gen | Batch generator for expansion faction emblems (run once). |
| [`generate_more_2k_faction_emblems.py`](generate_more_2k_faction_emblems.py) | **`RETIRED`** | Python 3 | Historical Asset Gen | Historical batch generator for supplementary faction emblems. |
| [`generate_deep_lore_2k_emblems.py`](generate_deep_lore_2k_emblems.py) | **`RETIRED`** | Python 3 | Historical Asset Gen | Historical batch generator for lore factions. |
| [`generate_expanded_36_faction_emblems.py`](generate_expanded_36_faction_emblems.py) | **`RETIRED`** | Python 3 | Historical Asset Gen | Historical batch generator for expanded 36-faction set. |
| [`generate_holdfast_json.py`](generate_holdfast_json.py) | **`RETIRED`** | Python 3 | Historical Data Gen | Historical generator used during initial Holdfast expansion data authoring. |
| [`merge_item_text.py`](merge_item_text.py) | **`RETIRED`** | Python 3 | Historical Migration | One-off migration tool for item descriptions into items.json. |
| [`write_replacement_queue.py`](write_replacement_queue.py) | **`RETIRED`** | Python 3 | Historical Audit | Legacy script for dumping asset replacement queue markdown. |
| [`write_visual_audit.py`](write_visual_audit.py) | **`RETIRED`** | Python 3 | Historical Audit | Legacy visual audit dump generator (superseded by `visual_asset_audit.py`). |

---

## 3. Tool Maintenance Guidelines

1. **Active Tools (`ACTIVE`)**:
   - Must build cleanly under `Directory.Build.props` / `Directory.Packages.props`.
   - Must have zero compiler warnings (`warning-baseline-gate.sh`).
2. **Maintenance-Only Tools (`MAINTENANCE-ONLY`)**:
   - Kept functional for ongoing diagnostics.
   - Do not run writes against production assets without a clean git working tree.
3. **Retired Tools (`RETIRED`)**:
   - Kept strictly for provenance and audit history. Do not invoke in CI or runtime pipelines.
