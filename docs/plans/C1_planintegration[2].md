# C1 — Flagship Integration Plan [2]: Honest Navigation, Campaign Authority & Panel Lifecycle Integrity

> **Output:** `C1_planintegration[2].md`
>
> **Source baseline:** Plan 16 — Honest Navigation: Console Triage & Campaign Authority
>
> **Wave:** Continuity Wave 1
>
> **Primary execution sequence:** 16A → 16B → 16C
>
> **Cross-plan sequencing constraint:** 15A → 16A → 16B → 15B → 16C where the shared `Main.PlayerSurfaces.cs` region would otherwise conflict.
>
> **Implementation posture:** subtract false capability first, repair ownership second, harden lifecycle third. Do not design new consoles in this plan.

---

## 0. Mission

This plan closes a continuity defect in the player-facing UI layer: routed surfaces imply that gameplay capability exists even when the panel is a fixture shell, a no-op telemetry view, a throwaway system instance, or a subscription leak waiting to accumulate handlers.

The goal is not to make every registered panel "more complete." The goal is to make every **player-navigable** panel truthful.

A routed surface must satisfy all of the following:

1. It is backed by a real campaign authority or a deliberate host session that belongs to the active campaign.
2. It reads the same state the day loop ticks.
3. Its mutating actions affect the same state the save system captures.
4. It can survive new-game/load session replacement without holding a dead authority.
5. It binds and unbinds event handlers symmetrically.
6. Reopening the panel does not increase refresh count per event.
7. It does not depend on production fixture IDs such as demo survivor IDs, canned signal IDs, or synthetic incident IDs.
8. It is either player navigable and real, or non-player-navigable and explicitly classified as prototype/shelved.
9. Documentation and coverage gates report the truthful live-surface count.

The plan is complete when the UI surface registry represents **capability truth**, not class presence.

---

## 1. Source Evidence Baseline

The source plan identifies three separate defect classes.

### 1.1 False-capability routing

Thirty panels reportedly self-declare as bound while at least a known sample of their buttons only call feedback text over hard-coded values. The corrective strategy is classification and subtraction, not a 30-panel rewrite.

### 1.2 Authority fragmentation

Five routed panels are called out as binding freshly constructed systems or sessions rather than the campaign-owned instances:

- `fire_incident`
- `faction_matrix`
- `factions_narrative`
- `skill_matrix`
- `weather_sonde`

This violates the one-authority-per-fact rule.

### 1.3 Subscription identity defects

At least these panels are identified with broken lambda unsubscribe or missing unsubscribe behavior:

- `TriangulationPanel`
- `WeatherHistoryPanel`
- `GeigerCalibrationPanel`
- `FireIncidentPanel`

The repair must become a reusable lifecycle convention and CI gate, not four isolated patches.

---

## 2. Non-Negotiable Architectural Invariants

### INV-16.1 — One authority per fact

A player-facing fact may have many views but one campaign authority.

Forbidden:

```csharp
descriptor.BindAction = panel =>
    panel.Bind(new FactionStanceEngine());
```

Required shape:

```csharp
descriptor.BindAction = panel =>
    panel.Bind(RequireAuthority<FactionStanceEngine>());
```

The exact accessor name may differ, but the ownership rule may not.

### INV-16.2 — Player navigation is a promise

`PanelRegistry` may retain descriptors for previews, snapshots, tests, prototypes, and future work. Only descriptors explicitly classified as player navigable may appear in live routing.

### INV-16.3 — No silent fallback authority

If a required campaign authority is absent in a development build, fail with a descriptive error. Do not construct a substitute system and do not render plausible defaults.

### INV-16.4 — No production fixture IDs

Literal IDs used only to make a panel "work" in a demo must be moved to explicit selftest/demo paths.

### INV-16.5 — Lifecycle symmetry

Every subscription site has a semantically identical unsubscribe site using the same delegate identity.

### INV-16.6 — Session swap invalidates bindings

New game and load are authority-generation boundaries. Panels must re-resolve their authority after a session swap.

### INV-16.7 — Subtractive repair before additive implementation

If a console has no authority, shelve it. This plan does not create an authority merely to preserve a route.

---

## 3. Definition of Done

The plan closes only when all of these statements are true:

- player-openable panel count equals live-capability panel count;
- zero player-routed panels construct a Core authority at bind time;
- zero player-routed panels construct a replacement host session at bind time;
- the five named authority offenders are either bound to campaign instances or shelved;
- literal production fixture IDs named by the source plan are removed from live routes;
- the four known subscription offenders survive 100 bind/unbind/rebind cycles with one refresh per event;
- the static lambda-unsubscribe gate reports zero hits;
- save/load/new-game panel journeys rebind to the active campaign;
- shelved panels remain constructible for preview/tests unless explicitly deleted by verdict;
- snapshot and canon documentation distinguishes live from prototype surfaces;
- the complete verification suite passes.

---

# PHASE P0 — Baseline Capture Before Editing

## P0.1 Freeze the observed revision

Record:

- commit SHA;
- branch;
- dirty-file count;
- `dotnet --info`;
- Godot version;
- current player-navigable panel count;
- current panel descriptor count;
- current `IsBound = true` count;
- current source-scan hits for `new .*System` in player-surface binding code;
- current source-scan hits for lambda unsubscribe patterns.

Do not compare after-state metrics against memory.

## P0.2 Read ownership and routing files

Read completely before touching code:

- `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`
- `Assets/Ashfall.Core/UI/PanelRegistry.cs`
- `src/Main.PlayerSurfaces.cs`
- `src/Main.UiPanels.cs`
- `src/Main.CampaignOwners.cs`
- `src/Main.Lifecycle.cs`
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`
- campaign-day registration/composition code
- coverage gates around player surfaces
- snapshot manifest and policy documentation

## P0.3 Capture route inventory

Generate a machine-readable CSV/Markdown table:

```text
panel_id
descriptor_class
route_source
player_navigable_today
is_bound_default
bind_action
authority_type
authority_origin
mutating_action_present
save_section
day_tick_owner
fixture_ids
subscription_count
verdict
notes
```

This becomes the artifact used by 16A and 16B.

## P0.4 Reproduce one authority defect

Before fixing, open or selftest one disconnected surface and prove:

```text
ReferenceEquals(panel authority, campaign authority) == false
```

Save the failing output.

## P0.5 Reproduce one subscription defect

Bind a named panel twice, fire one authority event once, count refreshes.

Expected pre-fix evidence: refresh count > 1 or a stale handler remains attached.

---

# TASK 16A — Triage the 30 Consoles: LIVE, BACKIT, SHELVE, MERGE, DELETE

## 16A.0 Objective

Reduce the player-facing navigation graph to surfaces that correspond to real capabilities. Preserve descriptors when useful for previews/tests so architecture coverage remains broad without lying to the player.

## 16A.1 Build the authoritative triage sheet

For every candidate console, record:

| Field | Required meaning |
|---|---|
| `panelId` | Registry identity |
| Core authority | Existing system that owns the fact |
| host session | Existing live host wrapper, if any |
| mutation | At least one action that changes campaign state |
| persistence | Save section or deliberate derived state |
| day loop | Tick owner if time-dependent |
| production fixture IDs | Any hard-coded sample IDs |
| UI data source | live read model vs literals |
| verdict | LIVE/BACKIT/SHELVE/MERGE/DELETE |
| rationale | One sentence tied to evidence |

### Verdict rules

**LIVE**
- already bound to campaign truth;
- action semantics are real;
- persistence/lifecycle are coherent.

**BACKIT**
- authority already exists;
- panel is a shell or bound incorrectly;
- fixing it requires wiring, not inventing gameplay.

**SHELVE**
- no current authority;
- no justified near-term backing;
- keep class/descriptor for previews/tests if valuable.

**MERGE**
- duplicates another live surface around the same authority;
- preserve one canonical player route.

**DELETE**
- duplicate, obsolete, misleading, and no preview/test value;
- deletion requires evidence and dependency scan.

## 16A.2 Add maturity/navigation metadata

Prefer an explicit enum:

```csharp
public enum PanelMaturity
{
    Live,
    BackingRequired,
    Prototype,
    Merged,
    Retired
}
```

And derive navigation:

```csharp
public bool PlayerNavigable => Maturity == PanelMaturity.Live;
```

Alternative: a dedicated `PlayerNavigable` field if adding the enum would cause unnecessary churn.

Requirements:

- default must not accidentally expose prototypes;
- tests must enumerate all descriptors;
- snapshot tools may still construct prototypes;
- player router filters on `PlayerNavigable`.

## 16A.3 Decouple descriptor existence from player navigation

Refactor the coverage rule from:

```text
every descriptor must have a player route
```

to:

```text
every PlayerNavigable descriptor must have exactly one live route
```

And add reciprocal coverage:

```text
every live player route resolves to a PlayerNavigable descriptor
```

This prevents hidden legacy routes.

## 16A.4 SHELVE implementation

For each shelved panel:

1. Remove it from player routing only.
2. Keep the class if still useful.
3. Preserve snapshot coverage under a `PROTOTYPE` grouping.
4. Preserve save sections if campaign state still exists.
5. Remove menu/journal affordances that imply the console is usable.
6. If authored narrative references the console, redirect the reference into an existing journal/echo representation rather than a dead button.
7. Add backlog owner metadata for future expansion.

Never replace a shelved panel with a modal saying "coming soon" unless the product explicitly wants that promise.

## 16A.5 BACKIT candidate filter

A panel may become BACKIT only if all are true:

- authority exists now;
- authority is registered/ticked or otherwise live;
- state is persisted or deliberately derived;
- UI can bind without inventing new game rules.

Prioritize low-risk rebindings before any new architecture.

## 16A.6 MERGE candidate procedure

For overlapping faction surfaces or similar groups:

1. List all panel IDs.
2. List unique information/actions each exposes.
3. Choose canonical live route.
4. Move any essential read-only content into the canonical surface if already supported.
5. Shelve duplicate routes.
6. Preserve descriptor/snapshot history.
7. Update docs to point to canonical panel ID.
8. Add route uniqueness test.

## 16A.7 Save envelope audit

For every shelved or merged panel:

- does its underlying authority write a save section?
- is the section read elsewhere?
- is removing navigation safe?
- is a section now orphaned but still valid for forward/backward save compatibility?

Rule: do not delete save sections merely because a UI route is removed.

Update `SAVE_STORE_CONTRACT_MATRIX.md` where required.

## 16A.8 Snapshot and visual-regression handling

Update:

- snapshot manifest;
- `SNAPSHOT_COVERAGE.md`;
- maturity category;
- route expectations.

Do not regenerate visual goldens unless layout/rendering actually changes.

## 16A.9 Canon/documentation truth pass

Correct claims that equate registry presence with implemented capability.

Add a concise surface status table:

```text
LIVE: player-routed and campaign-backed
PROTOTYPE: constructible but not player-routed
MERGED: replaced by canonical route
RETIRED: no supported route
```

## 16A.10 Tests

Add/modify tests for:

1. navigable descriptor → live route;
2. live route → navigable descriptor;
3. no duplicate live route per panel ID;
4. prototype descriptor is constructible but not routable;
5. shelved panel does not appear in player navigation;
6. save sections remain registered where required;
7. triage table covers every candidate panel;
8. content-utilization selftest delta is recorded.

## 16A.11 Acceptance evidence

Attach to task log:

- before descriptor count;
- before player route count;
- after player route count;
- after live-capability count;
- verdict count by category;
- list of all shelved IDs;
- list of all merged IDs;
- zero coverage-gate failures.

### 16A DoD

```text
player_openable_panels == panels_backed_by_real_campaign_state
```

---

# TASK 16B — One Authority Per Fact

## 16B.0 Objective

Every surviving panel must read/write the same authority used by the campaign day loop and save system.

## 16B.1 Publish ownership table first

Create `docs/ui/PANEL_AUTHORITY_OWNERSHIP.md` or equivalent.

Required columns:

```text
panel_id
authority_type
construction_owner
campaign_field_or_service
tick_owner
save_capture
save_restore
host_session
rebind_on_load
```

Minimum entries:

- `fire_incident`
- `faction_matrix`
- `factions_narrative`
- `skill_matrix`
- `weather_sonde`

No code fix is complete until this table can name the actual owner.

## 16B.2 Add a provider seam

Prefer a single host-side resolution mechanism.

Possible shape:

```csharp
private T RequireAuthority<T>() where T : class
{
    var value = _campaignServices.Resolve<T>();
    return value ?? throw new InvalidOperationException(
        $"Panel requires live campaign authority {typeof(T).Name}.");
}
```

Constraints:

- host-side only;
- no service locator inside Core;
- no hidden creation on miss;
- descriptive failure;
- testable reference identity.

## 16B.3 `fire_incident`

Procedure:

1. Locate the campaign fire/hazard authority.
2. Confirm it is the object ticked by the campaign.
3. Confirm save capture/restore ownership.
4. Rebind the panel to that authority.
5. Replace `"inc_default"` with current incident selection/state.
6. If no real campaign authority exists, change verdict to SHELVE.
7. Do not create a new fire system in Plan 16.

Tests:

- panel shows incident produced by campaign;
- panel action mutates same incident authority;
- save/load preserves incident and panel view;
- reference identity holds.

## 16B.4 `faction_matrix`

Procedure:

1. Locate campaign `FactionStanceEngine`.
2. Remove panel-local construction.
3. Bind to the campaign engine.
4. Verify Plan 15B stance changes appear without reopening.
5. Verify panel mutation, if any, is visible to other faction consumers.

## 16B.5 `factions_narrative`

Same authority as `faction_matrix` unless source proves otherwise.

Critical test:

```text
ReferenceEquals(factionMatrix.Authority, factionsNarrative.Authority) == true
```

No duplicate stance engine may survive composition.

## 16B.6 `skill_matrix`

Bind to live `SkillProgressionSystem`.

Also verify its relationship to:

- `SkillAtrophySystem`;
- `SurvivorSocialCoordinator`;
- duty/shift progression.

Required scenario:

1. run missed-shift/day-loop mutation;
2. observe atrophy in live authority;
3. open panel;
4. panel displays updated value;
5. save/load;
6. panel still displays the same persisted value.

## 16B.7 `weather_sonde`

Remove per-open `new WeatherHostSession(...)`.

Bind to the world/campaign weather host session that owns the same `WeatherSystem` used by:

- weather forecast;
- expedition risk;
- world weather/day loop.

Identity test:

```text
ReferenceEquals(
    weatherSonde.Session.WeatherSystem,
    weatherForecast.Session.WeatherSystem
) == true
```

## 16B.8 Remove fixture IDs from live routes

Explicitly sweep for the source-plan literals and nearby variants:

- `"inc_default"`
- `"tag_1"`
- `"sig_distress"`
- `"sv_cohort_demo"`
- fallback `"surv_01"`

For each:

1. identify whether the route needs current selection, first valid entity, or no route;
2. bind from live selection/state;
3. if demo behavior is intentional, move it under explicit CLI selftest/demo command;
4. add static test preventing the exact fixture IDs from reappearing in production surface binding code.

## 16B.9 Session swap/rebind protocol

New game and load may replace host sessions and campaign owners.

Implement a single rebind pass:

```text
Create/restore campaign
→ replace owner/session fields
→ invalidate panel bindings
→ re-resolve authorities
→ refresh open panels
```

Do not let panels cache old-session references across the boundary.

Tests:

- new game A → open panel;
- new game B → same panel uses B authority;
- load save C → panel uses C authority;
- stale A/B event no longer refreshes panel.

## 16B.10 Reference identity gate

For all authority-backed routed panels, add a host-level test family:

```text
panel authority is campaign authority
panel session is campaign session
panel authority survives refresh without replacement
```

This is more valuable than checking equal values.

## 16B.11 Determinism

Any old `new X()` path that implicitly seeded itself must be replaced with campaign RNG ownership.

Forbidden:

- `string.GetHashCode()` seed;
- local `Random()` per bind;
- clock-derived seed;
- new seeded service independent of campaign RNG.

Require:

- `ICampaignRngManager`;
- `ISeededRng`;
- existing deterministic world/session source.

## 16B.12 Interaction tests per panel

For each survivor:

A. **Campaign → panel**
1. mutate through day loop/system;
2. open/refresh panel;
3. assert value.

B. **Panel → campaign**
1. perform panel action;
2. inspect campaign authority;
3. tick one day;
4. verify downstream effect.

C. **Persistence**
1. act;
2. save;
3. load;
4. panel matches;
5. downstream authority matches.

## 16B.13 Source gate

Add a narrow test/source scan:

```text
No `new <CoreSystem>` inside player panel bind configuration.
No `new <HostSession>` inside player panel bind configuration.
```

Allow explicit test/demo code paths by scoped exclusions, not blanket ignores.

### 16B DoD

Zero routed panels instantiate their own campaign authority at bind/open time.

---

# TASK 16C — Subscription Identity and Reopen Stability

## 16C.0 Objective

A panel may bind, unbind, leave the tree, re-enter, and bind to a new campaign without duplicate handlers, stale handlers, or multiplied feedback/audio.

## 16C.1 Reproduce before repair

For each known panel:

- bind;
- fire event;
- record refresh count;
- unbind;
- bind again;
- fire event;
- record refresh count.

Known set:

- `TriangulationPanel`
- `WeatherHistoryPanel`
- `GeigerCalibrationPanel`
- `FireIncidentPanel`

Record current failure.

## 16C.2 Fix delegate identity

Bad:

```csharp
_system.OnChanged -= _ => RefreshView();
```

Good:

```csharp
private Action<State>? _onStateChanged;

private void BindHandlers()
{
    _onStateChanged ??= _ => RefreshView();
    _system.OnChanged += _onStateChanged;
}

private void UnbindHandlers()
{
    if (_onStateChanged != null)
        _system.OnChanged -= _onStateChanged;
}
```

Use signatures appropriate to each event.

## 16C.3 Close all missing unsubscribe paths

Do a complete event table per panel:

```text
publisher
event
handler field
subscribe method
unsubscribe method
rebind-safe?
exit-tree-safe?
```

For `TriangulationPanel`, explicitly include `OnLocationRevealed`.

## 16C.4 Add reusable lifecycle helper

Preferred host/UI-side design: `SubscriptionBag`.

Example contract:

```csharp
public sealed class SubscriptionBag : IDisposable
{
    public void Add(Action unsubscribe);
    public void Clear();
    public void Dispose();
}
```

Usage:

```csharp
_system.OnChanged += OnChanged;
_subscriptions.Add(() => _system.OnChanged -= OnChanged);
```

Rules:

- no Core dependency;
- idempotent `Clear`;
- safe repeated bind/unbind;
- disposed before publisher/session replacement.

An explicit `BindOnce`/`UnbindOnce` helper is also acceptable if simpler.

## 16C.5 Lifecycle convention

Document:

```text
Bind authority
→ Clear old subscriptions
→ assign new authority
→ subscribe
→ initial RefreshView

Unbind
→ clear subscriptions
→ clear authority refs

_ExitTree
→ Unbind
```

If Godot lifecycle requires a different exact order, document and test that order.

## 16C.6 Static CI gate

Scan `src/UI/*.cs` for unsubscribe lambda literals.

Gate intent:

```text
`-=` must not be followed by a newly allocated lambda.
```

Implement with the repository's existing source-scan test idiom where practical.

Add explicit exclusions only for generated/test code after review.

## 16C.7 Sweep all remaining panel classes

Run static gate across every panel class.

Any additional hit is part of 16C, not deferred automatically.

For each hit:
- classify real defect vs safe pattern;
- repair;
- add to task evidence count.

## 16C.8 ×100 reopen test

For each known offender and at least one representative clean panel:

```text
repeat 100:
    bind
    fire event
    assert +1 refresh
    unbind
```

Final assertions:

- exactly 100 expected refreshes;
- not 101+;
- no event fires after unbind;
- weak-reference or explicit publisher count shows no retained old panel where testable.

## 16C.9 Session-load leak journey

Journey:

1. New Game.
2. Open panel.
3. Close/reopen several times.
4. Trigger events.
5. Save.
6. Load.
7. Trigger old-session publisher deliberately in test.
8. Assert no panel reaction.
9. Trigger new-session publisher.
10. Assert exactly one reaction.

Also compare node/handler counts to baseline where instrumentation exists.

## 16C.10 Audio double-fire regression

Run audio selftest and any UI cue test because duplicated handlers can duplicate SFX even when visual state looks correct.

## 16C.11 Snapshot policy

Subscription-only fixes must not trigger wholesale golden-image replacement.

If a screenshot changes:
- determine why;
- accept only if state presentation legitimately changed;
- document the cause.

### 16C DoD

- four known panels pass ×100 reopen;
- static gate has zero hits;
- all discovered additional offenders fixed;
- no disposed/old session can refresh a current panel;
- one authority event produces one UI refresh and one cue.

---

# 4. Cross-Task Integration Matrix

| Concern | 16A | 16B | 16C |
|---|---|---|---|
| Registry truth | owns | consumes | consumes |
| Player routing | owns | binds survivors | lifecycle-tests survivors |
| Campaign authority | classifies | owns repair | verifies stable identity |
| Save sections | audits | validates owner identity | validates post-load bindings |
| Fixture IDs | detects | removes | guards rebind behavior |
| Snapshots | reclassifies | minimal | no churn expected |
| CI gates | nav liveness | authority construction | subscription identity |
| Docs | maturity truth | ownership map | lifecycle guide |

Execution dependency:

```text
16A
 │
 ├── decide survivors
 ▼
16B
 │
 ├── campaign identity fixed
 ▼
16C
    lifecycle hygiene on the surviving live set
```

---

# 5. Conflict and Merge Strategy

`src/Main.PlayerSurfaces.cs` is a high-conflict file.

Required discipline:

1. Do not combine unrelated formatting/refactor changes.
2. Land 16A route subtraction separately.
3. Rebase.
4. Land 16B authority rebinding separately.
5. Sequence around Plan 15A/15B as specified by the source plan.
6. Keep each panel rebinding reviewable.
7. Avoid renaming descriptor IDs in the same commits.

Recommended commit series:

```text
16A-1 panel maturity + coverage gate
16A-2 route triage + snapshot/doc classification
16A-3 save-envelope audit
16B-1 host authority resolver + ownership doc
16B-2 factions authority repair
16B-3 skill/weather/fire repairs + fixture removal
16B-4 load/new-game rebind + identity tests
16C-1 subscription helper + known four repairs
16C-2 repository-wide sweep + CI source gate
16C-3 ×100 runtime journey + audio/leak verification
```

---

# 6. Verification Pyramid

## Tier 1 — Static

- no routed descriptor marked non-navigable;
- no duplicate route;
- no production fixture ID;
- no `new CoreSystem` in bind routes;
- no lambda literal on unsubscribe;
- every live panel has an authority classification.

## Tier 2 — Core/host unit tests

- resolver returns campaign owner;
- missing owner fails loudly;
- rebind swaps references;
- subscription bag is idempotent.

## Tier 3 — Integration

- day-loop mutation appears in panel;
- panel mutation appears in day loop;
- save/load maintains state;
- load replaces session identity.

## Tier 4 — Runtime selftests

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/triad-drift-gate.sh
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/verify-fast.sh
godot --headless --path . -- --audio-selftest
```

Expected:
- 0 build errors;
- 0 warnings if repository policy requires it;
- all tests pass;
- all selftests exit 0;
- content-utilization delta documented.

---

# 7. Failure Injection and Negative Tests

Add explicit negative cases.

### N16.1 Missing authority
Mark a panel live while withholding the owner.
Expected: descriptive development failure, never synthetic defaults.

### N16.2 Old session event
After load, fire old publisher.
Expected: zero refresh.

### N16.3 Repeated route registration
Register same live route twice.
Expected: gate failure.

### N16.4 Prototype route leak
Attempt navigation to SHELVE descriptor.
Expected: not available through player route.

### N16.5 Save with shelved UI
Persist an authority whose panel is shelved.
Expected: save still round-trips.

### N16.6 Fixture fallback absent
No selected survivor/signal/incident.
Expected: explicit unavailable/empty state, never `"surv_01"` or equivalent.

### N16.7 Rebind storm
Rapid bind/unbind repeated 100 times.
Expected: stable handler count.

---

# 8. Performance and Allocation Guardrails

This plan should not create per-frame service resolution.

Requirements:

- resolve authority at bind/rebind boundaries;
- cache the active reference until session swap;
- do not poll `RequireAuthority<T>()` every `_Process`;
- do not allocate new delegates every refresh;
- do not rebuild entire registry every panel open;
- do not create hidden host sessions.

If instrumentation exists, record:
- allocations per panel reopen before/after;
- node count after ×100 reopen;
- event-handler count after ×100 reopen.

---

# 9. Security / Robustness Considerations

Although this is not a security plan, production routing must not permit malformed IDs to mutate arbitrary state.

- validate selected entity IDs against live authority;
- reject absent IDs;
- do not silently pick a demo entity;
- do not accept stale-session object references after load;
- ensure developer-only demo routes are not exposed through normal menu actions.

---

# 10. Documentation Deliverables

Update or create as appropriate:

- `docs/ui/SURFACE_GAP_REPORT.md`
- `docs/ui/PANEL_AUTHORITY_OWNERSHIP.md`
- `docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md`
- `docs/ui/SNAPSHOT_COVERAGE.md`
- `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`
- `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md`

Each must be derived from current source after implementation.

---

# 11. Final Acceptance Checklist

## 16A
- [ ] triage sheet covers all candidate consoles
- [ ] every descriptor has maturity/navigation metadata
- [ ] all SHELVE routes removed from player navigation
- [ ] MERGE duplicates have one canonical route
- [ ] BACKIT set contains only systems with real authorities
- [ ] coverage gate checks only player-navigable surfaces
- [ ] snapshots preserve prototype history
- [ ] save-envelope audit complete
- [ ] docs report truthful live count

## 16B
- [ ] five named disconnected bindings resolved or shelved
- [ ] faction panels share same stance engine
- [ ] skill panel uses live progression authority
- [ ] weather sonde uses world weather session
- [ ] fire panel uses campaign fire authority or is shelved
- [ ] fixture IDs removed from production route bindings
- [ ] new game/load rebinds open panels
- [ ] reference-identity tests pass
- [ ] deterministic RNG ownership preserved
- [ ] source construction gate passes

## 16C
- [ ] four known panels fixed
- [ ] missing unsubscribe paths closed
- [ ] reusable lifecycle helper in place
- [ ] lifecycle convention documented
- [ ] static gate reports zero lambda-unsubscribe hits
- [ ] remaining UI classes swept
- [ ] ×100 reopen passes
- [ ] load-session leak journey passes
- [ ] audio selftest passes
- [ ] no unrelated snapshot churn

---

# 12. Ship / No-Ship Gate

**SHIP** only when:

```text
live route count == campaign-backed route count
AND throwaway authority constructions == 0
AND fixture IDs in live routing == 0
AND lambda-unsubscribe gate hits == 0
AND ×100 reopen failures == 0
AND all verification gates pass
```

Otherwise: **NO SHIP**.

---

# 13. Handoff Notes for the Implementing Agent

1. Start from evidence, not panel names.
2. Do not preserve a route merely because the class exists.
3. Do not create missing gameplay systems in this plan.
4. Prefer explicit campaign identity assertions over equal-value assertions.
5. Do not patch one lambda unsubscribe and declare the defect closed; sweep the class and the directory.
6. Treat new-game/load as authority replacement.
7. Keep prototype panels available to tests if useful, but not to players.
8. Keep commits narrow because `Main.PlayerSurfaces.cs` is shared with adjacent plans.
9. Record the before/after live-panel counts in the final task log.
10. End with a full green `verify-fast.sh` and the specified runtime selftests.

---

# 14. Final Outcome

When this plan is complete, the navigation layer stops overstating the game. Every console a player can reach is backed by the current campaign, every surviving panel shares the same authority as simulation and persistence, and repeated panel use cannot accumulate stale event behavior.

That is the continuity guarantee Plan 16 is meant to establish.

---

# CHECKPOINT REPORTS

## Checkpoint C2-P0 — Baseline Capture & Premise Verification (2026-09-15)

### P0.1 Frozen revision

| Metric | Value |
|---|---|
| Commit SHA | `87b199b270669d6a847d5df76790bcdb446839ad` |
| Branch | `lane/trapping-flagship-verification` |
| Dirty files (incl. prior-wave uncommitted work + this package) | 846 |
| dotnet | 10.0.302 |
| Godot | 4.7.1.stable.mono.official.a13da4feb |
| Panel descriptors registered | 180 (`PanelRegistryBootstrap`) |
| Shelved prototypes | 29 (maturity `Prototype`, pinned by `PlayerSurfaceCoverageGateTests` / `PlayerSurfaceLivenessGateTests`) |
| Player-navigable (manifest) | 151 — manifest == navigable count, all routed/bound/closeable per `PlayerSurfaceCoverageGateTests` (89/89 UI gates green this session) |
| `new *System/*Engine/*HostSession` in bind config (`src/Main.PlayerSurfaces.cs`) | 0 — but 1 throwaway Core construction found: `?? new Ashfall.Core.Inventory.Inventory()` (shelter_barter, INV-16.3) |
| Lambda-unsubscribe hits in `src/UI` | 0 (`PanelSubscriptionHygieneTests` gate + manual scan) |

### P0.2/P0.3 Files read & route inventory

All routing/ownership files listed in §P0.2 were read in full or in the
regions named below. The mechanical route inventory is owned by
`PlayerSurfaceManifest.Generate()` (Core, gated by tests); the deep authority
inventory for the plan's focus set is published as
`docs/ui/PANEL_AUTHORITY_OWNERSHIP.md` (§16B.1 deliverable, this package).

### Premise-verification matrix (STALE_PLAN findings)

The source plan's three defect classes were re-verified against current
source BEFORE editing. Most of Plan 16 was already implemented and committed
at HEAD by earlier remediation lineages (REM-005/R09, Task 107/109) — the
ledger never carried a Plan 16 row, so this checkpoint records the truth:

| Source-plan premise | Current evidence | Verdict |
|---|---|---|
| §1.1 thirty false-capability consoles | `PanelMaturity` (Live/Prototype) + `IsPlayerNavigable` exist in committed `PanelRegistry.cs`; `TryOpen` blocks prototype routes with diagnostics; 29 prototypes shelved + pinned by liveness/coverage gates; `SURFACE_GAP_REPORT.md` (Phase 26 audit) already triaged the 30-surface set | **Already implemented (16A)** |
| §1.2 five panels bind freshly constructed authorities | `faction_matrix`/`factions_narrative` → `EnsureSharedFactionStance()` (campaign `GuildStanceEngine`, loud failure); `skill_matrix` → `EnsureSharedSkillProgression()` (day-ticked, persisted); `fire_incident` → `_shelterFireSession` (day-ticked at `Main.CampaignOwners.cs:484`, saved, swap-nulled); `weather_sonde` → guarded wrapper around `_world.Weather` (identity with `weather_forecast` holds); `"inc_default"` absent | **Already implemented (16B.2–16B.7)** |
| §1.3 four subscription offenders with broken lambda unsubscribe | All four use named method-group unsubscribe; `TriangulationPanel` covers `OnLocationRevealed`; `PanelSubscriptionHygieneTests` static gate exists; `PanelBindLifecycleSelfTest` covers bind/unbind/rebind ×16 gates | **Already implemented (16C.2–16C.7)** — stale premise |
| §16B.8 fixture IDs in live routes | `"surv_01"` fallback (`expedition_camp`), `"tag_1"` (geiger bind + panel defaults), `"sig_distress"` (triangulation bind + panel defaults) all present in live binding code | **REAL — repaired this package** |
| §16B.9 session swap invalidates bindings | Fire/world/radio/expeditions/dose participants null their authorities on reset, but `_weatherSondeHost` was never nulled — a stale wrapper could display the previous campaign's weather after swap | **REAL (weather sonde) — repaired this package** |
| §16B.13 source gate for bind-time construction | No gate scanned `src/Main.PlayerSurfaces.cs` (Task 107 gate scans `src/UI` only); the `?? new Inventory()` fallback lived there undetected | **REAL — gate added this package** |
| §16C.8 ×100 reopen cycles | Existing gates ran ×10/×6 | **REAL gap — hardened this package** |

### P0.4 Reproduced authority/fixture defect (pre-fix, saved)

`bash scripts/run_test.sh Ashfall.Core.Tests/UI/PlayerSurfaceBindingPurityGateTests.cs`
failed pre-fix with exactly the verified defects (2/2 failed):

```text
src/Main.PlayerSurfaces.cs:489 -> "surv_01"   (expedition_camp fallback)
src/Main.PlayerSurfaces.cs:508 -> "tag_1"      (geiger bind)
src/Main.PlayerSurfaces.cs:513 -> "sig_distress" (triangulation bind)
src/UI/GeigerCalibrationPanel.cs:21,46 -> "tag_1" (field + default param)
src/UI/TriangulationPanel.cs:22,44 -> "sig_distress" (field + default param)
src/Main.PlayerSurfaces.cs:575 [core construction] ->
    _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory()  (shelter_barter)
```

### P0.5 Subscription defect reproduction

Not reproducible: the lambda-unsubscribe defect class was already repaired at
HEAD (named method groups + `PanelSubscriptionHygieneTests` + gates 1–16).
Recorded as a stale premise; the ×100 hardening (Gate 17) now pins the
repaired behavior at the plan's required cycle count.

---

## Checkpoint C2-16B/16C — Executed Remainder Package (2026-09-15)

### Outcome

Close every Plan 16 DoD line that was still false at the frozen revision:
fixture IDs out of live routes, no throwaway authority at bind time, session
swap invalidates the weather-sonde wrapper, ×100 reopen stability, and the
missing 16B.1 ownership table — without re-implementing the (already-built)
16A/16B.2–16B.7/16C.2–16C.7 machinery.

### Files changed

| File | Change |
|---|---|
| `src/Main.PlayerSurfaces.cs` | `expedition_camp` bind: `?? "surv_01"` → explicit empty; `geiger_calibration` bind → `ResolveLiveDosimeterTag()`; `triangulation` bind → `ResolveLiveTriangulationSignalId()`; `shelter_barter` bind → `SetupInventory()` + `_inventory!.Inventory` (fallback removed); two live-selection resolver helpers added |
| `src/Main.Plans147.cs` | `EnsureShelterBarter()` composes `SetupInventory()` and binds the campaign inventory; `?? new Inventory()` fallback removed (INV-16.3) |
| `src/Main.Lifecycle.cs` | `world_weather` participant onReset now nulls `_weatherSondeHost` (INV-16.6 — stale wrapper can never outlive its campaign) |
| `src/UI/GeigerCalibrationPanel.cs` | `"tag_1"` field/default removed; explicit "None registered" empty state with all actions disabled (N16.6); `RefreshCount` test observable; public `Unbind()` (`_ExitTree` delegates to it) |
| `src/UI/TriangulationPanel.cs` | `"sig_distress"` field/default removed; explicit "None under direction-finding" empty state with record/triangulate disabled + action guards (N16.6); `RefreshCount`; public `Unbind()` |
| `src/UI/WeatherHistoryPanel.cs` | `RefreshCount`; public `Unbind()` (previously only `_ExitTree`) |
| `src/UI/FireIncidentPanel.cs` | `RefreshCount` |
| `src/UI/WeatherPanel.cs` | `RefreshCount` (clean representative for Gate 17) |
| `src/Host/PanelBindLifecycleSelfTest.cs` | gates 8/15 loops ×10 → ×100; new Gate 17: ×100 bind→fire→unbind cycles for WeatherHistory/Geiger/FireIncident/Triangulation + WeatherPanel with exact per-cycle refresh-count assertions (exactly +1 while bound, 0 while unbound, 100 total); `totalGates` 16 → 17 |
| `Ashfall.Core.Tests/UI/PlayerSurfaceBindingPurityGateTests.cs` | NEW — fixture-ID ban (exact literals, `src/UI/**` + `src/Main.PlayerSurfaces.cs`) + bind-config construction ban (16B.13) |
| `docs/ui/PANEL_AUTHORITY_OWNERSHIP.md` | NEW — §16B.1 ownership table (9 focus panels), fixture-ID verdicts, flagged items |
| `docs/plans/C1_planintegration[2].md` | This plan + checkpoint reports |

### Verification (all green)

| Command | Result |
|---|---|
| `dotnet build Ashfall.csproj` | 0 errors; 4 pre-existing CS8602 warnings from the user's uncommitted `SilentFoundryPanel.cs` (outside claim, documented in ledger) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/UI/PlayerSurfaceBindingPurityGateTests.cs` | 2/2 (was 2 FAIL pre-fix — saved as P0.4 evidence) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/UI/` | 89/89 (15 gate classes incl. route parity, coverage, liveness, subscription hygiene, fabricated-fallback) |
| `godot --headless --path . -- --panel-bind-lifecycle-selftest` | **17/17 gates PASS** incl. Gate 17 ×100 reopen stability |
| `godot --headless --path . -- --audio-selftest` | PASS 624/624 (no cue duplication — §16C.10) |
| `godot --headless --path . -- --ui-accessibility-selftest` | PASS (empty-state copy/button-disable changes remain a11y-clean — §16C.11: no snapshot churn, no layout change) |

Not run (scoped out per TEST_POLICY): full suite, data-integrity selftest
(zero data/catalog changes), bridge/triad/content-utilization (no Core/data
or shared seam changes beyond those verified above).

### DoD delta (§3 of this plan)

| DoD line | Before | After |
|---|---|---|
| fixture IDs removed from live routes | false (3 bind sites + 4 panel defaults) | **true** — purity gate green |
| zero routed panels construct a Core authority at bind time | false (`?? new Inventory()`) | **true** — construction gate green |
| zero routed panels construct a replacement host session at bind time | true (guarded, campaign-owned) | true (unchanged; now gated) |
| five named authority offenders bound to campaign instances | true (already at HEAD) | true — now documented in the ownership table |
| ×100 reopen, one refresh per event | false (×10) | **true** — Gate 17 green |
| static lambda-unsubscribe gate zero hits | true | true |
| save/load/new-game journeys rebind | true except stale sonde wrapper | **true** — sonde invalidated with its world |
| ownership table (§16B.1) | missing | **published** |

### Known limitations / flagged for foreman

1. Pre-campaign `new SeededRng(147)` fallback in `EnsureShelterBarter`
   (product decision: should barter be constructible before a campaign?).
2. `stationId` default `"station_alpha"` on `RadioHostSession.RecordObservation`
   (station-selection seam is a design change, not a Plan 16 repair).
3. `_silentFoundry`/`_sharedFactionStance`/`_sharedSkillProgression` survive
   session swaps with state restored via their save sections (load path);
   new-game reset coverage should be verified by the owning foundry/
   apprenticeship packages (see ownership doc §"Known flagged items").
4. `surviv_01`-style fallbacks in other un-audited bind actions were not
   swept beyond the purity gate's literal set — the gate bans the exact
   source-plan literals; broadening it is a foreman decision.
5. Plan 15 (AGY economy/traveling-caravan presentation wave) had NOT landed
   at this revision; this package deliberately avoided the
   `economy_detail`/`traveling_caravan` regions of `Main.PlayerSurfaces.cs`, so
   the 15A → 16B sequencing constraint is respected in both directions.
