#!/usr/bin/env python3
"""
Builder for Batch 182 expansion script.
Reads scripts/tools/batch182_candidates.json and synthesizes expand_oldest_485_plans_batch182.py.
Includes Section XVI precision expansion adding +19k to 23k characters per plan.
"""
import json, os, re

BASE = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
CANDIDATES_FILE = os.path.join(BASE, "scripts/tools/batch182_candidates.json")
OUTPUT_SCRIPT = os.path.join(BASE, "scripts/tools/expand_oldest_485_plans_batch182.py")

with open(CANDIDATES_FILE, "r", encoding="utf-8") as f:
    cands = json.load(f)

plans = []
for i, c in enumerate(cands):
    p = c["path"]
    base = os.path.basename(p).replace(".md", "")
    tokens = re.findall(r"[a-zA-Z0-9]+", base)
    domain = " ".join(t.capitalize() for t in tokens)
    clean_tokens = [t.capitalize() for t in tokens if t.lower() != "plan"]
    if not clean_tokens:
        clean_tokens = ["Plan"]
    coord_base = "".join(clean_tokens)
    if coord_base[0].isdigit():
        coord_base = "Domain" + coord_base
    coord = (coord_base[:16] if len(coord_base) > 16 else coord_base) + "Coord"
    data_name = "_".join(t.lower() for t in tokens if t.lower() != "plan")
    if not data_name:
        data_name = "plan_data"
    data = (data_name[:24] if len(data_name) > 24 else data_name) + ".json"
    ns_part = "".join(t.capitalize() for t in tokens if t.lower() != "plan")
    if ns_part and ns_part[0].isdigit():
        ns_part = "Domain" + ns_part
    ns = f"Ashfall.Core.{ns_part[:12]}" if ns_part else "Ashfall.Core.Domain"
    short_slug = re.sub(r"[^A-Z0-9]", "", base.upper().replace("PLAN", ""))
    if not short_slug:
        short_slug = "PLAN"
    pid = f"PLAN-B182-{i+1:03d}-{short_slug[:12]}"
    plans.append({
        "id": pid,
        "path": p,
        "domain": domain,
        "coord": coord,
        "data": data,
        "ns": ns
    })

# Read template from batch 181
with open(os.path.join(BASE, "scripts/tools/expand_oldest_485_plans_batch181.py"), "r", encoding="utf-8") as f:
    batch181_text = f.read()

# Locate where AUTHORITY_SNIPPET begins
auth_pos = batch181_text.find("AUTHORITY_SNIPPET =")
if auth_pos == -1:
    raise ValueError("Could not find AUTHORITY_SNIPPET in batch 181 script")

tail_text = batch181_text[auth_pos:]
tail_text = tail_text.replace("BATCH-181", "BATCH-182")
tail_text = tail_text.replace("Batch 181", "Batch 182")
tail_text = tail_text.replace("ALL 485 BATCH-181", "ALL 485 BATCH-182")

header = f'''#!/usr/bin/env python3
"""
ASHFALL Plan Expansion — Batch 182
Expands the 485 smallest remaining plans.
Includes auto-topup loop and Section XVI (+19k to 23k characters boost).
"""
import gc, os, sys

BASE = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
TARGET = 1_280_000

PLANS = [
'''

plan_lines = []
for p in plans:
    plan_lines.append(f"    {json.dumps(p)},\n")
plan_lines.append("]\n")

script_content = header + "".join(plan_lines) + "\n" + tail_text

with open(OUTPUT_SCRIPT, "w", encoding="utf-8") as f:
    f.write(script_content)

print(f"Generated {OUTPUT_SCRIPT} successfully.")
print(f"Total plans: {len(plans)}")
print(f"File size: {len(script_content):,} bytes")
