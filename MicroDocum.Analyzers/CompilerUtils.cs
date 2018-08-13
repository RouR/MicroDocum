using System;
using System.Linq;
using System.Reflection;

#if (NET35 || NET40)
#else
using System.Runtime.CompilerServices;
#endif

/*
 https://docs.microsoft.com/en-us/dotnet/standard/frameworks
 https://docs.microsoft.com/en-us/dotnet/core/tutorials/libraries
.NET Framework	NET20, NET35, NET40, NET45, NET451, NET452, NET46, NET461, NET462, NET47, NET471
.NET Standard	NETSTANDARD1_0, NETSTANDARD1_1, NETSTANDARD1_2, NETSTANDARD1_3, NETSTANDARD1_4, NETSTANDARD1_5, NETSTANDARD1_6, NETSTANDARD2_0
.NET Core	NETCOREAPP1_0, NETCOREAPP1_1, NETCOREAPP2_0
 */

namespace MicroDocum.Analyzers
{
    public static class CompilerUtils
    {
#if (NET35 || NET40)
       public static bool MessageTypeHasAttribute(Type message, Type attribute)
        {
            return message.IsDefined(attribute, false);
        }

        public static Attribute[] GetAttributes(Type message)
        {
            return Attribute.GetCustomAttributes(message).ToArray();
        }

        public static Type[] GetLinks(Type message)
        {
            var interfaces = message.GetInterfaces().ToArray();
            var uniq = interfaces.Select(x => x.MetadataToken).Distinct();
            return uniq.Select(u => interfaces.First(x=> x.MetadataToken == u))
                .ToArray();
        }

        public static Type[] GenericTypeArguments(Type type)
        {
            return type.GetGenericArguments().ToArray();
        }

        public static bool IsGeneric(Type type)
        {
            return type.IsGenericType;
        }

        public static bool CheckHasFlag(this Enum variable, Enum value)
        {
            if (variable == null)
                return false;

            if (value == null)
                throw new ArgumentNullException("value");

            // Not as good as the .NET 4 version of this function, but should be good enough
            if (!Enum.IsDefined(variable.GetType(), value))
            {
                throw new ArgumentException(string.Format(
                    "Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
                    value.GetType(), variable.GetType()));
            }

            ulong num = Convert.ToUInt64(value);
            return ((Convert.ToUInt64(variable) & num) == num);

        }

        public static bool CheckIsAssignableFrom(this Type variable, Type value)
        {
            return variable.IsAssignableFrom(value);

        }

        public static bool AttributeIs(Attribute customAttributeData, Type type)
        {
            return customAttributeData.GetType() == type;
            //return customAttributeData.GetType() == type;
        }
#else

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MessageTypeHasAttribute(Type message, Type attribute)
        {
            return message.GetTypeInfo().CustomAttributes.Any(x => x.AttributeType == attribute);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Attribute[] GetAttributes(Type message)
        {
#if (NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47) //NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47
            return Attribute.GetCustomAttributes(message).ToArray();
#elif(NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0)
            return message.GetTypeInfo().GetCustomAttributes().ToArray();
#else
            var debug = message.GetTypeInfo().GetCustomAttributes();
            return debug
#if(NETCOREAPP2_0)
                //.Select(x=> (Attribute)x)
#endif
                .Distinct().ToArray();
#endif
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GetLinks(Type message)
        {
            var interfaces = message.GetTypeInfo().ImplementedInterfaces.ToArray();
            var uniq = interfaces.Select(x => x.GetTypeInfo().MetadataToken).Distinct();
            return uniq.Select(u => interfaces.First(x => x.GetTypeInfo().MetadataToken == u))
                .ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GenericTypeArguments(Type type)
        {
            return type.GenericTypeArguments.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGeneric(Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckHasFlag(this Enum variable, Enum value)
        {
            return variable.HasFlag(value);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckIsAssignableFrom(this Type variable, Type value)
        {
            return variable.GetTypeInfo().IsAssignableFrom(value);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AttributeIs(Attribute customAttributeData, Type type)
        {
            return customAttributeData.GetType() == type;
            //customAttributeData.GetType() == type; is wrong
        }
#endif

    }
}