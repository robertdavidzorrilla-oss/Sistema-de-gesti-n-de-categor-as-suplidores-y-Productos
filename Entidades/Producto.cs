using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Representa la entidad Producto registrada en el inventario.
    /// </summary>
    public class Producto
    {
        /// <summary>
        /// Obtiene o establece el identificador único del producto.
        /// </summary>
        public int IdProducto { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre comercial del producto.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el precio unitario del producto.
        /// </summary>
        public decimal Precio { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la categoría a la que pertenece.
        /// </summary>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Obtiene o establece la entidad de la categoría vinculada.
        /// </summary>
        public Categoria CategoriaNavegacion { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del suplidor asociado.
        /// </summary>
        public int IdSuplidor { get; set; }

        /// <summary>
        /// Obtiene o establece la entidad del suplidor vinculado.
        /// </summary>
        public Suplidor SuplidorNavegacion { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de creación del producto.
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
