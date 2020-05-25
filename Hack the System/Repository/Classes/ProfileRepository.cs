// <copyright file="ProfileRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Repository.Classes
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Repository.Interfaces;

    /// <summary>
    /// This reposytory manage profile datas
    /// </summary>
    public class ProfileRepository : IProfileRepository
    {
        private readonly string url = "../../Resources/Profiles.xml";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileRepository"/> class.
        /// </summary>
        public ProfileRepository()
        {
            this.Document = XDocument.Load(this.url);
        }

        private XDocument Document { get; set; }

        /// <inheritdoc/>
        public void Delete(string name)
        {
            this.Document.Element("ProfilesFile").Element("Profiles").Descendants("Profile").Where(x => x.Element("Name")?.Value == name).Remove();
            this.Document.Save(this.url);
        }

        /// <inheritdoc/>
        public IQueryable<IProfileData> GetAll()
        {
            return this.Document.Element("ProfilesFile").Element("Profiles").Descendants("Profile")
                .Select(x => new ProfileData()
                {
                    Name = x.Element("Name")?.Value,
                    CompletedRandomLevelCount = int.Parse(x.Element("CompletedRandomLevelCount")?.Value),
                    CampaignScore = x.Element("CampaignScores")?.Value.Length > 0 ?
                    x.Element("CampaignScores")?.Value.Split('#').Select(y => int.Parse(y)).ToList() :
                    new List<int>(),
                    SavedGame = File.Exists($"Saves/{x.Element("Name")?.Value}.sgs")
                }
                as IProfileData).AsQueryable();
        }

        /// <inheritdoc/>
        public IProfileData GetDefault()
        {
           var name = this.Document.Element("ProfilesFile").Element("Default").Element("Name")?.Value;
            if (name != null)
            {
                return this.GetAll().Where(x => x.Name == name).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public void Insert(IProfileData newItem)
        {
            if (this.GetAll().Where(x => x.Name == newItem.Name).Count() > 0)
            {
                throw new Exception();
            }
            else
            {
                this.Document.Element("ProfilesFile").Element("Profiles").Add(
                    new XElement(
                        "Profile",
                    new XElement("Name", newItem.Name),
                    new XElement("CampaignScores"),
                    new XElement("CompletedRandomLevelCount", newItem.CompletedRandomLevelCount)));
                this.Document.Save(this.url);
            }
        }

        /// <inheritdoc/>
        public void Update(IProfileData item)
        {
            XElement element = this.Document.Element("ProfilesFile").Element("Profiles").Descendants("Profile").Where(x => x.Element("Name")?.Value == item.Name).Single();

            XElement newelement = new XElement(
                "Profile",
                    new XElement("Name", item.Name),
                    new XElement("CampaignScores", string.Join("#", item.CampaignScore)),
                    new XElement("CompletedRandomLevelCount", item.CompletedRandomLevelCount));

            element.ReplaceWith(newelement);
            this.Document.Save(this.url);
        }

        /// <inheritdoc/>
        public void SetDefault(string name)
        {
            this.Document.Element("ProfilesFile").Element("Default").Element("Name").SetValue(name);
            this.Document.Save(this.url);
        }

        /// <inheritdoc/>
        public void SaveGame(MemoryStream serializedgamestate, string name)
        {
            if (!Directory.Exists("Saves"))
            {
                Directory.CreateDirectory("Saves");
            }

            using (FileStream fileStream = new FileStream($"Saves/{name}.sgs", FileMode.Create, FileAccess.Write))
            {
                serializedgamestate.WriteTo(fileStream);

                // fileStream.Close();
            }
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed")]
        public MemoryStream LoadGame(string name)
        {
            FileStream stream = new FileStream($"Saves/{name}.sgs", FileMode.Open, FileAccess.Read);
            MemoryStream memory = new MemoryStream();
            stream.CopyTo(memory);
            memory.Position = 0;
            return memory;
        }
    }
}
