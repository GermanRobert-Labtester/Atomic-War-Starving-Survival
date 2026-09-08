# Final Wish Step Grammar Contract

**Document:** `docs/survivors/FINAL_WISH_STEP_GRAMMAR.md`

---

## 1. Step Progression Model

In `FinalWishSystem.cs`:
- Progression is strictly ordered: `AdvanceWishStep(survivorId, stepId)` increments `stepsCompleted` by 1.
- Completion occurs only when `stepsCompleted >= requiredSteps`.
- Premature progression is prohibited: calling `AdvanceWishStep` when inactive, expired, or completed returns `false` without advancing state.
- Step IDs are local snake_case strings describing the action (e.g. `weigh_the_filler`, `inspect_manifold_sleeves`).

---

## 2. Objective Typology in Authored Data

Each step includes rich presentation and host-gating metadata:

1. **Item Requisition (`required_items`):**
   - The survivor requires specific items from storage or the field (e.g. `["trap_improvised_wire"]`, `["item_preservation_salt", "clean_water"]`).
   - Host checks player inventory before invoking `AdvanceWishStep`.

2. **Expedition Target (`requires_location`):**
   - The step requires visiting a canonical location in the wasteland (e.g. `"loc_settlement_cape_beacon"`, `"location_ash_dune_cemetery"`).
   - Resolves when an expedition arrives at that node.

3. **Dialogue / Person Interaction (`requires_npc`):**
   - The step requires interacting with a specific living NPC or fellow survivor (e.g. `"npc_mara_veln"`, `"npc_lina"`, `"npc_oskar_ruut"`).

4. **Knowledge Handoff (`skill_transfer`):**
   - Marks the transfer of a skill or trade technique to an apprentice survivor (e.g. `"skill_field_dressing"`, `"skill_trap_setter"`).

5. **Internal Ritual / Vigil:**
   - Unattended or quiet bedside actions (e.g. preparing an alcove, setting tool benches in order) requiring no external items.
