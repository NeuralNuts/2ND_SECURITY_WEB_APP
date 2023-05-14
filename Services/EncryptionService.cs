using System.Security.Cryptography;

namespace _2ND_SECURITY_WEB_APP.Services
{
    public class EncryptionService
    {
        string? secretKey;

        public EncryptionService(IConfiguration config)
        {
            secretKey = config["SecretKey"];
        }

        /// <summary>
        /// Takes the provided file and encrypts it using the AES encryption algorithm.
        /// Stores the IV (initialisation vector) within the encrypted data.
        /// </summary>
        /// <param name="fileData">The file to be decrypted</param>
        /// <returns></returns>
        public byte[] EncryptByteArray(byte[] fileData)
        {
            //Using statement to manage and dispose of our class which implements
            //the AES (Asymetrical Enryption Standard) encryption algorithm.
            using (var aesAlg = new AesManaged())
            {
                //Takes the secret key and converts it to a byte array before passing
                //it to the AES class to be used when ready.
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(secretKey);
                //Sets the encryptor up with the kay and IV (initialisation vector) to be used
                //for the encryption.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                //Using statement to manage and dispose of our class which manages the
                //file stream for managin our byte arrays
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    //Writes the IV to the front of the stream so that it is included in the file.
                    msEncrypt.Write(aesAlg.IV, 0, 16);

                    //Using statement to manage and dispose of our class which implements
                    //and manages the encryption process.
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        //Encrypts the data and then writes it to the memory stream.
                        csEncrypt.Write(fileData, 0, fileData.Length);
                        //Flushes the stream to ensure it is finished writing.
                        csEncrypt.FlushFinalBlock();
                        //Returns the contents of the Memory stream.
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Takes the provided data and decrypts it using the AES encryption algorithm.
        /// Retrieves the IV (initialisation vector) from within the encrypted data propr to decyption.
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        public byte[] DecryptByteArray(byte[] encryptedData)
        {
            //Using statement to manage and dispose of our class which implements
            //the AES (Asymetrical Enryption Standard) encryption algorithm.
            using (var aesAlg = new AesManaged())
            {
                //Takes the secret key and converts it to a byte array before passing
                //it to the AES class to be used when ready.
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(secretKey);
                //Creates a byte array to hold the IV (initialisation vector) once it retrieved from the file.
                byte[] IV = new byte[16];
                //Copy the IV from the first 16 bytes of the encrypted file's data inot the IV byte array.
                Array.Copy(encryptedData, 0, IV, 0, 16);

                //Using statement to manage and dispose of our class which manages the
                //file stream for managin our byte arrays
                using (MemoryStream msDecrypt = new MemoryStream())
                {
                    //Sets the encryptor up with the kay and IV (initialisation vector) to be used
                    //for the decryption.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, IV);
                    //Using statement to manage and dispose of our class which implements
                    //and manages the encryption process.
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        //Decrypts the file starting from the byte after where the IV was hidden.
                        csDecrypt.Write(encryptedData, 16, encryptedData.Length - 16);
                        //Flushes the stream to ensure it is finished writing.
                        csDecrypt.FlushFinalBlock();
                        //Returns the contents of the Memory stream.
                        return msDecrypt.ToArray();
                    }
                }
            }
        }
    }
}
