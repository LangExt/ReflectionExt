using System;
using LangExt;
using ReflectionExt;
using NUnit.Framework;

namespace ReflectionExt.Tests
{
    [TestFixture]
    public class TypeSketchTest
    {
        public class intを表すTypeSketch
        {
            [Test]
            public void ApplyTypesでClosedTypeに変換できる()
            {
                ClosedType sut = Reflect.Type<int>().ApplyTypes();
                Assert.That(sut.ToType(), Is.EqualTo(typeof(int)));
            }
        }

        public class intのSeqを表すTypeSketch
        {
            [Test]
            public void ApplyTypesでClosedTypeに変換できる()
            {
                ClosedType sut = Reflect.Type<Seq<int>>().ApplyTypes();
                Assert.That(sut.ToType(), Is.EqualTo(typeof(Seq<int>)));
            }
        }

        public class Seqを表すTypeSketch
        {
            [Test]
            public void ApplyTypesでClosedTypeに変換できる()
            {
                ClosedType sut = Reflect.Type(typeof(Seq<>)).ApplyTypes(Reflect.Type<int>().ApplyTypes());
                Assert.That(sut.ToType(), Is.EqualTo(typeof(Seq<int>)));
            }
        }

        public class 型パラメータが多すぎる場合
        {
            [Test]
            public void ApplyTypesで例外が発生する()
            {
                var sut = Reflect.Type<Seq<int>>();
                Assert.That(() => sut.ApplyTypes(Reflect.Type<int>().ApplyTypes()), Throws.Exception.TypeOf<ArgumentException>());
            }
        }

        public class 型パラメータが少なすぎる場合
        {
            [Test]
            public void ApplyTypesで例外が発生する()
            {
                var sut = Reflect.Type(typeof(Seq<>));
                Assert.That(() => sut.ApplyTypes(), Throws.Exception.TypeOf<ArgumentException>());
            }
        }

        [TestCase(typeof(Seq<int>), typeof(Seq<>))]
        [TestCase(typeof(Seq<>), typeof(Seq<>))]
        [TestCase(typeof(int), null)]
        public void 型パラメータを持つ型の場合UnapplyTypesでOpenTypesが取得できる(Type type, Type expected)
        {
            var sut = Reflect.Type(type);
            Assert.That(sut.UnapplyTypes().Map(t => t.ToType()), Is.EqualTo(expected == null ? Option.None : Option.Some(expected)));
        }

        [Test]
        public void BaseTypeで取得した中途半端に型指定された型でもUnapplyTypesできる()
        {
            var sut = Reflect.Type(typeof(Sub5<>)).BaseType;
            Assert.That(sut.UnapplyTypes().GetOr(null).Name.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.TypeSketchTest.Base3<T, U>"));

            sut = Reflect.Type(typeof(Sub5<int>)).BaseType;
            Assert.That(sut.UnapplyTypes().GetOr(null).Name.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.TypeSketchTest.Base3<T, U>"));
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
