using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio
{
    /// <summary>
    /// Proporciona validaciones para los archivos de imagen adjuntos a las categorías.
    /// </summary>
    public static class ValidadorImagen
    {
        private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png" };

        
        private const long TamanoMaximoBytes = 2 * 1024 * 1024;

        /// <summary>
        /// Valida la extensión y el tamaño de un archivo de imagen seleccionado por el usuario.
        /// </summary>
        /// <param name="rutaArchivo">Ruta absoluta del archivo local.</param>
        /// <returns>ResultadoOperacion que indica si el archivo cumple con las restricciones.</returns>
        public static ResultadoOperacion<bool> ValidarArchivoImagen(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo) || !File.Exists(rutaArchivo))
            {
                return new ResultadoOperacion<bool>(false, "El archivo de imagen no existe o la ruta no es válida.");
            }

            FileInfo informacionArchivo = new FileInfo(rutaArchivo);

            
            string extension = informacionArchivo.Extension.ToLower();
            if (!ExtensionesPermitidas.Contains(extension))
            {
                return new ResultadoOperacion<bool>(false, "Formato de imagen no permitido. Solo se aceptan archivos .jpg, .jpeg y .png.");
            }

          
            if (informacionArchivo.Length > TamanoMaximoBytes)
            {
                return new ResultadoOperacion<bool>(false, "El tamaño de la imagen supera el límite máximo permitido (2 MB).");
            }

            return new ResultadoOperacion<bool>(true, "Imagen válida.");
        }
    }
}
