using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZipCodeLookup02
{
    class Program
    {
        private static List<ZipInfo> standardZipContainer = new List<ZipInfo>();
        private static Dictionary<string, ZipInfo> dictionaryZipContainer = new Dictionary<string, ZipInfo>();

        static void Main(string[] args)
        {

            //Todo: get the list of states from https://gist.githubusercontent.com/mshafrir/2646763/raw/8b0dbb93521f5d6889502305335104218454c2bf/states_hash.json
            int zipCodecount = 0;
            foreach (var s in StateList._states)
            {                 
                string json = new WebClient().DownloadString("http://gomashup.com/json.php?fds=geo/usa/zipcode/state/" + s + "&jsoncallback=?");

                //Cleanup the list... not sure if these are supposed to be there "?(" and start and ")" at the end.
                if (!String.IsNullOrEmpty(json) && json.StartsWith("?("))
                    json = json.Replace("?(", "");

                if (!String.IsNullOrEmpty(json) && json.EndsWith(")"))
                    json = json.Replace(")", "");
                                
                var result = JObject.Parse(json).SelectToken("result").ToObject<List<ZipInfo>>();               
                zipCodecount += result.Count;
                Console.WriteLine(s + ": " + result.Count.ToString());

                standardZipContainer.AddRange(result);
            }

            Console.WriteLine("Total Zip Found:" + zipCodecount.ToString());

            Console.WriteLine("Setting up the test");

            //Generate a random list of 100k indexes.
            //Get the Zipcode from the standardList based on random list, this will be the lookup sequence.
            var rng = new Random(DateTime.Now.Second);
            var test_indexes = Enumerable.Range(0, 100000).Select(x => rng.Next(0, zipCodecount)).ToList();
            var test_values = test_indexes.Select(i => standardZipContainer[i].Zipcode).ToList();

            //Make a dictionary.
            dictionaryZipContainer = standardZipContainer.ToDictionary(x => x.Zipcode, x => x);

            Console.WriteLine("Starting...");

            //Test one using List<>
            Stopwatch sw = new Stopwatch();

            sw.Start();

            foreach (var t in test_values)
                TestUsingList(t);

            sw.Stop();
            Console.WriteLine("Test 1 Elapsed={0}", sw.ElapsedTicks);


            //Test two using Dictionary<string, ZipInfo>
             sw.Start();

            foreach (var t in test_values)
                TestUsingDictionary(t);

            sw.Stop();
            Console.WriteLine("Test 2 Elapsed={0}", sw.ElapsedTicks);

            Console.WriteLine("Test Done.");
            Console.ReadKey();
        }


        private static ZipInfo TestUsingList( string zipCode)
        {
            //Bypassing any sanity check for brevity.

            return standardZipContainer.FirstOrDefault(z => z.Zipcode.Equals(zipCode));
        }

        private static ZipInfo TestUsingDictionary(string zipCode)
        {
            //Bypassing any sanity check for brevity.

            return dictionaryZipContainer[zipCode];
        }
    }

    //References
    //http://jsonapi.org/format/
    //http://gomashup.com/cms/usa_zipcode_json
    //http://stackoverflow.com/questions/20953713/how-do-i-deserialize-a-json-array-and-ignore-the-root-node
    //https://gist.githubusercontent.com/mshafrir/2646763/raw/8b0dbb93521f5d6889502305335104218454c2bf/states_hash.json
}
