namespace Warp.NET.SourceGen.Generators.Data;

public class GameObjectState
{
	public GameObjectState(string stateType, string stateName)
	{
		StateType = stateType;
		StateName = stateName;
	}

	public string StateType { get; }
	public string StateName { get; }
}
