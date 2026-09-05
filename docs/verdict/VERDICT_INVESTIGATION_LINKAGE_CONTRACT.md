# Verdict Investigation Linkage Contract

> **Architecture Principle:** No artificial `trail_id` or parallel graph runtime. Investigation progression is emergent from existing authorities.

---

## 1. Authority Distribution

```text
Location Catalog (Data)
      │  (defines physical evidence, description, travel/danger/radiation)
      ▼
Player Exploration & Map Surface (Presentation)
      │  (reveals sites through expeditions, radio clues, or discovery triggers)
      ▼
Evidence Ledger & Quest State (Logic)
      │  (enrolls items, unlocks flags, advances narrative progression)
      ▼
NPCs & Radio System (Diegetic Context)
      │  (provides testimony, echoes, and telemetry broadcasts)
      ▼
Verdict Ending Evaluation (Conclusion)
```

---

## 2. Linkage Mechanisms Used in Expansion

| Mechanism | Authority | How It Links Arcs & Sites |
|---|---|---|
| **Physical Cable / Conduit** | Location Description | Direct spatial connection (e.g. Tide Gauge cable leads to Maritime pines / Met Station; Array cable leads to Fuse World). |
| **Administrative Markings** | Location Description / Items | Identical bureaucratic stamps (e.g. Department of the Interior linen charters, Tempest anchor marks). |
| **Cross-Referenced Coordinates** | Location Description | Azimuth bearings on plotting boards (Observation Bunker sector 5 points to Twelve-Gauge Array; Observation Tower bearings connect valley sites). |
| **Radio Broadcasts** | `verdict_radio.json` | Telemetry bursts (99.0 MHz carrier), witness bleed, and automated census calls referencing location activities. |
| **NPC Testimony & Traces** | `verdict_npcs.json` | Characters like Eden Vale, Ferris Voss, and Selya Saltmarsh who cite specific machine registers, shift charters, and survey sites. |
| **Quest & Discovery Flags** | `VerdictQuestCatalogLoader.cs` | Discrete flag keys (`flag_verdict_*`) gating evidence enrollment and investigation progression. |

---

## 3. Non-Linear Discovery Robustness

Because sites can be reached in non-linear order:
- No description relies on the player having already visited another site.
- Each description presents a standalone mystery that makes sense in isolation.
- When multiple sites are visited, their clues reinforce each other and converge toward the systemic truth.
