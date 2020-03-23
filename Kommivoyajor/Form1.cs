using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Kommivoyajor
{
    public partial class Form1 : Form
    {
        Button[] but_mas;
        Label[] lab_mas;
        int[] feramon_mas;
        int city_count;
        //
        int temp_count_city = 0;
        Point[] mas;
        public Graphics GF;

        //
        double t;
        double koef;
        double end_koef;
        //

        List<string> list_pereb = new List<string>();
        double pereb_minL = 1000000;
        Point[] pereb_best;


        public Form1()
        {
            InitializeComponent();
            but_mas = new Button[9] { button2, button3, button4, button5, button6, button7, button8, button9, button10 };
            lab_mas = new Label[9] { label2, label3, label4, label5, label6, label7, label8, label9, label10 };
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(int.TryParse(textBox1.Text, out city_count))
            {
                if (city_count < 4 || city_count > 9)
                    MessageBox.Show("Значение должно быть больше 3 и меньше 10");
                else
                {
                    city_count--;
                    textBox1.Enabled = false;
                    button1.Enabled = false;
                    pictureBox1.Enabled = true;
                    mas = new Point[city_count + 1];

                    feramon_mas = new int[city_count + 1];
                    Random r = new Random();


                }
            }
            else
                MessageBox.Show("Введите число!");
        }

        public double L2(Point p1, Point p2)
        {
            double temp = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

            return temp;
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (temp_count_city <= city_count)
            {
                Pen brush = new Pen(but_mas[temp_count_city].BackColor, 6);

                Point point = e.Location;
                mas[temp_count_city] = point;
                GF = pictureBox1.CreateGraphics();
                GF.DrawEllipse(brush, point.X, point.Y, 5, 5);

                lab_mas[temp_count_city].Text = point.ToString();


                temp_count_city++;

                if(temp_count_city > city_count)
                {
                    for(int i = temp_count_city; i < 9; i++)
                    {
                        lab_mas[i].Text = "*********************";
                    }
                    textBox2.Enabled = true;
                    textBox3.Enabled = true;
                    textBox4.Enabled = true;
                    button11.Enabled = true;

                    textBox5.Enabled = true;
                    textBox6.Enabled = true;
                    textBox7.Enabled = true;
                    textBox8.Enabled = true;
                    textBox9.Enabled = true;
                    button13.Enabled = true;

                    label24.Text += L(mas) + L2(mas[mas.Length - 1], mas[0]);
                    if (int.Parse(textBox1.Text) < 6)
                        button12.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Эй, нельзя больше добавлять!");
            }
            
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox2.Text, out t) && double.TryParse(textBox3.Text, out koef) && double.TryParse(textBox4.Text, out end_koef))
            {
                if (t <= 0)
                    MessageBox.Show("T должен быть больше 0");
                else if (koef <= 0 || koef > 0.99)
                    MessageBox.Show("Коэффициент Koef должен быть больше 0, но меньше 1");
                else if (end_koef <= 0)
                    MessageBox.Show("End должен быть больше 0");
                else
                {
                    ///////

                    Otjog a = new Otjog(mas, t, koef, end_koef);
                    Point[] res = a.DoIt();

                    string str = "";

                    foreach (Point p in res)
                        str += p.ToString() + "\n";

                    MessageBox.Show("Возможная минимальная длина: " + Math.Round(a.L(), 2).ToString());
                    MessageBox.Show(str);

                    ////////
                }
            }
            else
                MessageBox.Show("Ошибка, введите числа!");
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            Point[] mas_pereb = new Point[mas.Length - 1];
            pereb_best = new Point[mas.Length + 1];
            int[] temp_mas = new int[mas.Length - 1];
            for(int i = 0; i < mas_pereb.Length; i++)
            {
                mas_pereb[i] = mas[i + 1];
                temp_mas[i] = i;
            }


            ShowAllCombinations(temp_mas, ref list_pereb);
            

            foreach(var str in list_pereb)
            {
                Point[] mas_point = new Point[str.Length + 2];
                mas_point[0] = mas[0];
                mas_point[mas_point.Length - 1] = mas[0];


                for(int i = 0; i < str.Length; i++)
                {
                    int temp = int.Parse(str[i].ToString());
                    mas_point[i + 1] = mas_pereb[temp];

                }
                // Получен новый переб. массив с начальной и конечной точкой (mas_point)

                double l = L(mas_point);
                if(l < pereb_minL)
                {
                    pereb_minL = l;

                    for(int i = 0; i < mas_point.Length; i++)
                    {
                        pereb_best[i] = mas_point[i];
                    }

                }

            }
            label15.Text += pereb_best[0].ToString();
            label16.Text += pereb_best[1].ToString();
            label17.Text += pereb_best[2].ToString();
            label18.Text += pereb_best[3].ToString();
            label19.Text += pereb_best[4].ToString();
            if (pereb_best.Length == 6)
                label20.Text += pereb_best[5].ToString();
            else
                label20.Text = "";
            button12.Text = Math.Round(pereb_minL, 2).ToString();
            button12.Enabled = false;
            button12.BackColor = Color.White;
            MessageBox.Show("Минимальная длина = " + Math.Round(pereb_minL, 2));

        }

        public static double L(Point[] mas) // Сумма пути для всех городов
        {
            double temp = 0;

            for (int i = 0; i < mas.Length - 1; i++)
            {
                temp += Math.Sqrt(Math.Pow((mas[i + 1].X - mas[i].X), 2) + Math.Pow((mas[i + 1].Y - mas[i].Y), 2));
            }


            return temp;
        }


        public static void ShowAllCombinations<T>(IList<T> arr, ref List<string> list, string current = "")
        {
            if (arr.Count == 0) //если все элементы использованы, выводим на консоль получившуюся строку и возвращаемся
            {
                list.Add(current);
                return;
            }
            for (int i = 0; i < arr.Count; i++) //в цикле для каждого элемента прибавляем его к итоговой строке, создаем новый список из оставшихся элементов, и вызываем эту же функцию рекурсивно с новыми параметрами.
            {
                List<T> lst = new List<T>(arr);
                lst.RemoveAt(i);
                ShowAllCombinations(lst, ref list, current + arr[i].ToString());
            }
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            
            Hashtable hashtable = new Hashtable();
            Random r = new Random();

            for (int i = 0; i < mas.Length - 1; i++) 
            {
                for (int j = i + 1; j < mas.Length; j++)
                {
                    hashtable[mas[i].ToString() + mas[j].ToString()] = r.Next(1, 6);
                }
            }

            int mur_count;
            double alfa, betta, q, pamyat;

            if(!int.TryParse(textBox5.Text, out mur_count) || !double.TryParse(textBox6.Text, out alfa) || !double.TryParse(textBox7.Text, out betta) || !double.TryParse(textBox8.Text, out q) || !double.TryParse(textBox9.Text, out pamyat))
            {
                MessageBox.Show("Что-то пошло не так...");
            }
            else
            {
                Murav m = new Murav(mas, mur_count, hashtable, alfa, betta, q, pamyat);
                Point[] res = m.DoIt().ToArray();

                string str = "";

                foreach (Point p in res)
                    str += p.ToString() + "\n";

                MessageBox.Show("Возможная минимальная длина: " + Math.Round(L(res), 2).ToString());
                MessageBox.Show(str);
            }

        }
    }
}
