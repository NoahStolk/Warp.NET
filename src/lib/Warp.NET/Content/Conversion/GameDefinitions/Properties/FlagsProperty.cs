namespace Warp.NET.Content.Conversion.GameDefinitions.Properties;

internal record FlagsProperty
{
	public FlagsProperty(string name, IReadOnlyList<Flag> flags)
	{
		foreach (Flag flag in flags)
		{
			if (flag.Value < 1 || (flag.Value & flag.Value - 1) != 0)
				throw new ArgumentException("Flags must be a power of two.", nameof(flags));

			if (flags.Any(f => f != flag && f.Value == flag.Value))
				throw new ArgumentException("Flag values must be unique.", nameof(flags));
		}

		// TODO: Check if in TrenchBroom, the Name must always be "spawnflags".
		Name = name;
		Flags = flags;
	}

	public string Name { get; }
	public IReadOnlyList<Flag> Flags { get; }
}
