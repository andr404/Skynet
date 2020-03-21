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
    class Perebor
    {
        int dec = 1;

        Point[] mas;
        public Perebor(Point[] mas)
        {
            this.mas = new Point[mas.Length - 1];
            for(int i = 0; i < this.mas.Length; i++)
            {
                this.mas[i] = mas[i + 1];
            }
        }


        public Point[] DoIt()
        {
            while(true)
            {

            }
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
