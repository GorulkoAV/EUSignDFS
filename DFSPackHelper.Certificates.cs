using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSignCP
{
    public static partial class DFSPackHelper
    {
        public static class Certificates
        {
            public static CertInfo Own = new CertInfo();
            public static CertInfo Recipient = new CertInfo();

            public class CertInfo
            {
                private IEUSignCP.EU_CERT_INFO_EX _certInfoEx;
                public IEUSignCP.EU_CERT_INFO_EX CertInfoEx
                {
                    get
                    {
                        return _certInfoEx;
                    }
                    set
                    {
                        _certInfoEx = value;
                    }
                }
                public CertInfo()
                {
                    this._certInfoEx = new IEUSignCP.EU_CERT_INFO_EX(IntPtr.Zero);
                }
                public bool IsLoaded()
                {
                    return this._certInfoEx.filled;
                }
                public string ToText()
                {
                    if (!this.IsLoaded()) return "Сертифікат не зчитано ...";
                    
                    return this._certInfoEx.subjCN.ToString() + System.Environment.NewLine +
						this._certInfoEx.subjTitle + System.Environment.NewLine +
                        this._certInfoEx.subjOrgUnit + System.Environment.NewLine +
                        this._certInfoEx.subjOrg;
                }
                public void Clear()
                {
                    this._certInfoEx = new IEUSignCP.EU_CERT_INFO_EX(IntPtr.Zero);
                }
            }
        }
    }
}
