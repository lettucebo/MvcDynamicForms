using System;
using System.Collections.Generic;
using System.Linq;
using MvcDynamicForms.NetCore.Exceptions;
using MvcDynamicForms.NetCore.Fields.Abstract;

namespace MvcDynamicForms.NetCore
{
    /// <summary>
    /// A collection of Field objects.
    /// </summary>
    [Serializable]
    public class FieldList : IList<Field>
    {
        private List<Field> _fields = new List<Field>();
        internal Form Form { get; set; }

        internal FieldList(Form form)
        {
            this.Form = form;
        }

        internal void ValidateKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Field keys cannot be null nor empty.");

            if (this._fields.Select(x => x.Key).Contains(key))
                throw new DuplicateException(string.Format(@"The key ""{0}"" is in use by another field.", key));
        }

        #region IList<Field> Members

        public int IndexOf(Field item)
        {
            return this._fields.IndexOf(item);
        }

        public void Insert(int index, Field item)
        {
            this.ValidateKey(item.Key);
            item.Form = this.Form;
            this._fields.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this._fields[index].Form = null;
            this._fields.RemoveAt(index);
        }

        public Field this[int index]
        {
            get { return this._fields[index]; }
            set
            {
                this.ValidateKey(value.Key);
                this._fields[index] = value;
            }
        }

        #endregion

        #region ICollection<Field> Members

        public void Add(Field item)
        {
            this.ValidateKey(item.Key);
            item.Form = this.Form;
            this._fields.Add(item);
        }

        public void Clear()
        {
            this._fields.ForEach(x => x.Form = null);
            this._fields.Clear();
        }

        public bool Contains(Field item)
        {
            return this._fields.Contains(item);
        }

        public void CopyTo(Field[] array, int arrayIndex)
        {
            this._fields.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this._fields.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Field item)
        {
            item.Form = null;
            return this._fields.Remove(item);
        }

        #endregion

        #region IEnumerable<Field> Members

        public IEnumerator<Field> GetEnumerator()
        {
            return this._fields.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._fields.GetEnumerator();
        }

        #endregion
    }
}