# The Spine — *Who Rode Out First*

> Target files: `world_history.json`, `echoes.json`, `events.json`
> Uses: `discovery_location_id` / `knowledge_key` (located knowledge),
> `threateningBodyText` (trust-reactive prose)

## Premise

Canon establishes three facts and never connects them:

- `The Bunker Boom` (Exchange−3Y) — shelters were built, privately and publicly
- `The Quiet Evacuation` (Exchange−1M) — people were moved, quietly, early
- `The Final Broadcasts` (Exchange−1W) — everyone else was told to stay calm

Connect them and you get the only conclusion available: **there was a list, and
somebody was on it.**

The instrument was the **Continuity Allocation Schedule** — a civil-service
document that assigned every prepared shelter in the district a number and
every number a household. Your bunker is **Allocation 12**.

You are not Allocation 12.

## The thing that makes it work

The allocation was not decided by generals or money. It was decided by a
**formula**: age, dependent count, and a "Reconstruction Utility Rating"
derived from occupation. A score above 60 qualified. The scoring was applied
consistently, without corruption, to four million people, by a department that
was proud of how fair it was.

That is the horror, and it is a bureaucratic one rather than a conspiratorial
one. Nobody in Sector 4 was betrayed by a villain. They were **scored**.

And the reason your people are alive is smaller still:

> The door was unlocked because the people it was locked for had not arrived
> yet.

Allocation 12's convoy was held six hours at a checkpoint over a discrepancy in
a dependent's date of birth. By the time it was released, the roads had closed.
Whoever was standing near an open, stocked, empty shelter that afternoon went
in. That is the founding event of the player's community, and it is pure
proximity.

---

## The discovery ladder

Five layers, each gated on day count. The player is never *told* any of this —
each layer is a physical object in a real place.

### Layer 1 — Wrongnesses (Day 1–30, in the bunker itself)

The player's own shelter is the first lore location. Nothing dramatic; only
fittings that do not match the people using them.

- A dental chair, bolted, with no dental instruments and no dentist
- Bunks sized for eleven when the manifest sheet in the airlock reads *14*
- Four unfaded rectangles on the corridor wall where nameplates were unscrewed
- A crate of children's winter boots, sizes 1 through 4, never opened
- A wall-mounted chart headed **ALLOCATION 12 — DUTY ROSTER**, all rows blank

`knowledge_key: lore_allocation_wrongness` · `discovery_location_id:` player
shelter · `discovery_trigger: inspection`

### Layer 2 — The word (Day 30–90)

The word *ALLOCATION* starts recurring on salvage. A stencil on a crate. A
shipping label. A ration tin stamped `ALLOC-12 / NOT FOR GENERAL ISSUE`.

The player has no context yet. That is the point — the word arrives before the
meaning, the way it would.

### Layer 3 — The Ministry (Day 90–150)

`location_ministry_of_truth_bunker` stops being a generic ruin and becomes the
department that ran the Schedule. Partial manifests, a scoring rubric, and a
memo instructing staff to stop answering public enquiries about shelter
eligibility "pending clarification." The clarification never came.

### Layer 4 — The Archivists (Day 150–200)

Nine people in `location_the_memory_vault` who can read the fragments, because
they wrote them. See below.

### Layer 5 — The arrival (Day 200+)

They come. See *Allocation 12* below.

---

## New `world_history` entries

Schema: `era`, `year_month`, `title`, `body`, `discovery_location_id`,
`discovery_trigger`, `knowledge_key`. Bodies below are summarised to their
first line; full prose to be written at conversion.

### `pre_exchange`

| id / knowledge_key | year_month | Title | Found at |
|---|---|---|---|
| `lore_pre_continuity_office` | Exchange−4Y | **The Office of Continuity** | `location_ministry_of_truth_bunker` |
| `lore_pre_the_formula` | Exchange−3Y | **The Reconstruction Utility Rating** | `location_ministry_of_truth_bunker` |
| `lore_pre_allocation_letters` | Exchange−2M | **The Letters That Went Out** | `loc_evacuation_bus_depot` |
| `lore_pre_the_discrepancy` | Exchange−1M | **A Discrepancy in a Date of Birth** | `loc_highway_checkpoint` |

> **The Reconstruction Utility Rating** — "Occupation was worth up to forty
> points. A water engineer scored 31. A paediatric nurse scored 28. A records
> clerk scored 9. The department published the rubric openly, in the belief
> that transparency was the same thing as fairness."

> **A Discrepancy in a Date of Birth** — "Convoy 12 was held at the Sector 4
> east checkpoint for six hours and eleven minutes while a duty officer
> telephoned a department that had already been evacuated. The child's birth
> year was recorded twice, once correctly. The officer was following
> procedure."

### `hour_zero`

| id / knowledge_key | year_month | Title | Found at |
|---|---|---|---|
| `lore_hz_open_door` | Exchange+0 | **The Open Door** | player shelter |
| `lore_hz_convoy_12_turns_back` | Exchange+0 | **Convoy 12 Turns Back** | `location_abandoned_convoy_yard` |
| `lore_hz_the_registrar_stays` | Exchange+1D | **The Registrar Stays** | `location_ministry_of_truth_bunker` |
| `lore_hz_nameplates` | Exchange+3D | **The Nameplates** | player shelter |

> **The Open Door** — "Allocation 12 was provisioned for fourteen and sealed
> for none. The outer hatch was on standby cycle, awaiting arrival
> authentication. Standby cycle holds the hatch unlocked."

> **The Nameplates** — "Somebody in the first week walked the corridor with a
> screwdriver and took down fourteen brass nameplates belonging to people who
> had not come. They were not thrown away. They are in a tin, behind the
> filtration stack, and everyone who has found them has put them back."

### `black_sky`

| id / knowledge_key | year_month | Title | Found at |
|---|---|---|---|
| `lore_bs_alloc_12b` | Exchange+2M | **Allocation 12-B** | `location_flooded_subway_depot` |
| `lore_bs_the_vault_holds` | Exchange+4M | **The Vault Holds** | `location_the_memory_vault` |
| `lore_bs_garrison_requests_schedule` | Exchange+5M | **The Garrison Requests the Schedule** | `location_ministry_of_truth_bunker` |
| `lore_bs_what_score_were_you` | Exchange+6M | **"What Score Were You?"** | `loc_toll_house` |

> **"What Score Were You?"** — "It became a way of asking a stranger what they
> had been, without asking what they had been. In the Toll they still ask it.
> Nobody who answers with a number above sixty is telling the truth, because
> everyone above sixty went inside."

### `ashfall`

| id / knowledge_key | year_month | Title | Found at |
|---|---|---|---|
| `lore_af_renn_dies` | Exchange+2Y | **The Engineer Dies at 12-B** | `location_sub_level_4_transit` |
| `lore_af_the_walk_begins` | Exchange+4Y | **The Walk** | `loc_metro_tunnel` |
| `lore_af_paperwork_survives` | Exchange+5Y | **The Paperwork Survives** | `location_the_memory_vault` |
| `lore_af_the_claim` | Exchange+5Y | **The Claim** | player shelter |

---

## The Archivists

**Already implemented.** `NPC_Archivists.cs` defines
`faction_archivists` — *"The Archivists of the Before: monastic order of
Bunker-Born hoarding pre-war media as ancestral spirits,"* taking tithes of
photo albums and cassettes and paying in morale and `item_encrypted_drive`.
This bible does not replace that. It supplies its **origin**, which the code
does not have.

The order is two generations in one room.

The first generation is one woman. When the Ministry evacuated, the senior
grades left; the filing staff stayed, because nobody had issued instructions to
stop, and because the alternative was to go outside. Margit Sole kept working.

The second generation are the Bunker-Born — children who grew up watching an
adult handle brittle paper with cotton gloves, speak the names of the dead
aloud while transcribing them, and refuse to discard anything with a person's
handwriting on it. They were not taught a religion. They **inferred** one, from
observed practice, correctly in every particular except the reason.

Sole has never corrected them. Asked why, she says the ritual keeps the records
in better condition than her instructions ever did.

They live in `location_the_memory_vault` in the Drown, reachable only by boat,
which is why they still exist. They maintain the Continuity Allocation Schedule
in full. It is useless. They maintain it anyway.

**What they want:** for the record to be complete and correct. Not justice. Not
restitution. Completeness. They will trade lore fragments for verified
information — a name, a death, a birth — and they will refuse a trade if the
information cannot be corroborated.

**Registrar Margit Sole** — Records Clerk Grade II, Office of Continuity.
Ran the Schedule for Sector 4. Late-game, if trust is high enough, the player
can find her own entry:

> `SOLE, MARGIT J. — Records Clerk II — RUR 9 — dependents 0 — **score 41.2** —
> **NOT ALLOCATED**`

She filed the list that excluded her, correctly, and then stayed at her desk
while it was executed. She has never once described this as unfair. When asked
why she still keeps the Schedule, she says the same sentence every time:

> "Because if nobody holds the record, then it only happened to us once, and
> then it stops having happened at all."

---

## Allocation 12 — the arrival

**Trigger:** Day 200+, gated on the player having reached Layer 4.

Six people at the outer hatch. They do not attempt entry. They have been
walking for eleven days, they know exactly which hatch this is, and one of
them is carrying a laminated card in a freezer bag.

**Sela Renn**, thirteen. Allocated at age eight as a dependent of *RENN,
HALVARD — Water Engineer — RUR 31 — score 71.8*. Her father is four years dead
at Allocation 12-B, a subway maintenance level that was never provisioned for
occupancy. She does not remember the world that promised her this room. She
remembers the tunnel.

The five adults with her are **not on the list**. They are the people who kept
her alive to get her here — a fact they all understand and none of them
mention. If the claim is honoured as written, only Sela goes in.

**She knows this.** That is the encounter's real weight. The thirteen-year-old
at the door has already worked out that her paperwork is worth exactly one
person, and she has walked eleven days anyway, because the adults told her to,
and because she is thirteen.

### What the game must not do

- Not make them raiders in disguise
- Not make the paperwork forged
- Not have them attack if refused
- Not tell the player which choice was right, ever, including in the ending

They are correct under a law that no longer exists. Your people are correct
under five years of living here. Both are true. The game presents the hatch,
the card in the freezer bag, and the temperature outside, and then it stops
talking.

### Branches

| Branch | Flag | Shape |
|---|---|---|
| **Honour it in full** | `alloc12_honoured` | All six admitted. Immediate strain: rations, bunks, water. Long-term: Sela is a water engineer's daughter and remembers more than she lets on. |
| **Honour the letter** | `alloc12_letter_only` | Sela in, adults out. She refuses, or she does not, and either is worse. Heavy, permanent morale cost on any survivor with the `parent` history. |
| **Refuse** | `alloc12_refused` | They leave. No combat. Approximately forty days later a scavenging party finds the freezer bag, and the card is still in it. |
| **Negotiate** | `alloc12_terms` | Admission on labour terms — which is, precisely and unmistakably, the Iron Garrison's doctrine. Voss's approval rises. Somebody in your shelter says so out loud. |

The refusal branch is deliberately the quietest. Nothing happens. Nobody
retaliates. The bunker is simply a little easier to feed, and there is a card
in a bag out there in the ash with a child's date of birth on it, recorded
twice, once correctly.
