﻿using Filuet.Infrastructure.Abstractions.Enums;
using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class KioskDto
    {
        [JsonPropertyName("uid")]
        /// <summary>
        /// Unique identifier
        /// </summary>
        public string UID { get; set; }
        [JsonPropertyName("languages")]
        public IEnumerable<Language> Languages { get; set; }
        [JsonPropertyName("idleTimeout")]
        public TimeSpan IdleTimeout { get; set; }
        [JsonPropertyName("productLinks")]
        public IEnumerable<KioskProductLink> ProductLinks { get; set; }
    }
}