using System;
using System.Collections.Generic;

#nullable disable

namespace TrevoDaSorteDigital.Dao.Models
{
    public partial class Estado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Uf { get; set; }
    }
}
