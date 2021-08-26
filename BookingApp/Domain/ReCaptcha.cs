using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

public static class ReCaptcha
{
    public static string RecaptchaSiteKey
    {
        get
        {
            return "6LdoOcYUAAAAAL1Pjy9C4Piq2ToYNcvsr4ZTgqPn";
        }
    }
    public static string RecaptchaSecretKey
    {
        get
        {
            return "6LdoOcYUAAAAACZa6Mt02ny15mjiiMw6a4CVNMK0";
        }
    }

    public static CaptchaResponse ValidateCaptcha(string response)
    {
        string secret = RecaptchaSecretKey;
        string URI = "https://www.google.com/recaptcha/api/siteverify";
        string myParameters = string.Format("secret={0}&response={1}", secret, response);

        var client = new WebClient();
        client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        var jsonResult = client.UploadString(URI, myParameters);
        return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());

    }
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success
        {
            get;
            set;
        }
        [JsonProperty("error-codes")]
        public List<string> ErrorMessage
        {
            get;
            set;
        }
    }
}