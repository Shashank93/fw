using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EncryptionDecryptionRESTApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _privateKey = string.Empty;
        private string _publicKey = string.Empty;
        Encoding _encoder = Encoding.UTF8;
        public MainWindow()
        {
            InitializeComponent();
            GeneratePublicAndPrivateKeys();

            //ENCRYPTION


            //CREATE OBJECT FOR RESPONSE
            Response rsp = new Response();
            ResponseHeader rspHeader = new ResponseHeader();
            rspHeader.ResponseCode = "SUCCESS";
            rspHeader.ResponseMessage = "Save Successful";
            var respData = "{'expcreateddate':'2019 - 05 - 2406:10:12','expcreatedtime':'06:10:12.8855530','expid':'ExpId1','selectedgroup':'A','tags':{'departement':'Animalkingdom','departement1':'Animal','departement2':'Forest','departement3':'Rhino','dnaClass':'F71','frogNo':'11','frogType':'1521','result':'Pass','status':'Normal','testCycle':'1'},'username':'spai1'}";
            var base64text = Base64Encode(respData);

            var publicKeyEncryptedData = EncryptWithPublicKey(base64text);

            ResponseBody rspData = new ResponseBody();
            rspData.ResponseData = publicKeyEncryptedData;
            rsp.ResponseHeader = rspHeader;
            rsp.ResponseBody = rspData;

            var jsonResponse = JsonConvert.SerializeObject(rsp);
            var finalEncryptedData = EncryptTripleDES(jsonResponse, true);

            //DECRYPTION
            var initialDecryptedData = DecryptTripleDES(finalEncryptedData, true);
            var decryptedModelData = JsonConvert.DeserializeObject<Response>(initialDecryptedData);
            var responseData = DecryptFromPrivateKey(decryptedModelData.ResponseBody.ResponseData);
            decryptedModelData.ResponseBody.ResponseData = Base64Decode(responseData);

            var finalData = decryptedModelData;


        }

        public void GeneratePublicAndPrivateKeys()
        {
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = "XML_ENC_RSA_KEY";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParams);

            // Generate a public/private key using RSA  
            // Read private key in a string  
            _privateKey = rsa.ToXmlString(true);

            // Read public key in a string  
            _publicKey = rsa.ToXmlString(false);


            // Get key into parameters  
            //RSAParameters RSAKeyInfo = rsa.ExportParameters(true);
            //string pvtModulus = System.Text.Encoding.UTF8.GetString(RSAKeyInfo.Modulus);
            //string pvtexponent = System.Text.Encoding.UTF8.GetString(RSAKeyInfo.Exponent);
            //string pvtP = System.Text.Encoding.UTF8.GetString(RSAKeyInfo.P);
            //string pvtQ = System.Text.Encoding.UTF8.GetString(RSAKeyInfo.Q);
            //string pvtDP = System.Text.Encoding.UTF8.GetString(RSAKeyInfo.DP);
            //string pvtDQ = System.Text.Encoding.UTF8.GetString(RSAKeyInfo.DQ);

        }

        public string EncryptTripleDES(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);


            // Get the key from config file

            string key = "LASX_AUTH_TOKEN";
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string DecryptTripleDES(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);


            //Get your key from config file to open the lock!
            string key = "LASX_AUTH_TOKEN";


            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);

        }

        public string DecryptFromPrivateKey(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            var dataArray = data.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            rsa.FromXmlString(_privateKey);
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }

        public string EncryptWithPublicKey(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_publicKey);
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            var length = encryptedByteArray.Count();
            var item = 0;
            var sb = new StringBuilder();
            foreach (var x in encryptedByteArray)
            {
                item++;
                sb.Append(x);

                if (item < length)
                    sb.Append(",");
            }

            return sb.ToString();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
