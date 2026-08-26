# BUG-01 Repair Plan — WorldSaveStore Checksum-Mismatch Softening

## 1. Bug

`src/Host/WorldSaveStore.cs:84-88` (working tree, uncommitted): when a checksummed
`world_save.json` envelope's stored `Checksum` fails verification, the store logs a
warning and **loads the envelope anyway**. All 16 sibling stores reject on mismatch;
HEAD's committed version of this store rejected too. The softening exists only in the
working tree, introduced by the same uncommitted sweep that added `customPath`.

## 2. Reproduction

1. `WorldSaveStore.TrySave(state, …, tmpPath)` — writes a valid checksummed envelope.
2. Flip one state field in the JSON text on disk (e.g. change a weather value).
3. `WorldSaveStore.TryLoadEnvelope(tmpPath)`:
   - **Expected:** null, error log (sibling parity, AGENTS.md contract).
   - **Actual (broken):** non-null envelope containing the mutated state; the next
     `TrySave` writes a *valid* checksum over the corrupt state — laundering it
     permanently into the save chain.

## 3. Root Cause

DTO-history conflation. `WorldHostSave` shipped at `748339f9` with 2 fields
(`State`, `Checksum`); four satellite fields (`SkyArmor`, `LocationEvolution`,
`Wildlife`, `Landmark`) were added later. A save written by the old build legitimately
fails hash verification under the grown DTO. The working-tree change treated every
mismatch as "version migration" and tolerated it, collapsing two disjoint causes —
(a) old-but-honest saves, (b) corruption/tampering — into one code path with no
schema-version discriminator. The project contract ("throw on future, migrate on past"
via versioned codecs) forbids resolving this by trusting the payload.

## 4. Blast Radius

| Surface | Effect of bug | Effect of repair |
|---|---|---|
| `WorldHostSession.Create` (production load) | restores unverified state | rejects corrupt save; world state reseeds clean |
| Save laundering chain | corruption becomes canonical on next save | broken at first load |
| `HostCli.PanelTests` world round-trip | asserts presence only | unchanged (clean round-trip unaffected) |
| Other 16 stores | none | none |
| 2-field-era saves (Aug-16..DTO-growth window) | tolerated | rejected — matches every committed build's behavior |
| Legacy bare-state (pre-checksum) saves | load via fallback | unchanged |
| xUnit suite | n/a (store is Godot-tied; mirrors test DTOs) | unchanged |

## 5. Invariants

1. New-format envelope + failing checksum ⇒ reject (sibling parity).
2. New-format envelope + missing/empty checksum ⇒ reject (already holds).
3. Bare legacy `WorldWeatherState` files ⇒ still load.
4. Clean round-trip ⇒ unchanged.
5. No RNG consumption, event order, or Core changes.
6. `customPath` test hooks remain (they are good and stay).

## 6. Repair Options

- **A. Restore hard reject** (one line + message): mismatch ⇒ `return null`.
- **B. Schema-versioned envelope + V1→V2 codec**: principled, but the store has no
  version field today; retroactively defining V1 semantics cannot distinguish
  "honest old" from "corrupted old" without per-version hashing rules. Oversized.
- **C. Tolerate-and-reseed**: same outcome as A via more code. Strictly worse.

## 7. Selected Repair

**Option A** — restore the hard reject, with the log message corrected so it no
longer advertises tolerated fallback. This is also a return to the committed HEAD
behavior, so zero released-build compatibility surface changes.

## 8. Why Other Options Were Rejected

- B: a versioned codec is the right *future* shape (see AGENTS.md codec pattern), but
  as a repair for this defect it adds machinery without changing outcomes: with no
  `schema_version` field on disk, old saves are indistinguishable from corrupt ones.
  If DTO growth pain recurs, add `schema_version` as a separate enhancement.
- C: extra code, same result, implies a tolerance that does not exist.

## 9. File Impact

- `src/Host/WorldSaveStore.cs` — 3 lines (reject + message).
- `src/Host/HostCli.PanelTests.cs` — add checksum-mutation assertions to the existing
  shelter-hazard-loop selftest's save round-trip section (the only host-side harness
  that already round-trips the real store via `customPath`).

No Core, data, or DTO changes.

## 10. Save/Data Implications

Corrupt/tampered world saves are rejected again (intended). Honest saves from the
2-field DTO window are rejected — identical to every committed build; the softening
never shipped, so nothing regresses relative to any release. World state is
reseedable (weather/wildlife/evolution are regenerable, not campaign-critical
progress), so a rejected world save does not lose player progress.

## 11. Determinism Implications

None. Load rejection returns null and the session constructs fresh deterministic
state — the same path already exercised by missing-checksum rejection.

## 12. Test Plan

1. Extend `--shelter-hazard-loop-selftest` save section: after the clean round-trip,
   mutate one field in `tmpWorld` on disk, assert `TryLoadEnvelope(tmpWorld)` returns
   null (currently FAILS — proves the defect), and assert the missing-checksum guard
   still rejects. Then clean round-trip again from a fresh save (guard against
   over-tightening).
2. `dotnet build Ashfall.csproj` — 0/0.
3. `dotnet test Ashfall.Core.Tests` — 2497 remain green (no Core change).
4. `godot --headless --path . -- --shelter-hazard-loop-selftest` — PASS.
5. `godot --headless --path . -- --data-integrity-selftest` — PASS (untouched, but
   cheap confidence).

## 13. Implementation Phases

- Phase 1: failing assertions in the selftest (red).
- Phase 2: one-line store repair (green).
- Phase 3: verification ladder + adversarial post-fix probes.

## 14. Rollback Strategy

Both files are tracked; `git checkout -- src/Host/WorldSaveStore.cs
src/Host/HostCli.PanelTests.cs` restores the exact pre-repair working-tree state.
No data, schema, or DTO change exists to roll back.

## 15. Definition of Done

- Mutated-checksum world save is rejected by the real store in the host selftest.
- Clean round-trip, legacy fallback, and missing-checksum rejection all still pass.
- Full verification ladder green.
- No diff outside the two listed files.
