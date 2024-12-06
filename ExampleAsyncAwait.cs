using System.Net;

public class ExampleAsyncAwait
{
    #region Before Await and Async
    public void FetchData(string url)
    {
#pragma warning disable SYSLIB0014 // Type or member is obsolete
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
#pragma warning restore SYSLIB0014 // Type or member is obsolete
        request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);
    }

    private void FinishWebRequest(IAsyncResult result)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        HttpWebRequest request = (HttpWebRequest)result.AsyncState;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result))
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseData = reader.ReadToEnd();
                Console.WriteLine(responseData);
            }
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
    #endregion

    #region After Await and Async
    public async Task<string> FetchDataAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseData = await response.Content.ReadAsStringAsync();
            return responseData;
        }
    }
    #endregion
}
