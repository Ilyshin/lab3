using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void DrawHeatMap()
        {
            double[,] data = Program.GetArray();
            int maxRow = data.GetLength(0);
            int maxCol = data.GetLength(1);
            double[] tempAr = new double[data.GetLength(0) *data.GetLength(1)];
            for (int i = 0; i<tempAr.Length; i++)
                {
                    tempAr[i] = data[i/10, i%10];
                }
                Array.Sort(tempAr);
            //}
            double factor = 60;        // коэфицинет, отвечающий за перевод значения потенциала в цвет на тепловой карте 

            DGV.RowHeadersVisible = false;
            DGV.ColumnHeadersVisible = false;
            DGV.AllowUserToAddRows = false;
            DGV.AllowUserToOrderColumns = false;
            DGV.ReadOnly = true;
            DGV.CellBorderStyle = DataGridViewCellBorderStyle.None;

            int rowHeight = DGV.ClientSize.Height / maxRow - 1;
            int colWidth = DGV.ClientSize.Width / maxCol - 1;

            for (int c = 0; c < maxRow; c++)
            {
                DGV.Columns.Add(c.ToString(), "");
                DGV.Columns[c.ToString()].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            for (int c = 0; c < maxRow; c++) DGV.Columns[c].Width = colWidth;
            DGV.Rows.Add(maxRow+5);
            for (int r = 0; r < maxRow; r++) DGV.Rows[r].Height = rowHeight;

            List<Color> baseColors = new List<Color>();  
            baseColors.Add(Color.RoyalBlue);
            baseColors.Add(Color.LightSkyBlue);
            baseColors.Add(Color.LightGreen);
            baseColors.Add(Color.Yellow);
            baseColors.Add(Color.Orange);
            baseColors.Add(Color.Red);
            List<Color> colors = interpolateColors(baseColors, 1000);

            for (int r = 0; r < maxRow; r++)
            {
                for (int c = 0; c < maxCol; c++)
                {
                    var a = Convert.ToInt16((Math.Abs(data[r, c])) * factor);
                    DGV[r, c].Style.BackColor =
                                   colors[a];
                    DGV[r, c].Value = Math.Round(data[r, c], 4).ToString();
                    DGV.Font = new Font("Consolas", 9f);


                }
            }
            DGV.DefaultCellStyle.SelectionBackColor = BackColor;
        }
        List<Color> interpolateColors(List<Color> stopColors, int count)
        {
            SortedDictionary<float, Color> gradient = new SortedDictionary<float, Color>();
            for (int i = 0; i < stopColors.Count; i++)
                gradient.Add(1f * i / (stopColors.Count - 1), stopColors[i]);
            List<Color> ColorList = new List<Color>();

            using (Bitmap bmp = new Bitmap(count, 1))
            using (Graphics G = Graphics.FromImage(bmp))
            {
                Rectangle bmpCRect = new Rectangle(Point.Empty, bmp.Size);
                LinearGradientBrush br = new LinearGradientBrush
                                        (bmpCRect, Color.Empty, Color.Empty, 0, false);
                ColorBlend cb = new ColorBlend();
                cb.Positions = new float[gradient.Count];
                for (int i = 0; i < gradient.Count; i++)
                    cb.Positions[i] = gradient.ElementAt(i).Key;
                cb.Colors = gradient.Values.ToArray();
                br.InterpolationColors = cb;
                G.FillRectangle(br, bmpCRect);
                for (int i = 0; i < count; i++) ColorList.Add(bmp.GetPixel(i, 0));
                br.Dispose();
            }
            return ColorList;
        }

        private void Form1_Load(object sender, EventArgs e)
           
        {
            DrawHeatMap();

        }

    }
}
