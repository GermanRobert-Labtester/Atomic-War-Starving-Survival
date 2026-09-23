# PLAN-SECRETS-CONFESSION-TRUTH-127 — Held Secrets, Disclosure Pressure & Confession Outcomes

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MENTAL-HEALTH-THERAPY-64, PLAN-NARRATIVE-GRAPH-18, PLAN-BACKSTORY-REVEAL-TRUTH-126.
**Non-goals:** no second psychology model (Plan 64), no investigation system
(Plan 121), no public verdict logic (Plan 37).

## 1. Outcome
`Phantoms/ConfessionSecretSystem.cs` is host-unreachable (Plan 1 Appendix A),
and Plan 64 owns psychological state. A held secret is different from a
backstory fact (Plan 126): it has a **holder**, a **disclosure pressure**, and
consequences when told — confession to a person, exposure to the holdfast, or
keeping it. Nothing today models that lifecycle.

| Deliverable | Detail |
|---|---|
| Secret model | subject, holder, severity, disclosure pressure sources (guilt, witnessing, leverage) |
| Disclosure paths | confession (to a survivor), exposure (through an event/accusation), voluntary publication — each with an owner |
| Pressure effect | pressure enters Plan 64's model as a typed input; no local stress counter |
| Consequence table | per severity: relationship effects via Plan 43's owner, standing effects via the existing standing owner, no local score |
| Persistence | holder and state restore; load never discloses or resets a secret |

## 2. Evidence
- Plan 1 Appendix A/E/H: `ConfessionSecretSystem` host-unreachable; no banned deterministic source; sized in Appendix H.
- Plan 64 owns the psychological model this plan feeds.
- Plan 126 supplies the revelation model for facts; secrets link to a fact id but are not the same object.
- Plan 121 owns evidence chains; exposure uses that chain when a secret becomes evidence.

## 3. Packages
- **SCT-127A** secret model + pressure source table.
- **SCT-127B** disclosure paths + one test per path.
- **SCT-127C** pressure typed input into Plan 64.
- **SCT-127D** consequence table wired to relationship/standing owners.
- **SCT-127E** persistence round-trip incl. no accidental disclosure on load.

## 4. Acceptance & verification
- Every secret ends held, confessed, or exposed; no other state.
- Pressure is observable in Plan 64's model, not a parallel counter.
- Save/load preserves secrecy state exactly.
- `bash scripts/run_test.sh` on the psychology/phantoms regions.

## 5. Risks
Overlap with 121/126 → roles stated: 126 is fact revelation, 121 is evidence, this plan is held secret lifecycle.
Stress duplication → typed input to Plan 64 only.

---

## 6. Expanded census (2 files · 417 lines)

Scope: `Assets/Ashfall.Core/Phantoms/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ConfessionSecretCatalog.cs` | 129 | Catalog | — | 0 | 0 | 0 |
| `ConfessionSecretSystem.cs` | 288 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `confession_secrets.json` | array[38] |
| `wire_confessions.json` | object[3 keys] |

**State surfaces:** `ConfessionSecretSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Phantoms/` |
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

Domain method: plan-body artifact list.
Governed artifacts: 5. Other plans referencing them: **3**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ANOMALY-PHANTOM-63` | 4 |
| `EVIDENCE` | 1 |
| `PLAN-DATA-CONSUMER-22` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `ConfessionSecretCatalog.cs` |
| `ConfessionSecretSystem.cs` |
| `Phantoms/ConfessionSecretSystem.cs` |
| `confession_secrets.json` |
| `wire_confessions.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `SCT-127A` | `ConfessionSecretCatalog.cs`, `ConfessionSecretSystem.cs`, `Phantoms/ConfessionSecretSystem.cs` |
| `SCT-127B` | no name match — resolve at claim time |
| `SCT-127C` | no name match — resolve at claim time |
| `SCT-127D` | no name match — resolve at claim time |
| `SCT-127E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **0** · Test files: **2** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs`, `Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names)

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--narrative-selftest` |
| `--selftest-manifest` |
| `--test-manifest` |
| `--utility-ai-selftest` |
| `--utility-ai-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/confession_secrets.json` |

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

Host files (`src/`) whose names share a domain token: **0**
(0 of them panels/HUD).

| Host file |
|---|
| — | no host filename shares a token with this domain |

**Verdict:** no host file shares a token with this domain — the surface may be driven through a generic panel, or may not be surfaced at all. Verify before claiming a route.

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
| `confession_secrets.json` | OPTIONAL |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 0 · catalogs 2 · test regions 0 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SECRETS-CONFESSION-TRUTH-127
wave: 10
status: PROPOSED — foreman claim required
packages: SCT-127A, SCT-127B, SCT-127C, SCT-127D, SCT-127E
claim paths:
  - Assets/StreamingAssets/Data/confession_secrets.json  # §17 catalog (verify schema + consumer)
  - confession_secrets.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --narrative-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
