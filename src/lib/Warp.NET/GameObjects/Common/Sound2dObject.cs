using Silk.NET.OpenAL;
using Warp.NET.Content;

namespace Warp.NET.GameObjects.Common;

public class Sound2dObject : GameObject
{
	private float _pitch = 1;
	private float _gain = 1;

	public Sound2dObject(Sound sound)
	{
		Sound = sound;
		SourceId = Sound.CreateSource();

		SetPitch();
		SetGain();
	}

	protected uint SourceId { get; private set; }

	public Sound Sound { get; }

	public float Pitch
	{
		get => _pitch;
		set
		{
			_pitch = value;
			SetPitch();
		}
	}

	public float Gain
	{
		get => _gain;
		set
		{
			_gain = value;
			SetGain();
		}
	}

	public SourceState State
	{
		get
		{
			if (SourceId == 0)
				return SourceState.Initial;

			Audio.Al.GetSourceProperty(SourceId, GetSourceInteger.SourceState, out int state);
			return (SourceState)state;
		}
	}

	public bool Looping
	{
		get
		{
			Audio.Al.GetSourceProperty(SourceId, SourceBoolean.Looping, out bool value);
			return value;
		}
		set => Audio.Al.SetSourceProperty(SourceId, SourceBoolean.Looping, value);
	}

	public override void Update()
	{
		base.Update();

		SetPitch();
		SetGain();

		if (State == SourceState.Stopped)
			Remove();
	}

	private void SetPitch()
		=> Audio.Al.SetSourceProperty(SourceId, SourceFloat.Pitch, Pitch * WarpBase.Game.TimeMultiplier);

	private void SetGain()
		=> Audio.Al.SetSourceProperty(SourceId, SourceFloat.Gain, Gain * WarpBase.Game.AudioVolume);

	private void Play()
	{
		if (State != SourceState.Playing)
			Audio.Al.SourcePlay(SourceId);
	}

	private void Stop()
	{
		if (State != SourceState.Stopped)
			Audio.Al.SourceStop(SourceId);
	}

	public void Toggle(bool isPaused)
	{
		if (!isPaused && State == SourceState.Paused)
			Audio.Al.SourcePlay(SourceId);
		else if (isPaused && State == SourceState.Playing)
			Audio.Al.SourcePause(SourceId);
	}

	public unsafe void Delete()
	{
		if (SourceId == 0)
			return;

		Stop();

		uint[] buffers = new uint[1];
		fixed (uint* b = &buffers[0])
		{
			Audio.Al.SourceUnqueueBuffers(SourceId, 1, b);
			Audio.Al.DeleteBuffers(1, b);
		}

		uint[] sources = { SourceId };
		fixed (uint* s = &sources[0])
			Audio.Al.DeleteSources(1, s);

		SourceId = 0;
	}

	public override void Add()
	{
		base.Add();

		Play();
	}

	public override void Remove()
	{
		base.Remove();

		Delete();
	}
}
