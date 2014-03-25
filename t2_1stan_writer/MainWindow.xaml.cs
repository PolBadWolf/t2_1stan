﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using t2_1stan_writer.Properties;

namespace t2_1stan_writer
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Parameters _parameters = new Parameters();
        private readonly Writer _writer = new Writer();
        private int _count;


        public MainWindow()
        {
            InitializeComponent();
            _writer.MainWindow = this;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TabItem2.IsEnabled = false;
            TabItem3.IsEnabled = false;
            TabItem1.Visibility = Visibility.Visible;
            TabControl1.SelectedIndex = 0;
            try
            {
                ComboBox1.ItemsSource = _parameters.get_db_worksmens();
                ComboBox2.ItemsSource = _parameters.get_db_timeintervalsmens();
                ComboBox3.ItemsSource = _parameters.get_db_surnames();
                ComboBox4.ItemsSource = _parameters.get_db_surnames();
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            TabItem1.IsEnabled = false;
            TabItem3.IsEnabled = false;
            TabItem2.Visibility = Visibility.Visible;
            TabControl1.SelectedIndex = 1;
            try
            {
                ComboBox7.ItemsSource = _parameters.get_db_gosts();
                ComboBox5.ItemsSource = _parameters.get_db_sizetubes();
                ComboBox8.ItemsSource = _parameters.get_db_controlsamples();
                ComboBox9.ItemsSource = _parameters.get_db_listdefects();
                ComboBox11.ItemsSource = _parameters.get_db_device();
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox1.SelectedIndex != -1 &&
                ComboBox2.SelectedIndex != -1 &&
                ComboBox3.SelectedIndex != -1 &&
                ComboBox4.SelectedIndex != -1 &&
                TextBox4.Text != "" &&
                ComboBox8.SelectedIndex != -1 &&
                ComboBox5.SelectedIndex != -1 &&
                TextBox4.Text != "" &&
                ComboBox9.SelectedIndex != -1 &&
                ComboBox11.SelectedIndex != -1 &&
                TextBox2.Text != "" &&
                TextBox3.Text != "")
            {
                _writer.port_Open();
                TabItem1.IsEnabled = false;
                TabItem2.IsEnabled = false;
                TabItem3.Visibility = Visibility.Visible;
                TabControl1.SelectedIndex = 2;

                var greenBrush = new SolidColorBrush
                {
                    Color = Colors.Green
                };

                for (int i = 0; i < 14; i++)
                {
                    var myLine = new Line();
                    Canvas.SetLeft(myLine, 50);

                    if (i == 0)
                    {
                        myLine.X1 = 0;
                    }
                    else
                    {
                        myLine.X1 = i*48;
                    }
                    myLine.X2 = myLine.X1;

                    myLine.Y1 = 120;
                    myLine.Y2 = 370;

                    myLine.StrokeThickness = 2;
                    myLine.Stroke = greenBrush;

                    Canvas.Children.Add(myLine);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        public void new_tube()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                Tube.Width = 0;
                for (int i = 0; i <= _count; i++)
                {
                    Canvas.Children.Remove((UIElement) Canvas.FindName("errorLine" + i));
                    try
                    {
                        Canvas.UnregisterName("errorLine" + i);
                    }
// ReSharper disable EmptyGeneralCatchClause
                    catch
// ReSharper restore EmptyGeneralCatchClause
                    {
                    }
                }
                _count = 0;
            }));
        }

        public void move_tube()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate { Tube.Width += 8; }));
        }

        public void error_segment()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                var redBrush = new SolidColorBrush
                {
                    Color = Colors.Red
                };
                var errorLine = new Line();

                Canvas.SetLeft(errorLine, Tube.Width + Canvas.GetLeft(Tube) - 4);
                errorLine.X1 = 0;
                errorLine.X2 = 0;
                errorLine.Y1 = 220 - 8;
                errorLine.Y2 = 320 + 8;
                errorLine.StrokeThickness = 8;
                errorLine.Stroke = redBrush;
                errorLine.Fill = redBrush;
                Canvas.RegisterName("errorLine" + _count, errorLine);
                _count++;
                Canvas.Children.Add(errorLine);
            }));
        }

        private void textBox4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void textBox2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void textBox3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && char.IsControl(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //TODO Need to do it
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Settings ps = Settings.Default;
            Top = ps.Top;
            Left = ps.Left;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Settings ps = Settings.Default;
            ps.Top = Top;
            ps.Left = Left;
            ps.Save();

            _writer.port_Close();
        }

        private void comboBox7_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox5.ItemsSource =
                    _parameters.get_db_sizetubes_current(((KeyValuePair<int, string>) ComboBox7.SelectedItem).Key);
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
            }
        }

        private void comboBox5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox8.ItemsSource =
                    _parameters.get_db_controlsamples_current(((KeyValuePair<int, string>) ComboBox5.SelectedItem).Key);
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            var aw = new ArchiveWindow
            {
                Owner = this
            };
            try
            {
                aw.Show();
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
            }
        }
    }
}