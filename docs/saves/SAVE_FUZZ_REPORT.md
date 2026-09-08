# Save Fuzz Report — Dose & Collectible State Chains (Plans 81/86)

> **Scope:** persistence side of the dose registers and the collectible
> flagship — `DoseLedgerSaveCodec`/`DoseLedgerSaveStore`, `DoseLedgerSystem`,
> `SickListSystem`, `CohortSystem`, `VoluntaryRegisterSystem`, `QuestlineSystem`,
> `CollectibleDiscoveryState`, `UniqueItemClaimRegistry`,
> `CollectibleTutorialTracker`, `ScavengingTableCatalog`.
> **Skill:** `ashfall-save-fuzz`. **Harness:** `Ashfall.Core.Tests/DoseCollectibleSaveFuzzTests.cs`
> (16 cases, all deterministic, `ISeededRng` only).
> **Result: 16/16 battery cases PASS. Full suite 9,739/9,739 PASS. No defects
> found in production save code — all fuzz failures were test-authoring errors,
> fixed in the test file.**

---

## 1. Persistence surface map (Phase 1)

| Chain | Component | Save surface | Envelope |
|---|---|---|---|
| Dose | `DoseLedgerSystem` | `CaptureState/RestoreState` inside `DoseLedgerSave` v2 envelope | checksummed (`SaveChecksum`), versioned |
| Dose | `SickListSystem` | section of the same v2 envelope | checksummed |
| Dose | `CohortSystem` | section of the same v2 envelope | checksummed |
| Dose | `VoluntaryRegisterSystem` | section of the same v2 envelope | checksummed |
| Dose | `QuestlineSystem` | section of the same v2 envelope (v2 addition) | checksummed |
| Dose (host) | `DoseLedgerSaveStore` | thin facade → `DoseLedgerSaveCodec.Decode` (reject path shared) | — |
| Collectible | `CollectibleDiscoveryState` | `CollectibleDiscoverySave` (schema v2; v1 legacy restore) | bare-state DTO |
| Collectible | `UniqueItemClaimRegistry` | `UniqueClaimSave` (schema v1, ordinal-sorted) | bare-state DTO |
| Collectible | `CollectibleTutorialTracker` | `CollectibleTutorialSave` (schema v1) | bare-state DTO |
| Collectible | `ScavengingTableCatalog` | **none — pinned data-only** (see §5) | — |

Pre-existing coverage (not duplicated): `DoseLedgerSystemTests` (tamper-reject,
checksumless-reject, capture/restore), `CollectibleDiscoveryPersistenceTests`
(ordinal serialization, round-trip, pre-Plan-47 behavior), host verb
`--dose-ledger-selftest`.

## 2. Battery matrix (Phase 2) — `DoseCollectibleSaveFuzzTests`

| Case | Dose codec | Discovery | Claims | Tutorial |
|---|---|---|---|---|
| Clean round-trip (all sections/state) | ✅ full 5-register + quest start | ✅ partitions + locations | ✅ claims + availability gate | ✅ seen + queue |
| Checksum mutation reject | ✅ `checksum mismatch` | n/a (bare-state) | n/a | n/a |
| Null-checksum reject | ✅ `no checksum` | n/a | n/a | n/a |
| Legacy fallback | ✅ v1→v2 migration, empty quest section, re-stamped checksum | ✅ schema v1 restore marks all acknowledged | ✅ stale-id drop | ✅ null restore |
| Version guard | ✅ future version rejected (`newer than supported`) | n/a | n/a | n/a |
| Serialize-twice byte-identical | ✅ | ✅ (out-of-order insertion normalized) | ✅ (ordinal-sorted) | — |
| Null restore honest-empty | — | ✅ | — | ✅ |

## 3. Findings

1. **No production defects.** The dose codec's reject paths fire with the exact
   documented errors; the v1→v2 migration validates the checksum over the FROZEN
   v1 shape and re-stamps the migrated payload; the collectible chain's
   bare-state captures are ordinal-sorted and byte-stable across serializations.
2. **`TryClaim` semantics documented by test:** claim-or-confirm — returns true
   when the id was already claimed; the claim set stays idempotent. The initial
   battery draft assumed a false-return no-op; the implementation is correct and
   the contract is now pinned.
3. **`UniqueClaimSave` stale-id drop verified:** ids no longer unique under the
   current catalog are dropped on restore so a stale save cannot suppress an
   ordinary item forever; currently-unique ids survive.
4. **`ScavengingTableCatalog` pinned data-only** via reflection guard — a future
   `CaptureState` on it forces a persistence-ownership review.

## 4. Determinism & wire parity (Phase 3)

- `DoseLedgerSave`: two `Encode` calls on the same capture are byte-identical
  (checksum recomputed deterministically over a stable field walk).
- `CollectibleDiscoverySave`: two captures byte-identical even with
  out-of-ordinal insertion (capture sorts every partition + location entries).
- `UniqueClaimSave`: byte-identical with out-of-order claims (ordinal sort).

## 5. Host-level smoke (Phase 4)

```text
godot --headless --path . -- --dose-ledger-selftest   → PASS
godot --headless --path . -- --data-integrity-selftest → PASS (0 findings, 298 catalogs)
dotnet test (full suite)                               → PASS 9,739/9,739
```

## 6. Residual risks (out of scope, noted)

- `CollectibleDiscoverySave`/`UniqueClaimSave`/`CollectibleTutorialSave` are
  bare-state DTOs without checksum envelopes. They ride inside host envelopes
  (or are tamper-relevant only via the host's own integrity); if they ever ship
  as standalone files, wrap them in the `SaveEnvelopeHelper` pattern per the
  Initiative #41 convention.
- `DoseLedgerSaveCodec` v1→v2 adoption of Year-of-Ash-carried quest progress
  (`DoseQuestMigration`) is covered by `DoseQuestOwnershipTests`, not re-fuzzed here.
