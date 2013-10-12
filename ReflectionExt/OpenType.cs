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
    /// OpenType型のオブジェクトは、TypeSketch型のオブジェクトやClosedType型のオブジェクトに対してUnapplyTypesを呼ぶことでしか生成できないようにすることで、
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

        /// <summary>
        /// 型パラメータを適用し、ClosedTypeに変換します。
        /// 型パラメータは、全て適用する必要があります。
        /// 型パラメータが多すぎる場合や、足りない場合、例外が発生します。
        /// </summary>
        /// <remarks>
        /// OpenTypeは、型パラメータを最低1つは持っていることが確実なため、このメソッドは引数を全く与えないということは出来ません。
        /// これは、ClosedTypeの配列を持っており、それをそのまま適用する場合にはこのメソッドを直接使うことができないということを意味します。
        /// ClosedTypeの配列を持っている場合は、この型をTypeSketchに変換してからTypeSketchのAppyTypesを呼び出すといいでしょう。
        /// </remarks>
        public ClosedType ApplyTypes(ClosedType firstTypeParameter, params ClosedType[] restTypeParameters)
        {
            return ClosedType.FromType(this.Type, Seq.Singleton(firstTypeParameter).Append(restTypeParameters.ToSeq()));
        }

        /// <summary>
        /// このオブジェクトをTypeSketchに変換します。
        /// この変換は失敗しません。
        /// </summary>
        public TypeSketch ToSketch()
        {
            return new TypeSketch(this.Type);
        }

        /// <summary>
        /// このオブジェクトをTypeSketchに暗黙変換します。
        /// 対象がnullの場合はnullが返されますが、この挙動は将来にわたって保証されるものではありません。
        /// </summary>
        public static implicit operator TypeSketch(OpenType openType)
        {
            if (openType == null)
                return null;
            return new TypeSketch(openType.Type);
        }
    }
}
