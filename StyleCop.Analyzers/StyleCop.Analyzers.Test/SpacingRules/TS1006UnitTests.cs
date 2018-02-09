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
    using static StyleCop.Analyzers.SpacingRules.TS1006SubscribingShouldBeAligned;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.TS1006SubscribingShouldBeAligned,
        StyleCop.Analyzers.SpacingRules.TS1006CodeFixProvider>;

    /// <summary>
    /// Unit tests for the <see cref="TS1006SubscribingShouldBeAligned"/> class.
    /// </summary>
    public class TS1006UnitTests
   {

      [Fact]
      public async Task SimpleClass_SubscribedToHandlerLinedUp_NoErrorsAsync()
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
      }
   }
}
";

         await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
      }

      [Fact]
      public async Task SimpleClass_HandlerSubscriptionNotLinedUp_OneErrorsAsync()
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
         handler1 += Whatever;
         handlerLonger += Whatever;
      }
   }
}
";

         DiagnosticResult[] expectedDiagnostic =
         {
            Diagnostic(Descriptor).WithLocation(11, 19)
         };

         await VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
        }

      [Fact]
      public async Task SimpleClass_HandlerSubscriptionNotLinedUp_FixedErrorAsync()
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
         handler1 += Whatever;
         handlerLonger += Whatever;
      }
   }
}
";

         var fixedCode = @"using System;
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
      }
   }
}
";

        var expectedDiagnostic = Diagnostic(Descriptor).WithLocation(11, 19);
        await VerifyCSharpFixAsync(testCode, new[] { expectedDiagnostic }, fixedCode, CancellationToken.None).ConfigureAwait(false);
      }
   }
}
