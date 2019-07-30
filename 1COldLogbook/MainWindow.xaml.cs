using System;
using System.Collections.Generic;
using System.IO;
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

namespace _1COldLogbook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // чтение из файла
            string readPath = @"C:\Program Files\1cv8\srvinfo\reg_1541\1CV8Clst.lst";

            List<string> lines = new List<string>();
            //List<string> UID = new List<string>();
            Dictionary<string, string> name_UID = new Dictionary<string, string>();
            List<string[]> UID = new List<string[]>();
            try
            {
                // считываем построчно в list<string> lines
                using (StreamReader sr = new StreamReader(readPath, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("MSSQLServer"))// || line.Contains("00010101000000"))
                        {
                            lines.Add(line);
                        }
                    }
                }//----------------------------------------
                //получим из файла UID и имя базы 
                foreach (string s in lines)
                {
                    string[] words = s.Split(new char[] { '"', ',', '"' }, StringSplitOptions.RemoveEmptyEntries); // вычленяем 
                    //UID.Add(words[0].Remove(0,1)); //UID базы содержится в первом элементе массива, также удаляем первый символ - {
                    name_UID.Add(words[0].Remove(0, 1), words[1]);  //UID базы содержится в первом элементе массива, также удаляем первый символ - {
                }//-------------------------

                if (!Directory.Exists(@"C:\Program Files\1cv8\srvinfo\moved logbooks new format"))
                {
                    Directory.CreateDirectory(@"C:\Program Files\1cv8\srvinfo\moved logbooks new format");
                }

                foreach (KeyValuePair<string, string> name_uid in name_UID)
                {
                    string dirName = @"C:\Program Files\1cv8\srvinfo\reg_1541\"+name_uid.Key+ @"\1Cv8Log";

                    //перемещаем журнал нового формата (*.lgd) во временный каталог и создаем пустой файл журнала старого формата (*.lgf)
                    string[] files = Directory.GetFiles(dirName,"*.lgd");
                    if (files.Length != 0)  //если в каталоге есть файлы журнала нового формата (*.lgd)
                    {
                        foreach (string filepath in files)
                        {
                            if (!Directory.Exists(@"C:\Program Files\1cv8\srvinfo\moved logbooks new format\" + name_uid.Value+"_"+name_uid.Key+ @"\1Cv8Log"))
                            {
                                Directory.CreateDirectory(@"C:\Program Files\1cv8\srvinfo\moved logbooks new format\" + name_uid.Value + "_" + name_uid.Key + @"\1Cv8Log");
                            }
                            //if (File.Exists(@"C:\Program Files\1cv8\srvinfo\moved logbooks new format\" + uid + @"\1Cv8.lgd"))
                            //{
                            //    File.Delete(@"C:\Program Files\1cv8\srvinfo\moved logbooks new format\" + uid + @"\1Cv8.lgd");
                            //}
                            File.Move(filepath, @"C:\Program Files\1cv8\srvinfo\moved logbooks new format\" + name_uid.Value + "_" + name_uid.Key + @"\1Cv8Log\1Cv8.lgd"); //перемещаем журнал нового формата (*.lgd) во временный каталог
                        }
                        File.Create(dirName+ @"\1Cv8.lgf"); //создаем пустой файл старого формата
                    }//------------------------------------------------------------------
                }
                MessageBox.Show("Для всех баз журнал регистрации переведен в старый формат!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
