using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using ErrorCode = Silk.NET.GLFW.ErrorCode;
using Monitor = Silk.NET.GLFW.Monitor;

namespace Warp.NET;

public static class Graphics
{
	private static bool _windowIsActive = true;
	private static Glfw? _glfw;
	private static GL? _gl;

	public static Glfw Glfw => _glfw ?? throw new InvalidOperationException("GLFW is not initialized.");
	public static GL Gl => _gl ?? throw new InvalidOperationException("OpenGL is not initialized.");

	public static Action<bool>? OnChangeWindowIsActive { get; set; }
	public static Action<int, int>? OnChangeWindowSize { get; set; }

	public static unsafe WindowHandle* Window { get; private set; }
	public static int WindowWidth { get; set; }
	public static int WindowHeight { get; set; }
	public static bool WindowIsActive
	{
		get => _windowIsActive;
		private set
		{
			_windowIsActive = value;
			OnChangeWindowIsActive?.Invoke(_windowIsActive);
		}
	}

	public static string WindowTitle { get; set; } = string.Empty;

	public static int PrimaryMonitorWidth { get; private set; }
	public static int PrimaryMonitorHeight { get; private set; }

	public static void CreateWindowFull(string title)
		=> CreateWindow(title, PrimaryMonitorWidth, PrimaryMonitorHeight, true);

	public static void CreateWindow(string title, int width, int height)
		=> CreateWindow(title, width, height, false);

	private static unsafe void CreateWindow(string title, int width, int height, bool isFullScreen)
	{
		WindowWidth = isFullScreen ? PrimaryMonitorWidth : width;
		WindowHeight = isFullScreen ? PrimaryMonitorHeight : height;
		WindowTitle = title;

		_glfw = Glfw.GetApi();
		_glfw.Init();
		CheckGlfwError(_glfw);

		_glfw.WindowHint(WindowHintInt.ContextVersionMajor, 3);
		_glfw.WindowHint(WindowHintInt.ContextVersionMinor, 3);
		_glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);

		_glfw.WindowHint(WindowHintBool.Focused, true);
		_glfw.WindowHint(WindowHintBool.Resizable, true);
		CheckGlfwError(_glfw);

		Monitor* primaryMonitor = _glfw.GetPrimaryMonitor();
		if (primaryMonitor == (Monitor*)0)
		{
			// TODO: Log warning with Serilog.
			PrimaryMonitorWidth = 1024;
			PrimaryMonitorHeight = 768;
		}
		else
		{
			_glfw.GetMonitorWorkarea(primaryMonitor, out _, out _, out int primaryMonitorWidth, out int primaryMonitorHeight);
			PrimaryMonitorWidth = primaryMonitorWidth;
			PrimaryMonitorHeight = primaryMonitorHeight;
		}

		Window = _glfw.CreateWindow(width, height, title, isFullScreen ? primaryMonitor : (Monitor*)0, (WindowHandle*)0);
		CheckGlfwError(_glfw);
		if (Window == (WindowHandle*)0)
			throw new InvalidOperationException("Could not create window.");

		_glfw.SetKeyCallback(Window, (_, keys, _, state, _) => Input.KeyCallback(keys, state));
		_glfw.SetMouseButtonCallback(Window, (_, button, state, _) => Input.ButtonCallback(button, state));
		_glfw.SetScrollCallback(Window, (_, _, y) => Input.MouseWheelCallback(y));
		_glfw.SetFramebufferSizeCallback(Window, (_, w, h) => SetWindowSize(w, h));
		_glfw.SetWindowFocusCallback(Window, (_, focusing) => WindowIsActive = focusing);

		int x = (PrimaryMonitorWidth - width) / 2;
		int y = (PrimaryMonitorHeight - height) / 2;

		_glfw.SetWindowPos(Window, x, y);

		_glfw.MakeContextCurrent(Window);
		_gl = GL.GetApi(_glfw.GetProcAddress);

		SetWindowSize(WindowWidth, WindowHeight);

		_glfw.SwapInterval(0); // Turns VSync off.
	}

	private static void SetWindowSize(int width, int height)
	{
		WindowWidth = width;
		WindowHeight = height;
		OnChangeWindowSize?.Invoke(width, height);
	}

	private static unsafe void CheckGlfwError(Glfw glfw)
	{
		ErrorCode errorCode = glfw.GetError(out byte* c);
		if (errorCode == ErrorCode.NoError || c == (byte*)0)
			return;

		StringBuilder errorBuilder = new();
		while (*c != 0x00)
			errorBuilder.Append((char)*c++);

		throw new InvalidOperationException($"GLFW {errorCode}: {errorBuilder}");
	}
}
