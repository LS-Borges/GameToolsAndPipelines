
namespace WeaponLib
{
    public interface IPersistence
    {
        public bool Load(string filename);
        public bool Save(bool appendToFile, string filename);
    }
}
