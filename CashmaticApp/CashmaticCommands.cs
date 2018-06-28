﻿//using hgi.Environment;
using hgi.Environment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace CashmaticApp
{
    public static class CashmaticCommands
    {


        public static void DeleteCashmaticFiles()
        {
            Debug.Log("CashmaticApp", "DeleteCashmaticFiles");
            try
            {
                System.IO.File.Delete(Global.cashmaticBasePath + "erogazione.txt");
                System.IO.File.Delete(Global.cashmaticBasePath + "nonerogato.txt");
                System.IO.File.Delete(Global.cashmaticBasePath + "erogato.txt");
                System.IO.File.Delete(Global.cashmaticBasePath + "pagato.txt");
                System.IO.File.Delete(Global.cashmaticBasePath + "saldato.txt");
            }
            catch(Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
           
        }

        /// <summary>
        /// The creation of the file “subtotale.txt” starts a new transaction.
        /// </summary>
        /// <param name="amount"></param>
        public static void WriteSubtotale(int amount)
        {
            try
            {

                if (amount <= 999999999 && amount >= -999999999)
                {
                    Global.subtotale = amount;
                    string file_name = "subtotale.txt";
                    string write_path = Global.cashmaticBasePath + file_name;
                    string correct_amount = amount.ToString("000000000");
                    string encrypted_content = encrypt(correct_amount);
                    System.IO.File.WriteAllText(write_path, encrypted_content);
                }

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
        }
        /// <summary>
        /// The creation of the file “annulla.txt” cancels a transaction in progress.
        /// </summary>
        public static void WriteAnnulla()
        {
            try
            {
                string file_name = "annulla.txt";
                string write_path = Global.cashmaticBasePath + file_name;
                System.IO.File.WriteAllText(write_path, "");
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
        }
        /// <summary>
        /// The creation of the file “checklevels.txt” returns a file "levels.xml" containing the current levels of automatic cash drawer.
        /// </summary>
        public static void WriteChecklevels()
        {
            try
            {
                string file_name = "checklevels.txt";
                string write_path = Global.cashmaticBasePath + file_name;
                System.IO.File.WriteAllText(write_path, "");
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
        }

        /// <summary>
        /// Read a response file named "levels.xml" whose content is divided into three nodes: coins, banknotes and stacker (cashbox).
        /// </summary>
        /// <returns></returns>
        public static MoneyLevels ReadLevels()
        {
            MoneyLevels ml = null;
            try
            {
                string file_name = "levels.xml";
                string read_path = Global.cashmaticBasePath + file_name;
                ml = new MoneyLevels();
                if (File.Exists(read_path))
                {
                    string decrypted_content = System.IO.File.ReadAllText(read_path);

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(decrypted_content);
                    XmlNodeList levels = xml.GetElementsByTagName("coins");
                    XmlNode levelsRoot = levels[0];
                    if (levelsRoot.ChildNodes.Count > 0)
                    {
                        ml.Coins = ReadMoneyLevels(levelsRoot.ChildNodes);
                    }
                    levels = xml.GetElementsByTagName("notes");
                    levelsRoot = levels[0];
                    if (levelsRoot.ChildNodes.Count > 0)
                    {
                        ml.Notes = ReadMoneyLevels(levelsRoot.ChildNodes);
                    }
                    levels = xml.GetElementsByTagName("stacker");
                    levelsRoot = levels[0];
                    if (levelsRoot.ChildNodes.Count > 0)
                    {
                        ml.Stacker = ReadMoneyLevels(levelsRoot.ChildNodes);
                    }
                }
                else
                {
                    //TODO: handle non existing level file
                }

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return ml;
        }
        /// <summary>
        /// The file is generated by the management panel and contains the partial paid amount.
        /// </summary>
        /// <returns></returns>
        public static int ReadSaldato()
        {
            int amount = 0;
            try
            {
                string file_name = "saldato.txt";
                amount = ReadFile(file_name);
               
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return amount;
        }
        /// <summary>
        /// The file is generated by the management panel and contains the total paid amount.
        /// </summary>
        /// <returns></returns>
        public static int ReadPagato()
        {
            int amount = 0;
            try
            {
                string file_name = "pagato.txt";
                amount = ReadFile(file_name);

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return amount;
        }
        /// <summary>
        /// The file is generated by the management panel at the end of each dispensed event. Contains the total dispensed amount.
        /// </summary>
        /// <returns></returns>
        public static int ReadErogato()
        {
            int amount = 0;
            try
            {
                string file_name = "erogato.txt";
                amount = ReadFile(file_name);

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return amount;
        }
        /// <summary>
        /// The file is generated by the management panel in case the automatic cash drawer cannot dispense the full amount. 
        /// The file, contains the total not dispensed amount.
        /// </summary>
        /// <returns></returns>
        public static int ReadNonerogato()
        {
            int amount = 0;
            try
            {
                string file_name = "nonerogato.txt";
                amount = ReadFile(file_name);

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return amount;
        }

        public static int ReadErogazione()
        {
            int amount = 0;
            try
            {
                string file_name = "erogazione.txt";
                amount = ReadFile(file_name);

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return amount;
        }

        /// <summary>
        /// The file "connected" lets you know the status of the automatic cash drawer. 
        /// In this case, the automatic cash drawer is connected and working correctly.
        /// </summary>
        /// <returns></returns>
        public static bool IsConnected()
        {
            bool flag = false;
            try
            {
                string file_name = "connected";
                string read_path = Global.cashmaticBasePath + file_name;
                flag = File.Exists(read_path);
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                flag = false;
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return flag;
        }

        /// <summary>
        /// The file "disconnected" lets you know the status of the automatic cash drawer. 
        /// In this case, it is disconnected or not working correctly.
        /// </summary>
        /// <returns></returns>
        public static bool IsDisconnected()
        {
            bool flag = false;
            try
            {
                string file_name = "disconnected";
                string read_path = Global.cashmaticBasePath + file_name;
                flag = File.Exists(read_path);
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                flag = false;
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return flag;
        }

        /// <summary>
        /// The file "cmd_response", contains the answer to the custom command sent previously. 
        /// If the command is unknown the answer will be: "unknown_comand".
        /// </summary>
        /// <returns>Returns cmd response</returns>
        public static string ReadCmdResponse()
        {
            string response = "";
            try
            {
                string file_name = "cmd_response";
                string read_path = Global.cashmaticBasePath + file_name;

                if (File.Exists(read_path))
                {
                    string encrypted_content = System.IO.File.ReadAllText(read_path);
                    response = decrypt(encrypted_content);

                    //unknown_comand
                    //cannot_process_command
                    //ok
                    //stacker_is_not_removed


                }
                else
                {
                    Debug.Log("CashmaticApp", "File" + file_name + " do not exists.");
                }
            }
            catch (Exception ex)
            {
                //TODO: handle exception
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return response;
        }

        /// <summary>
        /// The command starts dispensing coins in excess to cash fund level.
        /// Contains the amount withdrawn (emptied) formatted as 9 digits number, 
        /// or "cannot_process_command" if there was already a transaction in progress.
        /// </summary>
        /// <returns></returns>
        public static bool PartialCoinWithdrowal()
        {
            bool flag = false;

            try
            {
                WriteCmdCommand("coins_partial_withdrawal");
                string cmd_response = ReadCmdResponse();

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

            return flag;
        }

        /// <summary>
        /// The command starts dispensing all coins.
        /// Contains the amount withdrawn (emptied) formatted as 9 digits number,
        /// or "cannot_process_command" if there was already a transaction in progress.
        /// </summary>
        /// <returns></returns>
        public static bool TotalCoinWithdrowal()
        {
            bool flag = false;

            try
            {
                WriteCmdCommand("coins_total_withdrawal");
                string cmd_response = ReadCmdResponse();

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// The command starts the transfer of notes in excess to cash fund level from recycler to the stacker.
        /// The device reply is "ok" at the end of operation, or "cannot_process_command" if there was already a transaction in progress.
        /// </summary>
        /// <returns></returns>
        public static bool PartialNoteTransfer()
        {
            bool flag = false;

            try
            {
                WriteCmdCommand("notes_partial_transfer");
                string cmd_response = ReadCmdResponse();

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

            return flag;
        }
        /// <summary>
        /// The command starts the transfer of all notes from recycler to the stacker.
        /// The device reply is "ok" at the end of operation, or "cannot_process_command" if there was already a transaction in progress.
        /// </summary>
        /// <returns></returns>
        public static bool TotalNoteTransfer()
        {
            bool flag = false;

            try
            {
                WriteCmdCommand("notes_total_transfer");
                string cmd_response = ReadCmdResponse();

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

            return flag;
        }
        /// <summary>
        /// The command resets the stacker counter. To perform the reset the stacker will be removed.
        /// Contains the amount withdrawn (emptied) formatted as 9 digits number, 
        /// or "cannot_process_command" if there was already a transaction in progress.
        /// If the stacker is not removed, the device responds is: "stacker_is_not_removed".
        /// </summary>
        /// <returns></returns>
        public static bool TotalNoteWithdrawal()
        {
            bool flag = false;

            try
            {
                WriteCmdCommand("notes_total_withdrawal");
                string cmd_response = ReadCmdResponse();

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

            return flag;
        }

        

        /// <summary>
        /// The file "cmd" allows you to send a custom command. 
        /// The file is deleted at the end of execution of the command and the response is written in "cmd_response" file.
        /// </summary>
        /// <param name="custom_command"></param>
        private static void WriteCmdCommand(string custom_command)
        {
            try
            {
                string file_name = "cmd";
                string write_path = Global.cashmaticBasePath + file_name;
                string encrypted_command = encrypt(custom_command);
                System.IO.File.WriteAllText(write_path, encrypted_command);

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
        }


        private static int ReadFile(string file_name)
        {
            int amount = 0;
            try
            {

                string read_path = Global.cashmaticBasePath + file_name;

                if (File.Exists(read_path))
                {
                    string encrypted_content = System.IO.File.ReadAllText(read_path);
                    string decrypted_content = decrypt(encrypted_content);
                    amount = int.Parse(decrypted_content);
                }
                else
                {
                  
                    Debug.Log("CashmaticApp", "File"+file_name+" do not exists.");
                }

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return amount;
        }

        private static string encrypt(string text)
        {
            byte[] plaintextbytes = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(Global.cashmaticKey);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(Global.cashmaticInitializationVector);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encrypted = crypto.TransformFinalBlock(plaintextbytes, 0, plaintextbytes.Length);
            crypto.Dispose();
            return Convert.ToBase64String(encrypted);
        }

        private static string decrypt(string text)
        {
            byte[] encryptedbytes = Convert.FromBase64String(text);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(Global.cashmaticKey);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(Global.cashmaticInitializationVector);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform decrypto = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] decrypted = decrypto.TransformFinalBlock(encryptedbytes, 0, encryptedbytes.Length);
            decrypto.Dispose();
            return System.Text.ASCIIEncoding.ASCII.GetString(decrypted);
        }

        private static Dictionary<int, int> ReadMoneyLevels(XmlNodeList list)
        {
            Dictionary<int, int> moneyLevels = new Dictionary<int, int>();

            foreach (XmlElement el in list)
            {
                string elValue = el.GetAttribute("value");
                int levelKey = int.Parse(elValue);
                XmlNode inEl = el.FirstChild;
                int levelValue = int.Parse(inEl.Value.ToString());

                if (!moneyLevels.ContainsKey(levelKey))
                {
                    moneyLevels.Add(levelKey, levelValue);
                }

            }
            return moneyLevels;
        }

    }
}
