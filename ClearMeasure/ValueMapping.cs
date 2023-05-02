namespace ClearMeasure;

public interface IValueMapping
{
    public IEnumerable<IEnumerable<string>> MapToUpperLimit(int upperLimit, bool fallbackToValue = true);
}

public class ValueMapping : IValueMapping
{
    private readonly IDictionary<int, string> _valueMap;
    private readonly ILogger<ValueMapping> _logger;

    public ValueMapping(
        IDictionary<int, string> valueMap,
        ILogger<ValueMapping> logger)
    {
        _valueMap = valueMap;
        _logger = logger;
    }

    public IEnumerable<IEnumerable<string>> MapToUpperLimit(int upperLimit, bool fallbackToValue)
    {
        const int lowerLimit = 1;

        if (upperLimit < lowerLimit)
        {
            _logger.LogError("Upper limit {upperLimit} was lower than lower limit {lowerLimit}", upperLimit, lowerLimit);
            throw new ArgumentOutOfRangeException(nameof(upperLimit), upperLimit, $"The upper limit was lower than the starting value of {lowerLimit}.");
        }

        _logger.LogInformation("Mapping values {lowerLimit} to {upperLimit}.", lowerLimit, upperLimit);

        var output = new IEnumerable<string>[upperLimit];
        
        for (var i = lowerLimit; i <= upperLimit; i++)
        {
            var valueResult = GetMatches(i).ToList();

            if (valueResult.Any())
            {
                _logger.LogDebug("Values ({count}) were found for {key}.", valueResult.Count, i);
                output[i - 1] = valueResult;
            }
            else
            {
                if (fallbackToValue)
                {
                    _logger.LogDebug("Value was not found for {key}. Using key value.", i);
                    output[i - 1] = new List<string> { $"{i}" };
                }
                else
                {
                    _logger.LogDebug("Value was not found for {key}.", i);
                    output[i - 1] = valueResult;
                }
            }
        }

        return output;
    }

    public IEnumerable<string> GetMatches(int input)
    {
        _logger.LogDebug("Getting matches for {input}.", input);

        return _valueMap
            .Where(x => input % x.Key == 0)
            .Select(x => x.Value);
    } 
}
