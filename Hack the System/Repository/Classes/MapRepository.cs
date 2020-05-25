// <copyright file="MapRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Repository.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Xml.Linq;
    using Repository.Interfaces;

    /// <summary>
    /// This class handle map file
    /// </summary>
    public class MapRepository : IMapRepository
    {
        private readonly string url = "../../Resources/Maps.xml";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRepository"/> class.
        /// </summary>
        public MapRepository()
        {
            this.Document = XDocument.Load(this.url);
        }

        private XDocument Document { get; set; }

        /// <inheritdoc/>
        public IQueryable<IMapDatas> GetAll()
        {
            return this.Document.Element("MapsFile").Element("Maps").Descendants("Map")
                .Select(x => new MapData()
                {
                    MapSize = new Size(int.Parse(x.Element("MapSize")?.Value.Split(',')[0]), int.Parse(x.Element("MapSize")?.Value.Split(',')[1])),
                    Fields = x.Element("Fields")?.Value,
                    PowerLimit = int.Parse(x.Element("PowerLimit")?.Value),
                    Players = x.Element("Players")?.Value.Split('#').ToList(),
                    Servers = x.Element("GameObjects").Descendants("Server").Select(y => y?.Value).ToList(),
                    Routers = x.Element("GameObjects").Descendants("Router").Select(y => y?.Value).ToList(),
                    FireWalls = x.Element("GameObjects").Descendants("FireWall").Select(y => y?.Value).ToList()
                }
                as IMapDatas).AsQueryable();
        }
    }
}
