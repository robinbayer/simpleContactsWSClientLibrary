using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace Overthink.SimpleContactsWSClientLibrary
{
    public class Controller
    {

        private string m_strSCWebServicesBaseUrl;

        public const int METHOD_RETURN_SUCCESS = 0;
        public const int METHOD_RETURN_RECORD_NOT_FOUND = -1;
        public const int METHOD_USER_CANCELED_OPERATION = -2;
        public const int METHOD_RETURN_UNKNOWN_EXCEPTION_OCCURRED = -999;

        public Controller()
        {

            m_strSCWebServicesBaseUrl = System.Configuration.ConfigurationManager.AppSettings["SimpleContactsWebServiceBaseURL"];
            if (m_strSCWebServicesBaseUrl.Substring(m_strSCWebServicesBaseUrl.Length - 1, 1) != "/")
            {
                m_strSCWebServicesBaseUrl += "/";
            }
        }

        public Contact getContactById(string contactId)
        {

            Contact objReturnValue = new Contact();

            string strRequestURL = m_strSCWebServicesBaseUrl + string.Format("ws/contact/{0}/", contactId);
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
                    objReturnValue = JsonConvert.DeserializeObject<Contact>(strsb);
                }
                else
                {
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                }
            }

            return objReturnValue;

        }       // getContactById()

        public APICallResult addContact(Contact contact)
        {

            APICallResult objReturnValue = new APICallResult();

            string strRequestURL = m_strSCWebServicesBaseUrl + "ws/contact/";
            byte[] bytPostData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(contact, Formatting.Indented));

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
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    objReturnValue = JsonConvert.DeserializeObject<APICallResult>(strsb);
                }
                else
                {
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                }
            }

            return objReturnValue;

        }       // addContact()

        public APICallResult updateContact(Contact contact)
        {

            APICallResult objReturnValue = new APICallResult();

            string strRequestURL = m_strSCWebServicesBaseUrl + "ws/contact/";
            byte[] bytPostData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(contact, Formatting.Indented));

            HttpWebRequest request = WebRequest.Create(strRequestURL) as HttpWebRequest;

            request.Method = "PUT";
            request.ContentType = "application/json";
            request.ContentLength = bytPostData.Length;

            Stream st = request.GetRequestStream();
            st.Write(bytPostData, 0, bytPostData.Length);
            st.Close();

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    objReturnValue = JsonConvert.DeserializeObject<APICallResult>(strsb);
                }
                else
                {
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                }
            }

            return objReturnValue;

        }       // updateContact()

        public APICallResult deleteContact(string contactId)
        {

            APICallResult objReturnValue = new APICallResult();

            string strRequestURL = m_strSCWebServicesBaseUrl + string.Format("ws/contact/{0}/", contactId);

            HttpWebRequest request = WebRequest.Create(strRequestURL) as HttpWebRequest;

            request.Method = "DELETE";
            request.ContentType = "application/json";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    objReturnValue = JsonConvert.DeserializeObject<APICallResult>(strsb);
                }
                else
                {
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                }
            }

            return objReturnValue;

        }       // delectContact()

    }       // class
}       // namespace
