﻿namespace Registeration.Main.Application.Helpers
{
    public class Response<T>
        where T : class
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
