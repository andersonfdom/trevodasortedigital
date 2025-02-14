using System;
using System.Collections.Generic;

#nullable disable

namespace TrevoDaSorteDigital.Dao.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public sbyte? Usuariologado { get; set; }
        public DateTime? Ultimoacesso { get; set; }
        public string Serialrecovery { get; set; }
        public string Tokenacesso { get; set; }
    }
}
