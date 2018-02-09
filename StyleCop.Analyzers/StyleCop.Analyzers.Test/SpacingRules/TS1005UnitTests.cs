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
    using static StyleCop.Analyzers.SpacingRules.TS1005AssignmentsShouldBeAligned;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.TS1005AssignmentsShouldBeAligned,
        StyleCop.Analyzers.SpacingRules.TS1005CodeFixProvider>;

    /// <summary>
    /// Unit tests for the <see cref="TS1005AssignmentsShouldBeAligned"/> class.
    /// </summary>
    public class TS1005UnitTests
   {
      [Fact]
      public async Task SimpleClass_AssignmentsLinedUp_NoErrorsAsync()
      {
         var testCode = @"using System;
namespace TestNamespace
{
   public class Whatever
   {
      public static void SomeWhere()
      {
         var ub = new UriBuilder
         {
            Host  = ""Host"",
            Path  = ""redirect.asp"",
            Query = ""Query""
         };
      }
   }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

      [Fact]
      public async Task SimpleClass_AssignemntNotLinedUp_TwoErrorsAsync()
      {
         var testCode = @"using System;
namespace TestNamespace
{
   public class Whatever
   {
      public static void SomeWhere()
      {
         var ub = new UriBuilder
         {
            Host = ""Host"",
            Path = ""redirect.asp"",
            Query = ""Query""
         };
      }
   }
}
";
         DiagnosticResult[] expectedDiagnostic =
         {
            Diagnostic(Descriptor).WithLocation(10, 18),
            Diagnostic(Descriptor).WithLocation(11, 18)
         };

         await VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
        }

      [Fact]
      public async Task SimpleClass_AssignmentsNotLinedUp_FixedErrorAsync()
      {
         var testCode = @"using System;
namespace TestNamespace
{
   public class Whatever
   {
      public static void SomeWhere()
      {
         var ub = new UriBuilder
         {
            Host = ""Host"",
            Path = ""redirect.asp"",
            Query = ""Query""
         };
      }
   }
}
";

         var fixedCode = @"using System;
namespace TestNamespace
{
   public class Whatever
   {
      public static void SomeWhere()
      {
         var ub = new UriBuilder
         {
            Host  = ""Host"",
            Path  = ""redirect.asp"",
            Query = ""Query""
         };
      }
   }
}
";

         DiagnosticResult[] expectedDiagnostic =
         {
            Diagnostic(Descriptor).WithLocation(10, 18),
            Diagnostic(Descriptor).WithLocation(11, 18)
         };

            await VerifyCSharpFixAsync(testCode, expectedDiagnostic, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
