using System.Text.Json.Nodes;

namespace GridSystem.Domain.Extensions;

public static partial class JsonExtensions
{
    const string Id = "$id";
    const string Ref = "$ref";
    
    static bool DefaultIsTypeDiscriminator(string s) => s == "$type";
    
    public static TJsonNode? MoveMetadataToBeginning<TJsonNode>(this TJsonNode? node) where TJsonNode : JsonNode => node.MoveMetadataToBeginning(DefaultIsTypeDiscriminator);

    public static TJsonNode? MoveMetadataToBeginning<TJsonNode>(this TJsonNode? node, Predicate<string> isTypeDiscriminator) where TJsonNode : JsonNode
    {
        ArgumentNullException.ThrowIfNull(isTypeDiscriminator);
        foreach (var n in node.DescendantsAndSelf().OfType<JsonObject>())
        {
            var properties = n.ToLookup(p => isTypeDiscriminator(p.Key) || p.Key == Id || p.Key == Ref);
            var newProperties = properties[true].Concat(properties[false]).ToList();
            n.Clear();
            newProperties.ForEach(p => n.Add(p));
        }
        return node;
    } 
    
    // From this answer https://stackoverflow.com/a/73887518/3744182
    // To https://stackoverflow.com/questions/73887517/how-to-recursively-descend-a-system-text-json-jsonnode-hierarchy-equivalent-to
    public static IEnumerable<JsonNode?> Descendants(this JsonNode? root) => root.DescendantsAndSelf(false);

    /// Recursively enumerates all JsonNodes in the given JsonNode object in document order.
    public static IEnumerable<JsonNode?> DescendantsAndSelf(this JsonNode? root, bool includeSelf = true) => 
        root.DescendantItemsAndSelf(includeSelf).Select(i => i.node);
    
    /// Recursively enumerates all JsonNodes (including their index or name and parent) in the given JsonNode object in document order.
    public static IEnumerable<(JsonNode? node, int? index, string? name, JsonNode? parent)> DescendantItemsAndSelf(this JsonNode? root, bool includeSelf = true) => 
        RecursiveEnumerableExtensions.Traverse(
            (node: root, index: (int?)null, name: (string?)null, parent: (JsonNode?)null),
            (i) => i.node switch
            {
                JsonObject o => o.AsDictionary().Select(p => (p.Value, (int?)null, p.Key.AsNullableReference(), i.node.AsNullableReference())),
                JsonArray a => a.Select((item, index) => (item, index.AsNullableValue(), (string?)null, i.node.AsNullableReference())),
                _ => i.ToEmptyEnumerable(),
            }, includeSelf);
    
    static IEnumerable<T> ToEmptyEnumerable<T>(this T item) => Enumerable.Empty<T>();
    static T? AsNullableReference<T>(this T item) where T : class => item;
    static Nullable<T> AsNullableValue<T>(this T item) where T : struct => item;
    static IDictionary<string, JsonNode?> AsDictionary(this JsonObject o) => o;
}