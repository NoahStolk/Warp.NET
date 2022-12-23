using System.Numerics;
using Warp.NET.Extensions;
using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace Warp.NET.RenderImpl.Ui.Components;

public class IconButton : Button
{
	private readonly Texture _texture;
	private readonly Color _disabledColor;
	private readonly Color _enabledColor;
	private readonly Vector2 _textureSize;

	public IconButton(IBounds bounds, Action onClick, ButtonStyle buttonStyle, Texture texture, Color disabledColor, Color enabledColor)
		: base(bounds, onClick, buttonStyle)
	{
		_texture = texture;
		_disabledColor = disabledColor;
		_enabledColor = enabledColor;
		_textureSize = new(texture.Width, texture.Height);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Vector2i<int> scale = Bounds.Size;
		Vector2i<int> topLeft = Bounds.TopLeft;
		Vector2i<int> center = topLeft + scale / 2;
		RenderImplUiBase.Game.SpriteRenderer.Schedule(_textureSize, (scrollOffset + center).ToVector2(), Depth + 2, _texture, IsDisabled ? _disabledColor : _enabledColor);
	}
}
