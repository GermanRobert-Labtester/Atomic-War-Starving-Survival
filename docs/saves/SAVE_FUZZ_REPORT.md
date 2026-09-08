# Save Fuzz Report — `wasteland_map` / `RegisteredMapFragments` (Plan 85 follow-up)

Scope: the ashfall-save-fuzz battery applied to the wasteland-map save section —
the persisted home of `RegisteredMapFragments`, the single fragment-progress
authority for the Plan 85 damaged-map layer. Other stores/codecs are covered by
their existing suites (`SaveStoreChecksumSweepTests`, `SaveWireContractTests`,
`BareSaveStoreSealTests`) and were not re-fuzzed here.

## Persistence surface (Phase 1)

| Concern | Owner | Notes |
|---|---|---|
| State DTO | `Ashfall.Core.World.WastelandMapState` | `Discovered`, `Completed`, `Locked`, `Unlocked`, `RegisteredMapFragments`, `Knowledge` |
| Capture/restore | `WastelandMapSystem.CaptureState` / `RestoreState` | Snapshot-isolated; `RegisteredMapFragments` copied verbatim (never sanitized — unknown ids persist and are inert) |
| Store | `src/Host/WastelandMapSaveStore.cs` | Thin façade over Core `SaveStore<T>` via `SaveStoreHub.Checksummed` — checksummed envelope, atomic write, **`allowLegacyBareState: false`** |
| Envelope contract | `Ashfall.Core.Save.SaveEnvelopeHelper` | Core-side; testable from the net9.0 test project |
| Migration | n/a | `WastelandMapState` has no codec versions; envelope is the only on-disk format (bare-state pre-envelope files deliberately rejected) |
| Known silent-loss types | n/a | `LocationEvolutionSaveable`/`WildlifeSaveable`/`LandmarkSaveable` are not part of this section (resolved per AGENTS audit note) |

## Battery results (Phase 2–3)

New suite: `Ashfall.Core.Tests/World/WastelandMapFragmentPersistenceFuzzTests.cs` — 9 tests.

| # | Case | Result |
|---|---|---|
| 1 | Clean round-trip through the live `DamagedMapSystem` (partial + completed/revealed zone): registration counts, completion, and reveal state preserved; replaying registrations after restore fires **zero** completions | PASS |
| 2 | Checksummed-envelope round-trip via `SaveEnvelopeHelper.CaptureEnvelope`/`RestoreEnvelope` (exact host-store contract) | PASS |
| 3 | Checksum mutation (one fragment id tampered in the persisted JSON) → rejected, error contains `Checksum mismatch` | PASS |
| 4 | Null / empty checksum on the new-format envelope → rejected with `Checksum field missing` (no silent legacy fallback) | PASS |
| 5 | Bare-state (pre-envelope) payload → rejected when `allowBareFallback: false` (the section's documented strictness) | PASS |
| 6 | Byte-for-byte serialization determinism across two captures **and** across a restore generation (capture → restore → capture is identical) | PASS |
| 7 | Seeded fuzz sweep — 25 iterations of random fragment subsets (with deliberate duplicate registrations) through the checksummed envelope; registration set stable, subset-of-catalog invariant holds, duplicates never double-count | PASS |
| 8 | Unknown/rolled-back fragment id in an old save: loads without failure, does not corrupt zone progress, completion of the touched zone still works, and the unknown id round-trips untouched | PASS |

## Findings

1. **No defects in the section.** The envelope contract, checksum rejection,
   bare-state strictness, and snapshot isolation all behaved exactly as
   documented. `RegisteredMapFragments` is copied verbatim on capture/restore —
   unknown ids are preserved faithfully, never rewritten (save bytes remain a
   truthful record; plan §7.5 policy respected).
2. **Gap closed:** the existing `WastelandMapPersistenceTests` covered every
   state list *except* `RegisteredMapFragments` — the Plan 85 field had zero
   persistence coverage. This battery closes that gap.
3. **No guard was weakened.** The battery only asserts rejection behavior the
   store already documents.

## Phase 4 — host-level smoke

- `godot --headless --path . -- --data-integrity-selftest` — PASS (298 catalogs, 0 findings).
- Full `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all green
  (see final counts in the run record; includes this battery).

## Quality gate

- [x] `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` all green
- [x] Section in scope has the five battery cases (clean round-trip, checksum
      mutation reject, null/empty checksum reject, legacy bare-state behavior,
      determinism) plus a fuzz sweep and an old-save unknown-id fixture
