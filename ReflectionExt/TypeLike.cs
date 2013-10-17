using System;
using System.ComponentModel;
using System.Reflection;
using LangExt;

namespace ReflectionExt
{
    /// <summary>
    /// 型を表す実装を共有するためのクラスです。
    /// </summary>
    public abstract class TypeLike
    {
        /// <summary>
        /// このオブジェクトが表す型です。
        /// </summary>
        protected readonly Type Type;
        /// <summary>
        /// 保持する型を指定してオブジェクトを構築します。
        /// </summary>
        protected TypeLike(Type type)
        {
            this.Type = type;
        }

        /// <summary>
        /// オブジェクトをSystem.Typeに変換します。
        /// </summary>
        public Type ToType()
        {
            return this.Type;
        }

        /// <summary>
        /// このオブジェクトの表す型の名前を取得します。
        /// </summary>
        public Name Name { get { return Name.FromType(this.Type); } }
    }

    /// <summary>
    /// 型を表す実装を共有するためのクラスです。
    /// </summary>
    /// <typeparam name="TSelf">派生クラス自身を指定してください。</typeparam>
    public abstract class TypeLike<TSelf> : TypeLike, IEquatable<TSelf> where TSelf : TypeLike<TSelf>
    {
        /// <summary>
        /// 保持する型を指定してオブジェクトを構築します。
        /// </summary>
        protected TypeLike(Type type) : base(type) { }

        protected abstract TSelf ToSelf(Type rawType);

        /// <summary>
        /// 基底クラスを取得します。
        /// </summary>
        public TSelf BaseType { get { return ToSelf(this.Type.BaseType); } }

        /// <summary>
        /// このオブジェクトの表す型に直接含まれるネストした型をすべて取得します。
        /// </summary>
        public Seq<TSelf> AllNestedTypes
        {
            get { return this.Type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic).Map(this.ToSelf).ToSeq(); }
        }

        /// <summary>
        /// このオブジェクトの表す型に直接含まれるネストしたpublicな型を取得します。
        /// 非publicな型も含めて取得したい場合、AllNestedTypesを使用してください。
        /// </summary>
        public Seq<TSelf> PublicNestedTypes
        {
            get { return this.Type.GetNestedTypes(BindingFlags.Public).Map(this.ToSelf).ToSeq(); }
        }

        protected static Seq<Type> TypeArgs(Type type, Seq<TSelf> typeParameterTypes, Func<Type, Seq<TSelf>, TSelf> fromTypeFunc)
        {
            var offset = 0;
            return type.GetGenericArguments().ToSeq().Map(t =>
            {
                if (t.IsGenericParameter)
                {
                    if (offset + 1 > typeParameterTypes.Size())
                        throw new ArgumentException();
                    return LangExt.Unsafe.SeqUnsafe.Get(typeParameterTypes, offset++).ToType();
                }
                if (t.IsGenericTypeDefinition)
                {
                    var count = CountOfTypeParameterTypes(t);
                    if (offset + count > typeParameterTypes.Size())
                        throw new ArgumentException();
                    var res = fromTypeFunc(t, typeParameterTypes.Skip(offset).Take(count));
                    offset += count;
                    return res.ToType();
                }
                return t;
            });
        }

        static int CountOfTypeParameterTypes(Type type)
        {
            if (type.ContainsGenericParameters == false)
                return 0;
            var nested = type.GetGenericArguments();
            var count = nested.ToSeq().Count(t => t.IsGenericParameter);
            return count + nested.ToSeq().SumBy(CountOfTypeParameterTypes);
        }

        /// <summary>
        /// 現在のオブジェクトが、同じ型の別のオブジェクトと等しいかどうかを判定します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するTSelf</param>
        /// <returns>現在のオブジェクトがotherで指定されたオブジェクトと等しい場合はtrue、それ以外の場合はfalse</returns>
        public bool Equals(TSelf other)
        {
            return this.Name.CSharpFullName == other.Name.CSharpFullName;
        }

        /// <summary>
        /// このオブジェクトを文字列表現に変換します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return string.Format("{0}(value={1})", this.GetType().Name, this.Name.CSharpFullName);
        }

        /// <summary>
        /// 現在のオブジェクトが、別のオブジェクトと等しいかどうかを判定します。
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            var other = obj as TSelf;
            if (other == null)
                return false;
            return this.Equals(other);
        }

        /// <summary>
        /// 現在のオブジェクトのハッシュ値を計算して返します。
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
