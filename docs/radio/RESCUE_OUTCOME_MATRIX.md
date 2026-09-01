# Rescue Outcome Matrix & Survivor Integration

> **Document Status:** Authoritative Distress Resolution Specifications
> **Authority:** Plan 24 (Task 24U–24Z)
> **Target:** Canonical Survivor integration, standing hooks, combat resolutions, and terminal state cleanup.

---

## 1. Outcome Categories & Terminal Rules

Every distress mission terminates in exactly one authoritative resolution. Orphan active distress entries are structurally prohibited.

| Category | Primary Outcome | Survivor Integration (`SurvivorCatalog`) | Faction / Standing Impact (`StandingRecord`) | Loot / Physical Rewards |
|---|---|---|---|---|
| **Genuine Rescue (Survivor Recruit)** | Survivor joins shelter or safe colony | Generates canonical character from `SurvivorCatalog` (e.g. Elena Vasquez, Dr. Tomas Araujo) with authentic traits and health deficits | +15 Standing with native faction (e.g. Scavengers, Works, Independent) | Professional gear, medical kits, unique recipe knowledge |
| **Genuine Rescue (Allied Group)** | Faction outpost or caravan secured | No direct shelter recruit; establishes active barter line or discount | +25 Standing with recipient faction; opens persistent trading route | Bulk grain, water cisterns, machine parts |
| **Grim / Too Late** | Source perished before arrival | None (memorial log / deceased body discovered) | Neutral (sadness morale check; resolved peacefully) | Personal effects, cassette diary, salvageable scrap, keycard |
| **False / Raider Trap** | Hostile encounter triggered | None (hostile combatants) | Defeat raiders: +10 Regional Security; loss: party damage/injuries | Enemy weapons, ammunition, stolen barter goods |
| **Mystery / Sigint** | Pre-war beacon / cipher node discovered | None | Unlocks signal intelligence log entry; feeds Plan 11B Cipher Quests | Technology blueprints, rare vacuum tubes, cipher codebook sheets |

---

## 2. Survivor Recruit Integrity

When a distress mission yields a new shelter recruit:
1. **Catalog Authority:** Recruit attributes (Name, Traits, Starting Condition, Medical State) are drawn strictly from `SurvivorCatalog.cs` definitions. No ad-hoc characters with random unvalidated attributes are spawned.
2. **Initial Condition:** Rescued survivors always enter with realistic survival debuffs (e.g. `Dehydrated`, `Malnourished`, `Hypothermia`, or `RadDose_Minor`), requiring immediate shelter triage.
3. **Capacity Discipline:** If the shelter is at maximum capacity, the player can choose to direct the survivor to a friendly settlement (yielding high Faction Standing and future barter discounts) instead of forcing shelter overcrowding.
