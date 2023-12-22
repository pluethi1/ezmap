using System.Reflection;

namespace EZMap.Utilities
{
    internal static class ReflectionExtensions
    {
        internal static Type GetMemberType(this MemberInfo memberInfo)
        {
            return memberInfo switch
            {
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                FieldInfo fieldInfo => fieldInfo.FieldType,
                _ => throw new NotSupportedException("Can only get types of fields and properties")
            };
        }

        internal static object? GetMemberValue(this MemberInfo memberInfo, object? @object)
        {
            return memberInfo switch
            {
                PropertyInfo propertyInfo => propertyInfo.GetValue(@object),
                FieldInfo fieldInfo => fieldInfo.GetValue(@object),
                _ => throw new NotSupportedException("Can only get value of fields and properties")
            };
        }

        internal static void SetMemberValue(this MemberInfo memberInfo, object? @object, object? value)
        {
            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(@object, value);
                    break;
                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(@object, value);
                    break;
                default:
                    throw new NotSupportedException("Can only set value of fields and properties");
            }
        }

        internal static bool IsAssignableToAny(this Type type, params Type[] types)
        {
            foreach (var t in types)
            {
                if (type.IsAssignableTo(t))
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool IsMappableMember(this MemberInfo memberInfo, bool? source)
        {
            if (memberInfo is PropertyInfo propertyInfo)
            {
                switch (source)
                {
                    case true:
                        return propertyInfo.CanRead;
                    case false:
                        return propertyInfo.CanWrite; 
                    case null:
                        return true;
                }
            }

            if (memberInfo is FieldInfo fieldInfo)
            {
                switch (source)
                {
                    case true:
                        return true;
                    case false:
                        return !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral;
                    case null:
                        return true;
                }
            }

            return false;
        }
    }
}
