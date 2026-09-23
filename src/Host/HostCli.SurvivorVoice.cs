// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 42 / C2[18] host probe.
//
// --survivor-voice-selftest proves, headlessly, the voice composition the host
// wires: authored catalog selection with day cooldowns and register weighting,
// dispatch-bus arbitration (per-speaker/global cooldowns, crisis preemption),
// utterance history, and capture/restore parity of the combined section.
// ============================================================================
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Voice;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunSurvivorVoiceSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] survivor-voice/{gate}"); }
                else { fail++; GD.Print($"[FAIL] survivor-voice/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            // 1 — the authored catalog is the only line source.
            var system = new SurvivorVoiceSystem();
            string catalogJson = SurvivorVoiceHostSession.LoadCatalogJson(dataDirectory);
            system.LoadCatalog(catalogJson);
            Check("catalog_loaded", system.Catalog.Count == 8, system.Catalog.Count.ToString());
            Check("catalog_triggers", system.Catalog.Any(l => l.Trigger == "survivor_perished"));

            var dispatch = new VoiceLineDispatchCoordinator();
            var session = new SurvivorVoiceHostSession(system, dispatch);

            VoiceLinePayload? first = null;
            system.VoiceLineDeliveredSink = payload => first ??= payload;

            // 2 — a matching trigger delivers a line and records it in history.
            var delivered = session.Speak(
                "radiation_spike", day: 4, speakerId: "srv_medic",
                morale: 70f, fatigue: 20f, preferredRegister: null, profession: "medic",
                priority: VoiceLinePriority.NeedsWarning);
            Check("line_delivered", delivered != null && !string.IsNullOrEmpty(delivered.TextEnglish));
            Check("delivery_sink_fired", first != null && first.LineId == delivered!.LineId);
            Check("history_recorded", system.History.Count == 1 && system.History[0].Trigger == "radiation_spike");

            // 3 — per-survivor day cooldown blocks a second line the same day.
            var sameDay = session.Speak(
                "radiation_spike", day: 4, speakerId: "srv_medic",
                morale: 70f, fatigue: 20f, preferredRegister: null, profession: "medic",
                priority: VoiceLinePriority.NeedsWarning);
            Check("survivor_day_cooldown", sameDay == null);

            // 4 — another day re-opens the catalog selection.
            var nextDay = session.Speak(
                "radiation_spike", day: 12, speakerId: "srv_medic",
                morale: 70f, fatigue: 20f, preferredRegister: null, profession: "medic",
                priority: VoiceLinePriority.NeedsWarning);
            Check("next_day_reopens", nextDay != null && system.History.Count == 2);

            // 5 — line cooldown (2 days on the authored line) blocks reuse.
            var tooSoon = session.Speak(
                "radiation_spike", day: 13, speakerId: "srv_medic",
                morale: 70f, fatigue: 20f, preferredRegister: null, profession: "medic",
                priority: VoiceLinePriority.NeedsWarning);
            Check("line_cooldown_blocks_reuse", tooSoon == null);

            // 6 — unmatched speaker profession / trigger yields nothing.
            var wrongTrigger = session.Speak(
                "visitor_arrived", day: 12, speakerId: "srv_medic",
                morale: 70f, fatigue: 20f, preferredRegister: null, profession: "medic",
                priority: VoiceLinePriority.AmbientIdle);
            var wrongProfession = session.Speak(
                "season_changed", day: 12, speakerId: "srv_medic",
                morale: 70f, fatigue: 20f, preferredRegister: null, profession: "broker",
                priority: VoiceLinePriority.AmbientIdle);
            Check("no_match_no_line", wrongTrigger == null && wrongProfession == null);

            // 7 — dispatch bus: the first bark holds the bus so a second
            // same-day bark is arbitrated away (active line/global cooldown).
            var speakerA = session.Speak(
                "survivor_perished", day: 22, speakerId: "srv_a",
                morale: 40f, fatigue: 10f, preferredRegister: "weary", profession: "scavenger",
                priority: VoiceLinePriority.AmbientIdle);
            var speakerB = session.Speak(
                "survivor_perished", day: 22, speakerId: "srv_b",
                morale: 40f, fatigue: 10f, preferredRegister: "weary", profession: "scavenger",
                priority: VoiceLinePriority.AmbientIdle);
            Check("bus_holds_single_active_line", speakerA != null && speakerB == null);
            Check("bus_tracks_active_speaker", dispatch.ActivePlayback != null
                && dispatch.ActivePlayback.SpeakerSurvivorId == "srv_a");

            // 8 — capture/restore parity of catalog state + dispatch bus.
            var captured = session.CaptureState();
            var restoredSystem = new SurvivorVoiceSystem();
            restoredSystem.LoadCatalog(catalogJson);
            var restoredSession = new SurvivorVoiceHostSession(restoredSystem, new VoiceLineDispatchCoordinator());
            restoredSession.RestoreState(captured);
            Check("restore_history_preserved", restoredSystem.History.Count == system.History.Count);
            Check("restore_cooldowns_preserved",
                restoredSystem.State.SurvivorLastUtteredDay.Count == system.State.SurvivorLastUtteredDay.Count
                && restoredSession.TotalUtterances == session.TotalUtterances);

            // 9 — a restored session keeps honoring the day cooldown.
            var afterRestoreSameDay = restoredSession.Speak(
                "radiation_spike", day: 12, speakerId: "srv_medic",
                morale: 70f, fatigue: 20f, preferredRegister: null, profession: "medic",
                priority: VoiceLinePriority.NeedsWarning);
            Check("restore_cooldown_enforced", afterRestoreSameDay == null);

            GD.Print($"[survivor-voice-selftest] {pass} passed, {fail} failed");
            return fail == 0 ? 0 : 1;
        }
    }
}
