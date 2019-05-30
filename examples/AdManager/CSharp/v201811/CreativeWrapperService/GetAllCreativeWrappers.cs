// Copyright 2017, Google Inc. All Rights Reserved.
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

namespace Google.Api.Ads.AdManager.Examples.CSharp.v201811
{
    /// <summary>
    /// This example gets all creative wrappers.
    /// </summary>
    public class GetAllCreativeWrappers : SampleBase
    {
        /// <summary>
        /// Returns a description about the code example.
        /// </summary>
        public override string Description
        {
            get { return "This example gets all creative wrappers."; }
        }

        /// <summary>
        /// Main method, to run this code example as a standalone application.
        /// </summary>
        public static void Main()
        {
            GetAllCreativeWrappers codeExample = new GetAllCreativeWrappers();
            Console.WriteLine(codeExample.Description);
            try
            {
                codeExample.Run(new AdManagerUser());
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to get creative wrappers. Exception says \"{0}\"",
                    e.Message);
            }
        }

        /// <summary>
        /// Run the code example.
        /// </summary>
        public void Run(AdManagerUser user)
        {
            using (CreativeWrapperService creativeWrapperService =
                user.GetService<CreativeWrapperService>())
            {
                // Create a statement to select creative wrappers.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder =
                    new StatementBuilder().OrderBy("id ASC").Limit(pageSize);

                // Retrieve a small amount of creative wrappers at a time, paging through until all
                // creative wrappers have been retrieved.
                int totalResultSetSize = 0;
                do
                {
                    CreativeWrapperPage page =
                        creativeWrapperService.getCreativeWrappersByStatement(
                            statementBuilder.ToStatement());

                    // Print out some information for each creative wrapper.
                    if (page.results != null)
                    {
                        totalResultSetSize = page.totalResultSetSize;
                        int i = page.startIndex;
                        foreach (CreativeWrapper creativeWrapper in page.results)
                        {
                            Console.WriteLine(
                                "{0}) Creative wrapper with ID {1} and label ID {2} was found.",
                                i++, creativeWrapper.id, creativeWrapper.labelId);
                        }
                    }

                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);

                Console.WriteLine("Number of results found: {0}", totalResultSetSize);
            }
        }
    }
}
