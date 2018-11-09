using System;

namespace MvcDynamicForms.NetCore
{
    /// <summary>
    /// Stores arbitrary data.
    /// </summary>
    [Serializable]
    public class DataItem
    {
        public DataItem(object value, bool clientSide)
        {
            this.Value = value;
            this.ClientSide = clientSide;
        }

        /// <summary>
        /// The stored object.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Whether to render the data.
        /// </summary>
        public bool ClientSide { get; set; }
    }
}