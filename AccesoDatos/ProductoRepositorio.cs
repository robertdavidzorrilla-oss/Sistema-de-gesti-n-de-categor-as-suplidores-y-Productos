using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class ProductoRepositorio : IRepositorio<Producto>
    {
        public List<Producto> ObtenerTodos()
        {
            var lista = new List<Producto>();
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand(@"SELECT * FROM vw_Productos WHERE estado = 1", conexion))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearProductoConRelaciones(dr));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar los productos activos: " + ex.Message, ex);
            }
            return lista;
        }

        public List<Producto> ObtenerPapelera()
        {
            var lista = new List<Producto>();
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand(@"SELECT * FROM vw_Productos WHERE estado = 0", conexion))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearProductoConRelaciones(dr));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la papelera de productos: " + ex.Message, ex);
            }
            return lista;
        }

        public Producto ObtenerPorId(int id)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand(@"SELECT * FROM vw_Productos WHERE idProducto = @id", conexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return MapearProductoConRelaciones(dr);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar el producto por ID: " + ex.Message, ex);
            }
            return null;
        }

        public bool Insertar(Producto entidad)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("sp_Producto_Crear", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", entidad.Nombre);
                    cmd.Parameters.AddWithValue("@precio", entidad.Precio);
                    cmd.Parameters.AddWithValue("@idCategoria", entidad.IdCategoria);
                    cmd.Parameters.AddWithValue("@idSuplidor", entidad.IdSuplidor);

                    return cmd.ExecuteNonQuery() != 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar el producto: " + ex.Message, ex);
            }
        }

        public bool Actualizar(Producto entidad)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("sp_Producto_Editar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProducto", entidad.IdProducto);
                    cmd.Parameters.AddWithValue("@nombre", entidad.Nombre);
                    cmd.Parameters.AddWithValue("@precio", entidad.Precio);
                    cmd.Parameters.AddWithValue("@idCategoria", entidad.IdCategoria);
                    cmd.Parameters.AddWithValue("@idSuplidor", entidad.IdSuplidor);

                    return cmd.ExecuteNonQuery() != 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al actualizar el producto: " + ex.Message, ex);
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("sp_Producto_Eliminar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProducto", id);

                    return cmd.ExecuteNonQuery() !=0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar el producto: " + ex.Message, ex);
            }
        }

        public bool Restaurar(int id)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("sp_Producto_Restaurar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProducto", id);

                    return cmd.ExecuteNonQuery() != 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al restaurar el producto: " + ex.Message, ex);
            }
        }

        private Producto MapearProductoConRelaciones(SqlDataReader dr)
        {
            return new Producto
            {
                IdProducto = Convert.ToInt32(dr["idProducto"]),
                Nombre = dr["nombre"].ToString(),
                Precio = Convert.ToDecimal(dr["precio"]),
                IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                CategoriaNavegacion = new Categoria { IdCategoria = Convert.ToInt32(dr["idCategoria"]), Nombre = dr["CategoriaNombre"].ToString() },
                IdSuplidor = Convert.ToInt32(dr["idSuplidor"]),
                SuplidorNavegacion = new Suplidor { IdSuplidor = Convert.ToInt32(dr["idSuplidor"]), NombreEmpresa = dr["SuplidorNombre"].ToString() },
                FechaCreacion = Convert.ToDateTime(dr["fechaCreacion"]),
                FechaModificacion = dr["fechaModificacion"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["fechaModificacion"]) : null,
                Estado = Convert.ToBoolean(dr["estado"])
            };
        }
    }
}
