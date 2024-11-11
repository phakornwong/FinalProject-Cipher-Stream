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
        private SqlConnection connection;

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

                    string username = "";
                    if (User.Identity.Name == null)
                    {
                        username = "";
                    }
                    else
                    {
                        username = User.Identity.Name;
                    }

                    if (emailInfo.EmailReceiver != username)
                    {
                        // แสดงหน้าจอพร้อมข้อความ "Hey Bro!!??" ก่อน Redirect
                        TempData["ErrorMessage"] = "Hey Bro!!?? why are you doing that HUH???";
                        Response.Redirect("/RedirectWarning");
                        return;
                    }


                    // อัปเดตค่า emailisread เป็น 1
                    String updateSql = "UPDATE emails SET emailisread = 1 WHERE email_id = @emailid";

                    using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@emailid", emailid);
                        updateCommand.ExecuteNonQuery();
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

