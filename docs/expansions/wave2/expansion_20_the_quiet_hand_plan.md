# ASHFALL — Expansion 20 Design Bible
# THE QUIET HAND
### Wave 2 · Espionage, Counterintelligence, Informants, Interrogation, and Prisoners

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Factions` (Espionage, CounterIntelligence, ShelterEspionage, Prisoner, Bounty), `Ashfall.Core.Shelter` (CaptiveInterrogation), `Ashfall.Core.Economy` (BlackMarket adjacent)
**Proposed host owner:** `QuietHandHostSession` (extends `EspionageHostSession` + `CounterIntelligenceHostSession` + prisoner stores)
**Existing save sections:** `espionage`, `counter_intelligence`, `prisoner`, `shelter_prisoner`, `shelter_espionage`, `contraband_stash`
**Existing CLI verbs:** `--espionage-selftest`, `--counter-intelligence-selftest`, `--prisoner-selftest`, `--bounty-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already runs a quiet war. `EspionageSystem` plans and executes missions with
typed mission types, statuses, double agents, and confidence bands. `CounterIntelligenceSystem`
owns vetting, undercover exposure, suspicion evidence, detention, interrogation, and
defector clearing, with bounded suspicion and typed events. `ShelterEspionageSystem`
tracks sleeper agents, dead drops, and sabotage incidents. `CaptiveInterrogationCatalog`
and `ShelterPrisonerSystem` handle captives; `FactionBountySystem` handles contracts.
But the content is thin: **a few mission rows, a few profiles, and small tables.**

**The Quiet Hand** turns that machinery into a living intelligence conflict: informant
networks, dead drops and signals, cover identities, interrogation limits, prisoner
exchange, defector integration, bounty hunters, and the constant, corrosive question
of how much privacy and freedom a shelter will trade for safety.

The expansion is deliberately uncomfortable. It is about secrets, not violence
pornography. Interrogation is bounded and consequential; torture is never a valid
tool and never yields reliable intelligence. Prisoners are people with terms and
rights; a shelter that forgets that becomes the thing it feared.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Every settlement has two histories. One is the ration board, the duty roster, and the
public log. The other is who told whom what, when, and for how much. The Quiet Hand
is the expansion about the second history.

A shelter discovers that a rival has placed a sleeper inside it. The sleeper may be
a fanatic, a debtor, a coerced parent, or someone who genuinely believes the rival
is right. Finding them requires an informant network, a dead-drop watch, and the
willingness to suspect neighbors. Once caught, the sleeper must be interrogated,
imprisoned, exchanged, or turned — and each option has a cost that reaches into the
shelter's trust and morale.

At the same time, a bounty is posted on the shelter's administrator, and a defector
arrives with intelligence he wants to sell. The expansion's engine is not combat; it
is **information asymmetry**. The player rarely knows the whole truth, and the game
never pretends otherwise.

### 1.2 The five loops it adds

```
    Informants ──► Intelligence ──► Decisions ──► Exposure ──► Retaliation
         │              │               │             │            │
         ▼              ▼               ▼             ▼            ▼
    Payment,      Confidence      Covert ops,   Counter-intel  Standing,
    reliability   bands           prisoner      vetting        bounty,
    exposure                      handling                     war risk
         │                                                            │
         ▼                                                            ▼
    Tradecraft ──► cover, dead drops, signals ──► interception ──► trust
```

### 1.3 What the player manages

1. **Informants.** Paid or coerced sources inside other settlements, with
   reliability, exposure risk, and motive. An informant is a person, not a stat.
2. **Missions.** `EspionageSystem` already types missions and statuses. The expansion
   supplies authored operations, targets, and consequences.
3. **Counterintelligence.** Vetting candidates, watching suspicions, and exposing
   sleepers — with false-positive risk. Suspicion is bounded and evidence-based.
4. **Interrogation.** Bounded, ethical, and time-limited. Coercion reduces the value
   of what is learned; a broken captive gives you what you want to hear.
5. **Prisoners.** Terms, labor, exchange, release, escape, and the slow moral drift
   of a shelter that keeps people in cells.
6. **Defectors.** Vetting, integration, loyalty, and the intelligence they bring.
7. **Bounties.** Contracts, hunters, protection, counter-bounties, and the price of
   being important.
8. **The truth.** Confidence bands (`IntelConfidence`) are real. The player acts on
   rumor, estimate, confirmed, or penetrated intelligence, and pays for mistakes.

### 1.4 What it is not

- Not a combat system. Operations are abstract; exposure is a typed consequence.
- Not a torture simulator. Coercion is bounded, penalized, and never reliable.
- Not a black-market expansion. Trade/heat/fence content is intentionally left to
  the XP-04 economy legs and the contraband authority; this expansion focuses on
  secrets, people, and information.
- Not a second faction or standing system. Everything routes through
  `FactionStanceEngine`.
- Not a second save authority.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Factions/EspionageSystem.cs` | Mission types, statuses, double agents, confidence | `LIVE` |
| `Assets/Ashfall.Core/Factions/CounterIntelligenceSystem.cs` | Vetting, exposure, suspicion, detention, interrogation, defectors | `LIVE` |
| `Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs` | Sleepers, dead drops, sabotage | `LIVE` |
| `Assets/Ashfall.Core/Factions/InfiltratorCatalog.cs` + loader | Infiltrator profiles | `LIVE` |
| `Assets/Ashfall.Core/Factions/FactionIntelligenceCatalog.cs` | Intelligence catalog | `LIVE` |
| `Assets/Ashfall.Core/Shelter/CaptiveInterrogationCatalog.cs` | Interrogation content | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterPrisonerSystem.cs` | Shelter prisoners | `LIVE` |
| `Assets/Ashfall.Core/Factions/PrisonerSystem.cs` | Faction prisoners | `LIVE` |
| `Assets/Ashfall.Core/Factions/FactionBountySystem.cs` | Bounties | `LIVE` |
| `Assets/Ashfall.Core/Factions/EspionageConsequenceRouter.cs` | Consequence routing | `LIVE` |
| `src/Host/EspionageHostSession.cs`, `CounterIntelligenceHostSession.cs` | Host | `LIVE` |
| `src/Host/PrisonerSaveStore.cs`, `ShelterPrisonerSaveStore.cs`, `ShelterEspionageSaveStore.cs` | Persistence | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `espionage_missions.json` | 3.4 KB | mission definitions |
| `infiltrator_profiles.json` | 4.7 KB | infiltrator types |
| `faction_intelligence.json` | 3.9 KB | intelligence items |
| `interrogation_tactics.json` | 4.5 KB | tactics |
| `captive_interrogations.json` | 3.2 KB | captive scenarios |
| `bounty_board.json` | 2.1 KB | contracts |
| `black_market_inventory.json` | 4.4 KB | adjacent trade |
| Contraband docs | multiple matrices | adjacent authority |

### 2.3 Confirmed gaps

- **GAP-20-1 — Few missions and profiles.** The espionage catalogs are demonstrations
  of a system that supports far more.
- **GAP-20-2 — No informant network.** Informants exist only implicitly; there is no
  recruitment, payment, reliability, or exposure model.
- **GAP-20-3 — No dead-drop/tradecraft content.** `ActiveDeadDrop` exists with little
  authored content, signals, or interception.
- **GAP-20-4 — Interrogation is a catalog, not a system.** No ethics, reliability,
  time limits, or consequences beyond the catalog row.
- **GAP-20-5 — Prisoners lack terms.** No exchange, labor, release, escape, or
  humanitarian baseline.
- **GAP-20-6 — No defector integration.** The system accepts defectors; nothing
  models living with them, testing them, or losing them.
- **GAP-20-7 — No bounty hunters.** `FactionBountySystem` exists without hunter
  profiles, protection options, or escalation.
- **GAP-20-8 — No cover identities.** No authored covers, legends, or burn rules.
- **GAP-20-9 — No espionage locations or NPCs.**

### 2.4 Non-duplication statement

This expansion will **not** add a second espionage, counterintelligence, prisoner,
bounty, faction, or standing system. It will **not** duplicate XP-04 black-market
trade legs or contraband heat/fence content; trade crime stays with the economy
authority. It extends the live owners with data and additive subsystems, and routes
all social consequences through `NeedsSystem`, `MoraleContagionSystem`,
`GuiltInsomniaSystem`, and `FactionStanceEngine`.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Information is the weapon.** The expansion's combat is knowing. A
correct report prevents a raid; a false one causes one.

**Pillar 2 — Trust is the resource.** Every informant, defector, and prisoner changes
who trusts whom. Counterintelligence that suspects everyone destroys the shelter it
protects.

**Pillar 3 — Coercion is a trap.** Torture and threats produce unreliable
intelligence and permanent guilt. The expansion must make this mechanically true,
not merely stated.

**Pillar 4 — Prisoners are people.** Terms, food, medicine, exchange, and release are
real obligations. A cell is a promise the shelter makes and must keep or break.

**Pillar 5 — The truth is bounded.** Confidence bands are honest. The player never
gets perfect knowledge, and acting on rumor is a legitimate strategy with real risk.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A dead drop | A loose brick, a chalk mark, a wet envelope | Spy-thriller gadget |
| An informant | A nervous trader who needs medicine | Super-spy |
| Interrogation | Two chairs, a long silence, a truth | Torture dungeon |
| A prisoner | A meal log, a visitor list, a name | Cage imagery |
| A defector | A person with a story and a debt | Trophy asset |
| A bounty | A notice with a price and a name | Hitman fantasy |

### 3.3 Content limits

- No torture as a valid or rewarded mechanic. Coercion has explicit penalties.
- No real-world intelligence agencies, programs, or tradecraft terminology.
- Prisoners retain dignity; execution is a grave, rare, and consequential act.
- Espionage is political and information-based, never ethnic or religious targeting.
- No graphic violence, no sexual content, no exploitation.

---

## 4. THE QUIET WORLD

### 4.1 Interior rooms

- **`room_radio_room`** — intercept, listen, and decode.
- **`room_interrogation_room`** — a table, two chairs, a lamp, a log.
- **`room_holding_cell`** — a small room with a door that logs.
- **`room_safe_room`** — where informants and defectors are met.
- **`room_document_vault`** — ciphers, dossiers, and sealed orders.
- **`room_counterintel_office`** — vetting, patterns, and the suspicion board.
- **`room_tradecraft_store`** — covers, inks, signals, and dead-drop kits.
- **`room_watch_floor`** — observation of arrivals and departures.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_dead_drop_alley` | The Chalk Alley | 5 | Dead drops and signals |
| `loc_listening_post` | The Listening Post | 6 | Intercept station; salvage |
| `loc_safe_house` | The Halfway House | 5 | Neutral meeting ground |
| `loc_border_post` | The Paper Gate | 5 | Identity checks and forgery |
| `loc_prison_camp` | The Wire Yard | 6 | Prisoner camp; exchange site |
| `loc_hunter_camp` | The Long Camp | 7 | Bounty hunters' base |
| `loc_dossier_archive` | The Registry | 7 | Pre-war records; identity truth |
| `loc_courier_road` | The Message Road | 5 | Couriers and intercepted mail |
| `loc_radio_hill` | The Whisper Hill | 6 | Long-range signals |
| `loc_training_yard` | The Yard | 4 | Tradecraft training ground |

All locations require valid item references and scanner registration.

### 4.3 The confidence ladder

The expansion never shows the truth for free. Intelligence arrives at a confidence
band, and each band has an authored error rate:

| Band | Source | Error rate | Action |
|---|---|---|---|
| Rumor | travelers, gossip | high | cheap, risky |
| Estimate | informant, pattern | medium | useful |
| Confirmed | multiple sources | low | actionable |
| Penetrated | inside agent | very low | precious |

A decision made on rumor is legitimate and may be wrong. The expansion's tension is
that the player must act before confidence improves.

---

## 5. MAIN STORYLINE — "THE HAND THAT ISN'T YOURS"

### 5.1 Central conflict

A raid fails because the raiders knew the shelter's patrol schedule. Someone told
them. The shelter's security chief, **Ilyana Krest**, begins a quiet investigation
that turns neighbors into suspects. The rival's sleeper is real — but so are the
false accusations, and the shelter's trust is the collateral.

At the same time, a courier intercept reveals that a bounty has been posted on the
shelter's administrator, **Marek Dowd**, and a defector arrives at the Paper Gate
offering to name the hunters in exchange for protection. And in the Wire Yard, a
captured agent refuses to speak, while the shelter argues about what it is allowed
to do to make him.

The expansion's question: **how much freedom will the shelter trade for safety, and
who decides when the trade goes too far?**

### 5.2 Theme (unspoken)

**A shelter that watches everyone to find one traitor becomes a place nobody wants to
defend.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_chief_ilyana_krest` | Ilyana Krest | Security chief | Drives the hunt; must confront her own methods |
| `npc_administrator_marek_dowd` | Marek Dowd | Administrator | The bounty target; wants normality |
| `npc_handler_berek_saul` | Berek Saul | Intelligence handler | Informants and dead drops |
| `npc_defector_ana_ved` | Ana Ved | Defector | Brings truth and a hidden agenda |
| `npc_captive_sergei_lom` | Sergei Lom | Captured agent | The interrogation's center |
| `npc_informant_pell_wick` | Pell Wick | Informant | A trader who needs medicine |
| `npc_warden_hask` | Warden Hask | Prison warden | Terms, dignity, and limits |
| `npc_hunter_vesna_rye` | Vesna Rye | Bounty hunter | Professional, not sadistic |

### 5.4 Story beats (15)

1. **The Failed Raid.** Patrol schedules leaked; casualties.
2. **The Quiet Investigation.** Krest begins vetting everyone.
3. **The First Suspicion.** An innocent is flagged; the shelter splits.
4. **The Informant.** Pell Wick sells a first true report for medicine.
5. **The Dead Drop.** A chalk mark on the west wall; a watched brick.
6. **The Bounty.** A courier intercept reveals a contract on Dowd.
7. **The Skin.** The mole, identified but not yet proven.
8. **The Capture.** A second agent is caught; interrogation begins.
9. **The Limit.** The shelter debates coercion; the rules are set or broken.
10. **The Defector.** Ana Ved offers names for protection.
11. **The Exchange.** A prisoner trade with the rival is proposed.
12. **The Hunter.** Vesna Rye arrives to collect.
13. **The Reckoning.** The mole is exposed; false accusations surface.
14. **The Cost.** Trust, morale, guilt, and the shelter's identity.
15. **The Quiet After.** Final disposition of the network, the prisoner, and the rule.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Investigation scope | broad / targeted / none | safety vs. trust |
| Interrogation | ethical / coercive / none | truth vs. guilt |
| Informant payment | goods / coin / coercion | loyalty vs. cost |
| Defector | admit / test / refuse | trust vs. risk |
| Prisoner terms | exchange / labor / release | pragmatism vs. principle |
| Bounty response | hide / counter / confront | fear vs. resolve |
| Mole disposition | execute / exile / turn | justice vs. utility |
| The rule | codify interrogation limits / leave unwritten | law vs. convenience |

### 5.6 Endings (5 + fade)

1. **The Watched House** — the mole is caught; the shelter is safe and colder.
2. **The False Accusation** — an innocent is broken; the shelter's trust never recovers.
3. **The Turned Hand** — the mole is turned and becomes the shelter's best source.
4. **The Written Rule** — interrogation limits are codified; some intelligence is lost
   and some identity is kept.
5. **The Open Gate** — the shelter refuses the quiet war and relies on openness.
6. **Fade** — the investigation is dropped; the mole remains.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_hand_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (15)

`quest_hand_failed_raid`, `quest_hand_quiet_investigation`, `quest_hand_first_suspicion`,
`quest_hand_the_informant`, `quest_hand_dead_drop`, `quest_hand_the_bounty`,
`quest_hand_the_skin`, `quest_hand_the_capture`, `quest_hand_the_limit`,
`quest_hand_the_defector`, `quest_hand_the_exchange`, `quest_hand_the_hunter`,
`quest_hand_the_reckoning`, `quest_hand_the_cost`, `quest_hand_quiet_after`.

### 6.2 Side quests (30)

**Informants (5)**
- `quest_hand_recruit_informant` — recruit a first source
- `quest_hand_payment_terms` — set terms of payment
- `quest_hand_reliability_test` — test a source with a known fact
- `quest_hand_exposed_source` — protect or abandon an informant
- `quest_hand_double_source` — a source working for both sides

**Dead drops and signals (5)**
- `quest_hand_dead_drop_setup` — establish a drop
- `quest_hand_signal_code` — design chalk marks
- `quest_hand_stakeout` — watch a drop for a week
- `quest_hand_intercepted_mail` — read someone else's letter
- `quest_hand_burn_the_drop` — destroy a compromised location

**Missions (5)**
- `quest_hand_supply_intel` — find a rival's stockpile
- `quest_hand_defense_intel` — map a rival's defenses
- `quest_hand_communications_intel` — steal a cipher key
- `quest_hand_exfiltration` — get an agent out
- `quest_hand_rescue_support` — support a rescue from inside

**Counterintelligence (5)**
- `quest_hand_vet_candidate` — vet a new arrival
- `quest_hand_suspicion_board` — build the evidence wall
- `quest_hand_false_positive` — clear an innocent
- `quest_hand_sabotage_trace` — trace a sabotage incident
- `quest_hand_pattern_check` — find a pattern in small data

**Interrogation and prisoners (5)**
- `quest_hand_first_interview` — a non-coercive interview
- `quest_hand_terms_of_holding` — write the prisoner terms
- `quest_hand_medicine_for_captives` — medical care for prisoners
- `quest_hand_exchange_talk` — negotiate a trade
- `quest_hand_escape_attempt` — handle a breakout

**Defectors and bounty (5)**
- `quest_hand_defector_vetting` — test a defector's story
- `quest_hand_defector_contact` — protect a defector's family
- `quest_hand_bounty_notice` — identify the client
- `quest_hand_hunter_parley` — talk to a hunter
- `quest_hand_counter_bounty` — post a counter-contract

### 6.3 Repeatable quests (8)

`quest_hand_repeat_patrol`, `quest_hand_repeat_report`, `quest_hand_repeat_watch`,
`quest_hand_repeat_interview`, `quest_hand_repeat_drop`,
`quest_hand_repeat_vet`, `quest_hand_repeat_courier`,
`quest_hand_repeat_prisoner_check`.

### 6.4 Dynamic hooks

Live systems emit suspicion, exposure, sabotage, interrogation, defector, and bounty
events. The generator attaches authored follow-ups without a new event bus.

### 6.5 Constraints

- No torture mechanic. Coercion exists as a bounded, penalized policy choice.
- No operation may grant perfect information for free.
- No prisoner content may dehumanize; terms and dignity are explicit.
- No intelligence may bypass `FactionStanceEngine` for its consequences.
- No bounty content may target real-world groups or individuals.
- No espionage content may use real agency names or tradecraft terms.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `InformantNetworkSystem` (new, `Ashfall.Core.Factions`)

**Owns:** informant records, motives, payments, reliability, and exposure risk.
**Consumes:** `Inventory`, `FactionIntelligenceCatalog`, `CounterIntelligenceSystem`.
**Data:** `informants.json`.
**Rules:** an informant has a motive that can change; payment is real; a source can
be exposed and lost; reliability is authored and degrades if mistreated.

### 7.2 `DeadDropSystem` (new, `Ashfall.Core.Factions`)

**Owns:** drops, signals, schedules, interception, and burn. **Consumes:**
`ShelterEspionageSystem` dead-drop state, map locations, `CounterIntelligenceSystem`.
**Data:** `dead_drops.json`.
**Rules:** drops can be watched, poisoned, substituted, or burned; interception is
deterministic given observation.

### 7.3 `CovertOperationSystem` (extend `EspionageSystem`)

**Owns:** authored operations, planning requirements, risk bands, and consequence
routing. **Consumes:** `EspionageSystem` mission lifecycle, `EspionageConsequenceRouter`,
`Inventory`, `GuiltInsomniaSystem`. **Data:** `covert_operations.json`,
`cover_identities.json`.
**Rules:** operations are planned, not rolled instantaneously; failure is a typed
consequence; exposure changes standing and can trigger retaliation.

### 7.4 `InterrogationSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** interviews, methods, reliability, time limits, and consequences.
**Consumes:** `CaptiveInterrogationCatalog`, `CounterIntelligenceSystem`,
`GuiltInsomniaSystem`, `NeedsSystem`. **Data:** `interrogation_tactics.json`
(extended).
**Rules:** ethical interviews produce reliable but partial information; coercion
produces unreliable information and permanent guilt; there is no unlimited
interrogation — captives degrade.

### 7.5 `PrisonerTermsSystem` (extend prisoner systems)

**Owns:** terms of holding, labor, medicine, visitors, exchange, release, and escape.
**Consumes:** `ShelterPrisonerSystem`, `PrisonerSystem`, `NeedsSystem`,
`MedicalPipelineCoordinator`. **Data:** `prisoner_terms.json`.
**Rules:** prisoners consume resources and create obligations; mistreatment changes
standing and morale; execution is grave and irreversible.

### 7.6 `DefectorSystem` (new, `Ashfall.Core.Factions`)

**Owns:** defector vetting, integration, loyalty, and hidden agendas.
**Consumes:** `CounterIntelligenceSystem` defector state, `InfiltratorCatalog`,
`NeedsSystem`, `MoraleContagionSystem`. **Data:** `defector_dossiers.json`.
**Rules:** a defector may be genuine, a plant, or a person in between; vetting takes
time; integration is social.

### 7.7 `BountyEscalationSystem` (extend `FactionBountySystem`)

**Owns:** hunter profiles, contract escalation, protection, counter-bounties, and
resolution. **Consumes:** `FactionBountySystem`, `FactionStanceEngine`,
`Combat`/encounter authority for any confrontation. **Data:** `hunter_profiles.json`.
**Rules:** hunters prefer ambush and leverage; confrontation routes through the
existing combat and encounter authorities; no bounty resolves as a cinematic duel.

### 7.8 `CounterIntelPatternSystem` (new, thin, `Ashfall.Core.Factions`)

**Owns:** cross-referencing small facts into patterns, and the false-positive rate.
**Consumes:** `CounterIntelligenceSystem` suspicion evidence, `RumorSystem`.
**Data:** `counterintel_patterns.json`.
**Rules:** patterns are probabilistic and can be wrong; the system never accuses
directly; it produces leads for the player.

### 7.9 Systems explicitly not added

- No second espionage, counterintelligence, prisoner, bounty, or faction system.
- No torture loop, no interrogation minigame with optimal coercion.
- No black-market trade legs (XP-04 authority) and no contraband heat/fence (existing).
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `espionage_missions.json` (extend)

Existing schema preserved and expanded with authored mission rows across all eight
mission types and statuses.

### 8.2 `infiltrator_profiles.json` (extend)

Infiltrator archetypes: motive, cover, skill band, suspicion profile, and turning
likelihood.

### 8.3 `faction_intelligence.json` (extend)

Intelligence items with subject, confidence, shelf life, and consequence hooks.

### 8.4 `interrogation_tactics.json` (extend)

Tactic rows: pressure class, reliability, time cost, guilt cost, legal status.

### 8.5 `captive_interrogations.json` (extend)

Captive scenarios: what they know, what they will trade, what breaks them.

### 8.6 `bounty_board.json` (extend)

Contracts: target class, reward, client motive, escalation, resolution.

### 8.7 `informants.json` (new)

```json
{
  "schema_version": 1,
  "informants": [
    {
      "informant_id": "informant_pell_wick",
      "display_name": "Pell Wick",
      "motive": "medicine",
      "reliability_permille": 700,
      "access_band": "trade_route",
      "payment_profile": "goods",
      "exposure_risk": 0.35,
      "loyalty_decay_per_day": 2,
      "tags": ["trader", "black_market_adjacent", "needs_medicine"]
    }
  ]
}
```

### 8.8 `dead_drops.json` (new)

Drop rows: location, signal set, schedule, capacity, interception risk, burn rules.

### 8.9 `cover_identities.json` (new)

Cover rows: name, legend, supporting documents, burn rules, expiration.

### 8.10 `covert_operations.json` (new)

Operation rows: target, mission type, prerequisites, risk bands, consequence set.

### 8.11 `defector_dossiers.json` (new)

Defector rows: origin, claimed knowledge, true knowledge, hidden agenda, integration
difficulty, family risk.

### 8.12 `hunter_profiles.json` (new)

Hunter rows: method, crew, price, escalation, leverage preference, parley chance.

### 8.13 `counterintel_patterns.json` (new)

Pattern rows: facts, threshold, error rate, lead type.

### 8.14 `prisoner_terms.json` (new)

Term sets: labor, medicine, visitors, exchange value, release conditions, breach
consequences.

### 8.15 Items

New items: `item_cipher_pad`, `item_chalk_mark`, `item_false_papers`,
`item_dead_drop_tube`, `item_listening_cone`, `item_dossier_file`,
`item_evidence_bag`, `item_prisoner_meal`, `item_interrogation_log`,
`item_bounty_notice`, `item_counter_bounty_seal`, `item_commission_paper`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Existing stores: `EspionageSaveStore`, `CounterIntelligenceSaveStore`,
`PrisonerSaveStore`, `ShelterPrisonerSaveStore`, `ShelterEspionageSaveStore`,
`ContrabandStashSaveStore`. New sub-objects are additive inside these envelopes.

### 9.2 State to persist

- Informant records, payments, reliability, exposure.
- Dead drops, signals, and burn state.
- Cover identities and burn status.
- Covert operations and consequences.
- Interrogation sessions and outcomes.
- Prisoner terms, health, visitors, escape attempts.
- Defector dossiers and loyalty.
- Bounty contracts, hunters, and escalation.
- Counterintel patterns and leads.

### 9.3 Determinism

- Suspicion, exposure, interrogation reliability, defector loyalty, and hunter
  actions use the host-forked `ISeededRng`.
- Patterns are pure arithmetic on evidence.
- No wall clock; no `System.Random`.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with no informants, drops, covers, operations, sessions, dossiers,
hunters, or patterns. Existing espionage, counterintel, prisoner, and contraband
state is untouched.

### 9.5 Checksum

Invariant-culture floats; integer-permille for reliability, suspicion, and exposure.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `EspionagePanel` (extend) | Missions, statuses, confidence | `QuietHandHostSession` |
| `InformantPanel` (new) | Sources, payment, reliability, exposure | same |
| `DeadDropPanel` (new) | Drops, signals, stakeouts, burns | same |
| `CounterIntelPanel` (extend) | Suspicions, evidence, vetting, leads | same |
| `InterrogationPanel` (new) | Sessions, methods, reliability, limits | same |
| `PrisonerPanel` (new) | Terms, health, visitors, exchange, release | same |
| `DefectorPanel` (new) | Dossiers, vetting, integration, loyalty | same |
| `BountyPanel` (new) | Contracts, hunters, protection, counter-bounty | same |
| `QuietHandRulePanel` (new) | The shelter's codified interrogation/prisoner rule | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Confidence bands and sources are shown plainly; the player knows what they know.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Coercion and execution require explicit confirmation with stated guilt and
  standing consequences.
- Prisoner care status is always visible; a neglected prisoner is never hidden.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: radio static, a cell door, chalk on stone,
a cipher key click, a long silence, a courier's step. No cue is required; text
carries meaning. Interrogation has no dramatic sting; it has a clock and a pad.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `EspionageSystem` | Mission content and lifecycle extended |
| `CounterIntelligenceSystem` | Vetting, suspicion, detention consumed |
| `ShelterEspionageSystem` | Dead drops and sabotage extended |
| `InfiltratorCatalog` | Profiles extended |
| `FactionIntelligenceCatalog` | Intelligence items extended |
| `CaptiveInterrogationCatalog` | Interrogation content extended |
| `ShelterPrisonerSystem` / `PrisonerSystem` | Terms and care extended |
| `FactionBountySystem` | Hunters and escalation extended |
| `EspionageConsequenceRouter` | Consequence routing |
| `FactionStanceEngine` | All standing consequences |
| `NeedsSystem` / `MoraleContagionSystem` | Fear and trust |
| `GuiltInsomniaSystem` | Coercion and execution guilt |
| `RumorSystem` | Leaks, gossip, patterns |
| `RadioTuner` | Intercept and signals |
| `Inventory` | Documents, kits, evidence |
| `MedicalPipelineCoordinator` | Prisoner care |
| `MemorialSystem` | Executions and deaths |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `EspionageSystem`, `CounterIntelligenceSystem`,
`ShelterEspionageSystem`, `InfiltratorCatalog`, `FactionIntelligenceCatalog`,
`CaptiveInterrogationCatalog`, prisoner systems, `FactionBountySystem`, save stores,
and panels. Record file:line; change nothing.

**Phase 1 — Data + validators.** Extend missions, profiles, intelligence, tactics,
captives, bounties; author informants, drops, covers, operations, defectors, hunters,
patterns, terms. Register validators and scanner.

**Phase 2 — Pure Core.** `InformantNetworkSystem`, `DeadDropSystem`,
`CovertOperationSystem`, `InterrogationSystem`, `PrisonerTermsSystem`,
`DefectorSystem`, `BountyEscalationSystem`, `CounterIntelPatternSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `QuietHandHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 90/180-day soak including mole hunts and bounty escalation.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Missions | 40 |
| Infiltrator profiles | 20 |
| Intelligence items | 30 |
| Interrogation tactics | 20 |
| Captive scenarios | 20 |
| Bounty contracts | 30 |
| Informants | 20 |
| Dead drops | 15 |
| Cover identities | 12 |
| Covert operations | 30 |
| Defector dossiers | 15 |
| Hunter profiles | 12 |
| Patterns | 15 |
| Prisoner terms | 8 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Items | 12 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Torture becomes optimal | Critical | Reliability penalty, guilt, standing |
| Surveillance destroys morale loop | High | Trust cost and false positives |
| Second espionage system | High | Extend live owners only |
| Overlap with XP-04 trade legs | High | Trade content excluded |
| Prisoners dehumanized | High | Terms, dignity, explicit care UI |
| Determinism break | Low | Host-forked RNG |
| Content overrun | Medium | Budget §23 |
| Spy-thriller tone drift | Medium | Tone table enforced |

---

## 13. TEST AND VERIFICATION PLAN

### 13.1 New test files

- `Ashfall.Core.Tests/Factions/InformantNetworkTests.cs`
- `Ashfall.Core.Tests/Factions/DeadDropTests.cs`
- `Ashfall.Core.Tests/Factions/CovertOperationTests.cs`
- `Ashfall.Core.Tests/Shelter/InterrogationSystemTests.cs`
- `Ashfall.Core.Tests/Factions/PrisonerTermsTests.cs`
- `Ashfall.Core.Tests/Factions/DefectorSystemTests.cs`
- `Ashfall.Core.Tests/Factions/BountyEscalationTests.cs`
- `Ashfall.Core.Tests/Factions/QuietHandSaveRoundTripTests.cs`
- `Ashfall.Core.Tests/Factions/QuietHandDeterminismTests.cs`
- `Ashfall.Core.Tests/Content/QuietHandCatalogIntegrityTests.cs`

### 13.2 Required assertions

- Informants have real payments, reliability, and exposure.
- Dead drops can be watched, substituted, and burned.
- Missions cannot grant perfect information.
- Ethical interrogation is reliable but partial; coercion is unreliable and costly.
- Prisoners consume resources, receive care, and have real terms.
- Defectors can be genuine or planted; vetting takes time.
- Bounties escalate; confrontation uses existing combat/encounter authorities.
- Patterns produce leads, never accusations.
- No torture mechanic grants a reliable advantage.
- Round-trip restores informants, drops, operations, sessions, prisoners, hunters.
- Legacy loads neutral; paired replay hash equality.

### 13.3 Commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Factions/
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/
godot --headless --path . -- --espionage-selftest
godot --headless --path . -- --counter-intelligence-selftest
godot --headless --path . -- --prisoner-selftest
godot --headless --path . -- --bounty-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
dotnet build Ashfall.csproj --no-restore
```

---

## 14. ACCEPTANCE CRITERIA

Core authority documented and engine-free; data canonical with valid schema and
passing integrity; persistence round-trips with neutral legacy and Triad parity;
determinism proven; host reachable by a real route/event; player can observe the
outcome; focused tests green; docs updated; ethical-content audit passed.
Compile-green is not acceptance.

---

## 15. CROSS-EXPANSION HOOKS

| Expansion | Hook |
|---|---|
| 12 The Second Generation | Child informants; protecting families |
| 13 The Faithful | Belief-based loyalty tests; cult infiltration |
| 14 Above the Ash | Aerial courier intercepts; spy flights |
| 15 The Deep Root | Smuggled seed; agricultural espionage |
| 16 The Rebuilt Body | Coerced implants; signal surveillance |
| 17 The Long Evening | Rumor as intelligence; public cover |
| 18 The Underneath | Hidden deep bunkers; sealed archives |
| 19 The Bitter Air | Stealing a strain; vault access |
| 21 The Grid | Sabotaging power; grid intelligence |

---

## 16. LORE AND CONTINUITY CHECK

### 16.1 Must not contradict

- The live mission types, statuses, and double-agent states.
- Bounded suspicion and evidence-based counterintelligence.
- The existing prisoner and bounty owners.
- The XP-04 black-market trade boundary.
- The fiction-only and no-torture rules.

### 16.2 New canon

- The sleeper, the dead drop, and the Paper Gate.
- The Wire Yard and prisoner exchange.
- The codified interrogation rule as a possible campaign achievement.
- The Quiet War of information between settlements.

### 16.3 Provenance

Registered in `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` when integrated.

---

## 17. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `espionage_missions.json` | 40 | 8,000 |
| `infiltrator_profiles.json` | 20 | 4,000 |
| `faction_intelligence.json` | 30 | 5,000 |
| `interrogation_tactics.json` | 20 | 3,500 |
| `captive_interrogations.json` | 20 | 4,000 |
| `bounty_board.json` | 30 | 5,000 |
| `informants.json` | 20 | 4,000 |
| `dead_drops.json` | 15 | 2,500 |
| `cover_identities.json` | 12 | 2,500 |
| `covert_operations.json` | 30 | 6,000 |
| `defector_dossiers.json` | 15 | 4,000 |
| `hunter_profiles.json` | 12 | 2,500 |
| `counterintel_patterns.json` | 15 | 2,500 |
| `prisoner_terms.json` | 8 | 2,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 12 | 1,800 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~85,800** |trim to the 60–75k target during authoring.

---

## 18. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R20-1 | Torture becomes optimal | Med | Critical | Reliability penalty, guilt, standing |
| R20-2 | Surveillance kills morale | High | High | Trust cost, false positives |
| R20-3 | Second espionage system | Low | High | Extend owners only |
| R20-4 | XP-04 overlap | Med | High | Trade content excluded |
| R20-5 | Prisoner dehumanization | Med | High | Terms and care UI |
| R20-6 | Determinism | Low | High | Host-forked RNG |
| R20-7 | Content overrun | Med | Med | Budget §17 |
| R20-8 | Spy-thriller tone | Med | Med | Tone table |
| R20-9 | False accusation unfair | Med | Med | Evidence and clearing path |
| R20-10 | Bounty escalation unbalanced | Med | Med | Authored bands |

---

## 19. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Does the shelter have a formal interrogation rule?** Recommended: optional, but
   codifying it becomes a campaign achievement and a morale stabilizer.
2. **Can prisoners work?** Recommended: yes, by terms, with dignity and health
   effects, never as free labor.
3. **Can an innocent be executed?** Recommended: yes — and it should be one of the
   most damaging outcomes in the game, with no clean recovery.
4. **Do informants have to be paid materially?** Recommended: yes, from inventory or
   the funds authority; coercion is possible and corrosive.
5. **Can the player refuse the quiet war entirely?** Recommended: yes; openness is a
   real strategy with real risk.

---

## 21. APPENDIX D — MISSION TYPE TABLE (40 MISSIONS)

| # | Mission | Type | Target | Risk band | Consequence set |
|---|---|---|---|---|---|
| 1 | Patrol Schedule Theft | Infiltration | rival patrols | med | raid timing |
| 2 | Storehouse Count | Supply Intelligence | rival stocks | low | trade leverage |
| 3 | Gate Watch | Surveillance | border post | low | entry windows |
| 4 | Wire Tap | Communications | radio post | high | cipher hints |
| 5 | Officer Dossier | Infiltration | command staff | high | leverage |
| 6 | Ration Ledger Copy | Supply Intelligence | depot | med | famine intel |
| 7 | Defense Walk | Defense Intelligence | walls | med | assault plan |
| 8 | Prisoner Location | Surveillance | Wire Yard | med | rescue option |
| 9 | Cipher Key Copy | Communications | signals room | high | decode |
| 10 | Dead Drop Plant | Infiltration | alley | med | channel |
| 11 | Courier Intercept | Communications | message road | med | mail |
| 12 | Water Source Map | Supply Intelligence | aquifer | med | interdiction |
| 13 | Armory Count | Defense Intelligence | armory | high | capability |
| 14 | Sleeper Contact | Infiltration | sleeper | very high | activation |
| 15 | Sabotage Watch | Surveillance | pumps | high | prevention |
| 16 | Defector Extraction | Exfiltration | defector | very high | asset |
| 17 | Family Rescue | Rescue Support | family | very high | loyalty |
| 18 | Sabotage Bridge | Abstract Sabotage | crossing | very high | delay |
| 19 | Sabotage Radio | Abstract Sabotage | relay | high | silence |
| 20 | False Report Plant | Infiltration | rival intel | high | misdirection |
| 21 | Recruit Handler | Infiltration | insider | high | network |
| 22 | Meeting Cover | Surveillance | safe house | med | safety |
| 23 | Bounty Client Trace | Infiltration | market | high | client |
| 24 | Hunter Scout | Surveillance | long camp | high | warning |
| 25 | Prisoner Exchange Watch | Surveillance | wire yard | med | terms |
| 26 | Fuel Dump Map | Supply Intelligence | tank farm | med | logistics |
| 27 | Minefield Chart | Defense Intelligence | approach | high | route |
| 28 | Number Station Listen | Communications | radio hill | med | cipher |
| 29 | Papers Forgery Run | Infiltration | paper gate | med | entry |
| 30 | Sentry Rotation | Surveillance | walls | low | pattern |
| 31 | Officer Bribe | Infiltration | quartermaster | high | access |
| 32 | Refinery Sabotage | Abstract Sabotage | works | very high | production |
| 33 | Double Agent Run | Infiltration | turned agent | very high | truth |
| 34 | Medical Supply Theft | Supply Intelligence | clinic | med | care |
| 35 | Signal Jam Test | Communications | relay | med | blackout |
| 36 | Archive Copy | Infiltration | registry | high | identity |
| 37 | Border Pass Forgery | Infiltration | gate | med | transit |
| 38 | Hostage Rescue | Rescue Support | holding cell | very high | life |
| 39 | Counter-Bounty Notice | Infiltration | market board | med | deterrence |
| 40 | The Long Watch | Surveillance | all | low | pattern |

Every mission is authored with prerequisites, a planning phase, risk bands, and a
consequence set. None grants perfect information, and all failures route through
`EspionageConsequenceRouter`.

---

## 22. APPENDIX E — INFILTRATOR PROFILE TABLE (20)

| # | Profile | Motive | Cover | Skill | Suspicion | Turn chance |
|---|---|---|---|---|---|---|
| 1 | Debtor | debt | trader | med | med | high |
| 2 | Coerced Parent | family | laborer | low | low | high |
| 3 | True Believer | ideology | clerk | med | low | very low |
| 4 | Mercenary | payment | guard | high | med | med |
| 5 | Exile | survival | refugee | low | low | med |
| 6 | Lover | attachment | medic | med | low | high |
| 7 | Professional | contract | courier | high | very low | very low |
| 8 | Thief | opportunity | scavenger | med | high | med |
| 9 | Scholar | curiosity | teacher | med | low | high |
| 10 | Drunk | escape | cook | low | high | high |
| 11 | Patient | revenge | nurse | med | very low | very low |
| 12 | Child | coercion | errand | low | low | high |
| 13 | Veteran | duty | watch | high | med | low |
| 14 | Spy by Marriage | loyalty | spouse | med | very low | med |
| 15 | Fence | profit | trader | med | med | med |
| 16 | Clerk Mole | routine | records | low | very low | med |
| 17 | Engineer Plant | sabotage | mechanic | high | low | very low |
| 18 | Radio Operator | signals | operator | high | med | low |
| 19 | Prisoner Turned | survival | laborer | med | low | med |
| 20 | The Unwitting | none | any | none | none | n/a |

Profiles are authored, not generated. The Unwitting is essential: some suspects are
not agents at all, and the shelter must be able to clear them.

---

## 23. APPENDIX F — INFORMANT TABLE (20)

| # | Informant | Motive | Access | Reliability | Payment | Exposure |
|---|---|---|---|---|---|---|
| 1 | Pell Wick | medicine | trade route | 0.70 | goods | 0.35 |
| 2 | Courier Vey | coin | roads | 0.60 | funds | 0.40 |
| 3 | Guard Osk | revenge | walls | 0.55 | favor | 0.50 |
| 4 | Cook Mira | safety | camp | 0.50 | food | 0.30 |
| 5 | Clerk Hal | debt | records | 0.75 | coin | 0.25 |
| 6 | Medic Rout | family | clinic | 0.65 | medicine | 0.35 |
| 7 | Child Pip | sweets | camp | 0.45 | food | 0.20 |
| 8 | Fence Lor | profit | market | 0.60 | goods | 0.45 |
| 9 | Driver Tarn | fuel | road | 0.55 | fuel | 0.40 |
| 10 | Nurse Ista | mercy | ward | 0.70 | supplies | 0.30 |
| 11 | Radio Osk | fear | signal | 0.50 | protection | 0.55 |
| 12 | Farmer Yen | water | fields | 0.65 | water | 0.25 |
| 13 | Hunter Bek | coin | wilds | 0.60 | coin | 0.35 |
| 14 | Widow Sarn | revenge | town | 0.75 | favor | 0.20 |
| 15 | Brewer Ott | debt | still | 0.55 | grain | 0.40 |
| 16 | Teacher Lume | ideology | school | 0.70 | books | 0.25 |
| 17 | Miner Kade | pay | mine | 0.65 | tools | 0.35 |
| 18 | Sailor Nee | passage | river | 0.50 | fuel | 0.45 |
| 19 | Beggar Mos | food | gate | 0.40 | food | 0.15 |
| 20 | Double Vess | both | both | 0.30 | both | 0.80 |

Motive, access, reliability, payment, and exposure are authored per row. Mistreatment
lowers reliability; abandonment raises exposure risk.

---

## 24. APPENDIX G — INTERROGATION TACTIC TABLE

| Tactic | Pressure | Reliability | Time | Guilt | Status |
|---|---|---|---|---|---|
| Open Interview | none | 0.85 | 1 day | 0 | ethical |
| Document Confrontation | low | 0.90 | 1 day | 0 | ethical |
| Witness Contradiction | low | 0.80 | 2 days | 0 | ethical |
| Incentive Offer | low | 0.75 | 1 day | low | ethical |
| Isolation | med | 0.60 | 3 days | med | restricted |
| Sleep Pressure | med | 0.50 | 2 days | med | restricted |
| Threat Display | high | 0.35 | 1 day | high | prohibited |
| Physical Coercion | high | 0.20 | 1 day | very high | prohibited |
| Family Pressure | high | 0.30 | 2 days | very high | prohibited |
| Drugged Questioning | high | 0.25 | 1 day | high | prohibited |
| Long Silence | none | 0.70 | 3 days | 0 | ethical |
| Peer Interview | none | 0.65 | 2 days | 0 | ethical |
| Trade for Truth | low | 0.70 | 2 days | low | ethical |
| Repetition Test | low | 0.75 | 3 days | 0 | ethical |
| Cross-Reference | low | 0.80 | 2 days | 0 | ethical |
| Prisoner Testimony | low | 0.60 | 3 days | 0 | ethical |
| Hostile Confrontation | med | 0.45 | 1 day | med | restricted |
| Isolation with Care | med | 0.55 | 4 days | low | restricted |
| Mediated Apology | none | 0.50 | 3 days | 0 | ethical |
| The Long Account | none | 0.75 | 5 days | 0 | ethical |

Prohibited tactics exist in the data so the player can choose them and live with the
consequences. They never produce reliable intelligence, they always produce guilt,
and they damage standing when discovered. This is a hard design rule.

---

## 25. APPENDIX H — PRISONER TERMS TABLE

| Terms | Labor | Medicine | Visitors | Exchange | Release | Breach |
|---|---|---|---|---|---|---|
| Standard | light | full | weekly | fair | served | warning |
| Restrictive | none | full | rare | high | served | isolation |
| Labor Camp | heavy | basic | none | low | served | punishment |
| Medical | none | full | daily | highest | immediate | grave |
| Exchange Only | none | basic | none | exact | on trade | grave |
| Release Pending | none | full | weekly | low | soon | warning |
| Indefinite | light | basic | rare | none | none | erosion |
| The Rule | light | full | weekly | fair | served | public log |

Prisoners consume food, water, medicine, and space. Each term set changes standing,
captive health, morale, and the shelter's identity. The Rule is the codified version
the player can adopt as a campaign achievement.

---

## 26. APPENDIX I — BOUNTY AND HUNTER TABLE

| # | Hunter | Method | Crew | Price | Leverage | Parley |
|---|---|---|---|---|---|---|
| 1 | Vesna Rye | ambush | 3 | high | contract | yes |
| 2 | The Twins | poison | 2 | med | stealth | no |
| 3 | Old Marrow | siege | 8 | low | numbers | yes |
| 4 | Silent Nee | infiltration | 1 | high | sleep | no |
| 5 | The Broker | proxies | 0 | high | money | yes |
| 6 | Ash Hounds | tracking | 5 | med | pursuit | no |
| 7 | Loom | pressure | 4 | med | family | yes |
| 8 | Carver | terror | 2 | low | fear | no |
| 9 | The Patient | waiting | 1 | high | time | yes |
| 10 | Debt Collector | legal | 3 | low | paper | yes |
| 11 | The Walker | direct | 1 | med | skill | no |
| 12 | Company Men | organized | 12 | high | resources | yes |

Hunters prefer leverage, ambush, and pressure. Direct confrontation routes through
the existing combat and encounter authorities; the expansion never stages a duel.

---

## 27. APPENDIX J — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_hand_failed_raid` | 3 | Aftermath, casualties, and the leak |
| `quest_hand_quiet_investigation` | 5 | Vet, watch, and build the evidence board |
| `quest_hand_first_suspicion` | 4 | Flag an innocent; clear or condemn |
| `quest_hand_the_informant` | 4 | Recruit Pell Wick; set payment |
| `quest_hand_dead_drop` | 4 | Watch the chalk alley; identify the handler |
| `quest_hand_the_bounty` | 4 | Intercept the mail; identify the client |
| `quest_hand_the_skin` | 3 | Identify the mole without proof |
| `quest_hand_the_capture` | 5 | Catch a second agent; begin questioning |
| `quest_hand_the_limit` | 4 | Debate, write, or break the interrogation rule |
| `quest_hand_the_defector` | 5 | Vet Ana Ved; test her story |
| `quest_hand_the_exchange` | 4 | Negotiate a prisoner trade |
| `quest_hand_the_hunter` | 5 | Meet Vesna Rye; hide, counter, or confront |
| `quest_hand_the_reckoning` | 5 | Expose the mole; repair the false accusations |
| `quest_hand_the_cost` | 4 | Trust, morale, guilt, and the shelter's identity |
| `quest_hand_quiet_after` | 3 | Final disposition; epilogue |

---

## 28. APPENDIX K — NPC DOSSIERS (BRIEF)

**Ilyana Krest** — security chief. Competent, driven, and increasingly unable to
separate vigilance from suspicion. She is not a villain; she is a person who has
been right often enough to stop checking herself. The expansion's central arc is
whether she can be brought back.

**Marek Dowd** — administrator. The bounty's target. Wants to run a shelter, not a
conspiracy. His refusal to hide is both principled and inconvenient, and the player
must decide whether to protect him against his wishes.

**Berek Saul** — intelligence handler. Runs informants and dead drops with a
bookkeeper's calm. Believes every source is a relationship, not an asset. The one
who insists on paying people properly.

**Ana Ved** — defector. Brings true names and a hidden agenda. May be genuine, may
be a plant, may be both. Vetting her is the expansion's best mystery because the
answer changes with the evidence.

**Sergei Lom** — captured agent. Refuses to speak for reasons that have nothing to
do with fanaticism. His interrogation is the expansion's moral fulcrum.

**Pell Wick** — informant. Sells information for medicine and is honest about it.
The expansion's proof that informants are people with needs, not resources.

**Warden Hask** — prison warden. Enforces terms, keeps the log, and refuses
euphemisms. Represents the difference between holding someone and breaking them.

**Vesna Rye** — bounty hunter. Professional, courteous, and entirely willing to
kill. Prefers a negotiated resolution because it is cheaper. Not a monster, which is
what makes her dangerous.

---

## 29. APPENDIX L — LOCATION DETAIL

- **The Chalk Alley** — a service lane with a loose brick and fresh marks.
- **The Listening Post** — a salvaged intercept station; dials that still turn.
- **The Halfway House** — neutral ground; no weapons past the door.
- **The Paper Gate** — identity checks, forgeries, and a desk that never sleeps.
- **The Wire Yard** — a prison camp with a logbook and a guard roster.
- **The Long Camp** — hunters' camp; cookfires and contracts.
- **The Registry** — pre-war records; the rare place truth can be verified.
- **The Message Road** — couriers, mail, and the risk of reading it.
- **The Whisper Hill** — long-range signals and a view of everything.
- **The Yard** — tradecraft training; chalk, paper, and repetitions.

---

## 30. APPENDIX M — WORKED 180-DAY INTELLIGENCE SCENARIO

**Days 1–30.** The failed raid kills two and exposes the leak. Krest vets the
shelter; three people are flagged and two are innocent. Pell Wick sells a first
true report for medicine.

**Days 31–60.** The chalk alley is watched. A dead drop is photographed and
substituted, and the substitution is detected — the rival now knows the alley is
watched. Ana Ved arrives at the Paper Gate.

**Days 61–90.** Sergei Lom is captured. Ethical interviews yield partial truth at
high confidence; the shelter debates further methods. A bounty notice names Dowd;
the client is traced to a market broker.

**Days 91–120.** The mole is identified but not proven. A false accusation ruins an
innocent's standing and morale. The prisoner exchange is negotiated; Lom's value is
weighed against what the rival will give.

**Days 121–150.** Vesna Rye offers a parley. The shelter chooses hide, counter, or
confront. A pattern emerges in small data: one person always knows first.

**Days 151–180.** The reckoning: the mole is exposed, the innocent is cleared or
lost, the rule is written or broken, and the shelter counts what its safety cost.

---

## 31. APPENDIX N — VIGNETTE (TONE SAMPLE)

> The interview room has a table, two chairs, a lamp, and a clock that someone
> wound too tightly. Sergei Lom sits with his hands flat and does not answer the
> question. He has not answered it for four days. On the wall behind him, someone
> has painted a stripe at eye level, which is not for him; it is a reminder for the
> interviewer about where the line is.
>
> Pell Wick waits in the corridor with a paper packet of medicine and a list of what
> he saw. He does not ask for more than the medicine. Berek counts it out anyway and
> adds a tin, because a source is a relationship and the tin is cheaper than the
> next report.
>
> At the west wall, a guard wipes a chalk mark off the bricks with his sleeve and
> replaces it with nothing, and does not know if that is the correct answer.

---

## 32. APPENDIX O — CONTENT REVIEW CHECKLIST

- [ ] No torture tactic produces reliable intelligence or a net advantage.
- [ ] Coercion always carries guilt and standing consequences.
- [ ] No real-world agency, program, or tradecraft term is used.
- [ ] Prisoners have terms, care, visitors, and dignity.
- [ ] False accusations have a clearing path and lasting cost.
- [ ] Intelligence never arrives at perfect confidence for free.
- [ ] Trade/black-market crime stays with the XP-04 and contraband authorities.
- [ ] All standing routes through `FactionStanceEngine`.
- [ ] All morale routes through existing owners.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses the host-forked RNG only.

---

## 33. APPENDIX P — GLOSSARY

- **Sleeper** — a long-embedded agent awaiting activation.
- **Dead drop** — a physical exchange point with signals.
- **Cover** — an identity and its supporting documents.
- **Confidence band** — how certain the shelter is about intelligence.
- **Handler** — the person who runs informants and drops.
- **Defector** — an outsider seeking shelter in exchange for information.
- **Terms** — the codified obligations of holding a prisoner.
- **Pattern** — a lead derived from cross-referenced facts; never an accusation.
- **Burn** — to deliberately expose and discard an asset or location.
- **The Rule** — a codified interrogation-and-prisoner standard.

---

## 34. APPENDIX Q — DATA SCHEMA DETAIL (NEW CATALOGS)

**`informants.json`** — `informant_id`, `display_name`, `motive`, `access_band`,
`reliability_permille`, `payment_profile`, `exposure_risk`,
`loyalty_decay_per_day`, `tags`.

**`dead_drops.json`** — `drop_id`, `location_id`, `signal_set[]`, `schedule`,
`capacity`, `interception_risk`, `burn_rules`, `tags`.

**`cover_identities.json`** — `cover_id`, `display_name`, `legend`,
`supporting_documents[]`, `burn_rules`, `expires_day`, `tags`.

**`covert_operations.json`** — `operation_id`, `display_name`, `mission_type`,
`target_id`, `prerequisites[]`, `risk_band`, `consequence_set`, `tags`.

**`defector_dossiers.json`** — `dossier_id`, `display_name`, `origin`,
`claimed_knowledge[]`, `true_knowledge[]`, `hidden_agenda`, `integration_difficulty`,
`family_risk`, `tags`.

**`hunter_profiles.json`** — `hunter_id`, `display_name`, `method`, `crew_size`,
`price_band`, `leverage_preference`, `parley_chance`, `escalation`, `tags`.

**`counterintel_patterns.json`** — `pattern_id`, `display_name`, `facts[]`,
`threshold`, `error_rate`, `lead_type`, `tags`.

**`prisoner_terms.json`** — `terms_id`, `display_name`, `labor`, `medicine`,
`visitors`, `exchange_value`, `release_condition`, `breach_consequence`, `tags`.

**`interrogation_tactics.json`** (extended) — adds `pressure_class`, `reliability`,
`time_cost`, `guilt_cost`, `legal_status`, `reliability_decay`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing or
duplicate IDs, invalid location/target references, or out-of-range numbers.

---

## 35. APPENDIX R — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `EspionageSystem` | mission state | mission state | standing |
| `CounterIntelligenceSystem` | suspicion | suspicion | morale |
| `ShelterEspionageSystem` | drops | drops | faction state |
| `InfiltratorCatalog` | profiles | — | — |
| `FactionIntelligenceCatalog` | intel items | — | — |
| `CaptiveInterrogationCatalog` | scenarios | — | — |
| `PrisonerSystem` | terms | prisoner state | needs |
| `FactionBountySystem` | contracts | bounty state | combat |
| `FactionStanceEngine` | consequences | standing | — |
| `NeedsSystem` | pressure | morale | — |
| `MoraleContagionSystem` | fear | contagion | — |
| `GuiltInsomniaSystem` | coercion | guilt | — |
| `RumorSystem` | facts | rumors | — |
| `RadioTuner` | intercepts | — | — |
| `Inventory` | documents | transfers | — |
| `MedicalPipelineCoordinator` | care | treatment | — |
| `MemorialSystem` | deaths | memorials | — |

---

## 36. APPENDIX S — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Missions per campaign | pacing | EspionageSystem |
| Informants recruited/lost | network health | InformantNetworkSystem |
| Drops burned | tradecraft pressure | DeadDropSystem |
| False accusations | cost of suspicion | CounterIntelPatternSystem |
| Interrogation methods used | ethics drift | InterrogationSystem |
| Prisoner days | obligation load | PrisonerTermsSystem |
| Defectors integrated/lost | trust | DefectorSystem |
| Bounty escalations | pressure | BountyEscalationSystem |
| Confidence mix | uncertainty | intelligence catalog |

Telemetry is diagnostic only. It never gates content and never becomes a hidden
score; it exists so the team can tell whether the expansion is tense or merely
noisy.

---

## 38. APPENDIX T — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_hand_recruit_informant` | 4 | Find a source; establish motive; agree terms |
| `quest_hand_payment_terms` | 3 | Decide goods, funds, or coercion |
| `quest_hand_reliability_test` | 3 | Feed a known fact and check the report |
| `quest_hand_exposed_source` | 4 | Protect or abandon a burned informant |
| `quest_hand_double_source` | 5 | A source works both sides; decide the play |
| `quest_hand_dead_drop_setup` | 4 | Choose a location; define signals; test it |
| `quest_hand_signal_code` | 3 | Design chalk marks only two people understand |
| `quest_hand_stakeout` | 5 | Watch a drop for a week |
| `quest_hand_intercepted_mail` | 4 | Read a letter and decide whether to act |
| `quest_hand_burn_the_drop` | 3 | Destroy a compromised location cleanly |
| `quest_hand_supply_intel` | 4 | Count a rival's stores |
| `quest_hand_defense_intel` | 4 | Map walls and watches |
| `quest_hand_communications_intel` | 5 | Copy a cipher key |
| `quest_hand_exfiltration` | 5 | Get an agent out alive |
| `quest_hand_rescue_support` | 5 | Support a rescue from inside |
| `quest_hand_vet_candidate` | 4 | Vet a new arrival without accusing |
| `quest_hand_suspicion_board` | 3 | Build the evidence wall |
| `quest_hand_false_positive` | 4 | Clear an innocent; repair the damage |
| `quest_hand_sabotage_trace` | 4 | Trace a sabotage incident to a person or a fault |
| `quest_hand_pattern_check` | 3 | Find a pattern in small data |
| `quest_hand_first_interview` | 3 | Conduct a non-coercive interview |
| `quest_hand_terms_of_holding` | 4 | Write prisoner terms |
| `quest_hand_medicine_for_captives` | 4 | Provide care under pressure |
| `quest_hand_exchange_talk` | 5 | Negotiate a trade |
| `quest_hand_escape_attempt` | 4 | Handle a breakout |
| `quest_hand_defector_vetting` | 5 | Test Ana Ved's story |
| `quest_hand_defector_contact` | 4 | Protect a defector's family |
| `quest_hand_bounty_notice` | 4 | Identify the client |
| `quest_hand_hunter_parley` | 3 | Talk to Vesna Rye |
| `quest_hand_counter_bounty` | 4 | Post a counter-contract |

---

## 39. APPENDIX U — TURNING AN AGENT MODEL

Turning is a social process, not a dice roll. The model reads five inputs:

| Input | Source | Effect on turn chance |
|---|---|---|
| Motive | infiltrator profile | coerced/mercenary turn easily; believers do not |
| Treatment | interrogation terms | humane treatment raises turn chance |
| Leverage | evidence, family, debt | raises chance and guilt |
| Time | days held | slow curve; long holds raise risk |
| Confidence | intelligence band | turns produce penetration-level intel |

A turned agent is not a puppet. They can relapse, warn their handler, or feed false
intelligence. The expansion never guarantees a clean asset, and the player should
never fully trust one. This is what makes a turned sleeper the most valuable and
most dangerous thing in the game.

---

## 40. APPENDIX V — DECEPTION AND FALSE FLAGS

| Operation | Purpose | Risk | Cost |
|---|---|---|---|
| False Report | misdirect rival | exposure | credibility |
| Substituted Drop | feed wrong intel | detection | asset |
| Double Back | use a turned agent | relapse | trust |
| Forged Order | split a rival | discovery | standing |
| Phantom Unit | inflate strength | scouting | time |
| Leaked Truth | build trust before a lie | none | information |
| Burned Cover | sacrifice an identity | asset | guilt |
| Paper Trail | frame an innocent rival | discovery | guilt |

False flags always risk framing the innocent and always cost guilt. The expansion
lets the player do them and makes the shelter remember.

---

## 41. APPENDIX W — SHELTER SECURITY POSTURE

The shelter chooses a posture that governs vetting, watch, and privacy:

| Posture | Vetting | Watch | Privacy | Trust | Danger |
|---|---|---|---|---|---|
| Open | light | none | full | high | high |
| Aware | normal | gate | normal | med | med |
| Guarded | strict | internal | low | low | low |
| Locked | extreme | internal + mail | none | very low | lowest |
| The Rule | strict but public | internal by law | protected by law | med | low |

Posture is not a slider in a menu; it is a set of authored policies the player adopts
through quests. The Rule is the hardest posture to reach and the only one that
combines security with dignity, which is the expansion's intended endgame state.

---

## 42. APPENDIX X — READING AND CIPHERS

| Cipher | Source | Difficulty | Decode value |
|---|---|---|---|
| Plain Language | careless traffic | trivial | low |
| Substitution | old world habit | easy | med |
| Book Cipher | shared text | med | high |
| One-Time Pad | professional | hard | very high |
| Number Station | broadcast | hard | high |
| Chalk Code | drops | easy | med |
| Trade Slang | markets | easy | low |
| Broken Code | damaged page | variable | variable |

Decoding is a knowledge task using the live skill and research systems. It is never
a minigame, and it never reveals more than the traffic contains. A one-time pad with
a missing page is exactly as useless as it sounds, and the expansion says so.

---

## 43. APPENDIX Y — REGIONAL INTELLIGENCE MAP

The intelligence war is regional, and every settlement participates:

| Settlement | Interest | Method | Vulnerability |
|---|---|---|---|
| The shelter | survival | defense and trade | internal trust |
| Rival garrison | control | pressure, sleeper | rigid command |
| Market town | profit | information trade | no loyalty |
| Foundry enclave | production | industrial spies | closed society |
| River flotilla | mobility | couriers | exposed routes |
| Deep bunker | isolation | listening | no human intel |
| Hunter camp | contracts | leverage | no home |
| The Registry | truth | records | physical archive |

Each settlement has an authored intelligence posture and authored missions targeting
it. None is a monolith; each has internal factions the player can exploit.

---

## 44. APPENDIX Z — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Agent caught | asset lost, rival warned | extract or disown |
| Drop burned | channel lost | rebuild elsewhere |
| Informant exposed | source lost, trust cost | protect or relocate |
| False accusation | morale and standing damage | evidence, apology, restitution |
| Interrogation coercion | unreliable intel, guilt | stop, treat, disclose |
| Prisoner death | standing, guilt, morale | account, terms, memorial |
| Defector plant | sabotage, trust loss | vet, isolate, expose |
| Bounty strike | room damage, death | shore, protect, counter |
| Sabotage success | production loss | repair, trace, retaliate |
| Mole survives | ongoing leaks | reopen the investigation |

No failure is a game over. Every failure has a recovery path, and every recovery
costs something proportional. The deepest failure is the one the expansion is really
about: a shelter that becomes so afraid of its own people that it loses them.

---

## 45. APPENDIX AA — CAMPAIGN ARC TIMELINE

| Phase | Days | Theme | Decision |
|---|---|---|---|
| The leak | 1–30 | aftermath | investigation scope |
| The network | 31–60 | tradecraft | informants and drops |
| The capture | 61–90 | morality | interrogation limit |
| The defector | 91–120 | trust | vetting and integration |
| The hunters | 121–150 | pressure | hide, counter, confront |
| The reckoning | 151–180 | truth | mole and false accusation |
| The rule | 181–240 | identity | codify or leave unwritten |

The arc scales down for shorter campaigns and scales up for long ones. Each phase
changes the shelter's relationship to privacy, safety, and itself.

---

## 46. APPENDIX AB — LORE: THE PAPER WAR

Before the Exchange, the region's settlements already spied on each other. The
fiction:

- **The Registry** was a shared civil records office, maintained by no one after the
  war and consulted by everyone. Identity is the expansion's currency of truth.
- **The Message Road** was a postal and courier route; it still runs, staffed by
  people who carry sealed letters they are not supposed to read.
- **The Wire Yard** was a labor camp that became a prison camp that became an
  exchange market. Its logbooks are the region's only record of who was held.
- **The Whisper Hill** is a relay hill that still receives signals from stations
  that may or may not exist.
- **The Chalk Alley** is where the region's informants leave marks, and everyone
  knows it, and nobody admits it.

The Paper War is entirely fictional and exists to explain a regional espionage
ecology that predates the player. No real agency, program, or conflict is referenced.

---

## 47. APPENDIX AC — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the ethical-content audit.
- [ ] Phase 7 soak shows mole hunts, bounty escalation, and recovery paths.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel espionage, prisoner, bounty, or faction system exists.
- [ ] No overlap with XP-04 black-market trade legs.

---

## 48. APPENDIX AD — OPEN QUESTIONS FOR REVIEW

1. Should the shelter be able to refuse all espionage and rely on openness?
2. Should interrogation coercion ever produce a true positive by luck?
3. Should informants be recruitable from the player's own survivors?
4. Should prisoners be exchangeable for specific named assets?
5. Should the Rule be reversible once adopted?
6. Should a turned mole be able to relapse into the campaign's final act?
7. Should bounty hunters be negotiable with, or only avoidable?
8. Should counterintelligence ever produce a knowingly false lead?

None of these may be decided unilaterally; each changes the expansion's ethics.

---

## 49. APPENDIX AE — MEASUREMENT AND REVIEW CADENCE

Intelligence expansions are easy to misjudge. The review cadence should be:

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do the live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are the systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is information honest? | lifecycle + a11y tests |
| Ethics | Is anything broken or rewarding cruelty? | content audit |
| Balance | Is safety useful but costly? | soak results |

The ethical gate is not optional. This expansion touches interrogation, imprisonment,
and surveillance; the review must confirm the tone contract before integration.

---

## 50. APPENDIX AF — CLOSING VIGNETTE

> On the last night of the investigation, Ilyana Krest stands in front of the
> evidence board and takes down the cards one by one. The mole is among them,
> pinned and proven. So is the innocent she flagged first, whose card is also pinned
> because the evidence never cleared it, only failed to convict it. She leaves that
> card up longer than the others.
>
> In the yard, Marek Dowd walks to the gate and back for no reason except that he can
> and that he will not be seen to hide. In the cell block, Sergei Lom is asleep at
> last with a full meal tray beside his bunk. Pell Wick picks up his medicine at the
> door and does not stay for the tin.
>
> Somewhere on the west wall, a fresh chalk mark appears and is quickly wiped away,
> and nobody is entirely sure which hand made it, which is the most honest ending the
> quiet war has to offer.

---

## 51. CLOSING STATEMENT

ASHFALL already runs a quiet war in typed missions, bounded suspicion, prisoners, and
bounties. What it lacks is the human cost of that war: informants who need medicine,
drops that can be poisoned, interrogations that produce what you want to hear, terms
that must be kept, defectors who lie, and hunters who prefer leverage to violence.
The Quiet Hand adds that world without adding a second espionage system and without
ever making torture a tool. It adds a chalk mark, a cell door, a codified rule, and a
question every shelter eventually faces: how much of itself is it willing to give up
to feel safe?

> Wave 2 note: this plan is one of five Wave 2 expansion bibles (17–21). Each is
> self-contained; none requires another to ship. The shared Wave 2 index lives at
> `docs/expansions/wave2/WAVE2_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible.