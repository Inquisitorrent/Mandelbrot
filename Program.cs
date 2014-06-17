using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Mandelbrot
{
    class Program
    {
        //Can move these to Main as parameters or as user input if desired.
        private const int maxIterations = 1400;
        private const int imageWidth = 19200;//19200;
        private const int imageHeight = 19200;//10800;
        private static double minPlotXDouble = -0.75061213d;
        private static double maxPlotXDouble = -0.72706540d;
        private static double minPlotYDouble = 0.15520929d;
        private static double maxPlotYDouble = 0.17753355d;
        private static double blockSizeXDouble = (maxPlotXDouble - minPlotXDouble) / imageWidth;
        private static double blockSizeYDouble = (maxPlotYDouble - minPlotYDouble) / imageHeight;

        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;
            int[,] iterations = new int[imageWidth, imageHeight];

            System.Threading.Tasks.Parallel.For(0, imageWidth, w =>
            {
                for (int h = 0; h < imageHeight; h++)
                {
                    double dx = (w * blockSizeXDouble) + minPlotXDouble;
                    double dy = maxPlotYDouble - (h * blockSizeYDouble);
                    iterations[w, h] = GetIterations(dx, dy);
                }
            });

            //Color and save
            Console.WriteLine("Writing data to image.");
            Bitmap bitmap = new Bitmap(imageWidth, imageHeight);
            byte red = 0, green = 0, blue = 0;
            for (int w = 0; w < imageWidth; w++)
            {
                for (int h = 0; h < imageHeight; h++)
                {
                    GetColorFromIterations(iterations[w, h], maxIterations, out red, out green, out blue);
                    bitmap.SetPixel(w, h, Color.FromArgb(red, green, blue));
                }
            }
            bitmap.Save(@"C:\dev\test_bitmap.bmp");
        }

        static void GetColorFromIterations(int iterations, int maxIterations, out byte red, out byte green, out byte blue)
        {
            // coloring code taken and adapted from http://www.codeproject.com/Articles/15000/Mandelbrot-in-C-and-Windows-forms
            
            double ratio = System.Convert.ToDouble(iterations) / System.Convert.ToDouble(maxIterations);
            red = 0;
            green = 0;
            blue = 0;

            if ((ratio >= 0D) && (ratio < 0.125))
            {
                red = (byte)((ratio / 0.125) * (512D) + 0.5);
                green = 0;
                blue = 0;
            }

            else if ((ratio >= 0.125) && (ratio < 0.250))
            {
                red = 255;
                green = (byte)(((ratio - 0.125) / 0.125) * (512D) + 0.5);
                blue = 0;
            }

            else if ((ratio >= 0.250) && (ratio < 0.375))
            {
                red = (byte)((1D - ((ratio - 0.250) / 0.125)) * (512D) + 0.5);
                green = 255;
                blue = 0;
            }

            else if ((ratio >= 0.375) && (ratio < 0.500))
            {
                red = 0;
                green = 255;
                blue = (byte)(((ratio - 0.375) / 0.125) * (512D) + 0.5);
            }

            else if ((ratio >= 0.500) && (ratio < 0.625))
            {
                red = 0;
                green = (byte)((1.0 - ((ratio - 0.500) / 0.125)) * (512D) + 0.5);
                blue = 255;
            }

            else if ((ratio >= 0.625) && (ratio < 0.750))
            {
                red = (byte)(((ratio - 0.625) / 0.125) * (512D) + 0.5);
                green = 0;
                blue = 255;
            }

            else if ((ratio >= 0.750) && (ratio < 0.875))
            {
                red = 255;
                green = (byte)(((ratio - 0.750) / 0.125) * (512D) + 0.5);
                blue = 255;
            }

            else if ((ratio >= 0.875) && (ratio <= 1.000))
            {
                red = (byte)((1D - ((ratio - 0.875) / 0.125)) * (512D) + 0.5);
                green = (byte)((1D - ((ratio - 0.875) / 0.125)) * (512D) + 0.5);
                blue = (byte)((1D - ((ratio - 0.875) / 0.125)) * (512D) + 0.5);
            }
        }

        static int GetIterations(double x, double y)
        {
            double Zx = 0d, Zy = 0d, ZSquaredx = 0d, ZSquaredy = 0d, Magnitudex = 0d, Magnitudey = 0d, Magnitude = 0d;
            int iteration = 0;
            while ((iteration < maxIterations) && (Magnitude < 4d))
            {
                ZSquaredx = (Zx * Zx) - (Zy * Zy);
                ZSquaredy = 2d * Zx * Zy;
                Magnitudex = ZSquaredx + x;
                Magnitudey = ZSquaredy + y;
                Magnitude = (Magnitudex * Magnitudex) + (Magnitudey * Magnitudey);
                Zx = Magnitudex;
                Zy = Magnitudey;
                iteration++;
            }

            return iteration;
        }
    }
}
