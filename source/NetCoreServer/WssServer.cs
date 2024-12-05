﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetCoreServer;

/// <summary>
/// WebSocket secure server
/// </summary>
/// <remarks> WebSocket secure server is used to communicate with clients using WebSocket protocol. Thread-safe.</remarks>
public class WssServer : HttpsServer, IWebSocket
{
    internal readonly WebSocket WebSocket;

    /// <summary>
    /// Initialize WebSocket server with a given IP address and port number
    /// </summary>
    /// <param name="context">SSL context</param>
    /// <param name="address">IP address</param>
    /// <param name="port">Port number</param>
    public WssServer(SslContext context, IPAddress address, int port) : base(context, address, port) { WebSocket = new WebSocket(this); }
    /// <summary>
    /// Initialize WebSocket server with a given IP address and port number
    /// </summary>
    /// <param name="context">SSL context</param>
    /// <param name="address">IP address</param>
    /// <param name="port">Port number</param>
    public WssServer(SslContext context, string address, int port) : base(context, address, port) { WebSocket = new WebSocket(this); }
    /// <summary>
    /// Initialize WebSocket server with a given DNS endpoint
    /// </summary>
    /// <param name="context">SSL context</param>
    /// <param name="endpoint">DNS endpoint</param>
    public WssServer(SslContext context, DnsEndPoint endpoint) : base(context, endpoint) { WebSocket = new WebSocket(this); }
    /// <summary>
    /// Initialize WebSocket server with a given IP endpoint
    /// </summary>
    /// <param name="context">SSL context</param>
    /// <param name="endpoint">IP endpoint</param>
    public WssServer(SslContext context, IPEndPoint endpoint) : base(context, endpoint) { WebSocket = new WebSocket(this); }

    #region Session management

    public virtual bool CloseAll() => CloseAll(0, Span<byte>.Empty);
    public virtual bool CloseAll(int status) => CloseAll(status, Span<byte>.Empty);
    public virtual bool CloseAll(int status, string text) => CloseAll(status, Encoding.UTF8.GetBytes(text));
    public virtual bool CloseAll(int status, ReadOnlySpan<char> text) => CloseAll(status, Encoding.UTF8.GetBytes(text.ToArray()));
    public virtual bool CloseAll(int status, byte[] buffer) => CloseAll(status, buffer.AsSpan());
    public virtual bool CloseAll(int status, byte[] buffer, long offset, long size) => CloseAll(status, buffer.AsSpan((int)offset, (int)size));
    public virtual bool CloseAll(int status, ReadOnlySpan<byte> buffer)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame(WebSocket.WS_FIN | WebSocket.WS_CLOSE, false, buffer, status);
            if (!Multicast(WebSocket.WsSendBuffer.AsSpan()))
                return false;

            return base.DisconnectAll();
        }
    }

    #endregion

    #region Multicasting

    public override bool Multicast(ReadOnlySpan<byte> buffer)
    {
        if (!IsStarted)
            return false;

        if (buffer.IsEmpty)
            return true;

        // Multicast data to all WebSocket sessions
        foreach (var session in Sessions.Values)
        {
            if (session is WssSession wsSession)
            {
                if (wsSession.WebSocket.WsHandshaked)
                    wsSession.SendAsync(buffer);
            }
        }

        return true;
    }

    #endregion

    #region WebSocket multicast text methods

    public bool MulticastText(string text) => MulticastText(Encoding.UTF8.GetBytes(text));
    public bool MulticastText(ReadOnlySpan<char> text) => MulticastText(Encoding.UTF8.GetBytes(text.ToArray()));
    public bool MulticastText(byte[] buffer) => MulticastText(buffer.AsSpan());
    public bool MulticastText(byte[] buffer, long offset, long size) => MulticastText(buffer.AsSpan((int)offset, (int)size));
    public bool MulticastText(ReadOnlySpan<byte> buffer)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame(WebSocket.WS_FIN | WebSocket.WS_TEXT, false, buffer);
            return Multicast(WebSocket.WsSendBuffer.AsSpan());
        }
    }

    #endregion

    #region WebSocket multicast binary methods

    public bool MulticastBinary(string text) => MulticastBinary(Encoding.UTF8.GetBytes(text));
    public bool MulticastBinary(ReadOnlySpan<char> text) => MulticastBinary(Encoding.UTF8.GetBytes(text.ToArray()));
    public bool MulticastBinary(byte[] buffer) => MulticastBinary(buffer.AsSpan());
    public bool MulticastBinary(byte[] buffer, long offset, long size) => MulticastBinary(buffer.AsSpan((int)offset, (int)size));
    public bool MulticastBinary(ReadOnlySpan<byte> buffer)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame(WebSocket.WS_FIN | WebSocket.WS_BINARY, false, buffer);
            return Multicast(WebSocket.WsSendBuffer.AsSpan());
        }
    }


    #endregion

    #region WebSocket multicast ping methods

    public bool MulticastPing(string text) => MulticastPing(Encoding.UTF8.GetBytes(text));
    public bool MulticastPing(ReadOnlySpan<char> text) => MulticastPing(Encoding.UTF8.GetBytes(text.ToArray()));
    public bool MulticastPing(byte[] buffer) => MulticastPing(buffer.AsSpan());
    public bool MulticastPing(byte[] buffer, long offset, long size) => MulticastPing(buffer.AsSpan((int)offset, (int)size));
    public bool MulticastPing(ReadOnlySpan<byte> buffer)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame(WebSocket.WS_FIN | WebSocket.WS_PING, false, buffer);
            return Multicast(WebSocket.WsSendBuffer.AsSpan());
        }
    }

    #endregion

    protected override SslSession CreateSession() { return new WssSession(this); }
    public virtual void OnWsConnecting(HttpRequest request)
    {
        
    }

    public virtual void OnWsConnected(HttpResponse response)
    {
        
    }

    public virtual bool OnWsConnecting(HttpRequest request, HttpResponse response)
    {
        return true;
    }

    public virtual void OnWsConnected(HttpRequest request)
    {
        
    }

    public virtual void OnWsDisconnecting()
    {
        
    }

    public virtual void OnWsDisconnected()
    {
        
    }

    public virtual void OnWsReceived(byte[] buffer, long offset, long size)
    {
        
    }

    public virtual void OnWsClose(byte[] buffer, long offset, long size, int status = 1000)
    {
        
    }

    public virtual void OnWsPing(byte[] buffer, long offset, long size)
    {
        
    }

    public virtual void OnWsPong(byte[] buffer, long offset, long size)
    {
        
    }

    public virtual void OnWsError(string error)
    {
        
    }

    public virtual void OnWsError(SocketError error)
    {
        
    }

    public virtual void SendUpgrade(HttpResponse response)
    {
        
    }
}