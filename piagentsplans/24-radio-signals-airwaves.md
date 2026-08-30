# Plan 24 — Radio, Signals & the Airwaves World

> **Theme:** The airwaves are ASHFALL's signature information channel (analog tuner, number
> stations, faction radio, distress calls) but the content is fragmented across 6+ files with
> no unified programming schedule. This plan turns radio into a living broadcast world.
>
> **Key evidence (verified):** 118 broadcasts across `radio.json` (50) + `year_of_ash_radio.json`
> (50) + `verdict_radio.json` (13) + `radio_distress_signals.json` (5); `faction_radio_corpus.json`;
> `RadioTuner.cs`, `FactionRadioEngine.cs`, `VerdictRadioSystem.cs` live; **0 audio refs** (Plan 07).

---

## Task 24A — Unified broadcast schedule & programming grid

**Goal:** Turn scattered broadcasts into a coherent *programming schedule* — stations with
identities, time slots, and a reason to tune in daily.

**Files:** the 4 radio JSONs + `faction_radio_corpus.json` (read/cross-ref; add a schedule
index catalog if the tuner supports it), read-only `RadioTuner.cs`, `FactionRadioEngine.cs`.

**Substeps:**
1. Read `RadioTuner` + `FactionRadioEngine` to learn how broadcasts are found (frequency, signal strength, day-gating).
2. Inventory all 118 broadcasts; tag each by station/source, content type (news, music, distress, propaganda, number-station), and day-window.
3. Define 5–6 stations with identities: a state remnant service, a faction propaganda voice, a religious broadcaster, a pirate/music station, a number station (11B), an automated emergency loop.
4. Assign each broadcast to a station + a frequency band + a rough schedule (so tuning at a given freq/day finds coherent content).
5. Fill schedule gaps: author 12 new broadcasts so each station has daily-ish content across the campaign.
6. Author 6 "appointment" broadcasts (a daily serial, a weather service, a lost-and-found roll) that reward habitual tuning.
7. Wire faction-war broadcasts (06C's 33) into the propaganda/news stations' schedules.
8. Validate ids/frequencies; data-integrity selftest; no frequency collisions.
9. xUnit: tuner resolves the right broadcast for freq+day; schedule coherence; no orphan broadcasts.
10. Radio selftest + a manual "tune around the dial" trace to confirm discovery feels alive.

**Next steps:** station loyalty (a faction notices you never tune their voice); a jamming event
(19B EMP); broadcast recording to cassettes (06B) for replay/trade.

---

## Task 24B — Distress calls, SOS & rescue missions

**Goal:** Expand `radio_distress_signals.json` (5) into a rescue-mission engine: distress calls
the player can investigate, with real people (or grim answers) at the end.

**Files:** `radio_distress_signals.json` (extend), expedition/location data, `characters.json`
(rescuees), read-only `RadioTuner`, `ExpeditionSystem`, `SurvivorCatalog` (recruits).

**Substeps:**
1. Read the 5 existing distress signals + how a distress call currently resolves (does it spawn a mission?).
2. Design the rescue loop: intercept distress → triangulate (tuner) → expedition to the source → outcome (survivor, trap, too-late, supplies).
3. Author 8 genuine-rescue calls (a trapped family, an injured scavenger, a besieged waystation) that yield a recruit or ally.
4. Author 6 grim-outcome calls (too late, a recording loop, a burial) — restrained, human.
5. Author 4 trap calls (raiders faking distress — ties to 10A enemies) that ambush the expedition.
6. Author 3 mystery calls (a pre-war automated beacon, a coded SOS — ties to 11B ciphers).
7. Wire rescue outcomes: survivor → `SurvivorCatalog` recruit (with a backstory); supplies → loot; trap → combat.
8. Validate ids; data-integrity selftest; dialog-graph lint (no orphan flags).
9. xUnit: distress → mission spawn → each outcome branch; recruit creation; determinism.
10. Save round-trip for an in-progress rescue mission.

**Next steps:** a reputation effect (rescuees spread word → standing, ties to 16C); a recurring
"do we risk it" triage when calls outnumber expedition capacity; VO for the most affecting calls (07B).

---

## Task 24C — Number stations & signal-intelligence programming

**Goal:** Give the signal-intelligence / number-station layer (11B ciphers) ongoing *content*:
a rotating cast of eerie broadcasts that are both atmosphere and puzzle feedstock.

**Files:** number-station broadcasts (radio data), `SignalIntelligenceCatalog` data,
`cassette_sets.json` (recordings), read-only `SignalIntelligenceCatalog.cs`, `RadioTuner.cs`.

**Substeps:**
1. Read `SignalIntelligenceCatalog` (cipher dictionaries, signal logs, wiretap transcripts) + how 11B's decode loop consumes broadcasts.
2. Author 8 number-station broadcasts (a voice reading groups, a musical-interval station, a buzzer-with-message) — grounded, unsettling, no real-world station references.
3. Author 4 wiretap transcripts as findable intel (they reference real factions/locations — continuity-checked).
4. Make 4 broadcasts *carriers* for 11B cipher quests (the numbers decode to coordinates).
5. Make 2 broadcasts pure dread atmosphere (a station that reads names — the dead? a list that includes a survivor's kin) for emotional weight.
6. Author a signal log the player builds (auto-record intercepted stations) surfaced in the radio panel.
7. Wire cassette recording (06B) so a broadcast can be captured and re-analyzed/traded.
8. Validate ids; data-integrity selftest; narrative-continuity.
9. xUnit: number-station emission, cipher-dictionary match, coordinate resolution, log accrual.
10. Confirm interplay with 11B (this task is its content feedstock — coordinate, don't duplicate).

**Next steps:** the oscilloscope mini-game (white space #16) becomes the *tactile* decode
front-end; a "source triangulation" meta-puzzle across multiple stations; a number station that
goes silent the day the war arc resolves (06C) — noticed only by habitual listeners.
