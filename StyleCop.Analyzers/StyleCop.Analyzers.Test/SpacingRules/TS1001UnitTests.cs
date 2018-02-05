// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.SpacingRules
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.SpacingRules;
    using TestHelper;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.TS1001ClosingParenthesisMustBePrecededByASpace;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.TS1001ClosingParenthesisMustBePrecededByASpace,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    /// <summary>
    /// Unit tests for the <see cref="TS1001ClosingParenthesisMustBePrecededByASpace"/> class.
    /// </summary>
    public class TS1001UnitTests
   {
      [Fact]
      public async Task IfStatement_FineParens_NoErrorsAsync()
      {
         var testCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod()
        {
            int n = 1;
            if( n == 1 ){}
        }
    }
}
";

         await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

      [Fact]
      public async Task CallMethod_FineParens_NoErrorsAsync()
      {
         var testCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void SomeMethod()
        {
        }
        public void TestMethod()
        {
            SomeMethod();
        }
    }
}
";

         await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

      [Fact]
      public async Task CastType_FineParens_NoErrorsAsync()
      {
         var testCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod()
        {
            double d = 1d;
            int n = (int)d;
        }
    }
}
";

         await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

      [Fact]
      public async Task CastTypeWithExtraParens_FineParens_NoErrorsAsync()
      {
         var testCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod()
        {
            double d = 1d;
            int n = ( (int)d );
        }
    }
}
";

         await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

      [Fact]
      public async Task IfStatement_WrongParens_ReportsAndFixesAsync()
      {
         var testCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod()
        {
            int n = 1;
            if( n == 1){}
        }
    }
}
";

         var fixedCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod()
        {
            int n = 1;
            if( n == 1 ){}
        }
    }
}
";

         var expectedDiagnostic = Diagnostic(DescriptorNotFollowed).WithLocation(8, 23);
         await VerifyCSharpFixAsync(testCode, expectedDiagnostic, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
   }
}
