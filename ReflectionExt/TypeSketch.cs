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
    /// 「閉じているかどうかが分からない」TypeSketchの2つの型を用意しています。
    /// ClosedType型のオブジェクトは、TypeSketch型のオブジェクトに対してApplyTypesを呼ぶことでしか生成できないようにすることで、
    /// 「確実に閉じた型であること」を保証しています。
    /// </remarks>
    public class TypeSketch : TypeLike<TypeSketch>, IEquatable<TypeSketch>
    {
        internal TypeSketch(Type type) : base(type) { }

        protected override TypeSketch ToSelf(Type rawType)
        {
            return new TypeSketch(rawType);
        }

        /// <summary>
        /// 現在のオブジェクトが、同じ型の別のオブジェクトと等しいかどうかを判定します。 
        /// </summary>
        /// <param name="other">このオブジェクトと比較するTypeSketch</param>
        /// <returns>現在のオブジェクトがotherで指定されたオブジェクトと等しい場合はtrue、それ以外の場合はfalse</returns>
        public bool Equals(TypeSketch other)
        {
            return this.Name.CSharpFullName == other.Name.CSharpFullName;
        }

        /// <summary>使用しません。</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            var other = obj as TypeSketch;
            if (other == null)
                return false;
            return Equals(other);
        }

        /// <summary>使用しません。</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        /// <summary>
        /// このオブジェクトを文字列表現に変換します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return string.Format("TypeSketch(value={0})", this.Name.CSharpFullName);
        }

        /// <summary>
        /// 型パラメータを適用し、ClosedTypeに変換します。
        /// 型パラメータは、全て適用する必要があります。
        /// 型パラメータが多すぎる場合や、足りない場合、例外が発生します。
        /// </summary>
        public ClosedType ApplyTypes(params ClosedType[] typeParameterTypes)
        {
            return this.Type.GetGenericArguments().ToSeq().Partition(t => t.IsGenericParameter).Match(
                (genParams, appliedParams) =>
                {
                    if (genParams.IsEmpty() && typeParameterTypes.Length == 0)
                        return new ClosedType(this.Type);
                    if (genParams.Size() != typeParameterTypes.Length)
                        throw new ArgumentException();
                    var res = this.Type.MakeGenericType(typeParameterTypes.Map(t => t.ToType()).ToSeq().ToArray());
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
