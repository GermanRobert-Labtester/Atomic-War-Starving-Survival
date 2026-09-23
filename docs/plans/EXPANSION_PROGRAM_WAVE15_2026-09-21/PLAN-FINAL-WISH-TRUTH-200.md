# PLAN-FINAL-WISH-TRUTH-200 — End-of-Life Wishes, Fulfilment & Memorial Link

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MORTUARY-MEMORIAL-TRUTH-123, PLAN-MENTAL-HEALTH-THERAPY-64, PLAN-RELATIONSHIP (Plan 43).
**Non-goals:** no memorial pipeline (Plan 123), no psychological model
(Plan 64), no graphic content; wishes are administrative and narrative.

## 1. Outcome
`Survivors/FinalWishSystem.cs` (**399 lines**) is reachable and unaddressed:
a survivor's recorded end-of-life wish (burial place, a letter, a keepsake
delivered) and whether it was fulfilled. Plan 123 handles the body and rites;
this system is the **promise side** — and unfulfilled wishes are a natural,
restrained source of survivor reaction.

| Deliverable | Detail |
|---|---|
| Wish model | wishes with a type (rite, letter, keepsake, place), a recorded day, and a fulfilment condition |
| Fulfilment detection | conditions evaluate over existing state (burial site Plan 123, letters Plan 122, keepsakes Plan 149, place Plan 95) |
| Consequence routing | fulfilment/unfulfilment routes to Plan 43/64 owners; no private guilt score |
| Visibility | a wish is visible to the ones who can fulfil it (roles Plan 141), not globally announced |
| Save truth | wishes and fulfilment restore; a load never fulfils or voids silently |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` (399 lines; unaddressed — Wave 13/15 audit).
- Plan 123's death pipeline is the trigger; Plan 149's heirlooms an object path.
- Plan 122's obligations are structurally related (promises among the living) — boundary stated.
- Plan 95 owns place facts a burial-place wish references.

## 3. Packages
- **FWT-200A** wish model + type table.
- **FWT-200B** fulfilment detection tests per type.
- **FWT-200C** consequence routing (no private score proof).
- **FWT-200D** visibility/role test.
- **FWT-200E** save round-trip; no silent fulfilment/void on load.

## 4. Acceptance & verification
- Every wish resolves to fulfilled/unfulfilled/void with a recorded reason.
- Consequences appear in the named owners.
- Save/load preserves wish state exactly.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Guilt as hidden stat → consequences route to owners; the proof test enforces it.
Overlap with 122 → wishes are death-contingent; obligations are living promises.

---

## 6. Expanded census (3 files · 581 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Loader 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `FinalWishCatalog.cs` | 115 | Catalog | — | 0 | 0 | 0 |
| `FinalWishCatalogLoader.cs` | 67 | Loader | — | 0 | 0 | 0 |
| `FinalWishSystem.cs` | 399 | System | **yes** | 0 | 0 | 3 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `FinalWishSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 10 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 3 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `FWT-200A` | no name match — resolve at claim time |
| `FWT-200B` | no name match — resolve at claim time |
| `FWT-200C` | no name match — resolve at claim time |
| `FWT-200D` | no name match — resolve at claim time |
| `FWT-200E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **6** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/HostCli.PanelTests.cs`, `src/Host/Phase0HostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 6 | `Ashfall.Core.Tests/Campaign/Plan47_65ModAgencyIntegrationTests.cs`, `Ashfall.Core.Tests/FinalWishPlan65CatalogTests.cs`, `Ashfall.Core.Tests/FinalWishSystemTests.cs`, `Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`, `Ashfall.Core.Tests/SurvivorFateSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **3**; isolated: **0**.

| From | → To |
|---|---|
| `FinalWishCatalog` | `FinalWishCatalogLoader` |
| `FinalWishCatalog` | `FinalWishSystem` |
| `FinalWishCatalogLoader` | `FinalWishCatalog` |

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

Matching flags in `HostCliRegistry.cs`: **0** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| — | no CLI flag shares a token with this domain |

**Verdict:** no CLI or selftest flag shares a token with this domain — the outcome is not operator-observable yet. Add coverage in the owning plan if it must be verifiable.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnFinalWishCompleted` | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` |
| `OnFinalWishFailed` | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` |
| `OnFinalWishStepCompleted` | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/final_wishes.json` |

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

Host files (`src/`) whose names share a domain token: **3**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/UI/PanelSceneLoader.cs` |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(OPTIONAL 1).

| Catalog | Classification |
|---|---|
| `final_wishes.json` | OPTIONAL |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 3 · catalogs 2 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-FINAL-WISH-TRUTH-200
wave: 15
status: PROPOSED — foreman claim required
packages: FWT-200A, FWT-200B, FWT-200C, FWT-200D, FWT-200E
claim paths:
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/LoaderWiringSelfTest.cs  # §19 candidate host surface
  - src/UI/PanelSceneLoader.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/final_wishes.json  # §17 catalog (verify schema + consumer)
  - final_wishes.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
