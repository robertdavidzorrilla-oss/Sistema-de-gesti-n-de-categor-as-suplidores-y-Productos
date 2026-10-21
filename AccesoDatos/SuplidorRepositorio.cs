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
    public class SuplidorRepositorio : IRepositorio<Suplidor>
    {
        public List<Suplidor> ObtenerTodos()
        {
            var lista = new List<Suplidor>();
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("SELECT * FROM vw_Suplidores WHERE estado = 1", conexion))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearSuplidor(dr));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar los suplidores activos: " + ex.Message, ex);
            }
            return lista;
        }

        public List<Suplidor> ObtenerPapelera()
        {
            var lista = new List<Suplidor>();
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("SELECT * FROM vw_Suplidores WHERE estado = 0", conexion))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearSuplidor(dr));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar la papelera de suplidores: " + ex.Message, ex);
            }
            return lista;
        }

        public Suplidor ObtenerPorId(int id)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("SELECT * FROM vw_Suplidores WHERE idSuplidor = @id", conexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return MapearSuplidor(dr);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar el suplidor por ID: " + ex.Message, ex);
            }
            return null;
        }

        public bool Insertar(Suplidor entidad)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("sp_Suplidor_Crear", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombreEmpresa", entidad.NombreEmpresa);
                    cmd.Parameters.AddWithValue("@nombreContacto", entidad.NombreContacto);
                    cmd.Parameters.AddWithValue("@telefono", entidad.Telefono);
                    cmd.Parameters.AddWithValue("@email", entidad.Email);

                    return cmd.ExecuteNonQuery() != 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al crear el suplidor: " + ex.Message, ex);
            }
        }

        public bool Actualizar(Suplidor entidad)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("sp_Suplidor_Editar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idSuplidor", entidad.IdSuplidor);
                    cmd.Parameters.AddWithValue("@nombreEmpresa", entidad.NombreEmpresa);
                    cmd.Parameters.AddWithValue("@nombreContacto", entidad.NombreContacto);
                    cmd.Parameters.AddWithValue("@telefono", entidad.Telefono);
                    cmd.Parameters.AddWithValue("@email", entidad.Email);

                    return cmd.ExecuteNonQuery() != 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al editar el suplidor: " + ex.Message, ex);
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                using (var conexion = Conexion.Instancia.ObtenerConexion())
                using (var cmd = new SqlCommand("sp_Suplidor_Eliminar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idSuplidor", id);

                    return cmd.ExecuteNonQuery() != 0;
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
                using (var cmd = new SqlCommand("sp_Suplidor_Restaurar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idSuplidor", id);

                    return cmd.ExecuteNonQuery() != 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al restaurar el suplidor: " + ex.Message, ex);
            }
        }

        private Suplidor MapearSuplidor(SqlDataReader dr)
        {
            return new Suplidor
            {
                IdSuplidor = Convert.ToInt32(dr["idSuplidor"]),
                NombreEmpresa = dr["nombreEmpresa"].ToString(),
                NombreContacto = dr["nombreContacto"].ToString(),
                Telefono = dr["telefono"].ToString(),
                Email = dr["email"].ToString(),
                FechaCreacion = Convert.ToDateTime(dr["fechaCreacion"]),
                FechaModificacion = dr["fechaModificacion"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["fechaModificacion"]) : null,
                Estado = Convert.ToBoolean(dr["estado"])
            };
        }
    }
}
