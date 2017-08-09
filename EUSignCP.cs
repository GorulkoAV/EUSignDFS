using System;
using System.Runtime.InteropServices;

namespace EUSignCP
{
    public class IEUSignCP
    {
        // Функції бібліотеки, що повертають коди помилок типу DWORD, повертають нульове значення у разі успіху 
        // або ненульове значення впротилежному випадку. Бібліотека у разі виникнення помилок за замовчанням видає 
        // відповідні повідомлення у діалогові вікна (ОС Microsoft Windows). Цю можливість можна відключити, 
        // якщо встановити режим без використання графічного інтерфейсу. При цьому функції бібліотеки 
        // з діалоговими вікнами будуть повертати помилку EU_ERROR_NO_GUI_DIALOGS.

        // --------------------------------------------------------------------
        // Значення кодів помилок.

        /// <summary>
        /// Помилка відсутня. Не є помилкою. Повідомляє про успішне виконання.
        /// </summary>
        public const int EU_ERROR_NONE = 0x0000;
        /// <summary>
        /// Невідома помилка. Відмова бібліотеки.
        /// </summary>
        public const int EU_ERROR_UNKNOWN = 0xFFFF;
        /// <summary>
        /// Виникла помилка при виділенні пам'яті.
        /// </summary>
        public const int EU_ERROR_MEMORY_ALLOCATION = 0x0006;
        /// <summary>
        /// Код помилки - Перелічення закінчено. Не є помилкою. Повідомляє про завершення списку переліку.
        /// </summary>
        public const int EU_WARNING_END_OF_ENUM = 0x0007;
        /// <summary>
        /// Операція відмінена оператором. Не виникає за відсутності діалогів з оператором.
        /// </summary>
        public const int EU_ERROR_CANCELED_BY_GUI = 0x000C;
        /// <summary>
        /// Код помилки - Сертифікат не знайдено. Сертифікат не знайдено жодними з доступних засобів. Послідовність пошуку – файлове сховище, протокол OCSP, LDAP-каталог.
        /// </summary>
        public const int EU_ERROR_CERT_NOT_FOUND = 0x0033;

        /// <summary>
        /// Виключення - помилка при виділенні пам'яті.
        /// </summary>
        private class ErrorMemoryAllocation : System.Exception
        {
            public ErrorMemoryAllocation() : base() { }
        }

        // --------------------------------------------------------------------
        // Розміри некерованих типів даних в максимальному размірі (64-бітна операційна система).

        /// <summary>
        /// Розмір структури EU_CERT_OWNER_INFO в байтах. 
        /// </summary>
        private const int EU_CERT_OWNER_INFO_SIZE = 140;
        /// <summary>
        /// Розмір структури EU_CERT_INFO_EX в байтах.
        /// </summary>
        private const int EU_CERT_INFO_EX_SIZE = 416;
        /// <summary>
        /// Розмір структури EU_SIGN_INFO_SIZE в байтах.
        /// </summary>
        private const int EU_SIGN_INFO_SIZE = 164;
        /// <summary>
        /// Розмір структури EU_ENVELOP_INFO_SIZE в байтах.
        /// </summary>
        private const int EU_SENDER_INFO_SIZE = 164;

        // --------------------------------------------------------------------
        // Структури даних, що створюються на основі вихідних даних некореваного коду.

        /// <summary>
        /// Структура із описом часу (SIZE = 8 * 2 bytes = 16 bytes).
        /// </summary>
        public struct SYSTEMTIME
        {
            public short Year;        // Рік.
            public short Month;       // Місяць.
            public short DayOfWeek;   // День тижня.
            public short Day;         // День.
            public short Hour;        // Година.
            public short Minute;      // Хвилина. 
            public short Second;      // Секунда.
            public short Millisecond; // Мілісекунд.
        };

        /// <summary>
        /// Структура із описом інформації про сертифікат власника особистого ключа.
        /// </summary>
        public struct EU_CERT_OWNER_INFO
        {
            public bool filled;           // Признак заповнення структури.
            public string issuer;         // Ім'я ЦСК, що видав сертифікат.
            public string issuerCN;       // Реквізити ЦСК, що видав сертифікат.
            public string serial;         // Реєстраційний номер сертифіката.
            public string subject;        // Ім'я власника сертифіката.
            public string subjCN;         // Реквізити власника сертифіката.
            public string subjOrg;        // Організація, до якої належить власник сертифіката.
            public string subjOrgUnit;    // Підрозділ організації, до якої належить власник сертифіката.
            public string subjTitle;      // Посада власника сертифіката.
            public string subjState;      // Назва області, до якої належить власник сертифіката.
            public string subjLocality;   // Назва населеного пункту, до якого належить власник сертифіката.
            public string subjFullName;   // Повне ім'я власника сертифіката.
            public string subjAddress;    // Адреса власника сертифіката.
            public string subjPhone;      // Номер телефону власника сертифіката.
            public string subjEMail;      // Адреса електронної пошти власника сертифіката.
            public string subjDNS;        // DNS-ім'я чи інше технічного засобу.
            public string subjEDRPOUCode; // Код ЄДРПОУ власника сертифіката.
            public string subjDRFOCode;   // Код ДРФО власника сертифіката.

            public EU_CERT_OWNER_INFO(IntPtr pCertOwnerInfo)
            {
                try
                {
                    long ptr = pCertOwnerInfo.ToInt64();

                    // BOOL - 4 / 4 bytes.
                    this.filled = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // PSTR - 8 / 12 bytes.
                    this.issuer = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 20 bytes.
                    this.issuerCN = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 28 bytes.
                    this.serial = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 36 bytes.
                    this.subject = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 44 bytes.
                    this.subjCN = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 52 bytes.
                    this.subjOrg = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 60 bytes.
                    this.subjOrgUnit = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 68 bytes.
                    this.subjTitle = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 76 bytes.
                    this.subjState = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 84 bytes.
                    this.subjLocality = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 92 bytes.
                    this.subjFullName = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 100 bytes.
                    this.subjAddress = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 108 bytes.
                    this.subjPhone = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 116 bytes.
                    this.subjEMail = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 124 bytes.
                    this.subjDNS = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 132 bytes.
                    this.subjEDRPOUCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 140 bytes.
                    this.subjDRFOCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                }
                catch (Exception)
                {
                    this.filled = false;
                    this.issuer = "";
                    this.issuerCN = "";
                    this.serial = "";
                    this.subject = "";
                    this.subjCN = "";
                    this.subjOrg = "";
                    this.subjOrgUnit = "";
                    this.subjTitle = "";
                    this.subjState = "";
                    this.subjLocality = "";
                    this.subjFullName = "";
                    this.subjAddress = "";
                    this.subjPhone = "";
                    this.subjEMail = "";
                    this.subjDNS = "";
                    this.subjEDRPOUCode = "";
                    this.subjDRFOCode = "";
                }
            }
        };

        /// <summary>
        /// Структура із описом детальної інформації про сертифікат (розширена).
        /// </summary>
        public struct EU_CERT_INFO_EX
        {
            public bool filled;                 // Признак заповнення структури.
            public int version;                 // Версія структури з сертифікатом.
            public string issuer;               // Ім'я ЦСК, що видав сертифікат.
            public string issuerCN;             // Реквізити ЦСК, що видав сертифікат.
            public string serial;               // Реєстраційний номер сертифіката.
            public string subject;              // Ім'я власника сертифіката.
            public string subjCN;               // Реквізити власника сертифіката. 
            public string subjOrg;              // Організація до якої належить, власник сертифіката.
            public string subjOrgUnit;          // Підрозділ організації, до якої належить власник сертифіката.
            public string subjTitle;            // Посада власника сертифіката.
            public string subjState;            // Назва області, до якої належить власник сертифіката.
            public string subjLocality;         // Назва населеного пункту до якого, належить власник сертифіката.
            public string subjFullName;         // Повне ім'я власника сертифіката.
            public string subjAddress;          // Адреса власника сертифіката.
            public string subjPhone;            // Номер телефону власника сертифіката.
            public string subjEMail;            // Адреса електронної пошти власника сертифіката.
            public string subjDNS;              // DNS-ім'я чи інше технічного засобу.
            public string subjEDRPOUCode;       // Код ЄДРПОУ власника сертифіката.
            public string subjDRFOCode;         // Код ДРФО власника сертифіката.
            public string subjNBUCode;          // Ідентифікатор НБУ власника сертифіката.
            public string subjSPFMCode;         // Код СПФМ власника сертифіката.
            public string subjOCode;            // Код організації власника сертифіката.
            public string subjOUCode;           // Код підрозділу власника сертифіката.
            public string subjUserCode;         // Код користувача власника сертифіката.
            public SYSTEMTIME certBeginTime;    // Час введення сертифіката в дію.
            public SYSTEMTIME certEndTime;      // Дата закінчення дії сертифіката.
            public bool privKeyTimesExists;     // Признак наявності строку дії особистого ключа.
            public SYSTEMTIME privKeyBeginTime; // Час введення в дію особистого ключа.
            public SYSTEMTIME privKeyEndTime;   // Час виведення з дії особистого ключа.
            public int publicKeyBits;           // Довжина відкритого ключа в бітах.
            public string publicKey;            // Відкритий ключ у вигляді строки.
            public string publicKeyID;          // Ідентифікатор відкритого ключа у вигляді строки.
            public string issuerPublicKeyID;    // Ідентифікатор відкритого ключа ЦСК у вигляді строки.
            public string keyUsage;             // Використання ключів у вигляді строки. 
            public string extKeyUsages;         // Уточнене призначення ключів.
            public string policies;             // Правила сертифікації.
            public string crlDistribPoint1;     // Точка доступу до повних СВС
            public string crlDistribPoint2;     // Точка доступу до часткових СВС
            public bool powerCert;              // Признак того, що сертифікат посилений.
            public bool subjType;               // Тип власника сертифікату.
            public bool subjCA;                 // Признак того, що власник сертифікату ЦСК.
            public int chainLength;		        // Обмеження на довжину ланцюжка сертифікатів.
            public string upn;			        // UPN-ім'я власника сертифіката.
            public int publicKeyType;		    // Тип відкритого ключа.
            public int keyUsageBits;			// Використання ключів у вигляді бітів.
            public string rsaModul;		        // Модуль RSA у вигляді строки.
            public string rsaExponent;		    // Експонента RSA у вигляді строки.
            public string ocspAccessInfo;	    // Точка доступу до OCSP-сервера.
            public string issuerAccessInfo;	    // Точка доступу до сертифікатів.
            public string tspAccessInfo;		// Точка доступу до TSP-сервера.
            public bool limitValueAvailable;	// Признак наявності обмеження на транзакцію.
            public int limitValue;		        // Максимальне обмеження на транзакцію.
            public string limitValueCurrency;	// Валюта максимального обмеження на транзакцію.
            public int subjTypeCert;		    // Тип власника сертифіката (поле доступне з dwVersion > 2).
            public int subjSubTypeCert;		    // Тип власника сертифіката для серверів ЦСК (поле доступне з dwVersion > 2).

            public EU_CERT_INFO_EX(IntPtr pCertInfoEx)
            {
                try
                {
                    if (pCertInfoEx == IntPtr.Zero) throw new Exception();

                    long ptr = pCertInfoEx.ToInt64();

                    // BOOL - 4 / 4 bytes.
                    this.filled = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // DWORD - 4 / 8 bytes.
                    this.version = Marshal.ReadInt32((IntPtr)ptr);
                    ptr += 4;
                    // PSTR - 8 / 16 bytes.
                    this.issuer = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 24 bytes.
                    this.issuerCN = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 32 bytes.
                    this.serial = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 40 bytes.
                    this.subject = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 48 bytes.
                    this.subjCN = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 56 bytes.
                    this.subjOrg = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 64 bytes.
                    this.subjOrgUnit = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 72 bytes.
                    this.subjTitle = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 80 bytes.
                    this.subjState = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 88 bytes.
                    this.subjLocality = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 96 bytes.
                    this.subjFullName = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 104 bytes.
                    this.subjAddress = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 112 bytes.
                    this.subjPhone = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 120 bytes.
                    this.subjEMail = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 128 bytes.
                    this.subjDNS = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 136 bytes.
                    this.subjEDRPOUCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 144 bytes.
                    this.subjDRFOCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 152 bytes.
                    this.subjNBUCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 160 bytes.
                    this.subjSPFMCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 168 bytes.
                    this.subjOCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 176 bytes.
                    this.subjOUCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 184 bytes.
                    this.subjUserCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // SYSTEMTIME - 16 / 200 bytes.
                    this.certBeginTime = (SYSTEMTIME)Marshal.PtrToStructure((IntPtr)ptr, typeof(SYSTEMTIME));
                    ptr += Marshal.SizeOf(typeof(SYSTEMTIME));
                    // SYSTEMTIME - 16 / 216 bytes.
                    this.certEndTime = (SYSTEMTIME)Marshal.PtrToStructure((IntPtr)ptr, typeof(SYSTEMTIME));
                    ptr += Marshal.SizeOf(typeof(SYSTEMTIME));
                    // BOOL - 4 / 220 bytes.
                    this.privKeyTimesExists = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // SYSTEMTIME - 16 / 236 bytes.
                    this.privKeyBeginTime = (SYSTEMTIME)Marshal.PtrToStructure((IntPtr)ptr, typeof(SYSTEMTIME));
                    ptr += Marshal.SizeOf(typeof(SYSTEMTIME));
                    // SYSTEMTIME - 16 / 252 bytes.
                    this.privKeyEndTime = (SYSTEMTIME)Marshal.PtrToStructure((IntPtr)ptr, typeof(SYSTEMTIME));
                    ptr += Marshal.SizeOf(typeof(SYSTEMTIME));
                    // DWORD - 4 / 256 bytes.
                    this.publicKeyBits = Marshal.ReadInt32((IntPtr)ptr);
                    ptr += 4;
                    // PSTR - 8 / 264 bytes.
                    this.publicKey = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 272 bytes.
                    this.publicKeyID = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 280 bytes.
                    this.issuerPublicKeyID = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 288 bytes.
                    this.keyUsage = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 296 bytes.
                    this.extKeyUsages = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 304 bytes.
                    this.policies = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 312 bytes.
                    this.crlDistribPoint1 = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 320 bytes.
                    this.crlDistribPoint2 = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // BOOL - 4 / 324 bytes.
                    this.powerCert = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // BOOL - 4 / 328 bytes.
                    this.subjType = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // BOOL - 4 / 332 bytes.
                    this.subjCA = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // BOOL - 4 / 336 bytes.
                    this.chainLength = Marshal.ReadInt32((IntPtr)ptr);
                    ptr += 4;
                    // PSTR - 8 / 344 bytes.
                    this.upn = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // DWORD - 4 / 348 bytes.
                    this.publicKeyType = Marshal.ReadInt32((IntPtr)ptr);
                    ptr += 4;
                    // DWORD - 4 / 352 bytes.
                    this.keyUsageBits = Marshal.ReadInt32((IntPtr)ptr);
                    ptr += 4;
                    // PSTR - 8 / 360 bytes.
                    this.rsaModul = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 368 bytes.
                    this.rsaExponent = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 376 bytes.
                    this.ocspAccessInfo = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 384 bytes.
                    this.issuerAccessInfo = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 392 bytes.
                    this.tspAccessInfo = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // BOOL - 4 / 396 bytes.
                    this.limitValueAvailable = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // DWORD - 4 / 400 bytes.
                    this.limitValue = Marshal.ReadInt32((IntPtr)ptr);
                    ptr += 4;
                    // PSTR - 8 / 408 bytes.
                    this.limitValueCurrency = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // DWORD - 4 / 412 bytes.
                    this.subjTypeCert = Marshal.ReadInt32((IntPtr)ptr);
                    ptr += 4;
                    // PSTR - 4 / 416 bytes.
                    this.subjSubTypeCert = Marshal.ReadInt32((IntPtr)ptr);
                    ptr += 4;
                }
                catch (Exception)
                {
                    this.filled = false;
                    this.version = 0;
                    this.issuer = "";
                    this.issuerCN = "";
                    this.serial = "";
                    this.subject = "";
                    this.subjCN = "";
                    this.subjOrg = "";
                    this.subjOrgUnit = "";
                    this.subjTitle = "";
                    this.subjState = "";
                    this.subjLocality = "";
                    this.subjFullName = "";
                    this.subjAddress = "";
                    this.subjPhone = "";
                    this.subjEMail = "";
                    this.subjDNS = "";
                    this.subjEDRPOUCode = "";
                    this.subjDRFOCode = "";
                    this.subjNBUCode = "";
                    this.subjSPFMCode = "";
                    this.subjOCode = "";
                    this.subjOUCode = "";
                    this.subjUserCode = "";
                    this.certBeginTime = new SYSTEMTIME();
                    this.certEndTime = new SYSTEMTIME();
                    this.privKeyTimesExists = false;
                    this.privKeyBeginTime = new SYSTEMTIME();
                    this.privKeyEndTime = new SYSTEMTIME();
                    this.publicKeyBits = 0;
                    this.publicKey = "";
                    this.publicKeyID = "";
                    this.issuerPublicKeyID = "";
                    this.keyUsage = "";
                    this.extKeyUsages = "";
                    this.policies = "";
                    this.crlDistribPoint1 = "";
                    this.crlDistribPoint2 = "";
                    this.powerCert = false;
                    this.subjType = false;
                    this.subjCA = false;
                    this.chainLength = 0;
                    this.upn = "";
                    this.publicKeyType = 0;
                    this.keyUsageBits = 0;
                    this.rsaModul = "";
                    this.rsaExponent = "";
                    this.ocspAccessInfo = "";
                    this.issuerAccessInfo = "";
                    this.tspAccessInfo = "";
                    this.limitValueAvailable = false;
                    this.limitValue = 0;
                    this.limitValueCurrency = "";
                    this.subjTypeCert = 0;
                    this.subjSubTypeCert = 0;
                }
            }
        };

        /// <summary>
        /// Структура з інформацією про сертифікати.
        /// </summary>
        public struct EU_CERTIFICATES
        {
            public int count;                      // Кількість сертифікатів.
            public EU_CERT_INFO_EX[] items; // Масив з інформацією про сертифікати

            public EU_CERTIFICATES(IntPtr pCertificates)
            {
                try
                {
                    long ptr = pCertificates.ToInt64();

                    // DWORD 4 / 4 bytes.
                    this.count = Marshal.ReadInt32((IntPtr)ptr);
                    ptr += 4;
                    // P -> P[] -> EU_CERT_INFO_EX.
                    this.items = new EU_CERT_INFO_EX[this.count];
                    ptr = Marshal.ReadIntPtr((IntPtr)ptr).ToInt64();
                    for (int i = 0; i < count; i++)
                        items[i] = new EU_CERT_INFO_EX(Marshal.ReadIntPtr((IntPtr)(ptr + i * Marshal.SizeOf(typeof(IntPtr)))));
                }
                catch (Exception)
                {
                    this.count = 0;
                    this.items = new EU_CERT_INFO_EX[0];
                }
            }
        };

        /// <summary>
        /// Структура із описом інформації про підпис (сертифікат підписувача та час підпису).
        /// </summary>
        public struct EU_SIGN_INFO
        {
            public bool filled;           // Признак заповнення структури.
            public string issuer;         // Ім'я ЦСК, що видав сертифікат.
            public string issuerCN;       // Реквізити ЦСК, що видав сертифікат.
            public string serial;         // Реєстраційний номер сертифіката.
            public string subject;        // Ім'я власника сертифіката.
            public string subjCN;         // Реквізити власника сертифіката.
            public string subjOrg;        // Організація, до якої належить власник сертифіката.
            public string subjOrgUnit;    // Підрозділ організації, до якої належить власник сертифіката.
            public string subjTitle;      // Посада власника сертифіката.
            public string subjState;      // Назва області, до якої належить власник сертифіката.
            public string subjLocality;   // Назва населеного пункту, до якого належить власник сертифіката.
            public string subjFullName;   // Повне ім'я власника сертифіката.
            public string subjAddress;    // Адреса власника сертифіката.
            public string subjPhone;      // Номер телефону власника сертифіката.
            public string subjEMail;      // Адреса електронної пошти власника сертифіката.
            public string subjDNS;        // DNS-ім'я чи інше технічного засобу.
            public string subjEDRPOUCode; // Код ЄДРПОУ власника сертифіката.
            public string subjDRFOCode;   // Код ДРФО власника сертифіката.
            public bool timeAvail;        // Признак наявності часу підпису.
            public bool timeStamp;        // Признак наявності позначки часу отриманої з TSP сервера.
            public SYSTEMTIME time;       // Час підпису або позначка часу.

            public EU_SIGN_INFO(IntPtr pSignInfo)
            {
                try
                {
                    long ptr = pSignInfo.ToInt64();

                    // BOOL - 4 / 4 bytes.
                    this.filled = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // PSTR - 8 / 12 bytes.
                    this.issuer = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 20 bytes.
                    this.issuerCN = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 28 bytes.
                    this.serial = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 36 bytes.
                    this.subject = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 44 bytes.
                    this.subjCN = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 52 bytes.
                    this.subjOrg = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 60 bytes.
                    this.subjOrgUnit = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 68 bytes.
                    this.subjTitle = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 76 bytes.
                    this.subjState = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 84 bytes.
                    this.subjLocality = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 92 bytes.
                    this.subjFullName = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 100 bytes.
                    this.subjAddress = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 108 bytes.
                    this.subjPhone = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 116 bytes.
                    this.subjEMail = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 124 bytes.
                    this.subjDNS = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 132 bytes.
                    this.subjEDRPOUCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 140 bytes.
                    this.subjDRFOCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // BOOL - 4 / 144 bytes.
                    this.timeAvail = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // BOOL - 4 / 148 bytes.
                    this.timeStamp = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // SYSTEMTIME - 16 / 164 bytes.
                    this.time = (SYSTEMTIME)Marshal.PtrToStructure((IntPtr)ptr, typeof(SYSTEMTIME));
                }
                catch (Exception)
                {
                    this.filled = false;
                    this.issuer = "";
                    this.issuerCN = "";
                    this.serial = "";
                    this.subject = "";
                    this.subjCN = "";
                    this.subjOrg = "";
                    this.subjOrgUnit = "";
                    this.subjTitle = "";
                    this.subjState = "";
                    this.subjLocality = "";
                    this.subjFullName = "";
                    this.subjAddress = "";
                    this.subjPhone = "";
                    this.subjEMail = "";
                    this.subjDNS = "";
                    this.subjEDRPOUCode = "";
                    this.subjDRFOCode = "";
                    this.timeAvail = false;
                    this.timeStamp = false;
                    this.time = new SYSTEMTIME();
                }
            }
        };

        /// <summary>
        /// Структура із описом інформації про відправника зашифрованих даних (сертифікат відправника та час підпису, якщо наявний останній).      
        /// </summary>
        public struct EU_SENDER_INFO
        {
            public bool filled;           // Признак заповнення структури.
            public string issuer;         // Ім'я ЦСК, що видав сертифікат.
            public string issuerCN;       // Реквізити ЦСК, що видав сертифікат.
            public string serial;         // Реєстраційний номер сертифіката.
            public string subject;        // Ім'я власника сертифіката.
            public string subjCN;         // Реквізити власника сертифіката.
            public string subjOrg;        // Організація, до якої належить власник сертифіката.
            public string subjOrgUnit;    // Підрозділ організації, до якої належить власник сертифіката.
            public string subjTitle;      // Посада власника сертифіката.
            public string subjState;      // Назва області, до якої належить власник сертифіката.
            public string subjLocality;   // Назва населеного пункту, до якого належить власник сертифіката.
            public string subjFullName;   // Повне ім'я власника сертифіката.
            public string subjAddress;    // Адреса власника сертифіката.
            public string subjPhone;      // Номер телефону власника сертифіката.
            public string subjEMail;      // Адреса електронної пошти власника сертифіката.
            public string subjDNS;        // DNS-ім`я чи інше технічного засобу.
            public string subjEDRPOUCode; // Код ЄДРПОУ власника сертифіката.
            public string subjDRFOCode;   // Код ДРФО власника сертифіката.
            public bool timeAvail;        // Признак наявності часу підпису.
            public bool timeStamp;        // Признак наявності позначки часу отриманої з TSP сервера.
            public SYSTEMTIME time;       // Час підпису або позначка часу.

            public EU_SENDER_INFO(IntPtr pSenderInfo)
            {
                try
                {
                    long ptr = pSenderInfo.ToInt64();

                    // BOOL - 4 / 4 bytes.
                    this.filled = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // PSTR - 8 / 12 bytes.
                    this.issuer = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 20 bytes.
                    this.issuerCN = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 28 bytes.
                    this.serial = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 36 bytes.
                    this.subject = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 44 bytes.
                    this.subjCN = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 52 bytes.
                    this.subjOrg = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 60 bytes.
                    this.subjOrgUnit = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 68 bytes.
                    this.subjTitle = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 76 bytes.
                    this.subjState = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 84 bytes.
                    this.subjLocality = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 92 bytes.
                    this.subjFullName = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 100 bytes.
                    this.subjAddress = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 108 bytes.
                    this.subjPhone = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 116 bytes.
                    this.subjEMail = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 124 bytes.
                    this.subjDNS = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 132 bytes.
                    this.subjEDRPOUCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // PSTR - 8 / 140 bytes.
                    this.subjDRFOCode = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr((IntPtr)ptr));
                    ptr += Marshal.SizeOf(typeof(IntPtr));
                    // BOOL - 4 / 144 bytes.
                    this.timeAvail = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // BOOL - 4 / 148 bytes.
                    this.timeStamp = Marshal.ReadInt32((IntPtr)ptr) != 0;
                    ptr += 4;
                    // SYSTEMTIME - 16 / 164 bytes.
                    this.time = (SYSTEMTIME)Marshal.PtrToStructure((IntPtr)ptr, typeof(SYSTEMTIME));
                }
                catch (Exception)
                {
                    this.filled = false;
                    this.issuer = "";
                    this.issuerCN = "";
                    this.serial = "";
                    this.subject = "";
                    this.subjCN = "";
                    this.subjOrg = "";
                    this.subjOrgUnit = "";
                    this.subjTitle = "";
                    this.subjState = "";
                    this.subjLocality = "";
                    this.subjFullName = "";
                    this.subjAddress = "";
                    this.subjPhone = "";
                    this.subjEMail = "";
                    this.subjDNS = "";
                    this.subjEDRPOUCode = "";
                    this.subjDRFOCode = "";
                    this.timeAvail = false;
                    this.timeStamp = false;
                    this.time = new SYSTEMTIME();
                }
            }
        };

        // --------------------------------------------------------------------
        // Імпорт функцій з бібліотеки динамічного компонування (EUSignCP.dll).

        /// <summary>
        /// Встановлення режиму використання графічного інтерфейсу у разі виникнення помилок. 
        /// Якщо викливається до функції ініціалізації бібліотеки Initialize, бібліотеку буде завантажено без графічного модуля.
        /// </summary>
        /// <param name="bUIMode">Вхідний (BOOL). Режим використання графічного інтерфейсу у разі виникнення помилок</param>
        [DllImport("EUSignCP.dll")]
        private static extern void EUSetUIMode(int bUIMode);

        /// <summary>
        /// Ініціалізація бібліотеки. 
        /// Для завантаження бібліотеки без графічного модуля (або графічний модуль відсутній), 
        /// перед функцією ініціалізації потрібно викликати функцію EUSetUIMode з значенням FALSE.
        /// </summary>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUInitialize();

        /// <summary>
        /// Перевірка стану бібліотеки.
        /// </summary>
        /// <returns>(BOOL) Значення true, якщо бібліотека завантажена; в іншому випадку - значення false.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUIsInitialized();

        /// <summary>
        /// Завершення роботи з бібліотекою.
        /// </summary>
        [DllImport("EUSignCP.dll")]
        private static extern void EUFinalize();

        /// <summary>
        /// Отримання опису помилки за кодом.
        /// </summary>
        /// <param name="dwError">Вхідний (DWORD). Код помилки.</param>
        /// <returns>(PSTR) Опис помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern IntPtr EUGetErrorDesc(int dwError);

        /// <summary>
        /// Встановлення параметрів роботи з бібліотекою за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        [DllImport("EUSignCP.dll")]
        private static extern void EUSetSettings();

        /// <summary>
        /// Зчитування особистого ключа за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        /// <param name="pKeyMedia">Вхідний (PEU_KEY_MEDIA). Параметри носія особистого ключа.</param>
        /// <param name="pCertOwnerInfo">Вихідний (PEU_CERT_OWNER_INFO). Інформація про сертифікат власника.</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUReadPrivateKey(IntPtr pKeyMedia, IntPtr pCertOwnerInfo);

        /// <summary>
        /// Перевірка наявності зчитаного особистого ключа.
        /// </summary>
        /// <returns>(BOOL) Значення true, якщо ключ вже зчитано у пам'ять; в іншому випадку - значення false.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUIsPrivateKeyReaded();

        /// <summary>
        /// Затирання особистого ключа у пам'яті.
        /// </summary>
        [DllImport("EUSignCP.dll")]
        private static extern void EUResetPrivateKey();

        /// <summary>
        /// Відображення власного сертифіката за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        [DllImport("EUSignCP.dll")]
        private static extern void EUShowOwnCertificate();

        /// <summary>
        /// Вивільнення пам'яті з інформацією про сертифікат власника особистого ключа.
        /// </summary>
        /// <param name="pCertOwnerInfo">Вхідний (PEU_CERT_OWNER_INFO). Вказівник на дані щодо інформації про cертифікат власника.</param>
        [DllImport("EUSignCP.dll")]
        private static extern void EUFreeCertOwnerInfo(IntPtr pCertOwnerInfo);

        /// <summary>
        /// Перелічення наявних сертифікатів користувача.
        /// </summary>
        /// <param name="dwIndex">Вхідний (DWORD). Індекс сертифіката (нумераця з 0).</param>
        /// <param name="ppCertInfoEx">Вихідний (PPEU_CERT_INFO_EX). Інформація про сертифікат (розширена). 
        /// Якщо параметр дорівнює 0, сертифіката та послідуючих сертифікатів не існує.</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUEnumOwnCertificates(int dwIndex, IntPtr ppCertInfoEx);

        /// <summary>
        /// Отримання детальної інформації(розширеної) про сертифікат.
        /// </summary>
        /// <param name="pszIssuer">Вхідний (PSTR). Реквізити ЦСК.</param>
        /// <param name="pszSerial">Вхідний (PSTR). Серійний номер сертифіката.</param>
        /// <param name="ppCertInfoEx">Вихідний (PPEU_CERT_INFO_EX). Інформація про сертифікат (розширена).</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUGetCertificateInfoEx(IntPtr pszIssuer, IntPtr pszSerial, IntPtr ppCertInfoEx);

        /// <summary>
        /// Вивільнення пам'яті з детальною інформацією (розширеною) про сертифікат.
        /// </summary>
        /// <param name="pCertInfoEx">Вхідний (PEU_CERT_INFO_EX). Вказівник на дані щодо інформації про сертифікат (розширена).</param>
        [DllImport("EUSignCP.dll")]
        private static extern void EUFreeCertificateInfoEx(IntPtr pCertInfoEx);

        /// <summary>
        /// Отримання сертифіката користувача.
        /// </summary>
        /// <param name="pszIssuer">Вхідний (PSTR). Реквізити ЦСК.</param>
        /// <param name="pszSerial">Вхідний (PSTR). Серійний номер сертифіката.</param>
        /// <param name="ppszCertificate">Вихідний (PSTR). Сертифікат у вигляді BASE64-строки. 
        /// Якщо параметр дорівнює 0, сертифікат повертається у вигляді масиву байт.</param>
        /// <param name="ppbCertificate">Вихідний (PBYTE). Сертифікат у вигляді масиву байт.</param>
        /// <param name="pdwCertificateLength">Вихідний (PDWORD). Розмір сертифіката вигляді масиву байт</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUGetCertificate(IntPtr pszIssuer, IntPtr pszSerial,
            IntPtr ppszCertificate, IntPtr ppbCertificate, IntPtr pdwCertificateLength);

        /// <summary>
        /// Вивільнення пам’яті, що виділяється автоматично бібліотекою.
        /// </summary>
        /// <param name="pbMemory">Вхідний (PBYTE). Вказівник на дані для вивільнення.</param>
        [DllImport("EUSignCP.dll")]
        private static extern void EUFreeMemory(IntPtr pbMemory);

        /// <summary>
        /// Формування внутрішнього (підпис знаходиться разом з даними) ЕЦП.
        /// </summary>
        /// <param name="bAppendCert">Вхідний (BOOL). Включати сертифікат підписувача у підписані дані.</param>
        /// <param name="pbData">Вхідний (PBYTE). Дані для підпису.</param>
        /// <param name="dwDataLength">Вхідний(DWORD). Розмір даних для підпису.</param>
        /// <param name="ppszSignedData">Вихідний (PSTR). Підписані дані у вигляді BASE64-строки (пам'ять виділяється автоматично).
        /// Якщо параметр дорівнює 0, повертається у вигляді масиву байт.</param>
        /// <param name="ppbSignedData">Вихідний (PBYTE). Підписані дані у вигляді масиву байт (пам'ять виділяється автоматично).</param>
        /// <param name="pdwSignedDataLength">Вихідний (PDWORD). Розмір підписаних даних у вигляді масиву байт.</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUSignDataInternal(int bAppendCert, IntPtr pbData, int dwDataLength,
            IntPtr ppszSignedData, IntPtr ppbSignedData, IntPtr pdwSignedDataLength);

        /// <summary>
        /// Формування ЕЦП файлу (Примітка: Розмір файла не обмежений).
        /// </summary>
        /// <param name="pszFileName">Вхідний (PSTR). Ім'я файлу з даними.</param>
        /// <param name="pszFileNameWithSign">Вхідний (PSTR). Ім’я файлу, в який необхідно записати
        /// підпис (якщо тип підпису зовнішній) або підписані дані (якщо тип підпису внутрішній).</param>
        /// <param name="bExternalSign">Вхідний (BOOL). Тип ЕЦП (зовнішний або внутрішній).</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUSignFile(IntPtr pszFileName, IntPtr pszFileNameWithSign, int bExternalSign);

        /// <summary>
        /// Отримання інформації про обраний за допомогою графічного інтерфейсу сертифікат.
        /// </summary>
        /// <param name="pCertOwnerInfo">Вихідний (PEU_CERT_OWNER_INFO). Інформація про сертифікат.</param>
        /// <returns></returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUSelectCertificateInfo(IntPtr pCertOwnerInfo);

        /// <summary>
        /// Зашифрування даних.
        /// </summary>
        /// <param name="pszRecipientCertIssuer">Вхідний (PSTR). Реквізити ЦСК сертифіката одержувача.</param>
        /// <param name="pszRecipientCertSerial">Вхідний (PSTR). Серійний номер сертифіката одержувача.</param>
        /// <param name="bSignData">Вхідний (BOOL). Признак необхідності додатково підписувати дані.</param>
        /// <param name="pbData">Вхідний (PBYTE). Дані для зашифрування у вигляді масиву байт.</param>
        /// <param name="dwDataLength">Вхідний (DWORD). Розмір даних у вигляді масиву байт.</param>
        /// <param name="ppszEnvelopedData">Вихідний (PSTR). Зашифровані дані у вигляді BASE64-строки (пам'ять виділяється автоматично). 
        /// Якщо параметр дорівнює 0, зашифровані дані повертаються у вигляді масиву байт.</param>
        /// <param name="ppbEnvelopedData">Вихідний (PBYTE). Зашифровані дані у вигляді масиву байт (пам'ять виділяється автоматично).</param>
        /// <param name="pdwEnvelopedDataLength">Вихідний (PDWORD). Розмір зашифрованих даних у вигляді масиву байт.</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUEnvelopData(IntPtr pszRecipientCertIssuer, IntPtr pszRecipientCertSerial,
            int bSignData, IntPtr pbData, int dwDataLength, IntPtr ppszEnvelopedData,
            IntPtr ppbEnvelopedData, IntPtr pdwEnvelopedDataLength);

        /// <summary>
        /// Отримання інформації про сертифікати користувачів для направленого шифрування за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        /// <param name="pCertificates">Вихідний (PPEU_CERTIFICATES). Інформація про сертифікати користувачів для направленого шифрування.</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUGetReceiversCertificates(IntPtr ppCertificates);

        /// <summary>
        /// Вивільнення пам’яті з інформацією про сертифікати користувачів для направленого шифрування.
        /// </summary>
        /// <picates>Вхідний (PEU_CERTIFICATES). Вказівник на дані щодо сертифікатів користувачів для направленого шифрування.</param>
        [DllImport("EUSignCP.dll")]
        private static extern void EUFreeReceiversCertificates(IntPtr pCertificates);

        /// <summary>
        /// Зашифрування файла одночасно на декількох одержувачів. 
        /// Файл зашифровується з використанням ключа ГОСТ-28147, після чого ключ зашифровується направлено для кожного з абонентів
        /// (Примітка: Розмір файла не обмежений).
        /// </summary>
        /// <param name="pszRecipientCertIssuers">Вхідний (PSTR). Реквізити ЦСК сертифікатів одержувачів
        /// перелічені в рядку з символом-роздільником "\0". Рядок повинен закінчуватися "\0". 
        /// Якщо кілкість реквізитів ЦСК менша за кількість серійних номерів сертифікатів одержувачів 
        /// останній реквізит ЦСК в рядку буде використано для серійних номерів, що лишилися.</param>
        /// <param name="pszRecipientCertSerials">Вхідний (PSTR). Серійні номера сертифікатів одержувачів
        /// перелічені в рядку з символом-роздільником "\0". Рядок повинен закінчуватися "\0".</param>
        /// <param name="bSignData">Вхідний (BOOL). Признак необхідності додатково підписувати дані.</param>
        /// <param name="pszFileName">Вхідний (PSTR). Ім'я файлу з даними.</param>
        /// <param name="pszEnvelopedFileName">Вхідний. Ім'я файлу, в який необхідно записати зашифровані дані.</param>
        /// <returns></returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUEnvelopFileEx(IntPtr pszRecipientCertIssuers, IntPtr pszRecipientCertSerials,
            int bSignData, IntPtr pszFileName, IntPtr pszEnvelopedFileName);

        /// <summary>
        /// Розшифрування даних.
        /// </summary>
        /// <param name="pszEnvelopedData">Вхідний (PSTR). Зашифровані дані у вигляді BASE64-строки.
        /// Якщо параметр дорівнює 0, зашифровані дані передаються у вигляді масиву байт.</param>
        /// <param name="pbEnvelopedData">Вхідний (PBYTE). Зашифровані дані у вигляді масиву байт.</param>
        /// <param name="dwEnvelopedDataLength">Вхідний (DWORD). Розмір зашифрованих даних у вигляді масиву байт.</param>
        /// <param name="ppbData">Вихідний (PBYTE). Розшифровані дані для у вигляді масиву байт.</param>
        /// <param name="pdwDataLength">Вихідний (PDWORD). Розмір розшифрованих даних у вигляді масиву байт.</param>
        /// <param name="pEnvelopInfo">Вихідний (PEU_SENDER_INFO). Інформація про відправника зашифрованих даних.</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUDevelopData(IntPtr pszEnvelopedData,
            IntPtr pbEnvelopedData, int dwEnvelopedDataLength,
            IntPtr ppbData, IntPtr pdwDataLength, IntPtr pSenderInfo);

        /// <summary>
        /// Розшифрування файлу (Примітка: Розмір файла не обмежений).
        /// </summary>
        /// <param name="pszEnvelopedFileName">Вхідний (PSTR). Ім'я файлу з зашифрованими даними.</param>
        /// <param name="pszFileName">Вхідний (PSTR). Ім'я файлу в який необхідно записати розшифровані дані.</param>
        /// <param name="pEnvelopInfo">Вихідний (PEU_ENVELOP_INFO). Інформація про відправника зашифрованих даних.</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUDevelopFile(IntPtr pszEnvelopedFileName, IntPtr pszFileName, IntPtr pEnvelopInfo);

        /// <summary>
        /// Відображення інформації про відправника зашифрованих даних за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        /// <param name="pSenderInfo">Вхідний (PEU_SENDER_INFO). Інформація про відправника зашифрованих даних.</param>
        [DllImport("EUSignCP.dll")]
        private static extern void EUShowSenderInfo(IntPtr pSenderInfo);

        /// <summary>
        /// Вивільнення інформації про відправника зашифрованих даних.
        /// </summary>
        /// <param name="pSenderInfo">Вхідний (PEU_SENDER_INFO). Вказівник на дані щодо інформації про відправника зашифрованих даних.</param>
        [DllImport("EUSignCP.dll")]
        private static extern void EUFreeSenderInfo(IntPtr pSenderInfo);

        /// <summary>
        /// Перевірка внутрішнього ЕЦП.
        /// </summary>
        /// <param name="pszSignedData">Вхідний (PSTR). Підписані дані для перевірки у вигляді BASE64-строки.
        /// Якщо  параметр дорівнює 0, перевіряються підписані дані у вигляді масиву байт.</param>
        /// <param name="pbSignedData">Вхідний (PBYTE). Підписані дані у вигляді масиву байт.</param>
        /// <param name="dwSignedDataLength">Вхідний (DWORD). Розмір підписаних даних у вигляді масиву байт.</param>
        /// <param name="ppbData">Вихідний (PBYTE). Отримані після перевірки ЕЦП дані.</param>
        /// <param name="pdwDataLength">Вихідний (PDWORD). Розмір отриманих після перевірки ЕЦП даних.</param>
        /// <param name="pSignInfo">Вихідний (PEU_SIGN_INFO). Інформація про підпис.</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUVerifyDataInternal(IntPtr pszSignedData,
            IntPtr pbSignedData, int dwSignedDataLength, IntPtr ppbData,
            IntPtr pdwDataLength, IntPtr pSignInfo);

        /// <summary>
        /// Перевірка ЕЦП файлу (Примітка: Розмір файла не обмежений).
        /// </summary>
        /// <param name="pszFileNameWithSign">Вхідний (PSTR). Ім'я файлу 
        /// з підписом (якщо тип підпису зовнішній) або з підписаними даними (якщо тип підпису внутрішній).</param>
        /// <param name="pszFileName">Вхідний (PSTR). Ім'я файлу 
        /// з даними (якщо тип підпису зовнішній) або ім'я файлу, в який необхідно записати дані (якщо тип підпису внутрішній).</param>
        /// <param name="pSignInfo">Вихідний (PEU_SIGN_INFO). Інформація про підпис.</param>
        /// <returns>(DWORD) Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        [DllImport("EUSignCP.dll")]
        private static extern int EUVerifyFile(IntPtr pszFileNameWithSign, IntPtr pszFileName, IntPtr pSignInfo);

        /// <summary>
        /// Відображення інформації про підпис.
        /// </summary>
        /// <param name="pSignInfo">Вхідний. Інформація про підпис.</param>
        [DllImport("EUSignCP.dll")]
        private static extern void EUShowSignInfo(IntPtr pSignInfo);

        /// <summary>
        /// Вивільнення інформації про підпис.
        /// </summary>
        /// <param name="pSignInfo">Вхідний (PEU_SIGN_INFO). Вказівник на дані щодо інформації про підпис.</param>
        [DllImport("EUSignCP.dll")]
        private static extern void EUFreeSignInfo(IntPtr pSignInfo);


        // --------------------------------------------------------------------
        // Методи классу - обгортки імпортованих функцій бібліотеки

        /// <summary>
        /// Встановлення режиму використання графічного інтерфейсу у разі виникнення помилок. 
        /// </summary>
        /// <param name="uiMode">Значення true - з графічним інтерфейсом; значення false - без графічного інтерфейсу.</param>
        public static void SetUIMode(bool uiMode)
        {
            try
            {
                IEUSignCP.EUSetUIMode(uiMode ? 1 : 0);
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Ініціалізація бібліотеки.
        /// </summary>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int Initialize()
        {
            try
            {
                return IEUSignCP.EUInitialize();
            }
            catch (Exception)
            {
                return IEUSignCP.EU_ERROR_UNKNOWN;
            }
        }

        /// <summary>
        /// Перевірка стану бібліотеки.
        /// </summary>
        /// <returns>Значення true, якщо бібліотека завантажена; в іншому випадку - значення false.</returns>
        public static bool IsInitialized()
        {
            try
            {
                return IEUSignCP.EUIsInitialized() != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Завершення роботи з бібліотекою.
        /// </summary>
        public static void Finalize()
        {
            try
            {
                IEUSignCP.EUFinalize();
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Отримання опису помилки за кодом.
        /// </summary>
        /// <param name="error">Код помилки.</param>
        /// <returns>Опис помилки.</returns>
        public static string GetErrorDesc(int error)
        {
            IntPtr pError = IntPtr.Zero;
            string errorDesc = "";

            try
            {
                pError = IEUSignCP.EUGetErrorDesc(error);

                if (pError != IntPtr.Zero)
                {
                    errorDesc = Marshal.PtrToStringAnsi(pError);
                }
            }
            catch (Exception e)
            {
                errorDesc = e.ToString();
            }
            return errorDesc;
        }

        /// <summary>
        /// Встановлення параметрів роботи з бібліотекою за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        public static void SetSettings()
        {
            try
            {
                IEUSignCP.EUSetSettings();
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Зчитування особистого ключа за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        /// <param name="certOwnerInfo">Вихідний. Інформація про сертифікат власника.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int ReadPrivateKey(out EU_CERT_OWNER_INFO certOwnerInfo)
        {
            certOwnerInfo = new EU_CERT_OWNER_INFO();
            int error = 0;

            IntPtr pCertOwnerInfo = IntPtr.Zero;

            try
            {
                pCertOwnerInfo = Marshal.AllocHGlobal(EU_CERT_OWNER_INFO_SIZE);
                if (pCertOwnerInfo == IntPtr.Zero) throw new ErrorMemoryAllocation();

                Marshal.WriteInt32(pCertOwnerInfo, 0);

                error = IEUSignCP.EUReadPrivateKey(IntPtr.Zero, pCertOwnerInfo);

                if (error == IEUSignCP.EU_ERROR_NONE)
                {
                    certOwnerInfo = new EU_CERT_OWNER_INFO(pCertOwnerInfo);
                }
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pCertOwnerInfo != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeCertOwnerInfo(pCertOwnerInfo);               
                        pCertOwnerInfo = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Перевірка наявності зчитаного особистого ключа.
        /// </summary>
        /// <returns>Значення true, якщо ключ вже зчитано у пам'ять; в іншому випадку - значення false.</returns>
        public static bool IsPrivateKeyReaded()
        {
            try
            {
                return IEUSignCP.EUIsPrivateKeyReaded() != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Затирання особистого ключа у пам’яті.
        /// </summary>
        public static void ResetPrivateKey()
        {
            try
            {
                IEUSignCP.EUResetPrivateKey();
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Відображення власного сертифіката за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        public static void ShowOwnCertificate()
        {
            try
            {
                IEUSignCP.EUShowOwnCertificate();
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Перелічення наявних сертифікатів користувача.
        /// </summary>
        /// <param name="index">Індекс сертифіката (нумерація з 0)</param>
        /// <param name="ownCertificate">Інформація про сертифікат. 
        /// Якщо структура не заповнена (ownCertificate.filled = false), то сертифіката і послідуючих сертифікатів не існує.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int EnumOwnCertificates(int index, out EU_CERT_INFO_EX certInfoEx)
        {
            certInfoEx = new EU_CERT_INFO_EX();
            int error = 0;

            IntPtr ppCertInfoEx = IntPtr.Zero;
            IntPtr pCertInfoEx = IntPtr.Zero;

            try
            {
                ppCertInfoEx = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                if (ppCertInfoEx == IntPtr.Zero) throw new ErrorMemoryAllocation();

                error = IEUSignCP.EUEnumOwnCertificates(index, ppCertInfoEx);

                if (error == IEUSignCP.EU_ERROR_NONE)
                {
                    pCertInfoEx = Marshal.ReadIntPtr(ppCertInfoEx);
                    certInfoEx = new EU_CERT_INFO_EX(pCertInfoEx);
                }
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pCertInfoEx != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeCertificateInfoEx(pCertInfoEx);
                        pCertInfoEx = IntPtr.Zero;
                    }
                    if (ppCertInfoEx != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ppCertInfoEx);
                        ppCertInfoEx = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Отримання детальної інформації(розширеної) про сертифікат.
        /// </summary>
        /// <param name="issuer">Реквізити ЦСК.</param>
        /// <param name="serial">Серійний номер сертифіката.</param>
        /// <param name="certInfoEx">Інформація про сертифікат (розширена).</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int GetCertificateInfoEx(string issuer, string serial, out EU_CERT_INFO_EX certInfoEx)
        {
            certInfoEx = new EU_CERT_INFO_EX();
            int error = 0;

            IntPtr pIssuer = IntPtr.Zero;
            IntPtr pSerial = IntPtr.Zero;
            IntPtr ppCertInfoEx = IntPtr.Zero;
            IntPtr pCertInfoEx = IntPtr.Zero;

            try
            {
                pIssuer = Marshal.StringToHGlobalAnsi(issuer);
                if (pIssuer == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pSerial = Marshal.StringToHGlobalAnsi(serial);
                if (pSerial == IntPtr.Zero) throw new ErrorMemoryAllocation();
                ppCertInfoEx = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                if (ppCertInfoEx == IntPtr.Zero) throw new ErrorMemoryAllocation();

                error = IEUSignCP.EUGetCertificateInfoEx(pIssuer, pSerial, ppCertInfoEx);

                if (error == IEUSignCP.EU_ERROR_NONE)
                {
                    pCertInfoEx = Marshal.ReadIntPtr(ppCertInfoEx);
                    certInfoEx = new EU_CERT_INFO_EX(pCertInfoEx);
                }
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pCertInfoEx != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeCertificateInfoEx(pCertInfoEx);
                        pCertInfoEx = IntPtr.Zero;
                    }
                    if (ppCertInfoEx != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ppCertInfoEx);
                        ppCertInfoEx = IntPtr.Zero;
                    }
                    if (pSerial != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pSerial);
                        pSerial = IntPtr.Zero;
                    }
                    if (pIssuer != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pIssuer);
                        pIssuer = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Отримання сертифіката користувача.
        /// </summary>
        /// <param name="issuer">Реквізити ЦСК.</param>
        /// <param name="serial">Серійний номер сертифіката.</param>
        /// <param name="certificate">Сертифікат у вигляді масиву байт.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int GetCertificate(string issuer, string serial, out byte[] certificate)
        {
            certificate = new byte[0];
            int error = 0;

            IntPtr pIssuer = IntPtr.Zero;
            IntPtr pSerial = IntPtr.Zero;
            IntPtr ppCertificateBinary = IntPtr.Zero;
            IntPtr ppCertificateLength = IntPtr.Zero;
            IntPtr pCertificateBinary = IntPtr.Zero;
            int certificateLength;

            try
            {
                pIssuer = Marshal.StringToHGlobalAnsi(issuer);
                if (pIssuer == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pSerial = Marshal.StringToHGlobalAnsi(serial);
                if (pSerial == IntPtr.Zero) throw new ErrorMemoryAllocation();
                ppCertificateBinary = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                if (ppCertificateBinary == IntPtr.Zero) throw new ErrorMemoryAllocation();
                ppCertificateLength = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
                if (ppCertificateLength == IntPtr.Zero) throw new ErrorMemoryAllocation();

                error = IEUSignCP.EUGetCertificate(pIssuer, pSerial,
                    IntPtr.Zero, ppCertificateBinary, ppCertificateLength);

                if (error == IEUSignCP.EU_ERROR_NONE)
                {
                    pCertificateBinary = Marshal.ReadIntPtr(ppCertificateBinary);
                    certificateLength = Marshal.ReadInt32(ppCertificateLength);
                    certificate = new byte[certificateLength];
                    Marshal.Copy(pCertificateBinary, certificate, 0, certificateLength);
                }
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pCertificateBinary != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeMemory(pCertificateBinary);
                        pCertificateBinary = IntPtr.Zero;
                    }
                    if (ppCertificateLength != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ppCertificateLength);
                        ppCertificateLength = IntPtr.Zero;
                    }
                    if (ppCertificateBinary != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ppCertificateBinary);
                        ppCertificateBinary = IntPtr.Zero;
                    }
                    if (pSerial != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pSerial);
                        pSerial = IntPtr.Zero;
                    }
                    if (pIssuer != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pIssuer);
                        pIssuer = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Формування внутрішнього (підпис знаходиться разом з даними) ЕЦП.
        /// </summary>
        /// <param name="appendCert">Включати сертифікат підписувача у підписані дані.
        /// Значення true - включати; значення false - не включати.</param>
        /// <param name="data">Дані для підпису.</param>
        /// <param name="signedData">Підписані дані.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int SignDataInternal(bool appendCert, byte[] data, out byte[] signedData)
        {
            signedData = new byte[0];
            int error = 0;

            IntPtr pData = IntPtr.Zero;
            IntPtr ppSignedDataBinary = IntPtr.Zero;
            IntPtr pSignedDataBinaryLength = IntPtr.Zero;
            IntPtr pSignedDataBinary = IntPtr.Zero;
            int signedDataBinaryLength;

            try
            {
                pData = Marshal.AllocHGlobal(data.Length);
                if (pData == IntPtr.Zero) throw new ErrorMemoryAllocation();
                Marshal.Copy(data, 0, pData, data.Length);
                ppSignedDataBinary = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                if (ppSignedDataBinary == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pSignedDataBinaryLength = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
                if (pSignedDataBinaryLength == IntPtr.Zero) throw new ErrorMemoryAllocation();

                error = EUSignDataInternal(appendCert ? 1 : 0, pData, data.Length,
                    IntPtr.Zero, ppSignedDataBinary, pSignedDataBinaryLength);

                if (error == IEUSignCP.EU_ERROR_NONE)
                {
                    pSignedDataBinary = Marshal.ReadIntPtr(ppSignedDataBinary);
                    signedDataBinaryLength = Marshal.ReadInt32(pSignedDataBinaryLength);
                    signedData = new byte[signedDataBinaryLength];
                    Marshal.Copy(pSignedDataBinary, signedData, 0, signedDataBinaryLength);
                }
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pSignedDataBinary != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeMemory(pSignedDataBinary);
                        pSignedDataBinary = IntPtr.Zero;
                    }
                    if (pSignedDataBinaryLength != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pSignedDataBinaryLength);
                        pSignedDataBinaryLength = IntPtr.Zero;
                    }
                    if (ppSignedDataBinary != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ppSignedDataBinary);
                        ppSignedDataBinary = IntPtr.Zero;
                    }
                    if (pData != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pData);
                        pData = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Формування ЕЦП файлу (Примітка: Розмір файла не обмежений).
        /// </summary>
        /// <param name="fileName">Ім'я файлу з даними.</param>
        /// <param name="fileNameWithSign">Ім'я файлу, в який необхідно записати підписані дані.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int SignFile(string fileName, string fileNameWithSign)
        {
            IntPtr pFileName = IntPtr.Zero;
            IntPtr pFileNameWithSign = IntPtr.Zero;

            int error;

            try
            {
                pFileName = Marshal.StringToHGlobalAnsi(fileName);
                if (pFileName == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pFileNameWithSign = Marshal.StringToHGlobalAnsi(fileNameWithSign);
                if (pFileNameWithSign == IntPtr.Zero) throw new ErrorMemoryAllocation();

                error = IEUSignCP.EUSignFile(pFileName, pFileNameWithSign, 0);
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                if (pFileName != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pFileName);
                    pFileName = IntPtr.Zero;
                }
                if (pFileNameWithSign != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pFileNameWithSign);
                    pFileNameWithSign = IntPtr.Zero;
                }
            }
            return error;
        }

        /// <summary>
        /// Отримання інформації про обраний за допомогою графічного інтерфейсу сертифікат.
        /// </summary>
        /// <param name="certOwnerInfo">Інформація про сертифікат.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int SelectCertInfo(out EU_CERT_OWNER_INFO certOwnerInfo)
        {
            certOwnerInfo = new EU_CERT_OWNER_INFO();
            int error = 0;

            IntPtr pCertOwnerInfo = IntPtr.Zero;

            try
            {
                pCertOwnerInfo = Marshal.AllocHGlobal(EU_CERT_OWNER_INFO_SIZE);
                if (pCertOwnerInfo == IntPtr.Zero) throw new ErrorMemoryAllocation();

                Marshal.WriteInt32(pCertOwnerInfo, 0);

                error = IEUSignCP.EUSelectCertificateInfo(pCertOwnerInfo);

                if (error == IEUSignCP.EU_ERROR_NONE)
                {
                    certOwnerInfo = new EU_CERT_OWNER_INFO(pCertOwnerInfo);
                }
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pCertOwnerInfo != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeCertOwnerInfo(pCertOwnerInfo);
                        pCertOwnerInfo = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Зашифрування даних.
        /// </summary>
        /// <param name="issuer">Реквізити ЦСК сертифіката одержувача.</param>
        /// <param name="serial">Серійний номер сертифіката одержувача.</param>
        /// <param name="data">Дані для зашифрування у вигляді масиву байт.</param>
        /// <param name="envelopeddData">Зашифровані дані у вигляді масиву байт.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int EnvelopData(string recipientCertIssuer, string recipientCertSerial, byte[] data, out byte[] envelopedData)
        {
            envelopedData = new byte[0];
            int error = 0;

            IntPtr pRecipientCertIssuer = IntPtr.Zero;
            IntPtr pRecipientCertSerial = IntPtr.Zero;
            IntPtr pData = IntPtr.Zero;
            IntPtr ppEnvelopedDataBinary = IntPtr.Zero;
            IntPtr pEnvelopedDataBinaryLength = IntPtr.Zero;
            IntPtr pEnvelopedDataBinary = IntPtr.Zero;
            int envelopedDataBinaryLength;

            try
            {
                pRecipientCertIssuer = Marshal.StringToHGlobalAnsi(recipientCertIssuer);
                if (pRecipientCertIssuer == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pRecipientCertSerial = Marshal.StringToHGlobalAnsi(recipientCertSerial);
                if (pRecipientCertSerial == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pData = Marshal.AllocHGlobal(data.Length);
                if (pData == IntPtr.Zero) throw new ErrorMemoryAllocation();
                Marshal.Copy(data, 0, pData, data.Length);
                ppEnvelopedDataBinary = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                if (ppEnvelopedDataBinary == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pEnvelopedDataBinaryLength = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
                if (pEnvelopedDataBinaryLength == IntPtr.Zero) throw new ErrorMemoryAllocation();

                error = IEUSignCP.EUEnvelopData(pRecipientCertIssuer, pRecipientCertSerial,
                    0, pData, data.Length, IntPtr.Zero, ppEnvelopedDataBinary, pEnvelopedDataBinaryLength);

                if (error == IEUSignCP.EU_ERROR_NONE)
                {
                    pEnvelopedDataBinary = Marshal.ReadIntPtr(ppEnvelopedDataBinary);
                    envelopedDataBinaryLength = Marshal.ReadInt32(pEnvelopedDataBinaryLength);
                    envelopedData = new byte[envelopedDataBinaryLength];
                    Marshal.Copy(pEnvelopedDataBinary, envelopedData, 0, envelopedDataBinaryLength);
                }
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pEnvelopedDataBinary != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeMemory(pEnvelopedDataBinary);
                        pEnvelopedDataBinary = IntPtr.Zero;
                    }
                    if (pEnvelopedDataBinaryLength != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pEnvelopedDataBinaryLength);
                        pEnvelopedDataBinaryLength = IntPtr.Zero;
                    }
                    if (ppEnvelopedDataBinary != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ppEnvelopedDataBinary);
                        ppEnvelopedDataBinary = IntPtr.Zero;
                    }
                    if (pData != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pData);
                        pData = IntPtr.Zero;
                    }
                    if (pRecipientCertSerial != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pRecipientCertSerial);
                        pRecipientCertSerial = IntPtr.Zero;
                    }
                    if (pRecipientCertIssuer != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pRecipientCertIssuer);
                        pRecipientCertIssuer = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Отримання інформації про сертифікати користувачів для направленого шифрування за допомогою графічного інтерфейсу бібліотеки.
        /// </summary>
        /// <param name="certificates">Інформація про сертифікати користувачів для направленого шифрування.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int GetReceiversCertificates(out EU_CERTIFICATES certificates)
        {
            certificates = new EU_CERTIFICATES();
            int error = 0;

            IntPtr ppCertificates = IntPtr.Zero;
            IntPtr pCertificates = IntPtr.Zero;

            try
            {
                ppCertificates = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                if (ppCertificates == IntPtr.Zero) throw new ErrorMemoryAllocation();

                error = IEUSignCP.EUGetReceiversCertificates(ppCertificates);

                if (error == IEUSignCP.EU_ERROR_NONE)
                {
                    pCertificates = Marshal.ReadIntPtr(ppCertificates);
                    certificates = new EU_CERTIFICATES(pCertificates);
                }
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pCertificates != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeReceiversCertificates(pCertificates);
                        pCertificates = IntPtr.Zero;
                    }
                    if (ppCertificates != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ppCertificates);
                        ppCertificates = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Зашифрування файла одночасно на декількох одержувачів (Примітка: Розмір файла не обмежений). 
        /// Файл зашифровується з використанням ключа ГОСТ-28147, після чого ключ зашифровується направлено для кожного з абонентів.
        /// </summary>
        /// <param name="receiversCertificates">Інформація про сертифікати одержувачів.</param>
        /// <param name="fileName">Ім'я файлу з даними.</param>
        /// <param name="envelopedFileName">Ім'я файлу, в який необхідно записати зашифровані дані.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int EnvelopFileEx(EU_CERTIFICATES recipientCertificates, string fileName, string envelopedFileName)
        {
            string recipientCertIssuers = "";
            string recipientCertSerials = "";
            for (int i = 0; i < recipientCertificates.items.Length; i++)
            {
                recipientCertIssuers += recipientCertificates.items[i].issuer + '\0';
                recipientCertSerials += recipientCertificates.items[i].serial + '\0';
            }
            int error = 0;

            IntPtr pRecipientCertIssuers = IntPtr.Zero;
            IntPtr pRecipientCertSerials = IntPtr.Zero;
            IntPtr pFileName = IntPtr.Zero;
            IntPtr pEnvelopedFileName = IntPtr.Zero;

            try
            {
                pRecipientCertIssuers = Marshal.StringToHGlobalAnsi(recipientCertIssuers);
                if (pRecipientCertIssuers == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pRecipientCertSerials = Marshal.StringToHGlobalAnsi(recipientCertSerials);
                if (pRecipientCertSerials == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pFileName = Marshal.StringToHGlobalAnsi(fileName);
                if (pFileName == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pEnvelopedFileName = Marshal.StringToHGlobalAnsi(envelopedFileName);
                if (pEnvelopedFileName == IntPtr.Zero) throw new ErrorMemoryAllocation();

                error = IEUSignCP.EUEnvelopFileEx(pRecipientCertIssuers, pRecipientCertSerials,
                    0, pFileName, pEnvelopedFileName);
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pEnvelopedFileName != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pEnvelopedFileName);
                        pEnvelopedFileName = IntPtr.Zero;
                    }
                    if (pFileName != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pFileName);
                        pFileName = IntPtr.Zero;
                    }
                    if (pRecipientCertSerials != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pRecipientCertSerials);
                        pRecipientCertSerials = IntPtr.Zero;
                    }
                    if (pRecipientCertIssuers != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pRecipientCertIssuers);
                        pRecipientCertIssuers = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Розшифрування даних.
        /// </summary>
        /// <param name="envelopedData">Зашифровані дані у вигляді масиву байт.</param>
        /// <param name="data">Розшифровані дані для у вигляді масиву байт.</param>
        /// <param name="senderInfo">Інформація про відправника зашифрованих даних.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int DevelopData(byte[] envelopedData, out byte[] data, out EU_SENDER_INFO senderInfo)
        {
            data = new byte[0];
            senderInfo = new EU_SENDER_INFO();
            int error = 0;

            IntPtr pEnvelopedDataBinary = IntPtr.Zero;
            IntPtr ppData = IntPtr.Zero;
            IntPtr ppDataLength = IntPtr.Zero;
            IntPtr pSenderInfo = IntPtr.Zero;
            IntPtr pData = IntPtr.Zero;
            int dataLength;

            try
            {
                pEnvelopedDataBinary = Marshal.AllocHGlobal(envelopedData.Length);
                if (pEnvelopedDataBinary == IntPtr.Zero) throw new ErrorMemoryAllocation();
                Marshal.Copy(envelopedData, 0, pEnvelopedDataBinary, envelopedData.Length);
                pSenderInfo = Marshal.AllocHGlobal(IEUSignCP.EU_SENDER_INFO_SIZE);
                if (pSenderInfo == IntPtr.Zero) throw new ErrorMemoryAllocation();
                Marshal.WriteInt32(pSenderInfo, 0);
                ppData = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                if (ppData == IntPtr.Zero) throw new ErrorMemoryAllocation();
                ppDataLength = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                if (ppDataLength == IntPtr.Zero) throw new ErrorMemoryAllocation();

                error = IEUSignCP.EUDevelopData(IntPtr.Zero, pEnvelopedDataBinary, envelopedData.Length,
                    ppData, ppDataLength, pSenderInfo);

                if (error == IEUSignCP.EU_ERROR_NONE)
                {
                    pData = Marshal.ReadIntPtr(ppData);
                    dataLength = Marshal.ReadInt32(ppDataLength);
                    data = new byte[dataLength];
                    Marshal.Copy(pData, data, 0, dataLength);
                    senderInfo = new EU_SENDER_INFO(pSenderInfo);
                }
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pData != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeMemory(pData);
                        Marshal.FreeHGlobal(pData);
                        pData = IntPtr.Zero;
                    }
                    if (pSenderInfo != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeSenderInfo(pSenderInfo);
                        Marshal.FreeHGlobal(pSenderInfo);
                        pSenderInfo = IntPtr.Zero;
                    }
                    if (ppDataLength != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ppDataLength);
                        ppDataLength = IntPtr.Zero;
                    }
                    if (ppData != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ppData);
                        ppData = IntPtr.Zero;
                    }
                    if (pEnvelopedDataBinary != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pEnvelopedDataBinary);
                        pEnvelopedDataBinary = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Розшифрування файлу (Примітка: Розмір файла не обмежений).
        /// </summary>
        /// <param name="envelopedFileName">Ім'я файлу з зашифрованими даними.</param>
        /// <param name="fileName">Ім'я файлу в який необхідно записати розшифровані дані.</param>
        /// <param name="senderInfo">Інформація про відправника зашифрованих даних.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int DevelopFile(string envelopedFileName, string fileName, out EU_SENDER_INFO senderInfo)
        {
            senderInfo = new EU_SENDER_INFO();
            int error = 0;

            IntPtr pEnvelopedFileName = IntPtr.Zero;
            IntPtr pFileName = IntPtr.Zero;
            IntPtr pSenderInfo = IntPtr.Zero;

            try
            {
                pEnvelopedFileName = Marshal.StringToHGlobalAnsi(envelopedFileName);
                if (pEnvelopedFileName == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pFileName = Marshal.StringToHGlobalAnsi(fileName);
                if (pFileName == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pSenderInfo = Marshal.AllocHGlobal(IEUSignCP.EU_SENDER_INFO_SIZE);
                if (pSenderInfo == IntPtr.Zero) throw new ErrorMemoryAllocation();
                Marshal.WriteInt32(pSenderInfo, 0);

                error = IEUSignCP.EUDevelopFile(pEnvelopedFileName, pFileName, pSenderInfo);
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pSenderInfo != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeSenderInfo(pSenderInfo);
                        Marshal.FreeHGlobal(pSenderInfo);
                        pSenderInfo = IntPtr.Zero;
                    }
                    if (pFileName != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pFileName);
                        pFileName = IntPtr.Zero;
                    }
                    if (pEnvelopedFileName != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pEnvelopedFileName);
                        pEnvelopedFileName = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Перевірка внутрішнього ЕЦП.
        /// </summary>
        /// <param name="signedData">Підписані дані у вигляді масиву байт.</param>
        /// <param name="data">Отримані після перевірки ЕЦП дані.</param>
        /// <param name="signInfo">Інформація про підпис.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int VerifyDataInternal(byte[] signedData, out byte[] data, out EU_SIGN_INFO signInfo)
        {
            data = new byte[0];
            signInfo = new EU_SIGN_INFO();
            int error = 0;

            IntPtr pSignedDataBinary = IntPtr.Zero;
            IntPtr ppData = IntPtr.Zero;
            IntPtr ppDataLength = IntPtr.Zero;
            IntPtr pSignInfo = IntPtr.Zero;
            IntPtr pData = IntPtr.Zero;
            int dataLength;

            try
            {
                pSignedDataBinary = Marshal.AllocHGlobal(signedData.Length);
                if (pSignedDataBinary == IntPtr.Zero) throw new ErrorMemoryAllocation();
                Marshal.Copy(signedData, 0, pSignedDataBinary, signedData.Length);
                pSignInfo = Marshal.AllocHGlobal(IEUSignCP.EU_SIGN_INFO_SIZE);
                if (pSignInfo == IntPtr.Zero) throw new ErrorMemoryAllocation();
                Marshal.WriteInt32(pSignInfo, 0);
                ppData = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                if (ppData == IntPtr.Zero) throw new ErrorMemoryAllocation();
                ppDataLength = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                if (ppDataLength == IntPtr.Zero) throw new ErrorMemoryAllocation();

                error = IEUSignCP.EUVerifyDataInternal(IntPtr.Zero, pSignedDataBinary, signedData.Length,
                    ppData, ppDataLength, pSignInfo);

                if (error == IEUSignCP.EU_ERROR_NONE)
                {
                    pData = Marshal.ReadIntPtr(ppData);
                    dataLength = Marshal.ReadInt32(ppDataLength);
                    data = new byte[dataLength];
                    Marshal.Copy(pData, data, 0, dataLength);
                    signInfo = new EU_SIGN_INFO(pSignInfo);
                }
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pData != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeMemory(pData);
                        Marshal.FreeHGlobal(pData);
                        pData = IntPtr.Zero;
                    }
                    if (pSignInfo != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeSignInfo(pSignInfo);
                        Marshal.FreeHGlobal(pSignInfo);
                        pSignInfo = IntPtr.Zero;
                    }
                    if (ppDataLength != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ppDataLength);
                        ppDataLength = IntPtr.Zero;
                    }
                    if (ppData != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ppData);
                        ppData = IntPtr.Zero;
                    }
                    if (pSignedDataBinary != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pSignedDataBinary);
                        pSignedDataBinary = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }

        /// <summary>
        /// Перевірка ЕЦП файлу (Примітка: Розмір файла не обмежений).
        /// </summary>
        /// <param name="fileNameWithSign">Ім'я файлу з підписаними даними.</param>
        /// <param name="fileName">Ім'я файлу, в який необхідно записати дані.</param>
        /// <param name="signInfo">Інформація про підпис.</param>
        /// <returns>Значення 0, якщо виконано без помилок; в іншому випадку - значення коду помилки.</returns>
        public static int VerifyFile(string fileNameWithSign, string fileName, out EU_SIGN_INFO signInfo)
        {
            signInfo = new EU_SIGN_INFO();
            int error = 0;

            IntPtr pFileNameWithSign = IntPtr.Zero;
            IntPtr pFileName = IntPtr.Zero;
            IntPtr pSignInfo = IntPtr.Zero;

            try
            {
                pFileNameWithSign = Marshal.StringToHGlobalAnsi(fileNameWithSign);
                if (pFileNameWithSign == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pFileName = Marshal.StringToHGlobalAnsi(fileName);
                if (pFileName == IntPtr.Zero) throw new ErrorMemoryAllocation();
                pSignInfo = Marshal.AllocHGlobal(IEUSignCP.EU_SIGN_INFO_SIZE);
                if (pSignInfo == IntPtr.Zero) throw new ErrorMemoryAllocation();
                Marshal.WriteInt32(pSignInfo, 0);

                error = IEUSignCP.EUVerifyFile(pFileNameWithSign, pFileName, pSignInfo);
            }
            catch (ErrorMemoryAllocation)
            {
                error = IEUSignCP.EU_ERROR_MEMORY_ALLOCATION;
            }
            catch (Exception)
            {
                error = IEUSignCP.EU_ERROR_UNKNOWN;
            }
            finally
            {
                try
                {
                    if (pSignInfo != IntPtr.Zero)
                    {
                        IEUSignCP.EUFreeSenderInfo(pSignInfo);
                        Marshal.FreeHGlobal(pSignInfo);
                        pSignInfo = IntPtr.Zero;
                    }
                    if (pFileName != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pFileName);
                        pFileName = IntPtr.Zero;
                    }
                    if (pFileNameWithSign != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pFileNameWithSign);
                        pFileNameWithSign = IntPtr.Zero;
                    }
                }
                catch (Exception)
                {
                    error = IEUSignCP.EU_ERROR_UNKNOWN;
                }
            }
            return error;
        }
    }
}