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
            if (rol == null) {
                throw new ArgumentNullException(nameof(rol));
            }

            if (RolesDisponibles.Any(r => r.Nombre == rol.Nombre)) {
                throw new InvalidOperationException($"Ya existe un rol con el nombre '{rol.Nombre}' en este proyecto.");
            }

            RolesDisponibles.Add(rol);
        }

        public void AsignarRolAUsuario(Usuario usuario, Rol rol) {
            if (usuario == null) {
                throw new ArgumentNullException(nameof(usuario));
            }
            if (rol == null) {
                throw new ArgumentNullException(nameof(rol));
            }

            if (!RolesDisponibles.Contains(rol)) {
                throw new InvalidOperationException($"El rol '{rol.Nombre}' no está disponible en este proyecto y no puede ser asignado.");
            }

            string userKey = usuario.Username;

            if (!AsignacionesUsuarios.ContainsKey(userKey)) {
                AsignacionesUsuarios[userKey] = new List<Rol>();
            }

            var rolesAsignados = AsignacionesUsuarios[userKey];

            if (!rolesAsignados.Contains(rol)) {
                rolesAsignados.Add(rol);
            }
        }

        public void DesasignarRolDeUsuario(Usuario usuario, Rol rol) {
            if (usuario == null) {
                throw new ArgumentNullException(nameof(usuario));
            }
            if (rol == null) {
                throw new ArgumentNullException(nameof(rol));
            }

            string userKey = usuario.Username;

            if (AsignacionesUsuarios.TryGetValue(userKey, out List<Rol> rolesUsuario)) {
                rolesUsuario.Remove(rol);
            }
        }

        public List<string> ObtenerPermisosDeUsuario(Usuario usuario) {
            if (usuario == null) {
                throw new ArgumentNullException(nameof(usuario));
            }

            var permisosSet = new HashSet<string>();

            string userKey = usuario.Username;

            if (AsignacionesUsuarios.TryGetValue(userKey, out List<Rol> rolesAsignados)) {
                foreach (var rol in rolesAsignados) {
                    var permisosDelRol = rol.ObtenerPermisos();
                    permisosSet.UnionWith(permisosDelRol);
                }
            }

            return permisosSet.ToList();
        }
    }
}