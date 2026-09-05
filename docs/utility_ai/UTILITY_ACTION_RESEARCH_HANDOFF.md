# Utility Action Research Handoff

> **Research Seams:** Interface between `action_conduct_research` and Plan 34 knowledge research systems (`research_knowledge.json`).

---

## 1. Authority Model

1. **Utility AI Owns Worker Commitment:** Scores whether an idle survivor with scientific affinity should walk to the lab and spend a shift studying technical schematics.
2. **`ResearchSystem` Owns Progression:**
   - Tracks active project nodes and accumulated research points.
   - If no project is actively queued by the player, `action_conduct_research` evaluates to ineligible (score = 0).
   - When active, work ticks contribute deterministic progress towards the active node.

---

## 2. Preemption Rules

- Survival needs (severe hunger, critical radiation, emergency medical trauma) strictly preempt research work.
- If the shelter power grid experiences brownout or `room_laboratory_research` is unpowered (`fx_laboratory_offline`), the action is immediately disabled.
