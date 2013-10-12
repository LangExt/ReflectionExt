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
}
