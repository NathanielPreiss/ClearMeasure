namespace ClearMeasure;

public class LibraryModule : Module
{
    private readonly IEnumerable<Tuple<int, string>>? _valueMap;

    public LibraryModule(IEnumerable<Tuple<int, string>>? valueMap)
    {
        _valueMap = valueMap;
    }
    protected override void Load(ContainerBuilder builder)
    {
        if (_valueMap == null || !_valueMap.Any())
        {
            throw new Exception("Registration Failure: Mapping values could not be determined.");
        }

        var mappingDictionary = _valueMap
            .DistinctBy(x => x.Item1)
            .ToDictionary(x => x.Item1, x => x.Item2);

        builder.RegisterInstance(mappingDictionary)
            .As<IDictionary<int, string>>();

        builder.RegisterAssemblyTypes(typeof(LibraryModule).Assembly).AsImplementedInterfaces();
    }
}
