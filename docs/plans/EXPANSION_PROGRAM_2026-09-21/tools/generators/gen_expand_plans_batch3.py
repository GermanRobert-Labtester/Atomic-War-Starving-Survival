import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
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
def klass(fn):
    f=fn.lower()
    if f.endswith('catalog.cs'): return 'Catalog'
    if 'loader' in f: return 'Loader'
    if f.endswith('save.cs'): return 'Save'
    if 'demo' in f: return 'Demo'
    if f.endswith(('types.cs','data.cs','state.cs','dto.cs')) or 'ids' in f: return 'DTO/Type'
    if re.search(r'(system|engine|coordinator|manager)\.cs$',f): return 'System'
    return 'Support'
T=[
 ('docs/plans/EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-AUDIO-CONDITION-TRUTH-255.md',['Audio'],['audio','condition'],'Audio',['AudioConditionSystem.cs']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-TRADE-TELL-TRUTH-248.md',['Economy'],['trade','tell','market'],'Economy',['TradeTellEngine.cs']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-GUILT-INSOMNIA-TRUTH-246.md',['Survivors'],['guilt','insomnia','sleep'],'Survivors',['GuiltInsomniaSystem.cs']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-QUEST-RUNTIME-TRUTH-247.md',['Quests'],['quest'],'Quests',['QuestRuntimeCoordinator.cs']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-UV-CORONA-DETECTION-TRUTH-250.md',['Radio'],['uv','corona','detection'],'Radio',['UvCoronaDetectionEngine.cs']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE17_2026-09-21/PLAN-LATENT-EXPERT-TRUTH-239.md',['Survivors'],['latent','expert','skill'],'Survivors',['LatentExpertAwakeningSystem.cs']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-MATERIAL-SHIELDING-TRUTH-257.md',['Shelter'],['shielding','material'],'Shelter',['MaterialShieldingSystem.cs']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-RADIO-RECORDING-TRUTH-258.md',['Radio'],['recording','media'],'Radio',['RadioRecordingSystem.cs']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE17_2026-09-21/PLAN-JUSTICE-SYSTEM-TRUTH-222.md',['Narrative'],['justice','verdict','evidence'],'Narrative',['JusticeSystem.cs']),
 ('docs/plans/EXPANSION_PROGRAM_WAVE17_2026-09-21/PLAN-DREAM-SYSTEM-TRUTH-229.md',['Survivors'],['dream'],'Survivors',['DreamSystem.cs']),
]
for plan_path,dirs,tokens,region,keyfiles in T:
    t=rd(plan_path)
    if '## 6. Expanded census' in t: print('already:',os.path.basename(plan_path)); continue
    files=set()
    for d in dirs:
        root=os.path.join('Assets/Ashfall.Core',d)
        if not os.path.isdir(root): continue
        for b,_,fs in os.walk(root):
            for f in fs:
                if not f.endswith('.cs'): continue
                if any(tk in f.lower() for tk in tokens) or f in keyfiles: files.add(os.path.join(b,f))
    files=sorted(files)
    rows=[]; total=0
    for p in files:
        tt=rd(p); l=tt.count('\n')+1; total+=l
        rows.append((p,l,klass(os.path.basename(p)),os.path.basename(p) in keyfiles,
                     len(re.findall(r'System\.Random|new Random|Guid\.NewGuid|DateTime\.(?:Now|UtcNow)',tt)),
                     len(re.findall(r'catch\s*\([^)]*\)\s*\{\s*\}',tt)),
                     len(re.findall(r'(?:Capture|Restore)\w*\s*\(',tt))))
    save_files=[p for p,l,k,pre,b,s,sc in rows if sc]
    cats=[(f,p) for f,p in data_pairs if any(tk in f.lower() for tk in tokens)][:6]
    tested=0
    for b,_,fs in os.walk('Ashfall.Core.Tests'):
        for f in fs:
            if f.endswith('.cs'):
                tt=rd(os.path.join(b,f))
                tested+=sum(1 for p,_,_,_,_,_,_ in [(r[0],)+r[1:] for r in rows] if os.path.basename(p)[:-3] in tt)
    banned=sum(r[4] for r in rows); swal=sum(r[5] for r in rows)
    sec=f"""
---

## 6. Expanded census ({len(rows)} files · {total:,} lines)

Scope: `Assets/Ashfall.Core/{dirs[0]}/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
{' · '.join(f"{k} {sum(1 for r in rows if r[2]==k)}" for k in sorted(set(r[2] for r in rows)))}

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
"""
    for p,l,k,pre,b,s,sc in rows[:30]:
        sec+=f"| `{os.path.basename(p)}` | {l} | {k} | {'**yes**' if pre else '—'} | {b} | {s} | {sc} |\n"
    if len(rows)>30: sec+=f"\n… and {len(rows)-30} more domain files.\n"
    sec+=f"\n**Totals:** {banned} banned refs · {swal} empty catches · {len(save_files)} files with capture/restore.\n"
    sec+="\n## 7. Expanded data & state surface\n\n"
    if cats:
        sec+="| Catalog | Shape |\n|---|---|\n"
        for f,p in cats: sec+=f"| `{f}` | {rec(p)} |\n"
    else: sec+="No domain catalog matched; the plan's data path is loader-injected — verify before claiming.\n"
    sec+="\n**State surfaces:** "+(", ".join('`'+os.path.basename(p)+'`' for p in save_files[:10]) if save_files else 'none')+".\n"
    sec+=f"""
## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/{region}/`{' (create if absent)' if not os.path.isdir('Ashfall.Core.Tests/'+region) else ''} |
| Test references | {tested} name references across the test tree |
| Determinism | {banned} banned refs to fix or justify |
| Failures | {swal} empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.
"""
    open(plan_path,'a',encoding='utf-8').write(sec)
    print(f"{os.path.basename(plan_path)}: {len(rows)} files, {total} lines, cats={len(cats)}, save={len(save_files)}, tests={tested}, +{len(sec)}")
