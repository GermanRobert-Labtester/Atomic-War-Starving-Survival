# PLAN-COATING-TECH-TRUTH-188 — Thin-Film Deposition, Coating Quality & Coverage

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-METROLOGY-TRUTH-172, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Non-goals:** no industry engines (Plan 45), no standards set (Plan 172), no
quality tiers (Plan 112).

## 1. Outcome
`Shelter/EbPvdCoatingEngine.cs` (**537 lines**) is reachable and unaddressed:
electron-beam deposition for precision coatings (optics, wear surfaces).
Coating is a capability with coverage/quality outcomes and consumable targets —
unstated today, so it is either a flat upgrade or inert.

| Deliverable | Detail |
|---|---|
| Process model | targets/substrates per batch, deposition time on the canonical clock, consumable target material |
| Coverage/quality | coating quality from substrate preparation + chamber condition (Plan 172 standards, Plan 119 decay); a poor coat is marked, never silently equal |
| Effect application | a coated part's benefit applies through its own owner (optics, tool wear per Plan 119's contract); no global buff |
| Failure modes | target exhaustion, chamber contamination with documented partial outputs |
| Save truth | chamber state and in-progress batches restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/EbPvdCoatingEngine.cs` (537 lines; unaddressed — Wave 13 audit).
- Plan 172 supplies tolerance/capability inputs; Plan 119 the decay contract.
- Plan 45 owns the industrial family this bench belongs to.
- Plan 112 grades outputs where a coat is a produced good.

## 3. Packages
- **CTT-188A** process model + consumable table.
- **CTT-188B** coverage/quality tests across preparation and chamber states.
- **CTT-188C** effect-application audit (no global buff proof).
- **CTT-188D** failure fixtures (target, contamination) + partial outputs.
- **CTT-188E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Coating quality is marked and derives from documented inputs.
- Effects appear only in the coated part's owner; removing the coat removes the effect.
- Failures produce documented partials; save/load preserves chamber state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Global buff → effect audit forbids it; the removal test proves it.
Capability invisibility → chamber/substrate states are surfaced in the bench view.

---

## 6. Expanded census (4 files · 1,590 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Loader 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CvdDiamondCatalog.cs` | 350 | Catalog | — | 0 | 0 | 0 |
| `CvdDiamondSynthesisEngine.cs` | 527 | System | — | 0 | 0 | 2 |
| `EbPvdCoatingCatalogLoader.cs` | 176 | Loader | — | 0 | 0 | 0 |
| `EbPvdCoatingEngine.cs` | 537 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `ebpvd_coating_catalog.json` | object[6 keys] |
| `cvd_diamond_catalog.json` | object[10 keys] |
| `optical_coating_rad_browning_reports.json` | array[7] |

**State surfaces:** `CvdDiamondSynthesisEngine.cs`, `EbPvdCoatingEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 8 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-INDUSTRY-AUTOMATION-45` | 2 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CTT-188A` | no name match — resolve at claim time |
| `CTT-188B` | no name match — resolve at claim time |
| `CTT-188C` | no name match — resolve at claim time |
| `CTT-188D` | no name match — resolve at claim time |
| `CTT-188E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **5** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/CvdDiamondHostSession.cs`, `src/Host/EbPvdCoatingHostSession.cs`, `src/Host/HostCli.Plans122to125.cs`, `src/Main.Plans122to125.cs`, `src/Main.Plans146_149.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/Integration/Plans146_149IntegrationTests.cs`, `Ashfall.Core.Tests/Save/Plans122to125PersistenceTests.cs`, `Ashfall.Core.Tests/Shelter/EbPvdCoatingEngineTests.cs`, `Ashfall.Core.Tests/Shelter/Plan124CvdDiamondCatalogTests.cs`, `Ashfall.Core.Tests/Shelter/Plan124CvdDiamondSynthesisEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **1**; isolated: **2**.

| From | → To |
|---|---|
| `CvdDiamondSynthesisEngine` | `CvdDiamondCatalog` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_synthesis` |
| `chlor_alkali_synthesis` |
| `cvd_diamond` |
| `ebpvd_coating` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **4** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--cvd-diamond-selftest` |
| `--ebpvd-coating-selftest` |
| `--ebpvd-coating-uitest` |
| `--late-tech-mobility-selftest` |

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

Catalog JSON files whose names share a domain token: **7**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/chlor_alkali_synthesis_catalog.json` |
| `Assets/StreamingAssets/Data/cvd_diamond_catalog.json` |
| `Assets/StreamingAssets/Data/ebpvd_coating_catalog.json` |
| `Assets/StreamingAssets/Data/mineral_acid_synthesis_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/lost_tech_manuals.json` |
| `Assets/StreamingAssets/Data/narrative/optical_coating_rad_browning_reports.json` |
| `Assets/StreamingAssets/Data/tech_salvage.json` |

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

Host files (`src/`) whose names share a domain token: **12**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/ChemicalSynthesisHostSession.cs` |
| `src/Host/ChemicalSynthesisSaveStore.cs` |
| `src/Host/CvdDiamondHostSession.cs` |
| `src/Host/CvdDiamondSaveStore.cs` |
| `src/Host/EbPvdCoatingHostSession.cs` |
| `src/Host/EbPvdCoatingSaveStore.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Main.ChemicalSynthesis.cs` |
| `src/UI/CvdDiamondPanel.cs` |
| `src/UI/EbPvdCoatingPanel.cs` |
| `src/UI/PanelSceneLoader.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **4**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `chemical_synthesis` | no |
| `chlor_alkali_synthesis` | no |
| `cvd_diamond` | no |
| `ebpvd_coating` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `cvd_diamond` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **4**
(CODEX_ONLY 2, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `chlor_alkali_synthesis_catalog.json` | UNRESOLVED |
| `mineral_acid_synthesis_catalog.json` | UNRESOLVED |
| `narrative/lost_tech_manuals.json` | CODEX_ONLY |
| `narrative/optical_coating_rad_browning_reports.json` | CODEX_ONLY |

**Verdict:** 2 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 4 (laddered 0) · RNG streams 2 · host files 14 · catalogs 11 · test regions 0 · flags 4

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-COATING-TECH-TRUTH-188
wave: 14
status: PROPOSED — foreman claim required
packages: CTT-188A, CTT-188B, CTT-188C, CTT-188D, CTT-188E
claim paths:
  - src/Host/ChemicalSynthesisHostSession.cs  # §19 candidate host surface
  - src/Host/ChemicalSynthesisSaveStore.cs  # §19 candidate host surface
  - src/Host/CvdDiamondHostSession.cs  # §19 candidate host surface
  - src/Host/CvdDiamondSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/chlor_alkali_synthesis_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/cvd_diamond_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --cvd-diamond-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
