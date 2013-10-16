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
            Name sut = Name.FromType(typeof(int));
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
            Name name1 = t1 == null ? null : Name.FromType(t1);
            Name name2 = t2 == null ? null : Name.FromType(t2);
            Assert.That(t1 == t2, Is.EqualTo(expected));
            Assert.That(t1 != t2, Is.Not.EqualTo(expected));
        }

        public class int型
        {
            Name sut = Name.FromType(typeof(int));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Int32")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo("System.Int32")); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("System")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("int")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("int")); }
        }

        public class DateTime型
        {
            Name sut = Name.FromType(typeof(DateTime));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("DateTime")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo("System.DateTime")); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("System")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("DateTime")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::System.DateTime")); }
        }

        public class intのnull許容型
        {
            Name sut = Name.FromType(typeof(int?));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Nullable`1")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo(typeof(int?).FullName)); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("System")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("int?")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("int?")); }
        }

        public class DateTimeのnull許容型
        {
            Name sut = Name.FromType(typeof(DateTime?));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Nullable`1")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo(typeof(DateTime?).FullName)); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("System")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("DateTime?")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::System.DateTime?")); }
        }

        public class Seq型
        {
            Name sut = Name.FromType(typeof(Seq<>));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Seq`1")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo("LangExt.Seq`1")); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("LangExt")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("Seq<T>")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::LangExt.Seq<T>")); }
        }

        public class intのSeq型
        {
            Name sut = Name.FromType(typeof(Seq<int>));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Seq`1")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo(typeof(Seq<int>).FullName)); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("LangExt")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("Seq<int>")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::LangExt.Seq<int>")); }
        }

        public class ジェネリック型にネストした非ジェネリック型
        {
            Name sut = Name.FromType(typeof(Parent<int>.Nested));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Nested")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo(typeof(Parent<int>.Nested).FullName)); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("ReflectionExt.Tests")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("Nested")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.Parent<int>.Nested")); }
        }

        public class ジェネリック型にネストしたジェネリック型
        {
            Name sut = Name.FromType(typeof(Parent<int>.Nested<string>));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Nested`1")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo(typeof(Parent<int>.Nested<string>).FullName)); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("ReflectionExt.Tests")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("Nested<string>")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.Parent<int>.Nested<string>")); }
        }

        public class 複雑なネスト型
        {
            Name sut = Name.FromType(typeof(Parent<int>.Nested<string, int>.Nested2.Nested3.Nested4<bool>.Nested5));

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Nested5")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.EqualTo(typeof(Parent<int>.Nested<string, int>.Nested2.Nested3.Nested4<bool>.Nested5).FullName)); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("ReflectionExt.Tests")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("Nested5")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.Parent<int>.Nested<string, int>.Nested2.Nested3.Nested4<bool>.Nested5")); }
        }

        public class 中途半端に型指定された型
        {
            Name sut = Name.FromType(typeof(Sub<>).BaseType);

            [Test] public void 単純名が取得できる() { Assert.That(sut.Value, Is.EqualTo("Nested`2")); }
            [Test] public void 完全名が取得できる() { Assert.That(sut.FullName, Is.Null); }
            [Test] public void 名前空間が取得できる() { Assert.That(sut.Namespace, Is.EqualTo("ReflectionExt.Tests")); }
            [Test] public void CSharpライクな名前が取得できる() { Assert.That(sut.CSharpName, Is.EqualTo("Nested<T, bool>")); }
            [Test] public void CSharpライクな完全名が取得できる() { Assert.That(sut.CSharpFullName, Is.EqualTo("global::ReflectionExt.Tests.Parent<T>.Nested<T, bool>")); }
        }
    }

    public class Parent<T>
    {
        public class Nested { }
        public class Nested<U> { }
        public class Nested<T1, T2>
        {
            public class Nested2
            {
                public class Nested3
                {
                    public class Nested4<T3>
                    {
                        public class Nested5 { }
                    }
                }
            }
        }
    }
    public class Sub<T> : Parent<T>.Nested<T, bool> { }
}
