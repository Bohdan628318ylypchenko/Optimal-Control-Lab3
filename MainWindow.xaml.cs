using System.Windows;
using System.Windows.Input;

namespace Optimal_Control_Lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly String LEGEND = "{0}: run {1}; seed {2}; segment count {3}; istep {4}; mstep {5}; fail {6}; coeff {7}";

        int _runCount = 0;
        bool _isActualPlotted = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            double a = 0; double aValue = Math.Pow(Math.E, 2.0);
            double b = 1; double bValue = 1;

            try
            {
                int seed = int.Parse(SeedTextBox.Text);
                int segmentCount = int.Parse(SegmentCountTextBox.Text);
                double startStep = double.Parse(StartStepTextBox.Text);
                double minStep = double.Parse(MinStepTextBox.Text);
                int failAttemptStepCount = int.Parse(FailAttemptCountStepTextBox.Text);
                double stepCoefficient = double.Parse(StepCoefficientTextBox.Text);

                Random random = new Random(seed);
            
                (double dt, double[] T, double[] X, VN start) = RandomSearchProblem14Solver.Solve(a, aValue, b, bValue, segmentCount,
                                                                                                  startStep, minStep, stepCoefficient, failAttemptStepCount,
                                                                                                  random);
                double integralSum = RandomSearchProblem14Solver.IntegralSum(new VN(X), dt, aValue, bValue);
                RunTextBox.Text += $"""
                                    ==========> Run {++_runCount} ============
                                    input parameters:
                                        a = {a}, x(a) = {aValue}
                                        b = {b}, x(b) = {bValue}
                                        seed = {seed}
                                        segmentCount = {segmentCount}
                                        start step = {startStep} > min step {minStep},
                                        fail attempt count = {failAttemptStepCount}
                                        step decrease coefficient = {stepCoefficient}
                                    output:
                                        Minimization result =
                                        {string.Join("\n    ", T.Zip(X, (x, y) => (x, y)).Select(r => String.Format("t = {0}; x = {1}", r.x.ToString("0.####"), r.y.ToString("0.####"))))}

                                        Integral sum value = {integralSum}

                                        Start point (except a and b): {start}
                                    ===========================================


                                    """;

                if (!_isActualPlotted)
                {
                    double[] actualX = Enumerable.Range(0, 1000).Select(x => x / 1000.0).ToArray();
                    double[] actualY = actualX.Select(x => Math.Pow(Math.E, 2.0 - 2.0 * x)).ToArray();
                    var actualFunctionPlotSignal = FunctionPlot.Plot.Add.SignalXY(actualX, actualY);
                    actualFunctionPlotSignal.Label = "Analytical function";

                    var actualIntegrationSum = IntegralSumPlot.Plot.Add.Line(0.0, 107.2, 1000.0, 107.2);
                    actualIntegrationSum.Label = "Analytical integration sum";
                    _isActualPlotted = true;
                }

                var numericFunctionPlotScatter = FunctionPlot.Plot.Add.Scatter(T, X);
                numericFunctionPlotScatter.Label = String.Format(LEGEND, "Numeric function",
                                                                 _runCount, seed, segmentCount, startStep, minStep, failAttemptStepCount, stepCoefficient);

                FunctionPlot.Plot.ShowLegend();
                FunctionPlot.Refresh();

                var integralSumPlotMarker = IntegralSumPlot.Plot.Add.Marker(_runCount, integralSum);
                integralSumPlotMarker.Label = String.Format(LEGEND, "Integral sum value",
                                                            _runCount, seed, segmentCount, startStep, minStep, failAttemptStepCount, stepCoefficient);
                IntegralSumPlot.Plot.ShowLegend();
                IntegralSumPlot.Refresh();
            }
            catch
            {
                MessageBox.Show("Something went wrong, check input.");
            }

            Mouse.OverrideCursor = null;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            FunctionPlot.Plot.Clear();
            FunctionPlot.Refresh();
            IntegralSumPlot.Plot.Clear();
            IntegralSumPlot.Refresh();
            RunTextBox.Text = "";
            _runCount = 0;
            _isActualPlotted = false;
        }
    }
}