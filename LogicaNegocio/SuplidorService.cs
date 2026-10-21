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
    /// Valida y gestiona las operaciones relacionadas con los suplidores.
    /// <summary>
    public class SuplidorService
    {
        private readonly SuplidorRepositorio _suplidorRepo = new SuplidorRepositorio();
        private readonly ProductoRepositorio _productoRepo = new ProductoRepositorio();

        public List<Suplidor> ListarActivos()
        {
            try
            {
                return _suplidorRepo.ObtenerTodos();
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("SuplidorService.ListarActivos", ex);
                throw;
            }
        }

        public List<Suplidor> ListarPapelera()
        {
            try
            {
                return _suplidorRepo.ObtenerPapelera();
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("SuplidorService.ListarPapelera", ex);
                throw;
            }
        }

        public ResultadoOperacion<bool> Guardar(Suplidor suplidor)
        {
            try
            {
                // Validaciones con Regex
                if (string.IsNullOrWhiteSpace(suplidor.NombreEmpresa) || !ValidadorRegex.EsTextoValido(suplidor.NombreEmpresa))
                {
                    return new ResultadoOperacion<bool>(false, "El nombre de la empresa no es válido.");
                }

                if (string.IsNullOrWhiteSpace(suplidor.NombreContacto) || !ValidadorRegex.EsTextoValido(suplidor.NombreContacto))
                {
                    return new ResultadoOperacion<bool>(false, "El nombre del contacto no es válido.");
                }

                if (!ValidadorRegex.EsTelefonoRDValido(suplidor.Telefono))
                {
                    return new ResultadoOperacion<bool>(false, "El teléfono debe ser un número válido en R.D. (809, 829, 849).");
                }

                if (!ValidadorRegex.EsEmailValido(suplidor.Email))
                {
                    return new ResultadoOperacion<bool>(false, "El correo electrónico introducido no tiene un formato válido.");
                }

                bool exito;
                if (suplidor.IdSuplidor == 0)
                {
                    exito = _suplidorRepo.Insertar(suplidor);
                }
                else
                {
                    exito = _suplidorRepo.Actualizar(suplidor);
                }

                return new ResultadoOperacion<bool>(exito, exito ? "Suplidor guardado exitosamente." : "No se pudo guardar el suplidor.");
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("SuplidorService.Guardar", ex);
                return new ResultadoOperacion<bool>(false, "Error al guardar el suplidor: " + ex.Message);
            }
        }

        public ResultadoOperacion<bool> Eliminar(int idSuplidor)
        {
            try
            {
                // Regla de Negocio: No eliminar suplidores con productos activos
                var productos = _productoRepo.ObtenerTodos();
                bool tieneProductos = productos.Exists(p => p.IdSuplidor == idSuplidor);

                if (tieneProductos)
                {
                    return new ResultadoOperacion<bool>(false, "No se puede eliminar el suplidor porque tiene productos activos asociados.");
                }

                bool exito = _suplidorRepo.Eliminar(idSuplidor);
                return new ResultadoOperacion<bool>(exito, exito ? "Suplidor enviado a la papelera." : "No se pudo eliminar el suplidor.");
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("SuplidorService.Eliminar", ex);
                return new ResultadoOperacion<bool>(false, ex.Message);
            }
        }

        public ResultadoOperacion<bool> Restaurar(int idSuplidor)
        {
            try
            {
                bool exito = _suplidorRepo.Restaurar(idSuplidor);
                return new ResultadoOperacion<bool>(exito, exito ? "Suplidor restaurado con éxito." : "No se pudo restaurar el suplidor.");
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("SuplidorService.Restaurar", ex);
                return new ResultadoOperacion<bool>(false, ex.Message);
            }
        }

    }
}
