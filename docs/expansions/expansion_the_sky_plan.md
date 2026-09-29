# ASHFALL — THE SKY (ORBITAL HARROW)
### Storms that breach roofs, and an orbital countdown · The roof was always the last wall. Now it has weather, and a schedule.

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/the-sky-2026-09-29.md`
**Family:** "New Pressures and Places" — `docs/expansions/expansion_new_pressures_and_places_index.md` (subject 16 of the user's list).
**Tone lock (inherited):** cold, exhausted, human, restrained. The sky here is *physics with a schedule*, never a god. No injuries by default.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.
**Name note:** *Orbital Harrow* (the in-world debris programme and the code family `OrbitalHarrow*`), *Sky Defense* (the counter-battery), *Sky Layer Armor* (the ceiling cells), and *Olympus* (a platform named in eight archive records) are related and distinct. Countdown ids are `sky_countdown_*`; roof ledger ids are `roof_*`.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

ASHFALL built a roof, an alarm and a gun — and forgot to build the sky.

The roof is real: five ceiling materials, an attenuation formula, a penetration check, per-column durability, a repair call. The alarm is real: an orbital telemetry system that issues a warning, accepts a one-shot brace, resolves an impact on its day, spawns seven days of salvage and can reveal an excavation site. The gun is real: a counter-battery with turrets, magazines, crew, an intercept chance and a panel. Twelve authored events wait in a catalogue, each with a signal type, an energy, a spread, a radio line. The whole thing is ticked every day.

And **nothing in a campaign ever schedules an impact.** The only callers are self-tests and two functions named `…Demo`. No player can install a ceiling; only a demo can. The armour catalogue's costs and degradation rates are authored and never read. Storms — the other thing that falls on a roof — do not touch a single ceiling cell; they subtract from one abstract "resilience" number. A breach's only consequence anywhere is a power surge.

The Sky is the expansion that **gives the roof a weather and the sky a schedule.** Storms wear the roof by the numbers the armour catalogue already carries. A worn column fails; a failed column exposes the rooms beneath it; the shelter patches, tarps, argues and pays. And over the campaign a **countdown** of twelve authored passes — strays, clusters, blackouts, a dead-hand ping that will not stop repeating — walks toward one terminal pass, on one day, over one column of your roof. You brace it, clear the top, fire at it, or simply sit under a foot of concrete and count.

The promise: **you always knew where the roof was thin. Now something is going to check.**

### 1.2 Pillars

1. **The roof is the character.** Every column has a material, a thickness and a condition. Players learn their roof the way they learn their pantry.
2. **Weather is wear, not damage-by-fiat.** Storm load is the existing damage magnitude times the material's own authored degradation rate — carved *out of* the existing resilience hit, never added to it.
3. **The countdown is legible.** Every pass has a signal type, a warning, a confidence, an answer (brace, clear, intercept) and an aftermath. False alarms exist so that *trust* is a resource.
4. **Breach is a bad week, not a funeral.** Consequences route through existing owners (power, air, stress, damp). No injuries by default.
5. **Ship dark.** No roof plan, no countdown rows → today's behaviour byte for byte.

### 1.3 Not this

- Not a tower-defence game. Sky Defense stays exactly what it is; this expansion gives it something to shoot.
- Not a disaster movie. The terminal pass is 45 MJ: a standard 1.5 m concrete slab (37.5 MJ) loses to it unbraced and wins braced; composite plate (160 MJ) shrugs. The drama is preparation and doubt.
- Not an Olympus playthrough. The archive's platform is a *far-horizon* story (its records run to day 5,110); this plan does not compress it into the campaign (see §2.1).
- Not a new damage number. Roof wear is a carve-out of the resilience hit, and the roof ledger is one nested record on the armour owner.
- Not a body count. Harm modes need their own signature.

## 2. What the code and data actually say (audit)

| # | Fact | Where | Status |
|---|---|---|---|
| 1 | Orbital telemetry: warnings, single-slot `nextImpactDay`, brace, resolution, salvage (7-day window), revealed sites; ticked daily by the weather-intelligence coordinator. | `OrbitalHarrowTelemetrySystem.cs` L69–330; `World/WeatherIntelligenceCoordinator.cs` L162; `src/Main.CampaignOwners.cs` L1195 | LIVE |
| 2 | **No campaign path schedules an impact**: callers of `ActivateTelemetry` / `ScheduleImpact` / `ScheduleEventDef` are CLI self-tests and `ActivateOrbitalTelemetryDemo` / `ScheduleOrbitalImpactDemo`. | `src/Host/WorldHostSession.cs` L353–362; `src/Host/HostCli.*.cs` | LIVE / **GAP** |
| 3 | Scheduling **overwrites** the single pending slot; the warning is emitted at schedule time (`warning.day` = impact day). | `OrbitalHarrowTelemetrySystem.cs` L114–152 | LIVE |
| 4 | 12 authored events: 10 impacts (14–45 MJ, spread 1–4) and 2 zero-energy false alarms; each names a signal type, a radio line and (for 10) a revealed excavation site. `is_false_positive`, `radio_hook_text`, `signal_type`, `penetration_power_mj`, `lead_time_days` are **loaded and never read**. | `orbital_harrow_events.json`; `Shelter/OrbitalHarrowCatalog.cs` L10–25 | LIVE / GAP |
| 5 | A zero-energy event, if scheduled, still spawns **one** salvage item (`max(1, round(E/6))`). | `OrbitalHarrowTelemetrySystem.cs` `ResolveImpact` | LIVE / GAP |
| 6 | An impact's only downstream consumer is a **power-grid surge**. | `src/Main.World.cs` L528–535 | LIVE / GAP |
| 7 | `SkyLayerArmorSystem`: per-column material/thickness/durability; an **unarmoured column is full penetration** (damage = 10×E); `RepairCell`; `GetAttenuationFactor`. Callers of `SetCellArmor`: self-tests and `SetSkyArmorDemo`. **No player install path.** | `Shelter/SkyLayerArmorSystem.cs` L28–110; `src/Host/WorldHostSession.cs` L292–303 | LIVE / GAP |
| 8 | Armour catalogue: 6 configs (sandbag, scrap overlay, reinforced concrete, steel hull, composite, **emergency blast canopy**) with `composition`, `repair_cost`, `degradation_rate`, `blast_resistance_mj`. Loader exists in Core; **no host caller**. `InstallConfiguration` copies only tier and thickness. | `sky_layer_armor_catalog.json`; `Shelter/SkyLayerArmorCatalog.cs` L11–65; `Shelter/SkyLayerArmorSystem.cs` L43–47 | LIVE / GAP |
| 8b | **Catalogue and evaluator disagree.** The evaluator computes absorption as tier × thickness (Dirt 5, Wood 2, Concrete 25, Lead 15, Tungsten 80 MJ per metre). Three configs carry a higher printed `blast_resistance_mj` than that gives: scrap overlay (tier `Wood`, 1.0 m: prints 12, computes 2), steel hull (tier `LeadSheeting`, 1.2 m: prints 45, computes 18), emergency canopy (tier `Wood`, 0.6 m: prints 8, computes 1.2); sandbag prints 5 and computes 4. Concrete (37.5) and composite (160) agree. | `sky_layer_armor_catalog.json` L4–205; `SkyLayerArmorSystem.cs` L81–110 | LIVE (**conflict**) |
| 9 | `Brace(materialId, amount)` consumes nothing; once per impact; halves the energy. | `OrbitalHarrowTelemetrySystem.cs` L156–170 | LIVE / GAP |
| 10 | Storms: the cascade host routes shelter `damage` effects to `DisasterResponseSystem.AdjustResilience(-magnitude)`; resilience sets the fortification level that mitigates later storms. Two roof-relevant rows exist (black-rain corrosion 25 / 2 d; acid-snow roof strain 15 / 3 d). **No storm touches a ceiling cell.** | `src/Host/WeatherCascadeHostSession.cs` L227–299; `weather_gameplay_effects.json`; `Shelter/DisasterResponseSystem.cs` L397 | LIVE / GAP |
| 11 | Sky Defense: turrets, magazines, crew, intercept-chance preview, track intake from warnings, `ApplyInterceptionMitigation`, a player panel and its own save section. | `SkyDefense/SkyDefenseBatterySystem.cs` L81–466; `src/Main.SkyDefense.cs`; `Save/SaveSectionRegistry.cs` L230 | LIVE |
| 12 | Orbital state persists in `WeatherIntelligenceSaveState`; armour cells in `SkyArmorSaveState`; both ride the world save. | `World/WeatherIntelligenceCoordinator.cs` L332–347; `src/Host/WorldSaveStore.cs` L54–86, L135–136 | LIVE |
| 13 | **Olympus** records (8): timestamps `DAY_0001` → `DAY_5110 POST_BURST`, a kinetic yield of 84,000 MJ, a terminal deorbit at day 5,110, ground-station silence on day 1; truth class instrument telemetry, "never a permission". | `narrative/orbital_kinetic_telemetry.json`; `docs/content/BLACK_PROJECTS_INTELLIGENCE_MATRIX.md` | LIVE (**constraint**) |
| 14 | 14 authored storm windows drive caloric, radon and faction-morale penalties; not linked to the roof. | `year_of_ash_storm_windows.json`; `YearOfAsh/YearOfAshStormCatalog.cs` | LIVE |
| 15 | Weather hardening has 8 cold-weather upgrades (insulation, plumbing, ventilation, foundation) — **no roof target**. | `weather_hardening_upgrades.json` | LIVE (boundary) |
| 16 | What "grid x" means for a room column vs a ceiling column, and which rooms lie under which column. | `ExpansionRoomDto.GridX`; `CeilingCellArmor.gridX` | **VERIFY (P0)** |
| 17 | Where the Reckoning day and branch id are read from, for the countdown's schedule. | Year Two / campaign-clock owners | **VERIFY (P0)** |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

1. **Storms already have a damage path — just not to the roof.** The Sky does **not** add a second storm-damage number. Roof wear is a **carve-out**: the share of a `damage` effect that lands on the roof is *subtracted* from the resilience adjustment, so total pressure is unchanged and a shelter with no roof plan sees no change (DEC-SK-02).
2. **Olympus lives on a fourteen-year clock.** Its records run to day 5,110 with an 84,000 MJ yield; the campaign is at most 720 days and the authored events top out at 45 MJ. The countdown therefore uses the **twelve debris events** and treats Olympus as *context* — the platform whose outriders these are — never as a schedule source (DEC-SK-04). Whether those timestamps are the platform's own log clock or a campaign-day count is a VERIFY.
3. **A false alarm currently pays out.** If a zero-energy event were scheduled today it would still spawn one salvage item. The fizzle (no salvage, no surge) is an additive fix on the telemetry owner, decided in P0.
4. **Brace is free.** It halves an impact and costs nothing. The Sky makes it consume real stock through the inventory owner.
5. **A second armour instance can exist.** The flagship helper constructs a private `SkyLayerArmorSystem` if the world session is absent. Campaigns must always use the world-owned one; P0 checks that no path builds a parallel telemetry.
6. **Two numbers for "how much can this hold".** JSON is authoritative for authored data, but the evaluator is the existing owner of the arithmetic — and they disagree for three configs (audit row 8b). The proposal (DEC-SK-03): installing from the catalogue stores an **optional per-cell override** equal to the config's printed resistance; cells with no override keep today's formula exactly. That keeps the demo path unchanged and lets the authored numbers finally mean something. Worth noticing: the terminal pass is 45 MJ, which is exactly what the catalogue prints for **steel hull plating**.

---

# PART II — THE ROOF AND THE SCHEDULE

## 3. The Roof

### 3.1 Columns
The shelter's ceiling is a row of **columns**. An empty column is *open sky*: any impact through it is full penetration. A built column has a **material, a thickness and a condition** — Dirt, Wood, Reinforced Concrete, Lead Sheeting, Tungsten Composite. Its blast resistance is what the evaluator computes from material and thickness (concrete at 1.5 m holds 37.5 MJ); for the three configs whose printed number disagrees, see §2.1 #6.

### 3.2 Install, repair, tarp
- **Install:** the catalogue's `composition` is the bill; the shelter pays it through the inventory owner, then the column exists.
- **Repair:** the catalogue's `repair_cost` per call; condition rises by the amount bought.
- **Tarp:** the **Emergency Blast Canopy** is authored as "a sacrificial pitched timber canopy for rapid deployment ahead of predicted debris showers". That is the stopgap: cheap, weak (8 MJ), and useful for exactly one bad week.

### 3.3 Storm load
A storm front that already reaches the shelter as a `damage` effect also *loads* the roof: the loaded columns lose condition by `magnitude × the material's degradation rate × a data-set share`. The same points are **removed** from the resilience hit. Sandbags rot under acid snow; lead does not care; concrete cracks slowly and forever. The roof tells you what kind of winter it was.

### 3.4 Failing and breached
Condition sets four honest states: **Sound**, **Worn**, **Failing**, **Breached**. A Failing column drips (an air-purity input, a damp signal). A Breached column *exposes* the rooms beneath: air and stress through their owners, a repair job with a due date, and — for an impact — the existing power surge.

## 4. The Countdown

### 4.1 What is falling
The **Harrow** is the debris programme's shorthand for what the platform above sheds as its orbit decays: strays, clusters, blackouts, false returns. The twelve authored events are its outriders. The platform itself — Olympus — is the far story; the archive has its records, and they are honest about how long it has.

### 4.2 The four phases
| Phase | What the sky does | Events (existing) |
|---|---|---|
| **I — Stray Tracks** | A single track. A radar return that turns out to be ducting. | early kinetic track; radar-ducting false alarm; debris misclassification |
| **II — Clusters and Blackouts** | Several returns at once; the radio goes to static; signatures don't match. | fragmented track; kinetic seismic precursor; cluster ×2; EMP blackout; EMP signature mismatch |
| **III — The Dead Hand** | An automated system repeats a ping, then a broken checksum. Nobody is sending it. | dead-hand repeating ping; dead-hand broken checksum |
| **IV — The Terminal Pass** | One large, slow, heavy descent — 45 MJ — with three days' notice and one column's name on it. | thermal descent |

### 4.3 The rules of the sky
- **One pending pass at a time.** The scheduler refuses while an impact is pending — the existing slot would silently overwrite it.
- **Lead time is data.** The catalogue's unread `lead_time_days` finally means something: the warning is issued that many days ahead.
- **Truth arrives late.** A track's confidence rises as the day nears; a calibrated weather station raises it faster. False alarms resolve as **fizzles** with no salvage.
- **Profile-aware days.** Pass days come from authored rows per difficulty profile and branch, anchored to the Reckoning day the campaign already knows.

### 4.4 Impact Day
You have three answers, and can use all three:
- **Brace:** buy the cover — timber, canopy, cloth — through the inventory owner. Once per pass; halves the energy, as today.
- **Clear the top:** move people out of the rooms under the named column. Nobody has to be hurt if nobody is there.
- **Intercept:** the existing battery fires; a hit reduces the energy through the existing mitigation call.

Then the outcome, in three grades: **Absorbed** (a line and a crack), **Breached** (§3.4), **Penetrated** (a bad week with a long repair list). Each is authored prose plus owner calls. Afterwards: **salvage** for seven days, and sometimes a **revealed site** — one of the excavation entrances the events already name.

## 5. What it costs and what it gives

- **Costs:** materials for cover and repair; crew-days for install and patching; the tension of a five-day window.
- **Gives:** salvage; revealed sites; a shelter whose roof is *known*, and whose people learned to check it.

## 6. The whole shape

**Act I — the thin roof.** One or two columns, mostly open. Storms leave marks. **Act II — the first tracks.** Strays and a false alarm; the shelter learns to read confidence. **Act III — clusters and static.** A blackout that takes the radio; a repair list. **Act IV — the dead hand.** A ping that repeats. **Act V — the terminal pass.** The day. The roof holds, or it doesn't, and either way the sky goes quiet and the shelter goes on. One closing clause per outcome feeds the epilogue owner (VERIFY).

---

# PART III — HOW IT MEETS THE WORLD

## 7. Four stories

1. **The sandbag winter.** Acid snow eats the two cheapest columns in a week. The shelter has scrap for one repair. Someone argues for the column over the greenhouse; someone else for the one over the bunks. The roof is a ledger of who mattered.
2. **The ping.** Phase III. A repeating tone on a dead band. The Listeners say it is asking. The engineers say it is a checksum. The station, freshly calibrated, says *probably nothing*. It is a false alarm — but the next one is not, and now nobody trusts the station.
3. **Clear the top.** The named column sits over the schoolroom. Moving it costs three days of lessons and a fight. The brace costs a week of cloth. The pass lands on the empty room.
4. **The terminal pass.** Forty-five megajoules over a column that was Wood in the spring. Someone poured a concrete slab on a bet — thirty-seven and a half megajoules of it, not enough alone. The night before, the shelter braced it with every bolt of cloth it owned, and the pass arrived at half strength. The slab holds; the shelter argues for a season about whether it was luck.

## 8. Voice samples

- *Warning:* "Track acquired. Three days. Grid seven. It's not a rumour when it has a column number."
- *Confidence:* "Unconfirmed. That means the station doesn't know. It does not mean the station is wrong."
- *False alarm:* "Ducting. Cold air over warm, and the radar sees a bird the size of a truck. We pay for that scare in cloth."
- *Breach:* "It isn't the hole. It's the drip. You can live with a hole."
- *Absorbed:* "A crack you can put a finger in. Somebody put a finger in it."

## 9. Boundaries with other expansions

- **The Long Siege:** roofs are a siege's oldest weakness; a besieged shelter's roof state is read-only context.
- **The Deep:** no coupling. A breached roof does not reach the shaft.
- **Radio Free Ashfall:** the events' unread `radio_hook_text` becomes a broadcast line through the existing radio surface; RF owns the voice.
- **Faith and Schism:** impact days and the dead-hand ping are sect flashpoints (data hooks only).
- **The Underworld:** impact salvage can be fenced; no shared state.
- **The Plague Year:** damp is a read-only signal (mould risk).
- **The Record Keepers:** damp appears as an archive risk; RK owns the archive.
- **The Ration Wars:** cover materials compete with food for the same stock; the ledger already handles that.
- **Year Two:** the countdown is anchored to the branch's Reckoning day; outposts are out of scope.
- **Shelter Governance:** roof repair priority is a law-shaped choice (read-only).

## 10. Content plan

- **W1:** roof ledger, install/repair commands, one storm-load table, breach states.
- **W2:** breach effects (air, stress, power, damp) and patching.
- **W3:** countdown Phases I–II with false-alarm fizzles.
- **W4:** Phases III–IV, Impact Day plan, salvage and site reveals.
- **W5:** radio hooks and epilogue clauses.
- Every wave is data plus one focused test set.

## 11. Non-goals (restated)

No tower-defence rework; no Olympus compression; no new damage number; no injuries by default; no new save section; no new routed panel; no Unity.

## 12. Risks

| Risk | Mitigation |
|---|---|
| Double-counting storm damage | Carve-out only; parity test with no roof plan |
| Countdown feels scripted | Seeded target column, profile-aware days, false alarms |
| The roof becomes a chore | Storm wear is slow; a Worn column is playable; repair is a choice |
| Two armour instances | P0 audit; the world-owned instance only |
| Olympus timestamps misread | Context only; VERIFY clock; never a schedule source |
