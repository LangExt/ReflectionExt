using System;
using System.Diagnostics;
using LangExt;

namespace ReflectionExt
{
    /// <summary>
    /// 閉じた型を表すクラスです。
    /// </summary>
    /// <remarks>
    /// 「閉じた」とは、指定されていない型パラメータを持たない型のことです。
    /// 例えば、List[int]は閉じていますが、List[T]のままでは閉じていません。
    /// System.Typeは、閉じていない型に対してもMethodInfoなどの情報が取得できますが、
    /// 閉じていない型経由で取得したMethodInfoを呼び出そうとすると実行時エラーが発生します。
    /// この意図しない実行時エラーを減らすために、ReflectionExtでは「確実に閉じた型であることが保証されている」ClosedTypeと、
    /// 「閉じているかどうかが分からない」TypeSketch、「確実に開いた型であることが保障されている」OpenTypeの3つの型を用意しています。
    /// ClosedType型のオブジェクトは、TypeSketch型のオブジェクトやOpenType型のオブジェクトに対してApplyTypesを呼ぶことでしか生成できないようにすることで、
    /// 「確実に閉じた型であること」を保証しています。
    /// </remarks>
    public class ClosedType : TypeLike<ClosedType>
    {
        /// <summary>bool型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfBool = new ClosedType(typeof(bool));
        /// <summary>byte型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfByte = new ClosedType(typeof(byte));
        /// <summary>sbyte型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfSByte = new ClosedType(typeof(sbyte));
        /// <summary>short型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfShort = new ClosedType(typeof(short));
        /// <summary>ushort型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfUShort = new ClosedType(typeof(ushort));
        /// <summary>char型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfChar = new ClosedType(typeof(char));
        /// <summary>int型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfInt = new ClosedType(typeof(int));
        /// <summary>uint型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfUInt = new ClosedType(typeof(uint));
        /// <summary>long型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfLong = new ClosedType(typeof(long));
        /// <summary>ulong型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfULong = new ClosedType(typeof(ulong));
        /// <summary>float型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfFloat = new ClosedType(typeof(float));
        /// <summary>double型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfDouble = new ClosedType(typeof(double));
        /// <summary>decimal型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfDecimal = new ClosedType(typeof(decimal));
        /// <summary>string型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfString = new ClosedType(typeof(string));
        /// <summary>object型を表すClosedTypeです。</summary>
        public static readonly ClosedType OfObject = new ClosedType(typeof(object));

        internal ClosedType(Type type)
            : base(type)
        {
            Debug.Assert(type.IsGenericType == false);
        }

        protected override ClosedType ToSelf(Type rawType)
        {
            return new ClosedType(rawType);
        }

        internal static ClosedType FromType(Type type, Seq<ClosedType> typeParameterTypes)
        {
            if (type.ContainsGenericParameters == false && typeParameterTypes.IsNotEmpty())
                throw new ArgumentException();

            if (type.IsGenericType == false)
                return new ClosedType(type);

            var res = type.GetGenericTypeDefinition().MakeGenericType(TypeArgs(type, typeParameterTypes, FromType).ToArray());
            return new ClosedType(res);
        }

        /// <summary>
        /// 型パラメータの適用を解除し、OpenTypeへの変換を試みます。
        /// </summary>
        public Option<OpenType> UnapplyTypes()
        {
            return OpenType.FromType(this.Type, Seq.Empty<OpenType>());
        }
    }
}
