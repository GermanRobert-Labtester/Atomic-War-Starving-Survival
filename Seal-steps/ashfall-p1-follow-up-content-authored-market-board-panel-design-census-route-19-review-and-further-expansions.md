# ASHFALL — Authored Content, Panel Design, Census Review, and Third Expansion Wave

**Document class:** Third companion in the planning series (`ashfall-unblocked-integration-plans` → `ashfall-deep-dive-expansions` → this document). Four deliverables requested by the foreman 2026-09-18:

- **Part A** — P1 follow-up content **authored**: real `follow_up_signals` entries drafted against the live catalog identities (23 primary + 16 expansion frequencies pulled from `radio_distress_signals.json` and `radio_distress_signals_expansion.json`), each landing kind-classified per Part A of the deep-dive document's digest rules.
- **Part B** — Market board panel design in full: information hierarchy, layout, controller navigation, focus behavior, a11y contract.
- **Part C** — Census route #19 review: should merchant restock stay aggregate-only? A structured answer with the counter-position.
- **Part D** — A third wave of integrated-plan expansions (three more systems with identified headroom).

**Grounding:** signal identities, `outcome_type`s, trap/false-flag counts, and fragment structure come from the live catalog files pulled for this document. Authored rows below are **[drafted]** — they follow the sealed `follow_up_signals` schema (closed trigger grammar: `answered` / `rescue_success` / `rescue_failed` / `expired` / `trap_fallen_for`; self-contained payloads; no catalog-backed references) and are ready for validator pass and data-integrity walk.

---

# PART A — P1 Follow-Up Content, Authored

## A.1 Authoring rules recap (from the sealed contract + digest rules)

1. Triggers map to exactly-once mission transitions; payloads are self-contained (no cross-signal references — chains/cycles structurally impossible).
2. Max 2 follow-ups per parent signal; follow-up answers route the same +2 trust delta (no chain bonus).
3. `expired` follow-ups forbidden on no-consequence signals; trap-grammar triggers forbidden on genuine-never-hostile identities (validator rules from P1 Wave 1).
4. Kind classification lands at authoring time: every follow-up fires as `Progression` (echoes of resolution) or `Warning` (regret/lure/escalation) — never alert-class; they surface in the journal and the radio event line, and the rescue strip shows stage text.
5. Each follow-up carries its own `audio_cue` (Intercept-edge gated, dedupe keyed `distress:{id}:{cue}`).

## A.2 Family 1 — Aftermath chains (`rescue_success`, kind `Progression`)

**A.2.1 — `freq_distress_217_4` / Checkpoint Kilo (survivor_community, 12 survivors, armory cache)**

```json
{
  "follow_up_id": "fup_kilo_thanks",
  "parent": "freq_distress_217_4",
  "trigger": "rescue_success",
  "delay_days": 6,
  "audio_cue": "radio_distress_beacon",
  "source_name": "Checkpoint Kilo — Relocation Net",
  "message_fragments": [
    { "day": 1, "clarity": 0.7, "text": "...this is Corporal Maren, twelfth of twelve... the convoy took us in... sector 7 is clear of the filtration sickness...",
      "outcome_hint": "All twelve survived the relocation. The count is still exact." }
  ],
  "revealed_location": null,
  "revealed_items": ["field_dressing_kit"],
  "consequence": { "type": "standing_gain", "faction": "military", "amount": 2 }
}
```
*Kind: Progression. Design intent: closes the arc the player invested four trace days in; the exact-count verbal tic ("twelfth of twelve") ties the echo to the parent's authored voice. Small military standing gain routes through the canonical `FactionStanceEngine`, exactly-once.*

**A.2.2 — `freq_distress_401_9` / Injured Trader on Route 6 (survivor_community)**

```json
{ "follow_up_id": "fup_trader_repayment", "parent": "freq_distress_401_9",
  "trigger": "rescue_success", "delay_days": 8, "audio_cue": "radio_trade_chime",
  "source_name": "Route 6 — Trading Post Relay",
  "message_fragments": [
    { "day": 1, "clarity": 0.8, "text": "...the trader you pulled off Route 6 made it... left something at the crossing for the shelter that answered... tell them the road remembers...",
      "outcome_hint": "A gratitude cache waits at a location the trader's route passes." } ],
  "revealed_location": "route6_gratitude_cache",
  "revealed_items": ["medical_supplies", "fuel_can"], "consequence": null }
```
*Kind: Progression; single-resolution loot site (the AnomalyHazardSystem one-shot pattern). Feeds the acquisition digest under `scavenged` with source "gratitude cache" (census rule 3 flavor).*

**A.2.3 — `freq_distress_88_3` / Isolated Water Treatment Worker (survivor_community)**

```json
{ "follow_up_id": "fup_watertech_advisory", "parent": "freq_distress_88_3",
  "trigger": "rescue_success", "delay_days": 5, "audio_cue": "radio_distress_beacon",
  "source_name": "North Dam Control — Reopened",
  "message_fragments": [
    { "day": 1, "clarity": 0.75, "text": "...plant's running at half flow... your shelter's intake is downstream of ours now, so I'm flagging the turbidity spikes before you taste them... watch days nine and ten...",
      "outcome_hint": "A water contamination warning is coming on a known day — the rescued worker now watches your intake." } ],
  "revealed_location": null, "revealed_items": [],
  "consequence": { "type": "advisory_event", "event_id": "water_turbidity_spike", "day_offset": 2 } }
```
*Kind: Progression with a `Warning` child advisory. Design intent: rescue produces ongoing relationship, not just a one-time reward — the water-source management system (census route #14) receives a forecast it otherwise wouldn't have. This is the strongest "rescue pays forward" hook in the set.*

## A.3 Family 2 — Regret chains (`rescue_failed` / sender death, kind `Warning`)

**A.3.1 — `freq_distress_156_8` / Stranded Expedition Group (survivor_community)**

```json
{ "follow_up_id": "fup_expedition_finalwords", "parent": "freq_distress_156_8",
  "trigger": "rescue_failed", "delay_days": 4, "audio_cue": "radio_static_low",
  "source_name": "Automated Relay — Expedition Black Box",
  "message_fragments": [
    { "day": 1, "clarity": 0.5, "text": "...auto-relay: final log of expedition group... 'we heard footsteps on the third night and they were not rescue'... cache coordinates follow... make it count...",
      "outcome_hint": "The expedition's supplies remain where they fell — the group did not." } ],
  "revealed_location": "expedition_blackbox_cache", "revealed_items": ["expedition_supplies", "climbing_gear"],
  "consequence": { "type": "journal_entry", "entry_id": "regret_expedition_finalwords" } }
```
*Kind: Warning. Salvage-granted-exactly-once with rep 0 (the sealed sender-survival branch); the journal entry is the emotional payload. The follow-up does NOT add trust penalties — the failure already applied them; this is echo, not re-punishment (anti-double-count rule).*

**A.3.2 — `freq_distress_129_6` / Family Shelter Distress Call (survivor_community)**

```json
{ "follow_up_id": "fup_family_verdictinquiry", "parent": "freq_distress_129_6",
  "trigger": "rescue_failed", "delay_days": 10, "audio_cue": "radio_verdict_band",
  "source_name": "Verdict Field Inquiry — Channel Gray",
  "message_fragments": [
    { "day": 1, "clarity": 0.65, "text": "...Verdict inquiry, matter number four-forty-one: the family shelter on the east line. The band of silence lasted nine days. We are asking the frequency itself who listened...",
      "outcome_hint": "Verdict investigates unanswered deaths. Their questions have a way of finding answers." } ],
  "revealed_location": null, "revealed_items": [],
  "consequence": { "type": "quest_hook", "quest_id": "verdict_inquiry_441" } }
```
*Kind: Warning. The most narrative-forward row in the set: routes into the Verdict questline corpus (existing authored questline master — no new quest system). The inquiry quest fires regardless of whether the player answers it — "interesting even when the player fails," per the design language.*

## A.4 Family 3 — Lure escalation (`trap_fallen_for`, kind `Warning`; genuine signals never use this grammar)

**A.4.1 — `freq_distress_148_2` / Civilian Bunker 4-East (bait_trap, raiders, 4 authored traps incl. this)**

```json
{ "follow_up_id": "fup_bunker4_taunt", "parent": "freq_distress_148_2",
  "trigger": "trap_fallen_for", "delay_days": 3, "audio_cue": "radio_raider_laughter",
  "source_name": "Same Frequency — New Voice",
  "message_fragments": [
    { "day": 1, "clarity": 0.9, "text": "...'bunker 4-East' sends her regards... you walked right up to the door with a medkit in your hand... we'll leave the frequency warm for you, shelter-rat...",
      "outcome_hint": "The raiders know the shelter answered. The frequency stays live because it worked once." } ],
  "revealed_location": null, "revealed_items": [],
  "consequence": { "type": "standing_loss", "faction": "raiders_rival", "amount": 0 } }
```
*Kind: Warning. The trap-aftermath legitimacy path (already proven by the Wave 3 tests): falling for a trap has a *second* beat — the taunt. Consequence field is null-weight (standing loss already applied at trap resolution; the taunt is pure narrative pressure, anti-double-count).*

**A.4.2 — `freq_distress_278_3` / Raider Lure: Fuel Cache (bait_trap) — second attempt variant**

```json
{ "follow_up_id": "fup_fuellure_retry", "parent": "freq_distress_278_3",
  "trigger": "trap_fallen_for", "delay_days": 12, "audio_cue": "radio_static",
  "source_name": "Bunker 9-West — 'Survivors'",
  "message_fragments": [
    { "day": 1, "clarity": 0.55, "text": "...nine of us in bunker 9-west, fuel for the taking, no questions... same sector as before... we won't broadcast twice...",
      "outcome_hint": "The same trap pattern, re-baited. The details are near-identical to a call you have already answered." } ],
  "revealed_location": null, "revealed_items": [],
  "consequence": null }
```
*Kind: Warning. Design intent: the *teachable* trap — the outcome_hint deliberately flags the pattern-match to the parent ("same sector as before"). A player who reads hints learns; the trust ledger already prices the first fall. The near-identical phrasing is authored voice, not lazy duplication: it IS the tell.*

## A.5 Family 4 — Deadline echoes (`expired`, kind `Warning`; only on consequence-bearing signals)

**A.5.1 — `freq_distress_445_2` / Scavenger Kidnap Setup (bait_trap — expired echo, NOT trust-penalized)**

Per the sealed rule, trap-class expiry is excluded from penalties, and `expired` follow-ups are forbidden on no-consequence signals. For traps, expiry gets a *closure* echo, not a punishment:

```json
{ "follow_up_id": "fup_kidnap_fades", "parent": "freq_distress_445_2",
  "trigger": "expired", "delay_days": 2, "audio_cue": "radio_static",
  "source_name": "Band 6 — Carrier Drop",
  "message_fragments": [
    { "day": 1, "clarity": 0.3, "text": "...[carrier signal lost]... [frequency reassigned]...", "outcome_hint": "The frequency is dead air now. Whatever it was, it found no answer." } ],
  "revealed_location": null, "revealed_items": [], "consequence": null }
```
*Kind: Warning (atmospheric). Zero consequence, pure tonal closure — ignoring a lure should feel like a door closing, not a score settling.*

**A.5.2 — `freq_distress_203_1` / Repeating Emergency Beacon (narrative, consequence-bearing)**

```json
{ "follow_up_id": "fup_beacon_silence", "parent": "freq_distress_203_1",
  "trigger": "expired", "delay_days": 1, "audio_cue": "radio_silence_cue",
  "source_name": "—",
  "message_fragments": [
    { "day": 1, "clarity": 0.15, "text": "...[no carrier]...", "outcome_hint": "A repeating signal that stopped repeating. The silence itself is the message." } ],
  "revealed_location": null, "revealed_items": [],
  "consequence": { "type": "journal_entry", "entry_id": "expired_beacon_silence" } }
```
*Kind: Warning. The quietest possible consequence: the journal records that something went silent because nobody came. The ignore-consequences engine (sender_death/standing) already fired at deadline; this is its shadow.*

## A.6 Coverage summary and validator pass expectations

| Family | Entries | Triggers used | Kinds | Parents covered |
|---|---|---|---|---|
| Aftermath | 3 | rescue_success ×3 | Progression | 217_4, 401_9, 88_3 |
| Regret | 2 | rescue_failed ×2 | Warning | 156_8, 129_6 |
| Lure escalation | 2 | trap_fallen_for ×2 | Warning | 148_2, 278_3 |
| Deadline echo | 2 | expired ×2 | Warning | 445_2, 203_1 |
| **Total** | **9 drafted** (of ~29 planned; the remaining 20 follow the same four families against the expansion identities — Meridian Cold Store, Barge Olenka, School 14, Salt Mine Survey Team, Lighthouse Keeper, Almshouse, et al.) | 4/5 grammar values | — | 9/39 identities |

**Validator expectations (P1 Wave 1 rules):** zero errors — every drafted row passes (a) `expired`-on-consequence-bearing only, (b) trap grammar never on genuine identities, (c) text presence + clarity bounds, (d) max-2-children per parent, (e) self-contained payloads. The `quest_hook` consequence in A.3.2 requires one new validator rule: hooked quest ids must exist in `questline_master.json` (cross-catalog reference check — the one reference type the contract permits, since it points *outward* to an existing authored quest, never to another signal).

**Kind-classification landing (per the digest rules):** all 9 rows fire digest-class; the two `Progression` aftermath rows additionally feed the acquisitions digest when they carry items (A.2.2 under `scavenged — gratitude cache`); `Warning` rows never enter the alert concurrency window (they journal + strip only). This is exactly the "kind lands at authoring time" discipline Part A of the deep-dive document specified.

---

# PART B — Market Board Panel: Full Design

## B.1 Design goals and constraints

1. **Zero new state** — a pure read model over MarketSystem v2 (category indices, trade pressure, shocks, baselines) and the D.2 price-history window.
2. **Panel-open never advances state** (the C1 black-market invariant, tested).
3. **Information hierarchy: glance → inspect → verify.** Three depth levels, each opt-in.
4. **Controller-first navigation** (Godot focus system, `Control` nodes); mouse parity without duplication.
5. A11y contract: words + numbers, never color-only; three-band pressure language; focus visible and restored; no time-pressured interactions.

## B.2 Information hierarchy (three levels)

**Level 1 — The category table (glance, the default view).** One row per category (12, from `commodity_baselines.json` **[grounded]**), each row exactly four elements:

| Element | Content | Source |
|---|---|---|
| Name | category id rendered as title-case display name | catalog |
| Index vs. baseline | current multiplier in ‰ with a two-word band: "1080 — elevated" (bands: `below normal` <950, `normal` 950–1050, `elevated` 1050–1300, `scarcity` >1300 **[proposed]**) | MarketSystem index vs. baseline |
| Pressure | one word + direction: `rising` / `falling` / `easing` (easing = magnitude below decay threshold) | pressure sign + magnitude band |
| Shock flag | authored text: "blizzard scarcity — 2 days" or — | active shocks |

*Level 1 answers the only question a passing player has: "what is expensive right now, and is it getting worse?" Four elements, all textual, scannable in one screen without scrolling at 1280×800.*

**Level 2 — Category detail (inspect, one row selected).** Expanding/selecting a row reveals, in a fixed order:

1. **Explain rows** — `ExplainPrice` typed factors verbatim (Demand / Category / Shock), the machinery already built, now the player's tooltip.
2. **Trend line** — the 14-day history window as text: "950 → 980 → 1010 → 1080 — rising 3 days" (words + numbers, no chart widget).
3. **Scarcity band position** — "ceiling 1600‰; current 1080‰ — two-thirds toward scarcity" (grounded floor/ceiling per category).

**Level 3 — The rumor cross-reference (verify, one button).** A single action per category: "check radio chatter" — jumps focus to the RadioPanel market-rumor band filtered to this category (the band bridge already projects shock start/expiry). This closes the rumor→decision loop: hear it, check it, act on it.

## B.3 Layout

```
┌──────────────────────────────────────────────────────────────────┐
│ MARKET BOARD                                    [day 214] [close]│
├──────────────────────────────────────────────────────────────────┤
│ ▶ Food           1080 — elevated    rising   blizzard — 2 days   │ ← Level 1 (12 rows)
│   Water           960 — normal      easing   —                    │
│   Medical        1420 — scarcity    rising   outbreak — 5 days   │
│   Fuel            990 — normal      falling  —                    │
│   Weapons        1150 — elevated    rising   —                    │
│   Tools          1000 — normal      easing   —                    │
│   Materials       870 — below normal easing   —                    │
│   ...                                                            │
├──────────────────────────────────────────────────────────────────┤
│ [detail pane — Level 2 for the focused row]                      │
│ FOOD — explains: Demand 1040 · Category +120 · Shock +80         │
│ Trend: 950 → 980 → 1010 → 1080 — rising 3 days                   │
│ Band: ceiling 2000‰ · current 1080‰ — mid-band                   │
│                                        [ check radio chatter ▸ ] │
└──────────────────────────────────────────────────────────────────┘
```

Structure: a top-level `Control` panel; Level 1 is an `ItemList`-style focusable column; Level 2 is a detail pane bound to the focused row (live-updating on focus change, not on select — *focus is inspect, select is act*, which keeps the controller flow fast); Level 3 is one button in the pane. No tabs, no paging — 12 rows fit one screen; the pane is the only scrollable region if a category has many explain rows.

## B.4 Controller navigation (Godot focus system)

- **D-pad / left stick up-down:** moves focus through the 12 category rows. Focus change updates the detail pane immediately (inspect-on-focus).
- **A / Enter (select):** no-op on the row itself (rows are not buttons — they are focus targets; this prevents accidental actions on a read-only panel) — OR, per accessibility preference, select pins the detail pane to a row so focus can travel without the pane changing (a "pin" toggle; default off).
- **X / square — "check radio chatter":** the single action button in the pane; jumps to RadioPanel rumor band with category filter set, focus placed on the band item; Back returns focus to the exact row the player left (focus restoration contract, tested).
- **L1/R1 (bumpers):** jump focus to first row with an active shock / first scarcity-band row — the two "something is wrong" fast paths.
- **Y / triangle — guidance line:** one-line help: "Browse categories. Details show on focus. Chatter cross-checks rumors." (repo guidance pattern, F2-toggled globally).
- **Mouse parity:** hover = focus, click = pin toggle, click button = action. No drag, no multi-select, no context menu — the panel is read-only by contract.
- **Focus visuals:** a visible focus frame (not color-only — a bracket/underline affordance), 2px minimum, high contrast; the detail pane header repeats the focused row's name so focus is never ambiguous when the pane scrolls.

## B.5 A11y and state contracts (testable)

- Every Level 1 element readable as text by the screen-reader path (name, band, pressure, shock as one spoken line).
- Panel open/close fires zero ticks: `MarketBoardPanel` reads only captured state; the panel-open-never-advances test (modeled on the C1 black-market test) asserts index/pressure/shock byte-identical before and after open-refresh-close cycles.
- Restore-never-replays: history window rendering after save/load shows the same 14 lines (fingerprint pin).
- All bands are authored words with numeric anchors ("1080 — elevated"), never color or icon alone.
- Kind classification: the panel's data is `WorldState` (P3 Wave 1) — the market board never generates alert-class events; shocks are visible here but alarm elsewhere only per existing shock events.

## B.6 Panel gate checklist

`--panel-bind-lifecycle-selftest` (open/close/focus-restore), `--ui-a11y-selftest`, snapshot at 1280×800 (`market_board_default`), golden-day pins for three authored market states (calm, single-shock, scarcity-ceiling), controller navigation test via input-action simulation (up/down focus traversal, pin toggle, chatter jump + back-focus-restoration).

---

# PART C — Census Review: Should Route #19 (Merchant Restock) Stay Aggregate-Only?

**Short answer: yes — aggregate-only stands, but with one amendment.** Here is the structured reasoning, including where the counter-position is genuinely strong.

## C.1 The case FOR aggregate-only (the original recommendation)

1. **World-side state, player-side tone.** Restock is not a player action; the player did nothing to cause it. The census's kind discipline says events should track *decisions and their consequences*. A fully itemized restock list is inventory telemetry, not narrative — it belongs in the merchant panel (where the player can already inspect stock), not the daily briefing.
2. **Briefing budget.** The digest line already summarizes player-relevant gains across ~12 source classes. A 15-row restock manifest would dominate the briefing and collide with the P3 Wave 2 concurrency cap's spirit even if digest-class events bypass the alert window.
3. **Information asymmetry is content.** The player knowing *that* shelves changed but not *what* is exactly the texture a scarcity economy wants: you walk to the market to find out. Itemized restock in the briefing lets the player optimize trips without going to the world — the same reasoning that keeps discovery physical (Intercept-edge, travel) elsewhere in the design.
4. **The C1 invariant.** Restock state must never be advanced or exposed by UI concerns; the event is day-owner-timed only. An aggregate line is trivially compliant; itemization invites the panel to "helpfully" pre-render it.

## C.2 The case AGAINST (the counter-position, taken seriously)

1. **Shock-depth legibility.** Once D.2's shock-driven stock depth lands, "the merchant restocked" stops telling the whole story: a blizzard should make the *restock itself* feel different — piles of food, no tools. An aggregate line hides the one visible consequence of the world reacting to its own crisis.
2. **Route #19 is the player's only restock signal.** Miss the line and you miss the entire restock cycle for that cadence period (Plan 147 day-gating). Players who don't open the merchant panel regularly have no other channel.
3. **Planning value.** A player deciding between an expedition and a market trip has a real decision that itemized (or at least category-level) restock info would inform.

## C.3 The verdict: aggregate + one authored exception

Keep the aggregate line as the default, and add a single authored **category-level variant** for shock-active restock days only:

> Default: *"The merchant's shelves were restocked overnight."*
> Shock-day variant: *"The merchant restocked overnight — heavy on food, light on everything else."* (categories named from the active shock, capped at two category mentions, authored phrasing, no counts, no item lists)

**Why this resolves the counter-position without conceding it:**

- It answers C.2.1 (shock-depth becomes *felt* in the briefing) with two words of category flavor, not a manifest.
- It keeps C.1.3's asymmetry: you know the *shape* of the restock, not the contents; the trip is still worth taking.
- It is deterministic and cheap: the variant is a pure function of (active shocks at restock time × shock-depth rule) — no new state, one golden-day pin per variant.
- Counts and item ids remain merchant-panel-only, preserving the briefing budget and the C1 invariant.

**Matrix update:** census row #19 changes from `EMIT-ADD (aggregate)` to `EMIT-ADD (aggregate + shock-day category variant, ≤2 categories, no counts)` — one additional golden-day test, one additional wording pin. The D.2 closeout and the P5 memo both reference this amendment so the three packages stay coherent (P5 orders the set, D.2 scales the depth, #19's variant reports the shape).

**What would change my mind (recorded for the foreman):** if playtesting shows players routinely missing restock windows entirely (C.2.2), the next escalation is a *count-level* variant ("restocked: 3 categories"), never itemization. That decision stays with the foreman; it is one authored line away.

---

# PART D — Third Wave of Integrated-Plan Expansions

Three more sealed systems with headroom their own closeouts and the ledger's deferred notes name. Same package discipline as Parts D/F of the deep-dive document: bounded additions along existing seams, no reopenings.

## D.1 `PLAN-176-EXPANSION-ANOMALY-PREDICTION-LAYER` (extends Plan 176 — anomaly hazards)

**Current state (integrated, green):** deterministic moving-hazard authority — day-keyed wandered storm fronts, wind-drift bands, 0.01km-quantized positions, capped additive radiation, bounded wildlife modifiers, three-state detection (Undetected/Signature/Classified), one-shot approach warnings, loot-site single resolution; cross-system consumers (expedition dose, detection devices, wildlife avoidance).

**The headroom:** detection is *reactive* — the player learns an anomaly exists when the approach warning fires. The catalog has the data for *prediction*: wind-drift bands plus the day-keyed wander pattern mean a classified storm's tomorrow-position is computable. Nothing exposes it. The result is that detection capability tiers (the canonical devices) pay off identically regardless of how good the device is beyond the classification gate.

**Expansion design (two bounded additions):**
1. **Instrument forecast seam.** A pure Core function: given a Classified anomaly + current wind band, emit a one-day-ahead position forecast with authored uncertainty text ("storm front drifting east-northeast; tomorrow's track crosses sector 7" / "track uncertain — the band is shifting"). Device tier caps the forecast horizon: basic = tomorrow's sector only; advanced = two days + drift confidence words. Zero new state — the forecast is derived on demand from persisted anomaly state; determinism preserved (pure function, no RNG).
2. **Expedition route preflight consumption.** The existing `RescueDispatchPreflight`/expedition preflight pattern gains one advisory row: if a forecast track intersects a planned route, the preflight names the sector and the anomaly class (words, `Warning` kind, digest-adjacent not alert). No route is ever blocked — the forecast informs, the player decides. This is the "meaningful decision over stat increase" criterion in miniature: a better instrument buys *foresight*, not safety.
**Package shape:** one wave (pure functions + preflight row + ~9-case suite incl. determinism of forecast across save/restore, device-tier horizon caps, uncertainty wording bands); UI is one row on the existing hazard/ expedition screens — no new panel.

## D.2 `PLAN-147-EXPANSION-MERCHANT-MEMORY` (extends Plan 147 — merchant cadence, complements P5)

**Current state (integrated):** day-gated restock cadence; P5 (pending) adds priority ordering; D.2-of-deep-dive adds shock depth.

**The headroom:** merchants have no memory of the player. Every visit is a first visit. The economy tracks *categories* (pressure, shocks) but not *relationships* — the black market has heat/trust, and the legitimate market has nothing, which is backwards for a game about a small post-war trading community.

**Expansion design (one bounded seam):**
1. **Per-merchant trade history ledger.** Bounded rolling window (last 14 trading days **[proposed]**, per merchant, persisted in the existing economy save section — frozen-shape additive field, neutral default on old saves): categories bought/sold, volumes, and days-since-last-visit.
2. **Three authored relationship behaviors, all read-only decorations on existing pricing/stock authorities:** (a) *regular's courtesy* — a merchant the player trades with consistently offers a wider restock set next cycle (one extra row eligible for P5's priority sort — it feeds the sort, never bypasses it); (b) *category familiarity* — a merchant the player repeatedly buys medical goods from biases future restock depth toward medical within the existing bounds (composes with, never overrides, shock depth); (c) *absence drift* — 20+ days without a visit and the relationship decays one band (four bands: stranger / recognized / regular / trusted — words, shown in the trade screen header as one line).
**Design intent:** the market becomes a *place with people* rather than a vending machine, using only bounded decoration on sealed authorities. Prices stay MarketSystem's; stock sets stay P5's; depth stays D.2's; memory only shapes eligibility and one header line.
**Package shape:** one wave (ledger + three rules + ~10-case suite: window boundedness, decay, composition-order tests against both P5 and shock-depth in both orderings, save/restore fingerprint, panel-open never-advances).

## D.3 `DISTRESS-AUTHENTICITY-EXPANSION-SKILL-FEEDBACK` (extends the rescue runtime's authenticity system)

**Current state (integrated, green):** `SignalAuthenticityEvaluator` — skill-driven (signal-ear +15 / watchful +10 / cold-analysis +8 vs base 35 / difficulty 65), dedicated RNG sub-stream, genuine-never-hostile invariant, first-result-persisted anti-reroll.

**The headroom:** the evaluation is a black box with a verdict. The skill math is beautiful and entirely invisible: a player with three skilled listeners gets the same one-line result as a lucky roll, and the *first result is persisted* — meaning a player who improves their survivors' skills never feels the improvement on any signal already evaluated. The anti-reroll guard is correct (it must stay); what's missing is any expression of *why* the verdict was reached and *how close* it was.

**Expansion design (two bounded additions, both read-only):**
1. **Verdict reasoning line.** Alongside the persisted verdict, persist a bounded reasoning trace (already computed during evaluation): contributing skills + roll margin, rendered as one authored line: "Maren's ear caught the cadence — the pauses were wrong. (confident)" with a three-word confidence band (`uncertain` / `leaning` / `confident` **[proposed]**) derived from the roll margin. Persisted once with the verdict (same anti-reroll field group, one additive column — frozen-shape additive, neutral default).
2. **Skill-growth future value.** One line in the skill/certification UI (existing screens): when a survivor gains a signal-relevant skill, if any *undiscovered, unevaluated* signals exist, show "new ears on the radio band" (authored, `Progression` kind, once per skill gain, deduped). This makes skill investment *anticipatable* without ever re-rolling evaluated signals — the anti-reroll contract is preserved byte-identically; only the player's sense of future agency changes.
**Package shape:** one wave (additive persisted trace + two read surfaces + ~8-case suite: trace persisted-not-recomputed, confidence bands, once-per-gain dedupe, a11y words-only, migration neutrality).

---

# Closing Note

This third document delivers: **9 authored follow-up rows** against live catalog identities (4/5 trigger grammar values, kind-classified at authoring time), the **full market board design** (three-level hierarchy, layout, controller map, a11y and state contracts), a **structured verdict on census route #19** (aggregate stands, plus a shock-day category variant with a recorded escalation path), and **three new integrated-plan expansions** (anomaly prediction, merchant memory, authenticity feedback — 3 waves, ~27 test cases). Combined with its two parents, the series now covers 11 packages, 24 waves, and every named deferred item in the integration ledger that has any unblocking path.

**Recommended next claims:** P2 Wave 1 (memos signed, fully specified) remains the fastest green; P1 Wave 1 can now run its validator pass against the 9 drafted rows immediately; the market board (deep-dive D.2) and this document's Part B merge into a single D.2 Wave 2 claim.