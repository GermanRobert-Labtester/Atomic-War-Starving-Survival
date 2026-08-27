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
    # Outer tactical border ring
    for offset in range(30):
        draw.ellipse([cx - r + offset, cy - r + offset, cx + r - offset, cy + r - offset],
                     outline=(border_color[0], border_color[1], border_color[2], int(255 - offset * 6)), width=2)

    # Solid background shield
    draw.ellipse([cx - r + 30, cy - r + 30, cx + r - 30, cy + r - 30], fill=bg_color)

    # Inner metallic bezel
    draw.ellipse([cx - r + 60, cy - r + 60, cx + r - 60, cy + r - 60], outline=accent_color, width=12)
    draw.ellipse([cx - r + 90, cy - r + 90, cx + r - 90, cy + r - 90], outline=(40, 40, 40, 255), width=6)

    # Screw rivets
    num_rivets = 24
    for i in range(num_rivets):
        angle = (2 * math.pi / num_rivets) * i
        rx = cx + int((r - 45) * math.cos(angle))
        ry = cy + int((r - 45) * math.sin(angle))
        draw.ellipse([rx - 14, ry - 14, rx + 14, ry + 14], fill=(20, 20, 20, 255), outline=accent_color, width=4)
        draw.line([rx - 8, ry - 8, rx + 8, ry + 8], fill=accent_color, width=3)

# 1. THE SUN-SEEKERS
def generate_sun_seekers_emblem():
    img, draw = create_base_canvas()
    bg = (67, 20, 7, 255)           # Solar Flare Russet
    border = (245, 158, 11, 255)    # Solar Amber
    accent = (254, 240, 138, 255)   # Brilliant Sunlight
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)

    cx, cy = CENTER
    # Radiant Solar Corona Flare Rays
    num_rays = 16
    for i in range(num_rays):
        ang = (2 * math.pi / num_rays) * i
        length = 660 if i % 2 == 0 else 560
        x2 = cx + int(length * math.cos(ang))
        y2 = cy + int(length * math.sin(ang))
        draw.line([(cx, cy), (x2, y2)], fill=border, width=28)

    draw.ellipse([cx - 440, cy - 440, cx + 440, cy + 440], fill=(234, 88, 12, 255), outline=accent, width=16)
    draw.ellipse([cx - 300, cy - 300, cx + 300, cy + 300], fill=(254, 240, 138, 255), outline=(255, 255, 255, 255), width=10)

    # Welder Goggles Lens Silhouette across Sun
    draw.rectangle([cx - 320, cy - 60, cx + 320, cy + 60], fill=(24, 24, 27, 255))
    draw.ellipse([cx - 240, cy - 90, cx - 60, cy + 90], fill=(13, 148, 136, 255), outline=(217, 119, 6, 255), width=14)
    draw.ellipse([cx + 60, cy - 90, cx + 240, cy + 90], fill=(13, 148, 136, 255), outline=(217, 119, 6, 255), width=14)

    return img

# 2. THE OSTEOPHAGES
def generate_osteophages_emblem():
    img, draw = create_base_canvas()
    bg = (24, 24, 27, 255)          # Ashen Pitch Black
    border = (220, 38, 38, 255)     # Rad Blood Crimson
    accent = (243, 244, 246, 255)   # Bone Ivory
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)

    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(15, 15, 15, 255), outline=border, width=16)

    # Crossed Femur / Tibia Bones
    draw.line([cx - 360, cy - 360, cx + 360, cy + 360], fill=accent, width=44)
    draw.ellipse([cx - 420, cy - 420, cx - 300, cy - 300], fill=accent)
    draw.ellipse([cx + 300, cy + 300, cx + 420, cy + 420], fill=accent)

    draw.line([cx + 360, cy - 360, cx - 360, cy + 360], fill=accent, width=44)
    draw.ellipse([cx + 300, cy - 420, cx + 420, cy - 300], fill=accent)
    draw.ellipse([cx - 420, cy + 300, cx - 300, cy + 420], fill=accent)

    # Irradiated Skull Contour
    draw.ellipse([cx - 240, cy - 300, cx + 240, cy + 120], fill=accent, outline=(0, 0, 0, 255), width=8)
    draw.rectangle([cx - 140, cy + 80, cx + 140, cy + 240], fill=accent, outline=(0, 0, 0, 255), width=8)

    # Eye Sockets & Nasal Cavity
    draw.ellipse([cx - 160, cy - 140, cx - 40, cy + 20], fill=border)
    draw.ellipse([cx + 40, cy - 140, cx + 160, cy + 20], fill=border)
    draw.polygon([(cx, cy + 20), (cx - 35, cy + 100), (cx + 35, cy + 100)], fill=(0, 0, 0, 255))

    # Teeth notches
    for x in range(cx - 100, cx + 110, 40):
        draw.line([x, cy + 140, x, cy + 220], fill=(0, 0, 0, 255), width=6)

    return img

# 3. THE TALLY
def generate_the_tally_emblem():
    img, draw = create_base_canvas()
    bg = (41, 10, 10, 255)          # Dark Dried Blood
    border = (239, 68, 68, 255)     # Tally Red
    accent = (212, 212, 216, 255)   # Honed Steel
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)

    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(18, 18, 20, 255), outline=border, width=16)

    # Heavy Cleaver / Machete Blade Centerpiece
    blade_pts = [
        (cx - 380, cy - 380), (cx + 320, cy - 380), (cx + 380, cy + 80),
        (cx + 280, cy + 220), (cx - 380, cy + 220)
    ]
    draw.polygon(blade_pts, fill=accent, outline=(255, 255, 255, 255), width=10)
    draw.rectangle([cx - 480, cy + 140, cx - 380, cy + 220], fill=(120, 53, 15, 255)) # Wood handle

    # Blood-Red Tally Marks (Four Verticals + Diagonal Strike)
    # Tally Group 1
    for i, x in enumerate([-240, -180, -120, -60]):
        draw.line([cx + x, cy - 280, cx + x, cy + 40], fill=border, width=18)
    draw.line([cx - 280, cy + 10, cx - 20, cy - 250], fill=border, width=22)

    # Tally Group 2
    for i, x in enumerate([60, 120, 180, 240]):
        draw.line([cx + x, cy - 280, cx + x, cy + 40], fill=border, width=18)
    draw.line([cx + 20, cy + 10, cx + 280, cy - 250], fill=border, width=22)

    return img

# 4. THE UNDERTOW
def generate_undertow_emblem():
    img, draw = create_base_canvas()
    bg = (4, 47, 46, 255)           # Sump Green-Teal
    border = (45, 212, 191, 255)    # Bioluminescent Teal
    accent = (202, 138, 4, 255)     # Sump Slime Ochre
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)

    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(2, 20, 20, 255), outline=border, width=16)

    # Swirling Hydro Vortex / Whirlpool
    for r in range(120, 560, 80):
        draw.arc([cx - r, cy - r, cx + r, cy + r], r % 360, r % 360 + 240, fill=border, width=14)

    # Heavy Drainage Grate Sluice Bars
    for y in range(cy - 360, cy + 400, 100):
        draw.line([cx - 380, y, cx + 380, y], fill=(71, 85, 105, 255), width=24)
    draw.rectangle([cx - 380, cy - 380, cx + 380, cy + 380], outline=(148, 163, 184, 255), width=16)

    # Bioluminescent Predator Eyes in Drainage Darkness
    draw.ellipse([cx - 180, cy - 40, cx - 80, cy + 40], fill=(250, 204, 21, 255))
    draw.ellipse([cx - 140, cy - 30, cx - 120, cy + 30], fill=(0, 0, 0, 255))

    draw.ellipse([cx + 80, cy - 40, cx + 180, cy + 40], fill=(250, 204, 21, 255))
    draw.ellipse([cx + 120, cy - 30, cx + 140, cy + 30], fill=(0, 0, 0, 255))

    return img

# 5. THE TEMPEST
def generate_the_tempest_emblem():
    img, draw = create_base_canvas()
    bg = (46, 16, 101, 255)         # Deep Ionospheric Violet
    border = (168, 85, 247, 255)    # Storm Purple
    accent = (56, 189, 248, 255)    # Electric Lightning Blue
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)

    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(15, 10, 30, 255), outline=border, width=16)

    # Radio Antenna Mast
    draw.line([cx, cy + 440, cx, cy - 440], fill=(203, 213, 225, 255), width=24)
    draw.line([cx - 180, cy + 440, cx + 180, cy + 440], fill=(203, 213, 225, 255), width=24)
    for y in [cy + 220, cy, cy - 220]:
        draw.line([cx - 120, y, cx + 120, y], fill=(203, 213, 225, 255), width=16)

    # Zigzag Electric Lightning Bolt Discharge
    lightning = [
        (cx + 80, cy - 500), (cx - 140, cy - 180), (cx + 20, cy - 120),
        (cx - 200, cy + 220), (cx - 40, cy + 180), (cx - 280, cy + 480)
    ]
    draw.line(lightning, fill=accent, width=32)
    draw.line(lightning, fill=(255, 255, 255, 255), width=14)

    # Secondary Arc
    lightning2 = [
        (cx - 60, cy - 440), (cx + 160, cy - 140), (cx + 20, cy - 80), (cx + 260, cy + 340)
    ]
    draw.line(lightning2, fill=accent, width=22)

    return img

# 6. THE PROVISIONED
def generate_the_provisioned_emblem():
    img, draw = create_base_canvas()
    bg = (41, 37, 36, 255)          # Luxury Vault Titanium
    border = (234, 179, 8, 255)     # Pure Vault Gold
    accent = (245, 158, 11, 255)    # Amber
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)

    cx, cy = CENTER
    # Armored Heavy Vault Bulkhead Door
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(28, 25, 23, 255), outline=border, width=24)

    # 4-Spoke Stainless Vault Locking Handwheel
    draw.ellipse([cx - 360, cy - 360, cx + 360, cy + 360], outline=accent, width=36)
    draw.line([cx - 480, cy, cx + 480, cy], fill=accent, width=44)
    draw.line([cx, cy - 480, cx, cy + 480], fill=accent, width=44)
    draw.ellipse([cx - 120, cy - 120, cx + 120, cy + 120], fill=border, outline=(255, 255, 255, 255), width=8)

    # Central Luxury Cornucopia / Stockpile Key
    draw.polygon([(cx, cy - 60), (cx + 50, cy + 40), (cx - 50, cy + 40)], fill=(255, 255, 255, 255))

    return img

# 7. THE ARCHIVISTS OF THE BEFORE
def generate_archivists_emblem():
    img, draw = create_base_canvas()
    bg = (17, 24, 39, 255)          # Deep Archive Navy
    border = (147, 197, 253, 255)   # Microfilm Blue
    accent = (251, 191, 36, 255)    # Memory Amber
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)

    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(15, 23, 42, 255), outline=border, width=16)

    # Magnetic Cassette Tape Shell
    draw.rectangle([cx - 400, cy - 260, cx + 400, cy + 260], fill=(30, 41, 59, 255), outline=border, width=12)
    # Tape Spool Hubs
    draw.ellipse([cx - 260, cy - 100, cx - 60, cy + 100], fill=(15, 23, 42, 255), outline=accent, width=16)
    draw.ellipse([cx + 60, cy - 100, cx + 260, cy + 100], fill=(15, 23, 42, 255), outline=accent, width=16)
    # Center Tape Bridge Window
    draw.rectangle([cx - 120, cy - 60, cx + 120, cy + 60], fill=(2, 6, 23, 255), outline=border, width=6)

    # Memory Hologram Quill Pen
    draw.line([cx - 200, cy + 380, cx + 320, cy - 380], fill=accent, width=22)
    draw.polygon([(cx - 200, cy + 380), (cx - 240, cy + 420), (cx - 160, cy + 400)], fill=(255, 255, 255, 255))

    return img

# 8. THE LAMPLIGHTERS
def generate_lamplighters_emblem():
    img, draw = create_base_canvas()
    bg = (67, 40, 24, 255)          # Warm Lantern Hearth
    border = (245, 158, 11, 255)    # Lantern Brass Amber
    accent = (254, 240, 138, 255)   # Guiding Yellow Light
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)

    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 15, 10, 255), outline=border, width=16)

    # 360-Degree Radiating Light Beams
    for ang_deg in range(0, 360, 30):
        ang = math.radians(ang_deg)
        x2 = cx + int(600 * math.cos(ang))
        y2 = cy + int(600 * math.sin(ang))
        draw.line([(cx, cy - 40), (x2, y2)], fill=(245, 158, 11, 100), width=18)

    # Heavy Cast-Iron Kerosene Storm Lantern
    # Top Cap & Handle
    draw.ellipse([cx - 140, cy - 460, cx + 140, cy - 320], outline=border, width=24) # Handle loop
    draw.polygon([(cx - 180, cy - 320), (cx + 180, cy - 320), (cx + 120, cy - 240), (cx - 120, cy - 240)], fill=border)
    # Glass Chimney (Glowing Yellow Core)
    draw.rectangle([cx - 160, cy - 240, cx + 160, cy + 180], fill=accent, outline=border, width=16)
    # Flame Inside
    flame = [(cx, cy - 140), (cx + 50, cy + 40), (cx, cy + 100), (cx - 50, cy + 40)]
    draw.polygon(flame, fill=(234, 88, 12, 255), outline=(255, 255, 255, 255), width=6)
    # Base Oil Tank
    draw.rectangle([cx - 220, cy + 180, cx + 220, cy + 320], fill=border, outline=(0, 0, 0, 255), width=8)

    return img

# 9. THE GRAIN EXCHANGE
def generate_grain_exchange_emblem():
    img, draw = create_base_canvas()
    bg = (30, 41, 59, 255)          # Merchant Slate
    border = (234, 179, 8, 255)     # Ripe Wheat Gold
    accent = (16, 185, 129, 255)    # Harvest Green
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)

    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(15, 23, 42, 255), outline=border, width=16)

    # Crossed Golden Wheat Stalks
    for side in [-1, 1]:
        stalk_pts = [(cx + side * 40, cy + 400), (cx + side * 340, cy - 380)]
        draw.line(stalk_pts, fill=border, width=22)
        for y in range(cy - 320, cy + 240, 70):
            # Grain Ear
            gx = cx + side * (100 + int((cy - y) * 0.4))
            draw.ellipse([gx - 35, y - 25, gx + 35, y + 25], fill=border, outline=(255, 255, 255, 200), width=4)

    # Central Barter Merchant Coin Token
    draw.ellipse([cx - 180, cy - 180, cx + 180, cy + 180], fill=(217, 119, 6, 255), outline=(255, 255, 255, 255), width=12)
    # Grain Stencil Icon on Coin
    draw.line([cx, cy - 100, cx, cy + 100], fill=(255, 255, 255, 255), width=14)
    draw.chord([cx - 60, cy - 60, cx, cy], 0, 180, fill=(255, 255, 255, 255))
    draw.chord([cx, cy, cx + 60, cy + 60], 0, 180, fill=(255, 255, 255, 255))

    return img

more_emblems = [
    ("faction_icon_sun_seekers.png", generate_sun_seekers_emblem),
    ("faction_icon_osteophages.png", generate_osteophages_emblem),
    ("faction_icon_the_tally.png", generate_the_tally_emblem),
    ("faction_icon_undertow.png", generate_undertow_emblem),
    ("faction_icon_the_tempest.png", generate_the_tempest_emblem),
    ("faction_icon_the_provisioned.png", generate_the_provisioned_emblem),
    ("faction_icon_archivists.png", generate_archivists_emblem),
    ("faction_icon_lamplighters.png", generate_lamplighters_emblem),
    ("faction_icon_grain_exchange.png", generate_grain_exchange_emblem),
]

print("Generating additional necessary 2K (2048x2048) Faction Emblems...")
for filename, gen_func in more_emblems:
    img = gen_func()
    out_path_icons = os.path.join(OUTPUT_DIR, filename)
    img.save(out_path_icons, 'PNG', optimize=True)
    out_path_emblems = os.path.join(EMBLEMS_DIR, filename)
    img.save(out_path_emblems, 'PNG', optimize=True)
    print(f"-> Generated 2K Emblem: {filename}")

print("\nAll necessary 2K Faction Emblems generated and saved successfully!")
