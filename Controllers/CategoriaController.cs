using CrudsMercadoCampesino.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CrudsMercadoCampesino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly string cadenaSQL;
        public CategoriaController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSql");
        }
        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Categoria> lista = new List<Categoria>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))

                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_LISTAR_CATEOGORIA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Categoria
                            {
                                idCategoria = Convert.ToInt32(rd["ID_CATEGORIA"]),
                                nombre = rd["NOMBRE"].ToString(),
                                tipo = rd["TIPO"].ToString()
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
        [Route("obtener/{ID_CATEGORIA:int}")]
        public IActionResult Obtener(int idCategoria)
        {
            List<Categoria> lista = new List<Categoria>();
            Categoria categoria = new Categoria();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_LISTAR_CATEOGORIA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Categoria
                            {
                                idCategoria = Convert.ToInt32(rd["ID_CATEGORIA"]),
                                nombre = rd["NOMBRE"].ToString(),
                                tipo = rd["TIPO"].ToString()
                            });
                        }
                    }
                }
                categoria = lista.Where(item => item.idCategoria == idCategoria).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = categoria });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = categoria });
            }
        }
        [HttpPost]
        [Route("Registrar")]
        public IActionResult Registrar([FromBody] Categoria objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_AGREGAR_CATEGORIA", conexion);
                    cmd.Parameters.AddWithValue("ID_CATEGORIA", objeto.idCategoria);
                    cmd.Parameters.AddWithValue("NOMBRE", objeto.nombre);
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

        public IActionResult Editar([FromBody] Categoria objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_EDITAR_CATEGORIA", conexion);
                    cmd.Parameters.AddWithValue("ID_CATEGORIA", objeto.idCategoria == 0 ? DBNull.Value : objeto.idCategoria);
                    cmd.Parameters.AddWithValue("NOMBRE", objeto.nombre is null ? DBNull.Value : objeto.nombre);
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
        [Route("Eliminar/{ID_CATEGORIA:int}")]
        public IActionResult Eliminar(int idCategoria)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_ELIMINAR_CATEGORIA", conexion);
                    cmd.Parameters.AddWithValue("ID_CATEGORIA", idCategoria);
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
