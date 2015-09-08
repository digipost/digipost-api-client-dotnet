using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.CompareObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class SmsNotificationTests
    {
        Comparator _comparator = new Comparator();

        [TestClass]
        public class ConstructorMethod : SmsNotificationTests {
            
            [TestMethod]
            public void WithSendingTime()
            {
                //Arrange
                var firstSmsNotification = DateTime.Today;
                var secondSmsNotification = DateTime.Today.AddDays(1);
                var expected = new List<DateTime>{firstSmsNotification, secondSmsNotification};
                ISmsNotification smsNotification = new SmsNotification(expected.ToArray());

                //Act
                var actual = smsNotification.NotifyAtTimes;

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0,differences.Count());

            }

            [TestMethod]
            public void WithAfterHours()
            {
                //Arrange
                var firstSmsNotification = 2;
                var secondSmsNotification = 5;
                var expected = new List<int> { firstSmsNotification, secondSmsNotification };
                ISmsNotification smsNotification = new SmsNotification(expected.ToArray()); 
               
                //Act
                var actual = smsNotification.NotifyAfterHours;

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());

            }
        }
    }
}
