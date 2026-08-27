# Initiative #41 — Generic Injected Persistence Service for Save Stores

Date: 2026-08-27 · Status: approved (user decisions locked) · Implementer: ZCode

## 1. Objective

Replace the duplicated per-store persistence logic (checksum envelope, path
resolution, file IO, error handling, backup) inside all 51 static save stores
(`src/**/\*SaveStore*.cs`) with one generic, port-injected `SaveStore<T>`
service. On-disk save format stays byte-identical. The only deliberate
behavior change is a hardened (atomic) write path.

User decisions:

1. **Thin static façades** — keep every store class name, `FileName`/
   `SectionName` consts, and public static signatures; bodies become one-line
   delegations. ~254 static call sites untouched.
2. **Atomic write + optional `.bak`** in the service.
3. **All 51 stores**, migrated in separately committed, fully verified batches.

## 2. Current Reality

- **51 production stores** (53 `*SaveStore*.cs` files minus 2 self-tests):
  - **Variant A — 38 files (~4,750 lines)**: hand-rolled
    `{ State, Checksum }` envelope + legacy bare-state fallback + private
    `FileSystemIO`/`SystemTextJsonSerializer`/`GodotLog` + try/catch logging.
    Canonical example `src/Host/WeatherSaveStore.cs`.
  - **Variant B — 13 files (~1,240 lines)**: IO shell delegating to Core
    codecs (`HoldfastSaveCodec`, `YearOfAshSaveCodec`, `DoseLedgerSaveCodec`,
    …); checksum/versioning/migration live in Core.
- **Extension seam already exists** (discovered during planning):
  `Assets/Ashfall.Core/Save/SaveEnvelopeHelper.cs` — generic
  `SaveEnvelope<T> { State, Checksum }`, `TrySaveAtomic` (temp+rename,
  optional `.bak`), `TryLoad` (checksum verify + legacy fallbacks),
  `CaptureEnvelope`/`RestoreEnvelope`. One store
  (`src/Host/EncounterChoiceSaveStore.cs`) already delegates to it;
  `SaveStoreCoverageGateTests` already recognizes `SaveEnvelopeHelper`
  delegation; `Ashfall.Core.Tests/Save/SaveEnvelopeHelperTests.cs` covers it.
- Shared infra: `SaveSlotRoot` (`src/Host/SaveSlotRoot.cs:54-69`, per-call
  base-dir resolution), `FileSystemIO`/`SystemTextJsonSerializer`
  (`HostDefaults.cs:14-73`; options `IncludeFields=true,
  PropertyNameCaseInsensitive=true`, no naming policy ⇒ PascalCase
  `{"State":…,"Checksum":…}`), `SaveChecksum.Compute` (state-object
  reflection hash; root `Checksum` field skipped).
- Only `HoldfastTradeSaveStore` has backup logic (`.bak`).
- DI precedent: `SaveSlotService` (ctor-injected ports + atomic write).
- ~254 static store call sites across Main partials + host sessions.

## 3. Required Delta

An injected, generic persistence service owning checksum/backup/path/
serializer logic once, with all 51 stores reduced to thin façades over it.
Smallest change achieving it: extend the existing helper seam with an
instance-based, path-aware, log-tagged wrapper; migrate stores in batches.

## 4. Evidence

See §2; file:line references verified during planning (WeatherSaveStore,
SaveSlotRoot, SaveStoreCoverageGateTests, SaveSlotService, SaveChecksum,
HostDefaults read in full; store inventory enumerated via
`find src -name "*SaveStore*.cs"`).

## 5. Existing Extension Seams

- `SaveEnvelopeHelper` / `SaveEnvelope<T>` (Core) — envelope + atomic write +
  legacy load; **extended, not duplicated**.
- `SaveSlotRoot` — per-call path resolution; injected as
  `Func<string>` provider by the host hub.
- `SaveStoreCoverageGateTests` / `SaveStoreChecksumSelfTest` Gate A /
  `generate-save-store-matrix.py` — token-based source gates; already
  accept helper/codec delegation; will accept hub delegation.

## 6. Proposed Architecture

- **Core** `Assets/Ashfall.Core/Save/SaveStore.cs` (CREATE):
  `public sealed class SaveStore<T> where T : class`, ctor
  `(fileName, IFileIO, IJsonSerializer, ILog, Func<string> baseDirProvider,
  logTag, createBackup)`. Members: `SavePath` (provider evaluated per
  access), `Exists()`, `TrySave(T, pathOverride=null)`,
  `TryLoad(pathOverride=null) → T?`, `CaptureBare/RestoreBare` (aggregate
  path — most stores capture bare state), `CaptureEnvelope/RestoreEnvelope`
  (delegate to `SaveEnvelopeHelper`), static `FromCodec(fileName, encode,
  decode, …)` for Variant B. Envelope logic delegates to
  `SaveEnvelopeHelper` with the store's own `logTag`.
- **Host** `src/Host/SaveStoreHub.cs` (CREATE): constructs `SaveStore<T>`
  with `FileSystemIO`/`SystemTextJsonSerializer`/`GodotLog` +
  `SaveSlotRoot.ResolveBaseDirectory`. Only Godot-aware piece.
- **Façades**: keep names/consts/signatures; one-line delegations; per-store
  envelope DTOs deleted once unreferenced.

## 7. Ownership Matrix

| Concern | Owner |
|---|---|
| envelope + checksum + atomic write + legacy load | `SaveEnvelopeHelper` (Core) |
| path resolution + injection + façade API | `SaveStore<T>` (Core) / `SaveStoreHub` (host) |
| slot root / user:// globalization | `SaveSlotRoot` (host, unchanged) |
| versioned migration | Core codecs (unchanged) |
| section registry / triad | unchanged |
| gates | coverage gate + checksum selftest + matrix generator (updated once) |

## 8. Data Flow

Unchanged: subsystem `CaptureSave()` → façade `TrySave(state)` →
`SaveStore<T>` → `SaveEnvelopeHelper.TrySaveAtomic(SaveSlotRoot path)` →
`{"State":…,"Checksum":…}` on disk. Load mirrors with strict
reject-then-legacy-fallback. Aggregate path: `TryCaptureDirect` (bare or
envelope, per store, preserved) → `SaveSectionEnvelope.payloadJson`.

## 9. State Model

No new persistent state; no schema changes; DTOs untouched.

## 10. API/Contracts

Façade signatures preserved exactly (incl. `pathOverride` variants,
`Save`/`Load` void flavors, `BackupPath` for HoldfastTrade,
`TryCaptureDirect`/`TryRestoreDirect`). Log-tag prefixes
(`[WeatherSaveStore] …`) preserved.

## 11. Data Changes

None.

## 12. Save/Load

Byte-identical envelope JSON; `SaveChecksum.Compute(envelope)` stamp/verify
(unchanged); empty-checksum new-format saves rejected; legacy bare-state
fallback preserved; atomic temp+rename write; optional `.bak`
(HoldfastTrade only).

## 13. Determinism

No RNG, no IDs, no timestamps introduced; checksum algorithm untouched.

## 14. System/Event Wiring

None — stores are called by existing Main triads (`SaveXxx`/`FlushXxxIfDirty`)
and host sessions; no wiring changes.

## 15. Godot Integration

`SaveStoreHub` only. No scenes/UI.

## 16. Narrative/Content Integration

None.

## 17. Failure Modes

Null state ⇒ `TrySave` false. Missing/empty file ⇒ `TryLoad` null. Malformed
new-format envelope (null/empty checksum) ⇒ reject + log. Checksum mismatch
⇒ reject + log. Foreign/corrupt JSON ⇒ null + log (legacy fallback attempts
bare decode). Write failure ⇒ previous file intact (atomic), tmp cleaned
best-effort. Slot switch mid-session ⇒ provider re-evaluated per call.

## 18. Test Strategy

New `Ashfall.Core.Tests/Save/SaveStoreServiceTests.cs`: round-trip;
byte-identity vs hand-built envelope; tamper; empty-checksum reject; legacy
bare load; atomic (no `.tmp` leftover; `.bak` only when enabled; failed
write preserves previous); pathOverride; `FromCodec`; provider re-eval;
log-tag stability. Existing sweep/seal/wire/triad suites must pass
**unmodified** per batch (regression proof), plus headless save selftests.

## 19. Dependency-Ordered Phases

0. Baseline + this plan doc. *(Note: baseline gate 1 transiently failed due
   to a concurrent, unrelated UI-panel refactor in the working tree — HEAD
   itself is sound; Core test suite is the authoritative baseline.)*
1. Core `SaveStore<T>` (+ any `SaveEnvelopeHelper` extension for log tags).
2. Core tests.
3. Host `SaveStoreHub` + gate token updates + matrix regen (no migration yet).
4. Pilot batch (8 pinned stores): Weather, HostEvent, ChemicalDependency,
   Expedition, Medical, Narrative, World, Journal.
5. Batches 2–4: remaining ~30 Variant A stores (~10/batch, domain-grouped).
6. Batch 5: 13 Variant B stores via `FromCodec`.
7. Gate hardening (require delegation), AGENTS.md + REPO_REVIEW_REPORT,
   cross-tool review, final `verify-fast.sh`.

## 20. File Impact Map

CREATE: `Assets/Ashfall.Core/Save/SaveStore.cs`,
`src/Host/SaveStoreHub.cs`, `Ashfall.Core.Tests/Save/SaveStoreServiceTests.cs`,
this doc. MODIFY: 51 store files (phased), `SaveEnvelopeHelper.cs` (log-tag
parameter only, if needed), `SaveStoreCoverageGateTests.cs`,
`src/Host/SaveStoreChecksumSelfTest.cs`, `scripts/ci/generate-save-store-matrix.py`,
`docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` (regen), `AGENTS.md`,
`REPO_REVIEW_REPORT.md`. READ ONLY: `SaveChecksum.cs`, `HostDefaults.cs`,
`Ports.cs`, `SaveSlotRoot.cs`, `SaveSlotService.cs`, triad gate, registry,
codecs.

## 21. Risks

Format drift (mitigated: byte-identity test + unmodified pinned suites);
legacy-load regression (seal tests + per-service legacy tests); gate
doc-drift (regen in-commit); concurrent-session working-tree noise (stage
only initiative files); atomic-write delta (isolated, revertable).

## 22. Out of Scope

`SaveSlotService`/aggregate changes; `ResetAllSessions` hardcoded delete
list; stray `AtomicFileWriter.cs.uid`; DTO/schema changes; codec migration
logic; dirty-tracking redesign; Main decomposition; instance-DI of call
sites.

## 23. Rollback Strategy

One commit per batch; signatures + on-disk format unchanged ⇒ any batch
reverts cleanly; atomic write confined to the service.

## 24. Definition of Done

All 51 stores are façades over `SaveStore<T>`; ~4,700 duplicated lines
removed; coverage gate requires delegation; matrix regenerated; canonical
5-step checklist + `verify-fast.sh` green; cross-tool review passed.

## 25. Implementation Handoff

- **MUST PRESERVE:** envelope JSON (`{"State":…,"Checksum":…}` PascalCase,
  compact); `SaveChecksum.Compute(envelope)` stamp/verify; empty-checksum
  rejection; legacy bare-state fallback; façade signatures + consts;
  log-tag prefixes; per-call `SaveSlotRoot` resolution; per-store capture
  semantics (bare vs envelope).
- **MUST ADD:** `SaveStore<T>` + hub; `SaveStoreServiceTests`; hub-delegation
  recognition in all three scanning gates; atomic write + optional `.bak`.
- **MUST NOT DO:** touch state DTOs, codec migration logic, `SaveChecksum`,
  registry/triad methods, call sites, or the concurrent UI-panel refactor's
  files; introduce engine refs into Core; modify existing behavioral tests.
- **VERIFY WITH:** 5-step canonical checklist + `verify-fast.sh` +
  batch-relevant save selftests; PASS/FAIL reported per step.
- **FIRST SAFE STEP:** Core `SaveStore<T>` + tests (Phases 1–2), before any
  store or gate change.
