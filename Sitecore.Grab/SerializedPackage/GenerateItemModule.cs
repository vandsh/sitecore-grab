using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using Sitecore.Grab.Classes.Bases;
using Sitecore.Grab.Classes.Contracts.Interfaces;
using Sitecore.Grab.Classes.Domain;
using Sitecore.Grab.Classes.Exceptions;
using Sitecore.Grab.Classes.Services;

namespace Sitecore.Grab.SerializedPackage
{
    public class GenerateItemModule : GrabBaseModule
    {
        public GenerateItemModule(IAuthorizer authorizer) : base(authorizer, "/services/package/create")
        {
            //Get["/package/create/tree"] = CreateTreePackage;
            Get["/items"] = CreateItemPackage;
            Get["/items/{itemId}/{database}/{generateSubItems}"] = CreateItemPackage;
        }

        private dynamic CreateItemPackage(dynamic o)
        {
            try
            {
                var generateItemRequest = this.Bind<GenerateItemRequest>();

                using (new SecurityDisabler())
                {
                    Database authorityDatabase = Configuration.Factory.GetDatabase(generateItemRequest.Database);

                    var serializedList = new List<string>();
                    if (!string.IsNullOrEmpty(generateItemRequest.ItemId))
                    {
                        var item = authorityDatabase.GetItem(new ID(generateItemRequest.ItemId));
                        _getSerializedResponse(generateItemRequest, serializedList, item);
                    }
                    else if (!string.IsNullOrEmpty(generateItemRequest.ItemPath))
                    {
                        var item = authorityDatabase.GetItem(generateItemRequest.ItemPath);
                        _getSerializedResponse(generateItemRequest, serializedList, item);
                    }

                    return Response.AsJson(serializedList, HttpStatusCode.Accepted);
                }
            }
            catch (NotFoundException)
            {
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }

        }

        private static void _getSerializedResponse(GenerateItemRequest generateItemRequest, List<string> serializedList, Item item)
        {
            var itemService = new ItemService();
            if (bool.Parse(generateItemRequest.GenerateSubItems))
            {
                itemService.SerializeItemTree(serializedList, item);
            }
            else
            {
                serializedList.Add(itemService.SerializeItem(item));
            }
        }
    }
}
