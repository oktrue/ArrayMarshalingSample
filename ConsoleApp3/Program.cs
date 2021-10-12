using System.Runtime.InteropServices;

namespace ConsoleApp3
{
    public class Program
    {
        [DllImport("TransformationEstimation")]
        private static extern IntPtr EstimateTransformation([In] float[,] points, int count, [Out] float[] transformation, float tolerance = 10.0f);

        public static void Main(string[] args)
        {
            var src = new float[3, 3]
                {
                    { 64.044304f, 2693.0754f, -170.95724f },
                    { 4242.071f, 3637.872f, -194.96318f },
                    { 4917.587f, 1063.1492f, 863.1775f }
                };

            var transformation = new float[6];

            EstimateTransformation(src, src.GetLength(0), transformation);

            Console.ReadKey();
        }
    }
}