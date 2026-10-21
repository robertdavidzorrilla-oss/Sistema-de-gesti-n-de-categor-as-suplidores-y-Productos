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
    /// Gestiona la validación, almacenamiento en disco y registro en BD de las imágenes de categorías.
    /// </summary>
    public class ImagenService
    {
        private readonly string _carpetaDestinoImagenes;

        public ImagenService()
        {
            
            _carpetaDestinoImagenes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes");

            if (!Directory.Exists(_carpetaDestinoImagenes))
            {
                Directory.CreateDirectory(_carpetaDestinoImagenes);
            }
        }

        /// <summary>
        /// Valida el archivo, lo copia al directorio local de la aplicación y crea el registro de la imagen.
        /// </summary>
        /// <param name="rutaOrigenArchivo">Ruta completa del archivo seleccionado desde el OpenFileDialog.</param>
        /// <returns>ResultadoOperacion con el objeto Imagen recién procesado.</returns>
        public ResultadoOperacion<Imagen> ProcesarYGuardarImagen(string rutaOrigenArchivo)
        {
            try
            {
               
                ResultadoOperacion<bool> resultadoValidacion = ValidadorImagen.ValidarArchivoImagen(rutaOrigenArchivo);
                if (!resultadoValidacion.Exito)
                {
                    return new ResultadoOperacion<Imagen>(false, resultadoValidacion.Mensaje, null);
                }

                FileInfo infoArchivo = new FileInfo(rutaOrigenArchivo);
                string nombreUnicoArchivo = $"{Guid.NewGuid()}{infoArchivo.Extension}";
                string rutaDestinoCompleta = Path.Combine(_carpetaDestinoImagenes, nombreUnicoArchivo);

                File.Copy(rutaOrigenArchivo, rutaDestinoCompleta, overwrite: true);

                Imagen nuevaImagen = new Imagen
                {
                    FechaCreacion = DateTime.Now,
                    Estado = true
                };

                return new ResultadoOperacion<Imagen>(true, "Imagen guardada correctamente.", nuevaImagen);
            }
            catch (Exception ex)
            {
                LoggerService.RegistrarError("ImagenService.ProcesarYGuardarImagen", ex);
                return new ResultadoOperacion<Imagen>(false, "Error al procesar la imagen: " + ex.Message, null);
            }
        }
    }
}
