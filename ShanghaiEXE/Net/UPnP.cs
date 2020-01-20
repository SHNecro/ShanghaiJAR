using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace NSNet
{
    public sealed class UPnP
    {
        private IEnumerable<KeyValuePair<IPAddress, IPAddress>> NicAddressKeyValuePair
        {
            get
            {
                NetworkInterface[] networkInterfaceArray = NetworkInterface.GetAllNetworkInterfaces();
                for (int index = 0; index < networkInterfaceArray.Length; ++index)
                {
                    NetworkInterface nic = networkInterfaceArray[index];
                    IPInterfaceProperties ipProp = nic.GetIPProperties();
                    if (ipProp != null && ipProp.UnicastAddresses.Count > 0)
                    {
                        IPAddress localAddress = ipProp.UnicastAddresses[0].Address;
                        foreach (GatewayIPAddressInformation gatewayAddress in ipProp.GatewayAddresses)
                        {
                            GatewayIPAddressInformation gw = gatewayAddress;
                            yield return new KeyValuePair<IPAddress, IPAddress>(localAddress, gw.Address);
                            gw = null;
                        }
                        localAddress = null;
                    }
                    ipProp = null;
                    nic = null;
                }
                networkInterfaceArray = null;
            }
        }

        private string GetMultiCastResult(IPAddress remoteAddress)
        {
            string s = "M-SEARCH * HTTP/1.1\r\nHost:239.255.255.250:1900\r\nST:upnp:rootdevice\r\nMan:\"ssdp:discover\"\r\nMX:3\r\n\r\n";
            Socket socket = null;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint ipEndPoint = new IPEndPoint(remoteAddress, 1900);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                socket.SendTo(bytes, bytes.Length, SocketFlags.None, ipEndPoint);
                EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] numArray = new byte[1024];
                if (socket.ReceiveFrom(numArray, ref remoteEP) > 0)
                    return Encoding.ASCII.GetString(numArray);
                return null;
            }
            finally
            {
                if (socket != null)
                    socket.Close();
            }
        }

        private Uri GetServiceLocation(IPAddress remoteAddress)
        {
            try
            {
                string multiCastResult = this.GetMultiCastResult(remoteAddress);
                if (!string.IsNullOrEmpty(multiCastResult))
                {
                    string uriString = null;
                    string str1 = multiCastResult;
                    string[] separator = new string[1]
                    {
            Environment.NewLine
                    };
                    foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (str2.ToUpperInvariant().StartsWith("LOCATION", StringComparison.OrdinalIgnoreCase))
                        {
                            uriString = str2.Substring(str2.IndexOf(':') + 1);
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(uriString))
                        return new Uri(uriString);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        private string GetServices(Uri uri)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    return webClient.DownloadString(uri);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private UPnP.UPnpSetting GetUPnpSetting(IPAddress remoteAddress)
        {
            Uri serviceLocation = this.GetServiceLocation(remoteAddress);
            if (serviceLocation != null)
            {
                string services = this.GetServices(serviceLocation);
                if (!string.IsNullOrEmpty(services))
                {
                    string[] strArray = new string[2]
                    {
                        "urn:schemas-upnp-org:service:WANIPConnection:1",
                        "urn:schemas-upnp-org:service:WANPPPConnection:1"
                    };
                    foreach (string serviceType in strArray)
                    {
                        int startIndex = services.IndexOf(serviceType, StringComparison.OrdinalIgnoreCase);
                        if (startIndex != -1)
                        {
                            string str1 = services.Substring(startIndex);
                            string str2 = str1.Substring(str1.IndexOf("<controlURL>", StringComparison.OrdinalIgnoreCase) + "<controlURL>".Length);
                            string controlUrl = str2.Substring(0, str2.IndexOf("</controlURL>", StringComparison.OrdinalIgnoreCase));
                            return new UPnP.UPnpSetting(serviceLocation, services, serviceType, controlUrl);
                        }
                    }
                }
            }
            return null;
        }

        private byte[] CreateSoapBody(string body)
        {
            return Encoding.ASCII.GetBytes(string.Format(CultureInfo.InvariantCulture, "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n<s:Body>\r\n{0}\r\n</s:Body>\r\n</s:Envelope>", (object)body));
        }

        private string GetSoapResult(
          UPnP.UPnpSetting upnpSetting,
          string methodName,
          byte[] soapMessage)
        {
            WebRequest webRequest = WebRequest.Create(upnpSetting.RequestUri);
            webRequest.Method = "POST";
            webRequest.Headers.Add("SOAPAction", string.Format(CultureInfo.InvariantCulture, "\"{0}#{1}\"", upnpSetting.ServiceType, methodName));
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.ContentLength = soapMessage.Length;
            using (Stream requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(soapMessage, 0, soapMessage.Length);
                requestStream.Flush();
                requestStream.Close();
            }
            HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse;
            string str = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    str = streamReader.ReadToEnd();
                    streamReader.Close();
                }
            }
            return str;
        }

        public IPAddress GetWanAddress()
        {
            foreach (KeyValuePair<IPAddress, IPAddress> keyValuePair in this.NicAddressKeyValuePair)
            {
                IPAddress wanAddress = this.GetWanAddress(keyValuePair.Value);
                if (wanAddress != null)
                    return wanAddress;
            }
            return null;
        }

        public IPAddress GetWanAddress(IPAddress remoteAddress)
        {
            try
            {
                UPnP.UPnpSetting upnpSetting = this.GetUPnpSetting(remoteAddress);
                if (upnpSetting != null)
                {
                    string soapResult = this.GetSoapResult(upnpSetting, "GetExternalIPAddress", this.CreateSoapBody(string.Format(CultureInfo.InvariantCulture, "<u:GetExternalIPAddress xmlns:u=\"{0}\"></u:GetExternalIPAddress>", (object)upnpSetting.ServiceType)));
                    if (!string.IsNullOrEmpty(soapResult))
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(soapResult);
                        return IPAddress.Parse(xmlDocument.GetElementsByTagName("NewExternalIPAddress")[0].InnerText);
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        public bool AddTcpPortMapping(int port, int duration, string description)
        {
            foreach (KeyValuePair<IPAddress, IPAddress> keyValuePair in this.NicAddressKeyValuePair)
            {
                if (this.AddTcpPortMapping(keyValuePair.Key, keyValuePair.Value, port, duration, description))
                    return true;
            }
            return false;
        }

        public bool AddTcpPortMapping(
          IPAddress localAddress,
          IPAddress remoteAddress,
          int port,
          int duration,
          string description)
        {
            return this.AddPortMapping(localAddress, remoteAddress, port, duration, description, "TCP");
        }

        public bool AddUdpPortMapping(int port, int duration, string description)
        {
            foreach (KeyValuePair<IPAddress, IPAddress> keyValuePair in this.NicAddressKeyValuePair)
            {
                if (this.AddUdpPortMapping(keyValuePair.Key, keyValuePair.Value, port, duration, description))
                    return true;
            }
            return false;
        }

        public bool AddUdpPortMapping(
          IPAddress localAddress,
          IPAddress remoteAddress,
          int port,
          int duration,
          string description)
        {
            return this.AddPortMapping(localAddress, remoteAddress, port, duration, description, "UDP");
        }

        private bool AddPortMapping(
          IPAddress localAddress,
          IPAddress remoteAddress,
          int port,
          int duration,
          string description,
          string protocol)
        {
            try
            {
                UPnP.UPnpSetting upnpSetting = this.GetUPnpSetting(remoteAddress);
                if (upnpSetting != null)
                    return !string.IsNullOrEmpty(this.GetSoapResult(upnpSetting, nameof(AddPortMapping), this.CreateSoapBody(string.Format(CultureInfo.InvariantCulture, "<u:AddPortMapping xmlns:u=\"{0}\">\r\n<NewRemoteHost></NewRemoteHost>\r\n<NewExternalPort>{1}</NewExternalPort>\r\n<NewProtocol>{5}</NewProtocol>\r\n<NewInternalPort>{1}</NewInternalPort>\r\n<NewInternalClient>{2}</NewInternalClient>\r\n<NewEnabled>1</NewEnabled>\r\n<NewPortMappingDescription>{3}</NewPortMappingDescription>\r\n<NewLeaseDuration>{4}</NewLeaseDuration>\r\n</u:AddPortMapping>", upnpSetting.ServiceType, port, localAddress, description, duration, protocol))));
            }
            catch
            {
            }
            return false;
        }

        private DataTable CreatePortMappingTable()
        {
            return new DataTable()
            {
                Locale = CultureInfo.CurrentCulture,
                Columns = {
          {
            "RemoteAddress",
            typeof (string)
          },
          {
            "Index",
            typeof (int)
          },
          {
            "NewRemoteHost",
            typeof (string)
          },
          {
            "NewExternalPort",
            typeof (int)
          },
          {
            "NewProtocol",
            typeof (string)
          },
          {
            "NewInternalPort",
            typeof (int)
          },
          {
            "NewInternalClient",
            typeof (string)
          },
          {
            "NewEnabled",
            typeof (string)
          },
          {
            "NewPortMappingDescription",
            typeof (string)
          },
          {
            "NewLeaseDuration",
            typeof (int)
          }
        }
            };
        }

        public DataTable GetPortMapping()
        {
            DataTable portMappingTable = this.CreatePortMappingTable();
            foreach (KeyValuePair<IPAddress, IPAddress> keyValuePair in this.NicAddressKeyValuePair)
            {
                DataTable portMapping = this.GetPortMapping(keyValuePair.Value);
                if (portMapping != null)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)portMapping.Rows)
                        portMappingTable.Rows.Add(row.ItemArray);
                }
            }
            return portMappingTable;
        }

        public DataTable GetPortMapping(IPAddress remoteAddress)
        {
            DataTable portMappingTable = this.CreatePortMappingTable();
            int num = 0;
        label_8:
            DataTable portMapping = this.GetPortMapping(remoteAddress, num++);
            if (portMapping == null || portMapping.Rows.Count == 0)
                return portMappingTable;
            IEnumerator enumerator = portMapping.Rows.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DataRow current = (DataRow)enumerator.Current;
                    portMappingTable.Rows.Add(current.ItemArray);
                }
                goto label_8;
            }
            finally
            {
                (enumerator as IDisposable)?.Dispose();
            }
        }

        public DataTable GetPortMapping(IPAddress remoteAddress, int index)
        {
            DataTable portMappingTable = this.CreatePortMappingTable();
            try
            {
                UPnP.UPnpSetting upnpSetting = this.GetUPnpSetting(remoteAddress);
                if (upnpSetting != null)
                {
                    string soapResult = this.GetSoapResult(upnpSetting, "GetGenericPortMappingEntry", this.CreateSoapBody(string.Format(CultureInfo.InvariantCulture, "<u:GetGenericPortMappingEntry xmlns:u=\"{0}\">\r\n<NewPortMappingIndex>{1}</NewPortMappingIndex>\r\n</u:GetGenericPortMappingEntry>", upnpSetting.ServiceType, index)));
                    if (!string.IsNullOrEmpty(soapResult))
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(soapResult);
                        DataSet dataSet = new DataSet
                        {
                            Locale = CultureInfo.CurrentCulture
                        };
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            xmlDocument.Save(memoryStream);
                            memoryStream.Seek(0L, SeekOrigin.Begin);
                            XmlTextReader xmlTextReader = new XmlTextReader(memoryStream);
                            int num = (int)dataSet.ReadXml(xmlTextReader, XmlReadMode.Auto);
                            xmlTextReader.Close();
                        }
                        if (dataSet.Tables.Contains("GetGenericPortMappingEntryResponse"))
                        {
                            foreach (DataRow row1 in (InternalDataCollectionBase)dataSet.Tables["GetGenericPortMappingEntryResponse"].Rows)
                            {
                                DataRow row2 = portMappingTable.NewRow();
                                row2["RemoteAddress"] = remoteAddress.ToString();
                                row2["Index"] = index;
                                row2["NewRemoteHost"] = row1["NewRemoteHost"];
                                row2["NewExternalPort"] = row1["NewExternalPort"];
                                row2["NewProtocol"] = row1["NewProtocol"];
                                row2["NewInternalPort"] = row1["NewInternalPort"];
                                row2["NewInternalClient"] = row1["NewInternalClient"];
                                row2["NewEnabled"] = row1["NewEnabled"];
                                row2["NewPortMappingDescription"] = row1["NewPortMappingDescription"];
                                row2["NewLeaseDuration"] = row1["NewLeaseDuration"];
                                portMappingTable.Rows.Add(row2);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return portMappingTable;
        }

        public bool DeleteTcpPortMapping(int port)
        {
            foreach (KeyValuePair<IPAddress, IPAddress> keyValuePair in this.NicAddressKeyValuePair)
            {
                if (this.DeleteTcpPortMapping(keyValuePair.Value, port))
                    return true;
            }
            return false;
        }

        public bool DeleteTcpPortMapping(IPAddress remoteAddress, int port)
        {
            return this.DeletePortMapping(remoteAddress, port, "TCP");
        }

        public bool DeleteUdpPortMapping(int port)
        {
            foreach (KeyValuePair<IPAddress, IPAddress> keyValuePair in this.NicAddressKeyValuePair)
            {
                if (this.DeleteUdpPortMapping(keyValuePair.Value, port))
                    return true;
            }
            return false;
        }

        public bool DeleteUdpPortMapping(IPAddress remoteAddress, int port)
        {
            return this.DeletePortMapping(remoteAddress, port, "UDP");
        }

        private bool DeletePortMapping(IPAddress remoteAddress, int port, string protocol)
        {
            try
            {
                UPnP.UPnpSetting upnpSetting = this.GetUPnpSetting(remoteAddress);
                if (upnpSetting != null)
                    return !string.IsNullOrEmpty(this.GetSoapResult(upnpSetting, nameof(DeletePortMapping), this.CreateSoapBody(string.Format(CultureInfo.InvariantCulture, "<u:DeletePortMapping xmlns:u=\"{0}\">\r\n<NewRemoteHost></NewRemoteHost>\r\n<NewExternalPort>{1}</NewExternalPort>\r\n<NewProtocol>{2}</NewProtocol>\r\n</u:DeletePortMapping>", upnpSetting.ServiceType, port, protocol))));
            }
            catch
            {
            }
            return false;
        }

        private sealed class UPnpSetting
        {
            public UPnpSetting(
              Uri serviceLocation,
              string services,
              string serviceType,
              string controlUrl)
            {
                this.ServiceLocation = serviceLocation;
                this.Services = services;
                this.ServiceType = serviceType;
                this.ControlUrl = controlUrl;
            }

            public Uri ServiceLocation { get; private set; }

            public string Services { get; private set; }

            public string ServiceType { get; private set; }

            public string ControlUrl { get; private set; }

            public Uri RequestUri
            {
                get
                {
                    return new Uri(this.ServiceLocation, this.ControlUrl);
                }
            }
        }
    }
}
