using CrudsMercadoCampesino.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CrudsMercadoCampesino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly string cadenaSQL;
        public ClienteController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSql");
        }
        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Cliente> lista = new List<Cliente>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))

                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_Cliente", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Cliente
                            {
                                idCliente = Convert.ToInt32(rd["id_cliente"]),
                                nombre = rd["NOMBRE"].ToString(),
                                apellido = rd["APELLIDO"].ToString(),
                                telefono = rd["TELEFONO"].ToString(),
                                correo = rd["CORREO"].ToString(),
                                contraseña = rd["CONTRASENA"].ToString(),
                                direccion = rd["DIRECCION"].ToString()
                                
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
        [Route("obtener/{idCliente:int}")]
        public IActionResult Obtener(int idCliente)
        {
            List<Cliente> lista = new List<Cliente>();
            Cliente cliente = new Cliente();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_LISTAR_CLIENTE", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Cliente
                            {
                                idCliente = Convert.ToInt32(rd["ID_CLIENTE"]),
                                nombre = rd["NOMBRE"].ToString(),
                                apellido = rd["APELLIDO"].ToString(),
                                telefono = rd["UBICACION"].ToString(),
                                correo = rd["CORREO"].ToString(),
                                contraseña = rd["CONTRASENA"].ToString(),
                                direccion = rd["DIRECCION"].ToString()
                            });
                        }
                    }
                }
                cliente = lista.Where(item => item.idCliente == idCliente).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = cliente });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = cliente });
            }
        }
        [HttpPut]
        [Route("Editar")]

        public IActionResult Editar([FromBody] Cliente objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_EDITAR_CLIENTE", conexion);
                    cmd.Parameters.AddWithValue("ID_CLIENTE", objeto.idCliente == 0 ? DBNull.Value : objeto.idCliente);
                    cmd.Parameters.AddWithValue("NOMBRE", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("APELLIDO", objeto.apellido is null ? DBNull.Value : objeto.apellido);
                    cmd.Parameters.AddWithValue("TELEFONO", objeto.telefono is null ? DBNull.Value : objeto.telefono);
                    cmd.Parameters.AddWithValue("CORREO", objeto.correo is null ? DBNull.Value : objeto.correo);
                    cmd.Parameters.AddWithValue("CONTRASENA", objeto.contraseña is null ? DBNull.Value : objeto.contraseña);
                    cmd.Parameters.AddWithValue("DIRECCION", objeto.direccion is null ? DBNull.Value : objeto.direccion);
                }
                return StatusCode(StatusCodes.Status200OK, new { mensage = "editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpDelete]
        [Route("Eliminar/{id_Cliente:int}")]
        public IActionResult Eliminar(int id_Cliente)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_ELIMINAR_CLIENTE", conexion);
                    cmd.Parameters.AddWithValue("ID_CLIENTE", id_Cliente);
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
