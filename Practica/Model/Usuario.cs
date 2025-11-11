using System;
using System.Collections.Generic;
using System.Linq;
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
        public string Password { get { return this.password; } set { this.password = value; } }
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
            return true;
        }

        public bool VerificarContraseña(string contraseña) {
            return true;
        }

        public bool CambiarContraseña(string nuevaContraseña, string actualContraseña) {
            return true;
        }

        public bool ValidarEmail(string email) {
            return true;
        }

        public void ActualizarDatosPersonales(string nuevoNombre, string nuevoApellido, string nuevoEmail) {
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
    }
}
