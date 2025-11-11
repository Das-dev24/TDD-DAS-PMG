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
        public DateTime Expiration_Date { get; private set; }

        public Usuario(string usuario, string nombre, string apellidos, string contraseña, string email) {
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
        }
        
        public void DesactivarUsuario() { 
        }
        
        public bool HaExpirado() { 
            return true; 
        }
        
        public void ActualizarExpiracion(DateTime nuevaFecha) {
        }
    }
}
