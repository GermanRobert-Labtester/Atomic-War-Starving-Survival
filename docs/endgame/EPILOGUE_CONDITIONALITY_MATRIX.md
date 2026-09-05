# Epilogue Conditionality & Reachability Matrix

**Document ID:** `docs/endgame/EPILOGUE_CONDITIONALITY_MATRIX.md`
**Classification:** ALWAYS (Default Presentation Sequence) with Semantic Receptivity

---

## 1. Slide Conditionality Classification

Per Section 28 of the Plan 96 specification, each slide in the catalog is classified according to its runtime reachability:

| Order | Title | Conditionality Status | Trigger / Presentation Context |
|---:|---|---|---|
| 0 | `Opening` | **ALWAYS** | Renders in every campaign closing as the historical prologue. |
| 1 | `After the Flash` | **ALWAYS** | Renders in every campaign to commemorate the nuclear exchange. |
| 2 | `The Bunker` | **ALWAYS** | Renders in every campaign as the initial shelter redoubt. |
| 3 | `First Winter` | **ALWAYS** | Renders in every campaign to represent the nuclear winter survival phase. |
| 4 | `Water and Heat` | **ALWAYS** | Renders in every campaign; binds to water/power infrastructure state. |
| 5 | `Survivors` | **ALWAYS** | Renders in every campaign; binds to living dweller roster. |
| 6 | `Empty Bunks` | **ALWAYS** | Renders in every campaign; binds to deceased casualties / wall carvings. |
| 7 | `The Factions` | **ALWAYS** | Renders in every campaign; highlights dominant regional faction standings. |
| 8 | `Lines on the Map` | **ALWAYS** | Renders in every campaign; highlights completed expedition destinations. |
| 9 | `Voices in Static` | **ALWAYS** | Renders in every campaign; reflects frequency tuning & radio intercepts. |
| 10 | `The Verdict` | **ALWAYS** | Renders in every campaign; presents Verdict tribunal evidence findings. |
| 11 | `The Witnesses` | **ALWAYS** | Renders in every campaign; presents Muster depositions & confessions. |
| 12 | `Restored Relics` | **ALWAYS** | Renders in every campaign; reflects restored archive artifacts. |
| 13 | `What We Chose` | **ALWAYS** | Renders in every campaign; summarizes moral leaning (Mercy, Iron, Listener). |
| 14 | `The Muster` | **ALWAYS** | Renders in every campaign; presents coalition gathering or lack thereof. |
| 15 | `The Resolution` | **ALWAYS** | Renders in every campaign; holds the Plan 89 authoritative prose. |
| 16 | `The Census` | **ALWAYS** | Renders in every campaign; displays final dweller count & days survived. |
| 17 | `What Remains` | **ALWAYS** | Renders in every campaign; portrays physical shelter & valley aftermath. |
| 18 | `After Us` | **ALWAYS** | Renders in every campaign; reflects generational legacy & future thaw. |
| 19 | `Final Word` | **ALWAYS** | Renders in every campaign as the final fade-out reflection. |

---

## 2. Reachability & Dead Slide Analysis

- **Total Catalog Slides:** 20
- **Always Active in Default Sequence:** 20 (100% reachability)
- **Dead / Unreachable Slides:** 0
- **Conclusion:** There are zero orphaned or dead slide records in `epilogue_chronicle.json`. Every authored entry is loaded, validated, and sequentially composed into the final chronicle.
