using System.Net;

public class ExampleAsyncAwait
{
    #region Before Await and Async
    public void FetchData(string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);
    }

    private void FinishWebRequest(IAsyncResult result)
    {
        HttpWebRequest request = (HttpWebRequest)result.AsyncState;
        using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result))
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseData = reader.ReadToEnd();
                Console.WriteLine(responseData);
            }
        }
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
