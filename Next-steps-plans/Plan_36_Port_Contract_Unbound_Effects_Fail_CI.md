# Plan 36 — The Port Contract: Unbound Effects Must Fail CI

> **Wave:** Continuity Wave 5 — *The Human Interface*
> **Depends on:** 35A (it produces the first ports to gate); pairs with Wave 1's 15C and Wave 3's
> 27A/27B — this is that correction made mechanical and general.
>
> **Theme:** Core exposes 147 integration seams (`Bind*/Set*/Wire*/Register*/Apply*/Enable*/
> Configure*`) for the host to plug into. **74 have no caller in `src/`.** Some are reached from
> inside Core and are fine. Some are exercised only by unit tests — including `ApplyGrief` and
> `ApplySedative`, whose only callers are test files. Some are declared and referenced by nothing at
> all. The project's own best practice already exists (`CombatHostSession.ValidatePorts`); this plan
> makes it a rule the machine enforces.

---

## Evidence Inventory (re-verified @ `ccac926e`)

Method: all Core public methods named `Bind|Set|Wire|Register|Apply|Enable|Configure*` (excluding
headless demos), checked for callers in `src/`, in other Core files, and in tests.

| Metric | Value |
|---|---|
| Integration-shaped public methods in Core | **147** |
| …with **no caller in `src/`** | **74** |
| …of which also referenced by **no other Core file** and **no test** | **34** |

### Triage of named examples (each independently confirmed)

| Method | Core files mentioning it | Test files | Reading |
|---|---:|---:|---|
| `ApplyGrief` | **1** (its own declaration) | 3 | **Only tests call it.** `IGriefSink` + `DeathQuality`/`MemorialOutcome` landed at `b48b4494` ("Plan 09 9C Core") — the game never invokes grief |
| `ApplySedative` | **1** | 3 | Same shape: a chemical-dependency/medical behaviour proven in xUnit, unreachable in play |
| `ApplyChoice` | 1 | **0** | Declared, never called by anything |
| `RegisterFactions` | 1 | **0** | Catalog registration seam nobody uses — the host registers factions another way (or not at all) |
| `ApplyOpenWindowNeeds` | 1 | **0** | A ventilation/needs effect with no producer or consumer |
| `ApplyTreatment` | **7** | 0 | Reached through Core's medical pipeline — likely **live**; must be classified, not deleted |

**Why this matters more than any single bug:** four audit waves produced long lists of "authored but
inert" — `ServeMeal`, `SetHunterSkill`, `SetCellar/SetRefrigeration`, `ConsumeRation`,
`OnInventoryConsumeClicked`, `TryResolveMoralChoice`, `SimulateDailyFact`/`SimulateDailyFriction`,
`FactionWarChainRunner`, null `Consume` callbacks, `DegradeRate = 0f`. Every one of them is the same
event: **a Core seam the host never plugged in, with nothing standing between it and a green build.**

### The pattern that already exists in this codebase

| Fact | Evidence |
|---|---|
| Combat declares its required effects and validates them | `src/Host/CombatHostSession.cs:145–153` binds eight ports; `:161 ValidatePorts()` logs *"any production-required combat effects still unbound… An empty list means every health, morale, inventory, and progression effect reaches a real consumer"*; called from `src/Main.Expeditions.cs:122` |
| Optimistic-concurrency verbs already exist | `WaterTreatmentSystem.cs:180,219,325,340` `Preview*/Execute*(…, long expectedStateVersion, long currentStateVersion)` — the shape for "the panel is stale" being impossible |
| The liveness precedent is Wave 1's | `Plan_15` Task 15C (panel liveness gate) + Wave 3's 27A (fixture fidelity) — this plan is the Core-side twin |

---

## Task 36A — Classify every seam, then gate the classes

**Goal:** a machine-checked declaration of what each Core integration method is *for* and who must
call it, so "declared and never invoked" becomes a build failure.

**Files:** new `Assets/Ashfall.Core/Ports/PortContract.cs` (attributes), new
`scripts/ci/generate-port-contract.py`, new `Ashfall.Core.Tests/PortContractGateTests.cs`,
`docs/architecture/PORT_CONTRACT.md`, `docs/ci/CI_GATE_MANIFEST.json`, all Core systems with the 147
methods.

### Substeps

1. **Publish the triage table first** (no code): all 74 host-less seams, each marked
   `LIVE_VIA_CORE`, `HOST_MISSING`, `DEAD`, or `TEST_ONLY`, with the file:line that decides it.
   `ApplyTreatment` and `ApplyGrief` are the two contrasting examples — one is reached through the
   medical pipeline, the other only by xUnit.
2. **Delete `DEAD` immediately** with a note (never leave a phantom seam; a method nobody calls is a
   promise nobody made). Where `TEST_ONLY` proves a behaviour nobody can trigger, either wire it
   (preferred — grief and sedation are wanted) or delete the test's pretence that it ships.
3. **Introduce a lightweight declaration** on the Core side — e.g.
   `[PortContract(Owner = "medical", RequiredCaller = HostSession.Phase0, Effect = "sedation applied")]`
   — or a single generated table if attributes feel too invasive; either way the contract must live
   in **one** place and be readable without opening 60 files.
4. **Generate the manifest** (`generate-port-contract.py --check`) in the established
   `generate-save-store-matrix.sh --check` / `generate-ui-panel-catalog.py` house style, emitting
   `docs/architecture/PORT_CONTRACT.md` + JSON: seam → owner → declared caller → observed caller →
   status.
5. **Register it as a fast-tier critical gate**: any seam whose declared caller does not appear in
   `src/` fails; any host session that constructs a system but binds fewer required ports than
   declared fails.
6. **Adopt combat's runtime half too**: require every host session that wires ports to call a
   `ValidatePorts()`-style check at setup and **fail loudly in dev** (not `GD.PushWarning` that scrolls
   away) — the `Main.Expeditions.cs:122` precedent shows the call site is easy.
7. **Add unbound-effect coverage to an existing selftest** (`--panel-bind-lifecycle-selftest` or a
   new narrow verb) so CI prints the count of unbound ports per subsystem — a number, not a grep.
8. **Seed the ratchet from reality**: land the gate with the current `HOST_MISSING` set as an
   explicit, dated exemption list that may only shrink (Wave 1's 15C discipline), so this plan can't
   be "fixed" by silencing it.
9. **Wire the two highest-value `TEST_ONLY` seams as the pilot**: `ApplyGrief` (memorial → morale,
   which is exactly Wave 2's 24B step 6) and `ApplySedative` (chemical dependency / ward calm →
   fatigue) — each becomes a demonstration that the gate + a host call turns a unit-tested ghost into
   a gameplay fact.
10. **Keep Core engine-free** (Invariant 1): contracts are attributes/data and plain interfaces —
    never a `Godot.*` type in Core.
11. **Determinism**: a required effect must be bound or absent deterministically — no "bound if the
    panel was opened first". Bind at setup, validate at setup.
12. **Tests**: gate self-tests (intentionally drop a binding → gate must fail), a completeness test
    that every declared port has an owner, and per-seam behaviour tests for the two pilots.
13. **Run the checklist** + `verify-fast.sh`.

**DoD:** the port manifest is generated, gated, and shrinking; `ApplyGrief`-class ghosts fail CI.

---

## Task 36B — Make every host session say what it needs

**Goal:** extend the contract from Core seams to host wiring: sessions declare required
collaborators (inventory, needs, roster, power, dice), and a campaign that starts with an unplugged
dependency refuses to start instead of running a subtly-empty game.

**Files:** `src/Host/*.cs` (62 save stores + ~40 host sessions), `src/Main.Application.cs`
(setup order), `src/Main.CampaignOwners.cs`, `src/Main.Lifecycle.cs` (session swap), new
`src/Host/HostSessionContracts.cs`, `Ashfall.Core.Tests/HostWiringContractTests.cs` (new),
`docs/architecture/PORT_CONTRACT.md`.

### Substeps

1. **Convert nullable dependencies to declared requirements** where a session cannot function
   without them (start with the ones already found: `WaterTreatmentHostSession.cs:16`'s optional
   inventory; any `= null!` field that is really "must be wired by Setup").
2. **Introduce `IWiringReporter`**: each host session exposes the ports it bound and the ports it
   still needs at end-of-setup (the combat `UnboundRequiredEffects` idea, generalised), so the audit
   is a queryable list rather than a code reading.
3. **Add an end-of-boot validation pass** in `Main.Application` (after all `SetupXxx`) printing a
   single table: subsystem / required / bound / missing, and a non-zero exit for the headless
   selftest when a required port is missing — the "compiles and boots but does nothing" class dies
   here.
4. **Cover the session-swap path** (`src/Main.Lifecycle.cs` rebuilds sessions on new game/load):
   validation must run again after a swap, because that is when a session gets rebuilt and a panel
   keeps a stale reference (Wave 1's 16B/16C both live here).
5. **Tie to the manifest** (28A): a subsystem whose manifest row declares a required authority but
   whose bind site doesn't reference it is a gate failure.
6. **Ban silent fallbacks in production**: keep the documented headless fallbacks (H1's
   `Survivors == null` path, seed catalogs) but require them to be *named* as fallbacks and reported
   by the validator, so "fallback active" is visible rather than the default assumption
   (Wave 3's 27A does the fixture half).
7. **Version-stale UI**: generalise `WaterTreatmentSystem`'s
   `expectedStateVersion/currentStateVersion` pattern into the shared action surface so a panel
   opened before a day advance cannot submit a stale command — a silent-state-corruption class, not a
   nicety.
8. **Report count over time**: publish the unbound-port count into `docs/CI.md`'s status table and
   the artifacts dir, so a rise is a reviewed decision.
9. **Tests**: validator fails on a deliberately-unbound port, passes on a complete boot, survives
   new-game and load, and stale-version commands are rejected with an attributable message.
10. **Docs**: `docs/architecture/PORT_CONTRACT.md` gains the host-side half plus the migration list.
11. **Run the checklist** + the exported-build boot smoke (Wave 3's 26B) — an exported boot is the
    best possible place to discover an unplugged dependency.

**DoD:** the host announces what it failed to wire, in one table, every boot.

---

## Task 36C — Close the four waves' long tail with the new machinery

**Goal:** use the gate to find the *rest* of the same class — systematically, once — instead of
discovering them in playtests.

**Files:** generated `docs/architecture/PORT_CONTRACT.md` findings, the `HOST_MISSING` list from
36A step 1, `artifacts/content-utilization.json`, `scripts/ci/generate-core-systems-catalog.py`,
`Ashfall.Core.Tests/*`, plus whatever wiring the findings require.

### Substeps

1. **Run the gate with `--report-only` first** and dump the complete offender list (Core seams,
   host ports, dead catalogs) — the deliverable of this task's first day is a table, not a diff.
2. **Group findings by the four waves' categories** (decision, physical, production, world) so each
   fix lands inside the plan that owns the concept, not as a scattershot patch.
3. **Wire or delete each `HOST_MISSING` seam** with a one-line rationale in the manifest;
   `DEAD` with zero references goes away in the same commit that adds a test proving the
   functionality exists elsewhere (or a note that it never did).
4. **Hunt sibling classes while you're in there**: methods shaped differently but with the same
   disease (public API with no caller) — extend the scanner to cover events never subscribed,
   `CaptureState` with no `RestoreState`, save stores never registered in `SaveSectionRegistry`,
   and panel routes without a live bind (15C's set).
5. **Content half**: re-run `--content-utilization-selftest` with runtime collection (Wave 3's 27C)
   so a catalog counted as "consumed" because a class merely names it stops passing — the
   `exempt_no_source_evidence` bucket (26 catalogs / 429 defs) is the standing proof that naming is
   not wiring.
6. **Data-field half**: fold Wave 1's 18C field-utilization tier into the same reporting pass, so
   "authored but unread" is one report, not three.
7. **Fix the docs the findings contradict** (Wave 3's 29B pattern): every registry row or
   `AGENTS.md` known-issue invalidated by this sweep gets a corrected line with a file:line.
8. **Set the target explicitly**: `HOST_MISSING` → 0; `EFFECT_PRODUCED` catalogs up by a stated
   number; unbound required ports → 0. Publish before/after in the wave close.
9. **Prove the gate catches a regression**: add a permanent test that introduces a fake unwired
   session in a sandbox and expects failure — a gate that has never failed is a rumour (Wave 3's
   27A step 11, repeated because it is the most-skipped step in any plan).
10. **Timebox**: whatever remains after the sprint gets a named owner and a plan number, not a
    TODO comment.
11. **Run the checklist** + `verify-fast.sh` + the export boot smoke.

**DoD:** the long tail of four audit waves is closed once, mechanically, with a report attached.

---

## Cross-Task Dependencies

```
35A (delivery ports) ──► 36A (declare + gate) ──► 36B (host-side wiring report) ──► 36C (sweep)
15C panel liveness (W1) ──┤                                ▲
27A fixture fidelity (W3) ┴────────────────────────────────┘
31A event kinds ──► every wired seam must also report (W4)
28A subsystem manifest ──► 36B step 5 (declared authorities vs bound ones)
```

**Execution order:** 35A → 36A → 36B → 36C. 36A before 36B (a gate needs a vocabulary before the
host reports against it); 36C last, because its value is finding what the first two couldn't predict.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. python3 scripts/ci/generate-port-contract.py --check          # (new gate)
7. godot --headless --path . -- --panel-bind-lifecycle-selftest  # unbound-port table printed
8. godot --headless --path . -- --content-utilization-selftest   # naming ≠ wiring
9. bash scripts/ci/verify-fast.sh
10. regression proof: the gate fails a deliberately unwired port
```

---

## Estimated Effort & Risk

| Task | Core | Host | Tooling | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 36A | triage 147 / wire 2 | 2 | 1 generator + 1 gate | 6–9 | Medium | LOW (ships with a dated exemption list) |
| 36B | 0 | ~40 sessions (declared deps) | validator | 8–12 | Medium–High | MEDIUM (loud failures in dev — intended, but must be clear) |
| 36C | varies | varies | reports | 5–10 + ratchet | High (open-ended) | LOW–MED per item |

**Guardrails:** never weaken a gate to pass it; never delete a seam because it is unwired without
first asking whether the behaviour it promised is wanted (grief and sedation are — they were authored
*and tested*); no `Godot.*` in Core (Invariant 1); no new DI container or framework — attributes,
one generated table, one validator, matching the existing `generate-*.py --check` family.
