using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Pages
{
    public class ReadEmailModel : PageModel
    {

        public List<ReadEmailInfo> listEmails = new List<ReadEmailInfo>();

        private readonly ILogger<ReadEmailModel> _logger;

        public ReadEmailModel(ILogger<ReadEmailModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string emailId)
        {
            listEmails.Clear();
            try
            {
                String connectionString = "Server=tcp:cipherstream.database.windows.net,1433;Initial Catalog=emailsystem;Persist Security Info=False;User ID=cist;Password=@Cipherstream;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "SELECT * FROM emails WHERE email_id=@emailid";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ReadEmailInfo reademailInfo = new ReadEmailInfo();
                                reademailInfo.EmailID = "" + reader.GetInt32(0);
                                reademailInfo.EmailSubject = reader.GetString(1);
                                reademailInfo.EmailMessage = reader.GetString(2);
                                reademailInfo.EmailDate = reader.GetDateTime(3).ToString();
                                reademailInfo.EmailIsRead = "" + reader.GetInt32(4);
                                reademailInfo.EmailSender = reader.GetString(6);

                                listEmails.Add(reademailInfo);
                            }
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
    public class ReadEmailInfo
    {
        public String EmailID;
        public String EmailSubject;
        public String EmailMessage;
        public String EmailDate;
        public String EmailIsRead;
        public String EmailSender;

    }

}
