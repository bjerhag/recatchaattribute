using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RecaptchaAction.Models
{
    public class RecaptchaResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
        [JsonProperty(PropertyName = "challenge_ts")]
        public DateTime ChallengeTimeStamp { get; set; }
        [JsonProperty(PropertyName = "hostname")]
        public string Hostname { get; set; }
        [JsonProperty(PropertyName = "error-codes")]
        public List<string> ErrorCodes { get; set; }
        [JsonProperty(PropertyName = "score")]
        public float Score { get; set; }
        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }
    }
}
