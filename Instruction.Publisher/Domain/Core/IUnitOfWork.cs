﻿namespace Instruction.Publisher.Domain.Core;

public interface IUnitOfWork
{
    Task CommitAsync();
    void Commit();
}
