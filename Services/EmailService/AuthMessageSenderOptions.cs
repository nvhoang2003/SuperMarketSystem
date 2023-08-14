﻿namespace SuperMarketSystem.Services
{
    public class AuthMessageSenderOptions
    {

        public string? DisplayName { get; set; }
        public string? From { get; set; }
        public string? UserName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string? EmailSecurity { get; set; }
        public bool UseSSL { get; set; }
        public bool UseStartTls { get; set; }
    }
}