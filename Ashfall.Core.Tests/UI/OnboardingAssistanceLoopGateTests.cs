// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: Onboarding assistance-cycle event discipline (T04 —
// teach-vs-demand sweep repair).
//
// Finding: OnboardingHintPanel.CycleAssistance() raised OnAssistanceChanged,
// and the host handler Main.SetOnboardingAssistance() called back into
// CycleAssistance() — a synchronous, unbounded re-entrant event loop that
// overflows the stack on a single press of the panel's ASSISTANCE cycle
// button (OnboardingHintPanel.cs:265-270 -> Main.Onboarding.cs:235-243 ->
// wiring Main.Onboarding.cs:95). The contract below pins the fix: the button
// handler OnCycleAssistanceClicked() is the SINGLE site that raises
// OnAssistanceChanged; CycleAssistance() is the host-driven reactive update
// path (label + refresh) and must never re-raise, because the host handler
// funnels back into it.
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class OnboardingAssistanceLoopGateTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "src", "UI", "OnboardingHintPanel.cs"))
                    && File.Exists(Path.Combine(dir.FullName, "src", "Main.Onboarding.cs")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string PanelSource() =>
            File.ReadAllText(Path.Combine(RepoRoot(), "src", "UI", "OnboardingHintPanel.cs"));

        private static string HostOnboardingSource() =>
            File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.Onboarding.cs"));

        /// <summary>
        /// Extracts the body of the method whose declaration line contains
        /// <paramref name="methodSignature"/> by brace matching. Returns the
        /// body between the declaration's opening '{' and its matching '}'.
        /// </summary>
        private static string MethodBody(string source, string methodSignature)
        {
            int decl = source.IndexOf(methodSignature, StringComparison.Ordinal);
            if (decl < 0)
                return string.Empty;
            int open = source.IndexOf('{', decl);
            if (open < 0)
                return string.Empty;
            int depth = 0;
            for (int i = open; i < source.Length; i++)
            {
                if (source[i] == '{') depth++;
                else if (source[i] == '}')
                {
                    depth--;
                    if (depth == 0)
                        return source.Substring(open, i - open + 1);
                }
            }
            return string.Empty;
        }

        [Fact]
        public void CycleAssistance_IsAReactiveUpdate_AndNeverRaisesTheEvent()
        {
            string body = MethodBody(PanelSource(), "public void CycleAssistance(");
            Assert.False(string.IsNullOrWhiteSpace(body),
                "CycleAssistance method not found in OnboardingHintPanel.cs.");

            // The reactive path must not re-enter the host handler (which
            // funnels back into CycleAssistance). One raise from here is an
            // infinite synchronous loop -> stack overflow on the button press.
            Assert.False(body.Contains("OnAssistanceChanged?.Invoke", StringComparison.Ordinal),
                "CycleAssistance must be a one-way update (label + refresh); it must "
                + "not raise OnAssistanceChanged, or the host handler "
                + "(SetOnboardingAssistance -> CycleAssistance) re-enters forever.");
        }

        [Fact]
        public void AssistanceCycleButton_RemainsTheSingleEventRaiseSite()
        {
            string body = MethodBody(PanelSource(), "private void OnCycleAssistanceClicked()");
            Assert.False(string.IsNullOrWhiteSpace(body),
                "OnCycleAssistanceClicked method not found in OnboardingHintPanel.cs.");

            // The user-initiated button is the single raise site: remove it and
            // the journey's assistance level would never change.
            Assert.True(body.Contains("OnAssistanceChanged?.Invoke", StringComparison.Ordinal),
                "The ASSISTANCE cycle button must remain the single raise site for "
                + "OnAssistanceChanged; a raise must not be delegated to the reactive "
                + "CycleAssistance update path.");
        }

        [Fact]
        public void HostHandler_StillRefreshesThePanelOnAssistanceChange()
        {
            string body = MethodBody(HostOnboardingSource(),
                "public void SetOnboardingAssistance(");
            Assert.False(string.IsNullOrWhiteSpace(body),
                "SetOnboardingAssistance not found in Main.Onboarding.cs.");

            // The host must still push the new level into the panel's reactive
            // update path (label + view refresh) so the visible UI follows the
            // journey state — the fix removes the loop, not the refresh.
            Assert.True(body.Contains("_onboardingHintPanel?.CycleAssistance(level)", StringComparison.Ordinal),
                "SetOnboardingAssistance must keep refreshing the panel through "
                + "CycleAssistance after the re-entrancy fix.");
        }

        [Fact]
        public void AssistanceCycle_DoesNotOfferTheReservedNoOpGuidedTier()
        {
            string body = MethodBody(PanelSource(), "private void OnCycleAssistanceClicked()");
            Assert.False(string.IsNullOrWhiteSpace(body),
                "OnCycleAssistanceClicked method not found in OnboardingHintPanel.cs.");

            // GUIDED is a reserved enum value that behaves exactly like STANDARD
            // (no auto-highlight exists). A press that changes the label but not
            // the behavior is the same silent-mismatch class as the old
            // "HINT: —" sentinel — the cycle must not offer it.
            Assert.False(body.Contains("OnboardingAssistance.Guided", StringComparison.Ordinal),
                "The assistance cycle must not offer the reserved no-op GUIDED tier: "
                + "a player pressing it would see the label change with zero behavioral "
                + "difference (no auto-highlight exists in the codebase).");
        }

        [Fact]
        public void GuidedSaves_RenderTheStandardLabel_NotTheMissingTier()
        {
            string body = MethodBody(PanelSource(), "private static string AssistanceLabel(");
            Assert.False(string.IsNullOrWhiteSpace(body),
                "AssistanceLabel method not found in OnboardingHintPanel.cs.");

            // A legacy save may still carry assistance=Guided (persisted enum).
            // It must render the STANDARD label, because the GUIDED tier has no
            // distinct behavior — showing "ASSISTANCE: GUIDED" would re-assert
            // the missing tier. The key onboarding.assistance.guided must not be
            // presented at runtime.
            Assert.False(body.Contains("onboarding.assistance.guided", StringComparison.Ordinal),
                "AssistanceLabel must not present the reserved GUIDED-tier key to players "
                + "(renders the STANDARD label for legacy Guided saves).");
            Assert.True(body.Contains("onboarding.assistance.standard", StringComparison.Ordinal),
                "AssistanceLabel must resolve the label through the standard key for "
                + "non-Minimal tiers.");
        }

        [Fact]
        public void AssistanceCycleTooltip_DoesNotPromiseExtraHelp()
        {
            string source = PanelSource();
            Assert.True(source.Contains("onboarding.tooltip.assistance_cycle", StringComparison.Ordinal),
                "Assistance cycle tooltip fallback missing.");
            Assert.False(source.Contains("GUIDED (extra help)", StringComparison.Ordinal),
                "The assistance-cycle tooltip must not promise a GUIDED \"extra help\" "
                + "tier that does not exist in code.");
        }
    }
}