using Microsoft.VisualStudio.TestTools.UnitTesting;
using Practica.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticaTestProject.Model {

    [TestClass]
    public class AppTest {

        // Variables de entorno para las pruebas
        private Usuario usuarioValido;
        private Usuario usuarioSinRoles;
        private Proyecto proyectoA;
        private Proyecto proyectoB;
        private Proyecto proyectoSinAsignacion; // Proyecto donde el usuario no tiene roles
        private Proyecto proyectoNoListado; // Proyecto que no se pasará al constructor de App

        private Rol rolAdmin;   // Permisos: LEER, ESCRIBIR, BORRAR
        private Rol rolLector;  // Permisos: LEER

        private List<Proyecto> todosLosProyectos;

        [TestInitialize]
        public void Setup() {
            // Creacion Usuarios
            usuarioValido = new Usuario("UserApp", "Ana", "Test", "Pass123!", "ana@test.com");
            usuarioSinRoles = new Usuario("UserNoRoles", "Pepe", "Test", "Pass123!", "pepe@test.com");

            // Creacion Roles y Permisos
            rolAdmin = new Rol("Admin", "Control total");
            rolAdmin.AgregarPermiso("LEER");
            rolAdmin.AgregarPermiso("ESCRIBIR");
            rolAdmin.AgregarPermiso("BORRAR");

            rolLector = new Rol("Lector", "Solo lectura");
            rolLector.AgregarPermiso("LEER");

            // Proyecto A Usuario tiene rol Admin (LEER, ESCRIBIR, BORRAR)
            proyectoA = new Proyecto("Proyecto A", "Principal");
            proyectoA.AgregarRol(rolAdmin);
            proyectoA.AsignarRolAUsuario(usuarioValido, rolAdmin);

            // Proyecto B Usuario tiene rol Lector (LEER) -> "LEER" se repite, prueba solapamiento global
            proyectoB = new Proyecto("Proyecto B", "Secundario");
            proyectoB.AgregarRol(rolLector);
            proyectoB.AsignarRolAUsuario(usuarioValido, rolLector);

            // Proyecto Sin Asignación Usuario no tiene roles
            proyectoSinAsignacion = new Proyecto("Proyecto Vacío", "Sin roles para user");

            // Proyecto no listado no se pasará al constructor de App
            proyectoNoListado = new Proyecto("Proyecto Externo", "No cargado");

            // Lista de proyectos del sistema
            todosLosProyectos = new List<Proyecto> { proyectoA, proyectoB, proyectoSinAsignacion };
        }

        #region Pruebas del Constructor

        // Constructor_DatosValidos_InicializaEstadoCorrectamente
        [TestMethod]
        public void Constructor_DatosValidos_InicializaEstadoCorrectamente() {
            // Arrange (Setup)

            // Act
            var app = new App(usuarioValido, todosLosProyectos);

            // Assert
            Assert.AreEqual(usuarioValido, app.UsuarioActual, "El usuario actual no se asignó correctamente.");
            Assert.IsNotNull(app.PermisosPorProyecto, "El diccionario de permisos no debe ser nulo.");

            // Verificar que se cargaron los proyectos de la lista
            Assert.IsTrue(app.PermisosPorProyecto.ContainsKey(proyectoA), "Debe contener Proyecto A");
            Assert.IsTrue(app.PermisosPorProyecto.ContainsKey(proyectoB), "Debe contener Proyecto B");
            Assert.IsTrue(app.PermisosPorProyecto.ContainsKey(proyectoSinAsignacion), "Debe contener Proyecto Sin Asignación");

            // Verificar que NO contiene proyectos externos
            Assert.IsFalse(app.PermisosPorProyecto.ContainsKey(proyectoNoListado), "No debe contener proyectos no listados.");
        }

        // Constructor_UsuarioNulo_LanzaArgumentNullException
        [TestMethod]
        public void Constructor_UsuarioNulo_LanzaArgumentNullException() {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => {
                new App(null, todosLosProyectos);
            });
        }

        // Constructor_ListaProyectosNula_LanzaArgumentNullException
        [TestMethod]
        public void Constructor_ListaProyectosNula_LanzaArgumentNullException() {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => {
                new App(usuarioValido, null);
            });
        }

        #endregion

        #region Pruebas de TienePermisoEnProyecto

        // TienePermiso_PermisoExisteEnProyecto_DevuelveTrue
        [TestMethod]
        public void TienePermiso_PermisoExisteEnProyecto_DevuelveTrue() {
            // Arrange
            var app = new App(usuarioValido, todosLosProyectos);

            // Act
            bool tienePermiso = app.TienePermisoEnProyecto(proyectoA, "BORRAR");

            // Assert
            Assert.IsTrue(tienePermiso, "El usuario debería tener permiso BORRAR en Proyecto A.");
        }

        // TienePermiso_PermisoNoExisteEnProyecto_DevuelveFalse
        [TestMethod]
        public void TienePermiso_PermisoNoExisteEnProyecto_DevuelveFalse() {
            // Arrange
            var app = new App(usuarioValido, todosLosProyectos);

            // Act
            // El usuario es Admin en Proyecto A, pero "AUDITAR" no está en sus roles
            bool tienePermiso = app.TienePermisoEnProyecto(proyectoA, "AUDITAR");

            // Assert
            Assert.IsFalse(tienePermiso, "El usuario no debería tener un permiso que no posee su rol.");
        }

        // TienePermiso_UsuarioNoTieneRolesEnProyecto_DevuelveFalse
        [TestMethod]
        public void TienePermiso_UsuarioNoTieneRolesEnProyecto_DevuelveFalse() {
            // Arrange
            var app = new App(usuarioValido, todosLosProyectos);

            // Act
            // proyectoSinAsignacion está mapeado, pero el usuario no tiene roles allí
            bool tienePermiso = app.TienePermisoEnProyecto(proyectoSinAsignacion, "LEER");

            // Assert
            Assert.IsFalse(tienePermiso, "El usuario no debería tener permisos en un proyecto donde no tiene roles.");
        }

        // TienePermiso_ProyectoNoMapeado_DevuelveFalse
        [TestMethod]
        public void TienePermiso_ProyectoNoMapeado_DevuelveFalse() {
            // Arrange
            var app = new App(usuarioValido, todosLosProyectos);

            // Act
            // Consultamos por un proyecto que no estaba en la lista inicial
            bool tienePermiso = app.TienePermisoEnProyecto(proyectoNoListado, "LEER");

            // Assert
            Assert.IsFalse(tienePermiso, "Debe devolver false para proyectos desconocidos/no cargados.");
        }

        // TienePermiso_ArgumentosInvalidos_LanzaArgumentException
        [TestMethod]
        public void TienePermiso_ArgumentosInvalidos_LanzaExcepcion() {
            // Arrange
            var app = new App(usuarioValido, todosLosProyectos);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => {
                app.TienePermisoEnProyecto(null, "LEER");
            }, "Proyecto nulo debe lanzar ArgumentNullException");

            Assert.ThrowsException<ArgumentException>(() => {
                app.TienePermisoEnProyecto(proyectoA, "");
            }, "Permiso vacío debe lanzar ArgumentException");
        }

        #endregion

        #region Pruebas de ObtenerPermisosDeProyecto

        // ObtenerPermisos_ProyectoValido_DevuelveHashSetPermisosCorrecto
        [TestMethod]
        public void ObtenerPermisos_ProyectoValido_DevuelveHashSetPermisosCorrecto() {
            // Arrange
            var app = new App(usuarioValido, todosLosProyectos);
            var esperados = new List<string> { "LEER", "ESCRIBIR", "BORRAR" };

            // Act
            var permisos = app.ObtenerPermisosDeProyecto(proyectoA);

            // Assert
            Assert.IsNotNull(permisos);
            Assert.AreEqual(3, permisos.Count);
            // Verificamos que contenga todos los esperados
            foreach (var p in esperados) {
                Assert.IsTrue(permisos.Contains(p));
            }
        }

        // ObtenerPermisos_ProyectoNoMapeado_DevuelveHashSetVacio
        [TestMethod]
        public void ObtenerPermisos_ProyectoNoMapeado_DevuelveHashSetVacio() {
            // Arrange
            var app = new App(usuarioValido, todosLosProyectos);

            // Act
            var permisos = app.ObtenerPermisosDeProyecto(proyectoNoListado);

            // Assert
            Assert.IsNotNull(permisos, "No debe devolver null aunque el proyecto no exista.");
            Assert.AreEqual(0, permisos.Count, "Debe devolver una colección vacía.");
        }

        // ObtenerPermisos_ProyectoNulo_LanzaArgumentNullException
        [TestMethod]
        public void ObtenerPermisos_ProyectoNulo_LanzaArgumentNullException() {
            // Arrange
            var app = new App(usuarioValido, todosLosProyectos);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => {
                app.ObtenerPermisosDeProyecto(null);
            });
        }

        #endregion

        #region Pruebas de ObtenerPermisosGlobales

        // ObtenerPermisosGlobales_ConPermisosSolapados_DevuelveUnionSinDuplicados
        [TestMethod]
        public void ObtenerPermisosGlobales_ConPermisosSolapados_DevuelveUnionSinDuplicados() {
            // Arrange
            // Proyecto A tiene: LEER, ESCRIBIR, BORRAR
            // Proyecto B tiene: LEER
            // Global debería ser: LEER, ESCRIBIR, BORRAR (3 permisos únicos)
            var app = new App(usuarioValido, todosLosProyectos);

            // Act
            var globales = app.ObtenerPermisosGlobales();

            // Assert
            Assert.AreEqual(3, globales.Count, "El conteo global es incorrecto (debería eliminar duplicados).");
            Assert.IsTrue(globales.Contains("LEER"));
            Assert.IsTrue(globales.Contains("ESCRIBIR"));
            Assert.IsTrue(globales.Contains("BORRAR"));
        }

        // ObtenerPermisosGlobales_UsuarioSinPermisos_DevuelveHashSetVacio
        [TestMethod]
        public void ObtenerPermisosGlobales_UsuarioSinPermisos_DevuelveHashSetVacio() {
            // Arrange
            // Usamos el usuario que no tiene roles en ningún proyecto
            var app = new App(usuarioSinRoles, todosLosProyectos);

            // Act
            var globales = app.ObtenerPermisosGlobales();

            // Assert
            Assert.IsNotNull(globales);
            Assert.AreEqual(0, globales.Count);
        }

        #endregion

    }
}
