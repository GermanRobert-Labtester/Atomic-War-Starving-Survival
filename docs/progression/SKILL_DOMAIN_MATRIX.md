# Skill Domain Matrix

> **Document Status:** Authoritative Progression Matrix
> **Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# Core)
> **Date:** September 2026

---

## 1. Action-Driven Progression Skills

These skills unlock organically as survivors perform labor and accumulate hidden XP within a discipline:

| Skill ID | Display Name | Discipline | XP Threshold | Skill Bonus | Expert Gate |
|---|---|---|---|---|---|
| `skill_field_dressing` | Field Dressing | medical | 50.0 XP | +10% | No |
| `skill_steady_hands` | Steady Hands | medical | 120.0 XP | +20% | **Yes** |
| `skill_rough_repairs` | Rough Repairs | crafting | 50.0 XP | +10% | No |
| `skill_workshop_sense` | Workshop Sense | crafting | 120.0 XP | +20% | **Yes** |
| `skill_signal_ear` | Signal Ear | science | 50.0 XP | +10% | No |
| `skill_cold_analysis` | Cold Analysis | science | 120.0 XP | +20% | **Yes** |
| `skill_watchful` | Watchful | combat | 50.0 XP | +10% | No |
| `skill_trail_memory` | Trail Memory | scavenging | 50.0 XP | +10% | No |
| `skill_hard_living` | Hard Living | survival | 50.0 XP | +10% | No |

---

## 2. Milestone & Narrative Skills

Granted through quest decisions, library manual study, or trade mastery:

| Discipline | Key Milestone Skills |
|---|---|
| **Combat** | `skill_tap_rack_bang`, `skill_cold_bore`, `skill_suppressing_fire`, `skill_close_quarters`, `skill_trap_setter`, `skill_desensitized` |
| **Survival** | `skill_ration_stretcher`, `skill_iron_stomach`, `skill_wasteland_brewer`, `skill_butcher`, `skill_forager`, `skill_de_escalator`, `skill_taskmaster` |
| **Crafting** | `skill_jury_rigger`, `skill_structural_engineer`, `skill_hvac_tech`, `skill_scrapper`, `skill_sandhog` |
| **Medical** | `skill_pharmacologist`, `skill_steady_hands_field`, `skill_triage_under_fire`, `skill_radiologist`, `skill_anatomist`, `skill_paramedic` |
| **Scavenging** | `skill_looters_reflex`, `skill_pack_mule`, `skill_light_step`, `skill_urban_pathfinder`, `skill_quartermaster` |
| **Science** | `skill_mycology`, `skill_thermodynamics` |
