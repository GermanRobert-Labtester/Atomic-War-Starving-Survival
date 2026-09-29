# ASHFALL — RADIO FREE ASHFALL
### Run your own broadcast · A call sign, a schedule, a listening region, and someone who is listening back

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/radio-free-ashfall-2026-09-29.md`
**Family:** "New ways to play" — `docs/expansions/expansion_new_ways_to_play_index.md`.
**Tone lock (inherited):** cold, exhausted, human, restrained. No magic, no real countries, wars, people, stations or copied text.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

Every voice on the dial in ASHFALL belongs to someone else. Civil Defense reads the water hours. The Garrison reads its orders. The Crater's children sing. The Open Classroom tells a story at night and nobody knows who tells it. A machine counts numbers. The player *listens* — and, through Plan 173, can already prepare a bulletin or a bedtime story and deliver it into a slot on *someone else's* station, watching the audience morale move.

Radio Free Ashfall turns the listener into the **station**. Not a job, an identity: a call sign the region learns, a frequency you had to find room for, a schedule you keep or break, listeners who write back, and a hard fact that follows every broadcaster — **a transmitter is a lamp, and lamps are found.**

The fantasy is not "be popular". It is **be believed**: to be the voice a settlement three days east checks before it opens its gate, and to know what that belief costs to keep.

### 1.2 Pillars

1. **You are a station, not a slot.** One persistent identity: call sign, frequency, power, schedule.
2. **Trust is the currency.** Listeners reward what has been true. One caught lie costs a season.
3. **Every broadcast has a signature.** The louder and longer, the easier you are to find — by the same triangulation the player already uses on others.
4. **The audience is the region.** Listeners are settlements the Living Region already simulates; reach depends on power, weather and terrain.
5. **The mailbag is the story.** Listeners write back with requests, warnings, confessions, and lies.
6. **Ship dark.** With no station data present, radio is exactly today's.

### 1.3 Not this

Not a music-player. Not a second propaganda system (PsyOps/Propaganda stay). Not a real-time audio tool. Not a way to win the game by talking.

---

## 2. What the code and data actually say (audit)

| # | Finding | Evidence | Status |
|---|---|---|---|
| F1 | Six authored stations, **all NPC-owned** (Civil Defense 88.5 Official, Garrison 88.4 Partisan, Crater 104.2 Partisan, Open Classroom 91.3 Anonymous, Numbers/SIGINT 14.487 Automated, Automated Relay 142.85 Automated); each has persona voice, reliability class, default state, silence/jammed text, signal profile, equipment requirements, and a `schedule`. | `radio_stations.json`; `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs` | LIVE |
| F2 | Player program production (Plan 173): a program template → prep ticks → delivery into a named station **slot** → audience response (morale delta, reach grade, opportunity) → follow-up hook. **Two authored programs** (Shelter Morning Bulletin, Classroom Evening Story). Own save section. | `radio_programs.json`; `Radio/RadioProgramProductionSystem.cs`; `src/Main.RadioProgramProduction.cs`; `Save/SaveSectionRegistry.cs` L166 | LIVE |
| F3 | PsyOps (Plan 157): campaigns with target faction, theme, reach, power demand, receptiveness, loyalty pressure/day, jamming and counter-propaganda; propaganda campaigns/templates. | `Radio/PsyOpsSystem.cs`; `propaganda_campaigns.json`, `propaganda_templates.json`; section `psyops` | LIVE |
| F4 | Signal trust: a 0–100 ledger (+2 answered, +5 rescue, −2 ignored, −5 ambush) for **distress signals**. | `Radio/SignalTrustLedger.cs` | LIVE (distress only) |
| F5 | Direction-finding, triangulation, acoustic DF, UV corona detection, authenticity evaluation exist for **incoming** signals. | `Radio/SignalTriangulationSystem.cs`, `DirectionFindingCatalog.cs`, `SignalAuthenticityEvaluator.cs` | LIVE |
| F6 | Shelter radio station (tuning, signal lock, triangulation), propagation, recording, receiver plan, schedule coordinator; section `radio_station`, `radio`. | `Radio/ShelterRadioStationSystem.cs`, `RadioPropagation.cs`, `RadioRecordingSystem.cs`, `RadioScheduleCoordinator.cs` | LIVE |
| F7 | 87 authored radio broadcasts, 16 intercepts (encrypted, triangulable), 14-faction chatter corpus; comms array targets (8). | `radio.json`, `radio_intercepts.json`, `faction_radio_corpus.json`, `comms_targets.json`; `Communications/CommunicationsSystem.cs` | LIVE |
| F8 | Nothing tracks the **shelter as a source**: no call sign, no own frequency, no audience ledger, no listener mail, no adversary that reacts to being broadcast at. | grep `Radio/`; F2 | **GAP** |
| F9 | Audience response is computed per delivered job (reach grade, morale delta); it does not accumulate. | `RadioProgramProductionSystem.CalculateAudienceResponse` | GAP |
| F10 | Distress-signal follow-ups and rescue missions exist (destination resolver, dispatch preflight, rescued-arc projection). | `Radio/Distress*.cs`, `RescueDispatchPreflight.cs` | LIVE |
| F11 | UI: `RadioPanel`, `RadioIntelligencePanel`. | `src/UI/` | LIVE (VERIFY whether program production has a surface there) |
| F12 | Selftests: program production, black-project/PsyOps, radio verbs. | `src/Host/RadioProgramProductionSelfTest.cs`; HostCli | LIVE (VERIFY args) |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

- **"Own your broadcast" vs the six NPC stations (F1).** Systems win: the six stay untouched. The player's station is **one additional persisted entry**, not an edit to the authored catalog, and never occupies an NPC station's slot.
- **Trust exists but only for distress (F4).** Systems win: extend the *pattern* of the existing ledger (bounded 0–100, deltas as constants) for a **Voice Trust** keyed to the player's station; do not overload the distress score.

---

# PART II — THE STORY

## 3. The Station

### 3.1 Getting on the air

The player needs three things, each already a system: a **transmitter** (comms array tier / power), a **frequency** with room on it (the dial is crowded: 88.4, 88.5, 91.3, 104.2, 142.85, 14.487), and a **presenter** (a survivor; the existing program job already asks for one). The **call sign** is chosen once, from a short authored list plus the shelter's own name.

### 3.2 The Schedule Grid

The day has slots. Each slot is booked with a **program**: news, story, music, messages, classifieds, or a coded hour. Empty slots carry a test pattern; a station that goes dark for a week loses standing with its audience (they tune elsewhere).

| Program | What it does | What it costs |
|---|---|---|
| **Bulletin** | Reads the Board (*The Living Region*) — grade-honest news. Builds Voice Trust when true. | Prep ticks; a presenter; batteries. |
| **Story hour** | Morale for the audience; children's hour in Year Two. | A presenter who can hold a room. |
| **Messages** | Read out names; reunions; a family learns someone is alive. | Verification time; the risk it is a lie. |
| **Classifieds** | Trade offers and wants; lifts route demand (*The Long Line: Freight*). | Nothing, until someone is robbed on the strength of one. |
| **Health hour** | Public bulletins in an outbreak (*The Plague Year*). | Being wrong. |
| **The coded hour** | Numbers, for people who know what they mean. | A cipher the shelter has to keep. |

### 3.3 The Audience

Listeners are **regions**, not people: each canonical region (the Living Region vocabulary) has a **reach** (from power, propagation, weather, terrain) and an **affinity** (from what the station has said and whether it held). Both are derived and small; only *affinity* and *trust* persist.

### 3.4 Voice Trust and the truth policy

Every broadcast carries a **truth grade** — true, spun, or false — set by the *presenter's script*, not by the game. The world eventually reveals the truth; the Trust ledger moves accordingly. Spin works for a while. A caught lie costs a season. The game does not tell the player which was right.

### 3.5 The lamp

Every broadcast raises **Signature** — a function of power, duration, and repetition. Signature decays overnight. When it exceeds what a faction's direction-finding can resolve, that faction *finds* the shelter: a visitor, a raid probe, a jamming campaign, a knock at the door. The mechanisms already exist (triangulation, PsyOps jamming, door encounters); Radio Free Ashfall points them at the player for the first time.

The choice is old and simple: **be heard, or be safe.** Broadcast from the shelter and be found; broadcast from a **relay** (a waystation, an outpost, a hill) and be found *there*.

### 3.6 The Mailbag

Once a day, if the station has an audience, it receives **letters**: a request for a song, a warning about the river, a confession, a plea to read a name, a lie meant to be read out. Each letter is a small decision. The mailbag is authored content on existing choice machinery; it does not invent a new event system.

### 3.7 What other voices do

- **Civil Defense** treats you as an unlicensed competitor and then as a colleague.
- **The Garrison** jams and then invites.
- **The Crater** answers back — in song.
- **The Open Classroom** — the anonymous story-teller — is either your rival or your friend, and the player might never learn which.
- **The Numbers station** is not listening. Probably.

## 4. Four movements

**I — Dead Air (first month).** A transmitter, a call sign, three bulletins. No one answers. *Ends on:* the first letter.

**II — The Hour (first season).** A schedule the region can set its day by. *Ends on:* a settlement that opens its gate because you said it would be safe.

**III — The Lamp (second season).** Signature. Someone finds you. *Ends on:* the player's choice between going quiet, relaying, or answering.

**IV — Free Ashfall (third season onward).** The station is a fact of the region: children in Year Two grow up on it; the Board lists the settlements that listen. *Ends on:* the day the player reads a name that everyone has been waiting for — or doesn't.

## 5. Voice samples

- *Sign-on:* "This is [call sign], on a frequency somebody is going to complain about. Water hours are the sixth and the fourteenth. The river is up. That is all."
- *Letter:* "You said the road east was open. It was not. My brother is not coming back and I would like you to say that on the air. — anonymous"
- *Reply:* "We were wrong about the east road on the ninth. Two people did not come home. We are sorry. Water hours are the sixth and the fourteenth."
- *Jammer:* "…the… hour… brought… to you… by… the Garrison… stay… indoors…"

## 6. Content plan

- **W1 — Sign-ons, station idents, test patterns:** 24.
- **W2 — Programs:** 6 categories × 4 templates (24), replacing today's 2 as *additions*.
- **W3 — Mailbag:** 60 letters (requests 15, warnings 12, confessions 10, pleas 13, lies 10).
- **W4 — Reactions:** 5 factions × 4 (find / jam / invite / threaten) = 20.
- **W5 — Reveals:** truth-grade reveal lines (30).

## 7. Non-goals (restated)

No edits to the six authored stations. No second propaganda system. No new save section (nested in `radio_program_production`, DEC-RF-02). No new routed panel (DEC-RF-06). No real-time audio.

## 8. Risks

- **Becomes a menu game.** *Bound:* one book-a-slot action per day; the grid is a table, not a minigame.
- **Signature punishes without warning.** *Bound:* a visible Signature band, and a warning line before any faction acts.
- **A second trust system.** *Bound:* one Voice Trust, same 0–100 pattern, no cross-writes to the distress ledger.
- **Tone drift** to glorified propaganda. *Bound:* the game never rewards false grades beyond a short window.
