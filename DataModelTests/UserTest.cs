using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
// Asegúrate de tener también el 'using' para el proyecto donde está tu clase Usuario.
// Ejemplo: using MiProyecto.Modelos; 

namespace DataModelTests {
    [TestClass]
    public class UserTest {
        [TestMethod]
        public void ConstructorTest() {
            // Arrange: Preparamos los datos de la prueba
            int id = 0;
            string nombre = "Pedro";
            string apellidos = "Luis Miguel";
            string direccionPostal = "C\\ Casa, 09007";
            string email = "example@example.com";
            string contrasena = "admin1234";

            DateTime hoy = DateTime.Now;

            // Act: Ejecutamos el código que queremos probar
            Usuario u = new Usuario(id, nombre, apellidos, direccionPostal, email, contrasena);

            // Assert: Verificamos que los resultados son los esperados
            Assert.IsNotNull(u);
            Assert.AreEqual(id, u.ID);
            Assert.AreEqual(nombre, u.Nombre);
            Assert.AreEqual(apellidos, u.Apellidos);
            Assert.AreEqual(email, u.Email);
            Assert.IsTrue(u.CuentaActiva);
            Assert.IsTrue(u.comorobarContraseña(contrasena));
            Assert.AreEqual(DateTime.MinValue, u.UltimoAcceso);


            Assert.AreEqual(hoy.AddDays(365), u.FechaCaducidadCuenta());
            Assert.AreEqual(hoy.AddDays(365), u.FechaCaducidadCuenta(365));
        }
    }
}