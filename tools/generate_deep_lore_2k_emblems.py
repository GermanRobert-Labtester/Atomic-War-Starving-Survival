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

# 1. CULT OF THE GLOW
def generate_cult_of_the_glow_emblem():
    img, draw = create_base_canvas()
    bg = (6, 44, 28, 255)           # Rad Sump Emerald
    border = (74, 222, 128, 255)    # Glowing Neon Green
    accent = (187, 247, 208, 255)   # Intense Cherenkov White-Green
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(2, 20, 12, 255), outline=border, width=16)
    
    # Glowing Uranium-235 Crystal Shard Cluster
    crystal_pts = [
        (cx, cy - 460), (cx + 120, cy - 140), (cx + 80, cy + 340),
        (cx, cy + 420), (cx - 80, cy + 340), (cx - 120, cy - 140)
    ]
    draw.polygon(crystal_pts, fill=border, outline=accent, width=12)
    # Facet lines
    draw.line([(cx, cy - 460), (cx, cy + 420)], fill=accent, width=8)
    draw.line([(cx - 120, cy - 140), (cx, cy + 60)], fill=accent, width=6)
    draw.line([(cx + 120, cy - 140), (cx, cy + 60)], fill=accent, width=6)
    
    # Left & Right Flanking Crystals
    left_c = [(cx - 240, cy - 80), (cx - 120, cy + 180), (cx - 200, cy + 360), (cx - 300, cy + 160)]
    draw.polygon(left_c, fill=(34, 197, 94, 255), outline=accent, width=6)
    right_c = [(cx + 240, cy - 80), (cx + 300, cy + 160), (cx + 200, cy + 360), (cx + 120, cy + 180)]
    draw.polygon(right_c, fill=(34, 197, 94, 255), outline=accent, width=6)
    
    # Atomic Orbital Halos
    for rx, ry, ang in [(540, 200, 30), (540, 200, -30)]:
        # Draw tilted ellipse
        for a_deg in range(0, 360, 5):
            rad = math.radians(a_deg)
            x_raw = rx * math.cos(rad)
            y_raw = ry * math.sin(rad)
            rot = math.radians(ang)
            xr = cx + int(x_raw * math.cos(rot) - y_raw * math.sin(rot))
            yr = cy + int(x_raw * math.sin(rot) + y_raw * math.cos(rot))
            draw.ellipse([xr - 4, yr - 4, xr + 4, yr + 4], fill=border)
            
    return img

# 2. THE ROT FARMERS
def generate_rot_farmers_emblem():
    img, draw = create_base_canvas()
    bg = (41, 24, 16, 255)          # Rich Soil Compost
    border = (168, 85, 247, 255)    # Fungal Spore Violet
    accent = (250, 204, 21, 255)    # Bioluminescent Gold
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 12, 8, 255), outline=border, width=16)
    
    # Crossed Compost Rakes
    draw.line([cx - 360, cy + 360, cx + 320, cy - 320], fill=(161, 98, 7, 255), width=24)
    draw.line([cx + 360, cy + 360, cx - 320, cy - 320], fill=(161, 98, 7, 255), width=24)
    
    # 3 Giant Bioluminescent Mushrooms
    # Center Cap
    draw.chord([cx - 240, cy - 380, cx + 240, cy - 60], 180, 360, fill=(192, 132, 252, 255), outline=accent, width=12)
    draw.rectangle([cx - 50, cy - 140, cx + 50, cy + 280], fill=(243, 244, 246, 255), outline=(0,0,0,255), width=6)
    # Center Cap Glowing Spots
    for sx, sy in [(-120, -220), (0, -280), (120, -220), (-60, -160), (60, -160)]:
        draw.ellipse([cx + sx - 24, cy + sy - 24, cx + sx + 24, cy + sy + 24], fill=accent)
        
    # Left Small Cap
    draw.chord([cx - 420, cy - 180, cx - 120, cy + 60], 190, 350, fill=border, outline=accent, width=8)
    draw.rectangle([cx - 290, cy - 40, cx - 230, cy + 260], fill=(209, 213, 219, 255))
    
    # Right Small Cap
    draw.chord([cx + 120, cy - 180, cx + 420, cy + 60], 190, 350, fill=border, outline=accent, width=8)
    draw.rectangle([cx + 230, cy - 40, cx + 290, cy + 260], fill=(209, 213, 219, 255))
    
    return img

# 3. THE ECHO BATS
def generate_echo_bats_emblem():
    img, draw = create_base_canvas()
    bg = (15, 23, 42, 255)          # Cavern Obsidian
    border = (14, 165, 233, 255)    # Sonar Cyan
    accent = (248, 250, 252, 255)   # Acoustic White
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(8, 12, 24, 255), outline=border, width=16)
    
    # Concentric Acoustic Sonar Echolocation Wave Arcs
    for r in [220, 360, 500]:
        draw.arc([cx - r, cy - 200 - r, cx + r, cy - 200 + r], 40, 140, fill=border, width=14)
        
    # Stylized Geometric Bat Wings Silhouette
    bat_pts = [
        (cx, cy - 80), (cx + 140, cy - 180), (cx + 420, cy - 240), (cx + 340, cy + 80),
        (cx + 240, cy + 220), (cx + 140, cy + 120), (cx + 60, cy + 280), (cx, cy + 180),
        (cx - 60, cy + 280), (cx - 140, cy + 120), (cx - 240, cy + 220), (cx - 340, cy + 80),
        (cx - 420, cy - 240), (cx - 140, cy - 180)
    ]
    draw.polygon(bat_pts, fill=(30, 41, 59, 255), outline=accent, width=10)
    
    # Glowing Sonar Eyes
    draw.ellipse([cx - 40, cy - 40, cx - 15, cy - 15], fill=border)
    draw.ellipse([cx + 15, cy - 40, cx + 40, cy - 15], fill=border)
    
    return img

# 4. THE WIRE HEADS
def generate_wire_heads_emblem():
    img, draw = create_base_canvas()
    bg = (24, 24, 27, 255)          # Chassis Black
    border = (245, 158, 11, 255)    # Filament Amber
    accent = (56, 189, 248, 255)    # Copper Blue
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(12, 12, 14, 255), outline=border, width=16)
    
    # 1. Vacuum Tube Glass Envelope
    tube_rect = [cx - 220, cy - 420, cx + 220, cy + 260]
    draw.rectangle(tube_rect, fill=(30, 41, 59, 160), outline=(203, 213, 225, 255), width=10)
    draw.chord([cx - 220, cy - 540, cx + 220, cy - 300], 180, 360, fill=(30, 41, 59, 160), outline=(203, 213, 225, 255), width=10)
    
    # 2. Glowing Red Tungsten Filaments Inside
    draw.line([cx - 80, cy + 180, cx - 80, cy - 260], fill=(239, 68, 68, 255), width=14)
    draw.line([cx + 80, cy + 180, cx + 80, cy - 260], fill=(239, 68, 68, 255), width=14)
    # Filament Coils
    for y in range(cy - 240, cy + 120, 35):
        draw.line([cx - 80, y, cx + 80, y + 15], fill=border, width=8)
        
    # 3. Radiating PCB Circuit Traces & Solder Pads
    for angle_deg in [0, 45, 90, 135, 180, 225, 270, 315]:
        rad = math.radians(angle_deg)
        x1 = cx + int(360 * math.cos(rad))
        y1 = cy + int(360 * math.sin(rad))
        x2 = cx + int(540 * math.cos(rad))
        y2 = cy + int(540 * math.sin(rad))
        draw.line([(x1, y1), (x2, y2)], fill=accent, width=12)
        draw.ellipse([x2 - 18, y2 - 18, x2 + 18, y2 + 18], fill=border, outline=(255, 255, 255, 255), width=4)
        
    return img

# 5. THE SUMP DREDGERS
def generate_sump_dredgers_emblem():
    img, draw = create_base_canvas()
    bg = (30, 41, 59, 255)          # Sludge Slate
    border = (202, 138, 4, 255)     # Brass Heavy
    accent = (45, 212, 191, 255)    # Slime Cyan
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(15, 23, 42, 255), outline=border, width=16)
    
    # Crossed Heavy Dredge Shovels / Scoops
    draw.line([cx - 380, cy + 380, cx + 340, cy - 340], fill=(148, 163, 184, 255), width=28)
    draw.line([cx + 380, cy + 380, cx - 340, cy - 340], fill=(148, 163, 184, 255), width=28)
    
    # Vintage Brass Deep-Sea / Sump Diving Helmet
    # Helmet Dome
    draw.chord([cx - 260, cy - 360, cx + 260, cy + 120], 180, 360, fill=border, outline=(0,0,0,255), width=10)
    # Circular Viewport Bezel
    draw.ellipse([cx - 160, cy - 240, cx + 160, cy + 80], fill=(15, 23, 42, 255), outline=(255, 255, 255, 255), width=18)
    draw.ellipse([cx - 130, cy - 210, cx + 130, cy + 50], fill=accent) # Glass with green reflection
    # Viewport Cross-Grille
    draw.line([cx - 130, cy - 80, cx + 130, cy - 80], fill=border, width=10)
    draw.line([cx, cy - 210, cx, cy + 50], fill=border, width=10)
    # Breastplate
    draw.rectangle([cx - 280, cy + 100, cx + 280, cy + 340], fill=border, outline=(0,0,0,255), width=8)
    
    return img

# 6. THE UPLAND MILITIA
def generate_upland_militia_emblem():
    img, draw = create_base_canvas()
    bg = (20, 50, 35, 255)          # Alpine Pine Green
    border = (226, 232, 240, 255)   # Mountain Snow Silver
    accent = (217, 119, 6, 255)     # Sentry Amber
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(10, 30, 20, 255), outline=border, width=16)
    
    # Mountain Peak Silhouette
    mountain = [(cx, cy - 420), (cx + 380, cy + 280), (cx - 380, cy + 280)]
    draw.polygon(mountain, fill=(71, 85, 105, 255), outline=border, width=8)
    snow_cap = [(cx, cy - 420), (cx + 140, cy - 160), (cx - 140, cy - 160)]
    draw.polygon(snow_cap, fill=(248, 250, 252, 255))
    
    # Crossed Bolt-Action Rifles
    draw.line([cx - 360, cy + 340, cx + 340, cy - 300], fill=accent, width=22)
    draw.line([cx + 360, cy + 340, cx - 340, cy - 300], fill=accent, width=22)
    
    # Sentry Star in Sky
    draw.polygon([(cx, cy - 540), (cx + 30, cy - 480), (cx + 90, cy - 480), (cx + 40, cy - 440), (cx + 60, cy - 380), (cx, cy - 420), (cx - 60, cy - 380), (cx - 40, cy - 440), (cx - 90, cy - 480), (cx - 30, cy - 480)], fill=(250, 204, 21, 255))
    
    return img

# 7. THE WARLORD'S HOST
def generate_warlord_emblem():
    img, draw = create_base_canvas()
    bg = (40, 10, 10, 255)          # Warlord Blood Iron
    border = (220, 38, 38, 255)     # Crimson
    accent = (212, 212, 216, 255)   # Spiked Steel
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    # Spiked Iron Crest Ring
    num_spikes = 12
    for i in range(num_spikes):
        ang = (2 * math.pi / num_spikes) * i
        sx = cx + int(680 * math.cos(ang))
        sy = cy + int(680 * math.sin(ang))
        draw.polygon([(sx, sy), (cx + int(560 * math.cos(ang - 0.15)), cy + int(560 * math.sin(ang - 0.15))),
                      (cx + int(560 * math.cos(ang + 0.15)), cy + int(560 * math.sin(ang + 0.15)))], fill=accent, outline=border)
                      
    draw.ellipse([cx - 560, cy - 560, cx + 560, cy + 560], fill=(20, 5, 5, 255), outline=border, width=16)
    
    # Heavy Spiked Iron Crown
    crown_pts = [
        (cx - 320, cy + 180), (cx - 320, cy - 140), (cx - 160, cy + 40),
        (cx, cy - 260), (cx + 160, cy + 40), (cx + 320, cy - 140),
        (cx + 320, cy + 180)
    ]
    draw.polygon(crown_pts, fill=accent, outline=(255, 255, 255, 255), width=10)
    
    # Crossed Barbed Cleavers
    draw.line([cx - 320, cy + 340, cx + 320, cy - 300], fill=border, width=28)
    draw.line([cx + 320, cy + 340, cx - 320, cy - 300], fill=border, width=28)
    
    return img

# 8. SAFE HAVEN COMMUNITY
def generate_safe_haven_emblem():
    img, draw = create_base_canvas()
    bg = (40, 55, 20, 255)          # Agrarian Olive
    border = (234, 179, 8, 255)     # Harvest Gold
    accent = (134, 239, 172, 255)   # Tender Shoot Green
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 30, 12, 255), outline=border, width=16)
    
    # Wooden Stockade Archway of Sanctuary
    draw.arc([cx - 360, cy - 380, cx + 360, cy + 340], 180, 360, fill=(180, 83, 9, 255), width=38)
    draw.line([cx - 360, cy - 20, cx - 360, cy + 380], fill=(180, 83, 9, 255), width=38)
    draw.line([cx + 360, cy - 20, cx + 360, cy + 380], fill=(180, 83, 9, 255), width=38)
    
    # Cupped Protecting Hands
    draw.arc([cx - 280, cy + 60, cx, cy + 340], 0, 180, fill=border, width=24)
    draw.arc([cx, cy + 60, cx + 280, cy + 340], 0, 180, fill=border, width=24)
    
    # Sprouting Green Living Seedling
    draw.line([(cx, cy + 200), (cx, cy - 160)], fill=(34, 197, 94, 255), width=18)
    draw.chord([cx - 160, cy - 100, cx, cy], 0, 180, fill=accent, outline=(0,0,0,255), width=6)
    draw.chord([cx, cy - 100, cx + 160, cy], 0, 180, fill=accent, outline=(0,0,0,255), width=6)
    
    return img

# 9. 5TH ARMORED REMNANT
def generate_military_remnants_emblem():
    img, draw = create_base_canvas()
    bg = (40, 45, 30, 255)          # Military Olive Drab
    border = (203, 213, 225, 255)   # Armor Steel
    accent = (239, 68, 68, 255)     # Division Red
    draw_textured_shield(draw, CENTER, RADIUS, bg, border, accent)
    
    cx, cy = CENTER
    # Tank Track Continuous Tread Links
    draw.ellipse([cx - 640, cy - 640, cx + 640, cy + 640], fill=(20, 24, 16, 255), outline=border, width=28)
    for i in range(24):
        ang = (2 * math.pi / 24) * i
        tx1 = cx + int(610 * math.cos(ang))
        ty1 = cy + int(610 * math.sin(ang))
        tx2 = cx + int(670 * math.cos(ang))
        ty2 = cy + int(670 * math.sin(ang))
        draw.line([(tx1, ty1), (tx2, ty2)], fill=(15, 23, 42, 255), width=8)
        
    # Armor-Piercing Sabot Chevron & Crossed Bayonets
    draw.polygon([(cx, cy - 400), (cx + 260, cy + 40), (cx + 160, cy + 40), (cx, cy - 180), (cx - 160, cy + 40), (cx - 260, cy + 40)], fill=accent, outline=(255, 255, 255, 255), width=8)
    draw.polygon([(cx, cy - 120), (cx + 260, cy + 320), (cx + 160, cy + 320), (cx, cy + 100), (cx - 160, cy + 320), (cx - 260, cy + 320)], fill=accent, outline=(255, 255, 255, 255), width=8)
    
    # Central Stenciled Division Star
    draw.polygon([(cx, cy - 80), (cx + 25, cy - 30), (cx + 80, cy - 30), (cx + 35, cy + 5), (cx + 55, cy + 60), (cx, cy + 25), (cx - 55, cy + 60), (cx - 35, cy + 5), (cx - 80, cy - 30), (cx - 25, cy - 30)], fill=(255, 255, 255, 255))
    
    return img

deep_lore_emblems = [
    ("faction_icon_cult_of_the_glow.png", generate_cult_of_the_glow_emblem),
    ("faction_icon_rot_farmers.png", generate_rot_farmers_emblem),
    ("faction_icon_echo_bats.png", generate_echo_bats_emblem),
    ("faction_icon_wire_heads.png", generate_wire_heads_emblem),
    ("faction_icon_sump_dredgers.png", generate_sump_dredgers_emblem),
    ("faction_icon_upland_militia.png", generate_upland_militia_emblem),
    ("faction_icon_warlord.png", generate_warlord_emblem),
    ("faction_icon_safe_haven_community.png", generate_safe_haven_community_emblem if 'generate_safe_haven_community_emblem' in locals() else generate_safe_haven_emblem),
    ("faction_icon_military_remnants.png", generate_military_remnants_emblem),
]

print("Generating Deep Lore 2K Faction Emblems...")
for filename, gen_func in deep_lore_emblems:
    img = gen_func()
    out_path_icons = os.path.join(OUTPUT_DIR, filename)
    img.save(out_path_icons, 'PNG', optimize=True)
    out_path_emblems = os.path.join(EMBLEMS_DIR, filename)
    img.save(out_path_emblems, 'PNG', optimize=True)
    print(f"-> Generated 2K Emblem: {filename}")

print("\nAll Deep Lore 2K Faction Emblems generated and saved successfully!")
