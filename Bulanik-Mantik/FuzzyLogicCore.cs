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


        public List<double> KesisimHesapla(double d, KESISIM k)
        {
            switch (k)
            {
                case KESISIM.HASSASLIK:
                    return HassaslikVeMiktarKesisim(d, k);
                    break;
                case KESISIM.MIKTAR:
                    return HassaslikVeMiktarKesisim(d, k);
                    break;
                case KESISIM.KIRLILIK:
                    return KirlilikKesisim(d);
                    break;
            }
            return null;
        }
        private List<double> HassaslikVeMiktarKesisim(double d, KESISIM k)
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
                kesisimler.Add(d1);
            }

            if (d2 > -1)
            {
                tempList.Add("Orta");
                kesisimler.Add(d2);
            }

            if (d3 > -1)
            {
                tempList.Add(k == KESISIM.HASSASLIK ? "Hassas" : "Büyük");
                kesisimler.Add(d3);
            }

            if (k == KESISIM.HASSASLIK)
                _hassaslikList = tempList;
            else
                _miktarList = tempList;

            if (kesisimler.Count == 0) kesisimler.Add(0);
            return kesisimler;
        }
        private List<double> KirlilikKesisim(double d)
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
                kesisimler.Add(d1);
            }
            if (d2 > -1)
            {
                _kirlilikList.Add("Orta");
                kesisimler.Add(d2);
            }
            if (d3 > -1)
            {
                _kirlilikList.Add("Büyük");
                kesisimler.Add(d3);
            }
            if (kesisimler.Count == 0) kesisimler.Add(0);
            return kesisimler;
        }



    }

    public static class extensions
    {
        public static double AgirlikliOrtalamaExt<T>(this IEnumerable<T> records, Func<T, double> value, Func<T, double> weight)
        {
            double weightedValueSum = records.Sum(x => value(x) * weight(x));
            double weightSum = records.Sum(x => weight(x));

            if (weightSum != 0)
                return weightedValueSum / weightSum;
            else
                throw new DivideByZeroException("Your message here");
        }
    }
    public class Kural
    {

        #region Enumlar

        public enum StringType
        {
            Hassas,
            Miktr,
            Kirli
        }
        public enum AgirlikMerkez
        {
            Donus,
            Deterjan,
            Sure
        }
        public enum Hassas
        {
            hassas,
            orta,
            sağlam
        }
        public enum Miktr
        {
            büyük,
            orta,
            küçük
        }
        public enum Kirli
        {
            büyük,
            orta,
            küçük
        }



        public enum Donus
        {
            hassas,
            normalHassas,
            orta,
            normalGuclu,
            guclu
        }
        public enum Deterjan
        {
            cokAz,
            az,
            orta,
            fazla,
            cokFazla
        }
        public enum Sure
        {
            kısa,
            normalKısa,
            orta,
            normalUzun,
            uzun
        }

        #endregion

        public Hassas Hassaslık { get; set; }
        public Miktr Miktar { get; set; }
        public Kirli Kirlilik { get; set; }

        public Donus DonusHizi { get; set; }
        public Deterjan DeterjanMiktari { get; set; }
        public Sure Suresi { get; set; }
        public double x1 { get; set; }
        public double x2 { get; set; }
        public double x3 { get; set; }

        public double GetMinX
        {
            get
            {
                List<double> list = new List<double>();
                list.Add(x1);
                list.Add(x2);
                list.Add(x3);
                return list.Min();
            }
        }

        public Kural(Hassas Hassaslık, Miktr Miktar, Kirli kirlilik)
        {
            this.Hassaslık = Hassaslık;
            this.Miktar = Miktar;
            this.Kirlilik = kirlilik;
            Kurallar();
        }



        public Kural(string Hassaslık, string Miktar, string kirlilik)
        {
            Hassas h = default;
            Miktr m = default;
            Kirli k = default;
            switch (Hassaslık)
            {
                case "Sağlam":
                    h = Hassas.sağlam;
                    break;
                case "Orta":
                    h = Hassas.orta;
                    break;
                case "Hassas":
                    h = Hassas.hassas;
                    break;
            }

            switch (Miktar)
            {
                case "Küçük":
                    m = Miktr.küçük;
                    break;
                case "Orta":
                    m = Miktr.orta;
                    break;
                case "Büyük":
                    m = Miktr.büyük;
                    break;
            }

            switch (kirlilik)
            {
                case "Küçük":
                    k = Kirli.küçük;
                    break;
                case "Orta":
                    k = Kirli.orta;
                    break;
                case "Büyük":
                    k = Kirli.büyük;
                    break;
            }

            this.Hassaslık = h;
            this.Miktar = m;
            this.Kirlilik = k;
            Kurallar();

        }

        public  double AğırlıkGetir(AgirlikMerkez m)
        {
            double donme_agirlik = 0, sure_agirlik = 0, deterjan_agirlik = 0;
            switch (DonusHizi)
            {
                case Donus.hassas:
                    donme_agirlik = -1.15;
                    break;
                case Donus.normalHassas:
                    donme_agirlik = 2.75;
                    break;
                case Donus.orta:
                    donme_agirlik = 5;
                    break;
                case Donus.normalGuclu:
                    donme_agirlik = 7.25;
                    break;
                case Donus.guclu:
                    donme_agirlik = 11.15;
                    break;
            }

            switch (Suresi)
            {
                case Sure.kısa:
                    sure_agirlik = -1.49;
                    break;
                case Sure.normalKısa:
                    sure_agirlik = 39.9;
                    break;
                case Sure.orta:
                    sure_agirlik = 57.5;
                    break;
                case Sure.normalUzun:
                    sure_agirlik = 75.1;
                    break;
                case Sure.uzun:
                    sure_agirlik = 102.15;
                    break;
            }


            switch (DeterjanMiktari)
            {
                case Deterjan.cokAz:
                    deterjan_agirlik = 10;
                    break;
                case Deterjan.az:
                    deterjan_agirlik = 85;
                    break;
                case Deterjan.orta:
                    deterjan_agirlik = 150;
                    break;
                case Deterjan.fazla:
                    deterjan_agirlik = 215;
                    break;
                case Deterjan.cokFazla:
                    deterjan_agirlik = 290;
                    break;
            }

            switch (m)
            {
                case AgirlikMerkez.Donus: return donme_agirlik;
                case AgirlikMerkez.Deterjan: return deterjan_agirlik;
                case AgirlikMerkez.Sure: return sure_agirlik;
            }
            return 0;
        }


        public void XValues(Double x1, Double x2, Double x3)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.x3 = x3;
        }
        public string ToString(StringType type)
        {
            switch (type)
            {
                case StringType.Hassas:
                    switch (Hassaslık)
                    {
                        case Hassas.sağlam: return "Sağlam";
                        case Hassas.orta: return "Orta";
                        case Hassas.hassas: return "Hassas";
                    }
                    break;
                case StringType.Miktr:
                    switch (Miktar)
                    {
                        case Miktr.küçük: return "Küçük";
                        case Miktr.orta: return "Orta";
                        case Miktr.büyük: return "Büyük";
                    }
                    break;
                case StringType.Kirli:
                    switch (Kirlilik)
                    {
                        case Kirli.küçük: return "Küçük";
                        case Kirli.orta: return "Orta";
                        case Kirli.büyük: return "Büyük";
                    }
                    break;
            }
            return base.ToString();
        }
        public void Kurallar()
        {

            if (Hassaslık == Hassas.hassas && Miktar == Miktr.küçük && Kirlilik == Kirli.küçük)
            {
                DonusHizi = Donus.hassas;
                Suresi = Sure.kısa;
                DeterjanMiktari = Deterjan.cokAz;
            }
            if (Hassaslık == Hassas.hassas && Miktar == Miktr.küçük && Kirlilik == Kirli.orta)
            {
                DonusHizi = Donus.normalHassas;
                Suresi = Sure.kısa;
                DeterjanMiktari = Deterjan.az;
            }
            if (Hassaslık == Hassas.hassas && Miktar == Miktr.küçük && Kirlilik == Kirli.büyük)
            {
                DonusHizi = Donus.orta;
                Suresi = Sure.normalKısa;
                DeterjanMiktari = Deterjan.orta;
            }


            if (Hassaslık == Hassas.hassas && Miktar == Miktr.orta && Kirlilik == Kirli.küçük)
            {
                DonusHizi = Donus.hassas;
                Suresi = Sure.kısa;
                DeterjanMiktari = Deterjan.orta;
            }
            if (Hassaslık == Hassas.hassas && Miktar == Miktr.orta && Kirlilik == Kirli.orta)
            {
                DonusHizi = Donus.normalHassas;
                Suresi = Sure.normalKısa;
                DeterjanMiktari = Deterjan.orta;
            }
            if (Hassaslık == Hassas.hassas && Miktar == Miktr.orta && Kirlilik == Kirli.büyük)
            {
                DonusHizi = Donus.orta;
                Suresi = Sure.orta;
                DeterjanMiktari = Deterjan.fazla;
            }


            if (Hassaslık == Hassas.hassas && Miktar == Miktr.büyük && Kirlilik == Kirli.küçük)
            {
                DonusHizi = Donus.normalHassas;
                Suresi = Sure.normalKısa;
                DeterjanMiktari = Deterjan.orta;
            }
            if (Hassaslık == Hassas.hassas && Miktar == Miktr.büyük && Kirlilik == Kirli.orta)
            {
                DonusHizi = Donus.normalHassas;
                Suresi = Sure.orta;
                DeterjanMiktari = Deterjan.fazla;
            }
            if (Hassaslık == Hassas.hassas && Miktar == Miktr.büyük && Kirlilik == Kirli.büyük)
            {
                DonusHizi = Donus.orta;
                Suresi = Sure.normalUzun;
                DeterjanMiktari = Deterjan.fazla;
            }


            if (Hassaslık == Hassas.orta && Miktar == Miktr.küçük && Kirlilik == Kirli.küçük)
            {
                DonusHizi = Donus.normalHassas;
                Suresi = Sure.normalKısa;
                DeterjanMiktari = Deterjan.az;
            }
            if (Hassaslık == Hassas.orta && Miktar == Miktr.küçük && Kirlilik == Kirli.orta)
            {
                DonusHizi = Donus.orta;
                Suresi = Sure.kısa;
                DeterjanMiktari = Deterjan.orta;
            }
            if (Hassaslık == Hassas.orta && Miktar == Miktr.küçük && Kirlilik == Kirli.büyük)
            {
                DonusHizi = Donus.normalGuclu;
                Suresi = Sure.orta;
                DeterjanMiktari = Deterjan.fazla;
            }



            if (Hassaslık == Hassas.orta && Miktar == Miktr.orta && Kirlilik == Kirli.küçük)
            {
                DonusHizi = Donus.normalHassas;
                Suresi = Sure.normalKısa;
                DeterjanMiktari = Deterjan.orta;
            }
            if (Hassaslık == Hassas.orta && Miktar == Miktr.orta && Kirlilik == Kirli.orta)
            {
                DonusHizi = Donus.orta;
                Suresi = Sure.orta;
                DeterjanMiktari = Deterjan.orta;
            }
            if (Hassaslık == Hassas.orta && Miktar == Miktr.orta && Kirlilik == Kirli.büyük)
            {
                DonusHizi = Donus.hassas;
                Suresi = Sure.uzun;
                DeterjanMiktari = Deterjan.fazla;
            }


            if (Hassaslık == Hassas.orta && Miktar == Miktr.büyük && Kirlilik == Kirli.küçük)
            {
                DonusHizi = Donus.hassas;
                Suresi = Sure.orta;
                DeterjanMiktari = Deterjan.orta;
            }
            if (Hassaslık == Hassas.orta && Miktar == Miktr.büyük && Kirlilik == Kirli.orta)
            {
                DonusHizi = Donus.hassas;
                Suresi = Sure.normalUzun;
                DeterjanMiktari = Deterjan.fazla;
            }
            if (Hassaslık == Hassas.orta && Miktar == Miktr.büyük && Kirlilik == Kirli.büyük)
            {
                DonusHizi = Donus.hassas;
                Suresi = Sure.uzun;
                DeterjanMiktari = Deterjan.cokFazla;
            }



            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.küçük && Kirlilik == Kirli.küçük)
            {
                DonusHizi = Donus.orta;
                Suresi = Sure.orta;
                DeterjanMiktari = Deterjan.az;
            }
            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.küçük && Kirlilik == Kirli.orta)
            {
                DonusHizi = Donus.normalGuclu;
                Suresi = Sure.orta;
                DeterjanMiktari = Deterjan.orta;
            }
            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.küçük && Kirlilik == Kirli.büyük)
            {
                DonusHizi = Donus.guclu;
                Suresi = Sure.normalUzun;
                DeterjanMiktari = Deterjan.fazla;
            }


            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.orta && Kirlilik == Kirli.küçük)
            {
                DonusHizi = Donus.orta;
                Suresi = Sure.orta;
                DeterjanMiktari = Deterjan.orta;
            }
            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.orta && Kirlilik == Kirli.orta)
            {
                DonusHizi = Donus.normalGuclu;
                Suresi = Sure.normalUzun;
                DeterjanMiktari = Deterjan.orta;
            }
            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.orta && Kirlilik == Kirli.büyük)
            {
                DonusHizi = Donus.guclu;
                Suresi = Sure.orta;
                DeterjanMiktari = Deterjan.cokFazla;
            }


            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.büyük && Kirlilik == Kirli.küçük)
            {
                DonusHizi = Donus.normalGuclu;
                Suresi = Sure.normalUzun;
                DeterjanMiktari = Deterjan.fazla;
            }
            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.büyük && Kirlilik == Kirli.orta)
            {
                DonusHizi = Donus.normalGuclu;
                Suresi = Sure.uzun;
                DeterjanMiktari = Deterjan.fazla;
            }
            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.büyük && Kirlilik == Kirli.büyük)
            {
                DonusHizi = Donus.guclu;
                Suresi = Sure.uzun;
                DeterjanMiktari = Deterjan.cokFazla;
            }

        }
    }
}
