# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# T17 Recruitment Verify-Close + Combat Consoles (S1) + Salvage Survey Authority (S2) + Route Dispatch Wave 2 (S3)

> **STATUS: APPROVED BY USER** (chained-session authorization: "aim for full
> integration, repair, hardening, upgrading, polishing... Also check the
> previous 5 suggested and decide which 3, at most 4 are the most highest
> priority and work on those too")
> **Package:** `COMBAT-CONSOLES-SALVAGE-SURVEY-ROUTE-DISPATCH-T17-S1-S3`
> **Role:** Builder (this stream)
> **Claim:** `claim-combat-consoles-salvage-survey-t17-s2-2026-10-01`

---

## 1. T17 — RecruitmentSystem "orphan" premise: STALE, closed verify-only

Rule 7 premise check FIRST (`verify zero src/ refs`): the premise is stale.
`RecruitmentSystem` was promoted from Core orphan to fully integrated host
feature by Plan 204 on 2026-09-26
(`.ai/plans/integrated/systems/INTEGRATED_UNBLOCK_PLAN204_RECRUITMENT_DEFECTION_2026-09-26.md`):

- Save section `recruitment` → `recruitment_save.json`:
  `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:352,399`.
- Live route `recruitment` ("Survivor Recruitment Desk", Expanded):
  `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs:312`.
- `HostCliAction.RecruitmentSelfTest` + `--recruitment-selftest` (aliases
  `--defection-selftest`, `--survivor-recruitment-selftest`):
  `Assets/Ashfall.Core/HostCliRegistry.cs:297,1918-1922`.
- Day owner (TickDay, capture/restore, dirty-save): `src/Main.CampaignOwners.cs`
  (`_m._recruitment?.TickDay(day)` etc.), panel in overlay catalog
  (`src/Main.PanelLifecycle.cs:224`), host session `src/Host/RecruitmentHostSession.cs`.

**Verification:** `scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan204RecruitmentIntegrationTests.cs`
→ **7/7 PASS**. No duplicate work performed.

## 2. S1 — combat_detail / combat_history wiring (prior suggestion 3, unblocked)

Both routes were registered Live (`PanelRegistryBootstrap.cs:71-72`) with
ConfigureActions (`Main.PlayerSurfaces.cs:510,514`) but had **zero emitters**
and **no bind path** (ConfigureActions has no bindAction; `_combatDetailPanel`/
`_combatHistoryPanel` were instantiated in `Main.UiPanels.cs` but never bound
to a session — a route open would have shown an empty panel). Unblocked when
the combat-playability streams (T12–T27) completed and released their paths.

**Changes:**
- `src/UI/CombatPanel.cs`: `OnOpenCombatDetailRequested` /
  `OnOpenCombatHistoryRequested` events + ENGAGEMENT DETAIL / ENGAGEMENT
  HISTORY buttons in the console button row. (Foreign combat-wave hunks in
  this file — T13 movement, T14 bandage, T15 result log, T18 joypad, T20
  ammo cell — were staged by a concurrent stream and are deliberately
  excluded from this commit via line-level filtering.)
- `src/UI/CombatDetailPanel.cs` / `src/UI/CombatHistoryPanel.cs`: `Bind`
  made idempotent (Unbind-first; MapPanel T11 pattern) — the funnel re-binds
  on every open and would otherwise stack `StateChanged` subscriptions.
- `src/Main.UiPanels.cs`: funnels following the T16 live-monitor convention —
  `SetupCombat(); _combatXxxPanel.Bind(_combat); Open();`. Deliberately NOT
  `OpenPlayerPanel(...)` (it calls `CloseAllOverlayPanels()` and would close
  the combat console mid-realtime-encounter). Route ids quoted in the wiring
  comment so the reachability census tracks them.

## 3. S2 — salvage survey authority (prior suggestion 2)

**Defect found (Rule 7):** 20 wasteland map nodes carry `lootTable` ids
(`salvage_common` ×9, `salvage_rare` ×4, `trade_goods` ×3, `salvage_weapons`
×2, `salvage_electronic` ×2) that did **not exist** in
`Assets/StreamingAssets/Data/scavenging_tables.json` (all authored ids are
`table_loot_*`). Dangling references: the map estimate seam forwards
`scavenging_table_id = targetNode.LootTableId`
(`WastelandMapSystem.cs:778`), and `GetNodeIntel` leaked the raw internal id
to the player as "salvage data". The Plan 76 gate never caught this — it
validates `expeditions.json` only.

**Changes:**
- `scavenging_tables.json`: five tables authored in the canonical catalog
  (54 → 59 tables), schema-identical to existing tables, entries reference
  only item ids verified against `items.json` (724 ids; the four unresolvable
  luxury-good candidates were dropped), restrained fictional tone:
  `salvage_common` (Wasteland Structural Salvage), `salvage_rare`
  (Restricted-Zone Deep Salvage), `salvage_electronic` (Signal & Power
  Electronics Recovery), `salvage_weapons` (Munitions Site Ordnance Sweep),
  `trade_goods` (Caravanserai Trade Stock). Byte-identical JSON round-trip on
  the untouched portion verified before writing.
- `src/Main.UiHandlers.cs` (`OpenMapDetailPanel` funnel): resolves the node's
  table through the live Plan 46 catalog (`_expeditions?.Engine?
  .ScavengingCatalog` — the `Main.Anomaly.cs` convention, no parallel
  cache). Fog-gated exactly like the hazard rows: **Surveyed** → table
  display name; **Visited** → display name + "Yield tiers: …" derived from
  the actual entries; **Rumored/Unknown** → null (the R4 truthful empty state
  stands). Core `GetNodeIntel` untouched (host adapter owns presentation).
- `src/UI/MapDetailPanel.cs`: primary `Bind` gained optional `salvageSurvey`
  threaded to the existing `lootCategories` render path.
- `Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs`:
  new fact `WastelandMapLootTables_ResolveAgainstPlan46Authority` — every
  map `lootTable` resolves (20 bindings pinned), each salvage table has
  entries + display_name, and every entry item id resolves against the
  merged item catalog.
- `MapPanelTruthfulnessGateTests` funnel fact extended with
  `TryGetTable` / `MapFogState.Surveyed` / `MapFogState.Visited` /
  `salvageSurvey` pins.

## 4. S3 — route dispatch wave 2 (prior suggestion 1, subset)

Four PendingForeman routes dispatched (registry bindActions were already
self-contained via `Ensure*()`; only emitters were missing):

| Route | Natural host | Emitter |
|---|---|---|
| `comms_array_transceiver` | RadioPanel | COMMS ARRAY TRANSCEIVER button |
| `mercenary_bounty_board` | MusterPanel | MERCENARY BOUNTY BOARD button (footer row) |
| `railway_logistics` | ExpeditionPanel | RAILWAY LOGISTICS TERMINAL button (console row) |
| `expansion_fallout_plume` | RadiationDetailPanel | PlumeButton (tscn node + binder + contract row) |

`Main.UiPanels.cs` wires all four through `OpenPlayerPanel("<id>")` (correct
here, unlike S1: these panels are not mid-encounter surfaces).
`PanelRouteReachabilityGateTests` PendingForeman census: **21 → 15**
(+2 combat). Remaining 15 are claim-blocked (pfgl dashboard trio,
skill_matrix), foreman-decision (chem_warfare_defense, winter_freeze,
desperation_crisis, ceremony_ritual, emergency_response, verdict_dashboard,
robotics_assembly, archaeology_excavation), or debt-tied (justice_tribunal,
amputation_surgery, survivor_downtime).

## 5. Deliberate non-goals

- Suggestion 4 (`emergency_response` auto-open): needs the event-owner
  decision; stays pending (T19 doctrine — the combat stream likewise chose
  manual over auto-open).
- Suggestion 5 (integrator sweep commit of ~459 foreign dirty files):
  foreman-authorized only. A concurrent integrator stream has now staged all
  459 files; that sweep is theirs, not mine.
- Core `GetNodeIntel` LootDescription rewrite: host-adapter projection is
  the sanctioned seam; no Core architecture change.

## 6. Verification

- Build (`dotnet build Ashfall.csproj`): **0 errors**.
- `scripts/run_test.sh` focused: PanelRouteReachabilityGateTests **2/2**,
  MapPanelTruthfulnessGateTests **6/6**, Plan76DestinationLootReferenceTests
  **6/6**, UiPanelContractTests **1/1**, PanelRouteGateTests **22/22**,
  Plan204RecruitmentIntegrationTests **7/7**.
- `bin/ashfall-dev validate-json scavenging_tables.json`: **714/714 valid**.
- Headless `xvfb-run -a godot --headless -- --ui-layout-selftest`:
  **PASS, Failures: 0** (the two SceneBindingException ERROR lines from
  `VerifyUiControllerParity`'s bare `Activator.CreateInstance` on
  tscn-bound CombatDetailPanel/CraftingPanel pre-date this wave — caught-path
  noise, both panels tscn-bound since Ticket #125 / R1).
- No full suite (user did not type RUN FULL TESTS).

## 7. Commit

Built line-filtered via a **temporary index** (`GIT_INDEX_FILE` +
`read-tree HEAD` + `hash-object`/`update-index` + `commit-tree` +
`update-ref`): a concurrent integrator stream had `git add -A`'d 459 files,
including foreign uncommitted hunks inside `src/UI/CombatPanel.cs`
(T13/T14/T15/T16/T18/T20 combat wave), `src/Main.UiPanels.cs` (food overlay
selection, T16/T10 overlay feedback, dev-session), and
`src/UI/CombatDetailPanel.cs` (dead-field removal). Only this wave's lines
are in its commit; the foreign stream's staging is untouched for its own
sweep.
