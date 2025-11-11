using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace PracticaTestProject.Model {

    [TestClass]
    public class UsuarioTest {

        private string usuarioValido;
        private string nombreValido;
        private string apellidosValidos;
        private string contraseñaValida;
        private string EmailValido;
        private DateTime StartDate;

        [TestInitialize]
        public void Setup() {
            usuarioValido = "User123";
            nombreValido = "Usuario";
            apellidosValidos = "Normal Diez";
            contraseñaValida = "User1234567!";
            EmailValido = "example@example.com";
            StartDate = DateTime.Now;
        }

        #region Pruebas del Constructor

        // Constructor_DatosValidos_AsignaPropiedadesCorrectamente
        [TestMethod]
        public void Constructor_DatosValidos_AsignaPropiedadesCorrectamente() {
            // Arrange (Organizar)
            // Los datos ya están en las variables de la clase.

            // Act (Actuar)
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Assert (Afirmar)
            Assert.AreEqual(usuarioValido, usuario.Usuario, "El Usuario no coincide con el esperado.");
            Assert.AreEqual(nombreValido, usuario.Nombre, "El Nombre introducido no coincide con el esperado.");
            Assert.AreEqual(apellidosValidos, usuario.Apellidos, "Los Apellidos introducidos no coinciden con los esperados.");
            Assert.AreEqual(EmailValido, usuario.Email, "El Email introducido no coincide con el esperado.");


            // Probamos que la contraseña se guardó correctamente usando su propio método
            Assert.IsTrue(usuario.VerificarContraseña(contraseñaValida), "La contraseña válida no se verificó correctamente.");

            // Propiedades por defecto según tu especificación
            Assert.IsFalse(usuario.Activo, "El usuario debe crearse como 'Activo = false'");
            Assert.AreEqual(Idioma.Espanol, usuario.Idioma, "El idioma por defecto debe ser Español");

            // Verificar la fecha de expiración (365 días)
            Assert.IsTrue(usuario.Expiration_Date > StartDate.AddDays(364) &&
                          usuario.Expiration_Date < StartDate.AddDays(366),
                          "La fecha de expiración debe ser 365 días desde hoy.");
        }

        // Constructor_NuloOVacio_LanzaArgumentException
        [DataTestMethod]
        [DataRow(null, "Nombre", "Apellidos", "Pass123!", "email@test.com", "El parametro Usuario no puede ser Vacio/Nulo")]
        [DataRow("", "Nombre", "Apellidos", "Pass123!", "email@test.com", "El parametro Usuario no puede ser Vacio/Nulo")]
        [DataRow(" ", "Nombre", "Apellidos", "Pass123!", "email@test.com", "El parametro Usuario no puede ser Vacio/Nulo")]
        [DataRow("user", null, "Apellidos", "Pass123!", "email@test.com", "El parametro Nombre no puede ser Vacio/Nulo")]
        [DataRow("user", "", "Apellidos", "Pass123!", "email@test.com", "El parametro Nombre no puede ser Vacio/Nulo")]
        [DataRow("user", " ", "Apellidos", "Pass123!", "email@test.com", "El parametro Nombre no puede ser Vacio/Nulo")]
        [DataRow("user", "Nombre", null, "Pass123!", "email@test.com", "El parametro Apellido no puede ser Vacio/Nulo")]
        [DataRow("user", "Nombre", "", "Pass123!", "email@test.com", "El parametro Apellido no puede ser Vacio/Nulo")]
        [DataRow("user", "Nombre", " ", "Pass123!", "email@test.com", "El parametro Apellido no puede ser Vacio/Nulo")]
        [DataRow("user", "Nombre", "Apellidos", null, "email@test.com", "El parametro Contraseña no puede ser Vacio/Nulo")]
        [DataRow("user", "Nombre", "Apellidos", "", "email@test.com", "El parametro Contraseña no puede ser Vacio/Nulo")]
        [DataRow("user", "Nombre", "Apellidos", " ", "email@test.com", "El parametro Contraseña no puede ser Vacio/Nulo")]
        [DataRow("user", "Nombre", "Apellidos", "Pass123!", null, "El parametro Email no puede ser Vacio/Nulo")]
        [DataRow("user", "Nombre", "Apellidos", "Pass123!", "", "El parametro Email no puede ser Vacio/Nulo")]
        [DataRow("user", "Nombre", "Apellidos", "Pass123!", " ", "El parametro Email no puede ser Vacio/Nulo")]
        public void Constructor_ArgumentoInvalido_LanzaArgumentException(string user, string nombre, string apellidos, string pass, string email, string expectedParamName) {
            // Arrange, Act & Assert
            // Verificamos que se lanza la excepción esperada
            var ex = Assert.ThrowsException<ArgumentException>(() => {
                new Usuario(user, nombre, apellidos, pass, email);
            });

            // Verificar el mensaje de la excepción.
            Assert.AreEqual(expectedParamName, ex.Message);
        }

        #endregion

    }
}
