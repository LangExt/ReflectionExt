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

        [Test]
        public void OpenTypeをTypeSketchに変換できる()
        {
            OpenType sut = Reflect.Type<Seq<int>>().UnapplyTypes().GetOr(null);
            Assert.That(sut.ToSketch(), Is.EqualTo(Reflect.Type(typeof(Seq<>))));
        }

        [Test]
        public void OpenTypeをTypeSketchに暗黙変換できる()
        {
            OpenType sut = Reflect.Type<Seq<int>>().UnapplyTypes().GetOr(null);
            try
            {
                TypeSketch sketch = sut;
                Assert.That(sketch, Is.EqualTo(Reflect.Type(typeof(Seq<>))));
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
