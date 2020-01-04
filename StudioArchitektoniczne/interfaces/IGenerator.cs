namespace ArchitecturalStudio.interfaces
{
    public interface IGenerator
    {
        void Generate(int amount, params object[] parameters);
    }
}