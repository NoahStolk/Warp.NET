using Warp.NET.RenderImpl.Ui.Rendering.Renderers;
using Warp.NET.RenderImpl.Ui.Rendering.Text;

namespace Warp.NET.RenderImpl.Ui;

public abstract class RenderImplUiGameBase : GameBase
{
	protected RenderImplUiGameBase(GameParameters gameParameters)
		: base(gameParameters)
	{
	}

	public virtual MonoSpaceFontRenderer MonoSpaceFontRenderer8 { get; } = new(new(Textures.Spleen5x8, Charsets.Ascii_32_126));
	public virtual MonoSpaceFontRenderer MonoSpaceFontRenderer12 { get; } = new(new(Textures.Spleen6x12, Charsets.Ascii_32_126));
	public virtual MonoSpaceFontRenderer MonoSpaceFontRenderer16 { get; } = new(new(Textures.Spleen8x16, Charsets.Ascii_32_126));
	public virtual MonoSpaceFontRenderer MonoSpaceFontRenderer24 { get; } = new(new(Textures.Spleen12x24, Charsets.Ascii_32_126));
	public virtual MonoSpaceFontRenderer MonoSpaceFontRenderer32 { get; } = new(new(Textures.Spleen16x32, Charsets.Ascii_32_126));
	public virtual MonoSpaceFontRenderer MonoSpaceFontRenderer64 { get; } = new(new(Textures.Spleen32x64, Charsets.Ascii_32_126));

	public SpriteRenderer SpriteRenderer { get; } = new();
	public RectangleRenderer RectangleRenderer { get; } = new();
	public CircleRenderer CircleRenderer { get; } = new();

	public MonoSpaceFontRenderer GetFontRenderer(FontSize fontSize) => fontSize switch
	{
		FontSize.H8 => MonoSpaceFontRenderer8,
		FontSize.H12 => MonoSpaceFontRenderer12,
		FontSize.H16 => MonoSpaceFontRenderer16,
		FontSize.H24 => MonoSpaceFontRenderer24,
		FontSize.H32 => MonoSpaceFontRenderer32,
		FontSize.H64 => MonoSpaceFontRenderer64,
		_ => throw new NotSupportedException($"Font size '{fontSize}' is not supported."),
	};
}
