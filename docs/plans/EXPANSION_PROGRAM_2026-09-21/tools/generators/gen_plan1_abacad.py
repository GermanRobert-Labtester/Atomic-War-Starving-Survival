import os,re,json
def rd(p):
    try: return open(p,encoding='utf-8',errors='ignore').read()
    except: return ''
orph=json.load(open('/tmp/unreachable.json'))
dirp='docs/plans/EXPANSION_PROGRAM_2026-09-21/'
# load appendix Y batches
yt=rd(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-Y_BATCH_PLAN.md')
batches=[]
for m in re.finditer(r'### Batch (\d+) — (\d+) members · risk sum (\d+) · dominant region `([^`]+)`\n\n(.*?)(?=\n### |\Z)', yt, re.S):
    members=re.findall(r'\| `([^`]+)` \|', m.group(5))
    batches.append({'n':int(m.group(1)),'region':m.group(4),'members':members})
# clean member lists (first column only)
for b in batches:
    seen=[]; 
    for x in b['members']:
        if x not in seen and ('System' in x or 'Engine' in x or 'Coordinator' in x or 'Manager' in x): seen.append(x)
    b['members']=seen
# plan bodies (excluding appendices)
plan_texts={}
for base,_,fs in os.walk('docs/plans'):
    if 'EXPANSION_PROGRAM' not in base: continue
    for f in fs:
        if f.endswith('.md') and 'APPENDIX' not in f.upper() and not f.startswith('README'):
            plan_texts[f.replace('.md','')]=rd(os.path.join(base,f))
# AB
hdr_ab="""# PLAN-ORPHAN-SEAL-01 — Appendix AB: Batch → Programme Plan Cross-Reference

**Generated:** 2026-09-21. For each of Appendix Y's ten batches, the expansion
plans whose bodies mention a member orphan. This is the dependency direction
that matters operationally: a batch **unblocks** the plans listed beside it, so
batch order can follow programme order (or deliberately run ahead of it).
**Reading a row:** `no plan mentions` means the member is only referenced in
Plan 1's own appendices — sealing it is greenfield, not unblocking.

"""
body_ab=''
for b in batches:
    body_ab+=f"### Batch {b['n']:02d} — region `{b['region']}`\n\n| Orphan | Mentioned by plans |\n|---|---|\n"
    for a in b['members']:
        hits=[p for p,t in plan_texts.items() if re.search(r'\b'+re.escape(a)+r'\b', t)]
        body_ab+=f"| `{a}` | {', '.join('`'+h+'`' for h in hits[:4]) or 'no plan mentions (greenfield)'}{' …' if len(hits)>4 else ''} |\n"
    body_ab+='\n'
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-AB_BATCH_PLAN_LINKS.md','w',encoding='utf-8').write(hdr_ab+body_ab)
# AC: save DTO census
core_all=''.join(rd(os.path.join(b,f)) for b,_,fs in os.walk('Assets/Ashfall.Core') for f in fs if f.endswith('.cs'))
save_reg=rd('Assets/Ashfall.Core/Save/SaveSectionRegistry.cs')
hdr_ac="""# PLAN-ORPHAN-SEAL-01 — Appendix AC: Save-DTO Census for Stateful Orphans

**Generated:** 2026-09-21. The 56 orphans implementing
`CaptureState()`/`RestoreState()` (Appendix Z) carry save DTO types. This
appendix lists each orphan's DTO names, whether the DTO is defined in the same
file, elsewhere in Core, and whether the registry already knows it.
**Use:** a package wiring one of these orphans starts from an existing DTO —
it does not invent a new shape. A DTO referenced but never defined is a build
gap to report, not to paper over.

| Authority | Save DTO types | Defined in same file | Elsewhere in Core | Registry-known |
|---|---|---|---|---|
"""
body_ac=''; dto_total=0
for r in orph:
    a=r['auth'][0]; t=rd(r['file'])
    dtos=sorted(set(re.findall(r'\b(\w+Save)\b', t)))
    if not dtos: continue
    dto_total+=len(dtos)
    same=[d for d in dtos if re.search(r'(?:class|struct|record)\s+'+re.escape(d)+r'\b', t)]
    elsewhere=[d for d in dtos if d not in same and re.search(r'(?:class|struct|record)\s+'+re.escape(d)+r'\b', core_all)]
    unknown=[d for d in dtos if d not in same and d not in elsewhere]
    reg=[d for d in dtos if re.search(r'\b'+re.escape(d)+r'\b', save_reg)]
    body_ac+=f"| `{a}` | {', '.join('`'+d+'`' for d in dtos[:5])} | {', '.join(same[:3]) or '—'} | {', '.join(elsewhere[:3]) or '—'} | {', '.join(reg[:3]) or '—'} |\n"
hdr_ac+=f"\n**{dto_total} DTO references across stateful orphans.**\n\n"
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-AC_SAVE_DTOS.md','w',encoding='utf-8').write(hdr_ac+body_ac)
# AD: batch verification protocol
ot=rd(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-O_VERIFICATION_COMMANDS.md')
def cmd_for(a):
    m=re.search(r'\| `'+re.escape(a)+r'` \|.*?\| ([^|]+) \|$', ot, re.M)
    return m.group(1).strip() if m else 'new focused block'
hdr_ad=f"""# PLAN-ORPHAN-SEAL-01 — Appendix AD: Batch Verification Protocol

**Generated:** 2026-09-21. The exact verification loop per batch: the focused
commands from Appendix O, the reachability re-run, and the **expected delta** —
a sealed batch must remove its members from the 99-file orphan list, so the
audit becomes the batch's acceptance witness (99 → 99−N). If the count does not
move, the batch is not integrated regardless of green tests.

**Per-batch loop**

1. Run the members' focused commands (below); all green.
2. Run `python3 docs/plans/EXPANSION_PROGRAM_2026-09-21/tools/reachability-audit.py`
   and expect `host-unreachable authority files: {99-len([m for b in batches for m in b['members']])}−…` — specifically **{99} → {99}−N** for N sealed members.
3. Run `python3 scripts/ci/generate-docs-index.py --check` if docs changed.
4. Record the audit delta in the claim handoff; a zero delta means unreachable→reachable did not actually happen.

"""
body_ad=''
for b in batches:
    body_ad+=f"### Batch {b['n']:02d} — expected 99 → {99-len(b['members'])} when all {len(b['members'])} members seal\n\n| Orphan | Focused command |\n|---|---|\n"
    for a in b['members']:
        body_ad+=f"| `{a}` | {cmd_for(a)} |\n"
    body_ad+='\n'
open(dirp+'PLAN-ORPHAN-SEAL-01_APPENDIX-AD_BATCH_VERIFICATION.md','w',encoding='utf-8').write(hdr_ad+body_ad)
for n in ['AB_BATCH_PLAN_LINKS','AC_SAVE_DTOS','AD_BATCH_VERIFICATION']:
    p=dirp+f'PLAN-ORPHAN-SEAL-01_APPENDIX-{n}.md'
    print(n, os.path.getsize(p),'bytes')
print('batches parsed:',len(batches),[len(b['members']) for b in batches],'dto refs:',dto_total)
