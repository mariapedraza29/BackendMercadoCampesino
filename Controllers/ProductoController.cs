
using CrudsMercadoCampesino.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CrudsMercadoCampesino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly string cadenaSQL;
        public ProductoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSql");
        }
        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))

                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_LISTAR_PRODUCTO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto
                            {
                                idProducto = Convert.ToInt32(rd["ID_PRODUCTO"]),
                                nombre = rd["NOMBRE"].ToString(),
                                existencia = rd["EXISTENCIA"].ToString(),
                                precio = Convert.ToInt32(rd["PRECIO"])
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
        [Route("obtener/{idPersona:int}")]
        public IActionResult Obtener(int idPersona)
        {
            List<Producto> lista = new List<Producto>();
            Producto producto = new Producto();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_LISTAR_PRODUCTO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto
                            {
                                idProducto = Convert.ToInt32(rd["ID_PRODUCTO"]),
                                nombre = rd["nombre"].ToString(),
                                existencia = rd["apellido"].ToString(),
                                precio = Convert.ToInt32(rd["PRECIO"])
                            });
                        }
                    }
                }
                producto = lista.Where(item => item.idProducto == idPersona).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = producto });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = producto });
            }
        }
        [HttpPost]
        [Route("Registrar")]
        public IActionResult Registrar([FromBody] Producto objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_AGREGAR_PRODUCTO", conexion);
                    cmd.Parameters.AddWithValue("ID_PRODUCTO", objeto.idProducto);
                    cmd.Parameters.AddWithValue("NOMBRE", objeto.nombre);
                    cmd.Parameters.AddWithValue("EXISTENCIA", objeto.existencia);
                    cmd.Parameters.AddWithValue("PRECIO", objeto.precio);
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

        public IActionResult Editar([FromBody] Producto objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_EDITAR_PRODUCTO", conexion);
                    cmd.Parameters.AddWithValue("ID_PRODUCTO", objeto.idProducto == 0 ? DBNull.Value : objeto.idProducto);
                    cmd.Parameters.AddWithValue("NOMBRE", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("EXISTENCIA", objeto.existencia is null ? DBNull.Value : objeto.existencia);
                    cmd.Parameters.AddWithValue("PRECIO", objeto.precio == 0 ? DBNull.Value : objeto.precio);
                }
                return StatusCode(StatusCodes.Status200OK, new { mensage = "editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpDelete]
        [Route("Eliminar/{ID_PRODUCTO:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_ELIMINAR_PRODUCTO", conexion);
                    cmd.Parameters.AddWithValue("ID_PRODUCTO", idProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Producto eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
