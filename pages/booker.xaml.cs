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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace task1.pages
{
    /// <summary>
    /// Логика взаимодействия для booker.xaml
    /// </summary>
    public partial class booker : Window
    {
        private int bookerID;
        
        public booker(int bookerID)
        { 
            InitializeComponent();
            this.bookerID = bookerID;
               
        }
        bazdanEntities db = new bazdanEntities();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
              main.Show();    
           this.Close(); 
           

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new bazdanEntities())
            {
                var boker = context.Сотрудники.FirstOrDefault(x => x.Код_сотрудника == bookerID);
                if (boker != null)
                {
                    bookerFIO.Content = boker.ФИО_сотрудника;
                    bookerType.Content = "Бухгалтер";
                }


            }
        }
    }
}
