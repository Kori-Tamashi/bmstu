using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace code
{
    class TimeResearch
    {
        public static void ResearchTimeByPolygons()
        {
            Stopwatch overallStopwatch = new Stopwatch(); 
            overallStopwatch.Start();

            Stopwatch stopwatch = new Stopwatch();
            double totalMilliseconds = 0;

            PictureBox pb = new PictureBox();
            Canvas canvas = new Canvas(new Size(1396, 512), pb.CreateGraphics());
            Model model = new Model();

            int minFacesCount = 3;
            int maxFacesCount = 20;
            int measureCount = 1;
            RenderMode renderMode = RenderMode.Shadows;

            model.Height = 20;
            canvas.AddModel(model);
            canvas.Render(renderMode);

            string csvFilePath = "TimeByPolygons.csv";

            using (StreamWriter writer = new StreamWriter(csvFilePath))
            {
                writer.WriteLine("FacesCount,AverageTime(ms)");

                for (int facesCount = minFacesCount; facesCount <= maxFacesCount; facesCount++)
                {
                    canvas.Model(0).FacesCount = facesCount;

                    totalMilliseconds = 0;

                    for (int i = 0; i < measureCount; i++)
                    {
                        stopwatch.Reset();
                        stopwatch.Start();

                        canvas.Render(renderMode);

                        stopwatch.Stop();
                        totalMilliseconds += stopwatch.Elapsed.TotalMilliseconds;
                    }

                    double averageTime = totalMilliseconds / measureCount;

                    writer.WriteLine($"{facesCount + 2},{averageTime}");
                }
            }

            overallStopwatch.Stop(); 
            double totalTime = overallStopwatch.Elapsed.TotalSeconds;

            MessageBox.Show($"Результаты исследования зависимости времени визуализации от количества полигонов сохранены в {csvFilePath}\nОбщее время выполнения: {totalTime} сек");
        }
    }
}
