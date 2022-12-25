using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace Warp.NET.Samples.Ui.Layouts;

public class ScrollContentLayout : Layout
{
	public ScrollContentLayout()
	{
		Label titleLabel = new(new PixelBounds(960, 256, 128, 128), "Scroll Content Layout", LabelStyle.Default with { TextColor = Color.Green });
		TextButton backButton = new(new PixelBounds(256, 320, 256, 128), () => Game.Self.ActiveLayout = Game.Self.MainLayout, ButtonStyle.Default, TextButtonStyle.Default, "Back");

		const int scrollbarWidth = 32;
		ScrollViewer<CustomScrollContent> scrollViewer = new(new PixelBounds(704, 384, 512 + scrollbarWidth, 512), scrollbarWidth);

		NestingContext.Add(titleLabel);
		NestingContext.Add(backButton);
		NestingContext.Add(scrollViewer);

		scrollViewer.InitializeContent();
	}

	private sealed class CustomScrollContent : ScrollContent<CustomScrollContent, ScrollViewer<CustomScrollContent>>, IScrollContent<CustomScrollContent, ScrollViewer<CustomScrollContent>>
	{
		private readonly List<TextButton> _textButtons = new();

		private CustomScrollContent(IBounds bounds, ScrollViewer<CustomScrollContent> parent)
			: base(bounds, parent)
		{
		}

		public override int ContentHeightInPixels { get; protected set; }

		public static CustomScrollContent Construct(IBounds bounds, ScrollViewer<CustomScrollContent> parent)
		{
			return new(bounds, parent);
		}

		public override void SetContent()
		{
			foreach (TextButton component in _textButtons)
				NestingContext.Remove(component);

			_textButtons.Clear();

			const int buttonHeight = 64;
			for (int i = 0; i < 20; i++)
			{
				TextButton button = new(Bounds.CreateNested(0, i * buttonHeight, 512, buttonHeight), () => { }, ButtonStyle.Default, TextButtonStyle.Default, $"Button {i}")
				{
					Depth = Depth + 1,
				};
				_textButtons.Add(button);
				NestingContext.Add(button);
			}

			ContentHeightInPixels = _textButtons.Count * buttonHeight;
		}
	}
}
