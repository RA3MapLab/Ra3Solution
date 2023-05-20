using System.Text;
using Newtonsoft.Json;

namespace MapCoreLib.Util
{
    public class Result
    {
        public int code { get; set; }
        public string msg { get; set; }

        public static Result success(string msg)
        {
            return new Result()
            {
                code = 0,
                msg = msg
            };
        }
        
        public static Result error(int code, string msg)
        {
            return new Result()
            {
                code = code,
                msg = msg
            };
        }
        
        public static Result defaultError(string msg)
        {
            return new Result()
            {
                code = -1,
                msg = msg
            };
        }
        
        public static string successJson(string msg)
        {
            // return JsonConvert.SerializeObject(success(msg));
            // Convert the JSON string to a Unicode string
            byte[] utfBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(success(msg)));
            byte[] unicodeBytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utfBytes);
            return Encoding.Unicode.GetString(unicodeBytes);
        }
        
        public static string successJson()
        {
            return JsonConvert.SerializeObject(success(""));
        }
        
        public static string errorJson(int code, string msg)
        {
            return JsonConvert.SerializeObject(error(code, msg));
            // // Convert the JSON string to a Unicode string
            // byte[] utfBytes = Encoding.UTF8.GetBytes(res);
            // byte[] unicodeBytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utfBytes);
            // return Encoding.Unicode.GetString(unicodeBytes);
        }
        
        public static string defaultErrorJson(string msg)
        {
            return JsonConvert.SerializeObject(defaultError(msg));
        }
    }
}