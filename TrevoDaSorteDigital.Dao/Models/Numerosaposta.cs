using System;
using System.Collections.Generic;

#nullable disable

namespace TrevoDaSorteDigital.Dao.Models
{
    public partial class Numerosaposta
    {
        public int Id { get; set; }
        public int Idaposta { get; set; }
        public int? Dezena { get; set; }
        public int? Numerodezena1 { get; set; }
        public int? Numerodezena2 { get; set; }
        public int? Numerodezena3 { get; set; }
        public int? Numerodezena4 { get; set; }
        public int? Numerodezena5 { get; set; }
        public int? Numerodezena6 { get; set; }
        public int? Numerodezena7 { get; set; }
        public sbyte? Sorteadonumerodezena1 { get; set; }
        public sbyte? Sorteadonumerodezena2 { get; set; }
        public sbyte? Sorteadonumerodezena3 { get; set; }
        public sbyte? Sorteadonumerodezena4 { get; set; }
        public sbyte? Sorteadonumerodezena5 { get; set; }
        public sbyte? Sorteadonumerodezena6 { get; set; }
        public sbyte? Sorteadonumerodezena7 { get; set; }

        public virtual Aposta IdapostaNavigation { get; set; }
    }
}
