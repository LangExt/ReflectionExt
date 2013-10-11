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
    }
}
