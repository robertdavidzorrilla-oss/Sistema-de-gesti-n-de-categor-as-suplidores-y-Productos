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
    /// Validaciones regulares y capturas de excepciones para la entidad Categoria
    /// <summary>
    public class CategoriaService
    {
        private readonly CategoriaRepositorio _categoriaRepo = new CategoriaRepositorio();
        private readonly ProductoRepositorio _productoRepo = new ProductoRepositorio();


        public List<Categoria> ListarActivas()
        {
            try
            {
                return _categoriaRepo.ObtenerTodos();
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("CategoriaService.ListarActivas", ex);
                throw;
            }
        }

        public List<Categoria> ListarPapelera()
        {
            try
            {
                return _categoriaRepo.ObtenerPapelera();
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("CategoriaService.ListarPapelera", ex);
                throw;
            }
        }

        public ResultadoOperacion<bool> Guardar(Categoria categoria)
        {
            try
            {
                // Validation 1: Nombre obligatorio y Regex
                if (string.IsNullOrWhiteSpace(categoria.Nombre) || categoria.Nombre.Trim().Length < 3)
                {
                    return new ResultadoOperacion<bool>(false, "El nombre de la categoría debe tener al menos 3 caracteres.");
                }

                if (!ValidadorRegex.EsTextoValido(categoria.Nombre))
                {
                    return new ResultadoOperacion<bool>(false, "El nombre de la categoría contiene caracteres no válidos.");
                }

                // Validation 2: Descripción obligatoria
                if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                {
                    return new ResultadoOperacion<bool>(false, "La descripción de la categoría es obligatoria.");
                }

                bool exito;
                if (categoria.IdCategoria == 0)
                {
                    exito = _categoriaRepo.Insertar(categoria);
                }
                else
                {
                    exito = _categoriaRepo.Actualizar(categoria);
                }

                return new ResultadoOperacion<bool>(exito, exito ? "Categoría guardada correctamente." : "No se pudo guardar la categoría.");
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("CategoriaService.Guardar", ex);
                return new ResultadoOperacion<bool>(false, "Error interno al guardar la categoría: " + ex.Message);
            }
        }

        public ResultadoOperacion<bool> Eliminar(int idCategoria)
        {
            try
            {
                // Regla de Negocio: No eliminar categorías con productos activos
                var productos = _productoRepo.ObtenerTodos();
                bool tieneProductosActivos = productos.Exists(p => p.IdCategoria == idCategoria);

                if (tieneProductosActivos)
                {
                    return new ResultadoOperacion<bool>(false, "No se puede eliminar la categoría porque tiene productos activos asociados.");
                }

                bool exito = _categoriaRepo.Eliminar(idCategoria);
                return new ResultadoOperacion<bool>(exito, exito ? "Categoría enviada a la papelera." : "No se pudo eliminar la categoría.");
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("CategoriaService.Eliminar", ex);
                return new ResultadoOperacion<bool>(false, ex.Message);
            }
        }

        public ResultadoOperacion<bool> Restaurar(int idCategoria)
        {
            try
            {
                bool exito = _categoriaRepo.Restaurar(idCategoria);
                return new ResultadoOperacion<bool>(exito, exito ? "Categoría restaurada con éxito." : "No se pudo restaurar la categoría.");
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("CategoriaService.Restaurar", ex);
                return new ResultadoOperacion<bool>(false, ex.Message);
            }
        }

        public ResultadoOperacion<bool> GuardarConImagen(Categoria categoria, string rutaImagenLocal)
        {
            try
            {
                
                if (!string.IsNullOrWhiteSpace(rutaImagenLocal))
                {
                    ImagenService imagenServicio = new ImagenService();
                    ResultadoOperacion<Imagen> resultadoImagen = imagenServicio.ProcesarYGuardarImagen(rutaImagenLocal);

                    if (!resultadoImagen.Exito)
                    {
                        return new ResultadoOperacion<bool>(false, resultadoImagen.Mensaje);
                    }
                }

                
                return Guardar(categoria);
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("CategoriaService.GuardarConImagen", ex);
                return new ResultadoOperacion<bool>(false, ex.Message);
            }
        }

    }
}
