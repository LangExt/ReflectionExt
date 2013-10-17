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
            readonly TypeSketch sut = Reflect.Type<int>();

            [Test]
            public void ApplyTypesでClosedTypeに変換できる()
            {
                ClosedType res = sut.ApplyTypes();
                Assert.That(res.ToType(), Is.EqualTo(typeof(int)));
            }

            [Test, ExpectedException(typeof(InvalidOperationException))]
            public void ApplyTypeSketchesで例外が発生する()
            {
                sut.ApplyTypeSketches();
            }
        }

        public class intのSeqを表すTypeSketch
        {
            readonly TypeSketch sut = Reflect.Type<Seq<int>>();

            [Test]
            public void ApplyTypesでClosedTypeに変換できる()
            {
                ClosedType res = sut.ApplyTypes();
                Assert.That(res.ToType(), Is.EqualTo(typeof(Seq<int>)));
            }

            [Test, ExpectedException(typeof(InvalidOperationException))]
            public void ApplyTypeSketchesで例外が発生する()
            {
                sut.ApplyTypeSketches();
            }
        }

        public class Seqを表すTypeSketch
        {
            readonly TypeSketch sut = Reflect.Type(typeof(Seq<>));

            [Test]
            public void ApplyTypesでClosedTypeに変換できる()
            {
                ClosedType res = sut.ApplyTypes(Reflect.Type<int>().ApplyTypes());
                Assert.That(res.ToType(), Is.EqualTo(typeof(Seq<int>)));
            }

            [TestCase(typeof(int), "global::LangExt.Seq<int>")]
            [TestCase(typeof(Seq<int>), "global::LangExt.Seq<global::LangExt.Seq<int>>")]
            [TestCase(typeof(Seq<>), "global::LangExt.Seq<global::LangExt.Seq<T>>")]
            public void ApplyTypeSketchでTypeSketchが適用されたTypeSketchが得られる(Type type, string expected)
            {
                TypeSketch res = sut.ApplyTypeSketches(Reflect.Type(type));
                Assert.That(res.Name.CSharpFullName, Is.EqualTo(expected));
            }
        }

        public class 複数の型パラメータを持つTypeSketch
        {
            readonly TypeSketch sut = Reflect.Type(typeof(Base3<,>));

            [Test]
            public void ApplyTypesでClosedTypeに変換できる()
            {
                ClosedType res = sut.ApplyTypes(Reflect.Type<int>().ApplyTypes(), Reflect.Type<string>().ApplyTypes());
                Assert.That(res.ToType(), Is.EqualTo(typeof(Base3<int, string>)));
            }

            [TestCase(typeof(int), typeof(int), "Base3<int, int>")]
            [TestCase(typeof(Seq<>), typeof(int), "Base3<global::LangExt.Seq<T>, int>")]
            [TestCase(typeof(int), typeof(Seq<>), "Base3<int, global::LangExt.Seq<T>>")]
            [TestCase(typeof(Seq<>), typeof(Seq<>), "Base3<global::LangExt.Seq<T>, global::LangExt.Seq<T>>")]   // TODO : T1, T2が正しいのでは？
            public void ApplyTypeSketchでTypeSketchが適用されたTypeSketchが得られる(Type type1, Type type2, string expected)
            {
                TypeSketch res = sut.ApplyTypeSketches(Reflect.Type(type1), Reflect.Type(type2));
                Assert.That(res.Name.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.TypeSketchTest." + expected));
            }

            [Test]
            public void ApplyTypeSketchしたものに対してApplyTypesでClosedTypeに変換できる()
            {
                var intT = Reflect.Type<int>().ApplyTypes();
                var strT = Reflect.Type<string>().ApplyTypes();

                var res1 = sut.ApplyTypeSketches(Reflect.Type(typeof(Seq<>)), Reflect.Type(typeof(Seq<>)));
                Assert.That(res1.ApplyTypes(intT, strT).Name.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.TypeSketchTest.Base3<global::LangExt.Seq<int>, global::LangExt.Seq<string>>"));

                var res2 = sut.ApplyTypeSketches(Reflect.Type<int>(), Reflect.Type(typeof(Seq<>)));
                Assert.That(res2.ApplyTypes(intT).Name.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.TypeSketchTest.Base3<int, global::LangExt.Seq<int>>"));

                var res3 = sut.ApplyTypeSketches(Reflect.Type(typeof(Seq<>)), Reflect.Type<int>());
                Assert.That(res3.ApplyTypes(intT).Name.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.TypeSketchTest.Base3<global::LangExt.Seq<int>, int>"));
            }
        }

        public class ネストしたSeqを表すTypeSketch
        {
            readonly TypeSketch sut = Reflect.Type(typeof(Seq<>)).ApplyTypeSketches(Reflect.Type(typeof(Seq<>)));

            [Test]
            public void ApplyTypesでClosedTypeに変換できる()
            {
                ClosedType res = sut.ApplyTypes(Reflect.Type<int>().ApplyTypes());
                Assert.That(res.Name.CSharpFullName, Is.EqualTo("global::LangExt.Seq<global::LangExt.Seq<int>>"));
            }
        }

        public class 型パラメータが多すぎる場合
        {
            readonly TypeSketch sut = Reflect.Type<Seq<int>>();

            [Test, ExpectedException(typeof(ArgumentException))]
            public void ApplyTypesで例外が発生する()
            {
                sut.ApplyTypes(Reflect.Type<int>().ApplyTypes());
            }

            [Test, ExpectedException(typeof(InvalidOperationException))]
            public void ApplyTypeSketchesで例外が発生する()
            {
                sut.ApplyTypeSketches(Reflect.Type<int>());
            }
        }

        public class 型パラメータが少なすぎる場合
        {
            readonly TypeSketch sut = Reflect.Type(typeof(Seq<>));

            [Test, ExpectedException(typeof(ArgumentException))]
            public void ApplyTypesで例外が発生する()
            {
                sut.ApplyTypes();
            }

            [Test, ExpectedException(typeof(ArgumentException))]
            public void ApplyTypeSketchesで例外が発生する()
            {
                sut.ApplyTypeSketches();
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
