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
    /// Gestiona la conexión a la base de datos SQL Server mediante el patrón de diseño Singleton.
    /// </summary>
    public class Conexion
    {
        private static Conexion _instancia = null;
        private static readonly object _bloqueo = new object();
        private readonly string _cadenaConexion;

        /// <summary>
        /// Constructor privado para evitar la instanciación directa externa (Patrón Singleton).
        /// </summary>
        private Conexion()
        {
            _cadenaConexion = "Server=LENOVORD25\\ROBERT;Database=SistemaGestion;Integrated Security=True;TrustServerCertificate=True;";
        }

        /// <summary>
        /// Obtiene la instancia única global de la clase Conexion (Thread-Safe).
        /// </summary>
        public static Conexion Instancia
        {
            get
            {
                lock (_bloqueo)
                {
                    if (_instancia == null)
                    {
                        _instancia = new Conexion();
                    }
                    return _instancia;
                }
            }
        }

        /// <summary>
        /// Crea y retorna una nueva conexión abierta a la base de datos.
        /// </summary>
        /// <returns>Objeto SqlConnection listo para operar</returns>
        public SqlConnection ObtenerConexion()
        {
            SqlConnection conexion = new SqlConnection(_cadenaConexion);
            if (conexion.State == ConnectionState.Closed)
            {
                conexion.Open();
            }
            return conexion;
        }
    }
}
