/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Piranha.Models;

namespace Piranha.AttributeBuilder
{
    public class BlockTypeBuilder : ContentTypeBuilder<BlockTypeBuilder, BlockType>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api">The current api</param>        
        public BlockTypeBuilder(IApi api) : base(api) { }

        /// <summary>
        /// Builds the block types.
        /// </summary>
        public override BlockTypeBuilder Build() {
            foreach (var type in types) {
                var blockType = GetContentType(type);

                if (blockType != null)
                    api.BlockTypes.Save(blockType);
            }
            return this;
        }

        /// <summary>
        /// Deletes all block types in the database that doesn't
        /// exist in the database,
        /// </summary>
        /// <returns>The builder</returns>
        public BlockTypeBuilder DeleteOrphans() {
            var orphans = new List<BlockType>();
            var importTypes = new List<BlockType>();

            // Get all block types added for import.
            foreach (var type in types) {
                var importType = GetContentType(type);

                if (importType != null)
                    importTypes.Add(importType);
            }

            // Get all previously imported page types.
            foreach (var blockType in api.BlockTypes.GetAll()) {
                if (!importTypes.Any(t => t.Id == blockType.Id))
                    orphans.Add(blockType);
            }

            // Delete all orphans.
            foreach (var blockType in orphans) {
                api.BlockTypes.Delete(blockType);
            }
            return this;
        }

        #region Private methods
        /// <summary>
        /// Gets the possible block type for the given type.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The block type</returns>
        protected override BlockType GetContentType(Type type) {
            var attr = type.GetTypeInfo().GetCustomAttribute<BlockTypeAttribute>();

            if (attr != null) {
                if (string.IsNullOrWhiteSpace(attr.Id))
                    attr.Id = type.Name;

                if (!string.IsNullOrEmpty(attr.Id) && !string.IsNullOrEmpty(attr.Title)) {
                    var blockType = new BlockType() {
                        Id = attr.Id,
                        CLRType = type.GetTypeInfo().AssemblyQualifiedName,
                        Title = attr.Title
                    };

                    blockType.Fields = GetFields(type);

                    return blockType;
                }
            } else {
                throw new ArgumentException($"Title is mandatory in BlockTypeAttribute. No title provided for {type.Name}");
            }
            return null;
        }
        #endregion
    }
}
