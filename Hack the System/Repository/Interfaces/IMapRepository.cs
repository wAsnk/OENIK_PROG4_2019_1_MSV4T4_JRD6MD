// <copyright file="IMapRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Activities of maprepository
    /// </summary>
    public interface IMapRepository
    {
        /// <summary>
        /// Return all map from file
        /// </summary>
        /// <returns>List of maps</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed")]
        IQueryable<IMapDatas> GetAll();
    }
}
