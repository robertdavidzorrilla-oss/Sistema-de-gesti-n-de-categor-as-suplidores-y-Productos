using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace LogicaNegocio
{
    /// <summary>
    /// Registra excepciones y fallos del sistema 
    /// </summary>
    public static class LoggerService
    {
        private static readonly string RutaArchivoLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errores.log");
        private static readonly object BloqueoEscritura = new object();

        /// <summary>
        /// Escribe el detalle de una excepción en el archivo de logs.
        /// </summary>
        /// <param name="origen">Clase o método donde ocurrió el evento.</param>
        /// <param name="ex">Excepción capturada.</param>
        public static void RegistrarError(string origen, Exception ex)
        {
            lock (BloqueoEscritura)
            {
                try
                {
                    string contenido = $"============================================================\n" +
                                       $"FECHA/HORA : {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" +
                                       $"ORIGEN     : {origen}\n" +
                                       $"MENSAJE    : {ex.Message}\n" +
                                       $"STACK TRACE: {ex.StackTrace}\n" +
                                       $"============================================================\n\n";

                    File.AppendAllText(RutaArchivoLog, contenido);
                }
                catch
                {
                    // Evita bloquear la ejecución de la app en caso de que haya un fallo de escritura en el disco
                }
            }
        }
    }
}
