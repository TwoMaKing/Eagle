using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.List
{
    public class NotifyCollectionChangedEventArgs : EventArgs
    {
        /// <summary>
        ///  Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs
        ///  class that describes a System.Collections.Specialized.NotifyCollectionChangedAction.Reset
        ///  change.
        /// </summary>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action) 
        {
        
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems) 
        {
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem) 
        {
        
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems) 
        {
        
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex) 
        {
            this.Action = action;
            this.NewItems = changedItems;
            this.NewStartingIndex = startingIndex;
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index) 
        {
        
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem) 
        {
        
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex) 
        {
            this.Action = action;
            this.NewItems = newItems;
            this.OldItems = oldItems;
            this.NewStartingIndex = startingIndex;
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex) 
        {
            this.Action = action;
            this.NewItems = changedItems;
            this.NewStartingIndex = index;
            this.OldStartingIndex = oldIndex;
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex) 
        {
        
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index) 
        {
            
        }


        /// <summary>
        ///  Gets the action that caused the event.
        /// </summary>
        public NotifyCollectionChangedAction Action { get; private set; }

        /// <summary>
        /// Gets the list of new items involved in the change.
        /// </summary>
        public IList NewItems { get; private set; }

        /// <summary>
        ///  Gets the index at which the change occurred.
        /// </summary>
        public int NewStartingIndex { get; private set; }

        /// <summary>
        ///  Gets the list of items affected by a System.Collections.Specialized.NotifyCollectionChangedAction.Replace,
        //   Remove, or Move action.
        /// </summary>
        public IList OldItems { get; private set; }

        /// <summary>
        /// Gets the index at which a System.Collections.Specialized.NotifyCollectionChangedAction.Move,
        //  Remove, or Replace action occurred.
        /// </summary>
        public int OldStartingIndex { get; private set; }
    }
}
