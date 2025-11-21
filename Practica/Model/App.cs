using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Model {
    public class App {

        public Usuario UsuarioActual { get; private set; }
        public Dictionary<Proyecto, HashSet<string>> PermisosPorProyecto { get; private set; }

        public App(Usuario usuario, List<Proyecto> todosLosProyectos) {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");

            if (todosLosProyectos == null)
                throw new ArgumentNullException(nameof(todosLosProyectos), "La lista de proyectos no puede ser nula.");

            UsuarioActual = usuario;
            PermisosPorProyecto = new Dictionary<Proyecto, HashSet<string>>();

            // Procesar accesos para cada proyecto listado
            foreach (var proyecto in todosLosProyectos) {
                // Obtenemos la lista de permisos (List<string>) desde el Proyecto
                var listaPermisos = proyecto.ObtenerPermisosDeUsuario(usuario);

                PermisosPorProyecto[proyecto] = new HashSet<string>(listaPermisos);
            }
        }

        public bool TienePermisoEnProyecto(Proyecto proyecto, string permiso) {
            if (proyecto == null)
                throw new ArgumentNullException(nameof(proyecto));

            if (string.IsNullOrEmpty(permiso))
                throw new ArgumentException("El permiso no puede ser nulo o vacío.", nameof(permiso));

            // Si el proyecto está en esta sesión, verificamos el permiso
            if (PermisosPorProyecto.ContainsKey(proyecto)) {
                return PermisosPorProyecto[proyecto].Contains(permiso);
            }

            // Si el proyecto no está cargado o no existe en la sesión, asumimos sin permisos
            return false;
        }

        public HashSet<string> ObtenerPermisosDeProyecto(Proyecto proyecto) {
            if (proyecto == null)
                throw new ArgumentNullException(nameof(proyecto));

            if (PermisosPorProyecto.ContainsKey(proyecto)) {
                return new HashSet<string>(PermisosPorProyecto[proyecto]);
            }

            return new HashSet<string>();
        }

        public HashSet<string> ObtenerPermisosGlobales() {
            HashSet<string> permisosGlobales = new HashSet<string>();

            foreach (var permisosProyecto in PermisosPorProyecto.Values) {
                // UnionWith añade elementos al set actual, ignorando duplicados automáticamente
                permisosGlobales.UnionWith(permisosProyecto);
            }

            return permisosGlobales;
        }
    }
}