namespace JeeLee.DataStore
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
            OnSave(out bool result);
            return result;
        }

        public bool Load()
        {
            OnLoad(out bool result);
            return result;
        }

        public bool Delete()
        {
            OnDelete(out bool result);
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
        /// <param name="success">Sets the result of the process success. Is used in the Save() method.</param>
        protected abstract void OnSave(out bool success);

        /// <summary>
        /// Abstract method used by Load(), use this method to load the data
        /// </summary>
        /// <param name="success">Sets the result of the process success. Is used in the Load() method.</param>
        protected abstract void OnLoad(out bool success);

        /// <summary>
        /// Abstract method used by Load(), use this method to delete the data
        /// </summary>
        /// <param name="success">Sets the result of the process success. Is used in the Delete() method.</param>
        protected abstract void OnDelete(out bool success);

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