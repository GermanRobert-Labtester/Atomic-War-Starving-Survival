# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Dispatch Wave 3 + Cross-Encounter War Ledger + Authored Salvage Rumors + Economy Telemetry (W1–W5)

> **STATUS: APPROVED BY USER** ("Lets tackle the next 5 suggestions and
> foreman, i authorise it, make it unique and creative!")
> **Package:** `DISPATCH-WAVE3-WAR-LEDGER-SALVAGE-RUMORS-ECONOMY-W1-W5`
> **Role:** Builder (this stream), foreman host-pick authority user-granted
> **Claim:** `claim-dispatch-wave3-war-ledger-rumors-economy-w1-w5-2026-10-01`

---

## 1. W1/W3 — Dispatch wave 3: every decision-only route wired (7 routes)

Premise re-verified before wiring: all seven routes had self-contained
`ConfigureActions` bindActions (`Ensure*()` / coordinator binds) — emitters
were the only gap. Host picks made on panel identity (each target console's
own title names its family):

| Route | Console title | Host chosen | Emitter |
|---|---|---|---|
| `emergency_response` | RESPONSE STANDBY (crisis HUD) | EventsLogPanel | CRISIS RESPONSE CONSOLE |
| `chem_warfare_defense` | ATMOSPHERE WATCH // TOXIC HAZARD MONITOR | WeatherPanel | TOXIC ATMOSPHERE WATCH |
| `winter_freeze` | DEEP WINTER // FREEZE WATCH | WeatherPanel | DEEP FREEZE WATCH |
| `desperation_crisis` | SANCTUARY CRISIS // DESPERATION & TABOO | ShelterPanel | SANCTUARY CRISIS MONITOR |
| `archaeology_excavation` | BEFORE // PRE-WAR ARCHIVES & DIG SITES | JournalPanel | ARCHAEOLOGICAL DIG REGISTRY |
| `ceremony_ritual` | COMMON HEARTH // WASTELAND FESTIVALS | MusterPanel | CEREMONIES & RITUALS |
| `robotics_assembly` | AWAKENED STEEL // ROBOTICS WORKSHOP | WorkshopPanel | RoboticsButton (tscn) |

- `emergency_response` follows the T19 doctrine: **manual funnel, no
  auto-open** — the crisis console is one click from the events log.
- WorkshopPanel.tscn gained a `RoboticsButton` header node
  (`unique_name_in_owner`), wired via `binder.Get<Button>` + a
  `UiPanelContractTests` row.
- `Main.UiPanels.cs` wiring note: `_eventsLogPanel` is instantiated *after*
  the consolidated wiring block, so its subscription line lives at its own
  instantiation site (subscription before assignment would NPE at boot).

**Census: PendingForeman 15 → 8.** Every remaining entry is hard-blocked by
an ACTIVE claim (`skill_matrix`, `survivor_downtime`, `amputation_surgery`,
the pfgl dashboard trio) or a known unwired debt (`justice_tribunal`,
`verdict_dashboard`); the census comment now states exactly that so nobody
"helpfully" wires a blocked route.

## 2. W4 — Cross-encounter war ledger

**Premise verified:** every `BeginEncounter` builds a fresh `CombatState`
(only an `encounter_start` event survives into the new list), so
`CombatHistoryPanel`'s "history" was one fight deep.

**Design (one authority, existing save seam):** the ledger lives on
`CombatState` itself — the object `CombatSaveStore` already persists. No
parallel store, no new save section, no RNG.

- `CombatTypes.cs`: `CombatEncounterRecord` (Day, LocationName, OutcomeText,
  RoundNumber) + `CombatState.EncounterHistory` (cap 30; legacy saves
  restore to empty — verified by test) + `CombatSnapshot.History`.
- Append at all four resolution sites: Won + Lost
  (`TacticalCombatSystem.Damage.cs` `CheckResolution`), turn-based retreat
  (`TacticalCombatSystem.Actions.cs`), realtime flee-extract
  (`TacticalCombatSystem.RealtimeFlee.cs`) — via one
  `RecordWarLedgerEntry()` helper.
- `TacticalCombatSystem.StartCombat`: the ledger is carried across the
  per-encounter reset ("the history a player carries is the war, not the
  fight").
- `CaptureState` copies it; `Migrate` carries it (null-safe); `BuildSnapshot`
  formats `"D{day} · {location}: {outcome} (round {n})"`.
- `CombatHistoryPanel` renders an **ACROSS THE WAR** section above the
  per-encounter log.

**Tests:** `CombatEncounterLedgerTests` 4/4 — append fields; reset survival
(new encounter keeps the ledger while events reset to the fresh start line);
CaptureState/RestoreState round-trip + legacy-null restore; snapshot format.
Regression suites re-run: `CombatSaveRoundTripTests` 4/4,
`RealtimeRestoreHardeningTests` 3/3.

## 3. W5 — Authored rumor lines (creative, data-driven)

Twenty in-world hearsay lines — one per salvage-bearing node, restrained
tone, each hinting at its site's nature without confirming yields, e.g.:
*"Trappers swear the loading dock still holds pallets nobody has touched
since the ceasefire."* / *"Issue room seven, they say, was sealed from the
outside, and the inventory never reconciled."*

- `wasteland_map_v1.json`: optional `rumor` field on the 20 `lootTable`
  nodes (byte-identical JSON round-trip verified before writing).
- `WastelandMapCatalogLoader` DTO + mapping → `MapNode.Rumor`.
- `WastelandMapSystem.GetNodeIntel` Rumored branch projects the authored
  line; the old hardcoded "Unconfirmed scrap / rumor" remains the fallback
  for unauthored nodes.
- `OpenMapDetailPanel` funnel Rumored branch: a Rumored sector shows its
  hearsay line as the most it can honestly say — hearsay, not survey.
- Gates: Plan 76 map fact pins non-empty rumors (≤200 chars) on all 20
  nodes; truthfulness funnel fact pins `MapFogState.Rumored` +
  `salvageNode.Rumor`.

## 4. W2 — Salvage economy telemetry

New `SalvageEconomyBalanceTests` (5/5) — **fully analytic** (exact
weight-share arithmetic over the authored catalogs; no rolls, no RNG):

- Strict tier ordering: common < electronics < trade < rare < weapons
  (computed: 6.1 / 19.7 / 20.7 / 23.4 / 73.3 barter-per-roll).
- Each table pinned to its authored analog band: common≈collapsed_structure
  [0.5–2.0]× (actual 0.81), weapons≈military_depot [0.75–2.0]× (1.31),
  trade≈convoy_cache [0.6–2.0]× (1.09).
- Hazard must not invert against reward (rare/weapons hazard ≥ common).
- Failure messages carry the full per-table telemetry printout.

## 5. Verification

- Build (`dotnet build Ashfall.csproj`): **0 errors**.
- Focused: ledger 4/4, economy 5/5, Plan76 6/6, truthfulness 6/6,
  reachability 2/2 (census 8), contract 1/1, CombatSaveRoundTrip 4/4,
  RealtimeRestoreHardening 3/3.
- `bin/ashfall-dev validate-json` on both edited data files: **714/714**.
- Headless `--ui-layout-selftest`: **PASS**.
- No full suite (user did not type RUN FULL TESTS).

## 6. Deliberate non-goals

- The 8 remaining census routes stay frozen behind claims/debt.
- No auto-open for the crisis console (T19 doctrine).
- No Core catalog dependency injected into `WastelandMapSystem` — rumors are
  authored map data, not catalog lookups.
