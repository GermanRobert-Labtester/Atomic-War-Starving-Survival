// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

#pragma warning disable CS8618
namespace AtomicWar.GodotApp.World
{
    /// <summary>
    /// A single survivor's visual AND physics body on the shelter interior.
    ///
    /// Physics base (not placeholder): a grounded <see cref="CharacterBody2D"/>
    /// with gravity, floor snapping, a capsule collider, accelerated horizontal
    /// seek toward an assigned room anchor, and <c>MoveAndSlide</c> collision
    /// against the interior's static bounds. Movement is presentation only —
    /// Core still owns room assignment and needs.
    ///
    /// Visual base (placeholder): a non-real blockout mannequin sheet
    /// (4-frame walk x front/side/back), tinted per survivor. Falls back to the
    /// canonical portrait/sprite when the sheet art is absent.
    /// </summary>
    public partial class SurvivorActorView : CharacterBody2D
    {
        // ── Art ─────────────────────────────────────────────────────────────
        public const string FallbackTexturePath = AssetRegistry.FallbackSurvivorPath;
        private const string CharacterDir = "res://assets/sprites/Characters/";
        private const int Hframes = 4;
        private const int Vframes = 3; // rows: front, side, back
        private static readonly string[] CharacterVariants =
        {
            "char_base_sheet", "char_base_sheet_rust", "char_base_sheet_olive", "char_base_sheet_bone"
        };

        // ── Physics base ────────────────────────────────────────────────────
        public const float Gravity = 980f;
        public const float MaxSeekSpeed = 74f;
        public const float SeekAccel = 460f;
        public const float ArriveRadius = 3f;
        private const float FrameTime = 0.14f;

        private string _survivorId = string.Empty;

        public string SurvivorId
        {
            get => _survivorId;
            set
            {
                if (_survivorId != value)
                {
                    _survivorId = value;
                    UpdatePortrait();
                    UpdateBody();
                }
            }
        }

        public Label Label { get; private set; }
        /// <summary>Portrait fallback (shown only when the character sheet is absent).</summary>
        public Sprite2D Sprite { get; private set; }
        /// <summary>Blockout character sheet body (preferred visual).</summary>
        public Sprite2D Body { get; private set; }
        public ColorRect HealthIndicator { get; private set; }
        public ColorRect RadiationIndicator { get; private set; }
        public ColorRect StatusIndicator { get; private set; }
        public SurvivorNeedsState SurvivorState { get; private set; }

        private Vector2 _moveTarget;
        private bool _hasTarget;
        private float _animTime;
        private int _animFrame;
        private bool _moving;

        public SurvivorActorView()
        {
            // Collision capsule; actor origin sits at the feet.
            AddChild(new CollisionShape2D
            {
                Name = "BodyShape",
                Position = new Vector2(0, -18),
                Shape = new CapsuleShape2D { Radius = 8f, Height = 30f }
            });

            // Blockout character sheet. Offset lifts the frame so its bottom is the
            // feet at local origin.
            const int frameHeight = 96;
            Body = new Sprite2D
            {
                Name = "Body",
                Hframes = Hframes,
                Vframes = Vframes,
                Frame = 0,
                Offset = new Vector2(0, -frameHeight / 2f),
                Scale = new Vector2(0.46f, 0.46f)
            };
            AddChild(Body);

            // Portrait fallback.
            Sprite = new Sprite2D { Name = "Portrait", Offset = new Vector2(0, -24) };
            AddChild(Sprite);

            Label = new Label
            {
                Name = "Name",
                HorizontalAlignment = HorizontalAlignment.Center,
                Position = new Vector2(-60, -86),
                Size = new Vector2(120, 16)
            };
            Label.AddThemeFontSizeOverride("font_size", 10);
            AddChild(Label);

            HealthIndicator = CreateStatusIndicator(new Color(0.2f, 0.8f, 0.2f, 0.8f), new Vector2(-22, -8));
            RadiationIndicator = CreateStatusIndicator(new Color(0.8f, 0.8f, 0.2f, 0.8f), new Vector2(14, -8));
            StatusIndicator = CreateStatusIndicator(new Color(0.2f, 0.6f, 0.8f, 0.8f), new Vector2(-4, -100));

            MotionMode = MotionModeEnum.Grounded;
            UpDirection = Vector2.Up;
            FloorSnapLength = 8f;

            UpdatePortrait();
        }

        private ColorRect CreateStatusIndicator(Color color, Vector2 position)
        {
            var indicator = new ColorRect
            {
                Size = new Vector2(20, 5),
                Position = position,
                Color = color
            };
            AddChild(indicator);
            return indicator;
        }

        // ── Physics base ────────────────────────────────────────────────────

        /// <summary>Assigns the presentation anchor for this actor. First target snaps.</summary>
        public void SetMoveTarget(Vector2 target)
        {
            _moveTarget = target;
            if (!_hasTarget)
            {
                _hasTarget = true;
                GlobalPosition = target;
                Velocity = Vector2.Zero;
            }
        }

        public Vector2 MoveTarget => _moveTarget;
        public bool IsMoving => _moving;

        public override void _PhysicsProcess(double delta)
        {
            float dt = (float)delta;

            if (!IsOnFloor())
                Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * dt);

            float desiredVx = 0f;
            if (_hasTarget)
            {
                float dx = _moveTarget.X - GlobalPosition.X;
                if (Mathf.Abs(dx) > ArriveRadius)
                    desiredVx = Mathf.Sign(dx) * Mathf.Min(MaxSeekSpeed, Mathf.Abs(dx) * 4f);
            }

            Velocity = new Vector2(Mathf.MoveToward(Velocity.X, desiredVx, SeekAccel * dt), Velocity.Y);
            MoveAndSlide();

            _moving = Mathf.Abs(Velocity.X) > 4f && IsOnFloor();
            UpdateAnimation(dt);
        }

        private void UpdateAnimation(float dt)
        {
            if (Body == null || Body.Texture == null)
                return;

            int row = _moving ? 1 : 0; // side while moving, front while idle
            if (_moving)
            {
                _animTime += dt;
                _animFrame = (int)(_animTime / FrameTime) % Hframes;
            }
            else
            {
                _animTime = 0f;
                _animFrame = 0;
            }

            Body.Frame = row * Hframes + _animFrame;
            Body.FlipH = _moving && Velocity.X < 0f;
        }

        // ── Art resolution ──────────────────────────────────────────────────

        /// <summary>Loads the per-survivor blockout sheet, hiding the portrait when present.</summary>
        private void UpdateBody()
        {
            if (Body == null) return;

            Texture2D? texture = null;
            if (!string.IsNullOrEmpty(_survivorId))
            {
                int index = Math.Abs(_survivorId.GetHashCode()) % CharacterVariants.Length;
                texture = BackdropArt.TryLoad(CharacterDir + CharacterVariants[index] + ".png");
            }

            Body.Texture = texture;
            if (Sprite != null)
                Sprite.Visible = texture == null;
        }

        /// <summary>
        /// Resolves the survivor portrait using <see cref="AssetRegistry.GetPortrait"/>
        /// before falling back to the canonical generic character sprite.
        /// </summary>
        public void UpdatePortrait()
        {
            if (Sprite == null) return;

            Texture2D? texture = null;
            if (!string.IsNullOrEmpty(_survivorId))
            {
                var result = AssetRegistry.GetPortrait(_survivorId);
                if (result.Texture != null)
                {
                    texture = result.Texture;
                    Sprite.Scale = Vector2.One;
                }
            }

            texture ??= GD.Load<Texture2D>(FallbackTexturePath);

            Sprite.Texture = texture;
            if (texture != null)
            {
                Vector2 texSize = texture.GetSize();
                if (texSize.X > 0 && texSize.Y > 0)
                {
                    float targetDim = 44f;
                    float scaleFactor = targetDim / Math.Max(texSize.X, texSize.Y);
                    Sprite.Scale = new Vector2(scaleFactor, scaleFactor);
                }
            }
        }

        // ── Live state projection ───────────────────────────────────────────

        /// <summary>
        /// Refresh this actor from the authoritative survivor state. Needs drive
        /// the status pill; radiation drives the bottom-right indicator.
        /// </summary>
        public void UpdateFromSurvivor(SurvivorNeedsState state, SurvivorRadState? rad = null)
        {
            if (state == null)
            {
                Visible = false;
                return;
            }

            SurvivorState = state;
            SurvivorId = state.Id;
            Visible = true;
            Label.Text = FormatSurvivorName(state.Id);

            float healthRatio = Mathf.Clamp(state.Health / 100f, 0f, 1f);
            HealthIndicator.Color = new Color(
                Mathf.Lerp(0.8f, 0.2f, healthRatio),
                Mathf.Lerp(0.2f, 0.8f, healthRatio),
                0.2f,
                0.8f
            );

            float radiationRatio = rad != null ? Mathf.Clamp(rad.RadiationDose / 100f, 0f, 1f) : 0f;
            RadiationIndicator.Color = new Color(
                Mathf.Lerp(0.2f, 0.8f, radiationRatio),
                Mathf.Lerp(0.8f, 0.2f, radiationRatio),
                Mathf.Lerp(0.8f, 0.2f, radiationRatio),
                0.8f
            );

            UpdateStatusIndicator();
        }

        private void UpdateStatusIndicator()
        {
            if (SurvivorState == null)
                return;

            if (SurvivorState.Hunger > 70 || SurvivorState.Thirst > 70)
                StatusIndicator.Color = new Color(0.8f, 0.6f, 0.2f, 0.8f);
            else if (SurvivorState.Fatigue > 70)
                StatusIndicator.Color = new Color(0.2f, 0.4f, 0.8f, 0.8f);
            else if (SurvivorState.Warmth < 30)
                StatusIndicator.Color = new Color(0.4f, 0.2f, 0.8f, 0.8f);
            else
                StatusIndicator.Color = new Color(0.2f, 0.8f, 0.6f, 0.8f);
        }

        private static string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "Unknown";
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(id.Replace('_', ' '));
        }
    }
}
