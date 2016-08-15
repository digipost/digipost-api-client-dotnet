using System;
using System.IO;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.Fakes;
using Xunit;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    public class InvoiceTests
    {
        public class ConstructorMethod : InvoiceTests
        {
            [Fact]
            public void CreateFromContentBytes()
            {
                //Arrange
                var dueDate = DateTime.Now.AddDays(20);
                var invoice = new Invoice("An invoice", "txt", new byte[] {1, 2, 3}, 125, "13452564677", dueDate, "1234567890kid");

                //Act

                //Assert
                Assert.Equal("13452564677", invoice.Account);
                Assert.Equal(125, invoice.Amount);
                Assert.Equal(dueDate, invoice.Duedate);
                Assert.Equal("1234567890kid", invoice.Kid);
            }

            [Fact]
            public void CreateFromPath()
            {
                //Arrange
                var dueDate = DateTime.Now.AddDays(20);
                var invoice = new FakeInvoice("An invoice", "txt", Path.GetTempFileName(), 125, "13452564677", dueDate, "1234567890kid");

                //Act

                //Assert
                Assert.Equal("13452564677", invoice.Account);
                Assert.Equal(125, invoice.Amount);
                Assert.Equal(dueDate, invoice.Duedate);
                Assert.Equal("1234567890kid", invoice.Kid);
            }

            [Fact]
            public void CreateFromStream()
            {
                //Arrange
                var dueDate = DateTime.Now.AddDays(20);
                var invoice = new Invoice("An invoice", "txt", Stream.Null, 125, "13452564677", dueDate, "1234567890kid");

                //Act

                //Assert
                Assert.Equal("13452564677", invoice.Account);
                Assert.Equal(125, invoice.Amount);
                Assert.Equal(dueDate, invoice.Duedate);
                Assert.Equal("1234567890kid", invoice.Kid);
            }
        }
    }
}