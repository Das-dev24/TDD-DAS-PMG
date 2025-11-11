using Microsoft.VisualStudio.TestTools.UnitTesting;
using Practica.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security;
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
            Assert.AreEqual(usuarioValido, usuario.Username, "El Usuario no coincide con el esperado.");
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

        #region Pruebas de Contraseña (Validar, Verificar, Cambiar)

        // ValidarContraseña_ContraseñaCorrecta_DevuelveTrue
        [TestMethod]
        public void ValidarContraseña_ContraseñaCorrecta_DevuelveTrue() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Act
            bool resultado = usuario.ValidarContraseña("ContraseñaValida123");

            // Assert
            Assert.IsTrue(resultado);
        }

        // ValidarContraseña_ContraseñaIncorrecta_LanzaSecurityException
        [DataTestMethod]
        [DataRow("User123!")]               // Menos de 12
        [DataRow("user1234567!")]           // Sin mayúscula
        [DataRow("USER1234567!")]           // Sin minúscula
        [DataRow("User12345678")]           // Sin caracter especial
        [DataRow("User 123456!")]           // Con espacios
        public void ValidarContraseña_ContraseñaIncorrecta_LanzaSecurityException(string passIncorrecta) {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Act & Assert
            Assert.ThrowsException<SecurityException>(() => {
                usuario.ValidarContraseña(passIncorrecta);
            });
        }

        // VerificarContraseña_ContraseñaCorrecta_DevuelveTrue
        [TestMethod]
        public void VerificarContraseña_ContraseñaCorrecta_DevuelveTrue() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Act
            bool resultado = usuario.VerificarContraseña(contraseñaValida);

            // Assert
            Assert.IsTrue(resultado);
        }

        // VerificarContraseña_ContraseñaIncorrecta_DevuelveFalse
        [TestMethod]
        public void VerificarContraseña_ContraseñaIncorrecta_DevuelveFalse() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Act
            bool resultado = usuario.VerificarContraseña("User123!");

            // Assert
            Assert.IsFalse(resultado);
        }

        // CambiarContraseña_ContraseñaValida_ActualizaCorrectamente
        [TestMethod]
        public void CambiarContraseña_ContraseñaValida_ActualizaCorrectamente() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);
            string nuevaPassword = "NuevaContraseña123!";

            // Act
            bool resultado = usuario.CambiarContraseña(nuevaPassword, contraseñaValida);

            // Assert
            Assert.IsTrue(resultado, "CambiarContraseña debió devolver 'true'");
            Assert.IsTrue(usuario.VerificarContraseña(nuevaPassword), "La nueva contraseña no se verificó correctamente.");
            Assert.IsFalse(usuario.VerificarContraseña(contraseñaValida), "La contraseña antigua sigue funcionando.");
        }

        // CambiarContraseña_ContraseñaNulaOVacia_LanzaArgumentException
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CambiarContraseña_ContraseñaNulaOVacia_LanzaArgumentException() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Act
            usuario.CambiarContraseña(null, contraseñaValida);

            // Assert - [ExpectedException] se encarga
        }

        // CambiarContraseña_ContraseñaActualIncorrecta_DevuelveFalse
        [TestMethod]
        public void CambiarContraseña_ContraseñaActualIncorrecta_DevuelveFalse() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);
            string nuevaPassword = "NuevaPassSuperSegura456!";

            // Act
            bool resultado = usuario.CambiarContraseña(nuevaPassword, "ContraseñaEquivocadaActual123!");

            // Assert
            Assert.IsFalse(resultado, "CambiarContraseña debió devolver 'false'");
            Assert.IsTrue(usuario.VerificarContraseña(contraseñaValida), "La contraseña original debe seguir funcionando.");
        }

        // CambiarContraseña_NuevaPassNoCumplePolitica_LanzaSecurityException
        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void CambiarContraseña_NuevaPassNoCumplePolitica_LanzaSecurityException() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Act
            usuario.CambiarContraseña("User123!", contraseñaValida);

            // Assert - [ExpectedException] se encarga
        }

        #endregion

        #region Pruebas de Email

        // ValidarEmail_EmailCorrecta_DevuelveTrue
        [TestMethod]
        public void ValidarEmail_EmailCorrecta_DevuelveTrue() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Act
            bool resultado = usuario.ValidarEmail("example@example.com");

            // Assert
            Assert.IsTrue(resultado);
        }

        // ValidarEmail_EmailIncorrecta_LanzaFormatException
        [DataTestMethod]
        [DataRow(null, "Un email nulo debe ser inválido")]
        [DataRow("", "Un email vacío debe ser inválido")]
        [DataRow("   ", "Un email con solo espacios debe ser inválido")]
        [DataRow("username @ domain.com", "Un email con espacios intermedios debe ser inválido")]
        [DataRow(" test@test.com", "Un email con espacios al inicio debe ser inválido")]
        [DataRow("test@test.com ", "Un email con espacios al final debe ser inválido")]
        [DataRow("plainaddress", "No contiene '@'")]
        [DataRow("test@.com", "Dominio empieza con punto (inválido)")]
        [DataRow("test@domain.com.", "Dominio termina con punto (inválido)")]
        [DataRow("test@domain..com", "Dominio contiene puntos consecutivos (inválido)")]
        [DataRow("test@domain.c", "TLD es menor de 2 caracteres (inválido)")]
        [DataRow("@missinglocalpart.com", "Falta la parte local antes de '@'")]
        [DataRow("global@", "Falta el dominio después de '@'")]
        [DataRow("test@@example.com", "Contiene múltiples '@'")]
        [DataRow("test@localhost", "Dominio sin TLD (inválido)")]
        public void ValidarEmail_EmailIncorrecta_LanzaFormatException(string emailIncorrecto, string descripcionCaso) {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Act & Assert
            Assert.ThrowsException<FormatException>(() => {
                usuario.ValidarEmail(emailIncorrecto);
            }, descripcionCaso);
        }

        #endregion

        #region Pruebas de ActualizarDatosPersonales

        // Caso de prueba: ActualizarDatos_DatosValidos_ActualizaPropiedades
        [TestMethod]
        public void ActualizarDatos_DatosValidos_ActualizaPropiedades() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);
            string nuevoNombre = "Usuario2";
            string nuevosApellidos = "Diez Normal";
            string nuevoEmail = "example2@example2.com";

            // Act
            usuario.ActualizarDatosPersonales(nuevoNombre, nuevosApellidos, nuevoEmail);

            // Assert
            Assert.AreEqual(nuevoNombre, usuario.Nombre);
            Assert.AreEqual(nuevosApellidos, usuario.Apellidos);
            Assert.AreEqual(nuevoEmail, usuario.Email);
        }

        // ActualizarDatos_NuloOVacio_LanzaArgumentException
        [DataTestMethod]
        [DataRow(null, "Apellidos", "email@test.com", "El parametro del nuevo Nombre no puede ser Vacio/Nulo")]
        [DataRow("", "Apellidos", "email@test.com", "El parametro del nuevo Nombre no puede ser Vacio/Nulo")]
        [DataRow(" ", "Apellidos", "email@test.com", "El parametro del nuevo Nombre no puede ser Vacio/Nulo")]
        [DataRow("Nombre", null, "email@test.com", "El parametro del nuevo Apellidos no puede ser Vacio/Nulo")]
        [DataRow("Nombre", "", "email@test.com", "El parametro del nuevo Apellidos no puede ser Vacio/Nulo")]
        [DataRow("Nombre", " ", "email@test.com", "El parametro del nuevo Apellidos no puede ser Vacio/Nulo")]
        [DataRow("Nombre", "Apellidos", null, "El parametro del nuevo Email no puede ser Vacio/Nulo")]
        [DataRow("Nombre", "Apellidos", "", "El parametro del nuevo Email no puede ser Vacio/Nulo")]
        [DataRow("Nombre", "Apellidos", " ", "El parametro del nuevo Email no puede ser Vacio/Nulo")]
        public void ActualizarDatos_NuloOVacio_LanzaArgumentException(string nombre, string apellidos, string email, string expectedParamName) {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => {
                usuario.ActualizarDatosPersonales(nombre, apellidos, email);
            });

            // Verificar el mensaje de la excepción.
            Assert.AreEqual(expectedParamName, ex.Message);
        }

        // ActualizarDatos_EmailIncorrecta_LanzaFormatException
        [DataTestMethod]
        [DataRow("Nombre", "Apellidos", "email-invalido", "El formato del nuevo Email no es valido")]
        public void ActualizarDatos_EmailIncorrecta_LanzaFormatException(string nombre, string apellidos, string email, string expectedParamName) {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Act & Assert
            Assert.ThrowsException<FormatException>(() => {
                usuario.ActualizarDatosPersonales(nombre, apellidos, email);
            }, expectedParamName);
        }

        #endregion

        #region Pruebas de Estado de Cuenta (Activo, Expirado)

        // ActivarUsuario_CambiaPropiedadActivoATrue
        [TestMethod]
        public void ActivarUsuario_CambiaPropiedadActivoATrue() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);
            Assert.IsFalse(usuario.Activo, "El usuario no de inicializar como inactivo");

            // Act
            usuario.ActivarUsuario();

            // Assert
            Assert.IsTrue(usuario.Activo);
        }

        // DesactivarUsuario_CambiaPropiedadActivoAFalse
        [TestMethod]
        public void DesactivarUsuario_CambiaPropiedadActivoAFalse() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);
            usuario.ActivarUsuario(); // Activamos al Usuario
            Assert.IsTrue(usuario.Activo, "El usuario no se activó la funcion ActivarUsuario debe estar fallando");

            // Act
            usuario.DesactivarUsuario();

            // Assert
            Assert.IsFalse(usuario.Activo);
        }

        // HaExpirado_ConFechaFutura_DevuelveFalse
        [TestMethod]
        public void HaExpirado_ConFechaFutura_DevuelveFalse() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);

            // Act
            bool resultado = usuario.HaExpirado();

            // Assert
            Assert.IsFalse(resultado);
        }

        // HaExpirado_ConFechaPasada_DevuelveTrue
        [TestMethod]
        public void HaExpirado_ConFechaPasada_DevuelveTrue() {
            // Arrange
            var usuario = new Usuario(usuarioValido, nombreValido, apellidosValidos, contraseñaValida, EmailValido);
            DateTime fechaPasada = DateTime.Now.AddDays(-1);
            usuario.ActualizarExpiracion(fechaPasada);

            // Act
            bool resultado = usuario.HaExpirado();

            // Assert
            Assert.IsTrue(resultado);
        }

        #endregion

    }
}
