using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.Samples.Ui.Layouts;

public class ScrollContentLayout : Layout
{
	private const int _buttonHeight = 64;

	private readonly ButtonStyle _buttonStyle = new(Color.Black, Color.White, Color.Gray(0.5f), 2);
	private readonly TextButtonStyle _textButtonStyle = new(Color.White, TextAlign.Middle, FontSize.H24);
	private readonly ScrollArea _scrollArea;

	public ScrollContentLayout()
	{
		LabelStyle labelStyle = new(Color.Green, TextAlign.Middle, FontSize.H16);

		Label titleLabel = new(new PixelBounds(960 - 128, 256, 256, 128), "Scroll Content Layout", labelStyle);
		TextButton backButton = new(new PixelBounds(256, 320, 256, 128), () => Game.Self.ActiveLayout = Game.Self.MainLayout, _buttonStyle, _textButtonStyle, "Back");
		NestingContext.Add(titleLabel);
		NestingContext.Add(backButton);

		const int scrollbarWidth = 32;
		ScrollAreaStyle scrollAreaStyle = new(2, 2, Color.White, Color.Gray(0.75f), Color.Black, Color.Gray(0.5f), Color.Gray(0.25f));
		_scrollArea = new(new PixelBounds(960 - (256 + scrollbarWidth / 2), 384, 512 + scrollbarWidth, 512), 96, 32, scrollAreaStyle);

		BuildScrollContent();

		NestingContext.Add(_scrollArea);

		for (int i = 0; i < 20; i++)
		{
			const int height = 32;
			int index = i;
			TextButton scrollTo = new(new PixelBounds(960 + 480, 256 + i * height, 256, height), () => ScrollTo(index), _buttonStyle, new(Color.White, TextAlign.Middle, FontSize.H12), $"Scroll to button index {index}");
			NestingContext.Add(scrollTo);
		}

		void ScrollTo(int index)
		{
			BuildScrollContent();
			_scrollArea.ScheduleScrollTarget(_buttonHeight * index, _buttonHeight * (index + 1));
		}
	}

	private void BuildScrollContent()
	{
		foreach (AbstractComponent component in _scrollArea.NestingContext.OrderedComponents)
			_scrollArea.NestingContext.Remove(component);

		for (int i = 0; i < 20; i++)
		{
			TextButton button = new(_scrollArea.Bounds.CreateNested(0, i * _buttonHeight, 512, _buttonHeight), () => { }, _buttonStyle, _textButtonStyle, $"Button {i}") { Depth = _scrollArea.Depth + 1 };
			_scrollArea.NestingContext.Add(button);
		}
	}
}
