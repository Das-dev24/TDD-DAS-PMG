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

        #endregion

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


        #endregion

        #region Pruebas del Método QuitarPermiso

        [TestMethod]
        public void QuitarPermiso_PermisoExistente_EliminaPermisoDeHashSet()
        {
            Rol rol = new Rol("Manager", "Rol de gerente");
            string permiso = "aprobar_requisitos";
            rol.AgregarPermiso(permiso);
            Assert.AreEqual(1, rol.ObtenerPermisos().Count, "Pre-condición fallida: El permiso no se agregó.");

            rol.QuitarPermiso(permiso);

            Assert.AreEqual(0, rol.ObtenerPermisos().Count);
            Assert.IsFalse(rol.TienePermiso(permiso));
        }

        [TestMethod]
        public void QuitarPermiso_PermisoNoExistente_NoLanzaExcepcion()
        {

            Rol rol = new Rol("Manager", "Rol de gerente");
            string permisoExistente = "otro_permiso";
            string permisoNoExistente = "permiso_que_no_existe";
            rol.AgregarPermiso(permisoExistente);

            rol.QuitarPermiso(permisoNoExistente);

            Assert.AreEqual(1, rol.ObtenerPermisos().Count);
            Assert.IsFalse(rol.TienePermiso(permisoNoExistente));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void QuitarPermiso_PermisoNuloOVacio_LanzaArgumentException(string permiso)
        {
            Rol rol = new Rol("Observer", "Rol de observador");
            Assert.ThrowsException<ArgumentException>(() => rol.QuitarPermiso(permiso));
        }
        #endregion


        #region Pruebas de ObtenerPermisos y TienePermiso

        [TestMethod]
        public void ObtenerPermisos_ConVariosPermisos_DevuelveHashSetCorrecto()
        {
            Rol rol = new Rol("SuperAdmin", "Admin total");
            string p1 = "permiso_A";
            string p2 = "permiso_B";
            rol.AgregarPermiso(p1);
            rol.AgregarPermiso(p2);

            HashSet<string> permisos = rol.ObtenerPermisos();

            Assert.IsNotNull(permisos);
            Assert.AreEqual(2, permisos.Count);
            Assert.IsTrue(permisos.Contains(p1));
            Assert.IsTrue(permisos.Contains(p2));
        }

        [TestMethod]
        public void ObtenerPermisos_SinPermisos_DevuelveHashSetVacio()
        {
            Rol rol = new Rol("SuperAdmin", "Admin total");

            HashSet<string> permisos = rol.ObtenerPermisos();

            Assert.IsNotNull(permisos);
            Assert.AreEqual(0, permisos.Count);
        }

        [TestMethod]
        public void ObtenerPermisos_DevuelveCopia_ModificarCopiaNoAfectaOriginal()
        {
            Rol rol = new Rol("Auditor", "Rol de auditor");
            string p1 = "leer_logs";
            rol.AgregarPermiso(p1);

            HashSet<string> copiaPermisos = rol.ObtenerPermisos();
            copiaPermisos.Add("modificar_logs");

            Assert.AreEqual(2, copiaPermisos.Count);

            Assert.AreEqual(1, rol.ObtenerPermisos().Count);
            Assert.IsTrue(rol.TienePermiso(p1));
            Assert.IsFalse(rol.TienePermiso("modificar_logs"));
        }

        [TestMethod]
        public void TienePermiso_PermisoExiste_DevuelveTrue()
        {
            Rol rol = new Rol("SuperAdmin", "Admin total");
            string p1 = "permiso_A";
            rol.AgregarPermiso(p1);

            bool tiene = rol.TienePermiso(p1);

            Assert.IsTrue(tiene);
        }

        [TestMethod]
        public void TienePermiso_PermisoNoExiste_DevuelveFalse()
        {
            Rol rol = new Rol("SuperAdmin", "Admin total");
            rol.AgregarPermiso("permiso_A");

            bool tiene = rol.TienePermiso("permiso_B_no_existe");

            Assert.IsFalse(tiene);
        }


    }
}