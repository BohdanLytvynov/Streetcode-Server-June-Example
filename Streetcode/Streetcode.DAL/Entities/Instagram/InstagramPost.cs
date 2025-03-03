﻿using Streetcode.DAL.Entities.Base;
using System.Text.Json.Serialization;

namespace Streetcode.DAL.Entities.Instagram
{
    public class InstagramPost : IEntityId<string>
    {
        /// <summary>
        /// The Media's caption text. Not returnable for Media in albums.
        /// </summary>
        [JsonPropertyName("caption")]
        public string Caption { get; set; } = string.Empty;

        /// <summary>
        /// The Media's ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The Media's type. Can be IMAGE, VIDEO, or CAROUSEL_ALBUM.
        /// </summary>
        [JsonPropertyName("media_type")]
        public string MediaType { get; set; } = string.Empty;

        /// <summary>
        /// The Media's URL.
        /// </summary>
        [JsonPropertyName("media_url")]
        public string MediaUrl { get; set; } = string.Empty;

        /// <summary>
        /// The Media's permanent URL.
        /// </summary>
        [JsonPropertyName("permalink")]
        public string Permalink { get; set; } = string.Empty;

        /// <summary>
        /// The Media's thumbnail image URL. Only available on VIDEO Media.
        /// </summary>
        [JsonPropertyName("thumbnail_url")]
        public string ThumbnailUrl { get; set; } = string.Empty;

        [JsonPropertyName("is_pinned")]
        public bool IsPinned { get; set; }
    }
}
