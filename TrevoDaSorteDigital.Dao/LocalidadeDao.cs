using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrevoDaSorteDigital.Dao.Models;

namespace TrevoDaSorteDigital.Dao
{
    public class LocalidadeDao : Utils
    {
        public List<Estado> ListarEstados()
        {
            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    return ctx.Estados.OrderBy(c => c.Nome).ToList();
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public Registrocep CarregarDadosCep(CepModel cepModel)
        {
            Registrocep registrocep = null;

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var cep = cepModel.Cep.ToString().Replace(".", "").Replace("-", "");

                    registrocep = ctx.Registroceps.FirstOrDefault(c => c.Cep == cep);

                    if (registrocep == null)
                    {
                        CepAPIModel cepAPIModel = BuscarCepAPI(cep);

                        if (cepAPIModel == null)
                        {
                            return null;
                        }
                        else
                        {
                            registrocep = new Registrocep
                            {
                                Bairro = cepAPIModel.bairro,
                                Cep = cep,
                                Localidade = cepAPIModel.localidade,
                                Logradouro = cepAPIModel.logradouro,
                                Uf = cepAPIModel.estado
                            };

                            ctx.Registroceps.Add(registrocep);
                            ctx.SaveChanges();
                        }
                    }

                    return registrocep;
                }
            }
            catch (System.Exception)
            {
                return registrocep;
            }
        }

        private CepAPIModel BuscarCepAPI(string cep)
        {
            using (var client = new HttpClient())
            {
                string url = $"https://viacep.com.br/ws/{cep}/json/";

                try
                {
                    var response = client.GetStringAsync(url).Result; // Chamada síncrona
                    return JsonSerializer.Deserialize<CepAPIModel>(response);
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Erro ao chamar a API. Verifique sua conexão.");
                    return null;
                }
            }
        }
    }

    public class CepModel
    {
        public string Cep { get; set; }
    }

    public class CepAPIModel
    {
        public string? cep { get; set; }
        public string? logradouro { get; set; }
        public string? complemento { get; set; }
        public string? unidade { get; set; }
        public string? bairro { get; set; }
        public string? localidade { get; set; }
        public string? uf { get; set; }
        public string? estado { get; set; }
        public string? regiao { get; set; }
        public string? ibge { get; set; }
        public string? gia { get; set; }
        public string? ddd { get; set; }
        public string? siafi { get; set; }
    }
}
