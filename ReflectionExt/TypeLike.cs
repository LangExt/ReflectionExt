using System;

namespace ReflectionExt
{
    public class TypeLike
    {
        protected readonly Type Type;
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

        public Name Name { get { return Name.Of(this.Type); } }
    }
}
