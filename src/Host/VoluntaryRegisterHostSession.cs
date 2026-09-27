// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 253: Voluntary Register — save store.
    /// </summary>
    public static class VoluntaryRegisterSaveStore
    {
        public const string FileName = "voluntary_register_save.json";
        public const string SectionName = "voluntary_register";

        private static readonly SaveStore<VoluntaryRegisterSystemState> s_store =
            SaveStoreHub.Checksummed<VoluntaryRegisterSystemState>(FileName, nameof(VoluntaryRegisterSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(VoluntaryRegisterSystemState state) => s_store.TrySave(state);
        public static VoluntaryRegisterSystemState? TryLoad() => s_store.TryLoad();
        // Coordination repair 2026-09-27 (rumor/memorial seal session): added the
        // canonical capture method Main.VoluntaryRegister.SaveVoluntaryRegister
        // calls (Checksummed stores expose CaptureBare).
        public static string TryCapturePersisted(VoluntaryRegisterSystemState state) => s_store.CaptureBare(state);
    }

    /// <summary>
    /// Plan 253: Voluntary Register — host session binding the Core system to
    /// the campaign lifecycle. Manages high-dose volunteer work signatures.
    /// </summary>
    public sealed class VoluntaryRegisterHostSession : HostSessionBase
    {
        private readonly VoluntaryRegisterSystem _system;

        public VoluntaryRegisterSystem System => _system;
        public VoluntaryRegisterSystemState State => _system.State;

        public VoluntaryRegisterHostSession(VoluntaryRegisterSystem? system = null)
        {
            _system = system ?? new VoluntaryRegisterSystem();
        }

        public void Tick(int day)
        {
            RaiseStateChanged();
        }

        public VoluntaryRegisterSystemState CaptureState()
        {
            return _system.CaptureState();
        }

        public void RestoreState(VoluntaryRegisterSystemState state)
        {
            _system.RestoreState(state);
            RaiseStateChanged();
        }

        public void Reset()
        {
            _system.RestoreState(new VoluntaryRegisterSystemState());
            RaiseStateChanged();
        }
    }
}
