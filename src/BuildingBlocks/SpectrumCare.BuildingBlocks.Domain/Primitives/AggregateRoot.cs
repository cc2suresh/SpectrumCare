using SpectrumCare.BuildingBlocks.Domain.Events;

namespace SpectrumCare.BuildingBlocks.Domain.Primitives;

public abstract class AggregateRoot : BaseEntity
{
    private int _version = 0;

    protected AggregateRoot(Guid id) : base(id)
    {
    }

    protected AggregateRoot()
    {
    }

    public int Version => _version;

    public void IncrementVersion()
    {
        _version++;
    }
}