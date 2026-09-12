// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 87 follow-up QA — <c>--workshop-relic-uitest</c>.
    ///
    /// Headless smoke for the workshop dual-bind relic restoration path:
    /// route → bind (shelter crafting + relic catalog) → render (both
    /// sections present — mechanically gates the F1 branch regression) →
    /// select → command (component-gated start) → tick to completion →
    /// state deltas (morale + world flag, each exactly once) → save/reload
    /// (restored state persists, no duplicate application, re-repair blocked).
    /// Exits 0 on PASS, 1 on any failure.
    /// </summary>
    public partial class Main : Control
    {
        private void RunWorkshopRelicUiTestAndQuit()
        {
            bool pass = true;
            void Check(bool cond, string label)
            {
                if (cond) { GD.Print("[OK] " + label); }
                else { GD.PrintErr("[FAIL] " + label); pass = false; }
            }

            const string relicId = "mantel_clock";
            const string flagKey = "relic_restored_mantel_clock";

            BuildUserInterface();

            // Production-mirroring bind sequence (PanelRegistry "workshop" bindAction).
            EnsureShelterWorkshop();
            SetupInventory();
            SetupSurvivors();
            SetupCrafting();

            Check(_shelterWorkshop != null, "shelter workshop initialized");
            Check(_crafting?.Workshop != null, "relic workshop initialized");
            Check(_workshopPanel != null, "WorkshopPanel constructed");
            if (_shelterWorkshop == null || _crafting == null || _workshopPanel == null)
            {
                GD.Print("WorkshopRelicUiTest FAIL");
                GetTree().Quit(1);
                return;
            }

            _workshopPanel.Bind(_shelterWorkshop, _inventory.Inventory, null, null, _survivors);
            _workshopPanel.BindRelicWorkshop(_crafting.Workshop, _inventory.Inventory, _crafting.LoadedItemCatalog, _survivors);
            _workshopPanel.Open();

            // ── Gate A (F1 regression): both sections render after dual bind ──
            var relicList = _workshopPanel.GetNode<VBoxContainer>("%RelicListContainer");
            int buttonCount = 0;
            int restorationHeaders = 0;
            foreach (var child in relicList.GetChildren())
            {
                if (child is Button) buttonCount++;
                else if (child is Label l && l.Text.Contains("RESTORATION")) restorationHeaders++;
            }
            Check(restorationHeaders >= 1, "restoration section renders after dual bind");
            Check(buttonCount > _crafting.Workshop.Catalog.Count,
                $"crafting recipe buttons render beside relics (buttons={buttonCount}, relics={_crafting.Workshop.Catalog.Count})");

            // ── Select the mantel clock ──
            Button? clockButton = FindButtonByText(relicList, "Mantel Clock");
            Check(clockButton != null, "mantel clock relic button present");
            clockButton!.EmitSignal(BaseButton.SignalName.Pressed);

            var detail = _workshopPanel.GetNode<VBoxContainer>("%DetailContainer");
            var startBtn = FindButtonByText(detail, "START RESTORATION");
            Check(startBtn != null, "detail pane offers START RESTORATION");
            Check(startBtn!.Disabled, "START RESTORATION disabled while components missing");

            // ── Supply components, start the repair ──
            Check(_inventory.Inventory.AddById("spring_mechanism", 1), "component added: spring_mechanism");
            Check(_inventory.Inventory.AddById("mechanical_parts", 1), "component added: mechanical_parts");
            Check(_inventory.Inventory.AddById("machine_oil", 1), "component added: machine_oil");
            _workshopPanel.Open();
            startBtn = FindButtonByText(detail, "START RESTORATION");
            Check(startBtn != null && !startBtn.Disabled, "START RESTORATION enabled once components are held");

            float moraleBefore = 0f;
            int living = 0;
            foreach (var s in _survivors!.RosterState)
            {
                if (s != null && s.IsAliveState) { moraleBefore += s.Morale; living++; }
            }

            int flagEvents = 0;
            _consequenceLedger.OnConsequenceRecorded += rec =>
            {
                if (rec != null && rec.key == flagKey) flagEvents++;
            };

            startBtn!.EmitSignal(BaseButton.SignalName.Pressed);
            Check(_crafting.Workshop.IsBusy, "repair job started and reserved components");
            Check(_inventory.Inventory.CountById("spring_mechanism") == 0, "components consumed atomically at start");

            // ── Tick to completion (repair is 4 h at skill 1.0) ──
            _crafting.TickHours(24f);
            Check(_crafting.Workshop.IsRelicCompleted(relicId), "relic completed after tick");

            float moraleAfter = 0f;
            foreach (var s in _survivors.RosterState)
            {
                if (s != null && s.IsAliveState) moraleAfter += s.Morale;
            }
            Check(living > 0, "living survivors present for morale application");
            Check(System.Math.Abs(moraleAfter - moraleBefore - living * 3f) < 0.01f,
                $"shelter-wide morale applied exactly once (+3 x {living})");

            Check(_consequenceLedger.IsSet(flagKey), "restoration world flag set");
            Check(flagEvents == 1, $"flag recorded exactly once (got {flagEvents})");

            // ── Duplicate restoration blocked ──
            var again = _crafting.Workshop.StartRepair(relicId, string.Empty);
            Check(!again.IsSuccess, "re-restoring a completed relic is blocked");

            // ── Save / reload ──
            var captured = _crafting.CaptureSave();
            Check(captured?.WorkshopState != null, "capture includes workshop state");
            Check(captured!.WorkshopState!.completedRelicIds.Contains(relicId), "completed relic id persisted");
            _crafting.RestoreSave(captured);
            Check(_crafting.Workshop.IsRelicCompleted(relicId), "restored state preserves relic completion");

            // Reload must not re-apply morale or re-record the flag.
            _crafting.TickHours(24f);
            float moraleReloaded = 0f;
            foreach (var s in _survivors.RosterState)
            {
                if (s != null && s.IsAliveState) moraleReloaded += s.Morale;
            }
            Check(System.Math.Abs(moraleReloaded - moraleAfter) < 0.01f, "no duplicate morale after save/reload + tick");
            Check(flagEvents == 1, $"no duplicate flag after save/reload (got {flagEvents})");

            var reAgain = _crafting.Workshop.StartRepair(relicId, string.Empty);
            Check(!reAgain.IsSuccess, "re-repair still blocked after reload");

            if (pass) GD.Print("WorkshopRelicUiTest PASS");
            else GD.Print("WorkshopRelicUiTest FAIL");
            GetTree().Quit(pass ? 0 : 1);
        }

        private static Button? FindButtonByText(Node container, string text)
        {
            foreach (var child in container.GetChildren())
            {
                if (child is Button b && b.Text.Contains(text)) return b;
            }
            return null;
        }
    }
}
