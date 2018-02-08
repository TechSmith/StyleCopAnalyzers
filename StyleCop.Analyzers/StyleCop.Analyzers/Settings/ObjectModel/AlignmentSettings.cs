// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Settings.ObjectModel
{
    using LightJson;

    internal class AlignmentSettings
    {
        private double alignmentStandardDeviation;

      /// <summary>
      /// Initializes a new instance of the <see cref="AlignmentSettings"/> class.
      /// </summary>
      protected internal AlignmentSettings()
        {
            this.alignmentStandardDeviation = 1.25d;
        }

      /// <summary>
      /// Initializes a new instance of the <see cref="AlignmentSettings"/> class.
      /// </summary>
      /// <param name="alignmentSettingsObject">The JSON object containing the settings.</param>
      protected internal AlignmentSettings( JsonObject alignmentSettingsObject)
            : this()
        {
            foreach (var kvp in alignmentSettingsObject )
            {
                switch (kvp.Key)
                {
                case "alignmentStandardDeviation":
                    this.alignmentStandardDeviation = kvp.Value.AsNumber;
                    break;

                default:
                    break;
                }
            }
        }

        public double AlignmentStandardDeviation =>
            this.alignmentStandardDeviation;
    }
}
