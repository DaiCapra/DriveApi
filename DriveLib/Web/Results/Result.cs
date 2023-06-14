using System;
using System.Collections.Generic;
using DriveLib.Web.Errors;

namespace DriveLib.Web.Results
{
    public class Result
    {
        public Result()
        {
            Errors = new List<Error>();
        }

        public bool Success { get; set; }
        public bool HasError => Errors.Count > 0;
        public List<Error> Errors { get; set; }

        public void AddException(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            Errors.Add(new Error { Message = exception.Message });
        }
    }
}