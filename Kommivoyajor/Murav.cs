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
    class Murav
    {
        Point[] mas;
        List<Point> points = new List<Point>();
        Point first_point;
        int mur_count;
        Hashtable feramons;
        double alfa, betta, q, pamyat;
        public Murav(Point[] mas, int mur_count, Hashtable feramons, double alfa, double betta, double q, double pamyat)
        {
            points.AddRange(mas);
            points.Remove(points[0]);
            first_point = mas[0];
            this.mur_count = mur_count;
            this.feramons = feramons;
            this.alfa = alfa;
            this.betta = betta;
            this.q = q;
            this.pamyat = pamyat;
            this.mas = mas;
        }

        public List<Point> DoIt()
        {
            List<Point> res = new List<Point>();

            for(int mur = 0; mur < mur_count; mur++)
            {
                List<Point> temp = points.GetRange(0, points.Count);
                res.Clear();


                while (temp.Count > 0) // 2............................
                {
                    Point point_now = mas[0];

                    List<double> veroyatnosti = new List<double>();
                    double znam = 0;
                    foreach (Point p in temp) // Нахождение Числителей и знаменателя
                    {
                        double n = Math.Pow(1 / L2(point_now, p), betta);

                        double fer;
                        if (feramons.ContainsKey(point_now.ToString() + p.ToString()))  // Поиск ферамона
                            fer = double.Parse(feramons[point_now.ToString() + p.ToString()].ToString());
                        else
                            fer = double.Parse(feramons[p.ToString() + point_now.ToString()].ToString());

                        double tau = Math.Pow(fer, alfa);
                        double chisl = n * tau;
                        veroyatnosti.Add(chisl);
                        znam += chisl;
                    }
                    for(int i = 0; i < veroyatnosti.Count; i++) // Нахождение вероятности перехода в новый город
                    {
                        veroyatnosti[i] = veroyatnosti[i] * 100 / znam;
                    }

                    double dtemp = 0;
                    int r = new Random().Next(0, 100);
           //         MessageBox.Show("Random = " + r);
                    for (int i = 0; i < veroyatnosti.Count; i++) // Выбор следующего города и переход в него
                    {
                        dtemp += veroyatnosti[i];
            //            MessageBox.Show("dtemp = " + dtemp);
                        if (dtemp > r)
                        {
                            point_now = temp[i]; // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Под вопросом
                            res.Add(temp[i]);
                            temp.Remove(temp[i]);

                            break;
                        }
                    }

                }
                
                //     5. Нахождение длины
                res.Insert(0, first_point); // добавление 1ого города в начало и конец
                res.Add(first_point);

                double l = 0;
                for(int i = 0; i < res.Count - 1; i++) // Нахождение длины l
                {
                    l += L2(res[i], res[i + 1]);
                }
                

                double deltaTau = q / l; // Определение deltaTau

                for(int i = 0; i < points.Count; i++) // Перебор всех ферамонов
                {
                    for(int j = i + 1; j < points.Count + 1; j++)
                    {
                        if (feramons.ContainsKey(mas[i].ToString() + mas[j].ToString()))
                            feramons[mas[i].ToString() + mas[j].ToString()] = double.Parse(feramons[mas[i].ToString() + mas[j].ToString()].ToString()) * (1 - pamyat);
                        else
                            MessageBox.Show("Не найден ферамон");
                    }
                }

                for(int i = 0; i < res.Count - 1; i++)
                {
                    double fer;
                    if (feramons.ContainsKey(res[i].ToString() + res[i + 1].ToString()))  // Поиск ферамона
                        feramons[res[i].ToString() + res[i + 1].ToString()] = double.Parse(feramons[res[i].ToString() + res[i + 1].ToString()].ToString()) + deltaTau;
                    else
                        feramons[res[i + 1].ToString() + res[i].ToString()] = double.Parse(feramons[res[i + 1].ToString() + res[i].ToString()].ToString()) + deltaTau;
                }

            }

            return res;
        }

        public double L2(Point p1, Point p2)
        {
            double temp = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

            return temp;
        }


    }
}
