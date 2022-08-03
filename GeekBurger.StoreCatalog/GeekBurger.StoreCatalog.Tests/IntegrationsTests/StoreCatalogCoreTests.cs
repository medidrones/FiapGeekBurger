using GeekBurger.StoreCatalog.Core;
using GeekBurger.StoreCatalog.Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeekBurger.StoreCatalog.Tests.IntegrationsTests
{
    [TestClass]
    public class StoreCatalogCoreTests
    {
        private IStoreCatalogCore _storeCatalogCore;

        [TestInitialize]
        public void Setup()
        {
            _storeCatalogCore = new StoreCatalogCore();
        }

        [TestMethod]
        public void Check_If_Others_Services_Are_Available()
        {
            var result = _storeCatalogCore.StatusServers();

            Assert.IsTrue(result);
        }
    }
}
