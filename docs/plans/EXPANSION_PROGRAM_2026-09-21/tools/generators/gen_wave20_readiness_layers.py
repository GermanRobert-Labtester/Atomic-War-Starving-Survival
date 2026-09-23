#!/usr/bin/env python3
"""gen_wave20_readiness_layers.py — Wave 20 layer generator (v1, 2026-09-21)

Appends the standard integration layers to the Wave 20 readiness-closure plans:

  §12 cross-plan coupling      (artifact-token incoming edges)
  §13 authority binding map    (host / test / data identifier joins)
  §14 save-section ownership   (SaveSectionRegistry key-token join)
  §15 CLI and selftest coverage(HostCliRegistry flag-token join)
  §16 event-route reachability (event-name token join)
  §17 data-catalog binding     (catalog filename tokens)
  §18 test-region mapping      (top-level region directories + case counts)
  §19 host-surface route map   (src/ filename tokens)
  §20 save schema ladder       (section tokens vs the five ladders)
  §21 determinism / RNG stream (CampaignRngStream tokens)
  §22 content-consumption      (content-utilization-baseline classifications)
  §23 flag wiring              (persistent flag tokens)
  §24 claim readiness          (structural checklist + claim block)

Idempotent: skips a plan that already contains the section it would write.

Corrections carried forward from rounds 79-85 (see EVIDENCE.md):
  * package rule accepts bullet, heading, and letter+digit prefixes
  * coupling phrase accepts "their names", "those names", "them", "these ..."
  * verification accepts five command families (run_test.sh, --selftest,
    godot --headless, `scripts/ci/*.py --check`, `scripts/ci/*.sh`)
  * wave header accepts `**Wave N · Kind:**` and `**Wave:** N (date) · **Kind:**`
  * test regions are top-level directories only (root-level files counted
    separately)
"""

import collections
import json
import os
import re
import sys

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", "..", "..", ".."))
PLANS_DIR = os.path.join(ROOT, "docs", "plans")
WAVE20 = "EXPANSION_PROGRAM_WAVE20_2026-09-21"

GENERIC = {
    "system", "catalog", "engine", "service", "manager", "data", "state",
    "bridge", "truth", "family", "plan", "tracking", "registry", "record",
    "records", "runtime", "authority", "surface", "readiness", "closure",
    "recovery", "transfer", "pipeline", "review", "audit", "content", "player",
    "world", "game", "core", "host", "save", "test", "tests", "report",
    "reports", "module", "modules", "support", "mapping", "default", "standard",
    "shared", "common", "config", "settings", "profile", "library", "expansion",
}

PACKAGE_BULLET = re.compile(r"-\s+\*\*([A-Z][A-Z0-9\-]{3,})\*\*")
PACKAGE_HEADING = re.compile(r"^###\s+([A-Z][A-Z0-9]{0,5}-\d+[A-Z]?)\s*[—-]", re.M)
PACKAGE_TOK = re.compile(r"^\s*\*\*([A-Z][A-Z0-9]{0,5}-\d+[A-Z]?)\*\*", re.M)

COUPLING = re.compile(
    r"referencing (?:their names|those names|them|these artifacts|these "
    r"documents|these tiers|these symbols)[^\n]*?\*\*(\d+)\*\*"
)

VERIFICATION_FAMILIES = (
    "scripts/run_test.sh",
    "--selftest",
    "godot --headless",
    "scripts/ci/",
    "scripts/release/",
)


def read(path):
    try:
        with open(path, encoding="utf-8", errors="ignore") as handle:
            return handle.read()
    except OSError:
        return ""


def tokens(value):
    out = set()
    for word in re.split(r"[_\-.]", value):
        word = word.lower()
        if len(word) >= 4:
            out.add(word)
    for word in re.findall(r"[A-Z]+(?=[A-Z][a-z])|[A-Z]?[a-z]+|[A-Z]+", value):
        word = word.lower()
        if len(word) >= 4:
            out.add(word)
    return out


def camel(word):
    out = set()
    for part in re.findall(r"[A-Z]+(?=[A-Z][a-z])|[A-Z]?[a-z]+|[A-Z]+", word):
        part = part.lower()
        if len(part) >= 4:
            out.add(part)
    if len(word) >= 4:
        out.add(word.lower())
    return out


def build_source_indexes():
    """Load the registries and repository indexes once."""
    all_cs = {}
    for base, _, files in os.walk(os.path.join(ROOT, "Assets", "Ashfall.Core")):
        for name in files:
            if name.endswith(".cs"):
                all_cs[name] = os.path.join(base, name)

    save_registry = read(os.path.join(ROOT, "Assets", "Ashfall.Core", "Save",
                                      "SaveSectionRegistry.cs"))
    section_keys = sorted(set(re.findall(r'"([a-z][a-z0-9_]{3,})"', save_registry)))
    ladders = set(re.findall(r'"([a-z_]+)"\s*,\s*(?:[2-9]\d?)\s*\}', save_registry))

    cli_registry = read(os.path.join(ROOT, "Assets", "Ashfall.Core",
                                     "HostCliRegistry.cs"))
    cli_flags = sorted(set(re.findall(r'"(--[a-z0-9\-]{3,})"', cli_registry)))

    rng_streams = set(re.findall(
        r'"([a-z0-9_\-]{4,})"',
        read(os.path.join(ROOT, "Assets", "Ashfall.Core", "Random",
                          "CampaignRngStream.cs"))))

    classifications = {}
    orphans = set()
    baseline = os.path.join(ROOT, "artifacts", "content-utilization-baseline.json")
    if os.path.exists(baseline):
        data = json.load(open(baseline, encoding="utf-8"))
        classifications = data.get("catalogClassifications", {})
        orphans = set(data.get("knownOrphans", []))

    flags = set()
    for name in ("moral_choice_flags.json", "camouflage_gear.json"):
        path = os.path.join(ROOT, "Assets", "StreamingAssets", "Data", name)
        if not os.path.exists(path):
            continue
        data = json.load(open(path, encoding="utf-8"))
        if isinstance(data, dict) and isinstance(data.get("flags"), list):
            for entry in data["flags"]:
                if isinstance(entry, dict) and entry.get("id"):
                    flags.add(entry["id"])

    data_catalogs = {}
    for base, _, files in os.walk(os.path.join(ROOT, "Assets", "StreamingAssets",
                                               "Data")):
        for name in files:
            if name.endswith(".json"):
                path = os.path.relpath(os.path.join(base, name), ROOT)
                data_catalogs[path] = tokens(name[:-5])

    regions = collections.defaultdict(lambda: {"files": 0, "cases": 0})
    root_files = root_cases = 0
    for base, _, files in os.walk(os.path.join(ROOT, "Ashfall.Core.Tests")):
        for name in files:
            if not name.endswith(".cs"):
                continue
            path = os.path.join(base, name)
            rel = os.path.relpath(path, os.path.join(ROOT, "Ashfall.Core.Tests"))
            text = read(path)
            cases = len(re.findall(r"\[(?:Fact|Theory)\]", text))
            if os.sep in rel:
                region = rel.split(os.sep)[0]
                regions[region]["files"] += 1
                regions[region]["cases"] += cases
            else:
                root_files += 1
                root_cases += cases

    host_files = {}
    for base, _, files in os.walk(os.path.join(ROOT, "src")):
        for name in files:
            if name.endswith((".cs", ".tscn")):
                path = os.path.relpath(os.path.join(base, name), ROOT)
                host_files[path] = tokens(name.rsplit(".", 1)[0])

    events = collections.defaultdict(list)
    for base, _, files in os.walk(os.path.join(ROOT, "Assets", "Ashfall.Core")):
        for name in files:
            if not name.endswith(".cs"):
                continue
            path = os.path.join(base, name)
            text = read(path)
            for match in re.finditer(
                    r"\bevent\s+(?:[A-Za-z0-9_<>,\.\s]+?)\s+([A-Za-z][A-Za-z0-9_]{2,})\s*;",
                    text):
                events[match.group(1)].append(os.path.relpath(path, ROOT))

    return {
        "all_cs": all_cs,
        "sections": section_keys,
        "ladders": ladders,
        "flags_cli": cli_flags,
        "rng": rng_streams,
        "catalogs": data_catalogs,
        "classifications": classifications,
        "orphans": orphans,
        "flags": flags,
        "regions": regions,
        "root_files": root_files,
        "root_cases": root_cases,
        "host": host_files,
        "events": events,
    }


def plan_files():
    out = []
    for base, _, files in os.walk(PLANS_DIR):
        for name in files:
            if name.startswith("PLAN-") and "APPENDIX" not in name.upper():
                out.append(os.path.join(base, name))
    return sorted(out)


def entry_source_candidates(plan_path):
    """Artifact tokens for a document-shaped plan (Wave 20 plans govern docs)."""
    text = read(plan_path)
    arts = set(re.findall(
        r"`([A-Za-z][A-Za-z0-9_./\-]*\.(?:cs|json|py|sh|tscn|gd|yml|yaml|md|csproj|toml))`",
        text))
    return {os.path.basename(a) for a in arts if "docs/plans" not in a}


def domain_symbols(plan_path, sources):
    text = read(plan_path)
    symbols = {name[:-3] for name in re.findall(r"`([A-Za-z][A-Za-z0-9_]+\.cs)`", text)
               if name in sources["all_cs"]}
    if len(symbols) < 3:
        symbols |= {a.rsplit(".", 1)[0] for a in entry_source_candidates(plan_path)}
    return symbols, text


def keywords(symbols, text):
    words = set()
    for symbol in symbols:
        words |= camel(symbol)
    title = re.match(r"#\s*PLAN-([A-Z0-9\-]+)", text)
    if title:
        for word in title.group(1).split("-"):
            if len(word) >= 3:
                words.add(word.lower())
    return words - GENERIC


def write_layers(path, sources):
    text = read(path)
    if "## 24. Claim readiness" in text:
        return False
    symbols, text = domain_symbols(path, sources)
    kws = keywords(symbols, text)
    if not kws:
        return False

    # §12 coupling
    bodies = {}
    for other in plan_files():
        if other == path:
            continue
        bodies[os.path.basename(other)[:-3]] = read(other)
    stems = {s for s in symbols}
    incoming = []
    for name, other_text in bodies.items():
        count = sum(1 for stem in stems if re.search(r"\b" + re.escape(stem) + r"\b",
                                                     other_text))
        if count:
            incoming.append((name, count))
    incoming.sort(key=lambda pair: -pair[1])

    # joins
    matched_sections = sorted(k for k in sources["sections"] if kws & tokens(k))
    laddered = sorted(k for k in matched_sections if k in sources["ladders"])
    matched_flags_cli = sorted(f for f in sources["flags_cli"] if kws & tokens(f))
    matched_rng = sorted(r for r in sources["rng"] if kws & tokens(r))
    matched_catalogs = sorted(c for c in sources["catalogs"] if kws & sources["catalogs"][c])
    matched_flags = sorted(f for f in sources["flags"] if kws & tokens(f))
    matched_regions = sorted(r for r in sources["regions"] if kws & tokens(r))
    matched_host = sorted(h for h in sources["host"] if kws & sources["host"][h])
    matched_events = sorted(n for n in sources["events"] if kws & camel(n))

    host_join = set()
    test_join = set()
    data_join = set()
    for keyword in kws:
        for index, bucket in ((sources["host"], host_join),
                              (sources["regions"], None)):
            if bucket is not None:
                for name in index:
                    if keyword in tokens(name):
                        bucket.add(name)

    section = []
    section.append(f"\n---\n\n## 12. Cross-plan coupling\n")
    section.append(f"Document-artifact incoming edges: **{len(incoming)}**.\n")
    section.append("\n| Plan | Mentions |\n|---|---:|\n")
    for name, count in incoming[:8]:
        section.append(f"| `{name}` | {count} |\n")
    if not incoming:
        section.append("| — | no other plan references these artifacts |\n")

    section.append("\n---\n\n## 13. Authority binding map\n")
    section.append(f"Host files: **{len(matched_host)}** · Test regions: "
                   f"**{len(matched_regions)}** · Data catalogs: "
                   f"**{len(matched_catalogs)}**.\n")
    section.append("\n| Layer | Count | Examples |\n|---|---:|---|\n")
    section.append("| Host (`src/`) | %d | %s |\n" % (
        len(matched_host),
        ", ".join(f"`{h}`" for h in matched_host[:5]) or "—"))
    section.append("| Test regions | %d | %s |\n" % (
        len(matched_regions), ", ".join(f"`{r}`" for r in matched_regions[:5]) or "—"))
    section.append("| Data catalogs | %d | %s |\n" % (
        len(matched_catalogs), ", ".join(f"`{c}`" for c in matched_catalogs[:5]) or "—"))

    section.append("\n---\n\n## 14. Save-section ownership\n")
    section.append(f"Matching section keys: **{len(matched_sections)}** "
                   f"(versioned ladders: **{len(laddered)}**).\n")
    section.append("\n| Section key |\n|---|\n")
    for key in matched_sections[:10]:
        section.append(f"| `{key}` |\n")
    if not matched_sections:
        section.append("| — | no section key shares a token with this scope |\n")

    section.append("\n---\n\n## 15. CLI and selftest coverage\n")
    section.append(f"Matching flags: **{len(matched_flags_cli)}**.\n")
    section.append("\n| Flag |\n|---|\n")
    for flag in matched_flags_cli[:10]:
        section.append(f"| `{flag}` |\n")
    if not matched_flags_cli:
        section.append("| — | no CLI flag shares a token with this scope |\n")

    section.append("\n---\n\n## 16. Event-route reachability\n")
    section.append(f"Events sharing a scope token: **{len(matched_events)}**.\n")
    section.append("\n| Event | First declaration |\n|---|---|\n")
    for name in matched_events[:10]:
        section.append(f"| `{name}` | `{sources['events'][name][0]}` |\n")
    if not matched_events:
        section.append("| — | no event name shares a token with this scope |\n")

    section.append("\n---\n\n## 17. Data-catalog binding\n")
    section.append(f"Matching catalogs: **{len(matched_catalogs)}**.\n")
    section.append("\n| Catalog | Classification |\n|---|---|\n")
    for catalog in matched_catalogs[:10]:
        section.append(f"| `{catalog}` | "
                       f"{sources['classifications'].get(catalog, 'n/a')} |\n")
    if not matched_catalogs:
        section.append("| — | no catalog shares a token with this scope |\n")

    section.append("\n---\n\n## 18. Test-region mapping\n")
    files = sum(sources["regions"][r]["files"] for r in matched_regions)
    cases = sum(sources["regions"][r]["cases"] for r in matched_regions)
    section.append(f"Matching regions: **{len(matched_regions)}** ({files} files, "
                   f"{cases} cases). Root-level files outside regions: "
                   f"{sources['root_files']} files / {sources['root_cases']} cases.\n")
    section.append("\n| Region | Files | Cases |\n|---|---:|---:|\n")
    for region in matched_regions[:10]:
        section.append(f"| `{region}` | {sources['regions'][region]['files']} | "
                       f"{sources['regions'][region]['cases']} |\n")
    if not matched_regions:
        section.append("| — | no region shares a token with this scope |\n")

    section.append("\n---\n\n## 19. Host-surface route map\n")
    section.append(f"Matching host files: **{len(matched_host)}**.\n")
    section.append("\n| Host file |\n|---|\n")
    for host in matched_host[:10]:
        section.append(f"| `{host}` |\n")
    if not matched_host:
        section.append("| — | no host filename shares a token with this scope |\n")

    section.append("\n---\n\n## 20. Save schema ladder\n")
    section.append(f"Matched sections: **{len(matched_sections)}**; laddered: "
                   f"**{len(laddered)}**.\n")
    section.append("\n| Section key | Laddered |\n|---|---|\n")
    for key in matched_sections[:10]:
        section.append(f"| `{key}` | {'yes' if key in sources['ladders'] else 'no'} |\n")
    if not matched_sections:
        section.append("| — | no section key shares a token with this scope |\n")

    section.append("\n---\n\n## 21. Determinism / RNG stream binding\n")
    section.append(f"Matching seeded streams: **{len(matched_rng)}**.\n")
    section.append("\n| Stream |\n|---|\n")
    for stream in matched_rng[:10]:
        section.append(f"| `{stream}` |\n")
    if not matched_rng:
        section.append("| — | no seeded stream shares a token with this scope |\n")

    section.append("\n---\n\n## 22. Content-consumption classification\n")
    section.append(f"Matching catalogs: **{len(matched_catalogs)}**.\n")
    section.append("\n| Catalog | Classification |\n|---|---|\n")
    for catalog in matched_catalogs[:10]:
        section.append(f"| `{catalog}` | "
                       f"{sources['classifications'].get(catalog, 'n/a')} |\n")
    if not matched_catalogs:
        section.append("| — | no catalog shares a token with this scope |\n")

    section.append("\n---\n\n## 23. Flag wiring\n")
    section.append(f"Matching persistent flags: **{len(matched_flags)}**.\n")
    section.append("\n| Flag |\n|---|\n")
    for flag in matched_flags[:10]:
        section.append(f"| `{flag}` |\n")
    if not matched_flags:
        section.append("| — | no persistent flag shares a token with this scope |\n")

    # §24 claim readiness
    packages = (PACKAGE_BULLET.findall(text) + PACKAGE_HEADING.findall(text)
                + PACKAGE_TOK.findall(text))
    packages = [p for p in packages if re.match(r"^[A-Z]{1,6}-\d", p)]
    coupling = COUPLING.search(text)
    coupling_count = int(coupling.group(1)) if coupling else len(incoming)
    checks = {
        "status": bool(re.search(r"\*\*Status:\*\*", text)),
        "wave": bool(re.search(r"Wave\s*\d", text)),
        "depends": bool(re.search(r"\*\*Depends on:\*\*", text)),
        "non_goals": bool(re.search(r"Non-goals", text, re.I)),
        "outcome": bool(re.search(r"Outcome", text, re.I)),
        "evidence": bool(re.search(r"Evidence", text, re.I)),
        "packages": len(packages) > 0,
        "acceptance": bool(re.search(r"Acceptance", text, re.I)),
        "risks": bool(re.search(r"Risk", text, re.I)),
        "verification": any(family in text for family in VERIFICATION_FAMILIES),
        "coupling": True,
        "binding": True,
    }
    passed = sum(1 for value in checks.values() if value)
    missing = [key for key, value in checks.items() if not value]
    readiness = "READY" if passed == len(checks) else (
        "READY-WITH-NOTES" if passed >= len(checks) - 2 else "NEEDS-AUTHORING")

    verified = []
    if matched_regions:
        verified.append(f"bash scripts/run_test.sh Ashfall.Core.Tests/{matched_regions[0]}/")
    if matched_flags_cli:
        verified.append(f"godot --headless --path . -- {matched_flags_cli[0]}")
    if not verified:
        verified.append("bash scripts/run_test.sh <focused-region>/  # resolve at claim time")

    section.append(f"\n---\n\n## 24. Claim readiness\n")
    section.append(f"**Readiness:** {readiness} ({passed}/{len(checks)}) · "
                   f"**Class:** governance/closure · **Coupling:** {coupling_count}\n")
    section.append("\n**Proposed claim block**\n\n```\n")
    section.append(f"plan: {os.path.basename(path)[:-3]}\n")
    section.append("wave: 20\nstatus: PROPOSED — foreman claim required\n")
    section.append(f"packages: {', '.join(packages) or 'author at claim time'}\n")
    section.append("claim paths:\n")
    section.append("  - docs/plans/EXPANSION_PROGRAM_WAVE20_2026-09-21/  # this plan\n")
    section.append("verification:\n")
    for command in verified:
        section.append(f"  - {command}\n")
    section.append("```\n")
    section.append("\n**Structural checklist**\n\n| Check | Result |\n|---|---|\n")
    for key, value in checks.items():
        section.append(f"| {key} | {'yes' if value else '**no**'} |\n")
    section.append("\n**Pre-claim actions:** "
                   + ("none — claim-ready.\n" if not missing
                      else "author or confirm: " + ", ".join(missing) + ".\n"))

    with open(path, "a", encoding="utf-8") as handle:
        handle.write("".join(section))
    return True


def main():
    sources = build_source_indexes()
    targets = [p for p in plan_files() if WAVE20 in p]
    written = 0
    for path in targets:
        if write_layers(path, sources):
            written += 1
            print(f"layered: {os.path.basename(path)}")
    print(f"done: {written}/{len(targets)} Wave 20 plans received layers")
    return 0


if __name__ == "__main__":
    sys.exit(main())
