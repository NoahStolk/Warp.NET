using Silk.NET.OpenAL;
using Warp.NET.Content;

namespace Warp.NET.GameObjects.Common;

public class Sound3dObject : Sound2dObject
{
	public Sound3dObject(Sound sound)
		: base(sound)
	{
	}

	public Vector3 Position
	{
		get
		{
			Audio.Al.GetSourceProperty(SourceId, SourceVector3.Position, out Vector3 position);
			return position;
		}
		set => Audio.Al.SetSourceProperty(SourceId, SourceVector3.Position, value);
	}

	public Vector3 Velocity
	{
		get
		{
			Audio.Al.GetSourceProperty(SourceId, SourceVector3.Velocity, out Vector3 velocity);
			return velocity;
		}
		set => Audio.Al.SetSourceProperty(SourceId, SourceVector3.Velocity, value);
	}
}
