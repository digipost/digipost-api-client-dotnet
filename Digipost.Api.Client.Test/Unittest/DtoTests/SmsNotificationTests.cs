using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Test.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.Test.Unittest.DtoTests
{
    public class SmsNotificationTests
    {
        private readonly Comparator _comparator = new Comparator();

        public class ConstructorMethod : SmsNotificationTests
        {
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
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);

                Assert.Equal(0, differences.Count());
            }

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
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);

                Assert.Equal(0, differences.Count());
            }
        }
    }
}