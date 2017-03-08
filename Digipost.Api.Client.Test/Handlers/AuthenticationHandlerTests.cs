using System;
using System.Security.Cryptography;
using System.Text;
using Digipost.Api.Client.Common.Handlers;
using Digipost.Api.Client.Resources.Certificate;
using Xunit;

namespace Digipost.Api.Client.Test.Handlers
{
    public class AuthenticationHandlerTests
    {
        public class ComputeSignatureMethod
        {
            [Fact]
            public void ReturnsCorrectSignature()
            {
                //Arrange
                const string senderId = "1337";
                const string uri = "fakeuri";
                const string method = "POST";
                const string sha256Hash = "TheHashOfContentForHeader";
                const string expectedSignature =
                    "tE/WjL2wot+DB42kiuCnDNm0WgBT/UPFoVGCewomssiKKfILqVu0MDjRo3qwjFRnW3jVqppGnJL5Nq2cl3APCo9nlnizGSd/YXr26XijHPmcgEr48hfnSypQ54TQ6q6swW1Eigq8xcoU3c6R7IkEE5R+rN4f3qpFvc252NHEWgn9QEJGHOE5782PGXggBDPaW/rf1v2i/AtXU1gi0VGg+TagFwLcvF4qNFZB5sg1slGEFV1C9hKpDWrB+uxrRXGiOW60sYEQ+HpxgnHcg5IlKrgUwlbSH/wnKSVnrJ81UzA4zX+oqmtjYdww98E6JFebrS7FBVhG67wqbVDlxY/KVw==";
                var certificate = CertificateResource.Certificate();
                var dateTime = new DateTime(2014, 07, 07, 12, 00, 02).ToString("R");

                //Act
                var computedSignature = AuthenticationHandler.ComputeSignature(method, new Uri(uri, UriKind.Relative), dateTime, sha256Hash,
                    senderId,
                    certificate,
                    false);

                //Assert
                Assert.Equal(expectedSignature, computedSignature);
            }
        }

        public class CreateCryptoProviderMethod
        {
            [Fact]
            public void ReturnsCryptoProviderWithCorrectCspBlob()
            {
                //Arrange
                var certificate = CertificateResource.Certificate();
                const string expectedCspBlob =
                    "BwIAAAAkAABSU0EyAAgAAAEAAQDdeobqOibjM7STPbMYxxUOIy51AnzfR0nSljU6VdqO1jsogB7jlSgLq7UyHOMth7IYQt1eGZcVu1UmGhTX0YYBh2v4k7hyEXe3AsKuQvCPnNClA6FFhRjtmuvu408kbTrxbmSHjPz80k45brnYRatwvXERJElW5QME6YUhXAdTewli4tX49pnX9zu6PaHEG7A61MIGliFuZq+T81UYgsbT0U7kVNtDBvyP8mw7cTul4mzX/CjJuNZuCaOpC/I6g/lNlL/0JtlWZTjlvJMT6P8X0DMmgJnupogS3siQIoHb/X7OH/aIZnaSafBanyMtbdqhm9v18vOKcu0hIgPY1OnSW+syp1ak4i1UI6H7xjFZCZoWKpEQwyk0vySadjPqCv6mh8QSxflfD6N7ho6teQDtyAPwSBrBaKbNPPr9BG12SWZfEdgGc31jAIy5c2cKs5zm1oFT0W0kYFR9NrGPQPYdG+mFbkVmTFK1pQqI3m7sI1cPuUqJgi3dja/3R2fTKNUn4I8ElGvHrphD2hLcggve3DEEhztjmWhZRgB0QSzxNimU+TO72PQUojLb+JyEXuLrKleGGFyBLumMI6vJdJtzEbz2Uv6+QPMcGBGmtE5gtUd5AYCVLp8K/Sq7Ybg/j6BAAcOjbkMMAXVd9pX9sl0oJZVS/vK8xMh+1OqJqHFN/U/niW4cSWoHWfVGkv8QK5UEgLbsoA4vgvvpT1cTZcsnLBqTNqfst4pcVpmCCry74FlUIpanaueSqoNiqmdx6N+AUfvjU9uYHl8xy4dujqPlDkeNc+31hkNyHVMqixaftE8i5BVjizNu6kKvCcpYee1LosZE+i3dJSl5MbKxyzNeKUNGoj5U1zJ9ukQbKhwNAifo6Knb/ZaIP5/dFS185vMTO239TT4jvhqCB40fTMxK3HJGix1NMCItIWWoh7BpsFkkOsOVgUxHcnOq8he3IJVSXRq+/ydHgqp6aTzRjvRat/MzjbtRey7k/s61QzFr/g+Bv0c18vY6aSyhYcW6aRIc9urAyUMgl1LP6no1oeSYbY058yBW/JEmlCmmoinWgsPZoo2DSXQFZ//7ng68IiL5MK2L017b00/MMbeziVyAsDi2Nhj4g9l3uVjoVPfayRfCr9WPQeghOlmT+LIeu0QvD2mak2KI1mrn1n+zRD/RKDKc58+fkm1kr2iOfOYXFPs0XoVqAw3Rjj4dO/KFGkyQHrRECb0YA2zlpt3jxarrBKv+AhYBLuaG+MmRGSxg/hDgZZu+/nfIwBwQWNvk+pdDFi6Hxmby+0OXUuBiczPmGSgnHUaA9HO8jrj0aJJB9j0VgFcw/Pr7sZ16X+j2rErFW5a3cNTVR6un7ilvj8LqNux8HlPkEiOQmmOD4GqY7geqKMCuHD/7TNcFqYkCLyhluyFpm9Z9YRcSZlIKn4Q37YSrVMp4zTsCm/INgIsDCxQoNTTPVrFdCC12YY6eXuKukwqVloHj3c6IBl+PN93MtrGc7e8cBn3tOyZ+NO1PCZc3PaTNbEIhbffpGyE0Ghs=";

                //Act
                var actualRspCryptoServiceProvier = AuthenticationHandler.RsaCryptoServiceProvider(certificate);
                var actualCspBlob = Convert.ToBase64String(actualRspCryptoServiceProvier.ExportCspBlob(true));

                //Assert
                var expectedCspCryptoServiceProvider = new RSACryptoServiceProvider();
                expectedCspCryptoServiceProvider.ImportCspBlob(Convert.FromBase64String(expectedCspBlob));

                Assert.Equal(expectedCspBlob, actualCspBlob);
            }
        }

        public class ComputeHashMethod
        {
            [Fact]
            public void ReturnsCorrectHash()
            {
                //Arrange
                var contentBytes = Encoding.UTF8.GetBytes("This is the content to hash.");

                //Act
                var computedHash = AuthenticationHandler.ComputeHash(contentBytes);

                //Assert
                var expectedHash = "gvXOB75lBGBY6LVTAVVpapZkBOv531VUE0EHrP2rryE=";

                Assert.Equal(expectedHash, computedHash);
            }
        }
    }
}