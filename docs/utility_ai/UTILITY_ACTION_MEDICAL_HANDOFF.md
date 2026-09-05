# Utility Action Medical Handoff

> **Medical Seams:** Interface between `action_treat_wounded`, `action_seek_treatment`, and Plan 09 medical systems (`afflictions.json`, `disease_catalog.json`).

---

## 1. Medical Action Boundaries

1. **`action_treat_wounded` (Medic Perspective):**
   - *Target:* Any survivor suffering from bleeding, infection, severe trauma, or acute radiation sickness.
   - *Triage Sorting:* Patients sorted by lethal risk (arterial bleeding > radiation poisoning > wound infection > minor contusions).
   - *Resource Consumption:* Medical system validates and consumes bandages, suture needles, or antibiotics.
   - *Concurrency:* A patient claimed by one medic is locked; second medics move to the next patient or alternate actions.

2. **`action_seek_treatment` (Patient Perspective):**
   - *Trigger:* Wounded survivor whose health is depleted or who has untreated negative afflictions.
   - *Behavior:* Survivor heads to `room_clinic` triage beds rather than attempting heavy labor or wandering.

---

## 2. Trait Interactions

- `hitman` trait vetoes `TagMedicalTriage` (refuses to act as a healer).
- `germaphobe` trait vetoes `TagMedicalTriage` unless `context.HasHazmat` is true.
- Unconscious or immobilized survivors cannot perform `action_seek_treatment` and rely on rescue/triage by fellow survivors.
