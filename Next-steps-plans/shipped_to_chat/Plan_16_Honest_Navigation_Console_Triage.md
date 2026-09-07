# Plan 16 — Honest Navigation: Console Triage & Campaign Authority

> **Wave:** Continuity Wave 1
> **Depends on:** nothing to start; `15C` (liveness gate) makes the triage list official.
>
> **Theme:** the menu advertises more game than exists. **30** routed "flagship consoles"
> render literal telemetry and buttons that only print text; five other routed panels are
> bound to *freshly constructed* systems instead of the campaign's live ones. This plan makes
> the reachable set truthful, gives every surviving console a real authority, and fixes the
> subscription defect that makes reopened panels drift. No new consoles are designed here —
> that is explicitly forbidden until this plan closes.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| Finding | Evidence | Status |
|---|---|---|
| 30 panels self-declare as bound | `grep -rl "IsBound { get; private set; } = true" src/UI/ \| wc -l` → **30** | CONFIRMED |
| Those panels' buttons mutate nothing | `src/UI/AnaerobicBiogasDigesterPanel.cs:88–100` — four `Pressed += () => ShowFeedback("…")` handlers over hard-coded numbers | CONFIRMED |
| Same shape in the other offenders | `CryogenicPermafrostCorePanel`, `WarDogKennelPanel`, … (full list in BUG-UI-002 §7) | CONFIRMED |
| Routed panels bound to throwaway authorities | `src/Main.PlayerSurfaces.cs` — `fire_incident` binds `new Ashfall.Core.Shelter.ShelterFireHazardSystem(), "inc_default"`; `faction_matrix` and `factions_narrative` each bind `new Ashfall.Core.Economy.FactionStanceEngine()`; `skill_matrix` binds `new Ashfall.Core.Survivors.SkillProgressionSystem()`; `weather_sonde` binds `new WeatherHostSession(...)` | CONFIRMED by source read |
| Literal fixture ids in production routes | `"inc_default"`, `"tag_1"` (geiger_calibration), `"sig_distress"` (triangulation), `"sv_cohort_demo"` (`src/Dose/DoseRegisterSurface.cs:319`), `_survivors?.…?.Id ?? "surv_01"` (`Main.PlayerSurfaces.cs:298`) | CONFIRMED |
| Lambda-unsubscribe no-ops | `src/UI/TriangulationPanel.cs:44` `-=` a freshly allocated lambda, `:52` `+=` another, `:53` `OnLocationRevealed` never removed, `:187` `-=` yet another new lambda → handlers accumulate | CONFIRMED by source read |
| Same defect class in 3 more panels | `WeatherHistoryPanel`, `GeigerCalibrationPanel`, `FireIncidentPanel` (BUG-UI-003) | CONFIRMED (static, high confidence) |
| 5,186 lines are involved | BUG-UI-002 evidence | do **not** rewrite; classify and cut |

**Reading of the evidence:** the failure is not "UI is ugly" — it is *presence mistaken for
capability*. Every fix below is subtractive or ownership-corrective. The audit's root-cause
cluster #1 says exactly this.

---

## Task 16A — Triage the 30 consoles: live, prototype, or out of navigation

**Goal:** a player who opens any routed panel meets either a real system or nothing at all.
Zero new gameplay required; this is the cheapest large continuity win in the wave.

**Files:** `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`, `Assets/Ashfall.Core/UI/PanelRegistry.cs`,
`src/Main.PlayerSurfaces.cs`, `src/Main.UiPanels.cs`, `docs/ui/SURFACE_GAP_REPORT.md`,
`Ashfall.Core.Tests/PlayerSurfaceCoverageGateTests.cs`, 30 files in `src/UI/`.

### Substeps

1. **Produce the triage sheet first** (no code): one row per console —
   `panelId | Core authority? | host session? | mutating action? | persisted section? | verdict`.
   Verdicts are `LIVE`, `BACKIT` (authority exists, panel is a shell), `SHELVE` (no authority),
   `MERGE` (overlaps a live panel), `DELETE`.
2. **Add a registry field** for the verdict (`PanelMaturity` enum or a bool `PlayerNavigable`)
   rather than deleting descriptors — descriptors keep the composition-root test enumerating
   every class, which is valuable.
3. **SHELVE = remove from player routing, keep the class.** Drop the `ConfigureActions` route in
   `src/Main.PlayerSurfaces.cs` (the dynamic loop around `:525–528` is the shared mechanism) and
   leave the panel constructible for preview/tests. Nothing is deleted; nothing is rewritten.
4. **Add one shared "under repair" surface** — a single small non-panel string used by every
   shelved console's journal/echo entry, e.g. a line the journal already supports. Never a dead
   button, never a blank screen. If a shelved console is referenced by authored narrative, spend
   that reference in the journal instead.
5. **BACKIT candidates first:** for each console whose Core authority *does* exist (check
   `SaveSectionRegistry` and the 19 `_campaignDay.Register(...)` owners before assuming),
   rebind the panel to the campaign instance and mark the fake literals TODO for 16B. Prefer
   5 rebindings over 25 new systems.
6. **MERGE candidates:** where two consoles describe one system (e.g. `factions` /
   `faction_matrix` / `factions_narrative`) consolidate onto the live surface and shelve the rest.
7. **Spend the shelf list on the plan backlog**: the shelved set is now a *prioritized* feature
   list with named authorities — hand it to Plan 14's expansion owners so future expansion work
   picks consoles that already have data and a save section.
8. **Gate the nav count**: change the coverage assertion from "every descriptor has a route" to
   "every `PlayerNavigable` descriptor has a live route" (see 15C gate shape) so the number of
   promises the player can keep is the reported number.
9. **Update snapshots**: `docs/ui/SNAPSHOT_COVERAGE.md` and the snapshot manifest — shelved
   consoles move to a `PROTOTYPE` section, not silently dropped, so regression history survives.
10. **Save-state safety audit**: confirm no shelved console owns a save section that becomes
    orphaned-but-written; if one does, keep it in the envelope (do not delete sections) and note
    it in `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`.
11. **Re-run `--content-utilization-selftest`** and record the console-related delta.
12. **Documentation truth pass**: correct `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` where it
    overstates these capabilities (that doc already has two known stale claims, e.g. the
    `AirlockSecuritySystem.cs:80` GetHashCode issue, which no longer exists in source) and record
    `§26 PARTIAL/STUB` additions for each shelved console.
13. **Run the five-step verification checklist** + `bash scripts/ci/triad-drift-gate.sh`.

**DoD:** the count of player-openable panels equals the count of panels that act on real state.
Report both numbers.

---

## Task 16B — One authority per fact: repoint disconnected bindings to the campaign

**Goal:** eliminate "panel shows a freshly constructed system's defaults". After this, every
routed panel reads and writes the *same* object the day loop ticks and the save writes.

**Files:** `src/Main.PlayerSurfaces.cs`, `src/Main.UiPanels.cs`, `src/Main.CampaignOwners.cs`,
`src/Main.Lifecycle.cs`, `src/UI/FireIncidentPanel.cs`, `src/UI/FactionMatrixPanel.cs`,
`src/UI/FactionsNarrativePanel.cs`, `src/UI/SkillMatrixPanel.cs`, `src/UI/WeatherSondePanel.cs`.

### Substeps

1. **Publish an ownership table** (docs, one page) mapping each of the five offenders to its
   single campaign owner: which `Setup*` constructs it, which `_campaignDay.Register` owner
   ticks it, which `Save*` captures it. This is the Setup/Save/Flush triad made legible.
2. **Introduce a provider seam, not a parameter storm**: in `PanelDescriptor.BindAction` calls,
   resolve authorities through one small host accessor (e.g. `RequireAuthority<T>()`) that
   throws a *descriptive* error if the campaign instance is absent. A wrong panel must fail
   loudly in dev, not show defaults in a playtest.
3. **`fire_incident`**: find the campaign fire/hazard owner (search `Main.ExpandedShelterSystems.cs`,
   `Main.ShelterInfrastructure.cs`, phase0). Bind the real one; if the campaign genuinely has no
   fire system today, shelve the route in 16A rather than fabricate one here.
4. **`faction_matrix` + `factions_narrative`**: both currently build their own
   `FactionStanceEngine`. Replace with the campaign engine, and *remove the duplicated instance*
   so stance changes from Plan 15B actually appear. This is the clearest missing-link fix in the
   plan.
5. **`skill_matrix`**: bind the live `SkillProgressionSystem` (and the co-constructed
   `SkillAtrophySystem` via `SurvivorSocialCoordinator`) instead of a new one, so atrophy from
   missed shifts is visible.
6. **`weather_sonde`**: stop constructing `new WeatherHostSession(...)` per open; reuse the
   world host session, and confirm the sonde's readings come from the same `WeatherSystem` the
   forecast panel and the expedition risk estimate use.
7. **Kill literal fixture ids in production routes**: replace `"inc_default"`, `"tag_1"`,
   `"sig_distress"`, `"sv_cohort_demo"`, `?? "surv_01"` with values taken from live state, and
   where a demo target is genuinely intended, route it through a `--*-selftest` verb instead of
   the player panel.
8. **Session-swap safety**: `src/Main.Lifecycle.cs` nulls/rebuilds sessions on new game and on
   load (`_onboardingJourney = null!`, etc.). Add a rebind pass so every open panel re-resolves
   its authority after a session swap; a panel holding a dead session is the same bug as a panel
   holding a fresh one.
9. **Reference-identity assertions**: add host-level checks
   (`ReferenceEquals(panel.Authority, campaign.Authority)`) for the five panels — cheap, and it
   pins the fix permanently.
10. **Determinism pass** on anything reseeded by the old `new X()` constructions: the audit
    flagged a `String.GetHashCode()`-derived seed lurking in the fire route. Reuse the campaign's
    `ICampaignRngManager` / `ISeededRng`; never seed from a hash of a string.
11. **Tests**: for each repointed panel, one test per claim — "panel shows campaign value after
    the day loop mutates it" and "action from the panel is visible to the day loop next day".
    Round-trip: act → save → load → panel still correct.
12. **Run the checklist**, and re-run `bash scripts/ci/verify-fast.sh` (14 gates) since this task
    touches the composition root.

**DoD:** zero routed panels construct a Core system or host session at bind time.

---

## Task 16C — Subscription identity: reopen a panel a hundred times, drift zero

**Goal:** fix and permanently prevent duplicate/stale event handlers in panels that rebind.

**Files:** `src/UI/TriangulationPanel.cs`, `src/UI/WeatherHistoryPanel.cs`,
`src/UI/GeigerCalibrationPanel.cs`, `src/UI/FireIncidentPanel.cs`,
`src/Host/PanelBindLifecycleSelfTest.cs` (locate or extend the existing probe),
`Ashfall.Core.Tests/PanelSubscriptionHygieneTests.cs` (new).

### Substeps

1. **Prove it first** with a runtime probe before touching code: bind one of the four panels
   twice and count refreshes per event (an instance counter or a `RefreshCount` test hook).
   Record the failing number in the commit message.
2. **Fix the pattern once, everywhere**: hold the handler in a private field
   (`_onStateChanged = _ => RefreshView();`) and subscribe/unsubscribe *that* delegate. Never
   allocate a lambda at the unsubscribe site.
3. **Fix every missing unsubscribe** in the same files, not just the ones named in the audit —
   `TriangulationPanel.cs:53` leaves `OnLocationRevealed` subscribed with no removal at all.
4. **Centralise it**: add a tiny base/helper (`BindOnce`/`UnbindOnce` pair, or a
   `SubscriptionBag` disposed on rebind) in `src/UI/` so the next 140 panel classes inherit the
   correct behaviour instead of re-deriving it. Keep it host-side; do not put Godot-free
   bookkeeping into Core.
5. **Lifecycle symmetry**: subscribe in `Bind`/`_Ready`, unsubscribe in `Unbind`/`_ExitTree`.
   Document the convention in `docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md`.
6. **Static gate**: source-scan `src/UI/*.cs` for `-=` followed by a lambda literal
   (`-=> _ =>`, `-=> (`, `-=> id =>`) and fail CI. Same "gate scans source" idiom as
   `SaveStoreCoverageGateTests`, so it needs no new tooling concept.
7. **Repeated-bind runtime test**: extend the panel-lifecycle selftest to
   bind → unbind → rebind × N, then assert a single refresh per event and no handler held on a
   disposed session.
8. **Load-session leak check**: after `New Game` → play → `Load`, assert `GetTree().GetNodeCount()`
   and handler counts return to baseline (pairs with `docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md`).
9. **Sweep the remaining 136 panel classes** with the new static gate; triage any hits found
   beyond the four (expect some; add them to this task's DoD count, don't open a new plan).
10. **Snapshot re-capture only if layout changed** — follow
    `docs/ui/SNAPSHOT_FIXTURE_POLICY.md`; a subscription fix must not cause golden-image churn.
11. **Run the checklist** plus the audio selftest (`--audio-selftest`) since event wiring churn
    can double-fire cues.

**DoD:** the four known panels pass ×100 reopen; the static gate has 0 outstanding hits; a
reopened panel can never refresh twice per event.

---

## Cross-Task Dependencies

```
16A (triage: decide what is real) ──► 16B (repoint the survivors to campaign authority)
        │                                    │
        └── 15C gate supplies the list ──────┴──► 16C (subscription hygiene on surviving panels)
                                                 │
                                                 └──► Plan 17A needs stable, live panels to
                                                      publish cause/effect into
```

**Execution order:** 16A → 16B → 16C. Do not repoint bindings (16B) for panels that 16A will
shelve; and do not add subscription helpers (16C) to panels being deleted.

**Conflict warning:** 16B edits the same `src/Main.PlayerSurfaces.cs` region as 15A/15B.
Sequence: 15A → 16A → 16B → 15B → 16C.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. bash scripts/ci/triad-drift-gate.sh
7. godot --headless --path . -- --content-utilization-selftest   # record delta
8. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Files touched | New code | Deleted routes | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 16A | ~12 (registry + routes + docs) | small | 15–25 routes | 4–6 + gate | Low–Med | LOW (subtractive, classes retained) |
| 16B | ~9 | small | 5 fresh constructions | 8–12 | Medium | MEDIUM (composition root + save identity) |
| 16C | ~6 + helper | small | 0 | 3–5 + static gate | Low | LOW |

**Explicitly out of scope:** new consoles, new themes, new layout systems, Stitch redesigns of
the shelved panels (`google-stitch` output for a shelved console is wasted until it has an
authority). If a shelved console later gets a real system, it returns through Plan 14-style
expansion work with a save section and a liveness gate already satisfied.
