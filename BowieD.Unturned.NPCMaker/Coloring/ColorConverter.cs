﻿using System;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Coloring
{
#pragma warning restore CS0661
#pragma warning restore CS0660
    public static class ColorConverter
    {
        public static Color HSVtoColor(double H, double S, double V)
        {
            int hi = Convert.ToInt32(Math.Floor(H / 60)) % 6;
            double f = H / 60 - Math.Floor(H / 60);

            V *= 255;
            byte v = (byte)Convert.ToInt32(V);
            byte p = (byte)Convert.ToInt32(V * (1 - S));
            byte q = (byte)Convert.ToInt32(V * (1 - f * S));
            byte t = (byte)Convert.ToInt32(V * (1 - (1 - f) * S));

            if (hi == 0)
                return new Color(v, t, p);
            else if (hi == 1)
                return new Color(q, v, p);
            else if (hi == 2)
                return new Color(p, v, t);
            else if (hi == 3)
                return new Color(p, q, v);
            else if (hi == 4)
                return new Color(t, p, v);
            else
                return new Color(v, p, q);
        }
        public static Tuple<double, double, double> ColorToHSV(Color color)
        {
            double RR = color.R / 255d;
            double GG = color.G / 255d;
            double BB = color.B / 255d;
            double Cmax = new double[] { RR, GG, BB }.Max();
            double Cmin = new double[] { RR, GG, BB }.Min();
            double delta = Cmax - Cmin;
            double H = 0;
            if (delta == 0)
                H = 0;
            else if (Cmax == RR)
                H = 60d * (((GG - BB) / delta) % 6d);
            else if (Cmax == GG)
                H = 60d * (((BB - RR) / delta) + 2d);
            else if (Cmax == BB)
                H = 60d * (((RR - GG) / delta) + 4d);
            double S = 0;
            if (Cmax != 0)
                S = delta / Cmax;
            double V = Cmax;
            S = Math.Round(S, 2);
            V = Math.Round(V, 2);
            return new Tuple<double, double, double>(H, S, V);
        }
        public static Color HSLtoColor(double H, double S, double L)
        {
            double C = (1d - Math.Abs(2d * L - 1d)) * S;
            double X = C * (1d - Math.Abs((H / 60d) % 2d - 1d));
            double m = L - C / 2d;
            Tuple<double, double, double> preResult;
            if (H < 60)
                preResult = new Tuple<double, double, double>(C, X, 0);
            else if (H < 120)
                preResult = new Tuple<double, double, double>(X, C, 0);
            else if (H < 180)
                preResult = new Tuple<double, double, double>(0, C, X);
            else if (H < 240)
                preResult = new Tuple<double, double, double>(0, X, C);
            else if (H < 300)
                preResult = new Tuple<double, double, double>(X, 0, C);
            else
                preResult = new Tuple<double, double, double>(C, 0, X);
            return new Color((byte)((preResult.Item1 + m) * 255d), (byte)((preResult.Item2 + m) * 255d), (byte)((preResult.Item3 + m) * 255d));
        }
        public static Tuple<double, double, double> ColorToHSL(Color color)
        {
            double RR = color.R / 255d;
            double GG = color.G / 255d;
            double BB = color.B / 255d;
            double MAX = new double[] { RR, GG, BB }.Max();
            double MIN = new double[] { RR, GG, BB }.Min();
            double H = 0, S, L;
            if (MAX != MIN)
            {
                if (MAX == RR)
                {
                    if (GG >= BB)
                    {
                        H = 60 * ((GG - BB) / (MAX - MIN)) + 0;
                    }
                    else
                    {
                        H = 60 * ((GG - BB) / (MAX - MIN)) + 360;
                    }
                }
                else if (MAX == GG)
                {
                    H = 60 * ((BB - RR) / (MAX - MIN)) + 120;
                }
                else
                {
                    H = 60 * ((RR - GG) / (MAX - MIN)) + 240;
                }
            }
            L = (MAX + MIN) * 0.5;
            if (L == 0 || MAX == MIN)
            {
                S = 0;
            }
            else if (L <= 0.5)
            {
                S = ((MAX - MIN) / (MAX + MIN));
            }
            else if (L < 1)
            {
                S = ((MAX - MIN) / (2d - (MAX + MIN)));
            }
            else
            {
                S = ((MAX - MIN) / (1d - System.Math.Abs(1d - (MAX + MIN))));
            }
            S = Math.Round(S, 2);
            L = Math.Round(L, 2);
            return new Tuple<double, double, double>(H, S, L);
        }
        public static Color CMYKtoColor(double C, double M, double Y, double K)
        {
            return new Color((byte)(255 * (1 - C) * (1 - K)), (byte)(255 * (1 - M) * (1 - K)), (byte)(255 * (1 - Y) * (1 - K)));
        }
        public static Tuple<double, double, double, double> ColorToCMYK(Color color)
        {
            double RR = color.R / 255d;
            double GG = color.G / 255d;
            double BB = color.B / 255d;

            double K = 1d - new double[] { RR, GG, BB }.Max();
            double C = (1d - RR - K) / (1d - K);
            double M = (1d - GG - K) / (1d - K);
            double Y = (1d - BB - K) / (1d - K);
            // did not mention that on the website
            C = double.IsNaN(C) ? 0 : C;
            M = double.IsNaN(M) ? 0 : M;
            Y = double.IsNaN(Y) ? 0 : Y;
            return new Tuple<double, double, double, double>(C, M, Y, K);
        }
    }
}
