using System;
using System.ComponentModel;
using LangExt;

namespace ReflectionExt
{
    /// <summary>
    /// 閉じていない可能性のある型を表すクラスです。
    /// </summary>
    /// <remarks>
    /// 「閉じていない」とは、指定されていない型パラメータを持っている型のことです。
    /// 例えば、List[int]は閉じていますが、List[T]のままでは閉じていません。
    /// System.Typeは、閉じていない型に対してもMethodInfoなどの情報が取得できますが、
    /// 閉じていない型経由で取得したMethodInfoを呼び出そうとすると実行時エラーが発生します。
    /// この意図しない実行時エラーを減らすために、ReflectionExtでは「確実に閉じた型であることが保証されている」ClosedTypeと、
    /// 「閉じているかどうかが分からない」TypeSketch、「確実に開いた型であることが保障されている」OpenTypeの3つの型を用意しています。
    /// ClosedType型のオブジェクトは、TypeSketch型のオブジェクトやOpenType型のオブジェクトに対してApplyTypesを呼ぶことでしか生成できないようにすることで、
    /// 「確実に閉じた型であること」を保証しています。
    /// また、OpenType型のオブジェクトは、TypeSketch型のオブジェクトやCloedType型のオブジェクトに対してUnapplyTypesを呼ぶことでしか生成できないようにすることで、
    /// 「確実に開いた型であること」を保証しています。
    /// TypeSketch型は、開いた型でも閉じた型でも扱えますが、それに加えて「型パラメータは適用されているが、適用されたパラメータが開いている」ような型を扱うこともできます。
    /// 例えば、Seq[Seq[T]]を扱いたい場合は、TypeSketchを使うことになります。
    /// </remarks>
    public class TypeSketch : TypeLike<TypeSketch>
    {
        internal TypeSketch(Type type) : base(type) { }

        protected override TypeSketch ToSelf(Type rawType)
        {
            return new TypeSketch(rawType);
        }

        /// <summary>
        /// 型パラメータを適用し、ClosedTypeに変換します。
        /// 型パラメータは、全て適用する必要があります。
        /// 型パラメータが多すぎる場合や、足りない場合、例外が発生します。
        /// 型パラメータを適用していない型を適用したい場合、ApplyTypeSketchesやApplyOpenTypesを使ってください。
        /// </summary>
        public ClosedType ApplyTypes(params ClosedType[] typeParameterTypes)
        {
            return ClosedType.FromType(this.Type, typeParameterTypes.ToSeq());
        }

        /// <summary>
        /// 型パラメータを適用します。
        /// 型パラメータは、全て適用する必要があります。
        /// 型パラメータが多すぎる場合や、足りない場合、例外が発生します。
        /// ClosedTypeを得たい場合、ApplyTypesを使ってください。
        /// </summary>
        public TypeSketch ApplyTypeSketches(params TypeSketch[] typeParameterTypeSketches)
        {
            if (this.Type.ContainsGenericParameters == false)
                throw new InvalidOperationException();
            return FromType(this.Type, typeParameterTypeSketches.ToSeq());
        }

        /// <summary>
        /// 型パラメータを適用します。
        /// 型パラメータは、全て適用する必要があります。
        /// 型パラメータが多すぎる場合や、足りない場合、例外が発生します。
        /// ClosedTypeを得たい場合、ApplyTypesを使ってください。
        /// </summary>
        public OpenType ApplyOpenTypes(params OpenType[] typeParameterOpenTypes)
        {
            if (this.Type.ContainsGenericParameters == false)
                throw new InvalidOperationException();
            if (typeParameterOpenTypes.Length == 0)
                throw new ArgumentException();
            return OpenType.FromType(this.Type, typeParameterOpenTypes.ToSeq()).GetOrElse(() => { throw new Exception(); });
        }

        /// <summary>
        /// 型パラメータの適用を解除し、OpenTypeへの変換を試みます。
        /// </summary>
        public Option<OpenType> UnapplyTypes()
        {
            return OpenType.FromType(this.Type, Seq.Empty<OpenType>());
        }

        internal static TypeSketch FromType(Type type, Seq<TypeSketch> typeParameterTypes)
        {
            if (type.ContainsGenericParameters == false && typeParameterTypes.IsNotEmpty())
                throw new ArgumentException();

            if (type.IsGenericType == false)
                return new TypeSketch(type);

            var res = type.GetGenericTypeDefinition().MakeGenericType(TypeArgs(type, typeParameterTypes, FromType).ToArray());
            return new TypeSketch(res);
        }
    }
}
