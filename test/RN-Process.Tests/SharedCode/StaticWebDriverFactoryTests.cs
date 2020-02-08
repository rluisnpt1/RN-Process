using FluentAssertions;
using OpenQA.Selenium;
using RN_Process.Tests.WebDriver;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RN_Process.Tests
{
    //[TestFixture]
    public class StaticWebDriverFactoryTests //: IDisposable
    {
        private IWebDriver Driver { get; set; }
        private readonly PlatformType thisPlatformType = PlatformType.Windows;

        public StaticWebDriverFactoryTests()
        {
            Platform.CurrentPlatform.IsPlatformType(thisPlatformType);
        }

        //[OneTimeSetUp]
        //public void CheckForValidPlatform()
        //{
        //    Assume.That(() => Platform.CurrentPlatform.IsPlatformType(thisPlatformType));
        //}

        //[Test]
        //[TestCase(Browser.Firefox)]
        //[TestCase(Browser.InternetExplorer)]
        //[TestCase(Browser.Edge)]
        //[TestCase(Browser.Chrome)]
        //[Fact]
        //public void LocalWebDriverCanBeLaunchedAndLoadExampleDotCom()
        //{

        //    Driver = StaticWebDriverFactory.GetLocalWebDriver(Browser.Chrome);
        //    Driver.Url = "https://example.com/";
        //    Driver.Title.Should().Be("Example Domain");
        //}

        //public void Dispose()
        //{
        //    Driver?.Quit();
        //}

        //[Test]
        //[TestCase(Browser.Safari)]
        //public void RequestingUnsupportedWebDriverThrowsInformativeException(Browser browser)
        //{
        //    Action act = () => StaticWebDriverFactory.GetLocalWebDriver(browser);
        //    act.Should()
        //        .Throw<PlatformNotSupportedException>($"because {browser} is not supported on {thisPlatformType}.")
        //        .WithMessage("*is only available on*");
        //}

        //[Test]
        //[TestCase(Browser.Firefox)]
        //[TestCase(Browser.Chrome)]
        //public void HeadlessBrowsersCanBeLaunched(Browser browser)
        //{
        //    Driver = StaticWebDriverFactory.GetLocalWebDriver(browser, true);
        //    Driver.Url = "https://example.com/";
        //    Driver.Title.Should().Be("Example Domain");
        //}

        //[Test]
        //[TestCase(Browser.Edge)]
        //[TestCase(Browser.InternetExplorer)]
        //[TestCase(Browser.Safari)]
        //public void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        //{
        //    Action act = () => StaticWebDriverFactory.GetLocalWebDriver(browser, true);
        //    act.Should()
        //        .ThrowExactly<ArgumentException>($"because headless mode is not supported on {browser}.")
        //        .WithMessage($"Headless mode is not currently supported for {browser}.");
        //}

        //[Test]
        //public void HdBrowserIsOfRequestedSize()
        //{
        //    Driver = StaticWebDriverFactory.GetLocalWebDriver(StaticDriverOptionsFactory.GetFirefoxOptions(true), WindowSize.Hd);

        //    using (new AssertionScope())
        //    {
        //        Size size = Driver.Manage().Window.Size;
        //        size.Width.Should().Be(1366);
        //        size.Height.Should().Be(768);
        //    };
        //}

        //[Test]
        //public void FhdBrowserIsOfRequestedSize()
        //{
        //    Driver = StaticWebDriverFactory.GetLocalWebDriver(StaticDriverOptionsFactory.GetFirefoxOptions(true), WindowSize.Fhd);

        //    using (new AssertionScope())
        //    {
        //        Size size = Driver.Manage().Window.Size;
        //        size.Height.Should().Be(1080);
        //        size.Width.Should().Be(1920);
        //    };
        //}

        //[Test]
        //public void Uhd4KBrowserIsOfRequestedSize()
        //{
        //    Driver = StaticWebDriverFactory.GetLocalWebDriver(StaticDriverOptionsFactory.GetFirefoxOptions(true), WindowSize.Uhd4K);

        //    using (new AssertionScope())
        //    {
        //        Size size = Driver.Manage().Window.Size;
        //        size.Height.Should().Be(2160);
        //        size.Width.Should().Be(3840);
        //    };
        //}

        //[Test]
        //[TestCase(Browser.Chrome, PlatformType.Windows)]
        //[TestCase(Browser.Edge, PlatformType.Windows)]
        //[TestCase(Browser.Firefox, PlatformType.Windows)]
        //[TestCase(Browser.InternetExplorer, PlatformType.Windows)]
        //[TestCase(Browser.Chrome, PlatformType.Linux)]
        //[TestCase(Browser.Firefox, PlatformType.Linux)]
        //public void RemoteWebDriverCanBeLaunched(Browser browser, PlatformType platformType)
        //{
        //    Driver = StaticWebDriverFactory.GetRemoteWebDriver(browser, new Uri("http://192.168.0.200:4444/wd/hub"), platformType);
        //    Driver.Url = "https://example.com/";
        //    Driver.Title.Should().Be("Example Domain");
        //}


        //[TearDown]
        //public void Teardown()
        //{
        //Driver?.Quit();
        //}
    }
}
