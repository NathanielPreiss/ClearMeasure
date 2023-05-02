namespace ClearMeasure.Tests;

[TestClass]
public class ValueMappingTests
{
    private readonly AutoMock _mock;

    public ValueMappingTests()
    {
        _mock = AutoMock.GetLoose();
    }

    [DataTestMethod]
    [DynamicData(nameof(AdditionData))]
    public void MapToUpperLimit(Dictionary<int, string> map, int upperLimit)
    {
        // Arrange
        var unitUnderTest = _mock.Create<ValueMapping>(new TypedParameter(typeof(IDictionary<int, string>), map));
        
        // Act
        var result = unitUnderTest.MapToUpperLimit(upperLimit, true).ToList();

        // Assert
        Assert.AreEqual(upperLimit, result.Count);
        foreach (var (key, value) in map)
        {
            CollectionAssert.Contains(result[key-1].ToList(), value);
        }

        if (map.Count > 1)
        {
            var overlapCheck = map.Take(2).ToList();
            var index = overlapCheck[0].Key * overlapCheck[1].Key;
            CollectionAssert.Contains(result[index-1].ToList(), overlapCheck[0].Value);
            CollectionAssert.Contains(result[index-1].ToList(), overlapCheck[1].Value);
        }
    }

    [DataTestMethod]
    [DynamicData(nameof(AdditionData))]
    public void GetMatches(Dictionary<int, string> map, int _)
    {
        // Arrange
        var unitUnderTest = _mock.Create<ValueMapping>(new TypedParameter(typeof(IDictionary<int, string>), map));

        // Act
        foreach (var (key, value) in map)
        {
            // Assert
            var result = unitUnderTest.GetMatches(key).ToList();
            Assert.IsTrue(result.Count > 0);
            CollectionAssert.Contains(result, value);
        }
    }

    public static IEnumerable<object[]> AdditionData
    {
        get
        {
            return new[]
            {
                new object[] { new Dictionary<int, string> { {3, "Ricky"}, {5, "Bobby"} }, 100 },
                new object[] { new Dictionary<int, string> { {2, "Ricky"}, {4, "Bobby"}, {6, "Timmy"} }, 1000 }
            };
        }
    }

}