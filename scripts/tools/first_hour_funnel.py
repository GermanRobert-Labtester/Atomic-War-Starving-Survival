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
"""
import argparse
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
        })
        entry["events"] += 1
        entry["max_day"] = max(entry["max_day"], int(evt.get("day", 0) or 0))
        action = str(evt.get("action", "") or "")
        if action:
            entry["actions"][action] += 1
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
        lines.append("")
    return "\n".join(lines)


def write_selftest_fixture(path):
    rows = []
    day = 1
    order = [s[2] for s in LIVE_STEPS]
    for index, sigil in enumerate(order):
        rows.append({"record_type": "action", "session_id": "selftest", "day": day,
                     "t_session_ms": index * 1000, "action": "sigil", "sigil": sigil,
                     "target_id": "", "kind": ""})
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
        ok = entry is not None and len(entry["live_first"]) == len(LIVE_STEPS)
        report = render(sessions)
        print(report)
        print("FUNNEL_SELFTEST", "PASS" if ok else "FAIL",
              f"({len(entry['live_first']) if entry else 0}/{len(LIVE_STEPS)} steps)")
        return 0 if ok else 1

    if not paths:
        print("no JSONL paths given (use --jsonl or --discover); see --selftest", file=sys.stderr)
        return 2

    report = render(analyse(load_events(paths)))
    if args.out:
        with open(args.out, "w", encoding="utf-8") as handle:
            handle.write(report + "\n")
        print(f"written {args.out}")
    else:
        print(report)
    return 0


if __name__ == "__main__":
    sys.exit(main())
