using Doppler.BigQueryMicroservice.Entitites;
using System.Collections.Generic;

namespace Doppler.BigQueryMicroservice.Serialization
{
    /// <summary>
    /// Serialize structure for response in allow mails get
    /// </summary>
    public class AllowedEmails
    {
        public AllowedEmails()
        {
            Emails = new List<string>();
        }

        public AllowedEmails(IReadOnlyList<UserAccessByUser> data)
        {
            Emails = new List<string>();
            foreach (var allowEmail in data)
            {
                Emails.Add(allowEmail.Email);
            }
        }
        public List<string> Emails { get; set; }
    }
}
