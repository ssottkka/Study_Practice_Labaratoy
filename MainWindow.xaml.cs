using System;
using System.Collections.Generic;
using System.Globalization;
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
using task1.pages;
using CaptchaGen;
using Mail_LIB;
using System.ComponentModel.DataAnnotations;
using System.Windows.Threading;
using System.Runtime.Remoting.Contexts;
using System.Windows.Interop;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Media.Media3D;

namespace task1
{
    public partial class MainWindow : Window
    {
       // private int user;
        public MainWindow()
        {
            InitializeComponent();

        }
        bazdanEntities db = new bazdanEntities();
      

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //using (var context = new bazdanEntities())
            //{
            //    var bok = db.Сотрудники.FirstOrDefault(x => x.Логин == loginBox.Text && x.Пароль == pwdPasswordBox.Password && x.Роль_пользователя.Код_роли == 1);
            //    if (bok != null)
            //    {
            //        int bookerID = bok.Код_сотрудника;
            //        booker booker = new booker(bookerID);
            //        booker.Show();
            //        this.Close();
            //    }

            //    //var lab_assis = db.Сотрудники.FirstOrDefault(x => x.Логин == loginBox.Text && x.Пароль == pwdPasswordBox.Password && x.Роль_пользователя.Код_роли == 2);
            //    //if(lab_assis != null)
            //    //{
            //    //    int labAssisID = lab_assis.Код_сотрудника;

            //    //}
            //}

            using (var context = new bazdanEntities())
            {
                var bok = db.Сотрудники.FirstOrDefault(x => x.Логин == loginBox.Text && x.Пароль == pwdPasswordBox.Password && x.Роль_пользователя.Код_роли == 1);

                if (bok != null)
                {
                    int bookerID = bok.Код_сотрудника;
                    booker booker = new booker(bookerID);
                    booker.Show();
                    this.Hide();
                    using (var db = new bazdanEntities())
                    {
                        var historyEntry = new История
                        {
                            login = loginBox.Text,
                            date = DateTime.Now.ToString(),
                            enter_status = "Успешно"
                        };
                        //context.История.Add(historyEntry);
                        //context.SaveChanges();
                    }
                }




                var lab_assis = db.Сотрудники.FirstOrDefault(x => x.Логин == loginBox.Text && x.Пароль == pwdPasswordBox.Password && x.Роль_пользователя.Код_роли == 2);
                if (lab_assis != null)
                {
                    int labAssisID = lab_assis.Код_сотрудника;
                    Lab_Assistant lab_Assistant = new Lab_Assistant(labAssisID);
                    lab_Assistant.Show();
                    this.Hide();
                    using (var db = new bazdanEntities())
                    {
                        var historyEntry = new История
                        {
                            login = loginBox.Text,
                            date = DateTime.Now.ToString(),
                            enter_status = "Успешно"
                        };
                        //context.История.Add(historyEntry);
                        //context.SaveChanges();
                    }
                }








                var lab = db.Сотрудники.FirstOrDefault(x => x.Логин == loginBox.Text && x.Пароль == pwdPasswordBox.Password && x.Роль_пользователя.Код_роли == 3);
                if (lab != null)
                {
                    int labManID = lab.Код_сотрудника;
                    Lab_Man lab_Man = new Lab_Man(labManID);
                    lab_Man.Show();
                    this.Hide();
                    using (var db = new bazdanEntities())
                    {
                        var historyEntry = new История
                        {
                            login = loginBox.Text,
                            date = DateTime.Now.ToString(),
                            enter_status = "Успешно"
                        };
                        //context.История.Add(historyEntry);
                        //context.SaveChanges();
                    }
                }

                var adm = db.Сотрудники.FirstOrDefault(x => x.Логин == loginBox.Text && x.Пароль == pwdPasswordBox.Password && x.Роль_пользователя.Код_роли == 4);
                if (adm != null)
                {
                    enter.IsEnabled = true;
                    int adminID = adm.Код_сотрудника;
                    admin admin = new admin(adminID);
                    admin.Show();
                    this.Hide();
                    using (var db = new bazdanEntities())
                    {
                        var historyEntry = new История
                        {
                            login = loginBox.Text,
                            date = DateTime.Now.ToString(),
                            enter_status = "Успешно"
                        };
                        //context.История.Add(historyEntry);
                        //context.SaveChanges();
                    }
                }
                if (bok == null && lab_assis == null && lab == null && adm == null)
                {
                    Captcha.Source = GenerateCaptcha(100, 100);
                    textBox12.Visibility = Visibility.Visible;
                    Captcha.Visibility = Visibility.Visible;
                    Captchabutton.Visibility = Visibility.Visible;
                    MessageBox.Show("Неверный логин или пароль введите капчу");
                    using (var db = new bazdanEntities())
                    {
                        var historyEntry = new История
                        {
                            login = loginBox.Text,
                            date = DateTime.Now.ToString(),
                            enter_status = "Неуспешно"
                        };
                        //context.История.Add(historyEntry);
                        //context.SaveChanges();
                    }
                }
            }
            Mail_LIB.Validation validation = new Mail_LIB.Validation();
            bool isValidLogin = validation.Check_login(loginBox.Text);
            if (!isValidLogin)
            {
                MessageBox.Show("Ошибка: некорректный логин !");
            }

            bool isValidPassword = validation.Check_password(pwdPasswordBox.Password);
            if (!isValidPassword)
            {
                MessageBox.Show("Ошибка: Некорректный пароль !");
            }


            //analiz analiz = new analiz();
            //analiz.Show();
            //this.Close();
        }





























        private void CapBtn_Click(object sender, RoutedEventArgs e)
        {
          

        }






     





        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (char)value == '\0';
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? '\0' : '●';
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var passrod = checkBoxPwd.IsChecked.Value ? pwdTextBox.Text : pwdPasswordBox.Password;
            MessageBox.Show(passrod);
        }











        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                pwdTextBox.Text = pwdPasswordBox.Password;
                pwdTextBox.Visibility = Visibility.Visible; //  отобразить
                pwdPasswordBox.Visibility = Visibility.Hidden; //   скрыть
            }
            else
            {
                pwdPasswordBox.Password = pwdTextBox.Text; // скопируем в PasswordBox из TextBox 
                pwdTextBox.Visibility = Visibility.Hidden; // TextBox - скрыть
                pwdPasswordBox.Visibility = Visibility.Visible; // PasswordBox - отобразить
            }
        }
        private void cap_Click(object sender, RoutedEventArgs e)
        {

        }
        private const string CaptchaText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly Random _random = new Random();

        public BitmapSource GenerateCaptcha(int width, int height)
        {
            using (var bitmap = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var font = new Font("Arial", 20, System.Drawing.FontStyle.Bold);
                var brush = new SolidBrush(System.Drawing.Color.Black);
                var pen = new System.Drawing.Pen(System.Drawing.Color.Black, 2);
                var captchaChars = new char[4];
                for (var i = 0; i < captchaChars.Length; i++)
                {
                    captchaChars[i] = CaptchaText[_random.Next(CaptchaText.Length)];
                }
                var captchaText = new string(captchaChars);
                for (int i = 0; i < captchaChars.Length; i++)
                {
                    int minHeight = (int)(0.2 * height);
                    int maxHeight = (int)(0.8 * height);
                    float charHeight = _random.Next(minHeight, maxHeight);

                    graphics.DrawString(captchaChars[i].ToString(), font, brush, i * font.Size, charHeight);
                }
                for (var i = 0; i < 5; i++)
                {
                    graphics.DrawLine(pen, _random.Next(width), _random.Next(height), _random.Next(width), _random.Next(height));
                }
                for (var x = 0; x < bitmap.Width; x++)
                {
                    for (var y = 0; y < bitmap.Height; y++)
                    {
                        if (_random.Next(10) < 3)
                        {
                            bitmap.SetPixel(x, y, System.Drawing.Color.Black);
                        }
                    }
                }
                var handle = bitmap.GetHbitmap();
                try
                {
                    return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(handle);
                }
            }
        }















        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {

        }










































        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }


        private void checkBoxPwd_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void s_Click(object sender, RoutedEventArgs e)
        {

        }
        private void loginBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
