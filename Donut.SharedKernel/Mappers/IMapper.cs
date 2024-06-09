namespace Donut.SharedKernel.Mappers;

public interface IMapper<T1,T2>
{
    T1 Map(T2 from);
    T2 Map(T1 from);
}
