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

    }
}