import json

with open('/home/robertsrff/.gemini/antigravity-cli/brain/9a5142a0-8540-44d0-afa2-ccb128982e7c/.system_generated/steps/141/output.txt') as f:
    data = json.load(f)

screens = data.get('screens', [])

md = []
md.append('# ASHFALL: Complete Google Stitch UI Inventory (62 Generated Screens)\n\n')
md.append('**Stitch Project Resource:** `projects/17640704459929707404` (*Ashfall - Atomic War Survival*)\n')
md.append('**Total Screens Available in Stitch:** 62 Screens\n')
md.append('**Visual Theme Tokens:** Dark Charcoal (`#131313`) | Ashen Grey (`#D1D5DB`) | Muted Tactical Teal (`#2D5A5E`) | Critical Burnt Orange (`#CC5500`)\n\n')
md.append('| # | Screen ID | Title / System | Resolution | Type | Screenshot Preview |\n')
md.append('|---|---|---|---|---|---|\n')

for i, s in enumerate(screens, 1):
    sid = s.get('name', '').split('/')[-1]
    title = s.get('title', 'Untitled').replace('|', '-')
    w = s.get('width', '')
    h = s.get('height', '')
    device = s.get('deviceType', 'N/A')
    has_html = 'Interactive HTML' if bool(s.get('htmlCode')) else 'Graphic Blueprint'
    img_url = s.get('screenshot', {}).get('downloadUrl', '')
    preview = f'[View 2.5K Image]({img_url})' if img_url else 'N/A'
    md.append(f'| {i:02d} | `{sid}` | **{title}** | {w}x{h} | {has_html} | {preview} |\n')

with open('/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/STITCH_GENERATED_UI_INVENTORY.md', 'w') as out:
    out.writelines(md)

print('Generated STITCH_GENERATED_UI_INVENTORY.md successfully with exact screen IDs!')
