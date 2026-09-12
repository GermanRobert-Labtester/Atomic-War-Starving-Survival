# Partial Plans — Verified Audit (2026-09-12)

**Type:** read-only forensic re-verification · **Method:** `ashfall-scan` chain
(`DECLARED → COMPILED → CONSTRUCTED → REGISTERED → CALLED → OBSERVED → PERSISTED`)
· **Evidence rule:** AGENTS.md rule 7 — every claim rechecked in current source,
not inferred from plan/map prose.

Closes the loop on `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md` and
`PLAN_170_199_FORENSIC_AUDIT.md`, whose 173/184/187/189/196 entries have since
closed. Everything below was verified against source on this date.

---

## 1. Genuinely open — signed map, implement never landed

| # | Debt id | Verified evidence | Status | Priority |
|---|---|---|---|---|
| P1 | `DEBT-189-INTAKE-ADVISORY-BRIDGE` | `WaterTreatmentSystem.RegisterContaminationAdvisory` (WaterTreatmentSystem.cs:601) has **zero callers** repo-wide; `AquiferPiezometerEngine.BuildAdvisory()` (AquiferPiezometerEngine.cs:603) never reaches it. The doc comment at AquiferPiezometerEngine.cs:12 names the bridge explicitly. | **UNWIRED** | G1 |
| P2 | `DEBT-176-CAMPAIGN-AGE-CLOCK` | No `CampaignAge`/`AgeClock` type exists (0 files). | UNSTARTED | G2 |
| P3 | `DEBT-177-SLEEP-EVENT-CONSUMER` | No `SleepEvent` type (0 files). | UNSTARTED | G2 |
| P4 | `DEBT-178-CREATION-TO-VAULT` | No `CreateArtwork`/creation authority (0 files); vault exists only as archive. | UNSTARTED | G2 |
| P5 | `DEBT-182-LAST-INTERACTION-STAMP` | Only `_onboardingLastInteractionSeconds` (Main.Onboarding.cs:31) — no survivor-relation interaction stamp. | UNSTARTED | G2 |
| P6 | `DEBT-188-SCHEDULE-HOUR-CONSUMER` | No `ScheduleHour` type (0 files). | UNSTARTED | G3 |
| P7 | `DEBT-194-CRISIS-PRODUCER-WIRE` | No `CrisisCoordinator` type (0 files); only the HUD exists. | UNSTARTED | G3 |
| P8 | `DEBT-198-PIPELINE-EVENT-LOG` | No `HealthHistory`/`MedicalRecord` type (0 files). | UNSTARTED | G2 |

## 2. STALE debt — already implemented; verify-and-retire candidates

| # | Debt id | Verified evidence | Recommendation |
|---|---|---|---|
| S1 | `DEBT-185-SKILL-DORMANCY-TICK` | `SkillAtrophySystem` (Core) is ticked daily: `SurvivorSocialCoordinator.TickDay` step 6 → `Atrophy.Tick(24f, _actorAdapters)` (SurvivorSocialCoordinator.cs:237); coordinator is constructed at Main.SurvivorSocial.cs:26 and ticked at Main.SurvivorSocial.cs:124 from `TickSimDay`. Atrophy state is inside the persisted `survivor_social` section. | The "host-wire skill dormancy" contract appears satisfied. Needs a focused test proving dormancy crosses the threshold in-host before retiring the debt. |
| S2 | "Plans 127–129 land in later waves" | `src/Main.Plans126_129.cs:5` still says 127–129 are pending, but `docs/plans/PLAN127_IMPLEMENTATION_LOG.md` and `docs/plans/PLAN_129_FOUNDRY_PRODUCTION_CLOSEOUT.md` record them complete (129 = SilentFoundry production). | Stale header comment; correct the comment, do not rebuild. |
| S3 | Architecture-map GAP rows | Map reports **59 `❌ GAP` of 176 rows** (47 "no UI"), but sampled rows are partly stale: `chemical_recon` and `recon_telemetry` now have panel files, and Plan 138 (added this week) had to be added manually. | Re-audit the map before treating GAP as fact; the map is not proof either way. |

## 3. Live code that is genuinely partial / placeholder

| # | Finding | Evidence | Status |
|---|---|---|---|
| P9 | Amputation/prosthetic limb **avatar + equipment-restriction integration** never landed | `src/Main.Plans190_193.cs:71` TODO ("Avatar integration — portrait variant, sprite attachment, animation set, equipment restrictions") and `:393` TODO; `RefreshSurvivorVisuals(string)` at `:392-396` is a print-only no-op (`avatar system not yet integrated`). Limb state mutates and journals, but nothing visual or equipment-gating consumes it. | **PLACEHOLDER** |
| P10 | `SurvivorInspectionHostSession` (Core) has **zero consumers** | `Assets/Ashfall.Core/Survivors/SurvivorInspectionHostSession.cs:24` — no reference in `src/` or tests repo-wide. `ItemInspectionModel` *is* wired (InventoryHostSession.cs:87, InventoryDetailPanel.cs:45), so the survivor half is the orphan. This is the literal substrate of `DEBT-186-INSPECTION-PROJECTION`. | **UNWIRED** |
| P11 | UI gaps for settled systems | `vehicle_garage`, `dynamic_quests`, `sky_defense_battery` have Core+host+save but **0 panel files** under `src/UI/` (verified by filename search). May be intentional (headless-first), but each is an unimplemented player-facing route. | UI GAP (triage) |

## 4. Confirmed blocked/unstarted (unchanged from the 170–199 audit)

Plans **174** backstories, **175** meta/New Game+, **181** difficulty,
**191** identification/appraisal — all still absent from source (0 matching
types; "Difficulty/Identification/Appraisal" grep hits are unrelated words in
barter/safe-cracking code).

## 5. Explicitly NOT gaps (rejected false positives)

- `Main.Plans*.cs` numbering drift is real (`Main.Plans190_193.cs` actually
  wires amputation/railways/fungi/justice; `Main.Plans178_181.cs` similarly
  mismatches) — but the *systems* are implemented. Number drift is a
  documentation hazard, not a missing feature.
- Plans 163–165, 204–205: cited in source and landed under other partials.
- The 118-file `Assets/Quarantine` / Twin archives: preserved by design.

## 6. Ranked follow-up

1. **P1** (G1) — wire `BuildAdvisory` → `RegisterContaminationAdvisory`; it is a
   two-call bridge with a signed map and no new authority.
2. **S1** (G1) — focused host test for skill dormancy, then retire the debt row.
3. **P9** (G2) — decide whether limb visuals/equipment restrictions are in
   scope; if yes, it needs the avatar owner, not a local patch.
4. **P10/S3** (G2) — reconcile `DEBT-186` with `SurvivorInspectionHostSession`
   and re-verify the 59 GAP rows.
5. **P2–P8** (G2/G3) — each needs its signed map's §3 contract implemented;
   none should start from the historical plan prose.

**No production code, tests, or ledgers were modified by this audit.**

---

## 7. Decisions recorded 2026-09-12 (step 3 of the fix order)

### D1 — Amputation/prosthetic visuals: **OUT OF SCOPE until a survivor-avatar owner exists**

Evidence: limb state is already presented — `src/UI/AmputationTriagePanel.cs:99-124,163`
consumes `LimbCondition` for wound/gangrene/amputation/prosthetic counts and per-limb
text. The TODO's targets ("portrait variant, sprite attachment, animation set") have
no owning system: survivor portraits resolve only through
`AssetRegistry.FallbackSurvivorPath` / `AssetCoverageScanner`, and no survivor
animation/avatar layer exists in `src/`. Building one is a new presentation
subsystem, not a wiring fix.

The only *functional* candidate is limb-based **equipment restriction**:
`LimbCondition` has no consumer outside the triage panel, so an amputated arm does
not restrict gear today. That must route through the equipment owner (not the
medical panel), and it is tracked as `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION`.

### D2 — `SurvivorInspectionHostSession`: **RETIRED as a duplicate projection**

Evidence: the class (`Assets/Ashfall.Core/Survivors/SurvivorInspectionHostSession.cs:24`)
and its `SurvivorInspectionSnapshot` have **zero consumers** repo-wide. Its read
model (`Hunger/Thirst/Fatigue/Warmth/Health/Morale/Hygiene`) duplicates exactly
what the live consumer already reads directly — `src/UI/SurvivorDetailPanel.cs:115-121`
reads the same fields from `SurvivorsHostSession`. The item half of the same
debt is genuinely wired (`ItemInspectionModel` → `InventoryHostSession.cs:87`,
`InventoryDetailPanel.cs:45`).

Reconciliation: DEBT-186's "promote only an event/inspection projection with
named consumers" condition is **not met** by the survivor class — there is no
consumer that would benefit. `SurvivorDetailPanel` (survivors) and
`InventoryDetailPanel` (items) already cover the inspection scope, so no new
projection is promoted. The orphan is quarantined as unused code awaiting a
delete-or-adopt decision.
