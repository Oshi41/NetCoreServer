using System;
using System.Linq;
using System.Net.Sockets;

namespace NetCoreServer;

/// <summary>
/// String extensions utility class.
/// </summary>
public static class Extensions
{
    public static string RemoveWhiteSpace(this string self) => string.IsNullOrEmpty(self) ? self : new string(self.Where(c => !Char.IsWhiteSpace(c)).ToArray());

    public static void SetupSocket(this Socket socket, int keepAliveTime, int keepAliveInterval, int keepAliveRetryCount)
    {
        
    }
}