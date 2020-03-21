using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kommivoyajor
{
    class Otjog
    {
        public Point[] mas;
        double t, koef, end_koef;

        public Otjog(Point[] mas, double t, double koef, double end_koef)
        {
            this.t = t;
            this.koef = koef;
            this.end_koef = end_koef;

            this.mas = new Point[mas.Length + 1];
            for(int i = 0; i < mas.Length; i++)
            {
                this.mas[i] = mas[i];
            }
            this.mas[this.mas.Length - 1] = mas[0];
        }

        bool stop = false;
        public Point[] DoIt()
        {
            while(t >= end_koef && !stop)
            {
                Point[] mas_old = new Point[mas.Length];

                for(int i = 0; i < mas.Length; i++)
                {
                    mas_old[i] = mas[i];
                }

                double l1 = L();
                RandomChange();
                double l2 = L();

             //   MessageBox.Show("l1 = " + l1);
             //   MessageBox.Show("l2 = " + l2);

                if (l2 < l1)
                    continue;
                else
                {
                    double delta = l2 - l1;
                    double p = 100 * Math.Pow(Math.E, delta / t * -1);
                    int r = new Random().Next(0, 100);

                //    MessageBox.Show("p = " + p.ToString());
                //    MessageBox.Show("r = " + r.ToString());

                    if (p > r)
                    {
                        t *= koef;
                    }
                    else
                    {
                        mas = mas_old;
                        t *= koef;
                    }
                    if (p == 0)
                        stop = true;
                }

            }
            return mas;
        }



        public void RandomChange()
        {
            Point temp;
            RandomNotRepeat r = new RandomNotRepeat(1, mas.Length - 1);
            int r1 = r.Next();
            int r2 = r.Next();

                temp = mas[r1];
                mas[r1] = mas[r2];
                mas[r2] = temp;

        }


        public double L() // Сумма пути для всех городов
        {
            double temp = 0;

            for (int i = 0; i < mas.Length - 1; i++)
            {
                temp += Math.Sqrt(Math.Pow((mas[i + 1].X - mas[i].X), 2) + Math.Pow((mas[i + 1].Y - mas[i].Y), 2));
            }


            return temp;
        }



    }
}
