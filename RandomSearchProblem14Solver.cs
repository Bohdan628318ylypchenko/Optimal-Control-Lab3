namespace Optimal_Control_Lab3
{
    public class RandomSearchProblem14Solver
    {
        public struct TX
        {
            public double t; public double x;
        }

        private Random random;

        public RandomSearchProblem14Solver(Random random)
        {
            this.random = random;
        }

        public TX[] Solve(double a, double aValue, double b, double bValue, int segmentCount, VN start,
                          double startStep, double minStep, int attemptCountBeforeStepDecrement, double stepCoefficient)
        {
            double dt = (b - a) / (double)segmentCount;

            TX[] result = new TX[segmentCount + 1];
            for (int i = 0; i <= segmentCount; i++)
                result[i].t = a + i * (b - a) / (double)segmentCount;

            double step = startStep;
            VN current = start;
            current.Set(0, aValue); current.Set(current.Dimension - 1, bValue);
            while (step > minStep)
            {
                int failedAttemptCount = 0;

                for (int i = 1; i <= attemptCountBeforeStepDecrement; i++)
                {
                    VN direction = _GenerateDirectionVector(segmentCount + 1).MutateByNormalize();
                    VN next = current.DeepCopy().MutateByAdd(direction.MutateByMultiplyOnScalar(step));

                    if (TargetFunction(next, dt) < TargetFunction(current, dt))
                    {
                        current = next;
                        break;
                    }
                    else
                    {
                        failedAttemptCount++;
                    }
                }

                if (failedAttemptCount >= attemptCountBeforeStepDecrement)
                    step *= stepCoefficient;
            }

            for (int i = 0; i < segmentCount + 1; i++)
                result[i].x = current.At(i);
            return result;
        }

        private VN _GenerateDirectionVector(int dimension)
        {
            double[] coordinates = new double[dimension];

            coordinates[0] = coordinates[coordinates.Length - 1] = 0;

            for (int i = 1; i < coordinates.Length - 1; i++)
                coordinates[i] = random.NextDouble();
            return new VN(coordinates);
        }

        public double TargetFunction(VN vn, double dt)
        {
            double result = 0;
            for (int i = 0; i < vn.Dimension - 1; i++)
                result += (Math.Pow((vn.At(i + 1) - vn.At(i)) / dt, 2.0) + 4.0 * Math.Pow(vn.At(i), 2.0)) * dt;
            return result;
        }
    }
}
