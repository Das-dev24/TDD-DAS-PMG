using System;
using System.Collections.Generic;
using System.Linq;
using Practica.Model;

namespace Practica.Model {
    public class Proyecto {
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public List<Rol> RolesDisponibles { get; private set; }
        public Dictionary<string, List<Rol>> AsignacionesUsuarios { get; private set; }

        public Proyecto(string nombre, string descripcion) {
            if (string.IsNullOrWhiteSpace(nombre)) {
                throw new ArgumentException("El nombre del proyecto no puede ser nulo o vacío.", nameof(nombre));
            }

            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.RolesDisponibles = new List<Rol>();
            this.AsignacionesUsuarios = new Dictionary<string, List<Rol>>();
        }

        public void AgregarRol(Rol rol) {
        }

        public void AsignarRolAUsuario(Usuario usuario, Rol rol) {
        }

        public void DesasignarRolDeUsuario(Usuario usuario, Rol rol) {
        }

        public List<string> ObtenerPermisosDeUsuario(Usuario usuario) {
            return null;
        }
    }
}