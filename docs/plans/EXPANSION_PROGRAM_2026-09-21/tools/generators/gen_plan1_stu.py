import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
by_auth={r['auth'][0]:r for r in orph}
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
# S: test region inverse index
test_dirs=sorted(d for d in os.listdir('Ashfall.Core.Tests') if os.path.isdir(os.path.join('Ashfall.Core.Tests',d)))
tmap={}
for d in test_dirs:
    txt=''
    for base,_,fs in os.walk(os.path.join('Ashfall.Core.Tests',d)):
        for f in fs:
            if f.endswith('.cs'): txt+='\n'+rd(os.path.join(base,f))
    if not txt: continue
    hits=[r['auth'][0] for r in orph if re.search(r'\b'+re.escape(r['auth'][0])+r'\b',txt)]
    if hits: tmap[d]=hits
hdr_s=f"""# PLAN-ORPHAN-SEAL-01 — Appendix S: Test-Region Inverse Index

**Generated:** 2026-09-21. Appendix J is orphan→tests; this is the inverse:
each existing test region and the orphans it already touches. Used to bundle
claims by the focused verification they will run — one region can verify
several seals, and a region touching many orphans is a natural claim boundary.
**Regions with at least one orphan reference: {len(tmap)} of {len(test_dirs)}.**

| Test region | Orphans referenced | Suggested region command |
|---|---:|---|
"""
body_s=''
for d,hits in sorted(tmap.items(), key=lambda kv:-len(kv[1])):
    body_s+=f"| `Ashfall.Core.Tests/{d}/` | {len(hits)} | `bash scripts/run_test.sh Ashfall.Core.Tests/{d}/` |\n"
body_s+="\n## Per-region detail\n\n"
for d,hits in sorted(tmap.items()):
    body_s+=f"### `{d}`\n\n"+', '.join('`'+h+'`' for h in sorted(hits))+"\n\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-S_TEST_REGIONS.md','w',encoding='utf-8').write(hdr_s+body_s)
# U: data-reference verification (string literals matching Data filenames)
data_files=sorted(f for f in os.listdir('Assets/StreamingAssets/Data') if f.endswith('.json'))
data_set=set(data_files)
hdr_u="""# PLAN-ORPHAN-SEAL-01 — Appendix U: Data-Reference Verification

**Generated:** 2026-09-21. String literals inside each orphan's source matched
against the actual files in `Data/`: this shows what a system **actually loads**,
which is stronger than Appendix M's type-name matching. A literal that does not
match a file is either a partial path, a key rather than a filename, or a stale
reference — each is listed so the package can classify it.
**Use:** a package whose loader reads one of these files has its data dependency
proven from source, not inferred from naming.

"""
body_u=''
anymatch=0
for r in orph:
    a=r['auth'][0]; t=rd(r['file'])
    lits=sorted(set(re.findall(r'"([a-z0-9_\-/]+\.json)"', t)))
    if not lits: continue
    anymatch+=1
    body_u+=f"### `{a}`\n\n| Literal | File exists |\n|---|:---:|\n"
    for l in lits[:8]:
        base=os.path.basename(l)
        body_u+=f"| `{l}` | {'**yes**' if base in data_set else 'no'} |\n"
    body_u+='\n'
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-U_DATA_REFERENCES.md','w',encoding='utf-8').write(hdr_u+body_u)
# T: worked seal exemplars
def save_methods(t): return re.findall(r'(?:public|internal)\s+(?:void|string|bool)\s+((?:Capture|Restore|Save|Load)\w*)\s*\(', t)
def catalogs(a):
    s=re.sub(r'(System|Engine|Coordinator|Manager)$','',a); s=re.sub(r'([a-z0-9])([A-Z])',r'\1_\2',s).lower()
    tk=[w for w in s.split('_') if len(w)>3]
    return [f for f in data_files if tk and sum(1 for w in tk if w in f.lower())>=max(1,len(tk)-1)][:4]
def tests_for(a):
    out=[]
    for base,_,fs in os.walk('Ashfall.Core.Tests'):
        for f in fs:
            if f.endswith('.cs') and re.search(r'\b'+re.escape(a)+r'\b', rd(os.path.join(base,f))):
                out.append(os.path.relpath(os.path.join(base,f),'Ashfall.Core.Tests'))
    return out
routes=sorted(set(re.findall(r'"([a-z][a-z0-9_]{2,})"', rd('src/Main.PlayerSurfaces.cs'))))
exemplars=['CommunicationsSystem','BestiarySystem','CupolaFoundryEngine','StormForecastReadinessEngine']
hdr_t="""# PLAN-ORPHAN-SEAL-01 — Appendix T: Worked Seal Exemplars

**Generated:** 2026-09-21. Four fully worked seal specifications assembled from
Appendices A–U (one per archetype), so a package author can copy the shape:
**CommunicationsSystem** (top risk score 7), **BestiarySystem** (island with a
route but no attachment), **CupolaFoundryEngine** (stateful engine with bound
catalogs), **StormForecastReadinessEngine** (small stateless evaluator).
**Note:** each exemplar is a *proposal*; the claim and premise re-check still
belong to the foreman and the executing package.

"""
body_t=''
for a in exemplars:
    r=by_auth.get(a)
    if not r: continue
    t=rd(r['file']); lines=t.count('\n')+1
    caps=save_methods(t); cats=catalogs(a); tst=tests_for(a)
    dom=os.path.basename(os.path.dirname(r['file'])).lower()
    main_hits=[f for f in os.listdir('src') if f.startswith('Main.') and dom in f.lower()]
    rt=[x for x in routes if any(w in x for w in dom.split('_'))]
    body_t+=f"""## Exemplar: `{a}`

- **File:** `{r['file'].replace('Ashfall.Core/','')}` · **Lines:** {lines} · **Tests referencing:** {len(tst)}{(': `'+tst[0]+'`') if tst else ''}
- **Save surface:** {', '.join('`'+c+'`' for c in caps) or 'none found (decide statefulness in EP-01)'}
- **Candidate catalogs:** {', '.join('`'+c+'`' for c in cats) or 'none by name — check literal references (Appendix U)'}
- **Host partial candidates:** {', '.join('`'+m+'`' for m in main_hits[:3]) or 'none — new attachment or headless-only'}
- **Route candidates:** {', '.join('`'+x+'`' for x in rt[:3]) or 'none — surface owner decision'}

**Package shape (proposal):**

1. Premise re-check: file hash vs Appendix I; catalogs vs Appendix R; tests vs Appendix J.
2. Interface package: host adapter (or session method) exposing the members in Appendix K; no new authority.
3. State package (only if `Capture`/`Restore` exists): register through the save owner; key per Appendix Q (all proposals collision-free).
4. Surface package: attach the route above or ship headless-only with a recorded reason.
5. Verification package: focused command from Appendix O; re-run the reachability audit and expect this file to leave the orphan set.

"""
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-T_WORKED_EXEMPLARS.md','w',encoding='utf-8').write(hdr_t+body_t)
for n in ['S_TEST_REGIONS','T_WORKED_EXEMPLARS','U_DATA_REFERENCES']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, os.path.getsize(p),'bytes')
print('test regions with orphans:',len(tmap),'orphans with json literals:',anymatch)
