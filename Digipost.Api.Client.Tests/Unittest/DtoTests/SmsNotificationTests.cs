using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Domain.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class SmsNotificationTests
    {
        [TestClass]
        public class ConstructorMethod : SmsNotificationTests {

            [TestMethod]
            public void AddAfterHoursUpdatesSmsNotifcations()
            {
                //Arrange
                var firstSmsNotification = 2;
                var secondSmsNotification = 5;

                ISmsNotification smsNotification = new SmsNotification(firstSmsNotification);

                //Act
                smsNotification.AddAfterHours(secondSmsNotification);

                //Assert
                Assert.AreEqual(2, smsNotification.AfterHours.Count, "Should be 2 occurences of hours after");

                var hasFirstNotification = smsNotification.AfterHours.Contains(firstSmsNotification);
                var hasSecondNotification = smsNotification.AfterHours.Contains(secondSmsNotification);
                
                Assert.IsTrue(hasFirstNotification,"first smsNotification is missing");
                Assert.IsTrue(hasSecondNotification, "second smsNotification is missing");

            }

            [TestMethod]
            public void AddAtTimeUpdatesSmsNotifications()
            {
                //Arrange
                var firstSmsNotification = DateTime.Today;
                var secondSmsNotification = DateTime.Today.AddDays(1);

                ISmsNotification smsNotification = new SmsNotification(firstSmsNotification);

                //Act
                smsNotification.AddAtTime(secondSmsNotification);

                //Assert
                Assert.AreEqual(2, smsNotification.AtTime.Count, "Should be 2 occurences of atTime");
                
                foreach (var dateTime in smsNotification.AtTime)
                {
                    Assert.IsTrue(dateTime.Equals(firstSmsNotification) || dateTime.Equals(secondSmsNotification));
                }
              
            }
        }
    }
}
