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
    public class ClientesController : ControllerBase
    {
        ClienteDao dao = new ClienteDao();
        /// <summary>
        /// Incluir/Alterar dados Cliente
        /// </summary>
        /// <param name="ClienteModel"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [Route("Salvar")]
        public string Salvar([FromBody] ClienteModel ClienteModel)
        {
            return dao.Salvar(ClienteModel);
        }

        /// <summary>
        /// Exclusão dados Cliente
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
        /// Carrega os dados Cliente
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
        /// Lista dados Cliente
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [Route("ListarDados")]
        public List<Cliente> ListarDados()
        {
            return dao.ListarDados();
        }

        /// <summary>
        /// Realiza o Login Cliente, retornando o token a ser utilizado
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
        /// Carrega os dados Cliente logado
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
        /// Realiza logoff Cliente
        /// </summary>
        /// <param name="token"></param>
        [HttpPost]
        [Route("RealizarLogoff")]
        public void RealizarLogoff(string token)
        {
            dao.RealizarLogoff(token);
        }

        /// <summary>
        /// Envia link Recuperação Senha Cliente
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
        /// Realiza alteração senha Cliente
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
        /// Gera nova senha para o Cliente
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
        /// Ativa o Cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AtivarClienteEnviarSenhaAcesso")]
        public string AtivarClienteEnviarSenhaAcesso(int id)
        {
            return dao.AtivarClienteEnviarSenhaAcesso(id);
        }

        [HttpPost]
        [Route("CarregarDadosAfiliado")]
        public Dictionary<string, object> CarregarDadosAfiliado(string codAfiliado)
        {
            return dao.CarregarDadosAfiliado(codAfiliado);
        }
    }
}
