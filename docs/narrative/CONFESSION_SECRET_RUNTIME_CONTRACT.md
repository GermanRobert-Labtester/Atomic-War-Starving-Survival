# Confession & Secret Runtime Contract

> **Author:** Narrative & Social Systems Team
> **Authority:** `Assets/Ashfall.Core/` (`Assets/StreamingAssets/Data/confession_secrets.json`)
> **System:** `ConfessionSecretSystem` & `ConfessionSecretCatalog`

---

## 1. System Intent

The **Confession & Secret System** models how hidden truths—personal failures, buried faction atrocities, and covert shelter acts—are discovered, confessed, and leveraged in ASHFALL.

In ASHFALL, secrets are not abstract currency:
1. **Interpersonal Confessions:** Survivors confess guilt, moral failure, or past survival choices to each other, creating a **Forgiveness** or **Grudge** branch that permanently alters relationships and morale.
2. **World & Faction Secrets:** Scavenged documents, radio intercepts, deathbed confessions, and phantom memory items reveal institutional deceptions. Players choose whether to **Expose** the truth for faction justice/standing, **Blackmail** for survival supplies at the cost of moral hardening, or **Keep Secret** to build deep personal trust.

---

## 2. Catalog Schema (`confession_secrets.json`)

The schema supports both survivor interpersonal confessions and discoverable faction/bunker secrets:

```json
{
  "schema_version": 1,
  "items": [
    {
      "secret_id": "secret_surgeon_lost_patient",
      "archetype_id": "the_surgeon",
      "category": "npc_personal",
      "subject_id": "the_surgeon",
      "secret_title": "The Patient They Lost",
      "secret_text": "{name} stares at their hands...",
      "discovery_path": "direct_confession",
      "discovery_source_id": "item_silver_scalpel",
      "gating_flag": "flag_secret_surgeon_confessed",
      "forgiveness_outcome": "...",
      "forgiveness_affinity": 20,
      "forgiveness_morale": 15,
      "grudge_outcome": "...",
      "grudge_affinity": -30,
      "grudge_morale": -15,
      "expose_outcome": "...",
      "expose_standing_faction": "faction_independent",
      "expose_standing_delta": 10,
      "expose_guilt_delta": 5,
      "blackmail_outcome": "...",
      "blackmail_resource_gain": "medicine",
      "blackmail_hardening_delta": 0.15,
      "keep_outcome": "...",
      "keep_trust_delta": 25
    }
  ]
}
```

---

## 3. Secret Taxonomy

| Category | Description | Primary Discovery Path | Leverage Hooks |
|---|---|---|---|
| **NPC Personal** | Past desertion, stolen identity, abandoned family, buried guilt. | Direct confession, deathbed, diary. | Forgiveness vs Grudge, or Blackmail vs Keep. |
| **Faction Institutional** | Suppressed famine tolls, census fraud, false-flag sabotage. | Documents, radio intel, courier corpses. | Expose (Standing) vs Blackmail (Supplies) vs Keep. |
| **Bunker Internal** | Ration skimming, concealed rooms, hidden morphine reserve. | Shelter search, logbook, social event. | Confront / Expose vs Blackmail vs Pardon. |
| **Historical Confession** | High command evacuation orders, pre-war cover-ups. | Deep excavation, bunker blackboxes. | Lore corroborated in Journal, Verdict testimony. |

---

## 4. State & Consequence Resolution

- **Idempotence:** Every secret transition (Discovered, Confronted, Resolved) is recorded by `secret_id`. A resolved secret never awards duplicate morale, items, or faction standing on reload.
- **Routing:**
  - Morale deltas $\rightarrow$ `NeedsSystem`
  - Relationship & Trust deltas $\rightarrow$ `SurvivorRelationsSystem`
  - Guilt deltas $\rightarrow$ `GuiltInsomniaSystem`
  - Moral Hardening $\rightarrow$ `MoralBranchingSystem`
  - Faction Standing $\rightarrow$ `FactionRelationsSystem` / `WorldState`
  - World Flags $\rightarrow$ `FlagLedger` / `KnowledgeBase`
