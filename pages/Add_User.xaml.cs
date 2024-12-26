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

namespace task1.pages
{
    /// <summary>
    /// Логика взаимодействия для Add_User.xaml
    /// </summary>
    public partial class Add_User : Window
    {
        public int adminID;

        public Add_User(int adminID)
        {
            InitializeComponent();
            this.adminID = adminID;
        }
        private void btnAddNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text) ||
       string.IsNullOrWhiteSpace(Login.Text) ||
       string.IsNullOrWhiteSpace(Password.Password) ||
       TypeBox.SelectedItem == null)
            {
                MessageBox.Show("Необходимо заполнить все поля.");
                return;
            }
            else
            {
                var selectedService = (dynamic)comboBoxService.SelectedItem;
                int serviceId = selectedService.Id; 

                var selectedType = (dynamic)TypeBox.SelectedItem;
                int typeId = selectedType.Id; // Получаем идентификатор выбранного типа


                using (var context = new bazdanEntities())
                {
                    var users = new Сотрудники
                    {
                        Логин = Login.Text,
                        Пароль = Password.Password,
                        ФИО_сотрудника = Name.Text,
                        Услуги_сотрудника = serviceId, // Используем идентификатор услуги
                        ip = ip.Text,
                        Роль_сотрудника = typeId,
                        Email = email.Text,
                        Последний_вход = DateTime.Now,
                    };

                    context.Сотрудники.Add(users);
                    context.SaveChanges();
                }


            }



//Mail_LIB.Validation validation = new Mail_LIB.Validation();
//bool isValidLogin = validation.Check_login(Login.Text);
//if (!isValidLogin)
//{
//    MessageBox.Show("Ошибка: некорректный логин !");
//}

//bool isValidPassword = validation.Check_password(Password.Password);
//if (!isValidPassword)
//{
//    MessageBox.Show("Ошибка: Некорректный пароль !");
//}
//bool isValidEmail = validation.Check_mail(email.Text);
//if (!isValidEmail)
//{
//    MessageBox.Show("Ошибка: Некорректная почта !");
//}


//if (string.IsNullOrWhiteSpace(Name.Text) ||
//    string.IsNullOrWhiteSpace(Login.Text) ||
//    string.IsNullOrWhiteSpace(Password.Password) ||
//    string.IsNullOrWhiteSpace(TypeBox.Text))
//{
//    MessageBox.Show("Необходимо заполнить все поля.");
//    return;
//}
//else
//{
//    string name = Name.Text;
//    string login = Login.Text;
//    string password = Password.Password;
//    string type = TypeBox.Text;
//    using (var context = new labaratoriya777Entities())
//    {

//        var users = new users
//        {
//            login = Login.Text,
//            password = Password.Password,
//            name = Name.Text,
//            services = Convert.ToInt32(service.Text),
//            ip = ip.Text,
//            type = Convert.ToInt32(TypeBox.Text),
//            Email = email.Text,
//            lastenter = DateTime.Now,

//        };

//        context.users.Add(users);
//        context.SaveChanges();
//    }
//    Name.Clear();
//    Login.Clear();
//    Password.Clear();
//    service.Clear();
//    ip.Clear();
//    email.Clear();
//    MessageBox.Show("Пользователь добавлен");
//}




;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            admin adm = new admin(adminID);  
            adm.Show();   
            this.Close();   
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new bazdanEntities())
            {
                var services = context.Услуги.ToList();
                foreach (var service in services)
                {
                    comboBoxService.Items.Add(new { Id = service.Код_услуги, Name = service.Наименование_услуги });
                }
                comboBoxService.DisplayMemberPath = "Name";
                comboBoxService.SelectedValuePath = "Id";
            }
            using (var context = new bazdanEntities())
            {
                var types = context.Роль_пользователя.ToList();
                foreach (var type in types)
                {
                    TypeBox.Items.Add(new { Id = type.Код_роли, Name = type.Наименование_роли });
                }
                TypeBox.DisplayMemberPath = "Name";
                TypeBox.SelectedValuePath = "Id";
            }
        }

        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
