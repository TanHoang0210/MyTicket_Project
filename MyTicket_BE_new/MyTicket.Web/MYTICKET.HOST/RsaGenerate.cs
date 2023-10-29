using Org.BouncyCastle.Utilities.IO.Pem;
using System.Security.Cryptography;

namespace MYTICKET.Hostconsle
{
    public class RsaGenerate
    {
        public static void Generate()
        {
            int keyLength = 2048;
            using RSACryptoServiceProvider rsa = new(keyLength);

            byte[] privateKey = rsa.ExportRSAPrivateKey();

            var stringWriter = new StringWriter();
            var pemWriter = new PemWriter(stringWriter);
            pemWriter.WriteObject(new PemObject("RSA PRIVATE KEY", privateKey));
            pemWriter.Writer.Flush();
            var privateKeyPem = stringWriter.ToString();

            // Lưu khóa bí mật vào file
            File.WriteAllText("private_key.pem", privateKeyPem);

            byte[] publicKey = rsa.ExportRSAPublicKey();
            string publicKeyPem = rsa.ExportRSAPublicKeyPem();

            //stringWriter = new StringWriter();
            //pemWriter = new PemWriter(stringWriter);
            //pemWriter.WriteObject(new PemObject("RSA PUBLIC KEY", publicKey));
            //pemWriter.Writer.Flush();
            //var publicKeyPem = stringWriter.ToString();

            // Lưu khóa công khai vào file
            File.WriteAllText("public_key.pem", publicKeyPem);

            TryRead();
        }

        public static void TryRead()
        {
            // Đọc khóa bí mật từ file
            string privateKeyPem = File.ReadAllText("private_key.pem");
            var privateKeyObject = new PemReader(new StringReader(privateKeyPem)).ReadPemObject();
            byte[] privateKey = privateKeyObject.Content;

            // Đọc khóa công khai từ file
            string publicKeyPem = File.ReadAllText("public_key.pem");
            var publicKeyObject = new PemReader(new StringReader(publicKeyPem)).ReadPemObject();
            byte[] publicKey = publicKeyObject.Content;

            RSACryptoServiceProvider rsa = new();
            rsa.ImportRSAPublicKey(publicKey, out int _);
            rsa.ImportRSAPrivateKey(privateKey, out int _);

            var key = rsa.ExportParameters(true); //param kèm private key
        }
    }
}
