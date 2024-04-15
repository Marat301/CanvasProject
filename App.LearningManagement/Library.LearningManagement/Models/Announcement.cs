﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Models {
    public class Announcement {
        private static int lastID = 0;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int ID {
            get; private set;
        }

        public Announcement() {
            ID = ++lastID;
        }

        public override string ToString() {
            return $"[{ID}] {Name}: {Description}";
        }
    }
}