using System;
using LangExt;

namespace ReflectionExt
{
    public static class TypeModule
    {
        static string ToCSharpNameImpl(Type type, string defaultName, Func<Type, string> recFun, string prefix)
        {
            if (type.IsPrimitive)
            {
                if (type == typeof(bool)) return "bool";
                if (type == typeof(byte)) return "byte";
                if (type == typeof(sbyte)) return "sbyte";
                if (type == typeof(short)) return "short";
                if (type == typeof(ushort)) return "ushort";
                if (type == typeof(char)) return "char";
                if (type == typeof(int)) return "int";
                if (type == typeof(uint)) return "uint";
                if (type == typeof(long)) return "long";
                if (type == typeof(ulong)) return "ulong";
                if (type == typeof(float)) return "float";
                if (type == typeof(double)) return "double";
            }
            if (type == typeof(decimal)) return "decimal";
            if (type == typeof(string)) return "string";
            if (type == typeof(object)) return "object";

            var genArgs = type.GetGenericArguments();
            if (genArgs.Length == 0)
                return type.IsGenericParameter ? type.Name : prefix + defaultName;

            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return recFun(genArgs[0]) + "?";

            var openTypeName = prefix + defaultName.Substring(0, defaultName.IndexOf('`'));
            return openTypeName + genArgs.Map(recFun).ToSeq().ToString("<", ", ", ">");
        }

        public static string ToCSharpName(this Type self)
        {
            return ToCSharpNameImpl(self, self.Name, ToCSharpName, "");
        }

        public static string ToCSharpFullName(this Type self)
        {
            return ToCSharpNameImpl(self, self.FullName, ToCSharpFullName, "global::");
        }
    }
}
