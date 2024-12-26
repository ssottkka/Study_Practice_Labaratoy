﻿using System;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace task1.pages
{
    /// <summary>
    /// Логика взаимодействия для Lab_Man.xaml
    /// </summary>
    public partial class Lab_Assistant : Window
    {
        private int labAssisID;
        private DispatcherTimer timer;
        private TimeSpan timeLeft;
        public Lab_Assistant(int labAssisID)
        {
            InitializeComponent();
            this.labAssisID = labAssisID;
            // Установка начального значения таймера
            timeLeft = TimeSpan.FromHours(2).Add(TimeSpan.FromMinutes(30));

            // Создание и настройка DispatcherTimer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Обновление каждую секунду
            timer.Tick += Timer_Tick;
            timer.Start();

            // Отображение начального значения таймера
            UpdateTimerText();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Уменьшение оставшегося времени на 1 секунду
            timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));

            // Обновление текста таймера
            UpdateTimerText();

            // Проверка, закончилось ли время
            if (timeLeft.TotalSeconds <= 0)
            {
                timer.Stop();
                MessageBox.Show("Время вышло!");
                MainWindow main = new MainWindow();
                this.Close();
                main.Show();

            }
        }

        private void UpdateTimerText()
        {
            // Форматирование оставшегося времени в формат "mm:ss"
            string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}", timeLeft.Hours, timeLeft.Minutes, timeLeft.Seconds);
            TimerTextBlock.Text = timeString;
        }


        void timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToLongTimeString();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main
                = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new bazdanEntities())
            {
                var labassis = context.Сотрудники.FirstOrDefault(x => x.Код_сотрудника == labAssisID);
                if (labassis != null)
                {
                    labAssisFIO.Content = labassis.ФИО_сотрудника;
                    labAssisType.Content = "Лаборант-исследователь";
                }
            }
        }

        private void analizator_Click(object sender, RoutedEventArgs e)
        {
            Analizator analizator = new Analizator();   
            analizator.Show();
            this.Close();
        }
    }
}