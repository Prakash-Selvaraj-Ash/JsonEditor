using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestWPF
{
    internal class TemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
			FrameworkElement elemnt = container as FrameworkElement;
			Data data = item as Data;
            if (data.ArrayPairs != null && data.ArrayPairs.Count > 0)
            {
                return elemnt.FindResource("InnerTreeView") as DataTemplate;
            }
            if (data.Collection != null && data.Collection.Count > 0)
            {
                return elemnt.FindResource("collectionView") as DataTemplate;
            }

            return elemnt.FindResource("SingleListView") as HierarchicalDataTemplate;
		}
    }
}
