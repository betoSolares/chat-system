using System.Linq;

namespace external_process.Encryption
{
    class Permutations
    {
        private readonly string p10;
        private readonly string p8;
        private readonly string p4;
        private readonly string ep;
        private readonly string ip;
        private readonly string iip;

        /// <summary>Constructor</summary>
        /// <param name="p10">Ten numbers from 0 to 9 without repetition</param>
        /// <param name="p8">Eighth numbers from 0 to 9 without repetition</param>
        /// <param name="p4">Four numbers from 0 to 3 without repetition</param>
        /// <param name="ep">Eighth numbers divided in two blocks of four numbers from 0 to 3 without repetition</param>
        /// <param name="ip">Eight numbers from 0 to 7 without repetition</param>
        public Permutations(string p10, string p8, string p4, string ep, string ip)
        {
            this.p10 = p10;
            this.p8 = p8;
            this.p4 = p4;
            this.ep = ep;
            this.ip = ip;
            for (int i = 0; i < ip.Length; i++)
            {
                int index = ip.IndexOf(i.ToString());
                iip += index;
            }
        }

        /// <summary>Check the permutations</summary>
        /// <returns>True if it's correct, otherwise false</returns>
        public bool CheckPermutations()
        {
            return CheckP10() && CheckP8() && CheckP4() && CheckEP() && CheckIP();
        }

        /// <summary>Check the P10 permutation</summary>
        /// <returns>True if it's correct, otherwise false</returns>
        private bool CheckP10()
        {
            if (p10.Distinct().Count() != p10.Length || p10.Length != 10)
                return false;
            return true;
        }

        /// <summary>Check the P8 permutation</summary>
        /// <returns>True if it's correct, otherwise false</returns>
        private bool CheckP8()
        {
            if (p8.Distinct().Count() != p8.Length || p8.Length != 8)
                return false;
            return true;
        }

        /// <summary>Check the P4 permutation</summary>
        /// <returns>True if it's correct, otherwise false</returns>
        private bool CheckP4()
        {
            char[] values = new[] { '4', '5', '6', '7', '8', '9' };
            if (values.Any(p4.Contains))
            {
                return false;
            }
            else
            {
                if (p4.Distinct().Count() != p4.Length || p4.Length != 4)
                    return false;
                return true;
            }
        }

        /// <summary>Check the EP permutation</summary>
        /// <returns>True if it's correct, otherwise false</returns>
        private bool CheckEP()
        {
            char[] values = new[] { '4', '5', '6', '7', '8', '9' };
            if (values.Any(ep.Contains) || ep.Length != 8)
            {
                return false;
            }
            else
            {
                string firstBlock = ep.Substring(0, 4);
                string secondBlock = ep.Substring(4, 4);
                if (firstBlock.Distinct().Count() != firstBlock.Length || firstBlock.Length != 4)
                {
                    return false;
                }
                else
                {
                    if (secondBlock.Distinct().Count() != secondBlock.Length || secondBlock.Length != 4)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }

        /// <summary>Check the IP permutation</summary>
        /// <returns>True if it's correct, otherwise false</returns>
        private bool CheckIP()
        {
            char[] values = new[] { '8', '9' };
            if (values.Any(ip.Contains))
            {
                return false;
            }
            else
            {
                if (ip.Distinct().Count() != ip.Length || ip.Length != 8)
                    return false;
                return true;
            }
        }

        public string P10 { get => p10; }
        public string P8 { get => p8; }
        public string P4 { get => p4; }
        public string EP { get => ep; }
        public string IP { get => ip; }
        public string IIP { get => iip; }
    }
}
