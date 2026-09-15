# Shelter maintenance matrix

Plan 52 authority inventory. The starting-level system owns air state; player
inventory and shared research remain the campaign authorities.

| Component | Owner | Degradation | Warning | Action | Cost | Gate | Persisted state |
|---|---|---|---|---|---|---|---|
| HEPA air filter stack | `StartingLevelSystem` | 5 integrity/day in clear powered baseline; weather/power modifiers stay in the owner | `<50%` integrity or radon `>30` | service or replace | service: `scrap_mechanical ×1`; replace: `item_air_filter_hepa ×1` | replacement requires `knowledge_air_filtration` | integrity, air quality, radon, warning latch, legacy counters |
| Filtration power | power/grid authority | existing power availability modifier | projected through air warning | restore power through grid authority | existing grid/resource rules | existing room/power rules | existing power save section |
| Thermal/room shielding | material shielding / shelter systems | existing room and weather authorities | existing shelter projections | existing shielding/thermal actions | existing system costs | existing research/action gates | existing shelter sections |

The new maintenance path is `PreviewMaintainAirFilter` →
`MaintainAirFilter`. It validates research and inventory first, then consumes
one canonical item through `IPlayerInventoryPort.TryConsumeBill`, mutates the
air owner, and emits a typed `ActionResult`. Rejected actions consume nothing.

`knowledge_air_filtration` is read from the shared research capability query;
no shelter-local completion boolean is introduced. The authored `+50% lifespan`
claim is represented as a `2/3` degradation rate in the air owner, which is
the mathematically equivalent rate interpretation.
