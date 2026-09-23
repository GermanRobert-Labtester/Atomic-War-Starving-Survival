import os,re,json
from collections import defaultdict,deque
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
def cs(rel):
    out=[]
    for base,_,fs in os.walk(rel):
        for f in fs:
            if f.endswith('.cs'): out.append(os.path.join(base,f))
    return out
core=cs('Assets/Ashfall.Core'); host=cs('src'); tests=cs('Ashfall.Core.Tests')
type_re=re.compile(r'^\s*(?:public|internal)\s+(?:sealed\s+|abstract\s+|static\s+|partial\s+|readonly\s+|ref\s+)*\b(?:class|record|struct|interface|enum|delegate)\s+([A-Za-z_]\w*)',re.M)
tok_re=re.compile(r'\b[A-Za-z_]\w*\b')
file_types=defaultdict(set); tokens={}
for p in core:
    t=rd(p); tokens[p]=set(tok_re.findall(t))
    for m in type_re.finditer(t): file_types[m.group(1)].add(p)
host_tokens=set()
for p in host: host_tokens|=set(tok_re.findall(rd(p)))
roots={n for n,fs in file_types.items() if n in host_tokens}
reachable=set(); q=deque(roots)
while q:
    n=q.popleft()
    for p in file_types.get(n,()):
        if p in reachable: continue
        reachable.add(p)
        for tok in tokens[p]:
            if tok in file_types: q.append(tok)
orph=json.load(open('/tmp/unreachable.json'))
orphan_files={r['file'] for r in orph}
unreach_nonauth=[p for p in core if p not in reachable and p not in orphan_files]
hdr="""# PLAN-ORPHAN-SEAL-01 — Appendix P: Unreachable-Set Inbound Census

**Generated:** 2026-09-21 by re-running the reachability closure and classifying
**who references each orphan**: reachable files (the active game), unreachable
non-authority files (the dormant blob), or tests only.
**By construction a reachable file cannot reference an orphan by type name** —
if it did, the closure would pull the orphan in. This appendix verifies that
property (any positive case is an audit bug to fix, not a wiring lead) and then
answers the useful question: *is an orphan attached to the dormant blob, to
tests only, or to nothing at all?*
**Findings:** {orphan_count} orphan files · {blob_ref} referenced by at least
one dormant-blob file · {test_only} referenced by tests but nothing else ·
{islands} referenced by nothing anywhere (true islands).

"""
rows=[]
for r in orph:
    a=r['auth'][0]
    blob=[]; reach=[]
    for p in unreach_nonauth:
        if re.search(r'\b'+re.escape(a)+r'\b', rd(p)): blob.append(os.path.relpath(p,'Assets/Ashfall.Core'))
    for p in reachable:
        if re.search(r'\b'+re.escape(a)+r'\b', rd(p)): reach.append(os.path.relpath(p,'Assets/Ashfall.Core'))
    rows.append((a,reach,blob))
blob_ref=sum(1 for _,_,b in rows if b); reach_ref=sum(1 for _,r,_ in rows if r)
test_ref=0; islands=0
for a,reach,blob in rows:
    t=''
    for p in tests:
        if re.search(r'\b'+re.escape(a)+r'\b', rd(p)): t='x'; break
    if t and not blob: test_ref+=1
    if not t and not blob and not reach: islands+=1
hdr=hdr.format(orphan_count=len(orph),blob_ref=blob_ref,test_only=test_ref,islands=islands)
body=''
for a,reach,blob in rows:
    body+=f"### `{a}`\n\n"
    if reach:
        body+=f"**Reachability-contradiction (report as audit bug): referenced by reachable file(s) {', '.join('`'+x+'`' for x in reach[:3])}.**\n\n"
    if blob:
        body+="Dormant-blob references (the orphan is attached to archived work, not to the game):\n\n| File |\n|---|\n"+''.join(f"| `{f}` |\n" for f in blob[:6])+("\n" if len(blob)<=6 else f"\n… and {len(blob)-6} more\n\n")
    if not blob and not reach:
        body+="No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.\n\n"
open('docs/plans/EXPANSION_PROGRAM_2026-09-21/PLAN-ORPHAN-SEAL-01_APPENDIX-P_INCOMING_REFERENCES.md','w',encoding='utf-8').write(hdr+body)
print("P rewritten; blob_referenced:",blob_ref,"reach_contradictions:",reach_ref,"test_only:",test_ref,"islands:",islands)
