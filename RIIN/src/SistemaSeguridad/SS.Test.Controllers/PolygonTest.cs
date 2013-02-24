using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaSeguridad.Controllers;
using SS.Core.Entities;

namespace SS.Test.Controllers
{
    /// <summary>
    /// Summary description for IncidentesControllerTest
    /// </summary>
    [TestClass]
    public class PolygonTest
    {
        public PolygonTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetZoneForIncidentTest()
        {
            var point = new PoligonoDetalle { Latitud = float.Parse("25.721972"), Longitud = float.Parse("-100.237198") };
            var expectedZonaId = 2;
            var zona = GetZoneForIncident(point);
            Assert.IsNotNull(zona);
            Assert.AreEqual(expectedZonaId, zona.Id);
        }

        [TestMethod]
        public void NotZoneFoundForIncident()
        {
            var point = new PoligonoDetalle { Latitud = float.Parse("25.690419"), Longitud = float.Parse("-100.232048") };
            var zona = GetZoneForIncident(point);
            Assert.IsNull(zona);
        }

        public Zona GetZoneForIncident(PoligonoDetalle point)
        {
            var db = new RiinContainer();
            var result = false;
            Zona zone = null;
            var Zonas = db.Estados.First(e => e.Id == 2).Zonas.Where(z => z.Poligonos.PoligonoDetalles.Count > 0);
            foreach (var zona in Zonas)
            {
                var polygon = zona.Poligonos.PoligonoDetalles.ToList();//.Select(p => new Coordinate { Latitude = p.Latitud, Longitude = p.Longitud }).ToList();

                Assert.IsTrue(polygon.Count > 0, "Polygon should have coordinates");

                result = IsPointInPolygon(polygon, point);
                if (result)
                {
                    zone = zona;
                    break;
                }

            }
            return zone;
        }

        static bool IsPointInPolygon(IList<PoligonoDetalle> polygon, PoligonoDetalle point)
        {
            int i, j;
            var c = false;
            for (i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
            {
                if ((((polygon[i].Latitud <= point.Latitud) && (point.Latitud < polygon[j].Latitud)) ||
                    ((polygon[j].Latitud <= point.Latitud) && (point.Latitud < polygon[i].Latitud))) &&
                    (point.Longitud < (polygon[j].Longitud - polygon[i].Longitud) * (point.Latitud - polygon[i].Latitud) / (polygon[j].Latitud - polygon[i].Latitud) + polygon[i].Longitud))
                    c = !c;
            }
            return c;
        }
    }
}
