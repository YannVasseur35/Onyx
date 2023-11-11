namespace Onyx.Core.Interfaces
{
    public interface IAudit
    {
        Guid Id { get; } //Make a private set in class !!!
        DateTime CreatedAt { get; } //Make a private set in class !!!
        DateTime ModifiedAt { get; set; }

        void Init(Guid id, DateTime createdAt);//usefull for tests to set private fields
    }
}