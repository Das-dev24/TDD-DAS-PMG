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

        [TestInitialize]
        public void Setup() {
            // Datos base del proyecto
            nombreProyectoValido = "Proyecto de Prueba";
            descProyectoValida = "Descripción detallada del proyecto de prueba.";
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

    }
}