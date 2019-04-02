using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulanik_Mantik
{

    public class FuzzyLogicCore
    {
        public enum KESISIM
        {
            HASSASLIK,
            MIKTAR,
            KIRLILIK
        }

        public static PointF GetCentroid(List<PointF> poly)
        {
            float accumulatedArea = 0.0f;
            float centerX = 0.0f;
            float centerY = 0.0f;

            for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                float temp = poly[i].X * poly[j].Y - poly[j].X * poly[i].Y;
                accumulatedArea += temp;
                centerX += (poly[i].X + poly[j].X) * temp;
                centerY += (poly[i].Y + poly[j].Y) * temp;
            }

            if (Math.Abs(accumulatedArea) < 1E-7f)
                return PointF.Empty;

            accumulatedArea *= 3f;
            return new PointF(centerX / accumulatedArea, centerY / accumulatedArea);
        }

        public PointF FindIntersection(PointF s1, PointF e1, PointF s2, PointF e2)
        {
            float a1 = e1.Y - s1.Y;
            float b1 = s1.X - e1.X;
            float c1 = a1 * s1.X + b1 * s1.Y;

            float a2 = e2.Y - s2.Y;
            float b2 = s2.X - e2.X;
            float c2 = a2 * s2.X + b2 * s2.Y;

            float delta = a1 * b2 - a2 * b1;
            //If lines are parallel, the result will be (NaN, NaN).
            return delta == 0 ? new PointF(float.NaN, float.NaN)
                : new PointF((b2 * c1 - b1 * c2) / delta, (a1 * c2 - a2 * c1) / delta);
        }


        public List<double> KesisimHesapla(double d, KESISIM k)
        {
            switch (k)
            {
                case KESISIM.HASSASLIK:
                case KESISIM.MIKTAR:
                    return HassaslikVeMiktarKesisim(d);
                    break;
                case KESISIM.KIRLILIK:
                    return KirlilikKesisim(d);
                    break;
            }
            return null;
        }
        private List<double> HassaslikVeMiktarKesisim(double d)
        {
            // [-4, -1.5, 2, 4] - [3, 5, 7] - [5.5, 8, 12.5, 14]
            List<double> kesisimler= new List<double>();
            double d1, d2, d3;
            d1 = -1;
            d2 = -1;
            d3 = -1;


            if (d >= 0 && d <= 2)
                d1 = 1;
            else if (d >= 2 && d <= 4)
                d1 = 1 - (d - 2) * (1 / Math.Abs((2.0) - (4)));

            if (d >= 3 && d <= 5)
                d2 = (d - 3) * (1 / Math.Abs((3.0) - (5.0)));
            else if (d >= 5 && d <= 7)
                d2 = 1 - (d - 5) * (1 / Math.Abs((5.0) - (7.0)));


            if (d >= 5.5 && d <= 8)
                d3 = (d-5.5) * (1 / Math.Abs((5.5) - (8)));
            else if (d >= 8 && d <= 12.5)
                d3 = 1;
            else if (d >= 12.5 && d <= 14)
                d3 = 1 - ((d-12.5) * (1 / Math.Abs((12.5) - (14.0))));

            if (d1>-1) kesisimler.Add(d1);
            if (d2>-1) kesisimler.Add(d2);
            if (d3>-1) kesisimler.Add(d3);
            if(kesisimler.Count==0) kesisimler.Add(0);
            return kesisimler;
        }
        private List<double> KirlilikKesisim(double d)
        {
            // [-4.5, -2.5, 2, 4.5] - [3, 5, 7] - [5.5, 8, 12.5, 15]
            List<double> kesisimler= new List<double>();
            double d1, d2, d3;
            d1 = -1;
            d2 = -1;
            d3 = -1;
            
            if (d >= 0 && d <= 2)
                d1 = 1;
            else if (d >= 2 && d <= 4.5)
                d1 = 1 - (d - 2) * (1 / Math.Abs((2.0) - (4.5)));

            if (d >= 3 && d <= 5)
                d2 = (d - 3) * (1 / Math.Abs((3.0) - (5.0)));
            else if (d >= 5 && d <= 7)
                d2 = 1 - (d - 5) * (1 / Math.Abs((5.0) - (7.0)));

            if (d >= 5.5 && d <= 8)
                d3 = (d-5.5) * (1 / Math.Abs((5.5) - (8)));
            else if (d >= 8 && d <= 12.5)
                d3 = 1;
            else if (d >= 12.5 && d <= 15)
                d3 = 1 - ((d-12.5)* (1 / Math.Abs((12.5) - (15.0))));
            
            if (d1>-1) kesisimler.Add(d1);
            if (d2>-1) kesisimler.Add(d2);
            if (d3>-1) kesisimler.Add(d3);
            if (kesisimler.Count == 0) kesisimler.Add(0);
            return kesisimler;
        }

    }
}
