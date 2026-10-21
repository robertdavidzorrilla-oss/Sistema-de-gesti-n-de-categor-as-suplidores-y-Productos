using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    /// <summary>
    /// Encapsula la respuesta genérica de una operación de base de datos o lógica de negocio.
    /// </summary>
    /// <typeparam name="T">Tipo de dato o entidad retornado.</typeparam>
    public class ResultadoOperacion<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool Exito { get; set; }

        /// <summary>
        /// Mensaje informativo o de error.
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Datos devueltos por la operación.
        /// </summary>

        public T Datos { get; set; }

        /// <summary>
        /// Constructor para inicializar un resultado.
        /// </summary>
        public ResultadoOperacion(bool exito = true, string mensaje = "", T datos = default)
        {
            Exito = exito;
            Mensaje = mensaje;
            Datos = datos;
        }
    }
}
