using System;
using System.Diagnostics;
using LangExt;

namespace ReflectionExt
{
    /// <summary>
    /// 開いた型を表すクラスです。
    /// </summary>
    /// <remarks>
    /// 「開いた」とは、指定されていない型パラメータを持つ型のことです。
    /// 例えば、List[T]は開いていますが、List[int]は開いていません。
    /// OpenType型のオブジェクトは、TypeSketch型のオブジェクトに対してUnapplyTypesを呼ぶことでしか生成できないようにすることで、
    /// 「確実に開いた型であること」を保証しています。
    /// </remarks>
    public class OpenType : TypeLike<OpenType>
    {
        internal OpenType(Type type)
            : base(type)
        {
            Debug.Assert(type.ContainsGenericParameters);
        }

        protected override OpenType ToSelf(Type rawType)
        {
            return new OpenType(rawType);
        }

        internal static Option<OpenType> FromType(Type type)
        {
            if (type.IsGenericType == false)
                return Option.None;
            var res = type.GetGenericTypeDefinition();
            return Option.Some(new OpenType(res));
        }
    }
}
