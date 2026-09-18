# C2 — Unblock Amputation Integration — Premise Evidence

**Task:** Wave 8 Part 2, TASK C2 (`Seal-steps/847219_ASHFALL_WAVE8_IMPLEMENTATION_UNBLOCKER_PLAN_PART2.md`)
**Status:** PARTIAL — Phase 4 (avatar truth) executed; Phases 2/3 held at the signed-contract gate
**Date:** 2026-09-17
**Base commit:** `033df2b7` + D1 working changes
**Package:** `C2-AMPUTATION-INTEGRATION`
**Debt:** `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION`

## 1. Current-truth census (Phase 0)

### 1.1 `LimbCondition` consumers

| Consumer | Kind | Live? |
|---|---|---|
| `src/UI/AmputationTriagePanel.cs` | UI read/triage | yes (informational) |
| `AmputationSystem.GetWorkSpeedMultiplier(id)` | Core read API | **no production caller** (tests only) |
| `AmputationSystem.GetMovementSpeedMultiplier(id)` | Core read API | **no production caller** (tests only) |
| `AmputationSystem.GetCombatEffectivenessMultiplier(id)` | Core read API | **no production caller** (tests only) |
| `AmputationTriagePanel` counts | UI | yes |

Confirmed: limb state has **no equipment or expedition consumer** outside triage,
matching the debt row.

### 1.2 Equipment model (Phase 1.1)

- Owner: `Assets/Ashfall.Core/Inventory/Inventory.cs`.
- Slots: `EquipSlot { None, Body, Head, Face, Hands, Tool, Weapon }`.
- Preflight: `Inventory.Equip(ItemDefinition)` returns `bool`; the host wrapper
  `InventoryHostSession.EquipResult(itemId)` already returns a typed
  `ActionResult` (`Blocked("cannot_equip", …)`).
- **No handedness / two-handed / limb-requirement representation exists on
  `ItemDefinition`**, and no slot maps to a specific arm. There is therefore no
  model-supported way to express "arm state restricts two-handed equipment"
  without a schema redesign — which C2 §2 explicitly forbids doing inside this
  task ("If the equipment model cannot represent the signed restriction without
  a schema redesign, stop and split that redesign into a separate signed
  package").

### 1.3 Expedition model (Phase 1.2)

- Owner: `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`.
- Travel composition lives in `AdvanceOutbound`:
  `step = stance(1.0/1.5) × VehicleTravelMultiplier(exp) × weatherSpeedMultiplier`.
- Dispatch samples the weather multiplier once into
  `ExpeditionState.weatherSpeedMultiplier` (Plan 20C §36.2) so estimate and
  runtime agree.
- There is **no survivor-specific modifier input or field**. Adding one is a new
  (bounded, additive) contract on the expedition owner.

### 1.4 Avatar placeholder (Phase 4)

- `src/Main.Plans190_193.cs` `RefreshSurvivorVisuals(string)` was a print-only
  no-op with a TODO; callers were the amputation/prosthetic/bionic transition
  handlers. The recorded decision (`docs/gaps/PARTIAL_PLANS_VERIFIED_AUDIT.md`
  §D1; debt row) keeps avatar work OUT until a survivor-avatar owner exists;
  portraits resolve only via `AssetRegistry.FallbackSurvivorPath`.
- **Executed:** the method, its 5 call sites, and the misleading TODO were
  deleted. No sprite/portrait/animation work was introduced.

## 2. Authority boundaries (unchanged)

- Medical (`AmputationSystem`): sole writer/reader of limb state.
- Equipment (`Inventory`): owns equip eligibility and the disabled-reason
  vocabulary.
- Expedition (`ExpeditionSystem`): owns route speed and modifier composition.
- Presentation: displays reasons; never computes limb restrictions.

## 3. Contract matrix (proposed, Phase 1.3)

| Limb state | Work speed | Expedition movement | Equipment |
|---|---|---|---|
| Intact | 1.00 | 1.00 | unchanged |
| Wounded / Infected | (authored `GetWorkSpeedMultiplier`) | (authored `GetMovementSpeedMultiplier`) | unchanged |
| Gangrenous | (authored) | (authored) | unchanged |
| Amputated (arm) | (authored) | n/a (legs only) | **blocked** — no model representation |
| Amputated (leg) | n/a | (authored, floored) | n/a |
| Prosthetic / Bionic | (authored) | (authored) | unchanged |

The medical owner **already authors** every magnitude in
`GetWorkSpeedMultiplier` / `GetMovementSpeedMultiplier`
(`AmputationSystem.cs`), and those methods are unit-tested. Consuming them is
therefore not "inventing prosthetic grades" — it is wiring an existing,
tested contract.

**Equipment** cannot be represented without a schema change ⇒ split per §2.
