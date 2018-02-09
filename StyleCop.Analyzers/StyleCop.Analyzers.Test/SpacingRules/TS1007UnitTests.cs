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
    using static StyleCop.Analyzers.SpacingRules.TS1007UnsubscribingShouldBeAligned;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.TS1007UnsubscribingShouldBeAligned,
        StyleCop.Analyzers.SpacingRules.TS1007CodeFixProvider>;

    /// <summary>
    /// Unit tests for the <see cref="TS1006SubscribingShouldBeAligned"/> class.
    /// </summary>
    public class TS1007UnitTests
   {
      [Fact]
      public async Task SimpleClass_UnsubscribedToHandlerLinedUp_NoErrorsAsync()
      {
         var testCode = @"using System;
namespace TestNamespace
{
   public class WhateverClass
   {
      event EventHandler<bool> handler1;
      event EventHandler<bool> handlerLonger;
      private void Whatever( object sender, bool b ) { }
      public void SomeWhere()
      {
         handler1      += Whatever;
         handlerLonger += Whatever;

         handler1      -= Whatever;
         handlerLonger -= Whatever;
      }
   }
}
";

         await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
      }
   }
}
