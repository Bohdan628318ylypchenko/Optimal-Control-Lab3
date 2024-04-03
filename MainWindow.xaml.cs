using System.Windows;

namespace Optimal_Control_Lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random r;
        int segmentCount = 12;
        public MainWindow()
        {
            InitializeComponent();
            r = new Random(451);
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            double[] coordinates = new double[segmentCount + 1];
            for (int i = 0; i < segmentCount + 1; i++)
                coordinates[i] = r.NextDouble();

            RandomSearchProblem14Solver solver = new RandomSearchProblem14Solver(r);
            
            var result = solver.Solve(0, Math.Pow(Math.E, 2.0), 1, 1, 12, new VN(coordinates), 0.5, 0.001, 100, 0.9);
        }
    }
}