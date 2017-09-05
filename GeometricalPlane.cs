/* 
 * This class represents a geometrical plane in 3D space. Its functions
 * may create, rotate and transform points from World Space to Plane Space
 * and viceversa.
 *
 * @author  Olsyx (Olatz Castaño)
 * @source https://github.com/Darkatom/Hedra-Library
 * @since   Unity 2017.1.0p4
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hedras.Objects { 

    /// <summary>
    /// A 3D plane represented by its two axis I and J, and its normal vector.
    /// </summary>
    public class GeometricalPlane {
        public enum Axis {
            I, J, N
        }

        Vector3 I;
        Vector3 J;
        Vector3 N;

        public Vector3 IAxis {
            get {
                return I;
            }
        }
        public Vector3 JAxis {
            get {
                return J;
            }
        }
        public Vector3 NAxis {
            get {
                return N;
            }
        }
        
        float[][] m_planeToWorld;
        float[][] m_worldToPlane;

        public GeometricalPlane(Vector3 i, Vector3 j) {
            I = i.normalized;
            J = j.normalized;
            N = Vector3.Cross(I, J).normalized;

            GenerateTransformMatrixes();
        }

        public GeometricalPlane(Vector3 normal) {
            this.N = normal.normalized;

            Vector3 normalPerpendicular = Vector3.up;
            if (normal.z == 0f) {
                normalPerpendicular = new Vector3(this.N.y, -this.N.x, 0f);
            } else if (normal.x == 0f) {
                normalPerpendicular = new Vector3(0f, this.N.z, -this.N.y);
            } else { 
                normalPerpendicular = new Vector3(this.N.z, 0f, -this.N.x);
            }

            I = normalPerpendicular.normalized;
            J = Vector3.Cross(normal, I).normalized;

            GenerateTransformMatrixes();
        }
        
        void GenerateTransformMatrixes() {
            m_planeToWorld = new float[3][];
            m_planeToWorld[0] = new float[3];
            m_planeToWorld[0][0] = Vector3.Dot(Vector3.right, I);
            m_planeToWorld[0][1] = Vector3.Dot(Vector3.right, J);
            m_planeToWorld[0][2] = Vector3.Dot(Vector3.right, N);

            m_planeToWorld[1] = new float[3];
            m_planeToWorld[1][0] = Vector3.Dot(Vector3.up, I);
            m_planeToWorld[1][1] = Vector3.Dot(Vector3.up, J);
            m_planeToWorld[1][2] = Vector3.Dot(Vector3.up, N);

            m_planeToWorld[2] = new float[3];
            m_planeToWorld[2][0] = Vector3.Dot(Vector3.forward, I);
            m_planeToWorld[2][1] = Vector3.Dot(Vector3.forward, J);
            m_planeToWorld[2][2] = Vector3.Dot(Vector3.forward, N);


            m_worldToPlane = new float[3][];
            m_worldToPlane[0] = new float[3];
            m_worldToPlane[0][0] = Vector3.Dot(I, Vector3.right);
            m_worldToPlane[0][1] = Vector3.Dot(I, Vector3.up);
            m_worldToPlane[0][2] = Vector3.Dot(I, Vector3.forward);

            m_worldToPlane[1] = new float[3];
            m_worldToPlane[1][0] = Vector3.Dot(J, Vector3.right);
            m_worldToPlane[1][1] = Vector3.Dot(J, Vector3.up);
            m_worldToPlane[1][2] = Vector3.Dot(J, Vector3.forward);

            m_worldToPlane[2] = new float[3];
            m_worldToPlane[2][0] = Vector3.Dot(N, Vector3.right);
            m_worldToPlane[2][1] = Vector3.Dot(N, Vector3.up);
            m_worldToPlane[2][2] = Vector3.Dot(N, Vector3.forward);
        }
        
        /// <summary>
        /// Returns a point at given degrees and distance from an origin.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public Vector3 GetPoint(Vector3 origin, float radius, float degrees) {
            float angle = degrees * Mathf.Deg2Rad;
            Vector3 point = origin + radius * (I * Mathf.Sin(angle) + J * Mathf.Cos(angle));
            return point;
        }

        /// <summary>
        /// Returns a point at given degrees and distance from an origin.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public Vector3 GetPoint(Vector3 origin, float horizontalRadius, float verticalRadius, float degrees) {
            float angle = degrees * Mathf.Deg2Rad;
            Vector3 point = origin + (horizontalRadius * I * Mathf.Sin(angle) + verticalRadius * J * Mathf.Cos(angle));
            return point;
        }

        /// <summary>
        /// Rotates a point around an axis of the plane.
        /// </summary>
        /// <param name="point">Point to rotate.</param>
        /// <param name="degrees">Angle of the rotation.</param>
        /// <param name="pivotAxis">Axis of the plane that acts as pivot.</param>
        /// <returns></returns>
        public Vector3 Rotate(Vector3 point, float degrees, Axis pivotAxis) {
            float angle = degrees * Mathf.Deg2Rad;

            Vector3 rotatedPoint = point;

            if (pivotAxis == Axis.I) {
                rotatedPoint.y = point.y * Mathf.Cos(angle) - point.z * Mathf.Sin(angle);
                rotatedPoint.z = point.y * Mathf.Sin(angle) + point.z * Mathf.Cos(angle);
            }
            
            if (pivotAxis == Axis.J) {
                rotatedPoint.x = point.x * Mathf.Cos(angle) + point.z * Mathf.Sin(angle);
                rotatedPoint.z = - point.x * Mathf.Sin(angle) + point.z * Mathf.Cos(angle);
            }

            if (pivotAxis == Axis.N) {
                rotatedPoint.x = point.x * Mathf.Cos(angle) - point.y * Mathf.Sin(angle);
                rotatedPoint.y = point.x * Mathf.Sin(angle) + point.y * Mathf.Cos(angle);
            }

            return rotatedPoint;
        }

        /// <summary>
        /// Transforms point from plane coordinates to world coordinates.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3 TransformPoint(Vector3 point) {
            return TransformPoint(point.x, point.y, point.z);
        }

        /// <summary>
        /// Transforms plane coordinates to world coordinates.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3 TransformPoint(float x, float y, float z) {
            Vector3 worldPoint = new Vector3();
            worldPoint.x = (x * m_planeToWorld[0][0]) + (y * m_planeToWorld[0][1]) + (z * m_planeToWorld[0][2]);
            worldPoint.y = (x * m_planeToWorld[1][0]) + (y * m_planeToWorld[1][1]) + (z * m_planeToWorld[1][2]);
            worldPoint.z = (x * m_planeToWorld[2][0]) + (y * m_planeToWorld[2][1]) + (z * m_planeToWorld[2][2]);
            return worldPoint;
        }
    }

}
