using System;
using System.Collections.Generic; // Necesario para HashSet

namespace Practica.Model
{
    public class Rol
    {
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }

        private readonly HashSet<string> _permisos;

        public Rol(string nombre, string descripcion)
        {
            
            if (string.IsNullOrWhiteSpace(nombre))
            {
               
                throw new ArgumentException("El nombre del rol no puede ser nulo o vacío.", nameof(nombre));
            }

            Nombre = nombre;
            Descripcion = descripcion;

            _permisos = new HashSet<string>();
        }

        public void AgregarPermiso(string permiso)
        {
            if (string.IsNullOrWhiteSpace(permiso))
            {
                throw new ArgumentException("El permiso no puede ser nulo o vacío.", nameof(permiso));
            }
            _permisos.Add(permiso);
        }

        public void QuitarPermiso(string permiso)
        {
            // Satisface: QuitarPermiso_PermisoNuloOVacio_LanzaArgumentException
            if (string.IsNullOrWhiteSpace(permiso))
            {
                throw new ArgumentException("El permiso no puede ser nulo o vacío.", nameof(permiso));
            }
            _permisos.Remove(permiso);
        }

        public HashSet<string> ObtenerPermisos()
        {
            return new HashSet<string>(_permisos);
        }

        public bool TienePermiso(string permiso)
        {
            return _permisos.Contains(permiso);
        }
    }
}