using HedraLibrary.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hedron2D {

    protected Vector2 center;
    public Vector2 Center {
        get { return center; }
        set {
            center = value;
        }
    }

    protected float rotation;
    public float Rotation {
        get { return rotation; }
        set {
            rotation = value;
        }
    }

    public float Area { get; protected set; }

    public Hedron2D() {}
    
    public abstract bool Contains(Vector2 point);
    public abstract bool ContainsAll(Vector2[] points);
    public abstract bool ContainsAll(List<Vector2> points);
    public abstract bool Contains(Segment2D segment);

    public virtual bool Intersects(Line2D line) {
        List<Vector2> intersectionPoints = IntersectionPoints(line);
        return intersectionPoints != null && intersectionPoints.Count > 0;
    }

    public virtual bool Intersects(Segment2D segment) {
        List<Vector2> intersectionPoints = IntersectionPoints(segment);
        return intersectionPoints != null && intersectionPoints.Count > 0;
    }

    public abstract List<Vector2> IntersectionPoints(Line2D line);
    public abstract List<Vector2> IntersectionPoints(Segment2D line);

    public abstract Vector2 ClosestPointTo(Vector2 point);
    public abstract Vector2 FurthestPointFrom(Vector2 point);
}
