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
    public class AfiliadosController : ControllerBase
    {
        AfiliadoDao dao = new AfiliadoDao();

        /// <summary>
        /// Incluir/Alterar dados Afiliado
        /// </summary>
        /// <param name="afiliadoModel"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [Route("Salvar")]
        public string Salvar([FromBody] AfiliadoModel afiliadoModel)
        {
            return dao.Salvar(afiliadoModel);
        }

        /// <summary>
        /// Exclusão dados Afiliado
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
        /// Carrega os dados Afiliado
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
        /// Lista dados Afiliado
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [Route("ListarDados")]
        public List<Afiliado> ListarDados()
        {

            return dao.ListarDados();
        }

        /// <summary>
        /// Realiza o Login Afiliado, retornando o token a ser utilizado
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
        /// Carrega os dados Afiliado logado
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
        /// Realiza logoff Afiliado
        /// </summary>
        /// <param name="token"></param>
        [HttpPost]
        [Route("RealizarLogoff")]
        public void RealizarLogoff(string token)
        {
            dao.RealizarLogoff(token);
        }

        /// <summary>
        /// Envia link Recuperação Senha Afiliado
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
        /// Realiza alteração senha afiliado
        /// </summary>
        /// <param name="alterarSenhaModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AlterarSenha")]
        public string AlterarSenha([FromBody] TrevoDaSorteDigital.Dao.AlterarSenhaModel alterarSenhaModel)
        {
            return dao.AlterarSenha(alterarSenhaModel);
        }

        /// <summary>
        /// Gera nova senha para o afiliado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GerarNovaSenha")]
        public string GerarNovaSenha(int id)
        {
            return dao.GerarNovaSenha(id);
        }

        /// <summary>
        /// Ativa o Afiliado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AtivarAfiliadoEnviarSenhaAcesso")]
        public string AtivarAfiliadoEnviarSenhaAcesso(int id)
        {
            return dao.AtivarAfiliadoEnviarSenhaAcesso(id);
        }
    }
}
