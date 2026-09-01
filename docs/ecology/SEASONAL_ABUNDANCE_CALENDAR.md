# Seasonal Abundance Calendar

## 1. Seasonal Abundance Factors (Authored in `WildlifeSeasonalCalendar.cs`)

| Migration Archetype | Ashfall (0–59) | Deep Freeze (60–119) | Thaw (120–199) | Black Bloom (200–239) | High Cold (240+) | The Turning (Late) |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **Resident** | $1.0\times$ | $1.0\times$ | $1.0\times$ | $1.0\times$ | $1.0\times$ | $1.0\times$ |
| **HerdGrazer** | $1.0\times$ | $0.6\times$ (Scarcity) | $1.2\times$ | $1.25\times$ | $0.9\times$ | $1.1\times$ |
| **BurrowSwarm** | $1.0\times$ | $0.8\times$ | $1.3\times$ (Peak) | $1.3\times$ (Peak) | $0.7\times$ | $1.0\times$ |
| **Sounder** | $1.0\times$ | $0.9\times$ | $1.0\times$ | $1.1\times$ | $0.9\times$ | $1.4\times$ (Mast Run) |
| **PassageFlock** | $1.0\times$ | $0.4\times$ (Winter Thin) | $1.3\times$ (Passage) | $1.0\times$ | $0.6\times$ | $1.25\times$ |
| **CoastalRunner** | $0.8\times$ | $0.2\times$ (Frozen) | $1.5\times$ (Fish Run) | $1.4\times$ | $0.6\times$ | $0.8\times$ |
| **SwarmBlight** | $0.6\times$ | $0.1\times$ (Dormant) | $0.9\times$ | $1.5\times$ (Swarm Front) | $0.4\times$ | $0.5\times$ |

## 2. Dynamic Trapping Density Modulation
In `src/Main.EvolvingWorld.cs`, home sector trapping is calculated as:
$$\text{DensityMultiplier} = \text{Clamp}\Big((0.5 + 0.1 \times \text{SectorPopulation}) \times \text{SectorSeasonalAbundance}, 0.4, 1.5\Big)$$
This ensures realistic hunting surges during the Thaw Fish Run and Scarcity during Deep Freeze without creating runaway infinite food exploits.
