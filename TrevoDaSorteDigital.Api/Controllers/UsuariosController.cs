using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using TrevoDaSorteDigital.Dao;
using TrevoDaSorteDigital.Dao.Models;

namespace TrevoDaSorteDigital.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        UsuarioDao dao = new UsuarioDao();

        /// <summary>
        /// Incluir/Alterar dados Usuário
        /// </summary>
        /// <param name="UsuarioModel"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [Route("Salvar")]
        public string Salvar([FromBody] TrevoDaSorteDigital.Dao.UsuarioModel UsuarioModel)
        {
            return dao.Salvar(UsuarioModel);
        }

        /// <summary>
        /// Exclusão dados Usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [Route("Excluir")]
        public string Excluir(int id)
        {
            return dao.Excluir(id);
        }

        /// <summary>
        /// Carrega os dados Usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [Route("CarregarDados")]
        public Dictionary<string, object> CarregarDados(int id)
        {
            return dao.CarregarDados(id);
        }

        /// <summary>
        /// Lista dados Usuário
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [Route("ListarDados")]
        public List<Usuario> ListarDados()
        {
            return dao.ListarDados();
        }

        /// <summary>
        /// Realiza o Login Usuário, retornando o token a ser utilizado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RetornarTokenLogin")]
        public string RetornarTokenLogin([FromBody] TrevoDaSorteDigital.Dao.LoginModel model)
        {
            return dao.RetornarTokenLogin(model);
        }

        /// <summary>
        /// Carrega os dados Usuário logado
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CarregarDadosUsuarioLogado")]
        public Dictionary<string, string> CarregarDadosUsuarioLogado(string token)
        {
            return dao.CarregarDadosUsuarioLogado(token);
        }


        /// <summary>
        /// Realiza logoff Usuário
        /// </summary>
        /// <param name="token"></param>
        [HttpPost]
        [Route("RealizarLogoff")]
        public void RealizarLogoff(string token)
        {
            dao.RealizarLogoff(token);
        }

        /// <summary>
        /// Envia link Recuperação Senha Usuário
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RecuperarEmailSenha")]
        public string RecuperarEmailSenha(string login)
        {
            return dao.RecuperarEmailSenha(login);
        }

        /// <summary>
        /// Realiza alteração senha Usuário
        /// </summary>
        /// <param name="alterarSenhaModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AlterarSenha")]
        public string AlterarSenha([FromBody] TrevoDaSorteDigital.Dao.AlterarSenhaModel alterarSenhaModel)
        {
            return dao.AlterarSenha(alterarSenhaModel);
        }
    }
}
