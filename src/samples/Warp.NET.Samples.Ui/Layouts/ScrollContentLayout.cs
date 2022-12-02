using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;

namespace Warp.NET.Samples.Ui.Layouts;

public class ScrollContentLayout : Layout
{
	public ScrollContentLayout()
	{
		Label titleLabel = new(new NormalizedBounds(0.45f, 0.2f, 0.1f, 0.1f), "Scroll Content Layout", LabelStyle.Default with { TextColor = Color.Green });
		TextButton backButton = new(new NormalizedBounds(0.1f, 0.3f, 0.1f, 0.1f), () => Game.Self.ActiveLayout = Game.Self.MainLayout, ButtonStyle.Default, TextButtonStyle.Default, "Back");
		ScrollViewer<CustomScrollContent> scrollViewer = new(new NormalizedBounds(0.35f, 0.3f, 0.3f, 0.5f), 16);

		NestingContext.Add(titleLabel);
		NestingContext.Add(backButton);
		NestingContext.Add(scrollViewer);

		scrollViewer.InitializeContent();
	}

	private sealed class CustomScrollContent : ScrollContent<CustomScrollContent, ScrollViewer<CustomScrollContent>>, IScrollContent<CustomScrollContent, ScrollViewer<CustomScrollContent>>
	{
		private const int _buttonCount = 10;
		private const float _buttonHeight = 0.2f;

		private readonly List<TextButton> _textButtons = new();

		private CustomScrollContent(IBounds bounds, ScrollViewer<CustomScrollContent> parent)
			: base(bounds, parent)
		{
		}

		public override int ContentHeightInPixels => _textButtons.Sum(tb => tb.Bounds.Size.Y);

		public static CustomScrollContent Construct(IBounds bounds, ScrollViewer<CustomScrollContent> parent)
		{
			return new(bounds, parent);
		}

		public override void SetContent()
		{
			foreach (TextButton component in _textButtons)
				NestingContext.Remove(component);

			_textButtons.Clear();

			for (int i = 0; i < _buttonCount; i++)
			{
				int height = (int)(_buttonHeight * CurrentWindowState.Height);
				TextButton button = new(Bounds.CreateNested(0, i * height, 1, height), () => {}, ButtonStyle.Default, TextButtonStyle.Default, $"Button {i}")
				{
					Depth = Depth + 1,
				};
				_textButtons.Add(button);
				NestingContext.Add(button);
			}
		}
	}
}
