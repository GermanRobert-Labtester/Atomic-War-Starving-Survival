# Initiative #42 — Single Versioned Atomic Campaign Envelope

**Goal (2 lines):** Make the per-slot `campaign.json` envelope the single authoritative save — captured in memory, written once atomically — replacing the ~61-file write-then-repack pipeline. Old saves (V1 slot envelopes + pre-slot global files) migrate automatically on load.

**Decisions taken (user did not answer; my recommendation, reversible):** ① capture-based single-write save, load still explodes sections to slot files so the 26 `SetupXxx` flows are untouched; ② auto-migrate legacy saves on first load (corrupt legacy sections are skipped with a warning, not fatal); ③ envelope `manifestVersion` bumps to **V2** (registry-keyed sections, real schemaVersions) with a V1-acceptance ladder.

## 1. Current Reality (verified firsthand + 3 audits)

- The envelope **already exists** (`saves/profile-X/slot-Y/campaign.json`): atomic write (temp+validate+.bak+rename), per-section + aggregate SHA-256, validation, `.corrupt` quarantine, iron-man policy (`SaveSlotService`). For slot loads it is already authoritative.
- But it is **derived, not primary**: `SaveAll()` (`src/Main.SaveOrchestrator.cs:274-321`) writes ~61 individual files, then `PackAggregateEnvelope` (`src/Host/SaveLoadHostSession.cs:288-346`) re-reads whatever `*.json` sits in the slot root. A failed mid-sequence write packs stale content for that section beside fresh content for others → **mixed-generation envelope** (the cross-system partial-save problem). Stray files (e.g. `weather_save.json`, `holdfast_flavor.json`) get packed as bogus sections.
- Load: validate envelope → **explode sections back to slot files** (`TryLoadSlot:377-400`) → `RestoreAllSubsystemsFromDisk()` reads files via 26 `SetupXxx`. Section names in the envelope are **filenames** (`inventory_save`, `holdfast_s1_save`), not registry keys; `schemaVersion` hardcoded 1; `manifestVersion` only ever 1, `!= 1` hard-rejected (no ladder); manifest not refreshed on save.
- Registry drift: 60 `SaveSectionRegistry` keys vs real writers — `SaveMoralChoice` unregistered (writes `moral_choice_save.json`; triad gate carries a special alias for it); weather persisted twice (redundant stray `weather_save.json` + canonical world section); `ResetAllSessions`/`ResetExpandedShelterSessions` hardcode delete lists that miss 12+ files.
- No-slot Continue path loads global legacy files directly; `TryImportLegacySave` only wraps a single file as a blob section.
- `ICampaignSaveSection` (direct-capture contract) is defined but has zero implementors. Inititative #41 left every store with the right capture primitives (`TryCapture*`/codec flavors) — this initiative wires them into the envelope.

## 2. Proposed Architecture

### Core (engine-agnostic)
1. **Registry enrichment** (`SaveSectionRegistry`): each entry gains `FileName` (e.g. `"journal_save.json"`) and `int SchemaVersion` (codec `CurrentSaveVersion` where one exists: holdfast 5, year_of_ash 4, dose_ledger 2, expansion_hub 4; default 1). New entry `moral_choice -> SaveMoralChoice` (removes the triad-gate alias). Derived surfaces: filename→key map (migration + whitelist), registry-driven delete list. Weather stays **out** of the registry (world section is canonical).
2. **`SaveStore<T>.CapturePersisted(state)`** — returns *exactly* the bytes `TrySave` would write (checksummed flavor → envelope JSON; codec flavor → codec JSON). Pinned byte-equal by test.
3. **`CampaignEnvelopeBuilder`** — builds a V2 `AggregateSaveEnvelope` from a `key→payload` map: registry order, real schemaVersions, refreshed manifest, section+aggregate checksums. Rejects unknown keys (whitelist).
4. **`SaveSlotService` ladder**: `ValidateAggregate` accepts `manifestVersion` 1 or 2; `MigrateV1ToV2(envelope)` renames filename-sections to registry keys via the derived map, drops unknown sections with a logged warning, stamps V2. `CurrentEnvelopeVersion = 2`.

### Host
5. **Envelope-primary save**: every `SaveXxx` method becomes *capture into a payload map* (`_sectionPayloads[key] = Store.CapturePersisted(session.CaptureSave())`) instead of a file write; dirty flags cleared on capture. `SaveAll()` hands the map to `SaveLoadHostSession.SaveEnvelopeFromPayloads()` → builder → manifest refresh → one `WriteAggregateAtomically`. **A failed capture aborts the whole save** (previous envelope intact); a null session means the section is legitimately absent. `SaveAllDirect`/`PackAggregateEnvelope` (file-scan) are replaced; `FlushDirtyStoresForDayAdvance` triggers a full `SaveAll` (in-memory captures are cheaper than the old file writes).
6. **Load unchanged mechanically**: validate → (V1? migrate in memory) → explode V2 sections to slot files → `SetupXxx`. Files become a load-time cache, no longer written at save time. Section files remain readable/writable via stores (selftests, debugging).
7. **Legacy migration**: `ContinueGame` with no slots → `MigrateLegacyGlobalSaves()` packs registry-whitelisted global `user://` section files **verbatim** (payloads are already per-section file bytes — zero translation) into a fresh `migrated_N` slot; per-section skip-and-warn for corrupt files; originals left in place. The direct-file restore fallthrough is removed. Single-file `ImportLegacySave` UI path stays.
8. **Hygiene**: `ResetAllSessions`/`ResetExpandedShelterSessions` delete lists derived from registry FileNames (+`.bak`); `ClearContinuableSaves` also removes the active slot's `campaign.json`(+`.bak`); `WeatherHostSession` stops writing the redundant `weather_save.json` (world section restores weather — verified `WorldHostSave.State` is `WorldWeatherState`); `holdfast_flavor.json` remains a non-save catalog cache, excluded from the envelope by the whitelist.

## 3. Test Strategy

New Core tests: V1→V2 migration (filename-keyed → registry keys, unknown dropped); builder ordering/schemaVersion/checksums round-trip through `ValidateAggregate`; ladder acceptance (1 migrates, 2 passes, 3 rejected); `CapturePersisted` byte-identity to `TrySave` file content per flavor. Updated deliberately (contract evolution, not weakening): `SaveSlotServiceTests`/`SaveAggregateContractTests` (manifestVersion==1 pins), `SaveSectionRegistryTests` (new fields), triad-gate script regex (new registry fields + `moral_choice` entry replaces alias), `SaveLoadUiFailureSelfTest` extended with V2 + migration gates. Unmodified suites must keep passing: store sweeps/seals/wire, `SaveStoreServiceTests`, slot/failure-path tests.

## 4. Dependency-Ordered Phases (each = commit + full verification)

- **Phase 0** — Baseline `verify-fast` green + plan doc `docs/plans/campaign_envelope_INTEGRATION_PLAN.md`.
- **Phase 1** — Core: registry enrichment (+`moral_choice`), `CapturePersisted`, `CampaignEnvelopeBuilder`, `SaveSlotService` V2 ladder + `MigrateV1ToV2`. Touches `SaveSectionRegistry.cs`, `SaveStore.cs`, `SaveSlotService.cs`, new builder file.
- **Phase 2** — Core tests (above) green; existing suites updated for the ladder only where they pin `manifestVersion==1`.
- **Phase 3** — Host save path: payload-map `SaveXxx` conversions (mechanical, ~61 methods across Main partials), `SaveEnvelopeFromPayloads`, manifest refresh, flush→SaveAll, registry-derived reset lists, weather stray-write removal, `SaveLoadUiFailureSelfTest` extension. Host build 0/0.
- **Phase 4** — Legacy: `MigrateLegacyGlobalSaves` + `ContinueGame` rewiring; V1-slot acceptance (migrate-on-load, rewrite V2 on next save).
- **Phase 5** — Docs (AGENTS.md Save/Load section, REPO_REVIEW_REPORT, matrix regen if needed), full `verify-fast`, cross-tool review handoff (different AI client, diff + spec only, per AGENTS.md QA rule).

## 5. File Impact Map

CREATE: `Assets/Ashfall.Core/Save/CampaignEnvelopeBuilder.cs`, `docs/plans/campaign_envelope_INTEGRATION_PLAN.md`, Core test file(s). MODIFY: `SaveSectionRegistry.cs` (+2 fields, +1 entry), `SaveSlotService.cs` (ladder+migration), `SaveStore.cs` (CapturePersisted), `SaveLoadHostSession.cs` (SaveEnvelopeFromPayloads, load migration, legacy pack), `Main.SaveOrchestrator.cs` + Main partials (SaveXxx bodies, reset lists, ContinueGame), `WeatherHostSession.cs`, `SaveSectionRegistryTests.cs`, `SaveSlotServiceTests.cs`, `SaveAggregateContractTests.cs`, `SaveLoadUiFailureSelfTest.cs`, `scripts/ci/triad-drift-gate.sh`, AGENTS.md, REPO_REVIEW_REPORT.md. READ ONLY: `SaveChecksum`, stores (public APIs), `CampaignSaveEnvelope.cs` shapes (field additions avoided — V2 reuses existing fields).

## 6. Risks & Mitigations

- **61-method touch surface** — mechanical, compiler-checked, signatures unchanged (triad gate pins existence, not bodies); per-batch commit + build.
- **Coherence policy** — capture failure aborts the save (coherent-or-nothing); null sessions = absent sections (legitimate). Mixed-generation envelopes become impossible.
- **V1/legacy regression** — ladder + verbatim-payload migration mean no byte translation of section payloads; originals never deleted by migration; per-section skip-and-warn bounds corruption blast radius.
- **Pinned tests** — updated deliberately in the same commits as the contract change; everything else must pass unmodified.
- **Concurrent-session hazard** (live in this repo): stage only initiative files; commit immediately after each green verification.
- **Determinism** — no new RNG/IDs/timestamps beyond the existing manifest tick (unchanged semantics).

## 7. Out of Scope

In-memory restore without file explosion (S3 — future initiative; `ICampaignSaveSection` stays as its seam); `settings.json`/`audio_settings.json` (app settings, not campaign state); `chronicleSummary` population; multi-generation `.bak` retention; envelope-level compression/encryption; deleting the legacy globals after successful migration.

## 8. Rollback

One commit per phase; until Phase 3 lands, the file pipeline still works, so Phase 1-2 reverts are inert. After Phase 3, a revert restores file writes; exploded section files on disk mean any V2-era slot still loads under the old code path via its V1-compatible section files. Migration never mutates or deletes legacy inputs.

## 9. Definition of Done

`SaveAll` performs exactly one disk write (`campaign.json`, atomic); no section files written at save time; envelope sections are registry-keyed with real schemaVersions under `manifestVersion 2`; V1 envelopes and pre-slot global saves load via automatic migration; reset/clear lists registry-derived; all suites + selftests + `verify-fast` green; cross-tool review requested.

## Implementation Handoff

- **MUST PRESERVE:** section payload bytes (verbatim from today's file formats — `CapturePersisted` byte-identity); `SetupXxx` load mechanics (explode-then-setup); `SaveXxx` public signatures + registry/triad contract; `SaveLoadResult` statuses + quarantine behavior; iron-man policies; per-store selftests.
- **MUST ADD:** `CapturePersisted`; `CampaignEnvelopeBuilder`; V2 + ladder + `MigrateV1ToV2`; registry `FileName`/`SchemaVersion`/`moral_choice`; `SaveEnvelopeFromPayloads`; legacy global auto-migration; registry-derived reset lists; new/extended tests.
- **MUST NOT DO:** change any state DTO or codec; weaken checksum/quarantine guarantees; delete legacy files during migration; touch the concurrent UI/CI session's files; write envelope sections from anything but the capture map.
- **VERIFY WITH:** canonical 5-step checklist + `verify-fast` + save selftests per phase; PASS/FAIL reported before claiming done.
- **FIRST SAFE STEP:** Phase 0 baseline, then Phase 1 Core (registry enrichment + builder + ladder) with tests — before any host behavior changes.