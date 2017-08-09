using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace EUSignCP
{
    /// <summary>
    /// Робта з повідомленнями та квитанціями згідно з уніфікованим форматом
    /// транспортного повідомлення при інформаційній взаємодії платників податків 
    /// і податкових органів в електронному вигляді телекомунікаційними каналами зв’язку з 
    /// використанням електронного цифрового підпису (бібліотека EUSignCP.DLL).
    /// </summary>
    public static partial class DFSPackHelper
    {
        private const string ERROR_NOT_INITIALIZED = "Криптографічну бібліотеку не ініціалізовано.";
        private const string ERROR_BAD_PRIVATE_KEY = "Особистий ключ не зчитано.";
           
        /// <summary>
        /// Ініціалізація бібліотеки.
        /// </summary>
        public static void Initialize()
        {
            int error = IEUSignCP.Initialize();
            if (error == IEUSignCP.EU_ERROR_NONE)
            {
                IEUSignCP.SetUIMode(false);

                string issuer = "O=Інформаційно-довідковий департамент ДФС;OU=Управління (центр) сертифікації ключів ІДД ДФС;CN=Акредитований центр сертифікації ключів ІДД ДФС;Serial=UA-39384476;C=UA;L=Київ";
                string serial = "33B6CB7BF721B9CE0400000054FC1B008F645800";

                IEUSignCP.EU_CERT_INFO_EX certInfoEx;

                error = IEUSignCP.GetCertificateInfoEx(issuer, serial, out certInfoEx);

                IEUSignCP.SetUIMode(true);

                if (error != IEUSignCP.EU_ERROR_NONE &&
                    error != IEUSignCP.EU_ERROR_CERT_NOT_FOUND) throw new Exception(IEUSignCP.GetErrorDesc(error));

                if (error != IEUSignCP.EU_ERROR_CERT_NOT_FOUND)
                {
                    Certificates.Recipient.CertInfoEx = certInfoEx;
                }
            }
            else
            {
                throw new Exception(IEUSignCP.GetErrorDesc(error));
            }
        }

        /// <summary>
        /// Встановлення параметрів роботи з бібліотекою за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        public static void SetSettings()
        {
            if (!IEUSignCP.IsInitialized()) throw new Exception(ERROR_NOT_INITIALIZED);

            IEUSignCP.SetSettings();
        }

        /// <summary>
        /// Зчитування особистого ключа за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        /// <param name="certOwnerInfo">Інформація про сертифікат власника.</param>
        public static void ReadPrivateKey() 
        {         
            if (!IEUSignCP.IsInitialized()) throw new Exception(ERROR_NOT_INITIALIZED);

            if (IEUSignCP.IsPrivateKeyReaded())
            {
                IEUSignCP.ResetPrivateKey();
                Certificates.Own.Clear();              
            }

            IEUSignCP.EU_CERT_OWNER_INFO certOwnerInfo;

            int error = IEUSignCP.ReadPrivateKey(out certOwnerInfo);
            if (error != IEUSignCP.EU_ERROR_NONE) throw new Exception(IEUSignCP.GetErrorDesc(error));

            IEUSignCP.EU_CERT_INFO_EX certInfoEx;

            error = IEUSignCP.GetCertificateInfoEx(certOwnerInfo.issuer, certOwnerInfo.serial, out certInfoEx);
            if (error != IEUSignCP.EU_ERROR_NONE) throw new Exception(IEUSignCP.GetErrorDesc(error));

            Certificates.Own.CertInfoEx = certInfoEx;
        }

        /// <summary>
        /// Відображення власного сертифіката за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        public static void ShowOwnCertificate()
        {
            if (!IEUSignCP.IsInitialized()) throw new Exception(ERROR_NOT_INITIALIZED);
            if (!IEUSignCP.IsPrivateKeyReaded()) throw new Exception(ERROR_BAD_PRIVATE_KEY);

            IEUSignCP.ShowOwnCertificate();
            
        }

        /// <summary>
        /// Конвертація файлового потоку в масив байт.
        /// </summary>
        /// <param name="fs">Файловий поток.</param>
        /// <returns>Масив байт.</returns>
        private static byte[] fs2bytes(FileStream fs)
        {
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                fs.CopyTo(ms);
                bytes = ms.ToArray();
            }
            return bytes;
        }

        /// <summary>
        /// Формування ЕЦП файлу.
        /// </summary>
        /// <param name="fileName">Ім'я файлу з даними.</param>
        /// <param name="fileNameWithSign">Ім'я файлу, в який необхідно записати підписані дані.</param>
        public static void SignFile(string fileName, string fileNameWithSign)
        {
            if (!IEUSignCP.IsInitialized()) throw new Exception(ERROR_NOT_INITIALIZED);
            if (!IEUSignCP.IsPrivateKeyReaded()) throw new Exception(ERROR_BAD_PRIVATE_KEY);

            using (FileStream fsIn = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] data = DFSPackHelper.fs2bytes(fsIn);
                byte[] signData;

                int error = IEUSignCP.SignDataInternal(true, data, out signData);
                if (error != IEUSignCP.EU_ERROR_NONE) throw new Exception(IEUSignCP.GetErrorDesc(error));

                using (FileStream fsOut = new FileStream(fileNameWithSign, FileMode.Create, FileAccess.Write))
                {
                    byte[] tag = Encoding.ASCII.GetBytes("UA1_SIGN");
                    fsOut.Write(tag, 0, tag.Length);
                    fsOut.WriteByte(0);
                    fsOut.Write(BitConverter.GetBytes(signData.Length), 0, 4);
                    fsOut.Write(signData, 0, signData.Length);
                }
            }
        }

        /// <summary>
        /// Отримання інформації про сертифікат отримувача за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        public static void GetRecipientCertificate()
        {
            if (!IEUSignCP.IsInitialized()) throw new Exception(ERROR_NOT_INITIALIZED);

            Certificates.Recipient.Clear();

            IEUSignCP.EU_CERT_OWNER_INFO certOwnerInfo;
            IEUSignCP.EU_CERT_INFO_EX certInfoEx;

            int error = IEUSignCP.SelectCertInfo(out certOwnerInfo);
            if (error != IEUSignCP.EU_ERROR_NONE) throw new Exception(IEUSignCP.GetErrorDesc(error));

            error = IEUSignCP.GetCertificateInfoEx(certOwnerInfo.issuer, certOwnerInfo.serial, out certInfoEx);
            if (error != IEUSignCP.EU_ERROR_NONE) throw new Exception(IEUSignCP.GetErrorDesc(error));
            
            if (certInfoEx.keyUsageBits != 16) 
            {
                throw new Exception("Обраний сертифікат отримувача не призначений для шифрування.\nОберіть інший сертифікат отримувача.");
            }

            Certificates.Recipient.CertInfoEx = certInfoEx;
        }

        /// <summary>
        /// Отримання інформації про сертифікат відправника для зашифрування. 
        /// </summary>
        /// <param name="certInfoEx">Інформація про сертифікат (розширена).</param>
        public static byte[] GetOwnCertificateForEnvelop()
        {
            byte[] bCert = new byte[0];

            IEUSignCP.EU_CERT_INFO_EX cert;
            int index = 0;

            for (; ; )
            {
                int error = IEUSignCP.EnumOwnCertificates(index, out cert);
                if (error == IEUSignCP.EU_WARNING_END_OF_ENUM) break;
                if (error != IEUSignCP.EU_ERROR_NONE) throw new Exception(IEUSignCP.GetErrorDesc(error));

                if (cert.keyUsageBits == 16)
                {
                    error = IEUSignCP.GetCertificate(cert.issuer, cert.serial, out bCert);
                    if (error != IEUSignCP.EU_ERROR_NONE) throw new Exception(IEUSignCP.GetErrorDesc(error));
                    break;
                }
                index += 1;
            }

            if (bCert.Length == 0) 
            {
                throw new Exception("Відсутній сертифікат відправника для зашифрування.");
            }

            return bCert;
        }

        /// <summary>
        /// Зашифрування файла.
        /// </summary>
        /// <param name="fileName">Ім'я файлу з даними.</param>
        /// <param name="envelopedFileName">Ім'я файлу, в який необхідно записати зашифровані дані.</param>
        public static void EnvelopFile(string fileName, string envelopedFileName)
        {
            if (!IEUSignCP.IsInitialized()) throw new Exception(ERROR_NOT_INITIALIZED);
            if (!IEUSignCP.IsPrivateKeyReaded()) throw new Exception(ERROR_BAD_PRIVATE_KEY);

            if (!Certificates.Own.IsLoaded()) throw new Exception("Не обрано власний сертифікат відправника.");
            if (!Certificates.Recipient.IsLoaded()) throw new Exception("Не обрано сертифікат одержувача.");

            using (FileStream fsIn = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] data = fs2bytes(fsIn);           

                byte[] envelopedData;

                int error = IEUSignCP.EnvelopData(Certificates.Recipient.CertInfoEx.issuer,
                    Certificates.Recipient.CertInfoEx.serial, data, out envelopedData);
                if (error != IEUSignCP.EU_ERROR_NONE) throw new Exception(IEUSignCP.GetErrorDesc(error));

                using (FileStream fsOut = new FileStream(envelopedFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] tag = Encoding.ASCII.GetBytes("TRANSPORTABLE");
                    fsOut.Write(tag, 0, tag.Length);
                    fsOut.WriteByte(0);

                    byte[] header = Encoding.ASCII.GetBytes("PRG_TYPE=EUSignDFS\r\n" +
                        "PRG_VER=1.0\r\n" +
                        "FILENAME=" + Path.GetFileName(fileName.Replace(".signBDP", "")) + "\r\n" + 
                        "EDRPOU=" + Certificates.Own.CertInfoEx.subjEDRPOUCode + "\r\n" +
                        "STTYPE=1\r\n");
              
                    fsOut.Write(BitConverter.GetBytes(header.Length), 0, 4);
                    fsOut.Write(header, 0, header.Length);

                    byte[] tagCert = Encoding.ASCII.GetBytes("CERTCRYPT");
                    fsOut.Write(tagCert, 0, tagCert.Length);
                    fsOut.WriteByte(0);

                    byte[] cert = GetOwnCertificateForEnvelop();
                    fsOut.Write(BitConverter.GetBytes(cert.Length), 0, 4);
                    fsOut.Write(cert, 0, cert.Length);

                    byte[] tagCrypt = Encoding.ASCII.GetBytes("UA1_CRYPT");
                    fsOut.Write(tagCrypt, 0, tagCrypt.Length);
                    fsOut.WriteByte(0);

                    fsOut.Write(BitConverter.GetBytes(envelopedData.Length), 0, 4);
                    fsOut.Write(envelopedData, 0, envelopedData.Length);
                }
            }
        }

        /// <summary>
        /// Розшифрування файлу.
        /// </summary>
        /// <param name="envelopedFileName">Ім'я файлу з зашифрованими даними.</param>
        /// <param name="fileName">Ім'я файлу в який необхідно записати розшифровані дані.</param>
        public static void DevelopFile(string envelopedFileName)
        {
            if (!IEUSignCP.IsInitialized()) throw new Exception(ERROR_NOT_INITIALIZED);
            if (!IEUSignCP.IsPrivateKeyReaded()) throw new Exception(ERROR_BAD_PRIVATE_KEY);

            using (FileStream fs_in = new FileStream(envelopedFileName, FileMode.Open, FileAccess.Read))
            {
                // Зняття обгорток з шифрованих даних.
                fs_in.Seek(14, SeekOrigin.Begin);
                byte[] bSizeTRANSPORTABLE = new byte[4];
                fs_in.Read(bSizeTRANSPORTABLE, 0, 4);
                int sizeTRANSPORTABLE = BitConverter.ToInt32(bSizeTRANSPORTABLE, 0);
                fs_in.Seek(sizeTRANSPORTABLE, SeekOrigin.Current);

                fs_in.Seek(10, SeekOrigin.Current);
                byte[] bSizeCERTCRYPT = new byte[4];
                fs_in.Read(bSizeCERTCRYPT, 0, 4);
                int sizeCERTCRYPT = BitConverter.ToInt32(bSizeCERTCRYPT, 0);
                fs_in.Seek(sizeCERTCRYPT, SeekOrigin.Current);

                fs_in.Seek(14, SeekOrigin.Current);

                int sizeEnvelopData;
                sizeEnvelopData = (int)(fs_in.Length - 18 - sizeTRANSPORTABLE - 14 - sizeCERTCRYPT - 14);
                byte[] bEnvelopData = new byte[sizeEnvelopData];
                fs_in.Read(bEnvelopData, 0, sizeEnvelopData);

                // Розшифрування даних.
                byte[] bDevelopData;
                IEUSignCP.EU_SENDER_INFO senderInfo;

                IEUSignCP.DevelopData(bEnvelopData, out bDevelopData, out senderInfo);

                // Зняття обгорток з підписанних даних.
                int sizeSignData = bDevelopData.Length - 13;
                byte[] bSignData = new byte[sizeSignData];
                Array.Copy(bDevelopData, 13, bSignData, 0, sizeSignData);

                // Зняття підпису з данних XML-файлу.
                if (Path.GetExtension(envelopedFileName).ToUpper() == ".XML")
                {
                    string dirName = envelopedFileName + ".orig";
                    Directory.CreateDirectory(dirName);
                    string verifyFileName = dirName + "\\" + Path.GetFileName(envelopedFileName);

                    byte[] bData;
                    IEUSignCP.EU_SIGN_INFO signInfo;

                    IEUSignCP.VerifyDataInternal(bSignData, out bData, out signInfo);

                    using (FileStream fs_out = new FileStream(verifyFileName, FileMode.Create, FileAccess.Write))
                    {
                        fs_out.Write(bData, 0, bData.Length);
                    }
                    return;
                }

                // Зняття підпису з данних ZIP-файлу та підписів з вкладених до архіву файлів.
                if (Path.GetExtension(envelopedFileName).ToUpper() == ".ZIP")
                {
                    string dirName = envelopedFileName + ".orig";
                    Directory.CreateDirectory(dirName);

                    byte[] bZipData;
                    IEUSignCP.EU_SIGN_INFO signInfo;

                    IEUSignCP.VerifyDataInternal(bSignData, out bZipData, out signInfo);
                    MemoryStream msZipData = new MemoryStream(bZipData);

                    ZipArchive archive = new ZipArchive(msZipData);

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string verifyFileName = dirName + "\\" + entry.Name;

                        byte[] bEntryData;

                        using (MemoryStream ms = new MemoryStream())
                        {
                            entry.Open().CopyTo(ms);
                            bEntryData = ms.ToArray();
                        }

                        // Зняття обгорток з підписанних даних.
                        int sizeSignEntryData = bEntryData.Length - 13;
                        byte[] bSignEntryData = new byte[sizeSignEntryData];
                        Array.Copy(bEntryData, 13, bSignEntryData, 0, sizeSignEntryData);

                        byte[] bData;
                        IEUSignCP.EU_SIGN_INFO signInfoEntry;

                        IEUSignCP.VerifyDataInternal(bSignEntryData, out bData, out signInfoEntry);

                        using (FileStream fs_out = new FileStream(verifyFileName, FileMode.Create, FileAccess.Write))
                        {
                            fs_out.Write(bData, 0, bData.Length);
                        }
                   }
                }
            }
        }
    }
}
