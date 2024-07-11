﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPortal.DAL.Entities
{
    public class Performer
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public virtual ICollection<Song>? Songs { get; set; }
    }
}
