using System.Globalization;
using System.Numerics;

namespace Warp.NET.Samples.Map.MapBuilder;

public static class FileParser
{
    public static List<Brush> LoadBrushes(string fileName)
    {
        List<Brush> brushes = new();

        string mapData = File.ReadAllText(fileName);

        for (int i = 2; i < mapData.Split('{').Length; i++)
        {
            string mapDataSplit = mapData.Split('{')[i].Split('}')[0].Trim();
            string[] planeData = mapDataSplit.Split('\n');

            if (!planeData[0].Trim().StartsWith("("))
                continue;

            List<Face> faces = new();
            for (int j = 0; j < planeData.Length; j++)
                faces.Add(ParseFace(planeData[j]));

            brushes.Add(new(faces.ToArray()));
        }

        return brushes;
    }

    private static Face ParseFace(string brushData)
    {
        CultureInfo ci = CultureInfo.InvariantCulture;

        string brushDataP1 = brushData.Split('(')[1].Split(')')[0].Trim();
        string brushDataP2 = brushData.Split('(')[2].Split(')')[0].Trim();
        string brushDataP3 = brushData.Split('(')[3].Split(')')[0].Trim();
        string textureName = brushData.Split('(')[3].Split(')')[1].Trim().Split(' ')[0].Trim();

        string textureAxisU = brushData.Split(new[] { "[" }, StringSplitOptions.None)[1].Split(']')[0].Trim().Trim();
        string textureAxisV = brushData.Split(new[] { "[" }, StringSplitOptions.None)[2].Split(']')[0].Trim().Trim();

        string textureScale = brushData.Split(new[] { "]" }, StringSplitOptions.None)[2].Trim();

        Vector3 p1 = new(
            float.Parse(brushDataP1.Split(' ')[0], NumberStyles.Any, ci),
            float.Parse(brushDataP1.Split(' ')[1], NumberStyles.Any, ci),
            float.Parse(brushDataP1.Split(' ')[2], NumberStyles.Any, ci));

        Vector3 p2 = new(
            float.Parse(brushDataP2.Split(' ')[0], NumberStyles.Any, ci),
            float.Parse(brushDataP2.Split(' ')[1], NumberStyles.Any, ci),
            float.Parse(brushDataP2.Split(' ')[2], NumberStyles.Any, ci));

        Vector3 p3 = new(
            float.Parse(brushDataP3.Split(' ')[0], NumberStyles.Any, ci),
            float.Parse(brushDataP3.Split(' ')[1], NumberStyles.Any, ci),
            float.Parse(brushDataP3.Split(' ')[2], NumberStyles.Any, ci));

        Plane planeU = new(
            float.Parse(textureAxisU.Split(' ')[0], NumberStyles.Any, ci),
            float.Parse(textureAxisU.Split(' ')[2], NumberStyles.Any, ci),
            float.Parse(textureAxisU.Split(' ')[1], NumberStyles.Any, ci),
            float.Parse(textureAxisU.Split(' ')[3], NumberStyles.Any, ci));

        Plane planeV = new(
            float.Parse(textureAxisV.Split(' ')[0], NumberStyles.Any, ci),
            float.Parse(textureAxisV.Split(' ')[2], NumberStyles.Any, ci),
            float.Parse(textureAxisV.Split(' ')[1], NumberStyles.Any, ci),
            float.Parse(textureAxisV.Split(' ')[3], NumberStyles.Any, ci));

        // Texture rotation is given (which is useless information because the texture axis are already rotated).
        float textureScaleU = float.Parse(textureScale.Split(' ')[1].Trim(), NumberStyles.Any, ci);
        float textureScaleV = float.Parse(textureScale.Split(' ')[2], NumberStyles.Any, ci);

        // Rotate plane X Z Y
        return new(
            new(p1.X, p1.Z, p1.Y),
            new(p2.X, p2.Z, p2.Y),
            new(p3.X, p3.Z, p3.Y),
            textureName,
            planeU,
            planeV,
            new(textureScaleU, textureScaleV));
    }
}
