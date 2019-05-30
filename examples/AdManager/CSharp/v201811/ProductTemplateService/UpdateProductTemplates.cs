// Copyright 2018, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Ads.AdManager.Lib;
using Google.Api.Ads.AdManager.Util.v201811;
using Google.Api.Ads.AdManager.v201811;

using System;
using System.Collections.Generic;

namespace Google.Api.Ads.AdManager.Examples.CSharp.v201811
{
    /// <summary>
    /// This example updates a product template's targeting to include a new GeoTarget.
    /// To determine which product templates exist, run GetAllProductTemplates.cs.
    /// </summary>
    public class UpdateProductTemplates : SampleBase
    {
        /// <summary>
        /// Returns a description about the code example.
        /// </summary>
        public override string Description
        {
            get
            {
                return "This example updates a product template's targeting to include a new " +
                    "GeoTarget. To determine which product templates exist, " +
                    "run GetAllProductTemplates.cs.";
            }
        }

        /// <summary>
        /// Main method, to run this code example as a standalone application.
        /// </summary>
        public static void Main()
        {
            UpdateProductTemplates codeExample = new UpdateProductTemplates();
            Console.WriteLine(codeExample.Description);
            codeExample.Run(new AdManagerUser());
        }

        /// <summary>
        /// Run the code example.
        /// </summary>
        public void Run(AdManagerUser user)
        {
            using (ProductTemplateService productTemplateService =
                user.GetService<ProductTemplateService>())
            {
                // Set the ID of the product template.
                long productTemplateId = long.Parse(_T("INSERT_PRODUCT_TEMPLATE_ID_HERE"));

                // Create a statement to get the product template.
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("id = :id")
                    .OrderBy("id ASC")
                    .Limit(1)
                    .AddValue("id", productTemplateId);

                try
                {
                    // Get product templates by statement.
                    ProductTemplatePage page =
                        productTemplateService.getProductTemplatesByStatement(
                            statementBuilder.ToStatement());

                    ProductTemplate productTemplate = page.results[0];

                    // Add geo targeting for Canada to the product template.
                    Location countryLocation = new Location();
                    countryLocation.id = 2124L;

                    Targeting productTemplateTargeting = productTemplate.builtInTargeting;
                    GeoTargeting geoTargeting = productTemplateTargeting.geoTargeting;

                    List<Location> existingTargetedLocations = new List<Location>();

                    if (geoTargeting == null)
                    {
                        geoTargeting = new GeoTargeting();
                    }
                    else if (geoTargeting.targetedLocations != null)
                    {
                        existingTargetedLocations =
                            new List<Location>(geoTargeting.targetedLocations);
                    }

                    existingTargetedLocations.Add(countryLocation);

                    Location[] newTargetedLocations = new Location[existingTargetedLocations.Count];
                    existingTargetedLocations.CopyTo(newTargetedLocations);
                    geoTargeting.targetedLocations = newTargetedLocations;

                    productTemplateTargeting.geoTargeting = geoTargeting;
                    productTemplate.builtInTargeting = productTemplateTargeting;

                    // Update the product template on the server.
                    ProductTemplate[] productTemplates =
                        productTemplateService.updateProductTemplates(new ProductTemplate[]
                        {
                            productTemplate
                        });

                    if (productTemplates != null)
                    {
                        foreach (ProductTemplate updatedProductTemplate in productTemplates)
                        {
                            Console.WriteLine(
                                "A product template with ID = '{0}' and name '{1}' was updated.",
                                updatedProductTemplate.id, updatedProductTemplate.name);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No product templates updated.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to update product templates. Exception says \"{0}\"",
                        e.Message);
                }
            }
        }
    }
}
