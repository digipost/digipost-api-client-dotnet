using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Digipost.Api.Client.Domain;
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
                Console.WriteLine(myPropertyA.Name);

                var valueA = myPropertyA.GetValue(a, null);
                var valueB = myPropertyB.GetValue(b, null);
                
                if (IsList(valueA) || IsDictionary(valueA))
                {
                    var aType = valueA.GetType();
                    var isAlike = false;
                    if (aType == typeof(List<Listedtime>))
                    {
                        isAlike = ScrambledEquals((IEnumerable<Listedtime>) valueA, (IEnumerable<Listedtime>) valueB);     
                    }
                    else if (aType == typeof(List<int>))
                    {
                        isAlike = ScrambledEquals((IEnumerable<int>) valueA, (IEnumerable<int>) valueB);     
                    }

                   Assert.IsTrue(isAlike);
                   continue;
                }

                TestPrimitiveValue(valueA, valueB);
            }
        }

        public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var cnt = new Dictionary<T, int>();
            foreach (var s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (var s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }

        private static void TestPrimitiveValue(object valueA, object valueB)
        {
            if (valueA != null && !PrimitiveTypes.Test(valueA.GetType()))
            {
                LookLikeEachOther(valueA, valueB);
            }
            else
            {
                Assert.AreEqual(valueA, valueB,
                    string.Format(@"The value {1}  of the property {0} on instance a is different from 
            the value {2}  on instance b.",( valueA == null?"null":valueA.GetType().Name), valueA, valueB));    
            }

            
        }

        public static bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsDictionary(object o)
        {
            if (o == null) return false;
            return o is IDictionary &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }
        static class PrimitiveTypes
        {
            public static readonly Type[] List;

            static PrimitiveTypes()
            {
                var types = new[]
                          {
                              typeof (Enum),
                              typeof (String),
                              typeof (Char),
                              typeof (Guid),

                              typeof (Boolean),
                              typeof (Byte),
                              typeof (Int16),
                              typeof (Int32),
                              typeof (Int64),
                              typeof (Single),
                              typeof (Double),
                              typeof (Decimal),

                              typeof (SByte),
                              typeof (UInt16),
                              typeof (UInt32),
                              typeof (UInt64),

                              typeof (DateTime),
                              typeof (DateTimeOffset),
                              typeof (TimeSpan),
                          };


                var nullTypes = from t in types
                                where t.IsValueType
                                select typeof(Nullable<>).MakeGenericType(t);

                List = types.Concat(nullTypes).ToArray();
            }

            public static bool Test(Type type)
            {
                if (List.Any(x => x.IsAssignableFrom(type)))
                    return true;

                var nut = Nullable.GetUnderlyingType(type);
                return nut != null && nut.IsEnum;
            }
        }

    }
}
