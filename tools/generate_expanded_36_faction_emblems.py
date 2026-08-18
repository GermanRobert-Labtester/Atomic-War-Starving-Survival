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

def create_faction_emblem(bg_col, border_col, accent_col, draw_custom_elements):
    img, draw = create_base_canvas()
    draw_textured_shield(draw, CENTER, RADIUS, bg_col, border_col, accent_col)
    draw_custom_elements(draw, CENTER)
    return img

# 1. ASH MILITIA
def draw_ash_militia(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(25, 25, 25, 255), outline=(156, 163, 175, 255), width=16)
    # Crossed Bayonets
    draw.line([cx - 360, cy + 360, cx + 340, cy - 340], fill=(229, 231, 235, 255), width=26)
    draw.line([cx + 360, cy + 360, cx - 340, cy - 340], fill=(229, 231, 235, 255), width=26)
    # Burning Ember Heart
    draw.ellipse([cx - 160, cy - 160, cx + 160, cy + 160], fill=(220, 38, 38, 255), outline=(250, 204, 21, 255), width=14)
    draw.ellipse([cx - 80, cy - 80, cx + 80, cy + 80], fill=(254, 240, 138, 255))

# 2. ASH SIGN
def draw_ash_sign(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 20, 20, 255), outline=(239, 68, 68, 255), width=16)
    # Ash-Stained Handprint
    draw.ellipse([cx - 160, cy - 40, cx + 160, cy + 240], fill=(75, 85, 99, 255)) # Palm
    for i, x in enumerate([-180, -90, 0, 90, 180]): # Fingers
        draw.line([cx + x * 0.8, cy + 20, cx + x * 0.9, cy - 280 + abs(x) * 0.5], fill=(75, 85, 99, 255), width=48)
    # Radiant White Radiation Trefoil on Palm
    draw.ellipse([cx - 50, cy + 80, cx + 50, cy + 180], fill=(255, 255, 255, 255))

# 3. BLACK FLOTILLA
def draw_black_flotilla(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(10, 15, 25, 255), outline=(14, 165, 233, 255), width=16)
    # Pirate Icebreaker Sail & Crossed Harpoons
    draw.polygon([(cx, cy - 440), (cx + 260, cy + 120), (cx - 260, cy + 120)], fill=(30, 41, 59, 255), outline=(255, 255, 255, 255), width=10)
    # Hull
    draw.polygon([(cx - 360, cy + 140), (cx + 360, cy + 140), (cx + 240, cy + 320), (cx - 240, cy + 320)], fill=(15, 23, 42, 255), outline=(14, 165, 233, 255), width=12)
    # Harpoon Lines
    draw.line([cx - 400, cy - 200, cx + 400, cy + 380], fill=(226, 232, 240, 255), width=18)
    draw.line([cx + 400, cy - 200, cx - 400, cy + 380], fill=(226, 232, 240, 255), width=18)

# 4. BLACK OPS
def draw_black_ops(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(12, 12, 14, 255), outline=(71, 85, 105, 255), width=16)
    # Combat Stiletto Dagger through Gas Mask Filter
    draw.ellipse([cx - 280, cy - 100, cx + 280, cy + 360], fill=(30, 41, 59, 255), outline=(220, 38, 38, 255), width=12)
    draw.line([cx, cy - 500, cx, cy + 440], fill=(203, 213, 225, 255), width=32) # Blade
    draw.line([cx - 140, cy - 260, cx + 140, cy - 260], fill=(148, 163, 184, 255), width=24) # Guard

# 5. CENTRAL GARRISON
def draw_central_garrison(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 30, 45, 255), outline=(245, 158, 11, 255), width=16)
    # Fortified Bastion Castle Keep
    draw.rectangle([cx - 320, cy - 200, cx + 320, cy + 340], fill=(71, 85, 105, 255), outline=(255, 255, 255, 255), width=12)
    # Crenellations
    for x in [-320, -160, 0, 160, 260]:
        draw.rectangle([cx + x, cy - 320, cx + x + 60, cy - 200], fill=(71, 85, 105, 255), outline=(255, 255, 255, 255), width=8)
    # Portcullis Arch
    draw.arc([cx - 120, cy + 100, cx + 120, cy + 340], 180, 360, fill=(245, 158, 11, 255), width=16)

# 6. THE CHOKE
def draw_the_choke(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(25, 15, 15, 255), outline=(220, 38, 38, 255), width=16)
    # Heavy Spiked Trap Jaw & Binding Chains
    for y in range(cy - 300, cy + 360, 120):
        draw.line([cx - 380, y, cx + 380, y], fill=(148, 163, 184, 255), width=28)
    draw.polygon([(cx - 380, cy - 320), (cx, cy - 420), (cx + 380, cy - 320)], fill=(220, 38, 38, 255))
    draw.polygon([(cx - 380, cy + 320), (cx, cy + 420), (cx + 380, cy + 320)], fill=(220, 38, 38, 255))

# 7. GELATIN FOUL
def draw_gelatin_foul(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(10, 40, 30, 255), outline=(132, 204, 22, 255), width=16)
    # Chemical Flask with Bubbling Mutated Gelatin
    draw.polygon([(cx - 80, cy - 400), (cx + 80, cy - 400), (cx + 80, cy - 180), (cx + 320, cy + 280), (cx - 320, cy + 280), (cx - 80, cy - 180)], fill=(6, 95, 70, 200), outline=(255, 255, 255, 255), width=12)
    # Bubbles
    for bx, by, br in [(-120, 120, 40), (60, 80, 50), (-40, -40, 30), (140, 180, 35)]:
        draw.ellipse([cx + bx - br, cy + by - br, cx + bx + br, cy + by + br], fill=(163, 230, 53, 255))

# 8. GREEN THREAD
def draw_green_thread(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 40, 25, 255), outline=(74, 222, 128, 255), width=16)
    # Spool of Silk Thread with Living Shoot
    draw.ellipse([cx - 240, cy - 360, cx + 240, cy - 200], fill=(180, 83, 9, 255), outline=(255, 255, 255, 255), width=8)
    draw.rectangle([cx - 180, cy - 280, cx + 180, cy + 220], fill=(34, 197, 94, 255), outline=(255, 255, 255, 255), width=8)
    draw.ellipse([cx - 240, cy + 140, cx + 240, cy + 300], fill=(180, 83, 9, 255), outline=(255, 255, 255, 255), width=8)
    # Winding Thread
    draw.arc([cx - 380, cy - 240, cx + 380, cy + 240], 30, 150, fill=(250, 204, 21, 255), width=14)

# 9. HAIR SLIP
def draw_hair_slip(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(30, 25, 20, 255), outline=(217, 119, 6, 255), width=16)
    # Bone Comb & Slit Razor Blade
    draw.rectangle([cx - 320, cy - 180, cx + 320, cy + 180], fill=(229, 231, 235, 255), outline=(0,0,0,255), width=8)
    for x in range(cx - 280, cx + 290, 40):
        draw.line([x, cy - 180, x, cy - 340], fill=(217, 119, 6, 255), width=12)

# 10. IRON COVENANT
def draw_iron_covenant(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(25, 25, 30, 255), outline=(245, 158, 11, 255), width=16)
    # Iron Anvil & Molten Sledgehammer
    anvil = [(cx - 360, cy + 40), (cx + 360, cy + 40), (cx + 260, cy + 280), (cx - 260, cy + 280)]
    draw.polygon(anvil, fill=(71, 85, 105, 255), outline=(255, 255, 255, 255), width=10)
    # Hammer
    draw.line([cx - 200, cy - 380, cx + 180, cy + 100], fill=(180, 83, 9, 255), width=28)
    draw.rectangle([cx - 300, cy - 460, cx - 120, cy - 320], fill=(245, 158, 11, 255), outline=(255, 255, 255, 255), width=8)

# 11. JOINT TREATY GUARD
def draw_joint_treaty_guard(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 30, 50, 255), outline=(56, 189, 248, 255), width=16)
    # Clasping Gauntlets over Treaty Scroll
    draw.rectangle([cx - 280, cy - 320, cx + 280, cy + 320], fill=(243, 244, 246, 255), outline=(0,0,0,255), width=8)
    draw.line([cx - 340, cy, cx + 340, cy], fill=(14, 165, 233, 255), width=36)
    draw.ellipse([cx - 120, cy - 80, cx + 120, cy + 80], fill=(217, 119, 6, 255))

# 12. LEDGER KEEPERS
def draw_ledger_keepers(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(30, 20, 10, 255), outline=(234, 179, 8, 255), width=16)
    # Open Ledger Folio & Balance Beam
    draw.rectangle([cx - 340, cy - 240, cx + 340, cy + 240], fill=(254, 243, 199, 255), outline=(120, 53, 15, 255), width=12)
    draw.line([cx, cy - 240, cx, cy + 240], fill=(120, 53, 15, 255), width=8)
    draw.line([cx - 240, cy - 80, cx + 240, cy - 80], fill=(220, 38, 38, 255), width=16)

# 13. ORDNANCE FOUNDRY
def draw_ordnance_foundry(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(35, 20, 15, 255), outline=(234, 88, 12, 255), width=16)
    # Heavy Artillery Shell & Molten Ladle
    draw.polygon([(cx, cy - 440), (cx + 180, cy - 140), (cx + 180, cy + 340), (cx - 180, cy + 340), (cx - 180, cy - 140)], fill=(100, 116, 139, 255), outline=(255, 255, 255, 255), width=10)
    draw.ellipse([cx - 100, cy + 20, cx + 100, cy + 220], fill=(249, 115, 22, 255))

# 14. PENAL BATTALION
def draw_penal_battalion(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 20, 20, 255), outline=(185, 28, 28, 255), width=16)
    # Broken Ball & Chain with Trench Pick
    draw.ellipse([cx - 300, cy + 40, cx + 20, cy + 360], fill=(51, 65, 85, 255), outline=(220, 38, 38, 255), width=14)
    draw.line([cx - 140, cy + 40, cx + 280, cy - 380], fill=(203, 213, 225, 255), width=32)

# 15. RAILWAY GUILD
def draw_railway_guild(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(25, 30, 40, 255), outline=(226, 232, 240, 255), width=16)
    # Crossed Steel Rails & Locomotive Cowcatcher
    draw.line([cx - 360, cy + 360, cx + 360, cy - 360], fill=(203, 213, 225, 255), width=32)
    draw.line([cx + 360, cy + 360, cx - 360, cy - 360], fill=(203, 213, 225, 255), width=32)
    draw.polygon([(cx, cy - 240), (cx + 260, cy + 200), (cx - 260, cy + 200)], fill=(180, 83, 9, 255), outline=(255, 255, 255, 255), width=10)

# 16. REBUILDERS
def draw_rebuilders(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(30, 35, 45, 255), outline=(245, 158, 11, 255), width=16)
    # Masonry Trowel & Carpenter Square
    draw.polygon([(cx, cy - 380), (cx + 180, cy + 80), (cx - 180, cy + 80)], fill=(203, 213, 225, 255), outline=(255, 255, 255, 255), width=10)
    draw.line([cx, cy + 80, cx, cy + 320], fill=(180, 83, 9, 255), width=24)
    # Brick Grid
    for y in range(cy + 160, cy + 360, 50):
        draw.line([cx - 280, y, cx + 280, y], fill=(245, 158, 11, 255), width=6)

# 17. SALT FREEHOLDERS
def draw_salt_freeholders(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(15, 30, 35, 255), outline=(204, 251, 241, 255), width=16)
    # Crystalline Salt Pyramid
    draw.polygon([(cx, cy - 420), (cx + 340, cy + 260), (cx - 340, cy + 260)], fill=(240, 253, 250, 255), outline=(13, 148, 136, 255), width=14)
    draw.line([(cx, cy - 420), (cx, cy + 260)], fill=(13, 148, 136, 255), width=8)

# 18. SUPPLY CORPS
def draw_supply_corps(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(30, 35, 25, 255), outline=(234, 179, 8, 255), width=16)
    # Heavy Truck Wheel & Winged Logistics Chevron
    draw.ellipse([cx - 360, cy - 360, cx + 360, cy + 360], fill=(15, 23, 42, 255), outline=(234, 179, 8, 255), width=24)
    draw.polygon([(cx, cy - 240), (cx + 200, cy + 120), (cx - 200, cy + 120)], fill=(234, 179, 8, 255))

# 19. THE BOTANISTS
def draw_the_botanists(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(15, 35, 20, 255), outline=(52, 211, 153, 255), width=16)
    # Terrarium Bell Jar with Radiant Flower
    draw.chord([cx - 240, cy - 440, cx + 240, cy + 160], 180, 360, fill=(6, 78, 59, 180), outline=(255, 255, 255, 255), width=10)
    draw.ellipse([cx - 80, cy - 140, cx + 80, cy + 20], fill=(244, 63, 94, 255)) # Flower
    draw.line([cx, cy + 20, cx, cy + 280], fill=(34, 197, 94, 255), width=18)

# 20. BREWERS GUILD
def draw_brewers_guild(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(45, 25, 15, 255), outline=(245, 158, 11, 255), width=16)
    # Copper Distillation Pot Still & Cask
    draw.ellipse([cx - 280, cy - 200, cx + 280, cy + 320], fill=(180, 83, 9, 255), outline=(255, 255, 255, 255), width=12)
    draw.line([cx, cy - 200, cx, cy - 420], fill=(217, 119, 6, 255), width=24)
    draw.arc([cx - 180, cy - 440, cx + 180, cy - 240], 0, 180, fill=(217, 119, 6, 255), width=24)

# 21. CANAL SMUGGLERS
def draw_canal_smugglers(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(10, 20, 30, 255), outline=(56, 189, 248, 255), width=16)
    # Flat Barge & Muffled Crescent Moon
    draw.polygon([(cx - 380, cy + 80), (cx + 380, cy + 80), (cx + 280, cy + 280), (cx - 280, cy + 280)], fill=(30, 41, 59, 255), outline=(56, 189, 248, 255), width=10)
    draw.ellipse([cx - 240, cy - 420, cx - 40, cy - 220], fill=(248, 250, 252, 255))
    draw.ellipse([cx - 200, cy - 430, cx, cy - 230], fill=(10, 20, 30, 255))

# 22. THE COMMUNE
def draw_the_commune(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(35, 15, 15, 255), outline=(239, 68, 68, 255), width=16)
    # Crossed Sickle & Wrench
    draw.arc([cx - 320, cy - 320, cx + 80, cy + 80], 90, 270, fill=(234, 179, 8, 255), width=32)
    draw.line([cx - 80, cy - 340, cx + 320, cy + 280], fill=(203, 213, 225, 255), width=28)

# 23. FREE REPUBLIC
def draw_free_republic(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 30, 50, 255), outline=(234, 179, 8, 255), width=16)
    # Soaring Eagle / Hawk of Liberty
    eagle = [(cx, cy - 340), (cx + 280, cy - 80), (cx + 120, cy + 120), (cx, cy + 40), (cx - 120, cy + 120), (cx - 280, cy - 80)]
    draw.polygon(eagle, fill=(234, 179, 8, 255), outline=(255, 255, 255, 255), width=8)
    draw.polygon([(cx - 180, cy + 180), (cx + 180, cy + 180), (cx + 120, cy + 300), (cx - 120, cy + 300)], fill=(220, 38, 38, 255))

# 24. FREE TRADERS
def draw_free_traders(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(30, 40, 35, 255), outline=(16, 185, 129, 255), width=16)
    # Copper Trade Scales & Stacked Coins
    draw.line([cx - 360, cy - 140, cx + 360, cy - 140], fill=(245, 158, 11, 255), width=24)
    draw.ellipse([cx - 280, cy + 20, cx - 120, cy + 180], fill=(217, 119, 6, 255))
    draw.ellipse([cx + 120, cy + 20, cx + 280, cy + 180], fill=(217, 119, 6, 255))

# 25. GRID ENGINEERS
def draw_grid_engineers(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(15, 25, 35, 255), outline=(56, 189, 248, 255), width=16)
    # High-Voltage Pylon & Sparks
    draw.polygon([(cx, cy - 420), (cx + 220, cy + 360), (cx - 220, cy + 360)], outline=(56, 189, 248, 255), width=16)
    draw.line([cx - 320, cy - 140, cx + 320, cy - 140], fill=(245, 158, 11, 255), width=18)
    draw.line([cx - 240, cy + 100, cx + 240, cy + 100], fill=(245, 158, 11, 255), width=18)

# 26. THE LEECHERS
def draw_the_leechers(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(30, 10, 15, 255), outline=(225, 29, 72, 255), width=16)
    # Medical Leech Silhouette & Blood Drops
    draw.polygon([(cx, cy - 380), (cx + 140, cy), (cx, cy + 380), (cx - 140, cy)], fill=(136, 19, 55, 255), outline=(255, 255, 255, 255), width=10)
    draw.ellipse([cx - 40, cy - 180, cx + 40, cy - 100], fill=(244, 63, 94, 255))

# 27. MINERS UNION
def draw_miners_union(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(25, 25, 25, 255), outline=(245, 158, 11, 255), width=16)
    # Crossed Mining Picks & Carbide Lamp
    draw.line([cx - 360, cy + 360, cx + 360, cy - 360], fill=(148, 163, 184, 255), width=28)
    draw.line([cx + 360, cy + 360, cx - 360, cy - 360], fill=(148, 163, 184, 255), width=28)
    draw.ellipse([cx - 120, cy - 120, cx + 120, cy + 120], fill=(250, 204, 21, 255), outline=(0,0,0,255), width=8)

# 28. NOMAD CLANS
def draw_nomad_clans(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(45, 30, 20, 255), outline=(217, 119, 6, 255), width=16)
    # Yurt Tent Silhouette under Starlit Crescent
    draw.polygon([(cx, cy - 340), (cx + 340, cy + 180), (cx - 340, cy + 180)], fill=(180, 83, 9, 255), outline=(255, 255, 255, 255), width=10)
    draw.arc([cx - 80, cy + 20, cx + 80, cy + 180], 180, 360, fill=(245, 158, 11, 255), width=12)

# 29. PITCH BURNERS
def draw_pitch_burners(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(30, 15, 10, 255), outline=(234, 88, 12, 255), width=16)
    # Flaming Pitch Torch with Dripping Black Resin
    draw.line([cx - 120, cy + 380, cx + 120, cy - 180], fill=(120, 53, 15, 255), width=36)
    draw.polygon([(cx - 80, cy - 460), (cx + 180, cy - 140), (cx - 40, cy - 140)], fill=(249, 115, 22, 255), outline=(254, 240, 138, 255), width=10)

# 30. QUARRY BARONS
def draw_quarry_barons(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(35, 35, 40, 255), outline=(156, 163, 175, 255), width=16)
    # Heavy Stone Block & Quarry Splitter Chisel
    draw.rectangle([cx - 280, cy - 140, cx + 280, cy + 320], fill=(100, 116, 139, 255), outline=(255, 255, 255, 255), width=12)
    draw.polygon([(cx, cy - 420), (cx + 80, cy - 140), (cx - 80, cy - 140)], fill=(217, 119, 6, 255), outline=(0,0,0,255), width=6)

# 31. SISTERHOOD OF SOLACE
def draw_the_sisterhood(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 30, 45, 255), outline=(244, 114, 182, 255), width=16)
    # Veiled Sisterhood Silver Nursing Cross
    draw.rectangle([cx - 60, cy - 400, cx + 60, cy + 400], fill=(244, 114, 182, 255))
    draw.rectangle([cx - 280, cy - 140, cx + 280, cy - 20], fill=(244, 114, 182, 255))
    draw.ellipse([cx - 180, cy - 180, cx + 180, cy + 180], outline=(255, 255, 255, 255), width=12)

# 32. THE SYNDICATE
def draw_the_syndicate(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(15, 15, 20, 255), outline=(168, 85, 247, 255), width=16)
    # Hooded Shadow & Concealed Stiletto
    draw.chord([cx - 260, cy - 380, cx + 260, cy + 180], 180, 360, fill=(30, 27, 75, 255), outline=(168, 85, 247, 255), width=12)
    draw.line([cx, cy - 80, cx, cy + 420], fill=(226, 232, 240, 255), width=24)

# 33. TUNNEL RATS
def draw_the_tunnel_rats(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(25, 20, 20, 255), outline=(245, 158, 11, 255), width=16)
    # Subterranean Rat Silhouette & Flashlight Beam
    draw.polygon([(cx, cy - 100), (cx + 360, cy - 340), (cx + 360, cy + 140)], fill=(254, 240, 138, 150))
    draw.ellipse([cx - 240, cy - 200, cx + 80, cy + 120], fill=(51, 65, 85, 255), outline=(245, 158, 11, 255), width=10)

# 34. THE WATCHERS
def draw_the_watchers(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(15, 25, 35, 255), outline=(250, 204, 21, 255), width=16)
    # Lookout Sentry Tower & Sweeping Searchlight
    draw.polygon([(cx - 180, cy - 200), (cx + 180, cy - 200), (cx + 280, cy + 380), (cx - 280, cy + 380)], fill=(71, 85, 105, 255), outline=(255, 255, 255, 255), width=10)
    draw.ellipse([cx - 100, cy - 360, cx + 100, cy - 160], fill=(250, 204, 21, 255), outline=(0,0,0,255), width=8)

# 35. WIRE TAPPERS
def draw_the_wire_tappers(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 20, 30, 255), outline=(56, 189, 248, 255), width=16)
    # Headphones & Tapped Telephone Wire
    draw.arc([cx - 260, cy - 360, cx + 260, cy + 60], 180, 360, fill=(148, 163, 184, 255), width=28)
    draw.ellipse([cx - 320, cy - 100, cx - 160, cy + 140], fill=(30, 41, 59, 255), outline=(56, 189, 248, 255), width=12)
    draw.ellipse([cx + 160, cy - 100, cx + 320, cy + 140], fill=(30, 41, 59, 255), outline=(56, 189, 248, 255), width=12)

# 36. WANDERING MENDERS
def draw_wandering_menders(draw, c):
    cx, cy = c
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(25, 35, 30, 255), outline=(52, 211, 153, 255), width=16)
    # Doctor Field Bag with Medical Red Cross & Herbal Balm
    draw.rectangle([cx - 320, cy - 140, cx + 320, cy + 280], fill=(120, 53, 15, 255), outline=(255, 255, 255, 255), width=12)
    draw.arc([cx - 160, cy - 300, cx + 160, cy - 80], 180, 360, fill=(180, 83, 9, 255), width=24) # Handle
    draw.rectangle([cx - 30, cy - 60, cx + 30, cy + 200], fill=(220, 38, 38, 255))
    draw.rectangle([cx - 120, cy + 30, cx + 120, cy + 110], fill=(220, 38, 38, 255))

expanded_factions = [
    ("faction_icon_ash_militia.png", (30, 30, 30, 255), (156, 163, 175, 255), (220, 38, 38, 255), draw_ash_militia),
    ("faction_icon_ash_sign.png", (20, 20, 20, 255), (239, 68, 68, 255), (255, 255, 255, 255), draw_ash_sign),
    ("faction_icon_black_flotilla.png", (10, 15, 25, 255), (14, 165, 233, 255), (226, 232, 240, 255), draw_black_flotilla),
    ("faction_icon_black_ops.png", (12, 12, 14, 255), (71, 85, 105, 255), (220, 38, 38, 255), draw_black_ops),
    ("faction_icon_central_garrison.png", (20, 30, 45, 255), (245, 158, 11, 255), (71, 85, 105, 255), draw_central_garrison),
    ("faction_icon_choke.png", (25, 15, 15, 255), (220, 38, 38, 255), (148, 163, 184, 255), draw_the_choke),
    ("faction_icon_gelatin_foul.png", (10, 40, 30, 255), (132, 204, 22, 255), (163, 230, 53, 255), draw_gelatin_foul),
    ("faction_icon_green_thread.png", (20, 40, 25, 255), (74, 222, 128, 255), (250, 204, 21, 255), draw_green_thread),
    ("faction_icon_hair_slip.png", (30, 25, 20, 255), (217, 119, 6, 255), (229, 231, 235, 255), draw_hair_slip),
    ("faction_icon_iron_covenant.png", (25, 25, 30, 255), (245, 158, 11, 255), (71, 85, 105, 255), draw_iron_covenant),
    ("faction_icon_joint_treaty_guard.png", (20, 30, 50, 255), (56, 189, 248, 255), (217, 119, 6, 255), draw_joint_treaty_guard),
    ("faction_icon_ledger_keepers.png", (30, 20, 10, 255), (234, 179, 8, 255), (220, 38, 38, 255), draw_ledger_keepers),
    ("faction_icon_ordnance_foundry.png", (35, 20, 15, 255), (234, 88, 12, 255), (249, 115, 22, 255), draw_ordnance_foundry),
    ("faction_icon_penal_battalion.png", (20, 20, 20, 255), (185, 28, 28, 255), (203, 213, 225, 255), draw_penal_battalion),
    ("faction_icon_railway_guild.png", (25, 30, 40, 255), (226, 232, 240, 255), (180, 83, 9, 255), draw_railway_guild),
    ("faction_icon_rebuilders.png", (30, 35, 45, 255), (245, 158, 11, 255), (203, 213, 225, 255), draw_rebuilders),
    ("faction_icon_salt_freeholders.png", (15, 30, 35, 255), (204, 251, 241, 255), (13, 148, 136, 255), draw_salt_freeholders),
    ("faction_icon_supply_corps.png", (30, 35, 25, 255), (234, 179, 8, 255), (255, 255, 255, 255), draw_supply_corps),
    ("faction_icon_the_botanists.png", (15, 35, 20, 255), (52, 211, 153, 255), (244, 63, 94, 255), draw_the_botanists),
    ("faction_icon_the_brewers_guild.png", (45, 25, 15, 255), (245, 158, 11, 255), (217, 119, 6, 255), draw_brewers_guild),
    ("faction_icon_the_canal_smugglers.png", (10, 20, 30, 255), (56, 189, 248, 255), (248, 250, 252, 255), draw_canal_smugglers),
    ("faction_icon_the_commune.png", (35, 15, 15, 255), (239, 68, 68, 255), (234, 179, 8, 255), draw_the_commune),
    ("faction_icon_the_free_republic.png", (20, 30, 50, 255), (234, 179, 8, 255), (220, 38, 38, 255), draw_free_republic),
    ("faction_icon_the_free_traders.png", (30, 40, 35, 255), (16, 185, 129, 255), (245, 158, 11, 255), draw_free_traders),
    ("faction_icon_the_grid_engineers.png", (15, 25, 35, 255), (56, 189, 248, 255), (245, 158, 11, 255), draw_grid_engineers),
    ("faction_icon_the_leechers.png", (30, 10, 15, 255), (225, 29, 72, 255), (244, 63, 94, 255), draw_the_leechers),
    ("faction_icon_the_miners_union.png", (25, 25, 25, 255), (245, 158, 11, 255), (148, 163, 184, 255), draw_miners_union),
    ("faction_icon_the_nomad_clans.png", (45, 30, 20, 255), (217, 119, 6, 255), (180, 83, 9, 255), draw_nomad_clans),
    ("faction_icon_the_pitch_burners.png", (30, 15, 10, 255), (234, 88, 12, 255), (249, 115, 22, 255), draw_pitch_burners),
    ("faction_icon_the_quarry_barons.png", (35, 35, 40, 255), (156, 163, 175, 255), (217, 119, 6, 255), draw_quarry_barons),
    ("faction_icon_the_sisterhood.png", (20, 30, 45, 255), (244, 114, 182, 255), (255, 255, 255, 255), draw_the_sisterhood),
    ("faction_icon_the_syndicate.png", (15, 15, 20, 255), (168, 85, 247, 255), (226, 232, 240, 255), draw_the_syndicate),
    ("faction_icon_the_tunnel_rats.png", (25, 20, 20, 255), (245, 158, 11, 255), (254, 240, 138, 255), draw_the_tunnel_rats),
    ("faction_icon_the_watchers.png", (15, 25, 35, 255), (250, 204, 21, 255), (71, 85, 105, 255), draw_the_watchers),
    ("faction_icon_the_wire_tappers.png", (20, 20, 30, 255), (56, 189, 248, 255), (148, 163, 184, 255), draw_the_wire_tappers),
    ("faction_icon_wandering_menders.png", (25, 35, 30, 255), (52, 211, 153, 255), (220, 38, 38, 255), draw_wandering_menders),
]

print(f"Generating {len(expanded_factions)} Expanded 2K Faction Emblems...")
for filename, bg, bdr, acc, draw_func in expanded_factions:
    img = create_faction_emblem(bg, bdr, acc, draw_func)
    out_path_icons = os.path.join(OUTPUT_DIR, filename)
    img.save(out_path_icons, 'PNG', optimize=True)
    out_path_emblems = os.path.join(EMBLEMS_DIR, filename)
    img.save(out_path_emblems, 'PNG', optimize=True)
    print(f"-> Generated 2K Emblem: {filename}")

print(f"\nAll {len(expanded_factions)} Expanded 2K Faction Emblems generated and saved successfully!")
