using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace TequaCreek.eProcClientWSLibrary
{
    public class Controller
    {

        private string m_strEPWebServicesBaseUrl;

        public Controller()
        {

            m_strEPWebServicesBaseUrl = System.Configuration.ConfigurationManager.AppSettings["eProcurementWebServiceBaseURL"];
            if (m_strEPWebServicesBaseUrl.Substring(m_strEPWebServicesBaseUrl.Length - 1, 1) != "/")
            {
                m_strEPWebServicesBaseUrl += "/";
            }
        }

        public Controller(log4net.ILog objLogger)
        {

            this.Logger = objLogger;

            m_strEPWebServicesBaseUrl = System.Configuration.ConfigurationManager.AppSettings["eProcurementWebServiceBaseURL"];

            if (m_strEPWebServicesBaseUrl.Substring(m_strEPWebServicesBaseUrl.Length - 1, 1) != "/")
            {
                m_strEPWebServicesBaseUrl += "/";
            }
        }

        public log4net.ILog Logger { get; set; }

        private const int METHOD_RETURN_RECORD_NOT_FOUND = 2;
        private const int METHOD_USER_CANCELED_OPERATION = 3;
        private const int METHOD_RETURN_UNKNOWN_EXCEPTION_OCCURRED = -999;

        public bool AddApprovalHistoryEntry(int intRequisitionID, int intResubmitInstance, string strSupervisorToApproveUserID, 
                                            string strSupervisorApprovedUserID)
        {

            bool blnReturnValue = false;

            // RJB 2017-02-01
            //string strRequestURL = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurementWebServiceBaseURL") +
            //                       "/api/requisitionapprovalhistory";
            string strRequestURL = m_strEPWebServicesBaseUrl + "ws/requisitionapprovalhistory/";

            RequisitionApprovalHistoryEntry objEntry = new RequisitionApprovalHistoryEntry();
            objEntry.RequisitionID = intRequisitionID;
            objEntry.ResubmitInstance = intResubmitInstance;
            objEntry.ApprovalDateTime = System.DateTime.Now;
            objEntry.SupervisorToApproveUserID = strSupervisorToApproveUserID;
            objEntry.SupervisorApprovedUserID = strSupervisorApprovedUserID;

            byte[] bytPostData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objEntry, Formatting.Indented));

            HttpWebRequest request = WebRequest.Create(strRequestURL) as HttpWebRequest;

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = bytPostData.Length;

            Stream st = request.GetRequestStream();
            st.Write(bytPostData, 0, bytPostData.Length);
            st.Close();

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    blnReturnValue = true;
                }
                else
                {
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                                        response.StatusCode, response.StatusDescription));
                }
            }

            return blnReturnValue;

        }       // AddApprovalHistoryEntry()

        public ADUser GetADUserInformation(string strUserID)
        {

            ADUser objCallResult;
            string strRequestURL = m_strEPWebServicesBaseUrl + "api/aduser/";
            strRequestURL += strUserID;

            HttpWebRequest request = WebRequest.Create(strRequestURL) as HttpWebRequest;

            request.Method = "GET";
            request.ContentType = "application/json";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    objCallResult = JsonConvert.DeserializeObject<ADUser>(strsb);
                }
                else
                {
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));
                }

            }

            return objCallResult;

        }       // GetADUserInformation()

        public GenericCallResult SendDeniedRequisitionEMailMessage(int intRequisitionID, bool blnCopyAssistantTA, bool blnCopyTribalAdmin, 
                                                                   bool blnCopyTribalJudge, bool blnCopyExecCouncil)
        {

            GenericCallResult objReturnValue = new GenericCallResult();

            try
            {

                string strRequestURL = m_strEPWebServicesBaseUrl + "ws/deniedRequisitionEMailMessageSend/";

                DeniedRequisitionEMailMessageParameter objParameter = new DeniedRequisitionEMailMessageParameter();
                objParameter.RequisitionID = intRequisitionID;
                objParameter.CopyAssistantTA = blnCopyAssistantTA;
                objParameter.CopyTribalAdmin = blnCopyTribalAdmin;
                objParameter.CopyTribalJudge = blnCopyTribalJudge;
                objParameter.CopyExecCouncil = blnCopyExecCouncil;


                byte[] bytPostData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objParameter, Formatting.Indented));

                HttpWebRequest request = WebRequest.Create(strRequestURL) as HttpWebRequest;

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = bytPostData.Length;

                Stream st = request.GetRequestStream();
                st.Write(bytPostData, 0, bytPostData.Length);
                st.Close();

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_SUCCESS;
                    }
                    else
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                        objReturnValue.AdditionalInformation = response.StatusDescription;
                    }

                    response.Close();
                }

            }
            catch (Exception e)
            {
                Logger.Error("Unhandled exception occurred in Controller::SendDeniedRequisitionEMailMessage()", e);
                objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                objReturnValue.AdditionalInformation = e.Message;
            }

            return objReturnValue;

        }       // SendDeniedRequisitionEMailMessage

        public GenericCallResult SendPatternMatchedRequisitionEMailMessage(string strURL, int intRequisitionID)
        {

            GenericCallResult objReturnValue = new GenericCallResult();

            try
            {

                string strRequestURL = strURL.Replace("{requisitionId}", intRequisitionID.ToString());

                HttpWebRequest request = WebRequest.Create(strRequestURL) as HttpWebRequest;

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = 0;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_SUCCESS;
                    }
                    else
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                        objReturnValue.AdditionalInformation = response.StatusDescription;
                    }

                    response.Close();
                }

            }
            catch (Exception e)
            {
                Logger.Error("Unhandled exception occurred in Controller::SendPatternMatchedRequisitionEMailMessage()", e);
                objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                objReturnValue.AdditionalInformation = e.Message;
            }

            return objReturnValue;

        }       // SendPatternMatchedRequisitionEMailMessage()

        //

        public GenericCallResult SendPlacedInBudgetHoldModificationEMailMessage(PlacedInHoldForBudgetModificationEMailMessageParameter objPlacedInHoldForBudgetModificationEMailMessageParameter)
        {

            GenericCallResult objReturnValue = new GenericCallResult();

            try
            {

                string strURL = m_strEPWebServicesBaseUrl + System.Configuration.ConfigurationManager.AppSettings["PlacedInHoldForBudgetModificationEMailMessageSendBaseURL"];

                HttpWebRequest request = WebRequest.Create(strURL) as HttpWebRequest;

                byte[] bytPostData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objPlacedInHoldForBudgetModificationEMailMessageParameter, Formatting.Indented));

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = bytPostData.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bytPostData, 0, bytPostData.Length);
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_SUCCESS;
                    }
                    else
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                        objReturnValue.AdditionalInformation = response.StatusDescription;
                    }

                    response.Close();
                }

            }
            catch (Exception e)
            {
                Logger.Error("Unhandled exception occurred in Controller::SendReturnedRequisitionEMailMessage()", e);
                objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                objReturnValue.AdditionalInformation = e.Message;
            }

            return objReturnValue;

        }       // SendPatternMatchedRequisitionEMailMessage()



        public GenericCallResult SendReturnedRequisitionEMailMessage(ReturnedRequisitionEMailMessageParameter objReturnedRequisitionEMailMessageParameter)
        {

            GenericCallResult objReturnValue = new GenericCallResult();

            try
            {
                // ReturnToRequestorEMailMessageSendBaseURL ws/returnedToRequestorEMailMessageSend/

                // RJB 2019-05-23
                //string strURL = System.Configuration.ConfigurationManager.AppSettings["ReturnToRequestorEMailMessageSendBaseURL"];
                string strURL = m_strEPWebServicesBaseUrl + System.Configuration.ConfigurationManager.AppSettings["ReturnToRequestorEMailMessageSendBaseURL"];


                HttpWebRequest request = WebRequest.Create(strURL) as HttpWebRequest;

                byte[] bytPostData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objReturnedRequisitionEMailMessageParameter, Formatting.Indented));

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = bytPostData.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bytPostData, 0, bytPostData.Length);
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_SUCCESS;
                    }
                    else
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                        objReturnValue.AdditionalInformation = response.StatusDescription;
                    }

                    response.Close();
                }

            }
            catch (Exception e)
            {
                Logger.Error("Unhandled exception occurred in Controller::SendReturnedRequisitionEMailMessage()", e);
                objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                objReturnValue.AdditionalInformation = e.Message;
            }

            return objReturnValue;

        }       // SendPatternMatchedRequisitionEMailMessage()

        public GenericCallResult SendPurchaseOrderCreatedEMailMessage(int intRequisitionId)
        {

            GenericCallResult objReturnValue = new GenericCallResult();

            try
            {

                string strRequestURL = m_strEPWebServicesBaseUrl + "ws/purchaseOrderCreatedEMailMessageSend";

                PurchaseOrderCreatedEMailMessageParameter objParameter = new PurchaseOrderCreatedEMailMessageParameter();
                objParameter.requisitionId = intRequisitionId;


                byte[] bytPostData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objParameter, Formatting.Indented));

                HttpWebRequest request = WebRequest.Create(strRequestURL) as HttpWebRequest;

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = bytPostData.Length;

                Stream st = request.GetRequestStream();
                st.Write(bytPostData, 0, bytPostData.Length);
                st.Close();

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_SUCCESS;
                    }
                    else
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                        objReturnValue.AdditionalInformation = response.StatusDescription;
                    }

                    response.Close();
                }

            }
            catch (Exception e)
            {
                Logger.Error("Unhandled exception occurred in Controller::SendPurchaseOrderCreatedEMailMessage()", e);
                objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                objReturnValue.AdditionalInformation = e.Message;
            }

            return objReturnValue;

        }       // SendPurchaseOrderCreatedEMailMessage


        // RJB 2017-07-29
        public GenericCallResult ReturnRequisition(RequisitionForReturn objRequisitionForReturn)
        {

            GenericCallResult objReturnValue = new GenericCallResult();

            // RJB 2017-02-01
            //string strRequestURL = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurementWebServiceBaseURL") +
            //                       "/api/requisitionapprovalhistory";
            string strRequestURL = m_strEPWebServicesBaseUrl + "ws/requisition/return/";



            byte[] bytPostData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objRequisitionForReturn, Formatting.Indented));

            HttpWebRequest request = WebRequest.Create(strRequestURL) as HttpWebRequest;

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = bytPostData.Length;

            Stream st = request.GetRequestStream();
            st.Write(bytPostData, 0, bytPostData.Length);
            st.Close();

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_SUCCESS;
                }
                else
                {
                    objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                    objReturnValue.AdditionalInformation = String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription);
                }
            }

            return objReturnValue;

        }

        public purchaseOrder LookupPurchaseOrder(string strPurchaseOrderNumber)
        {

            purchaseOrder objReturnValue = new purchaseOrder();

            string strRequestURL = m_strEPWebServicesBaseUrl + "ws/resultantPurchaseOrder/";

            strRequestURL += strPurchaseOrderNumber + "/";

            HttpWebRequest request = WebRequest.Create(strRequestURL) as HttpWebRequest;

            request.Method = "GET";
            request.ContentType = "application/json";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    objReturnValue = JsonConvert.DeserializeObject<purchaseOrder>(strsb);
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {

                    objReturnValue.callresult = METHOD_RETURN_RECORD_NOT_FOUND;

                } else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    objReturnValue.callresult = METHOD_RETURN_UNKNOWN_EXCEPTION_OCCURRED;
                    Logger.Error(string.Format("Error returned from web service call {0} - {1}", response.StatusCode, response.StatusDescription));
                }

            }

            return objReturnValue;

        }       // LookupPurchaseOrder

        public GenericCallResult PushEProcurementAttachmentsIntoOnBase(int intRequisitionID)
        {

            GenericCallResult objReturnValue = new GenericCallResult();

            try
            {

                string strUrl = m_strEPWebServicesBaseUrl +
                                string.Format("ws/externalsysteminwardintegration/requisition/{0}/PushRequisitionAttachmentsIntoOnBase/", intRequisitionID);


                HttpWebRequest request = WebRequest.Create(strUrl) as HttpWebRequest;

                request.Method = "POST";
                request.ContentLength = 0;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_SUCCESS;
                    }
                    else
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                        objReturnValue.AdditionalInformation = response.StatusDescription;
                    }

                    response.Close();
                }

            }
            catch (Exception e)
            {
                Logger.Error("Unhandled exception occurred in Controller::PushEProcurementAttachmentsIntoOnBase()", e);
                objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                objReturnValue.AdditionalInformation = e.Message;
            }

            return objReturnValue;

        }       // PushEProcurementAttachmentsIntoOnBase()


        public GenericCallResult PushRequisitionDocumentIntoOnBase(int intRequisitionID)
        {

            GenericCallResult objReturnValue = new GenericCallResult();

            try
            {

                string strUrl = m_strEPWebServicesBaseUrl +
                                string.Format("ws/externalsysteminwardintegration/requisition/{0}/PushRequisitionDocumentIntoOnBase/", intRequisitionID);

                HttpWebRequest request = WebRequest.Create(strUrl) as HttpWebRequest;

                request.Method = "POST";
                request.ContentLength = 0;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_SUCCESS;
                    }
                    else
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                        objReturnValue.AdditionalInformation = response.StatusDescription;
                    }

                    response.Close();
                }

            }
            catch (Exception e)
            {
                Logger.Error("Unhandled exception occurred in Controller::PushRequisitionDocumentIntoOnBase()", e);
                objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                objReturnValue.AdditionalInformation = e.Message;
            }

            return objReturnValue;

        }       // PushRequisitionDocumentIntoOnBase()


        public GenericCallResult PushPrePayAuthorizationDocumentIntoOnBase(int intRequisitionID)
        {

            GenericCallResult objReturnValue = new GenericCallResult();

            try
            {

                string strUrl = m_strEPWebServicesBaseUrl +
                                string.Format("ws/externalsysteminwardintegration/requisition/{0}/PushPrepayAuthorizationDocumentIntoOnBase/", intRequisitionID);

                HttpWebRequest request = WebRequest.Create(strUrl) as HttpWebRequest;

                request.Method = "POST";
                request.ContentLength = 0;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_SUCCESS;
                    }
                    else
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                        objReturnValue.AdditionalInformation = response.StatusDescription;
                    }

                    response.Close();
                }

            }
            catch (Exception e)
            {
                Logger.Error("Unhandled exception occurred in Controller::PushPrePayAuthorizationDocumentIntoOnBase()", e);
                objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                objReturnValue.AdditionalInformation = e.Message;
            }

            return objReturnValue;

        }       // PushPrePayAuthorizationDocumentIntoOnBase()

        public GenericCallResult PushAttachmentsAsInvoiceIntoOnBase(int intRequisitionID)
        {

            GenericCallResult objReturnValue = new GenericCallResult();

            try
            {

                string strUrl = m_strEPWebServicesBaseUrl +
                                string.Format("ws/externalsysteminwardintegration/requisition/{0}/PushAttachmentsAsInvoiceIntoOnBase/", intRequisitionID);

                HttpWebRequest request = WebRequest.Create(strUrl) as HttpWebRequest;

                request.Method = "POST";
                request.ContentLength = 0;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_SUCCESS;
                    }
                    else
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                        objReturnValue.AdditionalInformation = response.StatusDescription;
                    }

                    response.Close();
                }

            }
            catch (Exception e)
            {
                Logger.Error("Unhandled exception occurred in Controller::PushAttachmentsAsInvoiceIntoOnBase()", e);
                objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                objReturnValue.AdditionalInformation = e.Message;
            }

            return objReturnValue;

        }       // PushAttachmentsAsInvoiceIntoOnBase()

        public GenericCallResult PushRequisitionAttachmentsIntoOnBase(int intRequisitionID)
        {

            GenericCallResult objReturnValue = new GenericCallResult();

            try
            {

                string strUrl = m_strEPWebServicesBaseUrl +
                                string.Format("ws/externalsysteminwardintegration/requisition/{0}/PushRequisitionAttachmentsIntoOnBase/", intRequisitionID);

                HttpWebRequest request = WebRequest.Create(strUrl) as HttpWebRequest;

                request.Method = "POST";
                request.ContentLength = 0;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_SUCCESS;
                    }
                    else
                    {
                        objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                        objReturnValue.AdditionalInformation = response.StatusDescription;
                    }

                    response.Close();
                }

            }
            catch (Exception e)
            {
                Logger.Error("Unhandled exception occurred in Controller::PushRequisitionDocumentIntoOnBase()", e);
                objReturnValue.ResultCode = GenericCallResult.RESULT_CODE_UNKNOWN_ERROR;
                objReturnValue.AdditionalInformation = e.Message;
            }

            return objReturnValue;

        }       // PushRequisitionAttachmentsIntoOnBase()


    }       // class
}       // namespace
