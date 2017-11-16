using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    public static class Math
    {
        /// <summary>
        /// Calculates a matrix-vector multiplication.
        /// Note: Only Vector3/3x3 Matrix with 3x3 Matrix [Vector3 array with 3 elements, each Vector3 represents a column] supported.
        /// </summary>
        public static Vector3 VectorMatrixProduct(Vector3 vector, Vector3[] matrix)
        {
            if (matrix.Length != 3)
                return Vector3.zero;

            Vector3 result = Vector3.zero;

            result += new Vector3(      matrix[0].x * vector.x + matrix[1].x * vector.y + matrix[2].x * vector.z, 0, 0);
            result += new Vector3(0,    matrix[0].y * vector.x + matrix[1].y * vector.y + matrix[2].y * vector.z, 0);
            result += new Vector3(0, 0, matrix[0].z * vector.x + matrix[1].z * vector.y + matrix[2].z * vector.z);

            return result;
        }

        /// <summary>
        /// Calculates a matrix-matrix multiplication.
        /// Note: Only Vector3/3x3 Matrix with 3x3 Matrix [Vector3 array with 3 elements, each Vector3 represents a column] supported.
        /// </summary>
        public static Vector3[] MatrixMatrixProduct(Vector3[] matrixA, Vector3[] matrixB)
        {
            if (matrixA.Length != 3 || matrixB.Length != 3)
            {
                return new Vector3[3];
                Debug.LogError("Error!");
            }

            Vector3[] result = new Vector3[3];

            for (int a = 0; a < 3; a++)
            {
                result[a] = new Vector3(        matrixA[0].x * matrixB[a].x + matrixA[1].x * matrixB[a].y + matrixA[2].x * matrixB[a].z, 0, 0);
                result[a] += new Vector3(0,     matrixA[0].y * matrixB[a].x + matrixA[1].y * matrixB[a].y + matrixA[2].y * matrixB[a].z, 0);
                result[a] += new Vector3(0, 0,  matrixA[0].z * matrixB[a].x + matrixA[1].z * matrixB[a].y + matrixA[2].z * matrixB[a].z);
            }

            return result;
        }

        /// <summary>
        /// Transposes a matrix. Only 3x3 Matrix supported [Vector3 array with 3 elements].
        /// </summary>
        public static Vector3[] TransposeMatrix(Vector3[] matrix)
        {
            Vector3[] result = new Vector3[3];

            if (matrix.Length != 3)
                return result;

            result[0] = new Vector3(matrix[0].x, matrix[1].x, matrix[2].x);
            result[1] = new Vector3(matrix[0].y, matrix[1].y, matrix[2].y);
            result[2] = new Vector3(matrix[0].z, matrix[1].z, matrix[2].z);

            return result;
        }

        /// <summary>
        /// Calculates the distance from a point to an axis.
        /// </summary>
        public static float GetOrthoToAxis(Vector3 origin, Vector3 n, Vector3 point)
        {
            Vector3 direction = n;
            Vector3 startingPoint = origin;

            Ray ray = new Ray(startingPoint, direction);
            float distance = Vector3.Cross(ray.direction, point - ray.origin).magnitude;

            return distance;
        }

        /// <summary>
        /// Converts a quaternion to its equivalent rotationmatrix.
        /// </summary>
        public static Vector3[] GetRotationMatrix(Quaternion q)
        {
            float a = q.x;
            float b = q.y;
            float c = q.z;
            float d = q.w;

            Vector3 firstColumn     = new Vector4(1f - 2f * (c * c + d * d), 2f * (b * c + a * d), 2f * (b * d - a * c)/*, 0*/);
            Vector3 secondColumn    = new Vector4(2f * (b * c - a * d), 1f - 2f * (d * d + b * b), 2f * (c * d + a * b)/*, 0*/);
            Vector3 thirdColumn     = new Vector4(2f * (b * d + a * c), 2f * (c * d - a * b), 1f - 2f * (b * b + c * c)/*, 0*/);
            //Vector3 fourthColumn    = new Vector4(0, 0, 0, 1);

            return new Vector3[3] { firstColumn, secondColumn, thirdColumn };
        }

        public static float GetVolumenOfSphere(float radius)
        {
            return Mathf.PI * (Mathf.Pow(radius * 2f, 3f) / 6f);
        }

        public static float GetRadiusOfSphere(float volumen)
        {
            return Mathf.Pow(volumen / (4f / 3f * Mathf.PI), 1f / 3f);
        }

        public static Vector3[] GetTensorOfCylinder(float radius, float height, float Mass)
        {
            Vector3[] result = new Vector3[3];

            float a = 1f / 2f * (Mathf.Pow(radius, 2) + Mathf.Pow(height, 2) / 3f);

            result[0] = new Vector3(a, 0, 0) * Mass / 2f;
            result[1] = new Vector3(0, Mathf.Pow(radius, 2), 0) * Mass / 2f;
            result[2] = new Vector3(0, 0, a) * Mass / 2f;

            return result;
        }

        public static Vector3[] GetTensorOfSphere(float radius, float Mass)
        {
            Vector3[] result = new Vector3[3];

            float a = 2f / 5f * Mass * Mathf.Pow(radius, 2f);
            Debug.Log("a is " + a);

            result[0] = new Vector3(a, 0, 0);
            result[1] = new Vector3(0, a, 0);
            result[2] = new Vector3(0, 0, a);

            return result;
        }

        public static Vector3[] GetTensorOfCube(float size, float Mass)
        {
            Vector3[] result = new Vector3[3];

            float a = 1f / 6f * Mass * Mathf.Pow(size, 2f);
            Debug.Log("a is " + a);

            result[0] = new Vector3(a, 0, 0);
            result[1] = new Vector3(0, a, 0);
            result[2] = new Vector3(0, 0, a);

            return result;
        }

        public static float GetRadian(float degree)
        {
            return (degree / 360f) * 2f * Mathf.PI;
        }
    }
}
