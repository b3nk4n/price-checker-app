using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using AmaScan.Common.Tools;
using UWPCore.Framework.Storage;
using System.Threading.Tasks;

namespace AmaScan.UnitTests.Tools
{
    [TestClass]
    public class AmazonHtmlToolsTest
    {
        private IStorageService StorageService = new LocalStorageService();

        #region Camera

        [TestMethod]
        public async Task TestCameraExtractProductTitle()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/camera.html");
            var htmlContent = await StorageService.ReadFile(file);
            var title = AmazonHtmlTools.ExtractTextFromHtmlInDetailPage(htmlContent, "productTitle");

            Assert.AreEqual("Sony Alpha 6000 Systemkamera (24 Megapixel, 7,6 cm (3\") LCD-Display, Exmor APS-C Sensor, Full-HD, High Speed Hybrid AF) inkl. SEL-P1650 Objektiv schwarz", title);
        }

        [TestMethod]
        public async Task TestCameraExtractImageUri()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/camera.html");
            var htmlContent = await StorageService.ReadFile(file);
            var uri = AmazonHtmlTools.ExtractImageUriFromHtmlInDetailPage(htmlContent);

            Assert.AreEqual("https://images-na.ssl-images-amazon.com/images/I/518a97hQRvL._SX355_.jpg", uri.AbsoluteUri);
        }

        #endregion

        #region Accu

        [TestMethod]
        public async Task TestAccuExtractProductTitle()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/accu.html");
            var htmlContent = await StorageService.ReadFile(file);
            var title = AmazonHtmlTools.ExtractTextFromHtmlInDetailPage(htmlContent, "productTitle");

            Assert.AreEqual("Lenovo Think Pad Battery (9 Cell)", title);
        }

        [TestMethod]
        public async Task TestAccuExtractImageUri()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/accu.html");
            var htmlContent = await StorageService.ReadFile(file);
            var uri = AmazonHtmlTools.ExtractImageUriFromHtmlInDetailPage(htmlContent);

            Assert.AreEqual("https://images-na.ssl-images-amazon.com/images/I/31s%2BtYHUYpL._SX425_.jpg", uri.AbsoluteUri);
        }

        #endregion

        #region ZeitWissen

        [TestMethod]
        public async Task TestZeitWissenExtractProductTitle()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/zeitwissen.html");
            var htmlContent = await StorageService.ReadFile(file);
            var title = AmazonHtmlTools.ExtractTextFromHtml(htmlContent, "productTitle");

            Assert.AreEqual("Die Zeit Wissen - Woolen wir zu Perfekt sein", title);
        }

        [TestMethod]
        public async Task TestZeitWissenExtractImageUri()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/zeitwissen.html");
            var htmlContent = await StorageService.ReadFile(file);
            var uri = AmazonHtmlTools.ExtractImageUriFromHtml(htmlContent);

            Assert.AreEqual("https://images-eu.ssl-images-amazon.com/images/I/61mLbN-4bkL._AC_US174_.jpg", uri.AbsoluteUri);
        }

        #endregion
    }
}
