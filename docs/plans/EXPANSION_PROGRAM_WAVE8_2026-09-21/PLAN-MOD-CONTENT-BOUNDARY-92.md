# PLAN-MOD-CONTENT-BOUNDARY-92 — Layering Precedence, Conflict Reports & Safe Disable

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DATA-AUTHORITY-14, PLAN-CONTENT-PIPELINE-QA-77, PLAN-SAVE-GOVERNANCE-12.
**Implementation scaffold:** [`PLAN-MOD-CONTENT-BOUNDARY-92_APPENDIX-A_SCAFFOLD.md`](PLAN-MOD-CONTENT-BOUNDARY-92_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-CONTENT-PIPELINE-QA-77` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no scripting surface for mods, no code execution, no
out-of-process loader, no ranked-list or service.

## 1. Outcome
`Assets/Ashfall.Core/Mods/` already contains `JsonModLayering.cs`,
`ModCompatibilityEvaluator.cs`, and `ModDataContract.cs` (with
`ModSupportSystem`, host-unreachable today), and
`Data/mod_manifest_schema.json` is the one structural schema in the data tree.
What is missing is the **boundary contract**: what a mod may change, in which
precedence, how conflicts surface, how mods are pinned, and how a mod is
disabled without corrupting a save.

| Deliverable | Detail |
|---|---|
| Precedence spec | base → patch layer → user override, per catalog; a single documented order with tests |
| Conflict report | collected conflicts (file, key, winner) reported to the player/log; never fail-fast on first conflict |
| Manifest pinning | mod id + content checksum pinned in the manifest; a changed file under the same id is flagged |
| Safe disable | disabling or removing a mod after load: referenced ids resolve to safe defaults or flagged missing, no crash, no silent corruption |
| Determinism gate | a modded run is same-seed reproducible; no mod path may inject wall-clock or unseeded RNG |

## 2. Evidence
- `Assets/Ashfall.Core/Mods/ModDataContract.cs` (`ModSupportSystem` in Plan 1 Appendix A), `JsonModLayering.cs`, `ModCompatibilityEvaluator.cs`.
- `Assets/StreamingAssets/Data/mod_manifest_schema.json` (only structural schema found in the data tree).
- Save integrity: `SaveSectionRegistry` aliases (`LifecycleSectionAliases`, `CanonicalizeSectionKey`) define how retired keys resolve; mod-removal resolution follows the same seam.
- Plan 77 owns first-party content pipeline QA; this plan owns third-party content only.

## 3. Packages
- **MCB-92A** precedence spec + layering tests over a two-layer fixture.
- **MCB-92B** conflict report: structured list + one panel/log surface; bounded sample in tests.
- **MCB-92C** manifest pinning: checksum recorded and verified on load; mismatch is a typed warning with the file named.
- **MCB-92D** safe disable: remove-after-load fixture; save still loads; missing ids flagged via the alias/default path.
- **MCB-92E** determinism gate: same-seed modded replay matches; banned-source scan covers `Mods/`.

## 4. Acceptance & verification
- Two-layer precedence test passes; conflict fixture reports both keys with the winner.
- Disabled-mod save loads with missing-id flags and no exception; re-enabling restores.
- Focused tests: `bash scripts/run_test.sh Ashfall.Core.Tests/Mods/`.

## 5. Risks
Mods as an authority side-door → only catalog layering is allowed; the
determinism gate and save alias path keep Core authoritative.

---

## 6. Expanded census (3 files · 1,352 lines)

Scope: `Assets/Ashfall.Core/Mods/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `JsonModLayering.cs` | 647 | Support | **yes** | 0 | 0 | 0 |
| `ModCompatibilityEvaluator.cs` | 199 | Support | **yes** | 0 | 0 | 0 |
| `ModDataContract.cs` | 506 | Support | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `vehicle_modifications.json` | object[2 keys] |
| `armored_crawler_modules.json` | object[2 keys] |
| `commodity_baselines.json` | object[4 keys] |
| `vehicle_modules.json` | object[2 keys] |
| `mod_manifest_schema.json` | object[9 keys] |
| `engineering_mod_notes.json` | object[4 keys] |

**State surfaces:** `ModDataContract.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Mods/` |
| Test references | 4 name references across the test tree |
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
| `EVIDENCE` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MCB-92A` | `JsonModLayering.cs` |
| `MCB-92B` | no name match — resolve at claim time |
| `MCB-92C` | no name match — resolve at claim time |
| `MCB-92D` | no name match — resolve at claim time |
| `MCB-92E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/HostCli.Mods.cs`, `src/Host/ModRuntime.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Campaign/Plan47_65ModAgencyIntegrationTests.cs`, `Ashfall.Core.Tests/Mods/JsonModLayeringTests.cs`, `Ashfall.Core.Tests/Mods/Plan47ModContractTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **2**; isolated: **0**.

| From | → To |
|---|---|
| `JsonModLayering` | `ModCompatibilityEvaluator` |
| `ModDataContract` | `ModCompatibilityEvaluator` |

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

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--port-contract-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **0**.

| Catalog |
|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog filename shares a token with this domain — the authority is likely code-defined or its data lives in a broader catalog. Not a conclusion; check the owning loader.

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

Host files (`src/`) whose names share a domain token: **1**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/PortContractSelfTest.cs` |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **538**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 162, OPTIONAL 24, UNRESOLVED 73).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `aircraft_parts.json` | GAMEPLAY_CONSUMED |
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `armored_crawler_modules.json` | UNRESOLVED |
| `atmospheric_sounding_catalog.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |

**Verdict:** 73 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 1 · catalogs 10 · test regions 0 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MOD-CONTENT-BOUNDARY-92
wave: 8
status: PROPOSED — foreman claim required
packages: MCB-92A, MCB-92B, MCB-92C, MCB-92D, MCB-92E
claim paths:
  - src/Host/PortContractSelfTest.cs  # §19 candidate host surface
  - acoustic_triangulation_catalog.json  # §17 catalog (verify schema + consumer)
  - aircraft_parts.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --port-contract-selftest
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
