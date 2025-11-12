using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Model {

    public enum Idioma { Espanol, Ingles, Frances }
    public enum Metodo_Autenticacion { Contraseña, SingleSignOn }

    public class Usuario {
        public string Username { get; private set; }
        public string Nombre { get; private set; }
        public string Apellidos { get; private set; }
        public string Email { get; private set; }
        private string password;
        public string Password { get { return this.password; } set { this.password = EncriptPassword(value); } }
        public bool Activo { get; private set; }
        public Idioma Idioma { get; private set; }
        public Metodo_Autenticacion Metodo_Autenticacion { get; private set; }
        public DateTime Expiration_Date { get; private set; }

        public Usuario(string usuario, string nombre, string apellidos, string contraseña, string email) {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("El parametro Usuario no puede ser Vacio/Nulo");
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El parametro Nombre no puede ser Vacio/Nulo");
            if (string.IsNullOrWhiteSpace(apellidos))
                throw new ArgumentException("El parametro Apellido no puede ser Vacio/Nulo");
            if (string.IsNullOrWhiteSpace(contraseña))
                throw new ArgumentException("El parametro Contraseña no puede ser Vacio/Nulo");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El parametro Email no puede ser Vacio/Nulo");

            if (ValidarEmail(email) || ValidarContraseña(contraseña)) {
                Username = usuario;
                Nombre = nombre;
                Apellidos = apellidos;
                Email = email;
                Password = contraseña;
                Activo = false;
                Idioma = Idioma.Espanol;
                Metodo_Autenticacion = Metodo_Autenticacion.Contraseña;
                Expiration_Date = DateTime.Now.AddDays(365);
            }

        }

        public bool ValidarContraseña(string contraseña) {
            if (string.IsNullOrWhiteSpace(contraseña))
                throw new ArgumentException("La contraseña no puede ser nula o vacía.");
            if (contraseña.Length < 12)
                throw new SecurityException("La contraseña debe tener al menos 12 caracteres.");
            if (contraseña.Any(char.IsWhiteSpace))
                throw new SecurityException("La contraseña no puede contener espacios en blanco.");
            if (!contraseña.Any(char.IsUpper))
                throw new SecurityException("La contraseña debe contener al menos una letra mayúscula.");
            if (!contraseña.Any(char.IsLower))
                throw new SecurityException("La contraseña debe contener al menos una letra minúscula.");
            if (!contraseña.Any(char.IsDigit))
                throw new SecurityException("La contraseña debe contener al menos un número.");
            string specialChars = "!@#$%^&*()_[]{};:'\",.<>/?`~";
            if (!contraseña.Any(c => specialChars.Contains(c)))
                throw new SecurityException("La contraseña debe contener al menos un carácter especial (ej: !@#$%).");

            return true;
        }

        public bool VerificarContraseña(string contraseña) {
            string hashAComparar = EncriptPassword(contraseña);
            return this.password == hashAComparar;
        }

        public bool CambiarContraseña(string nuevaContraseña, string actualContraseña) {
            if (string.IsNullOrWhiteSpace(nuevaContraseña))
                throw new ArgumentException("La nueva contraseña no puede ser nula o vacía.");

            if (!VerificarContraseña(actualContraseña)) {
                return false;
            }
            if (ValidarContraseña(nuevaContraseña)) {
                Password = nuevaContraseña;
            }

            return true;
        }

        public bool ValidarEmail(string email) {
            if (string.IsNullOrWhiteSpace(email)) {
                throw new FormatException("El email no puede ser nulo, vacío o contener solo espacios.");
            }
            if (email.Any(char.IsWhiteSpace)) {
                throw new FormatException("El email no puede contener espacios en blanco.");
            }

            try {
                var mailAddress = new MailAddress(email);
                if (mailAddress.Host.Contains("..")) {
                    throw new FormatException("El host del email no debe contener puntos consecutivos '..'.");
                }
                if (mailAddress.Host.StartsWith(".")) {
                    throw new FormatException("El host del email no debe empezar con un punto.");
                }
                if (mailAddress.Host.EndsWith(".")) {
                    throw new FormatException("El host del email no debe terminar con un punto.");
                }
                if (mailAddress.Host.Split('.').Last().Length < 2) {
                    throw new FormatException("El dominio de nivel superior (TLD) debe tener al menos 2 caracteres.");
                }
                if (!mailAddress.Host.Contains(".")) {
                    throw new FormatException("El email debe incluir un dominio válido (ej: .com, .es).");
                }
            } catch (FormatException ex) {
                throw new FormatException($"El formato del email '{email}' no es válido: {ex.Message}", ex);
            }

            return true;
        }

        public void ActualizarDatosPersonales(string nuevoNombre, string nuevoApellido, string nuevoEmail) {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El parametro del nuevo Nombre no puede ser Vacio/Nulo");

            if (string.IsNullOrWhiteSpace(nuevoApellido))
                throw new ArgumentException("El parametro del nuevo Apellidos no puede ser Vacio/Nulo");

            if (string.IsNullOrWhiteSpace(nuevoEmail))
                throw new ArgumentException("El parametro del nuevo Email no puede ser Vacio/Nulo");

            try {
                ValidarEmail(nuevoEmail);
            } catch (FormatException) {
                throw new FormatException("El formato del nuevo Email no es valido");
            }

            Nombre = nuevoNombre;
            Apellidos = nuevoApellido;
            Email = nuevoEmail;
        }

        public void ActivarUsuario() {
            this.Activo = true;
        }

        public void DesactivarUsuario() {
            this.Activo = false;
        }

        public bool HaExpirado() {
            return DateTime.Now > this.Expiration_Date;
        }

        public void ActualizarExpiracion(DateTime fecha) {
            Expiration_Date = fecha;
        }

        private static String EncriptPassword(String password) {
            // Crea una instancia del algoritmo SHA256.
            using (var sha256 = System.Security.Cryptography.SHA256.Create()) {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}