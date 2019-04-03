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
    public class Kural
    {
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

        public Hassas Hassaslık { get; set; }
        public Miktr Miktar { get; set; }
        public Kirli Kirlilik { get; set; }

        public Donus DonusHizi { get; set; }
        public Deterjan DeterjanMiktari { get; set; }
        public Sure Suresi { get; set; }
        public Kural(Hassas Hassaslık, Miktr Miktar, Kirli kirlilik)
        {
            this.Hassaslık = Hassaslık;
            this.Miktar = Miktar;
            this.Kirlilik = kirlilik;
            Kurallar();
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


            if (Hassaslık == Hassas.orta && Miktar == Miktr.küçük  && Kirlilik == Kirli.küçük)
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



            if (Hassaslık == Hassas.orta && Miktar == Miktr.orta  && Kirlilik == Kirli.küçük)
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

            
            if (Hassaslık == Hassas.orta && Miktar == Miktr.büyük  && Kirlilik == Kirli.küçük)
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


            
            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.küçük  && Kirlilik == Kirli.küçük)
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


            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.orta  && Kirlilik == Kirli.küçük)
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


            if (Hassaslık == Hassas.sağlam && Miktar == Miktr.büyük  && Kirlilik == Kirli.küçük)
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
