using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace simulation_of_pressure_distribution
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<double> {},
                        PointGeometry = DefaultGeometries.Circle,
                        PointGeometrySize = 5
                    }
                };
            SeriesCollection1 = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<double> {},
                        PointGeometry = DefaultGeometries.Circle,
                        PointGeometrySize = 5
                    }
                };


        }

        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection SeriesCollection1 { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public double f = 0.15, hf = 0.029;
        double L, Ge, a, c, pkr, fikr, gamma, beta, hkr, A, B, b, P, k = 1.041, fkr, psr, mu1, mu2, E1, E2, x1, x2, q, w;

        private void NumberValidationTextBox(object sender, KeyEventArgs e)
        {
            var keyEnum = (System.Windows.Input.Key)e.Key; 
            e.Handled = !(keyEnum >= System.Windows.Input.Key.D0 && keyEnum <= System.Windows.Input.Key.D9 
                || keyEnum >= System.Windows.Input.Key.NumPad0 && keyEnum <= System.Windows.Input.Key.NumPad9 
                || keyEnum == System.Windows.Input.Key.Back || keyEnum == System.Windows.Input.Key.Tab
                || keyEnum == System.Windows.Input.Key.Delete || keyEnum == System.Windows.Input.Key.Decimal
                || keyEnum == System.Windows.Input.Key.Subtract);
        }

            private bool load_data()
        {
            bool result = true;
            try
            {
                Ge = Convert.ToDouble(tbG.Text);
                B = Convert.ToDouble(tbB.Text);
                L = Convert.ToDouble(tbL.Text);
                a = L / 2;
                c = Convert.ToDouble(tbc.Text);
                pkr = Convert.ToDouble(tbpkp.Text);
                gamma = (Convert.ToDouble(tbgamma.Text) * 3.14) / 180;
                hkr = Convert.ToDouble(tbhkp.Text);
                P = Ge + pkr * Math.Cos(gamma);
                fikr = pkr / P;
                psr = Ge / (2 * B * L);
                A = (-1 * (fikr * (hkr * Math.Cos(gamma) + Math.Sin(gamma) * c) + f * hf));
                mu1 = Convert.ToDouble(tbmu1.Text);
                mu2 = Convert.ToDouble(tbmu2.Text);
                E1 = Convert.ToDouble(tbE1.Text);
                E2 = Convert.ToDouble(tbE2.Text);
                beta = 2 * (1 - Math.Pow(mu1, 2)) / (3.14 * E1) + 2 * (1 - Math.Pow(mu2, 2)) / (3.14 * E2);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        private void Simulation(object sender, RoutedEventArgs e)
        {

            if (!load_data())
            {
                MessageBox.Show("Ошибка! Введите корректные данные");
                return;
            }
            try
            {
               ch1.IsChecked = true;
                chartGrid.Visibility = Visibility.Visible;
                Optoins.Visibility = Visibility.Visible;
                ListBox1.Items.Clear();
                SeriesCollection[0].Values.Clear();
                ListBox2.Visibility = Visibility.Hidden;
                chartGrid1.Visibility = Visibility.Hidden;
                x1 = a * (-1);
                x2 = a;
                while (x1 <= x2)
                {
                    // F(x)
                    w = x1 * (A + fikr * (hkr * Math.Cos(gamma) + c * Math.Sin(gamma)) + f * hf);
                    q = (P * (1 + 2 * (w / Math.Pow(a, 2)))) / (3.14 * Math.Sqrt(Math.Pow(a, 2) - Math.Pow(x1, 2)));
                    ListBox1.Items.Add("x = " + String.Format("{0:0.00}", x1) + "   F(x) = " + String.Format("{0:0.0000}", q));
                    if (!double.IsInfinity(q))
                    {
                        SeriesCollection[0].Values.Add(q);
                    }
                    x1 += 0.17;

                }

                DataContext = this;
            } catch { }
        }

        private void pressure(object sender, RoutedEventArgs e)
        {
            if (!load_data())
            {
                MessageBox.Show("Ошибка! Введите корректные данные");
                return;
            }
            try
            {
                chartGrid.Visibility = Visibility.Visible;
                Optoins.Visibility = Visibility.Visible;
                ListBox1.Items.Clear();
                SeriesCollection[0].Values.Clear();
                ListBox2.Visibility = Visibility.Hidden;
                chartGrid1.Visibility = Visibility.Hidden;
                x1 = a * (-1);
                x2 = a;
                while (x1 <= x2)
                {
                    // F(x)

                    q = 0.5 * psr * 3.14 * beta * (x1 * Math.Asin(x1 / a) + A - (B * (x1 * A / Math.Pow(a, 2) + Math.Asin(x1 / a)))) + c;
                    ListBox1.Items.Add("x = " + String.Format("{0:0.00}", x1) + "   F(x) = " + String.Format("{0:0.0000}", q));
                    if (!double.IsInfinity(q))
                    {
                        SeriesCollection[0].Values.Add(q);
                    }
                    x1 += 0.17;

                }

                DataContext = this;
            } catch {};
        }

        private void load_distribution(object sender, RoutedEventArgs e)
        {
            if (!load_data())
            {
                MessageBox.Show("Ошибка! Введите корректные данные");
                return;
            }
            try
            {
                chartGrid.Visibility = Visibility.Visible;
                Optoins.Visibility = Visibility.Visible;
                ListBox1.Items.Clear();
                SeriesCollection[0].Values.Clear();
                chartGrid1.Visibility = Visibility.Visible;
                Optoins.Visibility = Visibility.Visible;
                ListBox1.Items.Clear();
                SeriesCollection1[0].Values.Clear();
                ListBox2.Visibility = Visibility.Visible;

                x1 = a * (-1);
                x2 = a;
                while (x1 <= x2)
                {
                    // F(x)
                    w = (P * (1 + 2 * ((x1 * (Math.Exp(1) + fikr * (hkr * Math.Cos(gamma) + c * Math.Sin(x1)) + f * hf)) / Math.Pow(a, 2)))) / (3.14 * Math.Sqrt(Math.Pow(a, 2) - Math.Pow(x1, 2)));
                    q = psr / (1 + 2 * ((x1 * (Math.Exp(1) + fikr * (hkr * Math.Cos(gamma) + Math.Sin(gamma)) + f * hf)) / Math.Pow(a, 2)));
                    if (q > 0)
                    {
                        ListBox1.Items.Add("x = " + String.Format("{0:0.00}", x1) + "   F(x) = " + String.Format("{0:0.0000}", q));
                        if (!double.IsInfinity(q))
                        {
                            SeriesCollection[0].Values.Add(q);
                        }
                    }
                    ListBox2.Items.Add("x = " + String.Format("{0:0.00}", x1) + "   g(x) = " + String.Format("{0:0.0000}", w));
                    if (!double.IsInfinity(w))
                    {
                        SeriesCollection1[0].Values.Add(w);
                    }
                    x1 += 0.17;

                }

                DataContext = this;
            } catch { };
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
            RadioButton pressed = (RadioButton)sender;
            switch (pressed.Name.ToString())
            {
                case "ch1": 
                    View_options.Content = pressed.Content.ToString();
                    if (!load_data()) return;
                    Simulation(sender,e);
                    break;
                case "ch2":
                    View_options.Content = pressed.Content.ToString();
                    if (!load_data()) return;
                    pressure(sender,e);
                    break;
                case "ch3":
                    View_options.Content = pressed.Content.ToString();
                    if (!load_data()) return;
                    load_distribution(sender,e);
                    break;
                default:
                    break;
            }
        }
    }
}