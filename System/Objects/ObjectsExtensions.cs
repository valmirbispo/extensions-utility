using System.Reflection;

namespace ExtensionsUtility.System.Objects
{
    public static class ObjectsExtensions
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static void IncludeChildObjects(this object parentObject, object[] childObjects)
        {
            var publicProperties = parentObject.GetType()
                                            .GetProperties(
                                                BindingFlags.Public
                                                | BindingFlags.Instance);
            var nonPublicProperties = parentObject.GetType()
                                            .GetProperties(
                                                BindingFlags.NonPublic
                                                | BindingFlags.Instance);

            foreach (var propertyInfo in publicProperties.Concat(nonPublicProperties))
            {
                if (!propertyInfo.PropertyType.IsGenericType)
                {
                    var property = childObjects.Where(x => x.IsNotNull()).FirstOrDefault(x => x.GetType() == propertyInfo.PropertyType);
                    if (property.IsNotNull())
                    {
                        propertyInfo.SetValue(parentObject, property);
                    }
                }
            }

            var publicFields = parentObject.GetType()
                                .GetFields(
                                    BindingFlags.Public
                                    | BindingFlags.Instance);

            var nonPublicFields = parentObject.GetType()
                                .GetFields(
                                    BindingFlags.NonPublic
                                    | BindingFlags.Instance);

            foreach (var fieldInfo in publicFields.Concat(nonPublicFields))
            {
                if (!fieldInfo.FieldType.IsGenericType)
                {
                    var field = childObjects.Where(x => x.IsNotNull()).FirstOrDefault(x => x.GetType() == fieldInfo.FieldType);
                    if (field.IsNotNull())
                    {
                        fieldInfo.SetValue(parentObject, field);
                    }
                }
            }
        }
    }
}
