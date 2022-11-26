using Warp.NET.Numerics;
using Warp.NET.RenderImpl.Ui;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace Warp.NET.Samples.Ui.Layouts;

public class ScrollContentLayout : Layout
{
	public ScrollContentLayout()
	{
		Label titleLabel = new(new(0.45f, 0.2f, 0.1f, 0.1f), "Scroll Content Layout", LabelStyle.Default with { TextColor = Color.Green });
		TextButton backButton = new(new(0.1f, 0.3f, 0.1f, 0.1f), () => Game.Self.ActiveLayout = Game.Self.MainLayout, ButtonStyle.Default, TextButtonStyle.Default, "Back");
		CustomScrollViewer scrollViewer = new(new(0.5f, 0.3f, 0.3f, 0.5f));

		NestingContext.Add(titleLabel);
		NestingContext.Add(backButton);
		NestingContext.Add(scrollViewer);

		scrollViewer.InitializeContent();
	}

	private sealed class CustomScrollViewer : AbstractScrollViewer<CustomScrollViewer, CustomScrollContent>
	{
		public CustomScrollViewer(Bounds bounds)
			: base(bounds)
		{
			Content = new(Bounds.CreateNested(0, 0, 0.95f, 1), this);
			Scrollbar = new Scrollbar(Bounds.CreateNested(0.95f, 0, 0.05f, 1), SetScrollPercentage);

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
		private const float _buttonHeight = 0.2f;

		public CustomScrollContent(Bounds bounds, AbstractScrollViewer<CustomScrollViewer, CustomScrollContent> parent)
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

			RenderImplUiBase.Game.RectangleRenderer.Schedule(scale, parentPosition + center, Depth, Color.Purple);
		}

		public void SetContent()
		{
			for (int i = 0; i < _buttonCount; i++)
			{
				TextButton button = new(Bounds.CreateNested(0, i * _buttonHeight, 1, _buttonHeight), () => {}, ButtonStyle.Default, TextButtonStyle.Default, i.ToString());
				NestingContext.Add(button);
			}
		}
	}
}
