# Plan 52 — Psychological Trauma, Shell-Shock & Catharsis Therapy
## Authority & Domain Mapping

**Catalog:** `Assets/StreamingAssets/Data/psychological_trauma.json` (`schema_version: 1`)
**Core System:** `Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs`
**Save Store:** `src/Host/SurvivorMentalHealthSaveStore.cs` (`survivor_mental_health_save.json`, section `survivor_mental_health`)
**UI Surface:** `SurvivorDetailPanel` & `AfflictionsPanel`

### 1. Domain Ownership
- `SurvivorMentalHealthSystem`: Owns survivor stress (`stress_permille` 0..1000), persistent trauma tokens, insomnia modifiers, quiet-room decompression progress, peer support synergy, and crisis event lifecycle.
- `CombatTraumaSystem`: Retains ownership of combat skirmish survival hypervigilance false-alarms.
- `JournalSystem`: Owns authored reflection entries triggered when survivors achieve therapeutic breakthroughs.
- `SurvivorsHostSession` / Needs: Supplies fatigue and physical survival constraints.

### 2. Design Rules & Tone Principles
- Non-stigmatizing presentation: Distinguish grief, shock, and exhaustion from moral failing or automatic aggression.
- Varied, non-violent acute crises: Withdrawal, hoarding, refusal of shift, panic, or barricading rather than generic berserk tropes.
- Multi-path recovery: Quiet-room meditation (`room_reading_quiet_room`), peer counseling (`trait_empathetic`, `skill_peacemaker`), and expressive journaling.
