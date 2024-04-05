using System.Text;

namespace Optimal_Control_Lab3
{
    public class VN
    {
        double[] coordinates;

        public VN(double[] coordinates)
        {
            this.coordinates = coordinates;
        }

        public VN(int dimension, Random random)
        {
            this.coordinates = new double[dimension];
            for (int i = 0; i < dimension; i++)
                this.coordinates[i] = -1.0 + 2.0 * random.NextDouble();
        }

        public int Dimension => coordinates.Length;

        public double At(int i)
        {
            return coordinates[i];
        }

        public VN Normalize()
        {
            double module = Math.Sqrt(coordinates.Select(x => x * x).Sum());

            double[] normalizedCoordinates = new double[coordinates.Length];
            for (int i = 0; i <  coordinates.Length; i++)
                normalizedCoordinates[i] = coordinates[i] / module;

            return new VN(normalizedCoordinates);
        }

        public VN MultiplyOnScalar(double scalar)
        {
            double[] multipliedCoordinates = new double[coordinates.Length];

            for (int i = 0; i < coordinates.Length; i++)
                multipliedCoordinates[i] = coordinates[i] * scalar;

            return new VN(multipliedCoordinates);
        }

        public VN Add(VN other)
        {
            if (this.Dimension != other.Dimension)
                throw new Exception();

            double[] sumCoordinates = new double[coordinates.Length];
            for (int i = 0; i < coordinates.Length; i++)
                sumCoordinates[i] = coordinates[i] + other.At(i);

            return new VN(sumCoordinates);
        }

        public override string? ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("VN ");
            stringBuilder.Append(this.GetHashCode());
            stringBuilder.Append(": dim = ");
            stringBuilder.Append(coordinates.Length);
            stringBuilder.Append("; coords = | ");
            for (int i = 0; i < coordinates.Length; i++)
                stringBuilder.Append(String.Format("{0} |", coordinates[i].ToString("0.####")));
            return stringBuilder.ToString();
        }
    }
}
