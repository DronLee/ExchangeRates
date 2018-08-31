using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.DataVisualization.Charting;
using GraphicView.Model;

namespace GraphicView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawGraphic(Chart chart, ExchangeRates exchangeRates, bool viewDay)
        {
            ChartArea area = new ChartArea("Default");
            chart.ChartAreas.Add(area);
            Series series = new Series("Series");
            chart.Series.Add(series);
            series.ChartArea = "Default";
            series.ChartType = SeriesChartType.Line;
            series.Color = System.Drawing.Color.DarkBlue;
            if (viewDay)
            {
                area.AxisX.Interval = 1;
                series.Points.DataBindXY(exchangeRates.Days, exchangeRates.Values);
            }
            else
                series.Points.DataBindXY(exchangeRates.Dates, exchangeRates.Values);
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            ERDolar erDolar = new ERDolar();
            EREuro erEuro = new EREuro();
            DrawGraphic(dolarChart, erDolar, false);
            DrawGraphic(euroChart, erEuro, false);
            DrawGraphic(lastMonthDolarChart, erDolar.LastMonth, true);
            DrawGraphic(lastMonthEuroChart, erEuro.LastMonth, true);
        }
    }
}
