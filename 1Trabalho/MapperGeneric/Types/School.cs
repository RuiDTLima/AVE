﻿namespace MapperGeneric {
    public class School {
        public string Location { get; set; }

        [ToMap]
        public int[] MembersIds { get; set; }

        [ToMap]
        public string Name { get; set; }
    }
}
