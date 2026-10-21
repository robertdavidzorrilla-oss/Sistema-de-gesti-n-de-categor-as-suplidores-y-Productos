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
    /// <summary>
    /// Implementación del repositorio de datos para la entidad Categoria.
    /// </summary>
    public class CategoriaRepositorio : IRepositorio<Categoria>
    {
        public List<Categoria> ObtenerTodos()
        {
            var lista = new List<Categoria>();
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("SELECT * FROM vw_Categorias WHERE estado = 1", conexion))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearCategoria(dr));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la vista de categorías activas: " + ex.Message, ex);
            }
            return lista;
        }

        public List<Categoria> ObtenerPapelera()
        {
            var lista = new List<Categoria>();
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("SELECT * FROM vw_Categorias WHERE estado = 0", conexion))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearCategoria(dr));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la papelera de categorías desde: " + ex.Message, ex);
            }
            return lista;
        }

        public Categoria ObtenerPorId(int id)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("SELECT * FROM vw_Categorias WHERE idCategoria = @id", conexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return MapearCategoria(dr);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener la categoría por ID desde: " + ex.Message, ex);
            }
            return null;
        }

        public bool Insertar(Categoria entidad)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("sp_Categoria_Crear", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", entidad.Nombre);
                    cmd.Parameters.AddWithValue("@descirpcion", entidad.Descirpcion);
                    cmd.Parameters.AddWithValue("@idImagen", (object)entidad.IdImagen ?? DBNull.Value);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al insertar la categoría: " + ex.Message, ex);
            }
        }

        public bool Actualizar(Categoria entidad)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("sp_Categoria_Editar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCategoria", entidad.IdCategoria);
                    cmd.Parameters.AddWithValue("@nombre", entidad.Nombre);
                    cmd.Parameters.AddWithValue("@descirpcion", entidad.Descirpcion);
                    cmd.Parameters.AddWithValue("@idImagen", (object)entidad.IdImagen ?? DBNull.Value);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al editar la categoría: " + ex.Message, ex);
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("sp_Categoria_Eliminar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCategoria", id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool Restaurar(int id)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("sp_Categoria_Restaurar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCategoria", id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al restaurar la categoría: " + ex.Message, ex);
            }
        }

        private Categoria MapearCategoria(SqlDataReader dr)
        {
            return new Categoria
            {
                IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                Nombre = dr["nombre"].ToString(),
                Descirpcion = dr["descirpcion"].ToString(),
                IdImagen = dr["idImagen"] != DBNull.Value ? (int?)Convert.ToInt32(dr["idImagen"]) : null,
                FechaCreacion = Convert.ToDateTime(dr["fechaCreacion"]),
                FechaModificacion = dr["fechaModificacion"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["fechaModificacion"]) : null,
                Estado = Convert.ToBoolean(dr["estado"])
            };
        }
    }

}
