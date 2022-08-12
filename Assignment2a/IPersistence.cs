namespace Assignment2a
{
    public interface IPersistence
    {
        public bool Load(string filename);
        public bool Save(bool appendToFile, string filename);
    }
}
