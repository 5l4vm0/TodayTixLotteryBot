using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

class GmailMonitor
{
    private static string[] Scopes = { GmailService.Scope.GmailReadonly };
    private static string ApplicationName = "LotteryBot";
    private UserCredential _credential;
    private GmailService _service;

    // Constructor for GmailMonitor
    public GmailMonitor()
    {
        SetupCredential().Wait();
    }

    private async Task SetupCredential()
    {
        // Load client secrets from file
        using (var stream = new FileStream("credentials\\client_secret.json", FileMode.Open, FileAccess.Read))
        {
            // Requesting user authorization
            _credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore("Gmail.Api.Auth.Store", true));
        }

        // Create Gmail API service
        _service = new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = _credential,
            ApplicationName = ApplicationName,
        });
    }

    public async Task ListEmails()
    {
        // List the most recent emails
        ListMessagesResponse response = await _service.Users.Messages.List("me").ExecuteAsync();
        if (response.Messages != null && response.Messages.Count > 0)
        {
            foreach (var email in response.Messages)
            {
                // Retrieve the full message
                var message = await _service.Users.Messages.Get("me", email.Id).ExecuteAsync();

                // Extract the "To" email address from headers
                var toHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "To");
                if (toHeader != null)
                {
                    Console.WriteLine($"Email ID: {message.Id}, To: {toHeader.Value}");
                }
                else
                {
                    Console.WriteLine($"Email ID: {message.Id}, To: Not found");
                }

                // Print the email snippet or other information
                Console.WriteLine($"Snippet: {message.Snippet}");
            }
        }
        else
        {
            Console.WriteLine("No emails found.");
        }
    }

    public async Task<string> GetCodeFromEmail(string address)
    {
        // List the most recent emails
        ListMessagesResponse response = await _service.Users.Messages.List("me").ExecuteAsync();
        if (response.Messages == null || response.Messages.Count == 0)
        {
            return null;
        }

        foreach (var email in response.Messages)
        {

            // Retrieve the full message
            var message = await _service.Users.Messages.Get("me", email.Id).ExecuteAsync();

            // Extract the "To" email address from headers
            var toHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "To");
            var subjectHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject");

            if ((toHeader?.Value == address + "@gmail.com") && (subjectHeader?.Value.Contains("Your one-time access code: ") == true))
            {
                var startIndex = subjectHeader.Value.IndexOf(":") + 2;
                Console.WriteLine($"Email ID: {message.Id}, To: {toHeader.Value}, Subject: {subjectHeader.Value}");
                string code = subjectHeader.Value.Substring(startIndex);
                Console.WriteLine($"Code: {code}");
                return code.Trim();
            }

        }
        return null;
    }
}
