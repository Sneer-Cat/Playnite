﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Playnite;
using PlayniteUI;
using System.Globalization;
using System.Windows;
using System.IO;
using System.Net;

namespace PlayniteUITests.Converters
{
    [TestFixture]
    public class ImageUrlToImageSourceConverterTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            FileSystem.DeleteFolder(Paths.ImagesCachePath);
        }

        [Test]
        public void ConvertTest()
        {
            var convererter = new ImageUrlToImageSourceConverter();

            Assert.AreEqual(DependencyProperty.UnsetValue, convererter.Convert(null, typeof(string), null, CultureInfo.InvariantCulture));

            var cachePath = (string)convererter.Convert(@"https://steamcdn-a.akamaihd.net/steam/apps/108710/page_bg_generated_v6b.jpg", typeof(string), null, CultureInfo.InvariantCulture);
            Assert.IsTrue(!string.IsNullOrEmpty(cachePath));
            Assert.IsTrue(File.Exists(cachePath));

            // 404 shouldn't throw
            Assert.AreEqual(DependencyProperty.UnsetValue, convererter.Convert(@"https://steamcdn-a.akamaihd.net/steam/apps/108710/dasdasd.jpg", typeof(string), null, CultureInfo.InvariantCulture));
        }

        [Test]
        public void ConvertInvalidTest()
        {
            var convererter = new ImageUrlToImageSourceConverter();
            Assert.Throws<WebException>(() => convererter.Convert(@"http://totaly.made.up.url/that/doesnt/exists.jpg", typeof(string), null, CultureInfo.InvariantCulture));
        }
    }
}
