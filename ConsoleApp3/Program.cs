using System.Runtime.InteropServices;

namespace ConsoleApp3
{
    public class Program
    {
        [DllImport("TransformationEstimation")]
        private static extern void EstimateTransformation([In, Out] float[,] points, int count, float tolerance = 10.0f);

        public static void Main(string[] args)
        {
            //var src = new float[5, 3]
            //    {
            //        { -52.0f, 63.11f, 0.28f },
            //        { 60.000f, 10.042f, -5.530f },
            //        { 0, 0, 0 },
            //        { 20.000f, 80.042f, 2.530f },
            //        { -10.00f, -13.000f, -2.030f }
            //    };

            var src = new float[3, 3]
                {
                    { 64.044304f, 2693.0754f, -170.95724f },
                    { 4242.071f, 3637.872f, -194.96318f },
                    { 4917.587f, 1063.1492f, 863.1775f }
                };

            EstimateTransformation(src, src.GetLength(0));
            Console.ReadKey();
        }
    }
}