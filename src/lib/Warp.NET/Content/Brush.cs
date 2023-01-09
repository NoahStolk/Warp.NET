namespace Warp.NET.Content;

public class Brush
{
	public Brush(List<Face> faces)
	{
		if (faces.Count < 4)
			throw new ArgumentException("There must be at least four faces defined in a brush.", nameof(faces));

		Faces = faces;
	}

	public List<Face> Faces { get; }
}
