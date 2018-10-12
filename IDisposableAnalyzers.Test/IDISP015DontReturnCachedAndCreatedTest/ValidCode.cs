namespace IDisposableAnalyzers.Test.IDISP015DontReturnCachedAndCreatedTest
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using NUnit.Framework;

    public class ValidCode
    {
        private static readonly DiagnosticAnalyzer Analyzer = new MethodReturnValuesAnalyzer();
        private static readonly DiagnosticDescriptor Descriptor = IDISP015DontReturnCachedAndCreated.Descriptor;

        [Test]
        public void WhenRetuningCreated()
        {
            var testCode = @"
namespace RoslynSandbox
{
    using System;
    using System.IO;

    public sealed class Foo
    {
        public IDisposable Bar()
        {
            return File.OpenRead(string.Empty);
        }
    }
}";
            AnalyzerAssert.Valid(Analyzer, Descriptor, testCode);
        }

        [Test]
        public void WhenRetuningInjected()
        {
            var testCode = @"
namespace RoslynSandbox
{
    using System;

    public sealed class Foo
    {
        private readonly IDisposable disposable;

        public Foo(IDisposable disposable)
        {
            this.disposable = disposable;
        }

        public IDisposable Bar()
        {
            return this.disposable;
        }
    }
}";
            AnalyzerAssert.Valid(Analyzer, Descriptor, testCode);
        }
    }
}
