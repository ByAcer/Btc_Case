namespace Instruction.Domain.Core
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void Commit();
    }
}
