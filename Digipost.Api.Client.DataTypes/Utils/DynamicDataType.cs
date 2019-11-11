using System.Collections.Generic;
using System.Dynamic;

namespace Digipost.Api.Client.DataTypes.Utils
{
    public class DynamicDataType : DynamicObject
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (properties.ContainsKey(binder.Name))
            {
                result = properties[binder.Name];
                return true;
            }

            result = "";
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (properties.ContainsKey(binder.Name))
            {
                properties[binder.Name] = value;
                return true;
            }

            return false;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            dynamic method = properties[binder.Name];
            result = method(args[0].ToString(), args[1].ToString());
            return true;
        }

        public void CreateNullableProperty(string propName)
        {
            properties[propName] = null;
        }

        public void CreateProperty<T>(string propName, T propValue)
        {
            properties[propName] = propValue;
        }
    }
}
