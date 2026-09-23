import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
by_file={r['auth'][0]:r['file'] for r in orph}
test_files=[os.path.join(b,f) for b,_,fs in os.walk('Ashfall.Core.Tests') for f in fs if f.endswith('.cs')]
regions={d:'' for d in os.listdir('Ashfall.Core.Tests') if os.path.isdir(os.path.join('Ashfall.Core.Tests',d))}
for d in regions:
    regions[d]=''.join(rd(os.path.join(b,f)) for b,_,fs in os.walk(os.path.join('Ashfall.Core.Tests',d)) for f in fs if f.endswith('.cs'))
def tokens(name):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',name); s=re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
    return [w for w in s.split('_') if len(w)>3]
items=[]
for r in orph:
    a=r['auth'][0]; t=rd(r['file']); lines=t.count('\n')+1
    risky=len(re.findall(r'System\.Random|new Random|Guid\.NewGuid|DateTime\.(?:Now|UtcNow)',t))
    dom=os.path.basename(os.path.dirname(r['file'])).lower()
    attach=[f for f in os.listdir('src') if f.startswith('Main.') and dom in f.lower()]
    tests=sum(1 for p in test_files if re.search(r'\b'+re.escape(a)+r'\b',rd(p)))
    state=1 if re.search(r'(?:Capture|Restore|Save|Load)\w*\s*\(',t) else 0
    sc=min(5,lines//400)+(3 if risky else 0)+state+(0 if attach else 2)+(2 if tests==0 else 0)
    tk=tokens(a)
    reg=''
    for d,txt in regions.items():
        if re.search(r'\b'+re.escape(a)+r'\b',txt): reg=d; break
    if not reg:
        cand=[d for d in regions if any(w in d.lower() for w in tk)]
        reg=cand[0] if cand else '(new)'
    items.append((a,sc,reg,lines))
items.sort(key=lambda x:(-x[1],-x[3]))
N=10
batches=[{'members':[],'risk':0,'regs':{}} for _ in range(N)]
# snake distribution to balance risk
idx=0; direction=1
for a,sc,reg,lines in items:
    b=batches[idx]
    b['members'].append(a); b['risk']+=sc
    b['regs'][reg]=b['regs'].get(reg,0)+1
    if direction==1:
        idx+=1
        if idx==N: idx=N-1; direction=-1
    else:
        idx-=1
        if idx<0: idx=0; direction=1
hdr_y=f"""# PLAN-ORPHAN-SEAL-01 — Appendix Y: Seal Batch Planner

**Generated:** 2026-09-21. A heuristic split of the 99 orphans into **{N}
balanced batches** (snake distribution by risk so each batch carries a mix of
anchors and small members). Each batch lists its dominant test region where one
exists (Appendix S) and a suggested focused command.
**This is a planning aid, not a claim schedule** — a batch may split further if
a member needs a new surface, and the foreman assigns claims.

"""
body_y=''
for i,b in enumerate(batches,1):
    reg=max(b['regs'].items(),key=lambda kv:kv[1])[0]
    body_y+=f"### Batch {i:02d} — {len(b['members'])} members · risk sum {b['risk']} · dominant region `{reg}`\n\n"
    body_y+='| Orphan | Risk | Own test region |\n|---|---:|---|\n'
    for a,sc,reg2,lines in sorted([x for x in items if x[0] in b['members']],key=lambda x:-x[1]):
        body_y+=f"| `{a}` | {sc} | `{reg2}` |\n"
    body_y+=(f"\nSuggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/{reg}/`\n\n" if reg!='(new)' else "\nNo existing home region — the batch creates one focused block.\n\n")
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-Y_BATCH_PLAN.md','w',encoding='utf-8').write(hdr_y+body_y)
# Z: shared interface shapes across orphans
shape={}
for r in orph:
    a=r['auth'][0]; t=rd(r['file'])
    for m in re.finditer(r'public\s+(?:static\s+)?(?:async\s+)?[\w<>,\[\]\.\?]+\s+(\w+)\s*\(([^)]*)\)', t):
        name=m.group(1); ar=m.group(2).count(',')+1 if m.group(2).strip() else 0
        if len(name)<4: continue
        shape.setdefault((name,ar),[]).append(a)
common={k:v for k,v in shape.items() if len(set(v))>=4}
rows=sorted(common.items(),key=lambda kv:-len(set(kv[1])))
hdr_z=f"""# PLAN-ORPHAN-SEAL-01 — Appendix Z: Shared Interface Shapes

**Generated:** 2026-09-21. Public methods that appear with the **same name and
arity in four or more orphans** — the de-facto interfaces the unreachable set
already shares. {len(rows)} shapes qualify.
**Use:** when sealing many members of one shape, a shared Core interface (e.g.
an `IDayStepped` for `TickDay`) can reduce adapter boilerplate — but only if the
package first proves the semantics match; identical signatures are not proof of
identical contracts. Appendix AA lists which orphans actually step on the day.

| Shape | Orphans | Examples |
|---|---:|---|
"""
body_z=''
for (name,ar),v in rows[:60]:
    uniq=sorted(set(v))
    body_z+=f"| `{name}` ({ar} arg{'s' if ar!=1 else ''}) | {len(uniq)} | {', '.join('`'+x+'`' for x in uniq[:4])}{' …' if len(uniq)>4 else ''} |\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-Z_SHARED_SHAPES.md','w',encoding='utf-8').write(hdr_z+body_z)
os.remove(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-Z_PRUNING_CANDIDATES.md')
for n in ['Y_BATCH_PLAN','Z_SHARED_SHAPES']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, os.path.getsize(p),'bytes')
print('batches:',N,[b['risk'] for b in batches],'common shapes:',len(rows))
for (nm,ar),v in rows[:8]: print('  ',nm,ar,len(set(v)))
