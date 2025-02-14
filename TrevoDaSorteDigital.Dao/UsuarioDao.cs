using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrevoDaSorteDigital.Dao.Models;

namespace TrevoDaSorteDigital.Dao
{
    public class UsuarioDao : Utils
    {
        /// <summary>
        /// Incluir/Alterar dados Usuário
        /// </summary>
        /// <param name="UsuarioModel"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string Salvar(UsuarioModel UsuarioModel)
        {
            bool novoRegistro = false;

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = ctx.Usuarios.FirstOrDefault(c => c.Id == UsuarioModel.Id);
                    novoRegistro = dados != null ? false : true;

                    if (novoRegistro == true)
                    {
                        bool existeEmail = ctx.Usuarios.FirstOrDefault(c => c.Email == UsuarioModel.Email) != null ? true : false;

                        if (existeEmail == true)
                        {
                            return "Login já cadastrado";
                        }

                        dados = new Usuario();
                        dados.Usuariologado = 0;
                    }
                    else
                    {
                        bool existeEmail = ctx.Usuarios.FirstOrDefault(c => c.Email == UsuarioModel.Email && c.Id != UsuarioModel.Id) != null ? true : false;

                        if (existeEmail == true)
                        {
                            return "Login já cadastrado";
                        }
                    }

                    dados.Email = UsuarioModel.Email;
                    dados.Senha = Criptografar(UsuarioModel.Senha);

                    if (novoRegistro == true)
                    {
                        ctx.Usuarios.Add(dados);
                    }

                    ctx.SaveChanges();

                    return "Dados Usuario salvo com sucesso!";
                }
            }
            catch (Exception e)
            {
                return "Erro ao salvar dados Usuario: \n" + e.Message;
            }
        }

        /// <summary>
        /// Exclusão dados Usuário
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
                    var dados = ctx.Usuarios.FirstOrDefault(c => c.Id == id);

                    if (dados != null)
                    {
                        if (dados.Usuariologado == 1)
                        {
                            return "Usuário está logado no sistema. Impossível excluir!";
                        }

                        ctx.Usuarios.Remove(dados);
                        ctx.SaveChanges();

                        return "Dados Usuario excluído com sucesso!";

                    }
                    else
                    {
                        return "Dados Usuario não excluído";
                    }
                }
            }
            catch (Exception e)
            {
                return "Erro ao excluir dados Usuario: \n" + e.Message;
            }
        }

        /// <summary>
        /// Carrega os dados Usuário
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
                    var dados = ctx.Usuarios.FirstOrDefault(c => c.Id == id);

                    if (dados != null)
                    {
                        var ultimoAcesso = dados.Ultimoacesso == null ? "" : FormatarDataHora(Convert.ToDateTime(dados.Ultimoacesso).ToString("dd/MM/yyyy HH:mm:ss"));

                        wDados = new Dictionary<string, object>
                  {
                      { "Id", dados.Id },
                      { "Email", dados.Email },
                      { "Senha", Descriptografar(dados.Senha) },
                      { "UsuarioLogado", dados.Usuariologado == 1 ? "Sim" : "Não" },
                      { "UltimoAcesso", ultimoAcesso },
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
        /// Lista dados Usuário
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Usuario> ListarDados()
        {

            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    return ctx.Usuarios.OrderBy(c => c.Id).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Realiza o Login Usuário, retornando o token a ser utilizado
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
                    var dados = (from c in ctx.Usuarios
                                 where c.Email == model.Login && c.Senha == senhaCriptografada
                                 select c).FirstOrDefault();

                    if (dados != null)
                    {
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
        /// Carrega os dados Usuário logado
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
                    var dados = ctx.Usuarios.FirstOrDefault(c => c.Tokenacesso == token);

                    if (dados != null)
                    {
                        dadosLogado = new Dictionary<string, string>
                 {
                   { "NomeUsuario", "Bem vindo Usuário: "+ dados.Email },
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
        /// Realiza logoff Usuário
        /// </summary>
        /// <param name="token"></param>
        public void RealizarLogoff(string token)
        {
            try
            {
                using (ApiTrevoDasorteDigitalContext ctx = new ApiTrevoDasorteDigitalContext())
                {
                    var dados = ctx.Usuarios.FirstOrDefault(a => a.Tokenacesso == token);

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
        /// Envia link Recuperação Senha Usuário
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
                    var dados = ctx.Usuarios.FirstOrDefault(a => a.Email == login);

                    if (dados != null)
                    {
                        serialRecovery = GerarToken(24);
                        dados.Serialrecovery = serialRecovery;
                        ctx.SaveChanges();

                        if (EmailEnviado("Usuario", dados.Email, "Recuperação de Senha acesso Sistema Gerador de Usuarios trevodasortedigital.com.br", MontarCorpoEmailRecSenha(serialRecovery), false))
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
        /// Realiza alteração senha Usuário
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
                    var dados = ctx.Usuarios.FirstOrDefault(c => c.Serialrecovery == alterarSenhaModel.SerialRecovery);

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

        private string MontarCorpoEmailRecSenha(string serialRecovery)
        {
            string HeaderEmail = "https://admin.trevodasortedigital.com.br/assets/img/logo.jpg";

            string corpoEmail = string.Empty;

            corpoEmail += "<div style='text-align:center'><div style='max-width: 600px; margin: 0 auto;'>" +
                      $"<img src='{HeaderEmail}' width=300 heigth=300>" + $"<br /></div><br />";
            corpoEmail += "<h2>Olá</h2>";
            corpoEmail += "<p>Recebemos sua solicitação de recuperação de senha.</p>";
            corpoEmail += $"<p>Para criar uma nova,<a href='https://admin.trevodasortedigital.com.br/Home/RecuperacaoSenha?code={serialRecovery}'>Clique aqui</a></p>";
            corpoEmail += "<p><b>Importante:</b>o link é válido por 24 horas.</p>";
            corpoEmail += "<p>Após este período, você deverá solicitar um novo, tá bem?!</p>";
            corpoEmail += "<p>Caso não tenha sido você, por gentileza, desconsidere este e-mail.</p>";
            corpoEmail += "<p>Se precisar de ajuda, conte com a gente!*</p><br />";
            corpoEmail += "<p>Equipe Sorte Digital</p>";

            return corpoEmail;
        }
    }

    public class LoginModel
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }

    public class AlterarSenhaModel
    {
        public string SerialRecovery { get; set; }
        public string NovaSenha { get; set; }
    }

    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
