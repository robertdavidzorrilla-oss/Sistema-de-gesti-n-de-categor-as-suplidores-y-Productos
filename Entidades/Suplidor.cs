using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Representa la entidad Suplidor con la información de contacto corporativo.
    /// </summary>
    public class Suplidor
    {
        /// <summary>
        /// Obtiene o establece el identificador único del suplidor.
        /// </summary>
        public int IdSuplidor { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre único de la empresa suplidora.
        /// </summary>
        public string NombreEmpresa { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la persona de contacto.
        /// </summary>
        public string NombreContacto { get; set; }

        /// <summary>
        /// Obtiene o establece el teléfono de contacto.
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Obtiene o establece el correo electrónico principal.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de registro del suplidor.
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Obtiene o establece la última fecha de modificación.
        /// </summary>
        public DateTime? FechaModificacion { get; set; }

        /// <summary>
        /// Obtiene o establece el estado lógico del registro.
        /// </summary>
        public bool Estado { get; set; }
    }
}
