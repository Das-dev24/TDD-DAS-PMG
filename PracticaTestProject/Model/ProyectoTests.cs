using Microsoft.VisualStudio.TestTools.UnitTesting;
using Practica.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticaTestProject.Model {

    [TestClass]
    public class ProyectoTest {

        // Variables para datos válidos
        private string nombreProyectoValido;
        private string descProyectoValida;
        private Usuario usuarioValido;
        private Rol rolValido1;
        private Rol rolValido2;
        private Rol rolConPermisosSolapados;

        // Proyecto que se usará en múltiples pruebas. Se reinicia en cada test.
        private Proyecto proyecto;

        [TestInitialize]
        public void Setup() {
            // Datos base del proyecto
            nombreProyectoValido = "Proyecto de Prueba";
            descProyectoValida = "Descripción detallada del proyecto de prueba.";

            // Instancia base del proyecto
            proyecto = new Proyecto(nombreProyectoValido, descProyectoValida);

            // Usuarios de prueba (según constructor de Usuario)
            usuarioValido = new Usuario("User001", "Ana", "Gomez Sanchez", "Contraseña123!", "ana@example.com");

            // Roles de prueba (según constructor de Rol)
            rolValido1 = new Rol("Administrador", "Acceso total");

            // Usamos el método AgregarPermiso() de la clase Rol
            rolValido1.AgregarPermiso("CREAR");
            rolValido1.AgregarPermiso("EDITAR");
            rolValido1.AgregarPermiso("BORRAR");

            rolValido2 = new Rol("Lector", "Solo lectura");
            rolValido2.AgregarPermiso("LEER");

            rolConPermisosSolapados = new Rol("Editor", "Lectura y edición");
            rolConPermisosSolapados.AgregarPermiso("LEER"); // Permiso solapado con rolValido2
            rolConPermisosSolapados.AgregarPermiso("EDITAR"); // Permiso solapado con rolValido1
        }

        #region Pruebas del Constructor

        // Constructor_DatosValidos_AsignaPropiedadesCorrectamente
        [TestMethod]
        public void Constructor_DatosValidos_AsignaPropiedadesCorrectamente() {
            // Arrange (Organizar)
            // Datos definidos en Setup

            // Act (Actuar)
            var p = new Proyecto(nombreProyectoValido, descProyectoValida);

            // Assert (Afirmar)
            Assert.AreEqual(nombreProyectoValido, p.Nombre, "El Nombre del proyecto no coincide.");
            Assert.AreEqual(descProyectoValida, p.Descripcion, "La Descripción del proyecto no coincide.");
        }

        // Constructor_DatosValidos_InicializaColeccionesVacias
        [TestMethod]
        public void Constructor_DatosValidos_InicializaColeccionesVacias() {
            // Arrange
            // Datos definidos en Setup

            // Act
            var p = new Proyecto(nombreProyectoValido, descProyectoValida);

            // Assert
            Assert.IsNotNull(p.RolesDisponibles, "RolesDisponibles no debe ser nulo.");
            Assert.AreEqual(0, p.RolesDisponibles.Count, "RolesDisponibles debe inicializarse vacía.");
            Assert.IsNotNull(p.AsignacionesUsuarios, "AsignacionesUsuarios no debe ser nulo.");
            Assert.AreEqual(0, p.AsignacionesUsuarios.Count, "AsignacionesUsuarios debe inicializarse vacío.");
        }

        // Constructor_NombreNuloOVacio_LanzaArgumentException
        [DataTestMethod]
        [DataRow(null, "desc")]
        [DataRow("", "desc")]
        [DataRow(" ", "desc")]
        public void Constructor_NombreNuloOVacio_LanzaArgumentException(string nombreInvalido, string desc) {
            // Arrange, Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => {
                new Proyecto(nombreInvalido, desc);
            });
        }

        #endregion

        #region Pruebas de AgregarRol

        // AgregarRol_RolValido_AñadeRolARolesDisponibles
        [TestMethod]
        public void AgregarRol_RolValido_AñadeRolARolesDisponibles() {
            // Arrange
            // proyecto y rolValido1 creados en Setup

            // Act
            proyecto.AgregarRol(rolValido1);

            // Assert
            Assert.AreEqual(1, proyecto.RolesDisponibles.Count, "La lista de roles debe tener 1 elemento.");
            Assert.IsTrue(proyecto.RolesDisponibles.Contains(rolValido1), "La lista debe contener el rol añadido.");
        }

        // AgregarRol_RolNulo_LanzaArgumentNullException
        [TestMethod]
        public void AgregarRol_RolNulo_LanzaArgumentNullException() {
            // Arrange
            // proyecto creado en Setup

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentNullException>(() => {
                proyecto.AgregarRol(null);
            });
        }

        // AgregarRol_RolDuplicado_LanzaInvalidOperationException
        [TestMethod]
        public void AgregarRol_RolDuplicado_LanzaInvalidOperationException() {
            // Arrange
            // Añadimos el rol una vez
            proyecto.AgregarRol(rolValido1);

            // Creamos un rol con el mismo nombre
            var rolDuplicado = new Rol("Administrador", "Descripción duplicada");

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => {
                proyecto.AgregarRol(rolDuplicado);
            }, "Debe lanzar InvalidOperationException si el nombre del rol ya existe.");
        }

        #endregion

        #region Pruebas de AsignarRolAUsuario

        // AsignarRol_UsuarioYRolValidos_AsignaCorrectamente
        [TestMethod]
        public void AsignarRol_UsuarioYRolValidos_AsignaCorrectamente() {
            // Arrange
            // El rol debe existir en RolesDisponibles primero
            proyecto.AgregarRol(rolValido1);

            // Act
            proyecto.AsignarRolAUsuario(usuarioValido, rolValido1);

            // Assert
            // Verifica que el usuario está en el diccionario (usando Username como clave)
            Assert.IsTrue(proyecto.AsignacionesUsuarios.ContainsKey(usuarioValido.Username));
            // Verifica que la lista de roles del usuario contiene el rol
            var rolesAsignados = proyecto.AsignacionesUsuarios[usuarioValido.Username];
            Assert.AreEqual(1, rolesAsignados.Count);
            Assert.IsTrue(rolesAsignados.Contains(rolValido1));
        }

        // AsignarRol_UsuarioNuevo_CreaEntradaEnDiccionario
        [TestMethod]
        public void AsignarRol_UsuarioNuevo_CreaEntradaEnDiccionario() {
            // Arrange
            proyecto.AgregarRol(rolValido1);
            // Pre-condición: El diccionario está vacío
            Assert.AreEqual(0, proyecto.AsignacionesUsuarios.Count, "El diccionario debe estar vacío al inicio.");

            // Act
            proyecto.AsignarRolAUsuario(usuarioValido, rolValido1);

            // Assert
            Assert.AreEqual(1, proyecto.AsignacionesUsuarios.Count, "El diccionario debe contener una entrada.");
            Assert.IsTrue(proyecto.AsignacionesUsuarios.ContainsKey(usuarioValido.Username), "La clave debe ser el Username del usuario.");
        }

        // AsignarRol_RolNoEnDisponibles_LanzaInvalidOperationException
        [TestMethod]
        public void AsignarRol_RolNoEnDisponibles_LanzaInvalidOperationException() {
            // Arrange
            // No añadimos el rolValido1 a RolesDisponibles

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => {
                proyecto.AsignarRolAUsuario(usuarioValido, rolValido1);
            }, "Debe lanzar excepción si el rol no está en RolesDisponibles.");
        }

        // AsignarRol_AsignarRolDuplicado_NoAñadeDuplicadoYNoLanzaExcepcion
        [TestMethod]
        public void AsignarRol_AsignarRolDuplicado_NoAñadeDuplicadoYNoLanzaExcepcion() {
            // Arrange
            proyecto.AgregarRol(rolValido1);
            // Asignamos una vez
            proyecto.AsignarRolAUsuario(usuarioValido, rolValido1);
            var rolesAsignados = proyecto.AsignacionesUsuarios[usuarioValido.Username];
            Assert.AreEqual(1, rolesAsignados.Count, "El usuario debe tener 1 rol.");

            // Act
            // Intentamos asignar el MISMO rol de nuevo
            proyecto.AsignarRolAUsuario(usuarioValido, rolValido1);

            // Assert
            // El método debe finalizar sin error y el conteo seguir siendo 1
            Assert.AreEqual(1, rolesAsignados.Count, "El conteo de roles no debe cambiar.");
        }

        // AsignarRol_UsuarioNulo_LanzaArgumentNullException
        [TestMethod]
        public void AsignarRol_UsuarioNulo_LanzaArgumentNullException() {
            // Arrange
            proyecto.AgregarRol(rolValido1);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => {
                proyecto.AsignarRolAUsuario(null, rolValido1);
            });
        }

        // AsignarRol_RolNulo_LanzaArgumentNullException
        [TestMethod]
        public void AsignarRol_RolNulo_LanzaArgumentNullException() {
            // Arrange
            // No se necesita añadir rol

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => {
                proyecto.AsignarRolAUsuario(usuarioValido, null);
            });
        }

        #endregion

        #region Pruebas de DesasignarRolDeUsuario

        // DesasignarRol_UsuarioYRolValidos_EliminaAsignacion
        [TestMethod]
        public void DesasignarRol_UsuarioYRolValidos_EliminaAsignacion() {
            // Arrange
            proyecto.AgregarRol(rolValido1);
            proyecto.AsignarRolAUsuario(usuarioValido, rolValido1);
            // Verificación previa
            var rolesUsuario = proyecto.AsignacionesUsuarios[usuarioValido.Username];
            Assert.AreEqual(1, rolesUsuario.Count, "El usuario debe tener 1 rol asignado antes de desasignar.");

            // Act
            proyecto.DesasignarRolDeUsuario(usuarioValido, rolValido1);

            // Assert
            Assert.AreEqual(0, rolesUsuario.Count, "La lista de roles del usuario debe estar vacía.");
            Assert.IsFalse(rolesUsuario.Contains(rolValido1), "El rol no debe estar en la lista.");
        }

        // DesasignarRol_UsuarioConMultiplesRoles_EliminaSoloElRolEspecifico
        [TestMethod]
        public void DesasignarRol_UsuarioConMultiplesRoles_EliminaSoloElRolEspecifico() {
            // Arrange
            proyecto.AgregarRol(rolValido1);
            proyecto.AgregarRol(rolValido2);
            proyecto.AsignarRolAUsuario(usuarioValido, rolValido1);
            proyecto.AsignarRolAUsuario(usuarioValido, rolValido2);
            // Verificación previa
            var rolesUsuario = proyecto.AsignacionesUsuarios[usuarioValido.Username];
            Assert.AreEqual(2, rolesUsuario.Count, "El usuario debe tener 2 roles asignados.");

            // Act
            // Eliminamos el rol 1
            proyecto.DesasignarRolDeUsuario(usuarioValido, rolValido1);

            // Assert
            Assert.AreEqual(1, rolesUsuario.Count, "La lista de roles del usuario debe tener 1 rol restante.");
            Assert.IsFalse(rolesUsuario.Contains(rolValido1), "El rol 1 debe haber sido eliminado.");
            Assert.IsTrue(rolesUsuario.Contains(rolValido2), "El rol 2 debe permanecer.");
        }

        // DesasignarRol_UsuarioNoTieneEseRol_NoLanzaExcepcion
        [TestMethod]
        public void DesasignarRol_UsuarioNoTieneEseRol_NoLanzaExcepcion() {
            // Arrange
            proyecto.AgregarRol(rolValido1); // Rol a desasignar
            proyecto.AgregarRol(rolValido2); // Rol que si tiene el usuario
            proyecto.AsignarRolAUsuario(usuarioValido, rolValido2);

            var rolesUsuario = proyecto.AsignacionesUsuarios[usuarioValido.Username];
            Assert.AreEqual(1, rolesUsuario.Count);

            // Act
            // Intentamos eliminar el rolValido1, que el usuario no tiene.
            proyecto.DesasignarRolDeUsuario(usuarioValido, rolValido1);

            // Assert
            // El estado no debe haber cambiado.
            Assert.AreEqual(1, rolesUsuario.Count, "El conteo de roles no debió cambiar.");
            Assert.IsTrue(rolesUsuario.Contains(rolValido2), "El rol 2 debe seguir presente.");
        }

        // DesasignarRol_UsuarioNoExisteEnDiccionario_NoLanzaExcepcion
        [TestMethod]
        public void DesasignarRol_UsuarioNoExisteEnDiccionario_NoLanzaExcepcion() {
            // Arrange
            proyecto.AgregarRol(rolValido1);
            // usuarioValido no ha sido añadido a AsignacionesUsuarios
            Assert.AreEqual(0, proyecto.AsignacionesUsuarios.Count, "El diccionario debe estar vacío.");

            // Act
            proyecto.DesasignarRolDeUsuario(usuarioValido, rolValido1);

            // Assert
            // El método simplemente debe finalizar.
            Assert.AreEqual(0, proyecto.AsignacionesUsuarios.Count, "El diccionario debe seguir vacío.");
        }

        // DesasignarRol_UsuarioNulo_LanzaArgumentNullException
        [TestMethod]
        public void DesasignarRol_UsuarioNulo_LanzaArgumentNullException() {
            // Arrange
            proyecto.AgregarRol(rolValido1);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => {
                proyecto.DesasignarRolDeUsuario(null, rolValido1);
            });
        }

        // DesasignarRol_RolNulo_LanzaArgumentNullException
        [TestMethod]
        public void DesasignarRol_RolNulo_LanzaArgumentNullException() {
            // Arrange
            // Asignamos un rol para que el usuario exista en el diccionario
            proyecto.AgregarRol(rolValido1);
            proyecto.AsignarRolAUsuario(usuarioValido, rolValido1);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => {
                proyecto.DesasignarRolDeUsuario(usuarioValido, null);
            });
        }

        #endregion


    }
}