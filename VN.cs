namespace Optimal_Control_Lab3
{
    public class VN
    {
        private double[] coordinates;

        public VN(double[] coordinates)
        {
            this.coordinates = coordinates;
        }

        public int Dimension => coordinates.Length;
        
        public double At(int i)
        {
            return coordinates[i];
        }

        public void Set(int i, double value)
        {
            coordinates[i] = value;
        }

        public VN MutateByNormalize()
        {
            var module = Math.Sqrt(coordinates.Select(c => c * c).Sum());
            for (int i = 0; i < coordinates.Length; i++)
                coordinates[i] /= module;
            return this;
        }

        public VN MutateByAdd(VN other)
        {
            if (coordinates.Length == other.coordinates.Length)
                for (int i = 0; i < coordinates.Length; i++)
                    coordinates[i] += other.coordinates[i];
            return this;
        }

        public VN MutateByMultiplyOnScalar(double k)
        {
            for (int i = 0; i < coordinates.Length; i++)
                coordinates[i] *= k;
            return this;
        }

        public VN DeepCopy()
        {
            double[] cloneCoordinates = new double[this.coordinates.Length];
            Array.Copy(this.coordinates, cloneCoordinates, this.coordinates.Length);
            return new VN(cloneCoordinates);
        }
    }
}
