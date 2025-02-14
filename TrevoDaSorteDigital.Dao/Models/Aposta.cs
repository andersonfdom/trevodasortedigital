using System;
using System.Collections.Generic;

#nullable disable

namespace TrevoDaSorteDigital.Dao.Models
{
    public partial class Aposta
    {
        public Aposta()
        {
            Numerosaposta = new HashSet<Numerosaposta>();
            Numerostrevos = new HashSet<Numerostrevo>();
        }

        public int Id { get; set; }
        public int Idcliente { get; set; }
        public int Idloteria { get; set; }
        public string Mesdasorte { get; set; }
        public string Timedocoracao { get; set; }
        public DateTime? Dataaposta { get; set; }

        public virtual Cliente IdclienteNavigation { get; set; }
        public virtual Loteria IdloteriaNavigation { get; set; }
        public virtual ICollection<Numerosaposta> Numerosaposta { get; set; }
        public virtual ICollection<Numerostrevo> Numerostrevos { get; set; }
    }
}
