import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
by_auth={r['auth'][0]:r for r in orph}
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
# shared derivations (mirroring earlier appendices)
save_txt=rd('Assets/Ashfall.Core/Save/SaveSectionRegistry.cs')
keys=set(re.findall(r'new\("([a-z0-9_]+)"', save_txt))
data_files=[f for f in os.listdir('Assets/StreamingAssets/Data') if f.endswith('.json')]
def propose(name):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',name)
    return re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
def catalogs(a):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',a); s=re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
    tk=[w for w in s.split('_') if len(w)>3]
    return [f for f in data_files if tk and sum(1 for w in tk if w in f.lower())>=max(1,len(tk)-1)][:3]
routes=sorted(set(re.findall(r'"([a-z][a-z0-9_]{2,})"', rd('src/Main.PlayerSurfaces.cs'))))
test_dirs=[d for d in os.listdir('Ashfall.Core.Tests') if os.path.isdir(os.path.join('Ashfall.Core.Tests',d))]
cli=rd('Assets/Ashfall.Core/HostCliRegistry.cs')+rd('src/Host/HostCli.cs')
flags=sorted(set(re.findall(r'--[a-z][a-z0-9-]*-selftest', cli)))
def tokens(name):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',name); s=re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
    return [w for w in s.split('_') if len(w)>3]
# id set from data
idset=set()
pat=re.compile(r'\b((?:item|loc|faction|quest|enc|echo|radio|recipe|survivor|event|perk|trait)_[a-z0-9_]{3,})\b')
for f in data_files:
    try: idset|=set(pat.findall(rd(os.path.join('Assets/StreamingAssets/Data',f))))
    except Exception: pass
# V: master worklist
hdr_v="""# PLAN-ORPHAN-SEAL-01 — Appendix V: Master Seal Worklist

**Generated:** 2026-09-21. The single combined worklist: one row per orphan
joining the per-appendix findings — risk score (L), save surface (D), state
proposal (Q), top catalog (M/R), route candidate (N), verification command (O).
**Use:** a package claim can be opened from one line; each column links back to
the appendix that proves it. Rows are ordered by risk score, then size.

| Authority | Risk | State | Key (Q) | Top catalog | Route | Focused command |
|---|---:|---|---|---|---|---|
"""
rows=[]
for r in orph:
    a=r['auth'][0]; t=rd(r['file']); lines=t.count('\n')+1
    risky=len(re.findall(r'System\.Random|new Random|Guid\.NewGuid|DateTime\.(?:Now|UtcNow)', t))
    state='yes' if re.search(r'(?:Capture|Restore|Save|Load)\w*\s*\(', t) else '—'
    dom=os.path.basename(os.path.dirname(r['file'])).lower()
    attach=[f for f in os.listdir('src') if f.startswith('Main.') and dom in f.lower()]
    tests=sum(1 for d0,_,fs in os.walk('Ashfall.Core.Tests') for f0 in fs if f0.endswith('.cs') and re.search(r'\b'+re.escape(a)+r'\b', rd(os.path.join(d0,f0))))
    score=min(5,lines//400)+(3 if risky else 0)+(1 if state=='yes' else 0)+(0 if attach else 2)+(2 if tests==0 else 0)
    cats=catalogs(a); rt=[x for x in routes if any(w in x for w in tokens(a))]
    tdir=[d for d in test_dirs if any(w in d.lower() for w in tokens(a))]
    fl=[f for f in flags if any(w in f for w in tokens(a))]
    cmd=f"`bash scripts/run_test.sh Ashfall.Core.Tests/{tdir[0]}/`" if tdir else (f"`godot --headless --path . -- {fl[0]}`" if fl else 'new focused block')
    rows.append((score,lines,a,state,propose(a),cats[0] if cats else '—',rt[0] if rt else '—',cmd))
body_v=''
for score,lines,a,state,pk,cat,rt,cmd in sorted(rows,key=lambda x:(-x[0],-x[1])):
    body_v+=f"| `{a}` | {score} | {state} | `{pk}` | {('`'+cat+'`') if cat!='—' else '—'} | {('`'+rt+'`') if rt!='—' else '—'} | {cmd} |\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-V_MASTER_WORKLIST.md','w',encoding='utf-8').write(hdr_v+body_v)
# W: id-family reference audit
hdr_w=f"""# PLAN-ORPHAN-SEAL-01 — Appendix W: Data-Id Reference Audit

**Generated:** 2026-09-21. String literals in orphan sources that look like
catalog ids (`item_`, `loc_`, `faction_`, `quest_`, `recipe_`, …) checked
against **{len(idset)} ids found anywhere under `Data/`**. Appendix 34 mapped the
id families; this appendix shows which orphans reference them directly and
whether those references resolve.
**Use:** an unresolvable id in an orphan is either a stale literal or a runtime
construction (prefix + variable); the package classifies it before wiring.

"""
body_w=''
withids=0
for r in orph:
    a=r['auth'][0]; t=rd(r['file'])
    lits=sorted(set(pat.findall(t)))
    if not lits: continue
    withids+=1
    body_w+=f"### `{a}`\n\n| Id literal | Resolves |\n|---|:---:|\n"
    for l in lits[:10]:
        body_w+=f"| `{l}` | {'yes' if l in idset else '**no**'} |\n"
    body_w+='\n'
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-W_DATA_IDS.md','w',encoding='utf-8').write(hdr_w+body_w)
# X: static state hazards
hdr_x="""# PLAN-ORPHAN-SEAL-01 — Appendix X: Static-State & Singleton Census

**Generated:** 2026-09-21. Static members per orphan with hazard flags. The
host-session pattern (Appendix C) expects state to belong to a session object;
**mutable statics, singletons, and shared caches** fight that pattern and are
the most common source of cross-save leakage when a system is wired.
**Flags:** `S` singleton-named member · `M` mutable static (non-readonly, non-const) ·
`C` collection-typed static (cache-shaped).
**Use:** a package treats flagged members as pre-work: convert to session state
or prove the static is a constant lookup.

| Authority | Static members | Flags |
|---|---|---|
"""
body_x=''
flagged=0
for r in orph:
    a=r['auth'][0]; t=rd(r['file'])
    members=re.findall(r'(?:public|private|internal|protected)?\s*static\s+(?:readonly\s+)?[\w<>,\[\]\.\?]+\s+(\w+)\s*[;=({]', t)
    mut=re.findall(r'static\s+(?!readonly|const)[\w<>,\[\]\.\?]+\s+(\w+)\s*[;=]', t)
    coll=re.findall(r'static\s+(?:readonly\s+)?(?:List|Dictionary|HashSet|IReadOnlyList|IReadOnlyDictionary|ConcurrentDictionary)<[^>]+>\s+(\w+)', t)
    single=[m for m in members if re.search(r'Instance|Singleton|Shared|Default$', m, re.I)]
    fl=[]
    if single: fl.append('S:'+','.join(single[:3]))
    if mut: fl.append('M:'+','.join(mut[:3]))
    if coll: fl.append('C:'+','.join(coll[:3]))
    if fl: flagged+=1
    body_x+=f"| `{a}` | {', '.join('`'+m+'`' for m in members[:8]) or '—'} | {' · '.join(fl) or '—'} |\n"
hdr_x=hdr_x+f"\n**{flagged} of {len(orph)} orphans carry at least one flagged static.** See the table.\n\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-X_STATIC_HAZARDS.md','w',encoding='utf-8').write(hdr_x+body_x)
for n in ['V_MASTER_WORKLIST','W_DATA_IDS','X_STATIC_HAZARDS']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, os.path.getsize(p),'bytes')
print('ids found:',len(idset),'orphans with id literals:',withids,'static-flagged:',flagged)
