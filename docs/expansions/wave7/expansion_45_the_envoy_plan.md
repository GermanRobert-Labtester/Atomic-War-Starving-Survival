# ASHFALL — Expansion 45 Design Bible
# THE ENVOY
### Wave 7 · Diplomacy, Summits, Treaties, Reputation, Protocol, and Public Messages

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Diplomacy` (DiplomaticSummitSystem, DiplomaticTreatyCatalog), `Ashfall.Core` (RegionalTreatySystem, RegionalTreatyCatalogLoader, RegionalTreatyFeed), `Ashfall.Core.Reputation` (ShelterReputationSystem), `Ashfall.Core.Propaganda` (PropagandaSystem), `Ashfall.Core.Economy` (TradeEmbargoSystem, FactionStanceEngine at their seams)
**Proposed host owner:** `EnvoyHostSession` (extends summit, treaty, reputation, and message surfaces)
**Existing save sections:** summit state, treaty state, reputation evidence, propaganda state
**Existing CLI verbs:** `--diplomacy-selftest` (if present), `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has diplomacy as state. `DiplomaticSummitSystem` defines
`IFactionStandingPort`, `IFactionContextPort`, `ISurvivorSkillsPort`, and
`DiplomaticSummitState` (`summit_id`, `location_id`, `framework_id`,
`convening_day`, `attending_faction_ids`, `delegate_survivor_ids`).
`DiplomaticTreatyCatalog` and `RegionalTreatySystem` with
`RegionalTreatyCatalogLoader` and `RegionalTreatyFeed` hold treaty content.
`ShelterReputationSystem` defines `ReputationDimension`, `ReputationTag`,
`InformationMedium`, and `ReputationEvidence` (`EvidenceId`, `Dimension`,
`Delta`, `Confidence`, `Salience`, `Medium`). `PropagandaSystem`,
`FactionStanceEngine`, `FactionStanceTypes`, `TradeEmbargoSystem`, and
`FactionEmbargoLedger` handle stance, access, and messaging. Data holds
`diplomatic_treaties.json` (6,846 B), `regional_treaties.json` (3,197 B),
`propaganda_campaigns.json` (4,998 B), `faction_war_location_overrides.json`
(14,411 B), and the 03 faction records.

What does not exist: envoys, credentials, missions, agendas, protocol, gifts,
seating, interpreters, ratification, reputation evidence, public messages,
embargo access rules, and the culture of meeting strangers formally. The
shelter has factions and no diplomacy.

**The Envoy** turns neighbors into relationships: envoys who travel, summits
that convene, treaties that are argued and signed, reputations built from
witnessed acts, protocol that makes meetings possible, and messages that a
community says to itself about itself. It extends the live summit, treaty,
reputation, and message owners and hands trade access to the live embargo
owner — never prices.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 03 The Standing Record | Faction facts and standing history | Reads it; records diplomatic outcomes |
| 20 The Quiet Hand | Espionage and covert action | Uses its information; never runs spies |
| 08 The Verdict | Justice and testimony | Refers crimes; never adjudicates |
| 36 The Watch (Wave 5) | Territory and patrols | Uses boundaries; never moves patrols |
| 44 The Outpost (Wave 7) | Neighbor contact at outpost scale | Owns the formal stage above it |
| Economy (blocked legs) | Prices and trade values | Never touches prices; delivers access only |
| 30 The Press (Wave 4) | Printing and publication | Uses its press for messages honestly |
| 13 The Faithful (Wave 1) | Rites and belief | Uses ceremony forms; never owns faith |
| 42 The Core (Wave 7) | Power | Offers power access; never generation |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter has been alone long enough that "neighbor" is a rumor.

**The Envoy** is the expansion about meeting other communities formally:
choosing envoys, carrying credentials, sitting at a table with strangers,
arguing a treaty line by line, giving and receiving gifts with meaning,
building a reputation out of witnessed acts, telling the truth at home about
what happened, and deciding what kind of neighbor the shelter intends to be.

### 1.2 The five loops it adds

```
  Choose ──► Travel ──► Meet ──► Agree ──► Keep
    │          │         │         │         │
    ▼          ▼         ▼         ▼         ▼
  Envoys,   Credentials Summit,  Treaties, Reputation,
  skills    and escort  protocol terms     evidence
                                        │
                                        ▼
                              Tell ──► Explain at home ──► Review
```

### 1.3 What the player manages

1. **Envoys.** Who goes, with what skills, and for how long.
2. **Credentials.** What the shelter claims and who may speak for it.
3. **Summits.** Agendas, seating, frameworks, and outcomes.
4. **Treaties.** Terms, ratification, review, and lapse.
5. **Reputation.** What others believe, based on witnessed acts.
6. **Protocol.** Gifts, forms, interpreters, and ceremonies.
7. **Access.** Roads, water, markets, and borders as agreements, not prices.
8. **Messages.** What the shelter says at home, honestly.
9. **Silence.** What the shelter refuses to say and why.
10. **Continuity.** Second meetings, envoys across years, and institutional
    memory.

### 1.4 What it is not

- Not a real-time strategy diplomacy layer. Each meeting is a scene and a
  record.
- Not a second faction, reputation, or propaganda system. It extends the live
  owners.
- Not a price-setting system; the economy's decision-blocked legs stay out.
- Not a lie machine. Propaganda content is messaging with truthfulness costs,
  never a reward for deceiving the player's own people.
- Not a conquest path. Treaties are agreements; borders are respected.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Diplomacy/DiplomaticSummitSystem.cs` | Summits and delegates | `LIVE` |
| `Assets/Ashfall.Core/Diplomacy/DiplomaticTreatyCatalog.cs` | Treaty content | `LIVE` |
| `Assets/Ashfall.Core/RegionalTreatySystem.cs` | Regional treaties | `LIVE` |
| `Assets/Ashfall.Core/RegionalTreatyFeed.cs` | Treaty feed | `LIVE` |
| `Assets/Ashfall.Core/Reputation/ShelterReputationSystem.cs` | Reputation evidence | `LIVE` |
| `Assets/Ashfall.Core/Propaganda/PropagandaSystem.cs` | Message campaigns | `LIVE` |
| `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs` | Faction stance | `LIVE` |
| `Assets/Ashfall.Core/Economy/TradeEmbargoSystem.cs` | Embargo and access | `LIVE` |
| `Assets/Ashfall.Core/StandingRecord` (Exp 03) | Faction records | `LIVE` |
| `Assets/Ashfall.Core/Needs/NeedsSystem.cs` | Delegate condition | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `diplomatic_treaties.json` | **6,846 B** | thin |
| `regional_treaties.json` | **3,197 B** | thin |
| `propaganda_campaigns.json` | **4,998 B** | thin |
| `faction_war_location_overrides.json` | 14,411 B | war context |
| `standing_record_factions.json` | 03 records | faction facts |
| Envoys, missions, protocol, gifts, credentials | absent | confirmed none |

### 2.3 Confirmed gaps

- **GAP-45-1 — No envoy or mission content.**
- **GAP-45-2 — No credentials or authority rules.**
- **GAP-45-3 — No summit agenda, seating, or framework content.**
- **GAP-45-4 — No treaty negotiation, ratification, or lapse content.**
- **GAP-45-5 — No protocol, gifts, or ceremony content.**
- **GAP-45-6 — No interpreter or language-broker content.**
- **GAP-45-7 — No reputation evidence content.**
- **GAP-45-8 — No public message content beyond thin campaigns.**
- **GAP-45-9 — No access or embargo agreement content.**
- **GAP-45-10 — No envoy reports, debriefs, or continuity content.**

### 2.4 Non-duplication statement

This expansion will **not** add a second summit, treaty, reputation,
propaganda, faction, or trade system. It extends `DiplomaticSummitSystem` with
agendas and outcomes, the treaty owners with negotiation content, the
reputation owner with evidence content, the propaganda owner with honest
messaging content, and the embargo owner with access agreements. It adds state
only as additive sub-objects of the existing diplomacy, reputation, and message
stores. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Credentials before charisma.** Who speaks for the shelter is a
written decision.

**Pillar 2 — A treaty is kept in the details.** Terms are read aloud until
everyone means the same thing.

**Pillar 3 — Reputation is evidence.** Others believe what they have seen, not
what the shelter says about itself.

**Pillar 4 — Protocol is kindness at a distance.** Forms let strangers meet
safely.

**Pillar 5 — Truth at home is part of diplomacy.** A community that lies to
itself cannot keep a promise abroad.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Envoy | Papers, patience, boots | Spies and seduction |
| Summit | Tables and arguments | Spectacle |
| Treaty | Line-by-line honesty | Fine print tricks |
| Gifts | Meaning and cost | Bribery loops |
| Protocol | Forms that help | Fussy etiquette puzzles |
| Reputation | Witnessed acts | Score shopping |
| Messages | Plain truth, carefully | Manipulation rewarded |
| Access | Agreements on paper | Price manipulation |

### 3.3 Content limits

- No bribery-as-strategy; gifts are diplomatic forms with reputational costs.
- No manipulation of the player's own people rewarded; public messages that
  lie damage the shelter's internal trust through the live systems.
- No espionage operations; information from 20 is used, never generated here.
- No ethnic, religious, or cultural slur content; factions are fictional and
  defined by interests, not identities.
- No real nations, leaders, treaties, or organizations.
- No conquest, annexation, or forced tribute.
- No price-setting; access only.

---

## 4. THE ENVOY WORLD

### 4.1 Interior rooms

- **`room_envoy_office`** — credentials, maps, and the envoy's desk.
- **`room_protocol_closet`** — gifts, cloth, and presentation.
- **`room_summit_hall`** — the table, seating, and a public gallery.
- **`room_interpreters_corner`** — two chairs and a shared dictionary.
- **`room_treaty_room`** — the signing room with a record desk.
- **`room_message_desk`** — public notices and message drafts.
- **`room_report_room`** — debriefs and the envoy archive.
- **`room_guest_quarters`** — visiting delegates, kept decent.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_road_shrine_gate` | The Meeting Gate | 3 | Formal arrival |
| `loc_summit_yard` | The Summit Yard | 3 | Open-air sessions |
| `loc_gift_garden` | The Gift Garden | 2 | Planting ceremonies |
| `loc_boundary_stone_envoy` | The Boundary Stone | 4 | Line reading |
| `loc_river_pavilion` | The River Pavilion | 2 | Neutral ground |
| `loc_lowlands_camp` | The Lowlands Camp | 4 | Host territory |
| `loc_old_bridge_table` | The Old Bridge | 3 | Crossing agreement |
| `loc_market_line` | The Market Line | 3 | Access boundary |
| `loc_message_wall` | The Message Wall | 2 | Public words |
| `loc_flag_hill` | The Flag Hill | 3 | Visibility and signal |

All locations require valid item references and scanner registration.

### 4.3 The diplomatic year

One summit per season where possible; a treaty review every year; a message to
the community when something changes; an envoy debrief after every mission.
The expansion's clock is the meeting.

---

## 5. MAIN STORYLINE — "THE TABLE ON THE BRIDGE"

### 5.1 Central conflict

An invitation arrives from the lowlands: a summit at the old bridge about water
and road access. **Sare Mel** is chosen as envoy and wants credentials that say
exactly what she may promise. **Quo** the interpreter wants a shared word list
before anyone talks about water. **Maret** the head of protocol wants gifts
that mean the right thing and a seating plan that prevents a quarrel.
**Ushka**, the lowlands delegate, wants a treaty that survives its first
winter. **Tark**, a rival envoy from the ridge, wants the crossing for his own
settlement and is willing to misquote everyone.

Then a message campaign at home turns a cautious treaty into a betrayal in the
telling, the shelter's own people lose trust in the table, and the envoys have
to explain what they actually signed.

The expansion's question: **can a shelter keep faith with strangers and with
its own people at the same time?**

### 5.2 Theme (unspoken)

**A promise is a technology for sharing a future.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_envoy_sare_mel` | Sare Mel | Envoy | Missions, promises, judgment |
| `npc_interpreter_quo` | Quo | Interpreter | Words, meaning, silence |
| `npc_head_of_protocol_maret` | Maret | Head of protocol | Gifts, seating, forms |
| `npc_scribe_lio` | Lio | Scribe | Records and exact wording |
| `npc_guard_captain_boden` | Boden | Guard captain | Safety without threat |
| `npc_delegate_ushka` | Ushka | Lowlands delegate | Counterpart and friend |
| `npc_rival_envoy_tark` | Tark | Rival envoy | Pressure and misquotation |
| `npc_keeper_of_gifts_ola` | Ola | Gift keeper | Meaning and craft |

### 5.4 Story beats (15)

1. **The Invitation.** A summit is called at the old bridge.
2. **The Credentials.** The shelter writes who may speak and what may be
   promised.
3. **The Envoy.** Sare is chosen and prepares.
4. **The Words.** Quo builds a shared word list.
5. **The Gifts.** Ola and Maret choose objects with meaning.
6. **The Road.** The journey is planned with the road and watch owners.
7. **The Table.** The first session: seating, agenda, and opening statements.
8. **The Water Line.** The hardest term: who draws and when.
9. **The Rival.** Tark misquotes the shelter and the room notices.
10. **The Draft.** Lio writes the exact words and reads them back.
11. **The Signing.** The treaty is signed with witnesses and copies.
12. **The Message.** Home hears a distorted version and the shelter answers.
13. **The First Winter.** The treaty is tested by ice and shortage.
14. **The Review.** The treaty is re-read and one term is amended honestly.
15. **The Table on the Bridge.** The shelter decides what kind of neighbor it
    will be.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Envoys | one / team / rotating | focus vs. memory |
| Credentials | narrow / broad / case-by-case | caution vs. speed |
| Gifts | practical / symbolic / both | meaning vs. use |
| Treaty style | short / detailed / annexes | trust vs. clarity |
| Messages | full truth / careful / silent | transparency vs. control |
| Access | road / water / both | need vs. leverage |
| Rival | engage / ignore / expose | dignity vs. tactics |
| Final | diplomacy as institution / habit / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Kept Word** — treaties hold because the shelter is known to keep them.
2. **The Bridge Table** — the old bridge becomes a standing meeting place.
3. **The Honest Report** — a hard truth told at home builds more trust than any
   success.
4. **The Narrow Promise** — a small treaty kept beats a grand one broken.
5. **The Cold Silence** — a treaty lapses in a cold season and everyone
   survives it, and the shelter learns to draft better.
6. **Fade** — two flags on a bridge and a table set for the next meeting.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_envoy_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_envoy_invitation`, `quest_envoy_credentials`, `quest_envoy_envoy`,
`quest_envoy_words`, `quest_envoy_gifts`, `quest_envoy_road`, `quest_envoy_table`,
`quest_envoy_water_line`, `quest_envoy_rival`, `quest_envoy_draft`,
`quest_envoy_signing`, `quest_envoy_message`, `quest_envoy_first_winter`,
`quest_envoy_review`, `quest_envoy_table_on_bridge`.

### 6.2 Side quests (30)

**Envoy (5)**
- `quest_envoy_choose` — choose an envoy
- `quest_envoy_prepare` — prepare a mission
- `quest_envoy_travel_kit` — pack the travel kit
- `quest_envoy_escort` — arrange an escort
- `quest_envoy_report_home` — report home

**Summit (5)**
- `quest_envoy_agenda` — write the agenda
- `quest_envoy_seating` — seat the room
- `quest_envoy_framework` — choose a framework
- `quest_envoy_gallery` — open a gallery
- `quest_envoy_minutes` — keep the minutes

**Treaty (5)**
- `quest_envoy_terms` — draft terms
- `quest_envoy_witness` — gather witnesses
- `quest_envoy_copies` — make fair copies
- `quest_envoy_ratify` — ratify at home
- `quest_envoy_renew` — renew or lapse

**Reputation (5)**
- `quest_envoy_evidence` — record evidence
- `quest_envoy_witnessed_act` — do a witnessed act
- `quest_envoy_rumor_check` — check a rumor
- `quest_envoy_correct_record` — correct the record
- `quest_envoy_standing_read` — read standing honestly

**Protocol (5)**
- `quest_envoy_gift_make` — make a gift
- `quest_envoy_gift_receive` — receive and log
- `quest_envoy_forms` — learn the forms
- `quest_envoy_titles` — agree titles
- `quest_envoy_ceremony` — hold a ceremony

**Messages (5)**
- `quest_envoy_notice` — post a notice
- `quest_envoy_explain` — explain a treaty
- `quest_envoy_answer_rumor` — answer a rumor honestly
- `quest_envoy_debate` — hold an open debate
- `quest_envoy_archive_words` — archive the record

### 6.3 Repeatable quests (8)

`quest_envoy_repeat_mission`, `quest_envoy_repeat_minutes`,
`quest_envoy_repeat_evidence`, `quest_envoy_repeat_gift`,
`quest_envoy_repeat_copies`, `quest_envoy_repeat_rumor`,
`quest_envoy_repeat_escort`, `quest_envoy_repeat_review`.

### 6.4 Dynamic hooks

Live events (faction stance changes, embargo changes, war overrides, summit
outcomes, reputation evidence, road and weather conditions, outpost neighbor
contact, press publication) attach authored follow-ups through existing seams.
No new event bus.

### 6.5 Constraints

- Summit state stays with `DiplomaticSummitSystem`.
- Treaties stay with their catalog and regional owners.
- Reputation stays with `ShelterReputationSystem`.
- Messages stay with `PropagandaSystem` and the press.
- Access stays with `TradeEmbargoSystem`; prices are untouched.
- Faction facts stay with 03; intel with 20; justice with 08.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `EnvoySystem` (new, `Ashfall.Core.Diplomacy`)

**Owns:** missions, credentials, envoy skills, travel kits, reports, and
continuity across years. **Consumes:** `DiplomaticSummitSystem` delegates,
`ISurvivorSkillsPort`, route and watch owners, `NeedsSystem`.
**Data:** `envoy_missions.json`, `credentials.json`, `envoy_reports.json`.
**Rules:** credentials define what an envoy may promise; promises are recorded
and testable; a mission has a purpose, a route, and a report; envoys rotate so
knowledge accumulates in the institution rather than one person.

### 7.2 `SummitSystem` (extend `DiplomaticSummitSystem`)

**Owns:** agendas, frameworks, seating, galleries, minutes, and outcomes.
**Consumes:** the live summit state, faction standing and context ports,
records owners. **Data:** `summit_agendas.json`. **Rules:** every summit has a
written agenda and minutes; seating follows protocol; outcomes are recorded as
facts, not as sentiment.

### 7.3 `TreatySystem` (extend `DiplomaticTreatyCatalog` and `RegionalTreatySystem`)

**Owns:** term negotiation, drafting, witnesses, ratification, review, and
lapse. **Consumes:** the live treaty catalogs, `StandingRecord`, `Lio`'s exact
wording practice, embargo access owner. **Data:** `treaty_terms.json`.
**Rules:** terms are read aloud and agreed line by line; ratification happens at
home; reviews are scheduled; lapse is explicit, never silent.

### 7.4 `ReputationSystem` (extend `ShelterReputationSystem`)

**Owns:** evidence gathering, witness records, rumor checks, corrections, and
standing reads. **Consumes:** the live evidence structure, summit outcomes,
trade access, witness travel reports. **Data:** `reputation_evidence.json`.
**Rules:** evidence has a medium, confidence, and salience; witnessed acts
outweigh claims; corrections require proof; the shelter may read its standing,
never edit it directly.

### 7.5 `ProtocolSystem` (new, thin, `Ashfall.Core.Diplomacy`)

**Owns:** gifts, forms, titles, ceremonies, and guest care. **Consumes:**
`Inventory`, craft owners (27, 31, 39), `StandingRecord`, 13's ceremony forms
(read). **Data:** `protocol_gifts.json`. **Rules:** gifts are declared and
logged; a gift creates obligation in both directions; forms exist to reduce
fear; guests are housed decently and never used as leverage.

### 7.6 `PublicMessageSystem` (extend `PropagandaSystem`, truthfully)

**Owns:** notices, explanations, debates, answers to rumors, and archives of
public words. **Consumes:** the live campaign system, `NoticeSystem` (Wave 4),
press owner, reputation owner. **Data:** `public_messages.json`.
**Rules:** messages carry a truthfulness value; false statements damage
internal trust through live systems even when they help externally; the shelter
can always choose candor; archived words are quotable forever.

### 7.7 `AccessAgreementSystem` (extend `TradeEmbargoSystem`)

**Owns:** road, water, market, and border access as agreement categories.
**Consumes:** the live embargo system, route owner, treaty owner.
**Data:** `embargo_access.json`. **Rules:** access is granted, denied, or
limited by agreement; prices are never set here; automatic consequences come
from the live embargo owner.

### 7.8 Systems explicitly not added

- No second summit, treaty, reputation, message, faction, or trade system.
- No bribery or manipulation rewards.
- No price setting or currency.
- No espionage generation; intel comes from 20.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `envoy_missions.json` (new)

```json
{
  "schema_version": 1,
  "missions": [
    {
      "mission_id": "mission_bridge_summit",
      "display_name": "Summit at the Old Bridge",
      "purpose": "water and road access",
      "envoy": "sare_mel",
      "escort": ["boden", "runner"],
      "may_promise": ["road access", "seasonal water"],
      "may_not_promise": ["exclusive crossing", "territory"],
      "days": 6,
      "tags": ["summit", "water"]
    }
  ]
}
```

### 8.2 `credentials.json` (new)

Credentials: bearer, authority, limits, seal, expiry, witness.

### 8.3 `summit_agendas.json` (new)

Agendas: session, order, speakers, framework, minutes, outcome.

### 8.4 `treaty_terms.json` (new)

Terms: clause, plain wording, obligation, duration, review, lapse.

### 8.5 `reputation_evidence.json` (new)

Evidence: act, witness, medium, confidence, salience, effect.

### 8.6 `protocol_gifts.json` (new)

Gifts: object, meaning, source, obligation, log, taboo check.

### 8.7 `public_messages.json` (new)

Messages: topic, wording, truthfulness, audience, channel, archive.

### 8.8 `embargo_access.json` (new)

Access: category, partner, grant, limits, review, status.

### 8.9 `envoy_reports.json` (new)

Reports: mission, meetings, promises made, promises heard, next steps.

### 8.10 `delegate_dossiers.json` (new)

Delegates: faction, name, interests, tells, past words, courtesy notes.

### 8.11 Items

New items appended to `items.json`: `item_envoy_credentials`,
`item_envoy_seal`, `item_gift_chest`, `item_treaty_scroll`,
`item_interpreter_notebook`, `item_formal_coat`, `item_table_flag`,
`item_gift_salt`, `item_welcome_banner`, `item_message_satchel`,
`item_house_codex`, `item_seating_chart`, `item_ink_and_pen`,
`item_signet_ring`, `item_shared_wordlist`, `item_ceremony_tea_set`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Summit, treaty, reputation, and message states remain the live save owners. New
sub-objects (missions, credentials, agendas, terms, evidence, gifts, messages,
access, reports, dossiers) are additive inside them. No new save section.

### 9.2 State to persist

- Missions and their outcomes.
- Credentials and their limits.
- Agendas, minutes, and summit outcomes.
- Treaty terms, signatures, ratifications, and reviews.
- Reputation evidence and corrections.
- Gift exchanges and obligations.
- Public messages and their truthfulness.
- Access agreements.
- Envoy reports and continuity notes.
- Delegate dossiers.

### 9.3 Determinism

- Summit and treaty state remain in the live owners.
- Reputation deltas derive from recorded evidence with the live confidence and
  salience inputs.
- Access changes route through the live embargo owner.
- Travel and weather effects use the live road and weather paths.
- Paired replay hashes must match; no wall-clock or `System.Random`.

### 9.4 Migration

Legacy saves load with factions, stance, embargoes, treaties, and reputation
untouched; no missions, credentials, or messages exist until started. Existing
treaties gain a review date and a plain-language summary when first opened.

### 9.5 Checksum

Invariant-culture floats; integer day and evidence-count fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `EnvoyPanel` (new) | Envoys, credentials, missions | `EnvoyHostSession` |
| `SummitPanel` (new) | Agendas, seating, minutes | same |
| `TreatyPanel` (new) | Terms, signatures, reviews | same |
| `ReputationPanel` (new) | Evidence and standing | same |
| `ProtocolPanel` (new) | Gifts, forms, guests | same |
| `MessagePanel` (new) | Public words and truth | same |
| `AccessPanel` (new) | Agreements and limits | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Treaty clauses are shown in plain language beside their formal text.
- Reputation shows evidence, not a mystery score.
- Message truthfulness is visible to the player before sending.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Seating and gallery layouts have text descriptions.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a gavel on a table, a seal pressed
into wax, a page turned, a kettle in the guest room, a flag snapping, boots on
a bridge. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `DiplomaticSummitSystem` | Summits and delegates |
| `DiplomaticTreatyCatalog` | Treaty content |
| `RegionalTreatySystem` | Regional treaties |
| `ShelterReputationSystem` | Evidence and standing |
| `PropagandaSystem` | Public messages |
| `FactionStanceEngine` | Stance changes |
| `TradeEmbargoSystem` | Access agreements |
| `StandingRecord` (Exp 03) | Faction facts and outcomes |
| `EspionageSystem` (Wave 2) | Information used, never generated |
| `RouteInfrastructureSystem` (Wave 5) | Travel to meetings |
| `WatchHouseHostSession` (Wave 5) | Escorts and boundaries |
| `OutpostRelationsSystem` (Wave 7) | Local contact below the stage |
| `NoticeSystem` (Wave 4) | Posted messages |
| `PressHostSession` (Wave 4) | Printed words |
| `NeedsSystem` | Delegate condition |
| `EpilogueChronicleBuilder` | Diplomatic milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm summit, treaty, reputation, message,
stance, embargo, record, and route owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; extend treaty and
message content; append items; register validators and scanner.

**Phase 2 — Pure Core.** `EnvoySystem`, `SummitSystem`, `TreatySystem`,
`ReputationSystem`, `ProtocolSystem`, `PublicMessageSystem`,
`AccessAgreementSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `EnvoyHostSession`, selftest coverage, fresh journey
from invitation to the table on the bridge.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak: summit, treaty winter, rumor, review, and
second meeting.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Missions | 14 |
| Credentials | 10 |
| Agendas | 12 |
| Treaty terms | 24 |
| Evidence entries | 24 |
| Gifts | 14 |
| Public messages | 20 |
| Access agreements | 12 |
| Envoy reports | 14 |
| Delegate dossiers | 10 |
| Items | 16 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Manipulation rewarded | Critical | Truthfulness cost |
| Second diplomacy system | Critical | Extend live owners |
| Price creep | High | Access only, no prices |
| Ethnic framing | Critical | Interest-based factions |
| Bribery loop | High | Gifts logged with obligation |
| Espionage overlap | Medium | Use intel, never generate |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `envoy_missions.json` | 14 | 3,500 |
| `credentials.json` | 10 | 2,000 |
| `summit_agendas.json` | 12 | 3,000 |
| `treaty_terms.json` | 24 | 5,000 |
| `reputation_evidence.json` | 24 | 4,500 |
| `protocol_gifts.json` | 14 | 3,000 |
| `public_messages.json` | 20 | 4,000 |
| `embargo_access.json` | 12 | 2,500 |
| `envoy_reports.json` | 14 | 3,500 |
| `delegate_dossiers.json` | 10 | 3,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~64,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R45-1 | Manipulation rewards | Med | Critical | Truth cost |
| R45-2 | Second diplomacy system | Low | Critical | Live owners |
| R45-3 | Price creep | Med | High | Access only |
| R45-4 | Ethnic framing | Low | Critical | Interest factions |
| R45-5 | Bribery loop | Med | High | Logged gifts |
| R45-6 | Intel overlap | Med | Medium | Use not generate |
| R45-7 | Score-chasing rep | Med | Medium | Evidence-based |
| R45-8 | Determinism | Low | High | Live paths |
| R45-9 | Content overrun | Med | Med | Budget §13 |
| R45-10 | Message spam | Med | Med | Archive discipline |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **May the shelter ever break a treaty?** Recommended: yes, with a recorded
   act, a reputation consequence, and no hidden option.
2. **Do public messages ever lie successfully?** Recommended: no; lies may
   delay consequences but always damage internal trust.
3. **Who ratifies a treaty?** Recommended: the steward and the standing record,
   with the charter process, never a single envoy.
4. **Can envoys serve for years?** Recommended: yes, with rotation rules and
   institutional memory kept by the scribe.
5. **Is access ever purchased?** Recommended: no; access is granted by
   agreement, with goods as gifts or labor, never as prices.

---

## 17. APPENDIX D — ENVOY MISSION TABLE (14 MISSIONS)

| # | Mission | Purpose | Envoy | Days | May promise |
|---|---|---|---|---|---|
| 1 | Bridge summit | water, road | Sare | 6 | access, seasonal water |
| 2 | River parley | fishing line | Sare | 3 | line, time |
| 3 | Ridge visit | ridge relay | Quo | 4 | snow report |
| 4 | Quarry talks | stone exchange | Maret | 5 | labor, haulage |
| 5 | Lowlands call | grain access | Sare | 7 | road maintenance |
| 6 | Neighbor line | shared watch | Boden | 2 | drill date |
| 7 | Old families | letters and respect | Ola | 4 | none |
| 8 | Forest camp | fire watch | Boden | 3 | shared patrol |
| 9 | Deep vale | leave alone | Sare | 2 | none |
| 10 | Passing traders | road use | Maret | 1 | market day |
| 11 | Wandering party | entry charter | Boden | 1 | entry terms |
| 12 | Treaty review | winter terms | Sare | 5 | amendment |
| 13 | Gift return | obligation | Ola | 3 | none |
| 14 | Emergency call | flood help | Sare | 2 | help, thanks |

Fourteen missions cover the shelter's diplomatic life for years. The may-promise
column is the credential discipline in table form: an envoy can only promise
what the shelter wrote down, and the two missions that may promise nothing are
as important as the rest.

---

## 18. APPENDIX E — CREDENTIAL TABLE

| # | Credential | Bearer | Authority | Limits | Expiry |
|---|---|---|---|---|---|
| 1 | Full envoy | Sare | speak and draft | nothing final | mission |
| 2 | Limited envoy | Quo | words and questions | no promises | mission |
| 3 | Protocol bearer | Maret | gifts and forms | no terms | mission |
| 4 | Escort captain | Boden | safety | no speech | mission |
| 5 | Scribe | Lio | record | no negotiation | mission |
| 6 | Trade contact | Maret | access talk | no prices | season |
| 7 | Emergency caller | any | ask help | no terms | event |
| 8 | Ratifier | steward | final signature | after home review | permanent |
| 9 | Witness | two residents | attest | none | per treaty |
| 10 | Observer | any | watch and report | no speech | mission |

Credentials are what separate diplomacy from improvisation. The limits column
is what an envoy points to when asked for more than the shelter has agreed to
give, and the witness row is how a treaty becomes a shared fact rather than a
private opinion.

---

## 19. APPENDIX F — SUMMIT AGENDA TABLE

| # | Agenda | Session | Order | Framework | Outcome |
|---|---|---|---|---|---|
| 1 | Bridge water | one | welcome, water, road | open table | draft treaty |
| 2 | River fishing | one | lines, times | neighboring | season agreement |
| 3 | Road upkeep | two | sections, labor | shared work | schedule |
| 4 | Market day | one | days, stalls | exchange | access note |
| 5 | Boundary walk | one | marks, patrols | joint line | map signed |
| 6 | Winter help | two | plowing, stores | mutual aid | aid pact |
| 7 | Seed sharing | one | varieties, returns | agricultural | seed covenant |
| 8 | Forest watch | one | fire, patrols | protective | watch pact |
| 9 | Guest terms | one | hospitality | protocol | guest accord |
| 10 | Disputed story | two | witnesses, records | record-based | correction |
| 11 | Treaty review | two | clauses | review | amendment |
| 12 | Second table | one | future meetings | standing | calendar |

Twelve agendas show the range of formal meetings the shelter can hold, from
water treaties to agreed calendars for future meetings. The framework column
matters because strangers meet more easily inside a structure both sides
expected.

---

## 20. APPENDIX G — TREATY TERM TABLE (24 TERMS)

| # | Term | Plain wording | Obligation | Duration | Review |
|---|---|---|---|---|---|
| 1 | Water draw | mornings ours, evenings theirs | share | one year | yearly |
| 2 | Road use | both may pass | maintain | two years | yearly |
| 3 | Road upkeep | each keeps their half | labor | ongoing | season |
| 4 | Seasonal water | high summer shared | monitor | yearly | yearly |
| 5 | Fishing line | no nets past the bend | respect | yearly | yearly |
| 6 | Market day | first and third | attend | season | season |
| 7 | Boundary marks | painted stones | not move | permanent | season |
| 8 | Joint watch | alternate patrols | attend | yearly | season |
| 9 | Winter aid | first to the other | help | yearly | yearly |
| 10 | Snow plow | shared crawler | fuel | winter | yearly |
| 11 | Seed return | return double | grow | season | season |
| 12 | Forest fire | call and help | help | ongoing | season |
| 13 | Guest terms | bed and bread | host | ongoing | yearly |
| 14 | Dispute record | write it down | record | permanent | event |
| 15 | Correction | wrong words fixed | correct | permanent | event |
| 16 | Messenger safety | no interference | protect | ongoing | yearly |
| 17 | Escort limit | no more than four | limit | ongoing | yearly |
| 18 | Weapons at table | left outside | respect | each meeting | each |
| 19 | Trade talk | access only | talk | ongoing | yearly |
| 20 | No exclusive claim | crossing shared | refrain | permanent | yearly |
| 21 | Lapse notice | one season warning | warn | permanent | event |
| 22 | Amendment | by review only | agree | permanent | yearly |
| 23 | Withdrawal | reasons written | explain | permanent | event |
| 24 | Witnesses | two each side | attest | permanent | permanent |

Twenty-four terms in plain wording and formal obligation, each with a duration
and a review. The table is the treaty system's body: small readable clauses,
written to be kept exactly as understood, and every one of them testable by the
world rather than by the player's goodwill.

---

## 21. APPENDIX H — REPUTATION EVIDENCE TABLE

| # | Witnessed act | Dimension | Medium | Confidence | Salience |
|---|---|---|---|---|---|
| 1 | Snow plow sent | reliability | convoy report | high | high |
| 2 | Water shared in drought | generosity | delegate | high | high |
| 3 | Treaty kept to the letter | reliability | review | high | med |
| 4 | Refugee taken in | humanity | traveler | med | high |
| 5 | Boundary respected | honesty | patrol | high | med |
| 6 | Seed returned double | generosity | field | high | med |
| 7 | Wrong word corrected | honesty | scribe | high | med |
| 8 | Gate refusal explained | humanity | visitor | med | low |
| 9 | Convoy late | reliability | host | med | low |
| 10 | Late with notice | honesty | host | high | low |
| 11 | Rumor denied falsely | honesty | rumor | med | high |
| 12 | Gifts received well | respect | host | high | low |
| 13 | Escort respected limit | respect | host | high | low |
| 14 | Table kept calm | dignity | minutes | high | med |
| 15 | Help sent after flood | humanity | witness | high | high |
| 16 | Market rule broken | reliability | stall | med | med |
| 17 | Watch patrols missed | reliability | joint log | high | med |
| 18 | Guest housed well | respect | delegate | high | med |
| 19 | Dispute recorded honestly | honesty | record | high | med |
| 20 | Message at home honest | internal | notice | high | high |
| 21 | Rival misquote answered | dignity | minutes | high | med |
| 22 | Withdrawal explained | honesty | letter | high | med |
| 23 | Year of keeping word | reliability | review | high | high |
| 24 | Correction accepted | humility | scribe | high | med |

Twenty-four pieces of evidence, each with a medium, confidence, and salience.
The table shows how reputation is earned by acts that other people can see and
write down, and the internal rows exist because the shelter's own people are an
audience too.

---

## 22. APPENDIX I — PROTOCOL GIFT TABLE

| # | Gift | Meaning | Source | Obligation | Taboo check |
|---|---|---|---|---|---|
| 1 | Salt | friendship | works | share table | none |
| 2 | Seed | future | gardens | grow and return | none |
| 3 | Bread | daily welcome | kitchen | eat together | none |
| 4 | Cloth | covering | thread | wear at meeting | color check |
| 5 | Tool | work shared | wheel | use, return | hand check |
| 6 | Glass cup | clear speech | glass | drink together | none |
| 7 | Lantern | light kept | stores | keep lit | none |
| 8 | Map copy | trust | survey | not copy further | boundary check |
| 9 | Tea set | ceremony | trade | host next time | none |
| 10 | Charm of stone | memory | quarry | display | faith check |
| 11 | Wood carving | story | workshop | tell the story | image check |
| 12 | Record scroll | respect | press | keep and cite | privacy check |
| 13 | Winter blanket | care | thread | use | none |
| 14 | Seed of return | reciprocity | gardens | plan a return | none |

Gifts are chosen for meaning, not for leverage, and every one carries an
obligation in both directions. The taboo column is the expansion's respect for
the other community: the shelter checks what a gift means before it gives, not
after.

---

## 23. APPENDIX J — PUBLIC MESSAGE TABLE (20 MESSAGES)

| # | Message | Topic | Truthfulness | Channel | Archive |
|---|---|---|---|---|---|
| 1 | Treaty summary | water and road | full | notice wall | yes |
| 2 | Debate notice | treaty open forum | full | notice | yes |
| 3 | Rumor answer | "we gave away the river" | full | meeting | yes |
| 4 | Mission report | summit results | full | notice | yes |
| 5 | Guest welcome | visiting delegates | full | notice | yes |
| 6 | Boundary reminder | painted stones | full | notice | yes |
| 7 | Seed share | return terms | full | notice | yes |
| 8 | Winter aid | help pact | full | meeting | yes |
| 9 | Market days | access note | full | notice | yes |
| 10 | Rival claim answer | misquotation | full | meeting | yes |
| 11 | Setback report | late convoy | full | notice | yes |
| 12 | Correction | wrong figure | full | notice | yes |
| 13 | Withdrawal letter | lapse | full | letter | yes |
| 14 | Review summary | amended clause | full | notice | yes |
| 15 | Praise of rival | fair words | full | meeting | yes |
| 16 | Apology | broken term | full | meeting | yes |
| 17 | Refusal notice | deep vale | full | letter | yes |
| 18 | Emergency call | flood help | full | radio | yes |
| 19 | Ceremony notice | gift return | full | notice | yes |
| 20 | Year review | standing | full | notice | yes |

Every message in the table is true, and the table is designed to make that
easy. The expansion's rule is that the shelter may choose to say less, to wait,
or to keep a private matter private, but never to say something false — and the
archive column means every public word can be compared to later facts forever.

---

## 24. APPENDIX K — ACCESS AGREEMENT TABLE

| # | Category | Partner | Grant | Limits | Review |
|---|---|---|---|---|---|
| 1 | Road passage | lowlands | both ways | no convoy escorts | yearly |
| 2 | Water draw | lowlands | shared hours | no damming | yearly |
| 3 | Market day | all | first and third | stall count | season |
| 4 | Fishing line | fishers | up to the bend | no nets | yearly |
| 5 | Forest entry | forest camp | fire watch only | no cutting | season |
| 6 | Ridge path | shepherds | walking | no carts | yearly |
| 7 | Quarry access | quarry crew | stone loads | schedule | season |
| 8 | River crossing | all | shared | no exclusive | permanent |
| 9 | Neighbor watch | neighbor line | joint drills | no patrol claims | season |
| 10 | Guest entry | all | three days | register | yearly |
| 11 | Emergency passage | all | always | help in return | ongoing |
| 12 | Deep vale | none | none | leave alone | yearly |

Twelve access agreements show that diplomacy's practical product is a set of
doors held open or closed. Prices never appear, and the leave-alone row is
included because a community's most important diplomatic decision is sometimes
the line it chooses not to cross.

---

## 25. APPENDIX L — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_envoy_invitation` | 4 | Summons arrives |
| `quest_envoy_credentials` | 4 | Limits written |
| `quest_envoy_envoy` | 3 | Sare chosen |
| `quest_envoy_words` | 4 | Shared word list |
| `quest_envoy_gifts` | 4 | Gifts chosen |
| `quest_envoy_road` | 3 | Journey planned |
| `quest_envoy_table` | 5 | First session held |
| `quest_envoy_water_line` | 5 | Water term argued |
| `quest_envoy_rival` | 4 | Misquote handled |
| `quest_envoy_draft` | 4 | Exact words |
| `quest_envoy_signing` | 4 | Signed with witnesses |
| `quest_envoy_message` | 5 | Home explained |
| `quest_envoy_first_winter` | 5 | Treaty tested |
| `quest_envoy_review` | 4 | Amendment agreed |
| `quest_envoy_table_on_bridge` | 3 | Final disposition |

---

## 26. APPENDIX M — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_envoy_choose` | 3 | Envoy chosen |
| `quest_envoy_prepare` | 3 | Mission prepared |
| `quest_envoy_travel_kit` | 3 | Kit packed |
| `quest_envoy_escort` | 3 | Escort arranged |
| `quest_envoy_report_home` | 4 | Report filed |
| `quest_envoy_agenda` | 3 | Agenda written |
| `quest_envoy_seating` | 3 | Seating planned |
| `quest_envoy_framework` | 3 | Framework chosen |
| `quest_envoy_gallery` | 3 | Gallery opened |
| `quest_envoy_minutes` | 3 | Minutes kept |
| `quest_envoy_terms` | 4 | Terms drafted |
| `quest_envoy_witness` | 3 | Witnesses gathered |
| `quest_envoy_copies` | 3 | Fair copies |
| `quest_envoy_ratify` | 4 | Ratified at home |
| `quest_envoy_renew` | 4 | Renewed or lapsed |
| `quest_envoy_evidence` | 3 | Evidence recorded |
| `quest_envoy_witnessed_act` | 4 | Act performed |
| `quest_envoy_rumor_check` | 3 | Rumor checked |
| `quest_envoy_correct_record` | 4 | Record corrected |
| `quest_envoy_standing_read` | 3 | Standing read |
| `quest_envoy_gift_make` | 4 | Gift made |
| `quest_envoy_gift_receive` | 3 | Gift received |
| `quest_envoy_forms` | 3 | Forms learned |
| `quest_envoy_titles` | 3 | Titles agreed |
| `quest_envoy_ceremony` | 4 | Ceremony held |
| `quest_envoy_notice` | 3 | Notice posted |
| `quest_envoy_explain` | 4 | Treaty explained |
| `quest_envoy_answer_rumor` | 4 | Rumor answered |
| `quest_envoy_debate` | 4 | Debate held |
| `quest_envoy_archive_words` | 3 | Words archived |

---

## 27. APPENDIX N — NPC DOSSIERS (BRIEF)

**Sare Mel** — envoy. Travels with a small bag and a written list of what she
may not promise. Believes an envoy's only real asset is a reputation for
meaning exactly what the paper says.

**Quo** — interpreter. Holds two dictionaries and one rule: translate the
meaning, not the tone. Believes most wars start as bad translation.

**Maret** — head of protocol. Chooses gifts for meaning and seats people where
they can succeed. Believes forms exist so that frightened strangers can meet.

**Lio** — scribe. Writes the exact words and reads them back twice. Believes a
recorded promise is a gift to the future.

**Boden** — guard captain. Keeps four escorts and no weapons at the table, and
knows the route home in the dark. Believes safety is a service, not a display.

**Ushka** — lowlands delegate. Wants a water term that survives her own
community's hardest month. Believes a treaty that pretends scarcity is gone
will break in the first dry week.

**Tark** — rival envoy. Quotes others generously when it helps him and
carelessly when it does not. Believes the crossing belongs to whoever says so
loudest; the shelter's answer is a written page.

**Ola** — gift keeper. Keeps the closet, the ledger, and the memory of what was
given. Believes a gift is a sentence with two subjects.

---

## 28. APPENDIX O — LOCATION DETAIL

- **The Meeting Gate** — formal arrival, papers checked kindly.
- **The Summit Yard** — an open-air table under a canopy.
- **The Gift Garden** — where planted gifts grow into obligations.
- **The Boundary Stone** — a line read aloud in both languages before it is
  walked.
- **The River Pavilion** — neutral ground built by both sides.
- **The Lowlands Camp** — a host's home seen from the visitor's side.
- **The Old Bridge** — the table, the flags, and the shared crossing.
- **The Market Line** — where access is counted and not priced.
- **The Message Wall** — public words in careful handwriting.
- **The Flag Hill** — visibility, signals, and an honest sky.

---

## 29. APPENDIX P — PROMISE-KEEPING PROTOCOL

| Step | Action | Owner |
|---|---|---|
| Read | read the promise aloud | scribe |
| Confirm | both sides confirm meaning | envoys |
| Record | write the exact words | scribe |
| Store | fair copies to both parties | keeper |
| Train | tell the people who must keep it | envoy |
| Watch | track the term through the year | steward |
| Test | note when the world tests it | all |
| Amend | review and change by agreement | summit |
| Lapse | warn, explain, record | envoy |
| Review | read the year's keeping | all |

The promise-keeping protocol is the expansion's operational spine. Its seventh
step is the interesting one: the world tests every treaty, and the shelter's
job is to notice the test and answer it honestly rather than to pretend the
term was never stressed.

---

## 30. APPENDIX Q — WORKED 720-DAY DIPLOMATIC SCENARIO

**Days 1–30.** The invitation arrives; the shelter debates whether to attend;
credentials are written with limits that feel stingy and prove wise.

**Days 31–60.** Sare and Quo prepare; the shared word list is built over four
evenings; gifts are chosen and logged; the road is checked by Yara.

**Days 61–75.** The journey; arrival at the gate; papers checked kindly; the
first night in guest quarters that are decent and not luxurious.

**Days 76–90.** The first session: agenda, seating, opening statements. Tark
attempts to speak for the shelter and is corrected by the minutes, not by
outrage.

**Days 91–120.** Water is the hard term; Ushka and Sare find the morning and
evening arrangement after three drafts; Lio reads every version aloud.

**Days 121–150.** The treaty is signed with four witnesses; fair copies made;
the shelter's copy travels home in a waxed tube.

**Days 151–210.** Home debate: a message campaign has told the worst version of
the water term; the shelter answers with the exact clause, read at a public
meeting, and the trust survives.

**Days 211–270.** First test: a dry spell. Both sides move their draw to the
agreed hours and nobody cheats; the term becomes real in the world instead of
on paper.

**Days 271–330.** Winter: the snow plow pact is used twice; the shared crawler
eats fuel; both sides split the cost without an argument because the treaty
said how.

**Days 331–390.** Rumor: a traveler says the shelter intends to dam the river.
The rumor is checked, traced to a misheard conversation, and answered with a
written correction delivered by hand.

**Days 391–450.** Second summit: designation of the bridge as a standing
meeting place; a calendar is agreed; the gallery is opened to ordinary
residents of both communities.

**Days 451–540.** A shelter act of mercy, taking in a stranded family, is
witnessed and recorded by a traveler; the shelter's standing rises in a
dimension it did not know it was building.

**Days 541–630.** Tark's community proposes an exclusive crossing; the shelter
refuses politely, in writing, with the reasons attached, and keeps the refusal
on file.

**Days 631–700.** The year review: every term is read aloud; one clause is
amended because the real world made it awkward; the amendment is celebrated
with tea rather than ceremony.

**Days 701–720.** The flags go up on the bridge for the second year, and the
shelter's envoy is no longer a traveler but an office with a desk and a
successor in training.

---

## 31. APPENDIX R — VIGNETTES (TONE SAMPLE)

> Sare reads the credential list before she leaves the yard, and the list is
> mostly things she may not promise, and she reads it twice because the list is
> what keeps her honest when a room full of reasonable people asks for more.

> Quo writes a word on the shared list and then writes three words under it for
> what that word could mean, and Ushka reads all four and nods, and the treaty's
> hardest clause becomes easy because two people did vocabulary for an evening.

> Lio reads the draft aloud and stops at "seasonal" and asks what season and
> whose year, and the room goes quiet, and the word is fixed, and the clause
> survives the drought that comes two summers later.

> At home, the message wall carries the exact clause in plain letters beside
> the questions it answers, and an old resident reads it three times and says
> that is not what she heard, and now she has heard it, and that is the whole
> job of telling the truth.

---

## 32. APPENDIX S — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Over-promise | treaty cannot be kept | correct early, strip term |
| Misquote | confusion | read the exact words |
| Broken term | reputation damage | apologize, repair, record |
| Rumored betrayal | internal distrust | public meeting, evidence |
| Bribery attempt | obligation trap | refuse, log, explain |
| Bad seating | quarrel | reseat, apologize |
| Translation error | wrong term agreed | re-read, amend |
| Late envoy | host insulted | notice sent, honest reason |
| Stolen copy | leak | announce the real text |
| Lapsed treaty | access loss | negotiate a quieter renewal |

Diplomacy's failures are survivable by design, because a community that can
admit a mistake and correct it in public is more trusted than one that has
never been caught wrong. The stolen-copy row is included because honesty is
also the best defense against leaks.

---

## 33. APPENDIX T — CONTENT REVIEW CHECKLIST

- [ ] No bribery or manipulation is rewarded.
- [ ] No espionage operations are generated here.
- [ ] No price setting or currency exists in this plan.
- [ ] `DiplomaticSummitSystem` remains the summit authority.
- [ ] Treaty catalogs remain the treaty authority.
- [ ] `ShelterReputationSystem` remains the reputation authority.
- [ ] Public messages can always be honest, and honesty is never punished.
- [ ] Factions are interest-based, never identity-based.
- [ ] Guests are housed decently and never used as leverage.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 34. APPENDIX U — GLOSSARY

- **Envoy** — a person trusted to speak for the shelter within written limits.
- **Credentials** — the document that says what an envoy may promise.
- **Summit** — a formal meeting with an agenda and minutes.
- **Framework** — the agreed structure a meeting follows.
- **Treaty** — a written agreement with terms, witnesses, and a review.
- **Ratification** — the shelter's own formal acceptance at home.
- **Reputation** — what others believe based on witnessed acts.
- **Evidence** — a recorded act with a medium, confidence, and salience.
- **Protocol** — the forms that let strangers meet safely.
- **Access** — a door opened or closed by agreement, never a price.

---

## 35. APPENDIX V — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `DiplomaticSummitSystem` | factions | summit state | treaties |
| `EnvoySystem` | skills | missions, credentials | summit state |
| `SummitSystem` | agendas | minutes, outcomes | faction facts |
| `TreatySystem` | catalogs | terms, reviews | stance |
| `ReputationSystem` | evidence | reputation | treaties |
| `ProtocolSystem` | gifts | gift logs | reputation |
| `PublicMessageSystem` | press | messages | reputation |
| `AccessAgreementSystem` | embargo | access | prices |
| `TradeEmbargoSystem` | agreements | embargo state | access content |
| `FactionStanceEngine` | treaties | stance | reputation |
| `StandingRecord` | outcomes | records | diplomacy |
| `NoticeSystem` | messages | notices | press |
| `PressHostSession` | messages | print | diplomacy |
| `EspionageSystem` | intel | nothing | diplomacy |
| `NeedsSystem` | delegates | needs | diplomacy |
| `EpilogueChronicleBuilder` | milestones | chronicle | diplomacy |

---

## 36. APPENDIX W — DATA SCHEMA DETAIL (NEW CATALOGS)

**`envoy_missions.json`** — `mission_id`, `display_name`, `purpose`, `envoy`,
`escort[]`, `may_promise[]`, `may_not_promise[]`, `days`, `tags`.

**`credentials.json`** — `credential_id`, `bearer`, `authority`, `limits[]`,
`seal`, `expiry`, `witness`, `tags`.

**`summit_agendas.json`** — `agenda_id`, `session`, `order[]`, `speakers[]`,
`framework`, `minutes`, `outcome`, `tags`.

**`treaty_terms.json`** — `term_id`, `plain_wording`, `obligation`, `duration`,
`review`, `tags`.

**`reputation_evidence.json`** — `evidence_id`, `act`, `witness`, `medium`,
`confidence`, `salience`, `effect`, `tags`.

**`protocol_gifts.json`** — `gift_id`, `object`, `meaning`, `source`,
`obligation`, `log`, `taboo_check`, `tags`.

**`public_messages.json`** — `message_id`, `topic`, `wording`, `truthfulness`,
`audience`, `channel`, `archive`, `tags`.

**`embargo_access.json`** — `access_id`, `category`, `partner`, `grant`,
`limits[]`, `review`, `status`, `tags`.

**`envoy_reports.json`** — `report_id`, `mission`, `meetings[]`,
`promises_made[]`, `promises_heard[]`, `next_steps`, `tags`.

**`delegate_dossiers.json`** — `delegate_id`, `faction`, `name`, `interests[]`,
`tells[]`, `past_words[]`, `courtesy_notes`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid references, or out-of-range numbers.

---

## 37. APPENDIX X — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Terms kept | reliability | Treaty |
| Terms amended | honesty | Treaty |
| Missions completed | activity | Envoy |
| Evidence recorded | reputation health | Reputation |
| Access agreements | practical reach | Access |
| Rumors answered | internal trust | Message |
| Summits held | continuity | Summit |
| Gifts logged | protocol | Protocol |
| Reports filed | institutional memory | Envoy |
| Guest stays | hospitality | Protocol |

Telemetry is diagnostic only; it never gates content and never becomes a score
against an envoy or a partner community.

---

## 38. APPENDIX Y — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Treaty and message catalogs extended with authored content.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Summit, treaty, reputation, message, stance, and embargo authorities
      remain untouched.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §33.
- [ ] Phase 7 soak shows a summit, a winter test, a rumor, and a review.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No bribery, price, espiionage, or identity-framing content exists.

---

## 39. APPENDIX Z — OPEN QUESTIONS FOR REVIEW

1. Can two envoys be sent to the same summit with different instructions?
2. Does the shelter's internal debate about a treaty affect its ratification?
3. Are envoys allowed to improvise beyond credentials in an emergency?
4. Can a treaty be ratified without a public reading?
5. Should rivals be invited to the shelter's own meetings?
6. Does a correct record ever change a rumor's effect after the fact?
7. Who maintains the delegate dossiers, and how private are they?
8. Can a treaty include an expiry by default, or must negotiation specify one?

None of these may be decided unilaterally; each changes tone and balance.

---

## 40. APPENDIX AA — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Envoys across generations |
| 1 | 13 The Faithful | Ceremony forms and vigils |
| 2 | 17 The Long Evening | Music at a summit |
| 2 | 20 The Quiet Hand | Intelligence used, not generated |
| 3 | 22 The Clean Flow | Water terms |
| 3 | 25 The Iron Road | Rail access talks |
| 3 | 26 The Common Table | Guest meals |
| 4 | 27 The Thread | Formal cloth and gifts |
| 4 | 29 The Glass | Gift cups and lenses |
| 4 | 30 The Press | Printed words and copies |
| 5 | 32 The Wild | Hunting-rights talks |
| 5 | 33 The Weather | Seasonal access |
| 5 | 34 The Long Road | Road and bridge terms |
| 5 | 36 The Watch | Boundary and escort rules |
| 6 | 37 The Quickening | Birth notices at distance |
| 6 | 38 The Ward | Medical aid terms |
| 7 | 42 The Core | Power assistance pacts |
| 7 | 43 The Question | Evidence and record |
| 7 | 44 The Outpost | Neighbor line below the stage |
| 7 | 46 The Long Change | Treaties across change |

Each hook is additive. The Envoy can ship alone, and every other expansion can
ship without it.

---

## 41. APPENDIX AB — ENDING PROSE SKETCHES

**The Kept Word.** Treaties hold because the shelter is known to keep them, and
its reputation does the work that guards never could.

**The Bridge Table.** The old bridge becomes a standing meeting place with two
flags and a table that stays set, and meetings become ordinary rather than rare.

**The Honest Report.** A hard truth told at home builds more trust than any
success, and the shelter's own people become its most important audience.

**The Narrow Promise.** A small treaty kept beats a grand one broken, and the
shelter learns to promise exactly what it can do and no more.

**The Cold Silence.** A treaty lapses in a cold season, everyone survives, and
the shelter drafts better terms for the next table.

**Fade.** Two flags on a bridge, a table set for the next meeting, and a scribe
sharpening a pencil.

---

## 42. APPENDIX AC — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Manipulation rewarded | cynical | truthfulness costs |
| Bribery ladder | reduces trust | logged gifts |
| Fine-print tricks | dishonest | plain wording read aloud |
| Reputation score | hollow | evidence-based |
| Espionage mix | authority break | intel used, not made |
| Price setting | blocked economy | access only |
| Identity factions | harmful | interest-based |
| Big promises | broken later | narrow credentials |
| Empire fantasy | wrong genre | neighbors and agreements |
| Silent failure | no learning | read the review aloud |

The list exists because diplomacy is easy to write as manipulation or as
score-mongering. The expansion's rule is that a promise is a small technology
for sharing a future, and keeping it is the entire point.

---

## 43. APPENDIX AD — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Missions | 14 | 3,500 |
| Credentials | 10 | 2,000 |
| Agendas | 12 | 3,000 |
| Treaty terms | 24 | 5,000 |
| Evidence entries | 24 | 4,500 |
| Gifts | 14 | 3,000 |
| Public messages | 20 | 4,000 |
| Access agreements | 12 | 2,500 |
| Envoy reports | 14 | 3,500 |
| Delegate dossiers | 10 | 3,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~64,000** |

---

## 44. APPENDIX AE — FIRST YEAR OF THE ENVOY

| Month | Focus | Milestone |
|---|---|---|
| 1 | Invitation | decision to attend |
| 2 | Credentials | limits written |
| 3 | Words | shared list built |
| 4 | Gifts | chosen and logged |
| 5 | Journey | arrival at the gate |
| 6 | Table | first session |
| 7 | Water | hardest term drafted |
| 8 | Signing | witnesses and copies |
| 9 | Message | home explained |
| 10 | Test | dry spell handled |
| 11 | Winter | plow pact used |
| 12 | Review | terms read aloud |

A year of the envoy is a year of learning that the world is full of people
rather than threats, and that the shelter's word is the most valuable thing it
can carry across a bridge.

---

## 45. APPENDIX AF — DIPLOMATIC COVENANT

| Clause | Promise |
|---|---|
| Credentials | No promise beyond the written limits |
| Exact words | Every term read aloud and confirmed |
| Witnesses | Every signature attested by both sides |
| Access | Doors by agreement, never by price |
| Gifts | Logged, meaningful, and never a bribe |
| Evidence | Reputation built on witnessed acts |
| Truth at home | Public words are true, or the shelter says less |
| Correction | Wrong words are corrected in public |
| Guests | Housed decently, never used as leverage |
| Keeping | Every year's promises are reviewed aloud |

The covenant is the expansion's first-class design object. A shelter's word is
only worth something if it is specific, witnessed, and kept through the winter,
and this table is what makes that true in the game.

---

## 46. APPENDIX AG — SECOND MEETING TABLE

| Meeting | Place | Purpose | Interval | Keeps |
|---|---|---|---|---|
| Bridge summit | old bridge | water, road | yearly | treaty |
| River parley | pavilion | fishing line | season | line |
| Ridge visit | ridge | relay, snow | season | reports |
| Quarry talks | spur | stone | season | loads |
| Market day | line | access | month | stall list |
| Boundary walk | stone | marks | season | map |
| Forest drill | cut | fire watch | season | plan |
| Guest return | gift garden | obligations | yearly | gifts |

Eight recurring meetings turn one summit into a relationship. The ceremony is
thin, the details are thick, and the second meeting is where diplomacy either
becomes a habit or quietly ends.

---

## 47. APPENDIX AH — DELEGATE DOSSER TABLE

| # | Delegate | Faction | Interests | Tells | Past words | Courtesy |
|---|---|---|---|---|---|---|
| 1 | Ushka | lowlands | water, road | quiet when serious | kept winter aid | tea, early |
| 2 | Tark | ridge | crossing, pride | talks over others | misquoted shelter | none |
| 3 | Marn of stone | quarry | labor, loads | hands dusty | paid in work | tools |
| 4 | Sella | fishers | line, season | watches water | kept line | salt |
| 5 | Old Renn | families | respect, no claim | long pauses | letters kept | letters |
| 6 | Quarry son | quarry | apprenticeship | shy | one letter | teaching |
| 7 | Ridge shepherd | shepherds | grazing, weather | counts sheep | shared forecast | wool |
| 8 | Market keeper | all | days, stalls | counts coins | fair stalls | stall |
| 9 | Forest elder | camp | fire, cutting | smells smoke | kept watch | smoke pack |
| 10 | Silent watcher | deep vale | leave alone | says nothing | none | distance |

Ten dossiers, each a memory of how a person behaves at a table. The courtesy
column is what the shelter offers before any negotiation starts, because
diplomacy in this expansion begins with knowing what someone does with their
hands when they are serious.

---

## 48. APPENDIX AI — TREATY WINTER TEST TABLE

| Term | Winter test | Pass | Failure response |
|---|---|---|---|
| Water draw | frozen river | hours kept | emergency meeting |
| Road use | snow block | shared plow | plow pact |
| Market day | no travel | notice sent | gather at line |
| Fishing line | ice fishing | no nets | warden walk |
| Winter aid | storm | first to help | thanks, record |
| Snow plow | drifts | crawler shared | fuel split |
| Forest watch | dry wood | patrol kept | extra drill |
| Guest terms | stranded party | housed | letters |
| Boundary | buried stones | re-mark | joint walk |
| Witnesses | far apart | copies checked | re-read aloud |

Ten treaty terms, ten winter tests, and one shared rule: the season is where
treaties are graded. The re-mark and re-read rows are how agreements survive
weather that hides the marks and the memories both.

---

## 49. APPENDIX AJ — MESSAGE TRUTH TABLE

| Message type | May say less | May delay | May be false |
|---|---|---|---|
| Treaty summary | no | no | never |
| Setback report | no | one day | never |
| Rumor answer | no | no | never |
| Guest notice | yes | yes | never |
| Negotiation update | yes | yes | never |
| Emergency call | no | no | never |
| Private matter | yes | yes | not reported |
| Correction | must say | no | never |

The truth table is the expansion's ethical spine written as data. The shelter
may always choose to say less or to wait, and it may keep private matters
private, but every column of "false" is sealed — there is no propaganda route
to success that does not damage the shelter's own trust.

---

## 50. APPENDIX AK — SUMMIT SEATING TABLE

| Seat | Who | Why | Notes |
|---|---|---|---|
| Head | host | convenes | neutral chair |
| Right | senior guest | honor | water delegate |
| Left | other senior | balance | ridge delegate |
| Center | scribe | record | reads aloud |
| Side | interpreters | words | between pairs |
| Gallery | residents | witnesses | open |
| Door | escort | safety | outside |
| Back | observers | watching | no speech |

Seating is a small technology for preventing quarrels, and the table shows how
much thought goes into a room before anyone speaks. The observer row is included
because being allowed to watch a decision is sometimes the most important
thing a community can be given.

---

## 51. CLOSING STATEMENT

ASHFALL already has summits, delegates, treaty catalogs, regional treaties,
reputation evidence, stance, embargoes, and message campaigns. What it lacks is
the practice of meeting: envoys with credentials, agendas and minutes, terms
read aloud, gifts with meaning, evidence of witnessed acts, public words that
are true, and agreements about access that outlive the people who signed them.
The Envoy adds that practice without adding a second diplomacy system or a
bribery loop. It adds a table on an old bridge, two flags, a scribe reading the
exact words, and a shelter that is known for keeping what it promises.

> Wave 7 note: this plan is one of five Wave 7 expansion bibles (42–46). Each is
> self-contained; none requires another to ship. The shared Wave 7 index lives
> at `docs/expansions/wave7/WAVE7_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `DiplomaticSummitSystem` (`IFactionStandingPort`,
> `IFactionContextPort`, `ISurvivorSkillsPort`, `DiplomaticSummitState` with
> `summit_id`, `location_id`, `framework_id`, `convening_day`,
> `attending_faction_ids`, `delegate_survivor_ids`), `DiplomaticTreatyCatalog`,
> `RegionalTreatySystem`/`RegionalTreatyFeed`, `ShelterReputationSystem`
> (`ReputationDimension`, `ReputationTag`, `InformationMedium`,
> `ReputationEvidence` with `Delta`, `Confidence`, `Salience`, `Medium`),
> `PropagandaSystem`, `FactionStanceEngine`, `TradeEmbargoSystem`,
> `diplomatic_treaties.json` (6,846 B), `regional_treaties.json` (3,197 B), and
> `propaganda_campaigns.json` (4,998 B).