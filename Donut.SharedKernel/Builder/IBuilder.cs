namespace Donut.SharedKernel.Builder;

public interface IBuilder<BuildableType> 
            where BuildableType : class
{
    BuildableType Build();
}
