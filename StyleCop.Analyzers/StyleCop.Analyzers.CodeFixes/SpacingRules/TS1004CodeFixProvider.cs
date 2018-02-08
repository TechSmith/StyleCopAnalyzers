// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.SpacingRules
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Threading;
    using System.Threading.Tasks;
    using Helpers;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// Implements a code fix for <see cref="SA1028CodeMustNotContainTrailingWhitespace"/>.
    /// </summary>
    /// <remarks>
    /// <para>To fix a violation of this rule, remove any whitespace at the end of a line of code.</para>
    /// </remarks>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TS1004CodeFixProvider))]
    [Shared]
    internal class TS1004CodeFixProvider : CodeFixProvider
    {
        /// <inheritdoc/>
        public override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(TS1004GettersAndSettersShouldBeAligned.DiagnosticId);

        /// <inheritdoc/>
        public sealed override FixAllProvider GetFixAllProvider()
        {
            return FixAll.Instance;
        }

        /// <inheritdoc/>
        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var settings = SettingsHelper.GetStyleCopSettings(context.Document.Project.AnalyzerOptions, context.CancellationToken);
            foreach (var diagnostic in context.Diagnostics)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        "Fix mis-aligned getters and setters :)",
                        ct => FixWhitespaceAsync( context.Document, diagnostic, settings.AlignmentRules.AlignmentStandardDeviation, ct),
                        nameof( TS1004CodeFixProvider ) ),
                    diagnostic);
            }

            return SpecializedTasks.CompletedTask;
        }

        private static async Task<Document> FixWhitespaceAsync(Document document, Diagnostic diagnostic, double dAlignmentStandardDeviation, CancellationToken cancellationToken)
        {
            var text = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);
            SyntaxNode root = await document.GetSyntaxRootAsync( cancellationToken ).ConfigureAwait( false );
            int nCharToAdd = await AlignmentHelper.AdditionalSpacesToAddAsync( root, diagnostic, dAlignmentStandardDeviation, cancellationToken ).ConfigureAwait( false );
            var newText = text.Replace( new TextSpan( diagnostic.Location.SourceSpan.Start, 0 ), AlignmentHelper.WhiteSpaceString( nCharToAdd ) );
            return document.WithText( newText );
        }

        private class FixAll : DocumentBasedFixAllProvider
        {
            public static FixAllProvider Instance { get; } =
                new FixAll();

            protected override string CodeActionTitle =>
                "Fix mis-aligned getters and setters :)";

            protected override async Task<SyntaxNode> FixAllInDocumentAsync(FixAllContext fixAllContext, Document document, ImmutableArray<Diagnostic> diagnostics)
            {
                if (diagnostics.IsEmpty)
                {
                    return null;
                }

                var settings = SettingsHelper.GetStyleCopSettings( document.Project.AnalyzerOptions, fixAllContext.CancellationToken);
                var text = await document.GetTextAsync().ConfigureAwait(false);
                SyntaxNode root = await document.GetSyntaxRootAsync().ConfigureAwait( false );

                for (int nDiagnostic = diagnostics.Length; nDiagnostic-->0; )
                {
                   var diagnostic = diagnostics[nDiagnostic];
                   int nCharToAdd = await AlignmentHelper.AdditionalSpacesToAddAsync( root, diagnostic, settings.AlignmentRules.AlignmentStandardDeviation, fixAllContext.CancellationToken ).ConfigureAwait( false );
                   text = text.Replace( new TextSpan( diagnostic.Location.SourceSpan.Start, 0 ), AlignmentHelper.WhiteSpaceString( nCharToAdd ) );
                }

                var tree = await document.GetSyntaxTreeAsync().ConfigureAwait(false);
                return await tree.WithChangedText(text).GetRootAsync().ConfigureAwait(false);
            }
        }
    }
}
