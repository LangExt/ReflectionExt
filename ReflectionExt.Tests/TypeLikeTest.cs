using System;
using LangExt;
using ReflectionExt;
using NUnit.Framework;

namespace ReflectionExt.Tests
{
    [TestFixture]
    public class TypeLikeTest
    {
        [Test]
        public void 名前を表すオブジェクトが取得できる()
        {
            var sut = Reflect.Type<int>();
            Assert.That(sut.Name, Is.EqualTo(Name.Of(typeof(int))));
            Assert.That(sut.ApplyTypes().Name, Is.EqualTo(Name.Of(typeof(int))));
        }

        [Test]
        public void ネストした型がすべて取得できる()
        {
            var sut = Reflect.Type<Parent>();
            Assert.That(sut.AllNestedTypes.Size(), Is.EqualTo(5));
        }

        [Test]
        public void ネストした型がpublicなもののみ取得できる()
        {
            var sut = Reflect.Type<Parent>();
            Assert.That(sut.PublicNestedTypes, Is.EqualTo(Seq.Singleton(Reflect.Type<Parent.Nested1>())));
        }
    }
    public class Parent
    {
        public class Nested1 { }
        internal class Nested2 { }
        private class Nested3 { }
        protected class Nested4 { }
        protected internal class Nested5 { }
    }
}
