# ASHFALL — Expansion 13 Design Bible
# THE FAITHFUL & THE FRACTURED
### Wave 1 · Belief, Ritual, Pilgrimage, Relics, and Schism

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Spiritual`, `Ashfall.Core.Survivors` (Zealotry, IdeologicalFriction), `Ashfall.Core.Memorial`, `Ashfall.Core.Culture`
**Proposed host owner:** `FaithHostSession` (extends `SpiritualSaveStore` + `ZealotrySaveStore`)
**Existing save sections:** `ceremony`, `spiritual` (mourning/ritual cooldowns), zealotry state
**Existing CLI verbs:** `--spiritual-selftest`, `--zealotry-selftest`, `--ceremony-selftest`
**Rule compliance:** fictional religions only; symmetrical mechanics; no real-world faith,
no moral asymmetry, no shorthand where belief equals violence; Core engine-free; JSON authoritative.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. It describes what should exist
and why current repository evidence supports it. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

The expansion's hardest constraint is ethical, not technical: **belief in ASHFALL
is a human coping mechanism under catastrophe, never a stand-in for real religion,
ethnicity, or politics, and never mechanically asymmetric.** `ZealotrySystem`
already encodes this in its header:

> "Models social influence, doctrinal cohesion, charismatic authority, conformity,
> and extremism in wasteland terms only ... mechanically symmetrical (§1.6): no
> real-world faiths, no moral asymmetry, no shorthand where belief = violence."

This expansion inherits that contract and may not violate it. The escalation ladder
still stops at a typed threat event; Core never resolves a killing.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Three movements keep the wasteland upright: the Ash Witnesses who count the dead,
the Rebuilders who count the work, and the Listeners who count the signals. They
have three creeds, nine rituals, five ceremonies, and a liturgical cult that worships
the blue glow of a spent fuel pool.

That is a beautiful beginning and nowhere near enough to carry a campaign whose
social simulation is this deep. Belief in ASHFALL should be the thing survivors do
when arithmetic fails: when the doses are wrong, when the dead are uncounted, when
the radio goes quiet. **The Faithful & The Fractured** expands the belief layer from
three movements into a living religious landscape — shrines, pilgrimage, relics of
contested authenticity, conversions, doctrinal disputes, and schism.

The central engine is simple and dangerous: **a belief that grows can also split.**
A charismatic elder can hold a shelter together or tear it in half. A relic can heal
grief or start a war. A pilgrimage can bind the wasteland together or march its young
people into a hot zone. The player does not choose whether faith exists — only whether
to shelter it, steer it, or fracture it.

### 1.2 What makes this different from a "religion system"

Most games treat faith as a resource bar. ASHFALL's live architecture forbids that.
There is no piety meter. `SpiritualMeaningCoordinator` explicitly "bridges spiritual,
mourning, and ritual content into existing simulation authorities ... without
introducing any parallel faith or piety meters." Faith here produces **meaning and
social pressure**, expressed through morale, grief, friction, and standing — all
owned by systems that already exist.

This expansion therefore adds **content and interpretation**, not a new resource.

### 1.3 The five loops it adds

```
    Belief growth        Conversion        Shrine           Pilgrimage        Schism
    ────────────►       ──────────►       ───────►         ─────────►       ──────►
    fervor rises        conviction        consecrated       hardship          dissent
    per authored        changes per       room grants       transforms        crosses
    ritual & grief      context+profile   ritual site       the pilgrim       threshold
        │                    │                 │                 │              │
        ▼                    ▼                 ▼                 ▼              ▼
    cohesion /          friction          morale &           relic /        new movement
    despair resist      with others       mourning           revelation     or faction
```

### 1.4 What it is not

- Not a missionary conquest game. No movement is "correct." All are symmetrical.
- Not a real-faith reskin. Every creed is invented, specific, and fictional.
- Not a violence pipeline. Extremism produces typed threat events, not killings.
- Not a parallel morale or needs system. It reports into the existing ones.
- Not a new save authority for grief. `MemorialSystem` and `GuiltInsomniaSystem` stay owners.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Spiritual/SpiritualCatalogLoader.cs` | Loads belief/ritual/ceremony catalogs | `LIVE` |
| `Assets/Ashfall.Core/Spiritual/SpiritualMeaningCoordinator.cs` | Ritual cooldown + mourning arc authority | `LIVE` |
| `Assets/Ashfall.Core/Spiritual/SpiritualModels.cs` | Ritual, memorial rite, belief, mourning DTOs | `LIVE` |
| `Assets/Ashfall.Core/Survivors/ZealotrySystem.cs` | Conviction/fervor/dissent, conversion, bounded caps | `LIVE` |
| `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` | Interpersonal friction + conflict groups | `LIVE` |
| `Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs` | Morale truth | `LIVE` |
| `Assets/Ashfall.Core/CombatTraumaSystem.cs` | Trauma truth | `LIVE` |
| `Assets/Ashfall.Core/GuiltInsomniaSystem.cs` | Guilt truth | `LIVE` |
| `Assets/Ashfall.Core/Memorial/` | Memorial authority | `LIVE` |
| `src/Host/ZealotrySaveStore.cs`, `src/Host/SpiritualSaveStore.cs` | Persistence | `LIVE` |
| `src/UI/BeliefsPanel.cs` | Existing belief UI | `LIVE` |
| `src/Main.Zealotry.cs`, `src/Main.Spiritual.cs` | Host wiring | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Entries |
|---|---|
| `wasteland_religions.json` | **3 mechanical profiles** (ash_witnesses, rebuilders, listeners) |
| `belief_movements.json` | **3 movements** with creed, comfort themes, blind spots, practices |
| `spiritual_rituals.json` | **9 rituals** (departure taps, outside crumb, birthday match, roster plate, roll call, first sip pause, empty seat, generator knock, hot zone participation) |
| `ceremonies.json` | **5 ceremonies** (founding day, remembrance vigil, long night bonfire, treaty market, ashfall harvest) |
| `narrative/cobalt_liturgies.json` | Cherenkov-death cult corpus (order of the cobalt flame) |
| `narrative/bunker_rituals_and_cults.json` | Folk rituals (boiler tithe, ash communion) |
| `narrative/cobalt_liturgies_batch_2.json` | Extended liturgy corpus |

### 2.3 Confirmed gaps

- **GAP-13-1 — Three movements is a sample, not a world.** A living belief landscape
  needs many movements with distinct creeds, not three.
- **GAP-13-2 — No pilgrimage authority.** No route, site, hardship, or revelation
  model exists in Core.
- **GAP-13-3 — No relic provenance.** Relics exist only as narrative documents, not
  as contested objects with authenticity.
- **GAP-13-4 — No schism.** Belief movements cannot split, heredity, or form factions.
- **GAP-13-5 — No shrine authoring.** `shrine_room_tags` exist in
  `wasteland_religions.json` but there is no consecration model or shrine catalog.
- **GAP-13-6 — No holy calendar.** Ceremonies exist but there is no shared sacred
  calendar linking them to seasons and observances.
- **GAP-13-7 — Rituals are thin.** Nine rituals across all contexts is far below
  what the trigger vocabulary (`expedition_departure`, `mealtime`, `birthday`,
  `blackout`, `return_muster`, `machine_maintenance`) can carry.
- **GAP-13-8 — No belief-specific locations.** No `loc_` node is a shrine, reliquary,
  or pilgrimage site.

### 2.4 Non-duplication statement

This expansion will **not** add a piety meter, a second morale system, a second
grief owner, a second faction system, a second memorial, or a second conversion
model. `ZealotrySystem` remains the conversion authority; `IdeologicalFrictionSystem`
remains the friction authority; `SpiritualMeaningCoordinator` remains the ritual and
mourning authority. New systems consume their outputs only.

---

## 3. DESIGN PILLARS AND ETHICAL CONTRACT

### 3.1 Ethical contract (hard, non-negotiable)

1. **Fictional only.** No real religion, denomination, scripture, prayer, symbol,
   holiday, figure, or practice is referenced, adapted, or lightly renamed.
2. **Symmetrical.** For every benefit a movement grants, it carries a cost and a
   blind spot. No movement is mechanically superior.
3. **No belief-equals-violence shorthand.** Extremism is a *social pressure* that
   produces typed events (sermons, refusals, exclusions, departures), and only the
   combat authority can resolve violence — and it should be rare and costly.
4. **No conversion by force.** Conversion is context + authored profile + host-forked
   RNG, exactly as `ZealotrySystem` defines. A player may create conditions; a player
   may not click a "convert" button.
5. **Belief is not ethnicity or politics.** Movements are chosen, argued, and left.
6. **The player is not a god.** The player shelters, steers, or fractures a movement;
   the player does not author its truth.

### 3.2 Design pillars

**Pillar 1 — Meaning under arithmetic.** Faith appears when the numbers fail: an
unrecorded death, an unknown dose, a silent radio. The expansion ties every belief
system to an existing uncertainty.

**Pillar 2 — Faith is social, not personal.** `IdeologicalFrictionSystem` already
models friction. Belief content should create *rooms that don't sleep together
well*, not private stat bars.

**Pillar 3 — Cost in the open.** Every doctrine grants cohesion and costs agency.
The Ash Witnesses comfort grief but encourage fatalism. The Rebuilders drive
production but despise the injured. The Listeners give hope but risk disappointment.

**Pillar 4 — Schism is a feature.** A movement that cannot split is a static label.
The expansion's signature mechanic is the fracture.

**Pillar 5 — Relics are contested.** A "relic" is only as real as the community
that believes it. Provenance can be true, forged, misattributed, or unknown — and
the truth matters less than the dispute.

### 3.3 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A sermon | A tired voice, a practical lesson | Preacher drama, miracles |
| A shrine | A corner with a cracked lamp and a memento | Gothic cathedral |
| A pilgrimage | Blistered feet, shared water, doubt | Holy vision, glowing sign |
| A relic | A bent spoon someone died holding | Magic artifact |
| A schism | Two people who still love each other, disagreeing | Heresy trial, burning |

---

## 4. THE BELIEF LANDSCAPE

### 4.1 The three live movements (unchanged canon)

| Movement | Creed kernel | Comfort | Blind spot |
|---|---|---|---|
| Ash Witnesses | Ash is proof; forgetting invites the fire | Language for moral injury | Fatalism; illness as penance |
| Rebuilders | Survival without creation is waiting | Agency; grief into craft | Work as avoidance; contempt for the injured |
| Listeners | Signals are hope; the air remembers | Patience; shared waiting | Disappointment; passivity |

These three remain the entry canon. The expansion authoring **deepens** them and
adds new movements without altering the existing three rows (IDs preserved).

### 4.2 Proposed new movements (12)

| ID | Name | Creed kernel | Signature tension |
|---|---|---|---|
| `belief_quiet_order` | The Quiet Order | Noise draws ruin; silence is safety | Extreme quietism vs. necessary alarms |
| `belief_ledger_saints` | The Ledger Saints | Every name must be written; the unwritten are lost | Record-keeping vs. mercy |
| `belief_cinder_makers` | The Cinder Makers | Fire cleans; controlled burning prevents wildfire | Arson risk vs. fuel economy |
| `belief_deep_breath` | The Deep Breath | The air is a gift that must be rationed | Air discipline vs. medical need |
| `belief_promised_return` | The Promised Return | Those who left will come back; keep the light on | Hope vs. wasted power |
| `belief_stone_mothers` | The Stone Mothers | The shelter itself is a living thing to be fed | Building worship vs. maintenance |
| `belief_third_shift` | The Third Shift | There is a hidden hour when the dead work | Night superstition vs. production |
| `belief_cold_arithmetic` | The Cold Arithmetic | Only what can be counted should be loved | Ruthless optimization vs. kinship |
| `belief_open_gate` | The Open Gate | The door must never be sealed against the living | Charity vs. security |
| `belief_ash_gardeners` | The Ash Gardeners | Something can grow from anything; patience is holy | Slow hope vs. hunger now |
| `belief_last_words` | The Last Words | What is said before death is binding | Oral authority vs. written law |
| `belief_iron_silence` | The Iron Silence | Machines have heard everything; do not lie near them | Surveillance paranoia vs. trust |

Each movement is authored in `belief_movements.json` using the live schema
(`id`, `display_name`, `latin_name`, `creed`, `comfort_themes`, `blind_spot_themes`,
`key_practices`, `conflict_profiles`, `tags`) plus a mechanical profile in
`wasteland_religions.json`.

### 4.3 The cult as a distinct class

`cobalt_liturgies.json` establishes the Order of the Cobalt Flame — a radiation-
death cult. The expansion formalizes a **cult** as a movement with three extra
properties: a *sacrament* (a dangerous physical rite), a *sacred threshold*
(a measurable exposure), and *martyr memory*. Cults are represented in the same
movement catalog with a `movement_class: "cult"` discriminator plus authored
sacraments in a new `cult_sacraments.json`. No new Core model is required; the
existing zealotry and ritual systems carry the behavior.

### 4.4 Movement lifecycle

```
Seed (lore) → Gathering (small fervor) → Established (shrine, converts)
     → Contested (friction with another movement) → Schism
     → { reform, splinter, dissolve, or martyrdom }
```

Lifecycle transitions are deterministic functions of fervor, dissent, cohesion,
and authored thresholds. The host surfaces the stage; Core computes it.

---

## 5. NEW WORLD REGIONS AND LOCATIONS

Locations attach to `locations.json` (canonical). Rooms attach to
`shelter_rooms.json` using `shrine_room_tags` from the religion profile.

### 5.1 Interior rooms

- **`room_shrine_quiet`** — a closet of silence; grant ritual site, low power.
- **`room_shrine_hearth`** — a communal fire shrine; couples to fuel cost.
- **`room_reliquary`** — a locked cabinet of contested objects; powers relic rites.
- **`room_pilgrim_dormitory`** — lodging for pilgrims passing through.
- **`room_meditation_walk`** — a looped corridor for walking meditation.
- **`room_liturgy_hall`** — for ceremonies and schism debates.

### 5.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_witness_cairn` | The Counting Cairn | 4 | A cairn of numbered stones; Ash Witness pilgrimage |
| `loc_rebuilder_yard` | The Unfinished Yard | 5 | A half-built monument; Rebuilder pilgrimage |
| `loc_listener_mast` | The Listening Mast | 6 | A bent radio mast; Listener pilgrimage |
| `loc_cobalt_basin` | The Blue Basin | 9 | Spent fuel pool; cobalt cult sacrament site |
| `loc_third_shift_tunnel` | The Hidden Hour | 7 | The "dead work" tunnel of the Third Shift |
| `loc_open_gate_ruins` | The Gate That Stood Open | 5 | A collapsed checkpoint kept open by belief |
| `loc_ash_garden` | The Garden in Ash | 3 | A tended plot in dead ground |
| `loc_schism_chapel` | The Divided Chapel | 6 | A chapel split by a wall built through it |
| `loc_relic_road` | The Relic Road | 7 | A trade road of relic-sellers and forgers |
| `loc_last_words_wall` | The Wall of Last Words | 4 | A wall of names and final sentences |

All locations require valid item references and must be registered with
`ContentUtilizationScanner`. No location may exceed the established danger scale.

### 5.3 Pilgrimage route

A pilgrimage is a typed expedition variant, not a new travel system. It reuses
`ExpeditionSystem` and `LocationLayoutSystem`, but adds authored *stages* (departure
rite, road hardship, site arrival, site rite, return vow) and hardship events that
consume real supplies. `PilgrimageSystem` (proposed) owns only the stages and
revelation eligibility; travel is still owned by `ExpeditionSystem`.

---

## 6. MAIN STORYLINE — "THE NINTH CREED"

### 6.1 Central conflict

A stranger arrives at the shelter carrying a slate written in a hand nobody
recognizes. It contains a creed that is, word for word, a merger of the Ash
Witnesses and the Rebuilders — but with one line added: *"The dead are not
witnesses. They are workers who have not been given their task."*

The movement that forms around this text, the **Ninth Creed**, grows fast and
splits the shelter. Its comfort is real: it gives the grieving a way to keep working
with the dead. Its danger is also real: it pressures survivors to treat grief as a
labor roster, and it devalues the recovery of the living.

The player must decide whether the Ninth Creed is a mercy, a madness, or a mirror —
and whether to shelter it, shape it, or fracture it. The expansion's name refers
to the player's own role: faith that grows will also fracture.

### 6.2 Theme (unspoken)

**Any belief strong enough to hold a shelter together is strong enough to tear it
apart. The skill is not choosing the right faith; it is choosing what to do when it
cracks.**

### 6.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_pilgrim_hesk_avor` | Hesk Avor | Stranger, slate-bearer | Ninth Creed founder; sincere, not sinister |
| `npc_elder_mara_sohn` | Elder Mara Sohn | Ash Witness elder | Defends orthodoxy; risks rigidity |
| `npc_deacon_tov_rik` | Deacon Tov Rik | Rebuilder deacon | Sees the Creed as theft |
| `npc_witness_ila_venn` | Ila Venn | Listener convert | Bridges movements; the peacemaker |
| `npc_relic_broker_oren_das` | Oren Das | Relic broker | Forger/seller; morally grey |
| `npc_sister_hale_ash` | Sister Hale | Cobalt cult acolyte | Sacrament; radiation hazard |
| `npc_questioner_pell` | Pell | Doubter | Represents the atheist/uncertain voice |
| `npc_child_novice_senna` | Senna | Young novice | The next generation's stake |

NPCs live in `characters.json` (canonical) and, when recruitable, in `survivors.json`.

### 6.4 Story beats (14)

1. **The Slate.** A stranger arrives with an unrecognized creed.
2. **First Reading.** The shelter hears the text; reactions split by existing belief.
3. **The Dead Roster.** A survivor begins assigning "work" to the dead; a grief event.
4. **The Rebuilder Refusal.** Deacon Tov refuses to share the foundry with the Creed.
5. **The Counting Cairn.** First pilgrimage; hardship on the road.
6. **The Relic.** A relic surfaces that seems to confirm the Creed.
7. **Provenance.** The broker's forgery is suspected; the truth is ambiguous.
8. **The First Schism.** The Listeners split over the Creed's quietism.
9. **The Blue Basin.** A cobalt sacrament goes wrong; a casualty.
10. **The Divided Chapel.** A wall is built through a shared shrine.
11. **The Ninth Question.** A grief-driven crisis forces a public debate.
12. **The Fracture.** The movement either reforms, splinters, or dissolves.
13. **The Relic Road.** Resolve the provenance and its consequences.
14. **The Creed Kept.** The player chooses what remains of the Ninth Creed.

### 6.5 Branching choices (7)

| Choice | Options | Axis |
|---|---|---|
| Shelter the Creed | shelter / tolerate / expel | growth vs. cohesion |
| Creed and the foundry | share / refuse / rotate | production vs. friction |
| Relic provenance | expose / confirm / stay silent | truth vs. stability |
| Cobalt sacrament | permit / restrict / ban | faith vs. safety |
| Shrine split | rebuild / divide / abandon | unity vs. purity |
| Pilgrimage policy | encourage / ration / forbid | meaning vs. resources |
| Final disposition | reform / splinter / dissolve | legacy |

### 6.6 Endings (5 + fade)

1. **The Ninth Creed Kept** — reform; the Creed becomes a moderate movement.
2. **The Two Chapels** — permanent splinter; two movements, half the cohesion.
3. **The Empty Roster** — the Creed dissolves; grief returns unmanaged.
4. **The Blue Silence** — cult martyrdom; the shelter gains a dark relic and loses people.
5. **The Question Left Open** — the player refuses; the debate continues in the epilogue.
6. **Fade** — the stranger leaves as quietly as they came.

Endings append to `campaign_epilogues.json` and `epilogue_chronicle.json`; no new
ending authority.

---

## 7. QUEST DESIGN

New IDs use the prefix `quest_faith_`. Schema follows `year_of_ash_quests.json`.

### 7.1 Main questline (14)

`quest_faith_the_slate`, `quest_faith_first_reading`, `quest_faith_dead_roster`,
`quest_faith_rebuilder_refusal`, `quest_faith_counting_cairn`,
`quest_faith_the_relic`, `quest_faith_provenance`, `quest_faith_first_schism`,
`quest_faith_blue_basin`, `quest_faith_divided_chapel`,
`quest_faith_ninth_question`, `quest_faith_the_fracture`,
`quest_faith_relic_road`, `quest_faith_creed_kept`.

### 7.2 Side quests (24)

**Ritual and comfort (5)**
- `quest_faith_new_ritual` — author a shelter ritual for a specific grief
- `quest_faith_ritual_clash` — two rituals claim the same trigger
- `quest_faith_broken_cooldown` — a ritual performed too often loses meaning
- `quest_faith_shared_silence` — a joint observance of two movements
- `quest_faith_offering_shortage` — ritual offerings consume scarce supplies

**Shrine (4)**
- `quest_faith_consecrate_room` — convert a room to a shrine
- `quest_faith_shrine_power` — the shrine draws power the grid needs
- `quest_faith_shrine_theft` — a sacred object is stolen
- `quest_faith_twin_shrines` — two movements demand the same room

**Pilgrimage (5)**
- `quest_faith_first_pilgrim` — sponsor one pilgrim
- `quest_faith_road_supply` — provision the route
- `quest_faith_lost_pilgrim` — a pilgrim does not return
- `quest_faith_site_claim` — two movements claim one site
- `quest_faith_return_vow` — a pilgrim's binding promise

**Relic (4)**
- `quest_faith_forged_relic` — a forgery is exposed
- `quest_faith_true_relic` — a relic is authenticated
- `quest_faith_relic_custody` — who keeps the object
- `quest_faith_relic_trade` — sell or keep

**Conversion and friction (4)**
- `quest_faith_quiet_conversion` — a survivor converts privately
- `quest_faith_split_family` — a family divided by belief
- `quest_faith_public_confession` — a public conversion ritual
- `quest_faith_doubt` — a believer asks to leave

**Cult and safety (2)**
- `quest_faith_sacrament_guard` — supervise a dangerous rite
- `quest_faith_threshold_warning` — a sacrament approaches a lethal exposure

### 7.3 Repeatable quests (6)

`quest_faith_repeat_observance`, `quest_faith_repeat_sermon`,
`quest_faith_repeat_pilgrim_meal`, `quest_faith_repeat_relic_appraisal`,
`quest_faith_repeat_counsel`, `quest_faith_repeat_vigil`.

### 7.4 Dynamic hooks

The existing generator can be extended with belief triggers: fervor threshold,
dissent threshold, conversion event, ritual skipped, mourning stage advanced,
schism threshold. All are typed facts; the host decides presentation.

### 7.5 Content constraints

- No quest may grant a conversion deterministically.
- No quest may reward violence against a movement.
- Cult sacrament quests must route exposure through `RadiationSystem` and injury
  through the medical pipeline; the rite itself never kills directly.
- A schism must be possible for any movement, not only the Ninth Creed.

---

## 8. NEW GAMEPLAY SYSTEMS

### 8.1 `PilgrimageSystem` (new, `Ashfall.Core.Spiritual`)

**Owns:** pilgrimage definitions, stages, hardship events, revelation eligibility.
**Consumes:** `ExpeditionSystem` travel, `NeedsSystem` supplies, `DiseaseSystem`
road illness, `RadiationSystem` site exposure.
**Data:** `pilgrimage_routes.json`, `pilgrimage_sites.json`.
**Determinism:** hardship rolls use the host-forked `ISeededRng`.

```csharp
public sealed class PilgrimageSystem
{
    public bool Begin(string pilgrimId, string routeId, int day);
    public PilgrimageStageResult Advance(int day);
    public bool TryRevelation(string pilgrimId, string siteId);
}
```

### 8.2 `SchismSystem` (new, `Ashfall.Core.Spiritual`)

**Owns:** dissent accumulation, schism threshold evaluation, splinter movement
creation, and coexistence penalties. It does **not** own faction standing
(`FactionStanceEngine`) or friction (`IdeologicalFrictionSystem`).
**Data:** `schisms.json`.
**Rules:** a schism creates a new movement ID derived from the parent; both movements
remain symmetrical; schism is deterministic and once per parent unless authored otherwise.

### 8.3 `RelicProvenanceSystem` (new, `Ashfall.Core.Culture`)

**Owns:** relic identity, claimed provenance, true provenance (separate fields),
authentication state, and rite eligibility. It does not own inventory; relics are items.
**Data:** `sacred_relics.json`.
**Rules:** authenticity is a state (`true`, `forged`, `misattributed`, `unknown`);
the system never tells the player the truth for free; exposing it is a quest action.

### 8.4 `ShrineConsecrationSystem` (new, `Ashfall.Core.Spiritual`)

**Owns:** which room is consecrated to which movement, shrine tier, and effective
ritual site. Consumes construction/room authority and power. **Data:** `shrines.json`.

### 8.5 `SacredCalendarSystem` (new, thin, `Ashfall.Core.Spiritual`)

**Owns:** holy days, observance windows, and which ceremonies are in season.
Consumes `Clock`/`YearOfAsh` calendar. **Data:** `holy_days.json`.

### 8.6 `DoctrinalDisputeSystem` (new, thin, `Ashfall.Core.Spiritual`)

**Owns:** authored disputes (interpretation contests) and their deterministic
resolution by conviction, rhetoric, and authored weights. Produces typed facts for
friction and standing. **Data:** `doctrinal_disputes.json`.

### 8.7 Systems explicitly not added

- No piety meter.
- No second conversion model (`ZealotrySystem` owns conversion).
- No second faction or friction model.
- No second memorial or grief model.
- No missionary combat system.
- No real-world-faith content.

---

## 9. DATA CATALOG SPECIFICATION

All catalogs are snake_case, integer `schema_version: 1`, validated by
`CatalogIntegrityValidator`, and registered with `ContentUtilizationScanner`.

### 9.1 `wasteland_religions.json` (extend 3 → 12)

Additive. Existing fields preserved: `belief_id`, `display_name`, `doctrine_tags`,
`conversion_base_bp`, `fervor_daily_decay_bp`, `fervor_ritual_gain_bp`,
`cohesion_bonus_bp`, `despair_resistance_bp`, `fanaticism_threshold`,
`dissent_tolerance`, `ritual_resource_item_ids`, `shrine_room_tags`,
`broadcast_profile`, `tags`. New optional fields: `movement_class`
(`movement`|`cult`|`heresy`), `schism_parent_id`, `sacrament_id`, `holy_day_ids`.

### 9.2 `belief_movements.json` (extend 3 → 15)

Existing schema preserved; 12 new movements as listed in §4.2.

### 9.3 `spiritual_rituals.json` (extend 9 → 60)

Existing schema preserved (`id`, `title`, `category`, `description`,
`context_trigger`, `morale_delta`, `friction_flag`, `is_optional`, `cooldown_days`,
`tags`). Proposed contexts and counts:

| Context | New rituals |
|---|---|
| `expedition_departure` | 4 |
| `mealtime` | 5 |
| `birthday` | 3 |
| `blackout` | 4 |
| `return_muster` | 4 |
| `machine_maintenance` | 5 |
| `birth` | 3 |
| `death` | 5 |
| `schism` | 3 |
| `relic` | 4 |
| `pilgrimage` | 4 |
| `conversion` | 3 |
| `harvest` | 3 |

### 9.4 `ceremonies.json` (extend 5 → 20)

Existing schema preserved. 15 new ceremonies across movements, seasons, and
thresholds (founding, mourning, harvest, treaty, schism reconciliation, relic
procession, pilgrimage departure/return, cult vigil, silence day, open-gate day).

### 9.5 `pilgrimage_sites.json` (new)

```json
{
  "schema_version": 1,
  "sites": [
    {
      "site_id": "site_counting_cairn",
      "display_name": "The Counting Cairn",
      "location_id": "loc_witness_cairn",
      "movement_id": "belief_ash_witnesses",
      "danger_level": 4,
      "travel_hours": 14,
      "hardship_profile": "road_exposed",
      "revelation_pool_id": "revelation_witness_memory",
      "required_offering": [{ "item_id": "item_counting_stone", "quantity": 1 }],
      "tags": ["pilgrimage", "cairn", "memory"]
    }
  ]
}
```

### 9.6 `pilgrimage_routes.json` (new)

Ordered site lists with supply windows and hardship weights.

### 9.7 `sacred_relics.json` (new)

```json
{
  "schema_version": 1,
  "relics": [
    {
      "relic_id": "relic_bent_spoon",
      "display_name": "The Bent Spoon",
      "claimed_provenance": "used by a founder on the last ration",
      "true_provenance": "unknown",
      "authenticity": "unknown",
      "movement_ids": ["belief_ash_witnesses"],
      "rite_id": "ritual_relic_procession",
      "grief_effect_bp": 150,
      "dispute_severity": 40,
      "tags": ["relic", "memory", "contested"]
    }
  ]
}
```

### 9.8 `shrines.json` (new)

Room-to-movement consecration, tier, power draw, and ritual site effects.

### 9.9 `holy_days.json` (new)

Observance names, calendar anchors, movement affiliation, and ceremony mapping.

### 9.10 `schisms.json` (new)

Parent movement, dissent threshold, splinter ID, creed delta, and coexistence rules.

### 9.11 `doctrinal_disputes.json` (new)

Dispute statements, positions, and deterministic resolution weights.

### 9.12 `cult_sacraments.json` (new)

Dangerous rites with a measurable threshold (for example exposure in cpm), guard
requirements, and medical consequence routing.

### 9.13 Items

New items appended to `items.json`: `item_counting_stone`, `item_pilgrim_staff`,
`item_shrine_lamp`, `item_votive_cloth`, `item_relic_case`, `item_liturgy_slate`,
`item_sacrament_lead_veil`, `item_open_gate_key`, `item_cairn_chisel`,
`item_last_word_token`, `item_offering_oil`, `item_ash_garden_seed`.

---

## 10. SAVE, DETERMINISM, AND PERSISTENCE

### 10.1 Ownership

Belief state is captured by `src/Host/ZealotrySaveStore.cs` and
`src/Host/SpiritualSaveStore.cs`. New sub-objects are additive inside the existing
envelopes. No new save section and no new slot root.

### 10.2 State to persist

- Movement membership and conviction per survivor (`ZealotrySystem` already owns).
- Shrine consecrations (new sub-object).
- Relic provenance and custody (new sub-object; relics also live in inventory).
- Pilgrimage in-progress records and vows.
- Schism history and derived movements.
- Holy-day observance flags (derived from calendar; only performance flags persist).

### 10.3 Determinism

- Conversion randomness already uses the host-forked RNG; new systems must do the same.
- Schism evaluation is a pure function of dissent/fervor and authored thresholds.
- Relic authentication never rolls publicly; it is a state transition.
- Paired replay hashes must match across continuous and interrupted runs.

### 10.4 Migration

Legacy saves load with no movements, no shrines, no relics, and no schisms. No
movement is invented. `SpiritualMeaningCoordinator`'s existing mourning arcs and
ritual cooldowns are untouched.

### 10.5 Checksum

All new floats are invariant-culture serialized; prefer integer-permille.

---

## 11. UI, ACCESSIBILITY, AND PRESENTATION

### 11.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `BeliefsPanel` (extend) | Movements, conviction, friction, dissent | `FaithHostSession` |
| `ShrinePanel` (new) | Consecration, tier, power, ritual site | same |
| `PilgrimagePanel` (new) | Routes, sites, hardship, vows | same |
| `ReliquaryPanel` (new) | Relics, claimed vs. true provenance (gated) | same |
| `SacredCalendarPanel` (new) | Holy days and upcoming observances | same |
| `SchismPanel` (new) | Dissent, splinter preview, coexistence | same |
| `MemorialPanel` (extend) | Belief-specific rites | `MemorialSystem` host |

### 11.2 Accessibility and honesty

- The panel shows *current* state and exposes existing commands; it never fakes
  a route.
- Keyboard/controller close and back behavior is preserved.
- No info by color alone; contrast and text scaling respected.
- Probability and threshold previews are expressed in-world, never as debug numbers.
- Provenance is shown as the community believes it, not as the engine knows it,
  until a quest exposes the truth.

### 11.3 Presentation

- Audio cues appended to `audio_cues.json`: distant hymn, stone on stone, a struck
  iron note, a lamp lit, a pilgrim's step, a schism silence.
- No cue is required; text always carries meaning. The cobalt sacrament has no
  triumphant cue — only a Geiger pulse and a held breath.

---

## 12. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `SpiritualMeaningCoordinator` | Extended with pilgrimage and shrine ritual execution only via its public methods |
| `ZealotrySystem` | Consumes/feeds conviction, fervor, dissent; never duplicated |
| `IdeologicalFrictionSystem` | Schism and conversion produce friction facts |
| `MoraleContagionSystem` | Observances and schisms emit morale facts |
| `MemorialSystem` | Rites route through existing memorial semantics |
| `GuiltInsomniaSystem` | Relic/doubt outcomes emit guilt facts |
| `FactionStanceEngine` | Movements can align with factions; standing effects |
| `ExpeditionSystem` | Pilgrimage travel reuses expedition resolution |
| `RadiationSystem` | Cult sacrament exposure is real radiation |
| `MedicalPipelineCoordinator` | Sacrament injury is real injury |
| `Construction`/rooms | Shrine consecration is a room property |
| `PowerGridSystem` | Shrine lamps and reliquary draw real watts |
| `Inventory` | Relics and offerings are normal items |
| `WeatherSystem` | Pilgrimage road hardship reads weather |

---

## 13. TECHNICAL IMPLEMENTATION PLAN

### 13.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `ZealotrySystem`, `SpiritualMeaningCoordinator`,
`IdeologicalFrictionSystem`, save stores, and `BeliefsPanel` still match. Record file:line.

**Phase 1 — Data + validators.** Extend the three belief catalogs; author
pilgrimage, relics, shrines, holy days, schisms, disputes, sacraments. Register
validators and scanner. No gameplay.

**Phase 2 — Pure Core.** `PilgrimageSystem`, `SchismSystem`, `RelicProvenanceSystem`,
`ShrineConsecrationSystem`, `SacredCalendarSystem`, `DoctrinalDisputeSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `FaithHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Seven surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 30/90/180-day soak including schism and pilgrimage pressure.

**Phase 8 — Verification and closeout.**

### 13.2 Content volume

| Content | Count |
|---|---|
| Movements | 12 new (3 → 15) |
| Religion profiles | 9 new (3 → 12) |
| Rituals | 51 new (9 → 60) |
| Ceremonies | 15 new (5 → 20) |
| Pilgrimage sites | 12 |
| Pilgrimage routes | 5 |
| Relics | 20 |
| Shrines | 6 room types |
| Holy days | 18 |
| Schism templates | 8 |
| Disputes | 15 |
| Sacraments | 6 |
| Locations | 10 |
| Rooms | 6 |
| NPCs | 8 |
| Main quests | 14 |
| Side quests | 24 |
| Repeatable | 6 |
| Items | 12 |
| Endings | 5 + fade |
| Prose estimate | 50,000–65,000 words |

### 13.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Real-world faith bleed | Critical | Fictional-only review gate before authoring |
| Moral asymmetry | High | Symmetry audit per movement |
| Belief=violence shorthand | High | Escalation stops at typed event; no reward for violence |
| Conversion determinism | High | Context+profile+RNG only |
| Save bloat | Medium | Compact state, derived where possible |
| Duplicate friction model | High | Consume `IdeologicalFrictionSystem` only |
| Ritual spam exploit | Medium | Cooldowns already enforced; extend, never bypass |
| Tone drift to church gothic | Medium | Tone table §3.3 enforced in review |

---

## 14. TEST AND VERIFICATION PLAN

### 14.1 New test files

- `Ashfall.Core.Tests/Spiritual/PilgrimageSystemTests.cs`
- `Ashfall.Core.Tests/Spiritual/SchismSystemTests.cs`
- `Ashfall.Core.Tests/Spiritual/ShrineConsecrationTests.cs`
- `Ashfall.Core.Tests/Spiritual/SacredCalendarTests.cs`
- `Ashfall.Core.Tests/Culture/RelicProvenanceTests.cs`
- `Ashfall.Core.Tests/Spiritual/FaithSaveRoundTripTests.cs`
- `Ashfall.Core.Tests/Spiritual/FaithDeterminismTests.cs`
- `Ashfall.Core.Tests/Content/BeliefCatalogIntegrityTests.cs`

### 14.2 Required assertions

- Symmetry: every movement's benefit has a cost; no dominant strategy.
- Fictional audit: no catalog string matches a real-world religious term list.
- Conversion: no deterministic path; bounded by `ZealotryCaps`.
- Schism: deterministic, once per parent unless authored, both movements valid.
- Relic: claimed vs. true provenance separate; exposing is a quest action only.
- Pilgrimage: supplies consumed; exposure routed to `RadiationSystem`.
- Sacrament: never kills directly; injury and exposure are real and routed.
- Round-trip: shrines, relics, pilgrimages, schisms restore exactly.
- Legacy: neutral load, no invented movements.
- Determinism: paired replay hash equality.

### 14.3 Commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Spiritual/
bash scripts/run_test.sh Ashfall.Core.Tests/Culture/
godot --headless --path . -- --spiritual-selftest
godot --headless --path . -- --zealotry-selftest
godot --headless --path . -- --ceremony-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
dotnet build Ashfall.csproj --no-restore
```

---

## 15. ACCEPTANCE CRITERIA

A component is done only when: Core authority documented and engine-free; data in
canonical JSON with valid `schema_version` and passing integrity; persistence
round-trips with neutral legacy load and Triad parity; determinism proven by paired
replay; host reachable by a real route or event; player can observe the outcome;
focused tests green; docs updated; and the ethical contract (§3.1) verified. A
compile-green result is not acceptance.

---

## 16. CROSS-EXPANSION HOOKS (WAVE 1)

| Expansion | Hook |
|---|---|
| 12 The Second Generation | Coming-of-age rites can be religious; movements recruit youth |
| 14 Above the Ash | The Listeners' mast; aircraft as "signs"; sky-god cult variant |
| 15 The Deep Root | Ash Gardener movement; garden shrines |
| 16 The Rebuilt Body | Machine reverence; the Iron Silence movement |

---

## 17. LORE AND CONTINUITY CHECK

### 17.1 Must not contradict

- The three existing movements and their creeds.
- The no-piety-meter contract of `SpiritualMeaningCoordinator`.
- The symmetry contract of `ZealotrySystem`.
- The fiction-only tone rule.
- The dual faction ID namespace.

### 17.2 New canon

- The Ninth Creed and its slate.
- Pilgrimage sites and the Relic Road.
- The schism mechanic as an in-world event class.
- The cult sacrament vocabulary.

### 17.3 Provenance

New canon registered in `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` when integrated,
with the owning file and proving test.

---

## 18. APPENDIX A — MOVEMENT MECHANICAL MATRIX

| Movement | Class | Cohesion bp | Despair resist bp | Conversion bp | Fervor decay | Dissent tolerance | Cost axis |
|---|---|---|---|---|---|---|---|
| Ash Witnesses | movement | 600 | 1600 | 1400 | 450 | low | fatalism |
| Rebuilders | movement | 1200 | 900 | 1100 | 380 | medium | injured neglected |
| Listeners | movement | 700 | 1300 | 900 | 500 | high | passivity |
| Quiet Order | movement | 900 | 1100 | 800 | 300 | low | alarms suppressed |
| Ledger Saints | movement | 800 | 1000 | 1000 | 420 | medium | mercy delayed |
| Cinder Makers | movement | 1000 | 1200 | 1200 | 360 | low | fire risk |
| Deep Breath | movement | 700 | 900 | 900 | 480 | medium | air rationing |
| Promised Return | movement | 850 | 1400 | 950 | 520 | high | power wasted |
| Stone Mothers | movement | 1100 | 950 | 1000 | 340 | low | maintenance diverted |
| Third Shift | movement | 750 | 1050 | 850 | 410 | medium | night production |
| Cold Arithmetic | movement | 950 | 700 | 1300 | 300 | low | kinship devalued |
| Open Gate | movement | 800 | 1000 | 1400 | 460 | high | security risk |
| Ash Gardeners | movement | 900 | 1100 | 1000 | 400 | medium | slow returns |
| Last Words | movement | 850 | 1200 | 950 | 440 | medium | oral over written |
| Iron Silence | cult | 1000 | 1300 | 1100 | 350 | low | surveillance paranoia |
| Cobalt Flame | cult | 1300 | 1500 | 900 | 300 | low | radiation hazard |

All values are authored and bounded; no movement may exceed the live cap ranges.

---

## 19. APPENDIX B — RITUAL TRIGGER MATRIX

| Trigger | Existing | Proposed | Example new ritual |
|---|---|---|---|
| `expedition_departure` | 1 | 4 | tying a knot in the door rope |
| `mealtime` | 1 | 5 | passing the salt hand to hand |
| `birthday` | 1 | 3 | the borrowed year token |
| `blackout` | 0 | 4 | naming the dark out loud |
| `return_muster` | 1 | 4 | counting heads twice |
| `machine_maintenance` | 1 | 5 | oil poured in a spiral |
| `birth` | 0 | 3 | the first-name held back |
| `death` | 1 | 5 | the empty bunk made once |
| `schism` | 0 | 3 | the shared last meal |
| `relic` | 0 | 4 | the relic walked around the room |
| `pilgrimage` | 0 | 4 | the staff left at the door |
| `conversion` | 0 | 3 | the old token handed over |
| `harvest` | 0 | 3 | the first grain set aside |

Every ritual keeps `is_optional` true unless authored otherwise, and every ritual
has a cooldown. Rituals never grant unbounded morale; deltas stay small and capped,
consistent with the live nine.

---

## 20. APPENDIX C — RELIC PROVENANCE STATES

| State | Meaning | Discovery path | Effect |
|---|---|---|---|
| `true` | Provenance is accurate | quest authentication | strongest grief relief |
| `misattributed` | Real object, wrong story | records cross-check | moderate; dispute risk |
| `forged` | Deliberately false | broker investigation | scandal; trust loss |
| `unknown` | No one knows | default | weak; community decides |

The engine always knows `authenticity`; the UI shows `claimed_provenance` until a
quest transitions the visible state. This preserves uncertainty as a real mechanic.

---

## 21. APPENDIX D — SACRED CALENDAR (18 HOLY DAYS PROPOSED)

| Day (campaign) | Observance | Movements |
|---|---|---|
| 10 | First Counting | Ash Witnesses |
| 25 | The Unfinished Hour | Rebuilders |
| 40 | The Long Listen | Listeners |
| 55 | Silence Day | Quiet Order |
| 70 | The Blank Page | Ledger Saints |
| 85 | Controlled Burn | Cinder Makers |
| 100 | The Rationed Breath | Deep Breath |
| 120 | The Light Kept | Promised Return |
| 135 | Foundation Vigil | Stone Mothers |
| 150 | The Hidden Hour | Third Shift |
| 165 | The Weighed Day | Cold Arithmetic |
| 180 | The Gate Open | Open Gate |
| 200 | First Green | Ash Gardeners |
| 215 | The Words Kept | Last Words |
| 240 | The Muted Machine | Iron Silence |
| 260 | The Blue Vigil | Cobalt Flame |
| 300 | The Ninth Reading | Ninth Creed |
| 360 | Year Turn | all movements |

Observances are informative, not compulsory; skipping one carries a small cohesion
cost, not a punitive one.

---

## 22. APPENDIX E — ECONOMY AND BALANCE MODEL

- **Offerings** consume real items (`item_offering_oil`, food, water). The cost is
  small per observance but accumulates over a campaign.
- **Shrines** draw power; a shrine in a blackout loses its tier bonus. This makes
  faith compete with the grid, which is the intended pressure.
- **Pilgrimage** consumes supplies and travel days; the return is meaning, a relic
  chance, and cohesion — never a resource jackpot.
- **Relics** have trade value but selling a sacred object has social cost.
- **Schism** costs cohesion and can cost population if a splinter leaves.
- No movement is a net resource positive; the return is morale, grief relief, and
  social resilience, which are their own currencies in this game.

---

## 23. APPENDIX F — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `belief_movements.json` | +12 | 8,000 |
| `wasteland_religions.json` | +9 | 2,000 |
| `spiritual_rituals.json` | +51 | 10,000 |
| `ceremonies.json` | +15 | 5,000 |
| `pilgrimage_sites.json` | 12 | 3,000 |
| `pilgrimage_routes.json` | 5 | 1,500 |
| `sacred_relics.json` | 20 | 3,500 |
| `shrines.json` | 6 | 1,200 |
| `holy_days.json` | 18 | 2,500 |
| `schisms.json` | 8 | 2,000 |
| `doctrinal_disputes.json` | 15 | 3,000 |
| `cult_sacraments.json` | 6 | 2,000 |
| Quest objectives | 44 quests | 13,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 12 | 1,800 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~72,000** |

Trim to the 50–65k target during authoring by consolidating minor ritual rows.

---

## 24. APPENDIX G — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R13-1 | Real-world faith bleed | Low | Critical | Fictional-only review gate |
| R13-2 | Moral asymmetry | Med | High | Symmetry audit |
| R13-3 | Belief=violence shorthand | Med | High | Typed events only; no reward |
| R13-4 | Conversion determinism | Low | High | Context+profile+RNG |
| R13-5 | Duplicate grief owner | Low | High | Consume `MemorialSystem` |
| R13-6 | Ritual exploit | Low | Med | Cooldowns; capped deltas |
| R13-7 | Save bloat | Med | Med | Derived calendar, compact state |
| R13-8 | Tone drift | Med | Med | Tone table review |
| R13-9 | Content overrun | Med | Med | Budget §23 |
| R13-10 | Schism trivialized | Med | Med | Real dissent threshold; population cost |

---

## 25. APPENDIX H — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **How many new movements ship in Wave 1?** Recommended 12, but 6 is a safe start.
2. **Cult sacrament lethality ceiling** — may a self-chosen rite kill? Recommended:
   exposure and injury yes; direct scripted death no.
3. **Relic truth exposure** — can the player ever learn `true_provenance`? Recommended:
   yes, through one quest line only.
4. **Schism population cost** — does a splinter physically leave the shelter?
   Recommended: optionally, as a typed departure event, never automatic.
5. **Pilgrimage ambient content** — how much road prose? Recommended: 3 sites fully
   authored, 9 lightweight, to protect the budget.

---

## 26. APPENDIX I — MAIN QUESTLINE STAGE DETAIL

Each quest uses the live `stages[]` schema. Below are the authored stage counts and
objective kernels; full prose is authored in Phase 6.

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_faith_the_slate` | 3 | Hear the stranger; read the slate; decide whether to share it |
| `quest_faith_first_reading` | 4 | Gather reactions; record positions; face the first refusal |
| `quest_faith_dead_roster` | 3 | A survivor assigns work to the dead; intervene or allow |
| `quest_faith_rebuilder_refusal` | 4 | Deacon Tov blocks the foundry; mediate or side |
| `quest_faith_counting_cairn` | 5 | Provision and escort the first pilgrimage; survive the road |
| `quest_faith_the_relic` | 3 | Recover a relic; hear the claimed story |
| `quest_faith_provenance` | 4 | Cross-check records; question the broker; choose exposure |
| `quest_faith_first_schism` | 5 | Listeners split; contain or permit; count the cost |
| `quest_faith_blue_basin` | 4 | Supervise the cobalt sacrament; triage the aftermath |
| `quest_faith_divided_chapel` | 3 | A wall is built; rebuild, divide, or abandon |
| `quest_faith_ninth_question` | 5 | Public debate; authored positions; the player's argument |
| `quest_faith_the_fracture` | 5 | Schism resolution; reform, splinter, or dissolve |
| `quest_faith_relic_road` | 4 | Settle provenance on the Relic Road; trade or keep |
| `quest_faith_creed_kept` | 3 | Final disposition; epilogue selection |

Stage requirements must reference valid item IDs and, where a location is needed,
a valid `loc_` node. No stage may set a conversion directly.

---

## 27. APPENDIX J — NPC DOSSIERS (BRIEF)

**Hesk Avor** — the slate-bearer. A former signal clerk who walked in from the
Relic Road. Sincere, patient, and completely uncharismatic in the way fanatics are
supposed to be — which is what makes the Creed spread. Does not want leadership.
Blind spot: believes any suffering can be repurposed.

**Elder Mara Sohn** — Ash Witness elder. Has counted more names than anyone. Fears
the Creed because it promises to make grief useful, and she knows grief is not for
use. Rigid under pressure; can be softened by a witnessed loss.

**Deacon Tov Rik** — Rebuilder deacon. Believes the Creed is theft of the living's
labor. Not cruel, just convinced that a shelter that buries in order to work will
stop working. Becomes the schism's hard edge if unmediated.

**Ila Venn** — Listener convert and the expansion's peacemaker. Holds pieces of
every movement. The only NPC who can broker a joint observance. May leave if the
player forces a purity test.

**Oren Das** — relic broker. Sells forged provenance as often as true. Not evil;
believes a story is worth what someone pays. The provenance quest is his.

**Sister Hale** — cobalt acolyte. Genuinely radiant, genuinely dying. The sacrament
quest is a slow conversation about whether meaning justifies exposure.

**Pell** — the questioner. Refuses all movements and says so plainly. Represents the
player who wants no part of belief; must be written with respect, not as an enemy.

**Senna** — a young novice. The next generation's stake in the Ninth Creed, and the
reason the decision cannot be purely tactical.

---

## 28. APPENDIX K — LOCATION DETAIL

Each exterior node carries `dangerLevel`, `travelHours`, `baseRadsPerHour`, `region`,
and `inspect` prose. Key authored beats:

- **Counting Cairn** — stones are numbered, not named; pilgrims add a blank stone.
- **Unfinished Yard** — a monument no one has agreed how to finish; each visit shows
  a little more or less work depending on movement fervor.
- **Listening Mast** — the mast still catches a signal at dusk; Listener pilgrims
  sit beneath it and say nothing.
- **Blue Basin** — the water does not freeze; the glow is beautiful and it is killing you.
- **Hidden Hour** — a tunnel where the Third Shift believe the dead work; the real
  hazard is unstable ground, not ghosts.
- **Gate That Stood Open** — a checkpoint whose gate was left open during the Exchange;
  the Open Gate movement treats that as a mercy and a warning both.
- **Garden in Ash** — a real plot, real soil, real disappointment most visits.
- **Divided Chapel** — a wall built through a shared room; both sides still use it.
- **Relic Road** — a market where provenance is a commodity and truth is not.
- **Wall of Last Words** — names and final sentences, many unfinished.

---

## 29. APPENDIX L — CONTENT REVIEW CHECKLIST (PRE-INTEGRATION)

Before any belief content is merged, a reviewer must confirm:

- [ ] No real-world religious term, figure, symbol, or practice appears.
- [ ] Every movement has at least one cost and one blind spot.
- [ ] No movement grants an unbounded or unique-resource benefit.
- [ ] No text frames belief as ethnicity, nation, or politics.
- [ ] No violence is rewarded; escalation stops at a typed event.
- [ ] Conversion has no deterministic trigger.
- [ ] Cult rites cannot script a death.
- [ ] Rituals keep cooldowns and capped morale deltas.
- [ ] Offerings reference valid item IDs.
- [ ] Locations reference valid IDs and stay within the danger scale.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism relies only on the host-forked RNG.

---

## 30. APPENDIX M — GLOSSARY

- **Movement** — a belief system with creed, practices, and mechanical profile.
- **Cult** — a movement with a sacrament, sacred threshold, and martyr memory.
- **Schism** — a deterministic split of a movement into parent and splinter.
- **Sacrament** — a dangerous physical rite with a measurable threshold.
- **Relic** — a contended object whose claimed and true provenance differ.
- **Pilgrimage** — an expedition variant with stages and revelation eligibility.
- **Observance** — a holy-day performance flag, informational not compulsory.

---

## 31. CLOSING STATEMENT

ASHFALL already knows that survivors need more than calories. It gave them three
movements, nine rituals, and a mourning arc that softens grief without erasing it.
The Faithful & The Fractured takes that seed and grows the wasteland's inner life:
shrines that draw power from a hungry grid, pilgrimages that cost real supplies,
relics whose truth matters less than the argument about it, and a schism mechanic
that turns any strong belief into a fault line. It adds no piety meter, no real
faith, and no violence fantasy — only people, doing the human thing, when the
arithmetic fails.