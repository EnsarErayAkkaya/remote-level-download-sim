using Cysharp.Threading.Tasks;
using EEA.LoggerService;
using EEA.Utilities;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace EEA.Web
{
    /// <summary>
    /// Wrapper class for sending web requests using UnityWebRequest.
    /// </summary>
    public static class RequestSender
    {
        public static async UniTask<Response> GetAsync(string url, Dictionary<string, string> headers = null, int timeout = 0)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // Set Timeout
                if (timeout != 0)
                    webRequest.timeout = timeout;

                // Send the request asynchronously
                var operation = webRequest.SendWebRequest();
                while (!operation.isDone)
                {
                    await UniTask.Yield(); // Wait for the request to complete
                }

                ErrorType errorType = ErrorType.NoError;

                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    // Indicates a network issue, such as no internet connection or DNS resolution failure.
                    if (ServiceManager.Instance.Settings.debugLog)
                        EEALogger.Log($"Connection (cannot reach the server) error: {webRequest.error}, url: {url}");
                }
                else if (webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    errorType = ErrorType.ProtocolError;
                    // Indicates an HTTP error returned by the server (e.g., 404 Not Found, 500 Internal Server Error).
                    if (ServiceManager.Instance.Settings.debugLog)
                        EEALogger.Log($"HTTP (protocol error) error: {webRequest.error}, url: {url}");
                }
                else if (webRequest.result == UnityWebRequest.Result.DataProcessingError)
                {
                    errorType = ErrorType.DataProcessingError;

                    // Indicates an error while processing the response data.
                    if (ServiceManager.Instance.Settings.debugLog)
                        EEALogger.Log($"Data processing (response was corrupted or not in correct format) error: {webRequest.error}, url: {url}");
                }

                // Collect response details
                string body = webRequest.downloadHandler.text;
                long statusCode = webRequest.responseCode;
                string error = webRequest.result != UnityWebRequest.Result.Success ? webRequest.error : null;

                // class pool response to optimize
                var response = ClassPool<Response>.Spawn() ?? new Response();

                response.StatusCode = statusCode;
                response.Data = body;
                response.Error = error;
                response.ErrorType = errorType;

                return response;
            }
        }
    }
}