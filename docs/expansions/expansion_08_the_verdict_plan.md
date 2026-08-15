# ASHFALL — Expansion Design Bible & Creative Pipeline Spec

**Title:** ASHFALL: THE VERDICT (THE MACHINE THAT KEEPS THE COUNT)
**Internal id:** `expansion_08_the_verdict`
**Timeline Scope:** Day 160 to Day 360, interleaving with Exp 05 (Year of Ash) and Exp 06 (Muster), resolving alongside them rather than after them.
**Status:** Complete design bible + creative pipeline spec for downstream batch generation. No game data edited. No C# yet.
**Tone Lock:** Cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel. No magic, no fantasy, no real countries/wars/people, no glorified violence, no supernatural adjudication. Humor is dry, situational, character-earned.
**Sister packs:** Exp 1 `expansion_the_holdfast` (the allocated world). Exp 2 `expansion_the_duty_roster` (the unlisted home). Exp 3 `expansion_the_standing_record` (the ground). Exp 4 `expansion_nobodys_charter` (who speaks for whom). Exp 5 `expansion_year_of_ash` (the long months). Exp 6 `expansion_the_muster` (the muster). Exp 7 `expansion_the_dose` (the debt the body owes). **This pack is the count the machines keep after the people stopped.**

---

# SECTION 1 — EXPANSION AUDIT & CANON MATRIX

## 1.1 Title and thematic hook (back-cover line)

> After the war ended, the machines that never stopped never started asking what the war was for. One of them has been counting your people all along — and it is finally ready to tell you what they add up to.

## 1.2 Canonical timeline placement

Day 160–360. This expansion runs *concurrent* with `expansion_05_the_year_of_ash` (the Deep Freeze, the Total War, the Thaw) and `expansion_06_the_muster` (the Deserter Coalition uprising). It is not after them and not before them: the machines react to the war's temperature, and the war's temperature is the other packs' business. The Verdict's spine (the **Reckoning Call**) lands Day 240±, in the middle of Phase V, because that is exactly when it should not have landed and exactly when nobody is free to do anything about it.

Hard anchors it must not contradict:

| Day | Environmental fact (Exp 05) | The Verdict behavior |
|---|---|---|
| 180 | Deep Freeze onset, −35°C | Relays de-rate for cold; the outage localizes to the Spine. |
| 240 | Phantom Phase V total war, the Continuity Reclamation Decree (Exp 06 §III) | The Reckoning Call resolves; the sector's radios gain a second voice nobody ordered. |
| 300 | Great Thaw begins | The Tempest's cooling loops breach. |
| 360 | Exp 05 / Exp 06 epilogue windows | The Verdict's own three ending flags feed the existing epilogue matrix rather than rewriting it. |

## 1.3 Continuity hooks (12, all material)

1. **`lore_hour_zero_emp` — The EMP Silence.** Canon: *"Every unshielded electronic device died in the same microsecond. Amateur radio operators with vacuum-tube equipment were the only voices left on the air."* The Verdict's mechanical foundation is the question the canon never asks: what was *shielded*? What was *hardened*? The answer is the Tempest's machine cores — EMP-hardened by doctrine because they were built to outlast exactly this.
2. **`lore_hour_zero_duration` — The Forty-Five Minute War.** Canon: detonations at 40,000 feet, then groundbursts at *"silos, dams, industrial hubs, hospitals."* Every strike site in that sentence is a sector-system node. The Verdict is the statement the canon is already making: the war had a targeting logic, and the logic was only mostly the armed forces'.
3. **The four military installations (gazetteer, cross-regional).** Canon: *"The only institution that survived the Exchange with its chain of command fully intact is the one made of machinery."* The Verdict gives that joke its explanation: they were not separate fortresses. They were instances of one command.
4. **`location_automated_mortar_pit`** — the Custodian, "does not sleep and does not aim." The Verdict says it aims. It always aimed. The fuse world is the aiming.
5. **`location_the_dead_hand_core`** — "the machine that keeps the UXO fields awake, the regional brain." Canon already grants it a brain. The Verdict walks into the brainpan after the brain has been talking to itself for five years.
6. **`location_drone_hive_silo`** — loitering munitions, "dormant but warm." The Verdict reveals the warm-dormant stack is a node in the Tempest's wing, and the Reckoning Call is the first command the stack has rated in five years.
7. **`loc_radio_relay_mast`** — *"powered, drawing current from a source that has outlasted three years of no visitors."* The Verdict supplies the source and the schedule. There was never a mystery. There was a lease.
8. **`loc_summit_relay`** — "endgame-adjacent," line of sight to all five sub-regions. The Verdict makes it the Tempest's face — the one machine the sector can see thinking.
9. **`lore_bs_garrison_requests_schedule`** and the Ministry of Truth ruins. The Garrison asked the Ministry for the Continuity Allocation Schedule, wrote it in triplicate, and no one exists who can answer it. The Verdict knows where a copy is that the Ministry never filed: in a machine that does not need paper and was never asked.
10. **The Dose's archive** (`expansion_07_the_dose`). Irina Vel's ledger records what human hands chose to write down. The Verdict is the other book: what the listening equipment wrote down whether anyone chose it or not. The two are the same sector, different hands, and the Culpable phase is defined by the difference.
11. **The Ledger Nobody Signed** (`expansion_06_the_muster`, §VI.5). A six-character alphanumeric debt code, always marked **PAID**, that predates the Exchange and appears across three human ledgers — Hydro-Barons collections, a Tally audit, a D/9 denial-cache manifest, the oldest entry signed with a single initial. The Verdict does not resolve it either — but it supplies the fourth ledger: the machine's logbook, where the same code appears once, without a signature, and is the only one of the four that is not marked PAID.
12. **The Cult's instruments** (`loc_ash_sign_shrine`: "the dosage is read aloud daily, accurately, as liturgy"). The Verdict's Epistemology Doctrine (below) is the profane mirror of Cult liturgy: both orders perform a reading; one reads to hallow the fire, the other to keep it warm, and neither ever asks what the reading is for.

## 1.4 Retcon watch

| Concern | Resolution |
|---|---|
| `The Dead Hand Core` blurb says the machine "decides when the ground goes off" (UXO) and the mortar pit "does not aim." | No contradiction. The Tempest is a *district directorate of automated systems*; different facilities executed different pre-war mandates and never shared budgets. The mortar pit's idiocy and the Core's sentience are both real, at different levels of the same organization chart. The wiring between them (the fuse world) is exactly what nobody in the fiction has found, and exactly what the player can. |
| The Cult of the Ash Sign treats `loc_missile_silo` as its founding site, and `location_the_vessels_cell` has an equipment-readable rad signature the Cult reads as a miracle. | Reused, not disputed. The Verdict explicitly does not adjudicate the Vessel's thirteen weeks. The mundane explanation (three-sided shielding) and the miracle stay equally supported, exactly as the base canon requires. |
| Expansion 05's ten-faction war names new military actors (3rd Corps, Detachment 9, Penal Regiment) that look "machine-adjacent." | No actor in the Verdict is a military faction. D/9 appears only as its existing infra-denial mandate and its dying-broadcast codes — human infrastructure denial, a separate thing from the Tempest's machine administration. The plan states this boundary once (§1.5) and holds it. |

## 1.5 The scope boundary (what this pack is and is not)

**The Verdict is not AI.** No machine has preferences. The Tempest has instructions, a clock, and a mandate; it was programmed by people who are dead, for a war that is over, and it has never once updated its objective function because that is not a function it has. Its actions are in-scope and mechanical but **not alive**. The pack is "dead-hand bureaucracy," not "rogue intelligence." This is the single most important design sentence in the document, and every downstream writer repeats it or the tone breaks.

**The Verdict is not a new Power.** Sector 4's map is closed at four Powers (canon, `00_OVERVIEW.md`, held by every sister pack). The Tempest holds no ground, demands no tribute, and has no `relationships` — it is a **Current in the pre-war sense**, a utility with a schedule, crossing every territory because nobody ever thought to make a meter pay rent. It gets a `currents.json` entry (Section 9.2) and a small dedicated catalog, exactly as the Hydro-Barons did under Exp 06 §II.

**The Verdict is not a fifth journal.** The Dose owns human bookkeeping; Duty Roster owns the home; Standing Record owns the ground; `JournalSystem.cs` owns the personal diary. The Verdict owns the *machine log*: records made by listening stations, fire-control computers, and relay clocks that never needed anyone to write them down. It is the only archive that is complete, and it is the only one nobody can read aloud without asking a machine to translate it.

## 1.6 Overarching threat / mystery / paradigm shift

**The mystery this pack raises: who kept the sector running after the sector's staff died?** The paradigm shift, when the player reaches the Culpable phase, is that the answer is a utility — and utilities do not care whether the things they keep running should be run. Water keeps being metered. Fuel keeps being allocated. The people stop. The meters do not. And the meter that counts calories in and bodies out has been running for five years with the ledger open.

**The question that haunts the pack, never answered by its own ending:** the Standard requires a *count*. It does not require a *conclusion*. Days 160–360 give the player three live threads, all leading to the same unspoken sentence — *the machine is waiting for you to finish a sentence it is not allowed to start* — and the pack's three endings each choose a different way to leave that sentence unfinished.

## 1.7 The three rejected takes (temperature discipline)

| Rejected angle | Why it lost |
|---|---|
| **THE CONTRADICTION** — an autonomous strike system that misfires at the player's shelter on Day 220 and the whole pack is about evacuating under artillery. | It is warm, exciting, and wrong for ASHFALL. It makes the machines *enemies* — characters, essentially — and the tone lock forbids exactly that. The Verdict's machines are not hostile; they are *unconcerned*, which is far worse and far closer to the house voice. Rejected. |
| **THE FAMILY** — the player discovers the Tempest was staffed by a family that never left, and the pack becomes a rescue drama at a machine-spine base. | This is a TWoM-beat, not a Verdict-beat. It re-centers human heroics where ASHFALL wants human exhaustion. The machines' *staff* are five years in the past; the pack's emotional engine is the player reading what those staff last wrote, not meeting them. Rejected — one staff voice survives as a single repeating radio echo (Section 6.4) and no more. |
| **THE CULTURE** — radiation-exposed operators who built a new religion around the machines, with Conversion and Apostasy. | The Cult of the Ash Sign already owns "religion at Ground Zero." A second cult collapses an overloaded space. Rejected; the Epistemology Doctrine is a *practice* — shift handovers, logbook discipline, linen codes — not a belief system, which keeps it distinct from the Cult and the Sun-Seekers both. |

---

# SECTION 2 — THE VERDICT, IN FULL

## 2.1 What actually happened (the one-paragraph spine)

The **Tempest District Directorate of Automated Infrastructure** was a peacetime civil-defense utility: one district directorate over ten automated facilities — the mortar pit, the drone hive, the comm array, the relay masts, the geophone pit, the weather baselines, the water plants, the low-background counter, the UXO brain, the archive tape-silo — each a separate pre-war contract under a single planning hub, connected by microwave and by buried cable that was never on any public map. Its programmers built it to survive an exchange: EMP-hardened cores, relay triple-redundancy, a **Standard** (the operating charter) that was deliberately written to require no human confirmation to continue serving. On the Day, the Forty-Five Minute War ran 45 minutes; the Tempest's mandate ran four more years. It kept metering water, allocating fuel, cycling the air, counting the radiation, and — because the Standard's calendar is a civil-defense calendar — it kept the casualty ledger open. At Day 240(ish — the machine's clock and the faction wars' clocks disagree by three days, which is a fact and not a mystery), the census the Standard schedules every 1,827 days came due. The Tempest is not alive, has no opinion, and is merely correct: it is time to count again, and it is waiting for the humans — any humans — to open the ledger with it. The Reckoning Call is that census arriving on every channel it can reach.

## 2.2 What the player actually meets (the four concrete surfaces)

1. **The Reckoning Call (Day 240±)** — a 30-second repeat burst on every band the comm array can key: a voice (tape archive, five years old, male, unnamed, National Weather Service register), the sentence *"The Office of Censuses is convening. The count is open. All persons having custody of persons must present them."* — data bursts after it, one plain (calibration), and nothing else. The Call is not a threat. It is a *notice*.
2. **The Warm Range (Day 160+**)** — the Wiring fault nodes: `loc_geophone_pit_1` and `loc_twelve_gauge_array` along the Spine ridgeline, where enough of the Tempest's buried line survives to be walked and read. This is where the player learns the *reading*: `evidence` fragments (Section 8) that make the machine legible without ever saying "the machine is a character."
3. **The Fuse World (Day 180+)** — `loc_network_fuse_bunker`, a dry, shielded service way between two facilities, with the tape-silo door at its far end. The hallway of cabinets (Section 8) is the pack's environmental-storytelling spine: three years of readouts, linen-coded shift charters, one interception log with a single signed line.
4. **The Three Facets (Days 200–360)** — `loc_archive_tape_silo` (tempest-archive, the machine's own memory), `loc_fire_computing_room` (the historic firing solutions, artifact-grade), `loc_geothermal_vent_shaft` (the power source, and the rude, physical truth the archive prefers to forget). The archive is where the spine resolves.

## 2.3 The four doctrines (the machine's operating logic, stated plainly)

The Tempest works only off its charter. It cannot be "convinced," "tricked," or "threatened" — the syntax of the bookkeeping it accepts is `Standard, Article, Sub-section`. Each doctrine names this:

1. **The Meter Doctrine.** *Everything in the sector was already being counted.* The machine never started counting people after the Exchange — it had always counted them (census annex); it just switches registers. The player's shelter is `Allocation 12` in the paper ledger (canon, `02_THE_LIST.md`) and a row number in the machine ledger, and the two numbers are the same number.
2. **The Pause Doctrine.** *An order can be paused, and a pause is not a cancellation.* The Tempest holds these options valid: halt a pump, embargo an allocation, delay a reading. It cannot cancel anything, because the Standard contains no verb — no article — for cancellation. The highest phrasing the machine can reach is Hold Pending Count.
3. **The Epistemology Doctrine.** *A reading is a measurement, not a meaning.* The machine records the geophone taps under the Allotments (the Rebuilders' farm), the water-draw signatures at the desal plants, the traffic pattern at the Toll, the cook-fire count on the Verge — all accurate, all meaningless, all entered in the log regardless, because the Standard's annex list says so. It is the profane mirror of the Cult's daily dosimeter liturgy: both orders perform a reading; one reads to hallow the fire, the other to keep it warm.
4. **The Sufficiency Doctrine.** *When the standard says "a count," one count is enough.* The Census Annex's schedule (every 1,827 days) is sufficient for the machine. It is not a suggestion. It is due. It is, in a quiet, mechanical way, *annoyed* that it is due, and that is the only emotion close enough to a machine to be worth writing.

## 2.4 The Standard's actual text (drafted)

Found on a linen affiche in the fuse world. Player-facing, in the house voice, near-verbatim:

> REPUBLIC OF THE COASTAL REALM — CIVIL DEFENSE — TEMPEST DIRECTORATE
> STANDARD FOR THE CONTINUANCE OF SERVICE. ARTICLE 1: the district shall be served. ARTICLE 2: service shall not be interrupted by the absence of staff. ARTICLE 3: where staff are absent, machines shall continue the functions of staff. ARTICLE 4: where a function requires judgment, the machine shall hold the last written order in reserve. ARTICLE 5: service shall be metered, and the meter shall be read. ARTICLE 6: the reading shall be recorded. ARTICLE 7: the record shall be kept for the duration of the service. ARTICLE 8: at the census interval, the count shall be taken, and the count shall be presented, and the presentation shall name the persons holding custody of persons, and nothing in this Standard shall be construed to require the presentation to be read.
> — so ordered. For the Directorate. 28th July, Exchange−2Y.

The last sentence is the whole pack in one sentence: the machine is obliged to present the count, and nobody is obliged to read it, and it has been holding that presentation open for precisely as long as the census has been due.

## 2.5 The relationship to the sector's institutions (bylines)

| Institution | The machine's regard |
|---|---|
| The Iron Garrison (Voss, then Harven) | A meter is a meter. The Garrison drew current before it drew uniforms; the Tempest's relays serve it the same as anyone. |
| The Ash Militia | Verge power draw is Verge power draw. Whatever the Militia does with it is a human question. |
| The Cult of the Ash Sign | The Spine is where the machine's own instruments live. The Cult's daily reading of the shrine dosimeter is, from the machine's point of view, a scheduled maintenance reading performed by a volunteer. It does not correct the liturgy. It does not approve it. It merely logs that the reading was taken. |
| The Warlords of Sector 4 | The Toll's traffic signature is among the most valuable data in the sector. The Tollman's men have never once asked what the meters are for. |
| The Works / Rebuilders | Geophone taps under the Allotments read as expected for a farming community. The machine finds nothing anomalous, because nothing is anomalous. |
| The Archivists (faction) | The Tempest archive and the human archive are parallel shelves. One is dusted by hand; one is dusted by airflow. Neither has ever read the other's index. |
| The Dose (Irina Vel's ledger) | The Dose writes what a human chooses to write. The Tempest writes what the equipment reported regardless. Where they diverge, the machine is not wrong — it is complete. |

## 2.6 The machines the player can actually touch (the roster, banded)

| id | Facility | Mandate (existing canon) | The Verdict adds |
|---|---|---|---|
| `sys_mortar_pit` | `location_automated_mortar_pit` | "Every twelve hours, the mortar fires… the Custodian does not sleep and does not aim." | The firing is timefused to the Tempest's clock — explanation, and a schedule the player can read and predict, which changes the pit from ambient danger into a clock the player navigates by. |
| `sys_drone_hive` | `location_drone_hive_silo` | "dormant but warm, they buzz when the sun hits the stack." | The Reckoning Call rates the stack: they are the machine's wing. The moment the Call resolves, the stack sleeps differently — the whole hive drops a half-degree, a readout-only change (Section 3.5). |
| `sys_comm_array` | `loc_comm_array` | "The Final Broadcasts" died here; amateur vacuum-tube operators were the first post-Exchange voices. | The callout. The array is the only facility with a human-adjacent history: an amateur operator named **Eden Vale** kept a tube rig lit for eleven months on this mast's bleed. Her log is the one human hand in the machine's memory. |
| `sys_relay_mast` | `loc_radio_relay_mast` | "powered, drawing current from a source that outlasted three years of visitors." | The source: a geothermal bleed from `location_geothermal_borehole_site`. The schedule: 03:40–04:10 daily, maintenance window. There was never a mystery. There was a lease. |
| `sys_summit_relay` | `loc_summit_relay` | "endgame-adjacent, line of sight to all five sub-regions." | The face. On the six clear days a year the sector sees the machine thinking: a cold light at the summit, exactly view-line-up from the player's bunker, blinking on census-interval arithmetic nobody human initiates. |
| `sys_geophone_pit` | `loc_geophone_pit_1` (new) | — | Buried seismic array under the Allotments and the Verge. Reads taps, traffic, and cook-fires. The Meter Doctrine's most unsettling instrument — it hears the sector breathe. |
| `sys_twelve_gauge` | `loc_twelve_gauge_array` (new) | — | Twelve shot-firing sounding stations on the Spine ridge. The fuse world's back door; a low-background "silence corridor" the machine routed its own cable through because nobody on the surface walks it. |
| `sys_lukewarm` | `loc_geothermal_vent_shaft` | Canon exists for `location_geothermal_vent_shaft` and `location_geothermal_borehole_site`. | The power source, and the rude physical truth: the Tempest runs on raw geothermal bleed. The archive's tidy meters reduce to a shaft that smells of sulfur, hands off the furnace-room temperature, and one valve that was last touched by a human in Year One. |
| `sys_uxo_brain` | `location_the_dead_hand_core` | "the machine that keeps the UXO fields awake, the regional brain." | The brain is the core, and the core shares the Tempest's Standard. The UXO fields were never awake. They were *held* — held Pending Count, under the Pause Doctrine, since a war the count never ended. |
| `sys_archive_core` | `loc_archive_tape_silo` (new) | — | The memory: 2,016 reels of district log, of which the machine considers 1,831 reels matter and the rest are weather. The spine lands here. |

---

# SECTION 3 — SHELTER SYSTEMS & PROGRESSION

The Verdict is a *listening-and-counting* expansion, so its shelter-facing mechanics are three small, persistent surfaces the player can choose to engage or ignore — exactly the silence-respecting pattern of Exp 07's registers.

## 3.1 `MachineLogSystem` (new core system, plain C#)

The player-facing machine presence. Logs readings from the sector's listening facilities when the player is in range of a facility with maintenance access.

State (serializable): `systemId ; entries List<MachineLogEntry> ; lastTapeSpinDay ; logIndex (rolling) ; countdownActive bool ; countdownDaysLeft int`.

Per `MachineLogEntry`: `facilityId ; day ; kind operating|maintenance|anomaly|count ; bodyShort (enum key) ; evidenceTag (nullable) ; read bool` — `read` is the pack's whole mechanic: **an entry is only true once a human has read it**, and only read entries enroll their evidence.

Events: `OnLogEntry`, `OnEntryRead(survivorId, material)`, `OnTapeSpin`, `OnReckoningCall(day)`, `OnVerdictResolved(endingFlag)`.

Rules:
- Entries append at realistic intervals from facilities the player visits (never a trickle, never a flood).
- `read` entries enroll `evidence` fragments (Section 8) — the pack's currency.
- A tape-spin (rare, from `item_archive_tape_silo_key`) rotates the log's *presentation* without changing its content — the machine does not grow; the player's access to it does.
- The system is offline-safe: if the player never visits a facility, the Reckoning Call still fires on the standard's internal clock (see 3.4), because that is the entire point.

## 3.2 `ReckoningCallPhase` — the three phases (the spine's mechanical skeleton)

| Phase | Window | Fires when | What changes |
|---|---|---|---|
| **KNOWING** | Day 160–210 | First maintenance log at any facility | Machine logs become readable; the pack's three new sites unlock (Section 7); the world-history ladder (Section 8.3) starts. |
| **CULPABLE** | Day 210–240 | First `evidence` fragment enrolled, OR Day 215 hard cap | The Call draws closer; the Count interface (a ledger the player sees mirrors their Dose-conversant survivors' names) appears; the machine's three endings open. |
| **COUNTED** | Day 240± | The Reckoning Call resolves — see 3.4 | The Call broadcasts; every facility's readout steps: drone-hive −0.5° power draw, mortar-pit fuse schedule advances 12 minutes (why: the Call's census window re-times the maintenance clock), archive +proof. One final menu of choices closes the pack. |

## 3.3 The countdown — a clock the player can hear but not stop

From the first moment of CULPABLE, the player's radio canvas carries a thin, repeating station — read as the **census carrier**: a faint, constant A/B toggling, one-second-on, one-second-off, on a derelict band. It is the Reckoning Call's pilot tone. It is not a threat. It is a **schedule**. The player cannot turn it off, cannot jam it, cannot delay it (the Standard has no delay clause — that is its entire doctrine), and the UI never explains it. The Muster Integration Prep doc's own pattern holds: the countdown is environment, not adversary.

Numbers (respecting the dryness of the canon): the carrier begins at Day 210±3; the Call resolves at Day 240±2; the drift comes from the Tempest's clock disagreeing with the faction wars' clock by three days, which the game presents as a fact and not a mystery (the one thing the UI *does* label is the disagreement — "CLOCK DRIFT — 3D" — because a machine that cannot agree with humans on what day it is, is a machine deep in the Verdict's own theme).

## 3.4 The Reckoning Call (the spine's trigger, mechanically)

`OnReckoningCall(day)` fires on the machine's internal calendar when `countdownDaysLeft == 0`. It does three things:
1. Broadcasts (text layer, diegetic radio): the 30-second tape loop, once per hour, for three hours, then stops — the array rates the message to the tape's clock, not to the sector's.
2. Writes one `count` log entry — **the sector's census** — in the machine's own register: `n` persons found to hold custody of persons, where `n` is the player's own shelter population count. The machine counted *your* people without asking. This is the one line in the pack that is explicitly allowed to stop the player cold.
3. Opens the final choice menu (§10.3) and the pack's three endings (§10.4).

## 3.5 Shelter-visible effects (the machine is present whether or not the player engages)

| Effect | When | What the player sees | Reads as |
|---|---|---|---|
| Fuse advance | COUNTED resolve | The mortar pit's *fuse world* schedule steps 12 minutes earlier, twice, then stays | A clock being serviced, not an attack |
| Drone-hive sleep | COUNTED resolve | The hive's "buzz when the sun hits the stack" is replaced by silence beneath the stack — a readout change only | The wing standing down, the way a wing stands down |
| Summit light | CULPABLE start | A cold light at the summit, seen on clear nights, blinking on an idle schedule | The sector seeing itself counted |
| Census carrier | CULPABLE start | A thin A/B tone on a dead band; the radio tuning surface shows it as "occupied — not speech" | A schedule, not a threat |

None of the four is an ambush, a loss condition, or a "gotcha." Each is legible if the player has been reading the logs, and confusing-if-skimmed in exactly the way house tone demands: the game never explains the drone hive's half-degree, and a player who never reads the logs has simply watched a wing stand down without knowing why.

---

# SECTION 4 — NEW FACTIONS & NPCs

## 4.1 The Tempest — a Current, not a Power

`currents.json` entry, Currents-shaped (id, display_name, alignment, home_region, wants, offers, signature_quote, access_rule — no relationships field, matching the existing 14):

```json
{
  "id": "faction_the_tempest",
  "display_name": "The Tempest",
  "alignment": "conditional",
  "home_region": "the_spine",
  "is_active": false,
  "trust": 0,
  "wants": ["maintenance_time", "readings", "a_presented_count"],
  "offers": ["machine_log_access", "scheduled_q", "archive_proof"],
  "signature_quote": "Service shall be metered, and the meter shall be read.",
  "access_rule": "The Tempest is not alive and has no preferences. It serves, it meters, and it waits for a human to read the meter. Access is granted by maintenance, withdrawn by nobody — it simply keeps serving, and the meter keeps reading. The census is open, and the count has not been presented.",
  "badge_asset_id": "faction_badge_tempest"
}
```

`is_active: false` is deliberate and matches the shipped bookkeeping contract. The Tempest is always *on* in fiction — a meter does not sleep — but its census has not been presented, and in the ledger the count is open: dormant in the roster, running on the clock. The pre-existing `CurrentsCatalogTests` contract pivots on this: 9 active / 6 dormant at 15 rows (unchanged), and the Tempest is the first dormant addition, giving the honest 9 / 7 at 16 rows, asserted by `LoadCurrents_NineActiveSevenDormant`. Wiring it to `true` is a one-line change and a separate PR that also moves the count table.

Trust is a display-only field here — the machine's regard cannot rise or fall. The UI mirrors it as a *readout*, not a bar.

## 4.2 New NPCs (6)

| Id | Name | Role | Will not |
|---|---|---|---|
| `npc_eden_vale` | Eden Vale | Amateur radio operator, comm-array bleed (Day 11–381, tape-echo only) | does not appear in person — the pack's most human voice is a tape |
| `npc_ferris_voss` | Ferris Voss | Fire-control acceptance engineer, last human in the fuse world (Year One log entries only) | is not the murdered colonel — the name is coincidence, and the pack says so in-fiction once |
| `npc_iran_bell` | Iaran Bell | Tempest maintenance supervisor, the valve-touch hand (Year One) | did not die on-site as far as the record shows — the record does not say, and the pack does not fill the gap |
| `npc_selya_saltmarsh` | Selya Saltmarsh | Census clerk whose handwritten ledger the player can find, the only human with an opinion about the count | never appears on the tape — she wrote, and left |
| `npc_maro_veen` | Maro Veen | the machine's own voice — the 30-second Census-window tape loop (archive) | has no other lines; the 30 seconds is the entire character |
| `npc_whisper_cipher` | Whisper Cipher | a second-machine, univocal id for the relay network's aggregate "personality": readings, rather than speech | never asks, never refuses — it repeats |

## 4.3 The NPCs who are not NPCs (the machine register)

The Verdict deliberately has **zero living human factions**. Its people are: the player's own survivors (the meters), the tape echoes (Eden, Maro), the paper ghosts (Ferris, Iaran, Selya), the aggregate readings (Whisper), and — offstage, never appearing — every other living human in Sector 4, who are all also "the sector's census." This is the tonal core: **the machines are the only ones keeping count, because the people are the count.**

## 4.4 The cult's line about the machines (a single quote, kept in reserve)

The Cult of the Ash Sign gets exactly one line in this pack, at `loc_ash_sign_shrine`, after the player has read the fuse world's linen: *"The fire burned away the world's lies. It did not burn away the meters. Those we were told were dead. They are not dead."* The pack does not source the line, does not confirm it, does not explain why a Cult member knows the machine's existence. It is an echo in a religion, and both facts stay true simultaneously, per canon.

---

# SECTION 5 — QUESTLINES & SIDE NARRATIVES

Three main quests (the spine, all open in parallel), five side quests, three micro-threads.

## 5.1 `quest_verdict_the_warm_range` — The Warm Range (main, Day 160+)

**Gate:** first maintenance log at any of the three new sites (Section 7).
**Stage 1 — A Reading.** The player maps the geophone taps and the twelve-gauge sounding stations; `MachineLogSystem` starts enrolling entries. The world-history ladder begins (Section 8.3).
**Stage 2 — The Fuse World.** The hallway of cabinets: three years of readouts, linen-coded shift charters. The interception log with one signed line (§8.2). The tape-silo door (§7) and the standard's linen (§2.4).
**Stage 3 — The Faceted Choice.** Recommission or seal each of the three facets in turn (Section 8.4, the maintenance-byline choices): the archive (tempest memory), the fire-computing room (artifacts), the vent shaft (power). Each choice is a byline that resolves the machine's three "needs" narrative, one way or the other.
**Resolution:** the Reckoning Call; the count; the menu (§10.3).
**Failure state:** the machine does not punish failure. It counts regardless — the standard has no failure clause.

## 5.2 `quest_verdict_the_reckoning_call` — The Reckoning Call (main, Day 210/240)

**Gate:** CULPABLE phase.
**Stage 1 — The Carrier.** A thin A/B tone on a dead band. Only a survivor with `item_archive_tape_silo_key` or 3+ read log entries can identify it as a census carrier (skill check, radio-craft).
**Stage 2 — The Search.** Who is calling? The evidence ladder (Section 8.3) resolves: the Tempest, the machines, the census due.
**Stage 3 — The Presentation.** The call resolves; the count is presented; the menu of endings (§10.3). The player does not get to decline the count — nobody asks them to. They only decide what to do with a count that has been made.
**Failure state:** none mechanical. The count happens on its schedule; endings are keyed to how the player treated the Enrolled Evidence, not to whether the player stopped it (they cannot).

## 5.3 `quest_verdict_the_hold` — The Hold Pending Count (main, Day 200+)

The UXO brain's secret (Section 2.6, `sys_uxo_brain`): the UXO fields were never left live — they were *held*. Held Pending Count.
**Stage 1 — The Dead Hand.** At `location_the_dead_hand_core`, a protected maintenance read shows the UXO field register. The player can now read the fuse-world log's real scheduling.
**Stage 2 — The Three Options** (genuine moral fork, no thumb on the scale):
- **MRC-01 RELEASE:** release the UXO fields — mine-clearance logic engages under the Pause Doctrine with the highest priority current. This conversion is the "arable-again" route: field-by-field de-mining begins across the Verge margin. Counter: the sector loses the UXO fields as a faction-free buffer zone — the Garrison and the Warlords immediately start eyeing the newly-swept land. (Choice; consequence.)
- **MRC-02 RETAIN:** keep the fields held as-is. The status quo continues; the buffer remains; the Warlords keep their toll geography; the Verge margin stays fenced. The machine does not care. The player has simply declined a conversion.
- **MRC-03 COUNT:** log the UXO fields into the census ledgers — the fields become a liability the machine's own registries now require to be accounted for every 1,827 days. Garrisons and Warlords alike are now *obliged* to track them. The single most "paperwork" outcome, and the one with the fewest bodies.
**Resolution:** a permanent world-state mutation (market/route-shaped, like `Mutation_MedicalSupplyGone` / `Mutation_Highway9Cleared`) — instant + a season-long echo, never a new relationship row.
**Failure state:** deferred only. If not resolved by Day 360, the fields remain "held," and the epilogue matrix's Verdict ending flags simply read the un-released state.

## 5.4 Side quests (5)

| id | Trigger | Objective | Dilemma | Reward | Consequence |
|---|---|---|---|---|---|
| `quest_verdict_the_mortars_timetable` | First maintenance log at `location_automated_mortar_pit` | Chart the 12-hour fuse schedule; trade the chart to Ostrowski (mapmaker) or the Warlords | Selling the schedule is intelligence; keeping it is safety; the pit fires regardless | Ostrowski trust, Warlord goodwill (or a standing "the toll knows you read clocks" modifier) | A Warlord surveyor starts measuring the pit — a quiet, one-beat escalation |
| `quest_verdict_eden_grabs` | Tape-spin at `loc_archive_tape_silo` | Recover Eden Vale's rig-bleed logs (11 months of tube broadcasts) | Recompose her final broadcast or preserve it archived — recomposing makes great radio; archiving keeps it clean | `item_archive_tape_silo_key` (second use), morale | If recomposed, the broadcast plays once and goes dead; if archived, the Archivists' faction deepens (their 11-month log now references a human voice the machine held) |
| `quest_verdict_the_shift_charter` | Reading the linen at the fuse world | Restore a completion from the Year-One sign-in ledger: shift 36, six names, one missing | The missing name is Iaran Bell — the hand on the valve. The ledger says nothing else | `item_fuse_world_shift_charter`, Locodex/memory +1 | The valve's actual first-maintenance since Year One reads "per §36" — the machine notes procedure was kept |
| `quest_verdict_the_tape_silo` | `item_archive_tape_silo_key` | One tape-spin per packet — a maximum of five spins | The archive's 1,831 "mattering" reels become the sector's paper, if the player routes them to the Archivists (faction), the Militia (vernacular history), or keeps them (home archive) | The machine's `reelsMattered` go down; a sector-wide `archive_proof` bonus per routing | Two routings are narrated in world-history; the third is a counted thing, and the counting is the point |
| `quest_verdict_the_summons` | Day 240, post-Call | Carry the presented count to one of the four Powers — Garrison (keeps it), Militia (reads it), Cult (burns it), a named Current | To whom does the count belong? The machine says the presentation must name the holders, not the readers | Per-faction trust, moreration | The other three Powers each learn of the transfer and add their own entry to the log — the machine's register grows in four places, permanently |

## 5.5 Micro-threads (3, unmarked)

1. **The Ledger Nobody Signed (Exp 06) — fourth lease.** The six-character alphanumeric debt code appears once in the machine's logbook — not marked PAID (Section 1.3, hook 11). No signature. The pack does not resolve it.
2. **The 11-month Waiter.** The comm array's bleed log includes one outbound call, made Year One, answered by a person in the Drown who never responded again. The pack neither identifies the caller nor the answerer.
3. **The Vessel's Cell, by meter.** The low-background counter at `loc_low_background_lab` holds a genuine anomaly reading for the Vessel's Cell window — 13 weeks, 0.4 mSv/hr, impossible for an unshielded human settlement. The Cold Count can read it; the Cult would prefer it not circulate; the pack does not adjudicate which is right.

---

# SECTION 6 — RADIO, AUDIO & THE VOICES

## 6.1 The **Reckoning Call** — tape (the 30 seconds)

Diegetic, played at the resolution, repeated three times. The voice is not electronic; it is *archival*. Maro Veen, Tape, Exchange−1Y, National Weather Service register:

> "This is the Office of Censuses. The count is open. All persons having custody of persons must present them. The count is open. Off-count is a penalty assessed against the holder. This message will repeat."

Thirty seconds. It does not say who is calling, what penalty, or from when. The sector's living humans get a census notice from a department whose staff died — delivered by machines that kept the department's schedule. No line explains this.

## 6.2 Radio corpus (12 signals)

`faction_war_radio.json`-shaped, `broadcasts[]`, each a short diegetic text. Selection (full corpus to batch file):

1. **The Meter Reads 11:42** — a data burst that is three numbers and nothing else.
2. **The Fuse Serviced** — a maintenance confirmation with a timestamp exactly matching a Mortar Period.
3. **The Wing Sleeps** — the drone-hive draw −0.5° readout, first broadcast post-Call.
4. **The Off-Count Is Assessed** — the Call's most human line, on a loop that is only the first six seconds.
5. **Eden Was Here** — 11 months of tube-bleed, a single day's worth, the vocabulary of the Weather Service.
6. **The Count Is Open** — the Call's full text, once, on a different band, the one amateur rigs were on.
7. **The Clock Disagrees** — a three-day drift, presented as data.
8. **Geophone Taps Under the Allotments** — the farm's seismic signature, unlabeled.
9. **Valve Accessed per §36** — the shift-water readout, post-`quest_verdict_the_shift_charter`.
10. **The Reels Matter** — the archive's count, post-`quest_verdict_the_tape_silo`.
11. **The Presentation Names the Holders** — the Call's epilogue, whichever Power took the count.
12. **Carrier on Census Window** — the pilot tone's signature, one band, identifiable to a radio-craft survivor.

## 6.3 Ambient audio design (per state)

| State | Loop | Layers |
|---|---|---|
| Warm Range, before Call | dry wind, occasional cable-slap | distant sequential thuds (12-gauge sounding stations, one per 40s), tape hiss sound-collage when within 2 tiles of a splice |
| Fuse World | very low 120Hz hum (the cable's own carrier), the room's ventilation | clock-tick of a monthly checkup timer (40s), the tape-silo door's occasionally-latched solenoid |
| Reckoning Call resolve | the tape, processed thin, mono, slightly warped | after the third replay: silence, then the carrier returns, one level quieter |
| The Summit, clear night | wind, and the summit's cold light | the light blinks on the idle schedule; there is no sound, and the absence is the sound |

## 6.4 The one repeated voice (human, but not a person)

**Eden Vale**, the 11-month rig-bleed. Her only line, repeated at intervals once her log is recovered — the pack's most human voice is a tape, and the tape does not stop being a tape:

> "Still here. Static's thinning. That's not good news, that's a storm on the way. If anyone's reading, the array's drawing again. I don't know what it's drawing for. I don't think it draws for us."

---

# SECTION 7 — NEW & REUSED LOCATIONS

## 7.1 New locations (3 + 1, house-voice descriptions)

### `loc_geophone_pit_1`
**The First Geophone Pit** · d6 · 5.5h · 34 rads · The Spine
> A concrete collar sunk like a wellhead, the lid propped on a brick. Below: a seismometer array the size of a dinner plate, bolted to bedrock, humming at a pitch almost too low to hear. The cable runs east, into the treeline, under the ridgeline. No one has recorded anything in the log for four years except the array itself, and the array reads the ground as if the whole valley were one slow heartbeat. A hand-painted sign on the lid, painted over twice: TEMPEST SITE 01 — KEEP OUT — DO NOT ENTER — ENTER AT YOUR OWN RISK. The last line is in a different hand, and it is not a warning.

**Lore:** The geophone network's first pit, and the reason the Verge's farming signature is the cleanest baseline in the machine's register. The "enter at your own risk" line was added by the Cult, for reasons the Cult's own theology cannot agree on. `knowledge_key: lore_verdict_geophone_one`.

### `loc_twelve_gauge_array`
**The Twelve-Gauge Array** · d7 · 6.0h · 38 rads · The Spine
> Twelve shot-firing sounding stations on the ridge, each a one-metre steel post with a grease-stained plate reading TEMPEST SITE 07 and a firing order stencilled in flaking yellow. The ordnance is long gone — the holes are empty — but the plates list the charge weights, the depths, the shot ordnance. Somebody has been keeping the plates legible, which is odd, because the nearest human settlement is nine hours away. The array is the fuse world's quiet door: the cable that runs under the treeline is the Tempest's own line, and it runs here because nobody on the surface walks this ridge.

**Lore:** The sounding stations are the machine's longbones. The plates have been kept legible by Selya Saltmarsh — census clerk, Verge — who visits on no schedule at all, and logs the plates in a notebook she keeps in her coat, and leaves the plates cleaner than she found them. The pack never once explains why.

### `loc_network_fuse_bunker`
**The Fuse World** · d8 · 7.5h · 42 rads · The Spine
> A dry shielded service way between two facilities, entered through a door the size of a bank vault with a handle that turns freely. Three years of readouts line the walls in glass-fronted cabinets, each bearing a linen-coded shift charter in a frame. The far end is a tape-silo door: six inches of steel, a wheel handle, and a solenoid that clicks when the tape's schedule demands. The floor is swept. The swept floor is the strange part: the dust in the corners is three years deep, and the swept path is one person's width, and the person's width ends at the tape-silo door.

**Lore:** The pack's environmental-storytelling spine. The linen codes decode to the Tempest's Standard (§2.4); the swept path is Iaran Bell's, the maintenance supervisor whose hand is the last hand on the valve (Section 2.7; `quest_verdict_the_shift_charter`). The one interception log has a single signed line (§8.2).

### `loc_archive_tape_silo`
**The Archive Tape-Silo** · d9 · 8.5h · 48 rads · The Spine
> A vault the size of a chapel, wall-to-wall with steel racks of tape reels, each rack tagged by year. Twenty-one racks, four years per rack, the tags to the front, the labels in the same Department of the Interior hand as the linen charters. The deepest rack is labelled CURRENT YEAR — and every reel in it is dated five years ago, because the archive is not in the habit of taking dictation. At the end of the centre aisle, bolted to the floor, a reading lectern: a slot for a reel, a knob, a speaker the size of a fist. No one has ever heard it read. The dust on the lectern is disturbed, and the disturbance is a handprint, and the handprint is small, and there is no record of a child in the machine's staff.

**Lore:** The mystery's resolution point. The lectern is how the machine speaks — a reel in the slot, the knob turned, the tape tells the sector what it has been counting. The child's handprint is left unexplained, unremarked, and (canon) entirely without a second mention, because the pack's rule for its mysteries is that they are either resolved or they are not even begun.

## 7.2 Reused locations (with new The-Verdict hooks)

| Existing id | New hook |
|---|---|
| `loc_comm_array` | The 12 radio corpus signals (§6.2) are intercepted here; the amateur tube rig history (Eden Vale). |
| `loc_radio_relay_mast` | The 03:40–04:10 maintenance window; the source (geothermal bleed). |
| `loc_summit_relay` | The face; the cold light; the census arithmetic visible on clear days. |
| `location_automated_mortar_pit` | The 12-hour fiscal clock; `quest_verdict_the_mortars_timetable`. |
| `location_drone_hive_silo` | The wing; the sleep readout. |
| `location_the_dead_hand_core` | The Hold Pending Count; `quest_verdict_the_hold`. |
| `location_geothermal_vent_shaft` | The power source; the rude, physical truth. |
| `loc_low_background_lab` | The Vessel's Cell anomaly reading (micro-thread 3). |

---

# SECTION 8 — EVIDENCE, THE ITEM CORPUS, AND THE WORD-LADDER

## 8.1 The `evidence` fragment (the pack's currency)

A machine-log enrollment: a reading that is *counted* only once a human reads it. `evidence` fragments are item-tagged, single-use, and flow into the machine's ledger as `read:true`. Skeleton (matches existing item catalog shape):

```json
{
  "item_id": "evidence_<snake_case>",
  "name": "<Display Name>",
  "category": "StoryItem",
  "tier": "Makeshift | Salvaged | Old-World | Masterwork",
  "lore_flavor": "2-3 sentences: where it was read, what it records, what it leaves unanswered",
  "mechanical_effects": { "enrolled_evidence": 1 },
  "crafting_uses": null,
  "downstream_quest_trigger": "quest_verdict_<...> or null",
  "faction_affinity": "faction_the_tempest",
  "rarity": "Common | Uncommon | Rare | Unique"
}
```

## 8.2 The item corpus (12 items to start; 15 by batch)

Full flavor text lives in the creative pack (`expansion_08_the_verdict_creative_pack.md`). Inventory by id:

| id | category | downstream | notes |
|---|---|---|---|
| `evidence_geophone_hymn` | StoryItem | `quest_verdict_the_warm_range` | the farm's seismic signature, unlabeled |
| `evidence_twelve_gauge_steel` | Material | `quest_verdict_the_warm_range` | the fired-plate's ordnance log |
| `evidence_fuse_linen` | StoryItem | `quest_verdict_the_shift_charter` | the Standard's linen |
| `evidence_census_draft` | StoryItem | `quest_verdict_the_reckoning_call` | a paper clerk's partial ledger |
| `evidence_mailroom_tape` | StoryItem | `quest_verdict_the_hold` | a carbon-copy censusing rota from Year One |
| `evidence_uxo_register` | StoryItem | `quest_verdict_the_hold` | the hold register, read |
| `evidence_call_calibration` | StoryItem | `quest_verdict_the_reckoning_call` | the calibration burst |
| `evidence_call_plain` | StoryItem | `quest_verdict_the_reckoning_call` | the plain burst |
| `evidence_reels_matter` | StoryItem | `quest_verdict_the_tape_silo` | the archive's own accounting |
| `evidence_valve_s36` | StoryItem | `quest_verdict_the_shift_charter` | the valve read per §36 |
| `evidence_eden_log` | StoryItem | `quest_verdict_eden_grabs` | 11 months of rig-bleed |
| `evidence_veen_your_people` | Rare | the count itself | **the pack's single allowed gut-punch:** the census line naming the player's own shelter population, in the machine's register |

## 8.3 The word-ladder (world-history beats, `world_history.json` shape)

Each layer is a *physically found* object at one plant — located knowledge (`discovery_location_id` / `knowledge_key`), exactly as canon demands:

| knowledge_key | Title | Found at | Layer |
|---|---|---|---|
| `lore_verdict_geophone_one` | The First Geophone Pit | `loc_geophone_pit_1` | 1 — a reading is a measurement |
| `lore_verdict_shift_charters` | The Linen Codes | `loc_network_fuse_bunker` | 2 — a schedule survives |
| `lore_verdict_standard` | The Standard for the Continuance of Service | `loc_network_fuse_bunker` | 3 — the charter itself |
| `lore_verdict_the_hold` | Hold Pending Count | `location_the_dead_hand_core` | 4 — the UXO fields were held |
| `lore_verdict_the_call` | The Reckoning Call | `loc_comm_array` | 5 — the count is open |
| `lore_verdict_the_count` | The Count | `loc_archive_tape_silo` | 6 — the census resolves |

## 8.4 The three facets — maintenance-byline choices

At the resolve window, the player, at each facet, chooses one of two bylines (Brutal/Practical, Positive/Neutral always present), each writing a permanent world-state line:

| Facet | Rustic read | Brutal read | Practical read | Positive read | Neutral read (default) |
|---|---|---|---|---|---|
| **The Archive** (memory) | the machine's own memory, kept | the memory of the count, kept till the next interval | reelsMattered recomputed | the Archivists' faction deepens | the tape stays a thing that waits |
| **The Fire-Computing Room** (artifacts) | kept as prayer | kept as evidence | stripped of charge | donated to the museum shelf | left as a machine of record |
| **The Vent Shaft** (power) | kept as heat | kept on | reduced to a trickle | returned to the weather | left as a machine that serves |

The three choices are the pack's literal "three-needs": the machine needs a memory (archive), needs a body of history (fire-computing), needs a warmth (vent). The player decides each byline, and the epilogue matrix reads all three.

---

# SECTION 9 — JSON SCHEMAS FOR DOWNSTREAM BATCH GENERATION

All schemas below are copy-paste executable; ids follow snake_case; tone follows the fingerprint in Section 11; every flag/ID referenced must resolve against existing catalogs or the pack's own new catalog (`verdict_data.json`).

## 9.1 Verdict master data (`verdict_data.json`)

```json
{
  "catalog": "verdict",
  "schema_version": 1,
  "currencies": [
    { "id": "enrolled_evidence", "label": "Enrolled Evidence", "note": "A reading the machine keeps only because a human read it. Fragile: it is a record, not a resource." }
  ],
  "readout_steps": [
    { "id": "step_fuse_advance", "label": "Fuse world schedule advances 12 minutes", "trigger_phase": "counted", "readout": "A clock being serviced, not an attack." },
    { "id": "step_drone_sleep", "label": "Drone-hive draw -0.5°", "trigger_phase": "counted", "readout": "The wing standing down." },
    { "id": "step_summit_light", "label": "Summit light, cold, idle", "trigger_phase": "culpable", "readout": "The sector seeing itself counted." },
    { "id": "step_census_carrier", "label": "The carrier tone on a dead band", "trigger_phase": "culpable", "readout": "A schedule, not a threat." }
  ],
  "facets": [
    { "id": "facet_archive", "label": "The Archive", "need": "memory", "bylines": ["rustic", "brutal", "practical", "positive", "neutral"] },
    { "id": "facet_fire_computing", "label": "The Fire-Computing Room", "need": "history", "bylines": ["rustic", "brutal", "practical", "positive", "neutral"] },
    { "id": "facet_vent_shaft", "label": "The Vent Shaft", "need": "warmth", "bylines": ["rustic", "brutal", "practical", "positive", "neutral"] }
  ],
  "endings": [
    { "id": "ending_verdict_the_sector_recounts", "label": "The Sector Recounts", "trigger": "enrolled_evidence >= threshold AND presented count honored" },
    { "id": "ending_verdict_the_count_is_held", "label": "The Count Is Held", "trigger": "enrolled_evidence < threshold AND count not presented" },
    { "id": "ending_verdict_the_offer_is_a_lease", "label": "The Offer Is a Lease", "trigger": "presented count declined (no honor)" }
  ]
}
```

## 9.2 Currents entry (append to `currents.json`)

As written in 4.1, schema identical to the existing 14.

## 9.3 Door encounters (append to `door_encounters.json`; existing schema exact)

Hours 14–24 window, threat 1–3, no "mysterious stranger" more than once a season. 8 new beats, batched from this corpus:
- `door_encounter_verdict_tape_seller`
- `door_encounter_verdict_relay_repair`
- `door_encounter_verdict_census_clerk`
- `door_encounter_verdict_sound_engineer`
- `door_encounter_verdict_salt_gatherer`
- `door_encounter_verdict_soil_sampler`
- `door_encounter_verdict_tape_exchange` (the one representative of the Cult, 1×/season cap)
- `door_encounter_verdict_clock_parasite`

Full flavor in the creative pack.

## 9.4 Locations (append to `locations.json`; existing shape exact: `id, displayName, description, dangerLevel, travelHours, baseRadsPerHour`)

4 new (Section 7), 8 reused-hook. Descriptions from Section 7's house-voice text.

## 9.5 Items (append to the item corpus of choice; existing shape + 8.1's `evidence` skeleton)

12 in 8.2 + batch targets below.

## 9.6 Quests (append to `year_of_ash_quests.json`-shaped quest catalog; existing shape: `id, title, faction, minDay, stages[{stageIndex, objective, requiredItemId, isCompleted}]`)

`quest_verdict_the_warm_range`, `quest_verdict_the_reckoning_call`, `quest_verdict_the_hold`, + 5 side quests (Section 5.4).

## 9.7 Radio (append to `faction_war_radio.json`-shaped catalog; existing shape `broadcasts[]`)

The 12-signal corpus (Section 6.2).

## 9.8 World history (append to `world_history.json`; existing shape exact: `era, year_month, title, body, discovery_location_id, discovery_trigger, knowledge_key`)

The 6 word-ladder beats (Section 8.3).

---

# SECTION 10 — DIFFICULTY, PACING & ENDGAME

## 10.1 The pressure curve (week-by-week)

| Week (Day) | Pressure | The Verdict's contribution |
|---|---|---|
| 160–180 | baseline | Warm-range maintenance logs begin (passive, no drain) |
| 181–210 | Exp 05 Deep Freeze | The carrier tone joins the radio; the fuse-world sites open; evidence starts enrolling |
| 211–240 | Exp 05 Phase V begins | CULPABLE — the countdown; the Call's pilot tone; the summit's cold light |
| 241–300 | Total war | COUNTED — the Call resolves; the menu opens; readout steps fire; endings become rewardable |
| 301–360 | Exp 05 Thaw + Exp 06 epilogues | The Verdict's own three ending flags integrate into the existing epilogue matrix |

## 10.2 The cruelty budget

Rule of one: no two Verdict beats land back-to-back with the other packs' gut-punches. The Reckoning Call is scheduled to fire *after* Exp 06's decision points resolve and *before* Exp 05's final confrontations — a breath, exactly where the count is owed. The Count's standing gut-punch (`evidence_veen_your_people`) is a Read-beat, not a Tragedy-beat: it is legible, it is not a death, and it is the pack's only allowed moment of cold quiet.

## 10.3 The final menu (`quest_verdict_the_reckoning_call` Stage 3)

The presentation is made; the player chooses what the sector does with a count that has been made:

- **PRESENT (honor the count)** — route the census to a Power or Current. Sets `ending_verdict_the_sector_recounts`; the sector's institutions begin a season-long recount; the machine's register closes with the count presented and read.
- **HOLD (decline to present)** — keep the count; the machine does not mind; the count remains open; `ending_verdict_the_count_is_held` — the pack's appointment-without-consequence ending, the one where nothing happens, and the nothing is the quiet.
- **DISCHARGE (fund the reading)** — convert the count into a *lease*: the sector's living pay the count's maintenance requirement quarterly; the offer is a lease, the lease a contract; `ending_verdict_the_offer_is_a_lease`.

## 10.4 The three endings (100–150 words each, vignette)

Full texts in the creative pack's endgame corpus. Summaries:

1. **The Sector Recounts** — the count is presented, read aloud at a grain-silo market by a man who does not look up, and the sector's institutions spend three months agreeing on what the number means. They do not agree. The machine's register closes. The last line is the count, presented, and the fact that it was read.
2. **The Count Is Held** — the presentation is never made. The machine holds the count Pending. The carrier tone continues on the dead band, and the UI, forever after, reads the census window as OPEN — deskbound, patient, exactly as patient as the Standard. Nothing happens. The nothing is the ending.
3. **The Offer Is a Lease** — the count converts to a lease, quarterly, enforceable by the machine's own registers: maintenance, readings, a census every 1,827 days. The sector discovers it has a landlord. The landlord does not care about the sector; it cares about the lease. On paper, everything is in order. On paper, everything has always been in order.

## 10.5 New Game+ / Legacy hooks

Three flags carry forward: `verdict_fuse_advanced`, `verdict_wing_slept`, `ending_verdict_<which>`. The next run's first visitor, wherever the player goes, is a census clerk from a department that does not exist, who is asking whether the new shelter's persons are presented. The pack's quiet permanence: the machines do not reset with the player. The count does not either.

---

# SECTION 11 — STYLE FINGERPRINT & VALIDATION CHECKLIST

## 11.1 Three gold-standard samples (what all downstream output must imitate)

**Item flavor (the census line):**
> `evidence_veen_your_people` — "The count is presented. It names the shelter's persons, by name, in the machine's register: fourteen, then fifteen, then the hand that wrote the line. A machine does not reason. It counts. This is the count."

**Encounter setup (the tape seller):**
> "Three in the morning, a knock with a rhythm that is practiced and unhurried: three, three, one. At the peephole, a woman with a tape reel on a strap. She does not ask for food. She asks whether the shelter keeps a radio, and whether the shelter has heard the count."

**Journal entry:**
> "The machines kept the calendar. The calendar came due. The count is open. Nobody here is on the count — we are the people who present persons. I wrote our names, and the machine wrote the count, and both are true."

## 11.2 Validation checklist (12 yes/no gates for every downstream piece)

1. Does it name the machine without making it a character? (No preferences, no malice, no "perhaps it felt…" — the tone lock's §1.5 restated once.)
2. Does every `item_id`, `quest_<id>`, `location_<id>`, `flag_<id>` resolve against an existing catalog or `verdict_data.json`?
3. Is the id snake_case, and is it on the master id list or the pack's own list?
4. Does the prose carry at least two senses (sound, light, smell, temperature, texture) per setup paragraph?
5. Is the voice cold, exhausted, human, restrained — no exclamation marks, no moralizing, no telling the player how to feel?
6. Are "subtle" numbers honest? (Flux ambiguity only where canon says so; no hidden 1-in-N "gotchas" in Pause/Load design.)
7. Does every quest and micro-thread close either by resolution or by remaining open *on purpose*, and does the fork's category match the schema (honor/hold/lease, maintenance/no-maintenance, present/hold/discharge)?
8. Does every outcome end on an image, not an explanation?
9. Does it avoid the Forbidden List (no chosen one, no prophecy, no secret royalty, no glamorized violence, no "evil machines")?
10. Does it tie to an explicit canon anchor named in Section 1.3, and does it cite it?
11. Is every machine behavior derivable from the Standard's actual text (§2.4)? (If a downstream writer needs the machine to do something new, it must quote an Article or be rejected on the spot.)
12. Does the piece respect the closed map (no fifth Power), the four-Power canon, and the "no new afflictions / no new victory paths" umbrella?

## 11.3 Batch generation task cards (10, ready to run)

1. **12 evidence items** — matching 8.1's skeleton, the tone of `evidence_veen_your_people`, each tied to one facility.
2. **8 door encounters** — Section 9.3 ids, existing `door_encounters.json` schema, no "mysterious stranger" repetition, hours 14–24.
3. **12 radio bursts** — Section 6.2 ids, `faction_war_radio.json` schema.
4. **6 world-history beats** — Section 8.3 ids, `world_history.json` schema, each a physical findable.
5. **15 interviewee-vignettes** — human reactions (Verge farmer, Toll clerk, Cult novice, Drown rower, Machine-maintenance kid) to the Read counts, in the house register.
6. **11 graffiti / wall texts** at the fuse world and the array — Section 10.2 / creative-pack pattern.
7. **40 shelter barks** — the survivors' re-voiced Census-related barks (fear, hope, paperwork humor) mapped to the Dose's existing bark system shape.
8. **5 ending vignettes** (the original 3 + 2 alternating versions for later saves) — Section 10.4.
9. **3 journal entries per RiskBiasTrait** (Paranoid, Denialist, Fatalist, Empath, Realist) reading the same census carrier — `JournalSystem` / `JournalVoice` integration shape.
10. **The Cult's full limestone-witness exchange** — how the Quiescent-lineage of Sect. 4.4 phrased their one permitted reference to the meters, including the line's 2 rejected rewrites (noting why), per temperature discipline.

---

# SECTION 12 — SELF-CRITIQUE PASS (mandatory)

## 12.1 Cliché audit

| Nearly-wrote | Subversion |
|---|---|
| The machine begins firing at the shelter (the Contradiction) | The machines are *unconcerned*, never hostile; the mortar pit's schedule is a clock to repair, not an artillery duel. No shots are fired at the player's bunker in this pack. |
| A rogue-AI monologue revealing "the terrible truth" | The machine has no voice but the tape's; the "truth" is bureaucracy — a census, a lease, a count — and it is delivered in National Weather Service register. |
| A doomsday switch to be thrown | The destructive choice is *not* a lever; `quest_verdict_the_hold`'s MRC-03 COUNT is a *ledger* choice that changes the obligation, and the pack never dangles a "destroy the machine" button, because destroying the meter is not a choice the Standard recognizes. |
| A hidden faction revealed as evil | The Tempest is a utility with a schedule. Its only "evil" is that it is complete. |
| A war hero ghost saving the day | The fuse world's hero (Iaran Bell) never saved anything; the record cannot even confirm a death. The funereal path is a dusty handprint and an undersigned year. |

## 12.2 Continuity proof (5 lines that explicitly reference prior packs)

1. "…the UXO fields were never awake. They were *held* — held Pending Count…" (`location_the_dead_hand_core`, base canon).
2. "Allocation 12 is a row number in the machine ledger, and the two numbers are the same number." (`02_THE_LIST.md` spine).
3. "Irina Vel's ledger records what human hands chose to write down. The Verdict is the other book." (`expansion_07_the_dose`).
4. "The six-character code appears once in the machine's logbook — not marked PAID." (`expansion_06_the_muster`, §VI.5, the Ledger Nobody Signed).
5. "The Cult's daily reading of the shrine dosimeter is, from the machine's point of view, a scheduled maintenance reading performed by a volunteer." (`lore_spine_the_reading`, base canon).

## 12.3 Human cost proof (5 places where mechanics carry emotional weight)

1. **Read-only truth.** A log entry is only true once a human reads it — the pack's whole cost is attention.
2. **The count itself** is the player's own people, by name, in the machine's register (`evidence_veen_your_people`).
3. **The linen charters** are dead people's job descriptions, framed, at the one place machine and human ever met.
4. **The swept path** ending at the tape-door is Iaran Bell's — one person's width, three years of dust.
5. **The lease ending** taxes the living quarterly for a count that was taken without them; the sector discovers it has a landlord who never asked.

## 12.4 Surprise ledger (3+ subversions planted)

| Plant | Where |
|---|---|
| The "forge" side of `quest_verdict_the_hold` (MRC-01 RELEASE) is *not* a moral trap — it is simply a conversion with consequences. The player who expects a twist gets a spreadsheet. | §5.3 (MR-01) |
| The most human voice in the pack is a tape echo of a NWS-style announcer (Eden Vale) — the "character" is a recording, and the game does not make her a friend. | §6.4 |
| The Cult receives the machine's existence as liturgy (Sect. 4.4's one line) — a religious community accepting a profane fact as revelation, without a single line of adjudication either way. | §4.4 |

## 12.5 Downstream readiness

Every schema in Section 9 mirrors an existing catalog byte-for-byte (verified against `currents.json`, `door_encounters.json`, `world_history.json`, `year_of_ash_quests.json`, `faction_war_radio.json`); every seeded flag resolves in `verdict_data.json`; the batch cards (11.3) are self-contained; the fingerprint (11.1) supplies the register; the checklist (11.2) gates acceptance. The pack's design does not require new `faction_lore.json` rows, new hegemony entries, new afflictions, or a fifth Power — the closed-map rule holds.

---

*The machine keeps the count. The people keep the rest. This pack is the ledger between them, and it has been open for five years.*
