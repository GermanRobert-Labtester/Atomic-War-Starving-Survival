#!/usr/bin/env python3
"""ASHFALL — first-hour funnel report from local play-session JSONL.

Consumes the opt-in local recorder stream (user:// JSONL rows; schema fields
`record_type`, `session_id`, `day`, `t_session_ms`, `action`, `target_id`,
`sigil`, `kind`). Reports the live FirstHour journey progression, the canonical
funnel steps, per-session summaries, and an action histogram. Pure read model:
no network, no gameplay writes.

Usage:
  python3 scripts/tools/first_hour_funnel.py --selftest
  python3 scripts/tools/first_hour_funnel.py --jsonl ~/.local/share/godot/app_userdata/<proj>/play_metrics.jsonl
  python3 scripts/tools/first_hour_funnel.py --discover --out docs/telemetry/FIRST_HOUR_FUNNEL.md
  python3 scripts/tools/first_hour_funnel.py --discover --append-delta docs/telemetry/FIRST_HOUR_FUNNEL.md

P001: --append-delta records a dated delta section (progress + per-stage hint
engagement) after every onboarding/playability batch without rewriting the
existing report.
"""
import argparse
import datetime
import glob
import json
import os
import sys
from collections import Counter, OrderedDict

# Live FirstHour journey: ordered sigils the onboarding journey completes.
LIVE_STEPS = [
    ("water", "Water treatment started", "water.treatment_started"),
    ("power", "Breaker toggled", "power.breaker_toggled"),
    ("food", "Food ration consumed", "food.ration_consumed"),
    ("duty", "Duty assigned", "duty.assigned"),
    ("dose", "Dose reading opened", "dose.read"),
    ("research", "Research started", "research.started"),
    ("expedition", "Expedition dispatched", "expedition.dispatched"),
]

# P002 — published first-hour verbs (PlayerCommandCode), in stage order. The
# host emits one metric action per verb so the top-actions histogram is
# per-stage measurable instead of collapsing every verb into `sigil`.
LIVE_VERBS = [
    "water.start", "power.breaker", "food.consume", "duty.assign",
    "dose.open", "research.start", "expedition.dispatch",
]

# Canonical funnel steps mirrored from Ashfall.Core.Telemetry.FirstHourFunnel.
CANONICAL_STEPS = [
    "guidance_opened", "first_craft", "first_dispatch", "first_ration_decision",
    "first_storm_survived", "first_day_past_tutorial", "first_death_witnessed",
    "first_water", "first_power", "first_food", "first_duty", "first_dose", "first_research",
]
CANONICAL_MATCH = {
    "guidance_opened": "protocol.ration", "first_craft": "inventory.used",
    "first_dispatch": "expedition.dispatched", "first_ration_decision": "ration_policy_set",
    "first_storm_survived": "weather.read", "first_day_past_tutorial": "day_advanced",
    "first_death_witnessed": "survivor_perished", "first_water": "water.treatment_started",
    "first_power": "power.breaker_toggled", "first_food": "food.ration_consumed",
    "first_duty": "duty.assigned", "first_dose": "dose.read", "first_research": "research.started",
}


def load_events(paths):
    events = []
    for path in paths:
        with open(path, encoding="utf-8") as handle:
            for line in handle:
                line = line.strip()
                if not line:
                    continue
                try:
                    events.append(json.loads(line))
                except json.JSONDecodeError:
                    continue
    return events


def event_matches(evt, token):
    token = token.lower()
    for field in ("sigil", "action", "target_id", "kind"):
        value = str(evt.get(field, "") or "").lower()
        if value == token:
            return True
    return False


def analyse(events):
    sessions = OrderedDict()
    for evt in events:
        sid = str(evt.get("session_id", "unknown"))
        entry = sessions.setdefault(sid, {
            "events": 0, "max_day": 0, "actions": Counter(),
            "live_first": {}, "canonical_first": {},
            "hints_shown": Counter(), "hints_dismissed": Counter(),
        })
        entry["events"] += 1
        entry["max_day"] = max(entry["max_day"], int(evt.get("day", 0) or 0))
        action = str(evt.get("action", "") or "")
        if action:
            entry["actions"][action] += 1
        # P004 — hint presentation / dismissal per stage (target_id = stage id).
        if action == "hint_shown" or action == "hint_dismissed":
            stage = str(evt.get("target_id", "") or "")
            if stage:
                bucket = entry["hints_shown"] if action == "hint_shown" else entry["hints_dismissed"]
                bucket[stage] += 1
        for step_id, _label, sigil in LIVE_STEPS:
            if step_id not in entry["live_first"] and event_matches(evt, sigil):
                entry["live_first"][step_id] = int(evt.get("day", 0) or 0)
        for step_id in CANONICAL_STEPS:
            if step_id not in entry["canonical_first"] and event_matches(evt, CANONICAL_MATCH[step_id]):
                entry["canonical_first"][step_id] = int(evt.get("day", 0) or 0)
    return sessions


def render(sessions):
    lines = ["# ASHFALL First-Hour Funnel Report", ""]
    for sid, entry in sessions.items():
        live_done = len(entry["live_first"])
        lines.append(f"## Session `{sid}`")
        lines.append("")
        lines.append(f"- Events: {entry['events']} · Max day: {entry['max_day']}")
        lines.append(f"- **Live first-hour progress: {live_done}/{len(LIVE_STEPS)} "
                     f"({live_done * 100 // len(LIVE_STEPS)}%)**")
        lines.append("")
        lines.append("| # | Step | Reached | Day |")
        lines.append("|---|------|---------|-----|")
        for index, (step_id, label, _sigil) in enumerate(LIVE_STEPS, 1):
            day = entry["live_first"].get(step_id)
            lines.append(f"| {index} | {label} | {'yes' if day is not None else 'no'} | {day if day is not None else '—'} |")
        done_canonical = len(entry["canonical_first"])
        lines.append("")
        lines.append(f"- Canonical funnel: {done_canonical}/{len(CANONICAL_STEPS)} steps")
        top = entry["actions"].most_common(8)
        if top:
            lines.append("- Top actions: " + ", ".join(f"`{a}`×{c}" for a, c in top))
        rows = hint_engagement(entry)
        if rows:
            lines.append("- Hint engagement (shown/dismissed/acted): "
                         + ", ".join(f"`{sid}` {s}/{d}/{a}" for sid, s, d, a in rows))
        else:
            lines.append("- Hint engagement: no `hint_shown` events recorded")
        lines.append("")
    return "\n".join(lines)


def hint_engagement(entry):
    """P004 — per-stage (shown, dismissed, acted) rows, in live stage order."""
    rows = []
    for step_id, _label, _sigil in LIVE_STEPS:
        shown = entry["hints_shown"].get(step_id, 0)
        dismissed = entry["hints_dismissed"].get(step_id, 0)
        if not shown and not dismissed:
            continue
        acted = "yes" if step_id in entry["live_first"] else "no"
        rows.append((step_id, shown, dismissed, acted))
    return rows


def render_delta(sessions):
    """P001 — appendable delta section: progress + hint engagement this batch."""
    today = datetime.date.today().isoformat()
    lines = [
        "",
        "---",
        "",
        f"## Delta — {today}",
        "",
        "P001 instrumentation re-run (`scripts/tools/first_hour_funnel.py`). "
        "Per-session live first-hour progress, verb histogram, and hint engagement.",
        "",
        "| Session | Events | Max day | Live progress | Top actions | Hint engagement |",
        "|---|---|---|---|---|---|",
    ]
    for sid, entry in sessions.items():
        live_done = len(entry["live_first"])
        top = entry["actions"].most_common(8)
        top_txt = ", ".join(f"`{a}`×{c}" for a, c in top) if top else "—"
        rows = hint_engagement(entry)
        hint_txt = ", ".join(f"`{s}` {sh}/{di}/{ac}" for s, sh, di, ac in rows) if rows else "none recorded"
        lines.append(
            f"| `{sid}` | {entry['events']} | {entry['max_day']} "
            f"| {live_done}/{len(LIVE_STEPS)} | {top_txt} | {hint_txt} |"
        )
    lines.append("")
    return "\n".join(lines)


def write_selftest_fixture(path):
    rows = []
    day = 1
    order = [s[2] for s in LIVE_STEPS]
    for index, sigil in enumerate(order):
        stage_id = LIVE_STEPS[index][0]
        verb = LIVE_VERBS[index]
        # P004 — the panel presents the stage hint before the action.
        rows.append({"record_type": "action", "session_id": "selftest", "day": day,
                     "t_session_ms": index * 1000, "action": "hint_shown", "sigil": "",
                     "target_id": stage_id, "kind": "observed"})
        # P002 — the observed sigil and its published verb.
        rows.append({"record_type": "action", "session_id": "selftest", "day": day,
                     "t_session_ms": index * 1000 + 1, "action": "sigil", "sigil": sigil,
                     "target_id": sigil, "kind": ""})
        rows.append({"record_type": "action", "session_id": "selftest", "day": day,
                     "t_session_ms": index * 1000 + 2, "action": verb, "sigil": "",
                     "target_id": stage_id, "kind": "observed"})
        if index == 1:
            rows.append({"record_type": "action", "session_id": "selftest", "day": day,
                         "t_session_ms": index * 1000 + 3, "action": "hint_dismissed",
                         "sigil": "", "target_id": stage_id, "kind": "observed"})
        if index == 2:
            day = 2
    rows.append({"record_type": "day_join", "session_id": "selftest", "day": 2,
                 "t_session_ms": 9000, "action": "day_advanced", "sigil": "",
                 "target_id": "campaign_day_coordinator", "kind": ""})
    with open(path, "w", encoding="utf-8") as handle:
        for row in rows:
            handle.write(json.dumps(row) + "\n")


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--jsonl", action="append", default=[])
    parser.add_argument("--discover", action="store_true", help="auto-discover Godot user:// JSONL files")
    parser.add_argument("--out", default=None)
    parser.add_argument("--append-delta", default=None, metavar="PATH",
                        help="P001 — append a dated delta section to an existing report file")
    parser.add_argument("--selftest", action="store_true")
    args = parser.parse_args()

    paths = list(args.jsonl)
    if args.discover:
        pattern = os.path.expanduser("~/.local/share/godot/app_userdata/*/**/*.jsonl")
        paths.extend(sorted(glob.glob(pattern, recursive=True)))

    if args.selftest:
        fixture = "/tmp/ashfall_first_hour_funnel_selftest.jsonl"
        write_selftest_fixture(fixture)
        sessions = analyse(load_events([fixture]))
        entry = sessions.get("selftest")
        verbs_ok = entry is not None and all(v in entry["actions"] for v in LIVE_VERBS)
        hints_ok = entry is not None and entry["hints_shown"].get("water", 0) >= 1 \
            and entry["hints_dismissed"].get("power", 0) >= 1
        ok = entry is not None and len(entry["live_first"]) == len(LIVE_STEPS) and verbs_ok and hints_ok
        report = render(sessions)
        print(report)
        print("FUNNEL_SELFTEST", "PASS" if ok else "FAIL",
              f"({len(entry['live_first']) if entry else 0}/{len(LIVE_STEPS)} steps, "
              f"verbs={'ok' if verbs_ok else 'MISSING'}, hints={'ok' if hints_ok else 'MISSING'})")
        return 0 if ok else 1

    if not paths:
        print("no JSONL paths given (use --jsonl or --discover); see --selftest", file=sys.stderr)
        return 2

    sessions = analyse(load_events(paths))
    if args.append_delta:
        with open(args.append_delta, "a", encoding="utf-8") as handle:
            handle.write(render_delta(sessions))
        print(f"delta appended to {args.append_delta}")
        return 0

    report = render(sessions)
    if args.out:
        with open(args.out, "w", encoding="utf-8") as handle:
            handle.write(report + "\n")
        print(f"written {args.out}")
    else:
        print(report)
    return 0


if __name__ == "__main__":
    sys.exit(main())
