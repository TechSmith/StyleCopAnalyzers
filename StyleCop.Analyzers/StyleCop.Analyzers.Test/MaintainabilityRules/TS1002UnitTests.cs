// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Threading;
    using System.Threading.Tasks;
    using Analyzers.MaintainabilityRules;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Testing;
    using TestHelper;
    using Xunit;
   using static StyleCop.Analyzers.MaintainabilityRules.TS1002NumericLiteralsMustHaveSuffix;
   using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.TS1002NumericLiteralsMustHaveSuffix,
        StyleCop.Analyzers.MaintainabilityRules.TS1002CodeFixProvider>;

    public class TS1002UnitTests
    {
        [Fact]
        public async Task IntLiteral_NoSuffix_NoProblemAsync()
        {
            var testCode = @"public class TestClass
{
    public void TestMethod()
    {
        int n = 1;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

      [Fact]
      public async Task DoubleLiteral_WithSuffix_NoProblemAsync()
      {
         var testCode = @"public class TestClass
{
    public void TestMethod()
    {
        double d1 = 1d;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

      [Fact]
      public async Task DoubleLiteral_NoSuffix_ShouldHaveSuffixAsync()
      {
         var testCode = @"public class TestClass
{
    public void TestMethod()
    {
        double d1 = 1.0;
    }
}";

         var expectedDiagnostic = Diagnostic(DescriptorNotFollowed).WithLocation(5, 21);
         await VerifyCSharpDiagnosticAsync( testCode, expectedDiagnostic, CancellationToken.None ).ConfigureAwait( false );
      }

      [Fact]
      public async Task DoubleLiteral_NoSuffix_ProperFixAsync()
      {
         var testCode = @"public class TestClass
{
    public void TestMethod()
    {
        double d1 = 1.0;
    }
}";

         var fixedCode = @"public class TestClass
{
    public void TestMethod()
    {
        double d1 = 1d;
    }
}";

         var expectedDiagnostic = Diagnostic(DescriptorNotFollowed).WithLocation(5, 21);
         await VerifyCSharpFixAsync(testCode, expectedDiagnostic, fixedCode, CancellationToken.None).ConfigureAwait(false);
      }
    }
}
