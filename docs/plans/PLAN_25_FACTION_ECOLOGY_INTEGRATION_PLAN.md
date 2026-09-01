# Plan 25 Integration Plan — Faction Ecology & the Muster

> Status: ACTIVE · Owner: Plan 25 execution stream · Created: 2026-09-01
> Source requirement: Plan 25 (Faction Ecology & the Muster: Politics, War & the Gathering)
> Method: forensic audit first (three parallel repo sweeps), then runtime seams, then authored content.

---

## 1. Objective

Turn ASHFALL's political systems from isolated reputation surfaces into a traceable late-game spine:

```
peacetime ecology → grievances → treaty strain → escalation → faction war
→ war weariness → Muster path → witness testimony → epilogue/Verdict consequence
```

without duplicating any existing authority (standing, treaty, war, Muster, quest, epilogue).

## 2. Current reality (verified 2026-09-01, three forensic sweeps)

### 2.1 What the plan document assumed vs what the repo actually has

| Plan 25 assumption | Verified reality |
|---|---|
| `holdfast_factions.json` (3 actions) / `standing_record_factions.json` (1 action) are action catalogs | Both are **faction dossiers** (`id, display_name, alignment, wants[], offers[], signature_quote, access_rule, trust`). `holdfast_factions.json` feeds the Holdfast terminal UI only (`HoldfastCatalog.cs:138`, consumers `src/Host/HoldfastTerminalPanel.cs:580`). `standing_record_factions.json` is **dead data** — no loader parses it (only a test content assertion and `FactionIconCatalog.cs:72`). |
| `foundry_faction.json` 6 internal divisions are usable | True, but display-only (`src/UI/FactionsPanel.cs:226`). |
| The four Muster systems consume authored actions | They are **hand-rolled numeric state machines** (`Assets/Ashfall.Core/Muster/`): no JSON loading, no action selection, no thresholds, no cooldowns, no RNG. Only gates that exist: `CoalitionCampSystem.Form(day)` ≥ `MusterSystem.MusterOpeningDay` (260) and one-shot `QuestApproach` locks. |
| Witness architecture supports testimony variants | `muster_witnesses.json` = 3 flat entries `{id, witness_name, location_id, knowledge_key, day_min, body}`. No variants, no flags, no faction, no ordering. Core (`WitnessCatalog.cs`) is a dumb list; only the host UI gates `day_min` (`src/Muster/JournalWitnessPanel.cs:67`). Loader returns **empty list** if `schema_version > 1`. |
| A "Muster path" concept exists | Only player-chosen `QuestApproach` A–D → `endingKey` (`the_amnesty`, `the_open_muster`, `the_corridor`, `the_blood_price`) → `MusterRecord` → `muster_epilogues.json`. No war-state derivation; `MusterSystem` triggers day-only (`MusterOpeningDay = 260`, `MusterSystem.cs:222`). Zero references to `FactionWarSystem`/treaties anywhere in `Muster`. |
| `RegionalTreatySystem` is consumable | Core system exists and is tested, **but the host never calls `LoadCatalog`** (`src/Main.ShelterSocial.cs:68` constructs + restores only) — catalog is empty in production; `Propose` always fails `unknown_treaty`. Authored treaty corpora (`narrative/regional_treaty_protocols.json` = 16 treaties, `foundry_accords.json` = 12) use a **different narrative schema** consumed by `RegionalTreatyCatalog` (read-model) and `SilentFoundryCatalog`. |
| Escalation/weariness flags exist | None. Zero `*grievance*`, `*treaty_breach*`, war-weariness ids in code or data. `escalation_*` flags in `events.json` are the leadership-crisis chain (different domain). |
| 06C war spine to surround | Real: `FactionWarSystem` (`YearOfAsh/FactionWarSystem.cs` — standing −100..+100 per faction, `isHostile` ≤−50 / `isAllied` ≥+50, `WarTension` 0–100, friction no-op ≤ day 240 then +1/day, territorial clash every 15 days, zero RNG) + `FactionWarChainRunner` (22 chains / 45 stages, trigger grammar closed set: `PlayerVisitedTrigger`, `ChainResolvedTrigger`, `DayOffsetTrigger`, `AndTrigger`, `AlwaysTrigger`; only choice effect = `moraleDelta`; **every stageId requires an explicit `FactionWarTriggerTable` entry** — test-pinned). |
| War precedes the Muster | **Inverted in authored canon.** Muster opens day 260 (inside `Phase5_FactionSiege` 241–300); the war chain is authored at **minDay 480–605** (bands `cold_war` 480–498, `open_conflict` 503–528, `the_offensive` 533–560, `culmination` 565–605, incl. `evt_d588_ceasefire_by_exhaustion`). The campaign calendar (`Campaign/CampaignCalendar.cs`) has **no day cap** — day 360 is the epilogue matrix *view*, play continues into the war window. |

### 2.2 Standing is fragmented (no single authority)

| Store | Range | Used by |
|---|---|---|
| `FactionStanceEngine` (`Economy/FactionStanceTypes.cs:45-54`) | −100..+100, thresholds (raid −50, rob −20, min-trade −40, intel-share +40), no decay | Trade surfaces (`SilentFoundryHostSession`, `DeepCoastHostSession`) |
| `PrpfStandingSystem` (`Factions/PrpfStandingSystem.cs:27-38`) | −100..+100 (Hostile ≤−50, Allied ≥+50) | PRPF join gate |
| `ScavengerGuildState.trust`, `HydroBaronsState.trust` | private floats, floor 0, **no ceiling** | Only their own systems |
| `IronRaidersState` | aggression 0..1, visibility (floor 0.1) — no trust at all; `SetAggressionLevel` has **no production caller** | Raid-chance formula (host never rolls) |
| `CoalitionCampState` | membersRallied, garrisonLockoutRisk 0..100 | Camp strategy |

Decision: Plan 25 uses **each system's own persisted scalar** as its standing read. No new cross-faction currency.

### 2.3 Cross-plan candidate-pool availability for witnesses

| Pool | Status | Binding |
|---|---|---|
| 20B named NPCs | SOLID | `npc_*` in `wasteland_settlement_npcs.json`, `characters.json` |
| 09 palliative/medical | SOLID | `SickListSystem.palliativePlan`, `AssignPalliative`, `item_palliative_morphine` |
| 12A raised children | PARTIAL | `LineageRecord` (parent/adopted/mentor childIds) — no "raised" boolean |
| 18A claimants | PARTIAL | quest ids only (`quest_holdfast_census_claimant_audit`) |
| 22C foundry labor | PARTIAL | `SilentFoundryIds.JournalStrike`, strike-day state — no actor ids |
| 10A spared warlord | MISSING | nearest existing: `flag_become_warlord`, `flag_messenger_kept` (MoralChoiceIds) |
| 24B rescuees | MISSING | distress signals are prose-only content |

Rule (Plan 25 G.12): every witness binds real flags; archetypes without stable flags get **substituted or flag-authored at their producer**, never left as dead content.

### 2.4 Known breakage when witnesses grow 3 → 15

- `Ashfall.Core.Tests/MusterContentCatalogTests.cs:52-55` — pins count 3 + all three ids.
- `src/Main.UiTests.Muster.cs:40` — `_muster.Witnesses.Count == 3`.
- `src/Main.Muster.cs:215-217` — "Three accounts: {n} loaded" copy.

## 3. Architecture — four runtime seams (Core, `Ashfall.Core.Muster`, engine-agnostic)

No new standing authority, no new war resolution, no new diplomacy engine. Each seam extends an existing owner.

### S1 — FactionActionBoard (peacetime faction actions)

- **Data authority (new):** `Assets/StreamingAssets/Data/muster_faction_actions.json`, `schema_version: 1`, snake_case.
- **Entry shape:** `{id, faction_id, title, min_day, max_day, once, cooldown_days, requires_flags[], forbids_flags[], variants: [{band, text, choices: [{choice_id, text, effects: {trust_delta, item_id, item_amount, flags[], journal}}]}]}` where `band ∈ hostile|poor|neutral|good|allied`.
- **Runtime (new):** `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` — deterministic per-day availability (day window → flag gates → once/cooldown → standing band), ordinal-sorted resolution, produces flags into `IFlagLedger`, persists action history `{action_id, day}` for idempotence, `CaptureState/RestoreState` DTO.
- **Standing bands (per system, documented in MUSTER_FACTION_RUNTIME_CONTRACT.md):** guild/hydro `trust` bands; raiders band derived from aggression/visibility; camp from formed/members/lockout. Thin additive `AdjustTrust(float)` seams where a system lacks one (guild, hydro) — events raised, clamped, save-safe.
- **Host:** `MusterHostSession` constructs the board; `MusterHostSave.FactionActions` (null-tolerant for old saves); `src/Main.Muster.cs` handler surfaces actions in the existing codex/status pattern.

### S2 — Witness schema v2 + WitnessSelector

- `muster_witnesses.json` → `schema_version: 2`. Entries gain optional `faction_id`, `priority`, and `testimonies: [{variant_id, requires_any_flags[], requires_all_flags[], forbids_flags[], body}]`. v1 `body` = one unconditional testimony (permanent back-compat path).
- `WitnessCatalog.CurrentSchemaVersion` → 2 with v1 fallback (fixes the silent-empty trap at `WitnessCatalog.cs:49`).
- New `WitnessSelector` (Core): day gate → eligibility via new port **`IWitnessEligibility`** (`IsFlagSet`, `IsSubjectAlive`, `IsFactionPresent`) → first-match testimony in authored order → deterministic ordering (`priority` desc, then id ordinal) → optional cap with faction-diversity rule.
- Results (`witness_id → {variant_id, delivered_day}`) persisted in `MusterHostSave` → stable epilogue/Verdict surface. Dead subjects never testify (absence/representation instead).

### S3 — MusterPathEvaluator

- New Core pure function: inputs = `FactionWarSystem` state (dominantFactionId, WarTension, standings), treaty read-model state, grievance/peace flags, `CoalitionCampState` → `muster_path ∈ {negotiated, victors, unsettled}`.
- Additive `musterPath` field on `MusterState` (default empty; old saves fine). Evaluated at Muster resolution; re-evaluated on war-state change (idempotent).
- Drives camp-scene variants, witness testimony pressure, and the epilogue/Verdict hook. Player Approach A–D selection unchanged; path is the political context around it.

### S4 — War-event flag/standing extension

- `FactionWarContentCatalog` DTO: optional `requires_flag`/`produces_flag` on stages; `requires_flag`/`produces_flag`/`standing_delta` on choices (`standing_delta` applied via `FactionWarSystem.ModifyStanding`).
- Exactly **one** new trigger node `FlagTrigger` added to the closed grammar + explicit `FactionWarTriggerTable` entries (totality test enforces).
- All 16 new chains authored into `faction_war_events.json`: 6 escalation (E-P1..P6, grievance-gated, ~day 200–300), 6 mid-war (E-W1..W6, gated via **existing** `ChainResolvedTrigger` on real 480–605 battle stages), 4 weariness (E-R1..R4, culmination band, feeding toward `evt_d588_ceasefire_by_exhaustion`).

### Cross-cutting

- Bounded flag vocabulary: `flag_grievance_*`, `flag_favor_*`, `flag_war_*`, `flag_peace_*` — each with producer → consumer → resolution recorded in `PLAN_25_LATE_GAME_CONTINUITY_MATRIX.md` and pinned by a lint test.
- New catalogs registered in `CatalogBootValidator` + `ContentUtilizationScanner`.
- Political journal/codex entries via existing host `TryAddRawEntry` pattern (major treaties/breaches/war transitions/Muster invite only — no standing-delta spam).
- Culture codex: `muster_faction_culture.json` + Core loader + codex panel consumption.
- Treaty host feed (isolated, revertible commit): adapter from `narrative/regional_treaty_protocols.json` → `TreatyDefinition`, loaded in `SetupRegionalTreaty`; fallback = read-model only, documented.

## 4. Timeline anchor (repo pacing; supersedes plan-example dates)

| Phase | Days | Content |
|---|---|---|
| Peacetime ecology | 1–199 | faction actions, culture, favors/grievances |
| Escalation backdrop | 200–259 | E-P1..P6 (friction begins 240; Muster opens 260) |
| Muster window | 260–360 | gathering, camp scenes, witnesses, path evaluation |
| Hot war (06C canon) | 480–605 | E-W1..W6 alongside authored bands; E-R1..R4 → ceasefire 588 |
| Post-ceasefire | 605+ | epilogue/Verdict consumption of testimony results |

Deviation from the plan document's "war → weariness → Muster" order is deliberate: continuity outranks narrative preference (plan §13.10); the 06C chain days and `MusterOpeningDay` are canon and untouched.

## 5. Batches (each = one commit; full verification gates per AGENTS.md)

1. Forensic docs (this file + 8 contract/audit docs).
2. Seam S1 → 3. Seam S2 → 4. Seam S3 → 5. Seam S4 (runtime before content).
6. **Vertical slice GATE** — A1 + grievance flag + E-P1 + W6 (2 testimonies) + arrivals scene + negotiated path + save/load tests. No scale-out until green.
7. 25A ecology (4 commits: Guild / Hydro / Raiders / Coalition). 8. 25E culture. 9. 25C escalation. 10. 25C war context + weariness. 11. Paths finalized. 12. 25B witnesses (15 total). 13. 25F camp scenes. 14. 25D + 25G cross-plan + treaty feed. 15. 25H QA + closeout.

## 6. MUST PRESERVE / MUST NOT

PRESERVE: `MusterOpeningDay = 260`; 06C chain ids/days/bands; Approach A–D → endingKey flow; all save formats (additive fields only); faction canon; v1 witness loading forever.
MUST NOT: new standing/war/treaty resolution systems; `System.Random`/`Guid.NewGuid()`; engine refs in Core; dead content; witness resurrection; retconned dates; new guild currency; display-name keys.

## 7. Verification

Per gate: `dotnet build Ashfall.Core.Tests/...` clean · `dotnet test` green · `dotnet build Ashfall.csproj` 0/0 · `godot --headless -- --data-integrity-selftest` 0 errors · `--bridge-selftest` exit 0 · domain gates `--muster-selftest`, `--muster-uitest`, content-utilization, narrative-continuity at content batches. Done = Plan 25 §17 checklist + `PLAN_25_FACTION_ECOLOGY_MUSTER_CLOSEOUT.md`.
