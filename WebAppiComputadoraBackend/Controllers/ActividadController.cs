using Microsoft.AspNetCore.Mvc;
using WebApiTalleresBackend.Models;
using WebApiTalleresBackend.Services.Interfaces;

namespace WebApiTalleresBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadController : ControllerBase
    {
        private readonly IActividadService _actividadService;

        public ActividadController(IActividadService actividadService)
        {
            _actividadService = actividadService;
        }

        // GET: api/Actividad
        [HttpGet]
        public ActionResult<List<Actividad>> ObtenerTodas()
        {
            var actividades = _actividadService.ObtenerTodas();

            return Ok(actividades);
        }

        // GET: api/Actividad/5
        [HttpGet("{id}")]
        public ActionResult<Actividad> ObtenerPorId(int id)
        {
            var actividad = _actividadService.ObtenerPorId(id);

            if (actividad == null)
            {
                return NotFound(
                    new { mensaje = "Actividad no encontrada." }
                );
            }

            return Ok(actividad);
        }

        // POST: api/Actividad
        [HttpPost]
        public ActionResult Crear(Actividad actividad)
        {
            _actividadService.Crear(actividad);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = actividad.IdActividad },
                actividad
            );
        }

        // PUT: api/Actividad/5
        [HttpPut("{id}")]
        public ActionResult Modificar(
            int id,
            Actividad actividad
        )
        {
            actividad.IdActividad = id;

            bool modificada =
                _actividadService.Modificar(actividad);

            if (!modificada)
            {
                return NotFound(
                    new { mensaje = "Actividad no encontrada." }
                );
            }

            return Ok(
                new { mensaje = "Actividad modificada correctamente." }
            );
        }

        // DELETE: api/Actividad/5
        [HttpDelete("{id}")]
        public ActionResult Eliminar(int id)
        {
            bool eliminada = _actividadService.Eliminar(id);

            if (!eliminada)
            {
                return NotFound(
                    new { mensaje = "Actividad no encontrada." }
                );
            }

            return Ok(
                new { mensaje = "Actividad eliminada correctamente." }
            );
        }
    }
}