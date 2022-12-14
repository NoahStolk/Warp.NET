namespace Warp.NET.Debugging;

public static class DebugStack
{
	private static readonly List<DebugStackEntry> _stack = new();

	public static DebugStackDisplaySetting DisplaySetting { get; set; }

	public static string GetString()
	{
		StringBuilder sb = new();
		foreach (IGrouping<float, DebugStackEntry> group in _stack.GroupBy(dse => dse.Timeout))
			sb.AppendJoin("\n", group.Select(dse => dse.ToString(DisplaySetting))).AppendLine();
		return sb.ToString();
	}

	public static void Update()
	{
		for (int i = _stack.Count - 1; i >= 0; i--)
		{
			DebugStackEntry entry = _stack[i];
			entry.Timeout -= WarpBase.Game.Dt;
			if (entry.Timeout <= 0)
				_stack.RemoveAt(i);
		}
	}

	public static void EndRender()
	{
		for (int i = _stack.Count - 1; i >= 0; i--)
		{
			if (_stack[i].ClearAfterFrame)
				_stack.RemoveAt(i);
		}
	}

	private static string GetCallerDeclaration(string filePath, string memberName)
	{
		string className = Path.GetFileNameWithoutExtension(filePath);
		return $"{className}.{memberName}";
	}

	/// <summary>
	/// Adds the <paramref name="value"/> to the debug stack for 1 update tick if <paramref name="clearAfterFrame"/> is set to <see langword="false"/>. If <paramref name="clearAfterFrame"/> is set to <see langword="true"/>, the value is cleared immediately on the next frame.
	/// </summary>
	/// <param name="value">The object to add to the debug stack.</param>
	/// <param name="clearAfterFrame">Whether to clear the object from the debug stack on the next frame or not.</param>
	/// <param name="expression">The name of the debug stack entry.</param>
	/// <param name="filePath">The caller file path (<see cref="CallerFilePathAttribute"/>).</param>
	/// <param name="memberName">The caller member name (<see cref="CallerMemberNameAttribute"/>).</param>
	/// <param name="lineNumber">The caller line number (<see cref="CallerLineNumberAttribute"/>).</param>
	public static void Add(object? value, bool clearAfterFrame = false, [CallerArgumentExpression(nameof(value))] string expression = "", [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
		=> Add(expression, value, WarpBase.Game.Dt, clearAfterFrame, GetCallerDeclaration(filePath, memberName), lineNumber);

	/// <summary>
	/// Adds the <paramref name="value"/> to the debug stack and removes it after the <paramref name="timeout"/> has ended.
	/// </summary>
	/// <param name="value">The object to add to the debug stack.</param>
	/// <param name="timeout">The amount of seconds the object should remain on the debug stack.</param>
	/// <param name="expression">The name of the debug stack entry.</param>
	/// <param name="filePath">The caller file path (<see cref="CallerFilePathAttribute"/>).</param>
	/// <param name="memberName">The caller member name (<see cref="CallerMemberNameAttribute"/>).</param>
	/// <param name="lineNumber">The caller line number (<see cref="CallerLineNumberAttribute"/>).</param>
	public static void Add(object? value, float timeout, [CallerArgumentExpression(nameof(value))] string expression = "", [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
		=> Add(expression, value, timeout, false, GetCallerDeclaration(filePath, memberName), lineNumber);

	private static void Add(string name, object? value, float timeout, bool clearFrame, string callerDeclaration, int callerLineNumber)
		=> _stack.Add(new(name, value?.ToString() ?? string.Empty, clearFrame, callerDeclaration, callerLineNumber, timeout));

	private sealed class DebugStackEntry
	{
		public DebugStackEntry(string name, string value, bool clearAfterFrame, string callerDeclaration, int callerLineNumber, float timeout)
		{
			Name = name;
			Value = value;
			ClearAfterFrame = clearAfterFrame;
			CallerDeclaration = callerDeclaration;
			CallerLineNumber = callerLineNumber;
			Timeout = timeout;
		}

		public string Name { get; }
		public string Value { get; }
		public bool ClearAfterFrame { get; }
		public string CallerDeclaration { get; }
		public int CallerLineNumber { get; }
		public float Timeout { get; set; }

		public string ToString(DebugStackDisplaySetting displaySetting) => displaySetting switch
		{
			DebugStackDisplaySetting.Simple => $"{Name,-30} {Value}",
			_ => ToVerboseString(),
		};

		private string ToVerboseString()
		{
			float frameCount = Timeout / WarpBase.Game.Dt;
			return $"{GetDisplayFrameCount(frameCount),3}: {CallerDeclaration,-30} L{CallerLineNumber} {Name,-30} {Value}";

			static string GetDisplayFrameCount(float frameCount)
			{
				if (float.IsNaN(frameCount))
					return "NaN";

				return float.IsInfinity(frameCount) ? "INF" : frameCount.ToString("###");
			}
		}
	}
}
