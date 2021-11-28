using System.Numerics;
using System.Runtime.InteropServices;

namespace ConsoleApp3
{
    public class Program
    {
        /// <summary>
        /// Преобразование трёх точек в матрицу поворота
        /// https://math.stackexchange.com/questions/1983054/using-3-points-create-new-coordinate-system-and-create-array-of-points-on-xy
        /// </summary>
        /// <param name="a">Задняя точка</param>
        /// <param name="b">Передняя точка</param>
        /// <param name="c">Верхняя точка</param>
        /// <returns>Матрица поворота</returns>
        public static Matrix4x4 ConvertThreePointsToRotationMatrix(Vector3 a, Vector3 b, Vector3 c)
        {
            // Базисный вектор X
            var d = Vector3.Normalize(b - a);
            var unitX = d / Vector3.Dot(d, d);

            // Базисный вектор Y
            var cross = Vector3.Cross(unitX, Vector3.Normalize(a - c));
            var unitY = cross / Vector3.Dot(cross, cross);

            // Базисный вектор Z
            var unitZ = Vector3.Cross(unitX, unitY);

            // Матрица поворота
            return new Matrix4x4(
                unitX.X, unitX.Y, unitX.Z, 0,
                unitY.X, unitY.Y, unitY.Z, 0,
                unitZ.X, unitZ.Y, unitZ.Z, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        /// Преобразование координат из локальной СК в глобальную
        /// </summary>
        private static Func<Vector3, float[], Vector3> TransformCoordinates = (coordinates, transformation) =>
        {
            var matrix =
            Matrix4x4.CreateRotationX(transformation[5] / 180 * MathF.PI) *
            Matrix4x4.CreateRotationY(transformation[4] / 180 * MathF.PI) *
            Matrix4x4.CreateRotationZ(transformation[3] / 180 * MathF.PI) *
            Matrix4x4.CreateTranslation(transformation[0], transformation[1], transformation[2]);

            var point = Matrix4x4.CreateTranslation(coordinates) * matrix;

            //return matrix;
            return point.Translation;
        };

        // Импортнутая либа для привязки
        // TODO: либы из папки AlignLibs кидаем в директорию с исполняемым файлом.
        [DllImport("TransformationEstimation")]
        private static extern void EstimateTransformation(float[,] src, int srcCount, float[,] tgt, int tgtCount, float[] transformation, float tolerance = 0.02f);

        public static void Main(string[] args)
        {
            // Измеренные координаты
            var src = new float[4, 3]
            {
                // { 2.6538, 2.2743, -0.2387 }, // 1
                { -1.3963f, 7.0468f, -0.1867f }, // 3
		        { -5.0561f, 10.6182f, -0.2651f }, // 7
		        { -8.3914f, 6.1820f, 0.0151f }, // 8
		        { -5.2040f, 3.1699f, 0.1021f }, // 4
            };

            // Исходные координаты 
            var tgt = new float[4, 3]
            {
                { -2.2395f, -5.5049f, -0.0456f }, // 3
		        { -3.8180f, -10.3650f, -0.0467f }, // 7
		        { 1.6457f, -11.3733f, 0.1453f }, // 8
		        { 2.9322f, -7.1756f, 0.1669f }, // 4
		        // { 3.4133, -0.7971, -0.2097 }, // 2
	        };

            // Полученная трансформация в формате X, Y, Z, A (deg), B (deg), C (deg)
            var transformation = new float[6];

            // Вызываем импортнутый метод либы с допуском tolerance = 0.02f и получаем в transformation 6dof.
            EstimateTransformation(src, src.GetLength(0), tgt, tgt.GetLength(0), transformation);

            // Здесь получается точка 3 с правого тахеометра в СК левого тахеометра, то есть transformedPt = tgt[0] чтд.
            var transformedPt = TransformCoordinates(new Vector3(src[0, 0], src[0, 1], src[0, 2]), transformation);

            // Тесты
            var p1 = new Vector3(-1, -1, 0);
            var p2 = new Vector3(1, 1, 0); 
            var p3 = new Vector3(1, -1, MathF.Sqrt(2));

            var r = ConvertThreePointsToRotationMatrix(p1, p2, p3);

            var euler = r.ToEulerAngles();

            // Поворот X aka крен
            var theta = euler.Gamma.Degrees;

            // Поворот Y aka дифферент
            var psi = euler.Beta.Degrees;

            // Поворот Z aka рыскание
            var phi = euler.Alpha.Degrees;

            Console.WriteLine();
            Console.WriteLine($"Рыскание: {phi:0.000}; Дифферент: {psi:0.000}; Крен: {theta:0.000}");

            Console.ReadKey();
        }
    }
}