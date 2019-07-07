using System;

namespace BlackRose.Domain
{
    public class Picture
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }

        public string Tags { get; set; }
        public DateTime Time { get; set; }

    }
}