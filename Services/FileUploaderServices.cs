namespace _2ND_SECURITY_WEB_APP.Services
{
    public class FileUploaderServices
    {
        string uploadPath;
        EncryptionService encryptionService;

        public FileUploaderServices(IWebHostEnvironment env, EncryptionService encryption)
        {
            uploadPath = Path.Combine(env.WebRootPath, "Uploads");
            encryptionService = encryption;
        }

        /// <summary>
        /// Takes the provided file and saves it to the specified file after it has been encrypted.
        /// </summary>
        /// <param name="file">The file to be saved and encrypted.</param>
        /// <returns></returns>
        public async Task SaveFile(IFormFile file)
        {
            //Retrieve the filename form the provided file
            string fileName = file.FileName;
            //CReate array to store fiel contents.
            byte[] fileContents;
            //Use a memory stream to convert the file to a byte array.
            //The using statement manages and disposes of the stream when needed.
            using (MemoryStream stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                fileContents = stream.ToArray();
            }

            //Passes the file data to the encryption service to be encrypted.
            byte[] encryptedData = encryptionService.EncryptByteArray(fileContents);
            //Using statement to manage and dispose of our class which implements
            //the management of the byte array in memory.
            using (MemoryStream encDataStream = new MemoryStream(encryptedData))
            {
                //Combine the file name and upload path to get the absolute path name for the file.
                var targetFile = Path.Combine(uploadPath, fileName);
                //Using statement to manage and dispose of our class which implements
                //and manages the file writing process.
                using (var fileStream = new FileStream(targetFile, FileMode.Create))
                {
                    //Write the file to the upload directory.
                    encDataStream.WriteTo(fileStream);
                }
            }
        }

        /// <summary>
        /// Iterates through the file directory to check if a file mathces the requested file name.
        /// </summary>
        /// <param name="fileName">The name of the file to be retrieved.</param>
        /// <returns>The requested file or null if no match is found.</returns>
        public async Task<FileInfo?> LoadFile(string fileName)
        {
            //Grab the directory info of the file path.
            DirectoryInfo dir = new DirectoryInfo(uploadPath);
            //Search the directory for a match for the provided fiel name.
            if (!dir.EnumerateFiles().Any(c => c.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase)))
            {
                //Return null of no match found.
                return null;
            }
            //Otherwise, retrun the first matching file.
            return dir.EnumerateFiles().Where(c => c.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                                       .FirstOrDefault();
        }

        /// <summary>
        /// Finds the desired file and converts it to a byte array.
        /// </summary>
        /// <param name="fileName">The name of the desired file.</param>
        /// <returns></returns>
        public async Task<byte[]?> ReadFileIntoMemory(string fileName)
        {
            //Requests the file from the upload directory
            var file = await LoadFile(fileName);
            //If no file found, return null.
            if (file == null)
            {
                return null;
            }
            //Using statement to manage and dispose of our class which implements
            //the management of the byte array in memory.
            using (var memStream = new MemoryStream())
            {
                //Using statement to manage and dispose of our class which implements
                //and manages the file reading process.
                using (var fileStream = File.OpenRead(file.FullName))
                {
                    //Send the file to the memory stream and convery it to a byte array
                    //before returning it.
                    fileStream.CopyTo(memStream);
                    return memStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Takes the specified file name and retrieves the fiel from the directory before
        /// sending it for decryption and returning it to the caller.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<byte[]?> LoadEncryptedFile(string fileName)
        {
            //Retrieve the fiel from the directory.
            var encryptedFileData = await ReadFileIntoMemory(fileName);
            //If no file found or it is empty, return null.
            if (encryptedFileData == null || encryptedFileData.Length == 0)
            {
                return null;
            }
            //Send the file to the encryption service to be decrypted.
            var decryptedData = encryptionService.DecryptByteArray(encryptedFileData);
            //Return the decrypted file.
            return decryptedData;
        }
    }
}
