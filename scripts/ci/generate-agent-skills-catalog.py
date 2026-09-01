#!/usr/bin/env python3
"""
generate-agent-skills-catalog.py — Multi-Agent Skills Registry & Taxonomy Generator

Scans all 80+ specialized skills in .agents/skills/*/SKILL.md, parses their YAML
frontmatter, and generates docs/agents/AGENT_SKILLS_INDEX.md organized by functional domain.

Usage:
  python3 scripts/ci/generate-agent-skills-catalog.py          # Regenerates AGENT_SKILLS_INDEX.md
  python3 scripts/ci/generate-agent-skills-catalog.py --check  # Verifies 0 drift in CI
"""

import re
import sys
import pathlib
from datetime import datetime, timezone

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
SKILLS_DIR = REPO_ROOT / ".agents" / "skills"
OUTPUT_FILE = REPO_ROOT / "docs" / "agents" / "AGENT_SKILLS_INDEX.md"

TAXONOMY_MAP = {
    "Forensics & Subsystem Audits": [
        "ashfall-analyze", "ashfall-audit", "ashfall-catalog-audit", "ashfall-determinism-scan",
        "ashfall-codehealth-sweep", "ashfall-dependency-map", "ashfall-input-map-audit",
        "ashfall-nuget-audit", "ashfall-dialog-graph-lint", "ashfall-tutorial-review",
        "ashfall-scan", "ashfall-test-gap", "ashfall-shader-material-lint"
    ],
    "Architecture & Integration Planning": [
        "ashfall-plan", "ashfall-prioritize", "ashfall-task-frame", "ashfall-optimize",
        "ashfall-decompose-godot", "ashfall-docs-atlas", "ashfall-mod-contract",
        "ashfall-integrate", "ashfall-design"
    ],
    "Expansions & Content Systems": [
        "ashfall-expand", "ashfall-expansion-scaffold", "ashfall-expansion-phase",
        "ashfall-expansion-save-evolve", "ashfall-expansion-tick-wire", "ashfall-expansion-balance-pack",
        "ashfall-expansion-data-gen", "ashfall-expansion-id-lint", "ashfall-expansion-narrative-weave",
        "ashfall-expansion-qa-playthrough", "ashfall-data-add", "ashfall-data-schema",
        "ashfall-foundry", "ashfall-write", "ashfall-narrative-check", "ashfall-narrative-continuity"
    ],
    "UI, Panels & Visual Presentation": [
        "ashfall-ui-access", "ashfall-ui-expansion-panel-kit", "ashfall-snapshot-guard",
        "ashfall-snapshot-diff", "ashfall-godot-scene-lint", "ashfall-godot-patterns",
        "ashfall-wire", "ashfall-shader-expansion-fx", "ashfall-sprite-family-gen",
        "ashfall-tilemap-expansion-kit", "ashfall-tilemap-world-qa"
    ],
    "Audio, Localization & Asset Pipeline": [
        "ashfall-audio-qa", "ashfall-audio-expansion-pack", "ashfall-asset-migration-batch",
        "ashfall-asset-pack-expansion", "ashfall-scene-port", "ashfall-localize",
        "ashfall-lfs-gate", "ashfall-string-extractor", "ashfall-export-build"
    ],
    "Determinism, Save Resilience & Hardening": [
        "ashfall-save-fuzz", "ashfall-seed-replay", "ashfall-save-roundtrip",
        "ashfall-save-migration", "ashfall-determinism-guard", "ashfall-coverage-gate",
        "ashfall-harden", "ashfall-seal", "ashfall-repair", "ashfall-hotfix-rollback",
        "ashfall-release-captain", "ashfall-repo-hygiene", "ashfall-ci-migrate",
        "ashfall-balance-sim", "ashfall-equipment-balance", "ashfall-headless-demo",
        "ashfall-telemetry-playtest", "ashfall-test-fixture", "ashfall-tune",
        "ashfall-agents-sync"
    ]
}


def parse_skills():
    if not SKILLS_DIR.is_dir():
        print(f"Error: {SKILLS_DIR} not found.", file=sys.stderr)
        sys.exit(1)

    skills = {}
    for skill_path in sorted(SKILLS_DIR.iterdir()):
        if not skill_path.is_dir():
            continue
        skill_file = skill_path / "SKILL.md"
        if not skill_file.is_file():
            continue

        content = skill_file.read_text(encoding="utf-8")
        name_match = re.search(r"^name:\s*([^\n]+)", content, re.MULTILINE)
        desc_match = re.search(r"^description:\s*([^\n]+)", content, re.MULTILINE)

        skill_name = name_match.group(1).strip() if name_match else skill_path.name
        description = desc_match.group(1).strip() if desc_match else "No description provided."

        # Clean quotes
        description = description.strip('"\'')

        skills[skill_name] = {
            "name": skill_name,
            "folder": skill_path.name,
            "description": description,
            "rel_path": f".agents/skills/{skill_path.name}/SKILL.md"
        }

    return skills


def generate_skills_index_markdown(skills) -> str:
    lines = [
        "# ASHFALL Multi-Agent Skills Registry & Taxonomy Index",
        "",
        "> **Living Multi-Agent Navigation Guide**: Documents all specialized agent skills in `.agents/skills/` organized across 6 functional capability domains. Used by Antigravity, Claude, Codex, Cline, Cursor, and Windsurf AI agents for instant tool discovery.",
        "",
        f"**Total Registered Skills:** `{len(skills)}`<br>",
        f"**Last Verified:** `{datetime.now(timezone.utc).strftime('%Y-%m-%d')}`<br>",
        "**Drift Gated:** `python3 scripts/ci/generate-agent-skills-catalog.py --check`",
        "",
        "---",
        "",
        "## Domain Taxonomy Navigation",
        "",
    ]

    for cat_name in TAXONOMY_MAP.keys():
        anchor = cat_name.lower().replace(" ", "-").replace("&", "").replace(",", "")
        lines.append(f"- [{cat_name}](#{anchor})")

    lines.append("")
    lines.append("---")
    lines.append("")

    accounted_skills = set()

    for cat_name, skill_names in TAXONOMY_MAP.items():
        lines.append(f"## {cat_name}")
        lines.append("")
        lines.append("| Skill Name | Purpose & Trigger | Location |")
        lines.append("|---|---|---|")

        for sname in skill_names:
            if sname in skills:
                s = skills[sname]
                accounted_skills.add(sname)
                lines.append(f"| [`{s['name']}`](../../{s['rel_path']}) | {s['description']} | `{s['rel_path']}` |")

        lines.append("")

    # Uncategorized catch-all if new skills were added
    remaining = [s for s in skills.values() if s["name"] not in accounted_skills]
    if remaining:
        lines.append("## Additional General Skills")
        lines.append("")
        lines.append("| Skill Name | Purpose & Trigger | Location |")
        lines.append("|---|---|---|")
        for s in remaining:
            lines.append(f"| [`{s['name']}`](../../{s['rel_path']}) | {s['description']} | `{s['rel_path']}` |")
        lines.append("")

    # Strip trailing empty lines before appending single final newline
    text = "\n".join(lines).rstrip() + "\n"
    return text


def main():
    check_mode = "--check" in sys.argv
    skills = parse_skills()
    generated_md = generate_skills_index_markdown(skills)

    if check_mode:
        if not OUTPUT_FILE.is_file():
            print(f"FAIL: {OUTPUT_FILE} does not exist. Run python3 scripts/ci/generate-agent-skills-catalog.py", file=sys.stderr)
            sys.exit(1)
        current_md = OUTPUT_FILE.read_text(encoding="utf-8")
        if current_md.strip() != generated_md.strip():
            print(f"FAIL: {OUTPUT_FILE} is out of date. Run python3 scripts/ci/generate-agent-skills-catalog.py", file=sys.stderr)
            sys.exit(1)
        print(f"OK: {OUTPUT_FILE} is in sync with .agents/skills/ ({len(skills)} skills).")
        sys.exit(0)

    OUTPUT_FILE.parent.mkdir(parents=True, exist_ok=True)
    OUTPUT_FILE.write_text(generated_md, encoding="utf-8")
    print(f"Wrote {OUTPUT_FILE} ({len(skills)} agent skills cataloged).")


if __name__ == "__main__":
    main()
