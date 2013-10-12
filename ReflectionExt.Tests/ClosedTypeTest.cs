using System;
using LangExt;
using ReflectionExt;
using NUnit.Framework;

namespace ReflectionExt.Tests
{
    [TestFixture]
    public class ClosedTypeTest
    {
        class Base1 {}
        class Base2<T> { }
        class Base3<T, U> { }
        class Sub1 : Base1 { }
        class Sub2<T> : Base2<T> { }
        class Sub3 : Base2<int> { }
        class Sub4<T> : Base3<T, int> { }
        class Sub5<T> : Base3<int, T> { }

        [TestCase(typeof(Sub1), "Base1")]
        [TestCase(typeof(Sub2<int>), "Base2<int>")]
        [TestCase(typeof(Sub3), "Base2<int>")]
        [TestCase(typeof(Sub4<string>), "Base3<string, int>")]
        [TestCase(typeof(Sub5<string>), "Base3<int, string>")]
        public void BaseTypeで基底クラスが取得できる(Type type, string expected)
        {
            var sut = Reflect.Type(type).ApplyTypes();
            Assert.That(sut.BaseType.Name.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.ClosedTypeTest." + expected));
        }

        [TestCase(typeof(Seq<int>), typeof(Seq<>))]
        [TestCase(typeof(int), null)]
        public void 型パラメータを持つ型の場合UnapplyTypesでOpenTypesが取得できる(Type type, Type expected)
        {
            var sut = Reflect.Type(type).ApplyTypes();
            Assert.That(sut.UnapplyTypes().Map(t => t.ToType()), Is.EqualTo(expected == null ? Option.None : Option.Some(expected)));
        }
    }
}
