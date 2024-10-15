using System;
using System.IO;
using System.Text;

namespace SaveSystem.GameRepository
{
    public interface IBinaryConverter
    {
        public string StringToBinary(string input);
        public string BinaryToString(string binaryInput);
    }

    public class BinaryConverter : IBinaryConverter
    {
        // Simple XOR encryption key
        private readonly char _encryptionKey = 'K';

        // public BinaryConverter()
        // {
        //     string def = Path.Combine(Environment.CurrentDirectory, "Assets", "Scripts", "SaveSystem", "Saves", "DefaultSaveSlot.txt");
        //     string zer = Path.Combine(Environment.CurrentDirectory, "Assets", "Scripts", "SaveSystem", "Saves", "SaveSlot#0.txt");
        //     string one = Path.Combine(Environment.CurrentDirectory, "Assets", "Scripts", "SaveSystem", "Saves", "SaveSlot#1.txt");
        //     string two = Path.Combine(Environment.CurrentDirectory, "Assets", "Scripts", "SaveSystem", "Saves", "SaveSlot#2.txt");
        //     
        //     string defReadData;
        //     string zerReadData;
        //     string oneReadData;
        //     string twoReadData;
        //     
        //     using (StreamReader srd = new StreamReader(def))
        //     {
        //         defReadData = StringToBinary(srd.ReadToEnd());
        //     }
        //     using (StreamReader sr0 = new StreamReader(zer))
        //     {
        //         zerReadData = StringToBinary(sr0.ReadToEnd());
        //     }
        //     using (StreamReader sr1 = new StreamReader(one))
        //     {
        //         oneReadData = StringToBinary(sr1.ReadToEnd());
        //     }
        //     using (StreamReader sr2 = new StreamReader(two))
        //     {
        //         twoReadData = StringToBinary(sr2.ReadToEnd());
        //     }
        //     
        //     using (StreamWriter swd = new StreamWriter(def, false))
        //     {
        //         swd.Write(defReadData);
        //     }
        //     using (StreamWriter sw0 = new StreamWriter(zer, false))
        //     {
        //         sw0.Write(zerReadData);
        //     }
        //     using (StreamWriter sw1 = new StreamWriter(one, false))
        //     {
        //         sw1.Write(oneReadData);
        //
        //     }
        //     using (StreamWriter sw2 = new StreamWriter(two, false))
        //     {
        //         sw2.Write(twoReadData);
        //     }
        // }

        public string StringToBinary(string input)
        {
            string encryptedString = Encrypt(input);

            StringBuilder binaryResult = new StringBuilder();

            foreach (char c in encryptedString)
            {
                binaryResult.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }

            return binaryResult.ToString();
        }

        public string BinaryToString(string binaryInput)
        {
            StringBuilder decryptedString = new StringBuilder();

            for (int i = 0; i < binaryInput.Length; i += 8)
            {
                string byteString = binaryInput.Substring(i, 8);

                byte character = Convert.ToByte(byteString, 2);

                decryptedString.Append((char)character);
            }

            return Decrypt(decryptedString.ToString());
        }

        private string Encrypt(string input)
        {
            StringBuilder encrypted = new StringBuilder();

            foreach (char c in input)
            {
                encrypted.Append((char)(c ^ _encryptionKey));
            }

            return encrypted.ToString();
        }

        private string Decrypt(string input)
        {
            StringBuilder decrypted = new StringBuilder();

            foreach (char c in input)
            {
                decrypted.Append((char)(c ^ _encryptionKey));
            }

            return decrypted.ToString();
        }
    }
}