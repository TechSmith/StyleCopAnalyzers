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
    using static StyleCop.Analyzers.SpacingRules.TS1004GettersAndSettersShouldBeAligned;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.TS1004GettersAndSettersShouldBeAligned,
        StyleCop.Analyzers.SpacingRules.TS1004CodeFixProvider>;

    /// <summary>
    /// Unit tests for the <see cref="TS1004GettersAndSettersShouldBeAligned"/> class.
    /// </summary>
    public class TS1004UnitTests
   {
      private const string TestSettings = @"
{
  ""settings"": {
    ""alignmentRules"": {
      ""alignmentStandardDeviation"": ""1.0""
    }
  }
}
";

      [Fact]
      public async Task SimpleClass_GettersSettersLinedUp_NoErrorsAsync()
      {
         var testCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id               { get; set; }
      public string FilePath      { get; set; }
      public string RequestSource { get; set; }
   }
}
";

         await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
      }

      [Fact]
      public async Task SimpleClass_GettersSettersNotLinedUp_TwoErrorsAsync()
      {
         var testCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id { get; set; }
      public string FilePath { get; set; }
      public string RequestSource { get; set; }
   }
}
";

         DiagnosticResult[] expectedDiagnostic =
         {
            Diagnostic(Descriptor).WithLocation(5, 21),
            Diagnostic(Descriptor).WithLocation(6, 30)
         };

         await VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
      }

      [Fact]
      public async Task SimpleClass_GettersSettersNotLinedUpWithNewLines_NoErrorsAsync()
      {
         var testCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id { get; set; }

      public string FilePath { get; set; }

      public string RequestSource { get; set; }
   }
}
";

         await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
      }

      [Fact]
      public async Task SimpleClass_GettersSettersNotLinedUp_FixedErrorAsync()
      {
         var testCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id { get; set; }
      public string FilePath { get; set; }
   }
}
";

         var fixedCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id          { get; set; }
      public string FilePath { get; set; }
   }
}
";

        var expectedDiagnostic = Diagnostic(Descriptor).WithLocation(5, 21);
        await VerifyCSharpFixAsync(testCode, new[] { expectedDiagnostic }, fixedCode, CancellationToken.None).ConfigureAwait(false);
      }

      [Fact]
      public async Task SimpleClass_GettersSettersNotLinedUp_FixedErrorsAsync()
      {
         var testCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id { get; set; }
      public string FilePath { get; set; }
      public string RequestSource { get; set; }
   }
}
";

         var fixedTestCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id               { get; set; }
      public string FilePath      { get; set; }
      public string RequestSource { get; set; }
   }
}
";

        DiagnosticResult[] expectedDiagnostic =
            {
            Diagnostic(Descriptor).WithLocation(5, 21),
            Diagnostic(Descriptor).WithLocation(6, 30)
            };

        await VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
      }

      [Fact]
      public async Task SimpleClass_GettersSettersLastOneNotLinedUpWithNewLines_AlignmentErrorAsync()
      {
         var testCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id               { get; set; }
      public string RequestSource { get; set; }
      public string FilePath { get; set; }
   }
}
";

         var expectedDiagnostic = Diagnostic(Descriptor).WithLocation(7, 30);
         await VerifyCSharpDiagnosticAsync(testCode, new[] { expectedDiagnostic }, CancellationToken.None).ConfigureAwait(false);
      }

      [Fact]
      public async Task SimpleClass_GettersSettersLastOneNotLinedUpWithNewLines_ErrorFixedAsync()
      {
         var testCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id               { get; set; }
      public string RequestSource { get; set; }
      public string FilePath { get; set; }
   }
}
";

         var fixedCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id               { get; set; }
      public string RequestSource { get; set; }
      public string FilePath      { get; set; }
   }
}
";

        var expectedDiagnostic = Diagnostic(Descriptor).WithLocation(7, 30);
        await VerifyCSharpFixAsync(testCode, new[] { expectedDiagnostic }, fixedCode, CancellationToken.None).ConfigureAwait(false);
      }

      [Fact]
      public async Task SimpleClass_PrivateGettersSettersNotLinedUpWithNewLines_ErrorFixedAsync()
      {
         var testCode = @"namespace TestNamespace
{
   public class Asset30Node
   {
      public string Name { get; private set; }
      public string DisplayName { get; private set; }
   }
}
";

         var fixedCode = @"namespace TestNamespace
{
   public class Asset30Node
   {
      public string Name        { get; private set; }
      public string DisplayName { get; private set; }
   }
}
";

        var expectedDiagnostic = Diagnostic(Descriptor).WithLocation(5, 26);
        await VerifyCSharpFixAsync(testCode, new[] { expectedDiagnostic }, fixedCode, CancellationToken.None).ConfigureAwait(false);
      }

      [Fact]
      public async Task SimpleClass_GettersSettersNotLinedWithLongOne_ErrorFixedAsync()
      {
         var testCode = @"namespace TestNamespace
{
   public class PartnersArgs
   {
      public bool PowerPointPromptAfterRecording { get; set; }
      public string ExtraLongVariableCalledPowerPointSlideNotesImportChoice { get; set; }
      public string MobileSharingConnectionName { get; set; }
      public string MobileSharingTempDirectory { get; set; }
      public string GoogleDriveTempDirectory { get; set; }
   }
}
";

         var fixedTestCode = @"namespace TestNamespace
{
   public class PartnersArgs
   {
      public bool PowerPointPromptAfterRecording { get; set; }
      public string ExtraLongVariableCalledPowerPointSlideNotesImportChoice { get; set; }
      public string MobileSharingConnectionName  { get; set; }
      public string MobileSharingTempDirectory   { get; set; }
      public string GoogleDriveTempDirectory     { get; set; }
   }
}
";
         DiagnosticResult[] expectedDiagnostic =
         {
            Diagnostic(Descriptor).WithLocation(7, 49),
            Diagnostic(Descriptor).WithLocation(8, 48),
            Diagnostic(Descriptor).WithLocation(9, 46)
         };

         await VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
      }

      [Fact]
      public async Task SimpleClass_GettersSettersNotLinedUp_ErrorFixedAsync()
      {
         var testCode = @"namespace TestNamespace
{
   public class MediaBinConfig
   {
      public int     ColumnSortFlag { get; set; }
      public int     ColumnDisplayFlag { get; set; }
      public bool    ShowListView { get; set; }
      public int     ColumnSortDescending { get; set; }
   }
}
";

         // If we change the tolerance this fixed code might need to be adjusted
         var fixedTestCode = @"namespace TestNamespace
{
   public class MediaBinConfig
   {
      public int     ColumnSortFlag    { get; set; }
      public int     ColumnDisplayFlag { get; set; }
      public bool    ShowListView      { get; set; }
      public int     ColumnSortDescending { get; set; }
   }
}
";

         // If we change the tolerance you might need another diagnostic
         DiagnosticResult[] expectedDiagnostic =
         {
            Diagnostic(Descriptor).WithLocation(5, 37),
            Diagnostic(Descriptor).WithLocation(7, 35)
         };

         await VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
      }

        private static Task VerifyCSharpDiagnosticAsync(string source, DiagnosticResult[] expected, CancellationToken cancellationToken)
        {
            var test = new StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier <
        StyleCop.Analyzers.SpacingRules.TS1004GettersAndSettersShouldBeAligned,
        StyleCop.Analyzers.SpacingRules.TS1004CodeFixProvider >.CSharpTest
            {
                TestCode = source,
                Settings = TestSettings,
            };

            test.ExpectedDiagnostics.AddRange(expected);
            return test.RunAsync(cancellationToken);
        }

        private static Task VerifyCSharpFixAsync(string source, DiagnosticResult[] expected, string fixedSource, CancellationToken cancellationToken)
        {
            var test = new StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.TS1004GettersAndSettersShouldBeAligned,
        StyleCop.Analyzers.SpacingRules.TS1004CodeFixProvider>.CSharpTest
            {
                TestCode = source,
                FixedCode = fixedSource,
                Settings = TestSettings,
            };

            test.ExpectedDiagnostics.AddRange(expected);
            return test.RunAsync(cancellationToken);
        }
    }
}
