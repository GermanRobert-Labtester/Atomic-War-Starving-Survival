# ASHFALL: 2D Atomic-War Survival

## Quality Implementation Plan — Batch 5, Steps 65–80

**Generated:** 2026-08-19<br>
**Source:** ASHFALL Quality Next Steps Roadmap, Batch 5<br>
**Status:** Implementation-ready after the Phase 0 audit<br>
**Host engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core target:** .NET Standard 2.1<br>
**Scope:** Expedition camps, instrumentation, shelter hazards, extraction, narrative delivery, economy, radio discovery, generational preservation, broadcasting, and presentation settings

---

## 1. Executive outcome

Batch 5 contains sixteen attractive feature briefs, but several of them currently point at narrative catalogs, host placeholders, or adjacent systems rather than complete gameplay authorities. This plan turns the batch into a safe implementation sequence that preserves ASHFALL’s architecture:

- Core simulation remains authoritative in Assets/Ashfall.Core/.
- Godot Nodes and panels handle input, presentation, audio, shader application, and wiring only.
- Assets/StreamingAssets/Data/ remains the single data authority.
- Stateful systems expose CaptureState and RestoreState and are included in the checksum-backed save envelope.
- Every stochastic result is driven by ISeededRng or deterministic arithmetic with an explicit seed.
- Existing systems are extended at their seams; parallel “mini engines” are not introduced.
- Resources, time, hazards, and faction consequences are real transactions rather than UI rewards.
- Narrative files are treated as authored content until a runtime system explicitly promotes their records into state.

The most important quality corrections are:

1. The campsite is a persisted expedition phase, not a second expedition simulator.
2. Dosimeter calibration improves measurement confidence and error bounds; it does not alter true radiation dose.
3. Salt extraction feeds the existing brine-water and Silent Foundry treaty authorities; it does not create an unrelated resource economy.
4. Letters, contraband, apiculture, cryogenic records, and industrial fire logs are content anchors, not active mechanics by themselves.
5. Weather sondes improve observations of the existing WeatherSystem forecast; they do not create a second weather oracle.
6. Safe cracking is a deterministic resolution system with an accessible tactile presentation, not a UI-only skill test.
7. Currency must have an explicit backing and acceptance policy before minting is implemented.
8. Radio triangulation must produce evidence and uncertainty before it reveals a canonical location.
9. CRT effects belong to user settings and the Godot presentation layer, never to Core simulation or campaign state.

Batch 4 Step 64, the save manager and campaign profile foundation, is a prerequisite gate. If that work is incomplete, Phase 0 must identify and close the minimum save-contract gaps before the new systems are added.

---

## 2. Dependency-first delivery order

The roadmap numbering is retained for traceability, but implementation order follows dependencies and risk.

### Phase 0 — Save, authority, and integration audit

Verify Batch 4 Step 64, the current save envelopes, the Godot host composition root, canonical IDs, and the dirty worktree. Do not begin sixteen feature branches until each proposed state owner has a save destination and each output item has a canonical ID.

Required outputs:

- Batch 4 save-slot, checksum, migration, and profile isolation decision.
- A single owner for each new runtime state.
- A list of existing systems to extend rather than replace.
- Canonical units for water, salt, brine, fuel, temperature, pressure, dose, currency, and time.
- A data-reference report for every existing narrative catalog used by Batch 5.
- A determinism risk list, including known order-sensitive code touched by this batch.

### Phase 1 — Simulation seams with high persistence risk

Implement and verify:

- 65 — Multi-Day Expedition Campsite.
- 66 — Dosimeter Calibration Station.
- 72 — Safe Cracking.
- 77 — Radio Direction Finding and Triangulation.

These features establish the patterns for phase transitions, instrument confidence, deterministic minigame resolution, and evidence-based discovery.

### Phase 2 — Infrastructure and production loops

Implement and verify:

- 68 — Salt Mine and Brine Extraction.
- 70 — Ventilation Fire and Smoke.
- 71 — Weather Balloon and Radiosonde.
- 73 — Greenhouse Apiculture.
- 78 — Cryogenic Germplasm Vault.

These features share power, water, fuel, temperature, worker assignment, contamination, maintenance, and save/load concerns.

### Phase 3 — Narrative, risk economy, and culture

Implement and verify:

- 69 — Dead-Letter Mailbox.
- 74 — Black Market.
- 75 — Sleepwalking and Dream Logs.
- 76 — Bunker Mint.
- 79 — Radio Studio.

Each must produce a journal, faction, morale, market, or recruitment consequence through an existing authority.

### Phase 4 — Identity and presentation

Implement and verify:

- 67 — Survivor Tattoos, Scars, and Heraldry.
- 80 — CRT Display Themes.

These are lower-risk only if their effect boundaries remain narrow. Step 67 depends on a verified survivor identity/progression authority; Step 80 depends on the existing UserSettings migration contract.

---

## 3. Repository-grounded baseline

| Area | Existing authority or seam | Batch 5 consequence |
|---|---|---|
| Expedition | Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs and src/Host/ExpeditionHostSession.cs | Add a saved camp companion or a carefully extended phase. Preserve existing outbound, looting, inbound, completed, and failed semantics. |
| Weather | Assets/Ashfall.Core/World/WeatherSystem.cs with PeekForecast and deterministic weather state | Reuse the existing forecast. Fix or isolate direct SeededRng construction before adding sensor confidence. |
| Radiation | Assets/Ashfall.Core/Radiation/RadiationSystem.cs | Keep true exposure and health effects here. Calibration affects readings, not physics. |
| Dose ledger | Assets/Ashfall.Core/DoseLedgerSystem.cs and src/Host/DoseLedgerHostSession.cs | Add device/station state around ledger readings without resetting cumulative dose as a side effect. |
| Foundry | Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs | Use the existing production, incident, repair, and treaty-compliance paths for brine and minting. |
| Brine water | Assets/Ashfall.Core/BrineWaterSystem.cs | Extend the existing brine/water contract or add a bounded extractor that feeds it; do not duplicate treaty quota logic. |
| Narrative letters | Assets/Ashfall.Core/Narrative/SurvivorLetterCatalog.cs and survivor_letters_lost_kin.json | The catalog is data-only. Add delivery/archive state and explicit recipient resolution. |
| Industrial fire content | Assets/Ashfall.Core/Narrative/IndustrialRuinsCatalog.cs | Narrative fire records are not a shelter fire simulator. Locate or create one active hazard owner in Core. |
| Research | Assets/Ashfall.Core/Research/ResearchSystem.cs and src/Host/ResearchHostSession.cs | Research gates must grant or unlock a real capability through a verified callback, not only change a panel label. |
| Indoor scavenging | Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs and Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs | Safe cracking must resolve through room/container/scavenge state and preserve deterministic visit ordering. |
| Greenhouse | Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs and greenhouse_items.json | Apiculture must share plots, water, temperature, contamination, and harvest outputs without free global yield. |
| Apiculture content | Assets/Ashfall.Core/Narrative/ApicultureBeeCatalog.cs | Use narrative records for flavor and evidence; create active hive state separately. |
| Economy | Assets/Ashfall.Core/Economy/MarketSystem.cs, GoodsCatalog, and the remaining DynamicEconomy integration | Black-market risk and currency acceptance must flow through existing price, inventory, and standing behavior. |
| Contraband content | Assets/Ashfall.Core/Narrative/BunkerContrabandCatalog.cs | Validate item references and risk profiles before allowing purchases. |
| Trauma and sleep | SomaticFlashbackSystem, GuiltInsomniaSystem, CombatTraumaSystem, NeedsSystem, and JournalSystem | Sleep disturbances should be a bounded extension of existing trauma and fatigue, not a second mental-health model. |
| Radio | Assets/Ashfall.Core/Radio/FactionRadioEngine.cs, src/Host/RadioHostSession.cs, and RadioPanel.cs | Fix stable channel ordering and fallback hashing before triangulation or studio attribution. |
| Generations | Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs and CenturySeedPanel.cs | Use the existing chapter/day clock. Do not introduce a competing age or century clock. |
| Cryo/seed content | CryoPreservationCatalog and SeedBankPreservationCatalog | Content records need an active germplasm vault state before they can affect Greenhouse. |
| User settings | src/Settings/UserSettings.cs, src/UI/SettingsPanel.cs, src/Audio/AudioSettings.cs, and Assets/Ashfall.Core/UI/Theme.cs | CRT preferences are versioned user settings, not campaign simulation state. |

The existing Godot UI has several real host sessions and several placeholder/read-model panels. Every panel named below must be checked against its host session before wiring controls. A visually complete panel is not evidence that the underlying mechanic exists.

---

## 4. Cross-cutting implementation rules

### 4.1 Core and host boundaries

Core owns:

- state transitions;
- deterministic resolution;
- resource and time costs;
- eligibility and requirement checks;
- effects on needs, health, inventory, standing, research, and journal;
- events for state change;
- serializable DTOs and migrations;
- data validation and canonical IDs.

Godot owns:

- panel layout and input;
- animation and visual feedback;
- AudioManager/AudioServer integration;
- shader and particle application;
- tooltips, accessibility affordances, and focus navigation;
- conversion between Godot resources and Core commands;
- headless-safe presentation adapters.

Do not put a resource deduction, faction reputation change, encounter result, or reward grant only inside a Godot callback.

### 4.2 Determinism

Every new random-looking decision must identify its seed inputs and use ISeededRng. Stable ordering is part of the simulation contract.

Before merging a touched system:

- run the same seed twice and compare a serialized state digest;
- run the same seed with different registration order where applicable;
- sort dictionary-derived collections before capture or event emission;
- avoid System.Random, Guid.NewGuid, HashCode.Combine, runtime hash codes, wall-clock timestamps, and unordered iteration in outcome selection;
- ensure a failed UI action does not consume RNG or resources unless the Core contract says it does.

Known touchpoints to audit as part of this batch:

- WeatherSystem directly constructing SeededRng for rolls and forecasts;
- ProceduralScavengeSystem capture ordering for location visits;
- FactionRadioEngine dictionary iteration and its HashCode.Combine fallback;
- any new safe, camp, radio, hive, currency, or vault identifier generation;
- any dictionary-backed survivor, accession, or treaty collections in save DTOs.

### 4.3 State and saves

Every stateful feature needs:

- a state DTO with an explicit schema version;
- CaptureState and RestoreState;
- validation on restore;
- stable ordering in serialized collections;
- a checksum-backed envelope;
- a migration path for future changes;
- an idempotent restore test;
- a malformed or missing-checksum rejection test;
- a save/load interruption test where the feature has resources or timers.

Do not add Batch 5 state to an unrelated monolithic DTO merely because it is convenient. The state owner and the host save store must be explicit.

### 4.4 Data authority

Prefer extending existing canonical files. Add a new JSON catalog only when the data has a separate lifecycle and stable schema. New files must use snake_case IDs and schema_version.

Before consuming any narrative record:

- validate required fields;
- validate duplicate IDs;
- validate referenced survivor, location, faction, item, trait, and recipe IDs;
- define behavior for an unknown or missing reference;
- keep authored prose separate from mechanical effects;
- preserve user-owned modifications in the current worktree.

Never add an invented item or location ID simply because it sounds appropriate. Run the data-integrity self-test after data changes.

### 4.5 Scarcity and consequence

Every positive result must have a cost, a gate, a failure mode, or a bounded opportunity cost. The following claims from the feature briefs must be made measurable rather than literal:

- “exact Roentgen warnings” becomes a calibrated confidence interval;
- “five days in advance” becomes a forecast horizon with uncertainty;
- “+40% crop harvest” becomes a configured, stress-tested pollination modifier;
- “+15 suspicion” becomes a data-defined event outcome;
- “50% lower tariffs” becomes a market result dependent on acceptance and reserves;
- “zero performance drop” becomes a one-pass shader frame-budget target with fallback.

### 4.6 Tone and content

Keep the presentation cold, exhausted, human, and restrained. Avoid real countries, real wars, real people, glorified violence, or magical certainty. Letters, dreams, graves, broadcasts, and instruments should reveal consequence through records and behavior rather than melodrama.

---

## 5. Detailed implementation slices

## [65] Multi-Day Expedition Campsite and Night Survival Phase

### Intended outcome

An expedition that reaches dusk during travel can establish a temporary camp, allocate shelter and watch resources, endure a bounded night simulation, and resume, retreat, or fail at dawn. Camp state must remain part of the expedition’s authoritative lifecycle.

### Current seam and quality correction

ExpeditionSystem currently models outbound, looting, inbound, completed, and failed phases. It tracks travel ticks, stamina, danger, encounters, loot, and push-luck behavior, but it has no camp phase. ExpeditionHostSession is a thin wrapper with pending encounter and save support.

Do not create a second expedition clock or a UI-only camp minigame. Prefer a companion ExpeditionCampSystem keyed by expedition ID if phase-extension risk is high. If the existing phase enum is extended, update every tick, save, encounter, history, completion, retreat, and host branch in one change.

### Core work

Add a saved camp record containing, at minimum:

- expedition ID and location/route context;
- camp start day and hour, night progress, and dawn resolution;
- tent, bedroll, and thermal shelter assignments;
- firewood or fuel reserved, consumed, remaining, and heat output;
- water and food reserve for the night;
- sentry roster, watch shifts, alertness, and fatigue;
- camouflage/noise posture;
- weather snapshot or forecast reference used for the night;
- cold exposure, surface radiation, contamination, and wildlife pressure;
- encounter key and resolution status;
- resume, retreat, injury, loss, and failure outcomes.

Use the existing WeatherSystem condition and expedition route context. Apply real needs, inventory, radiation, and fatigue effects through their Core systems or existing ports. Keep camp outcomes deterministic and keyed by expedition ID, simulation day, night index, and the seeded RNG contract.

Define a state machine such as:

1. Travel reaches the configured dusk boundary.
2. Core surfaces a camp decision.
3. Player supplies fire, shelter, food, water, and watches.
4. One night tick or bounded night segments resolve weather, warmth, fatigue, and threat.
5. A surfaced encounter pauses the camp without double-advancing the night.
6. Dawn resumes travel, retreats, or completes the camp consequence.

The camp cannot silently teleport the expedition, replenish supplies, or bypass an existing encounter.

### Godot work

Candidate files:

- src/UI/ExpeditionCampPanel.cs
- src/UI/ExpeditionPanel.cs
- src/Host/ExpeditionHostSession.cs
- a scene/resource under the existing expedition UI tree

The panel should show:

- dusk and dawn timing;
- current temperature/weather/radiation warnings;
- reserved and remaining supplies;
- bedroll/tent assignments;
- sentry shift coverage and fatigue;
- noise/camouflage trade-off;
- projected but uncertain night risks;
- a clear “break camp,” “hold,” or “retreat” action where Core permits it.

Use type, icon, and text together for hazards. A color-only frostbite or radiation indicator is insufficient.

### Data, persistence, and events

Reuse existing encounter and route data where possible. If camp encounters need new definitions, add a schema-versioned catalog with canonical encounter IDs and explicit outcome references.

Expose events for:

- camp entered;
- supplies reserved or consumed;
- watch shift changed;
- night segment resolved;
- encounter surfaced/resolved;
- dawn resume/retreat/failure;
- camp state changed.

Add camp state to the expedition save envelope, not to a panel-specific file.

### Acceptance tests

- Dusk enters camp exactly once for an expedition.
- A camp save restored mid-night produces the same dawn result as an uninterrupted run.
- Two runs with the same seed and state produce the same camp outcome.
- Firewood, food, water, and tool condition are consumed according to the contract.
- Insufficient supplies produce a bounded risk or blocked action rather than negative inventory.
- Covered sentry shifts alter detection probability and fatigue through Core.
- A night encounter pauses the simulation once and resolves once.
- Retreat returns the expedition through the existing inbound/completion path.
- A failed camp cannot be ticked again.
- CaptureState collection order is stable.

### Adversarial attack surface

- Camp is implemented as a disconnected minigame whose rewards do not touch the expedition.
- A camp tick advances both ExpeditionSystem and ExpeditionCampSystem, doubling time.
- Fire provides heat without consuming fuel.
- Sentries give perfect detection or remove all night risk.
- The UI directly edits survivor fatigue, health, or inventory.
- A save restore rerolls a night encounter.

---

## [66] Interactive Geiger Counter and Dosimeter Calibration Station

### Intended outcome

Players can service a field dosimeter, run a bounded calibration procedure, and understand the confidence of a reading. Calibration must make radiation information more trustworthy without rewriting the actual dose ledger or RadiationSystem physics.

### Current seam and quality correction

DoseLedgerSystem already tracks readings, calibration counts, assigned tags, reading history, shielding, anti-rad timing, dose bands, and save state. Its Calibrate method resets ledger calibration status, but it does not model device condition, battery, test sources, or an error margin. RadiationSystem owns exposure physics.

Do not turn calibration into a hidden cure. The core result should distinguish true dose, observed dose, uncertainty, and calibration status.

### Core work

Add a DosimeterCalibrationSystem or a narrowly scoped extension with:

- device tag and assigned survivor;
- battery/charger state;
- sensor condition and service parts;
- calibration quality or error band;
- test-source exposure budget;
- readings since calibration;
- overdue status;
- station occupancy and repair/calibration duration;
- operator skill or research gate if supported by existing systems;
- calibration history and last successful day.

Define a measurement contract such as:

- RadiationSystem computes actual exposure.
- DoseLedgerSystem records the exposure event.
- The calibration layer derives observed reading and confidence from device condition, calibration quality, shielding context, and deterministic measurement noise.
- A calibrated device narrows the error interval.
- A bad or overdue device widens the interval and can misclassify a map warning.
- Calibration does not reduce cumulative dose, health effects, or exposure already incurred.

Resolve whether the existing ledger’s global reading counter can safely become per-device/per-survivor state. Preserve backward compatibility for old saves.

### Godot work

Candidate files:

- src/UI/GeigerCalibrationPanel.cs
- src/UI/DoseLedgerPanel.cs
- src/Host/DoseLedgerHostSession.cs
- src/Audio/AudioManager.cs or the existing audio event bridge

The panel should include:

- needle/reading display;
- battery and sensor condition;
- test-source progress and exposure warning;
- calibration quality and uncertainty interval;
- calibration queue and worker assignment;
- accessible textual readout;
- click-rate intensity controlled by host audio, not Core.

Reduced motion and high-contrast settings must provide an equivalent non-animated reading.

### Data, persistence, and events

Use canonical item IDs for batteries, parts, test sources, and chargers. Do not invent a “calibration source” item without adding it to the authority and integrity checks.

Events should identify:

- calibration started;
- calibration completed or failed;
- device condition changed;
- reading confidence changed;
- calibration overdue;
- dose ledger reading corrected.

Save device state with the dose-ledger or instrumentation store, with an explicit migration from the existing ledger-only format.

### Acceptance tests

- Calibrating a valid device consumes the configured time and service resources.
- A calibrated device produces a narrower error interval than an overdue device under the same true exposure.
- The true RadiationSystem dose is identical in calibrated and uncalibrated runs.
- A missing tag or invalid device blocks the action without consuming resources.
- Test-source use has bounded exposure and cannot become a healing or free-reading exploit.
- Loading an old dose save creates valid default calibration state.
- Reading history and calibration history survive round-trip with checksum.
- Audio is absent but the panel remains functional in headless mode.

### Adversarial attack surface

- Calibration resets cumulative dose or removes sickness.
- The panel displays exact knowledge while Core stores no uncertainty.
- The calibration station creates a second radiation model.
- Device condition exists only in the UI.
- The source item is consumed by a failed click or duplicated by repeated callbacks.

---

## [67] Veteran Tattoos, Scars, and Faction Heraldry

### Intended outcome

Survivors can earn or choose a bounded identity mark that is visible in their dossier/portrait and, only when justified by an existing progression or faction authority, grants a small permanent effect.

### Current seam and quality correction

The roadmap names SurvivorNeedsState, TraitDefinition, and CurrentsCatalog, but those references do not by themselves establish a survivor identity/progression owner. Existing survivor and achievement panels include placeholder/read-model content. ExpeditionSystem also does not currently expose a navigation modifier hook.

First identify the authoritative survivor record and the existing trait/achievement grant path. Do not add a hidden bonus to a portrait overlay. If a navigation perk cannot be connected to the real travel calculation, ship the mark as cosmetic/identity state first and defer the mechanical perk.

### Core work

Create or extend a SurvivorHeraldrySystem with:

- survivor ID;
- mark type: tattoo, scar, patch, or honor;
- source event and earned day;
- faction or achievement provenance;
- cosmetic asset key;
- eligibility and consent/choice state;
- one-time effect application record;
- revocation or conflict rules where relevant.

Use explicit event sources such as a completed expedition, defense outcome, treaty service, or faction standing milestone. Do not allow arbitrary panel selection to manufacture battle honors.

If a mark grants an effect:

- use a canonical trait or modifier;
- apply it through the system that calculates the relevant outcome;
- make stacking and duplicate application explicit;
- save the provenance and applied-effect key.

### Godot work

Candidate files:

- src/UI/HeraldryTattooPanel.cs
- src/UI/SurvivorDetailPanel.cs
- src/UI/SurvivorRosterPanel.cs or the existing portrait component

Support a portrait overlay through existing asset registry and fallback icon paths. A new art pipeline is outside this batch unless separately approved. Provide text descriptions for the mark and effect.

### Data, persistence, and events

Prefer existing trait and faction IDs. Add a small schema-versioned heraldry catalog only if the effects and eligibility cannot be expressed in existing data.

Events:

- mark earned;
- mark selected/applied;
- trait or modifier granted;
- duplicate mark rejected;
- mark displayed.

### Acceptance tests

- An ineligible survivor cannot claim an earned-only mark.
- A valid source event creates exactly one mark.
- Replaying the source event does not duplicate the mark or trait.
- A navigation effect changes a real expedition calculation, or is rejected as unavailable.
- Faction provenance is preserved after save/load.
- Cosmetic display survives missing art through a safe fallback.

### Adversarial attack surface

- A UI dropdown grants any trait.
- A tattoo applies a global bonus to every survivor.
- The stated navigation improvement is visible but never affects route time.
- The same honor is re-earned after load or scene reload.

---

## [68] Subterranean Salt Mine and Mineral Brine Extraction

### Intended outcome

A mine can produce bounded rock salt, iodized brine, and possibly sulfur by consuming labor, power/fuel, drill condition, ventilation capacity, and maintenance resources. Production fulfills real District 8/Silent Foundry obligations only when the required goods are delivered.

### Current seam and quality correction

BrineWaterSystem already models membrane integrity, raw/clean water processing, steam trips, salt trade, transport loss, and save state. SilentFoundrySystem already models treaty obligations including the brine-pipe quota. Do not create a second water treaty, quota ledger, or generic “daily free output” system.

Decide during Phase 0 whether extraction is a new SaltMineExtractionSystem that emits production into BrineWaterSystem, or a focused extension of BrineWaterSystem. The choice must leave one owner for mine state and one owner for treaty fulfillment.

### Core work

Model:

- mine/vein or sector identifier;
- unlock and access state;
- drill condition and replacement parts;
- pump pressure and pipeline integrity;
- power/fuel requirement;
- worker assignments and respirator condition;
- dust/respiratory hazard;
- contamination and impurity;
- salt, brine, and sulfur output in explicit units;
- storage and transfer destination;
- treaty quota delivered versus produced;
- shutdown, repair, and incident state.

Normalize units before coding. The roadmap says “gallons,” while existing water systems use barrels and transport metrics. Choose canonical simulation units and expose display conversion only in the UI.

Connect outputs to canonical goods/recipes. If sulfur is not a current item, add it through JSON and the catalog validator before any recipe references it.

### Godot work

Candidate files:

- src/UI/BrineExtractionPanel.cs
- src/UI/ShelterOperationsPanel.cs
- src/Host/SilentFoundryHostSession.cs or the relevant shelter host

Show:

- mine access and worker slots;
- drill wear;
- pump pressure;
- output tanks/storage;
- respirator and dust warning;
- power/fuel load;
- treaty quota produced, delivered, and deadline.

Do not claim compliance while product is still sitting in a mine buffer.

### Data, persistence, and events

Reuse treaty IDs, item IDs, recipe IDs, and location IDs. Add mine definitions only if the location and output cannot be expressed by existing data.

Events:

- mine opened/closed;
- extraction batch produced;
- output contaminated or rejected;
- worker exposure;
- drill/pipe failure;
- treaty delivery accepted or missed.

Save extraction state, inventory transfers, and treaty delivery atomically enough that a crash cannot duplicate a batch.

### Acceptance tests

- Production consumes configured labor and power/fuel.
- Drill wear and respiratory exposure are deterministic and bounded.
- Output units convert consistently between Core and UI.
- Production alone does not fulfill a treaty; delivery does.
- Membrane or pipeline failures affect the existing water authority.
- No negative storage or duplicated output occurs after retry.
- Treaty compliance survives save/load and uses the canonical treaty ID.

### Adversarial attack surface

- Every day grants salt and brine without power, workers, or maintenance.
- The UI marks a treaty complete before delivery.
- A second water ledger silently diverges from BrineWaterSystem.
- Sulfur or salt recipes use unregistered IDs.

---

## [69] Wasteland Dead-Letter Mailbox and Courier Post

### Intended outcome

Scouts can discover, inspect, deliver, archive, or decline authored letters. A valid delivery changes a specific recipient’s narrative/social state through existing systems and records provenance in the journal. A letter remains a recoverable record, not a one-click universal morale buff.

### Current seam and quality correction

SurvivorLetterCatalog loads survivor_letters_lost_kin.json as a data-only catalog with authors, intended recipients, addresses, dates, envelope condition, text, and notes. It does not currently own delivery state or recipient resolution.

Create a bounded LetterDeliverySystem around the catalog, NarrativeEncounterSystem, JournalSystem, survivor identity, and existing grief/caregiving mechanics. Do not infer a recipient from a fuzzy name match or assume every named recipient exists in the current roster.

### Core work

Track:

- letter ID and source location;
- container/mailbox state;
- inspection and seal state;
- discovered, read, delivered, archived, burned, lost, or misdelivered status;
- intended recipient and resolved survivor ID, if any;
- courier route and delivery day;
- closure or grief-resolution result;
- journal entry ID and provenance;
- one-time reward/effect key.

Define explicit policies for:

- a surviving intended recipient;
- an absent or deceased recipient;
- an unknown name or fictional address;
- an unreadable or damaged letter;
- a letter already delivered after save/load;
- a player choosing to archive or burn it.

Use existing morale, guilt, grief, relationship, or quest systems where available. A closure result may reduce a specific active source, add a journal milestone, or unlock a bounded personal quest; it must not automatically grant a universal permanent perk.

### Godot work

Candidate files:

- src/UI/DeadLetterMailboxPanel.cs
- src/UI/JournalPanel.cs
- src/UI/MapAtlasPanel.cs or the existing location result panel
- src/Host/NarrativeHostSession.cs or the host owning mailbox interactions

Provide envelope metadata, inspection state, delivery choice, recipient resolution, and consequences. The panel must distinguish “read,” “delivered,” and “archived.”

### Data, persistence, and events

Validate all letter IDs and any referenced survivor/location IDs. Keep authored text in the catalog; keep delivery state in the campaign save.

Events:

- letter found;
- letter opened;
- recipient resolved/unresolved;
- letter delivered/archived/burned;
- closure effect applied;
- journal entry created.

### Acceptance tests

- A letter cannot be delivered twice.
- A missing recipient creates a defined archive or unresolved path.
- Delivery affects only the intended survivor or the explicit narrative target.
- Replaying a discovery does not duplicate the letter or journal entry.
- A damaged letter can follow the configured readability rules.
- The full state round-trips with checksum.
- Content validation reports malformed or duplicate letter IDs.

### Adversarial attack surface

- Every letter grants a permanent morale perk.
- A UI string match silently maps a letter to the wrong survivor.
- Reading a letter automatically delivers it.
- A delivery result is not saved, so the same expedition repeats it.

---

## [70] Bunker Ventilation Fire and Smoke Asphyxiation Drills

### Intended outcome

Industrial, kitchen, electrical, or maintenance fires can propagate through configured shelter zones. Players can raise an alarm, assign a brigade, close dampers, deploy extinguishers, isolate smoke, and repair damage before smoke and carbon monoxide cause lethal consequences.

### Current seam and quality correction

IndustrialRuinsCatalog contains fire-related narrative reports, but narrative prose is not a live shelter hazard. MaterialShieldingSystem may own structural/shielding concepts, but the active fire authority must be identified before implementation. Do not make a panel animate smoke without a Core incident state.

### Core work

Create or extend a ShelterFireHazardSystem with:

- incident ID and source room;
- fire, heat, smoke, and CO levels by shelter zone;
- duct adjacency and airflow direction;
- damper state and isolation;
- extinguisher type, quantity, and cooldown;
- assigned brigade workers and response time;
- alarm state;
- structural/equipment damage;
- ventilation/filter condition;
- survivor exposure and evacuation status;
- suppression, smolder, re-ignition, and resolved phases.

Advance the hazard on simulation ticks, not on frame time. Couple CO and smoke exposure to existing needs/medical systems with bounded thresholds. Airflow, dampers, and filter saturation must be one model shared with normal ventilation and smoke handling.

### Godot work

Candidate files:

- src/UI/ShelterOperationsPanel.cs
- src/UI/GameHudOverlay.cs
- a focused FireIncidentPanel or overlay
- the relevant shelter host/session

Show source, spread, CO/smoke warnings, dampers, brigade availability, extinguisher inventory, and time-to-critical estimates. Every urgent action needs a keyboard/controller path and a textual description.

### Data, persistence, and events

Use a schema-versioned hazard definition for ignition sources, spread coefficients, zone adjacency, and suppression resources. Do not use narrative report IDs as active incidents unless the data contract explicitly says so.

Events:

- alarm raised;
- fire ignited;
- smoke/CO zone changed;
- damper changed;
- brigade dispatched;
- equipment damaged;
- survivor exposed;
- incident suppressed/resolved.

Save active incidents, timers, damper states, damage, and worker assignments.

### Acceptance tests

- A fire advances once per simulation tick.
- Closing a damper changes airflow/spread according to the deterministic model.
- Extinguishers and brigade response consume real resources/time.
- CO exposure affects the correct zones and survivors only.
- A saved active fire resumes at the same state.
- A resolved fire cannot reapply its reward or damage on reload.
- A fire in a room without a valid adjacency does not spread through an arbitrary UI path.

### Adversarial attack surface

- Fire is a modal animation with no state or consequence.
- Each UI frame applies smoke damage.
- Closing a damper instantly removes all smoke without airflow cost.
- Fire can destroy items in the display but not inventory state.
- A hazard can be triggered repeatedly for free training rewards.

---

## [71] Wasteland Weather Balloon and Atmospheric Radiosonde

### Intended outcome

A launched sonde gathers additional weather observations and provides a longer, uncertain forecast window based on WeatherSystem state. It consumes launch materials, power, labor, and maintenance, and can fail or be lost in hazardous conditions.

### Current seam and quality correction

WeatherSystem already owns current weather, elapsed hours, deterministic transitions, modifiers, and PeekForecast. ResearchSystem owns knowledge progress, but its completion path must be checked before using it as a capability grant. Do not add a second forecast generator or a perfect five-day oracle.

### Core work

Add a WeatherSondeSystem or weather-observation extension with:

- sonde definition and equipment condition;
- launch site and eligibility;
- hydrogen/inflation resource;
- battery/transmitter state;
- launch day/hour and expected flight duration;
- altitude samples and radiation/noise loss;
- observation quality and forecast confidence;
- retrieved/lost/failed state;
- calibrated-versus-uncalibrated sensor quality;
- research or station requirement;
- journal/weather record ID.

The sonde should query or sample the existing WeatherSystem deterministic forecast. It may widen the forecast horizon or reduce uncertainty based on data quality, but it must not mutate future weather or expose hidden exact state without uncertainty.

Use a consistent observation contract:

- current weather remains authoritative;
- forecast output contains horizon, predicted kind, confidence, and error/uncertainty;
- a failed sonde creates incomplete or stale data;
- future forecast calls remain deterministic and side-effect free.

Audit and repair the direct SeededRng construction in WeatherSystem if the implementation uses its roll/forecast path. Preserve the existing same-seed behavior with regression tests.

### Godot work

Candidate files:

- src/UI/WeatherSondePanel.cs
- src/UI/WeatherForecastPanel.cs
- src/UI/GameHudOverlay.cs
- the relevant research/weather host session

Display:

- launch readiness;
- inflation/telemetry;
- altitude and radiation readings;
- forecast horizon;
- confidence bands and stale-data warnings;
- recommended expedition/greenhouse implications without forcing the player’s action.

### Data, persistence, and events

Use canonical equipment, gas, battery, research, weather, and location IDs. New sonde definitions require schema_version and data-integrity coverage.

Events:

- launch started;
- telemetry sample received/lost;
- sonde failed/recovered;
- forecast confidence changed;
- weather record journaled.

### Acceptance tests

- A sonde consumes its launch resources exactly once.
- A failed launch does not reveal perfect future weather.
- Sonde observations do not mutate WeatherSystem state.
- Same seed, same launch state, and same weather profile produce the same observation/forecast.
- Confidence decreases with missing samples or interference.
- Existing PeekForecast remains non-mutating.
- Research completion unlocks a real launch capability or the launch is correctly blocked.
- Active sonde state round-trips.

### Adversarial attack surface

- Five days of exact weather are revealed for one cheap click.
- The sonde rolls weather independently and diverges from the world.
- A UI “research unlocked” label bypasses the Core requirement.
- A failed balloon leaves its battery/gas available twice.

---

## [72] Tactile Lockpicking, Safe Cracking, and Stethoscope Dialing

### Intended outcome

During indoor scavenging, a scout can attempt to open a configured safe or locker using appropriate tools and skill. The resolution is deterministic, noisy, resource-aware, accessible, and integrated with existing room/container/loot state.

### Current seam and quality correction

LocationLayoutSystem provides room layout and inspection state. ProceduralScavengeSystem provides deterministic search/loot/contamination behavior, but its captured location visits need an explicit stable-order audit. No active SafeCrackingSystem was found; this is a genuine new Core seam, not a panel wiring task.

### Core work

Create a SafeCrackingSystem or a narrowly scoped extension with:

- location, room, container, and safe IDs;
- safe definition and lock difficulty;
- deterministic tumbler/gate state derived from seed and safe identity;
- scout skill and tool requirements;
- tension wrench and lockpick condition;
- attempt count and noise;
- time cost;
- breakage and alarm/encounter risk;
- opened, jammed, damaged, or abandoned state;
- loot handoff to ProceduralScavengeSystem.

The Core resolves a command such as attempt, listen, reset, or abandon. The presentation may show dial rotation and audio cues, but the UI cannot decide the actual combination or directly insert loot.

Define an accessibility alternate mode that substitutes visual gate feedback, text timing, or a simpler deterministic interaction. The alternate path must use the same Core outcome rules and not be a free success mode.

### Godot work

Candidate files:

- src/UI/SafeCrackModal.cs
- src/UI/ScavengeSearchPanel.cs
- src/Host/MaritimeHostSession.cs or the active expedition/scavenge host

Show:

- safe condition and lock profile;
- tool condition;
- tension/noise;
- attempt/time cost;
- stethoscope amplification as host audio;
- visual and text feedback;
- accessibility and input remapping.

### Data, persistence, and events

Add canonical safe/container definitions only where existing location data lacks them. Loot must resolve through existing item catalogs and the scavenge system. Avoid hard-coding military ammunition or surgical tools as the success result.

Events:

- safe inspected;
- attempt made;
- tool damaged/broken;
- noise generated;
- safe opened/jammed;
- loot transferred.

### Acceptance tests

- Same safe identity, seed, skill, and tool state resolve identically.
- A failed attempt can damage a tool or raise noise according to data.
- The safe cannot be opened twice for duplicate loot.
- Loot transfer is atomic or safely recoverable after an interrupted save.
- Accessibility mode produces equivalent Core outcomes.
- Location visit capture is stable regardless of dictionary insertion order.
- A safe outside the current room cannot be manipulated through a stale panel.

### Adversarial attack surface

- The client sends “combination solved” and bypasses Core.
- Each dial click consumes loot RNG or creates free attempts.
- Safe loot is granted before the open state is committed.
- Audio perception is required for access despite reduced-hearing settings.

---

## [73] Greenhouse Apiculture and Radiation-Hardy Bee Aviary

### Intended outcome

A maintained hive pollinates configured greenhouse crops and yields honey and beeswax over time. Temperature, feed, contamination, disease, radiation, water/power, and queen health constrain output.

### Current seam and quality correction

GreenhouseSystem already owns plots, growth, water, soil contamination, blight, harvests, and deterministic daily ticks. ApicultureBeeCatalog is a narrative/content catalog. There is no active hive state. Add a separate ApicultureSystem linked to GreenhouseSystem rather than hiding hive logic in the panel or rewriting crop growth.

### Core work

Track:

- hive ID and greenhouse bay;
- queen vitality and colony population;
- temperature/humidity;
- feed and water requirement;
- contamination and disease;
- radiation stress;
- pollination coverage and active crop links;
- honey and wax production buffers;
- extractor/rendering equipment condition;
- swarm, die-off, and recovery state;
- worker assignment and inspection cadence.

Pollination should modify only configured crop types/plots and use a bounded multiplier or yield chance that is data-defined. It should not grant a global 40% harvest increase regardless of crop, season, power, or hive health.

Honey and wax are distinct outputs. Medical honey must enter the medical/recipe authority before it can act as an antiseptic; producing a jar is not the same as sterilizing a wound.

### Godot work

Candidate files:

- src/UI/ApiculturePanel.cs
- src/UI/GreenhousePanel.cs
- src/UI/ShelterOperationsPanel.cs
- the greenhouse host session

Show hive inspection, queen/colony condition, environment, pollination links, honey/wax buffers, extractor state, and alerts. Use text and icons for radiation and contamination.

### Data, persistence, and events

Use existing narrative apiculture records for journal entries and inspection flavor. Add active hive/strain/recipe data only with canonical IDs and references.

Events:

- hive installed;
- inspection completed;
- pollination coverage changed;
- honey/wax produced;
- colony stressed/swarmed/died;
- medical processing completed.

Save hive state, plot links, output buffers, and maintenance timers.

### Acceptance tests

- Hive installation consumes the required equipment and space.
- Pollination changes only configured plots and respects environmental limits.
- Honey and wax output requires time and hive health.
- A failed or contaminated colony cannot produce pristine medical material automatically.
- Greenhouse and hive daily ticks do not double-advance growth.
- Outputs survive save/load without duplication.
- Same seed produces the same swarm/disease outcomes.

### Adversarial attack surface

- A hive is a passive +40% global multiplier.
- Honey is automatically sterile and bypasses MedicalSystem.
- The panel displays a colony while Core stores nothing.
- A dead hive continues to pollinate and produce.

---

## [74] Wasteland Black Market and Contraband Smuggling Enclave

### Intended outcome

A risky market encounter can offer scarce or restricted goods with traceability, dependency, heat, price, standing, and enforcement consequences. The player can refuse, negotiate, conceal, or accept a transaction through the existing economy and faction systems.

### Current seam and quality correction

MarketSystem is the active Core market authority. DynamicEconomySystem remains a legacy/integration hotspot. BunkerContrabandCatalog is a data-only typed catalog with risk, price, mechanics, and hidden-stash fields, but it does not itself execute inventory or faction consequences.

Create a BlackMarketSystem as a thin domain authority over MarketSystem, GoodsCatalog, inventory, faction standing, and the narrative/encounter route. Do not create a second price engine or an untracked suspicion counter.

### Core work

Track:

- market encounter ID and location;
- offer and item provenance;
- legality/restriction class;
- price and scarcity inputs;
- payment and inventory transaction;
- heat/suspicion evidence;
- faction standing or treaty exposure;
- dependency or medical risk;
- escort/inspection state;
- confiscation, investigation, or debt outcome;
- one-time transaction key.

Contraband definitions must reference real item IDs and real mechanics. A catalog field such as “morale_delta” is not sufficient until a Core system consumes it under explicit conditions.

Define an outcome matrix for clean purchase, failed negotiation, inspection, counterfeit, betrayal, and refusal. Risk should be consequential but not arbitrarily punitive.

### Godot work

Candidate files:

- src/UI/BlackMarketPanel.cs
- src/UI/CaravanBarterLedgerPanel.cs
- src/UI/AirlockSecurityPanel.cs
- the economy or narrative host session

Show offer provenance, total cost, legal/contract risk, heat, suspicion confidence, dependencies, and refusal/exit actions. Avoid celebratory presentation for addictive or coercive goods.

### Data, persistence, and events

Validate BunkerContrabandCatalog IDs, item references, price bounds, and effect keys. Faction suspicion must use the existing standing/event contract.

Events:

- market opened;
- offer generated;
- item purchased/refused;
- inventory transfer committed;
- suspicion evidence created;
- inspection/confiscation;
- faction standing changed.

Save open market state, transaction keys, heat, and consequences.

### Acceptance tests

- An accepted item is added exactly once and payment is removed exactly once.
- A failed transaction cannot create an item without payment.
- Suspicion is applied through a real faction/standing state.
- Dependency or medical risk uses existing systems and is not a tooltip-only warning.
- Reopening a resolved market does not duplicate inventory or reputation changes.
- Unknown contraband references are rejected by data validation.

### Adversarial attack surface

- The panel grants a banned item without a MarketSystem transaction.
- “+15 suspicion” is shown but no saved faction state changes.
- Contraband price ignores scarcity and reserves.
- Reloading before closing the panel duplicates the deal.

---

## [75] Survivor Sleepwalking, Night Terrors, and Dream Logs

### Intended outcome

High stress can create a bounded night disturbance affecting sleep quality, fatigue, security, and narrative records. Watchers can respond, a companion can ground the survivor, and a dream log can preserve an uncertain clue without turning dreams into a magic key system.

### Current seam and quality correction

SomaticFlashbackSystem and GuiltInsomniaSystem already track trauma-related state, sleep quality, grounding, guilt, sedatives, and ticks. JournalSystem already persists codex and journal records. Add a SleepDisturbanceSystem or focused extension rather than forking mental-health state.

### Core work

Track per survivor:

- disturbance type: nightmare, sleepwalking, panic, muttered memory, or night terror;
- severity and duration;
- trigger/source and last occurrence;
- room path or destination risk;
- sleep quality and fatigue effect;
- watchman observation;
- grounding/counseling/sedative response;
- journal/dream-log entry;
- resolved/active status.

Use the existing trauma and needs state as inputs. Trigger at a bounded night cadence, consume deterministic RNG only at the Core trigger point, and ensure a save/load at midnight does not reroll the same disturbance.

If a survivor mutters an access code or clue, store it as evidence with confidence and provenance. Unlocking a maintenance floor requires a validated quest, research, location, or code-check system. A dream text alone must not bypass access control.

### Godot work

Candidate files:

- src/UI/JournalPanel.cs
- src/UI/AirlockSecurityPanel.cs
- src/UI/PsychWardPanel.cs
- the survivor/night simulation host

Present privacy-sensitive information with clear context. Show whether a log is first-hand, watchman-reported, reconstructed, or uncertain. Do not imply that every dream is factual.

### Data, persistence, and events

Use existing narrative night-watch and wiretap content for authored event text. New disturbance definitions should reference existing trauma, condition, location, or action IDs.

Events:

- disturbance triggered;
- watchman alerted;
- survivor grounded/counseled/sedated;
- sleep quality changed;
- dream log recorded;
- validated clue unlocked.

### Acceptance tests

- The same survivor/night state produces one consistent disturbance after reload.
- Disturbance effects change fatigue/sleep/guard response through Core.
- Grounding or counseling uses real existing inputs.
- A dream clue cannot unlock a location without validation.
- Journal records include provenance and do not duplicate.
- Sedative use has the existing medical/dependency consequences.

### Adversarial attack surface

- A random dream grants a hidden floor or item directly.
- Night disturbance damage is applied every render frame.
- The same dream repeats forever because it is not saved.
- A watchman log silently reveals facts the survivor could not know.

---

## [76] Bunker Mint and Sovereign Currency Stamping

### Intended outcome

The bunker can stamp a controlled physical token or maintain a local unit of account only after the currency policy, backing, acceptance, and anti-inflation rules are explicit. Minting consumes foundry capacity and metal and creates accounting state that the market can recognize conditionally.

### Current seam and quality correction

SilentFoundrySystem already owns production, heat cycles, incidents, repairs, and treaty compliance. MarketSystem uses goods and price calculations; economy_goods.json currently does not establish a physical currency item or universal scrip acceptance.

Do not assume that minting 500 tokens automatically halves tariffs or creates market dominance. Phase 0 must decide:

- physical coin item versus ledger-only currency;
- reserve-backed, trust-backed, or barter-unit model;
- canonical minor unit and display denomination;
- which factions accept it and why;
- how coins enter, leave, degrade, melt, or become counterfeit.

### Core work

Create a CurrencyMintSystem or a tightly integrated foundry/economy extension with:

- currency schema and denomination;
- mint press condition and foundry slot;
- die/motto/purity metadata;
- copper/silver reserve and assay;
- backing ledger, such as water/grain reserve or treaty trust;
- issuance cap and inflation pressure;
- coin stock and circulation;
- counterfeiting/weight loss/melt rules;
- faction acceptance and exchange rate;
- transaction audit record.

Use integer minor units for currency math and serialize them deterministically. If the market remains barter-first, currency should be an optional settlement instrument rather than silently replacing goods.

### Godot work

Candidate files:

- src/UI/MintingPanel.cs
- src/UI/CaravanBarterLedgerPanel.cs
- src/UI/SilentFoundryPanel.cs
- economy/foundry host session

Show reserves, assay, press condition, issuance cap, resulting circulation, acceptance by faction, exchange range, and outstanding obligations. Make the warning about unbacked issuance actionable.

### Data, persistence, and events

Add currency data only after policy approval. Do not put physical coin metadata in a ScriptableObject. Reuse Silent Foundry production recipes and MarketSystem pricing hooks.

Events:

- mint batch requested;
- batch stamped/failed;
- reserves locked/released;
- currency accepted/rejected;
- exchange rate changed;
- coin melted/counterfeit detected.

### Acceptance tests

- Minting consumes metal, press capacity, and time.
- Issuance cannot exceed reserves or configured cap.
- A failed batch does not create valid currency.
- A faction that does not accept the currency cannot be forced into a coin transaction.
- Exchange rates change through the economy authority, not a panel constant.
- Currency state and reserve backing round-trip with checksum.
- Replaying a mint request does not duplicate a batch.

### Adversarial attack surface

- Currency is created with no resource or backing.
- A display-only exchange rate changes nothing in MarketSystem.
- A single batch grants permanent tariff reduction to every faction.
- The UI uses floating-point display values as authoritative balances.

---

## [77] Radio Direction Finding and Triangulation Antenna Console

### Intended outcome

Players can collect multiple directional observations, account for antenna/weather/noise quality, and derive a bounded location hypothesis. A canonical hidden location becomes discoverable only when enough evidence meets the configured confidence threshold.

### Current seam and quality correction

FactionRadioEngine handles channels and frequencies, RadioHostSession handles host audio and radio persistence, and LocationLayoutSystem handles layout/location state. FactionRadioEngine currently has dictionary-order and HashCode.Combine risks. These must be addressed before using radio observations as deterministic evidence.

### Core work

Create SignalTriangulationSystem with:

- signal/channel ID;
- antenna station and calibration;
- observation day/hour;
- bearing and error interval;
- signal strength and noise;
- weather/radio interference;
- operator/research quality;
- observation count and diversity;
- candidate locations;
- intersection/confidence state;
- discovered location key and one-time result.

Require two or three meaningful observations with different bearings or conditions. The result should be a geographic hypothesis with an uncertainty radius, not a perfect pin from a single reading. When the confidence threshold is met, reveal a canonical location through LocationLayout/Map authority and journal the evidence.

Use stable angle math and deterministic tie-breaking. Fix channel registration-order dependence and replace runtime-dependent hash fallback on the touched radio path with a documented stable hash or required injected RNG.

### Godot work

Candidate files:

- src/UI/TriangulationPanel.cs
- src/UI/RadioPanel.cs
- src/UI/MapAtlasPanel.cs
- src/Host/RadioHostSession.cs

Show:

- frequency and channel quality;
- antenna bearing;
- signal/noise;
- uncertainty cone or intersection lines;
- required next measurement;
- candidate locations and confidence;
- final map reveal and journal record.

The map must not reveal the location until Core reports discovery.

### Data, persistence, and events

Use canonical radio IDs, location IDs, frequencies, antenna definitions, and research IDs. Existing numbers-station and radio-cipher narrative content can supply flavor and clues, but a triangulated node must be a real map definition.

Events:

- frequency locked;
- observation recorded;
- antenna calibration changed;
- candidate set changed;
- triangulation completed/failed;
- location revealed;
- journal record created.

### Acceptance tests

- Two valid observations can produce a hypothesis; one weak observation cannot force discovery.
- Weather/noise reduces confidence according to data.
- Insertion order of radio channels does not alter tie selection.
- Same seed and observation sequence produces identical candidate/confidence state.
- A revealed location is canonical and persists after save/load.
- Repeating an already completed triangulation does not duplicate the map node or reward.
- Map rendering remains a projection of Core discovery state.

### Adversarial attack surface

- A single click reveals exact coordinates.
- A UI line intersection creates a location without a Core definition.
- Radio channel dictionary order changes the discovered faction.
- Runtime hash randomization changes a clue or location.

---

## [78] Cryogenic Germplasm Seed Vault and Extinction Registry

### Intended outcome

The Century Seed colony can preserve, test, and withdraw canonical crop accessions under coolant, power, maintenance, viability, and contamination constraints. Successful germination feeds an existing greenhouse/seed path and contributes to agricultural diversity.

### Current seam and quality correction

GenerationalSuccessionEngine already owns chapter/day progression, aging, retirement, mentorship, inheritance, and save state. CryoPreservationCatalog and SeedBankPreservationCatalog are data-only narrative catalogs. GreenhouseSystem owns active plots and crop growth.

Create a GermplasmVaultSystem or SeedVaultSystem with explicit links to those three authorities. Do not create a second century clock or treat narrative preservation records as already viable seed stock.

### Core work

Track:

- vault ID and accession ID;
- canonical crop/strain ID;
- source and preservation day;
- viability and genetic diversity contribution;
- coolant level and temperature;
- power draw and backup reserve;
- compressor/filter condition;
- contamination/quarantine;
- germination test state and result;
- withdrawal quantity and destination seed item;
- extinction/duplicate strain status;
- maintenance and failure history.

Use the existing generational day/chapter clock. Viability should change through bounded, deterministic maintenance and failure rules. A preserved accession must pass a germination or release rule before becoming a greenhouse seed resource.

### Godot work

Candidate files:

- src/UI/CryoSeedVaultPanel.cs
- src/UI/CenturySeedPanel.cs
- src/UI/GreenhousePanel.cs
- the Century Seed/greenhouse host session

Show:

- accession slots and crop identity;
- viability and diversity;
- nitrogen/coolant temperature;
- power and compressor status;
- maintenance schedule;
- germination test and withdrawal actions;
- extinction-risk summary with uncertainty.

### Data, persistence, and events

Validate accession crop IDs against greenhouse seed authority. Existing cryo and seed-bank catalogs can be journal sources. New active accession definitions require schema_version and canonical IDs.

Events:

- accession preserved;
- coolant/power threshold crossed;
- viability changed;
- germination test started/completed;
- accession quarantined/released;
- seed withdrawn;
- diversity milestone recorded.

### Acceptance tests

- Preserving an accession consumes the configured cryogenic resources.
- Coolant/power failure lowers viability or enters a defined emergency state.
- A germination test can fail and does not create seed output.
- A successful withdrawal creates a canonical greenhouse-compatible item exactly once.
- Existing GreenhouseSystem accepts the released seed through its normal planting path.
- The vault uses GenerationalSuccessionEngine time and survives chapter transition.
- Accession state, viability, and maintenance round-trip with checksum.
- Ten accessions are not treated as guaranteed viable merely because the UI has ten slots.

### Adversarial attack surface

- A narrative catalog entry automatically becomes a living seed.
- Cryo storage gives free seed indefinitely with no power or coolant.
- A seed vault creates an item ID that GreenhouseSystem cannot plant.
- A second age clock drifts from Century Seed succession.

---

## [79] Bunker Radio Studio and Wasteland Morale Broadcasts

### Intended outcome

Survivors can schedule and produce broadcasts using vinyl, music, news, and survival guidance. Transmission consumes staff time, power, equipment condition, and attention; the result affects radio reach, journal history, regional standing, and recruitment leads through existing authorities.

### Current seam and quality correction

RadioHostSession already wraps FactionRadioEngine, broadcast beacons, warlord warnings, persistence, and host audio. VinylRecordCatalog provides record metadata including morale modifiers and broadcast frequencies. Audio playback belongs in Godot. The reputation and refugee consequences belong in Core.

Create a BroadcastStudioSystem with:

- program ID and schedule;
- performer/reader assignment;
- record or script references;
- program quality and content tags;
- transmitter condition and power;
- range/coverage and weather interference;
- listenership evidence;
- regional reputation/standing outcome;
- refugee lead or encounter seed;
- one-time broadcast record.

Do not implement a flat global +20 reputation or directly spawn a surgeon in the panel. Emit a recruitment lead or encounter through the existing survivor/narrative authority, subject to capacity, standing, radio reach, and time.

### Godot work

Candidate files:

- src/UI/BroadcastStudioPanel.cs
- src/UI/RadioPanel.cs
- src/UI/JournalPanel.cs
- src/Host/RadioHostSession.cs
- src/Audio/AudioManager.cs

The panel should support:

- script/program selection;
- staff assignment;
- vinyl selection;
- schedule and on-air state;
- microphone/transmitter condition;
- power and range;
- listenership/coverage with uncertainty;
- audio preview with a headless-safe fallback;
- broadcast archive.

The host sends Core commands and renders events; it does not grant standing or recruit survivors directly.

### Data, persistence, and events

Use VinylRecordCatalog and existing radio corpus IDs. Add program definitions only when they have distinct mechanics or schedule data. Keep copyrighted or external audio assets governed by the project’s asset policy; this plan does not authorize new binary art/audio imports.

Events:

- program scheduled;
- broadcast started;
- program quality resolved;
- coverage/listenership updated;
- standing/reputation changed;
- recruitment lead created;
- broadcast archived.

### Acceptance tests

- Scheduling requires valid staff, equipment, and a time slot.
- A broadcast consumes configured power/time and cannot run twice for one schedule.
- Coverage and standing use transmitter/weather/faction state.
- A high-quality broadcast can create a recruitment lead but does not bypass roster capacity or encounter rules.
- Vinyl playback is host audio only and does not affect Core determinism.
- Broadcast history and consequences round-trip.
- A duplicate callback cannot duplicate reputation or recruitment lead state.

### Adversarial attack surface

- The UI awards reputation before the transmission resolves.
- Any record creates the same perfect audience.
- The studio spawns a specialist without an encounter or capacity check.
- Audio failure prevents the simulation result from being applied.

---

## [80] Retro CRT Display Themes and Hardware Shader Customizer

### Intended outcome

Players can select a bounded CRT/display style in user settings. A single lightweight Godot presentation pass applies scanlines, phosphor tint/persistence, curvature, and contrast while respecting reduced-motion, high-contrast, performance, and headless constraints.

### Current seam and quality correction

UserSettingsData and SettingsPanel already support display, audio, accessibility, UI scale, resolution, VSync, FPS, high contrast, hazard labels, reduced motion, and large fonts. AudioSettings is separate user preference state. Assets/Ashfall.Core/UI/Theme.cs is engine-agnostic design-token state.

Extend the UserSettings schema with a CrtDisplaySettings value object or equivalent. Do not put CRT preferences in campaign saves, Core simulation, or the Godot bridge. Do not promise literally zero performance cost; define a measured one-pass target and a clean fallback.

### Godot work

Candidate files:

- src/Settings/UserSettings.cs
- src/UI/SettingsPanel.cs
- src/UI/GameHudOverlay.cs or a dedicated CanvasLayer
- a Godot shader/material resource under assets/ui/ or the existing UI resource tree

Supported modes may include:

- Classic Green P1;
- Amber P3;
- High-Contrast Monochrome;
- Paper Teletype;
- Clean Modern.

Bounded controls:

- scanline strength;
- phosphor persistence;
- curvature;
- glow/bloom intensity;
- noise amount;
- vignette amount;
- reduced-motion override;
- high-contrast override.

Use one full-screen CanvasItem pass where supported. Avoid per-panel shaders and per-frame allocations. Disable or reduce persistence, flicker, noise, and curvature under reduced motion or accessibility settings. Headless mode must not require a GPU-only resource to initialize successfully.

### Persistence and events

Add a settings schema migration with clamped values and a safe default. Save through the existing atomic UserSettingsStore path. Do not include these values in simulation CaptureState.

Events:

- display style changed;
- CRT parameters changed;
- accessibility override applied;
- shader fallback activated.

### Acceptance tests

- Old user settings load with the default display mode.
- Values are clamped to safe ranges and survive a write/read cycle.
- Changing a style does not mutate campaign state or simulation RNG.
- High-contrast and reduced-motion settings override hazardous visual effects.
- A missing shader/material falls back to Clean Modern without crashing.
- Godot headless initialization and the settings self-test remain clean.
- A representative UI frame stays within the agreed frame-time budget on the project’s target hardware profile.

### Adversarial attack surface

- CRT settings are stored in the campaign save and create incompatible saves.
- Flicker and persistence remain active when reduced motion is enabled.
- A per-panel shader multiplies cost with UI complexity.
- A shader-only setting has no readable equivalent for accessibility.

---

## 6. Cross-system integration contracts

| New feature | Must read from | May write through | Must not own |
|---|---|---|---|
| 65 Campsite | Expedition, Weather, Needs, Radiation, Inventory, encounters | Expedition phase/camp state, survivor fatigue/exposure, encounter journal | A second expedition clock or inventory ledger |
| 66 Calibration | Radiation, DoseLedger, Inventory, worker skill | Device confidence/calibration, dose observation metadata | True radiation dose or medical cure |
| 67 Heraldry | Survivor identity, traits, achievements, faction standing | Survivor mark/provenance, validated modifier | Arbitrary portrait-only stat bonuses |
| 68 Salt mine | BrineWater, Foundry, Power, worker duty, canonical goods | Production batch, hazard, treaty delivery | A duplicate brine treaty or water store |
| 69 Letters | LetterCatalog, survivors, Narrative, Journal, grief/care | Delivery status, closure record, journal | Universal morale or fuzzy identity matching |
| 70 Fire | Shelter zones, ventilation, power, Needs, Medical, inventory | Fire incident, smoke/CO, damage, assignments | A frame-rate-driven hazard |
| 71 Radiosonde | Weather, Research, Power, Radio, location | Observation confidence, forecast record | A second weather generator |
| 72 Safe cracking | LocationLayout, Scavenge, Inventory, skills | Container state, noise, tool wear, loot transfer | UI-owned combinations or direct loot |
| 73 Apiculture | Greenhouse, Power, Water, Medical, inventory | Hive state, pollination, honey/wax | A global free crop multiplier |
| 74 Black market | Market, Goods, Factions, Inventory, Narrative | Transaction, heat, standing, dependency | A duplicate economy |
| 75 Sleep disturbances | Somatic, Insomnia, Trauma, Needs, Security, Journal | Disturbance state, fatigue, evidence | A magical clue/unlock system |
| 76 Mint | Foundry, Market, reserves, faction acceptance | Currency batches, backing, exchange, audit | Universal tariff reduction |
| 77 Triangulation | Radio, Research, Weather/noise, Locations, Map | Observations, confidence, canonical discovery | UI-only map pins |
| 78 Germplasm vault | Generational, Greenhouse, Power, Research, seed authority | Accession, viability, release, diversity | A second generational clock |
| 79 Broadcast studio | Radio, Vinyl, Audio host, Factions, survivor recruitment | Schedule, coverage, standing, recruitment lead | Direct host-side reputation/spawn |
| 80 CRT | UserSettings, Theme tokens, Godot rendering | User preference and shader state | Core/campaign simulation state |

Every row should have one authoritative state owner, one event path, and one save path before implementation begins.

---

## 7. Candidate file and data plan

The following are implementation candidates, not permission to create duplicate systems. Confirm each against the Phase 0 audit and use an existing owner when one already exists.

### Core candidates

Potential new or extended files:

- Assets/Ashfall.Core/Expeditions/ExpeditionCampSystem.cs
- Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs
- Assets/Ashfall.Core/Survivors/SurvivorHeraldrySystem.cs
- Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs
- Assets/Ashfall.Core/Narrative/LetterDeliverySystem.cs
- Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs
- Assets/Ashfall.Core/World/WeatherSondeSystem.cs
- Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs
- Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs
- Assets/Ashfall.Core/Economy/BlackMarketSystem.cs
- Assets/Ashfall.Core/Survivors/SleepDisturbanceSystem.cs
- Assets/Ashfall.Core/Economy/CurrencyMintSystem.cs
- Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs
- Assets/Ashfall.Core/CenturySeed/GermplasmVaultSystem.cs
- Assets/Ashfall.Core/Radio/BroadcastStudioSystem.cs

An extension of an existing class is preferred when it preserves ownership and keeps the public API coherent. Do not create every candidate file automatically.

### Godot candidates

Potential panels and host seams:

- src/UI/ExpeditionCampPanel.cs
- src/UI/GeigerCalibrationPanel.cs
- src/UI/HeraldryTattooPanel.cs
- src/UI/BrineExtractionPanel.cs
- src/UI/DeadLetterMailboxPanel.cs
- src/UI/SafeCrackModal.cs
- src/UI/ApiculturePanel.cs
- src/UI/BlackMarketPanel.cs
- src/UI/WeatherSondePanel.cs
- src/UI/TriangulationPanel.cs
- src/UI/CryoSeedVaultPanel.cs
- src/UI/BroadcastStudioPanel.cs
- src/UI/MintingPanel.cs
- existing ShelterOperationsPanel, RadioPanel, JournalPanel, GreenhousePanel, DoseLedgerPanel, SettingsPanel, and survivor panels

Panels should not become new state owners. Use existing host sessions or add thin host adapters where the state genuinely has no host boundary.

### Data candidates

Reuse first:

- survivor_letters_lost_kin.json;
- BunkerContrabandCatalog source data;
- apiculture narrative catalogs;
- cryo preservation and seed-bank catalogs;
- salt-mine inscriptions;
- industrial fire and night-watch records;
- vinyl_record_archive.json;
- existing radio corpus, location, goods, recipe, faction, research, survivor, and weather files.

Possible new schema-versioned catalogs:

- expedition camp definitions;
- dosimeter device/calibration definitions;
- heraldry eligibility/effect definitions;
- salt mine/vein definitions;
- shelter fire zone/source definitions;
- weather sonde definitions;
- safe mechanism/container definitions;
- active hive/strain definitions;
- black-market offer/risk definitions;
- sleep disturbance definitions;
- currency policy/denomination definitions;
- triangulation signal/location evidence definitions;
- germplasm accession definitions;
- broadcast program definitions.

The new catalog list must be reduced during Phase 0. Content should not be duplicated between a narrative file and a mechanical file without a clear relationship.

### Test candidates

Add focused test files under Ashfall.Core.Tests, using the existing naming style:

- ExpeditionCampSystemTests.cs
- DosimeterCalibrationSystemTests.cs
- SurvivorHeraldrySystemTests.cs
- SaltMineExtractionSystemTests.cs
- LetterDeliverySystemTests.cs
- ShelterFireHazardSystemTests.cs
- WeatherSondeSystemTests.cs
- SafeCrackingSystemTests.cs
- ApicultureSystemTests.cs
- BlackMarketSystemTests.cs
- SleepDisturbanceSystemTests.cs
- CurrencyMintSystemTests.cs
- SignalTriangulationSystemTests.cs
- GermplasmVaultSystemTests.cs
- BroadcastStudioSystemTests.cs
- UserSettingsCrtTests.cs, in the Godot/host test location if the project’s current test structure requires it

Also extend:

- save checksum sweep tests;
- catalog integrity and canonical ID tests;
- deterministic-seed tests;
- host self-test routing;
- SaveWireContract tests where new state crosses the save envelope.

---

## 8. Test and verification matrix

### Per-feature Core tests

Every system must cover:

1. happy-path outcome;
2. insufficient resource;
3. invalid ID or stale reference;
4. deterministic same-seed result;
5. different seed or state produces a permitted difference;
6. repeated command idempotency;
7. tick boundary and no double-tick behavior;
8. CaptureState/RestoreState;
9. malformed state rejection;
10. event emission exactly once;
11. negative inventory/overflow protection;
12. stable serialized ordering.

### Cross-feature integration tests

At least one test per integration contract should prove that:

- camp state is part of expedition save and affects real fatigue/exposure;
- calibration does not change true dose;
- brine production and delivery update Foundry treaty compliance;
- fire smoke affects the same ventilation/shelter model as normal operation;
- sonde observations use WeatherSystem rather than an independent forecast;
- safe loot reaches the normal inventory/scavenge path;
- apiculture affects configured greenhouse plots and produces real catalog items;
- black-market transactions update MarketSystem and faction state;
- dream clues require a real validation path;
- minting updates reserves and market acceptance;
- triangulation reveals only canonical map state;
- released seed enters GreenhouseSystem normally;
- broadcast creates a lead through the recruitment/narrative authority;
- CRT settings do not enter campaign state.

### Save and determinism gates

Before a feature is considered complete:

- capture a clean state;
- mutate all fields;
- capture and restore;
- compare the restored state field-by-field or by the project’s canonical digest;
- mutate one field and confirm the checksum changes;
- remove/null the new checksum and confirm new-format load rejects it;
- load the pre-feature save and confirm migration defaults are safe;
- run the same seeded scenario twice;
- serialize collections after different insertion orders and compare output.

### Canonical verification commands

Run from the repository root after each accepted vertical slice:

    dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet build Ashfall.csproj
    godot --headless --path . -- --data-integrity-selftest
    godot --headless --path . -- --bridge-selftest

Use the project’s additional headless self-tests where a feature already has one, such as survivors, weather, research, expansions, or audio. Report every command as PASS or FAIL. Do not use Unity commands.

### Manual Godot acceptance pass

For each UI slice, verify:

- mouse, keyboard, controller/focus navigation where supported;
- text labels for all hazards and uncertainty;
- reduced motion;
- high contrast;
- large fonts and UI scale;
- no duplicated click/submit events;
- panel closure while a simulation event is pending;
- no crash when audio assets are unavailable;
- no crash in headless mode;
- save/load while the modal or panel is open;
- reconnecting the panel after a scene change.

---

## 9. Adversarial review packet for GLM 5.2 and Qwen 3.7 Plus

Use the following as a targeted review prompt against each implementation diff. The reviewer should see the diff, this plan, the relevant data schema, and the acceptance tests, but not the implementer’s reasoning.

### Authority and boundaries

1. Does the feature introduce a second owner for an existing resource, weather, dose, market, radio, greenhouse, or generational concept?
2. Is every gameplay decision in Core rather than in a Godot callback?
3. Does any new Core file reference Godot, Unity, GodotSharp, UnityEngine, JsonUtility, or host-only APIs?
4. Does the host merely translate input and render authoritative state?
5. Are narrative catalogs being mistaken for runtime state?

### Determinism

6. Can the same seed and state produce a different result after dictionary insertion order changes?
7. Does any path use System.Random, Guid.NewGuid, HashCode, wall-clock time, or an unstable runtime hash?
8. Does a UI retry consume RNG or time without a committed Core command?
9. Are captured arrays and event lists sorted deterministically?
10. Does a save restored at a tick boundary reroll a night, fire, sonde, hive, safe, or radio result?

### Resources and economy

11. What real resource, time, capacity, or risk pays for each positive outcome?
12. Can the feature create an item, currency, treaty credit, reputation, or crop yield more than once?
13. Do output units match the existing canonical units?
14. Does a failed transaction leave inventory and reserves unchanged or safely committed according to contract?
15. Does a new reward bypass MarketSystem, Inventory, MedicalSystem, GreenhouseSystem, or Foundry compliance?

### Save and migration

16. Where is the state saved, and why is that the correct owner?
17. Is the state schema-versioned and checksum-covered?
18. What happens when a pre-feature save loads?
19. Does a null/missing checksum in a new-format envelope fail closed?
20. Can restore create duplicate events, resources, or journal entries?

### Feature-specific checks

21. Does the campsite advance the existing expedition exactly once?
22. Does calibration preserve true dose and represent uncertainty?
23. Can a heraldry mark grant an effect that no real simulation calculates?
24. Does salt production fulfill a treaty only after delivery?
25. Can a letter map to the wrong or nonexistent survivor?
26. Does fire propagate on simulation time and use real airflow/CO state?
27. Is the radiosonde an observation layer rather than a weather oracle?
28. Does safe cracking resolve in Core and use normal loot transfer?
29. Does apiculture have hive failure and produce medically valid, not magically sterile, output?
30. Does black-market suspicion affect a saved faction/standing authority?
31. Is a dream clue evidence rather than an unvalidated unlock?
32. Is minted currency backed, capped, and accepted conditionally?
33. Is triangulation uncertainty represented before a location is revealed?
34. Does cryogenic preservation require power/coolant and a real germination path?
35. Does the studio create a recruitment lead through an existing encounter/recruitment authority?
36. Are CRT settings isolated from simulation saves and accessibility-safe?

### Review verdict

The reviewer must return:

- PASS, PASS WITH REQUIRED CHANGES, or FAIL;
- exact invariant violations;
- missing tests;
- state/save risks;
- data-reference risks;
- any suspected duplicate authority;
- the smallest repair sequence.

No implementation slice is accepted on the basis of visual polish alone.

---

## 10. Definition of done for Batch 5

Batch 5 is complete only when:

- all sixteen features have an identified Core owner or an explicit documented deferral;
- every new state has a save DTO, migration, checksum, and round-trip tests;
- every cross-system resource transfer is transactional and idempotent;
- every new ID resolves through the data authority and passes integrity validation;
- same-seed repeatability is proven for all stochastic paths;
- touched existing nondeterminism risks are repaired or documented with a blocking issue;
- Godot panels are thin, accessible, keyboard/focus-safe, and headless-safe;
- audio and shader failures degrade gracefully;
- the original dirty worktree changes remain untouched unless explicitly in scope;
- the complete canonical verification checklist passes;
- a different tool or model reviews each implementation diff when two or more coupled state variables are introduced;
- the final handoff names files changed, tests run, migrations added, known limitations, and the next safe prompt.

“Done” does not mean every roadmap sentence is literal. It means the feature is authoritative, deterministic, persistent, testable, readable, and consistent with ASHFALL’s cold, human survival tone.

---

## 11. Recommended next implementation prompt

Use this as the first execution prompt after exporting this plan:

    ASHFALL Batch 5 Phase 0 — audit and prepare prerequisites only.

    Read AGENTS.md, REPO_REVIEW_REPORT.md, the Batch 5 quality plan, and the current git diff. Do not implement Steps 65–80 yet. Verify Batch 4 Step 64 save-slot/profile/checksum foundations and inspect the existing Core, Godot host, UI, JSON authority, and test seams named in the plan.

    Produce:
    1. a current-state audit for all sixteen Batch 5 steps;
    2. one authoritative state owner and one save destination for each;
    3. a dependency graph and implementation order;
    4. canonical ID and unit decisions;
    5. a list of existing systems to extend;
    6. deterministic-order and RNG risks to repair before use;
    7. a data-reference validation report;
    8. exact files proposed for the first vertical slice;
    9. blockers that require a product decision.

    Preserve all unrelated user changes. Do not edit Assets/_Game/, do not add Unity dependencies, do not add gameplay logic to Godot hosts, and do not create placeholder mechanics. End with PASS/FAIL for the five canonical verification commands and recommend the smallest first implementation slice.

After Phase 0, implement one vertical slice at a time. The recommended first slice is Expedition Campsite state plus save/restore and deterministic night resolution, followed by the calibration contract and its tests.

---

## 12. Final handoff summary

Batch 5 should be treated as a systems-integration batch, not sixteen independent UI screens. Its quality depends on preserving the authority boundaries already present in ASHFALL:

- expedition state remains expedition state;
- measurement remains distinct from exposure;
- production remains distinct from treaty delivery;
- authored records remain distinct from runtime effects;
- observations remain distinct from omniscient forecasts;
- physical currency remains distinct from a display number;
- presentation settings remain distinct from campaign simulation.

The implementation team can now use the numbered slices, integration contracts, test matrix, and adversarial review packet to produce targeted diffs for GLM 5.2, Qwen 3.7 Plus, or another coding agent without allowing the batch to grow duplicate authorities or untestable host logic.
