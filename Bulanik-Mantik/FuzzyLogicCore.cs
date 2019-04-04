using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulanik_Mantik
{

    public static class Enums
    {
        #region Enumlar
        public enum InputType
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
    }
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

        public Enums.Hassas Hassaslık { get; set; }
        public Enums.Miktr Miktar { get; set; }
        public Enums.Kirli Kirlilik { get; set; }

        public Enums.Donus DonusHizi { get; set; }
        public Enums.Deterjan DeterjanMiktari { get; set; }
        public Enums.Sure Suresi { get; set; }
        public double x1 { get; set; }
        public double x2 { get; set; }
        public double x3 { get; set; }

        FuzzyLogicCore core = new FuzzyLogicCore();
        //public double GetMinX
        //{
        //    get
        //    {

        //        List<double> list = new List<double>();

        //        list.Add(x1);
        //        list.Add(x2);
        //        list.Add(x3);
        //        return list.Min();
        //    }

        //}
        public double GetMinX
        {
            get
            {
                List<double> list = new List<double>();
                list.Add(core.KesisimHesapla(x1, FuzzyLogicCore.KESISIM.HASSASLIK, (int)Hassaslık).Single());
                list.Add(core.KesisimHesapla(x2, FuzzyLogicCore.KESISIM.MIKTAR, (int)Miktar).Single());
                list.Add(core.KesisimHesapla(x3, FuzzyLogicCore.KESISIM.KIRLILIK, (int)Kirlilik).Single());
                return list.Where(a=>a!=-1).Min();
            }
        }

        public Kural(Enums.Hassas Hassaslık, Enums.Miktr Miktar, Enums.Kirli kirlilik)
        {
            this.Hassaslık = Hassaslık;
            this.Miktar = Miktar;
            this.Kirlilik = kirlilik;
            Kurallar();
        }



        public Kural(string Hassaslık, string Miktar, string kirlilik)
        {
            Enums.Hassas h = default;
            Enums.Miktr m = default;
            Enums.Kirli k = default;
            switch (Hassaslık)
            {
                case "Sağlam":
                    h = Enums.Hassas.sağlam;
                    break;
                case "Orta":
                    h = Enums.Hassas.orta;
                    break;
                case "Hassas":
                    h = Enums.Hassas.hassas;
                    break;
            }

            switch (Miktar)
            {
                case "Küçük":
                    m = Enums.Miktr.küçük;
                    break;
                case "Orta":
                    m = Enums.Miktr.orta;
                    break;
                case "Büyük":
                    m = Enums.Miktr.büyük;
                    break;
            }

            switch (kirlilik)
            {
                case "Küçük":
                    k = Enums.Kirli.küçük;
                    break;
                case "Orta":
                    k = Enums.Kirli.orta;
                    break;
                case "Büyük":
                    k = Enums.Kirli.büyük;
                    break;
            }

            this.Hassaslık = h;
            this.Miktar = m;
            this.Kirlilik = k;
            Kurallar();

        }

        public double AğırlıkGetir(Enums.AgirlikMerkez m)
        {
            double donme_agirlik = 0, sure_agirlik = 0, deterjan_agirlik = 0;
            switch (DonusHizi)
            {
                case Enums.Donus.hassas:
                    donme_agirlik = -1.15;
                    break;
                case Enums.Donus.normalHassas:
                    donme_agirlik = 2.75;
                    break;
                case Enums.Donus.orta:
                    donme_agirlik = 5;
                    break;
                case Enums.Donus.normalGuclu:
                    donme_agirlik = 7.25;
                    break;
                case Enums.Donus.guclu:
                    donme_agirlik = 11.15;
                    break;
            }

            switch (Suresi)
            {
                case Enums.Sure.kısa:
                    sure_agirlik = -1.49;
                    break;
                case Enums.Sure.normalKısa:
                    sure_agirlik = 39.9;
                    break;
                case Enums.Sure.orta:
                    sure_agirlik = 57.5;
                    break;
                case Enums.Sure.normalUzun:
                    sure_agirlik = 75.1;
                    break;
                case Enums.Sure.uzun:
                    sure_agirlik = 102.15;
                    break;
            }


            switch (DeterjanMiktari)
            {
                case Enums.Deterjan.cokAz:
                    deterjan_agirlik = 10;
                    break;
                case Enums.Deterjan.az:
                    deterjan_agirlik = 85;
                    break;
                case Enums.Deterjan.orta:
                    deterjan_agirlik = 150;
                    break;
                case Enums.Deterjan.fazla:
                    deterjan_agirlik = 215;
                    break;
                case Enums.Deterjan.cokFazla:
                    deterjan_agirlik = 290;
                    break;
            }

            switch (m)
            {
                case Enums.AgirlikMerkez.Donus: return donme_agirlik;
                case Enums.AgirlikMerkez.Deterjan: return deterjan_agirlik;
                case Enums.AgirlikMerkez.Sure: return sure_agirlik;
            }
            return 0;
        }


        public void XValues(Double x1, Double x2, Double x3)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.x3 = x3;
        }
        public string ToString(Enums.InputType type)
        {
            switch (type)
            {
                case Enums.InputType.Hassas:
                    switch (Hassaslık)
                    {
                        case Enums.Hassas.sağlam: return "Sağlam";
                        case Enums.Hassas.orta: return "Orta";
                        case Enums.Hassas.hassas: return "Hassas";
                    }
                    break;
                case Enums.InputType.Miktr:
                    switch (Miktar)
                    {
                        case Enums.Miktr.küçük: return "Küçük";
                        case Enums.Miktr.orta: return "Orta";
                        case Enums.Miktr.büyük: return "Büyük";
                    }
                    break;
                case Enums.InputType.Kirli:
                    switch (Kirlilik)
                    {
                        case Enums.Kirli.küçük: return "Küçük";
                        case Enums.Kirli.orta: return "Orta";
                        case Enums.Kirli.büyük: return "Büyük";
                    }
                    break;
            }
            return base.ToString();
        }
        public void Kurallar()
        {

            if (Hassaslık == Enums.Hassas.hassas && Miktar == Enums.Miktr.küçük && Kirlilik == Enums.Kirli.küçük)
            {
                DonusHizi = Enums.Donus.hassas;
                Suresi = Enums.Sure.kısa;
                DeterjanMiktari = Enums.Deterjan.cokAz;
            }
            if (Hassaslık == Enums.Hassas.hassas && Miktar == Enums.Miktr.küçük && Kirlilik == Enums.Kirli.orta)
            {
                DonusHizi = Enums.Donus.normalHassas;
                Suresi = Enums.Sure.kısa;
                DeterjanMiktari = Enums.Deterjan.az;
            }
            if (Hassaslık == Enums.Hassas.hassas && Miktar == Enums.Miktr.küçük && Kirlilik == Enums.Kirli.büyük)
            {
                DonusHizi = Enums.Donus.orta;
                Suresi = Enums.Sure.normalKısa;
                DeterjanMiktari = Enums.Deterjan.orta;
            }


            if (Hassaslık == Enums.Hassas.hassas && Miktar == Enums.Miktr.orta && Kirlilik == Enums.Kirli.küçük)
            {
                DonusHizi = Enums.Donus.hassas;
                Suresi = Enums.Sure.kısa;
                DeterjanMiktari = Enums.Deterjan.orta;
            }
            if (Hassaslık == Enums.Hassas.hassas && Miktar == Enums.Miktr.orta && Kirlilik == Enums.Kirli.orta)
            {
                DonusHizi = Enums.Donus.normalHassas;
                Suresi = Enums.Sure.normalKısa;
                DeterjanMiktari = Enums.Deterjan.orta;
            }
            if (Hassaslık == Enums.Hassas.hassas && Miktar == Enums.Miktr.orta && Kirlilik == Enums.Kirli.büyük)
            {
                DonusHizi = Enums.Donus.orta;
                Suresi = Enums.Sure.orta;
                DeterjanMiktari = Enums.Deterjan.fazla;
            }


            if (Hassaslık == Enums.Hassas.hassas && Miktar == Enums.Miktr.büyük && Kirlilik == Enums.Kirli.küçük)
            {
                DonusHizi = Enums.Donus.normalHassas;
                Suresi = Enums.Sure.normalKısa;
                DeterjanMiktari = Enums.Deterjan.orta;
            }
            if (Hassaslık == Enums.Hassas.hassas && Miktar == Enums.Miktr.büyük && Kirlilik == Enums.Kirli.orta)
            {
                DonusHizi = Enums.Donus.normalHassas;
                Suresi = Enums.Sure.orta;
                DeterjanMiktari = Enums.Deterjan.fazla;
            }
            if (Hassaslık == Enums.Hassas.hassas && Miktar == Enums.Miktr.büyük && Kirlilik == Enums.Kirli.büyük)
            {
                DonusHizi = Enums.Donus.orta;
                Suresi = Enums.Sure.normalUzun;
                DeterjanMiktari = Enums.Deterjan.fazla;
            }


            if (Hassaslık == Enums.Hassas.orta && Miktar == Enums.Miktr.küçük && Kirlilik == Enums.Kirli.küçük)
            {
                DonusHizi = Enums.Donus.normalHassas;
                Suresi = Enums.Sure.normalKısa;
                DeterjanMiktari = Enums.Deterjan.az;
            }
            if (Hassaslık == Enums.Hassas.orta && Miktar == Enums.Miktr.küçük && Kirlilik == Enums.Kirli.orta)
            {
                DonusHizi = Enums.Donus.orta;
                Suresi = Enums.Sure.kısa;
                DeterjanMiktari = Enums.Deterjan.orta;
            }
            if (Hassaslık == Enums.Hassas.orta && Miktar == Enums.Miktr.küçük && Kirlilik == Enums.Kirli.büyük)
            {
                DonusHizi = Enums.Donus.normalGuclu;
                Suresi = Enums.Sure.orta;
                DeterjanMiktari = Enums.Deterjan.fazla;
            }



            if (Hassaslık == Enums.Hassas.orta && Miktar == Enums.Miktr.orta && Kirlilik == Enums.Kirli.küçük)
            {
                DonusHizi = Enums.Donus.normalHassas;
                Suresi = Enums.Sure.normalKısa;
                DeterjanMiktari = Enums.Deterjan.orta;
            }
            if (Hassaslık == Enums.Hassas.orta && Miktar == Enums.Miktr.orta && Kirlilik == Enums.Kirli.orta)
            {
                DonusHizi = Enums.Donus.orta;
                Suresi = Enums.Sure.orta;
                DeterjanMiktari = Enums.Deterjan.orta;
            }
            if (Hassaslık == Enums.Hassas.orta && Miktar == Enums.Miktr.orta && Kirlilik == Enums.Kirli.büyük)
            {
                DonusHizi = Enums.Donus.hassas;
                Suresi = Enums.Sure.uzun;
                DeterjanMiktari = Enums.Deterjan.fazla;
            }


            if (Hassaslık == Enums.Hassas.orta && Miktar == Enums.Miktr.büyük && Kirlilik == Enums.Kirli.küçük)
            {
                DonusHizi = Enums.Donus.hassas;
                Suresi = Enums.Sure.orta;
                DeterjanMiktari = Enums.Deterjan.orta;
            }
            if (Hassaslık == Enums.Hassas.orta && Miktar == Enums.Miktr.büyük && Kirlilik == Enums.Kirli.orta)
            {
                DonusHizi = Enums.Donus.hassas;
                Suresi = Enums.Sure.normalUzun;
                DeterjanMiktari = Enums.Deterjan.fazla;
            }
            if (Hassaslık == Enums.Hassas.orta && Miktar == Enums.Miktr.büyük && Kirlilik == Enums.Kirli.büyük)
            {
                DonusHizi = Enums.Donus.hassas;
                Suresi = Enums.Sure.uzun;
                DeterjanMiktari = Enums.Deterjan.cokFazla;
            }



            if (Hassaslık == Enums.Hassas.sağlam && Miktar == Enums.Miktr.küçük && Kirlilik == Enums.Kirli.küçük)
            {
                DonusHizi = Enums.Donus.orta;
                Suresi = Enums.Sure.orta;
                DeterjanMiktari = Enums.Deterjan.az;
            }
            if (Hassaslık == Enums.Hassas.sağlam && Miktar == Enums.Miktr.küçük && Kirlilik == Enums.Kirli.orta)
            {
                DonusHizi = Enums.Donus.normalGuclu;
                Suresi = Enums.Sure.orta;
                DeterjanMiktari = Enums.Deterjan.orta;
            }
            if (Hassaslık == Enums.Hassas.sağlam && Miktar == Enums.Miktr.küçük && Kirlilik == Enums.Kirli.büyük)
            {
                DonusHizi = Enums.Donus.guclu;
                Suresi = Enums.Sure.normalUzun;
                DeterjanMiktari = Enums.Deterjan.fazla;
            }


            if (Hassaslık == Enums.Hassas.sağlam && Miktar == Enums.Miktr.orta && Kirlilik == Enums.Kirli.küçük)
            {
                DonusHizi = Enums.Donus.orta;
                Suresi = Enums.Sure.orta;
                DeterjanMiktari = Enums.Deterjan.orta;
            }
            if (Hassaslık == Enums.Hassas.sağlam && Miktar == Enums.Miktr.orta && Kirlilik == Enums.Kirli.orta)
            {
                DonusHizi = Enums.Donus.normalGuclu;
                Suresi = Enums.Sure.normalUzun;
                DeterjanMiktari = Enums.Deterjan.orta;
            }
            if (Hassaslık == Enums.Hassas.sağlam && Miktar == Enums.Miktr.orta && Kirlilik == Enums.Kirli.büyük)
            {
                DonusHizi = Enums.Donus.guclu;
                Suresi = Enums.Sure.orta;
                DeterjanMiktari = Enums.Deterjan.cokFazla;
            }


            if (Hassaslık == Enums.Hassas.sağlam && Miktar == Enums.Miktr.büyük && Kirlilik == Enums.Kirli.küçük)
            {
                DonusHizi = Enums.Donus.normalGuclu;
                Suresi = Enums.Sure.normalUzun;
                DeterjanMiktari = Enums.Deterjan.fazla;
            }
            if (Hassaslık == Enums.Hassas.sağlam && Miktar == Enums.Miktr.büyük && Kirlilik == Enums.Kirli.orta)
            {
                DonusHizi = Enums.Donus.normalGuclu;
                Suresi = Enums.Sure.uzun;
                DeterjanMiktari = Enums.Deterjan.fazla;
            }
            if (Hassaslık == Enums.Hassas.sağlam && Miktar == Enums.Miktr.büyük && Kirlilik == Enums.Kirli.büyük)
            {
                DonusHizi = Enums.Donus.guclu;
                Suresi = Enums.Sure.uzun;
                DeterjanMiktari = Enums.Deterjan.cokFazla;
            }

        }
    }
}
