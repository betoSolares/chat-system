using System;

namespace external_process.Encryption
{
    public class SDES
    {
        private readonly Permutations permutations = new Permutations("8537926014", "79358216", "0321", "01323210",
                                                                      "63572014");

        /// <summary>Decrypt a byte using the S-DES algorithm</summary>
        /// <param name="encryptedByte">The byte to decrypt</param>
        /// <param name="key">The key for the decryption</param>
        /// <returns>True and the decrypted byte or false and a default byte</returns>
        public (bool, byte) Decrypt(byte encryptedByte, int key)
        {
            if (permutations.CheckPermutations())
            {
                string binaryKey = Convert.ToString(key, 2).PadLeft(10, '0');
                (string K1, string K2) = GenerateKeys(binaryKey, permutations.P10, permutations.P8);
                byte newByte = DecryptBytes(encryptedByte, K1, K2, permutations.IP, permutations.EP, permutations.P4,
                                            permutations.IIP);
                return (true, newByte);
            }
            return (false, default);
        }

        /// <summary>Encrypt a byte using the S-DES algorithm</summary>
        /// <param name="normalByte">The byte to encrypt</param>
        /// <param name="key">The key for the encryption</param>
        /// <returns>True and the encrypted byte or false and a default byte</returns>
        public (bool, byte) Encrypt(byte normalByte, int key)
        {
            if (permutations.CheckPermutations())
            {
                string binaryKey = Convert.ToString(key, 2).PadLeft(10, '0');
                (string K1, string K2) = GenerateKeys(binaryKey, permutations.P10, permutations.P8);
                byte encryptedByte = EncryptBytes(normalByte, K1, K2, permutations.IP, permutations.EP, permutations.P4,
                                                  permutations.IIP);
                return (true, encryptedByte);
            }
            return (false, default);
        }

        /// <summary>Decrypt a byte</summary>
        /// <param name="encryptedByte">The byte to decrypt</param>
        /// <param name="K1">The first key for the algorithm</param>
        /// <param name="K2">The second key for the algorithm</param>
        /// <param name="ip">The IP permutation</param>
        /// <param name="ep">The EP permutation</param>
        /// <param name="p4">The P4 permutation</param>
        /// <param name="iip">The IIP permutation</param>
        /// <returns>The new byte decrypted</returns>
        private byte DecryptBytes(byte encryptedByte, string K1, string K2, string ip, string ep, string p4, string iip)
        {
            string binary = Convert.ToString(encryptedByte, 2).PadLeft(8, '0');
            string IP = MakePermutations(binary, ip);
            string firstBlock = IP.Substring(0, 4);
            string secondBlock = IP.Substring(4, 4);
            string EP = MakePermutations(secondBlock, ep);
            string xor = XOR(EP, K2);
            string SBoxes = SubstitutionBoxes(xor);
            string P4 = MakePermutations(SBoxes, p4);
            xor = XOR(P4, firstBlock);
            string swap = secondBlock + xor;
            firstBlock = swap.Substring(0, 4);
            secondBlock = swap.Substring(4, 4);
            EP = MakePermutations(secondBlock, ep);
            xor = XOR(EP, K1);
            SBoxes = SubstitutionBoxes(xor);
            P4 = MakePermutations(SBoxes, p4);
            xor = XOR(P4, firstBlock);
            string union = xor + secondBlock;
            string IIP = MakePermutations(union, iip);
            return Convert.ToByte(Convert.ToInt32(IIP, 2));
        }

        /// <summary>Encrypt a byte</summary>
        /// <param name="normalByte">The byte to encrypt</param>
        /// <param name="K1">The first key for the algorithm</param>
        /// <param name="K2">The second key for the algorithm</param>
        /// <param name="ip">The IP permutation</param>
        /// <param name="ep">The EP permutation</param>
        /// <param name="p4">The P4 permutation</param>
        /// <param name="iip">The IIP permutation</param>
        /// <returns>The new byte encrypted</returns>
        private byte EncryptBytes(byte normalByte, string K1, string K2, string ip, string ep, string p4, string iip)
        {
            string binary = Convert.ToString(normalByte, 2).PadLeft(8, '0');
            string IP = MakePermutations(binary, ip);
            string firstBlock = IP.Substring(0, 4);
            string secondBlock = IP.Substring(4, 4);
            string EP = MakePermutations(secondBlock, ep);
            string xor = XOR(EP, K1);
            string SBoxes = SubstitutionBoxes(xor);
            string P4 = MakePermutations(SBoxes, p4);
            xor = XOR(P4, firstBlock);
            string swap = secondBlock + xor;
            firstBlock = swap.Substring(0, 4);
            secondBlock = swap.Substring(4, 4);
            EP = MakePermutations(secondBlock, ep);
            xor = XOR(EP, K2);
            SBoxes = SubstitutionBoxes(xor);
            P4 = MakePermutations(SBoxes, p4);
            xor = XOR(P4, firstBlock);
            string union = xor + secondBlock;
            string IIP = MakePermutations(union, iip);
            return Convert.ToByte(Convert.ToInt32(IIP, 2));
        }

        /// <summary>Generate the keys following the S-DES algorithm</summary>
        /// <param name="key">The 10 bits key</param>
        /// <param name="p10">The P10 permutation</param>
        /// <param name="p8">The P8 permutation</param>
        /// <returns>A tuple with the K1 and K2</returns>
        private (string K1, string K2) GenerateKeys(string key, string p10, string p8)
        {
            string P10 = string.Empty;
            foreach (char index in p10)
            {
                P10 += key[int.Parse(index.ToString())];
            }
            string LS1 = LeftShift(P10);
            string K1 = string.Empty;
            foreach (char index in p8)
            {
                K1 += LS1[int.Parse(index.ToString())];
            }
            string LS2 = LeftShift(LeftShift(LS1));
            string K2 = string.Empty;
            foreach (char index in p8)
            {
                K2 += LS2[int.Parse(index.ToString())];
            }
            return (K1, K2);
        }

        /// <summary>Shift the string in one position to the left</summary>
        /// <param name="input">The string to shift</param>
        /// <returns>A new string shifted</returns>
        private string LeftShift(string input)
        {
            string firstBlock = input.Substring(0, 5);
            string secondBlock = input.Substring(5, 5);
            string firstShift = firstBlock.Substring(1, firstBlock.Length - 1) + firstBlock.Substring(0, 1);
            string secondShift = secondBlock.Substring(1, secondBlock.Length - 1) + secondBlock.Substring(0, 1);
            return firstShift + secondShift;
        }

        /// <summary>Change the position of the characters in the string based on the permutation</summary>
        /// <param name="input">The binary to search the characters</param>
        /// <param name="indexer">The string with the index to search the characters</param>
        /// <returns>A new string permutated</returns>
        private string MakePermutations(string input, string indexer)
        {
            string result = string.Empty;
            foreach (char index in indexer)
            {
                result += input[int.Parse(index.ToString())];
            }
            return result;
        }

        /// <summary>Make the substitution for the input</summary>
        /// <param name="input">The binary to check the s-box</param>
        /// <returns>A new binary number switched in the s-boxes</returns>
        private string SubstitutionBoxes(string input)
        {
            string[,] SBox1 = new string[4, 4]
            {
                { "01", "00", "11", "10" },
                { "11", "10", "01", "00" },
                { "00", "10", "01", "11" },
                { "11", "01", "11", "10" }
            };
            string[,] SBox2 = new string[4, 4]
            {
                { "00", "01", "10", "11" },
                { "10", "00", "01", "11" },
                { "11", "00", "01", "00" },
                { "10", "01", "00", "11" }
            };
            string result = string.Empty;
            string firstBlock = input.Substring(0, 4);
            string secondBlock = input.Substring(4, 4);
            int y = Convert.ToInt32(firstBlock[0].ToString() + firstBlock[3].ToString(), 2);
            int x = Convert.ToInt32(firstBlock[1].ToString() + firstBlock[2].ToString(), 2);
            result += SBox1[y, x];
            y = Convert.ToInt32(secondBlock[0].ToString() + secondBlock[3].ToString(), 2);
            x = Convert.ToInt32(secondBlock[1].ToString() + secondBlock[2].ToString(), 2);
            result += SBox2[y, x];
            return result;
        }

        /// <summary>Make and xor operation in two binary strings</summary>
        /// <param name="input">The first string to compare</param>
        /// <param name="comparer">The second string to compare</param>
        /// <returns>The string with the XOR operation</returns>
        private string XOR(string input, string comparer)
        {
            string result = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].Equals(comparer[i]))
                    result += "0";
                else
                    result += "1";
            }
            return result;
        }
    }
}