using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using Entidades;

namespace LogicaNegocio
{
    #region DTOs para Proyecciones LINQ
    public class SuplidorResumenDTO
    {
        public int IdSuplidor { get; set; }
        public string NombreEmpresa { get; set; }
        public int TotalProductos { get; set; }
        public decimal PrecioPromedioProducto { get; set; }
    }

    public class CategoriaValorDTO
    {
        public int IdCategoria { get; set; }
        public string CategoriaNombre { get; set; }
        public int CantidadProductos { get; set; }
        public decimal ValorTotalInventario { get; set; }
    }

    public class EstadisticaCategoriaDTO
    {
        public string CategoriaNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioMinimo { get; set; }
        public decimal PrecioMaximo { get; set; }
        public decimal PrecioPromedio { get; set; }
    }

    public class ResumenDashboardDTO
    {
        public int TotalProductosActivos { get; set; }
        public decimal PrecioPromedioGeneral { get; set; }
        public string CategoriaMasPoblada { get; set; }
        public int TotalSuplidoresConProductos { get; set; }
    }
    #endregion

    /// <summary>
    /// Servicio que ejecuta consultas LINQ sobre el modelo de datos.
    /// </summary>
    public class ConsultasLinqService
    {
        private readonly ProductoRepositorio _productoRepo = new ProductoRepositorio();
        private readonly CategoriaRepositorio _categoriaRepo = new CategoriaRepositorio();
        private readonly SuplidorRepositorio _suplidorRepo = new SuplidorRepositorio();

        /// <summary>
        /// Agrupa productos por suplidor, calcula el total de artículos y precio promedio,
        /// ordenando descendentemente por cantidad de productos.
        /// </summary>
        public List<SuplidorResumenDTO> ObtenerTopSuplidoresConMasProductos()
        {
            var productos = _productoRepo.ObtenerTodos();

            return productos
                .GroupBy(p => new { p.IdSuplidor, p.SuplidorNavegacion.NombreEmpresa })
                .Select(g => new SuplidorResumenDTO
                {
                    IdSuplidor = g.Key.IdSuplidor,
                    NombreEmpresa = g.Key.NombreEmpresa,
                    TotalProductos = g.Count(),
                    PrecioPromedioProducto = Math.Round(g.Average(p => p.Precio), 2)
                })
                .OrderByDescending(dto => dto.TotalProductos)
                .ThenBy(dto => dto.NombreEmpresa)
                .ToList();
        }

        /// <summary>
        /// Agrupa productos por categoría y filtra únicamente aquellas cuyo
        /// valor total en inventario supera el promedio general de todas las categorías.
        /// </summary>
        public List<CategoriaValorDTO> ObtenerCategoriasConValorTotalAlto()
        {
            var productos = _productoRepo.ObtenerTodos();

            var agrupados = productos
                .GroupBy(p => new { p.IdCategoria, p.CategoriaNavegacion.Nombre })
                .Select(g => new CategoriaValorDTO
                {
                    IdCategoria = g.Key.IdCategoria,
                    CategoriaNombre = g.Key.Nombre,
                    CantidadProductos = g.Count(),
                    ValorTotalInventario = g.Sum(p => p.Precio)
                })
                .ToList();

            if (!agrupados.Any()) return new List<CategoriaValorDTO>();

            decimal promedioGeneral = agrupados.Average(c => c.ValorTotalInventario);

            return agrupados
                .Where(c => c.ValorTotalInventario >= promedioGeneral)
                .OrderByDescending(c => c.ValorTotalInventario)
                .ToList();
        }

        /// <summary>
        /// Proyecta métricas estadísticas de precios (mínimo, máximo, promedio)
        /// uniendo físicamente las categorías activas con sus productos activos mediante un Join explícito.
        /// </summary>
        public List<EstadisticaCategoriaDTO> ObtenerEstadisticasPreciosPorCategoria()
        {
            var categorias = _categoriaRepo.ObtenerTodos();
            var productos = _productoRepo.ObtenerTodos();

            return categorias
                .Join(productos,
                      c => c.IdCategoria,
                      p => p.IdCategoria,
                      (c, p) => new { Categoria = c.Nombre, Producto = p })
                .GroupBy(x => x.Categoria)
                .Select(g => new EstadisticaCategoriaDTO
                {
                    CategoriaNombre = g.Key,
                    Cantidad = g.Count(),
                    PrecioMinimo = g.Min(x => x.Producto.Precio),
                    PrecioMaximo = g.Max(x => x.Producto.Precio),
                    PrecioPromedio = Math.Round(g.Average(x => x.Producto.Precio), 2)
                })
                .OrderBy(e => e.CategoriaNombre)
                .ToList();
        }

        /// <summary>
        /// Búsqueda dinámica multicriterio con filtrado condicional por rango de precio
        /// y coincidencias parciales en el nombre de la categoría o suplidor.
        /// </summary>
        public List<Producto> BuscarProductosAvanzado(string terminoBusqueda, decimal? precioMin, decimal? precioMax)
        {
            var productos = _productoRepo.ObtenerTodos().AsQueryable();

            if (!string.IsNullOrWhiteSpace(terminoBusqueda))
            {
                string termino = terminoBusqueda.Trim().ToLower();
                productos = productos.Where(p =>
                    p.Nombre.ToLower().Contains(termino) ||
                    p.CategoriaNavegacion.Nombre.ToLower().Contains(termino) ||
                    p.SuplidorNavegacion.NombreEmpresa.ToLower().Contains(termino)
                );
            }

            if (precioMin.HasValue)
            {
                productos = productos.Where(p => p.Precio >= precioMin.Value);
            }

            if (precioMax.HasValue)
            {
                productos = productos.Where(p => p.Precio <= precioMax.Value);
            }

            return productos
                .OrderBy(p => p.CategoriaNavegacion.Nombre)
                .ThenBy(p => p.Precio)
                .ToList();
        }

        /// <summary>
        /// Detecta suplidores activos que actualmente no tienen ningún producto activo asociado
        /// utilizando un GroupJoin (LEFT JOIN) y filtrado de colecciones vacías.
        /// </summary>
        public List<Suplidor> ObtenerSuplidoresSinProductosActivos()
        {
            var suplidores = _suplidorRepo.ObtenerTodos();
            var productos = _productoRepo.ObtenerTodos();

            return suplidores
                .GroupJoin(
                    productos,
                    s => s.IdSuplidor,
                    p => p.IdSuplidor,
                    (suplidor, prods) => new { Suplidor = suplidor, Productos = prods }
                )
                .Where(x => !x.Productos.Any())
                .Select(x => x.Suplidor)
                .OrderBy(s => s.NombreEmpresa)
                .ToList();
        }

        /// <summary>
        /// Agregación compleja general para alimentar tarjetas de resumen en el Dashboard.
        /// </summary>
        public ResumenDashboardDTO ObtenerResumenDashboard()
        {
            var productos = _productoRepo.ObtenerTodos();

            if (!productos.Any())
            {
                return new ResumenDashboardDTO
                {
                    TotalProductosActivos = 0,
                    PrecioPromedioGeneral = 0,
                    CategoriaMasPoblada = "N/A",
                    TotalSuplidoresConProductos = 0
                };
            }

            int totalProductos = productos.Count();
            decimal promedioGeneral = Math.Round(productos.Average(p => p.Precio), 2);

            string categoriaMasPoblada = productos
                .GroupBy(p => p.CategoriaNavegacion.Nombre)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "N/A";

            int suplidoresActivosConProd = productos
                .Select(p => p.IdSuplidor)
                .Distinct()
                .Count();

            return new ResumenDashboardDTO
            {
                TotalProductosActivos = totalProductos,
                PrecioPromedioGeneral = promedioGeneral,
                CategoriaMasPoblada = categoriaMasPoblada,
                TotalSuplidoresConProductos = suplidoresActivosConProd
            };
        }
    }
}
