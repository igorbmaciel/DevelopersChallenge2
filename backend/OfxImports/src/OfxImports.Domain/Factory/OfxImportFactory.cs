using System;
using System.Text;
using System.IO;
using System.Xml;
using OfxImports.Domain.Queries.Response;
using OfxImports.Domain.Enum;
using Tnf.Notifications;
using OfxImports.Domain.AppConst;

namespace OfxImports.Domain.Factory
{
    public class OfxImportFactory
    {    
        private static StringBuilder TranslateToXml(string ofxSourceFile, INotificationHandler notification)
        {
            StringBuilder result = new StringBuilder();
            int level = 0;
            string line;

            if (!File.Exists(ofxSourceFile))            
                notification.RaiseError(AppConsts.LocalizationSourceName, EntityError.InvalidOfxSouceFile);            

            StreamReader sr = File.OpenText(ofxSourceFile);
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();

                if (line.StartsWith("</") && line.EndsWith(">"))
                {
                    AddTabs(result, level, true);
                    level--;
                    result.Append(line);
                }
                else if (line.StartsWith("<") && line.EndsWith(">"))
                {
                    level++;
                    AddTabs(result, level, true);
                    result.Append(line);
                }
                else if (line.StartsWith("<") && !line.EndsWith(">"))
                {
                    AddTabs(result, level + 1, true);
                    result.Append(line);
                    result.Append(ReturnFinalTag(line));
                }
            }
            sr.Close();

            return result;
        }

        public static ExtractResponse GenerateExtract(string ofxSourceFile, INotificationHandler notification)
            => GetExtract(ofxSourceFile, new ParserSettingsResponse(), notification);

        public static ExtractResponse GetExtract(string ofxSourceFile, ParserSettingsResponse settings, INotificationHandler notification)
        {
            if (settings == null) settings = new ParserSettingsResponse();

            bool hasHeader = false;
            bool hasAccountData = false;

            ExportToXml(ofxSourceFile, ofxSourceFile + ".xml", notification);

            string elementBeingRead = "";
            TransactionResponse currentTransaction = null;

            var bankAccount = new BankAccountResponse();
            var extract = new ExtractResponse(bankAccount, "");

            XmlTextReader myXml = new XmlTextReader(ofxSourceFile + ".xml");
            try
            {
                while (myXml.Read())
                {
                    if (myXml.NodeType == XmlNodeType.EndElement)
                    {
                        switch (myXml.Name)
                        {
                            case "STMTTRN":
                                if (currentTransaction != null)
                                {
                                    extract.AddTransaction(currentTransaction);
                                    currentTransaction = null;
                                }
                                break;
                        }
                    }
                    if (myXml.NodeType == XmlNodeType.Element)
                    {
                        elementBeingRead = myXml.Name;

                        switch (elementBeingRead)
                        {
                            case "STMTTRN":
                                currentTransaction = new TransactionResponse();
                                break;
                        }
                    }
                    if (myXml.NodeType == XmlNodeType.Text)
                    {
                        switch (elementBeingRead)
                        {
                            case "BANKID":
                                bankAccount.Code = GetBankId(myXml.Value, extract);
                                hasAccountData = true;
                                break;
                            case "BRANCHID":
                                bankAccount.AgencyCode = myXml.Value;
                                hasAccountData = true;
                                break;
                            case "ACCTID":
                                bankAccount.AccountCode = myXml.Value;
                                hasAccountData = true;
                                break;
                            case "ACCTTYPE":
                                bankAccount.Type = myXml.Value;
                                hasAccountData = true;
                                break;
                            case "TRNTYPE":
                                currentTransaction.Type = myXml.Value;
                                break;
                            case "DTPOSTED":
                                currentTransaction.Date = ConvertOfxDateToDateTime(myXml.Value, extract, notification);
                                break;
                            case "TRNAMT":
                                currentTransaction.TransactionValue = GetTransactionValue(myXml.Value, extract, notification);
                                break;
                            case "FITID":
                                currentTransaction.Id = myXml.Value;
                                break;
                            case "CHECKNUM":
                                currentTransaction.Checksum = Convert.ToInt64(myXml.Value);
                                break;
                            case "MEMO":
                                currentTransaction.Description = string.IsNullOrEmpty(myXml.Value) ? "" : myXml.Value.Trim().Replace("  ", " ");
                                break;
                        }
                    }
                }
            }
            catch (XmlException ex)
            {
                notification.RaiseError(AppConsts.LocalizationSourceName, EntityError.InvalidOfxFile);
            }
            finally
            {
                myXml.Close();
            }

            if ((settings.IsValidateHeader && hasHeader == false) ||
                (settings.IsValidateAccountData && hasAccountData == false))
            {
                notification.RaiseError(AppConsts.LocalizationSourceName, EntityError.InvalidOfxFile);
            }
            return extract;
        }

        private static void ExportToXml(string ofxSourceFile, string xmlNewFile, INotificationHandler notification)
        {
            if (File.Exists(ofxSourceFile))
            {
                if (xmlNewFile.ToLower().EndsWith(".xml"))
                {
                    StringBuilder ofxTranslated = TranslateToXml(ofxSourceFile, notification);

                    if (File.Exists(xmlNewFile))
                        File.Delete(xmlNewFile);

                    StreamWriter sw = File.CreateText(xmlNewFile);
                    sw.WriteLine(@"<?xml version=""1.0""?>");
                    sw.WriteLine(ofxTranslated.ToString());
                    sw.Close();
                }
                else                
                    notification.RaiseError(AppConsts.LocalizationSourceName, EntityError.InvalidXmlFile);                    
                
            }
            else            
                notification.RaiseError(AppConsts.LocalizationSourceName, EntityError.InvalidOfxSouceFile);
            
        }

        private static string ReturnFinalTag(string content)
        {
            string returnFinal = "";

            if ((content.IndexOf("<") != -1) && (content.IndexOf(">") != -1))
            {
                int position1 = content.IndexOf("<");
                int position2 = content.IndexOf(">");
                if ((position2 - position1) > 2)
                {
                    returnFinal = content.Substring(position1, (position2 - position1) + 1);
                    returnFinal = returnFinal.Replace("<", "</");
                }
            }

            return returnFinal;
        }

        private static void AddTabs(StringBuilder stringObject, int lengthTabs, bool newLine)
        {
            if (newLine)
            {
                stringObject.AppendLine();
            }
            for (int j = 1; j < lengthTabs; j++)
            {
                stringObject.Append("\t");
            }
        }

        private static int GetPartOfOfxDate(string ofxDate, PartDateTime partDateTime)
        {
            int result = 0;

            if (partDateTime == PartDateTime.YEAR)
            {
                result = int.Parse(ofxDate.Substring(0, 4));

            }
            else if (partDateTime == PartDateTime.MONTH)
            {
                result = int.Parse(ofxDate.Substring(4, 2));

            }
            if (partDateTime == PartDateTime.DAY)
            {
                result = int.Parse(ofxDate.Substring(6, 2));

            }
            if (partDateTime == PartDateTime.HOUR)
            {
                if (ofxDate.Length >= 10)
                    result = int.Parse(ofxDate.Substring(8, 2));
                else
                    result = 0;
            }
            if (partDateTime == PartDateTime.MINUTE)
            {
                if (ofxDate.Length >= 12)
                    result = int.Parse(ofxDate.Substring(10, 2));
                else
                    result = 0;
            }
            if (partDateTime == PartDateTime.SECOND)
            {
                if (ofxDate.Length >= 14)
                    result = int.Parse(ofxDate.Substring(12, 2));
                else
                    result = 0;
            }
            return result;
        }

        private static DateTime ConvertOfxDateToDateTime(string ofxDate, ExtractResponse extract, INotificationHandler notification)
        {
            DateTime dateTimeReturned = DateTime.MinValue;
            try
            {
                int year = GetPartOfOfxDate(ofxDate, PartDateTime.YEAR);
                int month = GetPartOfOfxDate(ofxDate, PartDateTime.MONTH);
                int day = GetPartOfOfxDate(ofxDate, PartDateTime.DAY);
                int hour = GetPartOfOfxDate(ofxDate, PartDateTime.HOUR);
                int minute = GetPartOfOfxDate(ofxDate, PartDateTime.MINUTE);
                int second = GetPartOfOfxDate(ofxDate, PartDateTime.SECOND);

                dateTimeReturned = new DateTime(year, month, day, hour, minute, second);
            }
            catch (Exception ex)
            {
                notification.RaiseError(AppConsts.LocalizationSourceName, EntityError.InvalidOfxDate);
            }
            return dateTimeReturned;
        }

        private static int GetBankId(string value, ExtractResponse extract)
        {
            int.TryParse(value, out int bankId);
            return bankId;
        }

        private static double GetTransactionValue(string value, ExtractResponse extract, INotificationHandler notification)
        {
            double returnValue = 0;
            try
            {
                returnValue = Convert.ToDouble(value.Replace('.', ','));
            }
            catch (Exception ex)
            {
                notification.RaiseError(AppConsts.LocalizationSourceName, EntityError.InvalidTransactionValue);
            }
            return returnValue;
        }

        public enum EntityError
        {
            InvalidOfxFile,
            InvalidXmlFile,
            InvalidOfxSouceFile,
            InvalidOfxDate,
            InvalidTransactionValue
        }
    }
}
