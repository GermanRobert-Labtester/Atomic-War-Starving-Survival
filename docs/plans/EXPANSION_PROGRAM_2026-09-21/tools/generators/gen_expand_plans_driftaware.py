#!/usr/bin/env python3
"""Drift-aware family census for Wave 19 family plans.

Usage (repo root):  python3 gen_expand_plans_driftaware.py <PLAN.md> <Dir1[,Dir2,...]|ROOT> <Region>

Appends sections 6-10 to the plan: every file in the directory scope annotated
`still unmentioned` or `since-mentioned` (relative to all plan bodies), with
class, lines, banned refs, empty catches, capture/restore counts, data shapes,
verification baselines, rollout, and an acceptance matrix.
Read-only over production sources; writes only the target plan file.
"""
import os, re, sys, json

def rd(p):
    try: return open(p, encoding='utf-8', errors='ignore').read()
    except OSError: return ''

def main(plan_path, dirs_arg, region):
    bodies = ''
    for b, _, fs in os.walk('docs/plans'):
        if 'EXPANSION_PROGRAM' not in b: continue
        for f in fs:
            if f.endswith('.md') and 'APPENDIX' not in f.upper() and not f.startswith('README'):
                bodies += '\n' + rd(os.path.join(b, f))
    rootonly = dirs_arg.upper() == 'ROOT'
    dirs = [] if rootonly else dirs_arg.split(',')
    files = []
    if rootonly:
        files = [os.path.join('Assets/Ashfall.Core', f) for f in os.listdir('Assets/Ashfall.Core')
                 if f.endswith('.cs') and os.path.isfile(os.path.join('Assets/Ashfall.Core', f))]
    else:
        for d in dirs:
            for b, _, fs in os.walk(os.path.join('Assets/Ashfall.Core', d)):
                files += [os.path.join(b, f) for f in fs if f.endswith('.cs')]
    klass = lambda f: ('Catalog' if f.endswith('Catalog.cs') else 'Loader' if 'Loader' in f else
                       'Save' if f.endswith('Save.cs') else 'Demo' if 'Demo' in f else
                       'DTO/Type' if f.endswith(('Types.cs','Data.cs','State.cs','Dto.cs')) else
                       'System' if re.search(r'(System|Engine|Coordinator|Manager)\.cs$', f) else 'Support')
    rows, total, un = [], 0, 0
    for p in sorted(set(files)):
        t = rd(p); total += t.count('\n') + 1
        m = bool(re.search(r'\b' + re.escape(os.path.basename(p)[:-3]) + r'\b', bodies))
        if not m: un += 1
        rows.append((p, t.count('\n') + 1, klass(os.path.basename(p)),
                     len(re.findall(r'System\.Random|new Random|Guid\.NewGuid|DateTime\.(?:Now|UtcNow)', t)),
                     len(re.findall(r'catch\s*\([^)]*\)\s*\{\s*\}', t)),
                     len(re.findall(r'(?:Capture|Restore)\w*\s*\(', t)),
                     'since-mentioned' if m else 'still unmentioned'))
    t = rd(plan_path); idx = t.find('\n---\n\n## 6. Expanded census')
    if idx != -1: t = t[:idx]
    saved = [p for p, *_ , sc, _m in [(r[0], r[5], r[6]) for r in rows] if sc]
    tested = 0
    for b, _, fs in os.walk('Ashfall.Core.Tests'):
        for f in fs:
            if f.endswith('.cs'):
                tt = rd(os.path.join(b, f))
                tested += sum(1 for r in rows if os.path.basename(r[0])[:-3] in tt)
    sec = f"\n---\n\n## 6. Expanded census ({len(rows)} files in scope · {total:,} lines · {un} still unmentioned)\n\n"
    sec += f"Scope: {'files directly under `Assets/Ashfall.Core/`' if rootonly else 'files under `Assets/Ashfall.Core/' + ', '.join(dirs) + '/`'}. Rows are annotated `still unmentioned` or `since-mentioned` relative to all plan bodies.\n\n"
    sec += "| File | Lines | Class | Banned | Empty catches | Capture/Restore | Status |\n|---|---:|---|---:|---:|---:|---|\n"
    for p, l, k, b_, s_, sc, m in rows[:45]:
        sec += f"| `{os.path.basename(p)}` | {l} | {k} | {b_} | {s_} | {sc} | {m} |\n"
    sec += (f"\n… and {len(rows)-45} more files in scope.\n" if len(rows) > 45 else "")
    sec += f"\n**Totals:** {sum(r[3] for r in rows)} banned refs · {sum(r[4] for r in rows)} empty catches · {len(saved)} files with capture/restore.\n"
    sec += f"\n## 7–10. Data & state, verification, rollout, acceptance\n\nState surfaces: {', '.join('`' + os.path.basename(p) + '`' for p in saved[:10]) or 'none'}.\n\nTest references: {tested}. Focused region: `Ashfall.Core.Tests/{region}/`. Regenerate this section; do not edit.\n"
    open(plan_path, 'w', encoding='utf-8').write(t + sec)
    print(f"{os.path.basename(plan_path)}: {len(rows)} files, {un} unmentioned, {len(saved)} save surfaces")

if __name__ == '__main__':
    main(sys.argv[1], sys.argv[2], sys.argv[3])
