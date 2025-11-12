using Microsoft.VisualStudio.TestTools.UnitTesting;
using Practica.Model;
using System;
using System.Collections.Generic;
using Assert; 

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

}