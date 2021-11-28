using System;
using System.Collections.Generic;
using System.Linq;
using static System.MathF;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;

namespace ConsoleApp3
{
    internal static class Static
    {
        public static EulerAngles ToEulerAngles(this System.Numerics.Matrix4x4 m)
        {
            if (Abs(m.M13) != 1)
            {
                var theta1 = -Asin(m.M13);
                var theta2 = PI - theta1;
                var psi1 = Atan2(m.M23 / Cos(theta1), m.M33 / Cos(theta1));
                var psi2 = Atan2(m.M23 / Cos(theta2), m.M33 / Cos(theta2));
                var phi1 = Atan2(m.M12 / Cos(theta1), m.M11 / Cos(theta1));
                var phi2 = Atan2(m.M12 / Cos(theta2), m.M11 / Cos(theta2));
                return new EulerAngles(Angle.FromRadians(phi1), Angle.FromRadians(theta1), Angle.FromRadians(psi1));
            }
            else
            {
                var phi = 0;

                if (m.M13 == -1)
                {
                    var theta = PI / 2;
                    var psi = phi + Atan2(m.M21, m.M31);
                    return new EulerAngles(Angle.FromRadians(phi), Angle.FromRadians(theta), Angle.FromRadians(psi));
                }
                else
                {
                    var theta = -PI / 2;
                    var psi = -phi + Atan2(-m.M21, -m.M31);
                    return new EulerAngles(Angle.FromRadians(phi), Angle.FromRadians(theta), Angle.FromRadians(psi));
                }
            }
        }

        public static System.Numerics.Matrix4x4 ToRotationMatrix(this EulerAngles a)
        {
            return System.Numerics.Matrix4x4.CreateRotationZ((float)a.Alpha.Radians) * System.Numerics.Matrix4x4.CreateRotationY((float)a.Beta.Radians) * System.Numerics.Matrix4x4.CreateRotationX((float)a.Gamma.Radians);
        }
    }
}
