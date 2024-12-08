using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
            int maxFacesCount = 100;
            int measureCount = 3;
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

            MessageBox.Show($"Результаты исследования зависимости времени визуализации от количества полигонов сохранены в {csvFilePath}\nОбщее время исследования: {totalTime} сек");
        }

        public static void ResearchTimeByModels()
        {
            Stopwatch overallStopwatch = new Stopwatch();
            overallStopwatch.Start();

            Stopwatch stopwatch = new Stopwatch();
            double totalMilliseconds3 = 0;
            double totalMilliseconds6 = 0;
            double totalMilliseconds9 = 0;
            double averageTime3 = 0;
            double averageTime6 = 0;
            double averageTime9 = 0;

            PictureBox pb = new PictureBox();
            Canvas canvas3Faces = new Canvas(new Size(1396, 512), pb.CreateGraphics());
            Canvas canvas6Faces = new Canvas(new Size(1396, 512), pb.CreateGraphics());
            Canvas canvas9Faces = new Canvas(new Size(1396, 512), pb.CreateGraphics());

            Model model3Faces = new Model { Height = 20, FacesCount = 3 };
            Model model6Faces = new Model { Height = 20, FacesCount = 6 };
            Model model9Faces = new Model { Height = 20, FacesCount = 9 };

            int minModels = 1;
            int maxModels = 100;
            int measureCount = 3;
            RenderMode renderMode = RenderMode.Shadows;

            canvas3Faces.Render(renderMode);
            canvas6Faces.Render(renderMode);
            canvas9Faces.Render(renderMode);

            string csvFilePath = "TimeByModels.csv";

            using (StreamWriter writer = new StreamWriter(csvFilePath))
            {
                writer.WriteLine("ModelsCount,AverageTime3(ms),AverageTime6(ms),AverageTime9(ms),");

                for (int modelsCount = minModels; modelsCount <= maxModels; modelsCount++)
                {
                    canvas3Faces.AddModel(model3Faces.Copy());
                    canvas6Faces.AddModel(model6Faces.Copy());
                    canvas9Faces.AddModel(model9Faces.Copy());

                    // 3

                    totalMilliseconds3 = 0;

                    for (int i = 0; i < measureCount; i++)
                    {
                        stopwatch.Reset();
                        stopwatch.Start();

                        canvas3Faces.Render(renderMode);

                        stopwatch.Stop();
                        totalMilliseconds3 += stopwatch.Elapsed.TotalMilliseconds;
                    }

                    averageTime3 = totalMilliseconds3 / measureCount;

                    // 6

                    totalMilliseconds6 = 0;

                    for (int i = 0; i < measureCount; i++)
                    {
                        stopwatch.Reset();
                        stopwatch.Start();

                        canvas6Faces.Render(renderMode);

                        stopwatch.Stop();
                        totalMilliseconds6 += stopwatch.Elapsed.TotalMilliseconds;
                    }

                    averageTime6 = totalMilliseconds6 / measureCount;

                    // 9

                    totalMilliseconds9 = 0;

                    for (int i = 0; i < measureCount; i++)
                    {
                        stopwatch.Reset();
                        stopwatch.Start();

                        canvas9Faces.Render(renderMode);

                        stopwatch.Stop();
                        totalMilliseconds9 += stopwatch.Elapsed.TotalMilliseconds;
                    }

                    averageTime9 = totalMilliseconds9 / measureCount;

                    writer.WriteLine($"{modelsCount},{averageTime3},{averageTime6},{averageTime9},");
                }
            }

            overallStopwatch.Stop();
            double totalTime = overallStopwatch.Elapsed.TotalSeconds;

            MessageBox.Show($"Результаты исследования зависимости времени визуализации от количества моделей сохранены в {csvFilePath}\nОбщее время исследования: {totalTime} сек");
        }

        public static void ResearchTimeBySize()
        {
            Stopwatch overallStopwatch = new Stopwatch();
            overallStopwatch.Start();

            Stopwatch stopwatch = new Stopwatch();
            double totalMilliseconds = 0;

            PictureBox pb = new PictureBox();
            Canvas canvas = new Canvas(new Size(1396, 512), pb.CreateGraphics());
            Cube cube = new Cube();

            int minSize = 1;
            int maxSize = 20;
            int measureCount = 5;
            RenderMode renderMode = RenderMode.Shadows;

            canvas.AddModel(cube);
            canvas.Render(renderMode);

            string csvFilePath = "TimeBySize.csv";

            using (StreamWriter writer = new StreamWriter(csvFilePath))
            {
                writer.WriteLine("Size,AverageTime(ms)");

                for (int size = minSize; size <= maxSize; size++)
                {
                    canvas.Model(0).Length = size;

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

                    writer.WriteLine($"{size},{averageTime}");
                }
            }

            overallStopwatch.Stop();
            double totalTime = overallStopwatch.Elapsed.TotalSeconds;

            MessageBox.Show($"Результаты исследования зависимости времени визуализации от размера примитива сохранены в {csvFilePath}\nОбщее время исследования: {totalTime} сек");
        }
    }
}
