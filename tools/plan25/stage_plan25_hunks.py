"""Stage ONLY Plan 25's hunks from shared files (concurrent-stream safety).

Builds 'HEAD + only my insertions' copies of the four shared files using the
same deterministic anchors the edits used, diffs them against HEAD, and
applies the resulting patches to the git index. The working tree is untouched.
"""
import subprocess, sys

JOBS = [
    ("Assets/Ashfall.Core/CatalogIntegrityValidator.cs", "validator"),
    ("src/Host/HostCli.cs", "hostcli"),
]

def head(path):
    return subprocess.run(["git", "show", f"HEAD:{path}"], capture_output=True, text=True,
                          check=True).stdout

def make_mine(kind, src):
    if kind == "runner":
        anchor = '''            t["evt_p25_marked_ruin_s1"] = new FlagTrigger("flag_grievance_scavenger_claim_disputed");
            t["evt_p25_marked_ruin_s2"] = new DayOffsetTrigger(3, "evt_p25_marked_ruin_s1");
'''
        block = anchor + open("tools/plan25/runner_block.txt").read()
        assert anchor in src, "runner anchor missing in HEAD"
        return src.replace(anchor, block, 1)
    if kind == "rules":
        anchor = '            "paper_scrap", "item_teddy_bear", "crayon", "ammo_9x19", "blood_bag",'
        assert anchor in src, "rules anchor missing in HEAD"
        return src.replace(anchor, open("tools/plan25/rules_block.txt").read() + "\n" + anchor, 1)
    if kind == "validator":
        anchor = '            "flag_exp07_vel_vigil_knock",\n'
        theirs = (
            '            "flag_grievance_scavenger_claim_disputed", "flag_escalation_marked_ruin",\n'
            '            "flag_escalation_marked_ruin_mediated", "flag_favor_scavenger_claim_recognized",\n'
            '            "flag_favor_scavenger_arbitration_fair",\n'
        )
        block = theirs + open("tools/plan25/validator_block.txt").read() + "\n"
        assert anchor in src, "validator anchor missing in HEAD"
        return src.replace(anchor, anchor + block, 1)
    if kind == "hostcli":
        anchor = '''            if (Has(args, "--muster-selftest") || Has(args, "--expansion-06-selftest"))
                return HostCliAction.MusterSelfTest;
'''
        add = '''            if (Has(args, "--faction-ecology-selftest"))
                return HostCliAction.FactionEcologySelfTest;
'''
        assert anchor in src, "hostcli parse anchor missing in HEAD"
        mine = src.replace(anchor, anchor + add, 1)
        help_anchor = '            GD.Print("  --muster-selftest / --expansion-06-selftest        MusterHeadlessDemo (Exp 06 the Muster)");\n'
        help_add = '            GD.Print("  --faction-ecology-selftest                      Plan 25 faction ecology vertical slice (action board, E-P1 chain, witness, camp scene, muster path)");\n'
        assert help_anchor in mine, "hostcli help anchor missing in HEAD"
        return mine.replace(help_anchor, help_anchor + help_add, 1)
    raise ValueError(kind)

for path, kind in JOBS:
    src = head(path)
    mine = make_mine(kind, src)
    with open(f"/tmp/head_{kind}.cs", "w") as f:
        f.write(src)
    with open(f"/tmp/mine_{kind}.cs", "w") as f:
        f.write(mine)
    with open(f"/tmp/{kind}.raw", "w") as f:
        subprocess.run(["diff", "-u", f"/tmp/head_{kind}.cs", f"/tmp/mine_{kind}.cs"], stdout=f)
    with open(f"/tmp/{kind}.raw") as f:
        text = f.read()
    text = text.replace(f"/tmp/head_{kind}.cs", "a/" + path)
    text = text.replace(f"/tmp/mine_{kind}.cs", "b/" + path)
    with open(f"/tmp/{kind}.patch", "w") as f:
        f.write(text)
    r = subprocess.run(["git", "apply", "--cached", f"/tmp/{kind}.patch"],
                       capture_output=True, text=True)
    print(kind, "->", "STAGED" if r.returncode == 0 else "FAILED: " + r.stderr.strip())
    if r.returncode != 0:
        sys.exit(1)
