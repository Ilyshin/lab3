using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        private static double[,] FindNewArea(double[,] area)
        {
            var newArea = new double[area.GetLength(0), area.GetLength(1)];
                
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (i == 0 || j == 0 || i == 9 || j == 9)
                        newArea[i, j] = area[i,j];
                    else
                    newArea[i, j] = 0.25 * (area[i + 1, j] + area[i - 1, j] + area[i, j + 1] + area[i, j - 1]);
                }
            }
            return newArea;
        }

        public static double[,] GetArray ()
        {
            Random rnd = new Random();
            var area = new double[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (i == 0 ||i == 9)
                        area[i, j] = 5; // Граничные значения изменял в зависимости от задания.
                    else if (j == 0 || j == 9)
                        area[i, j] = 10;
                    else if (i == 5 && j == 5)
                        area[i, j] = 0;

                    else area[i, j] = 0;  
                }
            }
            int count = 0;
            int count2 = 0;

            foreach (var item in area)
            {
                if (Math.Abs(10 - item) / 10 < 0.01)
                    count2 += 1;
            }

            while (count2 < 100)
            {
               // if (count == 100) // Буду останавливать усреднение, на опредёлнном количестве итераций для описания поведения системы.
                //    break;
                count2 = 0;
                var newarea = FindNewArea(area);

                for (int i = 0; i < area.GetLength(0); i++)
                {
                    for (int j = 0; j < area.GetLength(1); j++)
                    {
                        if (Math.Abs(newarea[i, j] - area[i, j]) / area[i, j] < 0.000001)
                            count2 += 1;
                       
                    }
                }
                area = FindNewArea(area);
                /* foreach (var item in area)
                {
                    if (Math.Abs(10 - item) / 10 < 0.01) ищу количество итераций для определённой точности
                        count2 += 1;
                }*/
                count += 1;
            }
            return area;
        }

        static void Main()
        {

             Application.EnableVisualStyles();
           Application.SetCompatibleTextRenderingDefault(false);
           Application.Run(new Form1());
        }
    }
}
