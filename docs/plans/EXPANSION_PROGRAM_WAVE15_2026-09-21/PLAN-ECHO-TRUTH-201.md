# PLAN-ECHO-TRUTH-201 — Echo Delivery: When the Past Speaks Back

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-NARRATIVE-GRAPH-18, PLAN-CODEX-SURFACE-TRUTH-110, PLAN-BACKSTORY-REVEAL-TRUTH-126.
**Implementation scaffold:** [`PLAN-ECHO-TRUTH-201_APPENDIX-A_SCAFFOLD.md`](PLAN-ECHO-TRUTH-201_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-NARRATIVE-ARC-EVENT-TRUTH-176` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no graph storage (Plan 18), no backstory revelation (Plan 126),
no codex rendering (Plan 110).

## 1. Outcome
`Narrative/EchoSystem.cs` (**398 lines**) is reachable and unaddressed: echoes —
moments where the holdfast's past resurfaces (a remembered voice, a recorded
fragment) tied to specific places, objects, or survivors. The content exists
(the narrative data set includes echoes); the **delivery contract** — trigger,
target, once-only semantics, and what an echo may reveal — is unstated.

| Deliverable | Detail |
|---|---|
| Trigger model | echo triggers over existing facts (EnterSite, ReturnToPlace, HeldItem, DayAnniversary) with one owner per class |
| Targeting | an echo names its subject (person/place/object) and reveals only facts that subject's records allow (Plan 126's scopes) |
| Once-only/rare | declared per echo; repeatable echoes have a bounded cooldown on the canonical clock |
| Delivery surface | echoes appear through the journal/codex path (Plan 110); no new UI system |
| Save truth | fired-echo ledger restores; no re-fire on load, no lost pending echo |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/EchoSystem.cs` (398 lines; unaddressed — Wave 13/15 audit).
- The data set includes echo content (Plan 14's catalog classification counted narrative files).
- Plan 126 owns revelation scopes echoes must respect.
- Plan 18 owns flags the triggers read.

## 3. Packages
- **ECH-201A** trigger model + owner table.
- **ECH-201B** targeting/revelation-scope tests.
- **ECH-201C** once-only/cooldown fixtures.
- **ECH-201D** delivery surface check against Plan 110.
- **ECH-201E** save round-trip; no re-fire on load.

## 4. Acceptance & verification
- Every trigger class has a fixture; revelations respect Plan 126 scopes.
- A once-only echo never re-fires; a cooldown respects game days.
- Save/load preserves the fired ledger.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Echo spam → once-only/cooldown are declared per echo and tested.
Revealing withheld facts → scope checks are the guard.

---

## 6. Expanded census (2 files · 798 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `EchoCatalog.cs` | 400 | Catalog | — | 0 | 0 | 0 |
| `EchoSystem.cs` | 398 | System | **yes** | 0 | 0 | 3 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `echoes.json` | object[2 keys] |

**State surfaces:** `EchoSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 3 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `ECH-201A` | no name match — resolve at claim time |
| `ECH-201B` | no name match — resolve at claim time |
| `ECH-201C` | no name match — resolve at claim time |
| `ECH-201D` | no name match — resolve at claim time |
| `ECH-201E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **10** · Test files: **7** · Data files: **17**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 10 | `src/Audio/AudioEventBridge.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/EchoHostSession.cs`, `src/Host/EchoSaveStore.cs`, `src/Host/HostCli.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/ContentUtilizationGraphTests.cs`, `Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs`, `Ashfall.Core.Tests/Narrative/EchoCatalogTests.cs`, `Ashfall.Core.Tests/Narrative/EchoSystemTests.cs`, `Ashfall.Core.Tests/Narrative/NarrativeContinuityTests.cs` |
| Data (`StreamingAssets/Data/`) | 17 | `Assets/StreamingAssets/Data/confession_secrets.json`, `Assets/StreamingAssets/Data/dive_sites.json`, `Assets/StreamingAssets/Data/door_encounters.json`, `Assets/StreamingAssets/Data/echoes.json`, `Assets/StreamingAssets/Data/events.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **10** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `echoes` |
| `encounter_choice` |
| `encounters` |
| `events` |
| `host_event` |
| `moral_choice` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |
| `travel_encounters` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--audio-selftest` |
| `--audio-test` |
| `--moral-choice-selftest` |
| `--narrative-selftest` |
| `--save-store-checksum-selftest` |
| `--save-store-checksums-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **9**.

| Event | First declaration |
|---|---|
| `MoralChoiceSystem` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` |
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |

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

Host files (`src/`) whose names share a domain token: **2**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/EchoHostSession.cs` |
| `src/Host/EchoSaveStore.cs` |

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

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `echo` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 0 (laddered 0) · RNG streams 1 · host files 3 · catalogs 0 · test regions 0 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ECHO-TRUTH-201
wave: 15
status: PROPOSED — foreman claim required
packages: ECH-201A, ECH-201B, ECH-201C, ECH-201D, ECH-201E
claim paths:
  - src/Host/EchoHostSession.cs  # §19 candidate host surface
  - src/Host/EchoSaveStore.cs  # §19 candidate host surface
  - echo  # §19 candidate host surface
verification:
  - godot --headless --path . -- --audio-selftest
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
