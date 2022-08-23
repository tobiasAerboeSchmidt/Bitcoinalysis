using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManipulation
{
    public static class LorenzExtraction
    {
        /// <summary>
        /// Returns a list of wallet addresses and their total transaction input/output count
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>List of tuple(address, total transaction inputs/outputs)</returns>
        public static List<(string, int)> LoadData(string filePath, int maxLines = int.MaxValue)
        {
            Console.WriteLine(DateTime.UtcNow.ToLocalTime() + " - parsing data...");
            var data = new List<(string, int)>();
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                var count = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    count++;
                    var splitted = line.Split(",");
                    data.Add((splitted[0], int.Parse(splitted[1])));
                    if (count >= maxLines)
                    {
                        break;
                    }
                }
            }
            Console.WriteLine("done");
            return data;
        }

        public static HashSet<string> LoadDirectoryOfLabelData(string folderPath)
        {
            var result = new HashSet<string>();
            var filenames = Directory.GetFiles(folderPath);
            foreach (var filename in filenames)
            {
                var addresses = LoadLabelData(filename);
                foreach (var address in addresses)
                {
                    result.Add(address);
                }
            }
            return result;
            
        }

        public static HashSet<string> LoadLabelData(string filePath)
        {
            Console.WriteLine(DateTime.UtcNow.ToLocalTime() + " - parsing label data...");
            var data = new HashSet<string>();
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                var count = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    count++;
                    data.Add(line);
                }
            }
            Console.WriteLine("done");
            return data;
        }

        /// <summary>
        /// Returns a tuple containing a an array (item1) of accumulated input/output percentages for each 1% of all addresses. 
        /// It also contains an array (item2) of how many percent of each bin matched the label
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static (double[], double[]) CalculateLorenzCurveData(List<(string, int)> data, HashSet<string> walletLabelData)
        {

            // casting to double is for optimization in the for loop as we avoid casting x million times for each address
            var totalInputOutputs = (double)data.Sum(d => d.Item2);
            var totalAddresses = (double)data.Count();

            data = data.OrderBy(tp => tp.Item2).ToList();

            double[] bins = new double[100];
            double[] labelBins = new double[100];

            var onePercentOfAddresses = totalAddresses / 100;

            //add all address I/Os to their respective bins
            for (int i = 0; i < totalAddresses; i++)
            {

                var binIndex = (int)Math.Floor(i / onePercentOfAddresses);
                bins[binIndex] = bins[binIndex] + data[i].Item2 / totalInputOutputs;
                // if the wallet has a label:
                if (walletLabelData.Contains(data[i].Item1))
                {
                    labelBins[binIndex] = labelBins[binIndex] + 1 / onePercentOfAddresses;
                }

            }

            double[] accumulating = new double[100];
            for (int i = 0; i < bins.Length; i++)
            {
                if (i != 0)
                {
                    accumulating[i] = accumulating[i - 1] + bins[i];
                }
                else
                {
                    accumulating[i] = bins[i];
                }
            }
            return (accumulating, labelBins);
        }
    }
}
