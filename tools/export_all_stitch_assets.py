import json
import os
import re
import urllib.request
import urllib.error
import time

INPUT_PATH = '/home/robertsrff/.gemini/antigravity-cli/brain/9a5142a0-8540-44d0-afa2-ccb128982e7c/.system_generated/steps/141/output.txt'
SCREENS_DIR = '/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/assets/ui/Screens'
HTML_DIR = '/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/assets/ui/HtmlBundles'

os.makedirs(SCREENS_DIR, exist_ok=True)
os.makedirs(HTML_DIR, exist_ok=True)

with open(INPUT_PATH) as f:
    data = json.load(f)

screens = data.get('screens', [])
print(f"Starting bulk export of {len(screens)} screens from Stitch...")

def sanitize_filename(name):
    clean = re.sub(r'[^a-zA-Z0-9_\-]', '_', name.lower())
    clean = re.sub(r'_+', '_', clean).strip('_')
    return clean[:60]

exported_html_count = 0
downloaded_img_count = 0

for i, s in enumerate(screens, 1):
    sid = s.get('name', '').split('/')[-1]
    title = s.get('title', f'screen_{i:02d}')
    slug = f"{i:02d}_{sanitize_filename(title)}"

    # 1. Download HTML code bundle if present
    html_info = s.get('htmlCode')
    if isinstance(html_info, dict) and html_info.get('downloadUrl'):
        html_url = html_info['downloadUrl']
        html_file = os.path.join(HTML_DIR, f"{slug}.html")
        try:
            req = urllib.request.Request(
                html_url,
                headers={'User-Agent': 'Mozilla/5.0'}
            )
            with urllib.request.urlopen(req, timeout=30) as resp, open(html_file, 'wb') as out_f:
                out_f.write(resp.read())
            exported_html_count += 1
        except Exception as e:
            print(f"[{i:02d}] HTML download error for {slug}: {e}")
    elif isinstance(html_info, str):
        html_file = os.path.join(HTML_DIR, f"{slug}.html")
        with open(html_file, 'w', encoding='utf-8') as hf:
            hf.write(html_info)
        exported_html_count += 1

    # 2. Download Screenshot PNG
    img_url = s.get('screenshot', {}).get('downloadUrl')
    if img_url:
        img_file = os.path.join(SCREENS_DIR, f"{slug}.png")
        try:
            req = urllib.request.Request(
                img_url,
                headers={'User-Agent': 'Mozilla/5.0'}
            )
            with urllib.request.urlopen(req, timeout=30) as resp, open(img_file, 'wb') as out_f:
                out_f.write(resp.read())
            downloaded_img_count += 1
            print(f"[{i:02d}/{len(screens)}] Exported: {slug} (PNG + HTML)")
        except Exception as e:
            print(f"[{i:02d}/{len(screens)}] Failed downloading image for {slug}: {e}")

print(f"\n=======================================================")
print(f"--- BULK EXPORT COMPLETED SUCCESSFULLY ---")
print(f"Total HTML Bundles Exported: {exported_html_count} / {len(screens)}")
print(f"Total Screenshots Downloaded: {downloaded_img_count} / {len(screens)}")
print(f"Screens Directory: {SCREENS_DIR}")
print(f"HTML Directory:    {HTML_DIR}")
print(f"=======================================================")
