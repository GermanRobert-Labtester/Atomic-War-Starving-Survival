# Orbital Damage Provenance & Shelter Cascades

> **Authority:** `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs`, `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs`

---

## 1. Armor Attenuation & Penetration Model

When an orbital strike resolves:
1. Total kinetic energy ($E_{\text{total}}$) is halved if the shelter is braced ($E_{\text{net}} = 0.5 \times E_{\text{total}}$).
2. Energy is divided across the footprint cells: $E_{\text{cell}} = E_{\text{net}} / \text{Spread}$.
3. For each cell $X$, `SkyLayerArmorSystem.EvaluateKineticImpact(X, E_{\text{cell}})` evaluates the material threshold:

$$\text{Absorption Threshold} = \text{MaterialTierWeight} \times \text{ThicknessMeters}$$

- **Tungsten Composite:** $80 \times \text{Thickness}$
- **Reinforced Concrete:** $25 \times \text{Thickness}$
- **Lead Sheeting:** $15 \times \text{Thickness}$
- **Dirt:** $5 \times \text{Thickness}$
- **Wood:** $2 \times \text{Thickness}$

---

## 2. Downstream Shelter Cascades

If impact energy exceeds cell absorption:
1. **Ceiling Durability:** Breached cell loses 50 durability points; non-breached absorbing cell loses $(E / \text{Threshold}) \times 20$ points.
2. **Penetration Damage:** Remainder energy $\Delta E = E - \text{Threshold}$ passes into the shelter.
3. **Power Grid Disruption:** Cascades into power grid busbars ($\text{Disruption} = \Delta E \times 2.5$), tripping breakers or draining battery reserves.
4. **Structural Fatigue:** Raises shelter repair demand.
