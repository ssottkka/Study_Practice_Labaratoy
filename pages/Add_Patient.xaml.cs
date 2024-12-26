using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для Add_Patient.xaml
    /// </summary>
    public partial class Add_Patient : Window
    {
        public  bazdanEntities _context;
        private List<Тип_полиса> _types;
        private List<Страховые_компании> _companyName;
        bazdanEntities bazdanEntities = new bazdanEntities();
        public Add_Patient()
        {
            InitializeComponent();
            _context = new bazdanEntities();
            LoadServices();
        }
        private void LoadServices()
        {
            // Загружаем данные из таблицы "Услуги_лаборатории"
            _types = _context.Тип_полиса.ToList();
            _companyName = _context.Страховые_компании.ToList();
            CompanyName.ItemsSource = _companyName;
            CompanyName.DisplayMemberPath = "Название_страховой_компании";
            CompanyName.SelectedValuePath = "ID_страховой_компании";


            // Заполняем ComboBox данными
            PolisType.ItemsSource = _types;
            PolisType.DisplayMemberPath = "Наименование"; // Указываем свойство, которое будет отображаться
            PolisType.SelectedValuePath = "id"; // Указываем свойство, которое будет использоваться как значение
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCompany = CompanyName.SelectedItem as Страховые_компании;
            int selectedCompanyId = selectedCompany.Код_компании;
            if(selectedCompany == null)
            {
                MessageBox.Show("Необходимо выбрать компанию");
                return;
            }
            
            var selectedType = PolisType.SelectedItem as Тип_полиса;
            int selectedTypeId = selectedType.Код_полиса; // Получаем Id выбранного типа полиса
            if (selectedType == null)
            {
                MessageBox.Show("Необходимо выбрать услугу.");
                return;
            }
            var Данные_пациентов = new Данные_пациентов
            {
                Дата_рождения = BirthDatePicker.SelectedDate,
                ФИО_пациента = Name.Text,
                //Surname = Surname.Text,
                //Lastname = Lastname.Text,
                Серия_паспорта = PassportSeries.Text,
                Номер_паспорта = PassportNumber.Text,
                Номер_полиса = Polis.Text,
                Электронная_почта = Email.Text,
                Логин = Login.Text,
                Пароль = Password.ToString(),
                Тип_полиса = selectedTypeId,
                Страховая_компания = selectedCompany.Код_компании
                // Записываем Id типа полиса
            };
            _context.Данные_пациентов.Add(Данные_пациентов);
            _context.SaveChanges();
            MessageBox.Show("Пациент добавлен");



        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Add_Material addMaterial = new Add_Material();
            addMaterial.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new bazdanEntities())
            {
                var types = context.Тип_полиса.ToList();
                foreach (var type in types)
                {
                    PolisType.Items.Add(new { Id = type.Код_полиса, Name = type.Наименование });
                }
                PolisType.DisplayMemberPath = "Наименование";
                PolisType.SelectedValuePath = "id";
            }

        }

        private void PolisType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           

        }
    }
}
