using System.Numerics;

namespace Warp.NET.Samples.Map.MapBuilder;

public class Brush
{
    public Brush(Face[] faces)
    {
        List<Plane> planes = new();

        for (int i = 0; i < faces.Length; i++)
        {
            planes.Add(Plane.CreateFromVertices(faces[i].P1, faces[i].P2, faces[i].P3));
        }

        Plane lfi = default;
        Plane lfj = default;
        Plane lfk = default;

        Polygons = new();

        for (int i = 0; i < faces.Length; i++)
        {
            Polygons.Add(new(planes[i], faces[i]));

            if (i == faces.Length - 3)
            {
                if (i + 1 < faces.Length)
                {
                    lfi = planes[i + 1];
                }
            }
            else if (i == faces.Length - 2)
            {
                if (i + 1 < faces.Length)
                {
                    lfj = planes[i + 1];
                }
            }
            else if (i == faces.Length - 1)
            {
                if (i + 1 < faces.Length)
                {
                    lfk = planes[i + 1];
                }
            }
        }

        for (int fi = 0; planes[fi] != lfi; fi++)
        {
            for (int fj = fi + 1; planes[fj] != lfj; fj++)
            {
                for (int fk = fj + 1; planes[fk] != lfk; fk++)
                {
                    if (GeometryMath.GetIntersection(planes[fj], planes[fk], planes[fi], out Vector3 p))
                    {
                        bool illegal = false;

                        for (int i = 0; i < faces.Length; i++)
                        {
                            if (GeometryMath.ClassifyPoint(p, planes[i]) == GeometryMath.ECp.Front)
                            {
                                illegal = true;
                                break;
                            }
                        }

                        if (!illegal)
                        {
                            Polygons[fi].Vertices.Add(p);
                            Polygons[fj].Vertices.Add(p);
                            Polygons[fk].Vertices.Add(p);
                        }
                    }

                    if (fk + 1 >= faces.Length)
                    {
                        break;
                    }
                }
            }
        }
    }

    public List<Polygon> Polygons { get; }
}
