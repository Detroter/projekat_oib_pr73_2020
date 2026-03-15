using NUnit.Framework;
using Autoservis.Model;
using System;

namespace Autoservis.Tests
{
    public class ReceiptTest
    {
        [Test]
        public void ReceiptProperties()
        {
            var receipt = new Receipt("Marko", DateTime.Now, 1500);

            Assert.That(receipt.MechanicName, Is.EqualTo("Marko"));
            Assert.That(receipt.Total, Is.GreaterThan(0));
        }
    }
}