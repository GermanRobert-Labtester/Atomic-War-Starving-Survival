# PLAN-SAVE-SLOT-UX-105 — Slot List Truth, Autosave Visibility & Failure Messaging

**Wave 9 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SAVE-GOVERNANCE-12, PLAN-SAVE-MIGRATION-CORRIDOR-87, PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98.
**Non-goals:** no format change, no recovery logic change (Plan 98 owns paths),
no new slot store; this is the **presentation contract** of existing state.

## 1. Outcome
Slots are owned by `SaveSlotService`/`SaveStore` with `SaveProfileId` and
`SaveSlotId` value types; verification produces typed results
(`SaveEnvelopeHelper`); recovery behavior is specified by Plan 98. What players
see today is unspecified: which slots exist, when the autosave last wrote, why
a slot failed to load, and what a rollback would restore. This plan makes the
slot surface a truthful projection.

| Deliverable | Detail |
|---|---|
| Slot list truth | the list is generated from the store; no cached slot names; empty vs corrupt vs version-mismatch states are distinct and labelled |
| Autosave visibility | last autosave day/time source (game day, not wall clock) shown; rotation count documented |
| Failure messaging | each typed load failure maps to a plain-language reason + next action; no raw exception text |
| Rollback clarity | if a previous-good copy exists (per Plan 98 policy), the surface says what restoring it means before the action |
| No orphan rows | deleted slots leave no ghost rows; a missing file is removed from the list or marked missing explicitly |

## 2. Evidence
- `Save/SaveSlotService.cs`, `Save/SaveStore.cs`, `Save/SaveSlotTypes.cs` (`SaveProfileId`, `SaveSlotId`).
- `Save/SaveEnvelopeHelper.cs`: checksum verify produces the failure classes the messaging maps.
- Plan 98 defines recovery paths; this plan renders their outcomes, never invents alternatives.
- Plan 87 defines version policy; a version-mismatch message cites it.

## 3. Packages
- **SSU-105A** slot-list projection + refresh (no cache) with state enum asserted per fixture.
- **SSU-105B** autosave indicator using game-day source; rotation count shown.
- **SSU-105C** failure message table (code → player text) with a test per typed failure.
- **SSU-105D** rollback confirmation text driven by the actual recovery policy.
- **SSU-105E** orphan-row test: delete a slot file, reopen the surface, assert list state.

## 4. Acceptance & verification
- Every typed store state renders exactly one labelled state; no raw exception strings.
- Autosave indicator does not change when the OS clock changes (game-day source).
- Orphan-row test passes; rollback confirmation matches Plan 98 policy text.
- `bash scripts/run_test.sh` on the save-surface region + the focused UI render check.

## 5. Risks
Duplicate truth with Plan 12 → Plan 12 governs integrity/history; this plan only presents it.
Rollback wording drifting from policy → the confirmation is generated from the policy table, not hand-written twice.

---

## 6. Expanded census (3 files · 1,809 lines)

Scope: `Assets/Ashfall.Core/Save/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 1 · Support 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SaveSlotService.cs` | 1294 | Support | **yes** | 0 | 0 | 0 |
| `SaveSlotTypes.cs` | 196 | DTO/Type | **yes** | 0 | 0 | 0 |
| `SaveStore.cs` | 319 | Support | — | 0 | 0 | 10 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `store_capability_claims.json` | object[6 keys] |

**State surfaces:** `SaveStore.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Save/` |
| Test references | 39 name references across the test tree |
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
Domain files: 3. Other plans referencing them: **13**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-CAMPAIGN-PORTABILITY-104` | 3 |
| `PLAN-HOST-EVENT-ARCHIVE-91` | 2 |
| `PLAN-SAVE-PREVIEW-METADATA-114` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-INTEGRATION-KIT-02` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |
| `PLAN-INPUT-HARDENING-25` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `SSU-105A` | no name match — resolve at claim time |
| `SSU-105B` | no name match — resolve at claim time |
| `SSU-105C` | no name match — resolve at claim time |
| `SSU-105D` | no name match — resolve at claim time |
| `SSU-105E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 3. Host files: **209** · Test files: **26** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 209 | `src/Host/AgricultureSaveStore.cs`, `src/Host/AirlockSecuritySaveStore.cs`, `src/Host/AmphibiousDraisineSaveStore.cs`, `src/Host/AmputationSaveStore.cs`, `src/Host/AnomalyHazardSaveStore.cs` |
| Tests (`Ashfall.Core.Tests/`) | 26 | `Ashfall.Core.Tests/BareSaveStoreSealTests.cs`, `Ashfall.Core.Tests/Campaign/WallClockMetadataSeparationTests.cs`, `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`, `Ashfall.Core.Tests/DutyRoster/DutyRosterSaveStoreTests.cs`, `Ashfall.Core.Tests/Economy/EconomySaveStoreTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **0** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| — | no section key shares a token with this domain |

**Verdict:** no section key shares a token with this domain — either the authority is derived/stateless, or its persistence key is named after a different owner. Not a conclusion; verify in the owning system.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--save-store-checksum-selftest` |
| `--save-store-checksums-selftest` |

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

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/food_types.json` |
| `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **188**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipSaveStore.cs` |
| `src/Host/AquaponicsSaveStore.cs` |
| `src/Host/ArchaeologySaveStore.cs` |
| `src/Host/ArmoredCrawlerSaveStore.cs` |
| `src/Host/AutopsySaveStore.cs` |
| `src/Host/AviationSaveStore.cs` |
| `src/Host/BallisticShieldSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **0**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this domain |

**Verdict:** no save section matches — persistence is owned under a differently-named section, or absent.

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 13
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 12 · catalogs 2 · test regions 0 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SAVE-SLOT-UX-105
wave: 9
status: PROPOSED — foreman claim required
packages: SSU-105A, SSU-105B, SSU-105C, SSU-105D, SSU-105E
claim paths:
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - src/Host/AirlockSecuritySaveStore.cs  # §19 candidate host surface
  - src/Host/AmphibiousDraisineSaveStore.cs  # §19 candidate host surface
  - src/Host/AmputationSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/food_types.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/store_capability_claims.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --save-store-checksum-selftest
dependencies:
  - coordinate: 13 other plan(s) name these artifacts (§12)
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
