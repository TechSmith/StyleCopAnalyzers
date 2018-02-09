// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.SpacingRules
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Linq;
   using Helpers;
   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp;
   using Microsoft.CodeAnalysis.Diagnostics;
   using Settings.ObjectModel;

   /// <summary>
   /// Assignments should be aligned
   /// </summary>
   /// <remarks>
   /// <para>A violation of this rule occurs whenever assignments are not aligned causing hard to read code.</para>
   /// </remarks>
   [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class TS1007UnsubscribingShouldBeAligned : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1025CodeMustNotContainMultipleWhitespaceInARow"/>
        /// analyzer.
        /// </summary>
        public const string DiagnosticId = "TS1007";
        private static readonly LocalizableString Title = "Unsubscribing must be aligned :)";
        private static readonly LocalizableString Description = "Unsubscribing should be lined up for better readability :)";
        private static readonly string HelpLink = "https://github.com/TechSmith/CamtasiaWin/wiki/Automated-Code-Standards";

        private const string MessageNotFollowed = "Unsubscribing should be aligned :)";

        public static DiagnosticDescriptor Descriptor { get; } =
            new DiagnosticDescriptor( DiagnosticId, Title, MessageNotFollowed, AnalyzerCategory.SpacingRules, DiagnosticSeverity.Warning, AnalyzerConstants.EnabledByDefault, Description, HelpLink );

        private static readonly Action<SyntaxTreeAnalysisContext, StyleCopSettings> SyntaxTreeAction = HandleSyntaxTree;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxTreeAction(SyntaxTreeAction);
        }

        private static void HandleSyntaxTree(SyntaxTreeAnalysisContext context, StyleCopSettings settings )
        {
            SyntaxNode root = context.Tree.GetCompilationUnitRoot(context.CancellationToken);
            List<Location> listLocations = new List<Location>();
            foreach (var token in root.DescendantTokens(descendIntoTrivia: true).Where(t => t.IsKind(SyntaxKind.MinusEqualsToken)))
            {
               if ( AlignmentHelper.IsSimpleUnsubscribing( root, token) )
               {
                  int nMyLine = token.GetLocation().GetLineSpan().StartLinePosition.Line;
                  int nMyColumn = token.GetLocation().GetLineSpan().StartLinePosition.Character;
                  int nAdditonalSpaces = AlignmentHelper.AdditionalSpacesToAddForConditionAsync( root, nMyLine, nMyColumn, settings.AlignmentRules.AlignmentStandardDeviation, AlignmentHelper.IsSimpleUnsubscribing, SyntaxKind.MinusEqualsToken ).Result;
                  if ( nAdditonalSpaces > 0 )
                  {
                     context.ReportDiagnostic( Diagnostic.Create( Descriptor, token.GetLocation() ) );
                     continue;
                  }
               }
            }
        }
   }
}
