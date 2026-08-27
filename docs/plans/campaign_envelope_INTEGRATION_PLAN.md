# Initiative #42 — Single Versioned Atomic Campaign Envelope

Date: 2026-08-27 · Status: approved & in execution · Plan preserved verbatim below.

## Execution log

- Phase 0: baseline `verify-fast` 17/17 PASS.
- Phase 1: `SaveSectionRegistry` gains `SectionFileNames` (61 entries — every
  registry key incl. new `moral_choice`; weather deliberately absent, world
  section is canonical), `SchemaVersions` (holdfast 5, year_of_ash 4,
  dose_ledger 2, expansion_hub 4), `FileNameFor`/`SchemaVersionFor`/
  `TryGetKeyForSectionName` (V1 filename→key reverse map);
  `SaveStore<T>.CapturePersisted` (bytes identical to TrySave output);
  `CampaignEnvelopeBuilder` (V2, registry-ordered, whitelisted, pure);
  `SaveSlotService` version ladder (accepts 1|current, rejects future) +
  `MigrateToCurrent` (V1→V2 in-memory rename, reserved `legacy` import section
  preserved verbatim, unknown sections dropped with warning) wired into
  `TryLoadAggregate`.

## Approved plan (verbatim)

**Goal:** Make the per-slot `campaign.json` envelope the single authoritative
save — captured in memory, written once atomically — replacing the ~61-file
write-then-repack pipeline. Old saves (V1 slot envelopes + pre-slot global
files) migrate automatically on load.

**Decisions:** ① capture-based single-write save, load still explodes sections
to slot files so the 26 `SetupXxx` flows are untouched; ② auto-migrate legacy
saves on first load (corrupt legacy sections skipped with a warning);
③ `manifestVersion` bumps to V2 (registry-keyed sections, real schemaVersions)
with a V1-acceptance ladder.

**Key mechanics:**
- Save: each `SaveXxx` captures `store.CapturePersisted(session.CaptureSave())`
  into a payload map; `SaveAll` → `CampaignEnvelopeBuilder.Build` → manifest
  refresh → one `WriteAggregateAtomically`. Capture failure aborts the whole
  save; null sessions mean legitimately absent sections.
- Load: validate → V1 migrates in memory → explode sections to their
  **registry FileName** (`journal_save.json`, not `journal.json`) → SetupXxx.
- Legacy: Continue with no slots packs global files verbatim into a
  `migrated_N` slot; single-file import path unchanged (`legacy` reserved
  section).
- Hygiene: registry-derived reset lists; weather stray write removed;
  `SaveAllDirect`/`PackAggregateEnvelope` replaced by `SaveEnvelopeFromPayloads`.

**MUST PRESERVE:** payload bytes (CapturePersisted byte-identity); SetupXxx
load mechanics; SaveXxx signatures + triad contract; SaveLoadResult statuses +
quarantine; iron-man policies; per-store selftests.
**MUST NOT DO:** change state DTOs/codecs; weaken checksum/quarantine; delete
legacy files during migration; touch the concurrent session's files.
**VERIFY WITH:** canonical 5-step checklist + verify-fast + save selftests per
phase.
