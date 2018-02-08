// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Helpers;
   using Microsoft.CodeAnalysis;
   using Microsoft.CodeAnalysis.CSharp;

   public static class AlignmentHelper
   {
      public static bool IsSimpleGetterSetter( SyntaxNode root, SyntaxToken token )
      {
         // Is next token a getter or a setter?
         var getSetToken = token.GetNextToken();

         if ( getSetToken.Kind() == SyntaxKind.PrivateKeyword ) // Could be a private getter
         {
            getSetToken = getSetToken.GetNextToken();
         }

         if ( getSetToken.Kind() != SyntaxKind.GetKeyword && getSetToken.Kind() != SyntaxKind.SetKeyword )
         {
            return false;
         }

         var possibleSemicolonToken = getSetToken.GetNextToken();

         if ( possibleSemicolonToken.Kind() != SyntaxKind.SemicolonToken )
         {
            return false;
         }

         var closeCurlyOrGetSetToken = possibleSemicolonToken.GetNextToken();

         if ( closeCurlyOrGetSetToken.Kind() == SyntaxKind.PrivateKeyword ) // Could be a private setter
         {
            closeCurlyOrGetSetToken = closeCurlyOrGetSetToken.GetNextToken();
         }

         if ( closeCurlyOrGetSetToken.Kind() == SyntaxKind.CloseBracketToken )
         {
            return true;
         }

         if ( closeCurlyOrGetSetToken.Kind() != SyntaxKind.GetKeyword && closeCurlyOrGetSetToken.Kind() != SyntaxKind.SetKeyword )
         {
            return false;
         }

         var possibleSemicolon2Token = closeCurlyOrGetSetToken.GetNextToken();

         if ( possibleSemicolon2Token.Kind() != SyntaxKind.SemicolonToken )
         {
            return false;
         }

         closeCurlyOrGetSetToken = possibleSemicolon2Token.GetNextToken();

         if ( closeCurlyOrGetSetToken.Kind() == SyntaxKind.CloseBracketToken )
         {
            return true;
         }

         return true;
      }

      public static async Task<int> AdditionalSpacesToAddAsync( SyntaxNode root, Diagnostic diagnostic, double dAlignmentStandardDeviation, CancellationToken cancellationToken = default(CancellationToken) )
      {
         int nLine = diagnostic.Location.GetLineSpan().StartLinePosition.Line;
         int nColumn = diagnostic.Location.GetLineSpan().StartLinePosition.Character;

         return await AdditionalSpacesToAddAsync( root, nLine, nColumn, dAlignmentStandardDeviation, cancellationToken ).ConfigureAwait( false );
      }

      public static async Task<int> AdditionalSpacesToAddAsync( SyntaxNode root, int nLine, int nColumn, double dAlignmentStandardDeviation, CancellationToken cancellationToken = default( CancellationToken ) )
      {
         List<Location> listGetterSetterLocations = new List<Location>();
         foreach ( var token in root.DescendantTokens( descendIntoTrivia: true ).Where( t => t.IsKind( SyntaxKind.OpenBraceToken ) ) )
         {
            if ( IsSimpleGetterSetter( root, token ) )
            {
               listGetterSetterLocations.Add( token.GetLocation() );
            }
         }

         List<Location> listRelevantGetterSetterLocations = new List<Location>();
         for ( int nL = nLine - 1; nL >= 0; nL-- )
         {
            var previousLineMatch = listGetterSetterLocations.Where( loc => loc.GetLineSpan().StartLinePosition.Line == nL );
            if ( !previousLineMatch.Any() )
            {
               // Might have been a new line that provided the gap or something.  I would like to handle this; but for now
               // let's skip it
               break;
            }
            else
            {
               listRelevantGetterSetterLocations.Add( previousLineMatch.First() );
            }
         }

         for ( int nL = nLine /*+ 1*/; nL < int.MaxValue; nL++ )
         {
            var nextLineMatch = listGetterSetterLocations.Where( loc => loc.GetLineSpan().StartLinePosition.Line == nL );
            if ( !nextLineMatch.Any() )
            {
               // Must have been a new line or something.  Would want to handle this.  But for now lets stop
               break;
            }
            else
            {
               listRelevantGetterSetterLocations.Add( nextLineMatch.First() );
            }
         }

         if ( !listRelevantGetterSetterLocations.Any() )
         {
            //Hmmmm; I wonder if this should ever happen.  Whether I should assert or not?  Anyhoo
            return 0;
         }

         // Let's find average column across the relevant getter and setter locations
         int nTotal = 0;
         int nTotalofSquaredItems = 0;
         int nLinesConsidered = 0;
         foreach ( var getterSetterLocation in listRelevantGetterSetterLocations )
         {
            int nGetterSetterColumn = ColumnForOneAwayFromVariable( root, getterSetterLocation );

            nTotal += nGetterSetterColumn;
            nTotalofSquaredItems += nGetterSetterColumn * nGetterSetterColumn;
            nLinesConsidered++;
         }

         double dTotalSquaredDivided = nTotal * nTotal / (double)nLinesConsidered;
         double dDifferencedFromTotalSquared = nTotalofSquaredItems - dTotalSquaredDivided;
         double dVariance = dDifferencedFromTotalSquared / ( nLinesConsidered - 1 );
         double dStandardDeviation = Math.Sqrt( dVariance );

         double dAverageColumn = nTotal / (double)nLinesConsidered;

         int nDesiredColumn = nColumn;
         foreach ( var getterSetterLocation in listRelevantGetterSetterLocations )
         {
            int nGetterSetterLine = getterSetterLocation.GetLineSpan().StartLinePosition.Line;

            if ( nGetterSetterLine == nLine )
            {
               continue;
            }

            int nGetterSetterColumn = ColumnForOneAwayFromVariable( root, getterSetterLocation );

            if ( nGetterSetterColumn > nDesiredColumn )
            {
               // Before we just change the desired column let's consider if this column stands out compared to other lines
               double dZScore = ( nGetterSetterColumn - dAverageColumn ) / dStandardDeviation;

               if ( dZScore > dAlignmentStandardDeviation || dZScore < -dAlignmentStandardDeviation )
               {
                  // Going to skip this outlier
                  continue;
               }

               nDesiredColumn = nGetterSetterColumn;
            }
         }

         return nDesiredColumn - nColumn;
      }

      public static string WhiteSpaceString( int nCharacters )
      {
         string s = string.Empty.PadLeft( nCharacters );
         return s;
      }

      private static int ColumnForOneAwayFromVariable( SyntaxNode root, Location getterSetterLocation )
      {
         // Now I could use this
         int nGetterSetterColumn = getterSetterLocation.GetLineSpan().StartLinePosition.Character;

         // For the column I'm going to use where it would be if there were no spacing applied.
         var getterSetterToken = root.FindToken( getterSetterLocation.SourceSpan.Start );
         Debug.Assert( getterSetterToken.GetLocation() == getterSetterLocation, "Found token should be at the same location I was looking at" );
         var prevToken = getterSetterToken.GetPreviousToken();
         nGetterSetterColumn = prevToken.GetLocation().GetLineSpan().EndLinePosition.Character + 1/*One away from end*/;

         return nGetterSetterColumn;
      }
   }
}
