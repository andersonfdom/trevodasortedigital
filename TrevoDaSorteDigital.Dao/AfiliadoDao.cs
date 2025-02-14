using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrevoDaSorteDigital.Dao.Models;

namespace TrevoDaSorteDigital.Dao
{
    public class AfiliadoDao : Utils
    {
        /// <summary>
        /// Incluir/Alterar dados Afiliado
        /// </summary>
        /// <param name="afiliadoModel"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string Salvar(AfiliadoModel afiliadoModel)
        {
            bool novoRegistro = false;

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = ctx.Afiliados.FirstOrDefault(c => c.Id == afiliadoModel.Id);
                    novoRegistro = dados != null ? false : true;

                    if (novoRegistro == true)
                    {
                        bool existeCpf = ctx.Afiliados.FirstOrDefault(c => c.Cpf == afiliadoModel.Cpf) != null ? true : false;
                        bool existeEmail = ctx.Afiliados.FirstOrDefault(c => c.Email == afiliadoModel.Email) != null ? true : false;

                        if (existeCpf == true)
                        {
                            return "Cpf já cadastrado";
                        }

                        if (existeEmail == true)
                        {
                            return "E-mail já cadastrado";
                        }

                        dados = new Afiliado();
                        dados.Ativo = 0;
                        dados.Usuariologado = 0;
                        dados.Codafiliado = GerarToken(12);
                    }
                    else
                    {
                        bool existeCpf = ctx.Afiliados.FirstOrDefault(c => c.Cpf == afiliadoModel.Cpf && c.Id != afiliadoModel.Id) != null ? true : false;
                        bool existeEmail = ctx.Afiliados.FirstOrDefault(c => c.Email == afiliadoModel.Email && c.Id != afiliadoModel.Id) != null ? true : false;

                        if (existeCpf == true)
                        {
                            return "Cpf já cadastrado";
                        }

                        if (existeEmail == true)
                        {
                            return "E-mail já cadastrado";
                        }
                    }

                    dados.Cpf = afiliadoModel.Cpf;
                    dados.Email = afiliadoModel.Email;
                    dados.Nome = afiliadoModel.Nome;
                    dados.Telefone = afiliadoModel.Telefone;

                    if (novoRegistro == true)
                    {
                        ctx.Afiliados.Add(dados);
                    }

                    ctx.SaveChanges();

                    if (novoRegistro == true)
                    {
                        EmailEnviado("Administrativo Trevo da Sorte Digital", "cadastro@trevodasortedigital.com.br", "Cadastro Afiliado pendente aprovação Sistema Gerador de Apostas trevodasortedigital.com.br", MontarCorpoEmailCadastroAfiliado(dados),true);
                    }

                    return "Dados Afiliado salvo com sucesso!";
                }
            }
            catch (Exception e)
            {
                return "Erro ao salvar dados Afiliado: \n" + e.Message;
            }
        }

        /// <summary>
        /// Exclusão dados Afiliado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string Excluir(int id)
        {
            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = ctx.Afiliados.FirstOrDefault(c => c.Id == id);

                    if (dados != null)
                    {
                        var clientesAfiliado = (from c in ctx.Clientes
                                                where c.Codafiliado == dados.Codafiliado
                                                select c).ToList();

                        if (clientesAfiliado != null)
                        {
                            foreach (var item in clientesAfiliado)
                            {
                                item.Codafiliado = "";
                                ctx.SaveChanges();
                            }
                        }

                        ctx.Afiliados.Remove(dados);
                        ctx.SaveChanges();

                        return "Dados Afiliado excluído com sucesso!";

                    }
                    else
                    {
                        return "Dados Afiliado não excluído";
                    }
                }
            }
            catch (Exception e)
            {
                return "Erro ao excluir dados Afiliado: \n" + e.Message;
            }
        }

        /// <summary>
        /// Carrega os dados Afiliado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public Dictionary<string, object> CarregarDados(int id)
        {
            Dictionary<string, object> wDados = null;

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = ctx.Afiliados.FirstOrDefault(c => c.Id == id);

                    if (dados != null)
                    {
                        var ultimoAcesso = dados.Ultimoacesso == null ? "" : FormatarDataHora(Convert.ToDateTime(dados.Ultimoacesso).ToString("dd/MM/yyyy HH:mm:ss"));

                        wDados = new Dictionary<string, object>
                {
                    { "Id", dados.Id },
                    { "Nome", dados.Nome },
                    { "Telefone", dados.Telefone },
                    { "Email", dados.Email },
                    { "Cpf", dados.Cpf },
                    { "CodAfiliado", dados.Codafiliado },
                    { "UsuarioLogado", dados.Usuariologado == 1 ? "Sim" : "Não" },
                    { "UltimoAcesso", ultimoAcesso },
                    { "Ativo", dados.Ativo == 1 ? "Sim" : "Não" }
                };
                    }

                    return wDados;

                }
            }
            catch (Exception)
            {
                return wDados;
            }
        }

        /// <summary>
        /// Lista dados Afiliado
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Afiliado> ListarDados()
        {
            ArrayList lista = null;

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    return ctx.Afiliados.OrderBy(c => c.Id).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Realiza o Login Afiliado, retornando o token a ser utilizado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string RetornarTokenLogin(LoginModel model)
        {
            var token = string.Empty;

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var senhaCriptografada = Criptografar(model.Senha);
                    var dados = (from c in ctx.Afiliados
                                 where c.Email == model.Login && c.Senha == senhaCriptografada
                                 select c).FirstOrDefault();

                    if (dados != null)
                    {
                        if (dados.Ativo == 0)
                        {
                            return "Seu cadastro ainda não está ativo! Favor entrar em contato com a equipe Sorte Digital.";
                        }

                        token = GerarToken(24);
                        dados.Tokenacesso = token;
                        dados.Usuariologado = 1;
                        dados.Ultimoacesso = DateTime.Now;
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return token;
        }

        /// <summary>
        /// Carrega os dados Afiliado logado
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Dictionary<string, string> CarregarDadosUsuarioLogado(string token)
        {
            Dictionary<string, string> dadosLogado = null;

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = ctx.Afiliados.FirstOrDefault(c => c.Tokenacesso == token);

                    if (dados != null)
                    {
                        dadosLogado = new Dictionary<string, string>
                {
                    { "NomeUsuario", "Bem vindo Usuário: "+ dados.Nome },
                    { "UltimoAcesso","Seu último acesso: " + FormatarDataHora(Convert.ToDateTime(dados.Ultimoacesso).ToString("dd/MM/yyyy HH:mm:ss")) }
                };
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return dadosLogado;
        }


        /// <summary>
        /// Realiza logoff Afiliado
        /// </summary>
        /// <param name="token"></param>
        public void RealizarLogoff(string token)
        {
            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = ctx.Afiliados.FirstOrDefault(a => a.Tokenacesso == token);

                    if (dados != null)
                    {
                        dados.Tokenacesso = "";
                        dados.Usuariologado = 0;
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Envia link Recuperação Senha Afiliado
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public string RecuperarEmailSenha(string login)
        {
            string serialRecovery = "";

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = ctx.Afiliados.FirstOrDefault(a => a.Email == login);

                    if (dados.Ativo == 0)
                    {
                        return "Seu cadastro não está ativo! Favor aguardar a aprovação de cadastro pela equipe Trevo da Sorte Digital.";
                    }

                    if (dados != null)
                    {
                        serialRecovery = GerarToken(24);
                        dados.Serialrecovery = serialRecovery;
                        ctx.SaveChanges();

                        if (EmailEnviado("Afiliado", dados.Email, "Recuperação de Senha acesso Sistema Gerador de afiliados trevodasortedigital.com.br", MontarCorpoEmailRecSenha(serialRecovery), false))
                        {
                            return $"Foi enviado um e-mail para {dados.Email}. Verifique sua Caixa de Entrada de e-mail ou SPAM e siga as intruções para Recuperação de Senha.";
                        }
                        else
                        {
                            return "E-mail não cadastrado";
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return serialRecovery;
        }

        /// <summary>
        /// Realiza alteração senha afiliado
        /// </summary>
        /// <param name="alterarSenhaModel"></param>
        /// <returns></returns>
        public string AlterarSenha(AlterarSenhaModel alterarSenhaModel)
        {
            string retorno = "";

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = ctx.Afiliados.FirstOrDefault(c => c.Serialrecovery == alterarSenhaModel.SerialRecovery);

                    if (dados != null)
                    {
                        dados.Senha = Criptografar(alterarSenhaModel.NovaSenha);
                        dados.Serialrecovery = "";
                        ctx.SaveChanges();
                        retorno = "Senha alterada com sucesso!";
                    }
                    else
                    {
                        retorno = "Token inválido.";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return retorno;
        }

        /// <summary>
        /// Gera nova senha para o afiliado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GerarNovaSenha(int id)
        {
            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = ctx.Afiliados.FirstOrDefault(c => c.Id == id);
                    dados.Senha = Criptografar(RemoverCaracteresEspeciais(dados.Cpf));

                    ctx.SaveChanges();


                    if (EmailEnviado("Afiliado", dados.Email, "Envio dados acesso Sistema Gerador de afiliados trevodasortedigital.com.br", MontarCorpoEmailGerarSenhaAcesso(dados), true))
                    {
                        return $"Foi enviado um e-mail para o Afiliado: {dados.Nome} e-mail: {dados.Email}. Peça para o Afiliado verificar sua Caixa de Entrada de e-mail ou SPAM para que siga as instruções para Acesso ao Sistema Gerador de afiliados.";
                    }
                    else
                    {
                        return "E-mail não cadastrado";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Ativa o Afiliado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string AtivarAfiliadoEnviarSenhaAcesso(int id)
        {
            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = ctx.Afiliados.FirstOrDefault(c => c.Id == id);
                    dados.Senha = Criptografar(RemoverCaracteresEspeciais(dados.Cpf));
                    dados.Ativo = 1;

                    ctx.SaveChanges();


                    if (EmailEnviado("Afiliado", dados.Email, "Aprovação cadastro e envio dados acesso Sistema Gerador de afiliados trevodasortedigital.com.br", MontarCorpoEmailAprovacaoCadastroGerarSenhaAcesso(dados), true))
                    {
                        return $"Foi enviado um e-mail para o Afiliado: {dados.Nome} e-mail: {dados.Email} confirmando a aprovação cadastro Afiliado. Peça para o Afiliado verificar sua Caixa de Entrada de e-mail ou SPAM para que siga as instruções para Acesso ao Sistema Gerador de afiliados.";
                    }
                    else
                    {
                        return "E-mail não cadastrado";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string MontarCorpoEmailRecSenha(string serialRecovery)
        {
            string HeaderEmail = "https://admin.trevodasortedigital.com.br/assets/img/logo.jpg";

            string corpoEmail = string.Empty;

            corpoEmail += "<div style='text-align:center'><div style='max-width: 600px; margin: 0 auto;'>" +
                      $"<img src='{HeaderEmail}' width=300 heigth=300>" + $"<br /></div><br />";
            corpoEmail += "<h2>Olá</h2>";
            corpoEmail += "<p>Recebemos sua solicitação de recuperação de senha.</p>";
            corpoEmail += $"<p>Para criar uma nova,<a href='https://afiliados.trevodasortedigital.com.br/Home/RecuperacaoSenha?code={serialRecovery}'>Clique aqui</a></p>";
            corpoEmail += "<p><b>Importante:</b>o link é válido por 24 horas.</p>";
            corpoEmail += "<p>Após este período, você deverá solicitar um novo, tá bem?!</p>";
            corpoEmail += "<p>Caso não tenha sido você, por gentileza, desconsidere este e-mail.</p>";
            corpoEmail += "<p>Se precisar de ajuda, conte com a gente!*</p><br />";
            corpoEmail += "<p>Equipe Sorte Digital</p>";

            return corpoEmail;
        }

        private string MontarCorpoEmailGerarSenhaAcesso(Afiliado dados)
        {
            string HeaderEmail = "https://admin.trevodasortedigital.com.br/assets/img/logo.jpg";
            string corpoEmail = string.Empty;

            corpoEmail += "<div style='text-align:center'><div style='max-width: 600px; margin: 0 auto;'>" +
                      $"<img src='{HeaderEmail}' width=300 heigth=300>" + $"<br /></div><br />";
            corpoEmail += "<h2>Prezado(a) Afiliado " + dados.Nome + "</h2>";
            corpoEmail += $"<p>Segue abaixo os dados de acesso:</p>";
            corpoEmail += $"<p>Login: {dados.Email}</p>";
            corpoEmail += $"<p>Senha: {Descriptografar(dados.Senha)}</p>";
            corpoEmail += $"<p>Para acessar o Sistema,<a href='https://afiliados.trevodasortedigital.com.br'>Clique aqui</a></p>";
            corpoEmail += "<br />";
            corpoEmail += "<p>Se precisar de ajuda, conte com a gente!*</p><br />";
            corpoEmail += "<p>Equipe Sorte Digital</p>";

            return corpoEmail;
        }

        private string MontarCorpoEmailAprovacaoCadastroGerarSenhaAcesso(Afiliado dados)
        {
            string HeaderEmail = "https://admin.trevodasortedigital.com.br/assets/img/logo.jpg";
            string corpoEmail = string.Empty;

            corpoEmail += "<div style='text-align:center'><div style='max-width: 600px; margin: 0 auto;'>" +
                     $"<img src='{HeaderEmail}' width=300 heigth=300>" + $"<br /></div><br />";

            corpoEmail += $"<h2>Segue os dados Afiliado abaixo para aprovação</h2>";
            corpoEmail += "<br />";
            corpoEmail += "<p><b>Data de Cadastro: </b>" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "</p>";
            corpoEmail += $"<p><b>Nome: </b>{dados.Nome}</p>";
            corpoEmail += $"<p><b>Cpf:  </b>{dados.Cpf}</p>";
            corpoEmail += $"<p><b>Telefone:  </b>{dados.Telefone}</p>";
            corpoEmail += $"<p><b>E-mail:  </b>{dados.Email}</p>";


            return corpoEmail;
        }

        private string MontarCorpoEmailCadastroAfiliado(Afiliado dados)
        {
            string HeaderEmail = "https://admin.trevodasortedigital.com.br/assets/img/logo.jpg";
            string corpoEmail = string.Empty;

            corpoEmail += "<div style='text-align:center'><div style='max-width: 600px; margin: 0 auto;'>" +
                     $"<img src='{HeaderEmail}' width=300 heigth=300>" + $"<br /></div><br />";

            corpoEmail += $"<h2>Segue os dados Afiliado abaixo para aprovação</h2>";
            corpoEmail += "<br />";
            corpoEmail += "<p><b>Data de Cadastro: </b>" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "</p>";
            corpoEmail += $"<p><b>Nome: </b>{dados.Nome}</p>";
            corpoEmail += $"<p><b>Cpf:  </b>{dados.Cpf}</p>";
            corpoEmail += $"<p><b>Telefone:  </b>{dados.Telefone}</p>";
            corpoEmail += $"<p><b>E-mail:  </b>{dados.Email}</p>";


            return corpoEmail;
        }
    }

    public class AfiliadoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }
    }
}
