using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;
using AmazonReviewAutoGenerator.Models;
using System.Text;

namespace AmazonReviewAutoGenerator.Business
{
    public class ReviewService // Making this a singleton in order to not have to train data over and over again
    {
        private int _keySize;
        private int _outputSize;
        private Dictionary<string, List<string>> _dictionary;
        private static ReviewService instance;
        
        static ReviewService() {} // forces laziness

        private ReviewService()
        {
            _keySize = 1;
            _outputSize = 30;
            _dictionary = new Dictionary<string, List<string>>();
        }

        public static ReviewService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReviewService();
                }
                return instance;
            }
        }

        public void IngestAndTrainData(string filePath)
        {
            string data = IngestData(filePath);
            TrainData(data);
        }

        public string GenerateReview()
        {
            Random rand = new Random();
            List<string> output = new List<string>();
            int n = 0;
            int rn = rand.Next(_dictionary.Count);
            string prefix = _dictionary.Keys.Skip(rn).Take(1).Single();
            output.AddRange(prefix.Split());

            while (true)
            {
                var suffix = _dictionary[prefix];
                if (suffix.Count == 1)
                {
                    if (suffix[0] == "")
                    {
                        return output.Aggregate(Join);
                    }
                    output.Add(suffix[0]);
                }
                else
                {
                    rn = rand.Next(suffix.Count);
                    output.Add(suffix[rn]);
                }
                if (output.Count >= _outputSize)
                {
                    return output.Take(_outputSize).Aggregate(Join);
                }
                n++;
                prefix = output.Skip(n).Take(_keySize).Aggregate(Join);
            }
        }

        public string GenerateRating()
        {
            string rating = new Random().Next(1, 6).ToString();
            return string.Concat(rating, ".0");
        }

        private string IngestData(string filePath)
        {
            string json = File.ReadAllText(filePath);
            string[] array = json.Split('\n');
            json = string.Join(',', array);
            json = string.Concat("[", json);
            json = string.Concat(json, "]");
            List<ReviewIngest> data = JsonConvert.DeserializeObject<List<ReviewIngest>>(json);
            StringBuilder allReviewText = new StringBuilder();

            for (int i = 0; i < data.Count; i++)
            {
                allReviewText.Append($"{data[i].ReviewText}. ");
            }

            return allReviewText.ToString();
        }

        private void TrainData(string data)
        {
            if (_keySize < 1) throw new ArgumentException("Key size can't be less than 1");

            var words = data.Split();
            if (_outputSize < _keySize || words.Length < _outputSize)
            {
                throw new ArgumentException("Output size is out of range");
            }

            for (int i = 0; i < words.Length - _keySize; i++)
            {
                var key = words.Skip(i).Take(_keySize).Aggregate(Join);
                string value;
                if (i + _keySize < words.Length)
                {
                    value = words[i + _keySize];
                }
                else
                {
                    value = "";
                }

                if (_dictionary.ContainsKey(key))
                {
                    _dictionary[key].Add(value);
                }
                else
                {
                    _dictionary.Add(key, new List<string>() { value });
                }
            }
        }

        private string Join(string a, string b)
        {
            return a + " " + b;
        }
    }
}