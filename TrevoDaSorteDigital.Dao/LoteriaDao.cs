using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrevoDaSorteDigital.Dao.Models;

namespace TrevoDaSorteDigital.Dao
{
    public class LoteriaDao : Utils
    {
        public List<LoteriaModel> ListarLoterias()
        {

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                    {
                        HttpResponseMessage response = client.GetAsync("https://loteriascaixa-api.herokuapp.com/api").Result;
                        response.EnsureSuccessStatusCode(); // Throw if not successful

                        string responseBody = response.Content.ReadAsStringAsync().Result;

                        // Replace double quotes with an empty string
                        responseBody = responseBody.Replace("\"", "").Replace("[", "").Replace("]", "");

                        // Split the responseBody string by comma and remove leading/trailing spaces
                        string[] lotteryArray = responseBody.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        List<string> lista = new List<string>(lotteryArray);

                        for (int i = 0; i < lista.Count; i++)
                        {
                            // if it is List<String>
                            if (lista[i].Equals("federal"))
                            {
                                lista.RemoveAt(i);
                            }

                            var loteria = ctx.Loterias.FirstOrDefault(c => c.Nome == lista[i]);

                            if (loteria == null)
                            {
                                loteria = new Loteria
                                {
                                    Nome = lista[i]
                                };

                                ctx.Loterias.Add(loteria);
                                ctx.SaveChanges();
                            }
                        }

                        List<LoteriaModel> listaLoterias = new List<LoteriaModel>();

                        var dadosLoterias = (from l in ctx.Loterias
                                             select new
                                             {
                                                 Id = l.Id,
                                                 Nome = l.Nome,
                                                 Descricao = l.Descricao
                                             }).ToList();

                        foreach (var item in dadosLoterias)
                        {
                            listaLoterias.Add(new LoteriaModel
                            {
                                Id = item.Id,
                                Nome = item.Nome,
                                Descricao = item.Descricao
                            });
                        }

                        return listaLoterias;
                    }
                }
                catch (HttpRequestException e)
                {
                    return null;
                }
            }
        }


        public string BuscarNomeLoteria(int idAposta)
        {
            string retorno = "";

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = (from a in ctx.Apostas
                                 join l in ctx.Loterias on a.Idloteria equals l.Id
                                 where a.Id == idAposta
                                 select new
                                 {
                                     NomeLoteria = l.Nome
                                 }).FirstOrDefault();

                    if (dados != null)
                    {
                        retorno = dados.NomeLoteria.ToString();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return retorno;
        }


        public ResultadoLoteriaModel BuscarResultadoPorUltimoConcurso(int? idAposta, string loteria)
        {
            using (HttpClient client = new HttpClient())
            {
                ResultadoLoteriaModel resultado = null;
                try
                {
                    HttpResponseMessage response = client.GetAsync($"https://loteriascaixa-api.herokuapp.com/api/{loteria}/latest").Result;
                    response.EnsureSuccessStatusCode(); // Throw if not successful

                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    resultado = JsonConvert.DeserializeObject<ResultadoLoteriaModel>(responseBody);

                    if (idAposta != null)
                    {
                        using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                        {
                            var numerosAposta = (from n in ctx.Numerosapostas
                                                 where n.Idaposta == idAposta
                                                 select n).ToList();

                            if (numerosAposta != null)
                            {
                                foreach (var item in numerosAposta)
                                {
                                    item.Sorteadonumerodezena1 = isNumeroSorteado(resultado.dezenas, (int)item.Numerodezena1);

                                    ctx.SaveChanges();

                                    if (loteria == "supersete")
                                    {
                                        item.Sorteadonumerodezena2 = isNumeroSorteado(resultado.dezenas, (int)item.Numerodezena2);
                                        item.Sorteadonumerodezena3 = isNumeroSorteado(resultado.dezenas, (int)item.Numerodezena3);
                                        item.Sorteadonumerodezena4 = isNumeroSorteado(resultado.dezenas, (int)item.Numerodezena4);
                                        item.Sorteadonumerodezena5 = isNumeroSorteado(resultado.dezenas, (int)item.Numerodezena5);
                                        item.Sorteadonumerodezena6 = isNumeroSorteado(resultado.dezenas, (int)item.Numerodezena6);
                                        item.Sorteadonumerodezena7 = isNumeroSorteado(resultado.dezenas, (int)item.Numerodezena7);

                                        ctx.SaveChanges();
                                    }
                                }
                            }

                            if (loteria == "maismilionaria")
                            {
                                var trevosAposta = (from n in ctx.Numerostrevos
                                                    where n.Idaposta == idAposta
                                                    select n).ToList();

                                if (trevosAposta != null)
                                {
                                    foreach (var item in trevosAposta)
                                    {
                                        item.Sorteadonumerodezena = isNumeroSorteado(resultado.trevos, (int)item.Numerodezena);
                                        ctx.SaveChanges();
                                    }
                                }
                            }
                        }
                    }

                    return resultado;

                }
                catch (HttpRequestException e)
                {
                    return null;
                }
            }
        }

        private sbyte isNumeroSorteado(string[] dezenas, int numero)
        {
            string numeroApostado = numero.ToString("D2");

            int pos = Array.IndexOf(dezenas, numeroApostado);
            sbyte isSorteado = pos > -1 ? (sbyte)1 : (sbyte)0;

            return isSorteado;
        }
    }


    public class LoteriaModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }
    }

    public class ResultadoLoteriaModel
    {
        public string loteria { get; set; }
        public int concurso { get; set; }
        public string data { get; set; }
        public string local { get; set; }
        public string[] dezenasOrdemSorteio { get; set; }
        public string[] dezenas { get; set; }
        public string[] trevos { get; set; }
        public string timeCoracao { get; set; }
        public string mesSorte { get; set; }
        public List<PremiacaoModel> premiacoes { get; set; }
        public string[] estadosPremiados { get; set; }
        public string observacao { get; set; }
        public bool acumulou { get; set; }
        public int proximoConcurso { get; set; }
        public string dataProximoConcurso { get; set; }
        public List<LocalGanhador> localGanhadores { get; set; }
        public decimal valorArrecadado { get; set; }
        public decimal valorAcumuladoConcurso_0_5 { get; set; }
        public decimal valorAcumuladoConcursoEspecial { get; set; }
        public decimal valorAcumuladoProximoConcurso { get; set; }
        public decimal valorEstimadoProximoConcurso { get; set; }
    }

    public class PremiacaoModel
    {
        public string descricao { get; set; }
        public int faixa { get; set; }
        public int ganhadores { get; set; }

        public decimal valorPremio { get; set; }
    }

    public class LocalGanhador
    {
        public int ganhadores { get; set; }
        public string municipio { get; set; }
        public string nomeFatansiaUL { get; set; }
        public string serie { get; set; }
        public int posicao { get; set; }
        public string uf { get; set; }
    }
}
