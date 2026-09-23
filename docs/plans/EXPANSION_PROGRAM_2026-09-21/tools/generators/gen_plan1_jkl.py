import os,re,json
root=os.getcwd()
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
# build test file index
test_files=[]
for d,_,fs in os.walk('Ashfall.Core.Tests'):
    for f in fs:
        if f.endswith('.cs'): test_files.append(os.path.join(d,f))
test_txt={p:rd(p) for p in test_files}
# J: test coverage inventory
hdr_j="""# PLAN-ORPHAN-SEAL-01 — Appendix J: Test Coverage Inventory

**Generated:** 2026-09-21. For every orphan authority: every test file that
references it by name, with the number of `[Fact]`/`[Theory]` cases in that
file. Appendix A lists a first sample; this is the complete list, so a seal
package knows exactly which tests must stay green and which behaviors are
untested.
**Reading a row:** a companion file's case count is the file's, not a claim
about how many cases touch the authority.

"""
body_j=''
total_cases=0
for r in orph:
    a=r['auth'][0]
    hits=[]
    for p,t in test_txt.items():
        if re.search(r'\b'+re.escape(a)+r'\b', t):
            facts=len(re.findall(r'\[Fact\]', t)); theories=len(re.findall(r'\[Theory\]', t))
            hits.append((os.path.relpath(p,'Ashfall.Core.Tests'), facts, theories))
    total_cases+=sum(f+t for _,f,t in hits)
    body_j+=f"### `{a}`\n\n"
    if hits:
        body_j+='| Test file | Facts | Theories |\n|---|---:|---:|\n'
        for f,fa,th in sorted(hits): body_j+=f"| `{f}` | {fa} | {th} |\n"
    else:
        body_j+="No test file references this authority by name.\n"
    body_j+='\n'
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-J_TEST_COVERAGE.md','w',encoding='utf-8').write(hdr_j+body_j)
# K: public member signatures
hdr_k="""# PLAN-ORPHAN-SEAL-01 — Appendix K: Public Member Signatures

**Generated:** 2026-09-21. The public surface a seal package must wire, listed
per orphan: public method signatures and public property names (up to 30
members per type; the count of any omitted members is noted). Derived by
source scan, so overload sets and generic constraints appear verbatim.
**Use:** this is the API the host adapter binds to; if a needed member is not
here, the package's job is to add it to the owner, not to reach around it.

"""
body_k=''
for r in orph:
    a=r['auth'][0]; t=rd(r['file'])
    methods=[]
    for m in re.finditer(r'^\s*public\s+((?:static\s+|async\s+|override\s+|virtual\s+|sealed\s+|new\s+)*[\w<>,\[\]\.\?]+\s+\w+)\s*\(([^)]*)\)', t, re.M):
        methods.append((m.group(1).strip()+'('+re.sub(r'\s+',' ',m.group(2).strip())+')'))
    props=re.findall(r'public\s+((?:static\s+)?[\w<>,\[\]\.\?]+\s+\w+)\s*\{\s*get', t)
    allm=methods+props
    body_k+=f"### `{a}`\n\n```\n"+'\n'.join(allm[:30])+("\n… ("+str(len(allm)-30)+" more)")+("\n" if len(allm)>30 else "")+"```\n\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-K_API_SIGNATURES.md','w',encoding='utf-8').write(hdr_k+body_k)
# L: risk scorecard
save_txt=rd('Assets/Ashfall.Core/Save/SaveSectionRegistry.cs')
sections=re.findall(r'new\("([a-z0-9_]+)",\s*"(\w+)",\s*"(\w+)",\s*"([a-z0-9_]+)"', save_txt)
main_files=[f for f in os.listdir('src') if f.endswith('.cs')]
cli_txt=rd('Assets/Ashfall.Core/HostCliRegistry.cs')+rd('src/Host/HostCli.cs')
pat={'System.Random':r'\bSystem\.Random\b','new Random':r'\bnew Random\b','Guid.NewGuid':r'\bGuid\.NewGuid\b',
 'DateTime.Now':r'\bDateTime\.Now\b','DateTime.UtcNow':r'\bDateTime\.UtcNow\b'}
rows=[]
for r in orph:
    a=r['auth'][0]; t=rd(r['file'])
    lines=t.count('\n')+1
    sizescore=min(5,lines//400)
    risky=sum(len(re.findall(v,t)) for v in pat.values())
    detscore=3 if risky else 0
    has_state=1 if (re.search(r'(?:Capture|Restore|Save|Load|Serialize)\w*\s*\(',t) or 'SaveSectionRegistry' in t) else 0
    dom=os.path.basename(os.path.dirname(r['file'])).lower()
    attach=[f for f in main_files if f.startswith('Main.') and dom in f.lower()]
    gscore=0 if attach else 2
    tests=sum(1 for p,t2 in test_txt.items() if re.search(r'\b'+re.escape(a)+r'\b',t2))
    tscore=2 if tests==0 else 0
    score=sizescore+detscore+has_state+gscore+tscore
    rows.append((a,lines,risky,bool(has_state),bool(attach),tests,score))
rows.sort(key=lambda x:-x[6])
hdr_l=f"""# PLAN-ORPHAN-SEAL-01 — Appendix L: Seal Risk Scorecard

**Generated:** 2026-09-21. A transparent triage score per orphan, so seal
packages can be ordered by cost and risk rather than by list position.
**Formula (max 12):** size (0–5; 1 point per 400 lines) + determinism risk
(3 if any banned primitive) + state (1 if capture/restore or registry
knowledge) + attachment need (2 if **no** matching `Main.*` candidate) +
test debt (2 if **no** test file references it).
**Reading:** high scores are expensive or risky seals — start there with
Appendix G/H/K open; low scores are small, attached, tested seals.
{sum(1 for r in rows if r[6]>=8)} orphans score ≥8; {sum(1 for r in rows if r[6]<=3)} score ≤3.

| Rank | Authority | Lines | Banned refs | Stateful | Host partial | Tests | Score |
|---:|---|---:|---:|:---:|:---:|---:|---:|
"""
body_l=''
for i,(a,l,risky,state,attach,tests,score) in enumerate(rows,1):
    body_l+=f"| {i} | `{a}` | {l} | {risky} | {'yes' if state else '—'} | {'yes' if attach else '**no**'} | {tests} | **{score}** |\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-L_RISK_SCORECARD.md','w',encoding='utf-8').write(hdr_l+body_l)
for n in ['J_TEST_COVERAGE','K_API_SIGNATURES','L_RISK_SCORECARD']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, os.path.getsize(p),'bytes')
