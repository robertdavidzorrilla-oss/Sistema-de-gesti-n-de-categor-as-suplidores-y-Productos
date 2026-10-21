using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Representa la entidad Categoría para clasificar productos.
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la categoría.
        /// </summary>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la categoría.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de la categoría.
        /// </summary>
        public string Descirpcion { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador opcional de la imagen asociada.
        /// </summary>
        public int? IdImagen { get; set; }

        /// <summary>
        /// Obtiene o establece la entidad de la imagen asociada (para navegación/relación).
        /// </summary>
        public Imagen ImagenNavegacion { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de creación de la categoría.
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
