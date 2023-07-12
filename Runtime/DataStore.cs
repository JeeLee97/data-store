using System;

namespace DataStore
{
    public abstract class DataStore<T> : IDataStore<T>
        where T : class, new()
    {
        /// <summary>
        /// The data set used in by store.
        /// </summary>
        public T Data { get; protected set; }

        /// <param name="loadOnInitialize">Determines if the data should be loaded on initialize.</param>
        protected DataStore(bool loadOnInitialize)
        {
            if (!loadOnInitialize || !Load())
            {
                Clear();
            }
        }

        public bool Save()
        {
            bool result = true;
            OnSave(b => result = b);
            return result;
        }

        public bool Load()
        {
            bool result = true;
            OnLoad(b => result = b);
            return result;
        }

        public bool Delete()
        {
            bool result = true;
            OnDelete(b => result = b);
            return result;
        }

        public void Clear(bool persist = false)
        {
            OnClear();

            if (persist)
            {
                Save();
            }
        }

        /// <summary>
        /// Abstract method used by Save(), use this method to store the data.
        /// Override this method with your own implementation.
        /// </summary>
        /// <param name="callback">Callback used to set the return value of Save().</param>
        protected abstract void OnSave(Action<bool> callback);

        /// <summary>
        /// Abstract method used by Load(), use this method to load the data
        /// </summary>
        /// <param name="callback">Callback used to set the return value of Load().</param>
        protected abstract void OnLoad(Action<bool> callback);

        /// <summary>
        /// Abstract method used by Load(), use this method to delete the data
        /// </summary>
        /// <param name="callback">Callback used to set the return value of Delete().</param>
        protected abstract void OnDelete(Action<bool> callback);

        /// <summary>
        /// Virtual method called by Clear(),
        /// override this method if you need some custom implementation of clearing your data set.
        /// </summary>
        protected virtual void OnClear()
        {
            Data = new T();
        }
    }
}