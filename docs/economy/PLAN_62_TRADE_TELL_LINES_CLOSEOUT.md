# Plan 62 — Trade Tell Lines Expansion: 4 Trust Bands → 60 Posture Tells
**Closeout Report**
**Date:** 2026-09-03
**Status:** COMPLETE — ALL GATES PASSING

---

## 1. Executive Overview

Plan 62 delivers **60 terse trader-tell lines (15 per trust band)** into [`Assets/StreamingAssets/Data/trade_tell_lines.json`](../../Assets/StreamingAssets/Data/trade_tell_lines.json).

These lines provide the human observation layer for the negotiation table across the four trust bands:
- **Hostile**: The trader expects exploitation, violence, deceit, or wasted time (15 tells).
- **Wary**: The trader is willing to remain in the conversation but is actively measuring risk (15 tells).
- **Neutral**: The trader treats the exchange as business and waits for the offer to justify itself (15 tells).
- **Warm**: The trader trusts the player enough to relax some defensive habits, without becoming cheerful or sentimental (15 tells).

In accordance with Plan 62's hard constraints, this is a **pure data and narrative-authoring pass**: zero C# Core code changes, zero RNG changes, zero save changes, and 100% preservation of all existing contracts.

---

## 2. Runtime Contract Audit

| Contract Property | Value / Behavior |
|---|---|
| **JSON Target File** | [`Assets/StreamingAssets/Data/trade_tell_lines.json`](../../Assets/StreamingAssets/Data/trade_tell_lines.json) |
| **Catalog Schema** | Root object: `trust_bands` array and `tells` dictionary keyed by stance (`trade`, `hostile_raid`, `rob`, `refuse`, `share_intel`) |
| **Trust Bands** | `hostile` ([-100, -40]), `wary` ([-39, 0]), `neutral` ([1, 40]), `warm` ([41, 100]) |
| **Selection Algorithm** | Deterministic uniform random index via `ISeededRng.Next(0, pool.Count)` in `TradeTellEngine.TrySelectTell` |
| **Engine Consumer** | `TradeScreenPresenter.Recalculate()` via `ITradeTellProvider` |
| **Order Sensitivity** | Frozen in authored order; array ordering is index-sensitive under deterministic seeded PRNG |
| **Character Limits** | All authored lines: 42–68 characters (well within runtime requirement of 20–140 chars) |
| **Word Counts** | All authored lines: 7–13 words (target: 5–15 words) |

---

## 3. Authored Tell Lines Matrix

### Hostile (15 Posture Tells)
1. *They keep their goods close and leave little space between you.*
2. *One hand stays beneath the table while they study the offer.*
3. *Their attention keeps drifting from your hands to the doorway.*
4. *They count their goods twice and yours a third time.*
5. *Nothing leaves their side of the table before the terms settle.*
6. *Their shoulders stay high, as if expecting the trade to turn.*
7. *They barely look at the goods before looking back at you.*
8. *The scale is pulled closer to them after every adjustment.*
9. *They stand instead of sitting, coat still fastened for departure.*
10. *Their jaw tightens whenever your hand moves toward the pile.*
11. *They push rejected goods back without bothering to straighten them.*
12. *The trader keeps the exit clear behind their own shoulder.*
13. *Their gaze settles on your pack longer than on the offer.*
14. *They wait through the silence without giving you anything back.*
15. *A thumb rests near the holster, never quite touching it.*

### Wary (15 Posture Tells)
1. *They turn each item over before placing it back down.*
2. *Their body stays half-turned, though they have not stepped away.*
3. *They watch the scale settle before answering with a small nod.*
4. *Their fingers stop drumming only when the offer changes.*
5. *They separate the better goods from the rest without explanation.*
6. *The trader studies your face after checking every marked price.*
7. *They lean closer to inspect, then return to the same distance.*
8. *Their hands hover over the goods without claiming them yet.*
9. *They recount one bundle quietly before considering the next.*
10. *The offer earns attention, but not enough to loosen their posture.*
11. *They glance toward their companions before moving anything forward.*
12. *A slow breath leaves them when the terms improve.*
13. *They keep one elbow on the table and both feet ready.*
14. *Their gaze softens briefly, then returns to the scale.*
15. *They wait for you to move first after each adjustment.*

### Neutral (15 Posture Tells)
1. *They set both hands on the table and wait.*
2. *Each item gets the same brief inspection before they look up.*
3. *The trader checks the scale once and leaves it alone.*
4. *Their expression barely changes as the piles shift between you.*
5. *They keep an even distance from both sides of the offer.*
6. *A steady gaze follows the numbers without lingering on your face.*
7. *They straighten one bundle while waiting for your next move.*
8. *Nothing in their posture hurries the exchange or delays it.*
9. *They listen without interrupting, then look back to the goods.*
10. *The trader rests their hands beside the scale, palms down.*
11. *They make room for another item without encouraging you to add it.*
12. *Their attention stays on the trade rather than the room around it.*
13. *They acknowledge the adjustment with a short, unreadable nod.*
14. *The goods are handled carefully, but without any visible attachment.*
15. *They wait in practiced silence for the offer to settle.*

### Warm (15 Posture Tells)
1. *Their shoulders loosen before they look over the new offer.*
2. *They leave the better goods within easy reach of your side.*
3. *The trader meets your eyes without checking the doorway afterward.*
4. *They stop recounting their pile once the terms begin to settle.*
5. *One corner of their mouth lifts, then the expression is gone.*
6. *They lean closer to the table instead of guarding their distance.*
7. *Their hands stay open while you rearrange the offer.*
8. *They nod once before checking the final weight on the scale.*
9. *The trader turns an item toward you to show its better side.*
10. *They let a silence pass without treating it as a challenge.*
11. *Their coat hangs open now, hands occupied with the goods.*
12. *They slide one bundle forward before you finish arranging yours.*
13. *Their attention stays with you even when someone passes behind them.*
14. *They inspect your goods quickly, trusting the familiar parts.*
15. *The final adjustment gets a tired nod that almost feels familiar.*

---

## 4. Verification & Gate Evidence

| Verification Gate | Command | Result | Telemetry / Status |
|---|---|---|---|
| **Corpus Test Suite** | `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~TradeTellCorpusTests` | **PASS (exit 0)** | 7/7 passed: 20 pools, band math, seed determinism, tone lint |
| **All Trade Unit Tests** | `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~Trade` | **PASS (exit 0)** | 147/147 passed (TradeScreenPresenter, Scenarios, Tells) |
| **DocLink Validation Gate** | `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~DocLinkValidationGateTests` | **PASS (exit 0)** | 2/2 passed; no machine-specific absolute URIs |
| **Host Data Integrity** | `godot --headless --path . -- --data-integrity-selftest` | **PASS (exit 0)** | 0 findings across 208 catalogs |
| **Host Economy Self-Test** | `godot --headless --path . -- --economy-selftest` | **PASS (exit 0)** | 11/11 passed |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | **PASS (exit 0)** | CI gate PASS; `trade_tell_lines.json` verified consumed |
| **Core Build** | `dotnet build Ashfall.csproj` | **PASS (exit 0)** | 0 warnings, 0 errors |
| **Core Test Suite** | `dotnet test Ashfall.Core.Tests` | **PASS (exit 0)** | 6,617 passed, 0 failed, 0 skipped |
