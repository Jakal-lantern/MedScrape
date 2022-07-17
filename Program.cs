using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using ScrapySharp.Network;
using System.IO;
using Newtonsoft.Json;

namespace Webscraper
{
    class Program
    {
        static ScrapingBrowser _scrapingBrowser = new ScrapingBrowser();

        static void Main(string[] args)
        {
            string userInput = "NONE";

            while(userInput == "NONE")
            {
                Console.Write("Enter a states name: ");
                userInput = Console.ReadLine();
                userInput = StateSelect(userInput.ToLower());
            }

            OutputToJson(GetGasPrices(userInput));
        }

        /* State Select Function
         * - Searches through a list of states to return url
         * PARAMS - (string) full state name
         * RETURNS - (string) state url / null
         */
        static string StateSelect(string state)
        {
            // Create lists
            List<string> states = new List<string>()
            {
                "alabama",
                "alaska",
                "arizona",
                "arkanas",
                "california",
                "colorado",
                "connecticut",
                "delaware",
                "florida",
                "georgia",
                "hawaii",
                "idaho",
                "illinois",
                "indiana",
                "iowa",
                "kansas",
                "kentucky",
                "louisiana",
                "maine",
                "maryland",
                "massachusetts",
                "michigan",
                "minnesota",
                "mississippi",
                "missouri",
                "montana",
                "nebraska",
                "nevada",
                "new hamspire",
                "new jersey",
                "new mexico",
                "new york",
                "north carolina",
                "north dakota",
                "ohio",
                "oklahoma",
                "oregon",
                "pennsylvania",
                "rhode esland",
                "south carolina",
                "south dakota",
                "tennessee",
                "texas",
                "utah",
                "vermont",
                "virginia",
                "washington",
                "west virginia",
                "wisconsin",
                "wyoming"
            };
            List<string> urls = new List<string>()
            {
                "https://gasprices.aaa.com/?state=AL",
                "https://gasprices.aaa.com/?state=AK",
                "https://gasprices.aaa.com/?state=AZ",
                "https://gasprices.aaa.com/?state=AR",
                "https://gasprices.aaa.com/?state=CA",
                "https://gasprices.aaa.com/?state=CO",
                "https://gasprices.aaa.com/?state=CT",
                "https://gasprices.aaa.com/?state=DE",
                "https://gasprices.aaa.com/?state=FL",
                "https://gasprices.aaa.com/?state=GA",
                "https://gasprices.aaa.com/?state=HI",
                "https://gasprices.aaa.com/?state=ID",
                "https://gasprices.aaa.com/?state=IL",
                "https://gasprices.aaa.com/?state=IN",
                "https://gasprices.aaa.com/?state=LA",
                "https://gasprices.aaa.com/?state=KS",
                "https://gasprices.aaa.com/?state=KY",
                "https://gasprices.aaa.com/?state=LA",
                "https://gasprices.aaa.com/?state=ME",
                "https://gasprices.aaa.com/?state=MD",
                "https://gasprices.aaa.com/?state=MA",
                "https://gasprices.aaa.com/?state=MI",
                "https://gasprices.aaa.com/?state=MN",
                "https://gasprices.aaa.com/?state=MS",
                "https://gasprices.aaa.com/?state=MO",
                "https://gasprices.aaa.com/?state=MT",
                "https://gasprices.aaa.com/?state=NE",
                "https://gasprices.aaa.com/?state=NV",
                "https://gasprices.aaa.com/?state=NH",
                "https://gasprices.aaa.com/?state=NJ",
                "https://gasprices.aaa.com/?state=NM",
                "https://gasprices.aaa.com/?state=NY",
                "https://gasprices.aaa.com/?state=NC",
                "https://gasprices.aaa.com/?state=ND",
                "https://gasprices.aaa.com/?state=OH",
                "https://gasprices.aaa.com/?state=OK",
                "https://gasprices.aaa.com/?state=OR",
                "https://gasprices.aaa.com/?state=PA",
                "https://gasprices.aaa.com/?state=RI",
                "https://gasprices.aaa.com/?state=SC",
                "https://gasprices.aaa.com/?state=SD",
                "https://gasprices.aaa.com/?state=TN",
                "https://gasprices.aaa.com/?state=TX",
                "https://gasprices.aaa.com/?state=UT",
                "https://gasprices.aaa.com/?state=VT",
                "https://gasprices.aaa.com/?state=VA",
                "https://gasprices.aaa.com/?state=WA",
                "https://gasprices.aaa.com/?state=WV",
                "https://gasprices.aaa.com/?state=WI",
                "https://gasprices.aaa.com/?state=WY"
            };

            // Iterate through lists
            for(int i = 0; i < states.Count; i++)
            {
                // Return url if true
                if(state == states[i])
                {
                    return urls[i];
                }
            }
            // If no state is found return 'NONE'
            return "NONE";
        }

        /* Get Gas Prices Function
         * - Accesses and stores prices from html
         * PARAMS - (string) state url
         * RETURNS - (CurrentPrices) object containing current prices
         */
        static CurrentPrices GetGasPrices(string url)
        {
            // Access Html
            var HtmlNode = GetHtml(url);
            // Create new 'CurrentPrices' object
            var currentPrices = new CurrentPrices();

            // Access and store gas prices
            currentPrices.Regular = HtmlNode.OwnerDocument.DocumentNode.SelectSingleNode("//html/body/main/div[4]/div[3]/table/tbody/tr/td[2]").InnerText;
            currentPrices.MidGrade = HtmlNode.OwnerDocument.DocumentNode.SelectSingleNode("//html/body/main/div[4]/div[3]/table/tbody/tr/td[3]").InnerText;
            currentPrices.Premium = HtmlNode.OwnerDocument.DocumentNode.SelectSingleNode("//html/body/main/div[4]/div[3]/table/tbody/tr/td[4]").InnerText;
            currentPrices.Diesel = HtmlNode.OwnerDocument.DocumentNode.SelectSingleNode("//html/body/main/div[4]/div[3]/table/tbody/tr/td[5]").InnerText;
            
            return currentPrices;
        }

        /* Get Html Function
         * - Accesses and store webpage html
         * PARAMS - (string) state url
         * RETURNS - (HtmlNode) webpage doc
         */
        static HtmlNode GetHtml(string url)
        {
            WebPage webpage = _scrapingBrowser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }

        /* Output to Json Function 
         * - Outputs CurrentPrices object to json file in 'Temp' folder
         * PARAMS - (CurrentPrices) object containing current prices
         */
        static void OutputToJson(CurrentPrices currentPrices)
        {
            // Create output variable
            List<CurrentPrices> outputData = new List<CurrentPrices>();
            // Format 'outputData'
            outputData.Add(new CurrentPrices()
            {
                Regular = currentPrices.Regular,
                MidGrade = currentPrices.MidGrade,
                Premium = currentPrices.Premium,
                Diesel = currentPrices.Diesel
            });

            // Convert to string
            string json = JsonConvert.SerializeObject(outputData.ToArray());

            // Write string to file
            File.WriteAllText(@"C:\Temp\CurrentGasPrices.json", json);
        }
    }
}
