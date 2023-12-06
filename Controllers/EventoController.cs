using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CrudsMercadoCampesino.Modelos;

namespace CrudsMercadoCampesino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly string cadenaSQL;
        public EventoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSql");
        }
        [HttpGet]
        [Route("ListaEvento")]
        public IActionResult Lista()
        {
            List<Evento> lista = new List<Evento>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))

                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_LISTAR_EVENTOS", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Evento
                            {
                                idEvento = Convert.ToInt32(rd["ID_EVENTO"]),
                                nombre = rd["NOMBRE"].ToString(),
                                descripcion = rd["DESCRIPCION"].ToString(),
                                tipo = rd["TIPO"].ToString(),
                            });
                        }
                    }

                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception error)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message }));
            }
        }
        [HttpGet]
        [Route("obtener/{ID_EVENTO:int}")]
        public IActionResult Obtener(int idEvento)
        {
            List<Evento> lista = new List<Evento>();
            Evento evento = new Evento();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_LISTAR_EVENTOS", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Evento
                            {
                                idEvento = Convert.ToInt32(rd["ID_EVENTO"]),
                                nombre = rd["NOOMBRE"].ToString(),
                                descripcion = rd["DESCRIPCION"].ToString(),
                                tipo = rd["TIPO"].ToString()
                            });
                        }
                    }
                }
                evento = lista.Where(item => item.idEvento == idEvento).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = evento });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = evento });
            }
        }
        [HttpPost]
        [Route("RegistrarEvento")]
        public IActionResult Registrar([FromBody] Evento objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_AGREGAR_EVENTOS", conexion);
                    cmd.Parameters.AddWithValue("ID_EVENTO", objeto.idEvento);
                    cmd.Parameters.AddWithValue("NOMBRE", objeto.nombre);
                    cmd.Parameters.AddWithValue("DESCRIPCION", objeto.descripcion);
                    cmd.Parameters.AddWithValue("TIPO", objeto.tipo);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "registrado" });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("Editar")]

        public IActionResult Editar([FromBody] Evento objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_EDITAR_EVENTOS", conexion);
                    cmd.Parameters.AddWithValue("ID_EVENTO", objeto.idEvento == 0 ? DBNull.Value : objeto.idEvento);
                    cmd.Parameters.AddWithValue("NOMBRE", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("DESCRIPCION", objeto.descripcion is null ? DBNull.Value : objeto.descripcion);
                    cmd.Parameters.AddWithValue("TIPO", objeto.tipo is null ? DBNull.Value : objeto.tipo);
                }
                return StatusCode(StatusCodes.Status200OK, new { mensage = "editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpDelete]
        [Route("Eliminar/{ID_EVENTO:int}")]
        public IActionResult Eliminar(int idEvento)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_ELIMINAR_EVENTOS", conexion);
                    cmd.Parameters.AddWithValue("ID_EVENTO", idEvento);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
