# Integrated Plans Archive — ASHFALL

This directory is the canonical repository for **already-integrated plans**.

## Policy & Workflow Rule

1. **Immediate Archival Upon Integration:**
   When a plan is integrated, it must **immediately** be moved to the integrated plans folder (`docs/plans/integrated/<category>/` or `.ai/plans/integrated/<category>/`).
   Even if producing and integrating a plan within the very same working session, the plan must be moved to the integrated plans repository as soon as implementation and verification are complete.

2. **Categorization:**
   Plans are categorized by subsystem matching the structure of `docs/`:
   - `ui/` — UI integration, controller parity, HUD, navigation
   - `systems/` — Core systems, simulation loops, gameplay rules
   - `shelter/` — Shelter interior, maintenance, room assignments
   - `medical/` — Trauma, triage, dose registers, treatments
   - `expeditions/` — Travel, map nodes, vehicle operations
   - `combat/` — Combat encounters, tactical engagements
   - `factions/` — Faction warfare, diplomacy, espionage
   - `saves/` — Save codecs, migrations, persistence roundtrips
   - `ci/` — CI pipelines, test gates, release verification
   - ... and all other subsystem domains corresponding to `docs/`.

3. **Status Preservation:**
   Moved plans retain their `STATUS: APPROVED BY USER` and implementation logs so full auditability and pre-commit checks are preserved.
