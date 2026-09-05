using System;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public sealed class BallisticShieldHostSession : HostSessionBase
    {
        public BallisticShieldEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public BallisticShieldHostSession(BallisticShieldEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnStanceChanged += stance =>
            {
                LastEvent = $"Ballistic shield stance changed: {stance}.";
                RaiseStateChanged();
            };
            System.OnDamageBlocked += block =>
            {
                LastEvent = block.shattered
                    ? "Shield shattered under incoming fire!"
                    : $"Blocked {block.absorbedDamage:F1} damage ({block.penetratingDamage:F1} penetrated).";
                RaiseStateChanged();
            };
            System.OnViewportCracked += integrity =>
            {
                LastEvent = $"Shield viewport struck! Clarity degraded to {integrity:P0}.";
                RaiseStateChanged();
            };
            System.OnShieldBroken += id =>
            {
                LastEvent = $"Ballistic shield '{id}' destroyed.";
                RaiseStateChanged();
            };
            System.OnStateChanged += _ => { RaiseStateChanged(); };
        }

        public ActionResult EquipShield(string shieldId)
        {
            var res = System.EquipShield(shieldId);
            if (res.IsFailure) LastEvent = "Equipping shield blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult SetStance(ShieldStance stance)
        {
            var res = System.SetStance(stance);
            if (res.IsFailure) LastEvent = "Stance change blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult AnchorToGround()
        {
            var res = System.AnchorToGround();
            if (res.IsFailure) LastEvent = "Ground anchor deployment blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult Unanchor()
        {
            var res = System.Unanchor();
            if (res.IsFailure) LastEvent = "Ground anchor retraction blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult JoinPhalanx(int allyCount)
        {
            var res = System.JoinPhalanx(allyCount);
            if (res.IsFailure) LastEvent = "Phalanx formation blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public override void Save()
        {
            if (!IsDirty) return;
            BallisticShieldSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
