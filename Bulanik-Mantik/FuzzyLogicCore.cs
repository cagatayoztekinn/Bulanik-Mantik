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
        private List<string> _hassaslikList = new List<string>();
        private List<string> _miktarList = new List<string>();
        private List<string> _kirlilikList = new List<string>();

        public List<List<string>> KesisimList
        {
            get
            {
                List<List<string>> list = new List<List<string>>();
                list.Add(_hassaslikList);
                list.Add(_miktarList);
                list.Add(_kirlilikList);
                return list;
            }
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


        public List<double> KesisimHesapla(double d, KESISIM k, int sekilIndex = -1)
        {

            switch (k)
            {
                case KESISIM.HASSASLIK:
                    return HassaslikVeMiktarKesisim(d, k, sekilIndex);
                case KESISIM.MIKTAR:
                    return HassaslikVeMiktarKesisim(d, k, sekilIndex);
                case KESISIM.KIRLILIK:
                    return KirlilikKesisim(d, sekilIndex);
            }
            return null;
        }
        private List<double> HassaslikVeMiktarKesisim(double d, KESISIM k, int sekilIndex)
        {
            // [-4, -1.5, 2, 4] - [3, 5, 7] - [5.5, 8, 12.5, 14]

            var tempList = new List<string>();

            List<double> kesisimler = new List<double>();
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
                d3 = (d - 5.5) * (1 / Math.Abs((5.5) - (8)));
            else if (d >= 8 && d <= 12.5)
                d3 = 1;
            else if (d >= 12.5 && d <= 14)
                d3 = 1 - ((d - 12.5) * (1 / Math.Abs((12.5) - (14.0))));


            if (d1 > -1)
            {
                tempList.Add(k == KESISIM.HASSASLIK ? "Sağlam" : "Küçük");
                if (sekilIndex == -1 || sekilIndex == 0)
                    kesisimler.Add(d1);
            }

            if (d2 > -1)
            {
                tempList.Add("Orta");
                if (sekilIndex == -1 || sekilIndex == 1)
                    kesisimler.Add(d2);
            }

            if (d3 > -1)
            {
                tempList.Add(k == KESISIM.HASSASLIK ? "Hassas" : "Büyük");
                if (sekilIndex == -1 || sekilIndex == 2)
                    kesisimler.Add(d3);
            }

            if (k == KESISIM.HASSASLIK)
                _hassaslikList = tempList;
            else
                _miktarList = tempList;

            if (kesisimler.Count == 0) kesisimler.Add(-1);
            return kesisimler;
        }

        #region Tablo alanlarının Koordinatların hesaplanması
        public List<PointF> AreaHesapla(Enums.AgirlikMerkez outputMerkez, double y, int areaId)
        {
            switch (outputMerkez)
            {
                case Enums.AgirlikMerkez.Donus:
                    return AreaDonusHizi(y, areaId);
                case Enums.AgirlikMerkez.Deterjan:
                    return AreaDeterjan(y, areaId);
                case Enums.AgirlikMerkez.Sure:
                    return AreaSure(y, areaId);
            }
            return new List<PointF>();
        }
        private List<PointF> AreaDonusHizi(double y, int areaId)
        {
            double[] d1, d2, d3, d4, d5;
            d1 = new double[] { -5.8, -2.8, 0.5, 1.5 };
            d2 = new double[] { 0.5, 2.75, 5 };
            d3 = new double[] { 2.75, 5, 7.25 };
            d4 = new double[] { 5, 7.25, 9.5 };
            d5 = new double[] { 8.5, 9.5, 12.8, 15.2 };
            List<PointF> noktalar = new List<PointF>();
            double sonuc;

            if (areaId == 0)
            {
                noktalar.Add(new PointF((float)d1[0], 0));
                sonuc = d1[0] + (y * (Math.Abs(d1[0] - d1[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d1[3] - (y * (Math.Abs(d1[2] - d1[3])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d1[3], 0));
            }

            if (areaId == 1)
            {
                noktalar.Add(new PointF((float)d2[0], 0));
                sonuc = d2[0] + (y * (Math.Abs(d2[0] - d2[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d2[2] - (y * (Math.Abs(d2[1] - d2[2])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d2[2], 0));
            }

            if (areaId == 2)
            {
                noktalar.Add(new PointF((float)d3[0], 0));
                sonuc = d3[0] + (y * (Math.Abs(d3[0] - d3[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d3[2] - (y * (Math.Abs(d3[1] - d3[2])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d3[2], 0));
            }
            if (areaId == 3)
            {
                noktalar.Add(new PointF((float)d4[0], 0));
                sonuc = d4[0] + (y * (Math.Abs(d4[0] - d4[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d4[2] - (y * (Math.Abs(d4[1] - d4[2])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d4[2], 0));
            }

            if (areaId == 4)
            {
                noktalar.Add(new PointF((float)d5[0], 0));
                sonuc = d5[0] + (y * (Math.Abs(d5[0] - d5[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d5[3] - (y * (Math.Abs(d5[2] - d5[3])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d5[3], 0));
            }

            return noktalar;
        }
        private List<PointF> AreaDeterjan(double y, int areaId)
        {
            double[] d1, d2, d3, d4, d5;
            d1 = new double[] { 0, 0, 20, 85 };
            d2 = new double[] { 20, 85, 150 };
            d3 = new double[] { 85, 150, 215 };
            d4 = new double[] { 150, 215, 280 };
            d5 = new double[] { 215, 280, 300, 300 };
            List<PointF> noktalar = new List<PointF>();
            double sonuc;


            if (areaId == 0)
            {
                noktalar.Add(new PointF((float)d1[0], 0));
                noktalar.Add(new PointF((float)d1[1], (float)y));
                sonuc = d1[3] - (y * (Math.Abs(d1[2] - d1[3])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d1[3], 0));
            }

            if (areaId == 1)
            {
                noktalar.Add(new PointF((float)d2[0], 0));
                sonuc = d2[0] + (y * (Math.Abs(d2[0] - d2[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d2[2] - (y * (Math.Abs(d2[1] - d2[2])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d2[2], 0));
            }

            if (areaId == 2)
            {
                noktalar.Add(new PointF((float)d3[0], 0));
                sonuc = d3[0] + (y * (Math.Abs(d3[0] - d3[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d3[2] - (y * (Math.Abs(d3[1] - d3[2])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d3[2], 0));
            }
            if (areaId == 3)
            {

                noktalar.Add(new PointF((float)d4[0], 0));
                sonuc = d4[0] + (y * (Math.Abs(d4[0] - d4[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d4[2] - (y * (Math.Abs(d4[1] - d4[2])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d4[2], 0));
            }

            if (areaId == 4)
            {
                noktalar.Add(new PointF((float)d5[0], 0));
                sonuc = d5[0] + (y * (Math.Abs(d5[0] - d5[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d5[3] - (y * (Math.Abs(d5[2] - d5[3])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d5[3], 0));
            }

            return noktalar;
        }
        private List<PointF> AreaSure(double y, int areaId)
        {
            double[] d1, d2, d3, d4, d5;
            d1 = new double[] { -46.5, -25.28, 22.3, 39.9 };
            d2 = new double[] { 22.3, 39.9, 57.5 };
            d3 = new double[] { 39.9, 57.5, 75.1 };
            d4 = new double[] { 57.5, 75.1, 92.7 };
            d5 = new double[] { 75, 92.7, 111.6, 130 };
            List<PointF> noktalar = new List<PointF>();
            double sonuc;


            if (areaId == 0)
            {
                noktalar.Add(new PointF((float)d1[0], 0));
                sonuc = d1[0] + (y * (Math.Abs(d1[0] - d1[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d1[3] - (y * (Math.Abs(d1[2] - d1[3])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d1[3], 0));
            }

            if (areaId == 1)
            {
                noktalar.Add(new PointF((float)d2[0], 0));
                sonuc = d2[0] + (y * (Math.Abs(d2[0] - d2[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d2[2] - (y * (Math.Abs(d2[1] - d2[2])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d2[2], 0));
            }

            if (areaId == 2)
            {
                noktalar.Add(new PointF((float)d3[0], 0));
                sonuc = d3[0] + (y * (Math.Abs(d3[0] - d3[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d3[2] - (y * (Math.Abs(d3[1] - d3[2])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d3[2], 0));
            }
            if (areaId == 3)
            {
                noktalar.Add(new PointF((float)d4[0], 0));
                sonuc = d4[0] + (y * (Math.Abs(d4[0] - d4[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d4[2] - (y * (Math.Abs(d4[1] - d4[2])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d4[2], 0));
            }

            if (areaId == 4)
            {
                noktalar.Add(new PointF((float)d5[0], 0));
                sonuc = d5[0] + (y * (Math.Abs(d5[0] - d5[1])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                sonuc = d5[3] - (y * (Math.Abs(d5[2] - d5[3])));
                noktalar.Add(new PointF((float)sonuc, (float)y));
                noktalar.Add(new PointF((float)d5[3], 0));
            }

            return noktalar;
        }


        #endregion


        private List<double> KirlilikKesisim(double d, int sekilIndex)
        {
            // [-4.5, -2.5, 2, 4.5] - [3, 5, 7] - [5.5, 8, 12.5, 15]
            _kirlilikList = new List<string>();
            List<double> kesisimler = new List<double>();
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
                d3 = (d - 5.5) * (1 / Math.Abs((5.5) - (8)));
            else if (d >= 8 && d <= 12.5)
                d3 = 1;
            else if (d >= 12.5 && d <= 15)
                d3 = 1 - ((d - 12.5) * (1 / Math.Abs((12.5) - (15.0))));

            if (d1 > -1)
            {
                _kirlilikList.Add("Küçük");
                if (sekilIndex == -1 || sekilIndex == 0)
                    kesisimler.Add(d1);
            }
            if (d2 > -1)
            {
                _kirlilikList.Add("Orta");
                if (sekilIndex == -1 || sekilIndex == 1)
                    kesisimler.Add(d2);
            }
            if (d3 > -1)
            {
                _kirlilikList.Add("Büyük");
                if (sekilIndex == -1 || sekilIndex == 2)
                    kesisimler.Add(d3);
            }
            if (kesisimler.Count == 0) kesisimler.Add(-1);
            return kesisimler;
        }



    }

    public static class Extensions
    {
        public static double AgirlikliOrtalamaExt<T>(this IEnumerable<T> records, Func<T, double> value, Func<T, double> weight)
        {
            double weightedValueSum = records.Sum(x => value(x) * weight(x));
            double weightSum = records.Sum(x => value(x));

            if (weightSum != 0)
                return weightedValueSum / weightSum;
            return 0;
        }
    }

}
