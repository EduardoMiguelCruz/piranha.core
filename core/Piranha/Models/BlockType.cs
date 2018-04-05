/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using System.Collections.Generic;

namespace Piranha.Models
{
    /// <summary>
    /// Block type.
    /// </summary>
    public sealed class BlockType
    {
        /// <summary>
        /// Gets/sets the unique id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets/sets the CLR type of the block model.
        /// </summary>
        public string CLRType { get; set; }

        /// <summary>
        /// Gets/sets the optional title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets/sets the available fields.
        /// </summary>
        public IList<FieldType> Fields { get; set; }

        /// <summary>
        /// Gets/sets if this block is a collection of values.
        /// </summary>
        public bool IsCollection { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BlockType() {
            Fields = new List<FieldType>();
        }
    }
}
