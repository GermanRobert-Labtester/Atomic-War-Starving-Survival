using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Core
{
    /// <summary>Counts from an EMPEvent.ApplyGlobal pass, for logging/tests.</summary>
    public struct EmpResult
    {
        public int DevicesBroken;
        public int ModulesDisabled;
        public bool RadioDestroyed;
    }

    /// <summary>
    /// Instantly destroys unshielded electronics at the atomic exchange: geiger
    /// counters / dosimeters (Device-type inventory items), shelter filtration
    /// panels and other modules, and the radio. Pure functions over existing state
    /// objects (DeviceState.Broken, ShelterModuleInstance.IsEnabled,
    /// RadioState.ApplyEmpDamage) — no new failure mechanism invented.
    /// </summary>
    public static class EMPEvent
    {
        public static bool ApplyToDevice(DeviceState device, bool shielded)
        {
            if (device == null || shielded) return false;
            bool wasBroken = device.Broken;
            device.Broken = true;
            return !wasBroken;
        }

        public static bool ApplyToShelterModule(ShelterModuleInstance module, bool shielded)
        {
            if (module == null || shielded) return false;
            bool wasEnabled = module.IsEnabled;
            module.IsEnabled = false;
            return wasEnabled;
        }

        public static bool ApplyToRadio(RadioState radio, bool shielded)
        {
            if (radio == null || shielded) return false;
            return radio.ApplyEmpDamage(100f);
        }

        /// <summary>Walks inventory devices and shelter modules, applies the radio if given.</summary>
        public static EmpResult ApplyGlobal(Inventory.Inventory inventory, Shelter.Shelter shelter, RadioState radio)
        {
            var result = new EmpResult();

            if (inventory != null)
            {
                foreach (var slot in inventory.Slots)
                {
                    if (slot == null || slot.Device == null) continue;
                    bool shielded = slot.Item != null && slot.Item.empShielded;
                    if (ApplyToDevice(slot.Device, shielded))
                    {
                        result.DevicesBroken++;
                    }
                }
            }

            if (shelter != null)
            {
                foreach (var module in shelter.Modules)
                {
                    if (module == null) continue;
                    bool shielded = module.Definition != null && module.Definition.EmpShielded;
                    if (ApplyToShelterModule(module, shielded))
                    {
                        result.ModulesDisabled++;
                    }
                }
            }

            result.RadioDestroyed = ApplyToRadio(radio, false);

            return result;
        }
    }
}
