# Partial Implementation Rebase

Status: `ACCEPTED` · Batch: `BATCH-2026-09-12-PARTIAL-IMPLEMENTATION-REBASE` · Owner: Foreman

## Decision

Historical plans marked `PARTIAL / DO NOT DUPLICATE` are not a builder queue.
Each has an adjacent live authority, but the missing contract differs: some
need a producer event, some need a save boundary, and some are already live
under a different authority. A builder may not choose one merely because its
historical plan number is nearby.

This rebase directly rechecked the three prior promotion candidates and the
accessibility proposal:

| Plan | Rebase result | Current evidence | Decision |
|---|---|---|---|
| 173 Radio production | Genuine gap, not builder-ready | `RadioScheduleCoordinator` owns scheduling; `ShelterRadioStationSystem` owns intercept reception; `RadioHostSession` composes current listening. No player-program production state or `radio_programs.json` exists. | First map the actual schedule, delivery, and propaganda adapters. No second schedule or receiver. |
| 184 Accessibility | Genuine partial, not directly builder-ready | `UserSettingsData`, codec, `UserSettingsStore`, and `SettingsPanel` persist/expose high contrast, hazard labels, reduced motion, and large fonts. Runtime source search finds no consumer for those flags. | First define a bounded presentation-policy/reapply lifecycle, then make only these existing preferences effective. |
| 187 Bestiary | Current equivalent | `WildlifeEcosystemSystem.RecordObservation`, persisted observation state, `WildlifeEcosystemSaveStore`, `BestiaryPanel`, and `BestiaryKnowledge_IsObservationGated` provide the requested ledger and UI. | Retire from the partial queue; never add a second bestiary save. |
| 196 Food spoilage | Genuine gap, boundary not ready | `FoodPreservationSystem` owns stored cohorts/power outages; its catalog declares `allowed_food_types`, but cohorts have no type/temperature and the system has no thermal input. `KitchenNutritionSystem` is an adjacent food model. | Decide the one perishable-item/type/temperature authority before implementation. |

## Partial portfolio

The remaining historical partials stay in the disposition below until a
package supplies the missing input and state boundary. The current evidence
column is deliberately about ownership, not a claim that a similarly named
proposal is complete.

| Plans | Existing authority | Missing contract that prevents isolated implementation | Disposition |
|---|---|---|---|
| 176 Aging; 183 child development | `SurvivorLifecycle` and setup/generational systems | A live age chronology and lifecycle transition authority. Child stages must not be encoded as starting cohorts. | One later survivor-lifecycle architecture decision; do not extend legacy migration code. |
| 177 dreams; 179 unified psychology | Current trauma, insomnia, psychological-arc, and treatment owners | A bounded per-survivor psychological record and an explicit narrative-event input. | Define the profile/event contract first; preserve present treatment authorities. |
| 178 culture creation; 190 item lore | Cultural archive and inventory-mutation provenance | A creator/artifact identity that is distinct from static archive data and inventory delta provenance. | Map immutable content, item-instance identity, and discovery ownership before a builder starts. |
| 180 certification; 185 knowledge decay; 195 specialization | Skill progression, apprenticeship, and duty roster | Whether status is derived from skills or persisted, plus the capability-to-duty projection. | Treat as one survivor-capability family; do not add a parallel role counter or silently change duty assignment. |
| 182 relationship drift | `SurvivorRelationsSystem` with persisted affinities and daily tick | Deterministic positive/negative interaction inputs. Time-only decay would invent social events and create a punitive fake loop. | Instrument real interaction producers before decay or drift code. |
| 186 shelter maintenance | Equipment, power, thermal, water, and structural owners | A cross-owner maintenance contract; a unified durability ledger would duplicate state. | Keep component owners; promote only an event/inspection projection with named consumers. |
| 188 daily routines | Shelter schedule and duty roster | A sub-day clock and deterministic individual routine input. | Decide whether routine is a schedule extension; do not add a second survivor scheduler. |
| 189 water sources | Water treatment, hydrogeology, and deferred fluid delivery | One bulk-water/source ledger and a transfer rule to treatment. | Blocked behind `DEBT-PLAN168-WATER-DELIVERY`; no source simulation now. |
| 192 trade routes; 199 human migration | Caravan catalog, economy, world, and faction owners | Player route ownership, standing/raid outcomes, and seasonal population authority. | Map world/economy/faction authority jointly; never reuse wildlife migration state. |
| 193 chronic conditions; 198 medical history | Medical pipeline, radiation, disease, dose ledger | Longitudinal medical record policy, retention, privacy, and clinical consumers. | Establish one medical-record owner before creating conditions or history. |
| 194 emergency alerts | Individual hazard producers and `EmergencyResponseHud` | Typed producer facts, priority policy, and acknowledgement/persistence semantics. | Inventory all producers before adding an alert controller. |

## Promotion order — maximum three disjoint packages

### 1. `ACCESSIBILITY-PREFERENCE-APPLICATION-DESIGN` — Plan 184

Outcome: existing preference flags visibly affect the common Godot presentation
path, survive settings reload, and remain truthful in the Settings panel.

Exact initial seam: `src/Settings/UserSettings.cs`, `src/UI/AshfallUiHelpers.cs`,
the existing settings self-test, and only any additionally proven common theme
adapter. The package must not add new preference fields, a second persistence
file, a scene-by-scene rewrite, colorblind modes, screen-reader support, or
input remapping.

Preflight result: `AshfallUiHelpers` is a high-reach construction/color seam
(about 2,604 direct helper/color call sites), but it only affects controls
created after a setting changes. Existing controls have no universal reapply
lifecycle, and reduced motion/hazard text have no common runtime renderer.
The next package is therefore a small presentation-policy design with an
explicit refresh owner; it must not claim that stored toggles already deliver
those modalities.

### 2. `FOOD-SPOILAGE-OWNERSHIP-BOUNDARY` — Plan 196

Outcome: a signed, source-backed decision names whether
`KitchenNutritionSystem` or `FoodPreservationSystem` is the sole perishable
cohort authority, what stable food-type input it consumes, and how a room
temperature reaches it. The first package is a contract map, not a new food
ledger.

Non-goals: parallel item freshness, a new inventory store, weather copied into
Core, or a temperature simulation disconnected from storage locations.

### 3. `RADIO-PROGRAM-ADAPTER-MAP` — Plan 173

Outcome: a current-interface map names one existing schedule slot reference,
one delivery/reception fact, and the real current propaganda consequence input
that a player program may consume. It scopes the program-production save state
without taking over frequencies, stations, range, interception, or broadcast
scheduling.

Non-goals: a second schedule, receiver, faction corpus, radio network, or
unmapped morale/reputation changes.

## Handoff gate

No feature builder begins from this document alone. Each promoted package must
be entered in `INTEGRATION_PLANS.md` with current source evidence, exact owned
paths, non-goals, save/event seams, acceptance criteria, and a focused command
that fits the 180-second policy. The foreman then creates one path claim; no
parallel agent may edit that claim.
