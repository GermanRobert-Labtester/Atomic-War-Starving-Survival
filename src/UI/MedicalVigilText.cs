// SPDX-License-Identifier: MIT
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Presentation formatter for the vigil status line.
    ///
    /// <para>The host owns the vigil state machine; the panel owns the localized
    /// text. Previously <c>MedicalHostSession.VigilStatusLine()</c> returned a
    /// host-owned English string, which meant the two panels that render it
    /// (MedicalPanel, MedicalWardPanel) could not be localized. Both now share
    /// this formatter over the public <see cref="VigilStateMachine"/> state.</para>
    ///
    /// <para>Presentation only — no simulation logic, no mutable state.</para>
    /// </summary>
    public static class MedicalVigilText
    {
        public static string Format(VigilStateMachine? vigil)
        {
            if (vigil == null || (!vigil.IsActive && !vigil.IsCompleted))
                return AshfallUiText.Tr("ui.medical.vigil_idle", "Vigil: idle");
            if (vigil.IsCompleted)
                return vigil.WasSkipped
                    ? AshfallUiText.Tr("ui.medical.vigil_left_early", "Vigil: left early")
                    : AshfallUiText.TrFormat("ui.medical.vigil_kept", vigil.DwellerId);

            string line = AshfallUiText.TrFormat("ui.medical.vigil_active",
                vigil.DwellerId,
                $"{vigil.ElapsedSeconds:F0}",
                $"{vigil.DurationSeconds:F0}",
                vigil.RecitedCount,
                vigil.Names.Count);
            return vigil.PhantomKnockFired
                ? line + " " + AshfallUiText.Tr("ui.medical.vigil_phantom", "· phantom knock")
                : line;
        }
    }
}