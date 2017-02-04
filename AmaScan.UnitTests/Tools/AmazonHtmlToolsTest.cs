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
            var title = AmazonHtmlTools.ExtractTextFromHtml(htmlContent);

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

        #region ZeitWissen Mobile

        [TestMethod]
        public async Task TestZeitWissenMobileExtractProductTitle()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/zeitwissen-mobile.html");
            var htmlContent = await StorageService.ReadFile(file);
            var title = AmazonHtmlTools.ExtractTextFromHtml(htmlContent);

            Assert.AreEqual("Die Zeit Wissen - Woolen wir zu Perfekt sein", title);
        }

        [TestMethod]
        public async Task TestZeitWissenMobileExtractImageUri()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/zeitwissen.html");
            var htmlContent = await StorageService.ReadFile(file);
            var uri = AmazonHtmlTools.ExtractImageUriFromHtml(htmlContent);

            Assert.AreEqual("https://images-eu.ssl-images-amazon.com/images/I/61mLbN-4bkL._AC_US174_.jpg", uri.AbsoluteUri);
        }

        #endregion

        #region TomTaylor

        [TestMethod]
        public async Task TestTomTaylorExtractProductTitle()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/tomtaylor.html");
            var htmlContent = await StorageService.ReadFile(file);
            var title = AmazonHtmlTools.ExtractTextFromHtml(htmlContent);

            Assert.AreEqual("TOM TAILOR Herren Freizeithemd Floyd Indigo Look Stripe Shirt", title);
        }

        [TestMethod]
        public async Task TestTomTaylorExtractImageUri()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/tomtaylor.html");
            var htmlContent = await StorageService.ReadFile(file);
            var uri = AmazonHtmlTools.ExtractImageUriFromHtml(htmlContent);

            Assert.AreEqual("https://images-eu.ssl-images-amazon.com/images/I/51LFoT9DwqL._AC_US174_.jpg", uri.AbsoluteUri);
        }

        #endregion

        #region TomTaylor Mobile

        [TestMethod]
        public async Task TestTomTaylorMobileExtractProductTitle()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/tomtaylor-mobile.html");
            var htmlContent = await StorageService.ReadFile(file);
            var title = AmazonHtmlTools.ExtractTextFromHtml(htmlContent);

            Assert.AreEqual("TOM TAILOR Herren Freizeithemd Floyd Indigo Look Stripe Shirt", title);
        }

        [TestMethod]
        public async Task TestTomTaylorMobileExtractImageUri()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/tomtaylor-mobile.html");
            var htmlContent = await StorageService.ReadFile(file);
            var uri = AmazonHtmlTools.ExtractImageUriFromHtml(htmlContent);

            Assert.AreEqual("https://images-eu.ssl-images-amazon.com/images/I/51LFoT9DwqL._AC_SX354_SY510_QL65_.jpg", uri.AbsoluteUri);
        }

        #endregion

        #region Shimano

        [TestMethod]
        public async Task TestShimanoExtractProductTitle()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/shimano.html");
            var htmlContent = await StorageService.ReadFile(file);
            var title = AmazonHtmlTools.ExtractTextFromHtml(htmlContent);

            Assert.AreEqual("Shimano Cycling Cassette CS-HG20-7 - 587632", title);
        }

        [TestMethod]
        public async Task TestShimanoExtractImageUri()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/shimano.html");
            var htmlContent = await StorageService.ReadFile(file);
            var uri = AmazonHtmlTools.ExtractImageUriFromHtml(htmlContent);

            Assert.AreEqual("https://images-na.ssl-images-amazon.com/images/I/51S4nyZB2qL._AC_US160_.jpg", uri.AbsoluteUri);
        }

        #endregion

        #region Shimano Mobile

        [TestMethod]
        public async Task TestShimanoMobileExtractProductTitle()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/shimano-mobile.html");
            var htmlContent = await StorageService.ReadFile(file);
            var title = AmazonHtmlTools.ExtractTextFromHtml(htmlContent);

            Assert.AreEqual("Shimano 7 Speed Cassette CS-HG20-7 (Brown/ Black) by Shimano", title);
        }

        [TestMethod]
        public async Task TestShimanoMobileExtractImageUri()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/shimano-mobile.html");
            var htmlContent = await StorageService.ReadFile(file);
            var uri = AmazonHtmlTools.ExtractImageUriFromHtml(htmlContent);

            Assert.AreEqual("https://images-eu.ssl-images-amazon.com/images/I/51S4nyZB2qL._AC_SX354_SY510_QL65_.jpg", uri.AbsoluteUri);
        }

        #endregion

        #region Mainboard

        [TestMethod]
        public async Task TestMainboardMobileExtractProductTitle()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/mainboard-mobile.html");
            var htmlContent = await StorageService.ReadFile(file);
            var title = AmazonHtmlTools.ExtractTextFromHtml(htmlContent);

            Assert.AreEqual("Gigabyte GA-965P-DS4 Rev 3.3 Mainboard", title);
        }

        [TestMethod]
        public async Task TestMainboardMobileExtractImageUri()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/mainboard-mobile.html");
            var htmlContent = await StorageService.ReadFile(file);
            var uri = AmazonHtmlTools.ExtractImageUriFromHtml(htmlContent);

            Assert.AreEqual("https://images-eu.ssl-images-amazon.com/images/I/41JUH-q9BwL._AC_SX118_SY170_QL70_.jpg", uri.AbsoluteUri);
        }

        #endregion

        #region Not Found

        [TestMethod]
        public async Task TestNotFoundExtractProductTitle()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/not-found.html");
            var htmlContent = await StorageService.ReadFile(file);
            var title = AmazonHtmlTools.ExtractTextFromHtml(htmlContent);

            Assert.AreEqual(string.Empty, title);
        }

        [TestMethod]
        public async Task TestNotFoundExtractImageUri()
        {
            var file = await StorageService.GetFileFromApplicationAsync("/Assets/Html/not-found.html");
            var htmlContent = await StorageService.ReadFile(file);
            var uri = AmazonHtmlTools.ExtractImageUriFromHtml(htmlContent);

            Assert.AreEqual(AmazonHtmlTools.FALLBACK_IMAGE, uri.OriginalString);
        }

        #endregion
    }
}
