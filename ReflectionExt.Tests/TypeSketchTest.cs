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
    }
}
