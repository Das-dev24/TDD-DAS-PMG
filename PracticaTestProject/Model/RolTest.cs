using Microsoft.VisualStudio.TestTools.UnitTesting;
using Practica.Model;
using System;
using System.Collections.Generic;
using Assert;

namespace PracticaTestProject.Model
{
    [TestClass]
    public class RolTests
    {
        #region Pruebas del Constructor

        [TestMethod]
        public void Constructor_DatosValidos_AsignaPropiedadesCorrectamente()
        {
            string nombre = "Admin";
            string descripcion = "Rol de administrador";

            Rol rol = new Rol(nombre, descripcion);

            Assert.AreEqual(nombre, rol.Nombre);
            Assert.AreEqual(descripcion, rol.Descripcion);
        }
        [TestMethod]
        public void Constructor_DatosValidos_InicializaHashSetPermisosVacio()
        {
            string nombre = "Editor";
            string descripcion = "Rol de editor";

            Rol rol = new Rol(nombre, descripcion);

            Assert.IsNotNull(rol.ObtenerPermisos());
            Assert.AreEqual(0, rol.ObtenerPermisos().Count);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void Constructor_NombreNuloOVacio_LanzaArgumentException(string nombre)
        {
            string descripcion = "Descripción válida";

            Assert.ThrowsException<ArgumentException>(() => new Rol(nombre, descripcion));
        }

        #region Pruebas del Método AgregarPermiso
        [TestMethod]
        public void AgregarPermiso_PermisoValido_AñadePermisoAHashSet()
        {
            Rol rol = new Rol("Tester", "Rol de pruebas");
            string permiso = "ejecutar_pruebas";

            rol.AgregarPermiso(permiso);

            Assert.AreEqual(1, rol.ObtenerPermisos().Count);
            Assert.IsTrue(rol.TienePermiso(permiso));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void AgregarPermiso_PermisoNuloOVacio_LanzaArgumentException(string permiso)
        {
            Rol rol = new Rol("Guest", "Rol de invitado");

            Assert.ThrowsException<ArgumentException>(() => rol.AgregarPermiso(permiso));
        }

        [TestMethod]
        public void AgregarPermiso_PermisoDuplicado_HashSetIgnoraDuplicadoSinExcepcion()
        {
            Rol rol = new Rol("Developer", "Rol de desarrollador");
            string permiso = "escribir_codigo";
            rol.AgregarPermiso(permiso);

            rol.AgregarPermiso(permiso);


            Assert.AreEqual(1, rol.ObtenerPermisos().Count);
            Assert.IsTrue(rol.TienePermiso(permiso));
        }



    }
}