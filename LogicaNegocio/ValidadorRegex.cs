using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogicaNegocio
{
    /// <summary>
    /// Proporciona métodos de validación basados en Expresiones Regulares.
    /// </summary>
    public static class ValidadorRegex
    {
        
        private static readonly Regex RegexEmail = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

         
        private static readonly Regex RegexTelefonoRD = new Regex(@"^(809|829|849)[ -]?\d{3}[ -]?\d{4}$", RegexOptions.Compiled);

       
        private static readonly Regex RegexTextoValido = new Regex(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\.\,\#\-\(\)]+$", RegexOptions.Compiled);

        public static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return RegexEmail.IsMatch(email.Trim());
        }

        public static bool EsTelefonoRDValido(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono)) return false;
            return RegexTelefonoRD.IsMatch(telefono.Trim());
        }

        public static bool EsTextoValido(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;
            return RegexTextoValido.IsMatch(texto.Trim());
        }
    }
}
