namespace MapperEmit {
    public class Person {
        [ToMapAttribute]
        public string Name { get; set; }

        [ToMapAttribute]
        public string _Name;

        [ToMapAttribute]
        public int Id { get; set; }

        [ToMapAttribute]
        public int _Id;

        public Subject[] Subjects { get; set; }

        [ToMap]
        public Organization _Org;
    }
}
