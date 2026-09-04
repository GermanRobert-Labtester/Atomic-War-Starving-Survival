# ASHFALL — Distress Signal Integration Contract

**Wave:** Flagship Distress Signal Production Hardening (Tasks 21–25)
**Status:** implemented + verified (see §Verification)
**Authority:** `Assets/StreamingAssets/Data/radio_distress_signals.json` (signals),
`Assets/StreamingAssets/Data/moral_choice_quests_distress.json` (rescue choices)

---

## 1. Signal identity (INV-01)

The canonical identity of a distress signal is its `frequency_id` (e.g.
`freq_distress_88_3`). Frequency (`frequency_mhz`) is a lookup property, never a
persistence key. Campaign progression lives on `ActiveDistressSignal`, keyed by
`frequency_id`.

## 2. Classification (INV-04, INV-08)

Genuineness is data-driven, never inferred from message text:

| Property | Rule (`DistressSignalDefinition`) |
|---|---|
| `IsGenuineRescue` | `authenticity == "genuine"` OR `outcome_type` ∈ {survivor_isolated, survivor_medic, survivor_drift} |
| `IsTrapOrDeception` | `authenticity` ∈ {trap, false_flag} OR `outcome_type` ∈ {bait_trap, false_flag} |
| `IsAutomated` | `authenticity == "stale"` OR `outcome_type` ∈ {knowledge, encrypted} OR source-name marker |

A signal enters the rescue moral-choice path only when `IsGenuineRescue &&
!IsTrapOrDeception && !IsAutomated && MoralChoiceId != ""`. Traps and false
flags are structurally excluded (INV-08); the six rescue choices are authored in
`moral_choice_quests_distress.json` with `quest_moral_distress_*` ids.

## 3. Lifecycle and single outcome path (INV-05/06/07, T24.4)

```
Listen (tune)  → FindSignalAtFrequency → Intercept        (once, status guard)
clarity ≥ 0.25 / MarkTriangulated → TryTriggerMoralChoice  (idempotent; refused once resolved)
player answer   → ResolveMoralChoice(0|1)
                    0 = rescue intent → moral delta, signal marked dispatched;
                        standing NOT paid yet (T24.5 — no reward for accepting)
                    1 = ignore (terminal) → moral delta + authored ignore penalty
                        (ignore_standing_delta; legacy rule −15/−10 as fallback)
expedition return at the revealed location → CompleteRescue
                    refuses ignored/expired/undispatched signals;
                    pays ReputationDelta to sender_faction_id exactly once
```

`FactionWarSystem.ModifyStanding` is the only standing mutation path
(±100 canonical clamp). Radio code never mutates standing directly.

## 4. Persistence (§12)

Markers (`isMoralChoiceAvailable`, `moralChoiceResolutionIndex`, `isIgnored`)
ride the radio save section (`DistressSignalSaveEntry`, codec **V3**). A pre-wave
V2 payload is verified against the frozen V2 field set (`RadioSaveStateFrozenV2`)
and migrated in memory with marker defaults — restore never replays
consequences (§12.1): `RestoreState` reconstructs state only.

## 5. Host wiring (Godot)

| Concern | Seam |
|---|---|
| Offer | `RadioHostSession` distress events → `Main.OfferDistressMoralChoice` (journal line; Core guard makes re-tunes no-ops) |
| Answer | `RadioPanel` DISTRESS DECISION section → `OnDistressMoralChoice` → `Main.ResolveDistressMoralChoice` (looks up the quest def in the merged moral catalog) |
| Complete | `Main` expedition `OnExpeditionCompleted`: destination `locationId` == dispatched signal's `revealed_location` → `CompleteRescue` |
| Production catalog | `Main.SetupMoralChoice` merges `moral_choice_quests_distress.json` via `MoralChoiceCatalogLoader.LoadFrom` |

Known heuristic: the completion hook attributes a returning expedition to a
dispatched signal purely by destination match. A regular scavenge returning from
the same location while a rescue is in flight completes the rescue early.
Full party-tracking is deferred (documented, not silently wrong: the rescue
still fires exactly once and only for a dispatched signal).

Deferred with host wiring: material reward granting on rescue (T25.10
inventory deltas). `revealed_items` remain catalog data for the loot tables;
no parallel reward store was introduced.

## 6. Validation (INV-12, T23.15/T24.15)

`CatalogIntegrityValidator` treats `moral_choice_id`, `sender_faction_id`,
`deceptive_faction_id`, and `reputation_faction_id` as strict reference keys —
every value must resolve against the global registry (quest ids from the moral
catalogs; faction ids from `faction_lore.json`). Gated by
`--data-integrity-selftest` and `DistressSignalFactionTests`.

## 7. Verification evidence (2026-09-04)

```
dotnet build Ashfall.Core.Tests  — 0 errors / 0 new warnings
dotnet test                      — all green incl. 37 distress/radio-save tests
                                   (V2→V3 migration, ignore-resurrect refusal,
                                    undispatched refusal, 3-run seed-42 smoke)
dotnet build Ashfall.csproj      — 0 errors / 0 warnings (host wiring)
--data-integrity-selftest        — PASS, 0 errors across 221 catalogs
--bridge-selftest                — PASS (exit 0)
```
