# ASHFALL Release Stability Remediation Plan — 65 Bugs

> **Status:** PLAN (approved for implementation). Mode: stability/correctness first.
> **Scope:** Burn down 65 evidence-backed stability/correctness defects that threaten release trust.
> **Out of scope:** New content expansions, art remasters, speculative feature invention, Unity work, building 30 new shelter systems to "fill" stubs.
> **Deliverable:** This document + phased fix commits, each closing a numbered BUG-RS-### set.

---

## 1. Objective

Make the Godot campaign **honest and safe**:

1. Players cannot open surfaces that pretend to be live systems when they are not.
2. Live systems that persist state can be reached and mutated through the host/UI.
3. Dual-authority / silent-fallback / disconnected-bind defects cannot ship.
4. Determinism and save ownership stay intact.

---

## 2. Current Reality (verified evidence)

All claims below were re-verified against the working tree at plan authoring time.
Project root: `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`.

| Fact | Evidence (file:line) | Verified |
|---|---|---|
| Most AGENTS Critical/High items already RESOLVED (C1–C6, H1/H3/H4/H6/H8–H10/H12) | `AGENTS.md:308–334` | ✅ |
| Remaining AGENTS High: H5 Utility AI fork, H7 Main triad drift, H2 WornGear (bridge exists), H11 Journal (core behaviour untested) | `AGENTS.md:327,330,332,336` | ✅ |
| 30 expanded consoles are player-routable with `openAction`+`closeAction` only — no campaign `bindAction` | `src/Main.PlayerSurfaces.cs:543–659` (each block: `openAction`/`closeAction`, no `bindAction`) | ✅ |
| Those consoles report `IsBound = true` while rendering literal telemetry / feedback-only buttons | `src/UI/CryogenicPermafrostCorePanel.cs:32` (`IsBound { get; private set; } = true`), `:36` (`IsBound = true`) | ✅ |
| `TryResolveMoralChoice` has zero callers outside its definition | `src/Main.MoralChoice.cs:91` (defined; no call sites under `src/`) | ✅ |
| Fire incident opens a fresh `ShelterFireHazardSystem` + `"inc_default"` | `src/Main.PlayerSurfaces.cs:389` (`_fireIncidentPanel.Bind(new Ashfall.Core.Shelter.ShelterFireHazardSystem(), "inc_default")`) | ✅ |
| Fire tick seeds via `string.GetHashCode()` | `src/UI/FireIncidentPanel.cs:165` (`new CoreSeededRng(_incidentId.GetHashCode())`) | ✅ |
| 4 panels unsubscribe new lambdas (no-op unsubscribe → leak/dup refresh) | `src/UI/WeatherHistoryPanel.cs:30` (`-= _ => RefreshView()`); same pattern in GeigerCalibration / FireIncident / Triangulation | ✅ |
| Radio stations still hardcoded via `RegisterDefaults()` | `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs` (`RegisterDefaults` present) | ✅ |
| `CraftContext` exists + unit tests, but no production consumer under `src/` | only `Assets/Ashfall.Core/Crafting/CraftContext.cs` + `Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs` reference it | ✅ |
| Loader allowlist still holds 6 unwired / dormant loaders | `Ashfall.Core.Tests/Tooling/LoaderWiringGateTests.cs:48–56` (SkyLayerArmor, Spiritual, Atmosphere, EnvironmentalText, DebtTemplate, HoldfastNpc) | ✅ |
| `CollectibleCatalogLoader` allowlist says "do not wire" | `LoaderWiringGateTests.cs:56` (`"Collectibles catalog is fresh concurrent-stream work; do not wire from this stream."`) | ✅ |
| Player-surface gates overstate health (route/setup ≠ live Core bind) | `docs/debug/10LOOP_player_ui_ux_BUG_AUDIT.md` §14 (cited); gate logic to be re-verified in Phase 6 | ✅ (cite) |
| Electrostatic scrubber is already Core-bound (do not treat as stub) | `ElectrostaticScrubberPanel` → `VentilationHostSession` (must not be quarantined) | ⚠ verify in Phase 1 |

### Notes on the worktree shadow
A `.claude/worktrees/plan06-narrative/` shadow tree exists with its own copies of several files
(e.g. `src/Main.MoralChoice.cs`, `CraftingHostSession.cs`). All evidence above is taken from the
**primary** tree (`./src`, `./Assets`), not the worktree. Implementation must edit primary-tree files.

---

## 3. Required Delta

| From | To |
|---|---|
| 30 false-affordance consoles in player routing | Quarantined (dev-only / unavailable) **or** honestly bound |
| Moral choice persisted but unresolvable | Player/host path calls `TryResolveMoralChoice` |
| Fire / skill / stance panels can bind disconnected instances | Bind campaign-owned systems only |
| Lambda event leaks | Stored-delegate subscribe/unsubscribe |
| Radio dual authority | JSON owns station defs; delete/gate `RegisterDefaults` |
| Trade specialty inert in production craft | Workbench craft emits `CraftContext` → specialty |
| Stale AGENTS / allowlist / gates | Authority docs and gates match runtime truth |

---

## 4. Severity & Batching Rules

Priority order (ashfall-repair multi-bug policy):

1. Shared root causes
2. Save / ownership corruption risk
3. Determinism
4. Foundational integration failures
5. Logic defects
6. UI symptoms / false affordances
7. Doc/gate drift

**Batching:** one root-cause cluster per PR. Quarantining the 30 consoles is **one** cluster
(shared policy + gate), counted as 30 closed bug IDs.

---

## 5. Bug Backlog (65 items)

### Cluster A — False-affordance quarantine (BUG-RS-001 … 030) — P0

**Root cause:** Visual console classes registered and routed (`openAction`/`closeAction`) before Core/host authority exists. No `bindAction` wired → `IsBound` is a hard `true` lie.
**Selected repair:** Quarantine first (stability). Bind later only when a real Core owner exists.

| ID | Panel route id | Class | Route line |
|---|---|---|---|
| 001 | `biogas_digester` | `AnaerobicBiogasDigesterPanel` | 543 |
| 002 | `cartography_gis` | `SubterraneanCartographyPanel` | 547 |
| 003 | `printing_press` | `UndergroundPrintingPressPanel` | 551 |
| 004 | `silicon_slicing` | `SiliconIngotSlicingPanel` | 555 |
| 005 | `geothermal_turbine` | `GeothermalSteamTurbinePanel` | 559 |
| 006 | `war_dog_kennel` | `WarDogKennelPanel` | 563 |
| 007 | `isotope_separator` | `IsotopeSeparatorPanel` | 567 |
| 008 | `plasma_smelting` | `PlasmaArcSmeltingPanel` | 571 |
| 009 | `borehole_seismograph` | `BoreholeSeismographPanel` | 575 |
| 010 | `logistics_airlock` | `HeavyLogisticsAirlockPanel` | 579 |
| 011 | `cryo_permafrost_core` | `CryogenicPermafrostCorePanel` | 583 |
| 012 | `basal_radon_migration` | `BasalRadonMigrationPanel` | 587 |
| 013 | `trauma_bonding_cohort` | `TraumaBondingCohortPanel` | 591 |
| 014 | `clandestine_insurgency` | `ClandestineInsurgencyPanel` | 595 |
| 015 | `subterranean_debt_ledger` | `SubterraneanDebtLedgerPanel` | 599 |
| 016 | `surface_shrapnel_aegis` | `SurfaceShrapnelAegisPanel` | 603 |
| 017 | `long_walk_expedition` | `LongWalkExpeditionPanel` | 607 |
| 018 | `sonic_rupture_drill` | `SonicRuptureDrillPanel` | 611 |
| 019 | `vault_door_breaching` | `VaultDoorBreachingPanel` | 615 |
| 020 | `iron_cenotaph_memorial` | `IronCenotaphMemorialPanel` | 619 |
| 021 | `aquifer_treaty_concession` | `AquiferTreatyConcessionPanel` | 623 |
| 022 | `crossing_safe_conduct_vouch` | `CrossingSafeConductVouchPanel` | 627 |
| 023 | `mechanical_prosthetics_lathe` | `MechanicalProstheticsLathePanel` | 631 |
| 024 | `fungal_protein_fermenter` | `FungalProteinFermenterPanel` | 635 |
| 025 | `ultrasonic_decontam_airlock` | `UltrasonicDecontaminationAirlockPanel` | 639 |
| 026 | `tropospheric_radio_relay` | `TroposphericRadioRelayPanel` | 643 |
| 027 | `induction_cupola_furnace` | `InductionCupolaFurnacePanel` | 647 |
| 028 | `heavy_marine_diesel_gen` | `HeavyMarineDieselGeneratorPanel` | 651 |
| 029 | `slurry_dewatering_sump` | `SlurryDewateringSumpPanel` | 655 *(re-verify: may already have Core; quarantine only if still feedback-only)* |
| 030 | `magnetic_drum_archive` | `MagneticDrumArchivePanel` | 659 |

**DoD (cluster):**
- Player menu / `OpenExpandedPanel` cannot open quarantined IDs (or opens an explicit non-interactive "system offline" notice).
- `IsBound` must not be hard-`true` without a typed Core/host session.
- New gate: routed panel without typed bind → CI fail (or explicit `Quarantined` disposition).
- Update AGENTS stub table + `docs/player_surface_manifest.json`.

**Must NOT:** invent 30 new Core systems in this remediation. **Must NOT quarantine:** `ElectrostaticScrubberPanel` (already Core-bound via `VentilationHostSession`).

---

### Cluster B — Live systems inert / disconnected (BUG-RS-031 … 038) — P0/P1

| ID | Bug | Evidence (file:line) | Repair |
|---|---|---|---|
| 031 | Moral choices cannot be resolved by player | `src/Main.MoralChoice.cs:91` (uncalled) | Add Moral Choice panel/modal; wire resolve + journal; journey test |
| 032 | Fire incident binds disposable system | `src/Main.PlayerSurfaces.cs:389` (`new ShelterFireHazardSystem()`) | Bind campaign `ShelterFireHazardSystem` / live incident id |
| 033 | Fire RNG uses `string.GetHashCode()` | `src/UI/FireIncidentPanel.cs:165` | Seed from campaign stream / stable hash |
| 034 | WeatherHistory lambda unsubscribe leak | `src/UI/WeatherHistoryPanel.cs:30` (`-= _ => RefreshView()`) | Store `EventHandler` field |
| 035 | GeigerCalibration lambda unsubscribe leak | `src/UI/GeigerCalibrationPanel.cs` (same pattern) | same |
| 036 | FireIncident lambda unsubscribe leak | `src/UI/FireIncidentPanel.cs` (same pattern) | same |
| 037 | Triangulation lambda unsubscribe + orphan `OnLocationRevealed` | `src/UI/TriangulationPanel.cs` (same pattern) | stored delegates for both |
| 038 | Onboarding not discoverable as guidance | `availableInMenu:false`; reopen UX weak | HUD/F1 reopen path + route test |

---

### Cluster C — Dual authority & silent integration gaps (BUG-RS-039 … 048) — P1

| ID | Bug | Evidence (file:line) | Repair |
|---|---|---|---|
| 039 | Radio station defs dual authority | `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs` (`RegisterDefaults`) | Plan-34 method: JSON authority → delete defaults → parity fixture → gate |
| 040 | CraftContext unused in production | `Assets/Ashfall.Core/Crafting/CraftContext.cs` (only tests reference) | Crafting completion emits `CraftContext`; host consumes |
| 041 | Trade specialty never advances from live craft | Phase0 debug-only `CraftItem` | Wire workbench/duty-roster crafter → `OnCraftCompleted` |
| 042 | Collectible allowlist stale | `LoaderWiringGateTests.cs:56` vs `HostCli.Collectibles` Load | Re-disposition: wired / prune allowlist / require `LoadAndRegister` |
| 043 | AtmosphereText loader unwired | `LoaderWiringGateTests.cs:51` | Decide venue (briefing/journal) **or** keep dormant with clearer disposition |
| 044 | EnvironmentalText loader unwired | `LoaderWiringGateTests.cs:52` | same |
| 045 | DebtTemplate loader unwired | `LoaderWiringGateTests.cs:53` | Wire into holdfast/trade debt **or** explicit backlog disposition with owner |
| 046 | HoldfastNpc loader unwired | `LoaderWiringGateTests.cs:54` | Wire into holdfast quest loops **or** disposition |
| 047 | SkyLayerArmor allowlist hygiene | `LoaderWiringGateTests.cs:48` (Expansion 11 dormant) | Keep dormant; ensure docs say "not a release blocker" |
| 048 | SpiritualCatalog allowlist hygiene | `LoaderWiringGateTests.cs:49` (Plan 30 dormant) | same |

---

### Cluster D — AGENTS / architecture correctness (BUG-RS-049 … 056) — P1/P2

| ID | Bug | Evidence | Repair |
|---|---|---|---|
| 049 | H5 Utility AI fork (Core vs `src/UtilityAI`) | `AGENTS.md:330` | Make host panel a thin binder over Core `UtilityAiSystem` only; delete duplicate scoring if present |
| 050 | H7 Setup-without-Save drift risk | `AGENTS.md:332` (31 Setup / 24 Save) | Audit the 13 `SetupX` without `SaveX`; add Save, fold into aggregate, or mark intentionally ephemeral in triad gate |
| 051 | Ephemeral setups silently dropping state | same triad audit | Cover `UtilityAi`, `Phantom`, `EventsHost`, `Expansions`, `IceRoad`, `NpcArcs`, `DeepCoast`, … |
| 052 | H2 WornGear AGENTS drift | `AGENTS.md:327` (bridge exists) | Verify single type; mark H2 RESOLVED in AGENTS if confirmed |
| 053 | H11 Journal AGENTS drift | `AGENTS.md:336` (core behaviour untested) | Confirm `JournalSystemCoreBehaviorTests` coverage; mark H11 RESOLVED or list remaining gaps |
| 054 | Invariant-4 AGENTS still lists Guid.NewGuid offender | `AGENTS.md` Invariant-4 | Confirm `ProceduralItemInstance.cs:48` fix; scrub stale offender bullet |
| 055 | PlayerSurfaceCoverageGate overstates binding | `docs/debug/10LOOP_player_ui_ux_BUG_AUDIT.md` §14 | Gate must require typed bind / quarantine disposition, not mere route presence |
| 056 | PanelRouteGate / manifest drift | `docs/player_surface_manifest.json` (~115 entries) | Regenerate + fail on open-only routes; many expanded stubs missing or mislabeled HostSession |

---

### Cluster E — Determinism / ownership micro-defects (BUG-RS-057 … 060) — P1

| ID | Bug | Repair |
|---|---|---|
| 057 | Disconnected `new SkillProgressionSystem()` in apprenticeship / library paths | Inject campaign skills system |
| 058 | Disconnected `new FactionStanceEngine()` in SilentFoundry / test-only paths leaking to runtime | Ensure production Foundry uses campaign stance authority |
| 059 | Phase0 craft debug button hardcodes survivor/profession | Gate behind debug CLI only; production path uses assignment |
| 060 | `OrdinalIgnoreCase` still used in RadioStationCatalog dictionaries | Align with flag ledger Ordinal policy **or** document intentional case-fold + normalize on ingest |

---

### Cluster F — Greenhouse / first-hour correctness (BUG-RS-061 … 065) — P2

From AGENTS Stitch handoff / greenhouse gap register — stability of "promised actions".

| ID | Bug | Repair |
|---|---|---|
| 061 | Seed selection not player-reachable / PLANT hardcodes path | Bind seed picker → `HandleGreenhouseAction` |
| 062 | Soil AMEND / drip / pest / shade / STERILIZE actions missing or feedback-only | Wire existing host APIs (Plan 22) |
| 063 | Water quantity / tainted choice UI gap | Wire host water APIs |
| 064 | Supply-stock strip / readiness columns missing | Bind inventory projection |
| 065 | Greenhouse regression selftest gap for new actions | Extend `--greenhouse-selftest` for each new action string |

---

## 6. Dependency-Ordered Phases

### Phase 0 — Baseline lock (0 code risk)
- Record HEAD + run: `dotnet test`, `dotnet build Ashfall.csproj`, `--data-integrity-selftest`, `--bridge-selftest`.
- Freeze bug IDs in this doc.
- **Gate:** baseline green or known-fail list attached below in §13.

### Phase 1 — Quarantine false affordances (BUG-RS-001…030)
**Why first:** largest release-trust win; no new gameplay; reversible.
**Files:** `src/Main.PlayerSurfaces.cs`, `PanelRegistryBootstrap.cs`, panel `IsBound` honesty, coverage gate tests, AGENTS stub table, manifest generator.
**Gate:** open-only expanded routes fail CI unless `Quarantined`; player cannot interact with fake telemetry as if live.

### Phase 2 — Event lifecycle + fire ownership (BUG-RS-032…037)
**Files:** four panels + fire host session wiring + seed.
**Gate:** repeated Bind/Unbind selftest; fire uses campaign system; no `GetHashCode` seed.

### Phase 3 — Moral choice reachability (BUG-RS-031, 038)
**Files:** new/extended Moral Choice UI, `src/Main.MoralChoice.cs` call sites, onboarding reopen.
**Gate:** headless/UI test resolves a choice; onboarding reopen test.

### Phase 4 — Craft attribution + radio authority (BUG-RS-039…041, 059)
**Files:** CraftingSystem / host craft path, TradeSpecialty, RadioStationCatalog + `radio` JSON, gates.
**Gate:** same-seed craft advances specialty; radio defaults deleted or unreachable; loader honesty.

### Phase 5 — Allowlist / loader dispositions (BUG-RS-042…048)
**Files:** `LoaderWiringGateTests`, optional host wiring for debt/holdfast/atmosphere **only if venue decided**.
**Gate:** no stale allowlist entries; each remaining entry has owner + disposition date.

### Phase 6 — Architecture known issues (BUG-RS-049…056, 057–058, 060)
**Files:** UtilityAI host thinness, triad gate, AGENTS.md truth scrub, surface gates, stance/skills injection.
**Gate:** AGENTS High table matches code; gates catch open-only / Setup-without-Save.

### Phase 7 — Greenhouse promised actions (BUG-RS-061…065)
**Files:** `src/UI/GreenhousePanel.cs`, `src/Main.World.cs` `HandleGreenhouseAction`, selftest.
**Gate:** `--greenhouse-selftest` covers new actions; no hardcoded-only plant path.

### Phase 8 — Full verification + closeout
Run full checklist; publish remediation closeout with PASS/FAIL per BUG-RS-### (§13 ledger).

---

## 7. Ownership Matrix

| Concern | Owner |
|---|---|
| Quarantine policy / panel contract | Core `PlayerSurfaceContract` + Godot registry |
| Moral / craft / radio / fire rules | `Assets/Ashfall.Core/` |
| Presentation + route wiring | `src/Main*.cs`, `src/UI/` |
| Data authority | `Assets/StreamingAssets/Data/` |
| CI honesty | `Ashfall.Core.Tests/Tooling/*GateTests.cs` |
| Agent truth | `AGENTS.md` (then sync rulebooks) |

---

## 8. Test Strategy

| Layer | What |
|---|---|
| Regression unit | Craft attribution, radio JSON parity, WornGear single-type, journal coverage claims |
| Panel lifecycle | Repeated bind/unbind for 4 leak panels (extend `PanelLifecycleTests.cs` / `PanelBindLifecycleSelfTest.cs`) |
| Route gate | Quarantined vs live bind dispositions (new `PlayerSurfaceQuarantineGateTests`) |
| Triad gate | Setup/Save parity for stateful systems |
| Headless | `--greenhouse-selftest`, moral resolve path, fire campaign bind, `--data-integrity-selftest` |
| Full | `dotnet test`, `dotnet build Ashfall.csproj`, bridge selftest |

---

## 9. File Impact Map (expected)

| Area | Action | Risk |
|---|---|---|
| `src/Main.PlayerSurfaces.cs` | MODIFY quarantine / binds | Medium |
| `src/UI/*Panel.cs` (30 stubs + 4 leak + moral + greenhouse) | MODIFY | Medium |
| `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs` | MODIFY | Medium (save/state overrides stay) |
| `Assets/Ashfall.Core/Crafting/*` + Survivors specialty | MODIFY | Medium |
| `Ashfall.Core.Tests/Tooling/*` | MODIFY/CREATE gates | Low |
| `AGENTS.md` + sync | MODIFY truth scrub | Low |
| `docs/player_surface_manifest.json` | REGENERATE | Low |
| New Core systems for 30 consoles | **MUST NOT** in this plan | — |

---

## 10. Risks

| Risk | Mitigation |
|---|---|
| Quarantine removes "content" players expected | Prefer explicit Offline notice over silent delete; keep classes for later binding |
| Radio JSON migration breaks station state saves | Preserve state-override save path; migrate defs only |
| Craft attribution changes RNG draw order | Attribution must not add extra RNG; specialty uses existing craft events |
| Triad "fixes" that add empty Save methods | Only persist real state; mark ephemeral explicitly |
| Scope explosion into building 30 systems | Hard out-of-scope; quarantine is the fix |
| Quarantining an already-bound panel (e.g. Electrostatic scrubber) | Explicit keep-list of Core-bound panels; re-verify 029 slurry sump |

---

## 11. Out of Scope

- Flagship asset generation / audio remaster (current branch noise)
- Inventing Core gameplay for quarantined consoles
- Expansion 11 Orbital Harrow / Plan 30 spiritual activation
- Main.cs full file-per-domain decomposition (beyond triad honesty)
- Balance retunes unrelated to correctness
- Unity / `_Game` anything

---

## 12. Rollback

- Phase 1 quarantine: revert registry route changes; panels remain in tree but unreachable.
- Radio/craft: feature-flag or keep legacy `RegisterDefaults` behind failing gate until JSON parity green.
- Each phase = one conventional commit: `fix(stability): BUG-RS-xxx…`.

---

## 13. Definition of Done + Verification Checklist

- [ ] All 65 BUG-RS IDs marked RESOLVED, QUARANTINED (accepted), or DEFERRED (with owner) in the ledger below
- [ ] No player-routable open-only false-affordance consoles without disposition
- [ ] Moral choice resolvable; fire/onboarding/subscriptions correct
- [ ] Radio single authority; craft attribution live
- [ ] AGENTS High table matches code
- [ ] Verification checklist all PASS

### Verification checklist
```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj
4. godot --headless --path . --data-integrity-selftest
5. godot --headless --path . --bridge-selftest
```
Plus phase-specific: `--greenhouse-selftest`, panel bind lifecycle, moral/fire focused tests.

### Phase 0 baseline (fill on execution)
- HEAD: `<to record>`
- `dotnet test`: `<PASS / known-fail list>`
- `dotnet build Ashfall.csproj`: `<PASS / known-fail>`
- `--data-integrity-selftest`: `<PASS / known-fail>`
- `--bridge-selftest`: `<PASS / known-fail>`

### Closeout ledger (PASS/FAIL per BUG-RS-###)

> Fill during Phase 8. Status values: `RESOLVED` / `QUARANTINED` (accepted) / `DEFERRED` (owner) / `FAIL`.

| ID | Status | Phase | Commit | Notes |
|---|---|---|---|---|
| 001 | _pending_ | 1 | — | |
| 002 | _pending_ | 1 | — | |
| 003 | _pending_ | 1 | — | |
| 004 | _pending_ | 1 | — | |
| 005 | _pending_ | 1 | — | |
| 006 | _pending_ | 1 | — | |
| 007 | _pending_ | 1 | — | |
| 008 | _pending_ | 1 | — | |
| 009 | _pending_ | 1 | — | |
| 010 | _pending_ | 1 | — | |
| 011 | _pending_ | 1 | — | |
| 012 | _pending_ | 1 | — | |
| 013 | _pending_ | 1 | — | |
| 014 | _pending_ | 1 | — | |
| 015 | _pending_ | 1 | — | |
| 016 | _pending_ | 1 | — | |
| 017 | _pending_ | 1 | — | |
| 018 | _pending_ | 1 | — | |
| 019 | _pending_ | 1 | — | |
| 020 | _pending_ | 1 | — | |
| 021 | _pending_ | 1 | — | |
| 022 | _pending_ | 1 | — | |
| 023 | _pending_ | 1 | — | |
| 024 | _pending_ | 1 | — | |
| 025 | _pending_ | 1 | — | |
| 026 | _pending_ | 1 | — | |
| 027 | _pending_ | 1 | — | |
| 028 | _pending_ | 1 | — | |
| 029 | _pending_ | 1 | — | re-verify Core bind first |
| 030 | _pending_ | 1 | — | |
| 031 | _pending_ | 3 | — | |
| 032 | _pending_ | 2 | — | |
| 033 | _pending_ | 2 | — | |
| 034 | _pending_ | 2 | — | |
| 035 | _pending_ | 2 | — | |
| 036 | _pending_ | 2 | — | |
| 037 | _pending_ | 2 | — | |
| 038 | _pending_ | 3 | — | |
| 039 | _pending_ | 4 | — | |
| 040 | _pending_ | 4 | — | |
| 041 | _pending_ | 4 | — | |
| 042 | _pending_ | 5 | — | |
| 043 | _pending_ | 5 | — | |
| 044 | _pending_ | 5 | — | |
| 045 | _pending_ | 5 | — | |
| 046 | _pending_ | 5 | — | |
| 047 | _pending_ | 5 | — | |
| 048 | _pending_ | 5 | — | |
| 049 | _pending_ | 6 | — | |
| 050 | _pending_ | 6 | — | |
| 051 | _pending_ | 6 | — | |
| 052 | _pending_ | 6 | — | |
| 053 | _pending_ | 6 | — | |
| 054 | _pending_ | 6 | — | |
| 055 | _pending_ | 6 | — | |
| 056 | _pending_ | 6 | — | |
| 057 | _pending_ | 6 | — | |
| 058 | _pending_ | 6 | — | |
| 059 | _pending_ | 4 | — | |
| 060 | _pending_ | 6 | — | |
| 061 | _pending_ | 7 | — | |
| 062 | _pending_ | 7 | — | |
| 063 | _pending_ | 7 | — | |
| 064 | _pending_ | 7 | — | |
| 065 | _pending_ | 7 | — | |

---

## 14. Implementation Handoff

### MUST PRESERVE
- Core engine-agnostic
- Campaign envelope / checksum saves
- `ISeededRng` determinism
- Electrostatic scrubber's existing `VentilationHostSession` bind

### MUST ADD
- Quarantine disposition + CI gate
- Moral resolve UI path
- Campaign-owned fire bind + stable seed
- Stored-delegate event lifecycle on 4 panels
- Production `CraftContext` emission/consumption
- Radio JSON authority
- AGENTS/gate truth scrub

### MUST NOT DO
- Build 30 new shelter systems to "fill" stubs
- Patch UI to fake Core state
- Touch Unity
- Bundle unrelated flagship asset work

### FIRST SAFE IMPLEMENTATION STEP (Phase 1 entry)
1. This document is now in place (Phase 0 deliverable).
2. Add failing `PlayerSurfaceQuarantineGateTests` that lists the 30 open-only route IDs (§5 Cluster A) and asserts each is either `Quarantined` or has a typed `bindAction` to a Core/host session.
3. Quarantine those routes in `src/Main.PlayerSurfaces.cs` (replace `openAction`/`closeAction`-only `ConfigureActions` with a `Quarantined` disposition, or route to a non-interactive "system offline" notice) until the gate goes green.
4. Fix `IsBound` honesty on the 30 panel classes (no hard-`true` without a typed bind).
5. Regenerate `docs/player_surface_manifest.json` and update the AGENTS stub table.

### Suggested follow-up skill after approval
`ashfall-implement` (or `ashfall-repair` per cluster), starting Phase 1.

---

## 15. Evidence Audit Trail

Re-verification performed at plan authoring time against the primary tree
(`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`):

- 30 route IDs confirmed present at `src/Main.PlayerSurfaces.cs:543–659`, each `ConfigureActions` with `openAction`+`closeAction` and **no** `bindAction`.
- `CryogenicPermafrostCorePanel.cs:32` confirms `IsBound = true` hardcoded.
- `src/Main.MoralChoice.cs:91` defines `TryResolveMoralChoice`; no call sites under `src/`.
- `src/Main.PlayerSurfaces.cs:389` binds `new ShelterFireHazardSystem()` + `"inc_default"`.
- `src/UI/FireIncidentPanel.cs:165` seeds `new CoreSeededRng(_incidentId.GetHashCode())`.
- `src/UI/WeatherHistoryPanel.cs:30` unsubscribes a fresh lambda (`-= _ => RefreshView()`); same no-op pattern in GeigerCalibration / FireIncident / Triangulation.
- `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs` contains `RegisterDefaults`.
- `CraftContext` referenced only by `Assets/Ashfall.Core/Crafting/CraftContext.cs` + `Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs` (no `src/` consumer).
- `Ashfall.Core.Tests/Tooling/LoaderWiringGateTests.cs:48–56` lists the 6 dormant + Collectible allowlist entries.
- `AGENTS.md:308–336` confirms Critical C1–C6 + High H1–H12 resolution state.

> Note: a `.claude/worktrees/plan06-narrative/` shadow tree exists. Implementation must target the primary tree, not the worktree.
