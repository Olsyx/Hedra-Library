/*
 * Hedra's partial class responsible for all space point generations.
 * Contains both 2D and 3D functions for each generator, with several 
 * overrides for easier use. 
 * 
 * Depends on HedraTrigonometry and HedraGeometry.
 *
 * @author  Olsyx (Olatz Castaño)
 * @source https://github.com/Darkatom/Hedra-Library
 * @since   Unity 2017.1.0p4
 */


using Hedras.Objects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

 public static partial class Hedra {
        /// <summary>
        /// Returns a path of zigzaguing points.
        /// </summary>
        /// <param name="origin">Origin of the path.</param>
        /// <param name="end">End point of the path.</param>
        /// <param name="deviation">Deviation of the path center from the origin. Values on range [-1, 1]: first point, center, first turning point.</param>
        /// <param name="direction">Initial direction of the path. Values on range [0, 1].</param>
        /// <param name="amplitude">Distance between the two sides of the zigzag.</param>
        /// <param name="turns">Number of turning points of the path.</param>
        /// <param name="subdivisions">Number of divisions of the vector represented by two turning points.</param>
        /// <returns></returns>        
        public static List<Vector2> Zigzag(Vector2 origin, Vector2 end, int deviation, int direction, float amplitude, int turns, int subdivisions) {
            float extent = (end - origin).magnitude;
            float rotation = -Angle(origin, origin + Vector2.up, end);
            return Zigzag(origin, deviation, direction, extent, amplitude, turns, subdivisions, rotation);
        }

        /// <summary>
        /// Returns a path of zigzaguing points.
        /// </summary>
        /// <param name="origin">Origin of the path.</param>
        /// <param name="deviation">Deviation of the path center from the origin. Values on range [-1, 1]: first point, center, first turning point.</param>
        /// <param name="direction">Initial direction of the path. Values on range [0, 1].</param>
        /// <param name="extent">Physical length of the path.</param>
        /// <param name="amplitude">Distance between the two sides of the zigzag.</param>
        /// <param name="turns">Number of turning points of the path.</param>
        /// <param name="subdivisions">Number of divisions of the vector represented by two turning points.</param>
        /// <param name="rotation">Initial rotation, in degrees, around origin.</param>
        /// <returns></returns>
        public static List<Vector2> Zigzag(Vector2 origin, int deviation, int direction, float extent, float amplitude, int turns, int subdivisions, float rotation) {      
            float width = amplitude / 2;
            float height = extent / (turns - 1);
            int pathDirection = direction >= 1 ? 1 : -1;
            int pathDeviation = Mathf.Clamp(deviation, -1, 1);

            Vector2 deviatedOrigin = origin;
            deviatedOrigin.x = origin.x + pathDirection * pathDeviation * width;

            List<Vector2> vectors = new List<Vector2>();
            for (int i = 0; i < turns; i++) {
                Vector2 point = new Vector2();
                point.x = deviatedOrigin.x + pathDirection * width;
                point.y = deviatedOrigin.y + height * i;

                point = Rotate(origin, point, rotation);

                vectors.Add(point);
                pathDirection = -pathDirection;
            }

            if (subdivisions <= 1) {
                return vectors;
            }

            List<Vector2> points = new List<Vector2>();
            for (int i = 1; i < vectors.Count; i++) {
                List<Vector2> dividedVector = Subdivide(vectors[i - 1], vectors[i], subdivisions);
                points.AddRange(dividedVector);
            }

            points = points.Distinct().ToList();  

            return points;
        }

        /// <summary>
        /// Returns points in a defined wave.
        /// </summary>
        /// <param name="origin">Origin of the wave.</param>
        /// <param name="end">End of wave.</param>
        /// <param name="amplitude">Amplitude of the wave; the distance between opposing peaks.</param>
        /// <param name="frequency">Frecuency of periods in the wave.</param>
        /// <param name="points">Points to draw accross the wave.</param>
        /// <returns></returns>
        public static List<Vector2> Wave(Vector2 origin, Vector2 end, float amplitude, float frequency, int points) {
            float extent = (end - origin).magnitude;
            float rotation = -Angle(origin, origin + Vector2.up, end);
            return Wave(origin, extent, amplitude, frequency, points, rotation);
        }

        /// <summary>
        /// Returns points in a defined wave.
        /// </summary>
        /// <param name="origin">Origin of the wave.</param>
        /// <param name="extent">Length of the wave.</param>
        /// <param name="amplitude">Amplitude of the wave; the distance between opposing peaks.</param>
        /// <param name="frequency">Frecuency of periods in the wave.</param>
        /// <param name="points">Points to draw accross the wave.</param>
        /// <param name="rotation">Rotation of the wave around the axis; rotates its direction.</param>
        /// <returns></returns>
        public static List<Vector2> Wave(Vector2 origin, float extent, float amplitude, float frequency, int points, float rotation) {
            float separation = extent / (points-1);
            
            List<Vector2> wavePoints = new List<Vector2>();
            for (int i = 0; i < points; i++) {
                Vector2 point = new Vector2();
                point.x = origin.x + amplitude * Mathf.Sin(frequency * (separation * i));
                point.y = origin.y + separation * i;

                point = Rotate(origin, point, rotation);

                wavePoints.Add(point);
            }            

            return wavePoints;
        }



        /// <summary>
        /// Returns points in a defined circle.
        /// </summary>
        /// <param name="origin">Center of the circle.</param>
        /// <param name="radius">Radius of the circle; the distance between its center and a point.</param>
        /// <param name="points">Number of points in the circle.</param>
        /// <returns></returns>
        public static List<Vector2> Circle(Vector2 origin, float radius, int points) {
            return Circle(origin, radius, points, 0f);
        }

        /// <summary>
        /// Returns points in a defined circle.
        /// </summary>
        /// <param name="origin">Center of the circle.</param>
        /// <param name="radius">Radius of the circle; the distance between its center and a point.</param>
        /// <param name="points">Number of points in the circle.</param>
        /// <param name="rotation">Rotation of the circle; change the position of the first point.</param>
        /// <returns></returns>
        public static List<Vector2> Circle(Vector2 origin, float radius, int points, float rotation) {
            return Arc(origin, 360f, radius, points, rotation);
        }


        /// <summary>
        /// Returns points in a defined ellipse.
        /// </summary>
        /// <param name="origin">Center of the ellipse.</param>
        /// <param name="horizontalRadius">Horizontal radius of the ellipse; maximum distance between its center and a point horizontally.</param>
        /// <param name="verticalRadius">Vertical radius of the ellipse; maximum distance between its center and a point vertically.</param>
        /// <param name="points">Number of points in the ellipse.</param>
        /// <param name="rotation">Rotation of the ellipse; change the position of the first point.</param>
        /// <returns></returns>
        public static List<Vector2> Ellipse(Vector2 origin, float horizontalRadius, float verticalRadius, int points, float rotation) {
            return Arc(origin, 360f, horizontalRadius, verticalRadius, points, rotation);
        }

        /// <summary>
        /// Returns points in a defined ellipse.
        /// </summary>
        /// <param name="origin">Center of the ellipse.</param>
        /// <param name="horizontalRadiusVector">Vector from origin to the horizontal radius. Defines the length of the horizontal radius.</param>
        /// <param name="verticalRadiusVector">Vector from origin to the vertical radius. Defines the length of the vertical radius.</param>
        /// <param name="points">Number of points in the ellipse.</param>
        /// <param name="rotation">Rotation of the ellipse; change the position of the first point.</param>
        /// <returns></returns>
        public static List<Vector2> Ellipse(Vector2 origin, Vector2 horizontalRadiusVector, Vector2 verticalRadiusVector, int points, float rotation) {
            return Arc(origin, 360f, horizontalRadiusVector, verticalRadiusVector, points, rotation);
        }
        

        /// <summary>
        /// Returns points in a defined arc.
        /// </summary>
        /// <param name="origin">Center from which the arc is drawn.</param>
        /// <param name="amplitude">Angle, in degrees, covered by the arc. 360 is a full circle.</param>
        /// <param name="radius">Radius of the arc; the distance between its center and a point.</param>
        /// <param name="points">Number of points in the arc.</param>
        /// <param name="rotation">Rotation of the circle; change the position of the first point.</param>
        /// <returns></returns>
        public static List<Vector2> Arc(Vector2 origin, float amplitude, float radius, int points, float rotation) {
            return Arc(origin, amplitude, radius, radius, points, rotation);
        }

        /// <summary>
        /// Returns points in a defined arc.
        /// </summary>
        /// <param name="origin">Center from which the arc is drawn.</param>
        /// <param name="horizontalRadiusVector">Vector from origin to the horizontal radius. Defines the length of the horizontal radius.</param>
        /// <param name="verticalRadiusVector">Vector from origin to the vertical radius. Defines the length of the vertical radius.</param>
        /// <param name="amplitude">Angle, in degrees, covered by the arc. 360 is a full circle.</param>
        /// <param name="radius">Radius of the arc; the distance between its center and a point.</param>
        /// <param name="points">Number of points in the arc.</param>
        /// <param name="rotation">Rotation of the circle; change the position of the first point.</param>
        /// <returns></returns>
        public static List<Vector2> Arc(Vector2 origin, float amplitude, Vector2 horizontalRadiusVector, Vector2 verticalRadiusVector, int points, float rotation) {
            float horizontal = (horizontalRadiusVector - origin).magnitude;
            float vertical = (verticalRadiusVector - origin).magnitude;
            return Arc(origin, amplitude, horizontal, vertical, points, rotation);
        }

        /// <summary>
        /// Returns points in a defined arc.
        /// </summary>
        /// <param name="origin">Center from which the arc is drawn.</param>
        /// <param name="horizontalRadius">Horizontal radius of an elliptic arc.</param>
        /// <param name="verticalRadius">Vertical radius of an elliptic arc.</param>
        /// <param name="amplitude">Angle, in degrees, covered by the arc. 360 is a full circle.</param>
        /// <param name="radius">Radius of the arc; the distance between its center and a point.</param>
        /// <param name="points">Number of points in the arc.</param>
        /// <param name="rotation">Rotation of the circle; change the position of the first point.</param>
        /// <returns></returns>
        public static List<Vector2> Arc(Vector2 origin, float amplitude, float horizontalRadius, float verticalRadius, int points, float rotation) {
            if (horizontalRadius <= 0f || verticalRadius <= 0f) {
                return null;
            }

            float amplitudeAngle = ClampAngle(amplitude);

            float angleBetweenPoints = 0f;
            if (amplitude < 360) {
                angleBetweenPoints = amplitude / (points - 1);
            } else {
                angleBetweenPoints = amplitude / points;
            }

            List<Vector2> circle = new List<Vector2>();
            for (int i = 0; i < points; i++) {
                float separationAngle = rotation + i * angleBetweenPoints;
                Vector2 point = GetPoint(origin, horizontalRadius, verticalRadius, separationAngle);
                circle.Add(point);
            }

            return circle;
        }




        /// <summary>
        /// Returns points in a defined spiral.
        /// </summary>
        /// <param name="origin">Center position of the spiral.</param>
        /// <param name="innerRadius">Inner radius of the spiral; the distance between the center and this position represents the distance to the first point of the spiral.</param>
        /// <param name="end">End position of the spiral; represents the outer radius of the spiral.</param>
        /// <param name="turns">Number of times the spiral turns in itself.</param>
        /// <param name="points">Number of points in the spiral.</param>
        /// <returns></returns>
        public static List<Vector2> Spiral(Vector2 origin, Vector2 innerRadius, Vector2 end, int turns, int points) {
            float radius = (innerRadius - origin).magnitude;
            return Spiral(origin, end, radius, turns, points);
        }

        /// <summary>
        /// Returns points in a defined spiral.
        /// </summary>
        /// <param name="origin">Center position of the spiral.</param>
        /// <param name="end">End position of the spiral; represents the outer radius of the spiral.</param>
        /// <param name="innerRadius">Inner radius of the spiral; the distance between the center and the first point of the spiral.</param>
        /// <param name="turns">Number of times the spiral turns in itself.</param>
        /// <param name="points">Number of points in the spiral.</param>
        /// <returns></returns>
        public static List<Vector2> Spiral(Vector2 origin, Vector2 end, float innerRadius, int turns, int points) {
            float extent = (end - origin).magnitude;
            float rotation = -Angle(origin, origin + Vector2.right, end);
            return Spiral(origin, extent, innerRadius, turns, points, rotation);
        }

        /// <summary>
        /// Returns points in a defined spiral.
        /// </summary>
        /// <param name="origin">Center position of the spiral.</param>
        /// <param name="extent">Outer radius of the spiral; the distance from the center to the last point of the spiral.</param>
        /// <param name="innerRadius">Inner radius of the spiral; the distance between the center and the first point of the spiral.</param>
        /// <param name="turns">Number of times the spiral turns in itself.</param>
        /// <param name="points">Number of points in the spiral.</param>
        /// <param name="rotation">Rotation of the spiral; change the direction in which it points.</param>
        /// <returns></returns>
        public static List<Vector2> Spiral(Vector3 origin, float extent, float innerRadius, int turns, int points, float rotation) {
            if (points <= 1 || innerRadius < 0) {
                return null;
            }

            float turnSeparation = (extent - innerRadius) / (points - 1);
            float angleBetweenPoints = turns * 360 / (points - 1);

            List<Vector2> spiral = new List<Vector2>();
            for (int i = 0; i < points; i++) {
                float separationAngle = rotation + i * angleBetweenPoints;
                Vector2 point = GetPoint(origin, innerRadius + turnSeparation * i, separationAngle);
                spiral.Add(point);
            }

            return spiral;
        }


        /// <summary>
        /// Returns a path of zigzaguing points.
        /// </summary>
        /// <param name="origin">Origin of the path.</param>
        /// <param name="end">End point of the path.</param>
        /// <param name="deviation">Deviation of the path center from the origin. Values on range [-1, 1]: first point, center, first turning point.</param>
        /// <param name="amplitude">Distance between the two sides of the zigzag.</param>
        /// <param name="turns">Number of turning points of the path.</param>
        /// <param name="subdivisions">Number of divisions of the vector represented by two turning points.</param>
        /// <param name="rotation">Rotation of the path around the vector formed by the origin and the end.</param>
        /// <returns></returns>        
        public static List<Vector3> Zigzag(Vector3 origin, Vector3 end, int deviation, float amplitude, int turns, int subdivisions, float rotation) {
            Vector3 direction = end - origin;
            float extent = direction.magnitude;
            return Zigzag(origin, direction, deviation, extent, amplitude, turns, subdivisions, rotation);
        }

        /// <summary>
        /// Returns a path of zigzaguing points.
        /// </summary>
        /// <param name="origin">Origin of the path.</param>
        /// <param name="direction">Direction in which the path extents.</param>
        /// <param name="deviation">Deviation of the path center from the origin. Values on range [-1, 1]: first point, center, first turning point.</param>
        /// <param name="extent">Length of the path.</param>
        /// <param name="amplitude">Distance between the two sides of the zigzag.</param>
        /// <param name="turns">Number of turning points of the path.</param>
        /// <param name="subdivisions">Number of divisions of the vector represented by two turning points.</param>
        /// <param name="rotation">Rotation of the path around the direction vector.</param>
        /// <returns></returns>
        public static List<Vector3> Zigzag(Vector3 origin, Vector3 direction, int deviation, float extent, float amplitude, int turns, int subdivisions, float rotation) {
            
            GeometricalPlane plane = new GeometricalPlane(direction);

            float width = amplitude / 2;
            float span = extent / (turns - 1);

            float horizontalDeviation = width * Mathf.Clamp(deviation, -1, 1);

            float vectorDirection = 1;

            List<Vector3> vectors = new List<Vector3>();
            for (int i = 0; i < turns; i++) {
                Vector3 point = new Vector3();
                point.x = vectorDirection * width + horizontalDeviation;
                point.y = 0f;
                point.z = span * i;

                point = plane.Rotate(point, rotation, GeometricalPlane.Axis.N);
                point = origin + plane.TransformPoint(point);

                vectors.Add(point);
                vectorDirection = -vectorDirection;
            }

            if (subdivisions <= 1) {
                return vectors;
            }

            List<Vector3> points = new List<Vector3>();
            for (int i = 1; i < vectors.Count; i++) {
                List<Vector3> dividedVector = Subdivide(vectors[i - 1], vectors[i], subdivisions);
                points.AddRange(dividedVector);
            }

            points = points.Distinct().ToList();

            return points;
        }

        /// <summary>
        /// Returns points in a defined wave.
        /// </summary>
        /// <param name="origin">Origin of the wave.</param>
        /// <param name="end">End point of the wave.</param>
        /// <param name="amplitude">Amplitude of the wave; the distance between opposing peaks.</param>
        /// <param name="frequency">Frecuency of periods in the wave.</param>
        /// <param name="points">Points to draw accross the wave.</param>
        /// <param name="rotation">Rotation of the wave around the direction vector.</param>
        /// <returns></returns>
        public static List<Vector3> Wave(Vector3 origin, Vector3 end, float amplitude, float frequency, int points, float rotation) {
            Vector3 direction = end - origin;
            float extent = direction.magnitude;
            return Wave(origin, direction, extent, amplitude, frequency, points, rotation);
        }

        
        /// <summary>
        /// Returns points in a defined wave.
        /// </summary>
        /// <param name="origin">Origin of the wave.</param>
        /// <param name="direction">Direction in which the wave spans.</param>
        /// <param name="extent">Length of th wave.</param>
        /// <param name="amplitude">Amplitude of the wave; the distance between opposing peaks.</param>
        /// <param name="frequency">Frecuency of periods in the wave.</param>
        /// <param name="points">Points to draw accross the wave.</param>
        /// <param name="rotation">Rotation of the wave around the direction vector.</param>
        /// <returns></returns>
        public static List<Vector3> Wave(Vector3 origin, Vector3 direction, float extent, float amplitude, float frequency, int points, float rotation) {

            GeometricalPlane plane = new GeometricalPlane(direction);
            float separation = extent / (points - 1);

            List<Vector3> wavePoints = new List<Vector3>();
            for (int i = 0; i < points; i++) {
                Vector3 point = new Vector3();
                point.x = amplitude * Mathf.Sin(frequency * (separation * i));
                point.y = 0f;
                point.z = separation * i;

                point = plane.Rotate(point, rotation, GeometricalPlane.Axis.N);
                point = origin + plane.TransformPoint(point);

                wavePoints.Add(point);
            }

            return wavePoints;
        }
        
        /// <summary>
        /// Returns points in a defined 3D circle.
        /// </summary>
        /// <param name="origin">Center of the circle.</param>
        /// <param name="normal">Normal vector of the circle; the direction in which it is pointing.</param>
        /// <param name="radius">Radius of the circle; the distance between its center and a point.</param>
        /// <param name="points">Number of points in the circle.</param>
        /// <returns></returns>
        public static List<Vector3> Circle(Vector3 origin, Vector3 normal, float radius, int points) {
            return Circle(origin, normal, radius, points, 0f);
        }
        
        /// <summary>
        /// Returns points in a defined 3D circle.
        /// </summary>
        /// <param name="origin">Center of the circle.</param>
        /// <param name="normal">Normal vector of the circle; the direction in which it is pointing.</param>
        /// <param name="radius">Radius of the circle; the distance between its center and a point.</param>
        /// <param name="points">Number of points in the circle.</param>
        /// <param name="rotation">Rotation of the circle; change the position of the first point.</param>
        /// <returns></returns>
        public static List<Vector3> Circle(Vector3 origin, Vector3 normal, float radius, int points, float rotation) {
            return Arc(origin, normal, 360f, radius, points, rotation);
        }

        /// <summary>
        /// Returns points in a defined arc.
        /// </summary>
        /// <param name="origin">Center from which the arc is drawn.</param>
        /// <param name="normal">Normal vector of the arc; the direction in which it is pointing.</param>
        /// <param name="amplitude">Angle, in degrees, covered by the arc. 360 is a full circle.</param>
        /// <param name="radius">Radius of the arc; the distance between its center and a point.</param>
        /// <param name="points">Number of points in the arc.</param>
        /// <param name="rotation">Rotation of the circle; change the position of the first point.</param>
        /// <returns></returns>
        public static List<Vector3> Arc(Vector3 origin, Vector3 normal, float amplitude, float radius, int points, float rotation) {
            return Arc(origin, normal, amplitude, radius, radius, points, rotation);
        }

        /// <summary>
        /// Returns points in a defined arc.
        /// </summary>
        /// <param name="origin">Center from which the arc is drawn.</param>
        /// <param name="normal">Normal vector of the arc; the direction in which it is pointing.</param>
        /// <param name="amplitude">Angle, in degrees, covered by the arc. 360 is a full circle.</param>
        /// <param name="horizontalRadius">Horizontal radius of an elliptic arc.</param>
        /// <param name="verticalRadius">Vertical radius of an elliptic arc.</param>
        /// <param name="radius">Radius of the arc; the distance between its center and a point.</param>
        /// <param name="points">Number of points in the arc.</param>
        /// <param name="rotation">Rotation of the circle; change the position of the first point.</param>
        /// <returns></returns>
        public static List<Vector3> Arc(Vector3 origin, Vector3 normal, float amplitude, float horizontalRadius, float verticalRadius, int points, float rotation) {
            if (horizontalRadius <= 0f || verticalRadius <= 0f) {
                return null;
            }

            Vector3 arcNormal = normal - origin;
            GeometricalPlane plane = new GeometricalPlane(arcNormal);

            float amplitudeAngle = ClampAngle(amplitude);

            float angleBetweenPoints = 0f;
            if (amplitude < 360) {
                angleBetweenPoints = amplitude / (points - 1);
            } else {
                angleBetweenPoints = amplitude / points;
            }

            List<Vector3> circle = new List<Vector3>();
            for (int i = 0; i < points; i++) {
                float separationAngle = rotation + i * angleBetweenPoints;
                Vector3 point = plane.GetPoint(origin, horizontalRadius, verticalRadius, separationAngle);
                circle.Add(point);
            }

            return circle;
        }




        /// <summary>
        /// Returns points in a defined spiral.
        /// </summary>
        /// <param name="origin">Center position of the spiral.</param>
        /// <param name="end">End point of the spiral. Defines both the extent and direction of the spiral.</param>
        /// <param name="innerRadius">Inner radius of the spiral; the distance between the center and the first point of the spiral.</param>
        /// <param name="outerRadius">Outer radius of the spiral; the distance from the center to the last point of the spiral.</param> 
        /// <param name="turns">Number of times the spiral turns in itself.</param>
        /// <param name="points">Number of points in the spiral.</param>
        /// <param name="rotation">Rotation of the spiral along the vector marked by the origin and the end.</param>
        /// <returns></returns>
        public static List<Vector3> Spiral(Vector3 origin, Vector3 end, float innerRadius, float outerRadius, int turns, int points, float rotation) {
            Vector3 direction = end - origin;
            float extent = direction.magnitude;
            return Spiral(origin, direction, extent, innerRadius, outerRadius, turns, points, rotation);
        }

        /// <summary>
        /// Returns points in a defined spiral.
        /// </summary>
        /// <param name="origin">Center position of the spiral.</param>
        /// <param name="direction">Direction in which the spiral spans.</param>
        /// <param name="extent">Length of the spiral along the direction.</param>
        /// <param name="innerRadius">Inner radius of the spiral; the distance between the center and the first point of the spiral.</param>
        /// <param name="outerRadius">Outer radius of the spiral; the distance from the center to the last point of the spiral.</param> 
        /// <param name="turns">Number of times the spiral turns in itself.</param>
        /// <param name="points">Number of points in the spiral.</param>
        /// <param name="rotation">Rotation of the spiral along the direction.</param>
        /// <returns></returns>
        public static List<Vector3> Spiral(Vector3 origin, Vector3 direction, float extent, float innerRadius, float outerRadius, int turns, int points, float rotation) {
            if (points <= 1 || innerRadius < 0) {
                return null;
            }

            Vector3 circleNormal = direction - origin;
            GeometricalPlane plane = new GeometricalPlane(direction);

            float width = (outerRadius - innerRadius) / (points - 1);
            float height = extent / (points - 1);
            float angleBetweenPoints = turns * 360 / (points - 1);

            List<Vector3> spiral = new List<Vector3>();
            for (int i = 0; i < points; i++) {
                float separationAngle = rotation + i * angleBetweenPoints;

                Vector3 originDeviation = GetPoint(origin, origin + direction, height * i);

                Vector3 point = plane.GetPoint(originDeviation, innerRadius + width * i, separationAngle);
                spiral.Add(point);
            }

            return spiral;
        }
    } 
