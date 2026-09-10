# Plan 146 Baseline — Bunker Court Records Runtime Activation & Institutional Consequence Projection

**Document ID:** ARCH-PLAN146-BASELINE
**Status:** Approved Architectural Baseline
**Project:** ASHFALL (Godot 4.7+ .NET 8 Host / C# Core)
**Date:** 2026-09-09

---

## 1. Executive Summary

Plan 146 activates the existing 24-record bunker tribunal corpus (`Assets/StreamingAssets/Data/narrative/bunker_court_verdicts_codex.json`) as discoverable, queryable institutional history within ASHFALL. This corpus chronicles a decade of communal justice, emergency decrees, minor infractions, and constitutional transitions within the shelter during the post-exchange era (spanning Day 84 to Day 3650).

While `BunkerCourtCatalog` and a partial adapter in `NarrativeDiscoveryCatalog` existed previously, only cases 1–4 were registered in `narrative_discovery_manifest.json`, the loader suffered from an enumeration duplication defect on repeated `Load()` calls, and no dedicated host accessor existed. Plan 146 hardens the Core loader, completes the discovery registry for all 24 cases, links the catalog in the content utilization baseline, and guarantees zero unintended gameplay side-effects.

---

## 2. Source Data Authority

- **File Path:** `Assets/StreamingAssets/Data/narrative/bunker_court_verdicts_codex.json`
- **Schema Version:** 1
- **Collection ID:** `the_24_bunker_popular_tribunal_court_records_and_decrees`
- **Total Cases:** 24 authored cases
- **Timeline Coverage:** Day 84 (`TRIB-084-MOONSHINE`) through Day 3650 (`TRIB-3650-CONSTITUTION`)
- **Key Entity Fields:**
  - `case_id` (string): Stable content identifier (e.g. `case_01_the_air_duct_moonshine_still`).
  - `docket_number` (string): Official tribunal docket number encoding trial day (e.g. `TRIB-084-MOONSHINE`).
  - `defendant_name` (string): In-world defendant name with role or title.
  - `presiding_magistrate` (string): In-world magistrate(s) or tribunal body.
  - `charge_summary` (string): Detailed summary of alleged violation or petition.
  - `evidence_presented` (string): Physical evidence and witness testimony.
  - `verdict_outcome` (string): Formal tribunal ruling.
  - `disciplinary_penalty` (string): Prescribed sentence, chore assignment, or resolution.
  - `clerk_margin_notes` (string): Color commentary and contextual observation by the tribunal clerk.
  - `tags` (string[]): Thematic and character keywords.

---

## 3. Forensic Defect Analysis

1. **Loader Idempotency Defect:**
   - In `BunkerCourtCatalog.cs`, `Load()` iterated through `file.cases`, writing `_byId[c.case_id] = c;` and `_allCases.Add(c);`.
   - Repeated calls to `Load()` duplicated all entries in `_allCases` (24 -> 48 -> 72), while `_byId` silently overwrote entries.
   - *Remediation:* Guard `_byId.ContainsKey(c.case_id)`, provide explicit `Clear()` method, and add `LoadFromDirectory`.

2. **Partial Manifest Registration:**
   - In `Assets/StreamingAssets/Data/narrative_discovery_manifest.json`, only cases 1 through 4 were registered (`disc_court_moonshine_still`, `disc_court_chore_chit_forgery`, `disc_court_accordion_recital`, `disc_court_smuggled_calico_cat`). Cases 5 through 24 remained undiscoverable via the runtime narrative discovery system.
   - *Remediation:* Register cases 5 through 24 with canonical discovery IDs, graded `min_day` unlock thresholds, and the established `library_terminal` channel with `government_bunker` producer.

3. **Content Utilization Scanner Absence:**
   - `bunker_court_verdicts_codex.json` was classified as `CODEX_ONLY` with empty loader and consumer mappings in `ContentUtilizationScanner.cs`.
   - *Remediation:* Add `IsPlan146CourtFile` recognition, map loader to `BunkerCourtCatalog`, registry to `BunkerCourtCatalog`, and consumers to `NarrativeDiscoveryCatalog` and `JournalPanel`.

4. **Host Accessor Seam:**
   - No direct host getter existed on `Main` for querying tribunal records directly by docket, magistrate, or defendant.
   - *Remediation:* Expose `GetBunkerCourtCatalog()` in `Main.ShelterInfrastructure.cs` following the pattern of `GetBunkerGraffitiCatalog()`.

---

## 4. Architectural Boundaries

- **Zero Gameplay Side-Effects:** Reading a verdict, fine, or disciplinary penalty does NOT mutate live resources, morale, survivor health, detention states, or faction reputations.
- **Independence from Live Justice Engines:** This historical tribunal corpus is strictly decoupled from live gameplay justice mechanics (such as Expansion 08 `THE VERDICT` or `JusticeSystem`).
- **Deterministic Presentation:** Cases are sorted deterministically by trial day (parsed from `docket_number`), then by `case_id` ordinal.
