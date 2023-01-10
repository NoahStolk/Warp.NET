using Warp.NET.Content.Conversion.GameDefinitions.Properties;

namespace Warp.NET.Content.Conversion.GameDefinitions;

internal record GameDefinitionBinary(IReadOnlyList<GameClass> GameClasses) : IBinary<GameDefinitionBinary>
{
	public ContentType ContentType => ContentType.GameDefinition;

	public byte[] ToBytes()
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write((ushort)GameClasses.Count);

		foreach (GameClass gameClass in GameClasses)
		{
			bw.Write((byte)gameClass.Type);

			bw.Write((ushort)gameClass.BaseClassNames.Count);
			foreach (string baseClassName in gameClass.BaseClassNames)
				bw.Write(baseClassName);

			bw.Write(gameClass.Name);

			bw.Write((ushort)gameClass.ChoicesProperties.Count);
			foreach (ChoicesProperty choicesProperty in gameClass.ChoicesProperties)
			{
				bw.Write(choicesProperty.Name);
				bw.Write(choicesProperty.DefaultValue);

				bw.Write((ushort)choicesProperty.Choices.Count);
				foreach (KeyValuePair<int, string> kvp in choicesProperty.Choices)
				{
					bw.Write(kvp.Key);
					bw.Write(kvp.Value);
				}
			}

			bw.Write((ushort)gameClass.FlagsProperties.Count);
			foreach (FlagsProperty flagsProperty in gameClass.FlagsProperties)
			{
				bw.Write(flagsProperty.Name);

				bw.Write((ushort)flagsProperty.Flags.Count);
				foreach (Flag flag in flagsProperty.Flags)
				{
					bw.Write(flag.Value);
					bw.Write(flag.Name);
					bw.Write(flag.Default);
				}
			}

			bw.Write((ushort)gameClass.IntegerProperties.Count);
			foreach (IntegerProperty integerProperty in gameClass.IntegerProperties)
			{
				bw.Write(integerProperty.Name);

				if (integerProperty.DefaultValue.HasValue)
				{
					bw.Write((byte)1);
					bw.Write(integerProperty.DefaultValue.Value);
				}
				else
				{
					bw.Write((byte)0);
				}
			}

			bw.Write((ushort)gameClass.StringProperties.Count);
			foreach (StringProperty stringProperty in gameClass.StringProperties)
			{
				bw.Write(stringProperty.Name);

				if (stringProperty.DefaultValue != null)
				{
					bw.Write((byte)1);
					bw.Write(stringProperty.DefaultValue);
				}
				else
				{
					bw.Write((byte)0);
				}
			}
		}

		return ms.ToArray();
	}

	public static GameDefinitionBinary FromStream(BinaryReader br)
	{
		ushort gameClassCount = br.ReadUInt16();
		List<GameClass> gameClasses = new(gameClassCount);

		for (int i = 0; i < gameClassCount; i++)
		{
			ClassType classType = (ClassType)br.ReadByte();

			ushort baseClassCount = br.ReadUInt16();
			List<string> baseClassNames = new(baseClassCount);

			for (int j = 0; j < baseClassCount; j++)
				baseClassNames.Add(br.ReadString());

			string name = br.ReadString();

			ushort choicesPropertyCount = br.ReadUInt16();
			List<ChoicesProperty> choicesProperties = new(choicesPropertyCount);

			for (int j = 0; j < choicesPropertyCount; j++)
			{
				string choicesPropertyName = br.ReadString();
				int choicesPropertyDefaultValue = br.ReadInt32();

				ushort choicesCount = br.ReadUInt16();
				Dictionary<int, string> choices = new(choicesCount);

				for (int k = 0; k < choicesCount; k++)
				{
					int key = br.ReadInt32();
					string value = br.ReadString();
					choices.Add(key, value);
				}

				choicesProperties.Add(new(choicesPropertyName, choicesPropertyDefaultValue, choices));
			}

			ushort flagsPropertyCount = br.ReadUInt16();
			List<FlagsProperty> flagsProperties = new(flagsPropertyCount);

			for (int j = 0; j < flagsPropertyCount; j++)
			{
				string flagsPropertyName = br.ReadString();

				ushort flagsCount = br.ReadUInt16();
				List<Flag> flags = new(flagsCount);

				for (int k = 0; k < flagsCount; k++)
				{
					int value = br.ReadInt32();
					string flagName = br.ReadString();
					bool @default = br.ReadBoolean();
					flags.Add(new(value, flagName, @default));
				}

				flagsProperties.Add(new(flagsPropertyName, flags));
			}

			ushort integerPropertyCount = br.ReadUInt16();
			List<IntegerProperty> integerProperties = new(integerPropertyCount);

			for (int j = 0; j < integerPropertyCount; j++)
			{
				string integerPropertyName = br.ReadString();
				bool hasDefaultValue = br.ReadByte() == 1;
				int? defaultValue = hasDefaultValue ? br.ReadInt32() : null;
				integerProperties.Add(new(integerPropertyName, defaultValue));
			}

			ushort stringPropertyCount = br.ReadUInt16();
			List<StringProperty> stringProperties = new(stringPropertyCount);

			for (int j = 0; j < stringPropertyCount; j++)
			{
				string stringPropertyName = br.ReadString();
				bool hasDefaultValue = br.ReadByte() == 1;
				string? defaultValue = hasDefaultValue ? br.ReadString() : null;
				stringProperties.Add(new(stringPropertyName, defaultValue));
			}

			gameClasses.Add(new(classType, baseClassNames, name, choicesProperties, flagsProperties, integerProperties, stringProperties));
		}

		return new(gameClasses);
	}
}
