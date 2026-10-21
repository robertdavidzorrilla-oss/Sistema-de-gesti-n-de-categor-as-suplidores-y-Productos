using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio
{

    /// <summary>
    /// Valida y gestiona las operaciones relacionadas con los productos.
    /// <summary>
    public class ProductoService
    {
        private readonly ProductoRepositorio _productoRepo = new ProductoRepositorio();

        public List<Producto> ListarActivos()
        {
            try
            {
                return _productoRepo.ObtenerTodos();
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("ProductoService.ListarActivos", ex);
                throw;
            }
        }

        public List<Producto> ListarPapelera()
        {
            try
            {
                return _productoRepo.ObtenerPapelera();
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("ProductoService.ListarPapelera", ex);
                throw;
            }
        }

        public ResultadoOperacion<bool> Guardar(Producto producto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(producto.Nombre) || !ValidadorRegex.EsTextoValido(producto.Nombre))
                {
                    return new ResultadoOperacion<bool>(false, "El nombre del producto no es válido.");
                }

                if (producto.Precio <= 0)
                {
                    return new ResultadoOperacion<bool>(false, "El precio del producto debe ser mayor a 0.");
                }

                if (producto.IdCategoria <= 0)
                {
                    return new ResultadoOperacion<bool>(false, "Debe seleccionar una categoría válida.");
                }

                if (producto.IdSuplidor <= 0)
                {
                    return new ResultadoOperacion<bool>(false, "Debe seleccionar un suplidor válido.");
                }

                bool exito;
                if (producto.IdProducto == 0)
                {
                    exito = _productoRepo.Insertar(producto);
                }
                else
                {
                    exito = _productoRepo.Actualizar(producto);
                }

                return new ResultadoOperacion<bool>(exito, exito ? "Producto guardado correctamente." : "No se pudo guardar el producto.");
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("ProductoService.Guardar", ex);
                return new ResultadoOperacion<bool>(false, "Error interno al guardar el producto: " + ex.Message);
            }
        }

        public ResultadoOperacion<bool> Eliminar(int idProducto)
        {
            try
            {
                bool exito = _productoRepo.Eliminar(idProducto);
                return new ResultadoOperacion<bool>(exito, exito ? "Producto enviado a la papelera." : "No se pudo eliminar el producto.");
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("ProductoService.Eliminar", ex);
                return new ResultadoOperacion<bool>(false, ex.Message);
            }
        }

        public ResultadoOperacion<bool> Restaurar(int idProducto)
        {
            try
            {
                bool exito = _productoRepo.Restaurar(idProducto);
                return new ResultadoOperacion<bool>(exito, exito?  "Producto restaurado con éxito." : "No se pudo restaurar el producto.");
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("ProductoService.Restaurar", ex);
                return new ResultadoOperacion<bool>(false, ex.Message);
            }
        }


    }
}
