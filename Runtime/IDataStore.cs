namespace JeeLee.DataStore
{
    public interface IDataStore<out T>
        where T : class, new()
    {
        /// <summary>
        /// The data set used in by store.
        /// </summary>
        T Data { get; }

        /// <summary>
        /// This method is used for saving data to the store.
        /// </summary>
        /// <returns>A boolean if the action was successfull.</returns>
        bool Save();

        /// <summary>
        /// This method is used for loading data from the store.
        /// </summary>
        /// <returns>A boolean if the action was successfull.</returns>
        bool Load();

        /// <summary>
        /// This method is used for deleting data on the store.
        /// </summary>
        /// <returns>A boolean if the action was successfull.</returns>
        bool Delete();

        /// <summary>
        /// Resets the data back to its default state.
        /// </summary>
        /// <param name="persist">Should the data be persisted to the store after clearing.</param>
        void Clear(bool persist);
    }
}