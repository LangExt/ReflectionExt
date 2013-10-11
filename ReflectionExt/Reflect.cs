using System;
using LangExt;

namespace ReflectionExt
{
    /// <summary>
    /// ReflectionExtのエントリポイントとなる、TypeSketchオブジェクトを取得するためのクラスです。
    /// </summary>
    public static class Reflect
    {
        /// <summary>
        /// System.TypeオブジェクトからTypeSketchオブジェクトを構築します。
        /// </summary>
        public static TypeSketch Type(Type type)
        {
            return new TypeSketch(type);
        }

        /// <summary>
        /// 型パラメータで指定した型からTypeSketchオブジェクトを構築します。
        /// </summary>
        public static TypeSketch Type<T>()
        {
            return new TypeSketch(typeof(T));
        }

        /// <summary>
        /// 型名からTypeSketchオブジェクトの構築を試みます。
        /// 型の構築に失敗した場合、Option.Noneが返されます。
        /// </summary>
        public static Option<TypeSketch> Type(string typeName)
        {
            try
            {
                return Option.Some(new TypeSketch(System.Type.GetType(typeName, true)));
            }
            catch
            {
                return Option.None;
            }
        }

        /// <summary>
        /// 型名からTypeSketchオブジェクトの構築を試みます。
        /// 型の構築に失敗した場合、例外が投げられます。
        /// </summary>
        public static TypeSketch TypeUnsafe(string typeName)
        {
            return new TypeSketch(System.Type.GetType(typeName, true));
        }
    }
}
