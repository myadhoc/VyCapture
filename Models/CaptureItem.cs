using System;

namespace Viadivy.Tools.VyCapture.Models
{
    public class CaptureItem
    {
        public long Id { get; set; }

        public string? Title { get; set; }

        public string Content { get; set; } =
            string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}