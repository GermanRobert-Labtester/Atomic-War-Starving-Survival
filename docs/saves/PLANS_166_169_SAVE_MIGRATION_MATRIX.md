# Plans 166–169 Save Migration Matrix

All new host stores use the existing Core `SaveStore<T>` path through `SaveStoreHub` and `SchemaVersionedEnvelope<T>`. Campaign save capture therefore receives the same checksummed section bytes as other current stores.

| Section | State | Missing section | Unknown IDs | Restore events |
|---|---|---|---|---|
| `research` | Existing `ResearchState`, extended with RP and blueprint progress | Empty wallet/progress | Existing research policy preserves unknown saved IDs; blueprint entries are copied | No completion or unlock event replay |
| `espionage` | `EspionageState` | Empty networks, intel, and missions | Unrecognized catalog missions are skipped safely by runtime lookup | No intel/capture/consequence event replay |
| `fluid_logistics` | `FluidLogisticsState` | Empty topology and zero totals | Unknown pipe definitions fall back to persisted edge values; invalid quantities are clamped on restore | No burst or distribution event replay |
| `procedural_narrative` | `ProceduralNarrativeSaveState` containing narrative metadata and `QuestRuntimeState` | Empty cooldowns, rivalries, and quest log | Unknown template IDs remain persisted in quest provenance; generation does not rerun on restore | No quest generation, expiry, or follow-up replay during restore |

The new sections are also present in `SaveSectionRegistry.SectionFileNames`, so legacy global-file migration and destructive test cleanup include them. Full campaign-envelope round-trip coverage for the new sections is still a required follow-up test pass after their consequence/UI adapters are connected.
