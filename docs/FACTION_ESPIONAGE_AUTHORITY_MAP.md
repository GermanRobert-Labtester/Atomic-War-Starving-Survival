# Plan 51 — Dynamic Faction Espionage, Sabotage & Infiltration Network
## Authority & Domain Mapping

**Catalog:** `Assets/StreamingAssets/Data/faction_intelligence.json` (`schema_version: 1`)
**Core System:** `Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs`
**Save Store:** `src/Host/ShelterEspionageSaveStore.cs` (`faction_espionage_save.json`, section `faction_espionage`)
**UI Surface:** `FactionDetailPanel` & `DailyBriefingModal`

### 1. Domain Ownership
- `ShelterEspionageSystem`: Owns sleeper informant tracking, suspicion levels, shelter infiltration risk, abstract sabotage attempts, counter-intelligence investigation, dead-drop generation, and turned double-agent status.
- `FactionSystem` / Faction Stances: Owns canonical faction standings and diplomatic hostility thresholds.
- `Inventory`: Source of supply data leaked to hostile factions; target of abstract sabotage; supplier of recruitment/bribe items.
- `DailyBriefingModal`: Presents detected leaks, sabotage alerts, dead drops, and uncovered spies without revealing unproven suspicions.

### 2. Safeguards & Fairness Rules
- Non-operational: No actionable real-world tradecraft or sabotage procedures.
- Low baseline arrival chance for sleeper agents to prevent player paranoia against all refugees.
- Counter-intelligence stations (`room_armory_munitions`, `skill_watchful`) provide evidence-based investigation, not instantaneous omniscience.
