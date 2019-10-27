using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using AmazonReviewAutoGenerator.Models;

namespace AmazonReviewAutoGenerator.Business
{
    public static class ReviewService
    {
        private static Dictionary<string, List<string>> _dictionary;
        private static int maxOutputLength;
        
        static ReviewService() 
        {
            _dictionary = new Dictionary<string, List<string>>();
        } 

        public static void IngestAndTrainData(string filePath)
        {
            string data = IngestData(filePath);
            CreateDictionaryWithTrainedData(data);
        }

        public static string GenerateReview()
        {
            Random rand = new Random();
            List<string> output = new List<string>();
            int index = rand.Next(_dictionary.Count);
            string prefix = _dictionary.Keys.Skip(index).Take(1).Single();
            output.Add(prefix);
            int reviewTextLength = Convert.ToInt32(Constants.Configuration.GetSection("AppSettings:ReviewTextLength").Value);
            
            if (reviewTextLength > maxOutputLength)
            {
                throw new Exception("Review text output length too large");
            }

            for (int i = 0; i < reviewTextLength; i++)
            {
                index = rand.Next(_dictionary[prefix].Count);
                var suffix = _dictionary[prefix][index];
                output.Add(suffix);
                prefix = suffix;
            }

            return string.Join(' ', output);
        }

        public static string GenerateRating()
        {
            string rating = new Random().Next(1, 6).ToString();
            return string.Concat(rating, ".0");
        }

        private static string IngestData(string filePath)
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
                allReviewText.Append($"{data[i].ReviewText} ");
            }

            return allReviewText.ToString();
        }

        private static void CreateDictionaryWithTrainedData(string data)
        {
            string[] words = data.Split();
            maxOutputLength = words.Length;

            for (int i = 0; i < words.Length - 1; i++)
            {
                if (_dictionary.ContainsKey(words[i].Trim()))
                {
                    _dictionary[words[i].Trim()].Add(words[i + 1].Trim());
                }
                else
                {
                    _dictionary.Add(words[i].Trim(), new List<string>() { words[i + 1].Trim() });
                }
            }
        }
    }
}