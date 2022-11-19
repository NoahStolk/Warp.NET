using Warp.NET.RenderImpl.Ui.Rendering.Renderers;

namespace Warp.NET.RenderImpl.Ui;

public abstract class RenderImplUiGameBase : GameBase
{
	protected RenderImplUiGameBase(GameParameters gameParameters)
		: base(gameParameters)
	{
	}

	public MonoSpaceFontRenderer MonoSpaceFontRenderer { get; } = new(new(Textures.Spleen6x12, Charsets.Ascii_32_126));
	public SpriteRenderer SpriteRenderer { get; } = new();
	public RectangleRenderer RectangleRenderer { get; } = new();
	public CircleRenderer CircleRenderer { get; } = new();
}
