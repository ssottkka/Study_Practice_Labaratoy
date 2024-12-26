using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Mail_LIB;

namespace task1.pages
{
    /// <summary>
    /// Логика взаимодействия для admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        public int adminID;
        bazdanEntities db = new bazdanEntities();
        private ObservableCollection<История> historyCollection;
        private CollectionViewSource historyViewSource;

        public admin(int adminID)
        {
            InitializeComponent();
            Loaded += Window_Loaded;

            this.adminID = adminID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();

            main.Show();
            this.Close();
           
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new bazdanEntities())
            {
                var admm = context.Сотрудники.FirstOrDefault(x => x.Код_сотрудника == adminID);
                if (admm != null)
                {
                    adminType.Content = ("Админ");
                    adminFIO.Content = admm.ФИО_сотрудника;
                }


            }
        }


        private void showHistory_Click(object sender, RoutedEventArgs e)
        {

            // Очистка текущих сортировок
           historyViewSource.SortDescriptions.Clear();

            // Добавление новой сортировки по логину
            historyViewSource.SortDescriptions.Add(new SortDescription("login", ListSortDirection.Ascending));
            historyViewSource.View.Refresh();
        }
        private void dateSort_Click(object sender, RoutedEventArgs e)
        {
            
            using (var context = new bazdanEntities())
            {
                // Загрузка данных из базы данных
                var historyData = context.История.ToList();

                // Создание ObservableCollection для привязки к DataGrid
                historyCollection = new ObservableCollection<История>(historyData);

                // Создание CollectionViewSource для сортировки
                historyViewSource = new CollectionViewSource { Source = historyCollection };
                historyViewSource.SortDescriptions.Add(new SortDescription("date", ListSortDirection.Ascending));

                // Привязка данных к DataGrid
                historyDataGrid.ItemsSource = historyViewSource.View;
            }
        }
        private void Type_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

       

        private void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            Add_User add_User = new Add_User(adminID);
            add_User.Show();
            this.Close();
        }
    }
}
