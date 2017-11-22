using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.Send.Tests
{
    public class SmsNotificationTests
    {
        private readonly Comparator _comparator = new Comparator();

        public class ConstructorMethod : SmsNotificationTests
        {
            [Fact]
            public void WithAfterHours()
            {
                //Arrange
                var firstSmsNotification = 2;
                var secondSmsNotification = 5;
                var expected = new List<int> {firstSmsNotification, secondSmsNotification};
                ISmsNotification smsNotification = new SmsNotification(expected.ToArray());

                //Act
                var actual = smsNotification.NotifyAfterHours;

                //Assert
                _comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void WithSendingTime()
            {
                //Arrange
                var firstSmsNotification = DateTime.Today;
                var secondSmsNotification = DateTime.Today.AddDays(1);
                var expected = new List<DateTime> {firstSmsNotification, secondSmsNotification};
                ISmsNotification smsNotification = new SmsNotification(expected.ToArray());

                //Act
                var actual = smsNotification.NotifyAtTimes;

                //Assert
                _comparator.AssertEqual(expected, actual);
            }
        }
    }
}
