using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Pages
{
public class ReadEmailModel : PageModel 
    { 
        public EmailInfo emailInfo = new EmailInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String emailid = Request.Query["emailid"];
            try
            {
                String connectionString = "Server=tcp:cipherstream.database.windows.net,1433;Initial Catalog=emailsystem;Persist Security Info=False;User ID=cist;Password=@Cipherstream;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM emails WHERE email_id=@emailid";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@emailid", emailid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                emailInfo.EmailID = "" + reader.GetInt32(0);
                                emailInfo.EmailSubject = reader.GetString(1);
                                emailInfo.EmailMessage = reader.GetString(2);
                                emailInfo.EmailDate = reader.GetDateTime(3).ToString();
                                emailInfo.EmailIsRead = "" + reader.GetInt32(4);
                                emailInfo.EmailSender = reader.GetString(6);
                                emailInfo.EmailReceiver = reader.GetString(5);

                                

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

        }

    
        }
}

