using Godot;
using GooglyEyes.config.persistence.models;

namespace GooglyEyes.GooglyEyes;

public partial class GooglyEye : Node2D
{
    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(nameof(GetType), MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public GooglyEye(Vector2 offset, int radius)
    {
        _iris = CreateIrisSprite(offset, radius);
        _sclera = CreateScleraSprite(offset, radius);

        _sclera.AddChild(_iris);
        AddChild(_sclera);
    }

    private static readonly Random Rng = new();
    
    private readonly Sprite2D _sclera;
    private readonly Sprite2D _iris;

    // State
    private Vector2 _pupilPos = Vector2.Zero;
    private Vector2 _pupilVel = Vector2.Zero;
    private Vector2 _prevCenter = Vector2.Zero;

    // Tunables (ported meaningfully)
    private const float PupilRadiusFactor = 0.52f;
    private const float MaxOffsetFactor = 0.95f - PupilRadiusFactor;

    private const float FrictionPupil = 4f;
    private const float RollingFriction = 0.5f;
    private const float Inertia = 0.5f;

    public override void _Process(double deltaD)
    {
        float delta = (float)deltaD;
        if (delta <= 0f) return;

        Vector2 center = _sclera.Position;

        // --- Eye movement delta (equivalent to dx, dy in original)
        Vector2 eyeDelta = center - _prevCenter;

        // --- Inertia: eye movement pushes pupil opposite
        _pupilPos -= eyeDelta * Inertia;

        // --- Random wobble (small, not dominant)
        Vector2 wobble = new Vector2(
            (float)(Rng.NextDouble() * 2.0 - 1.0),
            (float)(Rng.NextDouble() * 2.0 - 1.0)
        ) * 50f;

        _pupilVel += wobble * delta;

        // --- Friction (velocity damping)
        float speed = _pupilVel.Length();
        if (speed > 0f)
        {
            float friction = FrictionPupil * delta;
            float scale = Mathf.Max(0f, 1f - friction / Mathf.Max(speed, 0.0001f));
            _pupilVel *= scale;
        }

        // --- Integrate velocity
        _pupilPos += _pupilVel * delta;

        // --- Constrain to circle
        float radius = _sclera.Texture.GetSize().X * 0.5f;
        float maxOffset = radius * MaxOffsetFactor;

        float dist = _pupilPos.Length();
        if (dist > maxOffset && dist > 0.0001f)
        {
            Vector2 normal = _pupilPos / dist;

            // remove outward velocity
            float radialVel = _pupilVel.Dot(normal);
            if (radialVel > 0f)
                _pupilVel -= normal * radialVel;

            // rolling friction on edge
            _pupilVel *= Mathf.Exp(-RollingFriction * delta);

            // clamp position
            _pupilPos = normal * maxOffset;
        }

        // --- Apply to sprite (relative to sclera)
        _iris.Position = _pupilPos;

        _prevCenter = center;
    }

    private Sprite2D CreateIrisSprite(Vector2 offset, int radius)
    {
        var img = Image.CreateEmpty(radius, radius, false, Image.Format.Rgba8);
        float cx = radius / 2f, cy = radius / 2f, r = radius / 2f;

        float pr = r * 0.42f;
        float po = r * 0.22f;

        for (int x = 0; x < radius; x++)
        for (int y = 0; y < radius; y++)
        {
            float dx = x - (cx + po), dy = y - (cy + po);
            if (dx * dx + dy * dy < pr * pr)
            {
                img.SetPixel(x, y, Colors.Black);
            }
        }

        var tex = ImageTexture.CreateFromImage(img);
        return new Sprite2D
        {
            Position = offset,
            Texture = tex
        };
    }

    private Sprite2D CreateScleraSprite(Vector2 offset, int radius)
    {
        var img = Image.CreateEmpty(radius, radius, false, Image.Format.Rgba8);

        float cx = radius / 2f, cy = radius / 2f, r = radius / 2f;

        int pixelCount = 0;

        for (int x = 0; x < radius; x++)
        for (int y = 0; y < radius; y++)
        {
            float dx = x - cx, dy = y - cy;
            float dist = Mathf.Sqrt(dx * dx + dy * dy);

            if (dist > r) continue;

            if (dist > r - 1.5f)
            {
                img.SetPixel(x, y, Colors.Black);
                pixelCount++;
                continue;
            }

            img.SetPixel(x, y, Colors.White);
            pixelCount++;
        }

        var tex = ImageTexture.CreateFromImage(img);
        return new Sprite2D
        {
            Position = offset,
            Texture = tex
        };
    }
}