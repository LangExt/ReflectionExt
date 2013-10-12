using System;
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
        internal ClosedType(Type type) : base(type) { }

        protected override ClosedType ToSelf(Type rawType)
        {
            return new ClosedType(rawType);
        }
    }
}
