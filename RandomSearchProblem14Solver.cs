namespace Optimal_Control_Lab3
{
    public static class RandomSearchProblem14Solver
    {
        public static (double, double[], double[], VN) Solve(double a, double aValue, double b, double bValue, int segmentCount,
                                                             double startStep, double minStep, double stepCoefficient, double maxFailedAttemptCount,
                                                             Random random)
        {
            double dt = (b - a) / (double)segmentCount;

            double step = startStep;
            VN start = new VN(segmentCount - 1, random);
            VN current = start;
            while (step > minStep)
            {
                int failedAttemptCount = 0;

                for (int i = 0; i < maxFailedAttemptCount; i++)
                {
                    VN direction = new VN(segmentCount - 1, random).Normalize().MultiplyOnScalar(step);
                    VN next = current.Add(direction);

                    double currentIntegralSum = IntegralSum(current, dt, aValue, bValue);
                    double nextIntegralSum = IntegralSum(next, dt, aValue, bValue);
                    if (nextIntegralSum < currentIntegralSum)
                    {
                        current = next;
                        break;
                    }
                    else
                    {
                        failedAttemptCount++;
                    }
                }

                if (failedAttemptCount >= maxFailedAttemptCount)
                    step *= stepCoefficient;
            }

            double[] T = new double[segmentCount + 1];
            for (int i = 0; i < segmentCount + 1; i++)
                T[i] = a + (double)i * dt;

            double[] flX = new double[segmentCount + 1];
            flX[0] = aValue;
            for (int i = 1; i < segmentCount; i++)
                flX[i] = current.At(i - 1);
            flX[segmentCount] = bValue;

            return (dt, T, flX, start);
        }

        public static double IntegralSum(VN vn, double dt, double aValue, double bValue)
        {
            double result = 0;

            result += _TargetFunction(aValue, vn.At(0), dt);
            for (int i = 0; i < vn.Dimension - 1; i++)
                result += _TargetFunction(vn.At(i), vn.At(i + 1), dt);
            result += _TargetFunction(vn.At(vn.Dimension - 1), bValue, dt);

            return result;
        }

        private static double _TargetFunction(double xi, double xi1, double dt)
        {
            return (Math.Pow((xi1 - xi) / dt, 2.0) + 4.0 * xi * xi) * dt;
        }
    }
}
