using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using WPG.Data;

namespace WPG
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WpgCustomTemplateAttribute : Attribute
    {
        public string Name;

        public WpgCustomTemplateAttribute(string name)
        {
            Name = name;
        }
    }

	public class PropertyTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			Property property = item as Property;
			if (property == null)
			{
				throw new ArgumentException("item must be of type Property");
			}
			FrameworkElement element = container as FrameworkElement;
			if (element == null)
			{
				return base.SelectTemplate(property.Value, container);
			}
			DataTemplate template = FindDataTemplate(property, element);
			return template;
		}		

		private DataTemplate FindDataTemplate(Property property, FrameworkElement element)
		{
		    DataTemplate template = null;
            
            var attr = property.Attributes[typeof(EditorAttribute)];
		    if (attr != null)
		    {
		        var editorAttr = attr as EditorAttribute;
                template = TryFindDataTemplate(element, editorAttr.EditorTypeName);

                if (template!=null)
                    return template;
		    }

            Type propertyType = property.PropertyType;
            if (property.PropertyType != typeof(string) && GetEnumerableType(property.PropertyType))
                propertyType = typeof(List<object>);
            
			template = TryFindDataTemplate(element, propertyType);

    		while (template == null && propertyType.BaseType != null)
			{
				propertyType = propertyType.BaseType;
                
				template = TryFindDataTemplate(element, propertyType);
			}
			if (template == null)
			{
                
                template = TryFindDataTemplate(element, "default");
			}
			return template;
		}

        static bool GetEnumerableType(Type type)
        {
            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return true;
                }
            }
            return false;
        }

		private static DataTemplate TryFindDataTemplate(FrameworkElement element, object dataTemplateKey)
		{
			object dataTemplate = element.TryFindResource(dataTemplateKey);
			if (dataTemplate == null)
			{
				dataTemplateKey = new ComponentResourceKey(typeof(PropertyGrid), dataTemplateKey);
				dataTemplate = element.TryFindResource(dataTemplateKey);
			}
			return dataTemplate as DataTemplate;
		}
	}
}
