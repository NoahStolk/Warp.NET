namespace Warp.NET.Samples.Map.MapBuilder;

public static class Map
{
    public static List<Brush> Load(string fileName)
    {
        List<Brush> brushes = FileParser.LoadBrushes(fileName);

        SortVerticesCw(brushes);
        CalculateTextureCoordinates(brushes);
        Inverse(brushes);

        return brushes;
    }

    private static void Inverse(List<Brush> brushes)
    {
        foreach (Brush brush in brushes)
        {
            foreach (Polygon polygon in brush.Polygons)
            {
                for (int i = 0; i < polygon.Vertices.Count; i++)
                {
                    polygon.Vertices[i] = new(polygon.Vertices[i].X * -1, polygon.Vertices[i].Y, polygon.Vertices[i].Z);
                }
            }
        }
    }

    private static void CalculateTextureCoordinates(List<Brush> brushes)
    {
        foreach (Brush brush in brushes)
        {
            foreach (Polygon polygon in brush.Polygons)
            {
                GeometryMath.CalculateTextureCoordinates(polygon);
            }
        }
    }

    private static void SortVerticesCw(List<Brush> brushes)
    {
        foreach (Brush brush in brushes)
        {
            foreach (Polygon polygon in brush.Polygons)
            {
                GeometryMath.SortVerticesCw(polygon);
            }
        }
    }
}
