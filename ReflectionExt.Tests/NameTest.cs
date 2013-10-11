using System;
using LangExt;
using ReflectionExt;
using NUnit.Framework;

namespace ReflectionExt.Tests
{
    [TestFixture]
    public class NameTest
    {
        [Test]
        public void 型からNameが構築できる()
        {
            Name sut = Name.Of(typeof(int));
            Assert.That(sut, Is.Not.Null);
        }

        [TestCase(typeof(int), typeof(int), true)]
        [TestCase(typeof(int), typeof(string), false)]
        [TestCase(typeof(string), typeof(int), false)]
        [TestCase(typeof(int), null, false)]
        [TestCase(null, typeof(int), false)]
        [TestCase(null, null, true)]
        public void Nameどうしの比較ができる(Type t1, Type t2, bool expected)
        {
            Name name1 = t1 == null ? null : Name.Of(t1);
            Name name2 = t2 == null ? null : Name.Of(t2);
            Assert.That(t1 == t2, Is.EqualTo(expected));
            Assert.That(t1 != t2, Is.Not.EqualTo(expected));
        }

        public class int型
        {
            Name sut = Name.Of(typeof(int));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Int32")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo("System.Int32")); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("System")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("int")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("int")); }
        }

        public class DateTime型
        {
            Name sut = Name.Of(typeof(DateTime));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("DateTime")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo("System.DateTime")); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("System")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("DateTime")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::System.DateTime")); }
        }

        public class intのnull許容型
        {
            Name sut = Name.Of(typeof(int?));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Nullable`1")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo(typeof(int?).FullName)); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("System")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("int?")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("int?")); }
        }

        public class DateTimeのnull許容型
        {
            Name sut = Name.Of(typeof(DateTime?));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Nullable`1")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo(typeof(DateTime?).FullName)); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("System")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("DateTime?")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::System.DateTime?")); }
        }

        public class Seq型
        {
            Name sut = Name.Of(typeof(Seq<>));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Seq`1")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo("LangExt.Seq`1")); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("LangExt")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("Seq<T>")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::LangExt.Seq<T>")); }
        }

        public class intのSeq型
        {
            Name sut = Name.Of(typeof(Seq<int>));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Seq`1")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo(typeof(Seq<int>).FullName)); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("LangExt")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("Seq<int>")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::LangExt.Seq<int>")); }
        }
    }
}
