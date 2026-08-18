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
    # Dark textured industrial backdrop
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
    
    # Screw rivets around perimeter
    num_rivets = 24
    for i in range(num_rivets):
        angle = (2 * math.pi / num_rivets) * i
        rx = cx + int((r - 45) * math.cos(angle))
        ry = cy + int((r - 45) * math.sin(angle))
        draw.ellipse([rx - 14, ry - 14, rx + 14, ry + 14], fill=(20, 20, 20, 255), outline=accent_color, width=4)
        draw.line([rx - 8, ry - 8, rx + 8, ry + 8], fill=accent_color, width=3)

def generate_meridian_compact_emblem():
    img, draw = create_base_canvas()
    bg = (24, 38, 68, 255)         # Cobalt Navy
    border = (217, 119, 6, 255)     # Warm Gold
    accent = (245, 158, 11, 255)    # Amber
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    # 1. Outer Cogwheel of Industry
    num_cogs = 12
    for i in range(num_cogs):
        a1 = (2 * math.pi / num_cogs) * i - 0.12
        a2 = (2 * math.pi / num_cogs) * i + 0.12
        r_out = 680
        r_in = 590
        pts = [
            (cx + int(r_in * math.cos(a1)), cy + int(r_in * math.sin(a1))),
            (cx + int(r_out * math.cos(a1)), cy + int(r_out * math.sin(a1))),
            (cx + int(r_out * math.cos(a2)), cy + int(r_out * math.sin(a2))),
            (cx + int(r_in * math.cos(a2)), cy + int(r_in * math.sin(a2))),
        ]
        draw.polygon(pts, fill=(180, 83, 9, 255), outline=accent)
    
    draw.ellipse([cx - 590, cy - 590, cx + 590, cy + 590], fill=(15, 23, 42, 255), outline=accent, width=16)
    
    # 2. Central Compass Star / Military Laurel
    # Four-pointed major star
    star_pts = [
        (cx, cy - 480), (cx + 90, cy - 90), (cx + 480, cy), (cx + 90, cy + 90),
        (cx, cy + 480), (cx - 90, cy + 90), (cx - 480, cy), (cx - 90, cy - 90)
    ]
    draw.polygon(star_pts, fill=accent, outline=(255, 255, 255, 200))
    
    # Inner diamond
    inner_diamond = [(cx, cy - 200), (cx + 200, cy), (cx, cy + 200), (cx - 200, cy)]
    draw.polygon(inner_diamond, fill=(30, 58, 138, 255), outline=(255, 255, 255, 255))
    
    # Central Atom / Power Trefoil
    draw.ellipse([cx - 60, cy - 60, cx + 60, cy + 60], fill=(255, 255, 255, 255))
    
    return img

def generate_underwrite_emblem():
    img, draw = create_base_canvas()
    bg = (69, 10, 10, 255)          # Deep Blood Crimson
    border = (220, 38, 38, 255)     # Crimson Red
    accent = (229, 231, 235, 255)   # Silver Parchment
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    # 1. Wax Seal Stamp Border
    for i in range(36):
        ang = (2 * math.pi / 36) * i
        bx = cx + int((RADIUS - 120 + 20 * math.sin(i * 3)) * math.cos(ang))
        by = cy + int((RADIUS - 120 + 20 * math.sin(i * 3)) * math.sin(ang))
        draw.ellipse([bx - 40, by - 40, bx + 40, by + 40], fill=(127, 29, 29, 255))
        
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(24, 24, 27, 255), outline=accent, width=14)
    
    # 2. Crossed Silver Keys of Debt & Ledger Book
    # Open Ledger
    book_w, book_h = 420, 320
    draw.rectangle([cx - book_w, cy - book_h + 100, cx, cy + book_h - 100], fill=(243, 244, 246, 255), outline=(0,0,0,255), width=8)
    draw.rectangle([cx, cy - book_h + 100, cx + book_w, cy + book_h - 100], fill=(229, 231, 235, 255), outline=(0,0,0,255), width=8)
    
    # Ledger Lines
    for y in range(cy - book_h + 160, cy + book_h - 140, 45):
        draw.line([cx - book_w + 50, y, cx - 50, y], fill=(185, 28, 28, 255), width=6)
        draw.line([cx + 50, y, cx + book_w - 50, y], fill=(185, 28, 28, 255), width=6)
        
    # Crossed Keys
    # Key 1
    draw.line([cx - 380, cy + 320, cx + 380, cy - 320], fill=accent, width=32)
    draw.ellipse([cx - 440, cy + 260, cx - 320, cy + 380], outline=accent, width=28)
    # Key 2
    draw.line([cx + 380, cy + 320, cx - 380, cy - 320], fill=accent, width=32)
    draw.ellipse([cx + 320, cy + 260, cx + 440, cy + 380], outline=accent, width=28)
    
    # Central Wax Seal Monogram
    draw.ellipse([cx - 160, cy - 60, cx + 160, cy + 260], fill=(185, 28, 28, 255), outline=(254, 202, 202, 255), width=10)
    
    return img

def generate_scale_emblem():
    img, draw = create_base_canvas()
    bg = (17, 24, 39, 255)          # Tactical Slate
    border = (14, 116, 144, 255)    # Cyan Steel
    accent = (217, 119, 6, 255)     # Burnished Bronze
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    # 1. Central Tribunal Column
    draw.rectangle([cx - 40, cy - 480, cx + 40, cy + 480], fill=accent, outline=(255, 255, 255, 200), width=6)
    draw.polygon([(cx - 180, cy + 480), (cx + 180, cy + 480), (cx + 80, cy + 400), (cx - 80, cy + 400)], fill=accent)
    draw.ellipse([cx - 80, cy - 540, cx + 80, cy - 380], fill=accent, outline=(255, 255, 255, 255), width=8)
    
    # 2. Horizontal Balance Beam
    draw.line([cx - 460, cy - 260, cx + 460, cy - 260], fill=accent, width=28)
    draw.ellipse([cx - 30, cy - 290, cx + 30, cy - 230], fill=(255, 255, 255, 255))
    
    # 3. Left Scale Pan (Weighing Wheat / Grain Sheaf)
    draw.line([cx - 460, cy - 260, cx - 560, cy + 40], fill=(203, 213, 225, 255), width=6)
    draw.line([cx - 460, cy - 260, cx - 360, cy + 40], fill=(203, 213, 225, 255), width=6)
    draw.chord([cx - 600, cy + 20, cx - 320, cy + 160], 0, 180, fill=accent, outline=(255, 255, 255, 255), width=8)
    # Wheat stalks
    for off in [-60, -30, 0, 30, 60]:
        draw.line([cx - 460 + off, cy + 40, cx - 460 + off * 1.5, cy - 80], fill=(234, 179, 8, 255), width=10)
    
    # 4. Right Scale Pan (Weighing Cartridge / Bullet)
    draw.line([cx + 460, cy - 260, cx + 360, cy + 40], fill=(203, 213, 225, 255), width=6)
    draw.line([cx + 460, cy - 260, cx + 560, cy + 40], fill=(203, 213, 225, 255), width=6)
    draw.chord([cx + 320, cy + 20, cx + 600, cy + 160], 0, 180, fill=accent, outline=(255, 255, 255, 255), width=8)
    # Lead Bullet
    draw.rectangle([cx + 430, cy - 60, cx + 490, cy + 40], fill=(100, 116, 139, 255), outline=(255, 255, 255, 255), width=6)
    draw.polygon([(cx + 430, cy - 60), (cx + 490, cy - 60), (cx + 460, cy - 130)], fill=(203, 213, 225, 255))
    
    return img

def generate_cutters_emblem():
    img, draw = create_base_canvas()
    bg = (39, 39, 42, 255)          # Industrial Cast Iron
    border = (234, 88, 12, 255)     # Oxy-Torch Flame Orange
    accent = (37, 99, 235, 255)     # Acetylene Blue
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    # Circular saw blade teeth outer ring
    num_teeth = 16
    for i in range(num_teeth):
        a1 = (2 * math.pi / num_teeth) * i
        a2 = (2 * math.pi / num_teeth) * (i + 0.6)
        r1, r2 = 680, 580
        pts = [
            (cx + int(r2 * math.cos(a1)), cy + int(r2 * math.sin(a1))),
            (cx + int(r1 * math.cos(a1)), cy + int(r1 * math.sin(a1))),
            (cx + int(r2 * math.cos(a2)), cy + int(r2 * math.sin(a2))),
        ]
        draw.polygon(pts, fill=(194, 65, 12, 255), outline=(255, 255, 255, 180))
        
    draw.ellipse([cx - 580, cy - 580, cx + 580, cy + 580], fill=(24, 24, 27, 255), outline=border, width=14)
    
    # Crossed Oxy-Acetylene Torches with Jet Flame
    # Torch 1
    draw.line([cx - 360, cy + 360, cx + 300, cy - 300], fill=(212, 212, 216, 255), width=34)
    draw.rectangle([cx - 400, cy + 320, cx - 320, cy + 400], fill=(220, 38, 38, 255)) # Gas knob
    # Torch 2
    draw.line([cx + 360, cy + 360, cx - 300, cy - 300], fill=(212, 212, 216, 255), width=34)
    draw.rectangle([cx + 320, cy + 320, cx + 400, cy + 400], fill=(37, 99, 235, 255)) # Gas knob
    
    # Central Plasma Torch Flame Plume
    flame_pts = [
        (cx, cy - 420), (cx + 80, cy - 140), (cx + 140, cy + 80),
        (cx, cy + 180), (cx - 140, cy + 80), (cx - 80, cy - 140)
    ]
    draw.polygon(flame_pts, fill=border, outline=(254, 240, 138, 255), width=10)
    
    # Inner high-temp blue cone
    inner_flame = [(cx, cy - 340), (cx + 45, cy - 80), (cx, cy + 60), (cx - 45, cy - 80)]
    draw.polygon(inner_flame, fill=accent, outline=(255, 255, 255, 255), width=6)
    
    return img

def generate_fleet_emblem():
    img, draw = create_base_canvas()
    bg = (13, 148, 136, 255)        # Arctic Deep Sea Teal
    border = (226, 232, 240, 255)   # Ice Silver
    accent = (30, 41, 59, 255)      # Navy Hull
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(15, 23, 42, 255), outline=border, width=16)
    
    # 1. Heavy Maritime Anchor
    # Vertical Shank
    draw.rectangle([cx - 45, cy - 440, cx + 45, cy + 320], fill=border, outline=(0,0,0,255), width=6)
    # Ring
    draw.ellipse([cx - 110, cy - 560, cx + 110, cy - 380], outline=border, width=32)
    # Crossbeam
    draw.rectangle([cx - 260, cy - 340, cx + 260, cy - 280], fill=border)
    # Curved Arms & Flukes
    draw.arc([cx - 440, cy - 100, cx + 440, cy + 440], 20, 160, fill=border, width=54)
    # Left & Right Fluke points
    draw.polygon([(cx - 440, cy + 180), (cx - 360, cy + 20), (cx - 460, cy + 20)], fill=border)
    draw.polygon([(cx + 440, cy + 180), (cx + 360, cy + 20), (cx + 460, cy + 20)], fill=border)
    
    # 2. Icebreaker Bow Prow V-Shape Cutting Ice
    prow = [(cx, cy - 120), (cx + 180, cy + 240), (cx - 180, cy + 240)]
    draw.polygon(prow, fill=(2, 132, 199, 255), outline=(255, 255, 255, 255), width=10)
    
    return img

def generate_overlay_emblem():
    img, draw = create_base_canvas()
    bg = (6, 78, 59, 255)           # Phosphor Matrix Green
    border = (52, 211, 153, 255)    # Emerald Glow
    accent = (245, 158, 11, 255)    # Radar Amber
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(4, 20, 16, 255), outline=border, width=16)
    
    # 1. Radar Grid Circles & Azimuth Crosshairs
    for rad in [180, 360, 540]:
        draw.ellipse([cx - rad, cy - rad, cx + rad, cy + rad], outline=(16, 185, 129, 120), width=6)
    draw.line([cx - 560, cy, cx + 560, cy], fill=(16, 185, 129, 150), width=6)
    draw.line([cx, cy - 560, cx, cy + 560], fill=(16, 185, 129, 150), width=6)
    
    # 2. Radar Sweep Sector
    draw.pieslice([cx - 530, cy - 530, cx + 530, cy + 530], 315, 360, fill=(16, 185, 129, 80))
    
    # 3. All-Seeing Aperture Eye & Signal Tower
    # Eye Contour
    eye_pts = [
        (cx - 360, cy), (cx - 180, cy - 160), (cx, cy - 220), (cx + 180, cy - 160), (cx + 360, cy),
        (cx + 180, cy + 160), (cx, cy + 220), (cx - 180, cy + 160)
    ]
    draw.polygon(eye_pts, fill=(6, 95, 70, 200), outline=border, width=14)
    # Pupil / Aperture
    draw.ellipse([cx - 140, cy - 140, cx + 140, cy + 140], fill=accent, outline=(255, 255, 255, 255), width=8)
    draw.ellipse([cx - 60, cy - 60, cx + 60, cy + 60], fill=(0, 0, 0, 255))
    
    return img

def generate_blank_rows_emblem():
    img, draw = create_base_canvas()
    bg = (31, 41, 55, 255)          # Ashen Slate
    border = (156, 163, 175, 255)   # Stone Grey
    accent = (139, 92, 246, 255)    # Mourning Violet
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(17, 24, 39, 255), outline=border, width=16)
    
    # 1. Cold Crescent Moon
    draw.ellipse([cx - 380, cy - 500, cx - 180, cy - 300], fill=(243, 244, 246, 255))
    draw.ellipse([cx - 330, cy - 510, cx - 130, cy - 310], fill=(17, 24, 39, 255))
    
    # 2. Symmetrical Uncarved Cenotaph Monoliths (The Blank Rows)
    # Row 1 (Top)
    for x in [-320, -160, 0, 160, 320]:
        draw.rectangle([cx + x - 35, cy - 160, cx + x + 35, cy + 40], fill=(209, 213, 219, 255), outline=(0,0,0,255), width=6)
        draw.chord([cx + x - 35, cy - 195, cx + x + 35, cy - 125], 180, 360, fill=(209, 213, 219, 255), outline=(0,0,0,255), width=6)
        
    # Row 2 (Bottom - Foreground)
    for x in [-380, -220, -60, 100, 260, 420]:
        draw.rectangle([cx + x - 45, cy + 120, cx + x + 45, cy + 360], fill=(243, 244, 246, 255), outline=(0,0,0,255), width=8)
        draw.chord([cx + x - 45, cy + 75, cx + x + 45, cy + 165], 180, 360, fill=(243, 244, 246, 255), outline=(0,0,0,255), width=8)
        
    return img

def generate_deserter_asylum_emblem():
    img, draw = create_base_canvas()
    bg = (63, 98, 18, 255)          # Olive Drab Green
    border = (250, 204, 21, 255)    # Signal Amber
    accent = (239, 68, 68, 255)     # Rust Red
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 30, 15, 255), outline=border, width=16)
    
    # 1. Broken Rifle Snapped in Half
    # Barrel Part
    draw.line([cx - 380, cy - 180, cx - 40, cy - 40], fill=(148, 163, 184, 255), width=36)
    draw.polygon([(cx - 380, cy - 180), (cx - 440, cy - 240), (cx - 410, cy - 260)], fill=(71, 85, 105, 255))
    # Stock Part
    draw.line([cx + 380, cy + 180, cx + 40, cy + 40], fill=(180, 83, 9, 255), width=48)
    draw.rectangle([cx + 300, cy + 140, cx + 420, cy + 240], fill=(146, 64, 14, 255))
    
    # 2. Sprouting Green Wheat & Olive Branch of Peace
    branch_pts = [(cx, cy + 280), (cx, cy - 360)]
    draw.line(branch_pts, fill=(132, 204, 22, 255), width=18)
    for y in range(cy - 280, cy + 220, 80):
        # Left leaf
        draw.chord([cx - 160, y - 40, cx, y + 40], 0, 180, fill=(163, 230, 53, 255), outline=(0,0,0,255), width=4)
        # Right leaf
        draw.chord([cx, y - 40, cx + 160, y + 40], 0, 180, fill=(163, 230, 53, 255), outline=(0,0,0,255), width=4)
        
    return img

def generate_office_emblem():
    img, draw = create_base_canvas()
    bg = (120, 53, 15, 255)         # Bureaucratic Mahogany Leather
    border = (202, 138, 4, 255)     # Brass
    accent = (244, 244, 245, 255)   # Typewriter White
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(24, 24, 27, 255), outline=border, width=16)
    
    # 1. Punched Hollerith Microfilm Aperture Card
    draw.rectangle([cx - 360, cy - 240, cx + 360, cy + 240], fill=(254, 243, 199, 255), outline=(0,0,0,255), width=10)
    # Punched holes
    for r in range(4):
        for c in range(10):
            if (r + c) % 3 != 0:
                px = cx - 300 + c * 65
                py = cy - 180 + r * 55
                draw.rectangle([px, py, px + 25, py + 35], fill=(24, 24, 27, 255))
                
    # 2. Official "RESTRICTED / ARCHIVE" Red Rubber Stamp
    stamp_rect = [cx - 280, cy - 50, cx + 280, cy + 90]
    draw.rectangle(stamp_rect, outline=(220, 38, 38, 255), width=12)
    # Red lines
    draw.line([cx - 250, cy + 20, cx + 250, cy + 20], fill=(220, 38, 38, 255), width=16)
    
    return img

# Generate all 9 emblems
emblems = [
    ("faction_icon_the_compact.png", generate_meridian_compact_emblem),
    ("faction_icon_the_underwrite.png", generate_underwrite_emblem),
    ("faction_icon_the_scale.png", generate_scale_emblem),
    ("faction_icon_the_cutters.png", generate_cutters_emblem),
    ("faction_icon_the_fleet.png", generate_fleet_emblem),
    ("faction_icon_the_overlay.png", generate_overlay_emblem),
    ("faction_icon_blank_rows.png", generate_blank_rows_emblem),
    ("faction_icon_deserter_asylum.png", generate_deserter_asylum_emblem),
    ("faction_icon_the_office.png", generate_office_emblem),
]

print("Generating 2K (2048x2048) Faction Emblems...")
for filename, gen_func in emblems:
    img = gen_func()
    # Save to assets/ui/Icons/
    out_path_icons = os.path.join(OUTPUT_DIR, filename)
    img.save(out_path_icons, 'PNG', optimize=True)
    # Save to assets/ui/FactionEmblems/
    out_path_emblems = os.path.join(EMBLEMS_DIR, filename)
    img.save(out_path_emblems, 'PNG', optimize=True)
    print(f"-> Generated 2K Emblem: {filename} (Saved to Icons and FactionEmblems)")

print("\nAll 2K Faction Emblems generated successfully!")
