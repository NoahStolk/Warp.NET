using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Coordinates;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.Samples.Ui.Layouts;

public class ScrollContentLayout : Layout
{
	public ScrollContentLayout()
	{
		Label titleLabel = new(new Rectangle(0.45f, 0.2f, 0.1f, 0.1f), "Scroll Content Layout", LabelStyle.Default with { TextColor = Color.Green });
		TextButton backButton = new(new Rectangle(0.1f, 0.3f, 0.1f, 0.1f), () => Game.Self.ActiveLayout = Game.Self.MainLayout, ButtonStyle.Default, TextButtonStyle.Default, "Back");
		CustomScrollViewer scrollViewer = new(new Rectangle(0.5f, 0.3f, 0.3f, 0.5f));

		NestingContext.Add(titleLabel);
		NestingContext.Add(backButton);
		NestingContext.Add(scrollViewer);

		scrollViewer.InitializeContent();
	}

	private sealed class CustomScrollViewer : AbstractScrollViewer<CustomScrollViewer, CustomScrollContent>
	{
		public CustomScrollViewer(IBounds bounds)
			: base(bounds)
		{
			Content = new(new Rectangle(0, 0, 0.1f, 0.3f), this);
			Scrollbar = new Scrollbar(new Rectangle(0.1f, 0, 0.01f, 0.3f), SetScrollPercentage);

			NestingContext.Add(Content);
			NestingContext.Add(Scrollbar);
		}

		public override AbstractScrollbar Scrollbar { get; }
		public override CustomScrollContent Content { get; }

		public override void InitializeContent()
		{
			Content.SetContent();
			SetThumbPercentageSize();
			SetScrollPercentage(0);
		}

		public override void Render(Vector2i<int> parentPosition)
		{
			base.Render(parentPosition);

			Vector2i<int> scale = Bounds.Size;
			Vector2i<int> topLeft = Bounds.TopLeft;
			Vector2i<int> center = topLeft + scale / 2;

			RenderImplUiBase.Game.RectangleRenderer.Schedule(scale, parentPosition + center, Depth - 4, Color.HalfTransparentWhite);
		}
	}

	private sealed class CustomScrollContent : ScrollContent<CustomScrollContent, CustomScrollViewer>
	{
		private const int _buttonCount = 25;
		private const float _buttonHeight = 0.02f;

		public CustomScrollContent(IBounds bounds, AbstractScrollViewer<CustomScrollViewer, CustomScrollContent> parent)
			: base(bounds, parent)
		{
		}

		public override int ContentHeightInPixels => (int)(_buttonCount * _buttonHeight * CurrentWindowState.Height);

		public override void Render(Vector2i<int> parentPosition)
		{
			base.Render(parentPosition);

			Vector2i<int> scale = Bounds.Size;
			Vector2i<int> topLeft = Bounds.TopLeft;
			Vector2i<int> center = topLeft + scale / 2;

			RenderImplUiBase.Game.RectangleRenderer.Schedule(scale, parentPosition + center, Depth - 3, Color.Purple);
		}

		public void SetContent()
		{
			for (int i = 0; i < _buttonCount; i++)
			{
				TextButton button = new(new Rectangle(0, i * _buttonHeight, 0.1f, _buttonHeight), () => {}, ButtonStyle.Default, TextButtonStyle.Default, i.ToString());
				NestingContext.Add(button);
			}
		}
	}
}
