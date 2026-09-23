# PLAN-SAVE-PREVIEW-METADATA-114 — Slot Summaries Without Full Loads

**Wave 9 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SAVE-SLOT-UX-105, PLAN-SAVE-MIGRATION-CORRIDOR-87, PLAN-HOST-EVENT-ARCHIVE-91.
**Non-goals:** no screenshot system, no thumbnail rendering, no second slot
store; the preview is metadata read from the existing envelope.

## 1. Outcome
The slot UI (Plan 105) lists what the store knows; opening a slot currently
means loading it. A **preview block** — day count, roster size, difficulty
preset, build/version, last-played game day, mod presence — lets a player (and
support) identify a save without loading 204 sections. The data mostly exists
in section headers and the envelope manifest, so this is a read-only summary
contract, not a new store.

| Deliverable | Detail |
|---|---|
| Summary fields | a fixed, documented field set with source per field (envelope manifest, section header, or derived count) |
| Cheap read | summary read touches headers only; a test asserts the load path is not invoked (no full decode) |
| Version-safe read | a summary from an older/newer envelope follows Plan 87 policy and degrades to available fields rather than failing |
| Mods signal | whether the slot was written with mods layered (Plan 92 manifest pin) — binomial, no mod list expansion |
| Failure honesty | an unreadable summary shows the same typed state as Plan 105 (corrupt/version mismatch), never blank |

## 2. Evidence
- `Save/SaveEnvelopeHelper.cs`, `Save/CampaignEnvelopeBuilder.cs`: envelope exists with checksum and section framing.
- `Save/SaveSlotService.cs`/`SaveSlotTypes.cs`: slot/profile ids for the list.
- Plan 87 supplies the version policy for degradation; Plan 92 supplies the mod pin.
- Plan 105 owns the surface; this plan supplies the read model behind it.

## 3. Packages
- **SPM-114A** field table (field, source, fallback) + schema note.
- **SPM-114B** header-only reader + no-full-load test.
- **SPM-114C** version-degradation tests (older/newer envelopes).
- **SPM-114D** mods signal from the Plan 92 pin.
- **SPM-114E** failure-state parity with Plan 105 messages.

## 4. Acceptance & verification
- Reader completes with header bytes only (instrumented assertion) and yields the documented fields.
- A truncated/corrupt header yields the Plan 105 failure state, not a partial row.
- Version-degraded reads name which fields are unavailable.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/`.

## 5. Risks
Preview drifting into a second load path → header-only is tested and the full load is instrumented off.
Field creep → the table is closed; new fields need a source and fallback row.

---

## 6. Expanded census (6 files · 2,167 lines)

Scope: `Assets/Ashfall.Core/Save/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 1 · Support 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CampaignEnvelopeBuilder.cs` | 116 | Support | **yes** | 0 | 0 | 0 |
| `CampaignSaveEnvelope.cs` | 180 | Support | — | 0 | 0 | 2 |
| `SaveEnvelopeHelper.cs` | 320 | Support | — | 0 | 0 | 0 |
| `SaveSlotService.cs` | 1294 | Support | **yes** | 0 | 0 | 0 |
| `SaveSlotTypes.cs` | 196 | DTO/Type | **yes** | 0 | 0 | 0 |
| `SchemaVersionedEnvelope.cs` | 61 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `CampaignSaveEnvelope.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Save/` |
| Test references | 34 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 6. Other plans referencing them: **12**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-CAMPAIGN-PORTABILITY-104` | 6 |
| `PLAN-SAVE-MIGRATION-CORRIDOR-87` | 4 |
| `PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98` | 4 |
| `PLAN-SAVE-SLOT-UX-105` | 3 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `SPM-114A` | `SchemaVersionedEnvelope.cs` |
| `SPM-114B` | no name match — resolve at claim time |
| `SPM-114C` | `SchemaVersionedEnvelope.cs` |
| `SPM-114D` | no name match — resolve at claim time |
| `SPM-114E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **3**; isolated files:
**3**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `CampaignEnvelopeBuilder` | `SaveSlotService` |
| `SaveSlotService` | `CampaignEnvelopeBuilder` |
| `SaveSlotService` | `SaveEnvelopeHelper` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `CampaignEnvelopeBuilder` | 1 |
| `SaveEnvelopeHelper` | 1 |
| `SaveSlotService` | 1 |
| `CampaignSaveEnvelope` | 0 |
| `SaveSlotTypes` | 0 |
| `SchemaVersionedEnvelope` | 0 |

**Class split:** hub 2 · sink 1 · source 0 · isolated 3.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **120** · Test files: **28** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 120 | `src/Host/AirlockSecuritySaveStore.cs`, `src/Host/AmphibiousDraisineSaveStore.cs`, `src/Host/AmputationSaveStore.cs`, `src/Host/AnomalyHazardSaveStore.cs`, `src/Host/ApprenticeshipSaveStore.cs` |
| Tests (`Ashfall.Core.Tests/`) | 28 | `Ashfall.Core.Tests/Campaign/WallClockMetadataSeparationTests.cs`, `Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`, `Ashfall.Core.Tests/CollectibleDiscoveryStateTests.cs`, `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`, `Ashfall.Core.Tests/Host/HostSessionStateSemanticsTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/food_types.json` |
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (32 files, 187 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |

**Verdict:** 187 cases sit under matching regions — run those first (`Campaign`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **8**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/SaveSlotRoot.cs` |
| `src/Main.Campaign.cs` |
| `src/Main.CampaignOwners.cs` |
| `src/Main.CampaignServices.cs` |
| `src/Main.UiTests.RealCampaignJourney.cs` |
| `src/UI/MainMenuBuilder.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `campaign` | no |
| `campaign_day` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **0**
(none).

| Catalog | Classification |
|---|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog matches — the domain is code-authoritative or its data lives in a broader catalog.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 12
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 8 · catalogs 3 · test regions 1 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SAVE-PREVIEW-METADATA-114
wave: 9
status: PROPOSED — foreman claim required
packages: SPM-114A, SPM-114B, SPM-114C, SPM-114D, SPM-114E
claim paths:
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - src/Host/CampaignDaySaveStore.cs  # §19 candidate host surface
  - src/Host/SaveSlotRoot.cs  # §19 candidate host surface
  - src/Main.Campaign.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/food_types.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 12 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
