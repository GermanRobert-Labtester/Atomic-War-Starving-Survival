// SPDX-License-Identifier: MIT
// ASHFALL Plan 152 — Vehicle Customization & Mobile Base Host Wiring.

using System;
using Godot;
using Ashfall.Core.Vehicles;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private VehicleCustomizationHostSession? _vehicleCustomization;
        private bool _vehicleCustomizationDirty;

        public VehicleCustomizationHostSession? VehicleCustomization => _vehicleCustomization;

        public void SetupVehicleCustomization()
        {
            if (_vehicleCustomization != null) return;

            _vehicleCustomization = VehicleCustomizationHostSession.Create(_dataDir);

            var saved = VehicleCustomizationSaveStore.TryLoad();
            if (saved != null && !string.IsNullOrEmpty(saved.core_state))
            {
                _vehicleCustomization.RestoreCoreState(saved.core_state);
            }

            _vehicleCustomization.StateChanged += () => _vehicleCustomizationDirty = true;
        }

        public void SaveVehicleCustomization()
        {
            if (_vehicleCustomization == null) return;
            var state = _vehicleCustomization.CapturePersistedState();
            VehicleCustomizationSaveStore.TrySave(state);
            if (CaptureSection("vehicle_customization", VehicleCustomizationSaveStore.TryCapturePersisted(state)))
            {
                _vehicleCustomizationDirty = false;
            }
        }

        /// <summary>
        /// Daily tick for the customization owner. Vehicle condition and fuel
        /// are owned by the expedition vehicle authority and are deliberately
        /// NOT mutated here — doing so would be a second drain on the same
        /// resource. This seam only guarantees the module catalog stays bound
        /// and the owner is constructed before the day is reported.
        /// </summary>
        public void TickVehicleCustomization(int day)
        {
            if (_vehicleCustomization == null) SetupVehicleCustomization();
        }

        public void FlushVehicleCustomizationIfDirty()
        {
            if (_vehicleCustomizationDirty)
            {
                SaveVehicleCustomization();
            }
        }

        public void ResetVehicleCustomization()
        {
            _vehicleCustomization = null;
            _vehicleCustomizationDirty = false;
        }
    }
}
