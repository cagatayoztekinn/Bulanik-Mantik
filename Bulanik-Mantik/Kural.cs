using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulanik_Mantik
{
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
        public double GetMinKesisimX
        {
            get
            {
                //Minimum Seçimi
                return GetKesisimX.Where(a => a != -1).Min();
            }
        }
        public double[] GetKesisimX
        {
            get
            {
                List<double> list = new List<double>();
                list.Add(core.KesisimHesapla(x1, FuzzyLogicCore.KESISIM.HASSASLIK, (int)Hassaslık).Single());
                list.Add(core.KesisimHesapla(x2, FuzzyLogicCore.KESISIM.MIKTAR, (int)Miktar).Single());
                list.Add(core.KesisimHesapla(x3, FuzzyLogicCore.KESISIM.KIRLILIK, (int)Kirlilik).Single());
                return list.ToArray();
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
            Enums.Hassas h = 0;
            Enums.Miktr m = 0;
            Enums.Kirli k = 0;
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
                    sure_agirlik = 23.79;
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
        private void Kurallar()
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
