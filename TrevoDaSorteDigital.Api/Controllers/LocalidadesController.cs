using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TrevoDaSorteDigital.Dao;

namespace TrevoDaSorteDigital.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocalidadesController : ControllerBase
    {
        LocalidadeDao dao = new LocalidadeDao();

        [HttpPost]
        [Route("ListarEstados")]
        public List<TrevoDaSorteDigital.Dao.Models.Estado> ListarEstados()
        {
            return dao.ListarEstados();
        }

        [HttpPost]
        [Route("CarregarDadosCep")]
        public Dao.Models.Registrocep CarregarDadosCep([FromBody] CepModel cepModel)
        {
            return dao.CarregarDadosCep(cepModel);
        }
    }
}
