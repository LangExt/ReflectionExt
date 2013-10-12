using System;
using LangExt;
using ReflectionExt;
using NUnit.Framework;

namespace ReflectionExt.Tests
{
    [TestFixture]
    public class OpenTypeTest
    {
        [Test]
        public void ApplyTypesでSeqを表すOpenTypeからClosedTypeに変換できる()
        {
            OpenType sut = Reflect.Type<Seq<int>>().UnapplyTypes().GetOr(null);
            Assert.That(sut.ApplyTypes(Reflect.Type<int>().ApplyTypes()).ToType(), Is.EqualTo(typeof(Seq<int>)));
        }
    }
}
