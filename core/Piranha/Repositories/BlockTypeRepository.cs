/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha
 * 
 */

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Piranha.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Piranha.Repositories
{
    public class BlockTypeRepository : IBlockTypeRepository
    {
        private readonly IDb db;
        private readonly ICache cache;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="db">The current db connection</param>
        /// <param name="cache">The optional model cache</param>
        public BlockTypeRepository(IDb db, ICache cache = null) {
            this.db = db;
            this.cache = cache;
        }

        /// <summary>
        /// Gets all available models.
        /// </summary>
        /// <returns>The available models</returns>
        public IEnumerable<Models.BlockType> GetAll() {
            var types = db.BlockTypes
                .AsNoTracking()
                .OrderBy(t => t.Id)
                .ToList();
            var models = new List<Models.BlockType>();

            foreach (var type in types) {
                var model = JsonConvert.DeserializeObject<Models.BlockType>(type.Body);

                if (cache != null && model != null)
                    cache.Set(model.Id, model);

                models.Add(model);
            }
            return models;
        }

        /// <summary>
        /// Gets the model with the specified id.
        /// </summary>
        /// <param name="id">The unique i</param>
        /// <returns>The block type</returns>
        public Models.BlockType GetById(string id) {
            Models.BlockType model = cache != null ? cache.Get<Models.BlockType>(id) : null;

            if (model == null) {
                var type = db.BlockTypes
                    .AsNoTracking()
                    .FirstOrDefault(t => t.Id == id);

                if (type != null)
                    model = JsonConvert.DeserializeObject<Models.BlockType>(type.Body);

                if (cache != null && model != null)
                    cache.Set(model.Id, model);
            }
            return model;
        }

        /// <summary>
        /// Adds or updates the given model in the database
        /// depending on its state.
        /// </summary>
        /// <param name="model">The model</param>
        public void Save(Models.BlockType model) {
            var type = db.BlockTypes
                .FirstOrDefault(t => t.Id == model.Id);

            if (type == null) {
                type = new Data.BlockType() {
                    Id = model.Id,
                    Created = DateTime.Now
                };
                db.BlockTypes.Add(type);
            }
            type.CLRType = model.CLRType;
            type.Body = JsonConvert.SerializeObject(model);
            type.LastModified = DateTime.Now;

            db.SaveChanges();
            
            if (cache != null)
                cache.Remove(model.Id.ToString());
        }

        /// <summary>
        /// Deletes the model with the specified id.
        /// </summary>
        /// <param name="id">The unique id</param>
        public void Delete(string id) {
            var type = db.BlockTypes
                .FirstOrDefault(t => t.Id == id);

            if (type != null) {
                db.BlockTypes.Remove(type);
                db.SaveChanges();

                if (cache != null)
                    cache.Remove(id.ToString());
            }
        }

        /// <summary>
        /// Deletes the given model.
        /// </summary>
        /// <param name="model">The model</param>
        public void Delete(Models.BlockType model) {
            Delete(model.Id);
        }
    }
}
