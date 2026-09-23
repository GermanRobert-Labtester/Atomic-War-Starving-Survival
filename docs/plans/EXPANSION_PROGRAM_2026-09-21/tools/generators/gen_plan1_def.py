import os,re,json
root=os.getcwd()
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
# D: save ownership per orphan
rows=[]
for r in orph:
    f=r['file']; t=rd(f)
    caps=re.findall(r'(?:public|internal)\s+(?:void|string|bool)\s+((?:Capture|Restore|Save|Load|Serialize|Deserialize|ToDto|FromDto)\w*)\s*\(', t)
    savesect=len(re.findall(r'SaveSectionRegistry', t))
    fields=len(re.findall(r'^\s*(?:private|public|internal)\s+(?:readonly\s+)?[\w<>,\[\]\.]+\s+\w+\s*;', t, re.M))
    rows.append((r['auth'][0], r['file'].replace('Assets/Ashfall.Core/',''), caps, savesect, fields))
has=[x for x in rows if x[2] or x[3]]
hdr=f"""# PLAN-ORPHAN-SEAL-01 — Appendix D: Orphan State & Save Ownership Map

**Generated:** 2026-09-21. For each of the 99 host-unreachable authorities:
does it define capture/restore-shaped methods, does it know the save-section
registry, and how many instance fields would need state registration if it is
wired stateful?
**Findings:** {len(has)} of {len(rows)} have any capture/restore method or a
`SaveSectionRegistry` reference; **{len(rows)-len(has)} are stateless or
state-blind** and must not invent a save section merely to be reachable.

**Use:** O1 (state census). A system that owns campaign-visible state gets a
save row in its package before wiring; a stateless evaluator gets none. A
`—` in both columns means the package must first decide whether the system is
stateful at all (EP-01 decision), not add persistence defensively.

## Systems with capture/restore-shaped methods or save-registry knowledge

| Authority | File | Capture/restore methods | Registry refs | Fields |
|---|---|---|---:|---:|
"""
body=''
for a,f,caps,ss,fl in has:
    body+=f"| `{a}` | `{f}` | {', '.join('`'+c+'`' for c in caps) or '—'} | {ss} | {fl} |\n"
body+="\n## Stateless or state-blind systems (no save path found)\n\n| Authority | File | Fields |\n|---|---|---:|\n"
for a,f,caps,ss,fl in rows:
    if not (caps or ss): body+=f"| `{a}` | `{f}` | {fl} |\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-D_SAVE_OWNERSHIP.md','w',encoding='utf-8').write(hdr+body)

# E: determinism & time audit
pat={
 'System.Random':r'\bSystem\.Random\b','Random.Shared':r'\bRandom\.Shared\b','new Random':r'\bnew Random\b',
 'Guid.NewGuid':r'\bGuid\.NewGuid\b','DateTime.Now':r'\bDateTime\.Now\b','DateTime.UtcNow':r'\bDateTime\.UtcNow\b',
 'DateTimeOffset':r'\bDateTimeOffset\.(?:Now|UtcNow)\b','Environment.TickCount':r'\bEnvironment\.TickCount\b',
 'ISeededRng':r'\bISeededRng\b','CampaignRng':r'\bCampaignRng\b','Fork(':r'\bFork\(',
}
rows=[]
for r in orph:
    t=rd(r['file']); counts={k:len(re.findall(v,t)) for k,v in pat.items()}
    bad=sum(counts[k] for k in ['System.Random','Random.Shared','new Random','Guid.NewGuid','DateTime.Now','DateTime.UtcNow','DateTimeOffset','Environment.TickCount'])
    good=counts['ISeededRng']+counts['CampaignRng']+counts['Fork(']
    rows.append((r['auth'][0], r['file'].replace('Assets/Ashfall.Core/',''), counts, bad, good))
risky=[x for x in rows if x[3]>0]
seeded=[x for x in rows if x[4]>0]
hdr=f"""# PLAN-ORPHAN-SEAL-01 — Appendix E: Orphan Determinism & Time Audit

**Generated:** 2026-09-21. Banned-source audit across all 99 host-unreachable
authorities for nondeterministic primitives versus the seeded contract.
**Findings:** **{len(risky)} of {len(rows)} orphans touch a banned
nondeterministic source**; {len(seeded)} already reference `ISeededRng` /
`CampaignRng` / `Fork(`.
**Rule:** every package that is stateful or rolling must fork from
`CampaignRngStream` (63 registered streams, see
`PLAN-DETERMINISM-REPLAY-13` Appendix A); wall-clock and `System.Random`
sources are retired at the seam, not merely hidden behind a host adapter.

## Banned-source findings (wiring blockers)

| Authority | File | System.Random | new Random | Guid.NewGuid | DateTime.Now/UtcNow | Offset | TickCount |
|---|---|---:|---:|---:|---:|---:|---:|
"""
body=''
for a,f,c,bad,good in risky:
    body+=f"| `{a}` | `{f}` | {c['System.Random']} | {c['new Random']} | {c['Guid.NewGuid']} | {c['DateTime.Now']+c['DateTime.UtcNow']} | {c['DateTimeOffset']} | {c['Environment.TickCount']} |\n"
body+="\n## Seeded-contract references already present\n\n| Authority | ISeededRng | CampaignRng | Fork( |\n|---|---:|---:|---:|\n"
for a,f,c,bad,good in seeded:
    body+=f"| `{a}` | {c['ISeededRng']} | {c['CampaignRng']} | {c['Fork(']} |\n"
body+="\n## Clean authorities\n\nNo banned source and no seeded reference found in the remaining " + str(len(rows)-len(risky)-len(seeded)) + " authorities; each package still confirms its determinism class at the seam (EP-01).\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-E_DETERMINISM_AUDIT.md','w',encoding='utf-8').write(hdr+body)

# F: dependency clusters
names={r['auth'][0]: r for r in orph}
edges=[]
for r in orph:
    t=rd(r['file'])
    for other in names:
        if other==r['auth'][0]: continue
        if re.search(r'\b'+re.escape(other)+r'\b', t): edges.append((r['auth'][0], other))
adj={}
for a,b in edges: adj.setdefault(a,set()).add(b)
# simple cluster: undirected connected components
import collections
g=collections.defaultdict(set)
for a,b in edges: g[a].add(b); g[b].add(a)
seen=set(); comps=[]
for n in list(names):
    if n in seen: continue
    st=[n]; comp=set()
    while st:
        x=st.pop()
        if x in comp: continue
        comp.add(x); seen.add(x)
        st.extend(g[x]-comp)
    comps.append(comp)
comps.sort(key=len,reverse=True)
isolated=[c for c in comps if len(c)==1]
hdr=f"""# PLAN-ORPHAN-SEAL-01 — Appendix F: Orphan Dependency Clusters & Seal Order

**Generated:** 2026-09-21. Intra-orphan reference graph over the 99
host-unreachable authorities ({len(edges)} edges).
**Findings:** {len(comps)} connected clusters; {len(isolated)} authorities are
isolated (no orphan-to-orphan references) and can be sealed in any order.
**Rule:** seal a cluster bottom-up along its edges — a system referenced by
another orphan is wired first (its API is the dependency); a referenced-only
authority is never wired before its consumers exist.
**Note:** an edge is a compile-time reference, not proof of a runtime
relationship; each package re-verifies the direction at the seam (EP-01).

## Clusters (largest first)

"""
body=''
for i,c in enumerate(comps,1):
    if len(c)==1: continue
    body+=f"### Cluster {i:02d} — {len(c)} authorities\n\n```\n"+'\n'.join(sorted(c))+"\n```\n\nSuggested seal order (referenced-first):\n\n"
    indeg={x:0 for x in c}
    for a in c:
        for b in adj.get(a,()):
            if b in c: indeg[b]=indeg.get(b,0)+1
    order=sorted(c, key=lambda x:(indeg.get(x,0), x))
    body+='\n'.join(f"{j:02d}. `{x}`{' — referenced by '+str(indeg[x])+' orphan(s)' if indeg.get(x,0) else ' — leaf (safe first)'}" for j,x in enumerate(order,1))+"\n\n"
body+="## Isolated authorities (order-free)\n\n"+', '.join('`'+next(iter(c))+'`' for c in isolated)+"\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-F_DEPENDENCY_CLUSTERS.md','w',encoding='utf-8').write(hdr+body)
import os as o
for n in ['D_SAVE_OWNERSHIP','E_DETERMINISM_AUDIT','F_DEPENDENCY_CLUSTERS']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, o.path.getsize(p),'bytes')
print('risky orphans:',len(risky),'clusters:',len(comps),'isolated:',len(isolated))
for a,f,c,bad,good in risky[:12]: print('  RISK',a,bad)
