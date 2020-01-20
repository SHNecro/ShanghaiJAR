using System;
using System.Collections;
using System.Runtime.InteropServices;
using UPNPLib;

internal class UPnPControlPoint
{
    private UPnPService Service { get; set; }

    private static UPnPDevice GetDevice(UPnPDeviceFinderClass finder, string typeUri)
    {
        IEnumerator enumerator = ((IUPnPDevices)finder.FindByType(typeUri, 0U)).GetEnumerator();
        try
        {
            if (enumerator.MoveNext())
                return (UPnPDevice)enumerator.Current;
        }
        finally
        {
            (enumerator as IDisposable)?.Dispose();
        }
        return (UPnPDevice)null;
    }

    private static UPnPDevice GetDevice(UPnPDeviceFinderClass finder)
    {
        return UPnPControlPoint.GetDevice(finder, "urn:schemas-upnp-org:service:WANPPPConnection:1") ?? UPnPControlPoint.GetDevice(finder, "urn:schemas-upnp-org:service:WANIPConnection:1");
    }

    private static UPnPService GetService(UPnPDevice device, string serviceId)
    {
        try
        {
            return ((IUPnPServices)((IUPnPDevice)device).get_Services()).get_Item(serviceId);
        }
        catch (ArgumentException ex)
        {
            return (UPnPService)null;
        }
    }

    private static UPnPService GetService(UPnPDevice device)
    {
        return UPnPControlPoint.GetService(device, "urn:upnp-org:serviceId:WANPPPConn1") ?? UPnPControlPoint.GetService(device, "urn:upnp-org:serviceId:WANIPConn1");
    }

    private static UPnPService GetService()
    {
        UPnPDevice device = UPnPControlPoint.GetDevice(new UPnPDeviceFinderClass());
        if (device == null)
            return (UPnPService)null;
        return UPnPControlPoint.GetService(device);
    }

    public UPnPControlPoint()
    {
        this.Service = UPnPControlPoint.GetService();
    }

    private void InvokeAction(string bstrActionName, object vInActionArgs)
    {
        if (this.Service == null)
            return;
        try
        {
            object obj = new object();
            ((IUPnPService)this.Service).InvokeAction(bstrActionName, vInActionArgs, ref obj);
        }
        catch (COMException ex)
        {
        }
    }

    public void AddPortMapping(
      string remoteHost,
      ushort externalPort,
      string protocol,
      ushort internalPort,
      string internalClient,
      string description)
    {
        this.InvokeAction(nameof(AddPortMapping), (object)new object[8]
        {
      (object) remoteHost,
      (object) externalPort,
      (object) protocol,
      (object) internalPort,
      (object) internalClient,
      (object) true,
      (object) description,
      (object) 0
        });
    }

    public void DeletePortMapping(string remoteHost, ushort externalPort, string protocol)
    {
        this.InvokeAction(nameof(DeletePortMapping), (object)new object[3]
        {
      (object) remoteHost,
      (object) externalPort,
      (object) protocol
        });
    }
}
