using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HSR.CloudSolutions.SmartKitchen.UI.Templates
{
    public abstract class DataTemplateSelectorCollection : DataTemplateSelector, IList<DataTemplate>, IList
    {
        private readonly List<DataTemplate> _dataTemplates = new List<DataTemplate>();

        public IEnumerable<DataTemplate> DataTemplates => this._dataTemplates;


        public IEnumerator<DataTemplate> GetEnumerator()
        {
            return this._dataTemplates.GetEnumerator();
        }

        public void Add(DataTemplate item)
        {
            this.InternalAdd(item);
        }

        private int InternalAdd(DataTemplate item)
        {
            if (this.Contains(item))
            {
                return -1;
            }
            this._dataTemplates.Add(item);
            return this._dataTemplates.Count() - 1;
        }
        
        public void Clear()
        {
            this._dataTemplates.Clear();
        }

        public bool Contains(DataTemplate item)
        {
            return this._dataTemplates.Contains(item);
        }

        public void CopyTo(DataTemplate[] array, int arrayIndex)
        {
            this._dataTemplates.CopyTo(array, arrayIndex);
        }

        public bool Remove(DataTemplate item)
        {
            return this._dataTemplates.Remove(item);
        }

        public int Count => this._dataTemplates.Count;

        public bool IsReadOnly => ((IList)this._dataTemplates).IsReadOnly;

        public object SyncRoot => ((ICollection)this._dataTemplates).SyncRoot;
        public bool IsSynchronized => ((ICollection) this._dataTemplates).IsSynchronized;
        
        public int IndexOf(DataTemplate item)
        {
            return this._dataTemplates.IndexOf(item);
        }

        public void Insert(int index, DataTemplate item)
        {
            this._dataTemplates.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this._dataTemplates.RemoveAt(index);
        }

        public DataTemplate this[int index]
        {
            get { return this._dataTemplates[index]; }
            set { this._dataTemplates[index] = value; }
        }

        private DataTemplate Cast(object value)
        {
            var template = value as DataTemplate;
            if (template == null)
            {
                throw new ArgumentException($"DataTemplateSelectorCollection can only work with DataTemplates but it was tried to add a '{value?.GetType().FullName}' to the collection.");
            }
            return template;
        }

        #region IList

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            Array.Copy(this._dataTemplates.ToArray(), 0, array, index, this._dataTemplates.Count());
        }

        int IList.Add(object value)
        {
            return this.InternalAdd(this.Cast(value));
        }

        bool IList.Contains(object value)
        {
            return this.Contains(this.Cast(value));
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf(this.Cast(value));
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, this.Cast(value));
        }

        void IList.Remove(object value)
        {
            this.Remove(this.Cast(value));
        }

        bool IList.IsFixedSize => false;

        object IList.this[int index]
        {
            get { return this[index]; }
            set { this[index] = this.Cast(value); }
        }

        #endregion
    }
}
