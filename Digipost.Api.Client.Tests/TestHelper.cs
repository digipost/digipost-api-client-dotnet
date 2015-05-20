using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests
{
    public class TestHelper
    {
        public static void LookLikeEachOther(object a, object b)
        {
            Type typeA = a.GetType();
            Type typeB = b.GetType();

            Assert.AreEqual(typeA, typeB, "The types of instances a and b are not the same.");

            PropertyInfo[] myProperties = typeA.GetProperties(BindingFlags.DeclaredOnly
                                | BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo myPropertyA in myProperties)
            {
                PropertyInfo myPropertyB = typeB.GetProperty(myPropertyA.Name);
                Assert.IsNotNull(myPropertyB, string.Format(@"The property {0} from instance a 
           was not found on instance b.", myPropertyA.Name));

                Assert.AreEqual<Type>(myPropertyA.PropertyType, myPropertyB.PropertyType,
                       string.Format(@"The type of property {0} on instance a is different from 
           the one on instance b.", myPropertyA.Name));

                Assert.AreEqual(myPropertyA.GetValue(a, null), myPropertyB.GetValue(b, null),
                       string.Format(@"The value of the property {0} on instance a is different from 
           the value on instance b.", myPropertyA.Name));
            }
        }

    }
}
