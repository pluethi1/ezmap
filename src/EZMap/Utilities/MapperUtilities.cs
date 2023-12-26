using System.Linq.Expressions;
using System.Reflection;

namespace EZMap.Utilities
{
    internal static class MapperUtilities
    {
        internal static IEnumerable<MappableMember> GetMatchingFieldsAndProperties(Type sourceType, Type targetType)
        {
            var sourceMembers = GetReadableFieldsAndProperties(sourceType);
            var targetMembers = GetWriteableFieldsAndProperties(targetType);

            return sourceMembers.Where(sourceMember => targetMembers.Any(targetMember => targetMember.Name == sourceMember.Name)).Select(sourceMember =>
            {
                var targetMember = targetMembers.Single(t => t.Name == sourceMember.Name);

                    return new MappableMember(
                        sourceMember.Name,
                        sourceMember,
                        targetMember);
            });
        }

        internal static IEnumerable<MemberInfo> GetReadableFieldsAndProperties(Type type)
        {
            return GetFieldsAndProperties(
                type,
                pi => pi.CanRead,
                fi => true);
        }

        internal static IEnumerable<MemberInfo> GetWriteableFieldsAndProperties(Type type)
        {
            return GetFieldsAndProperties(
                type,
                pi => pi.CanWrite,
                fi => !fi.IsInitOnly && !fi.IsLiteral);
        }

        internal static object CreateObject(Type type, IServiceProvider serviceProvider)
        {
            //Find best constructor candidates
            var constructors = type.GetConstructors().OrderByDescending(x => x.GetParameters().Length);

            //Iterate through the constructors until an instance could be created
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                var args = new object?[parameters.Length];
                var failedToInjectAll = false;

                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    //Attempt to get object from service provider
                    var argValue = serviceProvider.GetService(parameter.ParameterType);
                    if (argValue == null)
                    {
                        if (!parameter.HasDefaultValue)
                        {
                            failedToInjectAll = true;
                            break;
                        }
                        argValue = parameter.DefaultValue;
                    }
                    args[i] = argValue;
                }

                if (failedToInjectAll)
                {
                    continue;
                }

                return Activator.CreateInstance(type, args)!;
            }

            throw new InvalidOperationException($"Could not create an object of type {type}");
        }

        internal static void AssertEqualType(Type type, Type expectedType, string parameterName)
        {
            if (type != expectedType)
            {
                throw new ArgumentException("Unexpected type", parameterName);
            }
        }

        internal static bool IsPrimitiveType(Type type)
        {
            return Type.GetTypeCode(type) != TypeCode.Object;
        }

        internal static void AssertEqualType<TExpected>(Type type, string parameterName)
        {
            AssertEqualType(type, typeof(TExpected), parameterName);
        }

        internal static string AssertValidMemberAndGetName<TSource, TMember>(Expression<Func<TSource, TMember>> expression, bool? source, string parameterName)
        {
            var memberExpression = (MemberExpression)expression.Body;
            var memberInfo = memberExpression.Member;
            if (!memberInfo.IsMappableMember(source))
            {
                throw new ArgumentException("Invalid member", parameterName);
            }
            return memberInfo.Name;
        }

        internal static Task<T> MakeTask<T>(Func<T> func, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<T>(cancellationToken);
            }
            try
            {
                return Task.FromResult(func());
            }
            catch (Exception ex)
            {
                return Task.FromException<T>(ex);
            }
        }

        private static IEnumerable<MemberInfo> GetFieldsAndProperties(Type type, Func<PropertyInfo, bool> propertyFilter, Func<FieldInfo, bool> fieldFilter)
        {
            const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.Instance;

            return type.GetProperties(BINDING_FLAGS).Where(propertyFilter).Union<MemberInfo>(
                   type.GetFields(BINDING_FLAGS).Where(fieldFilter));
        }
    }
}