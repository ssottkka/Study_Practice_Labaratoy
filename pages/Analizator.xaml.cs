using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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
    /// Логика взаимодействия для Analizator.xaml
    /// </summary>
    public partial class Analizator : Window
    {
     
        private int labAssisID;
        bazdanEntities db = new bazdanEntities();
        private readonly bazdanEntities _context;
        Сотрудники user = new Сотрудники(); 
        private List<Заказ> _order;
        List<Заказ> order = new List<Заказ> (); 
        List<Состояние_услуги> ser;
        List<Статус_Услуги> statysl;
        private DispatcherTimer analizator;
        Заказ ord = new Заказ();
        private DispatcherTimer timer;
        GetAnalizator getAnalizator = new GetAnalizator();
        List<Услуги> Услуги = new List<Услуги>();
        List<Биоматериалы> bio = new List<Биоматериалы>();


        public Analizator()
        {
            _context = new bazdanEntities();
            InitializeComponent();
            LoadOrder();
            analizator = new DispatcherTimer();
            timer = new DispatcherTimer();
            analizator = new DispatcherTimer();
            

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();

            order = db.Заказ.ToList();
            for (int i = 0; i < order.Count; i++)
            {
                cb1.Items.Add(order[i].Биоматериалы.Номер_биоматериала);
            }
            Услуги = db.Услуги.ToList();
            bio = db.Биоматериалы.ToList();


        }
        private void listAdd()
        {
            listServices.Items.Clear();
            Биоматериалы b = bio.Find(x => x.Номер_биоматериала == cb1.Text);
            ord = db.Заказ.Where(x => x.Код_биоматериала == b.Код_биоматериала).FirstOrDefault();
            ser = db.Состояние_услуги.Where(x => x.Код_заказа == ord.Код_заказа).ToList();
            for (int i = 0; i < ser.Count; i++)
            {
                int a = Convert.ToInt32(ser[i].Код_услуги);
                listServices.Items.Add($"Услуга: {Услуги[a - 1].Наименование_услуги} Статус: {ser[i].Статус_Услуги.Наименование_статуса_услуги}");
            }
        }
        private void LoadOrder()
        {
            // Очищаем ListBox перед добавлением новых элементов
            listServices.Items.Clear();

            // Объединяем данные из трех таблиц и выбираем нужные поля
            var query = from order in _context.Заказ
                        join patient in _context.Данные_пациентов on order.Код_пациента equals patient.Код_пациента
                        join service in _context.Услуги on order.Код_заказа equals service.Код_услуги
                        where order.Статус_заказа == 2
                        select new
                        {
                            order.Код_заказа,
                            order.Дата_создания,
                            order.Статус_заказа,
                            order.Код_биоматериала,
                            ServiceName = service.Наименование_услуги,
                            PatientName = patient.ФИО_пациента
                        };

            // Выполняем запрос и получаем результаты
            var orders = query.ToList();

            // Отображаем данные заказа, пациента и наименование услуги
            foreach (var order in orders)
            {
                listServices.Items.Add($"ID заказа: {order.Код_заказа}, " +
                                   $"Дата создания: {order.Дата_создания}, " +
                                   $"Статус заказа: {order.Статус_заказа}, " +
                                   $"Код материала: {order.Код_биоматериала}, " +
                                   $"Наименование услуги: {order.ServiceName}, " +
                                   $"Имя пациента: {order.PatientName}");
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)//Выйти в меню входа
        {
            string subKeyName = "Timer";
            RegistryKey key = Registry.CurrentUser;
            if (key.OpenSubKey(subKeyName) != null)
                key.DeleteSubKeyTree(subKeyName);

            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }
        private void button6_Click(object sender, RoutedEventArgs e)
        {

        }


        private void button2_Click(object sender, RoutedEventArgs e)//Вернутся назад
        {
             Lab_Assistant l = new Lab_Assistant(labAssisID);
            l.Show();
            Close();
        }

        private void cb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //listAdd();
             LoadOrder();
        }

        private void button4_Click(object sender, RoutedEventArgs e)//Отправить на анализатор
        {
            Import();
        }

        private void button5_Click(object sender, RoutedEventArgs e)//Одобрить результат
        {
            listServices.Items.Add("Все результаты сохранены!");
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
        }
        string name = "";

        private void Import()
        {
            List<Услуги> УслугиFirst = new List<Услуги>();
            List<Услуги> УслугиSecond = new List<Услуги>();

            if (rb1.IsChecked == true)
                name = rb1.Content.ToString();
            else
                name = rb2.Content.ToString();

            for (int i = 0; i < ser.Count; i++)
            {
                int analiz = ser[i].Услуги.Приборы.Код_прибора;

                if (analiz == 1 || analiz == 3)
                {
                    int code = Convert.ToInt32(Услуги[Convert.ToInt32(ser[i].Код_услуги) - 1].Номер_услуги);
                    Услуги Услугиce = new Услуги();
                    Услугиce.Код_услуги = code;
                    УслугиFirst.Add(Услугиce);
                }
                else
                {
                    int code = Convert.ToInt32(Услуги[Convert.ToInt32(ser[i].Код_услуги) - 1].Номер_услуги);
                    Услуги Услугиce = new Услуги();
                    Услугиce.Код_услуги = code;
                    УслугиSecond.Add(Услугиce);
                }
            }
            string pacient = ord.Код_пациента.ToString();

            if (УслугиFirst != null && rb1.IsChecked == true)
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5000/api/analyzer/Ledetect");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                List<Услуги> services = УслугиFirst;
                string patient = pacient;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        patient,
                        services
                    });
                    streamWriter.Write(json);
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                    MessageBox.Show("Услуги успешно отправленны!");
                else
                    MessageBox.Show("Ошибка отправки!");

                analizator.Interval = TimeSpan.FromSeconds(30);
           //     analizator.Tick += Analizator_Tick;
                analizator.Start();
            }
            else
                MessageBox.Show("Такой запрос не нужен!");

            if (УслугиSecond != null && rb2.IsChecked == true)
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5000/api/analyzer/Biorad");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                List<Услуги> services = УслугиSecond;
                string patient = pacient;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        patient,
                        services
                    });
                    streamWriter.Write(json);
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                    MessageBox.Show("Услуги успешно отправленны!");
                else
                    MessageBox.Show("Ошибка отправки!");

           
            }
            else
                MessageBox.Show("Такой запрос не нужен!");
        }

      

        private void GetInfo()
        {
            if (rb1.IsChecked == true)
                name = rb1.Content.ToString();
            else
                name = rb2.Content.ToString();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5000/api/analyzer/{name}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = httpResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string json = reader.ReadToEnd();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        getAnalizator = serializer.Deserialize<GetAnalizator>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (getAnalizator != null)
            {
                listServices.Items.Add($"Пациент: {getAnalizator.patient}");

                for (int i = 0; i < getAnalizator.Услугиces.Count; i++)
                {
                    if (getAnalizator.Услугиces[i].Состояние_услуги != null)
                    {
                        listServices.Items.Add($"Услуга: {getAnalizator.Услугиces[i].Код_услуги}");
                        listServices.Items.Add($"Результат: {getAnalizator.Услугиces[i].Состояние_услуги}");

                        int index = ord.Код_заказа;
                        int indexServices = getAnalizator.Услугиces[i].Код_услуги;
                        Услуги s = Услуги.Find(x => x.Код_услуги == indexServices);
                        Статус_Услуги ss = db.Статус_Услуги.Where(x => x.Код_статуса_услуги == index && x.Код_статуса_услуги == s.Код_услуги).FirstOrDefault();
                        ss.Состояние_услуги = getAnalizator.Услугиces[i].Состояние_услуги;
                        db.SaveChanges();
                        analizator.Stop();
                    }
                    else
                    {
                        listServices.Items.Add($"Услуга {getAnalizator.Услугиces[i].Код_услуги} не готова");
                    }
                }
                if (getAnalizator.progress == 100)
                {
                    analizator.Stop();
                    rb1.IsEnabled = true;
                    rb2.IsEnabled = true;
                }
            }
        }
        private void Analizator_Tick(object sender, EventArgs e)
        {
            GetInfo();
        }
     

      
    }
}
