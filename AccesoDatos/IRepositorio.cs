using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    /// <summary>
    /// Interfaz genérica para el patrón Repository con operaciones CRUD y soft-delete.
    /// </summary>
    /// <typeparam name="T">Clase de entidad del dominio.</typeparam>
    public interface IRepositorio<T> where T : class
    {
        /// <summary>
        /// Obtiene todos los registros activos (estado = 1).
        /// </summary>
        List<T> ObtenerTodos();

        /// <summary>
        /// Obtiene los registros eliminados lógicamente (estado = 0) para la Papelera.
        /// </summary>
        List<T> ObtenerPapelera();

        /// <summary>
        /// Obtiene un registro por su identificador único.
        /// </summary>
        T ObtenerPorId(int id);

        /// <summary>
        /// Inserta un nuevo registro en la base de datos.
        /// </summary>
        bool Insertar(T entidad);

        /// <summary>
        /// Actualiza un registro existente.
        /// </summary>
        bool Actualizar(T entidad);

        /// <summary>
        /// Elimina lógicamente un registro (cambia estado a 0).
        /// </summary>
        bool Eliminar(int id);

        /// <summary>
        /// Restaura un registro eliminado lógicamente (cambia estado a 1).
        /// </summary>
        bool Restaurar(int id);
    }
}
