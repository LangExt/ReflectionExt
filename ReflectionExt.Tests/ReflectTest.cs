using System;
using LangExt;
using ReflectionExt;
using NUnit.Framework;

namespace ReflectionExt.Tests
{
    [TestFixture]
    public class ReflectTest
    {
        [Test]
        public void intに関するTypeSketchがTypeから構築できる()
        {
            TypeSketch sut = Reflect.Type(typeof(int));
            Assert.That(sut.ToType(), Is.EqualTo(typeof(int)));
        }

        [Test]
        public void intに関するTypeSketchが型パラメータで構築できる()
        {
            TypeSketch sut = Reflect.Type<int>();
            Assert.That(sut, Is.EqualTo(Reflect.Type(typeof(int))));
        }

        [Test]
        public void intに関するTypeSketchのOptionが文字列から構築できる()
        {
            Option<TypeSketch> sut = Reflect.Type("System.Int32");
            Assert.That(sut, Is.EqualTo(Option.Some(Reflect.Type<int>())));
        }

        [Test]
        public void intに関するTypeSketchが文字列から構築できる()
        {
            TypeSketch sut = Reflect.TypeUnsafe("System.Int32");
            Assert.That(sut, Is.EqualTo(Reflect.Type<int>()));
        }

        [Test]
        public void intのSeqに関するTypeSketchが構築できる()
        {
            TypeSketch sut = Reflect.Type<Seq<int>>();
            Assert.That(sut.ToType(), Is.EqualTo(typeof(Seq<int>)));
        }

        [Test]
        public void 型パラメータを指定しないTypeSketchが構築できる()
        {
            TypeSketch sut = Reflect.Type(typeof(Seq<>));
            Assert.That(sut.ToType(), Is.EqualTo(typeof(Seq<>)));
        }
    }
}
