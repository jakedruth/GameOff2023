using System;
using Godot;


[Serializable]
public class QubicBezierCurve
{
    public Vector2[] points;
    public Vector2 p0 { get { return points[0]; } set { points[0] = value; } }
    public Vector2 p1 { get { return points[1]; } set { points[1] = value; } }
    public Vector2 p2 { get { return points[2]; } set { points[2] = value; } }
    public Vector2 p3 { get { return points[3]; } set { points[3] = value; } }
    public Vector2 this[int key] { get { return points[key]; } set { points[key] = value; } }

    public QubicBezierCurve()
    {
        points = new Vector2[4];
    }

    public QubicBezierCurve(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3)
    {
        points = new Vector2[] { P0, P1, P2, P3 };
    }

    // https://www.bit-101.com/blog/2022/12/coding-curves-08-bezier-curves/
    public Vector2 Sample(float t)
    {
        float m = 1 - t;
        float a = m * m * m;
        float b = 3 * m * m * t;
        float c = 3 * m * t * t;
        float d = t * t * t;
        return p0 * a + p1 * b + p2 * c + p3 * d;
    }

    public override string ToString()
    {
        return $"p0: {p0}\tp1: {p1}\tp2: {p2}\tp3: {p3}";
    }
}
