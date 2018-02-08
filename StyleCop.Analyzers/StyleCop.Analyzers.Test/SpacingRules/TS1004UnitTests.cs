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
    using StyleCop.Analyzers.SpacingRules;
    using TestHelper;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.TS1004GettersAndSettersShouldBeAligned;

   /// <summary>
   /// Unit tests for the <see cref="TS1004GettersAndSettersShouldBeAligned"/> class.
   /// </summary>
   public class TS1004UnitTests : CodeFixVerifier
   {
      private string multiLineSettings;

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

         await this.VerifyCSharpDiagnosticAsync( testCode, EmptyDiagnosticResults, CancellationToken.None ).ConfigureAwait( false );
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
            this.CSharpDiagnostic(Descriptor).WithLocation(5, 21),
            this.CSharpDiagnostic(Descriptor).WithLocation(6, 30)
         };

         await this.VerifyCSharpDiagnosticAsync( testCode, expectedDiagnostic, CancellationToken.None ).ConfigureAwait( false );
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

         await this.VerifyCSharpDiagnosticAsync( testCode, EmptyDiagnosticResults, CancellationToken.None ).ConfigureAwait( false );
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

         var fixedTestCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id          { get; set; }
      public string FilePath { get; set; }
   }
}
";

         DiagnosticResult[] expectedDiagnostic =
         {
            this.CSharpDiagnostic(Descriptor).WithLocation(5, 21)
         };

         await this.VerifyCSharpDiagnosticAsync( testCode, expectedDiagnostic, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpDiagnosticAsync( fixedTestCode, EmptyDiagnosticResults, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpFixAsync( testCode, fixedTestCode ).ConfigureAwait( false );
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
            this.CSharpDiagnostic(Descriptor).WithLocation(5, 21),
            this.CSharpDiagnostic(Descriptor).WithLocation(6, 30)
         };

         await this.VerifyCSharpDiagnosticAsync( testCode, expectedDiagnostic, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpDiagnosticAsync( fixedTestCode, EmptyDiagnosticResults, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpFixAllFixAsync( testCode, fixedTestCode ).ConfigureAwait( false );
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

         DiagnosticResult[] expectedDiagnostic =
         {
            this.CSharpDiagnostic(Descriptor).WithLocation(7, 30)
         };

         await this.VerifyCSharpDiagnosticAsync( testCode, expectedDiagnostic, CancellationToken.None ).ConfigureAwait( false );
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

         var fixedTestCode = @"namespace TestNamespace
{
   public class ThumbnailLoadedEventArgs
   {
      public int Id               { get; set; }
      public string RequestSource { get; set; }
      public string FilePath      { get; set; }
   }
}
";

         DiagnosticResult[] expectedDiagnostic =
         {
            this.CSharpDiagnostic(Descriptor).WithLocation(7, 30)
         };

         await this.VerifyCSharpDiagnosticAsync( testCode, expectedDiagnostic, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpDiagnosticAsync( fixedTestCode, EmptyDiagnosticResults, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpFixAllFixAsync( testCode, fixedTestCode ).ConfigureAwait( false );
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

         var fixedTestCode = @"namespace TestNamespace
{
   public class Asset30Node
   {
      public string Name        { get; private set; }
      public string DisplayName { get; private set; }
   }
}
";

         DiagnosticResult[] expectedDiagnostic =
         {
            this.CSharpDiagnostic(Descriptor).WithLocation(5, 26)
         };

         await this.VerifyCSharpDiagnosticAsync( testCode, expectedDiagnostic, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpDiagnosticAsync( fixedTestCode, EmptyDiagnosticResults, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpFixAllFixAsync( testCode, fixedTestCode ).ConfigureAwait( false );
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
            this.CSharpDiagnostic(Descriptor).WithLocation(7, 49),
            this.CSharpDiagnostic(Descriptor).WithLocation(8, 48),
            this.CSharpDiagnostic(Descriptor).WithLocation(9, 46)
         };

         await this.VerifyCSharpDiagnosticAsync( testCode, expectedDiagnostic, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpDiagnosticAsync( fixedTestCode, EmptyDiagnosticResults, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpFixAllFixAsync( testCode, fixedTestCode ).ConfigureAwait( false );
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

         this.multiLineSettings = @"
{
  ""settings"": {
    ""alignmentRules"": {
      ""alignmentStandardDeviation"": ""1.0""
    }
  }
}
";

         // If we change the tolerance you might need another diagnostic
         DiagnosticResult[] expectedDiagnostic =
         {
            this.CSharpDiagnostic(Descriptor).WithLocation(5, 37),
            this.CSharpDiagnostic(Descriptor).WithLocation(7, 35)
         };

         await this.VerifyCSharpDiagnosticAsync( testCode, expectedDiagnostic, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpDiagnosticAsync( fixedTestCode, EmptyDiagnosticResults, CancellationToken.None ).ConfigureAwait( false );
         await this.VerifyCSharpFixAllFixAsync( testCode, fixedTestCode ).ConfigureAwait( false );
      }

      protected override IEnumerable<DiagnosticAnalyzer> GetCSharpDiagnosticAnalyzers()
      {
         yield return new TS1004GettersAndSettersShouldBeAligned();
      }

      protected override CodeFixProvider GetCSharpCodeFixProvider()
      {
         return new TS1004CodeFixProvider();
      }

      protected override string GetSettings()
      {
         return this.multiLineSettings ?? base.GetSettings();
      }
   }
}
