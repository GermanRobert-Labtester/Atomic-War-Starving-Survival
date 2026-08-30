# Plan 06 — Narrative Depth Trilogy: Wishes, Echoes & the War Arc

> **Theme:** The emotional and long-arc narrative spine. Three tasks that turn already-built
> systems (`FinalWishSystem`, echo/flag infrastructure, and 6 fully-authored but **unwired**
> faction-war catalogs) into the game's deepest storytelling layer.
>
> **Key evidence:** `final_wishes.json` = **8 entries**; `echoes.json` = 23;
> `docs/narrative/NARRATIVE_NEEDS.md` documents 6 faction-war catalogs (22 chains / 45 stages /
> 93 choices / 33 broadcasts / 26 journal entries / 18 communiqués / 18 dialogue / 9 location
> overrides) that **no C# code loads today**.

---

## Task 6A — Final Wishes & Last Letters expansion (8 → 30)

**Goal:** Give the fully-live `FinalWishSystem` + terminal-prognosis pipeline enough content
that every elder death is a distinct authored moment, not a repeated card.

**Files:** `Assets/StreamingAssets/Data/final_wishes.json`, possibly
`narrative/survivor_letters_lost_kin.json` (existing family — extend it), `items.json` if any
wish produces a memento item. Read-only: `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs`,
`MemorialSystem.cs`.

**Substeps:**
1. Read `final_wishes.json` schema + `FinalWishSystem` consumer to extract exact field grammar (trigger traits, fulfillment quest shape, reward flags).
2. Read 2–3 narrative docs (`survivor_letters_lost_kin.json`) to lock voice: exhausted, restrained, no melodrama.
3. Draft a wish taxonomy: reconcile-with-estranged / return-an-object / carry-a-message / witness-a-place / forgive / withhold-forgiveness.
4. Author 8 reconciliation wishes (each with a named recipient who may already be dead — the discovery is the content).
5. Author 6 object-return wishes tied to real `item_*` ids (add 3–4 new memento items to `items.json`).
6. Author 4 place-witness wishes keyed to real `loc_*` ids — fulfillment requires an expedition leg.
7. Author 4 dark wishes (destroy the letter unread; let the debt die with me) with moral-branching hooks into `MoralBranchingSystem` flags.
8. Cross-check every `flag_`, `item_`, `loc_` id against the catalogs; no invented prefixes.
9. Run `--data-integrity-selftest` → 0 errors.
10. Run `ashfall-dialog-graph-lint` / `ashfall-narrative-continuity` over new flags → no orphan producers/consumers.
11. `dotnet test` full suite green.

**Next steps:** feeds Task 6C (war deaths generate wishes); memorial plaques (white space #18) can display fulfilled wishes.

---

## Task 6B — Echo & cassette found-audio expansion (23 → 40 echoes)

**Goal:** Expand the environmental-storytelling layer (echoes, cassettes, audio logs) so
exploration keeps paying narrative dividends into the late game.

**Files:** `Assets/StreamingAssets/Data/echoes.json`, `cassette_sets.json` (currently **4 sets**),
`audio_logs_expansion_05.json` (30 logs — check which loader consumes them and whether they're surfaced), read-only `PhantomMemoryEngine.cs`.

**Substeps:**
1. Verify how echoes surface (which system/panel reads `echoes.json`; confirm none are orphaned).
2. Audit the 30 `audio_logs_expansion_05.json` entries for live wiring; note gaps instead of duplicating.
3. Design echo geography: pin each new echo to a real `loc_*` with appropriate danger tier.
4. Author 6 pre-collapse echoes (ordinary life: a birthday message, a shift handover, a weather report that mentions "the drill").
5. Author 6 exchange-day echoes (fragmented, procedural, no gore — tone rules apply).
6. Author 5 post-collapse survivor echoes (other bunkers, now silent — foreshadow faction war).
7. Author 2 new cassette sets (each 3–5 tapes forming one serialized story, completable = small morale item or journal unlock).
8. Ensure echo discovery feeds `JournalCodex` entries where lore-significant.
9. Validate all ids; run data-integrity selftest.
10. Run narrative-continuity check across the 272 narrative docs for contradictions (dates, faction names).
11. Full `dotnet test` green.

**Next steps:** Plan 07 wires actual audio VO to the highest-value echoes; cassettes become barter goods (economy hook).

---

## Task 6C — Faction War arc activation (Days 480–600+)

**Goal:** Wire the six authored `faction_war_*.json` catalogs into a running
`FactionWarHostSession` + `FactionWarChainRunner` — the single largest ready-made content
activation in the repo (22 chains, 45 stages, 93 choices already written).

**Files:** new `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs` (or domain-appropriate
namespace), new `src/Host/FactionWarHostSession.cs`, `Main.*.cs` triad (Setup/Save/Flush),
`src/UI/` surface (reuse journal + radio + event modal patterns). Read `docs/narrative/NARRATIVE_NEEDS.md` **in full first** — it is the spec.

**Substeps:**
1. Read NARRATIVE_NEEDS.md §1–§4 completely; it defines the host-session shape, chain-runner semantics, and the location-override resolver contract.
2. Design the machine boolean grammar for `triggerCondition` (currently prose): `day_offset(stage)`, `visited(loc_)`, `flag_set(flag_)` — minimal closed grammar, documented in-code.
3. Implement `FactionWarChainRunner` in Core: per-chain current-stage tracking, `minDay` auto-advance, per-choice `leadsToStageId` fan-out, terminal-stage handling.
4. Wire `moraleDelta` choice application through the existing narrative-encounter morale path (NARRATIVE_NEEDS says reuse, don't rebuild).
5. Implement `LocationOverrideResolver`: day-windowed `pre_strike` / open-ended `post_strike` / `ambient_addendum` substitution for display only — base `locations.json` stays mechanical truth.
6. Add the `flag_<plaza>_struck`-style world-state flags on strike resolution so future player-agency can key off them (NARRATIVE_NEEDS §3 endgame note).
7. Build `FactionWarHostSession` on the `YearOfAshHostSession` pattern: load all six catalogs, expose day-gated queries (due broadcasts, unlocked journal entries, active overrides).
8. Register the full Setup/Save/Flush triad in `Main`; add the save section to `SaveSectionRegistry` (envelope via `SaveStoreHub` — no hand-rolled envelope, CI gate forbids it).
9. Surface in UI: radio terminal plays due broadcasts; journal unlocks entries; map/location panel shows override text.
10. Write xUnit tests: chain advance, branching fan-out (warn/loot/silent → distinct s2 stages → shared strike), day-gating, save round-trip, override resolver windows.
11. Add a `--faction-war-selftest` CLI verb following existing selftest conventions.
12. Full verification: `dotnet test`, `dotnet build` 0/0, data-integrity, narrative-continuity lint.

**Next steps:** NARRATIVE_NEEDS flags future work — preventable-strike player agency, rad/danger deltas on post-strike overrides, more chains. This task unlocks all of them. **Cross-tool QA rule applies** (new coupled system: runner + resolver + flags) — implementer ≠ reviewer.
