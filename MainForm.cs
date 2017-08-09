using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EUSignCP;

namespace EUSignDFS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.ActiveControl = this.btnReadPrivateKey;

            try
            {
                DFSPackHelper.Initialize();
                this.tsslInitialize.Image = global::EUSignDFS.Properties.Resources.checked_16x16;
                this.tsslInitialize.ToolTipText = "Бібліотеку завантажено.";

                this.tbCertOwnerInfo.Text = DFSPackHelper.Certificates.Own.ToText();
                this.tbRecipient.Text = DFSPackHelper.Certificates.Recipient.ToText();
            }
            catch (Exception ex)
            {
                this.ErrorMessageBox(ex.Message);
            }         
        }

        private void ErrorMessageBox(string message)
        {
            MessageBox.Show(this, message, "Повідомлення оператору", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void InfoMessageBox(string message)
        {
            MessageBox.Show(this, message, "Повідомлення оператору", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void tsbSettings_Click(object sender, EventArgs e)
        {
            try 
            {
                DFSPackHelper.SetSettings();
            }
            catch (Exception ex)
            {
                this.ErrorMessageBox(ex.Message);
            }             
        }

        private void btnReadPrivateKey_Click(object sender, EventArgs e)
        {
            try
            {
                DFSPackHelper.ReadPrivateKey();
            }
            catch (Exception ex)
            {
                this.ErrorMessageBox(ex.Message);
            }

            this.tbCertOwnerInfo.Text = DFSPackHelper.Certificates.Own.ToText();

            if (DFSPackHelper.Certificates.Own.IsLoaded())
            {
                this.btnShowOwnCertificate.Enabled = true;
                this.btnSignAccountant.Enabled = true;
                this.btnSignDirector.Enabled = true;
                this.btnSignStamp.Enabled = true;
                this.btnEnvelop.Enabled = true;
                this.btnDevelop.Enabled = true;
            }
            else
            {
                this.btnShowOwnCertificate.Enabled = false;
                this.btnSignAccountant.Enabled = false;
                this.btnSignDirector.Enabled = false;
                this.btnSignStamp.Enabled = false;
                this.btnEnvelop.Enabled = false;
                this.btnDevelop.Enabled = false;
            }
        }

        private void btnShowOwnCertificate_Click(object sender, EventArgs e)
        {
            try
            {
                DFSPackHelper.ShowOwnCertificate();
            }
            catch (Exception ex)
            {
                this.ErrorMessageBox(ex.Message);
            }
        }

        private void btnSignAccountant_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.Multiselect = false;
                this.openFileDialog.Filter = "Повідомлення (@F0*.XML)|@F0*.XML|Всі файли (*.*)|*.*";
                if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (String fileName in this.openFileDialog.FileNames)
                    {
                        DFSPackHelper.SignFile(fileName, fileName + ".signB");
                        InfoMessageBox("Файл підписано бухгалтером:\n" + fileName + ".signB");
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessageBox(ex.Message);
            }
        }

        private void btnSignDirector_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.Multiselect = false;
                this.openFileDialog.Filter = "Повідомлення (@F0*.XML.signB)|@F0*.XML.signB|Всі файли (*.*)|*.*";
                if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (String fileName in this.openFileDialog.FileNames)
                    {
                        DFSPackHelper.SignFile(fileName, fileName + "D");
                        InfoMessageBox("Файл підписано директором:\n" + fileName + "D");
                    }
                }                
            }
            catch (Exception ex)
            {
                this.ErrorMessageBox(ex.Message);
            }
        }

        private void btnSignStamp_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.Multiselect = false;
                this.openFileDialog.Filter = "Повідомлення (@F0*.XML.signBD)|@F0*.XML.signBD|Всі файли (*.*)|*.*";
                if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (String fileName in this.openFileDialog.FileNames)
                    {
                        DFSPackHelper.SignFile(fileName, fileName + "P");
                        InfoMessageBox("Файл підписано печаткою:\n" + fileName + "P");
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessageBox(ex.Message);
            }
        }

        private void btnGetRecipient_Click(object sender, EventArgs e)
        {
            try
            {
                DFSPackHelper.GetRecipientCertificate();

                this.tbRecipient.Text = DFSPackHelper.Certificates.Recipient.ToText();
            }
            catch (Exception ex)
            {
                this.ErrorMessageBox(ex.Message);
            }
        }

        private void btnEnvelop_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.Multiselect = false;
                this.openFileDialog.Filter = "Повідомлення (@F0*.XML.signBDP)|@F0*.XML.signBDP|Всі файли (*.*)|*.*";
                if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (String fileName in this.openFileDialog.FileNames)
                    {
                        string dirName = fileName.Replace(".signBDP", "") + ".pack";
                        Directory.CreateDirectory(dirName);

                        string envelopedFileName = dirName + "\\" + Path.GetFileName(fileName.Replace(".signBDP", ""));

                        DFSPackHelper.EnvelopFile(fileName, envelopedFileName);

                        InfoMessageBox("Файл упаковано до теки:\n" + dirName);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessageBox(ex.Message);
            }
        }

        private void btnDevelop_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.Multiselect = false;
                this.openFileDialog.Filter = "Квитанція (@F1*.XML;@R3*.ZIP)|@F1*.XML;@R3*.ZIP|Всі файли (*.*)|*.*";
                if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (String fileName in this.openFileDialog.FileNames)
                    {
                        DFSPackHelper.DevelopFile(fileName);
                        InfoMessageBox("Вміст квитанції розкрито до теки:\n" + fileName + ".orig");
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessageBox(ex.Message);
            }
        }
    }
}
