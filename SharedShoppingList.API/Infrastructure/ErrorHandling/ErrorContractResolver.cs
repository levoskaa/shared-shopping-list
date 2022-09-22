using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedShoppingList.API.Application.ViewModels;

namespace SharedShoppingList.API.Infrastructure.ErrorHandling;

public class ErrorContractResolver : CamelCasePropertyNamesContractResolver
{
    private readonly bool isDevelopment;

    public ErrorContractResolver(bool isDevelopment)
    {
        this.isDevelopment = isDevelopment;
    }

    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
        var propertiesToExclude = new string[] { nameof(ErrorViewModel.StackTrace) };

        if (!isDevelopment)
        {
            properties = properties.Where(property => !propertiesToExclude.Contains(property.PropertyName)).ToList();
        }
        return properties;
    }
}