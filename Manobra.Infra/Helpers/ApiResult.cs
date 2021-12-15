using System;
using System.Collections.Generic;
using System.Text;

namespace Manobra.Infra.Helpers
{
  public class ApiResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }      

        public static ApiResult New(bool success, string message, Exception ex = null)
        {
            var result = new ApiResult { Success = success, Message = message };
            //if (success)
            //    Log.Trace(message);
            //else
            //{
            //    if (ex != null)
            //        Log.Error(ex, message);
            //    else
            //        Log.Error(message);
            //}
            return result;
        }
    }
}
