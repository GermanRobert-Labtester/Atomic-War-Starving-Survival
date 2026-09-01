using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    public enum CeilingMaterialTier { Dirt, Wood, ReinforcedConcrete, LeadSheeting, TungstenComposite }

    [Serializable]
    public sealed class CeilingCellArmor
    {
        public int gridX;
        public CeilingMaterialTier material;
        public float thicknessMeters;
        public float currentDurability; // 0.0 to 100.0
    }

    [Serializable]
    public sealed class SkyArmorSaveState
    {
        public List<CeilingCellArmor> cells = new List<CeilingCellArmor>();
    }

    /// <summary>
    /// ASHFALL: THE ORBITAL HARROW (Expansion 11) — Sky Layer Armor System.
    /// Simulates 2D overhead vertical armor, atmospheric rad attenuation, and kinetic impact resistance.
    /// </summary>
    public sealed class SkyLayerArmorSystem
    {
        private readonly Dictionary<int, CeilingCellArmor> _cells = new Dictionary<int, CeilingCellArmor>();

        public void SetCellArmor(int gridX, CeilingMaterialTier material, float thicknessMeters, float durability = 100f)
        {
            _cells[gridX] = new CeilingCellArmor
            {
                gridX = gridX,
                material = material,
                thicknessMeters = Math.Max(0.1f, thicknessMeters),
                currentDurability = MathfCompat.Clamp(durability, 0f, 100f)
            };
        }

        public void InstallConfiguration(int gridX, SkyLayerArmorConfigDef config)
        {
            if (config == null) return;
            SetCellArmor(gridX, config.material_tier, config.default_thickness_meters, 100f);
        }

        public CeilingCellArmor? GetCell(int gridX)
        {
            return _cells.TryGetValue(gridX, out var cell) ? cell : null;
        }

        public float GetAttenuationFactor(int gridX)
        {
            if (!_cells.TryGetValue(gridX, out var cell))
                return 1.0f; // Unprotected surface bleed

            float baseMultiplier = cell.material switch
            {
                CeilingMaterialTier.Dirt => 0.60f,
                CeilingMaterialTier.Wood => 0.85f,
                CeilingMaterialTier.ReinforcedConcrete => 0.20f,
                CeilingMaterialTier.LeadSheeting => 0.05f,
                CeilingMaterialTier.TungstenComposite => 0.01f,
                _ => 1.0f
            };

            float conditionFactor = Math.Max(0.2f, cell.currentDurability / 100f);
            return MathfCompat.Clamp(baseMultiplier / (cell.thicknessMeters * conditionFactor), 0.005f, 1.0f);
        }

        public void RepairCell(int gridX, float durabilityAmount)
        {
            if (_cells.TryGetValue(gridX, out var cell))
            {
                cell.currentDurability = Math.Min(100f, cell.currentDurability + Math.Max(0f, durabilityAmount));
            }
        }

        public bool EvaluateKineticImpact(int gridX, float impactEnergyMegaJoules, out float damageDealtToRoof)
        {
            damageDealtToRoof = 0f;
            if (!_cells.TryGetValue(gridX, out var cell))
            {
                damageDealtToRoof = impactEnergyMegaJoules * 10f;
                return true; // Complete penetration into unprotected bunker
            }

            float absorptionThreshold = cell.material switch
            {
                CeilingMaterialTier.Dirt => 5f,
                CeilingMaterialTier.Wood => 2f,
                CeilingMaterialTier.ReinforcedConcrete => 25f,
                CeilingMaterialTier.LeadSheeting => 15f,
                CeilingMaterialTier.TungstenComposite => 80f,
                _ => 1f
            } * cell.thicknessMeters;

            if (impactEnergyMegaJoules > absorptionThreshold)
            {
                cell.currentDurability = Math.Max(0f, cell.currentDurability - 50f);
                damageDealtToRoof = impactEnergyMegaJoules - absorptionThreshold;
                return true; // Breached
            }

            cell.currentDurability = Math.Max(0f, cell.currentDurability - (impactEnergyMegaJoules / absorptionThreshold) * 20f);
            return false; // Absorbed by armor
        }

        public SkyArmorSaveState CaptureState()
        {
            var save = new SkyArmorSaveState();
            foreach (var kvp in _cells)
            {
                save.cells.Add(new CeilingCellArmor
                {
                    gridX = kvp.Value.gridX,
                    material = kvp.Value.material,
                    thicknessMeters = kvp.Value.thicknessMeters,
                    currentDurability = kvp.Value.currentDurability
                });
            }
            return save;
        }

        public void RestoreState(SkyArmorSaveState state)
        {
            _cells.Clear();
            if (state?.cells == null) return;
            foreach (var c in state.cells)
            {
                _cells[c.gridX] = new CeilingCellArmor
                {
                    gridX = c.gridX,
                    material = c.material,
                    thicknessMeters = c.thicknessMeters,
                    currentDurability = c.currentDurability
                };
            }
        }
    }
}
