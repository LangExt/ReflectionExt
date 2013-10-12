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
    /// 「閉じているかどうかが分からない」TypeSketchの2つの型を用意しています。
    /// ClosedType型のオブジェクトは、TypeSketch型のオブジェクトに対してApplyTypesを呼ぶことでしか生成できないようにすることで、
    /// 「確実に閉じた型であること」を保証しています。
    /// </remarks>
    public class ClosedType : TypeLike<ClosedType>
    {
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
            return type.GetGenericArguments().ToSeq().Partition(t => t.IsGenericParameter).Match(
                (genParams, appliedParams) =>
                {
                    if (genParams.IsEmpty() && typeParameterTypes.IsEmpty())
                        return new ClosedType(type);
                    if (genParams.Size() != typeParameterTypes.Size())
                        throw new ArgumentException();
                    var res = type.MakeGenericType(typeParameterTypes.Map(t => t.ToType()).ToArray());
                    return new ClosedType(res);
                }
            );
        }

        /// <summary>
        /// 型パラメータの適用を解除し、OpenTypeへの変換を試みます。
        /// </summary>
        public Option<OpenType> UnapplyTypes()
        {
            return OpenType.FromType(this.Type);
        }
    }
}
