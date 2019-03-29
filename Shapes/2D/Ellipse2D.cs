using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HedraLibrary.Shapes {

    public class Ellipse2D {

        public Vector2 Center { get; protected set; }
        public float HorizontalRadius { get; protected set; }
        public float VerticalRadius { get; protected set; }

        public Ellipse2D(Vector2 center, float horizontalRadius, float verticalRadius) {
            this.Center = center;
            this.HorizontalRadius = horizontalRadius;
            this.VerticalRadius = verticalRadius;
        }

        public Ellipse2D(Vector2 center, Vector2 horizontalRadiusVector, Vector2 verticalRadiusVector) {
            this.Center = center;
            float horizontal = (horizontalRadiusVector - center).magnitude;
            float vertical = (verticalRadiusVector - center).magnitude;

            this.HorizontalRadius = horizontal;
            this.VerticalRadius = vertical;
        }

        /// <summary>
        /// Generates points according to this ellipse.
        /// </summary>
        /// <param name="points">Number of points in the ellipse.</param>
        /// <param name="rotation">Rotation of the ellipse; changes the position of the first point.</param>
        /// <returns>A list of points generated evenly in this ellipse.</returns>
        public List<Vector2> GeneratePoints(int points, float rotation) {
            return Hedra.Arc(Center, 360f, HorizontalRadius, VerticalRadius, points, rotation);
        }

        public List<Vector2> GeneratePoints(int points, float amplitude, float rotation) {
            return Hedra.Arc(Center, amplitude, HorizontalRadius, VerticalRadius, points, rotation);
        }

        /// <summary>
        /// Calculates the intersection points of a line against this ellipse.
        /// </summary>
        /// <param name="lineP1">Origin point of the line.</param>
        /// <param name="lineP2">End point of the line.</param>
        /// <param name="inSegment">Choose only those intersection points in the segment P1-P2.</param>
        /// <returns></returns>
        public List<Vector2> Intersect(Vector3 lineP1, Vector3 lineP2, bool inSegment) {
            // Source:
            //http://csharphelper.com/blog/2017/08/calculate-where-a-line-segment-and-an-ellipse-intersect-in-c/

            float xVector2 = Mathf.Pow((lineP2.x - lineP1.x), 2);
            float yVector2 = Mathf.Pow((lineP2.y - lineP1.y), 2);
            float horizontalValue = (xVector2 / HorizontalRadius) / HorizontalRadius;
            float verticalValue = (yVector2 / VerticalRadius) / VerticalRadius;

            float A = horizontalValue + verticalValue;
            float B = 2 * lineP1.x * horizontalValue + 2 * lineP1.y * verticalValue;
            float C = lineP1.x * horizontalValue + lineP1.y * verticalValue - 1;

            float[] t = Hedra.QuadraticFormula(A, B, C);

            List<Vector2> intersectionPoints = new List<Vector2>();
            for (int i = 0; i < t.Length; i++) {
                if (!inSegment || ((t[i] >= 0f) && (t[i] <= 1f))) {
                    Vector2 point = new Vector2();
                    point.x = lineP1.x + (lineP2.x - lineP1.x) * t[i] + Center.x;
                    point.y = lineP1.y + (lineP2.y - lineP1.y) * t[i] + Center.y;

                    intersectionPoints.Add(point);
                }
            }

            return intersectionPoints;
        }
    }

}