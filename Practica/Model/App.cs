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
    }
}