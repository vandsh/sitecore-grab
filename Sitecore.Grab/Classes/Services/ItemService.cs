using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Sitecore.Data.Items;
using Sitecore.Data.Serialization;

namespace Sitecore.Grab.Classes.Services
{
    public class ItemService
    {
        public string SerializeItem(Item source)
        {
            var syncItem = ItemSynchronization.BuildSyncItem(source);
            var serializedSb = new StringBuilder();

            var writer = (TextWriter)new StringWriter(serializedSb);
            syncItem.Serialize(writer);

            return writer.ToString();
        }

        public Item DeserializeItem(Item targetParent, string serializedItem)
        {
            var updatedParent = Regex.Replace(serializedItem, "parent:.*", "parent: " + targetParent.ID);

            // deserialize
            var loadOptions = new LoadOptions(targetParent.Database)
            {
                DisableEvents = true,
                ForceUpdate = true
            };

            using (var reader = (TextReader)new StringReader(updatedParent))
            {
                try
                {
                    return ItemSynchronization.ReadItem(reader, loadOptions, true);
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error deserializing item: " + updatedParent, ex);
                    return null;
                }
            }
        }

        public void SerializeItemTree(List<string> deserializedList, Item item)
        {
            deserializedList.Add(SerializeItem(item));

            foreach (Item child in item.Children)
            {
                SerializeItemTree(deserializedList, child);
            }
        }
    }
}
