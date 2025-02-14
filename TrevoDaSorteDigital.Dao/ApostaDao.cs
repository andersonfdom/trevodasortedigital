using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrevoDaSorteDigital.Dao.Models;

namespace TrevoDaSorteDigital.Dao
{
    public class ApostaDao : Utils
    {
        public int CriarAposta(ApostaModel apostaModel)
        {
            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var cliente = ctx.Clientes.FirstOrDefault(c => c.Tokenacesso == apostaModel.Token);
                    var loteria = ctx.Loterias.FirstOrDefault(c => c.Nome == apostaModel.NomeLoteria);

                    Aposta aposta = new Aposta
                    {
                        Dataaposta = DateTime.Now,
                        Idcliente = cliente.Id,
                        IdclienteNavigation = cliente,
                        Idloteria = loteria.Id,
                        IdloteriaNavigation = loteria,
                        Mesdasorte = loteria.Nome == "diadesorte" ? GerarMesDaSorte() : "",
                        Timedocoracao = loteria.Nome == "timemania" ? GerarTimeDoCoracao() : ""
                    };

                    ctx.Apostas.Add(aposta);
                    ctx.SaveChanges();

                    List<int> numeros = null;
                    List<int> trevos = null;

                    switch (loteria.Nome)
                    {
                        case "maismilionaria":
                            numeros = GerarNumeros(6, 1, 51);
                            trevos = GerarNumeros(2, 1, 7);
                            break;
                        case "megasena":
                            numeros = GerarNumeros(6, 1, 61);
                            break;
                        case "lotofacil":
                            numeros = GerarNumeros(15, 1, 26);
                            break;
                        case "quina":
                            numeros = GerarNumeros(5, 1, 81);
                            break;
                        case "lotomania":
                            numeros = GerarNumeros(50, 0, 100);
                            break;
                        case "timemania":
                            numeros = GerarNumeros(10, 1, 81);
                            break;
                        case "duplasena":
                            numeros = GerarNumeros(6, 1, 51);
                            break;
                        case "diadesorte":
                            numeros = GerarNumeros(7, 1, 32);
                            break;
                        case "supersete":
                            numeros = GerarNumeros(7, 0, 10);
                            break;
                    }

                    int dezenaNumeros = 0;

                    if ((loteria.Nome != "supersete"))
                    {
                        foreach (var item in numeros)
                        {
                            dezenaNumeros = dezenaNumeros + 1;
                            Numerosaposta n = new Numerosaposta
                            {
                                Dezena = dezenaNumeros,
                                Idaposta = aposta.Id,
                                IdapostaNavigation = aposta,
                                Numerodezena1 = item
                            };

                            ctx.Numerosapostas.Add(n);
                            ctx.SaveChanges();
                        }
                    }
                    else
                    {
                        Numerosaposta n = new Numerosaposta();
                        n.Dezena = 1;
                        n.Idaposta = aposta.Id;
                        n.IdapostaNavigation = aposta;

                        int Numerodezena1 = 0;
                        int Numerodezena2 = 0;
                        int Numerodezena3 = 0;
                        int Numerodezena4 = 0;
                        int Numerodezena5 = 0;
                        int Numerodezena6 = 0;
                        int Numerodezena7 = 0;

                        for (int i = 0; i < numeros.Count(); i++)
                        {
                            if (i == 0)
                            {
                                Numerodezena1 = numeros[i];
                            }

                            else if (i == 1)
                            {
                                Numerodezena2 = numeros[i];
                            }

                            else if (i == 2)
                            {
                                Numerodezena3 = numeros[i];
                            }

                            else if (i == 3)
                            {
                                Numerodezena4 = numeros[i];
                            }

                            else if (i == 4)
                            {
                                Numerodezena5 = numeros[i];
                            }

                            else if (i == 5)
                            {
                                Numerodezena6 = numeros[i];
                            }

                            else
                            {
                                Numerodezena7 = numeros[i];
                            }
                        }

                        n.Numerodezena1 = Numerodezena1;
                        n.Numerodezena2 = Numerodezena2;
                        n.Numerodezena3 = Numerodezena3;
                        n.Numerodezena4 = Numerodezena4;
                        n.Numerodezena5 = Numerodezena5;
                        n.Numerodezena6 = Numerodezena6;
                        n.Numerodezena7 = Numerodezena7;

                        ctx.Numerosapostas.Add(n);
                        ctx.SaveChanges();
                    }

                    if (loteria.Nome == "maismilionaria")
                    {
                        int dezenaTrevos = 0;

                        foreach (var item in trevos)
                        {
                            dezenaTrevos = dezenaTrevos + 1;
                            Numerosaposta n = new Numerosaposta
                            {
                                Dezena = dezenaTrevos,
                                Idaposta = aposta.Id,
                                IdapostaNavigation = aposta,
                                Numerodezena1 = item
                            };

                            ctx.Numerosapostas.Add(n);
                            ctx.SaveChanges();
                        }
                    }

                    return aposta.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public List<DadosAposta> ListarApostas(string token)
        {
            List<DadosAposta> lista = null;

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var idCliente = ctx.Clientes.FirstOrDefault(c => c.Tokenacesso == token);

                    var dados = (from a in ctx.Apostas
                                 where a.Idcliente == idCliente.Id
                                 select new
                                 {
                                     Id = a.Id,
                                     IdCliente = a.Idcliente,
                                     IdLoteria = a.Idloteria,
                                     MesDaSorte = a.Mesdasorte,
                                     TimeDoCoracao = a.Timedocoracao,
                                     DataAposta = a.Dataaposta,
                                     dadosNumerosApostas = new List<DadosNumerosAposta>(),
                                     dadosNumerosTrevos = new List<DadosNumerosTrevo>()
                                 }).ToList();

                    if (dados != null)
                    {
                        lista = new List<DadosAposta>();

                        foreach (var item in dados)
                        {
                            DadosAposta dadosAposta = new DadosAposta();
                            dadosAposta.Id = item.Id;
                            dadosAposta.IdCliente = item.IdCliente;
                            dadosAposta.IdLoteria = item.IdLoteria;
                            dadosAposta.NomeLoteria = ctx.Loterias.FirstOrDefault(c => c.Id == item.IdLoteria).Descricao;
                            dadosAposta.MesDaSorte = item.MesDaSorte;
                            dadosAposta.TimeDoCoracao = item.TimeDoCoracao;
                            dadosAposta.DataAposta = Convert.ToDateTime(item.DataAposta).ToShortDateString();

                            var numerosApostados = ctx.Numerosapostas.Where(c => c.Idaposta == item.Id).ToList();

                            dadosAposta.dadosNumerosApostas = new List<DadosNumerosAposta>();

                            foreach (var item2 in numerosApostados)
                            {
                                DadosNumerosAposta dadosNumerosAposta = new DadosNumerosAposta();

                                dadosNumerosAposta.Dezena = item2.Dezena;
                                dadosNumerosAposta.IdAposta = item2.Idaposta;
                                dadosNumerosAposta.NumeroDezena1 = string.Format("{0:00}", item2.Numerodezena1);
                                dadosNumerosAposta.Sorteadonumerodezena1 = item2.Sorteadonumerodezena1;

                                //SuperSete
                                if (item.IdLoteria == 9)
                                {
                                    dadosNumerosAposta.NumeroDezena2 = item2.Numerodezena2;
                                    dadosNumerosAposta.NumeroDezena3 = item2.Numerodezena3;
                                    dadosNumerosAposta.NumeroDezena4 = item2.Numerodezena4;
                                    dadosNumerosAposta.NumeroDezena5 = item2.Numerodezena5;
                                    dadosNumerosAposta.NumeroDezena6 = item2.Numerodezena6;
                                    dadosNumerosAposta.NumeroDezena7 = item2.Numerodezena7;

                                    dadosNumerosAposta.Sorteadonumerodezena2 = item2.Sorteadonumerodezena2;
                                    dadosNumerosAposta.Sorteadonumerodezena3 = item2.Sorteadonumerodezena3;
                                    dadosNumerosAposta.Sorteadonumerodezena4 = item2.Sorteadonumerodezena4;
                                    dadosNumerosAposta.Sorteadonumerodezena5 = item2.Sorteadonumerodezena5;
                                    dadosNumerosAposta.Sorteadonumerodezena6 = item2.Sorteadonumerodezena6;
                                    dadosNumerosAposta.Sorteadonumerodezena7 = item2.Sorteadonumerodezena7;
                                }


                                dadosAposta.dadosNumerosApostas.Add(dadosNumerosAposta);
                            }

                            //Mais Milionária
                            if (item.IdLoteria == 1)
                            {
                                dadosAposta.dadosNumerosTrevos = new List<DadosNumerosTrevo>();
                                var trevosApostados = ctx.Numerostrevos.Where(c => c.Idaposta == item.Id).ToList();

                                foreach (var item3 in trevosApostados)
                                {
                                    DadosNumerosTrevo dadosNumerosTrevo = new DadosNumerosTrevo
                                    {
                                        Dezena = item3.Dezena,
                                        IdAposta = item3.Idaposta,
                                        NumeroDezena = item3.Numerodezena,
                                        Sorteadonumerodezena = item3.Sorteadonumerodezena
                                    };

                                    dadosAposta.dadosNumerosTrevos.Add(dadosNumerosTrevo);
                                }
                            }

                            lista.Add(dadosAposta);
                        }
                    }

                    return lista;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public DadosAposta CarregarDadosAposta(int id)
        {
            DadosAposta dadosAposta = null;

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var item = (from a in ctx.Apostas
                                where a.Id == id
                                select new
                                {
                                    Id = a.Id,
                                    IdCliente = a.Idcliente,
                                    IdLoteria = a.Idloteria,
                                    MesDaSorte = a.Mesdasorte,
                                    TimeDoCoracao = a.Timedocoracao,
                                    DataAposta = a.Dataaposta,
                                    dadosNumerosApostas = new List<DadosNumerosAposta>(),
                                    dadosNumerosTrevos = new List<DadosNumerosTrevo>(),
                                    dadosNumerosLotomania1 = new List<DadosNumerosLotomania1>(),
                                    dadosNumerosLotomania2 = new List<DadosNumerosLotomania2>(),
                                    dadosNumerosLotomania3 = new List<DadosNumerosLotomania3>(),
                                    dadosNumerosLotomania4 = new List<DadosNumerosLotomania4>(),
                                    dadosNumerosLotomania5 = new List<DadosNumerosLotomania5>(),
                                }).FirstOrDefault();

                    if (item != null)
                    {
                        dadosAposta = new DadosAposta();

                        dadosAposta.Id = item.Id;
                        dadosAposta.IdCliente = item.IdCliente;
                        dadosAposta.IdLoteria = item.IdLoteria;
                        dadosAposta.NomeLoteria = ctx.Loterias.FirstOrDefault(c => c.Id == item.IdLoteria).Descricao;
                        dadosAposta.MesDaSorte = item.MesDaSorte;
                        dadosAposta.TimeDoCoracao = item.TimeDoCoracao;
                        dadosAposta.DataAposta = Convert.ToDateTime(item.DataAposta).ToShortDateString();

                        var numerosApostados = ctx.Numerosapostas.Where(c => c.Idaposta == item.Id).ToList();
                        dadosAposta.dadosNumerosApostas = new List<DadosNumerosAposta>();

                        if (item.IdLoteria == 5)
                        {
                            dadosAposta.dadosNumerosLotomania1 = new List<DadosNumerosLotomania1>();
                            dadosAposta.dadosNumerosLotomania2 = new List<DadosNumerosLotomania2>();
                            dadosAposta.dadosNumerosLotomania3 = new List<DadosNumerosLotomania3>();
                            dadosAposta.dadosNumerosLotomania4 = new List<DadosNumerosLotomania4>();
                            dadosAposta.dadosNumerosLotomania5 = new List<DadosNumerosLotomania5>();
                        }

                        foreach (var item2 in numerosApostados)
                        {
                            //Lotomania
                            if (item.IdLoteria == 5)
                            {
                                if (item2.Dezena <= 10)
                                {
                                    DadosNumerosLotomania1 d = new DadosNumerosLotomania1
                                    {
                                        Dezena = item2.Dezena,
                                        IdAposta = item2.Idaposta,
                                        NumeroDezena = string.Format("{0:00}", item2.Numerodezena1),
                                        Sorteadonumerodezena = item2.Sorteadonumerodezena1
                                    };

                                    dadosAposta.dadosNumerosLotomania1.Add(d);
                                }

                                if (item2.Dezena >= 11 && item2.Dezena <= 20)
                                {
                                    DadosNumerosLotomania2 d = new DadosNumerosLotomania2
                                    {
                                        Dezena = item2.Dezena,
                                        IdAposta = item2.Idaposta,
                                        NumeroDezena = string.Format("{0:00}", item2.Numerodezena1),
                                        Sorteadonumerodezena = item2.Sorteadonumerodezena1,
                                    };

                                    dadosAposta.dadosNumerosLotomania2.Add(d);
                                }

                                if (item2.Dezena >= 21 && item2.Dezena <= 30)
                                {
                                    DadosNumerosLotomania3 d = new DadosNumerosLotomania3
                                    {
                                        Dezena = item2.Dezena,
                                        IdAposta = item2.Idaposta,
                                        NumeroDezena = string.Format("{0:00}", item2.Numerodezena1),
                                        Sorteadonumerodezena = item2.Sorteadonumerodezena1,
                                    };

                                    dadosAposta.dadosNumerosLotomania3.Add(d);
                                }

                                if (item2.Dezena >= 31 && item2.Dezena <= 40)
                                {
                                    DadosNumerosLotomania4 d = new DadosNumerosLotomania4
                                    {
                                        Dezena = item2.Dezena,
                                        IdAposta = item2.Idaposta,
                                        NumeroDezena = string.Format("{0:00}", item2.Numerodezena1),
                                        Sorteadonumerodezena = item2.Sorteadonumerodezena1,
                                    };

                                    dadosAposta.dadosNumerosLotomania4.Add(d);
                                }

                                if (item2.Dezena >= 41 && item2.Dezena <= 50)
                                {
                                    DadosNumerosLotomania5 d = new DadosNumerosLotomania5
                                    {
                                        Dezena = item2.Dezena,
                                        IdAposta = item2.Idaposta,
                                        NumeroDezena = string.Format("{0:00}", item2.Numerodezena1),
                                        Sorteadonumerodezena = item2.Sorteadonumerodezena1,
                                    };

                                    dadosAposta.dadosNumerosLotomania5.Add(d);
                                }
                            }
                            else
                            {
                                DadosNumerosAposta dadosNumerosAposta = new DadosNumerosAposta();

                                dadosNumerosAposta.Dezena = item2.Dezena;
                                dadosNumerosAposta.IdAposta = item2.Idaposta;
                                dadosNumerosAposta.NumeroDezena1 = string.Format("{0:00}", item2.Numerodezena1);
                                dadosNumerosAposta.Sorteadonumerodezena1 = item2.Sorteadonumerodezena1;

                                //SuperSete
                                if (item.IdLoteria == 9)
                                {
                                    dadosNumerosAposta.NumeroDezena2 = item2.Numerodezena2;
                                    dadosNumerosAposta.NumeroDezena3 = item2.Numerodezena3;
                                    dadosNumerosAposta.NumeroDezena4 = item2.Numerodezena4;
                                    dadosNumerosAposta.NumeroDezena5 = item2.Numerodezena5;
                                    dadosNumerosAposta.NumeroDezena6 = item2.Numerodezena6;
                                    dadosNumerosAposta.NumeroDezena7 = item2.Numerodezena7;
                                    dadosNumerosAposta.Sorteadonumerodezena2 = item2.Sorteadonumerodezena2;
                                    dadosNumerosAposta.Sorteadonumerodezena3 = item2.Sorteadonumerodezena3;
                                    dadosNumerosAposta.Sorteadonumerodezena4 = item2.Sorteadonumerodezena4;
                                    dadosNumerosAposta.Sorteadonumerodezena5 = item2.Sorteadonumerodezena5;
                                    dadosNumerosAposta.Sorteadonumerodezena6 = item2.Sorteadonumerodezena6;
                                    dadosNumerosAposta.Sorteadonumerodezena7 = item2.Sorteadonumerodezena7;
                                }


                                dadosAposta.dadosNumerosApostas.Add(dadosNumerosAposta);
                            }
                        }

                        //Mais Milionária
                        if (item.IdLoteria == 1)
                        {
                            dadosAposta.dadosNumerosTrevos = new List<DadosNumerosTrevo>();
                            var trevosApostados = ctx.Numerostrevos.Where(c => c.Idaposta == item.Id).ToList();

                            foreach (var item3 in trevosApostados)
                            {
                                DadosNumerosTrevo dadosNumerosTrevo = new DadosNumerosTrevo
                                {
                                    Dezena = item3.Dezena,
                                    IdAposta = item3.Idaposta,
                                    NumeroDezena = item3.Numerodezena,
                                    Sorteadonumerodezena = item3.Sorteadonumerodezena
                                };

                                dadosAposta.dadosNumerosTrevos.Add(dadosNumerosTrevo);
                            }
                        }

                    }

                    return dadosAposta;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public string ExcluirAposta(int id)
        {
            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var aposta = ctx.Apostas.FirstOrDefault(c => c.Id == id);

                    if (aposta != null)
                    {
                        var numerosApostas = ctx.Numerosapostas.Where(c => c.Idaposta == aposta.Id).ToList();
                        var numerosTrevos = ctx.Numerostrevos.Where(c => c.Idaposta == aposta.Id).ToList();

                        if (numerosApostas != null)
                        {
                            foreach (var item in numerosApostas)
                            {
                                var itemExclusao = ctx.Numerosapostas.FirstOrDefault(c => c.Id == item.Id);

                                ctx.Numerosapostas.Remove(itemExclusao);
                                ctx.SaveChanges();
                            }
                        }

                        if (numerosTrevos != null)
                        {
                            foreach (var item in numerosTrevos)
                            {
                                var itemExclusao = ctx.Numerostrevos.FirstOrDefault(c => c.Id == item.Id);

                                ctx.Numerostrevos.Remove(itemExclusao);
                                ctx.SaveChanges();
                            }
                        }

                        ctx.Apostas.Remove(aposta);
                        ctx.SaveChanges();

                        return "Aposta excluída com sucesso!";
                    }
                    else
                    {
                        return "Aposta não excluída!";
                    }
                }
            }
            catch (Exception)
            {
                return "Erro ao excluir aposta";
            }
        }

        private List<int> GerarNumeros(int qtdeNumeros, int numInicial, int numFinal)
        {
            List<int> numeros = new List<int>();
            Random random = new Random();

            while (numeros.Count < qtdeNumeros)
            {
                int numeroAleatorio = random.Next(numInicial, numFinal);
                if (!numeros.Contains(numeroAleatorio))
                {
                    numeros.Add(numeroAleatorio);
                }
            }

            // Ordena os números em ordem crescente
            numeros.Sort();

            return numeros;
        }

        private string GerarMesDaSorte()
        {
            string[] meses = {
                "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
            };

            Random random = new Random();
            int indice = random.Next(0, 12);

            return meses[indice];
        }

        private string GerarTimeDoCoracao()
        {
            // Lista de times do coração da Timemania
            string[] timesDoCoracao = {
                "América (MG)",
                "América (RN)",
                "Atlético (MG)",
                "Atlético (PR)",
                "Bahia",
                "Bangu",
                "Botafogo (RJ)",
                "Botafogo (SP)",
                "Ceará",
                "Corinthians",
                "Coritiba",
                "Cruzeiro",
                "Flamengo",
                "Fluminense",
                "Fortaleza",
                "Goiás",
                "Grêmio",
                "Guarani",
                "Internacional",
                "Palmeiras",
                "Paysandu",
                "Ponte Preta",
                "Portuguesa",
                "Remo",
                "Santa Cruz",
                "Santos",
                "São Paulo",
                "Sport",
                "Vasco da Gama",
                "Vila Nova",
                "Vitória"
            };

            Random random = new Random();
            int indice = random.Next(0, timesDoCoracao.Length);

            return timesDoCoracao[indice];
        }
    }

    public class ApostaModel
    {
        public string Token { get; set; }

        public string NomeLoteria { get; set; }
    }

    public class DadosAposta
    {
        public int Id { get; set; }

        public int IdCliente { get; set; }


        public int IdLoteria { get; set; }

        public string NomeLoteria { get; set; }

        public string? MesDaSorte { get; set; }

        public string? TimeDoCoracao { get; set; }

        public string DataAposta { get; set; }

        public List<DadosNumerosAposta> dadosNumerosApostas { get; set; }

        public List<DadosNumerosTrevo> dadosNumerosTrevos { get; set; }

        public List<DadosNumerosLotomania1> dadosNumerosLotomania1 { get; set; }
        public List<DadosNumerosLotomania2> dadosNumerosLotomania2 { get; set; }
        public List<DadosNumerosLotomania3> dadosNumerosLotomania3 { get; set; }
        public List<DadosNumerosLotomania4> dadosNumerosLotomania4 { get; set; }
        public List<DadosNumerosLotomania5> dadosNumerosLotomania5 { get; set; }

    }

    public class DadosNumerosAposta
    {
        public int Id { get; set; }

        public int IdAposta { get; set; }

        public int? Dezena { get; set; }

        public string? NumeroDezena1 { get; set; }

        public int? NumeroDezena2 { get; set; }

        public int? NumeroDezena3 { get; set; }

        public int? NumeroDezena4 { get; set; }

        public int? NumeroDezena5 { get; set; }

        public int? NumeroDezena6 { get; set; }

        public int? NumeroDezena7 { get; set; }

        public sbyte? Sorteadonumerodezena1 { get; set; }

        public sbyte? Sorteadonumerodezena2 { get; set; }

        public sbyte? Sorteadonumerodezena3 { get; set; }

        public sbyte? Sorteadonumerodezena4 { get; set; }

        public sbyte? Sorteadonumerodezena5 { get; set; }

        public sbyte? Sorteadonumerodezena6 { get; set; }

        public sbyte? Sorteadonumerodezena7 { get; set; }
    }

    public class DadosNumerosTrevo
    {
        public int Id { get; set; }

        public int IdAposta { get; set; }

        public int? Dezena { get; set; }

        public int? NumeroDezena { get; set; }

        public sbyte? Sorteadonumerodezena { get; set; }
    }

    public class DadosNumerosLotomania1
    {
        public int Id { get; set; }

        public int IdAposta { get; set; }

        public int? Dezena { get; set; }

        public string? NumeroDezena { get; set; }
        public sbyte? Sorteadonumerodezena { get; set; }
    }

    public class DadosNumerosLotomania2
    {
        public int Id { get; set; }

        public int IdAposta { get; set; }

        public int? Dezena { get; set; }

        public string? NumeroDezena { get; set; }
        public sbyte? Sorteadonumerodezena { get; set; }
    }

    public class DadosNumerosLotomania3
    {
        public int Id { get; set; }

        public int IdAposta { get; set; }

        public int? Dezena { get; set; }

        public string? NumeroDezena { get; set; }
        public sbyte? Sorteadonumerodezena { get; set; }
    }

    public class DadosNumerosLotomania4
    {
        public int Id { get; set; }

        public int IdAposta { get; set; }

        public int? Dezena { get; set; }

        public string? NumeroDezena { get; set; }
        public sbyte? Sorteadonumerodezena { get; set; }
    }

    public class DadosNumerosLotomania5
    {
        public int Id { get; set; }

        public int IdAposta { get; set; }

        public int? Dezena { get; set; }

        public string? NumeroDezena { get; set; }
        public sbyte? Sorteadonumerodezena { get; set; }
    }
}
