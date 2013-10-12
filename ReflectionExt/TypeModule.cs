using System;
using System.Text;
using LangExt;

namespace ReflectionExt
{
    /// <summary>
    /// System.Typeに関する操作を提供します。
    /// </summary>
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

            if (type.IsNested && type.IsGenericParameter == false)
                return ToCSharpNestedNameImpl(type, defaultName, recFun, prefix);

            var genArgs = type.GetGenericArguments();
            if (genArgs.Length == 0)
                return type.IsGenericParameter ? type.Name : prefix + defaultName;

            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return recFun(genArgs[0]) + "?";

            var openTypeName = prefix + defaultName.Substring(0, defaultName.IndexOf('`'));
            return openTypeName + genArgs.Map(recFun).ToSeq().ToString("<", ", ", ">");
        }

        private static string ToCSharpNestedNameImpl(Type type, string defaultName, Func<Type, string> recFun, string prefix)
        {
            if (type.IsGenericType == false)
                return prefix + defaultName.Replace('+', '.');
            if (defaultName.Contains("+") == false)
                return ToCSharpNestedSimpleName(type, defaultName, recFun);
            return ToCSharpNestedFullName(type, defaultName, recFun, prefix);
        }

        private static string ToCSharpNestedFullName(Type type, string defaultName, Func<Type, string> recFun, string prefix)
        {
            var appliedParamPos = defaultName.IndexOf('[');
            var types = (appliedParamPos == -1 ? defaultName : defaultName.Substring(0, defaultName.IndexOf('['))).Split('+');
            var genArgs = type.GetGenericArguments();
            var i = 0;
            var buf = new StringBuilder(prefix);
            foreach (var t in types)
            {
                var pos = t.IndexOf('`');
                if (pos != -1)
                {
                    var argNum = int.Parse(t.Substring(pos + 1));
                    buf.Append(t.Substring(0, pos)).Append("<");
                    buf.Append(string.Join(", ", Seq.Repeat(argNum, 0).Map(_ => recFun(genArgs[i++]))));
                    buf.Append(">.");
                }
                else
                {
                    buf.Append(t).Append(".");
                }
            }
            buf.Remove(buf.Length - 1, 1);
            return buf.ToString();
        }

        private static string ToCSharpNestedSimpleName(Type type, string defaultName, Func<Type, string> recFun)
        {
            var pos = defaultName.IndexOf("`");
            if (pos == -1)
                return defaultName;
            var argNum = int.Parse(defaultName.Substring(pos + 1));
            var args = type.GetGenericArguments().ToSeq().Reverse().Take(argNum).Reverse();
            return defaultName.Substring(0, pos) + "<" + string.Join(", ", args.Map(recFun)) + ">";
        }

        /// <summary>
        /// C#風の名前を表す文字列に変換します。
        /// ToCSharpFullnameと違い、どの場所でも有効な名前にはなりません。
        /// </summary>
        public static string ToCSharpName(this Type self)
        {
            return ToCSharpNameImpl(self, self.Name, ToCSharpName, "");
        }

        /// <summary>
        /// C#風の名前を表す文字列に変換します。
        /// この拡張メソッドが返す名前は、コード中のどの場所でも有効な名前になります。
        /// </summary>
        public static string ToCSharpFullName(this Type self)
        {
            return ToCSharpNameImpl(self, DefaultFullName(self), ToCSharpFullName, "global::");
        }

        private static string DefaultFullName(Type type)
        {
            if (type.FullName != null || type.IsGenericParameter)
                return type.FullName;
            if (type.IsNested == false)
                return type.Namespace + "." + type.Name;
            return type.DeclaringType.ToCSharpFullName() + "." + type.Name;
        }
    }
}
