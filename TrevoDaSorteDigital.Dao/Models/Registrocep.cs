using System;
using System.Collections.Generic;

#nullable disable

namespace TrevoDaSorteDigital.Dao.Models
{
    public partial class Registrocep
    {
        public int Id { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
    }
}
