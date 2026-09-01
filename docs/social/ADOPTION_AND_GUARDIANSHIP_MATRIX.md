# Adoption & Guardianship Matrix — Plan 12A

Four orphan/guardian arcs tied to real death/casualty producers.

## Arc 1 — The Relative (Warmarms)

| Field | Value |
|-------|-------|
| Quest ID | `quest_adoption_warmarms` |
| Event ID | `orphan_adoption_warmarms` |
| Narrative Hook | `adoption_warmarms` |
| Trigger | `RequireCasualtyOrphan` — parent died in shelter |
| Guardian | Family claim vs practical caregiver |
| Condition | Child cohort exists, orphan detected |
| Resolution | Relative takes guardianship; biological lineage preserved |
| Guardian Death | Re-orphan detection triggers new adoption arc |
| Relationship | Family bond (+affinity), obligation tension |

## Arc 2 — The Mentor (Fierce Mother)

| Field | Value |
|-------|-------|
| Quest ID | `quest_adoption_fierce_mother` |
| Event ID | `orphan_adoption_fierce_mother` |
| Narrative Hook | `adoption_rotational_vigil` |
| Trigger | `RequireCasualtyOrphan` — parent died on expedition/surface |
| Guardian | Apprenticeship mentor requests responsibility |
| Condition | Child cohort exists, mentor available |
| Resolution | Mentor becomes guardian; apprenticeship continues |
| Guardian Death | Apprenticeship pair dissolved; child re-orphaned |
| Relationship | Mentor-apprentice deepened to guardianship |

## Arc 3 — The Collective (Grange)

| Field | Value |
|-------|-------|
| Quest ID | `quest_adoption_grange` |
| Event ID | `orphan_adoption_grange` |
| Narrative Hook | `adoption_grange_plot` |
| Trigger | `RequireCasualtyOrphan` — both parents dead |
| Guardian | Shelter chooses communal guardianship |
| Condition | Child cohort exists, no valid individual guardian |
| Resolution | Collective responsibility; rotating care duty |
| Guardian Death | Not applicable (collective doesn't die) |
| Relationship | Diffused bond; child belongs to shelter |

## Arc 4 — The Ideological Split (Archive)

| Field | Value |
|-------|-------|
| Quest ID | `quest_adoption_archive` |
| Event ID | `orphan_adoption_archive` |
| Narrative Hook | `adoption_archive_intake` |
| Trigger | `RequireCasualtyOrphan` — parent died + ideological disagreement |
| Guardian | Two candidates disagree over upbringing |
| Condition | Child cohort exists, ≥2 guardians available, friction between them |
| Resolution | Mediation choice: one guardian wins, or collective fallback |
| Guardian Death | Reverts to next-priority guardian |
| Relationship | Contested bond; affects IdeologicalFrictionSystem |

## Common Mechanics

- **Orphan detection:** `RequireCasualtyOrphan` condition flag set by death cascade
- **Guardian candidate selection:** `RequireCohortAdoptionPending` flag gates choice events
- **Adoption conclusion:** `RequireAdoptionConcluded` flag set after resolution
- **Lineage:** Biological lineage preserved in `GenerationalLineageExtension`; guardianship modeled through relationship bonds in `SurvivorRelationsSystem`
- **No duplicate state:** Guardianship is a relationship type, not a second lineage record
- **Determinism:** Guardian candidate selection sorted by stable survivor ID
