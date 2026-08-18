import os
import math
from PIL import Image, ImageDraw, ImageFont, ImageFilter, ImageOps

OUTPUT_DIR = '/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/assets/ui/Icons'
EMBLEMS_DIR = '/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/assets/ui/FactionEmblems'

os.makedirs(OUTPUT_DIR, exist_ok=True)
os.makedirs(EMBLEMS_DIR, exist_ok=True)

SIZE = 2048
CENTER = (SIZE // 2, SIZE // 2)
RADIUS = 920

def create_base_canvas():
    img = Image.new('RGBA', (SIZE, SIZE), (19, 19, 19, 255))
    draw = ImageDraw.Draw(img)
    return img, draw

def draw_textured_shield(draw, center, r, bg_color, border_color, accent_color):
    cx, cy = center
    for offset in range(30):
        draw.ellipse([cx - r + offset, cy - r + offset, cx + r - offset, cy + r - offset],
                     outline=(border_color[0], border_color[1], border_color[2], int(255 - offset * 6)), width=2)
    draw.ellipse([cx - r + 30, cy - r + 30, cx + r - 30, cy + r - 30], fill=bg_color)
    draw.ellipse([cx - r + 60, cy - r + 60, cx + r - 60, cy + r - 60], outline=accent_color, width=12)
    draw.ellipse([cx - r + 90, cy - r + 90, cx + r - 90, cy + r - 90], outline=(40, 40, 40, 255), width=6)
    num_rivets = 24
    for i in range(num_rivets):
        angle = (2 * math.pi / num_rivets) * i
        rx = cx + int((r - 45) * math.cos(angle))
        ry = cy + int((r - 45) * math.sin(angle))
        draw.ellipse([rx - 14, ry - 14, rx + 14, ry + 14], fill=(20, 20, 20, 255), outline=accent_color, width=4)
        draw.line([rx - 8, ry - 8, rx + 8, ry + 8], fill=accent_color, width=3)

def create_emblem(filename, bg_col, bdr_col, acc_col, draw_fn):
    img, draw = create_base_canvas()
    draw_textured_shield(draw, CENTER, RADIUS, bg_col, bdr_col, acc_col)
    draw_fn(draw, CENTER)
    out1 = os.path.join(OUTPUT_DIR, filename)
    out2 = os.path.join(EMBLEMS_DIR, filename)
    img.save(out1, 'PNG', optimize=True)
    img.save(out2, 'PNG', optimize=True)
    print(f"-> Generated 2K Emblem: {filename}")

# --- 50 INDIVIDUAL DRAW FUNCTIONS ---

# 64. Vault 09 Technocrats
def d_vault_09(draw, c):
    cx, cy = c
    draw.rectangle([cx - 300, cy - 300, cx + 300, cy + 300], fill=(15, 23, 42, 255), outline=(56, 189, 248, 255), width=16)
    for i in range(-200, 250, 100):
        draw.line([cx + i, cy - 300, cx + i, cy - 420], fill=(56, 189, 248, 255), width=16)
        draw.line([cx + i, cy + 300, cx + i, cy + 420], fill=(56, 189, 248, 255), width=16)
    draw.ellipse([cx - 140, cy - 140, cx + 140, cy + 140], fill=(30, 58, 138, 255), outline=(255, 255, 255, 255), width=8)

# 65. Deep Strata Drillers
def d_strata_drillers(draw, c):
    cx, cy = c
    draw.polygon([(cx, cy + 420), (cx - 260, cy - 320), (cx + 260, cy - 320)], fill=(100, 116, 139, 255), outline=(245, 158, 11, 255), width=16)
    for y in range(cy - 240, cy + 300, 90):
        draw.line([cx - 180 + int((y - cy) * 0.4), y, cx + 180 - int((y - cy) * 0.4), y], fill=(234, 88, 12, 255), width=16)

# 66. Silent Monks
def d_silent_monks(draw, c):
    cx, cy = c
    draw.chord([cx - 260, cy - 420, cx + 260, cy + 200], 180, 360, fill=(30, 27, 75, 255), outline=(255, 255, 255, 255), width=12)
    draw.rectangle([cx - 40, cy - 60, cx + 40, cy + 320], fill=(254, 240, 138, 255)) # Candle
    draw.ellipse([cx - 30, cy - 140, cx + 30, cy - 60], fill=(234, 88, 12, 255)) # Flame

# 67. Bunker Alpha Sentinels
def d_bunker_alpha(draw, c):
    cx, cy = c
    draw.polygon([(cx, cy - 420), (cx + 360, cy - 120), (cx + 280, cy + 360), (cx - 280, cy + 360), (cx - 360, cy - 120)], fill=(30, 41, 59, 255), outline=(220, 38, 38, 255), width=16)
    draw.line([cx, cy - 340, cx, cy + 280], fill=(220, 38, 38, 255), width=20)
    draw.line([cx - 220, cy - 40, cx + 220, cy - 40], fill=(220, 38, 38, 255), width=20)

# 68. Airlock Wardens
def d_airlock_wardens(draw, c):
    cx, cy = c
    draw.ellipse([cx - 400, cy - 400, cx + 400, cy + 400], fill=(24, 24, 27, 255), outline=(250, 204, 21, 255), width=24)
    # Hazard stripes
    for i in range(-300, 400, 100):
        draw.line([cx + i, cy - 200, cx + i + 100, cy + 200], fill=(250, 204, 21, 255), width=28)

# 69. Hydro-Core Custodians
def d_hydro_core(draw, c):
    cx, cy = c
    draw.ellipse([cx - 420, cy - 420, cx + 420, cy + 420], outline=(14, 165, 233, 255), width=28)
    for i in range(8):
        a = (2 * math.pi / 8) * i
        draw.line([(cx, cy), (cx + int(360 * math.cos(a)), cy + int(360 * math.sin(a)))], fill=(56, 189, 248, 255), width=20)

# 70. Geothermal Stokers
def d_geothermal_stokers(draw, c):
    cx, cy = c
    draw.ellipse([cx - 380, cy - 380, cx + 380, cy + 380], fill=(67, 20, 7, 255), outline=(234, 88, 12, 255), width=20)
    draw.line([cx, cy, cx + 180, cy - 180], fill=(250, 204, 21, 255), width=24) # Needle
    draw.ellipse([cx - 50, cy - 50, cx + 50, cy + 50], fill=(255, 255, 255, 255))

# 71. Sub-Vault Metallurgists
def d_metallurgists(draw, c):
    cx, cy = c
    draw.polygon([(cx - 240, cy - 200), (cx + 240, cy - 200), (cx + 160, cy + 300), (cx - 160, cy + 300)], fill=(71, 85, 105, 255), outline=(245, 158, 11, 255), width=14)
    draw.ellipse([cx - 80, cy + 200, cx + 80, cy + 380], fill=(250, 204, 21, 255))

# 72. Atmospheric Scrubbers
def d_scrubbers(draw, c):
    cx, cy = c
    draw.ellipse([cx - 400, cy - 400, cx + 400, cy + 400], outline=(34, 197, 94, 255), width=20)
    for i in range(4):
        a = (2 * math.pi / 4) * i
        draw.chord([cx - 200 + int(120*math.cos(a)), cy - 200 + int(120*math.sin(a)), cx + 200 + int(120*math.cos(a)), cy + 200 + int(120*math.sin(a))], 0, 180, fill=(74, 222, 128, 255))

# 73. Vault Chronicle Scribes
def d_scribes(draw, c):
    cx, cy = c
    draw.rectangle([cx - 260, cy - 340, cx + 260, cy + 340], fill=(254, 243, 199, 255), outline=(120, 53, 15, 255), width=14)
    draw.line([cx - 200, cy + 300, cx + 240, cy - 380], fill=(217, 119, 6, 255), width=22)

# 74. Rad-Storm Riders
def d_storm_riders(draw, c):
    cx, cy = c
    draw.ellipse([cx - 360, cy - 360, cx + 360, cy + 360], fill=(24, 24, 27, 255), outline=(245, 158, 11, 255), width=32)
    draw.line([cx - 200, cy - 200, cx + 200, cy + 200], fill=(239, 68, 68, 255), width=24)
    draw.line([cx + 200, cy - 200, cx - 200, cy + 200], fill=(239, 68, 68, 255), width=24)

# 75. Ashen Crows
def d_ashen_crows(draw, c):
    cx, cy = c
    draw.line([cx - 440, cy + 200, cx + 440, cy + 200], fill=(148, 163, 184, 255), width=18)
    draw.polygon([(cx, cy - 260), (cx + 180, cy), (cx + 80, cy + 200), (cx - 80, cy + 200), (cx - 180, cy)], fill=(15, 23, 42, 255), outline=(226, 232, 240, 255), width=8)

# 76. Bone Carvers
def d_bone_carvers(draw, c):
    cx, cy = c
    draw.chord([cx - 400, cy - 200, cx + 400, cy + 200], 0, 180, fill=(243, 244, 246, 255), outline=(180, 83, 9, 255), width=12)
    draw.line([cx - 100, cy - 300, cx + 100, cy + 100], fill=(148, 163, 184, 255), width=24)

# 77. Glass Walkers
def d_glass_walkers(draw, c):
    cx, cy = c
    for i in range(6):
        a = (2 * math.pi / 6) * i
        draw.polygon([(cx, cy), (cx + int(340 * math.cos(a)), cy + int(340 * math.sin(a))), (cx + int(340 * math.cos(a + 0.5)), cy + int(340 * math.sin(a + 0.5)))], fill=(16, 185, 129, 200), outline=(255, 255, 255, 255), width=6)

# 78. Permafrost Trappers
def d_trappers(draw, c):
    cx, cy = c
    draw.arc([cx - 360, cy - 200, cx + 360, cy + 300], 0, 180, fill=(148, 163, 184, 255), width=32)
    for x in range(cx - 280, cx + 300, 80):
        draw.polygon([(x, cy + 120), (x + 30, cy + 20), (x + 60, cy + 120)], fill=(226, 232, 240, 255))

# 79. Scrapland Jackals
def d_jackals(draw, c):
    cx, cy = c
    draw.polygon([(cx - 280, cy - 100), (cx, cy - 340), (cx + 280, cy - 100), (cx, cy + 340)], fill=(71, 85, 105, 255), outline=(234, 88, 12, 255), width=12)
    draw.ellipse([cx - 100, cy - 60, cx - 40, cy], fill=(239, 68, 68, 255))
    draw.ellipse([cx + 40, cy - 60, cx + 100, cy], fill=(239, 68, 68, 255))

# 80. Highway Drifters
def d_drifters(draw, c):
    cx, cy = c
    draw.polygon([(cx, cy - 380), (cx + 360, cy), (cx, cy + 380), (cx - 360, cy)], fill=(234, 179, 8, 255), outline=(0,0,0,255), width=16)
    draw.line([cx, cy - 260, cx, cy + 260], fill=(0,0,0,255), width=24)

# 81. Sand Sailors
def d_sand_sailors(draw, c):
    cx, cy = c
    draw.polygon([(cx, cy - 400), (cx + 280, cy + 160), (cx, cy + 240)], fill=(245, 158, 11, 255), outline=(255, 255, 255, 255), width=10)
    draw.line([cx - 300, cy + 260, cx + 300, cy + 260], fill=(180, 83, 9, 255), width=24)

# 82. Rust Scorpions
def d_rust_scorpions(draw, c):
    cx, cy = c
    draw.arc([cx - 200, cy - 380, cx + 200, cy + 200], 270, 90, fill=(234, 88, 12, 255), width=32)
    draw.polygon([(cx - 40, cy - 380), (cx + 80, cy - 440), (cx + 20, cy - 340)], fill=(239, 68, 68, 255))

# 83. Ridge Vultures
def d_ridge_vultures(draw, c):
    cx, cy = c
    draw.polygon([(cx, cy - 100), (cx + 380, cy - 280), (cx + 200, cy + 180), (cx, cy + 100), (cx - 200, cy + 180), (cx - 380, cy - 280)], fill=(30, 41, 59, 255), outline=(220, 38, 38, 255), width=10)

# 84. Frozen Sound Whalers
def d_whalers(draw, c):
    cx, cy = c
    draw.polygon([(cx - 180, cy - 400), (cx + 180, cy - 400), (cx, cy - 180)], fill=(14, 165, 233, 255))
    draw.line([cx, cy - 200, cx, cy + 380], fill=(226, 232, 240, 255), width=24)

# 85. Deep Salvage Divers
def d_deep_divers(draw, c):
    cx, cy = c
    draw.rectangle([cx - 180, cy - 320, cx - 60, cy + 280], fill=(234, 179, 8, 255), outline=(0,0,0,255), width=8)
    draw.rectangle([cx + 60, cy - 320, cx + 180, cy + 280], fill=(234, 179, 8, 255), outline=(0,0,0,255), width=8)

# 86. Icebreaker Coalition
def d_icebreaker_coalition(draw, c):
    cx, cy = c
    draw.polygon([(cx, cy - 360), (cx + 240, cy + 240), (cx - 240, cy + 240)], fill=(2, 132, 199, 255), outline=(255, 255, 255, 255), width=14)

# 87. Lighthouse Keepers
def d_lighthouse(draw, c):
    cx, cy = c
    draw.polygon([(cx - 120, cy - 240), (cx + 120, cy - 240), (cx + 180, cy + 360), (cx - 180, cy + 360)], fill=(226, 232, 240, 255), outline=(0,0,0,255), width=10)
    draw.ellipse([cx - 90, cy - 380, cx + 90, cy - 220], fill=(250, 204, 21, 255))

# 88. Kelp Harvesters
def d_kelp(draw, c):
    cx, cy = c
    for off in [-120, 0, 120]:
        draw.line([(cx + off, cy + 340), (cx + off * 1.5, cy - 340)], fill=(13, 148, 136, 255), width=28)

# 89. Salt Marsh Poachers
def d_marsh_poachers(draw, c):
    cx, cy = c
    draw.line([cx - 380, cy + 180, cx + 380, cy + 180], fill=(71, 85, 105, 255), width=24)
    draw.line([cx, cy - 380, cx, cy + 380], fill=(234, 88, 12, 255), width=16)

# 90. Coastal Ferrymen
def d_ferrymen(draw, c):
    cx, cy = c
    draw.line([cx - 280, cy - 380, cx + 280, cy + 380], fill=(180, 83, 9, 255), width=32)
    draw.ellipse([cx - 100, cy - 100, cx + 100, cy + 100], fill=(245, 158, 11, 255))

# 91. Sunken Rig Squatters
def d_sunken_rig(draw, c):
    cx, cy = c
    draw.rectangle([cx - 240, cy - 140, cx + 240, cy - 40], fill=(220, 38, 38, 255))
    draw.line([cx - 180, cy - 40, cx - 220, cy + 360], fill=(148, 163, 184, 255), width=20)
    draw.line([cx + 180, cy - 40, cx + 220, cy + 360], fill=(148, 163, 184, 255), width=20)

# 92. Driftwood Carvers
def d_driftwood(draw, c):
    cx, cy = c
    draw.ellipse([cx - 360, cy - 100, cx + 360, cy + 100], fill=(120, 53, 15, 255), outline=(255, 255, 255, 255), width=8)

# 93. Arctic Harbor Masters
def d_harbor_masters(draw, c):
    cx, cy = c
    draw.ellipse([cx - 340, cy - 340, cx + 340, cy + 340], outline=(245, 158, 11, 255), width=28)
    draw.line([cx - 420, cy, cx + 420, cy], fill=(245, 158, 11, 255), width=24)
    draw.line([cx, cy - 420, cx, cy + 420], fill=(245, 158, 11, 255), width=24)

# 94. Century Seed Guardians
def d_century_seed(draw, c):
    cx, cy = c
    draw.ellipse([cx - 200, cy - 320, cx + 200, cy + 320], fill=(20, 83, 45, 255), outline=(74, 222, 128, 255), width=16)
    draw.line([cx, cy - 180, cx, cy + 180], fill=(250, 204, 21, 255), width=18)

# 95. Chelation Order
def d_chelation(draw, c):
    cx, cy = c
    draw.polygon([(cx - 100, cy - 360), (cx + 100, cy - 360), (cx + 280, cy + 280), (cx - 280, cy + 280)], fill=(30, 58, 138, 255), outline=(255, 255, 255, 255), width=12)

# 96. Prosthetic Guild
def d_prosthetic_guild(draw, c):
    cx, cy = c
    draw.rectangle([cx - 160, cy - 60, cx + 160, cy + 320], fill=(217, 119, 6, 255), outline=(0,0,0,255), width=8)
    for x in [-120, -40, 40, 120]:
        draw.line([cx + x, cy - 60, cx + x, cy - 300], fill=(203, 213, 225, 255), width=20)

# 97. Apothecary Circle
def d_apothecary(draw, c):
    cx, cy = c
    draw.chord([cx - 260, cy - 140, cx + 260, cy + 280], 0, 180, fill=(100, 116, 139, 255), outline=(255, 255, 255, 255), width=12)
    draw.line([cx - 140, cy - 340, cx + 80, cy + 80], fill=(180, 83, 9, 255), width=32)

# 98. Rad-Shield Physicists
def d_rad_shield(draw, c):
    cx, cy = c
    draw.rectangle([cx - 280, cy - 360, cx + 280, cy + 360], fill=(51, 65, 85, 255), outline=(245, 158, 11, 255), width=16)
    draw.line([cx - 420, cy, cx - 280, cy], fill=(239, 68, 68, 255), width=24)

# 99. Myco-Remediators
def d_myco_remediators(draw, c):
    cx, cy = c
    draw.chord([cx - 280, cy - 320, cx + 280, cy + 80], 180, 360, fill=(168, 85, 247, 255), outline=(255, 255, 255, 255), width=12)
    draw.rectangle([cx - 40, cy - 40, cx + 40, cy + 320], fill=(243, 244, 246, 255))

# 100. Blood Bankers
def d_blood_bankers(draw, c):
    cx, cy = c
    draw.rectangle([cx - 180, cy - 280, cx + 180, cy + 260], fill=(159, 18, 57, 255), outline=(255, 255, 255, 255), width=12)
    draw.line([cx, cy + 260, cx, cy + 420], fill=(225, 29, 72, 255), width=18)

# 101. Optometric Weavers
def d_optometric(draw, c):
    cx, cy = c
    draw.ellipse([cx - 300, cy - 120, cx - 60, cy + 120], fill=(245, 158, 11, 200), outline=(255, 255, 255, 255), width=14)
    draw.ellipse([cx + 60, cy - 120, cx + 300, cy + 120], fill=(245, 158, 11, 200), outline=(255, 255, 255, 255), width=14)
    draw.line([cx - 60, cy, cx + 60, cy], fill=(203, 213, 225, 255), width=16)

# 102. Trauma Surgeons
def d_trauma_surgeons(draw, c):
    cx, cy = c
    draw.line([cx - 260, cy - 260, cx + 260, cy + 260], fill=(226, 232, 240, 255), width=22)
    draw.line([cx + 260, cy - 260, cx - 260, cy + 260], fill=(226, 232, 240, 255), width=22)
    draw.ellipse([cx - 100, cy - 100, cx + 100, cy + 100], fill=(220, 38, 38, 255))

# 103. Genome Preservationists
def d_genome_preserv(draw, c):
    cx, cy = c
    draw.rectangle([cx - 180, cy - 320, cx + 180, cy + 300], fill=(30, 41, 59, 255), outline=(56, 189, 248, 255), width=16)
    draw.ellipse([cx - 80, cy - 100, cx + 80, cy + 100], fill=(14, 165, 233, 255))

# 104. Verdict Tribunal
def d_verdict_tribunal(draw, c):
    cx, cy = c
    draw.polygon([(cx - 280, cy - 300), (cx + 280, cy - 300), (cx + 200, cy - 160), (cx - 200, cy - 160)], fill=(180, 83, 9, 255), outline=(255, 255, 255, 255), width=10)
    draw.line([cx, cy - 160, cx, cy + 320], fill=(120, 53, 15, 255), width=32)

# 105. Standing Record Legion
def d_standing_record(draw, c):
    cx, cy = c
    draw.rectangle([cx - 240, cy - 360, cx + 240, cy + 360], fill=(71, 85, 105, 255), outline=(245, 158, 11, 255), width=16)
    for y in range(cy - 260, cy + 300, 60):
        draw.line([cx - 160, y, cx + 160, y], fill=(255, 255, 255, 255), width=8)

# 106. Nobody's Charter Pioneers
def d_nobodys_pioneers(draw, c):
    cx, cy = c
    draw.line([cx - 360, cy + 200, cx + 360, cy + 200], fill=(180, 83, 9, 255), width=32)
    draw.polygon([(cx - 200, cy + 160), (cx + 200, cy + 160), (cx, cy - 280)], fill=(203, 213, 225, 255), outline=(0,0,0,255), width=8)

# 107. Year of Ash Climatologists
def d_year_of_ash(draw, c):
    cx, cy = c
    draw.ellipse([cx - 360, cy - 360, cx + 360, cy + 360], fill=(24, 24, 27, 255), outline=(245, 158, 11, 255), width=20)
    draw.arc([cx - 260, cy - 260, cx + 260, cy + 260], 180, 360, fill=(239, 68, 68, 255), width=24)

# 108. Silent Foundry Automatons
def d_silent_foundry(draw, c):
    cx, cy = c
    draw.rectangle([cx - 240, cy - 140, cx + 240, cy + 140], fill=(51, 65, 85, 255), outline=(56, 189, 248, 255), width=16)
    draw.line([cx - 120, cy - 140, cx - 120, cy - 360], fill=(56, 189, 248, 255), width=24)
    draw.line([cx + 120, cy - 140, cx + 120, cy - 360], fill=(56, 189, 248, 255), width=24)

# 109. Deep Coast Cartographers
def d_deep_cartographers(draw, c):
    cx, cy = c
    draw.ellipse([cx - 340, cy - 340, cx + 340, cy + 340], fill=(15, 23, 42, 255), outline=(14, 165, 233, 255), width=16)
    draw.line([cx - 160, cy - 380, cx, cy], fill=(245, 158, 11, 255), width=16)
    draw.line([cx + 160, cy - 380, cx, cy], fill=(245, 158, 11, 255), width=16)

# 110. Unbroken Covenant
def d_unbroken_covenant(draw, c):
    cx, cy = c
    draw.ellipse([cx - 240, cy - 140, cx + 60, cy + 160], outline=(234, 179, 8, 255), width=24)
    draw.ellipse([cx - 60, cy - 140, cx + 240, cy + 160], outline=(234, 179, 8, 255), width=24)

# 111. Revelation Order
def d_revelation_order(draw, c):
    cx, cy = c
    draw.rectangle([cx - 320, cy - 200, cx + 320, cy + 240], fill=(254, 243, 199, 255), outline=(220, 38, 38, 255), width=12)
    draw.ellipse([cx - 100, cy - 60, cx + 100, cy + 100], fill=(234, 88, 12, 255))

# 112. Duty Roster Watch
def d_duty_roster(draw, c):
    cx, cy = c
    draw.ellipse([cx - 340, cy - 340, cx + 340, cy + 340], fill=(24, 24, 27, 255), outline=(245, 158, 11, 255), width=24)
    draw.line([cx, cy, cx, cy - 220], fill=(255, 255, 255, 255), width=18)
    draw.line([cx, cy, cx + 160, cy], fill=(255, 255, 255, 255), width=18)

# 113. Dawn Harbingers
def d_dawn_harbingers(draw, c):
    cx, cy = c
    draw.chord([cx - 380, cy - 140, cx + 380, cy + 380], 180, 360, fill=(250, 204, 21, 255), outline=(255, 255, 255, 255), width=14)
    for a_deg in range(200, 350, 25):
        a = math.radians(a_deg)
        draw.line([cx + int(240*math.cos(a)), cy + int(240*math.sin(a)), cx + int(420*math.cos(a)), cy + int(420*math.sin(a))], fill=(245, 158, 11, 255), width=18)

fifty_factions = [
    ("faction_icon_vault_09_technocrats.png", (15, 23, 42, 255), (56, 189, 248, 255), (255, 255, 255, 255), d_vault_09),
    ("faction_icon_deep_strata_drillers.png", (30, 25, 20, 255), (245, 158, 11, 255), (234, 88, 12, 255), d_strata_drillers),
    ("faction_icon_the_silent_monks.png", (20, 15, 30, 255), (147, 51, 234, 255), (254, 240, 138, 255), d_silent_monks),
    ("faction_icon_bunker_alpha_sentinels.png", (20, 25, 35, 255), (220, 38, 38, 255), (203, 213, 225, 255), d_bunker_alpha),
    ("faction_icon_airlock_wardens.png", (24, 24, 27, 255), (250, 204, 21, 255), (255, 255, 255, 255), d_airlock_wardens),
    ("faction_icon_hydro_core_custodians.png", (10, 30, 45, 255), (14, 165, 233, 255), (56, 189, 248, 255), d_hydro_core),
    ("faction_icon_geothermal_stokers.png", (45, 15, 10, 255), (234, 88, 12, 255), (250, 204, 21, 255), d_geothermal_stokers),
    ("faction_icon_sub_vault_metallurgists.png", (25, 30, 35, 255), (245, 158, 11, 255), (250, 204, 21, 255), d_metallurgists),
    ("faction_icon_atmospheric_scrubbers.png", (15, 35, 25, 255), (34, 197, 94, 255), (74, 222, 128, 255), d_scrubbers),
    ("faction_icon_vault_chronicle_scribes.png", (35, 25, 15, 255), (217, 119, 6, 255), (254, 243, 199, 255), d_scribes),
    ("faction_icon_rad_storm_riders.png", (30, 20, 15, 255), (245, 158, 11, 255), (239, 68, 68, 255), d_storm_riders),
    ("faction_icon_ashen_crows.png", (20, 20, 25, 255), (148, 163, 184, 255), (226, 232, 240, 255), d_ashen_crows),
    ("faction_icon_bone_carvers.png", (30, 25, 20, 255), (180, 83, 9, 255), (243, 244, 246, 255), d_bone_carvers),
    ("faction_icon_glass_walkers.png", (10, 35, 25, 255), (16, 185, 129, 255), (255, 255, 255, 255), d_glass_walkers),
    ("faction_icon_permafrost_trappers.png", (20, 30, 40, 255), (148, 163, 184, 255), (226, 232, 240, 255), d_trappers),
    ("faction_icon_scrapland_jackals.png", (35, 25, 20, 255), (234, 88, 12, 255), (239, 68, 68, 255), d_jackals),
    ("faction_icon_the_drifters.png", (40, 30, 15, 255), (234, 179, 8, 255), (0, 0, 0, 255), d_drifters),
    ("faction_icon_sand_sailors.png", (45, 30, 15, 255), (245, 158, 11, 255), (255, 255, 255, 255), d_sand_sailors),
    ("faction_icon_rust_scorpions.png", (35, 20, 10, 255), (234, 88, 12, 255), (239, 68, 68, 255), d_rust_scorpions),
    ("faction_icon_the_vultures.png", (25, 20, 25, 255), (220, 38, 38, 255), (203, 213, 225, 255), d_ridge_vultures),
    ("faction_icon_frozen_sound_whalers.png", (15, 30, 45, 255), (14, 165, 233, 255), (226, 232, 240, 255), d_whalers),
    ("faction_icon_deep_salvage_divers.png", (10, 25, 35, 255), (234, 179, 8, 255), (56, 189, 248, 255), d_deep_divers),
    ("faction_icon_icebreaker_coalition.png", (15, 35, 50, 255), (2, 132, 199, 255), (255, 255, 255, 255), d_icebreaker_coalition),
    ("faction_icon_lighthouse_keepers.png", (20, 30, 40, 255), (250, 204, 21, 255), (226, 232, 240, 255), d_lighthouse),
    ("faction_icon_kelp_harvesters.png", (10, 35, 30, 255), (13, 148, 136, 255), (52, 211, 153, 255), d_kelp),
    ("faction_icon_salt_marsh_poachers.png", (25, 35, 30, 255), (71, 85, 105, 255), (234, 88, 12, 255), d_marsh_poachers),
    ("faction_icon_coastal_ferrymen.png", (20, 25, 35, 255), (180, 83, 9, 255), (245, 158, 11, 255), d_ferrymen),
    ("faction_icon_sunken_rig_squatters.png", (30, 20, 20, 255), (220, 38, 38, 255), (148, 163, 184, 255), d_sunken_rig),
    ("faction_icon_driftwood_carvers.png", (35, 25, 15, 255), (120, 53, 15, 255), (255, 255, 255, 255), d_driftwood),
    ("faction_icon_arctic_harbor_masters.png", (20, 30, 45, 255), (245, 158, 11, 255), (226, 232, 240, 255), d_harbor_masters),
    ("faction_icon_century_seed_guardians.png", (15, 40, 25, 255), (74, 222, 128, 255), (250, 204, 21, 255), d_century_seed),
    ("faction_icon_chelation_order.png", (20, 30, 50, 255), (30, 58, 138, 255), (255, 255, 255, 255), d_chelation),
    ("faction_icon_prosthetic_guild.png", (30, 25, 20, 255), (217, 119, 6, 255), (203, 213, 225, 255), d_prosthetic_guild),
    ("faction_icon_apothecary_circle.png", (30, 35, 25, 255), (180, 83, 9, 255), (255, 255, 255, 255), d_apothecary),
    ("faction_icon_rad_shield_physicists.png", (25, 30, 40, 255), (245, 158, 11, 255), (239, 68, 68, 255), d_rad_shield),
    ("faction_icon_myco_remediators.png", (25, 15, 30, 255), (168, 85, 247, 255), (255, 255, 255, 255), d_myco_remediators),
    ("faction_icon_blood_bankers.png", (35, 10, 15, 255), (225, 29, 72, 255), (255, 255, 255, 255), d_blood_bankers),
    ("faction_icon_optometric_weavers.png", (20, 30, 35, 255), (245, 158, 11, 255), (203, 213, 225, 255), d_optometric),
    ("faction_icon_trauma_surgeons_guild.png", (30, 15, 20, 255), (220, 38, 38, 255), (226, 232, 240, 255), d_trauma_surgeons),
    ("faction_icon_genome_preservationists.png", (15, 25, 40, 255), (14, 165, 233, 255), (56, 189, 248, 255), d_genome_preserv),
    ("faction_icon_the_verdict_tribunal.png", (35, 25, 15, 255), (180, 83, 9, 255), (255, 255, 255, 255), d_verdict_tribunal),
    ("faction_icon_standing_record_legion.png", (25, 30, 40, 255), (245, 158, 11, 255), (255, 255, 255, 255), d_standing_record),
    ("faction_icon_nobodys_charter_pioneers.png", (30, 25, 20, 255), (180, 83, 9, 255), (203, 213, 225, 255), d_nobodys_pioneers),
    ("faction_icon_year_of_ash_climatologists.png", (24, 24, 27, 255), (245, 158, 11, 255), (239, 68, 68, 255), d_year_of_ash),
    ("faction_icon_silent_foundry_automatons.png", (20, 25, 35, 255), (56, 189, 248, 255), (255, 255, 255, 255), d_silent_foundry),
    ("faction_icon_deep_coast_cartographers.png", (15, 25, 40, 255), (14, 165, 233, 255), (245, 158, 11, 255), d_deep_cartographers),
    ("faction_icon_the_unbroken_covenant.png", (30, 25, 20, 255), (234, 179, 8, 255), (255, 255, 255, 255), d_unbroken_covenant),
    ("faction_icon_revelation_order.png", (35, 15, 15, 255), (234, 88, 12, 255), (220, 38, 38, 255), d_revelation_order),
    ("faction_icon_duty_roster_watch.png", (24, 24, 27, 255), (245, 158, 11, 255), (255, 255, 255, 255), d_duty_roster),
    ("faction_icon_the_dawn_harbingers.png", (40, 25, 10, 255), (250, 204, 21, 255), (255, 255, 255, 255), d_dawn_harbingers),
]

print(f"Generating 50 Additional 2K Faction Emblems (Total 113 Factions)...")
for filename, bg, bdr, acc, draw_fn in fifty_factions:
    create_emblem(filename, bg, bdr, acc, draw_fn)

print(f"\nAll 50 Additional 2K Faction Emblems generated and saved successfully!")
