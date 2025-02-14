using System;
using System.Collections.Generic;

#nullable disable

namespace TrevoDaSorteDigital.Dao.Models
{
    public partial class Loteria
    {
        public Loteria()
        {
            Aposta = new HashSet<Aposta>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<Aposta> Aposta { get; set; }
    }
}
