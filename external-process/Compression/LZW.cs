using System;
using System.Collections.Generic;
using System.Linq;

namespace external_process.Compression
{
    public class LZW
    {
        /// <summary>Compress all the text using the LZW algorithm</summary>
        /// <param name="text">The text to compress</param>
        /// <returns>The dictionary with each unique character and the compressed text</returns>
        public (Dictionary<string, int>, byte[]) Compress(string text)
        {
            Dictionary<string, int> initialDict = InitialDictionary(text);
            byte[] compressText = CompressText(text, initialDict);
            return (initialDict, compressText);
        }

        /// <summary>Decompress all the bytes using the LZW algorithm</summary>
        /// <param name="initialDictionary">The directory with each unique character</param>
        /// <param name="bytes">The compressed bytes</param>
        /// <returns>All the bytes decompressed</returns>
        public string Decompress(Dictionary<string, int> initialDictionary, byte[] bytes)
        {
            return DecompressBytes(bytes, initialDictionary);
        }

        /// <summary>Get each unique character in a dictionary.</summary>
        /// <param name="text">All the characters to read.</param>
        /// <returns>A dictionary with each unique character.</returns>
        private Dictionary<string, int> InitialDictionary(string text)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (char character in text)
            {
                if (!dict.ContainsKey(character.ToString()))
                    dict.Add(character.ToString(), dict.Count + 1);
            }
            return dict;
        }

        /// <summary>Compress all the text using the LZW algorithm</summary>
        /// <param name="text">The text to compress</param>
        /// <param name="dict">The initial dictionary for the algorithm</param>
        /// <returns>All the text compressed in bytes</returns>
        private byte[] CompressText(string text, Dictionary<string, int> dict)
        {
            Dictionary<string, int> dynamicDict = new Dictionary<string, int>(dict);
            int i = 0;
            string charsRead = string.Empty;
            List<byte> bytes = new List<byte>();
            while (i < text.Length)
            {
                charsRead += text[i];
                i++;

                while (dynamicDict.ContainsKey(charsRead) && i < text.Length)
                {
                    charsRead += text[i];
                    i++;
                }

                if (dynamicDict.ContainsKey(charsRead))
                {
                    bytes.Add(Convert.ToByte(dynamicDict[charsRead]));
                }
                else
                {
                    string key = charsRead.Substring(0, charsRead.Length - 1);
                    bytes.Add(Convert.ToByte(dynamicDict[key]));
                    dynamicDict.Add(charsRead, dynamicDict.Count + 1);
                    i--;
                    charsRead = string.Empty;
                }
            }
            return bytes.ToArray();
        }

        /// <summary>Decompress all the bytes using the LZW algorithm</summary>
        /// <param name="bytes">The compressed bytes</param>
        /// <param name="dict">The directory with each unique character</param>
        /// <returns>All the bytes decompressed</returns>
        private string DecompressBytes(byte[] bytes, Dictionary<string, int> dict)
        {
            Dictionary<string, int> dynamicDict = new Dictionary<string, int>(dict);
            int oldIndex = Convert.ToInt32(bytes[0]);
            string decompressText = dynamicDict.FirstOrDefault(x => x.Value == oldIndex).Key;
            int i = 1;
            while (i < bytes.Length)
            {
                string text = string.Empty;
                string charsRead = dynamicDict.FirstOrDefault(x => x.Value == oldIndex).Key;
                int newIndex = Convert.ToInt32(bytes[i]);
                text = dynamicDict.FirstOrDefault(x => x.Value == newIndex).Key;
                dynamicDict.Add(charsRead + text[0], dynamicDict.Count + 1);
                decompressText += text;
                oldIndex = newIndex;
                i++;
            }
            return decompressText;
        }
    }
}