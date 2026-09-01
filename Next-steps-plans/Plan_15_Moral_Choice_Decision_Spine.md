# Plan 15 — The Decision Spine: Make Moral Choice Playable

> **Wave:** Continuity Wave 1 (Plans 15–19)
> **Predecessor:** none required — this plan closes the single largest dead-end in the game.
>
> **Theme:** ASHFALL is *about* moral choices under radiation, and the moral system is the
> one spine every other system hangs from (guilt → insomnia → foundry accidents → dose →
→ triage → epilogue). Today the player cannot make one. This plan opens the valve, fans the
> consequences out to systems that already read them, and then builds the gate that stops the
> "panel exists ⇒ feature ships" lie from recurring.

---

## Verified Baseline (re-run before starting)

| Check | Command | Recorded state @ `ccac926e` |
|---|---|---|
| Host build | `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| Core tests | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 5303 passed, 0 failed |
| Catalogs | `godot --headless --path . -- --data-integrity-selftest` | PASS — 138 catalogs, 5563 ids, 0 errors |

## Evidence Inventory (all re-verified against source at `ccac926e`)

| Claim | Evidence | Verified how |
|---|---|---|
| The moral-choice resolver is never called | `src/Main.MoralChoice.cs:91` — `private bool TryResolveMoralChoice(string questId, int choiceIndex)`; repo-wide grep returns the declaration **and nothing else** | `grep -rn "TryResolveMoralChoice" --include=*.cs .` |
| Core moral state *is* live and persisted | `src/Main.MoralChoice.cs:61–65` subscribes `OnQuestResolved`, `OnThresholdEventFired`, `OnBranchLocked` → journal entries + dirty flags; state rides the campaign envelope | source read |
| The player-facing moral view is read-only | `src/UI/FactionsPanel.cs:315` — "weight of choices" progression display only | `docs/debug/10LOOP_player_ui_ux_BUG_AUDIT.md` BUG-UI-001 (CONFIRMED, active path) |
| Choice-driven downstream systems exist and wait for input | `GuiltInsomniaSystem` reads guilt records; `EpilogueMatrixRuntime` consumes flags; atlas §11 lists Guilt State as **High-Leverage (multi-system impact)** | `docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md` §11 |
| Quest detail surface is already routed | `quest_detail` registered `PanelGroup.Secondary` in `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`, routed in `src/Main.PlayerSurfaces.cs`, `PanelRouteGateTests` enforces parity | source read |
| Encounter choice resolution *does* work elsewhere (template to copy) | `src/Host/ExpeditionHostSession.cs:432` `_bridge.ResolveChoice(...)`, `src/Main.YearOfAsh.cs:300`, `src/Host/DutyRosterHostSession.cs:151` — three working choice-commit paths to imitate | grep |

**Interpretation:** this is a *wiring* gap, not a design gap. Three sibling systems already
implement "present choices → commit → apply effects → journal it". Moral choice is the odd one
out. Do not design a new choice mechanic.

---

## Task 15A — Open the valve: one player route that commits a moral choice

**Goal:** a player can, inside a normal campaign session, select a moral choice, have it
persist, and see it in the journal — with zero new Core mechanics.

**Files:** `src/Main.MoralChoice.cs`, `src/UI/QuestDetailPanel.cs`, `src/UI/FactionsPanel.cs`,
`src/Main.PlayerSurfaces.cs`, `src/Main.UiPanels.cs`, moral/quest catalogs (read-only).

### Substeps

1. **Read** `src/Main.MoralChoice.cs` end to end (it is small) and write down, in the task
   comment, the exact contract `TryResolveMoralChoice(questId, choiceIndex)` expects: where the
   quest id comes from, what `choiceIndex` indexes into, what it returns, what it marks dirty.
2. **Read** the three working choice paths (`ExpeditionHostSession.cs:432`,
   `Main.YearOfAsh.cs:300`, `DutyRosterHostSession.cs:151`) and copy their shape —
   *validate → resolve → journal → flush* — rather than inventing a fourth idiom.
3. **Find the catalog shape**: locate the moral-choice definitions actually loaded by
   `SetupMoralChoice()`, and confirm each definition carries a choices array with stable ids and
   authored outcome text. Record the field names verbatim; the UI must not invent labels.
4. **Promote the resolver**: make `TryResolveMoralChoice` reachable from UI by exposing a thin
   public host method (`ResolveMoralChoice(string questId, int choiceIndex) : bool`) on the
   existing `Main` partial — keep the private implementation intact, no signature churn.
5. **Add a presentation read-model** to the moral host session: a method that returns, for one
   quest id, the list of `{choiceLabel, stakesSummary, isAvailable, unavailableReason}` rows,
   so the panel never reaches into `MoralChoiceSystem` internals (keeps Invariant 5: hosts stay thin).
6. **Wire the buttons in `QuestDetailPanel`**: render one action row per available choice using
   the existing `AshfallUiHelpers.MakeDataRow` / `MakeActionButton` helpers and the established
   console style — no new visual language, no modal framework.
7. **Wire the click** → host `ResolveMoralChoice(...)` → on success, refresh the panel, on
   failure show the reason in the panel's existing status line (never a silent no-op).
8. **Guard irreversibility in the UI**: a resolved moral quest must render its locked state
   explicitly (choices disabled + "recorded" marker), because the branch-lockout event already
   exists in Core and the player must not be able to re-pick.
9. **Confirm the payoff lands**: after a choice, verify in-session that (a) the journal entry
   from `OnQuestResolved` appears, (b) `_moralChoiceDirty` is set, (c) a subsequent
   `SaveAll()` + reload reproduces the resolved state.
10. **Play a confirmation cue** — reuse `AudioCueCatalog.UiConfirm` (already wired in
    `src/Main.GameFlow.cs`) so the irreversible act has weight; do not register a new cue.
11. **Add one UI-level test** to the existing `Main.UiTests.*` triad that opens the panel,
    resolves a choice, and asserts the Core state changed (this is the journey test the audit
    records as missing).
12. **Add a headless verb** only if needed for CI; prefer extending an existing selftest over
    creating a new `--moral-choice-selftest` (see `ashfall-headless-demo` guidance).
13. **Run** `dotnet build Ashfall.csproj`, `dotnet test`, `bash scripts/ci/triad-drift-gate.sh`.

**DoD:** a player reaches a moral choice with mouse/keyboard in a real campaign, commits it,
cannot un-commit it, sees the journal entry, and the choice survives save/load.

**Next steps this unlocks:** 15B (fan-out), 15C (gate), Plan 17A (the choice must appear in the
day-advance cause/effect feed), Plan 18A (narrative consequence).

---

## Task 15B — Fan the consequence out: choices must move other systems

**Goal:** make one resolved choice visibly change the rest of the simulation, using only
systems that already read moral/guilt state. This is the "missing link" the whole wave is about.

**Files:** `Assets/Ashfall.Core/MoralChoice*` (read-first), `GuiltInsomniaSystem`,
`Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs`, `src/Main.MoralChoice.cs`,
flag ledger, moral choice data JSON.

### Substeps

1. **Map current fan-out** before editing: list every consumer of moral-choice output today
   (journal writer, threshold events, branch lockouts) and every consumer of *guilt* records
   (`GuiltInsomniaSystem`, needs/fatigue, memorial). Produce a one-page table in the PR body.
2. **Identify the orphans** — consequences authored in the moral catalog that no system reads
   (faction stance shifts, morale deltas, knowledge unlocks, epilogue flags). Classify each as
   `LINK`, `DELETE`, or `LATER`; never leave a written consequence unread.
3. **Link guilt first** (highest leverage per atlas §11): on `OnQuestResolved`, produce a guilt
   record with the authored severity/archetype so insomnia and fatigue start moving. Reuse the
   existing guilt-source vocabulary — do not add a second guilt representation.
4. **Link faction stance**: route authored stance deltas into the *campaign-owned*
   `FactionStanceEngine` (not a fresh instance — see Plan 16B), so a moral act changes how a
   faction trades and talks to you.
5. **Link morale** through whichever morale path the shelter already uses (decor/schedule/
   vinyl) instead of adding a new morale channel.
6. **Link the ending**: ensure each resolved choice writes the flags
   `EpilogueMatrixRuntime` reads. Assert the 32-permutation matrix is still reachable in
   every direction (no choice silently collapses the ending space).
7. **Add the feedback seam**: raise a `DayStateChangeEvent`-shaped record (see
   `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs:355`) for each consequence, so
   Plan 17A can display "you chose X → Y changed" for free.
8. **Determinism**: any seeded roll in consequence resolution must use `ISeededRng`; no
   `System.Random`, no `Guid.NewGuid()`, no parameterless `string.GetHashCode()` (Invariant 4).
9. **Save contract**: confirm the moral section round-trips the new consequence fields; if a
   DTO shape changes, bump the codec path (V1→V2→V3 discipline) and extend the
   `SaveWireContract`-style assertions rather than editing the pinned checksum test casually.
10. **Data sweep**: verify every moral choice id, guilt archetype, and flag id referenced by
    the fan-out resolves — `--data-integrity-selftest` must stay at 0 errors.
11. **Tests (behaviour):** choice → guilt record exists; choice → stance delta applied;
    choice → morale change; choice → epilogue flag set; locked branch cannot re-resolve.
12. **Tests (save):** capture → mutate → restore → assert for the whole chain; paired-seed
    replay proving identical consequences from identical choices.
13. **Balance probe:** run `ashfall-balance-sim` (or the existing dose/needs sweep verbs) over
    a 60-day scripted campaign with a fixed choice policy, and record whether the fan-out is
    meaningful but non-dominant; paste numbers into the plan log.
14. **Run the full five-step verification checklist.**

**DoD:** one decision, four visible consequences, all persisted, all deterministic, all gated.

---

## Task 15C — Kill the lie: a liveness gate for routed panels

**Goal:** make it structurally impossible to route a panel to the player that cannot act on
the real campaign. Without this, Plans 16–19 keep regenerating the same class of gap.

**Files:** `Ashfall.Core.Tests/*PanelRouteGateTests*`, new
`Ashfall.Core.Tests/PanelLivenessGateTests.cs`, `Assets/Ashfall.Core/UI/PanelRegistry.cs`,
`Assets/Ashfall.Core/UI/PanelDescriptor`, `docs/ui/SURFACE_GAP_REPORT.md`.

### Substeps

1. **Instrument the descriptor**: add a liveness marker to `PanelDescriptor` — e.g.
   `bool HasLiveBinding` / `bool HasMutatingAction` — set by the host at
   `ConfigureActions(...)` time rather than asserted by the panel class.
2. **Prohibit self-declared bound state**: ban `IsBound { get; private set; } = true`
   (found in **30** `src/UI` panel files) by making `IsBound` a computed property of a bound
   authority reference. Keep it source-compatible where possible.
3. **Gate A — mutating action**: a routed panel must expose ≥1 action whose handler calls into
   the injected authority. Detect statically by source-scanning the panel file for a handler
   that only calls `ShowFeedback`/writes a `Label.Text` (the
   `AnaerobicBiogasDigesterPanel.cs:88–100` shape) and failing.
4. **Gate B — no fresh authorities**: reject any `ConfigureActions` bind expression containing
   `new <SystemType>(` where a campaign owner for that type exists. (Offenders to be fixed in
   Plan 16B: `fire_incident`, `faction_matrix`, `factions_narrative`, `skill_matrix`,
   `weather_sonde`.)
5. **Gate C — no literal fixtures in production routes**: reject hardcoded ids in bind
   lambdas (`"inc_default"`, `"tag_1"`, `"sig_distress"`, `"surv_01"`) unless whitelisted as
   preview-only with a reason string.
6. **Mirror the existing coverage-gate idiom**: `SaveStoreCoverageGateTests` source-scans
   `src/**/*SaveStore*.cs` and fails on non-delegation — copy that exact pattern for panels so
   the team learns one gate style, not two.
7. **Baseline the debt honestly**: land the gate with an explicit, reviewed exemption list of
   the currently-failing panels; the list is the input to Plan 16A. An empty gate with 0
   exemptions would either block the build or be silently loosened later.
8. **Shrink exemptions, never grow them**: assert in the test that exemption count ≤ recorded
   baseline, so any new fake panel fails CI immediately.
9. **Add the journey probe**: extend the composition-root UI test (`Main.UiTests.CompositionRoot.cs`
   already opens every `PanelRegistry.AllIds`) to additionally assert each open caused a
   non-default bind — currently it proves "opens without throwing", which is exactly the weak
   bar BUG-UI-002 slipped past.
10. **Update docs**: rewrite the `SURFACE_GAP_REPORT.md` verdict column to
    `LIVE / PROTOTYPE / DECOY / REMOVED` so no future agent counts decoys as shipped UI.
11. **Re-run content utilization**: `godot --headless --path . -- --content-utilization-selftest`
    and record the new `EFFECT_PRODUCED` / unresolved counts as the wave's leading metric.
12. **Run the five-step checklist** plus `bash scripts/ci/verify-fast.sh`.

**DoD:** a new routed-but-dead panel fails CI on the day it is added, and the honest count of
live surfaces is published.

---

## Cross-Task Dependencies

```
15A (valve) ──► 15B (fan-out) ──► feeds Plan 17A (cause/effect feed)
     │                              feeds Plan 18A (narrative consequence)
     └──────────► 15C (liveness gate) ──► unblocks Plan 16A triage list
```

**Execution order:** 15A → 15C → 15B. Land the gate (15C) before the fan-out (15B) only if
15A is merged; 15B changes the most files and needs the gate to keep it honest. If capacity is
tight, 15A alone already changes how the game plays.

**Do not** start this plan concurrently with Plan 16B — both touch `src/Main.PlayerSurfaces.cs`
bind lambdas. Sequence them.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. bash scripts/ci/triad-drift-gate.sh                           # Setup/Save/Flush parity
7. bash scripts/ci/verify-fast.sh                                # full 14-gate local mirror
```

---

## Estimated Effort & Risk

| Task | Core | Host | UI | Data | New tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 15A | 0 | 1 file | 2 files | 0 | 3–5 | Low–Med | LOW (additive route) |
| 15B | 2–3 files | 1 | 0 | 1–2 | 10–14 | Medium | MEDIUM (save shape) |
| 15C | 1 (descriptor) | scan-only | 30 (report only) | 0 | 5–8 + gate | Medium | LOW (gate ships with exemption baseline) |

**Guardrails:** no new mechanic, no new panel framework, no new bus/cue, no redesign of the
console visual language. Cross-tool QA rule applies to 15B (≥2 new coupled variables:
guilt-severity × stance-delta) — implement in one tool, review in another.
