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
using ZXing;
using ZXing.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32; // Для сохранения файла
using BarcodeStandard;
using Barcode = BarcodeStandard.Barcode;
using Image = System.Drawing.Image;
using Path = System.IO.Path;
using Org.BouncyCastle.Asn1.X509;
namespace task1.pages
{
    /// <summary>
    /// Логика взаимодействия для Add_Material.xaml
    /// </summary>
    public partial class Add_Material : Window
    {
        private int labManID;
        private readonly bazdanEntities _context;
        private List<Услуги> _services;
        public Add_Material()
        {
            InitializeComponent();
            _context = new bazdanEntities();
            LoadServices();

        }
        private void LoadServices()
        {
            // Загружаем данные из таблицы "Услуги_лаборатории"
            _services = _context.Услуги.ToList();

            // Заполняем ComboBox данными
            comboBoxService.ItemsSource = _services;
            comboBoxService.DisplayMemberPath = "Наименование"; // Указываем свойство, которое будет отображаться
            comboBoxService.SelectedValuePath = "Id_Услуг_Лаборатории"; // Указываем свойство, которое будет использоваться как значение
        }
        private void GetOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedService = comboBoxService.SelectedItem as Услуги;
            if (selectedService == null)
            {
                MessageBox.Show("Необходимо выбрать услугу.");
                return;
            }

            string surname = Surname.Text;
            string name = Name.Text;
            string lastname = Lastname.Text;
            var patient = _context.Данные_пациентов.FirstOrDefault(p => p.ФИО_пациента == surname); //&& p.Name == name && p.Lastname == lastname);
            if (patient == null)
            {
                MessageBox.Show("Такого пациента нет, необходимо его добавить");
                // Открываем окно для добавления пациента
                Add_Patient add_Patient = new Add_Patient();
                add_Patient.Show();
                this.Close();
                base.Close();
            }
            else
            {
                SaveOrder(patient.Код_пациента, selectedService.Код_услуги);
            }
        }
        private void SaveOrder(int patientId, int serviceId)
        {
            if (string.IsNullOrWhiteSpace(MatNumber.Text))
            {
                MessageBox.Show("Необходимо ввести номер материала.");
                return;
            }

            int materialCode;
            if (!int.TryParse(MatNumber.Text, out materialCode))
            {
                MessageBox.Show("Номер материала должен быть корректным целым числом.");
                return;
            }
            var selectedService = _context.Услуги.FirstOrDefault(s => s.Код_услуги == serviceId);
            if (selectedService == null)
            {
                MessageBox.Show("Выбранная услуга не найдена.");
                return;
            }
            int? completionTimeNullable = selectedService.Время_выполнения;
            if (completionTimeNullable.HasValue)
            {
                int completionTime = completionTimeNullable.Value;
                var Заказ = new Заказ
                {
                    Код_пациента = patientId,
                    Дата_создания = DateTime.Now,
                    Статус_заказа = 2,
                    Код_заказа = serviceId,
                    Код_биоматериала = materialCode,
                    Время_выполнения_заказа =  completionTime.ToString(),   
                   // Состояние_услуги = 2
                    


                };
                _context.Заказ.Add(Заказ);
                _context.SaveChanges();
                MessageBox.Show("Заказ сформирован");
            }




               
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
        }






       








        private void numberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }





        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Lab_Man lab_Man = new Lab_Man(labManID);
            lab_Man.Show();
            this.Close();   
        }

        //private void Button_Click_2(object sender, RoutedEventArgs e)
        //{
        //    Add_Patient add_Patient = new Add_Patient();
        //    add_Patient.Show();
        //    this.Close();
        //}














        private void SaveBarcodeToPdf(Bitmap barcodeImage, string fileName)
        {
            // Получаем путь к папке "Documents" пользователя
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(documentsPath, fileName);

            // Создаем новый PDF-документ
            using (Document document = new Document(PageSize.A4, 36, 36, 54, 36))
            {
                // Создаем поток для записи PDF-файла
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    // Получаем объект PdfWriter для записи в документ
                    PdfWriter writer = PdfWriter.GetInstance(document, fs);

                    // Открываем документ
                    document.Open();

                    // Добавляем штрих-код на страницу PDF
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(barcodeImage, System.Drawing.Imaging.ImageFormat.Png);
                    img.ScaleToFit(PageSize.A4.Width - 72, PageSize.A4.Height - 72); // Масштабируем изображение под размер страницы
                    img.SetAbsolutePosition((PageSize.A4.Width - img.Width) / 2, (PageSize.A4.Height - img.Height) / 2);
                    document.Add(img);

                    // Закрываем документ
                    document.Close();
                }
            }

            MessageBox.Show($"Штрих-код сохранен в файл: {filePath}");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

            using (var context = new bazdanEntities())
            {
                var biomaterials = new Биоматериалы
                {
                    Код_биоматериала = Convert.ToInt32( MatNumber.Text),
                };
                context.Биоматериалы.Add(biomaterials);
                context.SaveChanges();
            }


            // Создаем штрих-код
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 100,
                    Width = 300
                }
            };

            Bitmap barcodeBitmap = writer.Write("514092020123456");

            // Сохраняем штрих-код в PDF
            SaveBarcodeToPdf(barcodeBitmap, "barcode.pdf");

            // Освобождаем ресурсы
            barcodeBitmap.Dispose();

            MessageBox.Show("Штрих-код создан и сохранен в файл barcode.pdf");

        }
    }

}
