using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TrevoDaSorteDigital.Dao;

namespace TrevoDaSorteDigital.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoteriasController : ControllerBase
    {
        LoteriaDao dao = new LoteriaDao();

        [HttpPost]
        [Route("ListarLoterias")]
        public List<LoteriaModel> ListarLoterias()
        {

            return dao.ListarLoterias();
        }

        [HttpPost]
        [Route("BuscarNomeLoteria")]
        public string BuscarNomeLoteria(int idAposta)
        {
            return dao.BuscarNomeLoteria(idAposta);
        }

        [HttpPost]
        [Route("BuscarResultadoPorUltimoConcurso")]
        public ResultadoLoteriaModel BuscarResultadoPorUltimoConcurso(int? idAposta, string loteria)
        {
            return dao.BuscarResultadoPorUltimoConcurso(idAposta, loteria);
        }
    }
}
