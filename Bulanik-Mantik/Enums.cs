using System;
using System.Collections.Generic;
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
            sağlam,
            orta,
            hassas
        }

        public enum Miktr
        {
            küçük,
            orta,
            büyük
        }

        public enum Kirli
        {
            küçük,
            orta,
            büyük
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
}
