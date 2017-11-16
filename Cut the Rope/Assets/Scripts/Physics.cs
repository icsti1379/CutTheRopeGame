using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Custom
{
    public static class Physics
    {
        public struct Pulse
        {
            public Vector3 Direction;
            public float Energy;
        }

        /// <summary>
        /// Gets the gravitational force between two MassObjects.
        /// </summary>
        /// <returns>The gravitational force.</returns>
        public static float GetGravitationalForce(float massA, double massB, float Distance, float gravitationalConstant)
        {
            return gravitationalConstant * (float)((massA * massB) / Mathf.Pow(Distance, 2));
        }

        /// <summary>
        /// Calculates the outcome of a Collision between two MassObjects.
        /// </summary>
        /// <returns>The rotational pulse.</returns>
        public static Pulse CalculatePulse(Weight weight, ContactPoint2D contactPoint)
        {
            Pulse result = new Pulse();

            float m = weight.Mass;

            // _________________________________________
            //
            // -----------------------------------------
            // Errechnung der Rotationsmatrix.
            // -----------------------------------------
            // _________________________________________

            Vector3[] M;        // Orientierung des Körpers.

            Vector3[] M_inverse;

            M = Math.GetRotationMatrix(weight.transform.rotation);

            M_inverse = Math.TransposeMatrix(M);

            // _________________________________________
            //
            // -----------------------------------------
            // Errechnung des Trägheitstensors.
            // -----------------------------------------
            // _________________________________________

            Vector3[] I;

            I = Math.GetTensorOfSphere(weight.Radius, weight.Mass);

            Vector4 column0 = new Vector4(I[0].x, I[0].y, I[0].z, 0);
            Vector4 column1 = new Vector4(I[1].x, I[1].y, I[1].z, 0);
            Vector4 column2 = new Vector4(I[2].x, I[2].y, I[2].z, 0);
            Vector4 column3 = new Vector4(0, 0, 0, 1);

            Matrix4x4 I_4 = new Matrix4x4(column0, column1, column2, column3);
            Matrix4x4 I_4_Inverse = Matrix4x4.Inverse(I_4);

            Vector3 column03 = new Vector3(I_4_Inverse.m00, 0, 0);
            Vector3 column13 = new Vector3(0, I_4_Inverse.m11, 0);
            Vector3 column23 = new Vector3(0, 0, I_4_Inverse.m22);

            Vector3[] I_inverse = new Vector3[3] { column03, column13, column23 };


            // _________________________________________
            //
            // -----------------------------------------
            // Erstellung des Kontaktkoordinatensystems.
            // -----------------------------------------
            // _________________________________________

            Vector3 n = contactPoint.normal; // Kontaktnormale.

            Vector3 xAchse = n;
            Vector3 errateneAchse;
            Vector3 zAchse;
            Vector3 yAchse;

            do
            {
                errateneAchse = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                zAchse = Vector3.Cross(xAchse, errateneAchse);
                yAchse = Vector3.Cross(xAchse, zAchse);
            }
            while (yAchse == Vector3.zero || zAchse == Vector3.zero);

            // _________________________________________
            //
            // -----------------------------------------
            // Erstellung der Basismatrix.
            // -----------------------------------------
            // _________________________________________

            Vector3[] M_basis = new Vector3[3];
            M_basis[0] = xAchse;
            M_basis[1] = yAchse.normalized;
            M_basis[2] = zAchse.normalized;

            Vector3[] M_basis_inverse = Math.TransposeMatrix(M_basis);

            // _________________________________________
            //
            // -----------------------------------------
            // Errechnung des impulsiven Drehmoments.
            // -----------------------------------------
            // _________________________________________

            Vector3 qRel;   // Relativer Kontaktpunkt.
            Vector3 q;      // Ortsvektor des Kontaktpunktes.
            Vector3 p;      // Ortsvektor des Objektschwerpunktes.

            q = Math.VectorMatrixProduct(contactPoint.point, M_basis_inverse);
            p = Math.VectorMatrixProduct(weight.transform.position, M_basis_inverse);

            qRel = q - p;

            // -----------------------------------------

            Vector3 u;      // Rotationsimpuls.
            Vector3 j;      // Wirkender linearer Impuls. [Nicht zu verwechseln mit J (Gesamtimpuls!)]

            j = weight.Energy * weight.Direction;
            u = Vector3.Cross(qRel, j);

            // -----------------------------------------

            Vector3 u_contact;  // Rotationsimpuls in Richtung der Kontaktnormalen.
            float j_skalar;     // Wirkender linearer Impuls in Richtung der Kontaktnormalen.

            float c = j.magnitude;
            float b = Math.GetOrthoToAxis(Vector3.zero, n, j);

            j_skalar = Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(b, 2));
            u_contact = Vector3.Cross(qRel, j_skalar * n);

            // -----------------------------------------

            Vector3 O;      // Rotationsgeschwindigkeit, O = M * I^-1 * M^-1 * u_contact.

            Vector3[] I_inverse_Basiswechsel = Math.MatrixMatrixProduct(Math.MatrixMatrixProduct(I_inverse, M_basis_inverse), M_basis);

            O = Math.VectorMatrixProduct(u_contact, I_inverse_Basiswechsel);


            // -----------------------------------------

            Vector3 v_rot;  // lineare Rotationsgeschwindigkeit.

            v_rot = Vector3.Cross(O, qRel);

            Vector3 rotationsAchse = Vector3.Cross(contactPoint.point, (Vector3)contactPoint.point + j_skalar * n).normalized;

            // -----------------------------------------

            float delta_v; // Geschwindigkeitsänderung in Richtung der Kontaktnormalen. Δv = j * 1 / m + n ((I^−1 (qrel × jn)) × qrel)

            delta_v = j_skalar * 1 / m + Vector3.Dot(n, v_rot);

            // -----------------------------------------

            float J;      // Gesamtimpuls. [Nicht zu verwechseln mit j (Wirkender linearer Impuls!)]

            J = delta_v / (1 / m + Vector3.Dot(n, Vector3.Cross(Math.VectorMatrixProduct(Vector3.Cross(qRel, n), I_inverse), qRel)));

            result.Direction = n;
            result.Energy = J;

            return result;
        }

        public static float GetEnergy(float speed, float mass)
        {
            return 0.5f * mass * Mathf.Pow(speed, 2);
        }

        public static float GetSpeed(float energy, float mass)
        {
            return Mathf.Sqrt(energy / (0.5f * mass));
        }
    }
}
