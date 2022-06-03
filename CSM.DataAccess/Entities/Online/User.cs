﻿using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}