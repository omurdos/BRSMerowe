using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.DTOs;
using Shared.Response;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace Shared.Services
{
    public class SMSService
    {
        static readonly HttpClient client = new HttpClient();
        private String User = "sinnaruniversityapi";
        private string Password = "sinnaruniversityapi#87";
        private string Sender = "Sinnar Univ";

        private string Unicode = "";
        private readonly TSTDBContext _context;
        private readonly ILogger<SMSService> _logger;


        // Specify the environment variable name
        string WhatsAppBotToken = "WHATSAPP_BOT_TOKEN";



        public SMSService(TSTDBContext context, ILogger<SMSService> logger)
        {
            _context= context;
            _logger= logger;
        }

        public async Task<bool> SendSMS(string PhoneNumber, string Code, string Source)
        {



            return await Send(PhoneNumber, Code, Source);



        }

        //public async Task<bool> SendWhatsApp(string PhoneNumber, string FullName, string OTP)
        //{

        //    _logger.LogInformation("Sending message through whatsapp");


        //    return await SendWhatsAppMessageWithLimit(PhoneNumber, FullName, OTP);



        //}

        public async Task<bool> BulkSMS(List<string> phonenUmbers, string message, string Source)
        {



            return await SendBulk(phonenUmbers, message, Source);



        }

        //public async Task<bool> SendFeesSMS(string PhoneNumber, string Message, string Source)
        //{
        //    return await Send(PhoneNumber, Message, Source);
        //}

        public async Task<bool> Message(string PhoneNumber, string Message, string Source)
        {
            return await Send(PhoneNumber, Message, Source);

        }

        private async Task<bool> Send(string PhoneNumber, string Message, string Source)
        {
            try
            {

                //Get current count of SMS sent to this number
                var smsaccess = await _context.SMSAccesses.FirstOrDefaultAsync(s => s.PhoneNumber == PhoneNumber);
                //incase we sent any sms to this user
                if (smsaccess != null)
                {
                    // check if phone has been blocked two times then block it foreever
                    if (smsaccess.BlockCounts >= 2)
                    {
                        return false;
                    }
                    //if phone number is blocked
                    if (smsaccess.IsBlocked)
                    {
                        //check if the user should be unblocked by the system
                        var TimeDifference = DateTime.Now.Subtract(smsaccess.LockedAt);
                        //Unblocking a user
                        if (TimeDifference > TimeSpan.FromHours(24))
                        {
                            //Unblock the user
                            smsaccess.IsBlocked = false;
                            //reset sms count
                            smsaccess.SendCount = 0;
                            //send sms to the user
                            _context.SMSAccesses.Update(smsaccess);
                            var updateResult = await _context.SaveChangesAsync();
                            if (updateResult > 0)
                            {
                                bool IsSent = false;

                                //This is were a lot of SMSs being sent to the users
                                IsSent = await ExecuteBarqSMS(PhoneNumber, Message);

                                if (IsSent)
                                {
                                    var otp = new OTPCode
                                    {
                                        Code= Message,
                                        PhoneNumber= PhoneNumber,
                                        Source = Source,
                                        CreatedAt= DateTime.Now,
                                        ModifiedAt= DateTime.Now,
                                    };

                                    await _context.OTPCodes.AddAsync(otp);
                                    var result = await _context.SaveChangesAsync();
                                    return true;
                                }
                                else
                                {
                                    _logger.LogInformation("Failed to send an sms to {phoneNumber}", PhoneNumber);

                                    return false;
                                }




                            }
                            return false;

                        }
                        return false;

                    }
                    if (!smsaccess.IsBlocked)
                    {


                        if (smsaccess.SendCount == 5)
                        {
                            smsaccess.IsBlocked = true;
                            smsaccess.LockedAt = DateTime.Now;
                            smsaccess.BlockCounts += 1;
                            _context.SMSAccesses.Update(smsaccess);
                            var result = await _context.SaveChangesAsync();
                            if (result > 0)
                            {
                                _logger.LogError("Failed to save sms access update for user blocking");
                                return false;
                            }
                            return false;
                        }


                        //reset sms count
                        smsaccess.SendCount += 1;
                        //send sms to the user
                        _context.SMSAccesses.Update(smsaccess);
                        var updateResult = await _context.SaveChangesAsync();
                        if (updateResult > 0)
                        {

                            var IsSent = await ExecuteBarqSMS(PhoneNumber, Message);

                            if (IsSent)
                            {
                                var otp = new OTPCode
                                {
                                    Code= Message,
                                    PhoneNumber= PhoneNumber,
                                    Source = Source,
                                    CreatedAt= DateTime.Now,
                                    ModifiedAt= DateTime.Now,
                                };

                                await _context.OTPCodes.AddAsync(otp);
                                var result = await _context.SaveChangesAsync();
                                return true;
                            }
                            else
                            {
                                _logger.LogInformation("Failed to send an sms to {phoneNumber}", PhoneNumber);
                                return false;
                            }




                        }
                        return false;
                    }





                }
                else
                {



                    smsaccess = new SMSAccess
                    {
                        PhoneNumber= PhoneNumber,
                        CreatedAt= DateTime.Now,
                        IsBlocked  = false,
                        SendCount= 1,
                        BlockCounts = 0
                    };

                    await _context.SMSAccesses.AddAsync(smsaccess);
                    var addResult = await _context.SaveChangesAsync();

                    if (addResult > 0)
                    {
                        var IsSent = await ExecuteBarqSMS(PhoneNumber, Message);

                        if (IsSent)
                        {
                            var otp = new OTPCode
                            {
                                Code= Message,
                                PhoneNumber= PhoneNumber,
                                Source = Source,
                                CreatedAt= DateTime.Now,
                                ModifiedAt= DateTime.Now,
                            };

                            await _context.OTPCodes.AddAsync(otp);
                            var result = await _context.SaveChangesAsync();
                            return true;
                        }
                        else
                        {
                            _logger.LogInformation("Failed to send an sms to {phoneNumber}", PhoneNumber);
                            return false;
                        }
                    }
                    else
                    {
                        return false;

                    }



                }

                return false;



            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw;
            }

        }



        private async Task<bool> SendWhatsAppMessage(string To, string FullName, string OTP)
        {
            try
            {


                var IsSent = await ExecuteWhatsAppBot(To, FullName, OTP);
                                

              

                if (IsSent)
                {
                    var otp = new OTPCode
                    {
                        Code= OTP,
                        PhoneNumber= To,
                        Source = "WhatsApp",
                        CreatedAt= DateTime.Now,
                        ModifiedAt= DateTime.Now,
                    };

                    await _context.OTPCodes.AddAsync(otp);
                    var result = await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _logger.LogInformation("Failed to send a whatsapp message to {phoneNumber}", To);

                    return false;
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw;
            }

        }



        private async Task<bool> SendWhatsAppMessageWithLimit(string PhoneNumber, string FullName, string OTP)
        {
            try
            {

                //Get current count of SMS sent to this number
                var smsaccess = await _context.SMSAccesses.FirstOrDefaultAsync(s => s.PhoneNumber == PhoneNumber);
                //incase we sent any sms to this user
                if (smsaccess != null)
                {
                    // check if phone has been blocked two times then block it foreever
                    if (smsaccess.BlockCounts >= 2)
                    {
                        return false;
                    }
                    //if phone number is blocked
                    if (smsaccess.IsBlocked)
                    {
                        //check if the user should be unblocked by the system
                        var TimeDifference = DateTime.Now.Subtract(smsaccess.LockedAt);
                        //Unblocking a user
                        if (TimeDifference > TimeSpan.FromHours(24))
                        {
                            //Unblock the user
                            smsaccess.IsBlocked = false;
                            //reset sms count
                            smsaccess.SendCount = 0;
                            //send sms to the user
                            _context.SMSAccesses.Update(smsaccess);
                            var updateResult = await _context.SaveChangesAsync();
                            if (updateResult > 0)
                            {
                                bool IsSent = false;

                                //This is were a lot of SMSs being sent to the users
                                IsSent = await ExecuteWhatsAppBot(PhoneNumber, FullName, OTP);

                                if (IsSent)
                                {
                                    var otp = new OTPCode
                                    {
                                        Code= OTP,
                                        PhoneNumber= PhoneNumber,
                                        Source = "WhatsApp",
                                        CreatedAt= DateTime.Now,
                                        ModifiedAt= DateTime.Now,
                                    };

                                    await _context.OTPCodes.AddAsync(otp);
                                    var result = await _context.SaveChangesAsync();
                                    return true;
                                }
                                else
                                {
                                    _logger.LogInformation("Failed to send an sms to {phoneNumber}", PhoneNumber);

                                    return false;
                                }




                            }
                            return false;

                        }
                        return false;

                    }
                    if (!smsaccess.IsBlocked)
                    {


                        if (smsaccess.SendCount == 5)
                        {
                            smsaccess.IsBlocked = true;
                            smsaccess.LockedAt = DateTime.Now;
                            smsaccess.BlockCounts += 1;
                            _context.SMSAccesses.Update(smsaccess);
                            var result = await _context.SaveChangesAsync();
                            if (result > 0)
                            {
                                _logger.LogError("Failed to save sms access update for user blocking");
                                return false;
                            }
                            return false;
                        }


                        //reset sms count
                        smsaccess.SendCount += 1;
                        //send sms to the user
                        _context.SMSAccesses.Update(smsaccess);
                        var updateResult = await _context.SaveChangesAsync();
                        if (updateResult > 0)
                        {

                            var IsSent = await ExecuteWhatsAppBot(PhoneNumber, FullName, OTP);

                            if (IsSent)
                            {
                                var otp = new OTPCode
                                {
                                    Code= OTP,
                                    PhoneNumber= PhoneNumber,
                                    Source = "WhatsApp",
                                    CreatedAt= DateTime.Now,
                                    ModifiedAt= DateTime.Now,
                                };

                                await _context.OTPCodes.AddAsync(otp);
                                var result = await _context.SaveChangesAsync();
                                return true;
                            }
                            else
                            {
                                _logger.LogInformation("Failed to send an sms to {phoneNumber}", PhoneNumber);
                                return false;
                            }




                        }
                        return false;
                    }





                }
                else
                {



                    smsaccess = new SMSAccess
                    {
                        PhoneNumber= PhoneNumber,
                        CreatedAt= DateTime.Now,
                        IsBlocked  = false,
                        SendCount= 1,
                        BlockCounts = 0
                    };

                    await _context.SMSAccesses.AddAsync(smsaccess);
                    var addResult = await _context.SaveChangesAsync();

                    if (addResult > 0)
                    {
                        var IsSent = await ExecuteWhatsAppBot(PhoneNumber, FullName, OTP);

                        if (IsSent)
                        {
                            var otp = new OTPCode
                            {
                                Code= OTP,
                                PhoneNumber= PhoneNumber,
                                Source = "WhatsApp",
                                CreatedAt= DateTime.Now,
                                ModifiedAt= DateTime.Now,
                            };

                            await _context.OTPCodes.AddAsync(otp);
                            var result = await _context.SaveChangesAsync();
                            return true;
                        }
                        else
                        {
                            _logger.LogInformation("Failed to send an sms to {phoneNumber}", PhoneNumber);
                            return false;
                        }
                    }
                    else
                    {
                        return false;

                    }



                }

                return false;



            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw;
            }

        }





        private async Task<bool> SendBulk(List<string> phoneNumbers, string Message, string Source)
        {
            try
            {

                var isSent = await ExecuteNilogyBulkSMS(phoneNumbers, Message);
                if (isSent)
                {
                    var otp = new OTPCode
                    {
                        Code= Message,
                        PhoneNumber= string.Join(",", phoneNumbers),
                        Source = Source,
                        CreatedAt= DateTime.Now,
                        ModifiedAt= DateTime.Now,
                    };

                    await _context.OTPCodes.AddAsync(otp);
                    var result = await _context.SaveChangesAsync();
                    return true;
                }
                return false;

            }
            catch (Exception)
            {

                throw;
            }
        }



        private async Task<bool> ExecuteNilogySMS(string phoneNumber, string message)
        {

            try
            {

                var uri = $"";

                HttpResponseMessage response = await client.GetAsync(uri);
                string responseBody = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseBody);
                var SMSResponse = JsonConvert.DeserializeObject<BotSailorResponse>(responseBody);
                if (SMSResponse != null)
                {
                    if (SMSResponse.Status!.Equals("1"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                throw;
            }

        }


        private async Task<bool> ExecuteBarqSMS(string phoneNumber, string message)
        {

            try
            {
                var authToken = "11|lztDoMxzMfbkuNAL8nCFAGdftW0CtkrHxiU0qIBAbf1c0331";
                var uri = $"https://dash.brqsms.com/api/v3/sms/send";


                // Data object to send
                var data = new
                {
                    recipient = "249" + phoneNumber,
                    sender_id = "SmartOTP",
                    type = "plain",
                    message = "Your OTP Code is: " + message,
                };

                // Convert object to JSON
                var json = System.Text.Json.JsonSerializer.Serialize(data);

                // Create content with JSON format and UTF-8 encoding
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
                client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", authToken);

                HttpResponseMessage response = await client.PostAsync(uri, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseBody);
                var SMSResponse = JsonConvert.DeserializeObject<BarqSMSResponse>(responseBody);
                if (SMSResponse != null)
                {
                    if (SMSResponse.Status!.Equals("success"))
                    {
                        _logger.LogInformation("SMS sent successfully to {phoneNumber}", phoneNumber);
                        _logger.LogInformation("SMS Response: {@response}", SMSResponse);
                        return true;
                    }
                    else
                    {
                        _logger.LogError("Failed to send SMS to {phoneNumber}. Response: {@response}", phoneNumber, SMSResponse);
                        _logger.LogError("SMS Response: {@response}", SMSResponse);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError("Error while sending SMS: {@ex}", e);
                throw;
            }

        }



        private async Task<bool> ExecuteNilogyBulkSMS(List<string> phoneNumbers, string message)
        {

            try
            {


                var uri = $"http://smss.nilogy.com/app/gateway/gateway.php?sendmessage=1&username=sinnaruniversityapi&password=sinnaruniversityapi%2387&text={message}&numbers={phoneNumbers}&sender=Sinnar Univ";

                HttpResponseMessage response = await client.GetAsync(uri);
                string responseBody = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseBody);
                var SMSResponse = JsonConvert.DeserializeObject<NilogySMSResponse>(responseBody);
                if (SMSResponse != null)
                {
                    if (SMSResponse.Success)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;


            }
            catch (Exception e)
            {
                throw;
            }

        }

        private async Task<bool> ExecuteSMS(string PhoneNumber, string Message)
        {
            try
            {
                var uri = $"http://212.0.129.229/bulksms/webacc.aspx?user=UOFS&pwd=13123&smstext={Message}&Sender=UOFS&Nums=249{PhoneNumber}";
                HttpResponseMessage response = await client.GetAsync(uri);
                string responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody == "OK")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{@ex}", ex);
                throw;
            }

        }


        private async Task<bool> ExecuteWhatsAppBot(string To, string FullName, string OTP)
        {

            try
            {
                using (HttpClient client = new())
                {
                    _logger.LogInformation("Httpclient is ready");

                    // Read the value of the environment variable
                    string token = Environment.GetEnvironmentVariable(WhatsAppBotToken) ?? "EAATZBbQFPq0kBOzFICZAXpRx0YZAfM2RolZAqBYFQpdkopW3gnW2dWWdZBYfsTLukcddBiIbAFR7rHixOGu9m9ZB9aJDES60MjD3jLdlSXXeeZBEYsavBw1tuHtXe3rkcGm4JkLvpgmNj1YAQ9xvQddENbDmH5d66vzkaz9KJnwTioIgExhOe4aZBYZBOdnSpAlui";


                    if (string.IsNullOrEmpty(token)) {
                        _logger.LogInformation("WhatsApp token is empty");

                        return false;
                    }
                    _logger.LogInformation("WhatsApp token is set, continuing with sending the OTP...");

                    // Create a new request message
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v21.0/532116629987639/messages");

                    // Add headers
                    request.Headers.Add("Authorization", $"Bearer {token}");

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    string jsonContent = System.Text.Json.JsonSerializer.Serialize(BuildOTPTemplateRequestBody(To,FullName,OTP), options);
                    request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    _logger.LogInformation($"WhatsApp request body: {jsonContent}");

                    // Send the request
                    HttpResponseMessage response = await client.SendAsync(request);
                    _logger.LogInformation($"{response.ToString()}");
                    _logger.LogInformation($"response status code: {response.StatusCode}");
                    _logger.LogInformation($"response body: {response.Content}");

                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Response Body: " + responseContent);
                        return true;
                    }
                    else
                    {
                        _logger.LogInformation("Status Code: " + response.StatusCode);
                        _logger.LogInformation("Response Body: " + responseContent);

                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"{ex}");
                throw ex;
            }

        }


        private WhatsAppOTPMessageDTO BuildOTPTemplateRequestBody(string To, string FullName, string OTPCode)
        {

            WhatsAppOTPMessageDTO message = new WhatsAppOTPMessageDTO
            {
                MessagingProduct = "whatsapp",
                RecipientType = "individual",
                To = $"249{To}",
                Type = "template",
                Template = new Template
                {
                    Name = "tala_order_code",
                    Language = new Language
                    {
                        Code = "en"
                    },
                    Components = new List<DTOs.Component>
        {
            new() {
                Type = "body",
                Parameters = new List<Parameter>
                {
                    new() { Type = "text", Text = FullName },
                    new() { Type = "text", Text = OTPCode }
                }
            }
        }
                }
            };

            return message;

            //string json = System.Text.Json.JsonSerializer.Serialize(message, new JsonSerializerOptions { WriteIndented = true });

        }


    }



}
