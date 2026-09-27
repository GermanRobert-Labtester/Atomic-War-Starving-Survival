# FOUR-TRACK ORPHAN CORE INTEGRATION — FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED** (2026-09-26)
> **STATUS: APPROVED BY USER** — "Please find 4 plans to fully integrate,
> don't leave as partials, don't commit and don't overly test!"

Selection method (Rule 7, current evidence): mechanical sweep of all 322 Core
`*System` classes for **zero `src/` references**, cross-referenced against the
plan corpus, authored data catalogs, ACTIVE path claims, and signed retirement
decisions. Systems whose seams were already sealed (Plan 196's type/temp seam,
signed ownership map) or whose plan sits under an ACTIVE claim (Plan 213,
PFGL octet) were excluded.

## Track 1 — Plan 193 · Chronic Conditions & Accommodations

- **Core owner:** `Assets/Ashfall.Core/Medical/ChronicConditionSystem.cs`
  (0 host refs; full AddCondition/Assign/Remove/capability/replay API).
- **Data:** `chronic_conditions.json` — 6 conditions + 6 accommodations.
- **Plan contract:** one truthful path clinical→accommodation→capability on
  the *real duty/needs owner*; saved under existing medical custody; shown via
  `AfflictionsPanel`/survivor care surface; one valid event, one refusal,
  replay-after-restore. No second health ledger.
- **Seams to bind:** `MedicalHostSession.Pipeline` event fan-out (`OnDiagnosisConfirmed`
  is currently consumed by nobody); duty/needs owners take the capability query.

## Track 2 — Plan 212 residual · capsule opening truthfulness

- **Core owner:** `Assets/Ashfall.Core/Communication/TimeCapsuleSystem.cs`
  (host/save/day tick/panel all exist; DEC-115 signed the catalog).
- **Verified defect (plan §3):** `OpenCapsule` only checks existence/IsOpen —
  the `TimeCapsulePanel` button passes literal `"Overseer"` and day `1`,
  bypassing DateBased/EventBased/SurvivorBased conditions; pending
  `LegacyMessage.Content` is rendered while still PENDING.
- **First deliverable (plan-owned):** host-level command that reads `_simDay`,
  validates author + condition, reports an explicit blocked reason; private
  projection filters message content until lawful delivery; exactly-once open;
  save/reload preserves IsOpen/OpenedDay without replaying morale.

## Track 3 — Plan 190 · Item Lore & Provenance

- **Core owner:** `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs` (0 host refs;
  one internal Core consumer `ItemInspectionModel` — reachability must be
  verified in Phase 0 before wiring).
- **Data:** item lore/provenance slice under the existing lore catalog family.

## Track 4 (SUPERSEDED mid-task, documented rather than integrated) — `ShelterPrisonerSystem`

- **Core owner:** `Assets/Ashfall.Core/Shelter/ShelterPrisonerSystem.cs`
  (`SystemId = "shelter_prisoners"`; 0 callers) — **except** a landed
  `src/Host/ShelterPrisonerSaveStore.cs` that nothing references (a stranded
  half-package). Resolving it recovers the missing host/lifecycle half.
- **Excluded:** `SurvivorBarterSystem` (Plan 213 lives under the ACTIVE PFGL
  octet claim), `FoodTypeSystem` (Plan 196 seam signed; mobile list must not
  compete with `FoodPreservationSystem`), Plan 24 cluster (Plan 24 CLOSED).
- **Superseded mid-task (Rule 7):** `ShelterPrisonerSystem` was already
  dispositioned — `INTEGRATION_PLANS.md` (`ORPHAN-SEAL-PRIORITY-W1`,
  2026-09-23) "retired as a live authority with one-time legacy import into
  canonical `PrisonerSystem`"; DEC-205 keeps it a sealed pure-domain
  authority. Integrating it would resurrect a retired authority and create a
  second captive ledger (Rules 1/5), so it was dropped.
- **Replacement Track 4 — Plan 212 letter-route · `LetterDeliverySystem`** (`Narrative/LetterDeliverySystem.cs`; census ORPHAN). This is the plan's own named "letter delivery" split (§10: "TimeCapsuleSystem for capsules; SurvivorLetterDeliverySystem for discovered letters") with its explicit contract: a named discoverer, recipient match, one explicit delivery decision, privacy-safe UI, and an explicit save-ownership decision — never folded into the capsule envelope. False-orphan exclusions recorded: `NvisC4ISystem` (thin subclass of the host-wired NvisCommunicationsSystem) and `ShelterPrisonerSystem` (retired authority per ORPHAN-SEAL-PRIORITY-W1 / DEC-205).

## Discipline

- One track at a time; each track ends **fully integrated** or is explicitly
  reported as blocked premises — no partial residue.
- No commit (user directive). Shared dirty worktree preserved.
- No over-testing: one focused integration test per track plus the standing
  build/data-integrity gates; no full suite, no speculative tests.

## Executed result (2026-09-26, final)

| Track | Plan | Outcome |
|---|---|---|
| 1 | Plan 193 chronic conditions | FULLY INTEGRATED — host session + save section + clinical producer seam + care-surface rows + probe 12/12 + tests 7/7 |
| 2 | Plan 212 capsule truthfulness | FULLY INTEGRATED — Core condition gate + live-day provider + privacy projection + tests 6/6 |
| 3 | Plan 190 item lore | FULLY INTEGRATED — provenance from committed craft/intake producers + registered section + panel read model + tests 7/7 |
| 4 | Plan 212 letter route | FULLY INTEGRATED — separate letter authority + privacy gate + canonical morale + panel section + tests 7/7 |

Evidence: full-tree build 0 errors; each track one focused test (27 assertions
total, all PASS, no speculative tests); `SaveSectionRegistryTests` 5/5;
comprehensive save/migration gate 1754/1754 (three new sections ride the pin);
`--data-integrity-selftest` 427/427 catalogs 0 errors; `--player-panels-uitest`
PASS; `--ui-layout-selftest` PASS; `--chronic-condition-selftest` 12/12.

Deliberately excluded during premise checks (Rule 7): `SurvivorBarterSystem`
(Plan 213 under the ACTIVE PFGL octet claim), `FoodTypeSystem` (Plan 196 seam
signed; mobile list must not compete with `FoodPreservationSystem`),
`ShelterPrisonerSystem` (retired per ORPHAN-SEAL-PRIORITY-W1 / DEC-205),
`NvisC4ISystem` (false orphan — thin subclass of the host-wired
`NvisCommunicationsSystem`), `DraisineRecoverySystem`/`ArmoredDraisineRecoverySystem`
(subclasses of the host-wired `DraisineRerailingSystem`).

No commit (user directive). No claim row (foreman-only ledger); scope recorded
in `.ai/state.md`.
