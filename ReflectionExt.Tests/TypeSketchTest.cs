using System;
using LangExt;
using ReflectionExt;
using NUnit.Framework;

namespace ReflectionExt.Tests
{
    [TestFixture]
    public class TypeSketchTest
    {
        [Test]
        public void ApplyTypesでintを表すTypeSketchからClosedTypeに変換できる()
        {
            ClosedType sut = Reflect.Type<int>().ApplyTypes();
            Assert.That(sut.ToType(), Is.EqualTo(typeof(int)));
        }

        [Test]
        public void ApplyTypesでintのSeqを表すTypeSketchからClosedTypeに変換できる()
        {
            ClosedType sut = Reflect.Type<Seq<int>>().ApplyTypes();
            Assert.That(sut.ToType(), Is.EqualTo(typeof(Seq<int>)));
        }

        [Test]
        public void ApplyTypesでSeqを表すTypeSketchからClosedTypeに変換できる()
        {
            ClosedType sut = Reflect.Type(typeof(Seq<>)).ApplyTypes(Reflect.Type<int>().ApplyTypes());
            Assert.That(sut.ToType(), Is.EqualTo(typeof(Seq<int>)));
        }

        [Test]
        public void ApplyTypesで型パラメータの数が多すぎる場合は例外が発生する()
        {
            var sut = Reflect.Type<Seq<int>>();
            Assert.That(() => sut.ApplyTypes(Reflect.Type<int>().ApplyTypes()), Throws.Exception.TypeOf<ArgumentException>());
        }

        [Test]
        public void ApplyTypesで型パラメータの数が少なすぎる場合は例外が発生する()
        {
            var sut = Reflect.Type(typeof(Seq<>));
            Assert.That(() => sut.ApplyTypes(), Throws.Exception.TypeOf<ArgumentException>());
        }
        
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
        [TestCase(typeof(Sub2<>), "Base2<T>")]
        [TestCase(typeof(Sub3), "Base2<int>")]
        [TestCase(typeof(Sub4<string>), "Base3<string, int>")]
        [TestCase(typeof(Sub4<>), "Base3<T, int>")]
        [TestCase(typeof(Sub5<string>), "Base3<int, string>")]
        [TestCase(typeof(Sub5<>), "Base3<int, T>")]
        public void BaseTypeで基底クラスが取得できる(Type type, string expected)
        {
            var sut = Reflect.Type(type);
            Assert.That(sut.BaseType.Name.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.TypeSketchTest." + expected));
        }
    }
}
