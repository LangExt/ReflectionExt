using System;

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
        public Name Name { get { return Name.Of(this.Type); } }
    }

    /// <summary>
    /// 型を表す実装を共有するためのクラスです。
    /// </summary>
    /// <typeparam name="TSelf">派生クラス自身を指定してください。</typeparam>
    public abstract class TypeLike<TSelf> : TypeLike where TSelf : TypeLike<TSelf>
    {
        /// <summary>
        /// 保持する型を指定してオブジェクトを構築します。
        /// </summary>
        protected TypeLike(Type type) : base(type) { }

        /// <summary>
        /// 基底クラスを取得します。
        /// </summary>
        public abstract TSelf BaseType { get; }
    }
}
