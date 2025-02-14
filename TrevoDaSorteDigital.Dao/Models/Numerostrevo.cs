using System;
using System.Collections.Generic;

#nullable disable

namespace TrevoDaSorteDigital.Dao.Models
{
    public partial class Numerostrevo
    {
        public int Id { get; set; }
        public int Idaposta { get; set; }
        public int? Dezena { get; set; }
        public int? Numerodezena { get; set; }
        public sbyte? Sorteadonumerodezena { get; set; }

        public virtual Aposta IdapostaNavigation { get; set; }
    }
}
