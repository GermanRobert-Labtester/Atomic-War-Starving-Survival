# PLAN-PERIMETER-DEFENSE-TRUTH-165 — Wall Integrity, Approaches & Breach Consequences

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-BASE-DEFENSE-RAIDS-61, PLAN-SKY-DEFENSE-TRUTH-135, PLAN-NOISE-DISCIPLINE-TRUTH-116, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Implementation scaffold:** [`PLAN-PERIMETER-DEFENSE-TRUTH-165_APPENDIX-A_SCAFFOLD.md`](PLAN-PERIMETER-DEFENSE-TRUTH-165_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-BASE-DEFENSE-RAIDS-61` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no raid resolution (Plan 61), no aerial battery (Plan 135), no
new construction catalog (Plan 40 owns builds).

## 1. Outcome
`Defense/PerimeterDefenseSystem.cs` (796 lines) is reachable and unaddressed.
Plan 61 resolves raids; Plan 135 handles aerial threats. The **perimeter** —
wall condition, gate state, approach coverage, breach points — is the layer
raids actually meet, and nothing states which surface owns it or how a breach
propagates into the raid that follows.

| Deliverable | Detail |
|---|---|
| Perimeter model | segments with condition from built state (Plan 40) and maintenance via Plan 119's contract |
| Gate policy | open/closed/guarded states with who may pass (Plan 155's arrival gate reads it) |
| Approach coverage | which approaches each segment covers; an uncovered approach is a visible weakness, not a hidden penalty |
| Breach propagation | a segment at/below threshold becomes a breach point; Plan 61 reads the breach list as an input |
| Save truth | segment condition and gate state restore; a load never repairs or damages silently |

## 2. Evidence
- `Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs` (796 lines; unmentioned in every plan body — Wave 13 audit).
- Plan 61 owns raid resolution that consumes breach state.
- Plan 116's quiet/loud model feeds approach detection; Plan 135 covers air.
- Plan 119 supplies the shared decay contract for segment wear.

## 3. Packages
- **PDT-165A** segment model + condition source (built state).
- **PDT-165B** gate policy states + Plan 155 read check.
- **PDT-165C** approach coverage table + uncovered-approach visibility test.
- **PDT-165D** breach threshold + list hand-off test to Plan 61.
- **PDT-165E** save round-trip; no silent repair/damage on load.

## 4. Acceptance & verification
- Segment condition follows the shared decay contract; a difficulty change scales it.
- A breached segment appears in the list Plan 61 reads; an intact one does not.
- Gate state changes only through its documented transitions.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Defense/`.

## 5. Risks
Hidden penalties → coverage and breach state are visible in the perimeter view.
Duplicating Plan 61 → this plan supplies state; raid math stays there.

---

## 6. Expanded census (3 files · 1,228 lines)

Scope: `Assets/Ashfall.Core/Defense/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PerimeterDefenseCatalog.cs` | 152 | Catalog | — | 0 | 0 | 0 |
| `PerimeterDefenseSystem.cs` | 796 | System | **yes** | 0 | 0 | 2 |
| `PerimeterEarlyWarningEngine.cs` | 280 | System | — | 0 | 0 | 4 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `perimeter_defenses.json` | object[2 keys] |

**State surfaces:** `PerimeterDefenseSystem.cs`, `PerimeterEarlyWarningEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Defense/` |
| Test references | 13 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **5**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-BASE-DEFENSE-RAIDS-61` | 3 |
| `PLAN-DEFENSE-COMMAND-TRUTH-207` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-NOISE-DISCIPLINE-TRUTH-116` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PDT-165A` | no name match — resolve at claim time |
| `PDT-165B` | no name match — resolve at claim time |
| `PDT-165C` | no name match — resolve at claim time |
| `PDT-165D` | no name match — resolve at claim time |
| `PDT-165E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **9** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/DefenseHostSession.cs`, `src/Main.AdvancedShelterSystems.cs` |
| Tests (`Ashfall.Core.Tests/`) | 9 | `Ashfall.Core.Tests/Defense/PerimeterDefensePhase7Tests.cs`, `Ashfall.Core.Tests/Defense/PerimeterDefensePlan203Tests.cs`, `Ashfall.Core.Tests/Defense/PerimeterDefenseTests.cs`, `Ashfall.Core.Tests/Defense/PerimeterEarlyWarningEngineTests.cs`, `Ashfall.Core.Tests/DefenseSystemTests.cs` |
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

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `perimeter_defense` |
| `sky_defense_battery` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--defense-selftest` |
| `--sky-defense-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnHazardWarning` | `Assets/Ashfall.Core/VentilationSystem.cs` |
| `OnImpactWarning` | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` |
| `OnSafetyWarning` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/perimeter_defenses.json` |
| `Assets/StreamingAssets/Data/sky_defense_ordnance.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (4 files, 36 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Defense` | 4 | 36 |

**Verdict:** 36 cases sit under matching regions — run those first (`Defense`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **9**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/DefenseHostSession.cs` |
| `src/Host/DefenseSaveStore.cs` |
| `src/Host/HostCli.SkyDefense.cs` |
| `src/Host/PerimeterDefenseSaveStore.cs` |
| `src/Host/SkyDefenseBatterySaveStore.cs` |
| `src/Main.SkyDefense.cs` |
| `src/UI/ChemWarfareDefensePanel.cs` |
| `src/UI/DefenseGridPanel.cs` |
| `src/UI/SkyDefenseBatteryPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `perimeter_defense` | no |
| `sky_defense_battery` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `defense_capture` |
| `defense_damage` |
| `defense_targeting` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(GAMEPLAY_CONSUMED 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `perimeter_defenses.json` | GAMEPLAY_CONSUMED |
| `sky_defense_ordnance.json` | UNRESOLVED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 2 (laddered 0) · RNG streams 3 · host files 12 · catalogs 4 · test regions 1 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PERIMETER-DEFENSE-TRUTH-165
wave: 13
status: PROPOSED — foreman claim required
packages: PDT-165A, PDT-165B, PDT-165C, PDT-165D, PDT-165E
claim paths:
  - src/Host/DefenseHostSession.cs  # §19 candidate host surface
  - src/Host/DefenseSaveStore.cs  # §19 candidate host surface
  - src/Host/HostCli.SkyDefense.cs  # §19 candidate host surface
  - src/Host/PerimeterDefenseSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/perimeter_defenses.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/sky_defense_ordnance.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Defense/
  - godot --headless --path . -- --defense-selftest
dependencies:
  - coordinate: 5 other plan(s) name these artifacts (§12)
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
