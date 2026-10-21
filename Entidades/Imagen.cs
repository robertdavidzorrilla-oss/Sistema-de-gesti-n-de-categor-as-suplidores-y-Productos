using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Representa la entidad de una imagen asociada a categorías en el sistema.
    /// </summary>
    public class Imagen
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la imagen.
        /// </summary>
        public int IdImagen { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de creación del registro.
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de la última modificación del registro.
        /// </summary>
        public DateTime? FechaModificacion { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del registro (true = Activo, false = Inactivo/Eliminado).
        /// </summary>
        public bool Estado { get; set; }
    }
}
