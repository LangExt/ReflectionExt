using System;
using System.ComponentModel;
using LangExt;

namespace ReflectionExt
{
    /// <summary>
    /// 型名を表すクラスです。
    /// </summary>
    public sealed class Name : IEquatable<Name>
    {
        readonly Type type;
        Name(Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// System.TypeオブジェクトからNameオブジェクトを構築します。
        /// </summary>
        public static Name Of(Type type)
        {
            return new Name(type);
        }

        /// <summary>
        /// 型の単純名を取得します。
        /// </summary>
        public string Value { get { return this.type.Name; } }

        /// <summary>
        /// 型の完全名を取得します。
        /// </summary>
        public string FullName { get { return this.type.FullName; } }

        /// <summary>
        /// 型の属する名前空間を取得します。
        /// </summary>
        public string Namespace { get { return this.type.Namespace; } }

        /// <summary>
        /// 型の単純名を取得します。
        /// Valueと違い、組み込み型がintなどのように特別扱いされて変換されます。
        /// また、ジェネリック型も型パラメータ名を含めて変換されます。
        /// </summary>
        public string CSharpName { get { return this.type.ToCSharpName(); } }

        /// <summary>
        /// 型の完全名を取得します。
        /// FullNameと違い、組み込み型がintなどのように特別扱いされて変換されます。
        /// また、ジェネリック型も型パラメータ名を含めて変換されます。
        /// </summary>
        public string CSharpFullName { get { return this.type.ToCSharpFullName(); } }

        /// <summary>
        /// 現在のオブジェクトが、同じ型の別のオブジェクトと等しいかどうかを判定します。 
        /// </summary>
        /// <param name="other">このオブジェクトと比較するName</param>
        /// <returns>現在のオブジェクトがotherで指定されたオブジェクトと等しい場合はtrue、それ以外の場合はfalse</returns>
        public bool Equals(Name other)
        {
            return this.CSharpFullName == other.CSharpFullName;
        }

        /// <summary>
        /// このオブジェクトを文字列表現に変換します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return "Name(" + this.CSharpFullName + ")";
        }

        /// <summary>
        /// 等値比較演算子です。
        /// </summary>
        public static bool operator ==(Name lhs, Name rhs)
        {
            if (lhs.IsNull()) return rhs.IsNull();
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// 否定の等値比較演算子です。
        /// </summary>
        public static bool operator !=(Name lhs, Name rhs)
        {
            if (lhs.IsNull()) return rhs.IsNotNull();
            return lhs.Equals(rhs) == false;
        }

        /// <summary>使用しません。</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            var other = obj as Name;
            if (other.IsNull())
                return false;
            return Equals(other);
        }

        /// <summary>使用しません。</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return this.CSharpFullName.GetHashCode();
        }
    }
}
