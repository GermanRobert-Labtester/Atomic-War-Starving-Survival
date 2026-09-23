import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
TARGETS=[
 ('docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-COMBAT-FAMILY-TRUTH-273.md',['Combat'],['combat','weapon','tactical','ballistic','breach']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-FOUNDRY-FAMILY-TRUTH-278.md',['Foundry'],['foundry','silentfoundry']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-ECONOMY-DATA-FAMILY-TRUTH-270.md',['Economy'],['economy','trade','caravan','market','goods','factionstance']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-MUSTER-FAMILY-TRUTH-275.md',['Muster'],['muster','factionculture','factionecology']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-PERF-HARNESS-FAMILY-TRUTH-279.md',['Performance'],['perf']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-SHELTER-FAMILY-TRUTH-265.md',['Shelter'],['catalog','cascade']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-WORLD-FAMILY-TRUTH-267.md',['World'],['route','anomaly','radar','seasonal','map']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-RADIO-FAMILY-TRUTH-266.md',['Radio'],['factionradio','psyops','recording','uvcorona']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-UI-CONTRACT-FAMILY-TRUTH-277.md',['UI'],['panel','surface','modal','confirmation','presentation']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-MORALCHOICE-LOADER-FAMILY-TRUTH-276.md',['MoralChoice'],['moralchoice']),
]
data_pairs=[(f,os.path.join(b,f)) for b,_,fs in os.walk('Assets/StreamingAssets/Data') for f in fs if f.endswith('.json')]
def rec(p):
    try:
        d=json.load(open(p,encoding='utf-8'))
        if isinstance(d,list): return f"array[{len(d)}]"
        if isinstance(d,dict):
            for k in ('records','entries','items','rows','definitions','waves'):
                if isinstance(d.get(k),list): return f"array[{len(d[k])}]"
            return f"object[{len(d)} keys]"
    except Exception: return 'unreadable'
    return '—'
def klass(fn):
    f=fn.lower()
    if f.endswith('catalog.cs'): return 'Catalog'
    if 'loader' in f: return 'Loader'
    if f.endswith('save.cs'): return 'Save'
    if 'demo' in f: return 'Demo'
    if f.endswith('types.cs') or f.endswith('data.cs') or f.endswith('state.cs') or f.endswith('dto.cs') or 'ids' in f: return 'DTO/Type'
    if re.search(r'(system|engine|coordinator|manager)\.cs$',f): return 'System'
    return 'Support'
for plan_path,dirs,tokens in TARGETS:
    t=rd(plan_path)
    if '## 10. Acceptance matrix' in t: print('already expanded:',os.path.basename(plan_path)); continue
    files=[]
    for d in dirs:
        root=os.path.join('Assets/Ashfall.Core',d)
        if not os.path.isdir(root): continue
        for b,_,fs in os.walk(root):
            for f in fs:
                if f.endswith('.cs'): files.append(os.path.join(b,f))
    # add src matches by token
    for b,_,fs in os.walk('src'):
        for f in fs:
            if f.endswith('.cs') and any(tk in f.lower() for tk in tokens): files.append(os.path.join(b,f))
    # add Core files by token if dirs empty-match
    for b,_,fs in os.walk('Assets/Ashfall.Core'):
        for f in fs:
            if f.endswith('.cs') and any(tk in f.lower() for tk in tokens) and not any(os.path.join(b,f).startswith(os.path.join('Assets/Ashfall.Core',d)) for d in dirs):
                files.append(os.path.join(b,f))
    files=sorted(set(files))
    rows=[]
    total_lines=0
    for p in files:
        tt=rd(p); lines=tt.count('\n')+1; total_lines+=lines
        rows.append((p,lines,klass(os.path.basename(p)),
                     len(re.findall(r'System\.Random|new Random|Guid\.NewGuid|DateTime\.(?:Now|UtcNow)',tt)),
                     len(re.findall(r'catch\s*\{',tt))+len(re.findall(r'catch\s*\(\s*\w+\s*\)\s*\{\s*\}',tt)),
                     len(re.findall(r'(?:Capture|Restore)\w*\s*\(',tt))))
    classes={}
    for _,_,k,_,_,_ in rows: classes[k]=classes.get(k,0)+1
    save_files=[p for p,l,k,_,_,sc in rows if sc]
    cats=[(f,p) for f,p in data_pairs if any(tk in f.lower() for tk in tokens)][:8]
    test_refs=0
    for b,_,fs in os.walk('Ashfall.Core.Tests'):
        for f in fs:
            if f.endswith('.cs'):
                tt=rd(os.path.join(b,f))
                test_refs+=sum(1 for p,_,_,_,_,_ in rows if os.path.basename(p)[:-3] in tt)
    banned=sum(r[3] for r in rows); swal=sum(r[4] for r in rows)
    sec=f"""
---

## 6. Expanded census ({len(files)} files · {total_lines:,} lines)

Class distribution: {' · '.join(f"{k} {v}" for k,v in sorted(classes.items(),key=lambda kv:-kv[1]))}

| File | Lines | Class | Banned refs | Swallow catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
"""
    for p,l,k,b_,s_,sc in rows[:50]:
        sec+=f"| `{os.path.basename(p)}` | {l} | {k} | {b_} | {s_} | {sc} |\n"
    if len(rows)>50: sec+=f"\n… and {len(rows)-50} more family files.\n"
    sec+=f"""
**Census totals:** {banned} banned nondeterministic references · {swal} swallow-catch sites · {len(save_files)} files with capture/restore methods.

## 7. Expanded data & state surface

"""
    if cats:
        sec+="| Catalog | Shape |\n|---|---|\n"
        for f,p in cats: sec+=f"| `{f}` | {rec(p)} |\n"
    else:
        sec+="No family catalog matched by name; treat data paths as loader-injected and verify per file.\n"
    sec+="\n**State surfaces (capture/restore present):**\n\n"
    if save_files: sec+='\n'.join(f"- `{os.path.basename(p)}`" for p in save_files[:15])+'\n'
    else: sec+="None — the family is data/support only; no save work is implied.\n"
    sec+=f"""
## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/{dirs[0] if dirs else 'Performance'}/` |
| Existing test references | {test_refs} file-name references found across the test tree |
| Determinism scan | {banned} banned references to classify (fix or justify) |
| Failure scan | {swal} swallow-catch sites to route to Plan 35's rules |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (this section) — classify every file; no edits.
2. Data/loader fixes: catalogs and loaders whose consumer is missing (typed failures, no silent defaults).
3. Type/DTO validation: round-trips and unknown-value failures.
4. System boundaries: confirm each system's owner and remove duplicated state.
5. Demos/tooling: verify every demo resolves to a real verb (Plan 86 pattern).
6. Regression: re-run the focused region plus the family's own census generator.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; a malformed fixture fails typed |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore pair round-trips; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing CLI verb or is retired |
| Support | either consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope or create new authorities.
"""
    open(plan_path,'a',encoding='utf-8').write(sec)
    print(f"{os.path.basename(plan_path)}: +{len(sec)} chars (files {len(files)}, lines {total_lines}, catalogs {len(cats)}, save surfaces {len(save_files)}, tests {test_refs})")
