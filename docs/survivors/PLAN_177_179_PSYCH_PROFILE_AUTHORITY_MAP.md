# Plans 177 / 179 — Dream / psych-profile authority map

**Status:** ACCEPTED — §3 signed 2026-09-12  
**Package:** `DEBT-170-199-REMAINING-FAMILY-MAPS`  
**Rebase:** 177 dreams; 179 unified psychology  
**Date:** 2026-09-12

**Stale historical prose:** `Plan_177_Dream_Sleep_Event_System.md`, `Plan_179_Unified_Psychology_Phobia_System.md`. `DreamSystem` / `dream_templates.json` / `PsychologicalProfileSystem` / `phobia_definitions.json` are **ABSENT**.

---

## 1. Premise

| Concern | Owner | Save |
|---|---|---|
| Per-survivor psych **record** | `SurvivorMentalHealthSystem` / `SurvivorMentalHealthRecord` | `survivor_mental_health` |
| Combat trauma / flashbacks / guilt insomnia | Phase-0 engines | `phase0` |
| Phantom item echoes | `PhantomMemoryEngine` | `phantom_memory` |
| Breakdown arcs | `PsychologicalArcSystem` | `psychological_arcs` |
| Crisis pipeline | `MentalHealthCrisisSystem` | `mental_health_crisis` |
| **Treatment** | `PsychologicalSanatoriumSystem` via `ISurvivorConditionPort` | `psychological_sanatorium` |
| Authored phobia **traits** | `survivors.json` traits | roster |
| Dream content / phobia development ledger | **ABSENT** | |

`trauma_nightmares` in `psychological_trauma.json` is a catalog trauma type, not a dream engine.

---

## 2. Ownership (proposed)

| Concern | Authority |
|---|---|
| Canonical conditions | Existing trauma / mental-health / arc owners (no merge) |
| Treatment progress | Sanatorium only |
| Sleep / nightmare **presentation** | Narrative/journal **consumer** of the record above |
| Phobia **presence** | Existing survivor traits + trauma catalog |

---

## 3. Recommended defaults

| Item | Default |
|---|---|
| `DreamSystem` / `PsychologicalProfileSystem` / `PhobiaSystem` | **OUT** |
| Folding treatment into trauma ledgers | **OUT** |
| Named read model / queries over existing records + one narrative-event input (sleep or day tick) that may emit dream text | **IN** |
| Phobia growth, if ever | **DEFER** — extend `psychological_trauma.json` / traits, never a second profile save |

**Honesty:** catalog nightmare type ≠ dream system. Sanatorium jobs ≠ personality rewrite.

**Next implement (separate claim):** `DEBT-177-SLEEP-EVENT-CONSUMER` — one host/day or sleep beat that reads `SurvivorMentalHealthRecord` + insomnia and writes journal/presentation only.

---

## 4. Exact paths (read-only evidence)

`Needs/SurvivorMentalHealthSystem.cs`, `PsychologicalArcSystem.cs`, `Sanatorium/PsychologicalSanatoriumSystem.cs`, `GuiltInsomniaSystem.cs`, `src/Host/Phase0HostSession.cs`, `psychological_trauma.json`, `mental_arcs.json`

**Signed 2026-09-12.** Implement packages listed in this map stay unclaimed until separately promoted.
