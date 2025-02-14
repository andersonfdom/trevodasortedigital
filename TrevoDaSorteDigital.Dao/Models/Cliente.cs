using System;
using System.Collections.Generic;

#nullable disable

namespace TrevoDaSorteDigital.Dao.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Aposta = new HashSet<Aposta>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Senha { get; set; }
        public sbyte? Ativo { get; set; }
        public string Tokenacesso { get; set; }
        public sbyte? Usuariologado { get; set; }
        public DateTime? Ultimoacesso { get; set; }
        public string Serialrecovery { get; set; }
        public string Codafiliado { get; set; }

        public virtual ICollection<Aposta> Aposta { get; set; }
    }
}
