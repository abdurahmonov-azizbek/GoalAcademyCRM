﻿namespace GoalAcademyCRM.Api.Services.Helpers.Auth.Models
{
    public class JwtSettings
    {
        public bool ValidateIssuer { get; set; }
        public string ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateLifeTime { get; set; }
        public int LifeTimeInMinutes { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public string SecretKey { get; set; }
    }
}
