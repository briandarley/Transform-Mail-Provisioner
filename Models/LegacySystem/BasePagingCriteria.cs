namespace TransformNewMailProvisionerData.Models.LegacySystem
{
    public abstract class BasePagingCriteria<T> where T : class
    {
        /// <summary>
        /// The number of records to return
        /// </summary>
        public int? PageSize { get; set; }
        /// <summary>
        /// The page number to return
        /// </summary>
        public int? Index { get; set; }
        /// <summary>
        /// The filter text to apply
        /// </summary>
        public string FilterText { get; set; }
        /// <summary>
        /// The sort to apply
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// The sort direction to apply
        /// </summary>
        //public ListSortDirection? ListSortDirection { get; set; }

    }
}
