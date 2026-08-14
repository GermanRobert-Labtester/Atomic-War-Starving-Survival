# MUSTER INTEGRATION PREP — Notes for the Later-Plan Sprint

> **Context:** This document is prep for the opencode session's `Next integration expansion sprint (expansion_06_the_muster_plan)` todo (currently `pending` after Loop 4+). It is a hand-off, not a task list — the sprint will do its own task breakdown. The point of this doc is to save the sprint the time spent re-discovering what the plan needs, what already exists, and what is genuinely missing.
>
> **Scope of "the Muster":** Expansion 06 — `docs/expansions/expansion_06_the_muster_plan.md` (660 lines). It is the integration layer for Days 180–360, builds on top of Expansion 05 (Year of Ash), and introduces the Coastal Hydro-Barons (fifteenth current) plus full mechanical activation of six Currents that today exist only as `currents.json` flavor text.

---

## 1. State of the world (verified 2026-08-14)

### 1.1 What already exists

| Asset | Location | Status |
|---|---|---|
| 6 silent currents in `currents.json` | `faction_cold_count`, `faction_deserter_coalition`, `faction_the_provisioned`, `faction_long_walk`, `faction_scavenger_guild`, `faction_iron_raiders` | PRESENT (1 hit each) |
| 8 working `NPC_*.cs` state classes | `Assets/_Game/Factions/NPC_*.cs` for Archivists, Sun-Seekers, Osteophages, Lamplighters, Quiet House, Grain Exchange, Tally, Undertow | PRESENT (wired via `GameBootstrap.Currents.cs`) |
| Year-of-Ash catalog family | `Assets/StreamingAssets/Data/year_of_ash_{items,events,locations,radio,survivors,quests}.json` — 30 locations, 36 items, 12 quests | PRESENT |
| `JournalSystem` / `JournalVoice` / `RiskBiasTrait` / `KnowledgeBase` | `Assets/Ashfall.Core/Journal/` | PRESENT (reused as Section III's Harven witness delivery) |
| `ExpansionHostSession` integration pattern | `src/Host/ExpansionHostSession.cs` — current shape: `Waystation, Layouts, Memory, SiteEncounters, Vouch, Greenhouse, Arbitration, Ledger` (8 systems, all with `OnStateChanged → StateChanged` wiring + `CaptureState`/`RestoreState` pass-through) | PRESENT (template for adding Muster systems) |
| `FactionWarSystem` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` | PRESENT (Section VI Muster uprising hooks) |
| `QuestlineSystem` (existing `quest_garrison_blood_debt` surrender/refuse fork) | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` | PRESENT (the existing 2-way fork is the seed for the 2–4 Approach pattern the Muster scales up) |
| `DeserterSystem` (Day-40 hatch defection) + `DesertersStandSystem` (map massacre discovery) | `Assets/_Game/Core/` | PRESENT (explicitly disambiguated from the Muster in Section VI.0 — leave untouched) |
| `npc_ivor_lasko` (Day-40 deserter vote) | `docs/lore/04_ENCOUNTERS.md`, `loc_grange_hall` | PRESENT (named in Section VI.1, not rerun) |
| `event_kittiwake_chart` + `NPC_Undertow.ChartDistributed()/OfferRescue()` | `Assets/_Game/Factions/NPC_Undertow.cs` | PRESENT (Section IV.7 only adds a wartime timing pressure — do not re-author) |

### 1.2 What is genuinely missing (the sprint's load)

| Missing | Detail | Plan ref |
|---|---|---|
| **`faction_hydro_barons` in `currents.json`** | Fifteenth current, `home_region: the_coast`, `is_active: false`. New sixth region tag `the_coast`. New badge asset `faction_badge_hydro_barons`. | §II |
| **`NPC_HydroBarons.cs`** | New state class following the `NPC_Tally.cs` / `NPC_Undertow.cs` pattern. Meret Odalen, Yurga Halvorsen, Dreth Iversen cast. | §II |
| **`NPC_ColdCount.cs`** | Four researchers at `loc_low_background_lab`. Methods: `TakeReadings()`, `TransmitFindings()`. New `event_measurement_broadcast`. | §V.1 |
| **`NPC_DeserterCoalition.cs`** | New state class for the Day-260+ Coalition (UNRELATED to `DeserterSystem` / `DesertersStandSystem` — that collision is documented in the plan, leave it). Reuses `loc_denial_cut_substation` as primary holding ground. | §V.2, §VI |
| **`NPC_Provisioned.cs`** | Quenna Brix at `loc_second_winter_homestead`. `RecordUnprompted()` method (the Long Walk listens for it). | §V.3 |
| **`NPC_LongWalk.cs`** | Their own class; circuit-tracking state; `RecordUnprompted()`-symmetric hook. | §V.4 |
| **`NPC_ScavengerGuild.cs`** | Brannick Sten at `loc_scavenger_guildhall`. Two-color claim map mechanic. | §V.5 |
| **`NPC_IronRaiders.cs`** | Den-based; only faction the player can strike first with no narrative gate. `EvaluateRaidChance()` reads `shelterVisibility` lowered by infrastructure fortify. | §V.6, `quest_nothing_to_offer` |
| **6 new locations** | `loc_muster_treeline_camp`, `loc_second_winter_homestead`, `loc_scavenger_guildhall`, `loc_iron_raiders_den`, `loc_the_tally_hall`, `loc_amnesty_petition_hall` — all MISSING from every JSON catalog. | §VIII |
| **New quests** | `quest_the_rate_card_war`, `quest_the_unsigned_order` (3 witnesses), `quest_four_names_on_the_roster`, `quest_the_second_winter`, `quest_the_eleven_month_circuit`, `quest_the_second_color_ledger`, `quest_nothing_to_offer`, `quest_the_muster` (the 4-strategy campaign), plus the cross-questline "Ledger Nobody Signed" unmarked thread. | §II, §III, §V, §VI |
| **New items / events** | `item_hydro_baron_queue_chit`, new Cold Count instruments, new Coalition items, `event_the_thin_margin_disclosure`, `event_the_thirsty_season`, `event_osteophage_explanation`, `event_measurement_broadcast`. | §II, §V, §IX |
| **`FactionWarSystem` extension** | New Day-260+ state for the Muster uprising, four strategy forks (hold / march / feint / scatter), each with distinct epilogue matrix entry. | §VI |
| **Approach pattern scaling** | Existing `quest_garrison_blood_debt` is a 2-way fork. The Muster scales the 2-way pattern to a labeled 2–4 fork per questline. Reusable enum `Approach` + helper on `QuestlineSystem` (or a new `ApproachResolver` system in `Ashfall.Core/YearOfAsh/`). | doc §I "The Approach system" |
| **Harven investigation (no verdict)** | Three contradictory witnesses — the Conscript, the Quartermaster, the Garrison clerk — each writes a different journal entry. Reuses `JournalVoice.ComposeFullText(knowledgeKey, bias, day)` keyed on survivor's `RiskBiasTrait`. The design is deliberate: the game never adjudicates which is right. | §III |
| **Epilogue matrix** | 8 named Day-360 outcomes additive to Expansion 05's 5. Likely lives in `FactionWarSystem.EpilogueOutcome` (or a new `MusterEpilogue` system). | §XII |

### 1.3 Known collision / disambiguation risks (the plan flags these)

1. **`quest_final_manifest_muster`** (the Aurora Departure / Northern Redoubt evacuation roll-call, Expansion 05) is unrelated to **The Muster** (Deserter Coalition uprising, Expansion 06). Both use the word "muster" for unrelated reasons. Do NOT rename either; the collision is a known design choice.
2. **`DeserterSystem` / `DesertersStandSystem`** are NOT the Muster. They are pre-existing, working, and explicitly out of scope. The Muster's `NPC_DeserterCoalition` is a new class; it does not inherit or replace.
3. **`loc_low_background_lab` cross-catalog duplication** — the plan references Expansion 05 Addendum item 3 on this. Verify both catalogs before adding entries.
4. **Voss vs Harven** — base game lore names the Iron Garrison commander as Colonel Voss (the Ivor Lasko Day-40 vote). Expansion 05 names Colonel Harven as the wartime CO. The plan §III says no code fix is required to reconcile them; only a timeline placement and the deliberate ambiguity.
5. **`home_region: the_coast`** — new sixth region tag. Verify `Region` enum and any region-keyed lookups (e.g., `FactionWarMapWidget`, `RadioBroadcastTerminal`) handle a sixth value before adding it.

---

## 2. Integration pattern (the existing house style to follow)

`ExpansionHostSession` is the host-side wiring point. Every new Muster system follows this exact pattern (worked example: the recently-added `CrossingArbitrationSystem` + `LedgerDebtSystem` pair for Expansion 04):

```
Assets/Ashfall.Core/<Domain>/<System>.cs     // engine-agnostic logic, ZERO Unity refs
src/Host/<System>HostSession.cs              // thin Godot host, calls into the system
src/Host/<System>SaveStore.cs                // save/load, deterministic over engine
Assets/Ashfall.Core/<System>Save.cs          // save envelope + codec
Ashfall.Core.Tests/<System>Tests.cs          // xUnit + dotnet test
src/Main.cs                                  // add property + StateChanged wire + buttons
src/Host/HostCli.cs                          // add --<system>-selftest gate
```

For Muster systems that are **NPC state classes** (the 7 new `NPC_*.cs`), the pattern is simpler — they live in `Assets/_Game/Factions/`, are `[Serializable]` plain C# with `CaptureState()`/`RestoreState()`, and are wired in `GameBootstrap.Currents.cs` (one entry per new faction).

For the **6 new locations**, append to whichever JSON catalog the plan specifies (likely `year_of_ash_locations.json` for `loc_muster_treeline_camp` etc., since the plan explicitly reuses Year-of-Ash sites; new sites probably get a new `muster_locations.json` or get folded into `year_of_ash_locations.json` — the plan doesn't pin this, the sprint will pick).

---

## 3. Verification gate the sprint should hit before claiming done

Per `AGENTS.md` (Godot-only override): `dotnet build Ashfall.csproj` (0 warnings, 0 errors) + `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` (all green) + relevant `godot --headless -- --<system>-selftest`. A `--muster-selftest` gate should be added to `src/Host/HostCli.cs` following the `dose-ledger-selftest` / `year-of-ash-save-selftest` pattern.

The **8 invented-id audit** that Loop 0 / Loop 1+ ran (every new id must be in the master catalog — `survivors.json`, `items.json`, `location_layouts.json`, `currents.json`, etc., or in a new catalog the sprint creates) must pass for the Muster's new ids. Likely new master entries: the 6 new `loc_*` ids, the new `faction_hydro_barons`, all new `item_*` ids, all new `event_*` ids, all new `npc_*` ids.

---

## 4. Opencode session context

- Session id: `ses_fff7a46b4ffeuPJhFZGmwtol19`
- Current todo: Loop 1 in progress (4 sweeps: core determinism, save roundtrip, null safety, host lifecycle)
- The Muster sprint is the todo AFTER `Loop 4+ continue until multiple consecutive zero-finding loops`
- When the sprint starts, the opencode DB (`~/.local/share/opencode/opencode.db`, currently 568MB / SQLite) is the agent's context — heavy compaction may be needed before the Muster work to keep token cost down.

---

## 5. Speed improvements already in place for the sprint

Done in the parent session (2026-08-14), non-disruptive to Loop 1:
- `.ignore` at project root (ripgrep's default ignore file) — explicitly excludes `Library/`, `Temp/`, `Builds/`, `Logs/`, `generated_AIassets/` (181MB of PNGs), `audit/`, `Figma-UI/`, `_quarantine_legacy/`, `deprecated_audits/`, `uam/`, `*.pdb`, `*.dll`, `*.so`, `mono_crash.*.blob`. `rg --files --type cs` now reports **1,737 .cs files** (Assets 1,669 + src 40 + Tests 28) — tight code-only index for every grep sweep in the sprint.
- 92 gitignored `.log`/`.txt` files (≈169MB) moved from project root to `audit/`. `ls`/`find`/`rg` directory traversals at the root are no longer weighed down by 22MB `art-wiring-full-log.txt` and friends. `git status` is also much shorter.
- `audit/` is now the single dump site (370MB, gitignored).

---

## 6. Risks the sprint should pre-empt

1. **NPC class collision** — `NPC_DeserterCoalition` is a new class; it must NOT collide with `DeserterSystem` (different system, different file). Filename check before commit.
2. **Approach enum + existing 2-way fork** — the existing `quest_garrison_blood_debt` surrender/refuse is a bool. Scaling to 2–4 approaches needs an enum that can degrade safely to 2 values for old quests. Don't break the Year-of-Ash save codec.
3. **Sixth region tag `the_coast`** — every region-keyed widget / map / radio picker that iterates a 5-element list will silently drop the new region. Audit the call sites before adding.
4. **Save format migration** — Expansion 04 added 2 systems to `ExpansionHostSession`'s save shape. Adding 7 NPC state classes + 1 FactionWar extension is a larger save migration. Bump the version, write a `MigrateFrom(version)` if old saves need to load.
5. **The cross-questline "Ledger Nobody Signed"** is a deliberately-unmarked thread. The sprint must not turn it into a flag-gated quest. It's a reward for readers of ledgers, not a system hook.
