# Plan 93 — Verdict NPCs Expansion: 6 → 15 Investigation-Site NPCs

> **Flagship implementation plan**
>
> **Theme:** Give every Verdict investigation site a human residue. The expanded Verdict layer should
> not be a sequence of empty facilities and evidence objects; each site should contain at least one
> named person, recorded presence, surviving specialist, witness trace, or semi-present archival voice
> that makes the investigation feel inhabited.
>
> **Core constraint:** This is a **pure data-authority expansion of the existing Verdict NPC system**.
> `VerdictNpcSystem.cs`, `VerdictSave.cs`, the canonical Verdict location catalog, investigation-phase
> state, flag authority, dialogue delivery, Muster witness network, radio corpus, recurring-NPC
> identity, and save state remain authoritative. Do not create a second NPC system, duplicate witness
> identities, invent unsupported `kind` values, or hardcode site-specific logic in Core.

---

# 0. Executive Summary

Plan 93 expands `verdict_npcs.json` from 6 verified NPC definitions to 15.

The expanded Verdict location set from Plan 82 targets 15 investigation sites. With only six
site-linked NPCs, many investigation sites would remain mechanically populated but emotionally empty.

The nine new NPCs fill that gap:

## Coastal Survey arc

1. tide-gauge keeper;
2. meteorological-station observer;
3. cliff-bunker signalman;
4. marine-lab researcher.

## Interior Caches arc

5. forestry surveyor;
6. geological core-sample technician;
7. river-gauge attendant;
8. agricultural-station botanist.

## Border Wire arc

9. border-relay operator.

The feature should preserve the strange, specific tone already established by the Verdict expansion:
these characters are not conventional quest-givers. Some may be living survivors, some archival
traces, some recorded echoes, some administrative ghosts, depending on the accepted `kind` model.

The implementation pipeline is:

```text
Verdict site
   ↓
location_id
   ↓
phase_min + gating_flag
   ↓
VerdictNpcSystem eligibility
   ↓
NPC becomes available
   ↓
dialogue/testimony/evidence context
   ↓
optional witness / radio / recurring-NPC hook
   ↓
save-safe investigation progression
```

The central implementation hazards are:

1. **`kind` validity** — use only values actually accepted by `VerdictNpcSystem`;
2. **location reachability** — every new NPC must map to a real Plan 82/current Verdict location;
3. **flag reachability** — every gating flag must be settable through real investigation progress;
4. **phase semantics** — phase minimum boundaries must match current investigation depth;
5. **identity reuse** — three NPCs reused as Muster witnesses must remain the same people, not cloned
   witness-only duplicates.

The plan is complete when:

- exactly 15 Verdict NPC entries exist;
- all IDs are unique;
- all kinds are valid;
- all location references resolve;
- all gating flags are valid and reachable;
- phase gating behaves correctly;
- every new Verdict site has a meaningful NPC presence;
- three NPCs reuse stable identity in the Muster witness network;
- save/load preserves availability and relevant interaction state;
- all integrity/tests/build gates remain green.

---

# 1. Priority, Scope and Risk

## Priority

**P2 — high-value narrative-density expansion.**

## Nominal risk

**LOW** because this should be data-only.

## Main risks

- unsupported `kind`;
- duplicate `npc_*` IDs with other character catalogs;
- Plan 82 location IDs differ from assumptions;
- gating flags never become true;
- NPC appears before intended investigation phase;
- multiple entries accidentally point to same site while another site remains empty;
- dialogue tone drifts from existing Verdict quality;
- witness integration duplicates identity;
- recurring-NPC integration creates a second canonical character record;
- save state fails to preserve seen/available state.

## Explicit non-goals

Plan 93 must not:

- create a new Verdict NPC runtime;
- create a generic NPC state machine;
- add new `kind` values unless the runtime already supports extensibility;
- create one bespoke C# class per NPC;
- add full temporal arcs here;
- duplicate Plan 52 recurring-NPC mechanics;
- duplicate Plan 84 witness mechanics;
- duplicate Plan 94 radio mechanics;
- create new Verdict locations;
- rewrite Plan 82;
- add quest chains for all nine NPCs;
- add combat AI;
- add follower behavior;
- add voice synthesis;
- add long exposition monologues;
- turn every site into a conventional conversation hub.

---

# 2. Evidence Classification

## Supplied as verified

- `verdict_npcs.json` contains 6 entries.
- Existing fields include:
  - `id`;
  - `name`;
  - `role`;
  - `kind`;
  - `gating_flag`;
  - `location_id`;
  - `phase_min`;
  - `dialogue`.
- `VerdictNpcSystem.cs` is live.
- `VerdictSave.cs` is live.
- Existing `kind` values include at least:
  - `tape_echo`;
  - `paper_ghost`.
- Plan 82/current Verdict locations target 15 sites.
- The nine new roles are intended to map one-to-one to the nine added sites.

## Must be confirmed before authoring

- exact JSON field casing;
- whether `id` namespace is global or Verdict-local;
- whether `npc_*` collisions with `characters.json` matter;
- whether `kind` is enum/string;
- complete accepted `kind` set;
- whether kind affects rendering, persistence, interaction, or dialogue only;
- exact gating-flag semantics;
- whether empty gating flag is allowed;
- whether flag must be set or absent;
- whether phase numbering is 0/1/2 or 1/2/3;
- whether `phase_min` is inclusive;
- whether location matching is exact ID;
- whether multiple NPCs can share a location;
- whether one NPC can appear at multiple locations;
- whether dialogue lines are raw strings or structured records;
- whether dialogue order is stable;
- whether NPC interaction state is persisted;
- whether an NPC can be "seen" only once;
- whether witness reuse expects `npc_id`;
- whether recurring NPCs in Plan 52 use the same global identity namespace;
- whether Plan 94 radio can reference Verdict NPC IDs directly.

**Repository truth overrides all proposed IDs, names, kinds, and flags below.**

---

# 3. Design Thesis

Verdict NPCs should act as **human evidence**.

They are valuable because they can answer questions that objects cannot:

- who maintained the system?
- what did they notice before it failed?
- what instructions came from elsewhere?
- what did people stop recording?
- who knew the measurements were wrong?
- what remained routine after everything stopped being normal?

They should not solve the mystery outright.

A good Verdict NPC provides:
- testimony;
- contradiction;
- procedural detail;
- memory;
- uncertainty.

---

# 4. Architectural Invariants

## 4.1 VerdictNpcSystem authority

`VerdictNpcSystem.cs` remains authoritative for:
- loading;
- eligibility;
- phase gating;
- flag gating;
- location matching;
- interaction state.

## 4.2 Verdict location authority

Plan 82/current `verdict_locations.json` remains authoritative for site IDs.

## 4.3 Flag authority

Existing campaign/Verdict flag registry remains authoritative.

No private flag namespace invented in prose.

## 4.4 Investigation-phase authority

The existing Verdict phase/depth system owns progression.

## 4.5 Identity invariant

Each NPC has one stable canonical `npc_*` ID.

If reused in Plan 84 or Plan 52:
- reuse the same identity;
- do not clone into `witness_*` or a second named character unless architecture explicitly separates
  identity and presentation records.

## 4.6 Save invariant

Availability and interaction state must remain save-safe through `VerdictSave` / current owner.

## 4.7 Dialogue authority

Dialogue content remains data-driven.

No NPC-specific Core branches.

## 4.8 Determinism

Same investigation state must yield the same eligible NPC set.

---

# 5. Mandatory Phase 0 — Repository Reconnaissance

No nine-NPC authoring begins until this pass is complete.

## 5.1 Read `VerdictNpcSystem.cs` end-to-end

Document:

1. DTO shape;
2. loader;
3. ID semantics;
4. kind parsing;
5. accepted kind values;
6. gating flag evaluation;
7. phase comparison;
8. location filtering;
9. ordering;
10. interaction/seen state;
11. save integration;
12. missing-reference behavior;
13. invalid-kind behavior;
14. dialogue retrieval;
15. whether NPCs can be reused by other systems.

## 5.2 Read `VerdictSave.cs`

Confirm:

- what NPC state is persisted;
- whether only flags are persisted;
- whether seen/interacted IDs are saved;
- whether NPC availability is derived;
- whether location/phase state round-trips;
- whether dialogue progress is tracked.

## 5.3 Read existing 6 NPC entries

Create a parity table:

| ID | Name | Role | Kind | Flag | Location | Phase | Dialogue count |
| --- | --- | --- | --- | --- | --- | ---: | ---: |

Also analyze:

- name style;
- role sentence length;
- dialogue length;
- punctuation;
- uncanny tone;
- whether characters are living, archival, or ambiguous.

## 5.4 Enumerate accepted `kind` values

Search:

```bash
grep -RniE \
  "tape_echo|paper_ghost|VerdictNpcKind|kind.*Verdict|verdict_npcs" \
  Assets/Ashfall.Core src Ashfall.Core.Tests Assets/StreamingAssets
```

Classify:

### Kind Case A — closed enum

Use existing values only.

### Kind Case B — data-driven string categories

Use only categories already consumed by generic presentation logic.

### Kind Case C — kind is purely descriptive

Still preserve existing conventions.

### Kind Case D — new kind requires runtime work

Do not add new kind in Plan 93.

Use accepted values and defer expansion.

## 5.5 Read Plan 82/current Verdict locations

Build canonical site list:

| Location ID | Site name | Arc | Existing NPC? | New NPC needed? |
| --- | --- | --- | --- | --- |

Ensure the final 15-site coverage is intentional.

## 5.6 Search global NPC IDs

Check:

- `verdict_npcs.json`;
- `characters.json`;
- `survivors.json`;
- witness data;
- other NPC catalogs.

Determine whether `npc_*` is a global ID namespace.

If global:
- all nine IDs must be globally unique.

## 5.7 Audit gating flags

Search existing Verdict flags.

Document:
- prefix convention;
- who sets each;
- phase relationship;
- once-only behavior;
- save ownership.

No gating flag may be invented without a real emitter.

## 5.8 Read Plan 84/current witness schema

Confirm whether witness linkage expects:
- `npc_id`;
- witness-local record with `source_npc_id`;
- name only;
- testimony key.

Preferred:
- stable ID reference.

## 5.9 Read Plan 94/current Verdict radio schema

Identify whether radio broadcasts can:
- reference NPC;
- quote NPC;
- unlock NPC;
- set NPC gating flag.

## 5.10 Baseline verification

Run:

```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

Also when present:

```bash
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --real-campaign-journey-selftest
python3 scripts/ci/run-gates.py --tier fast
```

---

# 6. Site Coverage — Central Content Requirement

The expanded location set should have at least one meaningful human/NPC presence per site.

Create a final coverage matrix:

| Verdict site | NPC ID | Existing/New | Kind | Phase |
| --- | --- | --- | --- | ---: |

If multiple existing NPCs occupy the same site:
- that is allowed;
- but ensure no new Plan 82 site remains completely unserved unless deliberately designed as an
  empty/nonhuman site.

The user's "one per site" target is a content-coverage goal, not necessarily a strict one-to-one
runtime invariant.

---

# 7. NPC Kind Strategy

The nine new NPCs should not force unsupported archetypes.

If only `tape_echo` and `paper_ghost` exist:

## Tape echo

Use for:
- recorded voice;
- radio log;
- answering-machine trace;
- preserved audio;
- looped broadcast.

## Paper ghost

Use for:
- written notes;
- annotations;
- ledgers;
- logbooks;
- marginalia;
- procedural records.

If additional accepted kinds exist, distribute intentionally.

Do not add:
- `living_survivor`;
- `hologram`;
- `memory_echo`;
- `specter`;
unless they are real supported kinds.

---

# 8. Phase-Gating Strategy

Assuming phases 1–3:

## Phase 1

Surface context:
- role;
- routine;
- initial anomaly.

## Phase 2

Contradiction:
- hidden observation;
- procedural irregularity;
- changed orders.

## Phase 3

Deep testimony:
- connection to Muster/Verdict;
- hidden chain of responsibility;
- cross-site clue.

Not every new NPC must be Phase 3.

Target distribution:
- 3–4 at Phase 1;
- 3–4 at Phase 2;
- 2–3 at Phase 3.

Use actual phase semantics.

---

# 9. Gating-Flag Strategy

Every new NPC should have a real reason to become available.

Good gates:
- site power restored;
- archive opened;
- radio frequency decoded;
- evidence recovered;
- prior site investigated;
- phase advanced;
- witness network unlocked.

Bad gates:
- arbitrary `flag_show_npc_7`.

Use current flag grammar.

---

# 10. Dialogue Quality Standard

Each NPC gets 2–4 lines.

Each line should be:

- terse;
- specific;
- site-aware;
- slightly uncanny;
- procedural or personal;
- incomplete.

Avoid:
- lore dumps;
- exposition summaries;
- quest instructions;
- repeated "I don't know";
- cryptic nonsense with no informational value.

A good line should sound like it came from someone who had a job before they became evidence.

---

# 11. Voice Principle — Professional Residue

Each NPC voice should carry their occupation.

## Tide-gauge keeper

Uses:
- marks;
- levels;
- tide tables;
- drift.

## Meteorological observer

Uses:
- pressure;
- visibility;
- instrument readings;
- fronts.

## Signalman

Uses:
- checks;
- relay language;
- timing;
- channels.

## Marine researcher

Uses:
- samples;
- tanks;
- salinity;
- specimens.

## Forestry surveyor

Uses:
- plots;
- stands;
- canopy;
- firebreaks.

## Core technician

Uses:
- depths;
- strata;
- labels;
- sample integrity.

## River-gauge attendant

Uses:
- flood marks;
- stage;
- flow;
- upstream/downstream.

## Botanist

Uses:
- germination;
- soil;
- trays;
- blight;
- seed lots.

## Border-relay operator

Uses:
- repeater;
- line checks;
- posts;
- handoff.

---

# 12. Naming Standard

Names should match existing Verdict naming tone.

Avoid:
- highly stylized fantasy names;
- obvious symbolic names;
- repeated surnames;
- collisions with Plan 52 or existing characters.

Before committing:
- search all named NPC catalogs.

The names below are working names only.

---

# 13. New NPC 01 — Tide-Gauge Keeper

## Working ID

`npc_verdict_tide_keeper`

Use actual ID convention.

## Working name

**Mara Elsen**

Rename if collision exists.

## Role

Tide-gauge keeper assigned to the coastal survey station after the civilian network stopped
reporting.

## Arc

Coastal Survey.

## Kind

Prefer accepted archival kind:
- `paper_ghost` if the character is primarily logbook-based;
- `tape_echo` if voice recordings exist.

## Location

Real Plan 82 tide-gauge site.

## Phase

Likely 1.

## Gate

Real site-discovery/evidence flag.

## Dialogue draft

1. “The old high-water mark was wrong by six centimetres. I checked it three times.”
2. “No storm came through that night. The harbour still climbed.”
3. “They told me to stop writing the corrections in red.”

## Information value

Introduces:
- anomalous water behavior;
- manipulated records;
- first cross-site discrepancy.

---

# 14. New NPC 02 — Meteorological Station Observer

## Working ID

`npc_verdict_weather_observer`

## Working name

**Ilya Venn**

## Role

Weather-station observer who kept manual readings after the automated feed began contradicting the
instruments outside.

## Arc

Coastal Survey.

## Kind

`tape_echo` is a strong fit if accepted.

## Phase

1 or 2.

## Dialogue draft

1. “The pressure trace dropped after the wind had already turned. Instruments are not supposed to
   remember weather late.”
2. “Central sent a correction packet. I kept the paper chart.”
3. “After that, the forecast was always cleaner than the sky.”

## Information value

Links:
- weather data manipulation;
- central correction;
- paper-vs-network evidence.

---

# 15. New NPC 03 — Cliff-Bunker Signalman

## Working ID

`npc_verdict_cliff_signalman`

## Working name

**Ferris Daal**

## Role

Signalman assigned to the cliff bunker relay, responsible for routing coastal military and civil
traffic through a failing repeater chain.

## Arc

Coastal Survey.

## Kind

`tape_echo`.

## Phase

2.

## Dialogue draft

1. “Channel four was civil traffic until someone changed the routing table.”
2. “After midnight, every third packet came back with a different origin.”
3. “I stopped acknowledging them. The system acknowledged for me.”

## Information value

Links:
- relay anomaly;
- automated response;
- Verdict radio / Plan 94.

---

# 16. New NPC 04 — Marine-Lab Researcher

## Working ID

`npc_verdict_marine_researcher`

## Working name

**Dr. Sena Korr**

## Role

Marine-laboratory researcher tracking contamination in coastal organisms after sample results no
longer matched the intake logs.

## Arc

Coastal Survey.

## Kind

Use accepted kind based on evidence source:
- paper ghost for lab notes;
- tape echo for recorded observations.

## Phase

2–3.

## Dialogue draft

1. “The mussels changed before the water report did.”
2. “Sample twelve was discarded twice. I still have both labels.”
3. “Someone wanted the contamination to begin on a date.”

## Information value

Deepens:
- falsified chronology;
- biological evidence;
- cross-check with tide/weather.

---

# 17. New NPC 05 — Forestry Surveyor

## Working ID

`npc_verdict_forestry_surveyor`

## Working name

**Eden Rask**

## Role

Forestry surveyor mapping dead zones and windthrow after the official burn maps stopped matching
what remained on the ground.

## Arc

Interior Caches.

## Kind

`paper_ghost` likely.

## Phase

1.

## Dialogue draft

1. “The dead stand starts seventy metres before the fire line.”
2. “I marked it as wind damage. They sent the map back without the mark.”
3. “Trees do not know where the administrative boundary is.”

## Information value

Shows:
- altered maps;
- physical evidence contradicting records.

---

# 18. New NPC 06 — Geological Core-Sample Technician

## Working ID

`npc_verdict_core_technician`

## Working name

**Oren Vale**

## Role

Core-sample technician who catalogued subsurface layers beneath an interior monitoring cache.

## Arc

Interior Caches.

## Kind

`paper_ghost`.

## Phase

2.

## Dialogue draft

1. “Core seven has two labels and one depth.”
2. “The ash layer is below material they dated earlier.”
3. “I was told to file the duplicate under equipment error.”

## Information value

Provides:
- chronology contradiction;
- tampered sample metadata.

---

# 19. New NPC 07 — River-Gauge Attendant

## Working ID

`npc_verdict_river_gauge_attendant`

## Working name

**Lena Voss**

## Role

River-gauge attendant responsible for manual flood-stage readings after remote telemetry failed.

## Arc

Interior Caches.

## Kind

`tape_echo` or paper ghost based on current evidence presentation.

## Phase

1–2.

## Dialogue draft

1. “The river rose without rain upstream.”
2. “Telemetry called it sensor drift. The concrete stairs were wet.”
3. “When the reading came back down, the mud line stayed.”

## Information value

Echoes coastal anomaly inland.

---

# 20. New NPC 08 — Agricultural-Station Botanist

## Working ID

`npc_verdict_agricultural_botanist`

## Working name

**Tessa Mirn**

## Role

Agricultural-station botanist who tracked germination failures across supposedly uncontaminated seed
lots.

## Arc

Interior Caches.

## Kind

`paper_ghost`.

## Phase

2–3.

## Dialogue draft

1. “The control trays failed first.”
2. “That should have ended the trial. Instead they changed which trays were called control.”
3. “The seeds were honest. The labels were not.”

## Information value

Reinforces:
- manipulated baseline data;
- food/agriculture stakes.

---

# 21. New NPC 09 — Border Relay Operator

## Working ID

`npc_verdict_border_relay_operator`

## Working name

**Karel Norn**

## Role

Border-relay operator who handled the last handoffs between civil warning traffic and restricted
military channels.

## Arc

Border Wire.

## Kind

`tape_echo`.

## Phase

3.

## Dialogue draft

1. “The warning crossed the border before the order authorizing it.”
2. “I logged the time twice because I thought the clock had slipped.”
3. “Then headquarters asked me which copy of the log I intended to keep.”

## Information value

Late Verdict clue:
- command chronology;
- preauthorization anomaly;
- direct bridge to Verdict/Muster testimony.

---

# 22. Final New-NPC Roster

| # | Role | Arc | Suggested phase | Suggested evidence kind |
| ---: | --- | --- | ---: | --- |
| 1 | tide-gauge keeper | Coastal | 1 | paper/tape |
| 2 | weather observer | Coastal | 1–2 | tape |
| 3 | cliff signalman | Coastal | 2 | tape |
| 4 | marine researcher | Coastal | 2–3 | paper/tape |
| 5 | forestry surveyor | Interior | 1 | paper |
| 6 | core technician | Interior | 2 | paper |
| 7 | river-gauge attendant | Interior | 1–2 | tape/paper |
| 8 | agricultural botanist | Interior | 2–3 | paper |
| 9 | border relay operator | Border | 3 | tape |

Final kind values come from the runtime.

---

# 23. Gating-Flag Reachability Matrix

Before committing JSON, create:

| NPC | Gating flag | Set by | Earliest phase | Reachable? |
| --- | --- | --- | ---: | --- |
| Tide keeper | real flag | site/evidence | 1 | yes |
| Weather observer | real flag | prior clue | 1/2 | yes |
| ... | ... | ... | ... | ... |

## Rule

No new NPC ships with a gating flag that:
- is never set;
- is misspelled;
- belongs to unrelated content;
- requires a later phase than the NPC's phase minimum in an impossible way.

---

# 24. Phase/Flag Interaction

Eligibility likely resembles:

```text
current_phase >= phase_min
AND gating_flag == true
AND current_location == location_id
```

Confirm exact system.

Tests should cover all combinations:

| Phase valid | Flag valid | Location valid | Expected |
| --- | --- | --- | --- |
| no | no | no | hidden |
| yes | no | yes | hidden |
| no | yes | yes | hidden |
| yes | yes | no | hidden |
| yes | yes | yes | available |

---

# 25. Location Coverage Matrix

After Plan 82:

| Site | Arc | NPC |
| --- | --- | --- |
| Existing site 1 | existing | existing NPC |
| Existing site 2 | existing | existing NPC |
| ... | ... | ... |
| Tide gauge | Coastal | new |
| Met station | Coastal | new |
| Cliff bunker | Coastal | new |
| Marine lab | Coastal | new |
| Forestry cache | Interior | new |
| Core-sample site | Interior | new |
| River gauge | Interior | new |
| Agricultural station | Interior | new |
| Border relay | Border | new |

Do not assume 6 existing NPCs map one-to-one to 6 existing sites. Audit actual coverage.

---

# 26. Global NPC ID Collision Audit

Because `npc_*` is used elsewhere, compare new IDs against:

- `characters.json`;
- `verdict_npcs.json`;
- `survivors.json` if IDs overlap;
- Plan 84 witness IDs;
- Plan 52 recurring NPC IDs;
- quest NPC references.

If global ID uniqueness is required:
- enforce it.

If Verdict IDs are scoped:
- still choose distinct semantic IDs.

---

# 27. Name Collision Audit

Search names, not just IDs.

Avoid:
- two Ferris Voss-like variants;
- same first/surname as major existing characters unless intentional relation;
- names visually similar in UI.

---

# 28. Role String Standard

Role is one sentence/phrase describing:

```text
occupation + site context + anomaly
```

Good:

> “Weather-station observer who kept manual readings after the automated feed began contradicting the
> instruments outside.”

Avoid:

> “Mysterious scientist with secrets.”

---

# 29. Dialogue Structure

If dialogue is a raw array:

- 2–4 strings;
- preserve stable order;
- no embedded stage directions unless existing data uses them.

If dialogue is structured:
- use exact schema.

## Dialogue progression

Prefer each line to do a different job:

1. concrete observation;
2. contradiction;
3. personal/procedural response;
4. optional cross-site clue.

---

# 30. Uncanny Tone Without Supernatural Claims

"Slightly uncanny" should emerge from:
- repeated bureaucracy;
- timestamps;
- mismatched records;
- systems replying automatically;
- people preserving wrong instructions;
- measurements disagreeing with official reports.

Avoid:
- ghosts literally speaking unless Verdict fiction establishes supernatural content;
- cryptic dream logic;
- impossible omniscience.

---

# 31. Coastal Survey Arc Cohesion

The four coastal NPCs should form a loose evidence chain.

Potential sequence:

```text
tide keeper
→ water anomaly

weather observer
→ weather data correction

signalman
→ communications routing anomaly

marine researcher
→ contamination chronology manipulation
```

No single NPC solves the arc.

Together they create a pattern.

---

# 32. Interior Caches Arc Cohesion

Potential sequence:

```text
forestry surveyor
→ maps altered

core technician
→ chronology altered

river attendant
→ telemetry contradicted physical evidence

botanist
→ control samples relabeled
```

Theme:

> The records changed more often than the world did.

---

# 33. Border Wire Arc Cohesion

The border-relay operator should provide a high-depth clue connecting:
- warning traffic;
- military/civil channel handoff;
- timing;
- authorization.

This character is a good candidate for:
- Muster witness;
- Plan 94 radio reference.

---

# 34. Plan 84 Muster Witness Integration

Exactly 3 Verdict NPCs should be reusable as Muster witnesses.

Recommended candidates:

1. cliff-bunker signalman;
2. marine-lab researcher;
3. border-relay operator.

Why:
- they possess testimony with institutional significance;
- they represent communications, science, and command chronology.

Alternative candidates may be chosen after Plan 84 schema audit.

## Identity rule

Prefer:

```text
witness entry
→ source_npc_id = npc_verdict_...
```

or current equivalent.

Do not create:
- `witness_ferris_daal`;
- a second duplicate NPC biography.

---

# 35. Witness Testimony Scope

Verdict dialogue and Muster testimony should not be identical.

Verdict:
- site-specific observation.

Muster:
- condensed testimony relevant to coalition decision/endgame.

Same identity, different content surface.

---

# 36. Plan 94 Verdict Radio Integration

Candidates:

- weather observer;
- cliff signalman;
- border relay operator.

Potential links:

```text
radio broadcast
→ mentions/records NPC
→ sets gating flag
→ NPC evidence becomes available
```

or:

```text
NPC dialogue
→ reveals frequency
→ Plan 94 broadcast unlocks
```

Use actual architecture.

No duplicate radio system.

---

# 37. Plan 52 Recurring NPC Integration

Some Verdict NPCs may recur later.

This must be evidence-gated.

If a Verdict NPC is a `tape_echo` representing a dead/archive-only figure:
- do not make them a living recurring NPC later.

If kind represents a living site NPC:
- stable ID can be referenced by Plan 52.

Do not contradict kind semantics.

---

# 38. Kind/Recurrence Compatibility

Create:

| NPC | Kind | Implies living? | Eligible for Plan 52 recurrence? |
| --- | --- | --- | --- |
| ... | tape_echo | maybe no | likely no |
| ... | paper_ghost | archival | no |
| ... | living kind if supported | yes | yes |

Do not assume.

---

# 39. Save Contract

Confirm whether availability is derived from:
- phase;
- flags;
- location.

If so:
- no additional state needed.

If interactions are persisted:
- new NPC IDs must be accepted.

Test:

```text
NPC becomes eligible
→ interact
→ save
→ reload
→ seen/interacted state preserved
```

If dialogue can repeat:
- preserve current semantics.

---

# 40. Location-Transition Behavior

NPC should not remain available after player leaves its site unless current system deliberately
supports remote testimony.

Test:
- valid site → available;
- other site → hidden.

---

# 41. Phase Boundary Behavior

If `phase_min` is inclusive:

```text
phase 1 NPC:
phase 0 hidden
phase 1 visible
phase 2 visible
phase 3 visible
```

If current system differs:
- pin exact behavior.

---

# 42. Gating-Flag Save Behavior

Sequence:

1. flag false;
2. NPC hidden;
3. set flag;
4. NPC visible;
5. save;
6. reload;
7. NPC remains visible.

No duplicate flag storage.

---

# 43. Dialogue Persistence

If dialogue progression is saved:

- line index/state must survive load.

If dialogue always replays:
- do not add new state.

Plan 93 follows existing behavior.

---

# 44. Data Integrity Requirements

Validate:

- exactly 15 NPC entries;
- original 6 retained;
- 9 new IDs unique;
- global ID uniqueness if required;
- names non-empty;
- roles non-empty;
- kinds valid;
- gating flags non-empty where required;
- gating flags resolve or follow registry rules;
- location IDs resolve;
- phase values valid;
- dialogue arrays non-empty;
- dialogue count within expected range;
- witness refs resolve for selected 3;
- no orphan site coverage.

---

# 45. Content Reachability Requirement

Every new NPC needs a full reachability witness:

```text
site reachable
+ phase reachable
+ gating flag settable
= NPC can appear
```

If one component fails:
- the NPC is dead content.

---

# 46. Vertical Slice Strategy

## Slice 1 — Tide-Gauge Keeper

Why:
- early phase;
- simple site link;
- simple gating;
- no cross-system dependency.

Prove:
`location + flag + phase → visible → dialogue → save`.

## Slice 2 — Cliff Signalman

Prove:
- Plan 94 radio hook;
- Phase 2;
- tape-echo kind.

## Slice 3 — Border Relay Operator

Prove:
- Phase 3;
- deep gating;
- Muster witness reuse.

## Slice 4 — Marine Researcher

Prove:
- second witness;
- multi-system clue;
- kind semantics.

Only then bulk-author remaining entries.

---

# 47. Implementation Waves

## Wave 0 — Forensics

Deliver:
- schema;
- kind enum;
- existing 6 parity;
- location coverage;
- flag registry;
- phase semantics;
- save behavior;
- baseline.

## Wave 1 — Early-site slice

Tide-Gauge Keeper.

## Wave 2 — Radio-linked slice

Cliff Signalman.

## Wave 3 — Witness-linked slice

Border Relay Operator.

## Wave 4 — Scientific witness slice

Marine Researcher.

## Wave 5 — Complete all 9 new NPCs

## Wave 6 — Plan 84 witness links

Wire 3.

## Wave 7 — Plan 94 radio links

Wire appropriate references.

## Wave 8 — Reachability audit

## Wave 9 — Dialogue editorial QA

## Wave 10 — Full regression

---

# 48. Detailed Test Catalogue

The following are required scenarios, not necessarily separate test methods.

## Catalog

1. Verdict NPC catalog parses.
2. total count is 15.
3. original 6 IDs remain.
4. 9 new IDs unique.
5. global ID collisions absent if required.
6. all names non-empty.
7. all roles non-empty.
8. all kinds valid.
9. all gating flags valid.
10. all location IDs resolve.
11. all phases valid.
12. all dialogue arrays non-empty.

## Kind

13. tape_echo accepted.
14. paper_ghost accepted.
15. any additional used kind accepted.
16. unsupported kind fails validation or loader according to current contract.
17. no new runtime kind required.

## Location

18. Tide NPC appears only at tide site.
19. Weather observer appears only at met site.
20. Signalman appears only at cliff bunker.
21. Marine researcher appears only at marine lab.
22. Forestry surveyor appears only at forestry site.
23. Core technician appears only at core site.
24. River attendant appears only at river site.
25. Botanist appears only at agricultural site.
26. Border operator appears only at border relay.
27. wrong location hides NPC.

## Phase

28. Phase 1 NPC visible at min phase.
29. Phase 2 NPC hidden at Phase 1.
30. Phase 2 NPC visible at Phase 2.
31. Phase 3 NPC hidden before Phase 3.
32. Phase 3 NPC visible at Phase 3.
33. higher phases retain eligibility if current semantics say so.

## Flag

34. false flag hides NPC.
35. true flag reveals NPC when phase/location valid.
36. missing flag behavior safe.
37. flag survives save/load.
38. flag spelling/reference validated.

## Combined eligibility

39. valid phase + invalid flag = hidden.
40. valid flag + invalid phase = hidden.
41. valid phase/flag + invalid location = hidden.
42. all three valid = visible.

## Dialogue

43. each new NPC returns 2–4 lines.
44. dialogue order stable.
45. empty line rejected if validator supports it.
46. dialogue content does not contain placeholder markers.
47. interaction state follows current repeatability semantics.

## Save

48. eligible NPC remains eligible after load.
49. seen/interacted state round-trips if stored.
50. phase state round-trips.
51. location state round-trips.
52. no duplicate NPC interaction created after reload.

## Witness integration

53. witness 1 references stable Verdict NPC ID.
54. witness 2 references stable Verdict NPC ID.
55. witness 3 references stable Verdict NPC ID.
56. witness data does not duplicate canonical NPC identity.
57. unavailable/archival kind is not incorrectly treated as living witness if semantics forbid it.

## Radio integration

58. radio-linked NPC reference resolves.
59. radio-triggered gating flag reveals correct NPC if supported.
60. NPC-provided radio clue resolves to valid broadcast if used.
61. radio save state does not duplicate NPC state.

## Site coverage

62. all Plan 82/current sites have intended NPC coverage.
63. no new site accidentally lacks human presence.
64. duplicate-site NPCs are intentional.
65. no NPC references obsolete location.

## Determinism

66. same phase/flag/location yields same eligible NPC set.
67. catalog ordering stable.
68. no RNG changes eligibility.

## Full loop

69. visit Coastal site → reveal NPC → dialogue → save/load.
70. progress to Phase 2 → reveal Signalman.
71. radio clue → NPC gate → testimony.
72. Phase 3 Border Wire → operator → Muster witness availability.
73. same campaign state reproduces same NPC availability.
74. content-utilization detects all 9 new NPCs as reachable.

---

# 49. Headless End-to-End Scenarios

## Scenario A — Basic site NPC

1. Start Verdict fixture below required site.
2. Set phase valid.
3. Set gating flag false.
4. Enter tide-gauge site.
5. Assert NPC hidden.
6. Set real gating flag.
7. Assert NPC visible.
8. Read dialogue.
9. Save.
10. Reload.
11. Assert same availability/interacted state.

## Scenario B — Phase gate

1. Enter cliff-bunker site at Phase 1.
2. Set Signalman flag.
3. Assert hidden if min phase 2.
4. Advance to Phase 2.
5. Assert visible.
6. Verify dialogue.

## Scenario C — Radio hook

1. Begin with radio-linked flag unset.
2. Intercept real Plan 94/current broadcast.
3. Set/unlock canonical flag through existing outcome.
4. Enter linked site.
5. Assert NPC availability.
6. Save/reload.

## Scenario D — Muster witness

1. Interact with Border Relay Operator.
2. Advance to Muster witness phase.
3. Assert witness network references same `npc_id`.
4. Verify testimony path.
5. Assert no duplicate identity record.

---

# 50. Dialogue Editorial QA

For each of the 9 new NPCs, review:

- occupation vocabulary;
- site specificity;
- mystery contribution;
- redundancy with other NPCs;
- line length;
- information density;
- voice distinction.

No two NPCs should repeat the same reveal with different nouns.

---

# 51. Cross-Site Mystery Progression

The nine new NPCs should deepen, not flatten, the investigation.

A useful hierarchy:

## Early clues

- measurements do not match records.

## Mid clues

- records were intentionally corrected/relabeled.

## Late clues

- communications/authorization chronology contradicts official sequence.

The exact mystery remains governed by existing Verdict canon.

---

# 52. Evidence Redundancy Guardrail

Multiple NPCs can corroborate a fact.

But each should add:
- different discipline;
- different medium;
- different consequence.

Example:

Tide keeper:
- water level.

Weather observer:
- pressure records.

River attendant:
- inland water stage.

Together:
- environmental anomaly across independent systems.

---

# 53. Role Diversity

The nine new NPCs span:

- hydrology;
- meteorology;
- signals;
- marine science;
- forestry;
- geology;
- river monitoring;
- botany;
- border communications.

This is strong.

Do not replace several with generic:
- technician;
- guard;
- scientist.

---

# 54. Gender/Name Distribution

Audit final 15-character roster for:
- name diversity;
- gender balance where inferable;
- repeated surname patterns;
- overuse of same cultural naming cues.

Match ASHFALL worldbuilding.

---

# 55. NPC Mortality / Living-State Ambiguity

The `kind` field may imply whether the NPC is:
- alive;
- recorded;
- archival;
- reconstructed.

Do not write role/dialogue that contradicts the actual presentation.

Example:
- a `paper_ghost` should not physically hand the player an item unless runtime supports a document
  representing that action.

---

# 56. Witness Reuse Guardrail

Before selecting the 3 witness NPCs:

Confirm the witness system can represent their ontology.

If `paper_ghost` means dead archival trace:
- it may not be a literal live witness.

In that case choose:
- living-compatible NPCs;
- or use "recorded testimony" only if Plan 84 accepts it.

---

# 57. Recurring-NPC Reuse Guardrail

Same issue for Plan 52.

Only living/continuing identities should recur.

Archival entities can recur as:
- references;
- documents;
- recordings;
but not as physically present people unless canon supports it.

---

# 58. Plan 94 Radio Cross-Reference Matrix

Suggested:

| NPC | Radio relation |
| --- | --- |
| Weather observer | forecast/correction recording |
| Cliff signalman | relay traffic |
| Border operator | warning handoff |
| Tide keeper | optional automated tide channel |
| Marine researcher | lab status broadcast if supported |

Use only real Plan 94 entries.

---

# 59. Plan 84 Witness Candidate Matrix

| NPC | Evidence value | Ontology risk | Witness fit |
| --- | --- | --- | --- |
| Cliff signalman | high | medium | strong |
| Marine researcher | high | medium | strong |
| Border operator | very high | medium | strong |
| Weather observer | high | medium | alternate |
| Core technician | high | low/medium | alternate |

Final choice depends on `kind`.

---

# 60. Data Integrity Extensions

Add/extend validation for:

- Verdict NPC ID uniqueness;
- valid kind values;
- valid location refs;
- valid phase bounds;
- nonempty flags;
- flag format;
- dialogue count;
- witness source-NPC refs;
- global NPC collisions if namespace is shared.

---

# 61. Content Utilization

Every new NPC should be reachable.

Use current content-utilization tooling where present.

A definition is not complete if:
- site is unreachable;
- flag never set;
- phase never reached.

---

# 62. Save Compatibility

No save migration should normally be required.

New NPC IDs should be additive.

Do not:
- rename original 6 IDs;
- change old phase values;
- change old flags;
unless separately justified.

---

# 63. Determinism Audit

NPC eligibility should be pure.

No RNG should decide whether an eligible Verdict NPC appears unless current system already has such
mechanics.

Same state → same NPC list.

---

# 64. Performance

15 NPC definitions are trivial.

Avoid:
- reparsing catalog per site query;
- repeated flag scans if current system already indexes.

No performance refactor needed.

---

# 65. Risk Register

## Risk 1 — Unsupported kind

Mitigation:
- mandatory enum audit.

## Risk 2 — Dead gating flag

Mitigation:
- flag reachability table.

## Risk 3 — Wrong Plan 82 location ID

Mitigation:
- location referential integrity.

## Risk 4 — Phase off-by-one

Mitigation:
- boundary tests.

## Risk 5 — Duplicate global NPC ID

Mitigation:
- global collision audit.

## Risk 6 — Witness duplicates identity

Mitigation:
- stable source NPC reference.

## Risk 7 — Archival NPC used as living witness

Mitigation:
- ontology/kind compatibility matrix.

## Risk 8 — Dialogue repeats mystery beats

Mitigation:
- cross-site evidence audit.

## Risk 9 — Existing 6 behavior changes

Mitigation:
- parity tests.

## Risk 10 — Radio hook creates duplicate gate state

Mitigation:
- existing flag authority.

## Risk 11 — One site still empty

Mitigation:
- final 15-site coverage matrix.

## Risk 12 — NPC exists but never reachable

Mitigation:
- utilization/reachability test.

---

# 66. Cross-Tool QA

Nominal risk is LOW.

Independent review becomes required if implementation changes:

- `VerdictNpcSystem`;
- accepted kind parsing;
- save schema;
- witness runtime;
- shared NPC identity architecture.

Reviewer checks:
- no duplicate NPC authority;
- kind semantics;
- identity reuse;
- flag/phase/location correctness;
- save safety;
- existing 6 parity.

---

# 67. Acceptance Matrix

| Requirement | Evidence |
| --- | --- |
| 15 total NPCs | catalog test |
| existing 6 preserved | parity |
| 9 new NPCs | ID diff |
| kinds valid | enum/loader test |
| location refs valid | integrity |
| flags valid/reachable | reachability |
| phase gates valid | boundary tests |
| dialogue non-empty | validation |
| site coverage | matrix |
| 3 witness links | Plan 84 refs |
| radio hooks valid | Plan 94 refs |
| save stable | round-trip |
| deterministic eligibility | repeat-state test |
| build/tests green | verification |

---

# 68. Definition of Done

## Catalog

- [ ] `verdict_npcs.json` contains exactly 15 NPCs.
- [ ] Original 6 IDs remain compatible.
- [ ] 9 new unique IDs exist.
- [ ] Names are collision-audited.
- [ ] Roles are distinct and site-specific.
- [ ] Every `kind` is accepted by the existing system.
- [ ] No new runtime kind is required.
- [ ] Every location ID resolves.
- [ ] Every gating flag is valid.
- [ ] Every `phase_min` is legal.
- [ ] Every dialogue array is non-empty.
- [ ] New dialogue contains 2–4 lines unless current schema allows/uses another count.

## Reachability

- [ ] every new site-linked NPC has a reachability witness;
- [ ] every gating flag can actually be set;
- [ ] every phase can actually be reached;
- [ ] every linked location exists;
- [ ] content-utilization finds all 9 new NPCs.

## Site coverage

- [ ] all 15 Verdict sites have intended NPC/human-presence coverage;
- [ ] any deliberate empty site is documented;
- [ ] duplicate site occupancy is intentional.

## Cross-system

- [ ] 3 NPCs reuse stable identity in Plan 84/current witness network;
- [ ] witness ontology matches NPC kind;
- [ ] Plan 94/current radio references resolve where used;
- [ ] Plan 52 reuse occurs only for living-compatible NPCs;
- [ ] no duplicate character identities are created.

## Persistence

- [ ] gating state survives save/load;
- [ ] phase state survives save/load;
- [ ] seen/interacted state survives if currently persisted;
- [ ] availability re-derives identically;
- [ ] original 6 save references remain valid.

## Determinism

- [ ] same location/phase/flags → same NPC availability;
- [ ] no new RNG;
- [ ] stable dialogue ordering;
- [ ] no unordered-set availability differences.

## Quality

- [ ] each NPC sounds occupationally distinct;
- [ ] dialogue is terse and specific;
- [ ] uncanny tone is procedural, not random;
- [ ] no exposition dumps;
- [ ] Coastal arc forms a coherent clue chain;
- [ ] Interior arc forms a coherent clue chain;
- [ ] Border Wire adds late-phase significance;
- [ ] no single NPC solves the entire Verdict mystery.

## Verification

- [ ] data-integrity selftest passes;
- [ ] all Core tests pass;
- [ ] main build passes;
- [ ] content-utilization gate passes if present;
- [ ] headless Verdict NPC scenarios pass;
- [ ] real-campaign/fast CI gates pass if present;
- [ ] no warnings/errors introduced.

---

# 69. Verification Commands

Run supported equivalents:

```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

Also when present:

```bash
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --real-campaign-journey-selftest
python3 scripts/ci/run-gates.py --tier fast
```

Do not invent a Verdict NPC selftest unless it already exists.

---

# 70. Required Implementation Report

The implementing agent must report:

## Repository facts

- exact Verdict NPC schema;
- accepted `kind` values;
- phase semantics;
- gating-flag semantics;
- location matching;
- save behavior;
- global/scoped NPC namespace.

## Existing 6 parity

List:
- ID;
- name;
- kind;
- flag;
- location;
- phase;
- dialogue count.

## Final 15 roster

For each:
- ID;
- name;
- role;
- kind;
- gating flag;
- location;
- phase;
- dialogue count;
- witness link?;
- radio link?;
- recurring-NPC eligible?.

## Coverage report

Provide:
- all Plan 82/current site IDs;
- NPC mapped to each;
- any deliberate empty site;
- reachability result.

## Witness report

List the 3 final Plan 84 witness-linked NPCs and how identity is reused.

## Radio report

List all Plan 94/current radio cross-references.

## Verification

Exact:
- test pass count;
- data integrity;
- build;
- headless Verdict scenarios;
- content utilization;
- fast gates.

## Deviations

For each:
1. original assumption;
2. repository evidence;
3. adaptation;
4. why architecture remains pure data.

---

# 71. Flagship Implementation Handoff Prompt

```text
Implement Plan 93 — Verdict NPCs Expansion as a repository-evidence-driven, pure-data expansion of
the existing VerdictNpcSystem.

PRIMARY OBJECTIVE
Expand verdict_npcs.json from 6 verified NPCs to exactly 15 by adding 9 site-linked investigation
characters for the Plan 82/current expanded Verdict location set. Each new NPC must provide terse,
specific human testimony or archival presence without duplicating the location, witness, radio, or
recurring-NPC systems.

ARCHITECTURAL RULES
- VerdictNpcSystem remains NPC eligibility/interaction authority.
- VerdictSave/current save owner remains persistence authority.
- verdict_locations/current Plan 82 data remains location authority.
- existing flag registry remains gating authority.
- existing Verdict investigation phase remains phase authority.
- Plan 84/current Muster witness system remains witness authority.
- Plan 94/current Verdict radio remains radio authority.
- Plan 52/current recurring NPC system remains temporal-character authority.
- Do not create a second NPC runtime.
- Do not hardcode per-NPC logic.
- Do not invent unsupported kind values.
- Do not duplicate named identities across systems.
- Do not rename the original 6 IDs without migration.

MANDATORY RECON
1. Read VerdictNpcSystem.cs completely.
2. Read VerdictSave.cs.
3. Read all 6 existing verdict_npcs entries and create a parity table.
4. Enumerate every accepted kind value from code/data/tests.
5. Read Plan 82/current verdict_locations and build the complete 15-site list.
6. Determine whether npc_* IDs are globally unique across character catalogs.
7. Search and inventory existing Verdict gating flags.
8. Confirm phase numbering and inclusive/exclusive behavior.
9. Read Plan 84/current witness schema.
10. Read Plan 94/current Verdict radio schema.
11. Run baseline integrity/tests/build and supported utilization/fast gates.

KIND RULE
If only tape_echo and paper_ghost are accepted:
- use only those.
If additional kinds exist:
- use only real supported values.
Do not add a new kind in this plan if it requires runtime code.

VERTICAL SLICES
Before bulk authoring:
1. Tide-Gauge Keeper:
   location + flag + phase -> NPC -> dialogue -> save/load.
2. Cliff-Bunker Signalman:
   Phase 2 + radio hook + tape-echo semantics.
3. Border Relay Operator:
   Phase 3 + Muster witness identity reuse.
4. Marine Researcher:
   science testimony + witness/ontology validation.

ONLY THEN AUTHOR THE REMAINING NEW NPCS.

ADD EXACTLY 9 NEW NPCS

COASTAL SURVEY (4)
1. tide-gauge keeper;
2. meteorological-station observer;
3. cliff-bunker signalman;
4. marine-lab researcher.

INTERIOR CACHES (4)
5. forestry surveyor;
6. geological core-sample technician;
7. river-gauge attendant;
8. agricultural-station botanist.

BORDER WIRE (1)
9. border-relay operator.

EVERY NPC NEEDS
- canonical unique npc_* ID;
- collision-audited full name;
- one-sentence role;
- real accepted kind;
- real reachable gating flag;
- real Plan 82/current location_id;
- valid phase_min;
- 2–4 terse dialogue lines using the exact current schema.

DIALOGUE STANDARD
Verdict NPCs are human evidence, not exposition delivery.
Each line should contain:
- occupational detail;
- observation;
- contradiction;
- procedural residue;
- or a cross-site clue.
Keep the tone slightly uncanny through records, timestamps, systems, labels, and bureaucracy—not
through random supernatural dialogue unless Verdict canon explicitly supports it.

COASTAL CLUE CHAIN
Use the four Coastal NPCs to progress:
water anomaly -> weather correction -> relay anomaly -> contamination chronology.
No single NPC should solve the arc.

INTERIOR CLUE CHAIN
Use the four Interior NPCs to progress:
altered maps -> relabeled strata -> physical river evidence -> relabeled biological controls.

BORDER CLUE
The border-relay operator should provide a deep Phase 3 clue around warning/authorization timing and
communications handoff.

REACHABILITY
For every new NPC prove:
location reachable + phase reachable + gating flag settable = NPC reachable.
No dead-content entries.

SITE COVERAGE
Build a final matrix mapping all 15 Verdict sites to at least one intended NPC/human presence.
Do not assume the original 6 map one-to-one to the original sites; audit actual coverage.

PLAN 84 WITNESSES
Reuse exactly 3 suitable Verdict NPC identities as Muster witnesses.
Prefer stable source_npc_id/reference semantics.
Do not clone them as separate people.
Verify kind/ontology compatibility: an archival paper ghost must not be treated as a physically living
witness unless Plan 84 supports recorded testimony.

PLAN 94 RADIO
Use appropriate stable NPC IDs for radio linkage where current schema supports it.
Strong candidates:
- weather observer;
- cliff signalman;
- border relay operator.
Radio may set a gating flag or be revealed by NPC dialogue, but radio remains its own authority.

PLAN 52 RECURRING NPCS
Only living-compatible Verdict NPCs may recur physically.
Archive-only/tape/paper entities may recur as references or recordings, not as contradictory living
characters.

SAVE / DETERMINISM
Prove:
- gating flags persist;
- phase persists;
- interacted state persists if current system tracks it;
- same phase + flags + location produces same NPC set;
- no new RNG affects NPC availability;
- original 6 save behavior remains valid.

TESTS
Cover:
- total count 15;
- original 6 parity;
- 9 new unique IDs;
- global ID uniqueness if required;
- accepted kind values;
- location refs;
- gating flags;
- phase boundaries;
- combined eligibility truth table;
- 2–4 dialogue lines;
- save/load;
- all 15-site coverage;
- 3 witness identity links;
- Plan 94 radio refs;
- deterministic eligibility;
- full Coastal/Interior/Border journeys.

VALIDATION
Run:
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj

Also run supported content-utilization, real-campaign, and fast CI gates.

DEFINITION OF DONE
verdict_npcs.json contains exactly 15 valid NPCs; original 6 remain compatible; 9 new site-linked
characters exist; all kinds, flags, locations, and phases resolve; all 15 Verdict sites have intended
human/NPC coverage; every new NPC is reachable; 3 reuse stable identity in the Muster witness
network; radio/recurring-NPC links use existing authorities; save/load preserves investigation state;
same state yields the same NPC set; no new NPC runtime/kind is introduced; and all relevant
integrity/tests/build/gates are green.

FINAL REPORT
Provide:
- actual schema;
- accepted kind enum/list;
- existing 6 parity;
- final 15 NPC roster;
- location coverage matrix;
- gating-flag reachability table;
- phase distribution;
- 3 witness links;
- Plan 94 radio links;
- recurrence compatibility;
- exact verification results;
- independent QA result if runtime/save/identity code changed;
- every evidence-driven deviation.
```

---

# 72. Final Product Standard

Plan 93 succeeds when the Verdict investigation sites stop feeling like abandoned data terminals and
start feeling like places where people worked, measured, logged, corrected, doubted, and eventually
left evidence behind.

The player should remember:

- the tide keeper who stopped trusting the red corrections;
- the weather observer who kept the paper chart;
- the signalman whose relay answered without him;
- the marine researcher who noticed the organisms before the official date;
- the surveyor whose map came back cleaner than the forest;
- the technician holding two labels for one core;
- the river attendant trusting mud over telemetry;
- the botanist whose control group was renamed;
- the relay operator asked which version of the log he intended to keep.

The strongest Verdict NPC is not the one who explains the mystery.

It is the one whose ordinary professional detail makes the official story harder to believe.
