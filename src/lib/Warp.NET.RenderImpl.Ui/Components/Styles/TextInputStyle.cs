using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Rendering.Text;

namespace Warp.NET.RenderImpl.Ui.Components.Styles;

public record TextInputStyle(Color BackgroundColor, Color BorderColor, Color HoverBackgroundColor, Color TextColor, Color ActiveBorderColor, Color CursorColor, Color SelectionColor, int BorderSize, int TextRenderingHorizontalOffset, FontSize FontSize);
