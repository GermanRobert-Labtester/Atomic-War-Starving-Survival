// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : OilseedPressingHostSession
// Purpose      : PLAN-PRESERVATION-TRUTH-118 — host the oilseed press. Install
//                state persists under `oilseed_pressing`; pressing consumes
//                canonical inventory seeds and yields canonical oil output.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Farming;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public sealed class OilseedPressingSaveState
    {
        public int schema_version { get; set; } = 1;
        public bool press_installed { get; set; }
        public int tool_grade { get; set; } = (int)PressToolGrade.ManualScrewPress;
        public int total_pressed { get; set; }
    }

    public static class OilseedPressingSaveStore
    {
        public const string FileName = "oilseed_pressing_save.json";
        public const string SectionName = "oilseed_pressing";
        private static readonly SaveStore<OilseedPressingSaveState> s_store =
            SaveStoreHub.Checksummed<OilseedPressingSaveState>(FileName, nameof(OilseedPressingSaveStore));
        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static string TryCapturePersisted(OilseedPressingSaveState state) => s_store.CaptureBare(state);
        public static OilseedPressingSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(OilseedPressingSaveState state) => s_store.TrySave(state);
        public static OilseedPressingSaveState? TryLoad() => s_store.TryLoad();
    }

    public sealed class OilseedPressingHostSession : HostSessionBase
    {
        public bool PressInstalled { get; private set; }
        public PressToolGrade ToolGrade { get; private set; } = PressToolGrade.ManualScrewPress;
        public int TotalPressed { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;

        public ActionResult InstallPress(PressToolGrade grade)
        {
            if (PressInstalled) return ActionResult.Blocked("oilseed_pressing.already_installed", "oilseed_pressing.already_installed");
            PressInstalled = true;
            ToolGrade = grade;
            LastEvent = $"Oilseed press installed ({grade}).";
            RaiseStateChanged();
            return ActionResult.Success("oilseed_pressing.installed");
        }

        public PressingYieldResult Evaluate(int seedCount, OilseedPressingMode mode)
            => OilseedPressingEngine.EvaluatePressing(seedCount, ToolGrade, mode);

        public PressingYieldResult Press(
            int seedCount, OilseedPressingMode mode,
            Func<string, int> countItem, Func<string, int, bool> consumeItem, Action<string, int> addItem)
        {
            if (!PressInstalled)
            {
                LastEvent = "No press installed.";
                return OilseedPressingEngine.EvaluatePressing(0, ToolGrade, mode);
            }
            if (countItem(OilseedPressingEngine.CropOilseedId) < seedCount)
            {
                LastEvent = "Insufficient oilseed in inventory.";
                return OilseedPressingEngine.EvaluatePressing(0, ToolGrade, mode);
            }

            var result = OilseedPressingEngine.EvaluatePressing(seedCount, ToolGrade, mode);
            if (result.PrimaryOutputAmount > 0)
            {
                consumeItem(OilseedPressingEngine.CropOilseedId, seedCount);
                addItem(result.PrimaryOutputItemId, result.PrimaryOutputAmount);
                if (result.ByproductMealAmount > 0)
                    addItem("meal_cake", result.ByproductMealAmount);
                TotalPressed += result.PrimaryOutputAmount;
                LastEvent = $"Pressed {seedCount} oilseed → {result.PrimaryOutputAmount}x {result.PrimaryOutputItemId}.";
            }
            else
            {
                LastEvent = $"Press rejected {seedCount} oilseed (below batch minimum).";
            }
            RaiseStateChanged();
            return result;
        }

        public OilseedPressingSaveState CaptureState() => new OilseedPressingSaveState
        {
            press_installed = PressInstalled,
            tool_grade = (int)ToolGrade,
            total_pressed = TotalPressed
        };

        public void RestoreState(OilseedPressingSaveState? state)
        {
            if (state == null) return;
            PressInstalled = state.press_installed;
            ToolGrade = (PressToolGrade)state.tool_grade;
            TotalPressed = state.total_pressed;
            RaiseStateChanged();
        }
    }
}
