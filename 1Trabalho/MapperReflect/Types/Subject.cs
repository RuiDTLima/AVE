namespace MapperReflect
{
    public class Subject {
        public string Name { get; set; }
        public int Id { get; set; }

        public Subject(string n, int nr) {
            Name = n;
            Id = nr;
        }

    }
}
