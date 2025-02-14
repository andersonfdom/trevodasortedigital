using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TrevoDaSorteDigital.Dao;

namespace TrevoDaSorteDigital.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApostasController : ControllerBase
    {
        ApostaDao dao = new ApostaDao();

        [HttpPost]
        [Route("CriarAposta")]
        public int CriarAposta([FromBody] ApostaModel apostaModel)
        {
            return dao.CriarAposta(apostaModel);
        }

        [HttpPost]
        [Route("ListarApostas")]
        public List<DadosAposta> ListarApostas(string token)
        {
            return dao.ListarApostas(token);
        }

        [HttpPost]
        [Route("CarregarDadosAposta")]
        public DadosAposta CarregarDadosAposta(int id)
        {
            return dao.CarregarDadosAposta(id);
        }

        [HttpPost]
        [Route("ExcluirAposta")]
        public string ExcluirAposta(int id)
        {
            return dao.ExcluirAposta(id);
        }

    }
}
