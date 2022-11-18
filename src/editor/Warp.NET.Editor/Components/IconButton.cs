using System.Numerics;
using Warp.NET.Editor.Components.Styles;
using Warp.NET.Extensions;
using Warp.NET.Numerics;
using Warp.NET.Ui;

namespace Warp.NET.Editor.Components;

public class IconButton : Button
{
	private readonly Texture _texture;
	private readonly Vector2 _textureSize;

	public IconButton(IBounds bounds, Action onClick, ButtonStyle buttonStyle, Texture texture)
		: base(bounds, onClick, buttonStyle)
	{
		_texture = texture;
		_textureSize = new(texture.Width, texture.Height);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Vector2i<int> scale = Bounds.Size;
		Vector2i<int> topLeft = Bounds.TopLeft;
		Vector2i<int> center = topLeft + scale / 2;
		Game.Self.SpriteRenderer.Schedule(_textureSize, (parentPosition + center).ToVector2(), Depth + 2, _texture, IsDisabled ? Color.HalfTransparentWhite : Color.White);
	}
}