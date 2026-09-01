using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.PlayerCommand;

namespace AtomicWar.GodotApp
{
    public class ChemicalDependencyHostSession : HostSessionBase
    {
        public ChemicalDependencySystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>
        /// Task #133 P1b: the unified medical pipeline, injected by Main once
        /// bound. Detox starts go through it (validate → apply); null keeps
        /// headless/CLI sessions working unchanged.
        /// </summary>
        public MedicalPipelineCoordinator? Pipeline { get; set; }
        private Action? _onstatechanged_handler;
        private Action<string, string>? _ondependencyformed_handler;
        private Action<string, string>? _onwithdrawalstarted_handler;
        private Action<string, string>? _ondetoxcompleted_handler;
        private Action<string, string>? _ondetoxfailed_handler;

        public ChemicalDependencyHostSession(ChemicalDependencySystem? system = null)
        {
            System = system ?? new ChemicalDependencySystem();
            _onstatechanged_handler = RaiseChanged;
            System.OnStateChanged += _onstatechanged_handler;
            _ondependencyformed_handler = (survivorId, itemId) =>
            {
                LastEvent = $"[ChemicalDependency] {survivorId} formed dependency on {itemId}";
                RaiseChanged();
            };
            System.OnDependencyFormed += _ondependencyformed_handler;
            _onwithdrawalstarted_handler = (survivorId, itemId) =>
            {
                LastEvent = $"[ChemicalDependency] {survivorId} withdrawal started for {itemId}";
                RaiseChanged();
            };
            System.OnWithdrawalStarted += _onwithdrawalstarted_handler;
            _ondetoxcompleted_handler = (survivorId, itemId) =>
            {
                LastEvent = $"[ChemicalDependency] {survivorId} detox completed for {itemId}";
                RaiseChanged();
            };
            System.OnDetoxCompleted += _ondetoxcompleted_handler;
            _ondetoxfailed_handler = (survivorId, itemId) =>
            {
                LastEvent = $"[ChemicalDependency] {survivorId} detox failed for {itemId}";
                RaiseChanged();
            };
            System.OnDetoxFailed += _ondetoxfailed_handler;
        }

        public override void Save()
        {
            if (!IsDirty) return;
            try
            {
                if (ChemicalDependencySaveStore.TrySave(System.CaptureState()))
                    base.Save();
            }
            catch (Exception e)
            {
                GD.PrintErr("[ChemicalDependency] save failed: " + e.Message);
            }
        }

        public void RestoreSave(ChemicalDependencyLedgerState? state)
        {
            if (state == null) return;
            try
            {
                System.RestoreState(state);
                IsDirty = false;
            }
            catch (Exception e)
            {
                GD.PrintErr("[ChemicalDependency] restore failed: " + e.Message);
            }
        }

        public CommandResult BeginManagedDetox(string survivorId, string itemId)
        {
            var result = System.ExecuteBeginManagedDetox(survivorId, itemId, expectedStateVersion: StateVersion, currentStateVersion: StateVersion);
            if (result.IsSuccess)
            {
                LastEvent = $"Managed detox begun for {survivorId} ({itemId}).";
                RaiseStateChanged();
            }
            return result;
        }

        public CommandResult BeginColdTurkey(string survivorId, string itemId)
        {
            var result = System.ExecuteBeginColdTurkey(survivorId, itemId, expectedStateVersion: StateVersion, currentStateVersion: StateVersion);
            if (result.IsSuccess)
            {
                LastEvent = $"Cold turkey begun for {survivorId} ({itemId}).";
                RaiseStateChanged();
            }
            return result;
        }

        private void RaiseChanged()
        {
            MarkDirty();
        }

        protected override void UnsubscribeSystemEvents()
        {
            System.OnStateChanged -= _onstatechanged_handler;
            System.OnDependencyFormed -= _ondependencyformed_handler;
            System.OnWithdrawalStarted -= _onwithdrawalstarted_handler;
            System.OnDetoxCompleted -= _ondetoxcompleted_handler;
            System.OnDetoxFailed -= _ondetoxfailed_handler;
        }
    }
}
