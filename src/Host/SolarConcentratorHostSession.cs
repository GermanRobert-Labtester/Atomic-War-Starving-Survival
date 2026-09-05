using System;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public sealed class SolarConcentratorHostSession : HostSessionBase
    {
        public SolarConcentratorEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public SolarConcentratorHostSession(SolarConcentratorEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnDishFouled += refl =>
            {
                LastEvent = $"Solar concentrator mirrors fouled by fallout dust. Reflectivity: {refl:P0}.";
                RaiseStateChanged();
            };
            System.OnSolarOutputChanged += (th, el) =>
            {
                LastEvent = $"Solar output: {th:F1} kW thermal, {el:F1} kW electrical.";
                RaiseStateChanged();
            };
            System.OnStateChanged += _ => { RaiseStateChanged(); };
        }

        public ActionResult CleanMirrors()
        {
            var res = System.CleanMirrors();
            if (res.IsFailure) LastEvent = "Cleaning mirrors blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult CalibrateTracking()
        {
            var res = System.CalibrateTracking();
            if (res.IsFailure) LastEvent = "Tracking calibration blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public override void Save()
        {
            if (!IsDirty) return;
            SolarConcentratorSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
