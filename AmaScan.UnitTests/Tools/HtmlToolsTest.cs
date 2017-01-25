using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using AmaScan.Common.Tools;
using UWPCore.Framework.Storage;
using System.Threading.Tasks;

namespace AmaScan.UnitTests.Tools
{
    [TestClass]
    public class HtmlToolsTest
    {
        private IStorageService StorageService = new LocalStorageService();

        [TestMethod]
        public async Task TestExtractProductTitle()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/camera.html");
            var htmlContent = await StorageService.ReadFile(file);
            var title = HtmlTools.ExtractTextFromHtml(htmlContent, "productTitle");

            Assert.AreEqual("Sony Alpha 6000 Systemkamera (24 Megapixel, 7,6 cm (3\") LCD-Display, Exmor APS-C Sensor, Full-HD, High Speed Hybrid AF) inkl. SEL-P1650 Objektiv schwarz", title);
        }

        [TestMethod]
        public async Task TestExtractImageUri()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/camera.html");
            var htmlContent = await StorageService.ReadFile(file);
            var uri = HtmlTools.ExtractImageUriFromHtml(htmlContent);

            Assert.AreEqual("https://images-eu.ssl-images-amazon.com/images/I/71%2BBSM8Af7L._SY355_.jpg", uri.AbsoluteUri);
        }
    }
}
