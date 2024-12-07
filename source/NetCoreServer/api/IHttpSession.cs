namespace NetCoreServer;

public interface IHttpSession : ISession
{
    /// <summary>
    /// Get the static content cache
    /// </summary>
    public FileCache Cache { get; }
    
    /// <summary>
    /// Get the HTTP request
    /// </summary>
    public HttpRequest Request { get; }   
    
    /// <summary>
    /// Get the HTTP response
    /// </summary>
    public HttpResponse Response { get; }
}