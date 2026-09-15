# Plans 150–153 Narrative Activation Seal Log

Date: 2026-09-12  
Package: `PLANS-150-153-NARRATIVE-ACTIVATION`

## Premise (validated)

| Plan | Before | Broken link |
|---|---|---|
| 150 Personal Letters | DEAD | No `DiscoverPersonalLetterRecords`; shelter inspect never unlocked letters |
| 151 Abyssal Anomalies | DEAD | No `DiscoverAbyssalAnomalyRecords`; map/room producers never called |
| 152 Black Projects | PARTIAL | UI/save live; producer IDs deep-lore-only → `DiscoverLocation` rejected; Log-to-Journal theater |
| 153 Fringe Cults | PARTIAL | Live unlocks existed but many were panel-open theater |

## Seal applied

1. **Core contracts** — `PersonalLetterRuntimeContract`, `AbyssalAnomaliesRuntimeContract` in `NarrativeDiscoveryCatalog.cs`.
2. **Host discovery** — `DiscoverPersonalLetterRecords` / `DiscoverAbyssalAnomalyRecords` in `Main.Narrative.cs`.
3. **Real inspect paths** — `HandleShelterRoomSelected` unlocks letters/abyssal/fringe/paper/bone; `OpenMapDetailPanel` unlocks location producers + BP inspect; archive desk keeps government_bunker drawer path.
4. **De-theater 153** — removed fringe dumps from `OpenCraftingPanel`, `OpenRadioPanel`, `OpenShelterPanel`, silent-foundry bind.
5. **Plan 152 bridge** — register 9 DefaultProducerMap sites into `ExpeditionDefinitionRegistry` + host Definitions from deep-lore travel data; bind `OnLocationDiscovered` + arrival (`Looting` tick → `DiscoverLocation`); map-detail `NotifyBlackProjectsProducerInspected`; Log-to-Journal refuses undiscovered records.

## Non-goals (intentionally untouched)

- Expanding deferred 14/30 BP records
- ContentUtilizationScanner honesty pass
- DEBT-167/168, Plans 170–199
- Microfluidic patient picker / coated-part PowerGrid (146–149 MED leftovers)

## Verification

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/PersonalLetterRuntimeActivationTests.cs` | 4/4 pass |
| `bash scripts/run_test.sh Ashfall.Core.Tests/AbyssalAnomaliesRuntimeActivationTests.cs` | 6/6 pass |
| `bash scripts/run_test.sh Ashfall.Core.Tests/FringeCultRuntimeActivationTests.cs` | 7/7 pass |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/BlackProjectsArchiveTests.cs` | 23/23 pass |
| `dotnet build Ashfall.csproj --nologo -v q` | 0/0 |

**Total focused:** 40/40 pass. Host build clean.

## Status

`SEALED` for live discovery paths on 150–153. Deferred BP 14/30 and ContentUtilization honesty remain out of scope.
