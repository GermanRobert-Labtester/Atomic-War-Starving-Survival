# ASHFALL — UNBLOCK PROGRAM · PLAN 1
## Body-Integrity Schema Unblock: F14 / XP-06, Equipment Restrictions, Prosthetics, Rehabilitation

**Status:** planning deliverable only. Read-only pass. No production, data, test,
save, or governance-ledger file is modified by this document. No path is claimed.
**Date:** 2026-09-21
**Baseline verified at:** `Zcode_Branch`, HEAD `5be1a30a63cd86cf23e4034473b739ac514f0f2a` (2026-09-20 02:03 +0300)
plus the current uncommitted worktree (untracked `docs/expansions/wave1..wave4/` only).
**Role:** unblock-plan author. This plan turns one blocked decision cluster into
an execution-ready, signed-decision-gated package that releases other plans. It
implements nothing.
**Authority chain:** `AGENTS.md` (Rules 1–10, active queue, decision-blocked list),
`INTEGRATION_PLANS.md` (current batch and operating rules), `WORKTREE_OWNERSHIP.md`
(claims), `TEST_POLICY.md` (focused verification), `KNOWN_DEBT.md` (debt rows),
`docs/governance/DECISION_REGISTER.md` (DEC-03, DEC-16), and current source.

---

## 0. How to read this plan

This is the first of five sibling unblocker plans produced on 2026-09-21. Each
sibling attacks one blocked region of the queue and is designed so that the
five can be signed and executed without file collisions:

| Sibling | Region | Primary releases |
|---|---|---|
| **Plan 1 (this)** | Equipment/body schema | XP-06, EN-04, DEBT-AMPUTATION-EQUIPMENT-RESTRICTION, Expansion 16 |
| Plan 2 | Funds, trade legs, trade routes | XP-04, XP-07 sequencing, XP-08, EN-03, expansions 17/25/26 |
| Plan 3 | Semantic kind, voice, string freeze | D11/CF-P3, Plan 42, Plan 46, Plan 49, EN-05, DEC-11/DEC-13, D19b |
| Plan 4 | Register truth, quarantine, census, residuals | D3, D4, D13, D16, D19a/c, D21, E1 continuation, Plan 24 residual, EN-08 |
| Plan 5 | Newest expansion waves, C3 holds, EN gate | Expansions 12–31 intake, 174/175/192/199, EN-01/02/06/07, XP-07/09/10 premise checks |

Sections 1–3 state what is blocked and prove the blocker today. Section 4 is the
signature bundle the foreman signs (or declines). Section 5 is the full technical
design so that a builder can execute immediately after signature without
re-deriving anything. Sections 6–10 are execution, verification, risk, and
ownership. Section 11 is the definition of done.

Vocabulary used throughout:

- **Release** = a plan/debt/pillar that becomes claimable once the signature in
  this plan is recorded.
- **Blast radius** = every file a signature's downstream execution is expected
  to touch, listed before the signature so decline is informed.
- **Premise audit** = the Rule 7 re-check of a plan's claims against current
  source and data before the first edit.

---

## 1. Executive summary

### 1.1 The block in one paragraph

`DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` records that the equipment half of the
amputation feature is **BLOCKED** because `ItemDefinition`/`EquipSlot` has no
handedness or limb-requirement field. That single missing schema is the named
blocker for the entire `XP-06-BODY-INTEGRITY` pillar (prosthetics catalog,
rehabilitation arc, equip gating), for `EN-04 Rehabilitation Medicine`, and for
the equipment-facing third of Expansion 16 *The Rebuilt Body*. It also mutates
the premise of a live test family (`Plan21ProtectiveWearTests`, `C2AmputationTravelTests`)
and touches the same `items.json` catalog region as the unresolved
`water_sample_contaminated` decision (D3, handled by Plan 4). Nothing about this
block is technical uncertainty: the schema design, grip vocabulary, migration
shape, validator rules, and gating algorithm are all derivable today, and the
`AmputationSystem.LimbState` record has already carried a `prostheticId` field
since the medical flagship, waiting for this schema to exist.

### 1.2 Signature bundle in one glance

| # | Sign-off line | Releases | Blast radius |
|---|---|---|---|
| F14-A | `I sign the additive ItemDefinition limb/schema extension: optional limb_requirements (hands, grip_class) + optional provides_limb; old catalogs deserialize byte-identical.` | Equipment gate, validator rows, ItemCatalogLoader | `ItemDefinitions.cs`, `ItemCatalogLoader.cs`, `CatalogIntegrityValidator.cs`, `items.json` (additive rows only) |
| F14-B | `I sign the two-class grip vocabulary: simple and full. No third class may be introduced without a new schema version.` | Dual-wield guard, prosthetic satisfaction rule | `ItemDefinitions.cs` (enum), validator, test vocabulary |
| F14-C | `I sign survivor body_state as an additive sub-object on the existing survivors save section; migration inserts intact limbs.` | Equip gate persistence, rehab arc persistence, reload equality | `SaveSectionRegistry` (survivors only), `Main.Survivors.cs`-area save owner, `SurvivorSave` shape |
| F14-D | `I sign the prosthetics launch set as tiered items resolved through ItemCatalog + EquipmentConditionSystem; no second condition model.` | Prosthetics catalog, crafting chain, decay | `items.json`, crafting catalogs, `EquipmentConditionSystem` consumers |
| F14-E | `I sign the rehabilitation arc: fitting → adaptation → mastery, driven by the existing medical pipeline event and the existing shared skill tick; no new psychology system.` | XP-06-F5, EN-04, expansion 16 rehab content | medical pipeline host beat, `TickSharedSkillProgression` call site |
| F14-F | `I sign phantom-pain nights as a variant of the existing SleepNarrativeProjection classification; no DreamSystem, no phobia ledger.` | XP-06-F6, XP-10 prerequisite clarity | `SleepNarrativeProjection.cs`, one journal beat |
| F14-G | `I sign survivor-panel prosthetic visibility for wave 1 (limb state + prosthetic slot + quality text; words-not-color).` | Presentation slice, a11y | `SurvivorDetailPanel.cs`, a11y selftest |

The bundle is deliberately split so the foreman can sign the schema (F14-A/B/C)
without signing the content or presentation. F14-A/B/C alone release the
equipment half of the debt and the gating tests. F14-D/E/F/G release XP-06 in
full and open EN-04's gate.

### 1.3 What is NOT in this plan

- Automation, drones, and rogue-robot arcs from Expansion 16 (`RoboticsSystem`)
  — those are a separate expansion scope, not part of F14. Section 5.9 draws the
  boundary explicitly so a builder cannot inflate this package.
- Any change to `AmputationSystem`'s surgical procedure behavior (sealed).
- Any new needs, health, or condition ledger. `EquipmentConditionSystem` stays
  the condition authority; `NeedsSystem` stays the need authority.
- Any revival of the deleted avatar/visual placeholder (`RefreshSurvivorVisuals`
  is RETIRED in `KNOWN_DEBT.md`; do not restore it).
- Any second currency or inventory model (that is Plan 2's territory).
- The `water_sample_contaminated` equipability quirk (D3, Plan 4). It sits in
  the same catalog region; this plan executes after D3 lands so `items.json` is
  touched once, not twice.

---

## 2. Verified current reality

Every statement below was verified against source on 2026-09-21 at HEAD
`5be1a30a`. Where a claim comes from a plan document rather than source, it is
labelled as such and treated as a premise to re-verify, per `AGENTS.md` Rule 7.

### 2.1 Equipment schema today

`Assets/Ashfall.Core/Inventory/ItemDefinitions.cs`:

- `class ItemDefinition` begins at line 75.
- The canonical slot field is `public EquipSlot equipSlot;` at line 90.
- Slot handling helpers at lines 197–199 special-case `Face` and `Body`.
- `EquipSlots` (line 246+) is the single parser: legacy aliases
  (`torso`/`chest` → `Body`), canonical enum names, `TryParse`, `Parse`,
  `IsCanonicalName`. **There is no handedness, grip, strength, or limb field
  anywhere in the file.** Verified by grep for `handedness|limb` — zero hits in
  this file.

Consequence: the type system cannot express "this weapon needs two hands", "this
prosthetic provides one simple hand", or "this boot needs a right leg". An
arm-state equip restriction therefore has nowhere to live; this is exactly the
recorded blocker.

### 2.2 Equip path today

`Assets/Ashfall.Core/Inventory/Inventory.cs`:

- `public bool Equip(ItemDefinition item)` at line 849.
- Its only preflight is `item == null || !item.isEquipable || item.equipSlot ==
  EquipSlot.None` (line 851). There is no strength check, no two-hand check, no
  body-state check, and no auto-unequip edge.
- `Unequip(EquipSlot slot)` (861), `TryUnequipTo(EquipSlot slot, Inventory
  destination)` (879), `GetEquipped(EquipSlot slot)` (901) are the surrounding
  API.

### 2.3 Amputation/limb state today

`Assets/Ashfall.Core/Medical/AmputationSystem.cs`:

- `enum LimbId` (line 11), `enum LimbCondition` (19).
- `sealed class LimbState` (31) **already carries** `prostheticId` (38),
  `recoveryDaysLeft` (39), and `hasPhantomPain` (40). The seat for prosthetics
  was carved during the medical flagship; what is missing is the catalog, the
  equip gate, and the persistence projection.
- `SurgicalProcedureDef` (44) carries `phantom_pain_chance` (56) — the phantom
  pain trigger data already exists.
- The expedition consumer is sealed: `ExpeditionState.survivorSpeedMultiplier`
  consumes `AmputationSystem.GetMovementSpeedMultiplier` at dispatch
  (`KNOWN_DEBT.md` `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION`, expedition half
  SEALED 2026-09-17).

### 2.4 Persistence today

`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`:

- `new("survivors", "SaveSurvivors", "SetupSurvivors", "survivors", "Living
  survivors, needs, and traits")` at line 67.
- Filename row `{ "survivors", "survivors_save.json" }` at line 295.

There is no `body_state` row and no dedicated body save section. The correct
home for limb/prosthetic state is an **additive sub-object inside the existing
`survivors` section**, because `AmputationSystem` already rides it (the surgical
state was persisted through that owner). Adding a sibling section would create a
second restore path for the same domain — forbidden by `AGENTS.md` Rule 5.

### 2.5 Validator and content-utilization gates today

- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` is the permanent data gate;
  it already validates dozens of catalogs including distress follow-ups
  (sealed 2026-09-19) and difficulty presets (XP-01, 2026-09-19).
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` is the dead-data
  gate, extended by CF-P6 for vehicle armor grades on 2026-09-19.

New optional schema fields must be validated (range/sanity) and new catalog
rows must be consumed by a real system, or the content-utilization gate will
report them as orphans. Section 5.6 states the exact rules.

### 2.6 The recorded blockers, exactly as written

`KNOWN_DEBT.md` — `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` (SPLIT-SEALED):

> **Equipment half BLOCKED → separate package:** `ItemDefinition`/`EquipSlot`
> has no handedness/limb-requirement field, so an arm-state equip restriction
> needs a schema redesign (C2 §2 forbids doing it inside C2). … Equipment:
> signed schema package. Never add presentation-only limb illusions.

`docs/governance/DECISION_REGISTER.md` — `DEC-03` is
`DEFERRED-WITH-CONDITION`, condition: "Equipment model lacks handedness/limb-slot
fields; requires schema extension package rather than inline patch", execution
package "Wave 11 Equipment Schema Tranche", recheck trigger "Schema migration
wave for equipable item slots". `DEC-16` (affliction-specific recovery ramps)
is `DEFERRED-WITH-CONDITION`, condition: "Medical ward admissions continue
using global bed-tier recovery curves until patient admission records author an
explicit cause field."

`docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` §4:

> `CF-XP06-BODY-INTEGRITY` (roster 15 / XP-06) — **F14** schema sign-off
> (DEC-03 successor); `ItemDefinition`/`EquipSlot` still has no
> handedness/limb-requirement field.

The 2026-09-18/19 wave sealed D1/D2/D5–D10/D14/D15/D23-item-1 but explicitly
did **not** touch D12/F14; it remains the largest single-catalog blocker after
the already-resolved D8.

### 2.7 Test families this change perturbs

| Test | Why it sees this change | Required handling |
|---|---|---|
| `Ashfall.Core.Tests/Inventory/Plan21ProtectiveWearTests.cs` | protective gear equips through `Inventory.Equip`; a stricter gate could change fixture outcomes if the fixture survivors lack limb data | migration default = intact limbs; tests must pass unmodified (proves additive) |
| `Ashfall.Core.Tests/Expeditions/C2AmputationTravelTests.cs` | sealed expedition movement consumer; must remain at 7/7 | no behavior change unless a prosthetic is equipped (parity tests) |
| `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | section-count gate | if body_state is a sub-object, **no count change**; if a sibling section were used, the count would change — one more reason to stay additive |
| `Ashfall.Core.Tests/Tooling/MainTriadDriftGateTests.cs` | new save fields must be registered | register in the same commit as the shape |
| `Ashfall.Core.Tests/Tooling/DataAuthorityFidelityTests.cs` | new items.json rows must be reachable | each prosthetic must be in a crafting chain / loot table |
| `CatalogIntegrityValidatorTests` | new validator hook | add positive + negative fixtures |

### 2.8 Newest plans that consume this same seam (enrichment targets)

The untracked 2026-09-20 expansion waves (Plans 12–31) repeatedly intersect this
schema, which raises the value of the signature:

- **Expansion 16 · The Rebuilt Body** (`docs/expansions/wave1/expansion_16_the_rebuilt_body_plan.md`)
  proposes simple prosthetics, rehabilitation, complications, and then
  automation/drones/rogue arcs. Its first half is exactly XP-06; its second half
  is `RoboticsSystem` scope. Plan 5 intakes the wave; **this plan should be
  referenced as Expansion 16's prerequisite for its prosthetic/rehab half.**
- **Expansion 27 · The Thread** (Wave 4) proposes garments/layers. Layered
  garments want slots that a limb-requirement vocabulary must not accidentally
  block; F14-A must remain strictly optional so expansion 27 is unaffected.
- **Expansion 29 · The Glass** mentions spectacles/vision care; if vision is
  ever a gating input, it must route through the medical pipeline, not a second
  limb-style field. This plan's enum should therefore stay narrowly scoped to
  `hands`/grip and legs, and explicitly reserve future needs to a schema
  version bump.
- **Expansion 5 / 13 · The Faithful** and **Expansion 24 · The Long Goodbye**
  mention rites around the dead and mechanical reverence; they must not depend
  on the schema. This plan keeps them independent.

### 2.9 Why the blocker is now cheaper than when it was recorded

When `C2` recorded the block (2026-09-17), the expedition consumer did not yet
exist and `AmputationSystem`'s persisted projection was unsettled. Today:

1. The expedition consumer is sealed and parity-tested, so the movement side is
   already done; only the **equipment** side is missing.
2. `LimbState.prostheticId` already exists, so the migration target fields are
   known, not speculative.
3. `EquipmentConditionSystem` has since become the sealed condition authority
   with a `RecordWear` single mutation API (C2 Plan 21), so prosthetics can
   reuse it instead of inventing decay.
4. Difficulty presets (XP-01) added a worked example of an additive catalog
   with validator + save stamping, which this plan mirrors.
5. `SaveSectionRegistry` count is now 338 catalogs / 190+ sections; an additive
   sub-object is the proven-safe pattern (used by `MedicalPipelineSaveState`
   `record` and `BlackMarketState.factionBounties` precedent).

---

## 3. Blocked-plan inventory released by this plan

### 3.1 Primary releases

#### 3.1.1 `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` — equipment half (the debt row itself)

- **Blocked since:** 2026-09-17 (`KNOWN_DEBT.md`, SPLIT-SEALED).
- **Block statement:** no handedness/limb-requirement field.
- **Released by:** F14-A + F14-B (the schema and vocabulary), executed by
  Phase 2 (gate) and Phase 3 (prosthetic provision).
- **Release evidence this plan pre-commits to:** `EquipLimbGateTests` proving
  (a) a two-hand item is refused with one `simple` hand available, (b) a
  `full`-grip item is refused when only a hook prosthetic is fitted, (c) an
  amputation event auto-unequips a violating item, once, with one journal line.
- **Closure:** the debt row may be marked RETIRED with the test path and a
  no-reopen condition.

#### 3.1.2 `XP-06-BODY-INTEGRITY` (roster 15 / the XP pillar)

- **Pillar features:** XP-06-F1 schema; F2 equip gating; F3 prosthetics catalog
  + crafting chain; F4 tiers/maintenance; F5 rehabilitation arc; F6 phantom pain
  + overuse.
- **Released by:** F14-A/B (F1, F2 schema), F14-C (persistence), F14-D (F3/F4),
  F14-E (F5), F14-F (F6), F14-G (presentation).
- **Sequencing internal to the pillar:** F1→F2→F4→F3→F5→F6. (Maintenance hooks
  must exist before content rows are authored, or content is dead on arrival.)
- **After this plan:** XP-06 stops being "decision-blocked" and becomes an
  execution package under the XP W1 umbrella (the active batch may claim it once
  each consumer premise check passes, per the batch's own rule).

#### 3.1.3 `EN-04 Rehabilitation Medicine`

- **Gate:** hard-blocked on F14 per `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` §4
  ("EN-04 … hard-blocked on Bundle D (D12/F14)").
- **Released by:** F14-E (rehab arc) + F14-C (persistence). EN-04's own
  authorization line is signed in Plan 5 after this plan's F14-E executes, per
  the gating rule that authorizing an EN ahead of its bundle recreates the
  blocker one layer up.
- **Enrichment note:** EN-04's "slate read model" (§C.4.4 of the EN program)
  should consume this plan's rehab phase state as the single source; it must not
  invent a second recovery ledger.

#### 3.1.4 Expansion 16 · The Rebuilt Body — prosthetic/rehab half

- **Status:** untracked design bible (2026-09-20), pre-integration, not
  authorized.
- **Blocked by:** absence of the schema; also by wave intake (Plan 5).
- **Released by:** F14-A–E. The automation/drone/rogue half remains with
  `RoboticsSystem` and Plan 5; this plan explicitly does not authorize it.
- **Enrichment:** Expansion 16's phases should be re-ordered so its prosthetic
  phase states "depends on UNBLOCK-01 F14-A..E; item ids from this plan's
  launch set are canonical" and its automation phase states "requires a separate
  Robotics claim".

#### 3.1.5 `DEC-03` and `DEC-16` register rows

- **DEC-03:** replaced by the signed F14 rows; the register gains a new DEC row
  (proposed `DEC-21`) recording the schema extension and retiring DEC-03's
  condition.
- **DEC-16:** not released by this plan's core, but its condition ("admission
  cause field") is adjacent: the rehab arc creates the first admission-cause-like
  record. Section 5.8 proposes the minimal path to also retire DEC-16 in the
  same tranche without inventing a ward-admission schema: reuse the rehab phase
  record as the cause carrier only if the ward consumer agrees. Otherwise DEC-16
  stays deferred and is handled by Plan 4's register-truth pass.

### 3.2 Secondary releases (things that gain clarity or lose a hidden dependency)

| Item | How this plan touches it |
|---|---|
| `Plan21ProtectiveWearTests` | passes unchanged after migration (additive proof) |
| `C2AmputationTravelTests` 7/7 | passes unchanged; new parity case added for prosthetized leg |
| `water_sample_contaminated` (D3) | **execution ordering constraint:** this plan touches `items.json`; Plan 4 signs D3 first so the catalog is edited once |
| Expansion 27 garments | receives an explicit "optional fields only, no slot multiplication" guarantee from F14-A |
| XP-10 phobia/trait growth | XP-06-F6 phantom pain is explicitly a sleep-beat variant, not phobia growth; DEC-18 (phobia growth RETIRED) remains untouched. This plan's F14-F wording must match |
| Plan 24 rehab/recovery ramp effort | F14-E gives the medical pipeline a phase record; that record is the first concrete candidate for the "admission cause" DEC-16 needs |

### 3.3 Non-releases (explicitly not claimed)

- No `RoboticsSystem` work; no drones; no rogue arcs.
- No avatar/visual work; no `RefreshSurvivorVisuals` restoration.
- No second condition ledger; no second needs ledger.
- No change to the sealed expedition speed multiplier semantics.
- No new survival-stat gating (vision, hearing, respiration) — reserved to a
  future schema version after separate signature.

---

## 4. Decision packet

Format follows the existing unblocker convention: each decision gets an
authoritative sign-off line, options, recommendation, blast radius, and decline
path. Signing is the only action a foreman must take; everything else in this
plan is builder work.

### 4.1 F14-A — additive `ItemDefinition` schema extension

**Sign-off line (copy exactly):**

> `I sign the additive ItemDefinition limb/schema extension: optional limb_requirements (hands, grip_class) + optional provides_limb; old catalogs deserialize byte-identical.`

**Meaning:**

- `ItemDefinition` gains two optional fields (nullable). Absent field = current
  behavior, byte-identical serialization for old catalogs.
- `limb_requirements.hands` is an integer 1–2. `limb_requirements.grip_class`
  is `simple` or `full` (F14-B).
- `provides_limb.hands` (0–2) and `provides_limb.legs` (0–1) describe what a
  prosthetic supplies when fitted, plus a `quality_permille` integer.

**Options considered:**

| Option | Description | Verdict |
|---|---|---|
| A. Additive optional fields (recommended) | two nullable sub-objects on `ItemDefinition` | smallest diff; no migration for old saves; validator can ignore absent fields |
| B. Required fields with defaults | every item row must author them | mass catalog churn; violates "smallest coherent change" |
| C. Separate `equipment_rules.json` keyed by item id | rules live outside the item | introduces a second lookup authority for equip decisions; slower, two files to keep in sync |
| D. Declined — never restrict equipment | amputation permanently does not restrict gear | keeps the debt permanently, blocks XP-06/EN-04; only viable if the product decision is "no gating ever" |

**Recommendation:** Option A, with Option B only for the small launch set of
two-handed weapons and prosthetics (authored explicitly). Option D is a valid
decline that costs nothing to the current game but must then retire the debt
row and the XP-06 pillar explicitly rather than leaving them parked.

**Blast radius if signed (execution paths):**

- `Assets/Ashfall.Core/Inventory/ItemDefinitions.cs` (fields + clone + parse)
- `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` (map snake_case JSON)
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` (new validation hook)
- `Assets/StreamingAssets/Data/items.json` (additive rows only: launch-set
  two-handed items gain `limb_requirements`; prosthetics are new rows)
- `Ashfall.Core.Tests/Inventory/` (schema tests)
- generated `docs/data/CATALOG_REGISTRY.md` (via generator, not by hand)

**Decline path:** debt row stays BLOCKED; no file is touched; Plan 5 records
Expansion 16's prosthetic half as permanently out of scope unless re-proposed.

### 4.2 F14-B — two-class grip vocabulary

**Sign-off line:**

> `I sign the two-class grip vocabulary: simple and full. No third class may be introduced without a new schema version.`

**Rationale:** the exploit surface is proportional to vocabulary size. With two
classes, the only satisfiable case is "hook/prosthetic hand can hold simple
items, not full weapons", which is exactly the narrative intent and is trivial
to test. Three or more classes create combinatorics the player cannot read and
the validator cannot cheaply pin.

**Semantics (normative):**

| Grip class | Meaning | Prosthetic `simple` hand satisfies? | Prosthetic `full` hand satisfies? |
|---|---|---|---|
| `simple` | one-handed tools, knives, pistols, lamps, documents | yes | yes |
| `full` | two-handed weapons, heavy tools, long arms | no | only with two providing limbs |
| (absent) | no requirement — current behavior | yes | yes |

**Blast radius:** enum + parser + validator + tests; no data change beyond the
launch set.

**Decline path:** schema can still ship with `hands` only and no grip class;
but then the "hooks cannot fire rifles" exploit is unguarded. Decline is
possible but should be recorded with that consequence.

### 4.3 F14-C — survivor `body_state` persistence

**Sign-off line:**

> `I sign survivor body_state as an additive sub-object on the existing survivors save section; migration inserts intact limbs.`

**Shape (normative):**

```jsonc
// inside the existing survivors section payload, per survivor
"body_state": {
  "limbs": {
    "left_arm":  { "condition": "prosthetized", "prosthetic_item_id": "item_hook_prosthetic" },
    "right_arm": { "condition": "intact" },
    "left_leg":  { "condition": "intact" },
    "right_leg": { "condition": "amputated" }
  },
  "rehab": { "prosthetic_type_key": "hand", "phase": "adaptation", "days_in_phase": 4, "quality_ramp_permille": 620 },
  "schema_version": 1
}
```

**Rules:**

- Old saves: field absent → migration constructs all-intact with
  `rehab = null`. No version bump of the `survivors` section is required because
  the sub-object is additive and nullable; the section's existing version stays.
- `AmputationSystem` remains the sole authority for limb condition transitions;
  `body_state` is its persisted projection, not a parallel state.
- One writer: the survivor save owner. `AmputationSystem` mutations flow into it
  through the existing host wiring; no panel writes it.
- Determinism: no RNG in this state. `quality_ramp_permille` is an integer,
  advanced by the rehab tick.

**Blast radius:** `SaveSectionRegistry.cs` (comment/owner registration only if
needed), the survivor save owner in `src/Main.*`, `AmputationSystem` projection
helpers, save round-trip tests, Triad drift gate registration.

**Decline path:** without persistence, prosthetics would reset on reload and the
rehab arc would be untestable across save/load; decline effectively declines
XP-06 as a whole.

### 4.4 F14-D — prosthetics launch set and condition integration

**Sign-off line:**

```text
I sign the prosthetics launch set as tiered items resolved through ItemCatalog + EquipmentConditionSystem; no second condition model.
```

**Launch set (proposal; item ids and craft chains are the enforcement detail):**

| Item id | Tier | Provides | Quality | Decay | Craft chain |
|---|---|---|---|---|---|
| `item_hook_prosthetic` | field | hands 1, simple only | 0.50 | 2.0%/day | cordage + bone carving |
| `item_leather_splint_hand` | field | hands 1, simple only | 0.40 | 1.5%/day | tanning + cordage |
| `item_pinned_metal_hand` | workshop | hands 1, simple+full single | 0.75 | 1.0%/day | foundry + ceramics joint |
| `item_articulated_hand` | advanced | hands 1, simple+full single | 0.90 | 0.7%/day | crucible foundry + wax bushings + glass bearings |
| `item_peg_leg` | field | legs 1 | 0.50 | 2.0%/day | carpentry |
| `item_sprung_leg_frame` | advanced | legs 1 | 0.85 | 0.8%/day | foundry + cordage cable |

All decay is applied through `EquipmentConditionSystem.RecordWear` — the sealed
single mutation API from Plan 21. A prosthetic at 0% condition behaves as
`amputated` until repaired or replaced; the limb state's `condition` remains
`prosthetized` but the effective gate reads condition first. Repair uses the
authored `repairRecipe` seam already wired by Plan 22.

**Blast radius:** `items.json` (6 new rows + launch-set requirements),
`metallurgy_recipes.json`/`crafting` catalogs (chains must resolve),
`ContentUtilizationScanner` (every new item must have a consumer),
`CatalogIntegrityValidator` (ranges), `EquipmentConditionSystem` consumers,
`radiation/contamination` fields default to 0 on new rows.

**Decline path:** ship only the field tier (two items) and defer advanced tiers;
XP-06-F4 remains parked but F1/F2/F5 still release.

### 4.5 F14-E — rehabilitation arc

**Sign-off line:**

```text
I sign the rehabilitation arc: fitting → adaptation → mastery, driven by the existing medical pipeline event and the existing shared skill tick; no new psychology system.
```

**Semantics:**

| Phase | Duration | Effect | Driver |
|---|---|---|---|
| `fitting` | 3–5 days (authored range, deterministic per procedure) | prosthetic provides limb at ×0.5 quality | medical pipeline event at fit time |
| `adaptation` | 10–20 days, scaled by resilience skill | quality ramps from ×0.5 to ×1.0 linearly, integer permille per day | daily tick beside `TickSharedSkillProgression` |
| `mastery` | permanent | small handling bonus for that prosthetic type; chronicle milestone through the Plan 178 hook | skill progression |

- The "existing medical pipeline event" is the `AmputationSystem` fit step;
  the pipeline already records treatment kinds. No new treatment kind is needed
  for fitting if the existing kind vocabulary can carry `prosthetic_fitting`;
  otherwise the smallest additive kind is proposed in Phase 5 design.
- The daily tick must not create a new day owner. It attaches to the existing
  `ShelterFacilitiesDayOwner` sequence exactly as the apprenticeship/skill
  ticks do.
- EN-04's rehab slate reads this phase state; it must not maintain its own.

**Blast radius:** `AmputationSystem` (fit command), medical host session,
`src/Main.Survivors.cs`/`src/Main.CampaignOwners.cs` tick attachment,
`SleepNarrativeProjection` untouched here, EN-04 later.

**Decline path:** fitting becomes instant and permanent; the arc collapses to a
quality number on the item. XP-06-F5 is then declined explicitly; EN-04 loses
its medical-rehab floor and should stay held.

### 4.6 F14-F — phantom-pain nights

**Sign-off line:**

```text
I sign phantom-pain nights as a variant of the existing SleepNarrativeProjection classification; no DreamSystem, no phobia ledger.
```

**Semantics:**

- `LimbState.hasPhantomPain` (already persisted) becomes an input to the
  existing sleep-beat classifier as a new severity variant ("phantom").
- Output is one restrained journal line, deduped by the existing per-survivor
  knowledge key, exactly as restful/crisis/insomnia beats are today.
- Nothing mutates. No dream system. No phobia vocabulary. `DEC-18` (phobia
  growth RETIRED) is untouched.

**Blast radius:** `SleepNarrativeProjection.cs` (classification input +
variant), `Plan177SleepNarrativeProjectionTests` (add one case), journal
presentation only.

**Decline path:** phantom pain stays a pure `LimbState` flag with no
presentation; XP-06-F6 is declined; the flag still exists for future use.

### 4.7 F14-G — survivor-panel presentation

**Sign-off line:**

```text
I sign survivor-panel prosthetic visibility for wave 1 (limb state + prosthetic slot + quality text; words-not-color).
```

**Semantics:** `SurvivorDetailPanel` gains a limb block: one row per limb with a
text state (`intact`, `amputated`, `prosthetic — <tier> (<condition>%)`,
`adapting <n> days`). Keyboard focusable; no color-only encoding (a11y gate);
no raw numbers without a label. No avatar, no body diagram.

**Blast radius:** `src/UI/SurvivorDetailPanel.cs`, a11y selftest,
`PanelRouteGate` unaffected (not a new panel).

**Decline path:** state remains API-only; the mechanical release (F14-A–E) is
unaffected.

### 4.8 Bundle ordering and interaction with the other four plans

- **Plan 4 first for D3.** `items.json` is touched by both D3 (water sample
  quirk) and this plan. Sign D3 before this plan executes, or execute this
  plan's catalog tranche in the same session as D3 so the file is edited once.
- **Plan 2 independent.** Funds/trade touch `economy_goods.json`, not `items.json`
  rows this plan needs; no collision.
- **Plan 3 independent.** Voice avoids inventory; the only shared surface is
  `SurvivorDetailPanel` presentation text, which Plan 3's string-freeze
  extraction may want to touch. If string freeze is declared, this plan's panel
  strings should use the freeze keys; coordination note in §9.3.
- **Plan 5 consumes.** Expansion 16 and the archetype expansions must be
  enriched with a dependency block referencing this plan (Section 3.1.4).

### 4.9 What a builder may NOT infer from this packet

- A signed F14-A does not authorize authoring 40 prosthetics; only the launch
  set in F14-D, and only after F14-D is signed.
- A signed F14-C does not authorize moving `body_state` to a new save section
  "later for cleanliness".
- A signed F14-E does not authorize touching `TickSharedSkillProgression`'s
  internals — only attaching beside it.
- A signed F14-F does not authorize a dream/phobia system; DEC-17/DEC-18 remain
  RETIRED.
- No signature here authorizes Expansion 16 as a whole.

### 4.10 Recommended signing order and first safe step

1. Sign F14-A, F14-B, F14-C together (the schema core). No data authoring yet.
2. Execute Phase 1 (schema + validator + migration) and Phase 2 (gate) — both
   additive and reversible.
3. Sign F14-D and author the launch set; execute Phase 3/4.
4. Sign F14-E and execute the arc.
5. Sign F14-F/F14-G for presentation.
6. Record all sign-offs in `DECISION_REGISTER.md` as new rows (proposed
   DEC-21..DEC-24) and flip DEC-03 to RETIRED/SIGNED with evidence.

**First safe step for a builder before any signature:** none of the production
phases may start. The only safe pre-signature work is re-running the premise
commands in Appendix A and confirming §2 facts still hold at the then-current
HEAD. If any fact has drifted, this plan's decision lines must be re-issued
against the new evidence.---

## 5. Technical design (execution-ready after signature)

This section is normative. A builder who starts after F14-A..C are signed should
be able to implement Phase 1–2 from this section alone, with only the Rule 7
premise commands in Appendix A as a re-check. Field names are proposed; the
ledger records the accepted names once signed, and no builder may rename them
mid-execution without a one-line register amendment.

### 5.1 Schema specification — C# side

`Assets/Ashfall.Core/Inventory/ItemDefinitions.cs` additions:

```csharp
/// <summary>
/// Optional limb/grip requirements for an equipable item.
/// Absent (null) means "no requirement" and preserves legacy behavior.
/// Introduced by UNBLOCK-01 F14-A. Additive: old catalogs deserialize unchanged.
/// </summary>
public sealed class LimbRequirement
{
    /// <summary>Number of hands the item occupies. Valid range: 1..2.</summary>
    public int hands;
    /// <summary>Grip class vocabulary (F14-B). "simple" or "full".</summary>
    public string gripClass = "simple";
}

/// <summary>
/// Optional limb provision for a prosthetic item.
/// Absent (null) for non-prosthetics.
/// </summary>
public sealed class LimbProvision
{
    /// <summary>Hands provided. Valid range: 0..2.</summary>
    public int hands;
    /// <summary>Legs provided. Valid range: 0..1.</summary>
    public int legs;
    /// <summary>Base quality in permille, 0..1000. Applied at fit; rehab ramps effective value.</summary>
    public int qualityPermille = 500;
}
```

`ItemDefinition` gains:

```csharp
public LimbRequirement? limbRequirements;   // JSON: limb_requirements
public LimbProvision? providesLimb;         // JSON: provides_limb
```

Clone behavior: both are deep-copied; `null` propagates as `null`. The catalog
loader binds snake_case (`limb_requirements`, `provides_limb`, `grip_class`,
`quality_permille`) using the file's existing attribute conventions. The
`EquipSlots` parser is untouched.

**Back-compat proof obligation:** a serialization round-trip of every existing
item row through the unchanged code path must produce byte-identical output
except where the new fields are explicitly authored. The Phase 1 test asserts
this on a fixture of representative rows (weapon, protective gear, document)
rather than only on one row.

### 5.2 Schema specification — JSON side

Authored form:

```json
{
  "id": "item_bolt_action_rifle",
  "equipSlot": "Weapon",
  "limbRequirements": { "hands": 2, "gripClass": "full" }
}
```

```json
{
  "id": "item_hook_prosthetic",
  "equipSlot": "ProstheticHand",
  "providesLimb": { "hands": 1, "legs": 0, "qualityPermille": 500 }
}
```

Catalog conventions: the repo's canonical serialization is snake_case per rule;
`items.json` currently mixes historical key styles in older rows, and the
loader already tolerates the established aliases. The **new** fields are
authored snake_case (`limb_requirements`, `grip_class`, `provides_limb`,
`quality_permille`) and the loader also accepts the PascalCase form for
robustness, exactly as the existing parser does for other fields. The validator
requires the canonical form in new rows it inspects; legacy rows are exempt.

New equip slot enum values: `ProstheticHand` and `ProstheticLeg`. These are
**not** new body slots on the survivor; they are item slots in the inventory
model. Mapping:

| Item slot | Limb served | Notes |
|---|---|---|
| `ProstheticHand` | `left_arm` or `right_arm` | selected at fit time, stored in `body_state.limbs[*].prosthetic_item_id` |
| `ProstheticLeg` | `left_leg` or `right_leg` | same |

The inventory holds the item as an equipped item in its own slot; the limb
record references its id. One item instance cannot serve two limbs (validator
enforces one limb per prosthetic instance at fit time).

### 5.3 Effective-hands algorithm (normative)

The gate is a **pure function** over (item, survivor limb state, equipped
prosthetic condition). It must be testable headless with no Godot dependency.

```text
function EffectiveHands(survivor, limbState):
    total = 0
    simpleOnly = true
    for limb in {left_arm, right_arm}:
        state = limbState[limb]
        if state.condition == Intact:
            total += 1
        elif state.condition == Prosthetized:
            item = catalog[state.prosthetic_item_id]
            if item == null or item.providesLimb == null:
                continue                     # missing catalog item: limb contributes 0
            if ConditionOf(item) <= 0:
                continue                     # failed prosthetic behaves as amputated
            total += item.providesLimb.hands
            if item.providesLimb.grip_class == "full": simpleOnly = false
    return (total, simpleOnly)

function CanEquip(item, survivor, limbState):
    req = item.limbRequirements
    if req == null: return true              # legacy path
    (hands, simpleOnly) = EffectiveHands(survivor, limbState)
    if req.hands > hands: return false
    if req.grip_class == "full" and simpleOnly: return false
    return true
```

Worked cases (these become the `EquipLimbGateTests` table):

| Limb state | Item | Result | Reason |
|---|---|---|---|
| both arms intact | rifle, hands 2, full | allow | 2 hands, full available |
| right arm amputated | rifle | refuse | 1 hand |
| right arm hook prosthetic, left amputated | rifle | refuse | 1 hand and simple-only |
| right arm hook prosthetic, left amputated | knife (simple, hands 1) | allow | 1 simple hand |
| right arm `item_pinned_metal_hand`, left amputated | rifle | refuse | grip full but only 1 hand |
| both arms `item_articulated_hand` | rifle | allow | 2 hands, full |
| right leg amputated | boots (legs 1) | refuse on right boot only | per-limb requirement if authored |
| right leg peg | right boot | allow | provision |
| limb state unknown/missing | any | allow | migration defaults intact; fail-open only for old saves |

**Leg equipment:** legs are typically slot `Feet` with no requirement (both feet
share one slot). The launch set does not require per-leg boot gating; the
schema permits it but no data is authored, keeping the blast radius small. If a
future expansion 27 wants per-leg garments, it must propose a schema version
bump rather than overload this one.

**Strength requirement:** the XP-06 proposal included
`strength_requirement`. This plan defers it. Rationale: no needs/strength model
exists as an equip input today, and adding an unused field is dead schema.
If the foreman wants it, it must sign a second line and name the strength owner;
otherwise `limb_requirements.hands` + `grip_class` is the whole schema.

### 5.4 Auto-unequip edge semantics

When a limb transition makes an equipped item illegal (amputation event), the
system must:

1. Detect at the mutation site: after `AmputationSystem` reports a completed
   amputation, run the gate over the survivor's equipped items.
2. Unequip violating items **once** per event, in deterministic order
   (slot enum order, then item id ordinal). Move them to the survivor's
   inventory if capacity allows; if not, drop to shelter stock with a
   deterministic overflow rule (the existing inventory overflow behavior).
3. Emit exactly one journal line per unequipped item, phrased as a fact
   ("The <item> no longer fits; stored in shelter stock.") — no morale effect,
   no hidden penalty.
4. Never emit the event on load/restore of a save that already reflects the
   post-amputation state. Exactly-once is proven by a save/load test that
   captures after the amputation and restores, asserting no second journal line
   and no second inventory mutation.

**Out of scope:** auto-equipping a replacement prosthetic. The player decides.

### 5.5 Prosthetics content model

Each prosthetic is an ordinary item row plus a crafting chain that resolves in
the live crafting catalogs. The launch set's chain requirements must use
existing material ids; the Phase 3 premise step greps each id before authoring.
Proposed chain materials (to be replaced by live ids found in the catalog):

| Symbolic requirement | Intended live source |
|---|---|
| cordage | existing cordage/cord item |
| bone carving | existing bone/carving material |
| tanning | existing leather/tanning chain |
| foundry + ceramics joint | existing foundry outputs + ceramics |
| crucible foundry + wax bushings + glass bearings | existing metallurgy purity outputs + apiculture wax + glassworks output |
| carpentry | existing wood/carpentry outputs |

Every new item must satisfy the `ContentUtilizationScanner`: the item must be
producible by a recipe and consumable by the fit command, or the scanner will
flag it. Phase 3 acceptance includes `--content-utilization-selftest` PASS with
zero new orphans.

### 5.6 Validator rules

New hook `ValidateLimbSchema` in `CatalogIntegrityValidator.cs` (name proposed;
the validator's existing convention uses descriptive method names). Rules:

| Rule id | Rule | Severity |
|---|---|---|
| LINB-1 | `limb_requirements.hands` ∈ {1,2} | error |
| LINB-2 | `limb_requirements.grip_class` ∈ {`simple`,`full`} | error |
| LINB-3 | `provides_limb.hands` ∈ {0,1,2}, `legs` ∈ {0,1}, not both 0 | error |
| LINB-4 | `provides_limb.quality_permille` ∈ [100,1000] | error |
| LINB-5 | `provides_limb` only on `ProstheticHand`/`ProstheticLeg` slots | error |
| LINB-6 | `limb_requirements` only on equipable items | error |
| LINB-7 | a prosthetic's `provides_limb` shape must match its slot (hand slot provides hands, leg slot provides legs) | error |
| LINB-8 | two equipable items cannot declare the same id (existing rule; re-asserted) | error |

Positive fixture: the launch set. Negative fixture: one row per rule with a
clear failure message naming catalog/item/field/value. The validator must not
warn on absent fields (legacy rows untouched).

### 5.7 Persistence, migration, determinism

**Shape:** `body_state` sub-object inside the existing `survivors` section per
survivor (see F14-C). The survivor save owner is the single writer.

**Migration:** when `body_state` is absent:

```text
body_state = {
  limbs: { left_arm: Intact, right_arm: Intact, left_leg: Intact, right_leg: Intact },
  rehab: null,
  schema_version: 1
}
```

If `AmputationSystem` has already persisted an amputation in the old format
(its own projection), the migration must prefer the existing medical state over
"all intact": the migration reads the already-restored `AmputationSystem` state
and projects it. **This ordering matters** and is the single largest migration
risk; Phase 1 includes a test with a hand-authored old save that has an
amputation and no `body_state`, asserting the limb remains amputated.

**Determinism:**

- No RNG in the schema, gate, migration, unequip, or reclaim paths.
- Rehab ramps use integer permille arithmetic; day boundaries are the existing
  campaign day ticks.
- `SaveChecksum` formatting is culture-invariant (existing rule). New numeric
  fields are integers; no float formatting appears in the checksum path.
- Paired replay: continuous 30 days == day-15 save → restore → 15 days,
  field-by-field including limb state, rehab phase, prosthetic condition.

**Version policy:** the `survivors` section's envelope version is unchanged
(additive nullable sub-object). If the implementation finds it cannot restore
without a version bump, it must stop and return for a signature amendment —
silent version bumps are forbidden by the save-owner rule.

### 5.8 Medical pipeline integration and the DEC-16 minimal path

Fitting a prosthetic is a medical event. Two designs:

| Option | Description | Verdict |
|---|---|---|
| A. Reuse an existing treatment kind | fitting records through the live pipeline kind vocabulary; rehab phase reads it | preferred if an existing kind fits without distorting it |
| B. Smallest additive kind `prosthetic_fitting` | one new kind in the authored vocabulary | acceptable if A's vocabulary is closed and validated |
| C. Skip the pipeline | fit command writes only body_state | rejected: creates a second medical record path |

The builder must inspect the pipeline kind vocabulary at execution time and
choose A or B; this choice is recorded in the package ledger as a premise note.

**DEC-16 minimal path (optional, same tranche):** the rehab phase record
contains `prosthetic_type_key` and phase/days. If the ward's admission records
can read a bounded, typed "cause" from the same record shape without inventing
fields, DEC-16's condition can be retired in the same register pass. If not,
DEC-16 stays deferred and Plan 4 handles it. **No ward admission schema is
invented by this plan.**

### 5.9 Expansion 16 boundary (what this plan does and does not take)

| Expansion 16 area | In this plan? | Where it belongs |
|---|---|---|
| simple prosthetics (hook, splint, pinned) | yes — F14-D | Phase 3 |
| advanced prosthetics (articulated, leg frame) | yes — F14-D | Phase 3 |
| rehabilitation content | yes — F14-E | Phase 4/5 |
| complications (infection at fit, overuse) | partially — overuse routes to condition wear; infection routes to existing medical pipeline | Phase 5 as a design note only |
| phantom pain | yes — F14-F | Phase 5 |
| automation | no | `RoboticsSystem` scope; separate authorization |
| drones | no | `RoboticsSystem` scope; separate authorization |
| rogue machine arcs | no | narrative scope; separate authorization |

This boundary must be copied into Expansion 16's plan as an enrichment note
when the wave is intaken (Plan 5), so a builder cannot read "Rebuilt Body" as
one authorization for all of it.

### 5.10 Presentation contract

`SurvivorDetailPanel` limb block:

- Header: `LIMBS — <survivor name>`.
- One row per limb in fixed order (left arm, right arm, left leg, right leg).
- Text states: `intact`, `amputated`, `prosthetic: <display name> (<condition>%
  serviceable | FAILED — REPLACE)`, `adapting (<n> days to full function)`.
- Quality is shown as a band word plus percent, never a bare bar.
- Keyboard focus order follows the panel's existing conventions; no new modal.
- No color-only encoding; a11y selftest must pass (5/5 floor).
- Empty state: if the survivor has no limb loss, the block shows `intact` rows
  only — it does not disappear (disappearing state teaches nothing).

### 5.11 Consumer matrix (who reads the new state)

| Consumer | Reads | Writes | Contract |
|---|---|---|---|
| `Inventory.Equip` | limb gate | equipped slot | refusal returns false today; keep false; no exception |
| `AmputationSystem` | own state | limb condition | remains sole writer of condition |
| expedition dispatch | `GetMovementSpeedMultiplier` | — | sealed; prosthetics approach intact, never exceed |
| `EquipmentConditionSystem` | prosthetic item condition | wear/repair | sealed single mutation API |
| crafting system | recipe requirements | new items | chains must resolve |
| `SurvivorDetailPanel` | limb + rehab state | — | read-only presentation |
| save owner | body_state | body_state | single writer |
| EN-04 (future) | rehab phase | — | read-only; must not duplicate |
| Expansion 16 (future) | all of the above | — | read-only until separately authorized |

### 5.12 Performance and allocation notes

- The gate runs at equip time and on limb-change events; both are rare. No
  per-tick scan of the whole roster is required.
- The auto-unequip scan is bounded by the survivor's equipped-slot count (a
  small fixed set).
- `EffectiveHands` allocates nothing when implemented with a small struct/loop;
  avoid LINQ in the mutation path to match the repo's existing style in hot
  paths.
- No new day-owner is created; the rehab tick piggybacks on the existing
  `ShelterFacilitiesDayOwner` sequence, so tick ordering and determinism gates
  are unaffected except for one new line in the owner's ordered list (documented
  in the owner's order comment).

### 5.13 Failure modes and diagnostics

| Failure | Behavior | Diagnostic |
|---|---|---|
| catalog item referenced by `prosthetic_item_id` is missing | gate contributes 0 for that limb; no crash | one warning at load, naming survivor + item id |
| unknown grip class in data | validator error at data-integrity gate; runtime treats as `full` (fail-closed for the player's benefit? no — fail-closed would block items; use fail-open to `simple`? The safe choice is: validator must prevent this; runtime treats unknown as the item's authored intent is unknowable, so treat as `full` to avoid an exploit, and log once) | validator + one runtime log |
| `body_state` present but malformed | restore refuses the sub-object, falls back to intact-with-medical-projection, logs once | save-load selftest |
| prosthetic condition at 0 | behaves amputated; limb label shows FAILED — REPLACE | panel + gate |
| double fit on same limb | refused; existing prosthetic must be removed first | fit command returns a typed failure |

The unknown-grip-class runtime choice is deliberately **fail-closed toward the
stricter interpretation** (`full` requires more), because the validator is the
primary defense and a data error should not create an exploit path.

### 5.14 Anti-exploit analysis

| Exploit | Guard | Test |
|---|---|---|
| hook + hook = two hands for a rifle | grip class `full` requires a `full` provision; hooks are simple | `EquipLimbGateTests` case 3 |
| hot-swap prosthetics to dodge decay | decay persists per item instance; un-equipping does not reset condition; refit during adaptation restarts adaptation (not the whole arc) | condition + rehab tests |
| amputate to reduce food needs | calorie need unchanged by limb state (needs model untouched); amputation adds trauma/rehab cost | needs regression + one policy test |
| farm mastery on cheap prosthetics | mastery is per prosthetic *type*; switching types begins adaptation for the new type; no stacking | skill test |
| use prosthetic as a free weapon | prosthetics are not weapons in this plan; no damage fields are added | validator + catalog scan |
| restore-scum a failed purity-fit | fitting is deterministic; no roll | determinism test |

### 5.15 Compatibility with sealed condition/repair seams

- Plan 21 sealed `RecordWear` as the single mutation API and the estimate/wear
  chain for protective gear. Prosthetics reuse the same wear path but must not
  enter protective-gear radiation math; the validator and scanner must confirm
  prosthetics are not classified as rad-protective. If the existing tag
  vocabulary auto-classifies them, add an explicit non-protective tag in the
  authored rows.
- Plan 22 sealed the `repairRecipe`/`TryConsumeBill` restore path for gear. If
  prosthetics are repairable, they use the same seam with their own authored
  cap; if not repairable, the panel must say "replace-only" truthfully.

### 5.16 Test fixture design

A shared fixture (in the existing fixture area, not a new framework):

```csharp
// symbolic; actual placement follows the repo's fixture policy
static Survivor WithLimbs(params (LimbId limb, LimbCondition cond, string? prosthetic)[ ] rows)
```

The fixture:

- constructs limb state without touching save files;
- is used by gate, rehab, and panel-contract tests;
- is registered wherever `DataAuthorityFidelityTests` requires campaign
  fixtures to use authority data (per `docs/testing/FIXTURE_POLICY.md`).

### 5.17 Open questions requiring source verification at execution time

These are deliberately open; the plan does not guess:

1. **Where does the `survivors` section's per-survivor payload actually live**
   (`Main.Survivors.cs` save method vs. a roster aggregate)? The builder must
   locate the single writer and add `body_state` there. Appendix A includes a
   grep recipe.
2. **What is the live crafted-item chain id vocabulary** for the six material
   classes in §5.5? Grep each catalog before authoring.
3. **Does the medical pipeline kind vocabulary accept a fitting kind without a
   version bump** (§5.8 option A)? Inspect at execution time.
4. **Is `ProstheticHand`/`ProstheticLeg` collision-free** against existing
   `EquipSlot` enum member names and JSON aliases? Grep before adding.
5. **Does the a11y selftest enumerate panel row text automatically**, or does a
   new row type need registration? Inspect `UiAccessibilitySelfTest`.
6. **Does `ContentUtilizationScanner` treat new items as consumed if the only
   consumer is a host command** (the fit command)? If not, the scan rule needs a
   documented consumer registration (the CF-P6 precedent shows the expected
   pattern).

Each answer is recorded as a premise note in the package's implementation log;
none blocks the signature.

### 5.18 Enrichment appendix — exact plan text to add to XP-06 and Expansion 16

When the signatures land, the forced enrichment edits are:

1. `KNOWN_DEBT.md` `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION`: replace the
   "Equipment half BLOCKED" clause with "Equipment half SEALED <date> by
   UNBLOCK-01 F14-A..G; tests <paths>; do not re-open without a new limb slot
   vocabulary".
2. `DECISION_REGISTER.md`: `DEC-03` → RETIRED with evidence; add `DEC-21`
   (schema), `DEC-22` (grip vocabulary), `DEC-23` (prosthetics/condition),
   `DEC-24` (rehab/phantom/panel) — or one combined row if the foreman prefers
   fewer ids; the register's own format allows a bundle row as long as each
   execution package is named.
3. `INTEGRATION_PLANS.md`: add the execution package row with exact paths and
   focused verify, after the current batch's rule allows a new package.
4. Expansion 16 plan: insert a "Prerequisite — UNBLOCK-01" block at the head of
   its prosthetic phase and a "Not authorized here — Robotics" block at its
   automation phase.
5. `EN-04` program text: replace "hard-blocked on F14" with "F14 signed <date>;
   authorization line pending per Plan 5".
6. `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md`: do not rewrite history;
   add a dated superseding note pointing at the executed seal (the audit's own
   convention is to leave the historical text and add the superseding line).

### 5.19 Character of the content (tone guard for authored names/prose)

- Prosthetic names are workshop-plain (`pinned metal hand`, `sprung leg
  frame`), never heroic or brand-like.
- No real-world medical brands, implant product names, or manufacturer marks.
- Journal lines state facts; no pity, no triumph.
- Panel labels say what a thing is, not how the player should feel.
- The dead keep their names; prosthetics are tools, not identity claims.---

## 6. Phased execution program

Each phase is independently verifiable and reversible. A builder may stop after
any phase and hand off. Phases 1–2 are the "schema core" and are the only phases
safe to run immediately after F14-A/B/C; Phases 3+ wait for their own signatures.

### Phase 0 — Premise re-verification (no edits; 0.5 day)

**Goal:** confirm §2 facts still hold at the then-current HEAD and record drift.

Steps:

1. Run the Appendix A commands; compare outputs to §2.
2. Locate the survivor save writer (`SaveSurvivors` implementation) and read
   the current per-survivor payload shape.
3. Read `AmputationSystem`'s current capture/restore projection to confirm the
   medical state source used by the migration.
4. Grep for `ProstheticHand`/`ProstheticLeg` collisions and for existing
   crafted-material ids in §5.5.
5. Record a premise-note block in the package's implementation log:
   `UNBLOCK-01 premise notes — date, HEAD, drift, decisions`.

**Exit:** premise notes recorded; any drift that invalidates a signed line is
reported to the foreman instead of improvised around (Rule 10).

### Phase 1 — Schema + validator + migration (additive; 1–1.5 days)

**SIGNATURE REQUIRED:** F14-A, F14-B, F14-C.

Files (expected; confirm in Phase 0):

- `Assets/Ashfall.Core/Inventory/ItemDefinitions.cs`
- `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs`
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`
- the survivor save owner file(s) in `src/Main.*`
- `Ashfall.Core.Tests/Inventory/LimbRequirementSchemaTests.cs` (new)
- `Ashfall.Core.Tests/Inventory/` adjacent fixtures

Work:

1. Add the two nullable sub-objects + enum values + clone/parse support.
2. Bind snake_case JSON on the loader; tolerate PascalCase; leave legacy rows
   untouched.
3. Add `ValidateLimbSchema` (LINB-1..8) with positive/negative fixtures.
4. Add `body_state` additive sub-object + migration (all-intact, but
   medical-state-preferring per §5.7).
5. Register the new save field wherever the Triad drift gate enumerates fields.
6. No data authoring in this phase — zero rows change.

Exits:

- `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/LimbRequirementSchemaTests.cs`
- adjacent `Ashfall.Core.Tests/Inventory/` suite green
- `godot --headless --path . -- --data-integrity-selftest` PASS (0 errors; no new warnings)
- `dotnet build Ashfall.csproj --no-restore` 0/0
- save round-trip: old-save-with-amputation fixture retains the amputation
- Triad drift gate green

### Phase 2 — Equipment gate + auto-unequip (mechanical; 1.5–2 days)

**SIGNATURE REQUIRED:** F14-A/B (C for the reload equality test).

Files:

- `Assets/Ashfall.Core/Inventory/Inventory.cs` (Equip preflight; no signature
  change beyond the gate)
- `Assets/Ashfall.Core/Medical/AmputationSystem.cs` (event surface only)
- the host wiring that observes the amputation completion
- `Ashfall.Core.Tests/Inventory/EquipLimbGateTests.cs` (new)
- `Ashfall.Core.Tests/Expeditions/C2AmputationTravelTests.cs` (one parity case,
  only if a prosthetized-leg case is added)

Work:

1. Implement `EffectiveHands`/`CanEquip` as a pure Core helper.
2. Wire the preflight; refusal returns `false` (existing contract), never throws.
3. Wire the limb-change scan: exactly-once unequip, deterministic order, journal
   line, no morale effect (§5.4).
4. Author `limb_requirements` on a bounded launch set of two-handed weapons
   (the smallest set that makes the gate meaningful; the list is recorded in the
   implementation log). **Every authored row is a data edit; keep it additive.**
5. Do not author prosthetics yet; gate tests use fixture items.

Exits:

- `EquipLimbGateTests` table green (all §5.3 cases)
- `Plan21ProtectiveWearTests` unchanged green (additive proof)
- `C2AmputationTravelTests` 7/7 unchanged
- determinism: same-run repeated gate evaluations identical
- host build 0/0

### Phase 3 — Prosthetics catalog + crafting chains (content; 2–3 days)

**SIGNATURE REQUIRED:** F14-D.

Files:

- `Assets/StreamingAssets/Data/items.json` (6 new rows; additive)
- crafting catalogs that resolve the chains (confirm in Phase 0)
- `Ashfall.Core.Tests/Inventory/ProstheticsCatalogTests.cs` (new)
- generated `docs/data/CATALOG_REGISTRY.md` via its generator

Work:

1. Author the six rows with canonical ids from §4.4; include the
   non-protective tag clarification (§5.15) if the tag vocabulary would
   otherwise classify them as gear.
2. Author the chains using live material ids; each chain resolve-tested.
3. Fit command: consumes the item from inventory, sets
   `limbs[*].prosthetic_item_id`, emits the medical event (§5.8), starts
   `fitting`.
4. Refit rules: removing a prosthetic returns it to inventory with its
   condition intact; double-fit refused (§5.13).
5. Condition decay via `RecordWear`; failed prosthetic behaves amputated.
6. `--content-utilization-selftest` zero new orphans.

Exits:

- `ProstheticsCatalogTests` green
- `--data-integrity-selftest` PASS with added positive/negative fixtures
- `--content-utilization-selftest` PASS
- one round-trip test: fit → save → restore → still fitted, condition preserved

### Phase 4 — Rehabilitation arc (state machine; 2 days)

**SIGNATURE REQUIRED:** F14-E.

Files:

- `Assets/Ashfall.Core/Medical/AmputationSystem.cs` (phase record; fit start)
- the day-owner line that already ticks shared skill progression
- `Ashfall.Core.Tests/Medical/RehabilitationArcTests.cs` (new)
- EN-04 read-model note (no EN-04 code in this phase)

Work:

1. Implement the three-phase state machine with integer permille ramp.
2. Attach the daily tick beside the existing skill tick; document the ordered
   line in the owner's order comment.
3. Deterministic durations from authored ranges (no wall-clock, no RNG).
4. Mastery milestone through the existing Plan 178 chronicle hook.
5. Save/load: phase and days survive; refit restarts adaptation only.

Exits:

- `RehabilitationArcTests` green (phase lengths, ramp monotonicity, mastery
  once, refit behavior)
- paired replay: continuous == mid-reload for 30 days (rehab fields included)
- no new day owner; owner-order doc updated

### Phase 5 — Phantom pain + medical-fit design resolution (presentation-adjacent; 1 day)

**SIGNATURE REQUIRED:** F14-F.

Files:

- `Assets/Ashfall.Core/…/SleepNarrativeProjection.cs`
- `Ashfall.Core.Tests/…/Plan177SleepNarrativeProjectionTests.cs` (one added case)
- optional: pipeline fitting-kind resolution per §5.8

Exits:

- projection test green; no mutation; journal dedup unchanged
- no phobia/dream API added (grep-verified)

### Phase 6 — Survivor panel presentation (UI; 1–1.5 days)

**SIGNATURE REQUIRED:** F14-G.

Files:

- `src/UI/SurvivorDetailPanel.cs`
- a11y selftest expectations if it enumerates rows
- `Ashfall.Core.Tests/UI/` adjacent contract test if one exists for the panel

Exits:

- `--panel-bind-lifecycle-selftest` PASS
- `--ui-accessibility-selftest` (or repo-equivalent) PASS at the existing floor
- panel route gate unaffected (no new panel)

### Phase 7 — Acceptance, ledger, and closeout (0.5–1 day)

Steps:

1. Focused battery rerun (§7).
2. Write the implementation log with premise notes, drift, evidence commands.
3. Update `KNOWN_DEBT.md`, `DECISION_REGISTER.md`, `INTEGRATION_PLANS.md`,
   WIN? only as the foreman/integrator permits (the builder proposes the exact
   rows; the integrator lands them).
4. Enrichment edits per §5.18.
5. Handoff per `AI_AGENT_WORKFLOW.md`.

**Total estimated effort:** 8–11 focused builder-days across seven phases; each
phase is a separate claim window.

---

## 7. Verification plan

### 7.1 Focused test matrix

| Phase | Target | Expected | Notes |
|---|---|---|---|
| 1 | `Ashfall.Core.Tests/Inventory/LimbRequirementSchemaTests.cs` | new, 15–25 cases | schema, parse, clone, validator negatives |
| 1 | `Ashfall.Core.Tests/Inventory/` (directory) | green | additive proof |
| 1 | old-save fixture round-trip | amputation retained | the highest-risk test |
| 2 | `Ashfall.Core.Tests/Inventory/EquipLimbGateTests.cs` | new, 10–16 cases | the §5.3 table |
| 2 | `Ashfall.Core.Tests/Inventory/Plan21ProtectiveWearTests.cs` | 16/16 unchanged | additive proof |
| 2 | `Ashfall.Core.Tests/Expeditions/C2AmputationTravelTests.cs` | 7/7 unchanged | sealed consumer |
| 3 | `Ashfall.Core.Tests/Inventory/ProstheticsCatalogTests.cs` | new, 8–12 cases | rows, chains, decay, refit |
| 4 | `Ashfall.Core.Tests/Medical/RehabilitationArcTests.cs` | new, 8–12 cases | phases, ramp, mastery |
| 4 | paired replay target | equality | continuous == interrupted |
| 5 | `Plan177SleepNarrativeProjectionTests.cs` | +1 case | variant only |
| 6 | `--panel-bind-lifecycle-selftest` | PASS | lifecycle |
| all | `--data-integrity-selftest` | 0 errors | validator |
| all | `--content-utilization-selftest` | 0 new orphans | content gate |
| all | `dotnet build Ashfall.csproj --no-restore` | 0/0 | build |
| closeout | `MainTriadDriftGateTests` | 7/7 | save registration |

### 7.2 Commands (exact, per TEST_POLICY)

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/LimbRequirementSchemaTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/EquipLimbGateTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/ProstheticsCatalogTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Medical/RehabilitationArcTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/C2AmputationTravelTests.cs
dotnet build Ashfall.csproj --no-restore
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --panel-bind-lifecycle-selftest
```

No full-suite run is part of this plan. If the integrator wants a wider window,
it requires an explicit foreman reason per TEST_POLICY.

### 7.3 Failure-proof obligations

At least one test per phase must **fail before the fix** (or have a recorded
pre-fix failure) so a green run means something:

- Phase 1: a hand-authored malformed `body_state` fixture must fail the
  migration test before the guard exists (or the guard test itself is proven by
  a fixture that violates LINB rules).
- Phase 2: gate tests fail against the pre-change `Equip` (trivially, since the
  field does not exist — record the compile-time/behavioral proof in the log).
- Phase 3: chain tests fail before rows are authored.
- Phase 4: ramp test fails before the tick exists.

### 7.4 What is NOT accepted as evidence

- A compile-green result without the focused tests.
- A data-integrity PASS without the new negative fixtures.
- A panel screenshot without the a11y selftest.
- "The plan said it" — every sealing claim needs source/test evidence at the
  current HEAD.

---

## 8. Risks and mitigations

| # | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| 1 | Migration misreads an old amputation as intact | medium | high (save corruption) | the old-save-with-amputation fixture is mandatory in Phase 1; medical state wins over "all intact" |
| 2 | Section-count or Triad gate fails because body_state was placed as a sibling | low | medium | F14-C forbids a sibling; sub-object only |
| 3 | `items.json` edit collides with D3/water-sample or another catalog claim | medium | medium | execution ordering: Plan 4/D3 first or same session; check `WORKTREE_OWNERSHIP.md` immediately before editing |
| 4 | Prosthetic rows flagged as content orphans | medium | low | Phase 3 includes scanner pass; each item has a recipe and the fit consumer |
| 5 | Grip semantics misread by builders (simple vs full) | low | medium | §5.2/§5.3 are normative; tests mirror the table; vocabulary may not grow without a signature |
| 6 | Fitting bypasses the medical pipeline and creates a second record | low | high (architecture) | §5.8 mandates A or B; C is rejected |
| 7 | Prosthetics enter radiation-protection math | low | medium | §5.15 tag clarification + a validator/scanner check |
| 8 | Rehab tick creates a new day owner or reorders ticks | low | high (determinism) | attach beside the existing tick; owner-order doc updated; paired replay gate |
| 9 | Expansion 16 is read as authorized in full | medium | high (scope) | §5.9 boundary + enrichment edit in Expansion 16 |
| 10 | Scope creep into strength, vision, hearing gating | medium | medium | §5.3 defers strength; §3.3 forbids new gating classes |
| 11 | DEC-16 is silently "resolved" by assertion | low | medium | §5.8 optional path requires the ward consumer to read a typed cause; otherwise deferred |
| 12 | Panel text drifts from the string-freeze decision (Plan 3) | low | low | §9.3 coordination: if freeze lands first, author panel strings with freeze keys |

---

## 9. Ownership, sequencing, and coordination

### 9.1 Proposed claim shapes (for the integrator to split)

| Phase | Proposed claim | Owner | Disjoint paths |
|---|---|---|---|
| 1 | `UNBLOCK-01-P1-LIMB-SCHEMA` | builder | ItemDefinitions/loader/validator/survivor save owner/tests |
| 2 | `UNBLOCK-01-P2-EQUIP-GATE` | builder | Inventory/AmputationSystem wiring/tests |
| 3 | `UNBLOCK-01-P3-PROSTHETICS` | builder | items.json/crafting catalogs/tests |
| 4 | `UNBLOCK-01-P4-REHAB` | builder | AmputationSystem/day-owner line/tests |
| 5 | `UNBLOCK-01-P5-PHANTOM` | builder | sleep projection/tests |
| 6 | `UNBLOCK-01-P6-PANEL` | builder | SurvivorDetailPanel/tests |
| 7 | `UNBLOCK-01-P7-CLOSEOUT` | integrator | ledgers/register/enrichment |

Phases 1–2 must be sequential (same files). Phase 3 may start once 2 is green.
Phase 5 is independent of 3/4 and may run in parallel with 4 if the same builder
is not used for both. Phase 6 waits for 3/4 (it displays their state).

### 9.2 Shared paths and race rules

- `Items.json` and any crafting catalog: **check ownership immediately before
  edit**; if Plan 2 or Plan 4 holds a catalog claim, sequence behind it.
- `AmputationSystem.cs`: Phases 2, 3, 4 touch it; keep them sequential in one
  claim or note the seam in `WORKTREE_OWNERSHIP.md`.
- `survivors` save section: single writer; if another live claim holds it,
  hand off rather than race.

### 9.3 Cross-plan coordination

| Sibling | Interface | Rule |
|---|---|---|
| Plan 2 (funds/trade) | none directly; both may want `items.json` in different regions | coordinate the catalog edit session |
| Plan 3 (string freeze) | `SurvivorDetailPanel` strings | if freeze lands first, use freeze keys; else note the debt |
| Plan 4 (register truth) | D3 ordering; DEC-03/DEC-16 rows | Plan 4 signs D3 before Phase 3; Plan 4 records DEC closures after Phase 7 |
| Plan 5 (expansions) | Expansion 16 intake | Plan 5 must reference this plan's phases as Expansion 16's prerequisite and copy the §5.9 boundary |

---

## 10. Rollback and decline paths

- **Before Phase 1:** deleting this document changes nothing.
- **After Phase 1 (schema only):** revert the additive fields; no data changed;
  no save can contain them yet. Fully reversible.
- **After Phase 2 (gate + some authored requirements):** revert the authored
  `limb_requirements` rows to remove gating; the schema and migration may stay
  (harmless) or be reverted together. Prosthetics not yet authored.
- **After Phase 3+:** prosthetics exist in saves; a rollback must leave the
  `body_state` sub-object restorable-but-ignored (unknown fields are already
  tolerated by the save codec pattern) rather than deleting it. Do not force a
  save wipe.
- **Decline of individual lines:** F14-D/E/F/G each decline independently; F14-A
  may ship alone as schema-only if the foreman wants the vocabulary reserved.
- **Decline of the whole bundle:** the debt row stays BLOCKED; XP-06 and EN-04
  stay decision-blocked; Expansion 16's prosthetic half stays unauthorized.
  Plan 5 records the decline so the wave intake does not silently assume it.

---

## 11. Definition of done and handoff

### 11.1 Definition of done (per line)

| Line | Done when |
|---|---|
| F14-A | schema fields deserialize/round-trip additively; validator hook green; old saves byte-equivalent where unaffected |
| F14-B | enum + parser + tests green; no third class possible without a version note |
| F14-C | `body_state` persists; old-save migration retains medical truth; Triad gate green |
| F14-D | six rows authored; chains resolve; decay via `RecordWear`; scanner zero orphans |
| F14-E | phase machine green; tick attached beside skill tick; paired replay equality |
| F14-F | one new projection case green; no phobia/dream API |
| F14-G | panel rows keyboard-focusable, text-only encoding, a11y PASS |

### 11.2 Handoff format (required fields)

1. Outcome: which lines landed, which deferred.
2. Files: exact paths changed, with a one-line purpose each.
3. Contract: the schema and gate semantics as implemented (any deviation from
   §5 must be named).
4. Evidence: commands + results + selected tests + known limitations, per
   `TEST_POLICY.md` reporting.
5. Shared paths intentionally untouched: `Items.json` regions before D3,
   `Main.*` shared roots untouched, `RoboticsSystem`, avatar paths.
6. Proposed ledger edits: exact `KNOWN_DEBT.md`/`DECISION_REGISTER.md` rows for
   the integrator to land.
7. Next safe step: which phase/unblock plan follows.

### 11.3 First safe step (restated for the record)

> Run Phase 0's premise commands and record drift. Do not edit production until
> the foreman signs F14-A/B/C.

---

## Appendix A — Premise re-verification commands

```bash
# 1. Schema absence/presence
grep -n "class ItemDefinition" -A 40 Assets/Ashfall.Core/Inventory/ItemDefinitions.cs
grep -n "limbRequirements\|LimbRequirement\|providesLimb\|LimbProvision" \
  Assets/Ashfall.Core/Inventory/ItemDefinitions.cs

# 2. Equip path
grep -n "public bool Equip" -A 12 Assets/Ashfall.Core/Inventory/Inventory.cs

# 3. Limb state seat
grep -n "prostheticId\|hasPhantomPain\|recoveryDaysLeft" \
  Assets/Ashfall.Core/Medical/AmputationSystem.cs

# 4. Save owner
grep -n "\"survivors\"" -B2 -A2 Assets/Ashfall.Core/Save/SaveSectionRegistry.cs
grep -rn "SaveSurvivors" src/ | head

# 5. Validator hook conventions (last added hook = XP-01 difficulty)
grep -n "ValidateDifficulty\|ValidateDistressFollowUp" \
  Assets/Ashfall.Core/CatalogIntegrityValidator.cs | head

# 6. Slot collisions
grep -rn "ProstheticHand\|ProstheticLeg" Assets/ src/ Ashfall.Core.Tests/ --include=*.cs

# 7. Existing crafted-material ids for the six chains
grep -n "\"id\"" Assets/StreamingAssets/Data/crafting_recipes.json | head -40
# (confirm the actual crafting catalog name in Phase 0)

# 8. Day-owner tick location for the skill tick
grep -rn "TickSharedSkillProgression\|ShelterFacilitiesDayOwner" src/ | head

# 9. Content scanner consumer registration precedents
grep -n "vehicle_armor_grades\|VehicleArmorGrade" \
  Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs | head

# 10. Test baselines (do not edit; record counts)
bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/Plan21ProtectiveWearTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/C2AmputationTravelTests.cs
```

Expected outputs are the §2 facts. Any mismatch is drift and must be reported,
not worked around.

---

## Appendix B — Exact file inventory this plan may touch (proposal for the claim)

**Core:**
`Assets/Ashfall.Core/Inventory/ItemDefinitions.cs`,
`Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs`,
`Assets/Ashfall.Core/Inventory/Inventory.cs`,
`Assets/Ashfall.Core/Medical/AmputationSystem.cs`,
`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`,
`Assets/Ashfall.Core/…/SleepNarrativeProjection.cs`

**Host:**
the survivor save owner file(s) under `src/Main.*`,
the amputation event wiring file,
the day-owner file that ticks skill progression.

**Data:**
`Assets/StreamingAssets/Data/items.json` (additive rows/fields only),
crafting catalog(s) that resolve the six chains,
(none else without a new signature).

**Tests:**
`Ashfall.Core.Tests/Inventory/LimbRequirementSchemaTests.cs` (new),
`Ashfall.Core.Tests/Inventory/EquipLimbGateTests.cs` (new),
`Ashfall.Core.Tests/Inventory/ProstheticsCatalogTests.cs` (new),
`Ashfall.Core.Tests/Medical/RehabilitationArcTests.cs` (new),
one added case in the sleep-projection test,
one added case in `C2AmputationTravelTests.cs` if the leg parity case is added.

**UI:**
`src/UI/SurvivorDetailPanel.cs`.

**Generated (via generators only):**
`docs/data/CATALOG_REGISTRY.md`, architecture/selftest manifests if a selftest
row changes.

**Governance (integrator only):**
`KNOWN_DEBT.md`, `docs/governance/DECISION_REGISTER.md`,
`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`.

---

## Appendix C — Glossary

| Term | Meaning |
|---|---|
| F14 | The schema decision cluster named in the 2026-09-19 audit; successor to DEC-03's condition |
| grip class | `simple`/`full` vocabulary limiting prosthetic hand satisfaction |
| effective hands | The integer hand count available after limb state and prosthetic condition |
| fitting/adaptation/mastery | The three rehab phases |
| fail-closed | On unknown data, choose the stricter interpretation (here: unknown grip = full) |
| additive sub-object | A nullable field inside an existing save section that requires no version bump |
| sealed consumer | A downstream system whose behavior is pinned by tests and must not drift |

---

## Appendix D — Cross-plan interface table

| Interface | This plan provides | Consumed by |
|---|---|---|
| `LimbRequirement`/`LimbProvision` schema | F14-A | XP-06, Expansion 16, future expansions |
| effective-hands gate | F14-B/C | Inventory, EN-04 read model |
| `body_state` projection | F14-C | save/load, EN-04, panel |
| prosthetics launch set | F14-D | crafting, XP-06-F4, Expansion 16 |
| rehab phase state | F14-E | EN-04, medical pipeline, Expansion 16 |
| phantom-pain variant | F14-F | sleep projection, XP-10 boundary |
| limb panel rows | F14-G | a11y, player surface |

**End of UNBLOCK-01.** This document is a proposal to release blocked plans; it
does not execute, claim, or certify any of them. The next action belongs to the
foreman: sign F14-A/B/C, decline, or defer with a condition.---

## 12. Worked scenarios (end-to-end traces)

These traces exist so that a builder can write the integration tests directly
from the plan and so a reviewer can see the intended player-visible behavior
without reading code. Each trace names the systems touched in order.

### 12.1 Scenario A — A survivor loses an arm mid-campaign with a rifle equipped

1. Medical procedure completes. `AmputationSystem` marks `right_arm` amputated
   and persists the condition in its existing projection.
2. The amputation completion event fires to the host wiring (one subscription,
   existing pattern).
3. The host runs the gate scan: the survivor's equipped `item_bolt_action_rifle`
   has `limb_requirements { hands: 2, grip_class: full }`. `EffectiveHands`
   returns `(1, simpleOnly=true)`. `CanEquip` returns false.
4. The rifle is unequipped, moved to the survivor's inventory (capacity
   permitting) or shelter stock with the deterministic overflow rule.
5. Exactly one journal line is emitted: "The bolt-action rifle no longer fits;
   stored in shelter stock." No morale mark, no hidden penalty.
6. `body_state.limbs.right_arm.condition = amputated` is written by the save
   owner on the next save.
7. The player opens `SurvivorDetailPanel`: `LIMBS` block shows
   `right arm — amputated`, `left arm — intact`, both legs intact. The rifle is
   visibly in inventory, not equipped.
8. The player crafts `item_hook_prosthetic` (field tier). The fit command
   consumes it, sets `prosthetic_item_id`, starts `fitting` (3–5 days), and the
   panel shows `right arm — prosthetic: hook hand (adapting, N days)`.
9. During `fitting`, the hook satisfies `simple` grip only. The rifle is still
   refused. A knife (simple, hands 1) can be equipped.
10. After adaptation completes, quality reaches 1.0 × base 0.5 → effective 0.5
    capability class; the rifle is still refused (grip), but the knife is
    fully functional.
11. The player later crafts `item_articulated_hand` and keeps the left arm
    intact: with two providing limbs, the rifle becomes legal. The expedition
    movement multiplier remains governed solely by the sealed amputation
    consumer and is never exceeded by prosthetics.

### 12.2 Scenario B — Save/load during adaptation

1. Day 10: `fitting` completed, `adaptation` at day 4 of 14, quality permille
   ramping.
2. Save. The `survivors` section contains `body_state` with the phase record.
3. Reload. The save owner restores `body_state`; the medical projection restores
   the same condition; no event replays (no second unequip, no second fit).
4. Days 11–20 tick the ramp; the phase completes exactly on the same day as a
   continuous run would (paired-replay equality test).
5. A hand-authored legacy save (pre-schema, with an amputation, no body_state)
   restores to `amputated` for the correct limb — not to intact.

### 12.3 Scenario C — A two-handed weapon is authored, validated, and gate-tested

1. The builder authors `limb_requirements` on the launch set only.
2. `--data-integrity-selftest` runs the new LINB rules; a deliberately malformed
   fixture (`hands: 3`, `grip_class: "power"`, `quality_permille: 5000`) fails
   with item/field/value in the message.
3. `--content-utilization-selftest` confirms every newly required item is still
   reachable (no item becomes dead because a requirement was added).
4. `EquipLimbGateTests` covers the full §5.3 table; the gate is proven to be a
   pure function (same inputs, same output, no RNG, no static mutation).

### 12.4 Scenario D — Refit and failure recovery

1. A prosthetic's condition reaches 0 through `RecordWear`. The limb reads
   `prosthetic — FAILED — REPLACE` in the panel; the gate treats it as amputated.
2. The player removes it (returns to inventory with condition 0), repairs it
   through the existing `repairRecipe` seam if repairable, or replaces it.
3. On refit, `adaptation` restarts for the prosthesis type (not the whole arc
   from `fitting`); the ramp is deterministic.
4. No state can be duplicated: one prosthetic instance serves one limb; double
   fit is refused with a typed failure shown as text.

---

## 13. Foreman briefing — anticipated questions

**Q1. Why is this one plan and not five separate schema decisions?**
Because they share one file region (`ItemDefinitions` + `items.json` + the
survivors save payload). Splitting them across sessions would re-touch the same
files repeatedly and create the exact class of race the ownership ledger exists
to prevent. The sign-off lines remain separate so decline can be granular.

**Q2. What is the cheapest possible version of this?**
F14-A + F14-B only: the vocabulary exists, two-handed weapons get authored
requirements, and the equipment half of the debt closes. No prosthetics, no
rehab, no panel. Approximate cost: 2–3 builder-days.

**Q3. What is the most expensive version and why might it be worth it?**
The full bundle: prosthetics, rehab, phantom pain, panel, and the EN-04
read-model. Cost: 8–11 builder-days. It releases XP-06 in full, opens EN-04,
and gives Expansion 16 its groundwork. The expensive half is content
authoring, which is reversible.

**Q4. Does this contradict anything in the retired Unity era?**
No. It adds engine-free Core vocabulary, an additive save sub-object, and
reuses the sealed condition authority. It does not restore `RefreshSurvivorVisuals`
or any avatar path. `DEBT-UNITY-LEGACY` stays RETIRED.

**Q5. Does this collide with the difficulty work (XP-01)?**
No files are shared. XP-01 touched `Difficulty`, save header, and scalar
consumers. This plan touches inventory/medical/survivors. The one overlap is
the `survivors` save owner file: if the active XP claim still holds it, sequence
behind it.

**Q6. Why not a separate equipment_rules.json (Option C in F14-A)?**
Because it creates a second lookup authority for a single equip decision and
requires two files to agree at runtime. The repo's one-authority rule and its
save/validator patterns both favor fields on the definition. The size argument
(keep items.json lean) is real but weaker than the correctness argument.

**Q7. What happens to old saves with an arm already amputated?**
`AmputationSystem`'s existing projection is the truth; the migration reads it
and writes the equivalent `body_state`. The mandatory fixture test in Phase 1
proves the limb stays amputated. If the implementation cannot determine the
medical projection ordering, it must stop and return for a design amendment —
it may not guess.

**Q8. Why is DEC-16 only optional here?**
Because DEC-16 is about ward admission causes, a different record shape. This
plan creates a rehab phase record, which is only a candidate carrier. Forcing
DEC-16 closed would repeat the mistake the register already warns about:
signing a decision on top of an unverified consumer. Plan 4 handles register
truth; EN-04 or a small follow-up can close DEC-16 with its own consumer
evidence.

**Q9. How does this affect the golden save fixtures?**
Historical golden saves must continue to load. The additive sub-object means
their bytes are unchanged on read; the golden fixture assertion remains a
fingerprint of the historical payload, while a new fixture captures the new
shape. Do not rewrite the historical fixtures to include `body_state`.

**Q10. What if the data-integrity gate already has a warning pinned for
items.json?**
The five pinned warnings are catalog-level and unrelated; the new rules add no
warnings for legacy rows because they only inspect authored fields. If a
launch-set row triggers an existing pinned warning, the builder must capture
that fact in the implementation log rather than silently changing the pin.

**Q11. Can a survivor with two advanced prosthetics dual-wield two-handed
weapons?**
No. Grip `full` requires two providing limbs; the gate counts hands, so two
articulated hands provide two hands and legalize exactly one two-handed weapon
(which occupies both). Two two-handed weapons cannot both equip because the
second equip finds no free hand slots under the existing single-slot-per-slot
inventory model; the validator and gate tests assert this.

**Q12. Does the plan add anything to the combat model?**
No. Prosthetics affect equip legality and (through the existing movement
consumer) the already-sealed movement multiplier. No damage, accuracy, or armor
fields are added.

**Q13. What is the player-facing minimum needed for the release to feel real?**
Field-tier hand and leg items plus the panel block. Without the panel, the
player cannot see why an item refuses to equip. F14-G is therefore recommended
even in the cheap version if any gating data ships at all.

**Q14. Where does "quality" come from, and is it farmable?**
From the item's `provides_limb.quality_permille`, scaled by the rehab ramp.
It is not rolled. Swapping prosthetics does not accumulate quality; mastery is
per type and offers a handling bonus only, never stacking with item quality in
a multiplicative exploit.

**Q15. How will we know the block is truly gone?**
When the debt row is RETIRED with: (a) the equipment gate tests green, (b) the
old-save migration test green, (c) the validator negative fixtures green, and
(d) the register row replaced by a signed schema row. Until all four exist, the
block is only "in progress".

---

## 14. Enrichment texts (paste-ready after signature)

These are the exact minimal edits that keep the existing plan documents truthful
once this bundle executes. They are written as replacement/introduction blocks;
the integrator applies them, not a builder.

### 14.1 `KNOWN_DEBT.md` — `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` (post-execution)

Replace the "Equipment half BLOCKED" sentence with:

> **Equipment half SEALED <date> by UNBLOCK-01 F14-A..G:** additive
> `limb_requirements`/`provides_limb` schema, two-class grip vocabulary,
> `body_state` sub-object on the survivors section, equipment gate +
> auto-unequip, six-item prosthetics launch set through the sealed condition
> authority, fitting→adaptation→mastery rehab arc, and a sleep-beat phantom
> variant. Evidence: `LimbRequirementSchemaTests`, `EquipLimbGateTests`,
> `ProstheticsCatalogTests`, `RehabilitationArcTests`, old-save migration test.

### 14.2 `DECISION_REGISTER.md` (post-execution)

- `DEC-03` → `RETIRED` with evidence pointer to the signed schema rows.
- Add `DEC-21` — Body-integrity schema extension: `SIGNED`, condition: additive
  optional fields only; third grip class requires a new version; execution
  package UNBLOCK-01 Phase 1–2; recheck trigger: any new limb vocabulary
  proposal.
- Add `DEC-22` — Prosthetics launch set + condition reuse: `SIGNED`.
- Add `DEC-23` — Rehabilitation arc driver: `SIGNED`.
- Add `DEC-24` — Phantom-pain presentation boundary: `SIGNED`.
(One combined row is acceptable if the execution package names all phases.)

### 14.3 Expansion 16 — `expansion_16_the_rebuilt_body_plan.md` (intake enrichment)

Insert before its prosthetic phase:

> **Prerequisite — UNBLOCK-01 (F14):** the limb schema, grip vocabulary,
> `body_state` persistence, prosthetics launch set, and rehab arc are defined by
> `docs/plans/unblockers/UNBLOCK-01_BODY-INTEGRITY_SCHEMA_F14_XP06.md`. This
> expansion's prosthetic/rehab phases consume those seams and may not author a
> second limb model. Its automation/drone/rogue phases are **not** covered by
> that unblock plan and require a separate `RoboticsSystem` authorization.

### 14.4 `UNBLOCKED_PLANS_AUDIT_2026-09-19.md` (superseding note, no rewrite)

> **Superseded <date>:** the F14 schema sign-off and XP-06/EN-04 releases are
> now tracked by `docs/plans/unblockers/UNBLOCK-01_…`. The historical text above
> is retained for provenance.

### 14.5 `INTEGRATION_PLANS.md` (package row, post-signature)

```markdown
| `UNBLOCK-01-BODY-INTEGRITY` | Integrator (signed <date>) | [exact paths from Appendix B] | **DONE <date>:** F14-A..G executed; debt equipment half sealed; XP-06/EN-04 released; see implementation log | `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/LimbRequirementSchemaTests.cs`; `EquipLimbGateTests`; `ProstheticsCatalogTests`; `RehabilitationArcTests`; data-integrity; content-utilization; panel-lifecycle |
```

---

## 15. Closing statement

The equipment block has been recorded as blocked since 2026-09-17 and has
survived two execution waves because it is a *schema* decision rather than a
code gap. This plan converts it into the smallest set of signatures that release
the largest adjacent set of plans: one optional pair of fields, one two-word
vocabulary, one additive save sub-object, and six items. Everything else —
prosthetics tiers, rehab pacing, phantom pain, panel visibility — is separable
and separately signable.

Execute in the order: Plan 4 signs D3 (catalog hygiene) → this plan's Phase 1–2
(schema + gate) → Plan 2 may proceed independently → this plan's Phase 3–7
(content + presentation) → Plan 5 intakes Expansion 16 against these seams.